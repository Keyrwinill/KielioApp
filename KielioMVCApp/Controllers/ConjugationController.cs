/*
	Update date			Description 
	--------------------------------
	20260317			Initial
*/
using DataAccessLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using ViewModel.Models.NeutralModels;
using WebService.Interfaces;

namespace KielioMVCApp.Controllers
{
	public class ConjugationController : Controller
	{
		private readonly IConjugationService _conjugationService;

		public ConjugationController(IConjugationService conjugationService)
		{
			_conjugationService = conjugationService;
		}

		[HttpGet]
		[Route("/Language/{languageName}/Conjugation")]
		public IActionResult GetConjugation(string? languageName, string? verb, string? mood = "", string? tense = "")
		{
			ViewBag.Title = $"{languageName} Verb";
			ViewBag.LanguageName = languageName;

			var response = FetchData(languageName, verb, mood, tense);
			return View("Index", response);
		}

		[HttpGet]
		[Route("/Language/{languageName}/Conjugation/Modify")]
		public IActionResult Modify(string? mode, string? languageName, string? verb = "")
		{
			//var response = FetchData(languageName, verb, mood, tense);
			var languageNameEng = languageName?.ToLower() switch
			{
				"deutsch" => "German",
				"français" => "French",
				_ => languageName
			};
			var response = new 
			{ 
				Mode = mode,
				Infinitive = verb,
				MoodList = _conjugationService.GetMoodList(languageNameEng)
			};
			return View("Modify", response);
		}

		[HttpGet]
		public IActionResult GetTenseListByMood(string mood)
		{
			var tenseList = _conjugationService.GetTenseList(mood);
			return Json(tenseList);
		}

		[HttpGet]
		public IActionResult GetPersonList(string mood)
		{
			var personList = _conjugationService.GetPersonList(mood);
			return Json(personList);
		}

		[HttpGet]
		public IActionResult GetVerbForms(string verb, string mood, string tense)
		{
			var verbForms = _conjugationService.GetFormByTense(verb, mood, tense);
			return Json(verbForms);
		}

		public ConjugationModel FetchData(string? languageName, string? verb = "", string? mood = "", string? tense = "")
		{
			var response = new ConjugationModel();
			var languageNameEng = languageName?.ToLower() switch
			{
				"deutsch" => "German",
				"français" => "French",
				_ => languageName
			};

			if (string.IsNullOrEmpty(verb))
			{
				response = new ConjugationModel
				{
					Language = languageNameEng,
					MoodList = _conjugationService.GetMoodList(languageNameEng),
					Infinitive = "",
					Translation = "",
					PastParticiple = "",
					Reflexive = false,
					Separable = false,
					Mood = mood,
					Tense = tense,
					Forms = new Dictionary<string, string>()
				};
			}
			else
			{
				response = _conjugationService.GetConjugation(languageNameEng, verb, mood, tense);
			}
			return response;
		}

		[HttpPost]
		[Route("/Language/Deutsch/Conjugation")]
		public async Task<IActionResult> SaveVerb(ConjugationModel request)
		{
			if (request == null)
			{
				return BadRequest("Invalid request.");
			}

			await _conjugationService.SaveVerb(request);
			return RedirectToAction(nameof(Index));
		}
	}
}
