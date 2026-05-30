/*
	Update date			Description 
	--------------------------------
	20260126			Initial
*/

namespace ViewModel.Models.NeutralModels;

public class ConjugationModel
{
	public string? Language { get; set; }
	public List<string> MoodList { get; set; } = new List<string>();
	public List<string> TenseList { get; set; } = new List<string>();
	public string? Infinitive { get; set; }
	public string? Translation { get; set; }
	public string? PastParticiple { get; set; }
	public bool Reflexive { get; set; }
	public bool Separable { get; set; }
	public string? Mood { get; set; }
	public string? Tense { get; set; }
	public Dictionary<string, string> Forms { get; set; } = new Dictionary<string, string>();
}