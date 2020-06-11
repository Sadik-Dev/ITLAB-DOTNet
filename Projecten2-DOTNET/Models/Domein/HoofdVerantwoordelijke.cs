using Projecten2_DOTNET.Models.Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projecten2_DOTNET {
	public class HoofdVerantwoordelijke : Gebruiker {

		public override IList<Sessie> GeorganiseerdeSessies { get; set; }

		public HoofdVerantwoordelijke(string naam, string gebruikersnaam, long barcode) : base(naam, gebruikersnaam, barcode) {
		}

		public HoofdVerantwoordelijke() {
			// voor EF
		}

		override
		public void SluitSessie(Sessie sessie) {
			//if (GeorganiseerdeSessies.Contains(sessie)) {
			//	try {

			//! De hoofdverantwoordelijke ag altijd een sessie sluiten
			sessie.SluitVoorRegistratie();
			//	}
			//	catch (InvalidOperationException e) {
			//		throw new InvalidOperationException(e.Message);
			//	}
			//}
			//else {
			//	throw new InvalidOperationException("Sessie niet gevonden");
			//}
		}

		override
		public void OpenVoorAanmelden(Sessie sessie) {
			//if (GeorganiseerdeSessies.Contains(sessie)) {
			//	try {

			//! De hoofdverantwoordelijke ag altijd een sessie openen
			sessie.OpenVoorAanmelden();
			//	}
			//	catch (InvalidOperationException e) {
			//		throw new InvalidOperationException(e.Message);
			//	}
			//}
			//else {
			//	throw new InvalidOperationException("Sessie niet gevonden");
			//}
		}

		override
		public void SluitVoorRegistratie(Sessie sessie) {
			if (GeorganiseerdeSessies.Contains(sessie)) {
				try {
					sessie.SluitVoorRegistratie();
				}
				catch (InvalidOperationException e) {
					throw new InvalidOperationException(e.Message);
				}
			}
			else {
				throw new InvalidOperationException("Sessie niet gevonden");
			}
		}
	}
}