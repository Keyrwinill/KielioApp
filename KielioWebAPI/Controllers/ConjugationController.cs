/*
	Update date			Description 
	--------------------------------
	20260126			Initial
*/
using Microsoft.AspNetCore.Mvc;
using ViewModel.Models.NeutralModel;
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
		public IActionResult GetConjugation([FromQuery] string? infinitive)
		{
			if (string.IsNullOrEmpty(infinitive))
			{
				return BadRequest("Infinitive is required.");
			}

			var response = _conjugationService.GetConjugation(infinitive);
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
