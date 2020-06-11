using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Projecten2_DOTNET.Models.Domein {
	public class Gebruiker {

		#region Properties
		public string Gebruikersnaam { get; set; }
		public string Naam { get; set; }
		public long Barcode { get; set; }

		public string Afbeelding { get; set; }

		public string Email { get; set; }
		public GebruikerState CurrentState { get; private set; }

		public int AantalAfwezigheden {
			get;
			private set;
		}

		public IList<SessieGebruikerIngeschreven> IngeschrevenSessies { get; set; }
		public IList<SessieGebruikerAanwezig> AanwezigeSessies { get; set; }

		public virtual IList<Sessie> GeorganiseerdeSessies { get => null; set { } }
		#endregion

		#region Constructors
		public Gebruiker(string naam, string gebruikersnaam, long barcode) {
			//validatie gebruikersnaam
			string naamPattern = @"(?=.{8}$)([0-9]{6})([a-z|A-Z]{2})";
			Regex rg = new Regex(naamPattern);
			if (rg.IsMatch(gebruikersnaam)) {
				Gebruikersnaam = gebruikersnaam;
			}
			else if (gebruikersnaam == null) {
				throw new ArgumentNullException("gebruikersnaam mag niet leeg zijn");
			}
			else {
				throw new ArgumentException("gebruikersnaam moet beginnen met 6 cijfers en eindigen op 2 letters");
			}

			//validatie wachtwoord
			//string wachtwoordPattern = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$";
			//Regex rg2 = new Regex(wachtwoordPattern);
			//if (rg2.IsMatch(barcode)) {
			//	Barcode = barcode;
			//}
			//else {
			//	throw new ArgumentException("wachtwoord moet tussen 8 en 15 karakters lang zijn, met minstens 1 hoofdletter en minstens 1 cijfer");
			//}

			// VOORLOPIG ZONDER VALIDATIE
			Gebruikersnaam = gebruikersnaam;
			Naam = naam;
			Barcode = barcode;

			IngeschrevenSessies = new List<SessieGebruikerIngeschreven>();
			AanwezigeSessies = new List<SessieGebruikerAanwezig>();
			CurrentState = new ActiefState(this);
		}

		public Gebruiker() {
			// Dit moet hier staan voor EF
		}
		#endregion

		#region Methods
		public string GeefInitials() {
			return Gebruikersnaam.Substring(6, 2).ToUpper();
			//string[] naam_voornaam = Naam.Split(" ");
			//return naam_voornaam[0].Substring(0, 1) + naam_voornaam[1].Substring(0, 1);
		}

		public void geefFeedbackOpSessie(Sessie sessie, string inhoud, int score) {
			// kijken of de gebruiker aanwezig was
			// zoja feedback toevoegen aan sessie (nieuwe feedback maken)
			if (AanwezigeSessies.FirstOrDefault(sga => sga.SessieId == sessie.Id) != null) {
				FeedbackEntry fb = new FeedbackEntry(this, score, inhoud);
				sessie.FeedbackEntries.Add(fb);
			}
			else {
				throw new InvalidOperationException("Gebruiker moet aanwezig zijn geweest bij sessie om feedback te mogen geven");
			}

		}

		public void SchrijfGebruikerInSessie(Sessie sessie) {
			CurrentState.SchrijfInVoorSessie(sessie);
		}

		public void SchrijfGebruikerUitSessie(Sessie sessie) {
			CurrentState.SchrijfUitVoorSessie(sessie);
		}

		public void ZetAanwezigVoorSessie(Sessie s) {
			CurrentState.RegistreerAanwezigheidVoorSessie(s);
		}

		public void ToState(GebruikerState state) {
			CurrentState = state;
		}

		public void SetAantalAfwezighedenToZero() {
			AantalAfwezigheden = 0;
		}

		public virtual void SluitSessie(Sessie sessie) {
			throw new InvalidOperationException("Enkel (hoofd)verandwoordelijken mogen een sessie sluiten");
		}

		public virtual void SluitVoorRegistratie(Sessie sessie) {
			throw new InvalidOperationException("Enkel (hoofd)verandwoordelijken mogen een sessie sluiten");
		}

		public virtual void OpenVoorAanmelden(Sessie sessie) {
			throw new InvalidOperationException("Enkel (hoofd)verandwoordelijken mogen een sessie openen");
		}
		#endregion


	}
}
