using Projecten2_DOTNET.Data.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_DOTNET.Models.Domein {
	public class RegistratieEnAanmeldenOpenState : SessieState {

		public RegistratieEnAanmeldenOpenState(Sessie context) : base(context) {

		}
		public RegistratieEnAanmeldenOpenState() {

		}

		override
		public void SchrijfGebruikerIn(SessieGebruikerIngeschreven sgi) {
			if (Context.IngeschrevenGebruikers.Count == Context.Lokaal.AantalPlaatsen) {
				throw new InvalidOperationException("Het lokaal is volzet");
			}
			SessieGebruikerIngeschreven sessieGebruikerIngeschreven = Context.IngeschrevenGebruikers.FirstOrDefault(s => s.SessieId == sgi.SessieId && s.GebruikerId.Equals(sgi.GebruikerId));
			if (sessieGebruikerIngeschreven != null) {
				throw new InvalidOperationException("Gebruiker is al ingeschreven");
			}


			Context.IngeschrevenGebruikers.Add(sgi);


		}

		override
		public void SchrijfGebruikerUit(SessieGebruikerIngeschreven sgi) {
			SessieGebruikerIngeschreven sessieGebruikerIngeschreven = Context.IngeschrevenGebruikers.FirstOrDefault(s => s.SessieId == sgi.SessieId && s.GebruikerId.Equals(sgi.GebruikerId));
			if (sessieGebruikerIngeschreven == null) {
				throw new InvalidOperationException("Deze gebruiker is niet ingeschreven");
			}
			Context.IngeschrevenGebruikers.Remove(sgi);
		}
		override
		public SessieGebruikerAanwezig GebruikerAanwezigMelden(SessieGebruikerIngeschreven sgi) {
			SessieGebruikerAanwezig sga = null;
			SessieGebruikerIngeschreven sessieGebruikerIngeschreven = Context.IngeschrevenGebruikers.FirstOrDefault(s => s.SessieId == sgi.SessieId && s.GebruikerId.Equals(sgi.GebruikerId));
			if (sessieGebruikerIngeschreven != null) {
				sga = new SessieGebruikerAanwezig(sgi.Sessie, sgi.Gebruiker);
				Context.AanwezigeGebruikers.Add(sga);
			}
			else {
				throw new InvalidOperationException("Gebruiker moet ingeschreven zijn om aan te melden");
			}

			return sga;
		}

		override
		public void SluitVoorRegistratie() {
			Context.ToState(new RegistratieGeslotenAanmeldenOpenState(Context));
		}


	}
}
