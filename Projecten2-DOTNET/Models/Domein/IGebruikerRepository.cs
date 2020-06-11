using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_DOTNET.Models.Domein {
	public interface IGebruikerRepository {
		IEnumerable<Gebruiker> GetAll();

		Gebruiker GetByGebruikersnaam(string gebruikersnaam); // id is de gebruikersnaam

		Gebruiker GetByBarcode(long barcode);
		void SaveChanges();

	}
}
