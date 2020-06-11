using Projecten2_DOTNET.Models.Domein;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projecten2_DOTNET {
	public abstract class GebruikerState {
		public string Id { get; set; }
		public Gebruiker Context { get; set; }

		public GebruikerState(Gebruiker context) {
			Context = context;
		}

		public GebruikerState() {
			// voor EF
		}


		public virtual void DesactiveerGebruiker() {
			throw new InvalidOperationException("gebruiker moet actief zijn om te kunnen deactiveren");
		}

		public virtual void BlokkeerGebruiker() {
			throw new InvalidOperationException("gebruiker moet actief zijn om te kunnen blokkeren");
		}

		public virtual void activeerGebruiker() {
			throw new InvalidOperationException("Gebruiker moet Nietactief of Geblokkerd zijn om te kunnen activeren");
		}

		public virtual void RegistreerAanwezigheidVoorSessie(Sessie sessie) {
			throw new InvalidOperationException("Gebruiker moet actief zijn om aanwezigheid te kunnen registreren");
		}

		public virtual void SchrijfInVoorSessie(Sessie sessie) {
			throw new InvalidOperationException("gebruiker state moet actief zijn om in te kunnen schrijven");
		}

		public virtual void SchrijfUitVoorSessie(Sessie sessie) {
			throw new InvalidOperationException("gebruiker state moet actief zijn om uit te kunnen schrijven");
		}
	}
}