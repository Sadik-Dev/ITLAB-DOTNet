using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projecten2_DOTNET.Filter;
using Projecten2_DOTNET.Models.Domein;
using Projecten2_DOTNET.Models.ViewModels;

namespace Projecten2_DOTNET.Controllers {
	[ServiceFilter(typeof(GebruikerFilter))]
	[Authorize(Policy = "Gebruiker")]
	public class FeedBackController : Controller {
		private readonly ISessieRepository _sessieRepository;
		private readonly IGebruikerRepository _gebruikerRepository;


		public FeedBackController(ISessieRepository sessieRepository, IGebruikerRepository gebruikerRepository) {
			_sessieRepository = sessieRepository;
			_gebruikerRepository = gebruikerRepository;
		}





		// GET: Feedback
		public IActionResult Index(Gebruiker gebruiker, int SessieID) {

			Gebruiker trackedGebruiker = _gebruikerRepository.GetByGebruikersnaam(gebruiker.Gebruikersnaam);
			if (trackedGebruiker == null) {
				TempData["error"] = "De gebruiker bestaat niet";
				return RedirectToAction(nameof(Index), "Overzicht", _sessieRepository.GetAll());
			}
			Sessie trackedSessie = _sessieRepository.GetById(SessieID);
			if (trackedSessie == null) {
				TempData["error"] = "De sessie bestaat niet";
				return RedirectToAction(nameof(Index), "Overzicht", _sessieRepository.GetAll());
			}

			ViewData["gebruikerId"] = gebruiker.Gebruikersnaam;
			ViewData["gebruikerINITIALS"] = gebruiker.GeefInitials();


			return View(trackedSessie);
		}


		public IActionResult maakNieuweFeedbackEntry(Gebruiker gebruiker, int sessieId, string inhoud, int score) {

			if (inhoud == null || inhoud.Equals("") || score == null) {
				TempData["error"] = "Geen geldige invoer";
				return RedirectToAction(nameof(Index), new { SessieId = sessieId });
			}

			Sessie trackedSessie = _sessieRepository.GetById(sessieId);
			if (trackedSessie == null) {
				TempData["error"] = "De sessie bestaat niet";
				return RedirectToAction(nameof(Index), new { SessieId = sessieId });
			}

			Gebruiker trackedGebruiker = _gebruikerRepository.GetByGebruikersnaam(gebruiker.Gebruikersnaam);
			if (trackedGebruiker == null) {
				TempData["error"] = "De gebruiker bestaat niet";
				return RedirectToAction(nameof(Index), new { SessieId = sessieId });
			}

			try {
				trackedGebruiker.geefFeedbackOpSessie(trackedSessie, inhoud, score);
				_sessieRepository.SaveChanges();

			}
			catch (Exception e) {
				TempData["error"] = e.Message;
				return RedirectToAction(nameof(Index), new { SessieId = sessieId });
			}

			ViewData["gebruikerId"] = gebruiker.Gebruikersnaam;
			ViewData["gebruikerINITIALS"] = gebruiker.GeefInitials();


			return RedirectToAction(nameof(Index), new { SessieID = sessieId });
		}
		/*
		// GET: Feedback/Create
		public ActionResult maakNieuweFeedbackEntry() {
			return View();
		}

		// POST: Feedback/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult maakNieuweFeedbackEntry(FeedbackEditViewModel feedbackEditViewModel, int sessieId) {
			Sessie trackedSessie = _sessieRepository.GetById(sessieId);

			if (ModelState.IsValid)
				try {
					FeedbackEntry feedback = new FeedbackEntry();
					MapFeedbackEditViewModelToFeedbackEntry(feedbackEditViewModel, feedback);
					trackedSessie.FeedbackEntries.Add(feedback);

					//TODO opslaan in databank
					_sessieRepository.SaveChanges();
					TempData["message"] = $"Feedback verzonden.";
				}
				catch {
					return View();
				}
			return RedirectToAction(nameof(Index));
		}
		*/

		private void MapFeedbackEditViewModelToFeedbackEntry(FeedbackEditViewModel feedbackViewmodel, FeedbackEntry feedback) {
			feedback.Inhoud = feedbackViewmodel.Inhoud;
			feedback.Score = feedbackViewmodel.Score;
			feedback.Auteur = feedbackViewmodel.Auteur;
		}

	}
}