/*
	Update date			Description 
	--------------------------------
	20260112			Initial
*/
using Microsoft.AspNetCore.Mvc;
using ViewModel.Models;
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
		public IActionResult Index(string operation, string languageName, string moodType)
		{
			var request = new MoodSettingRequestModel()
			{
				LanguageName = string.IsNullOrEmpty(languageName) ? null : languageName,
				MoodType = string.IsNullOrEmpty(moodType) ? null : moodType,
				Operation = string.IsNullOrEmpty(operation) ? "Initial" : operation
			};

			var response = _moodSettingService.GetMoodSettingView(request);
			return View(response);
		}
		/*
		[HttpPost]
		[Route("/Language/MoodSetting")]
		public IActionResult Index(string operation = "Initial", string languageName = "", string moodType = "")
		{
			var request = new MoodSettingRequestModel()
			{
				LanguageName = languageName,
				MoodType = moodType,
				Operation = operation
			};

			var response = _moodSettingService.GetMoodSettingView(request);
			return View(response);
		}
		*/
	}
}
