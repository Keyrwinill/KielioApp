/*
	Update date			Description 
	--------------------------------
	20251125			Initial
	20260104			Modify structure
*/
namespace DataAccessLibrary.Entities;

public class Language
{
	public Guid Oid { get; set; } = Guid.NewGuid();
	public string? Name { get; set; }

	// Navigation properties (Forward Link)

	// Navigation properties (Reverse Link)
	public ICollection<Verb>? Verbs { get; set; }
	public ICollection<Tense>? Tenses { get; set; }
	public ICollection<VerbPerson>? VerbPersons { get; set; }
	public ICollection<Mood>? Moods { get; set; }               //+20260104

	// Constructor
}
