using Projecten2_DOTNET.Data.Mappers;
using Projecten2_DOTNET.Models.Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projecten2_DOTNET {
	public class RegistratieGeslotenAanmeldenOpenState : SessieState {

		public RegistratieGeslotenAanmeldenOpenState() {

		}

		public RegistratieGeslotenAanmeldenOpenState(Sessie context) : base(context) {
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
		public void SluitSessie() {
			Context.ToState(new AanmeldenEnRegistratieGeslotenState(Context));
		}
	}
}