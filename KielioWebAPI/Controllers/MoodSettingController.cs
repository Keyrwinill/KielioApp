/*
	Update date			Description 
	--------------------------------
	20260122			Initial
*/
using Microsoft.AspNetCore.Mvc;
using WebService.Interfaces;
using ViewModel.Models.RequestModels;

namespace KielioWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MoodSettingController : ControllerBase
	{
		private readonly IMoodSettingService _moodSettingService;

		public MoodSettingController(IMoodSettingService moodSettingService)
		{
			_moodSettingService = moodSettingService;
		}

		[HttpGet]
		public IActionResult GetMood([FromQuery] string? language, [FromQuery] string? mood)
		{
			var response = _moodSettingService.GetMood(language, mood);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> SaveMood([FromBody] MoodSettingRequestModel request)
		{
			if (request == null || string.IsNullOrEmpty(request.LanguageName))
			{
				return BadRequest("Invalid request.");
			}

			await _moodSettingService.SaveMood(request);
			return Ok();
		}
	}
}
