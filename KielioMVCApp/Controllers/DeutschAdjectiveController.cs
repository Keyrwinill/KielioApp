/*
	Update date			Description 
	--------------------------------
	20251115			Initial
*/
using Microsoft.AspNetCore.Mvc;
using WebService.Interfaces;

namespace KielioApp.Controllers;

public class DeutschAdjectiveController : Controller
{
	private readonly IDeutschAdjektivService _deutschAdjektivService;

	public DeutschAdjectiveController(IDeutschAdjektivService deutschAdjektivService)
	{
		_deutschAdjektivService = deutschAdjektivService;
	}

	[HttpGet]
	[Route("/Language/Deutsch/Adjective")]
	public IActionResult Index(string articleType = "")
	{
		var adjectives = _deutschAdjektivService.GetViewByType(articleType);
		ViewBag.SelectedType = articleType;
		return View(adjectives);
	}
}
