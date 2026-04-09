/*
	Update date			Description 
	--------------------------------
	20260126			Initial
*/
using DataAccessLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using ViewModel.Models.NeutralModels;
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

		[HttpGet]
		public IActionResult GetConjugation([FromQuery] string? languageName, [FromQuery] string? verb, [FromQuery] string? mood = "", [FromQuery] string? tense = "")
		{
			if (string.IsNullOrEmpty(verb))
			{
				return BadRequest("Verb is required.");
			}

			var response = _conjugationService.GetConjugation(languageName, verb, mood, tense);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> SaveVerb([FromBody] ConjugationModel request)
		{
			if (request == null)
			{
				return BadRequest("Invalid request.");
			}

			await _conjugationService.SaveVerb(request);
			return Ok();
		}
	}
}
