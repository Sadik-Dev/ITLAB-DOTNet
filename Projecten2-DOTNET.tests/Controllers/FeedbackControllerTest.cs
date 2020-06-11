using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Projecten2_DOTNET.Controllers;
using Projecten2_DOTNET.Models.Domein;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Projecten2_DOTNET.tests.Controllers {
	public class FeedbackControllerTest {
		private readonly Mock<IGebruikerRepository> _gebruikerRepository;
		private readonly Mock<ISessieRepository> _sessieRepository;
		private readonly FeedBackController _controller;
		private readonly DummyData _dummyData;
		private Sessie testSessie;
		private Gebruiker testGebruiker;

		public FeedbackControllerTest() {
			_gebruikerRepository = new Mock<IGebruikerRepository>();
			_sessieRepository = new Mock<ISessieRepository>();
			_controller = new FeedBackController(_sessieRepository.Object, _gebruikerRepository.Object) {
				TempData = new Mock<ITempDataDictionary>().Object
			};

			_dummyData = new DummyData();
			// testdata ophalen
			testSessie = _dummyData.s1;
			testGebruiker = _dummyData.piet;
			// testGebruiker inschrijven en aanwezig stellen
			testSessie.ToState(new RegistratieEnAanmeldenOpenState(testSessie)); // zorgen dat gebruiker kan inschrijven en aanmelden
			testGebruiker.SchrijfGebruikerInSessie(testSessie);
			testGebruiker.ZetAanwezigVoorSessie(testSessie);
			// schrijf een feedback
			testGebruiker.geefFeedbackOpSessie(testSessie, "een feedbackinhoud", 5);

		}

		#region Feedback Tonen

		[Fact]
		public void Index_JuisteParameters_ToontOverzicht() {
			_gebruikerRepository.Setup(m => m.GetByGebruikersnaam(testGebruiker.Gebruikersnaam)).Returns(testGebruiker);
			_sessieRepository.Setup(m => m.GetById(testSessie.Id)).Returns(testSessie);

			var result = Assert.IsType<ViewResult>(_controller.Index(testGebruiker, testSessie.Id));
			var model = (Sessie)result.Model;
			var feedback = model.FeedbackEntries[0];

			//check op juiste sessieinfo
			Assert.Equal(model.Id, testSessie.Id);
			Assert.Equal(model.Titel, testSessie.Titel);
			Assert.Equal("een feedbackinhoud", feedback.Inhoud);
			//check op juiste feedbackinfo
			Assert.Equal(5, feedback.Score);
			Assert.Equal(testGebruiker.Gebruikersnaam, feedback.Auteur.Gebruikersnaam);

		}

		[Fact]
		public void Index_fouteSessie_redirectToIndex() {
			_gebruikerRepository.Setup(m => m.GetByGebruikersnaam(testGebruiker.Gebruikersnaam)).Returns(testGebruiker);
			_sessieRepository.Setup(m => m.GetById(321321)).Returns(null as Sessie);

			var result = Assert.IsType<RedirectToActionResult>(_controller.Index(testGebruiker, 321321));

		}


		#endregion


		#region Feedback Geven
		[Fact]
		public void MaakGeldigeFeedbackEntry_MAaktEnPersisteertEntryEnRedirectsToActionIndex() {
			_sessieRepository.Setup(s => s.GetById(testSessie.Id)).Returns(testSessie);
			_gebruikerRepository.Setup(g => g.GetByGebruikersnaam(testGebruiker.Gebruikersnaam)).Returns(testGebruiker);

			var result = Assert.IsType<RedirectToActionResult>(_controller.maakNieuweFeedbackEntry(testGebruiker, testSessie.Id, "Dit is feedback om te testen", 4));
			Assert.Equal("Index", result?.ActionName);
			_sessieRepository.Verify(m => m.SaveChanges(), Times.Once());
		}

		[Fact]
		public void MaakOngeldigeFeedbackEntry_MaaktNietNochPersisteertEntryAndRedirectsToActionIndex() {
			//Arrange gebruiker ingeschreven maar niet aanwezig
			Gebruiker testGebruiker2 = _dummyData.jan;
			testGebruiker2.SchrijfGebruikerInSessie(testSessie);
			_sessieRepository.Setup(s => s.GetById(testSessie.Id)).Returns(testSessie);
			_gebruikerRepository.Setup(g => g.GetByGebruikersnaam(testGebruiker2.Gebruikersnaam)).Returns(testGebruiker2);

			var result = Assert.IsType<RedirectToActionResult>(_controller.maakNieuweFeedbackEntry(testGebruiker2, testSessie.Id, "Dit is feedback om te testen", 4));
			Assert.Equal("Index", result?.ActionName);
			_sessieRepository.Verify(m => m.SaveChanges(), Times.Never());
		}
		#endregion
	}
}
