/*
	Update date			Description 
	--------------------------------
	20260106			Initial
*/
using ViewModel.Enums;

namespace ViewModel.Models.ResponseModels;

public class MoodSettingResponseModel
{
	public string? LanguageName { get; set; }
	public string? MoodType { get; set; }

	//Language dropdowns
	public List<string> LanguageList { get; set; } = new List<string>();

	//Mood dropdowns
	public List<string> MoodList { get; set; } = new List<string>();

	//Tense checkbox items
	public List<CheckboxViewModel> TenseList { get; set; } = new List<CheckboxViewModel>();

	//Person checkbox items
	public List<CheckboxViewModel> PersonList { get; set; } = new List<CheckboxViewModel>();
	public string? Status { get; set; }
	public bool CanCreate { get; set; }
	public bool CanUpdate { get; set; }
	public bool CanSave { get; set; }

}

public class CheckboxViewModel
{
	public string? Name { get; set; }
	public bool IsChecked { get; set; }
	public bool IsDisabled { get; set; }
}

