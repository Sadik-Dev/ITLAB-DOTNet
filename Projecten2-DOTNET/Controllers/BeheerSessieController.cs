using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projecten2_DOTNET.Filter;
using Projecten2_DOTNET.Models.Domein;

namespace Projecten2_DOTNET.Controllers {
	[ServiceFilter(typeof(GebruikerFilter))]
	[Authorize(Policy = "Verantwoordelijke")]
	public class BeheerSessieController : Controller {
		private readonly ISessieRepository _sessieRepository;
		private readonly IGebruikerRepository _gebruikerRepository;


		public BeheerSessieController(ISessieRepository sessieRepository, IGebruikerRepository gebruikerRepository) {
			_sessieRepository = sessieRepository;
			_gebruikerRepository = gebruikerRepository;
		}
		public IActionResult Index(Gebruiker gebruiker, string sorteerOptie = "datum") {
			IEnumerable<Sessie> sessies = null;

			Gebruiker trackedGebruiker = _gebruikerRepository.GetByGebruikersnaam(gebruiker.Gebruikersnaam);

			try {
				if (trackedGebruiker is Verantwoordelijke) {
					sessies = trackedGebruiker.GeorganiseerdeSessies.Where(s => s.CurrentState.GetType() == typeof(RegistratieOpenAanmeldenGeslotenState));
				}
				else if (trackedGebruiker is HoofdVerantwoordelijke) {

					sessies = _sessieRepository.GetAll().Where(s => s.CurrentState.GetType() == typeof(RegistratieOpenAanmeldenGeslotenState));

				}
				else {
					throw new Exception();
				}
			}
			catch (Exception) {
				TempData["error"] = "U bent geen (hoofd)verantwoordelijke";
				return RedirectToAction(nameof(Index), "Overzicht", _sessieRepository.GetAll());
			}

			Console.WriteLine(sorteerOptie);
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


			return View(sessies);
		}
		public IActionResult geOpendeSessie(Gebruiker gebruiker) {
			Gebruiker trackedGebruiker = _gebruikerRepository.GetByGebruikersnaam(gebruiker.Gebruikersnaam);
			int sessieid = (int)TempData["sessieid"];
			Sessie sessie = _sessieRepository.GetById(sessieid);
			//Controleren of sessie open is 
			if (sessie.CurrentState.GetType() == typeof(RegistratieGeslotenAanmeldenOpenState) || sessie.CurrentState.GetType() == typeof(RegistratieEnAanmeldenOpenState)) {
				//Controleren of gebruiker recht heeft op de view 
				if (sessie.Verantwoordelijke.Naam.Equals(gebruiker.Naam) || gebruiker is HoofdVerantwoordelijke) {
					ViewData["sessieid"] = sessieid;
					return View(sessie);

				}
				else {
					TempData["error"] = "U bent niet de verantwoorderlijke van deze sessie !";
					return RedirectToAction(nameof(Index));
				}

			}
			else {
				TempData["error"] = "De sessie is niet Open";
				return RedirectToAction(nameof(Index));
			}
		}
		//hiervoor moet een apparte view komen
		public IActionResult OpenSessieVoorAanmelden(Gebruiker gebruiker, int sessieId) {
			// de entities moeten getracked worden voor het updaten van de db
			Sessie trackedSessie = _sessieRepository.GetById(sessieId);
			Gebruiker trackedGebruiker = _gebruikerRepository.GetByGebruikersnaam(gebruiker.Gebruikersnaam);

			if (trackedSessie == null) { // De sessie bestaat niet
				TempData["error"] = "De sessie bestaat niet";
				return RedirectToAction(nameof(Index));
			}

			try {
				trackedGebruiker.OpenVoorAanmelden(trackedSessie);
				TempData["sessieid"] = trackedSessie.Id;
				// dit is om op te slaan in de db
				_sessieRepository.SaveChanges();
				//	TempData["message"] = $"openzetten van sessie '{trackedSessie.Titel}' voor aanmelden succesvol!";
				return RedirectToAction(nameof(geOpendeSessie));

			}
			catch (Exception e) {
				TempData["error"] = e.Message;
				return RedirectToAction(nameof(Index));
			}
		}

		public IActionResult MeldAanVoorSessie(Gebruiker gebruiker, int sessieId, string barcode) {
			// de entities moeten getracked worden voor het updaten van de db
			Sessie trackedSessie = _sessieRepository.GetById(sessieId);
			Gebruiker trackedGebruiker = _gebruikerRepository.GetByGebruikersnaam(gebruiker.Gebruikersnaam);

			if(barcode == null) {
				TempData["error"] = "De gebruiker bestaat niet";
				return RedirectToAction(nameof(geOpendeSessie));
			}

			if (trackedSessie == null) { // De sessie bestaat niet
				TempData["error"] = "De sessie bestaat niet";
				return RedirectToAction(nameof(Index));
			}

			Gebruiker aanTeMeldenGebruiker = null;

			Regex gebregex = new Regex("[0-9]{6}[a-zA-Z]{2}"); // regex voor een gebruikersnaam
			Regex barregex = new Regex("[0-9]{13}"); // regex voor een barcode

			if (gebregex.IsMatch(barcode)) { // het is een gebruikersnaam
				aanTeMeldenGebruiker = _gebruikerRepository.GetByGebruikersnaam(barcode);
			}
			else if (barregex.IsMatch(barcode)) { // het is een barcode
				aanTeMeldenGebruiker = _gebruikerRepository.GetByBarcode(long.Parse(barcode));
			}

			if (aanTeMeldenGebruiker == null) {
				TempData["error"] = "De gebruiker bestaat niet";
				return RedirectToAction(nameof(geOpendeSessie));
			}

			try {

				aanTeMeldenGebruiker.ZetAanwezigVoorSessie(trackedSessie);
				// dit is om op te slaan in de db
				_sessieRepository.SaveChanges();
				TempData["message"] = $"Aanmelden voor sessie '{trackedSessie.Titel}' succesvol!";
				return RedirectToAction(nameof(geOpendeSessie));

			}
			catch (Exception e) {
				TempData["error"] = e.Message;
				return RedirectToAction(nameof(geOpendeSessie));
			}
		}




	}
}