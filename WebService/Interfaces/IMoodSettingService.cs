/*
	Update date			Description 
	---------------------------------------------------------------
	20260106			Initial
	20260120			POST Methods
*/
using ViewModel.Models.RequestModels;
using ViewModel.Models.ResponseModels;

namespace WebService.Interfaces;

public interface IMoodSettingService
{
	MoodSettingResponseModel GetMood(string languageName, string moodType);
	//+>>20260120
	MoodSettingResponseModel RebuildResponse(MoodSettingRequestModel request);
	Task SaveMood(MoodSettingRequestModel request);
	//+<<20260120
}
