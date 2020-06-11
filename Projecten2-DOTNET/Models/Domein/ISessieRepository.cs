using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_DOTNET.Models.Domein {
	public interface ISessieRepository {
		IEnumerable<Sessie> GetAll();

		Sessie GetById(int id);

		IEnumerable<Sessie> GetByDate(DateTime date, DateTime tot);

		void SaveChanges();
	}
}
