/*
	Update date			Description 
	--------------------------------
	20251115			Initial
*/
namespace ViewModel.Models.ResponseModels;

public class Person
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public DateTime BirthDay { get; set; }
	public string? Nationality { get; set; }
}

public class HomeResponseModel
{
	public Person? Person { get; set; }
	public bool ExpandFlag { get; set; } = false;
	public List<string>? Notifications { get; set; }
}

