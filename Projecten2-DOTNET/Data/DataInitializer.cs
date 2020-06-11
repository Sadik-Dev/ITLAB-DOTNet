using Microsoft.AspNetCore.Identity;
using Projecten2_DOTNET.Data.Repositories;
using Projecten2_DOTNET.Models.Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Projecten2_DOTNET.Data {
	public class DataInitializer {
		private readonly ApplicationDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;


		public DataInitializer(ApplicationDbContext context, UserManager<IdentityUser> userManager) {
			_context = context;
			_userManager = userManager;


		}

		public async Task InitializeData() {
			// Op het juiste moment uit commentaar zetten om te voorkomen dat je de db steeds opnieuw maakt
			_context.Database.EnsureDeleted();
			if (_context.Database.EnsureCreated()) {
			//if (!_context.Sessies.Any()) {

				ISessieRepository sessieRepo = new SessieRepository(_context);

				HoofdVerantwoordelijke labijn = new HoofdVerantwoordelijke("Sebastiaan Labijn", "987654sl", 1234567891234);
				Verantwoordelijke gates = new Verantwoordelijke("Bill Gates", "789456bg", 9876543219876);
				Verantwoordelijke obama = new Verantwoordelijke("Barack Obama", "654123bo", 7412589631478);

				Lokaal b1025 = new Lokaal("B1.025", 50);
				Lokaal b2036 = new Lokaal("B2.036", 80);
				Lokaal b4026 = new Lokaal("B4.026", 30);

				Gebruiker geb1 = new Gebruiker("Kevin Degrote", "123456kd", 9512634879548);
				Gebruiker geb2 = new Gebruiker("Hans Peters", "123456hp", 0321456987589);


				IList<Gebruiker> gebruikers = new List<Gebruiker> { geb1, geb2, labijn, obama, gates };

				await InitializeUsers(gebruikers.ToArray());

				Sessie sessie1 = new Sessie(labijn, "Angular Update", "Geen", b1025, new DateTime(2020, 04, 13, 12, 30, 00), new DateTime(2020, 04, 13, 13, 25, 00));


				Sessie sessie2 = new Sessie(obama, "How to Switch from Carrier", "Michelle Obama", b2036, new DateTime(2020, 04, 13, 12, 30, 00), new DateTime(2020, 04, 13, 13, 30, 00));


				Sessie sessie3 = new Sessie(gates, "Windows 20 Conference", "Geen", b1025, new DateTime(2020, 04, 16, 14, 30, 00), new DateTime(2020, 04, 16, 16, 30, 00));


				Sessie sessie4 = new Sessie(labijn, "C++ Intro", "Thomas Kempens", b4026, new DateTime(2020, 06, 09, 14, 30, 00), new DateTime(2020, 06, 09, 18, 30, 00));


				Sessie sessie5 = new Sessie(gates, "How to be a Good Programmer ?", "Geen", b4026, new DateTime(2020, 06, 20, 12, 30, 00), new DateTime(2020, 06, 20, 12, 25, 00), new List<Media>() { new Media("/images/MediaImages/BillGatesProjecten.jpg"), new Media("/images/MediaFiles/BillGates.pdf"), new Media("/images/MediaLinks/BillGates.txt") });

				Sessie sessie6 = new Sessie(obama, "Advanced SQL", "Angeline Van Achter", b2036, new DateTime(2020, 06, 24, 09, 30, 00), new DateTime(2020, 06, 24, 12, 30, 00));


				Sessie sessie7 = new Sessie(labijn, "Advanced python", "Geen", b1025, new DateTime(2020, 08, 14, 14, 30, 00), new DateTime(2020, 08, 14, 17, 30, 00));


				Sessie sessie8 = new Sessie(gates, "Why Ruby on Rails ?", "Ruby expert", b4026, new DateTime(2020, 08, 15, 12, 30, 00), new DateTime(2020, 08, 15, 14, 30, 00));


				Sessie sessie9 = new Sessie(obama, "Java for newbies", "Geen", b4026, new DateTime(2020, 08, 28, 10, 00, 00), new DateTime(2020, 08, 28, 11, 00, 00));

				Sessie sessie10 = new Sessie(obama, "Java for pros", "Geen", b4026, new DateTime(2020, 08, 28, 10, 00, 00), new DateTime(2020, 08, 28, 11, 00, 00));


				// DIT WORDT NU AUTOMATISCHE GEDAAN IN DE SESSIEREPO
				//sessie2.OpenVoorAanmelden();

				//sessie3.OpenVoorAanmelden();

				//sessie7.OpenVoorAanmelden();
				//sessie7.SluitVoorRegistratie();

				//sessie9.OpenVoorAanmelden();
				//sessie9.SluitVoorRegistratie();
				//sessie9.SluitSessie();


				//! dit gaat indirect de sessiestates updaten
				sessieRepo.GetAll();
				//!

				_context.Sessies.AddRange(sessie1, sessie2, sessie3, sessie4, sessie5, sessie6, sessie7, sessie8, sessie9, sessie10);
				_context.SaveChanges();


				//geb1.SchrijfGebruikerInSessie(sessie1);
				//geb1.SchrijfGebruikerInSessie(sessie2);
				//geb1.SchrijfGebruikerInSessie(sessie3);
				geb1.SchrijfGebruikerInSessie(sessie4);
				geb1.SchrijfGebruikerInSessie(sessie5);
				geb1.SchrijfGebruikerInSessie(sessie8);

				geb2.SchrijfGebruikerInSessie(sessie4);
				geb2.SchrijfGebruikerInSessie(sessie5);
				geb2.SchrijfGebruikerInSessie(sessie7);

				//geb1.ZetAanwezigVoorSessie(sessie2);
				//geb1.ZetAanwezigVoorSessie(sessie3);


				_context.SaveChanges();


			}
		}


		private async Task InitializeUsers(Gebruiker[] gebruikers) {

			foreach (Gebruiker gebruiker in gebruikers) {

				IdentityUser user = new IdentityUser { UserName = gebruiker.Gebruikersnaam, NormalizedUserName = gebruiker.Naam };
				await _userManager.CreateAsync(user, "P@ssword1");

				if (gebruiker is Verantwoordelijke || gebruiker is HoofdVerantwoordelijke) {
					await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "verantwoordelijke"));
				}

				await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "gebruiker"));



				//if (gebruiker.Gebruikersnaam.Equals("123456kd")) {
				//	await _signInManager.SignInAsync(user, false);
				//}

				_context.Gebruikers.Add(gebruiker);
				_context.SaveChanges();
			}



		}
	}
}
