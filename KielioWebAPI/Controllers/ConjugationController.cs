/*
	Update date			Description 
	--------------------------------
	20260126			Initial
*/
using DataAccessLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using ViewModel.Models.NeutralModels;
using ViewModel.Models.ResponseModels;
using WebService.Interfaces;

namespace KielioWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ConjugationController : ControllerBase
	{
		private readonly IConjugationService _conjugationService;

		public ConjugationController(IConjugationService conjugationService)
		{
			_conjugationService = conjugationService;
		}

		[HttpGet("/Language/{languageName}/Conjugation")]
		public IActionResult GetConjugation([FromQuery] string? languageName, [FromQuery] string? verb, [FromQuery] string? mood = "", [FromQuery] string? tense = "")
		{
			try
			{
				var data = FetchData(languageName, verb, mood, tense);
				var response = new ResponseModel<ConjugationModel>
				{
					Success = true,
					ErrorCode = "0000",
					Message = "",
					Data = data
				};
				return Ok(response);
			}
			catch (HttpRequestException)
			{
				var response = new ResponseModel<ConjugationModel>
				{
					Success = false,
					ErrorCode = "9998",
					Message = "Cannot reach the server."
				};
				return BadRequest(response);
			}
			catch (Exception ex)
			{
				var response = new ResponseModel<ConjugationModel>
				{
					Success = false,
					ErrorCode = "9999",
					Message = $"An unexpected error occurred. {ex.Message}"
				};
				return BadRequest(response);
			}
		}

		[HttpGet("/Language/{languageName}/Conjugation/Modify")]
		public IActionResult Modify(string mode,
							string languageName,
							string verb = "",
							string mood = "",
							string tense = "",
							string saveCode = "",
							string saveMessage = "")
		{
			if (mode == "modify")
			{
				return Ok(FetchData(languageName, verb, mood, tense);
			}
			else
			{
				return Ok(new ConjugationModel
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
			return Ok(tenseList);
		}

		[HttpGet]
		public IActionResult GetPersonList(string mood)
		{
			var personList = _conjugationService.GetPersonList(mood);
			return Ok(personList);
		}

		[HttpGet]
		public IActionResult CheckVerb(string languageName, string verb)
		{
			bool exists = _conjugationService.Exists(languageName, verb);

			return Ok(new
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
			return Ok(verbForms);
		}

		public ConjugationModel? FetchData(string? languageName, string? verb = "", string? mood = "", string? tense = "")
		{
			return _conjugationService.GetConjugation(languageName, verb, mood, tense);
		}

		[HttpPost("/Language/Conjugation")]
		public async Task<IActionResult> SaveVerb([FromBody] ConjugationModel request, string languageName, string mode)
		{
			try
			{
				if (request == null)
				{
					// Go to create mode when there's error
					return BadRequest(new
					{
						mode = "create",
						languageName = languageName,
						saveCode = "error",
						saveMessage = "Invalid request!"
					});
				}

				await _conjugationService.SaveVerb(request, languageName);
				return Ok(new
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
				return BadRequest(new
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
