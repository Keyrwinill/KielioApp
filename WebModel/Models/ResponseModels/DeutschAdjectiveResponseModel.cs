/*
	Update date			Description 
	--------------------------------
	20251115			Initial
*/
using ViewModel.Enums;

namespace ViewModel.Models.ResponseModels;

public class DeutschAdjResponseViewModel
{
	public string? Type { get; set; }
	public string? Gender { get; set; }
	public string? Case { get; set; }
	public string? ArticleEnding { get; set; }
	public string? AdjectiveEnding { get; set; }
	public List<string?> ArticleTypeList { get; set; } = new List<string?>();
}