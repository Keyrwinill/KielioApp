/*
	Update date			Description 
	--------------------------------
	20251125			Initial
*/
namespace DataAccessLibrary.Entities;

public class DeutschAdjektiv
{
	public Guid Oid { get; set; } = new Guid();
	public string? Type { get; set; }
	public string? Gender { get; set; }
	public string? Case { get; set; }
	public string? ArticleEnding { get; set; }
	public string? AdjectiveEnding { get; set; }

	// Navigation properties (Forward Link)

	// Navigation properties (Reverse Link)

	// Constructor
}
