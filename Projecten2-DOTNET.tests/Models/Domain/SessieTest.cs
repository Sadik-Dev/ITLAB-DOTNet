using Projecten2_DOTNET.Data.Mappers;
using Projecten2_DOTNET.Models.Domein;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
namespace Projecten2_DOTNET.tests {
	public class SessieTest {

		private Sessie _session;
		private DummyData _dummydata;
		private Gebruiker piet;
		private Gebruiker pol;
		private Gebruiker thomas;
		private Gebruiker jan;

		public SessieTest() {
			_dummydata = new DummyData();
			piet = _dummydata.piet;
			pol = _dummydata.pol;
			thomas = _dummydata.thomas;
			jan = _dummydata.jan;
		}


		private Sessie MaakDefaultSessie() {
			return new Sessie(new Verantwoordelijke("Mik Koleman", "234567mk", 9876543219876), "JAVAFX", "GastSpreker1", new Lokaal("B0.001", 20), new DateTime(2020, 3, 1, 12, 30, 0), new DateTime(2020, 3, 1, 13, 30, 0));
			;
		}
		private SessieGebruikerIngeschreven MaakDefaultSGI(Gebruiker naam) {
			return new SessieGebruikerIngeschreven(_session, naam);
		}




		[Fact]
		public void SchrijfGebruikerIn_VoegtGebruikerToe() {

			_session = MaakDefaultSessie();
			_session.ToState(new RegistratieEnAanmeldenOpenState(_session));

			_session.SchrijfGebruikerIn(MaakDefaultSGI(piet));
			Assert.Equal(1, _session.IngeschrevenGebruikers.Count);
		}
		[Fact]
		public void AantalVrijePlaatsen_IsCorrect() {
			_session = MaakDefaultSessie();
			_session.ToState(new RegistratieEnAanmeldenOpenState(_session));
			_session.SchrijfGebruikerIn(MaakDefaultSGI(piet));
			_session.SchrijfGebruikerIn(MaakDefaultSGI(pol));
			_session.SchrijfGebruikerIn(MaakDefaultSGI(thomas));
			Assert.Equal(17, _session.GeefAantalVrijePlaatsen());
		}

		[Fact]
		public void InschrijvenAlsErGeenVrijePlaatsenZijn_ThrowtError() {

			_session = MaakDefaultSessie();
			_session.ToState(new RegistratieEnAanmeldenOpenState(_session));
			_session.Lokaal.AantalPlaatsen = 4;
			_session.SchrijfGebruikerIn(MaakDefaultSGI(piet));
			_session.SchrijfGebruikerIn(MaakDefaultSGI(pol));
			_session.SchrijfGebruikerIn(MaakDefaultSGI(jan));
			_session.SchrijfGebruikerIn(MaakDefaultSGI(thomas));

			Assert.Throws<InvalidOperationException>(() => _session.SchrijfGebruikerIn(new SessieGebruikerIngeschreven(_session, jan)));

		}

		[Fact]
		public void DezelfdeUserInschrijven_ThrowtError() {
			_session = MaakDefaultSessie();
			_session.ToState(new RegistratieEnAanmeldenOpenState(_session));
			SessieGebruikerIngeschreven pietSG = MaakDefaultSGI(piet);
			_session.SchrijfGebruikerIn(pietSG);
			Assert.Throws<InvalidOperationException>(() => _session.SchrijfGebruikerIn(pietSG));
		}

		[Fact]
		public void DezelfdeUserUitschrijven_ThrowtError() {
			_session = MaakDefaultSessie();
			_session.ToState(new RegistratieEnAanmeldenOpenState(_session));
			SessieGebruikerIngeschreven pietSG = MaakDefaultSGI(piet);

			_session.SchrijfGebruikerIn(pietSG);
			_session.SchrijfGebruikerUit(pietSG);

			Assert.Throws<InvalidOperationException>(() => _session.SchrijfGebruikerUit(pietSG));
		}
		[Fact]
		public void DezelfdeUserAanmelden_ThrowtError() {
			_session = MaakDefaultSessie();
			_session.ToState(new RegistratieEnAanmeldenOpenState(_session));
			piet.SchrijfGebruikerInSessie(_session);
			piet.ZetAanwezigVoorSessie(_session);

			Assert.Throws<InvalidOperationException>(() => piet.ZetAanwezigVoorSessie(_session));
		}

