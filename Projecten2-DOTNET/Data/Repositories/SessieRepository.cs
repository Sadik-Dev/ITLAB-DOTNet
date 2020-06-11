using Microsoft.EntityFrameworkCore;
using Projecten2_DOTNET.Models.Domein;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Projecten2_DOTNET.Data.Repositories {
	public class SessieRepository : ISessieRepository {
		private readonly ApplicationDbContext _dbContext;
		private readonly DbSet<Sessie> _sessies;

		public SessieRepository(ApplicationDbContext dbContext) {
			_dbContext = dbContext;
			_sessies = dbContext.Sessies;
		}

		public IEnumerable<Sessie> GetAll() {

			// sessiestates update
			this.UpdateSessieState();
			//

			return _sessies.Include(s => s.Verantwoordelijke)
				.Include(s => s.Lokaal)
				.Include(s => s.CurrentState).ThenInclude(s => s.Context).ThenInclude(c => c.IngeschrevenGebruikers)
				.Include(s => s.IngeschrevenGebruikers).ThenInclude(s => s.Gebruiker)
				.Include(s => s.AanwezigeGebruikers).ThenInclude(s => s.Gebruiker)
				.Include(s => s.FeedbackEntries).ThenInclude(f => f.Auteur)
				.Include(s => s.Media);
		}

		public IEnumerable<Sessie> GetByDate(DateTime van, DateTime tot) {

			// sessiestates update
			this.UpdateSessieState();

			return _sessies.Include(p => p.Verantwoordelijke)
				.Include(p => p.Lokaal)
				.Include(n => n.CurrentState).ThenInclude(s => s.Context).ThenInclude(c => c.IngeschrevenGebruikers)
				.Include(p => p.IngeschrevenGebruikers).ThenInclude(s => s.Gebruiker)
				.Include(p => p.AanwezigeGebruikers).ThenInclude(s => s.Gebruiker)
				.Include(s => s.FeedbackEntries).ThenInclude(f => f.Auteur)
				.Include(p => p.Media)
				.Where(p => DateTime.Compare(p.Start, van) >= 0 && DateTime.Compare(p.Einde, tot) <= 0);
		}

		public Sessie GetById(int id) {

			// sessiestates update
			this.UpdateSessieState();

			return _sessies.Include(p => p.Verantwoordelijke)
				.Include(p => p.Lokaal)
				.Include(n => n.CurrentState).ThenInclude(s => s.Context).ThenInclude(c => c.IngeschrevenGebruikers)
				.Include(p => p.IngeschrevenGebruikers).ThenInclude(s => s.Gebruiker)
				.Include(p => p.AanwezigeGebruikers).ThenInclude(s => s.Gebruiker)
				.Include(s => s.FeedbackEntries).ThenInclude(f => f.Auteur)
				.Include(p => p.Media)
				.FirstOrDefault(p => p.Id == id);
		}

		//public IEnumerable<Sessie> getForUser(Gebruiker g) {

		//	// sessiestates update
		//	this.UpdateSessieState();

		//	IEnumerable<Sessie> sessies;

		//	if (g.GetType().IsInstanceOfType(new Verantwoordelijke())) {
		//		sessies = _sessies
		//			.Where(s =>
		//				s.CurrentState.GetType() == typeof(RegistratieOpenAanmeldenGeslotenState)
		//			&&
		//				s.Verantwoordelijke.Gebruikersnaam.Equals(g.Gebruikersnaam)
		//			);
		//	}
		//	else if (g.GetType().IsInstanceOfType(new HoofdVerantwoordelijke())) {
		//		sessies = _sessies
		//			.Where(s => s.CurrentState.GetType() == typeof(RegistratieOpenAanmeldenGeslotenState));

		//	}
		//	else {
		//		throw new Exception();
		//	}

		//	return sessies;
		//}

		public void SaveChanges() {
			_dbContext.SaveChanges();
		}

		private void UpdateSessieState() {

			DateTime now = DateTime.Now;
			IEnumerable<Sessie> teCheckenSessies =
				_sessies.Include(n => n.CurrentState)
						.Where(s => s.Einde < now); // enkel de sessies die nog in de toekomst liggen

			foreach (Sessie s in teCheckenSessies) {
				if (now >= s.Einde) { // sessieeinde ligt in het verleden
					s.ToState(new AanmeldenEnRegistratieGeslotenState(s));
				}
				else if (now >= s.Start.AddMinutes(30)) { // de sessie is al 30 min bezig
					s.ToState(new AanmeldenEnRegistratieGeslotenState(s));
				}
				else if (now >= s.Start) { // de sessie is gestart
					s.ToState(new RegistratieGeslotenAanmeldenOpenState(s));
				}
				else if (now.AddHours(1) >= s.Start) { // de sessie begint in minder dan een uur
					s.ToState(new RegistratieEnAanmeldenOpenState(s));
				}

				// als we hier raken, duurt het nog langer dan 1 uur voor de sessie begint.
			}

			this.SaveChanges();
		}

	}
}
