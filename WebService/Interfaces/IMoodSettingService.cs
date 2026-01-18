/*
	Update date			Description 
	---------------------------------------------------------------
	20260106			Initial
	20260108			Create methods for Mood, Tense, VerbPerson
*/
using ViewModel.Models.RequestModels;
using ViewModel.Models.ResponseModels;

namespace WebService.Interfaces;

public interface IMoodSettingService
{
	MoodSettingResponseModel GetMoodSettingView(MoodSettingRequestModel request);
	void SetLink(string operation, string languageName, string moodType, List<int> tenseSelectionList, List<int> personSelectionList);	//+20260108
}
