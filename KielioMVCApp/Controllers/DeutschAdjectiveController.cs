/*
	Update date			Description 
	--------------------------------
	20251115			Initial
*/
using Microsoft.AspNetCore.Mvc;
using WebService.Interfaces;
using ViewModel.Models.ResponseModels;

namespace KielioApp.Controllers;

public class DeutschAdjectiveController : Controller
{
	private readonly IDeutschAdjektivService _deutschAdjektivService;

	public DeutschAdjectiveController(IDeutschAdjektivService deutschAdjektivService)
	{
		_deutschAdjektivService = deutschAdjektivService;
	}

	[Route("/Language/Deutsch/Adjective")]
	public IActionResult Index(string articleType = "")
	{
		var adjectives = _deutschAdjektivService.GetAll();

		// Map to ViewModel
		var adj = adjectives.Select(x => new DeutschAdjectiveResponseModel
		{
			Type = x.Type,
			Gender = x.Gender,
			Case = x.Case,
			ArticleEnding = x.ArticleEnding,
			AdjectiveEnding = x.AdjectiveEnding
		}).ToList();

		if (!string.IsNullOrEmpty(articleType) && articleType != "All")
		{
			adj = adj
				.Where(x => x.Type == articleType)
				.ToList();
		}
		var types = ( _deutschAdjektivService.GetAll())
			.Select(x => x.Type)
			.Distinct()
			.ToList();
		types.Insert(0, "All");
		types.Insert(0, "");
		ViewBag.Types = types;
		ViewBag.SelectedType = articleType;
		return View(adj);
	}
}
