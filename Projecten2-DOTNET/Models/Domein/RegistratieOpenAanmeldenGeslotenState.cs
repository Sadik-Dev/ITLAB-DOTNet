using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_DOTNET.Models.Domein {
	public class RegistratieOpenAanmeldenGeslotenState : SessieState {

		public RegistratieOpenAanmeldenGeslotenState(Sessie context) : base(context) {

		}
		public RegistratieOpenAanmeldenGeslotenState() {

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
		public void OpenVoorAanmelden() {
			Context.ToState(new RegistratieEnAanmeldenOpenState(Context));
		}

	}
}
