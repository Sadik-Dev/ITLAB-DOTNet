using Projecten2_DOTNET.Data.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_DOTNET.Models.Domein {
	public class Sessie {

		#region Propetries
		public Gebruiker Verantwoordelijke { get; set; }
		public bool IsGeopendGeweest { get; set; }
		public int Id { get; set; }
		public string Titel { get; set; }

		public string GastSpreker { get; set; }

		public Lokaal Lokaal { get; set; }

		public DateTime Start { get; set; }

		public DateTime Einde { get; set; }

		public IList<Media> Media { get; set; }

		public int AantalPlaatsen { get; set; }

		public string Description { get; set; }

		public IList<SessieGebruikerIngeschreven> IngeschrevenGebruikers { get; set; }

		public IList<SessieGebruikerAanwezig> AanwezigeGebruikers { get; set; }

		public IList<FeedbackEntry> FeedbackEntries { get; set; }

		public SessieState CurrentState { get; set; }

		#endregion

		public Sessie(Gebruiker verantwoordelijke, string titel, string Gastspreker, Lokaal lokaal, DateTime van, DateTime tot) {
			this.Verantwoordelijke = verantwoordelijke;
			this.Titel = titel;
			this.GastSpreker = Gastspreker;
			this.Start = van;
			this.Einde = tot;

			IngeschrevenGebruikers = new List<SessieGebruikerIngeschreven>();
			AanwezigeGebruikers = new List<SessieGebruikerAanwezig>();
			Lokaal = lokaal;
			CurrentState = new RegistratieOpenAanmeldenGeslotenState(this);
			IsGeopendGeweest = false;
			FeedbackEntries = new List<FeedbackEntry>();

		}
		public Sessie(Gebruiker hoofdV, string titel, string Gastspreker, Lokaal lokaal, DateTime van, DateTime tot, List<Media> media) {
			this.Verantwoordelijke = hoofdV;
			this.Titel = titel;
			this.GastSpreker = Gastspreker;
			this.Start = van;
			this.Einde = tot;
			Media = media;
			IngeschrevenGebruikers = new List<SessieGebruikerIngeschreven>();
			AanwezigeGebruikers = new List<SessieGebruikerAanwezig>();
			Lokaal = lokaal;
			CurrentState = new RegistratieOpenAanmeldenGeslotenState(this);
			IsGeopendGeweest = false;
			FeedbackEntries = new List<FeedbackEntry>();

		}
		public Sessie() {
			// voor EF
		}
		// Voorkomen van nullpointers op properties
		#region Methoden

		public void ToState(SessieState state) {
			CurrentState = state;
		}



		public int GeefAantalVrijePlaatsen() {
			//Om Front-End Te testen

			if (IngeschrevenGebruikers == null || IngeschrevenGebruikers.Count == 0)
				return Lokaal.AantalPlaatsen;


			return Lokaal.AantalPlaatsen - IngeschrevenGebruikers.Count();
		}

		public int GeefAantalAanwezigeGebruikers() {
			if (AanwezigeGebruikers == null)
				return 0;
			else
				return AanwezigeGebruikers.Count();
		}
		public bool IsGebruikerIngeschreven(string id) {
			if (IngeschrevenGebruikers == null || IngeschrevenGebruikers.Count == 0)
				return false;
			else if (IngeschrevenGebruikers.FirstOrDefault(b => b.GebruikerId.Equals(id)) != default)
				return true;
			else
				return false;
		}
		public bool IsGebruikerAanwezig(string id) {
			if (AanwezigeGebruikers == null) {
				return false;
			}
			else if (AanwezigeGebruikers.FirstOrDefault(b => b.GebruikerId.Equals(id)) == default)
				return false;
			else
				return true;
		}
		public void SchrijfGebruikerIn(SessieGebruikerIngeschreven sgi) {
			CurrentState.SchrijfGebruikerIn(sgi);

		}


		public void SchrijfGebruikerUit(SessieGebruikerIngeschreven sgi) {
			CurrentState.SchrijfGebruikerUit(sgi);
		}


		public string GeefInitialenVerantwoordelijke() {
			return Verantwoordelijke.Gebruikersnaam.Substring(6);
			//return naam_voornaam[0].Substring(0, 1) + naam_voornaam[1].Substring(0, 1);
		}

		public SessieGebruikerAanwezig GebruikerAanwezigMelden(SessieGebruikerIngeschreven sgi) {

			return CurrentState.GebruikerAanwezigMelden(sgi);

		}

		public void OpenVoorAanmelden() {
			CurrentState.OpenVoorAanmelden();
		}

		public void SluitVoorRegistratie() {
			CurrentState.SluitVoorRegistratie();
		}

		public void SluitSessie() {
			CurrentState.SluitSessie();
		}

		public void voegFeedBackToe(Gebruiker auteur, string inhoud, int score) {
			//	FeedbackEntries.Add(new FeedbackEntry(auteur, score, inhoud));
		}
		#endregion
	}
}
