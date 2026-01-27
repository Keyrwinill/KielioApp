/*
	Update date			Description 
	--------------------------------
	20260115			Initial
*/
using ViewModel.Models.Shared;

namespace ViewModel.Models.RequestModels;

public class MoodSettingRequestModel
{
	public string? LanguageName { get; set; }
	public string? MoodType { get; set; }
	public List<CheckboxViewModel> TenseList { get; set; } = new List<CheckboxViewModel>();
	public List<CheckboxViewModel> PersonList { get; set; } = new List<CheckboxViewModel> ();
}
