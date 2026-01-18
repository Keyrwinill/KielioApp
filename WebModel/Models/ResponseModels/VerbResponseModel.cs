/*
	Update date			Description 
	--------------------------------
	20251221			Initial
*/
namespace ViewModel.Models.ResponseModels;

public class PersonFormViewModel
{
	public string? Pronoun { get; set; }
	public string? Form { get; set; }

}

public class ConjugationViewModel
{
	public string? Tense { get; set; }
	public List<PersonFormViewModel> PersonForms { get; set; } = new List<PersonFormViewModel>();
}

public class VerbResponseModel
{
	public string? LanguageName { get; set; }
	public string? Infinitive { get; set; }
	public bool Reflexive { get; set; }
	public string? Translation { get; set; }
	public List<ConjugationViewModel> Conjugations { get; set; } = new List<ConjugationViewModel>();
}

