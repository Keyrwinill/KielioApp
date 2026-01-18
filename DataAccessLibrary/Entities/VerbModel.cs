/*
	Update date			Description 
	--------------------------------
	20251125			Initial
	20260104			Modify structure
	20260108			Modify structure
	20260109			New fields for seach order
*/
namespace DataAccessLibrary.Entities;

public class Verb
{
	public Guid Oid { get; set; } = Guid.NewGuid();
	public string? Infinitive { get; set; }
	public bool Reflexive { get; set; }
	public string? Translation { get; set; }

	//+>>20260104
	public bool Regular { get; set; }
	public bool Separable { get; set; }
	public string? PastParticiple { get; set; }
	//<<+20260104

	// Navigation properties (Forward Link)
	public Guid LinkLanguage { get; set; }
	public Language? Language { get; set; }

	// Navigation properties (Reverse Link)
	public ICollection<Conjugation>? Conjugations { get; set; }
	// Constructor
}

public class Tense
{
	public Guid Oid { get; set; } = new Guid();
	public string? Name { get; set; }
	public string? Description { get; set; }
	public int SortOrder { get; set; }									//+20260109

	// Navigation properties (Forward Link)
	public Guid LinkLanguage { get; set; }
	public Language? Language { get; set; }

	// Navigation properties (Reverse Link)
	public ICollection<MidMoodTense>? MidMoodTenses { get; set; }   //+20260104

	// Constructor
}

public class VerbPerson
{
	public Guid Oid { get; set; } = new Guid();
	public string? Pronoun { get; set; }
	public string? Type { get; set; }
	public int SortOrder { get; set; }									//+20260109

	// Navigation properties (Forward Link)
	public Guid LinkLanguage { get; set; }
	public Language? Language { get; set; }

	// Navigation properties (Reverse Link)
	public ICollection<MidMoodPerson>? MidMoodPersons { get; set; }     //+20260104

	// Constructor
}

public class Conjugation
{
	public Guid Oid { get; set; } = new Guid();
	public string? Form { get; set; }

	// Navigation properties (Forward Link)
	public Guid LinkVerb { get; set; }
	public Verb? Verb { get; set; }
	public Guid LinkMidMoodTense { get; set; }
	public MidMoodTense? MidMoodTense { get; set; }
	public Guid LinkMidMoodPerson { get; set; }	
	public MidMoodPerson? MidMoodPerson { get; set; }

	// Navigation properties (Reverse Link)

	// Constructor
}

//+>>20260104
public class Mood
{
	public Guid Oid { get; set; } = new Guid();
	public string? Type { get; set; }
	public int SortOrder { get; set; }                                  //+20260109

	// Navigation properties (Forward Link)
	public Guid LinkLanguage { get; set; }
	public Language? Language { get; set; }

	// Navigation properties (Reverse Link)
	public ICollection<MidMoodTense>? MidMoodTenses { get; set; }
	public ICollection<MidMoodPerson>? MidMoodPersons { get; set; }

	// Constructor
}

public class MidMoodTense
{
	public Guid Oid { get; set; } = new Guid();
	//+>>20260108
	public string? MoodType { get; set; }
	public string? TenseName { get; set; }
	//+<<20260108

	// Navigation properties (Forward Link)
	public Guid LinkTense { get; set; }
	public Tense? Tense { get; set; }
	public Guid LinkMood { get; set; }
	public Mood? Mood { get; set; }

	// Navigation properties (Reverse Link)
	public ICollection<Conjugation>? Conjugations { get; set; }

	// Constructor
}

public class MidMoodPerson
{
	public Guid Oid { get; set; } = new Guid();
	//+>>20260108
	public string? MoodType { get; set; }
	public string? PersonType { get; set; }
	//+<<20260108

	// Navigation properties (Forward Link)
	public Guid LinkVerbPerson{ get; set; }
	public VerbPerson? VerbPerson { get; set; }
	public Guid LinkMood { get; set; }
	public Mood? Mood { get; set; }

	// Navigation properties (Reverse Link)
	public ICollection<Conjugation>? Conjugations { get; set; }

	// Constructor
}
//+<<20260104