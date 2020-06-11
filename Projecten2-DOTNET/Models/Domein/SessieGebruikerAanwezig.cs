using Projecten2_DOTNET.Models.Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_DOTNET.Models.Domein {
	public class SessieGebruikerAanwezig {
		public int SessieId { get; set; }
		public Sessie Sessie { get; set; }

		public string GebruikerId { get; set; }
		public Gebruiker Gebruiker { get; set; }

		public SessieGebruikerAanwezig(Sessie s, Gebruiker g) {
			Sessie = s;
			Gebruiker = g;
			SessieId = s.Id;
			GebruikerId = g.Gebruikersnaam;
		}

		public SessieGebruikerAanwezig() {
			// Moet hiet staan voor EF
		}
	}
}
