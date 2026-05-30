/*
	Update date			Description 
	--------------------------------
	20260317			Initial
*/
using ViewModel.Models.NeutralModels;
using ViewModel.Models.ResponseModels;

namespace WebService.Interfaces;

public interface IConjugationService
{
	ConjugationModel? GetConjugation(string?languageNameEng, string? verb, string? mood, string? tensestring);
	bool Exists(string languageName, string verb);
	List<string> GetMoodList(string languageName);
	List<string> GetTenseList(string mood);
	Dictionary<string, string> GetPersonList(string mood);
	Dictionary<string, string> GetFormByTense(string? verb, string? mood, string? tense);
	Task SaveVerb(ConjugationModel request, string languageName);
}
