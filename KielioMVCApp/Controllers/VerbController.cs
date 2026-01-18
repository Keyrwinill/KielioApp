/*
	Update date			Description 
	--------------------------------
	20251222			Initial
*/
using Microsoft.AspNetCore.Mvc;
using WebService.Interfaces;

namespace KielioMVCApp.Controllers
{	
	public class VerbController : Controller
	{
		private readonly IVerbService _verbService;

		public VerbController(IVerbService verbService)
		{
			_verbService = verbService;
		}

		[Route("/Language/{language}/Verb")]
		[HttpGet]
		public IActionResult Index(string language, string Infinitive)
		{
			if (!string.IsNullOrEmpty(language) && !string.IsNullOrEmpty(Infinitive))
			{
				var verb = language switch
				{
					"Deutsch" => _verbService.GetVerbView("German", Infinitive),
					"Français" => _verbService.GetVerbView("French", Infinitive),
					_ => null
				};

				ViewBag.InfinitiveVerb = Infinitive;
				return View(verb);
			}
			else
			{
				ViewBag.InfinitiveVerb = null;
				return View(null);
			}
		}
	}
}
