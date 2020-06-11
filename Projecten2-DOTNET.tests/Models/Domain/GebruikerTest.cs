using Projecten2_DOTNET.Models.Domein;
using System;
using System.Linq;
using Xunit;
using static Projecten2_DOTNET.tests.DummyData;

namespace Projecten2_DOTNET.tests {
	public class GebruikerTest {

		public Sessie sRgeslotenAgesloten = new Sessie(hoofdV, "JAVAFX", "GastSpreker1", lokaal1, new DateTime(2020, 3, 1, 12, 30, 0), new DateTime(2020, 3, 1, 13, 30, 0));
		public Sessie sRgeopendAgesloten = new Sessie(hoofdV, "DOTNET", "GastSpreker1", lokaal2, new DateTime(2020, 4, 1, 12, 30, 0), new DateTime(2020, 4, 1, 13, 30, 0));
		public Sessie sRgeopendAgeopend = new Sessie(hoofdV, "ANGULAR", "GastSpreker1", lokaal3, new DateTime(2020, 5, 1, 12, 30, 0), new DateTime(2020, 5, 1, 13, 30, 0));
		public Sessie sRgeslotenAgeopend = new Sessie(hoofdV, "YOLOSCRIPT", "GastSpreker1", lokaal4, new DateTime(2020, 6, 1, 12, 30, 0), new DateTime(2020, 6, 1, 13, 30, 0));
		private DummyData _dummydata;
		private Sessie s1;

		public GebruikerTest() {
			sRgeopendAgeopend.OpenVoorAanmelden();

			sRgeslotenAgeopend.OpenVoorAanmelden();
			sRgeslotenAgeopend.SluitVoorRegistratie();

			sRgeslotenAgesloten.OpenVoorAanmelden();
			sRgeslotenAgesloten.SluitVoorRegistratie();
			sRgeslotenAgesloten.SluitSessie();

			_dummydata = new DummyData();
			s1 = _dummydata.s1;

		}


		#region constructor
		[Theory]
		[InlineData("Bart Anders", "123456ba", 9876543219876)]
		[InlineData("Berent Quotidien", "454545bq", 9876543219876)]
		[InlineData("Piet Pieters", "000000pp", 9876543219876)]
		public void NieuweGebruiker_geldigeInvoer_MaaktGebruikerJuisteState(string naam, string gebruikernaam, long barcode) {
			Gebruiker gebruiker = new Gebruiker(naam, gebruikernaam, barcode);
			Assert.Equal(0, gebruiker.AantalAfwezigheden);
			Assert.Empty(gebruiker.IngeschrevenSessies);
			Assert.True(gebruiker.CurrentState.GetType() == typeof(ActiefState));
			Assert.Equal(naam, gebruiker.Naam);
			Assert.Equal(gebruikernaam, gebruiker.Gebruikersnaam);

		}


		[Theory]
		[InlineData("")]
		[InlineData("  ")]
		[InlineData("123456")]
		[InlineData("abcdefgh")]
		[InlineData("123456cd1")]
		[InlineData("12a456cd")]
		[InlineData("1234567890123456789012")]
		public void NieuweGebruiker_OngeldigeGebruikersNaam_ThrowsArgumentException(string gebruikersnaam) {
			Assert.Throws<ArgumentException>(() => new Gebruiker("hen drik", gebruikersnaam, 9876543219876));
		}

		[Fact]
		public void NieuweGebruiker_LegeGebruikersNaam_ThrowsArgumentException() {
			Assert.Throws<ArgumentNullException>(() => new Gebruiker("hen drik", null, 9876543219876));
		}

		//[Theory]
		//[InlineData("")]
		//[InlineData("1")]
		//[InlineData("Wachtwoordjee")]
		//[InlineData("12345678901234")]
		//[InlineData("wAchta2")]
		//public void NieuweGebruiker_OngeldigWachtwoord_ThrowsArgumentException(string wachtwoord) {
		//	Assert.Throws<ArgumentException>(() => new Gebruiker("solomon Driest", "201734sd", wachtwoord));
		//}

		//[Fact]
		//public void NieuweGebruiker_leegWachtwoord_ThrowsArgumentException() {
		//	Assert.Throws<ArgumentNullException>(() => new Gebruiker("Solomon Driest", "201734sd", null));
		//}
		#endregion

