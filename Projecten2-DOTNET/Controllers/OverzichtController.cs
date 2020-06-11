using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Projecten2_DOTNET.Filter;
using Projecten2_DOTNET.Models.Domein;

namespace Projecten2_DOTNET.Controllers {
	[ServiceFilter(typeof(GebruikerFilter))]
	[Authorize(Policy = "Gebruiker")]

	public class OverzichtController : Controller {
		private readonly ISessieRepository _sessieRepository;
		private readonly IGebruikerRepository _gebruikerRepository;


		public OverzichtController(ISessieRepository sessieRepository, IGebruikerRepository gebruikerRepository) {
			_sessieRepository = sessieRepository;
			_gebruikerRepository = gebruikerRepository;
		}
		public IActionResult Index(Gebruiker gebruiker, string sorteerOptie = "datum") {

			IEnumerable<Sessie> sessies = _sessieRepository.GetAll().Where(s => s.Start.Month == DateTime.Now.Month);
			Console.WriteLine(sorteerOptie);
			Console.WriteLine(gebruiker.GetType());

			if (!sessies.Any()) { // toch alle sessies ophalen
				sessies = _sessieRepository.GetAll();
			}
			//Sorteer
			if (sorteerOptie.Equals("datum"))
				sessies = sessies.OrderBy(b => b.Start);
			else if (sorteerOptie.Equals("titel"))
				sessies = sessies.OrderBy(b => b.Titel);
			else if (sorteerOptie.Equals("verantwoordelijke"))
				sessies = sessies.OrderBy(b => b.Verantwoordelijke.Gebruikersnaam);

			ViewData["sorteerOptie"] = sorteerOptie;
			ViewData["gebruikerId"] = gebruiker.Gebruikersnaam;
			ViewData["gebruikerINITIALS"] = gebruiker.GeefInitials();


			ViewData["filterDate"] = new String[2] { sessies.First().Start.ToString("dd-MM-yyyy"), sessies.Last().Start.ToString("dd-MM-yyyy") };


			if (gebruiker is Verantwoordelijke || gebruiker is HoofdVerantwoordelijke) {
				ViewData["isVerantwoordelijke"] = true;
			}
			else {
				ViewData["isVerantwoordelijke"] = false;

			}


			return View(sessies);
		}

		public IActionResult Filter(Gebruiker gebruiker, string vanS, string totS, string sorteerOptie = "datum") {


			if (vanS.Equals("undefined") || totS.Equals("undefined")) {
				return RedirectToAction(nameof(Index));
			}

			DateTime van = DateTime.Parse(vanS);
			DateTime tot = DateTime.Parse(totS);

			IEnumerable<Sessie> sessies = _sessieRepository.GetByDate(van, tot);
			string geenSessie = "Er zijn geen sessies beschikbaar";
			if (!sessies.Any())
			{
				ViewData["geensessies"] = geenSessie;
			}
			//Sorteer
			if (sorteerOptie.Equals("datum"))
				sessies = sessies.OrderBy(b => b.Start);
			else if (sorteerOptie.Equals("titel"))
				sessies = sessies.OrderBy(b => b.Titel);
			else if (sorteerOptie.Equals("verantwoordelijke"))
				sessies = sessies.OrderBy(b => b.Verantwoordelijke.Gebruikersnaam);

			ViewData["sorteerOptie"] = sorteerOptie;

			ViewData["gebruikerId"] = gebruiker.Gebruikersnaam;
			ViewData["gebruikerINITIALS"] = gebruiker.GeefInitials();
			ViewData["filterDate"] = new String[2] { van.ToString("dd-MM-yyyy"), tot.ToString("dd-MM-yyyy") };
			if (gebruiker is Verantwoordelijke || gebruiker is HoofdVerantwoordelijke) {
				ViewData["isVerantwoordelijke"] = true;
			}
			else {
				ViewData["isVerantwoordelijke"] = false;

			}

			return View(nameof(Index), sessies);

		}

		public IActionResult ResetFilter() {
			// dit reset de filter naar niets
			return RedirectToAction(nameof(Index));
		}

		public IActionResult SchrijfInVoorSessie(Gebruiker gebruiker, int sessieId) {
			// de entities moeten getracked worden voor het updaten van de db
			Sessie trackedSessie = _sessieRepository.GetById(sessieId);
			Gebruiker trackedGebruiker = _gebruikerRepository.GetByGebruikersnaam(gebruiker.Gebruikersnaam);

			if (trackedSessie == null) { // De sessie bestaat niet
				TempData["error"] = "De sessie bestaat niet";
				return RedirectToAction(nameof(Index));
			}

			try {

				trackedGebruiker.SchrijfGebruikerInSessie(trackedSessie);
				// dit is om op te slaan in de db
				_sessieRepository.SaveChanges();
				TempData["message"] = $"Inschrijven voor sessie '{trackedSessie.Titel}' succesvol!";
				return RedirectToAction(nameof(Index));

			}
			catch (Exception e) {
				TempData["error"] = e.Message;
				return RedirectToAction(nameof(Index));
			}
		}

		public IActionResult SchrijfUitVoorSessie(Gebruiker gebruiker, int sessieId) {
			// de entities moeten getracked worden voor het updaten van de db
			Sessie trackedSessie = _sessieRepository.GetById(sessieId);
			Gebruiker trackedGebruiker = _gebruikerRepository.GetByGebruikersnaam(gebruiker.Gebruikersnaam);

			if (trackedSessie == null) { // De sessie bestaat niet
				TempData["error"] = "De sessie bestaat niet"; //TODO een tempdata vakje maken in de view voor de errors en de messages te tonen
				return RedirectToAction(nameof(Index));
			}

			try {

				trackedGebruiker.SchrijfGebruikerUitSessie(trackedSessie);
				// dit is om op te slaan in de db
				_sessieRepository.SaveChanges();
				TempData["message"] = $"Uitschrijven voor sessie '{trackedSessie.Titel}' succesvol!";
				return RedirectToAction(nameof(Index));

			}
			catch (Exception e) {
				TempData["error"] = e.Message;
				return RedirectToAction(nameof(Index));
			}
		}



	}
}