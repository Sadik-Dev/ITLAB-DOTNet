using Projecten2_DOTNET.Data.Mappers;
using Projecten2_DOTNET.Models.Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projecten2_DOTNET {
	public class ActiefState : GebruikerState {

		public ActiefState(Gebruiker context) : base(context) {

		}
		public ActiefState() {

		}

		override
		public void BlokkeerGebruiker() {
			Context.ToState(new GeblokkeerdState(Context));
		}

		override
		public void DesactiveerGebruiker() {
			Context.ToState(new NietActiefState(Context));
		}

		override
		public void RegistreerAanwezigheidVoorSessie(Sessie sessie) {
			SessieGebruikerIngeschreven sgi = Context.IngeschrevenSessies.FirstOrDefault(sgi => sgi.SessieId == sessie.Id);
			SessieGebruikerAanwezig sga = Context.AanwezigeSessies.FirstOrDefault(sga => sga.SessieId == sessie.Id);
			if (sgi != null && sga == null) { // gebruiker is wel degelijk ingeschreven en nog niet aanwezig
				try {
					sga = sessie.GebruikerAanwezigMelden(sgi);
					Context.AanwezigeSessies.Add(sga);
				}
				catch (InvalidOperationException e) {
					throw new InvalidOperationException(e.Message);
				}
			}
			else {
				throw new InvalidOperationException("Gebruiker is niet ingeschreven voor deze sessie of is al aanwezig");
			}
		}

		override
		public void SchrijfInVoorSessie(Sessie sessie) {
			SessieGebruikerIngeschreven sgi = Context.IngeschrevenSessies.FirstOrDefault(s => s.SessieId == sessie.Id && s.GebruikerId.Equals(Context.Gebruikersnaam));

			if (sgi != null) // De gebruiker is al ingeschreven
				throw new InvalidOperationException("Gebruiker is al ingeschreven");

			try {
				sgi = new SessieGebruikerIngeschreven(sessie, Context);
				sessie.SchrijfGebruikerIn(sgi);
				Context.IngeschrevenSessies.Add(sgi);
			}
			catch (InvalidOperationException e) {
				throw new InvalidOperationException(e.Message);
			}

		}

		override
		public void SchrijfUitVoorSessie(Sessie sessie) {
			SessieGebruikerIngeschreven sgi = Context.IngeschrevenSessies.FirstOrDefault(sgi => sgi.SessieId == sessie.Id);

			if (sgi == null) // De gebruiker is nietgeschreven
				throw new InvalidOperationException("gebruiker is niet ingeschreven");

			try {
				sessie.SchrijfGebruikerUit(sgi);
				Context.IngeschrevenSessies.Remove(sgi);
			}
			catch (InvalidOperationException e) {
				throw new InvalidOperationException(e.Message);
			}
		}
	}
}