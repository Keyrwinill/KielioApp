/*
	Update date			Description 
	--------------------------------
	20260112			Initial
	20260121			Update GET and POST methods
*/
using Microsoft.AspNetCore.Mvc;
using ViewModel.Models.RequestModels;
using WebService.Interfaces;

namespace KielioMVCApp.Controllers
{
	public class MoodSettingController : Controller
	{
		private readonly IMoodSettingService _moodSettingService;

		public MoodSettingController (IMoodSettingService moodSettingService)
		{
			_moodSettingService = moodSettingService;
		}

		[HttpGet]
		[Route("/Language/MoodSetting")]
		public IActionResult Index(string? languageName, string? moodType)
		{
			//+>>20260121
			var response = _moodSettingService.GetMood(languageName, moodType);
			return View(response);
			//+<<20260121
		}
		
		[HttpPost]
		[Route("/Language/MoodSetting")]
		public async Task<IActionResult> Index(MoodSettingRequestModel request)
		{
			//+>>20260121
			await _moodSettingService.SaveMood(request);

			// keep selection
			/*
			return RedirectToAction(nameof(Index), new 
				{ 
				  languageName = request.LanguageName, 
				  moodType = request.MoodType,
				});
			*/
			return RedirectToAction(nameof(Index));     // Redirect to GET method without parameters to reset the form
			//+<<20260121
		}
	}
}
