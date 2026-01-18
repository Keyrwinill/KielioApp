/*
	Update date			Description 
	----------------------------------------------------------------
	20260106			Initial creation for person type dictionary
*/
namespace DataAccessLibrary.Dictionaries;

public class PersonTypeDictionary
{
	public static readonly Dictionary<string, string> PersonType = new()
	{
		["1s"] = "Singular first person",
		["2s"] = "Singular second person",
		["3s"] = "Singular third person",
		["1p"] = "Plural first person",
		["2p"] = "Plural second person",
		["3p"] = "Plural third person",
		["2f"] = "Formal second person"
	};
}

	
