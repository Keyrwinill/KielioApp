/*
	Update date			Description 
	--------------------------------
	20260126			Initial
*/
using Microsoft.AspNetCore.Mvc;
using WebService.Interfaces;

namespace KielioWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class VerbController : ControllerBase
	{
		private readonly IMoodSettingService _moodSettingService;

		public VerbController(IMoodSettingService moodSettingService
							  )
		{
			_moodSettingService = moodSettingService;
		}

		[HttpGet]
		public IActionResult GetMood([FromQuery] string? language, [FromQuery] string? mood)
		{
			var response = _moodSettingService.GetMood(language, mood);
			return Ok(response);
		}
	}
}
