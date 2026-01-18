/*
	Update date			Description 
	--------------------------------
	20251115			Initial
*/
using System.Diagnostics;
using ViewModel.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;

namespace KielioApp.Controllers;

public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;
	public HomeController(ILogger<HomeController> logger)
	{
		_logger = logger;
	}

	public IActionResult Index()
	{
		var person = new Person
		{
			FirstName = "Erwin",
			LastName = "Wang",
			BirthDay = new DateTime(1992, 5, 9),
			Nationality = "Taiwan"
		};

		var model = new HomeResponseModel
		{
			Person = person,
			ExpandFlag = false,
			Notifications = new List<string>
		{
			$"Welcome {person.FirstName} {person.LastName} to My App",
			"Have a nice day!"
		}
		};
		return View(model);
	}

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorResponseModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
