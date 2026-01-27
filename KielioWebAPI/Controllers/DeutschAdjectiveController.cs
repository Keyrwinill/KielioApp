/*
	Update date			Description 
	--------------------------------
	20260122			Initial
*/
using Microsoft.AspNetCore.Mvc;
using WebService.Interfaces;

namespace KielioWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DeutschAdjectiveController : ControllerBase
	{
		private readonly IDeutschAdjektivService _deutschAdjektivService;

		public DeutschAdjectiveController(IDeutschAdjektivService deutschAdjektivService)
		{
			_deutschAdjektivService = deutschAdjektivService;
		}

		[HttpGet]
		public IActionResult GetByType([FromQuery] string? articleType = "")
		{
			var response = _deutschAdjektivService.GetViewByType(articleType);
			return Ok(response);
		}
	}
}
