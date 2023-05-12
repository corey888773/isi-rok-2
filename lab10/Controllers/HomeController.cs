using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using lab10.Models;

namespace lab10.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // check if user is logged in
        if (HttpContext.Session.GetString("login") != null)
        {
            return View();
        }
        else
        {
            return RedirectToAction("Login", "IO");
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
