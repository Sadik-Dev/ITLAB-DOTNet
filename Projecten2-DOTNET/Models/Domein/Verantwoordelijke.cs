using System;
using System.Collections.Generic;
using System.Linq;

namespace Projecten2_DOTNET.Models.Domein {
	public class Verantwoordelijke : Gebruiker {

		public override IList<Sessie> GeorganiseerdeSessies { get; set; }

		public Verantwoordelijke(string naam, string gebruikersnaam, long barcode) : base(naam, gebruikersnaam, barcode) {
		}

		public Verantwoordelijke() {
			// Dit moet hier staan voor EF
		}

		override
		public void OpenVoorAanmelden(Sessie sessie) {
			if (GeorganiseerdeSessies.Contains(sessie)) {
				try {
					sessie.OpenVoorAanmelden();
				}
				catch (InvalidOperationException e) {
					throw new InvalidOperationException(e.Message);
				}
			}
			else {
				throw new InvalidOperationException("sessie moet door deze verandwoordelijke zijn aangemaakt");
			}

		}


		public override void SluitSessie(Sessie sessie) {
			if (GeorganiseerdeSessies.Contains(sessie)) {
				try {
					sessie.SluitSessie();
				}
				catch (InvalidOperationException e) {
					throw new InvalidOperationException(e.Message);
				}
			}
			else {
				throw new InvalidOperationException("sessie moet door deze verandwoordelijke zijn aangemaakt");
			}
		}


		public override void SluitVoorRegistratie(Sessie sessie) {
			if (GeorganiseerdeSessies.Contains(sessie)) {
				try {
					sessie.SluitVoorRegistratie();
				}
				catch (InvalidOperationException e) {
					throw new InvalidOperationException(e.Message);
				}
			}
			else {
				throw new InvalidOperationException("sessie moet door deze verandwoordelijke zijn aangemaakt");
			}
		}
	}
}
