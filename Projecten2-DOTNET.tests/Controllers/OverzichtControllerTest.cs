using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Projecten2_DOTNET.Controllers;
using Projecten2_DOTNET.Models.Domein;
using Xunit;
using static Projecten2_DOTNET.tests.DummyData;


using System.Linq;
using System.Threading.Tasks;
using Projecten2_DOTNET.Filter;
using System.Collections;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Projecten2_DOTNET.tests.Controllers {
	public class OverzichtControllerTest {
		private readonly OverzichtController _controller;
		private DummyData _dummydata;
		private readonly Mock<IGebruikerRepository> _gebruikerRepository;
		private readonly Mock<ISessieRepository> _sessieRepository;
		private IEnumerable<Sessie> _sessies;

		private Gebruiker testGebruiker;
		private Sessie testSessie;

		public OverzichtControllerTest() {
			_gebruikerRepository = new Mock<IGebruikerRepository>();
			_sessieRepository = new Mock<ISessieRepository>();
			_controller = new OverzichtController(_sessieRepository.Object, _gebruikerRepository.Object) {
				TempData = new Mock<ITempDataDictionary>().Object
			};
			_dummydata = new DummyData();
			testGebruiker = _dummydata.piet;
			testSessie = _dummydata.s1;
			_sessies = new List<Sessie>() { _dummydata.s2, _dummydata.s1, _dummydata.s3 };
		}

		#region -- Index --
		[Fact]
		public void Index_PassesOrderedSessieList_byDate() {
			//_sessieRepository.Setup(m => m.GetAll()).Returns(_sessies);
			//var result = Assert.IsType<ViewResult>(_controller.Index(pieter));
			//var sessiesInModel = Assert.IsType<List<Sessie>>(result.Model);
			//Assert.Equal(3, sessiesInModel.Count);
			//Assert.Equal("JAVAFX", sessiesInModel[0].Titel);
			//Assert.Equal("DOTNET", sessiesInModel[1].Titel);
			//Assert.Equal("ANGULAR", sessiesInModel[2].Titel);
			//Assert.Equal(pieter.Gebruikersnaam, result.ViewData["gebruikerId"]);
		}

		[Fact]
		public void Index_viewdata_correct() {
			_sessieRepository.Setup(m => m.GetAll()).Returns(_sessies);
			var result = Assert.IsType<ViewResult>(_controller.Index(testGebruiker));
			Assert.Equal("432561he", result.ViewData["gebruikerId"]);
			Assert.Equal("datum", result.ViewData["sorteerOptie"]);
		}

		[Fact]
		public void Index_viewdata_correct2() {
			_sessieRepository.Setup(m => m.GetAll()).Returns(_sessies);
			var result = Assert.IsType<ViewResult>(_controller.Index(testGebruiker, "verandwoordelijke"));
			Assert.Equal("432561he", result.ViewData["gebruikerId"]);
			Assert.Equal("verandwoordelijke", result.ViewData["sorteerOptie"]);
		}
		#endregion

		#region Filter
		[Fact]
		public void Filter_PassesdSessieList() {
			//Arrange
			_sessieRepository.Setup(m => m.GetByDate(new DateTime(2019, 3, 3), new DateTime(2023, 3, 3))).Returns(_sessies);

			//Act
			var result = _controller.Filter(testGebruiker, new DateTime(2019, 3, 3).ToString("dd/MM/yyyy"), new DateTime(2023, 3, 3).ToString("dd/MM/yyyy"));

			// Assert
			var viewResult = Assert.IsType<ViewResult>(result);
			var model = Assert.IsAssignableFrom<IEnumerable<Sessie>>(
				viewResult.ViewData.Model);
			Assert.Equal(3, model.Count());
		}

		[Fact]
		public void Filter_PassesOrderedSessieList_byTitle() {
			_sessieRepository.Setup(m => m.GetByDate(new DateTime(2019, 3, 3), new DateTime(2023, 3, 3))).Returns(_sessies);
			var result = Assert.IsType<ViewResult>(_controller.Filter(testGebruiker, new DateTime(2019, 3, 3).ToString("dd/MM/yyyy"), new DateTime(2023, 3, 3).ToString("dd/MM/yyyy"), "titel"));
			var sessiesInModel = ((IEnumerable<Sessie>)result.Model).ToList();
			Assert.Equal(3, sessiesInModel.Count);
			Assert.Equal("JAVAFX", sessiesInModel[2].Titel);
			Assert.Equal("DOTNET", sessiesInModel[1].Titel);
			Assert.Equal("ANGULAR", sessiesInModel[0].Titel);
		}

		[Fact]
		public void Filter_viewdata_correct() {
			_sessieRepository.Setup(m => m.GetAll()).Returns(_sessies);
			var result = Assert.IsType<ViewResult>(_controller.Filter(testGebruiker, new DateTime(2019, 3, 3).ToString(), new DateTime(2023, 3, 3).ToString(), "verandwoordelijke"));
			Assert.Equal("432561he", result.ViewData["gebruikerId"]);
			Assert.Equal("verandwoordelijke", result.ViewData["sorteerOptie"]);
		}
		#endregion

		#region InschrijvenInSessie
		[Fact]
		public void SchrijfInVoorSessie_inschrijvenvoorJuisteSessie() {
			_sessieRepository.Setup(m => m.GetById(testSessie.Id)).Returns(testSessie);
			_gebruikerRepository.Setup(m => m.GetByGebruikersnaam(testGebruiker.Gebruikersnaam)).Returns(testGebruiker);
			var result = Assert.IsType<RedirectToActionResult>(_controller.SchrijfInVoorSessie(testGebruiker, testSessie.Id));
			Assert.Equal(nameof(Index), result.ActionName);
			//! onmogelijk te checken --> redirect heeft geen tempdata prop
			//Assert.Equal("Inschrijven voor sessie 'JAVAFX' succesvol!", result.TempData["message"]);
			_sessieRepository.Verify(m => m.SaveChanges(), Times.Once());
		}

		[Fact]
		public void SchrijfInVoorSessie_AlIngeschreven_PersisteertNiet() {
			_sessieRepository.Setup(m => m.GetById(testSessie.Id)).Returns(testSessie);
			_gebruikerRepository.Setup(m => m.GetByGebruikersnaam(testGebruiker.Gebruikersnaam)).Returns(testGebruiker);
			// piet eerst inschrijven
			testGebruiker.SchrijfGebruikerInSessie(testSessie);

			// Piet opnieuw proberen inschrijven
			var result = Assert.IsType<RedirectToActionResult>(_controller.SchrijfInVoorSessie(testGebruiker, testSessie.Id));
			Assert.Equal(nameof(Index), result.ActionName);
			//Assert.Equal("Gebruiker is al ingeschreven", result.TempData["error"]);
			_sessieRepository.Verify(m => m.SaveChanges(), Times.Never);
		}

		[Fact]
		public void SchrijfInVoorSessie_SessieBestaatNiet_PersisteertNiet() {
			_sessieRepository.Setup(m => m.GetById(testSessie.Id)).Returns(testSessie);
			_gebruikerRepository.Setup(m => m.GetByGebruikersnaam(testGebruiker.Gebruikersnaam)).Returns(testGebruiker);
			var result = Assert.IsType<RedirectToActionResult>(_controller.SchrijfInVoorSessie(testGebruiker, 654));
			Assert.Equal(nameof(Index), result.ActionName);
			//Assert.Equal("De sessie bestaat niet", result.TempData["error"]);
			_sessieRepository.Verify(m => m.SaveChanges(), Times.Never);
		}

		[Fact]
		public void SchrijfInVoorSessie_SessieVolzet_PersisteertNiet() {
			Sessie volleSessie = new Sessie(hoofdV, "test", "test", new Lokaal("A", 1), DateTime.Today, DateTime.Today.AddHours(1)) {
				Id = 1
			};
			// schrijf jan in 
			Gebruiker jan = new Gebruiker("jan", "987654jj", 9876543219876);
			jan.SchrijfGebruikerInSessie(volleSessie);
			// de sessie is nu vol
			_sessieRepository.Setup(m => m.GetById(volleSessie.Id)).Returns(volleSessie);
			_gebruikerRepository.Setup(m => m.GetByGebruikersnaam(testGebruiker.Gebruikersnaam)).Returns(testGebruiker);
			//probeer piet in te schrijven
			var result = Assert.IsType<RedirectToActionResult>(_controller.SchrijfInVoorSessie(testGebruiker, volleSessie.Id));
			Assert.Equal(nameof(Index), result.ActionName);
			//Assert.Equal("Het lokaal is volzet", result.TempData["error"]);
			_sessieRepository.Verify(m => m.SaveChanges(), Times.Never);
		}

		[Fact]
		public void SchrijfInVoorSessie_SessieVerlopen_PersisteertNiet() {
			testSessie.ToState(new AanmeldenEnRegistratieGeslotenState());
			// de sessie is nu vol
			_sessieRepository.Setup(m => m.GetById(testSessie.Id)).Returns(testSessie);
			_gebruikerRepository.Setup(m => m.GetByGebruikersnaam(testGebruiker.Gebruikersnaam)).Returns(testGebruiker);
			//probeer piet in te schrijven
			var result = Assert.IsType<RedirectToActionResult>(_controller.SchrijfInVoorSessie(testGebruiker, testSessie.Id));
			Assert.Equal(nameof(Index), result.ActionName);
			//Assert.Equal("Het lokaal is volzet", result.TempData["error"]);
			_sessieRepository.Verify(m => m.SaveChanges(), Times.Never);
		}

		#endregion

		#region UitschrijvenUitSessie
		[Fact]
		public void SchrijfUitVoorSessie_UitschrijvenvoorJuisteSessie() {
			_sessieRepository.Setup(m => m.GetById(testSessie.Id)).Returns(testSessie);
			_gebruikerRepository.Setup(m => m.GetByGebruikersnaam(testGebruiker.Gebruikersnaam)).Returns(testGebruiker);
			testGebruiker.SchrijfGebruikerInSessie(testSessie);
			var result = Assert.IsType<RedirectToActionResult>(_controller.SchrijfUitVoorSessie(testGebruiker, testSessie.Id));
			Assert.Equal(nameof(Index), result.ActionName);
			//! onmogelijk te checken --> redirect heeft geen tempdata prop
			//Assert.Equal("Inschrijven voor sessie 'JAVAFX' succesvol!", result.TempData["message"]);
			_sessieRepository.Verify(m => m.SaveChanges(), Times.Once());
		}

		[Fact]
		public void SchrijfUitVoorSessie_AlUitgeschreven_PersisteertNiet() {
			_sessieRepository.Setup(m => m.GetById(testSessie.Id)).Returns(testSessie);
			_gebruikerRepository.Setup(m => m.GetByGebruikersnaam(testGebruiker.Gebruikersnaam)).Returns(testGebruiker);

			// Piet opnieuw proberen inschrijven
			var result = Assert.IsType<RedirectToActionResult>(_controller.SchrijfUitVoorSessie(testGebruiker, testSessie.Id));
			Assert.Equal(nameof(Index), result.ActionName);
			//Assert.Equal("Gebruiker is al ingeschreven", result.TempData["error"]);
			_sessieRepository.Verify(m => m.SaveChanges(), Times.Never);
		}

		[Fact]
		public void SchrijfUitVoorSessie_SessieBestaatNiet_PersisteertNiet() {
			_sessieRepository.Setup(m => m.GetById(testSessie.Id)).Returns(testSessie);
			_gebruikerRepository.Setup(m => m.GetByGebruikersnaam(testGebruiker.Gebruikersnaam)).Returns(testGebruiker);

			testGebruiker.SchrijfGebruikerInSessie(testSessie);

			var result = Assert.IsType<RedirectToActionResult>(_controller.SchrijfUitVoorSessie(testGebruiker, 654));
			Assert.Equal(nameof(Index), result.ActionName);
			//Assert.Equal("De sessie bestaat niet", result.TempData["error"]);
			_sessieRepository.Verify(m => m.SaveChanges(), Times.Never);
		}


		[Fact]
		public void SchrijfUitVoorSessie_SessieVerlopen_PersisteertNiet() {
			testGebruiker.SchrijfGebruikerInSessie(testSessie);
			testSessie.ToState(new AanmeldenEnRegistratieGeslotenState(testSessie));
			// de sessie is nu vol
			_sessieRepository.Setup(m => m.GetById(testSessie.Id)).Returns(testSessie);
			_gebruikerRepository.Setup(m => m.GetByGebruikersnaam(testGebruiker.Gebruikersnaam)).Returns(testGebruiker);
			//probeer piet in te schrijven
			var result = Assert.IsType<RedirectToActionResult>(_controller.SchrijfUitVoorSessie(testGebruiker, testSessie.Id));
			Assert.Equal(nameof(Index), result.ActionName);
			//Assert.Equal("Het lokaal is volzet", result.TempData["error"]);
			_sessieRepository.Verify(m => m.SaveChanges(), Times.Never);
		}

		#endregion

		#region Sessieinfo Tonen

		[Fact]
		public void SessieInfoTonen_sessieinfoWordtJuistDoorgegeven() {
			testSessie.Media = new List<Media>() { new Media("/images/MediaImages/BillGates.pdf") };
			testSessie.GastSpreker = "gastsprekerVanS1";

			IEnumerable<Sessie> sessies = new List<Sessie>() { testSessie };

			_sessieRepository.Setup(m => m.GetAll()).Returns(sessies);

			var result = Assert.IsType<ViewResult>(_controller.Index(testGebruiker));
			IEnumerable<Sessie> model = (IEnumerable<Sessie>)result.Model;

			Sessie teTesten = model.ToList()[0];
			Media teTestenMedia = teTesten.Media.ToList()[0];

			Assert.Equal("gastsprekerVanS1", teTesten.GastSpreker);
			Assert.Equal("/images/MediaImages/BillGates.pdf", teTestenMedia.Path);

		}

		#endregion

	}
}
