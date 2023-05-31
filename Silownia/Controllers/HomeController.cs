using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Silownia.Models;

namespace Silownia.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var session = HttpContext.Session;
        if (session.GetString("userId") != null)
        {   
            Console.WriteLine("Logged in");
            ViewBag.LoggedIn = true;
        }
        else
        {
            ViewBag.LoggedIn = false;
        }


        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
