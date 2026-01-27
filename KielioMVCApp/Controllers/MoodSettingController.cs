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
		public IActionResult Index(string? languageName, string? moodType, bool edit = false)
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

			return RedirectToAction(nameof(Index), new 
				{ 
				  languageName = request.LanguageName, 
				  moodType = request.MoodType,
				});
			//+<<20260121
		}
	}
}
