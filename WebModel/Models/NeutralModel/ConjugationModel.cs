/*
	Update date			Description 
	--------------------------------
	20260126			Initial
*/

namespace ViewModel.Models.NeutralModel;

public class ConjugationModel
{
	public string? Language { get; set; }
	public string? Infinitive { get; set; }
	public string? Translation { get; set; }
	public string? PastParticiple { get; set; }
	public bool Reflexive { get; set; }
	public bool Separable { get; set; }
	public List<ConjugationFormModel> Conjugations { get; set; } = new List<ConjugationFormModel>();
}

public class ConjugationFormModel
{
	public string? Mood { get; set; }
	public string? Tense { get; set; }
	public Dictionary<string, string> Forms { get; set; }
}


