using Microsoft.EntityFrameworkCore;
using Projecten2_DOTNET.Models.Domein;
using System.Collections.Generic;
using System.Linq;

namespace Projecten2_DOTNET.Data.Repositories {
	public class GebruikerRepository : IGebruikerRepository {
		private readonly ApplicationDbContext _context;
		private readonly DbSet<Gebruiker> _gebruikers;

		public GebruikerRepository(ApplicationDbContext context) {
			_context = context;
			_gebruikers = _context.Gebruikers;
		}
		public IEnumerable<Gebruiker> GetAll() {
			return _gebruikers
				.Include(g => g.CurrentState)
				.Include(g => g.AanwezigeSessies)
				.Include(g => g.IngeschrevenSessies)
				.Include(g => g.GeorganiseerdeSessies).ThenInclude(sessie => sessie.CurrentState);
		}

		public Gebruiker GetByBarcode(long barcode) {
			return _gebruikers
				.Include(g => g.CurrentState)
				.Include(g => g.AanwezigeSessies)
				.Include(g => g.IngeschrevenSessies)
				.Include(g => g.GeorganiseerdeSessies).ThenInclude(sessie => sessie.CurrentState)
				.Include(g => g.GeorganiseerdeSessies).ThenInclude(sessie => sessie.Lokaal)
				.FirstOrDefault(g => g.Barcode == barcode);
		}

		public Gebruiker GetByGebruikersnaam(string gebruikersnaam) {
			return _gebruikers
				.Include(g => g.CurrentState)
				.Include(g => g.AanwezigeSessies)
				.Include(g => g.IngeschrevenSessies)
				.Include(g => g.GeorganiseerdeSessies).ThenInclude(sessie => sessie.CurrentState)
				.Include(g => g.GeorganiseerdeSessies).ThenInclude(sessie => sessie.Lokaal)
				.FirstOrDefault(g => g.Gebruikersnaam.Equals(gebruikersnaam));
		}

		public void SaveChanges() {
			_context.SaveChanges();
		}
	}
}