		[Fact]
		public void DeGebruikerIsIngeschrevenInSessie() {
			_session = MaakDefaultSessie();
			_session.ToState(new RegistratieEnAanmeldenOpenState(_session));
			SessieGebruikerIngeschreven pietSG = MaakDefaultSGI(piet);
			_session.SchrijfGebruikerIn(pietSG);
			Assert.True(_session.IngeschrevenGebruikers.Contains(pietSG));

		}
		[Fact]
		public void DeGebruikerIsUitgeschrevenUitSessie() {
			_session = MaakDefaultSessie();
			_session.ToState(new RegistratieEnAanmeldenOpenState(_session));
			SessieGebruikerIngeschreven pietSG = new SessieGebruikerIngeschreven(_session, piet);
			_session.SchrijfGebruikerIn(pietSG);
			_session.SchrijfGebruikerUit(pietSG);
			Assert.False(_session.IngeschrevenGebruikers.Contains(pietSG));

		}
		




		#region SessieGeslotenTesten
		[Fact]
		public void AlsSessieGeslotenIs_KanJeNietInschrijven() {
			_session = MaakDefaultSessie();
			_session.ToState(new AanmeldenEnRegistratieGeslotenState(_session));

			Assert.Throws<InvalidOperationException>(() => _session.SchrijfGebruikerIn(MaakDefaultSGI(piet)));
		}

		[Fact]
		public void AlsSessieGeslotenIs_KanJeNietAanmelden() {
			_session = MaakDefaultSessie();
			_session.ToState(new AanmeldenEnRegistratieGeslotenState(_session));
			Assert.Throws<InvalidOperationException>(() => _session.GebruikerAanwezigMelden(MaakDefaultSGI(piet)));
		}

		[Fact]
		public void AlsSessieGeslotenIsKan_JeNietUitschrijven() {
			_session = MaakDefaultSessie();
			_session.ToState(new AanmeldenEnRegistratieGeslotenState(_session));
			SessieGebruikerIngeschreven sg = MaakDefaultSGI(piet);
			_session.IngeschrevenGebruikers.Contains(sg);
			Assert.Throws<InvalidOperationException>(() => _session.SchrijfGebruikerUit(sg));
		}

        #endregion


        #region SessieOpenTesten
        [Fact]
		public void AlsSessieOpenRegistratieEnOpenAanmeldenKanJeAanmelden() {
			_session = MaakDefaultSessie();
			_session.ToState(new RegistratieEnAanmeldenOpenState(_session));
			SessieGebruikerIngeschreven sg = MaakDefaultSGI(piet);
			_session.SchrijfGebruikerIn(sg);
			_session.GebruikerAanwezigMelden(sg);
			Assert.Equal(1, _session.GeefAantalAanwezigeGebruikers());



		}
		[Fact]
		public void AlsSessieOpenAanMeldenGesloten_GebruikerKanInschrijven() {
			_session = MaakDefaultSessie();
			_session.ToState(new RegistratieOpenAanmeldenGeslotenState(_session));
			SessieGebruikerIngeschreven sg = MaakDefaultSGI(piet);
			_session.SchrijfGebruikerIn(sg);

			Assert.Equal(1, _session.IngeschrevenGebruikers.Count);
		}
		[Fact]
		public void AlsSessieOpenAanmeldenGesloten_GebruikerKanUitschrijven() {
			_session = MaakDefaultSessie();
			_session.ToState(new RegistratieOpenAanmeldenGeslotenState(_session));
			SessieGebruikerIngeschreven sg = MaakDefaultSGI(piet);
			_session.SchrijfGebruikerIn(sg);
			_session.SchrijfGebruikerUit(sg);
			Assert.Empty(_session.IngeschrevenGebruikers);
		}
		[Fact]
		public void AlsSessieOpenAanmeldenGesloten_GebruikerAanmelden_ThrowtError() {
			_session = MaakDefaultSessie();
			_session.ToState(new RegistratieOpenAanmeldenGeslotenState(_session));
			SessieGebruikerIngeschreven sg = MaakDefaultSGI(piet);
			_session.SchrijfGebruikerIn(sg);

			Assert.Throws<InvalidOperationException>(() => _session.GebruikerAanwezigMelden(sg));
		}

		#endregion


	}
}
