using Projecten2_DOTNET.Models.Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_DOTNET.Data {
	public class ViewModel {
		public IEnumerable<Sessie> Sessies { get; set; }
		public Gebruiker Gebruiker { get; set; }

	}
}