		#region Methods
		[Fact]
		public void InschrijvenSessie_voegtSessieToe() {

			Gebruiker gebruiker = new Gebruiker("Sam Doorsnee", "201734sd", 9876543219876);
			Assert.Empty(gebruiker.IngeschrevenSessies);

			gebruiker.SchrijfGebruikerInSessie(sRgeopendAgesloten);
			Assert.Single( gebruiker.IngeschrevenSessies);

		}


		[Fact]
		public void UitschrijvenSessie_verwijderdSessieUitlijst() {
			Gebruiker gebruiker = new Gebruiker("Samantha Dompel", "201734sd", 9876543219876);
			Assert.Empty(gebruiker.IngeschrevenSessies);


			gebruiker.SchrijfGebruikerInSessie(sRgeopendAgesloten);
			gebruiker.SchrijfGebruikerUitSessie(sRgeopendAgesloten);
			Assert.Empty(gebruiker.IngeschrevenSessies);

			gebruiker.SchrijfGebruikerInSessie(sRgeopendAgeopend);
			gebruiker.SchrijfGebruikerUitSessie(sRgeopendAgeopend);
			Assert.Empty(gebruiker.IngeschrevenSessies);

		}

		[Fact]
		public void Uitschrijven_sessieNietIngeschreven_throwsException() {
			Gebruiker gebruiker = new Gebruiker("Sien D'hore", "201734sd", 9876543219876);
			Assert.Throws<InvalidOperationException>(() => gebruiker.SchrijfGebruikerUitSessie(sRgeopendAgeopend));
		}

		[Fact]
		public void Registreer2x_zelfdeSessie_throwsException() {
			Gebruiker gebruiker = new Gebruiker("Sandra Doemsdag", "201734sd", 9876543219876);
			gebruiker.SchrijfGebruikerInSessie(sRgeopendAgeopend);
			Assert.Throws<InvalidOperationException>(() => gebruiker.SchrijfGebruikerInSessie(sRgeopendAgeopend));
		}

		[Fact]
		public void RegistreerAanwezigheid_zetAanwezigTrue() {
			Gebruiker gebruiker = new Gebruiker("Sukie Dageraad", "201734sd", 9876543219876);
			gebruiker.SchrijfGebruikerInSessie(sRgeopendAgeopend);
			gebruiker.ZetAanwezigVoorSessie(sRgeopendAgeopend);
			Assert.Single( sRgeopendAgeopend.AanwezigeGebruikers);

		}

		#endregion

		#region state testen
		[Fact]
		public void InschrijvenSessie_geblokkeerdeGebruiker_throwsException() {
			Gebruiker gebruiker = new Gebruiker("Sander DenDoncker", "201734sd", 9876543219876);
			gebruiker.CurrentState.BlokkeerGebruiker();
			Assert.Throws<InvalidOperationException>(() => gebruiker.CurrentState.SchrijfInVoorSessie(s1));
		}

		[Fact]
		public void InschrijvenSessie_nietActieveGebruiker_throwsException() {
			Gebruiker gebruiker = new Gebruiker("Slang Dromedaris", "201734sd", 9876543219876);
			gebruiker.CurrentState.DesactiveerGebruiker();
			Assert.Throws<InvalidOperationException>(() => gebruiker.CurrentState.SchrijfInVoorSessie(s1));
		}

		[Fact]
		public void UitschrijvenSessie_nietActieveGebruiker_throwsException() {
			Gebruiker gebruiker = new Gebruiker("Slangeman Diersoort", "201734sd", 9876543219876);
			gebruiker.CurrentState.DesactiveerGebruiker();
			Assert.Throws<InvalidOperationException>(() => gebruiker.CurrentState.SchrijfUitVoorSessie(s1));
		}

		[Fact]
		public void RegistrerenAanwezigheid_nietActieveGebruiker_throwsException() {
			Gebruiker gebruiker = new Gebruiker("Student Dendomme", "201734sd", 9876543219876);
			gebruiker.CurrentState.DesactiveerGebruiker();
			Assert.Throws<InvalidOperationException>(() => gebruiker.CurrentState.RegistreerAanwezigheidVoorSessie(s1));
		}
		#endregion
	}
}
