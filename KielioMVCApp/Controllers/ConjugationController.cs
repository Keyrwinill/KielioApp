/*
	Update date			Description 
	--------------------------------
	20260317			Initial
*/
using Microsoft.AspNetCore.Mvc;
using ViewModel.Models.NeutralModels;
using ViewModel.Models.ResponseModels;
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
			try
			{
				ViewBag.Title = $"{languageName} Verb";
				ViewBag.LanguageName = languageName;

				var data = FetchData(languageName, verb, mood, tense);
				var response = new ResponseModel<ConjugationModel>
				{
					Success = true,
					ErrorCode = "0000",
					Message = "",
					Data = data
				};
				return View("Index", response);
			}
			catch (HttpRequestException)
			{
				var response = new ResponseModel<ConjugationModel>
				{
					Success = false,
					ErrorCode = "9998",
					Message = "Cannot reach the server."
				};
				return View("Index", response);
			}
			catch (Exception ex)
			{
				var response = new ResponseModel<ConjugationModel>
				{
					Success = false,
					ErrorCode = "9999",
					Message = $"An unexpected error occurred. {ex.Message}"
				};
				return View("Index", response);
			}
		}

		[HttpGet]
		[Route("/Language/{languageName}/Conjugation/Modify")]
		public IActionResult Modify(string mode, 
									string languageName, 
									string verb = "",
									string mood = "",
									string tense = "",
									string saveCode = "", 
									string saveMessage = "")
		{
			ViewBag.Mode = mode;
			ViewBag.SaveCode = saveCode;
			ViewBag.SaveMessage = saveMessage;

			if (mode == "modify")
			{
				return View("Modify", FetchData(languageName, verb, mood, tense));
			}
			else
			{
				return View("Modify", new ConjugationModel
				{
					Language = languageName,
					MoodList = string.IsNullOrEmpty(languageName) ? new List<string>() : _conjugationService.GetMoodList(languageName),
					TenseList = new List<string>(),
					Infinitive = "",
					Translation = "",                                   //keep it for future design
					PastParticiple = "",                                //keep it for future design
					Reflexive = false,                                  //keep it for future design
					Separable = false,                                  //keep it for future design
					Mood = mood,                                        //chosen mood
					Tense = tense,                                      //chosen tense
					Forms = new Dictionary<string, string>()
				});
			}
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
		public IActionResult CheckVerb(string languageName, string verb)
		{
			bool exists = _conjugationService.Exists(languageName, verb);

			return Json(new
			{
				ErrorCode = exists ? "0000" : "0001",           //0000 => success; 0001 => not exists
				Message = exists ? "" : "Verb not found.",
				Language = languageName
			});
		}

		[HttpGet]
		public IActionResult GetVerbForms(string verb, string mood, string tense)
		{
			var verbForms = _conjugationService.GetFormByTense(verb, mood, tense);
			return Json(verbForms);
		}

		public ConjugationModel? FetchData(string? languageName, string? verb = "", string? mood = "", string? tense = "")
		{
			return _conjugationService.GetConjugation(languageName, verb, mood, tense);
		}

		[HttpPost]
		[Route("/Language/Conjugation")]
		public async Task<IActionResult> SaveVerb(ConjugationModel request, string languageName, string mode)
		{
			try
			{
				if (request == null)
				{
					// Go to create mode when there's error
					return RedirectToAction(nameof(Modify), new 
					{ 
						mode = "create",
						languageName = languageName,
						saveCode = "error",
						saveMessage = "Invalid request!" 
					});
				}

				await _conjugationService.SaveVerb(request, languageName);
				return RedirectToAction(nameof(Modify), new
				{ 
					mode = mode, 
					languageName = languageName,
					verb = request.Infinitive,
					mood = request.Mood,
					tense = request.Tense,
					saveCode = "succeed", 
					saveMessage = "Saving process is finished!" 
				});
			}
			catch (Exception ex)
			{
				return RedirectToAction(nameof(Modify), new 
				{ 
					mode = "create", 
					languageName = languageName, 
					saveCode = "error", 
					saveMessage = $"An unexpected error occurred. {ex.Message}" 
				});
			}
		}
	}
}
