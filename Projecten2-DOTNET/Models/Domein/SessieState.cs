using System;

namespace Projecten2_DOTNET.Models.Domein {
	public abstract class SessieState {
		public int Id { get; set; }

		public Sessie Context { get; set; }

		public SessieState(Sessie context) {
			Context = context;
		}
		public SessieState() {
		}


		public virtual void SluitSessie() {
			throw new InvalidOperationException("Sessie kan enkel gesloten worden als de sessie nog niet gesloten is");
		}
		public virtual void SchrijfGebruikerIn(SessieGebruikerIngeschreven sgi) {
			throw new InvalidOperationException("Kan gebruiker niet inschrijven op het huidige tijdstip");
		}


		public virtual void SchrijfGebruikerUit(SessieGebruikerIngeschreven sgi) {
			throw new InvalidOperationException("Kan gebruiker niet uitschrijven op het huidige tijdstip");
		}

		public virtual void SluitVoorRegistratie() {
			throw new InvalidOperationException("Sessie kan enkel gesloten worden als de sessie nog niet gesloten is");
		}

		public virtual SessieGebruikerAanwezig GebruikerAanwezigMelden(SessieGebruikerIngeschreven sgi) {

			throw new InvalidOperationException("Kan gebruiker niet aanmelden op het huidige tijdstip");

		}



		public virtual void OpenVoorAanmelden() {
			throw new InvalidOperationException("Kan sessie niet openen op het huidige tijdstip");
		}
	}
}