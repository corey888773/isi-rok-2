//Klasa oraz metoda modyfikująca standardowy routing 
//Wywołanie: http://localhost:5083/api/IO/index?id1=xxx&id2=yyy

using Microsoft.AspNetCore.Mvc;
namespace MvcMovie.Controllers;

[Route("api/[controller]")]
public class IO : Controller
{

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("Login")]
    public IActionResult Login()
    {
        return View("Logowanie");
    }

    [HttpPost("Login")] 
    public IActionResult Login(IFormCollection form)
    {
// - Jedynie zalogowani użytkownicy powinni mieć dostęp do innych zasobów aplikacji poza stroną logowania. "Zasoby aplikacja" to metody kontrolera, widoki Razor i strony Razor. 
// - Jeżeli ktoś nie jest zalogowany a wpisze adres URL jakiegoś zasobu powinien być przekierowywany na stronę logowania.

        string login = form["login"];
        string haslo = form["password"];
        if (login == "admin" && haslo == "admin")
        {   
            // add login to session
            HttpContext.Session.SetString("login", login);
            return View("LogowaniePoprawne");
        }
        else
        {
            // log in debug console login and password
            Console.WriteLine("Login: " + login + " Password: " + haslo);
            return View("LogowanieNiepoprawne");
        }
    }

    [HttpPost("Logout")]
    public IActionResult Logout()
    {
        // remove login from session
        HttpContext.Session.Remove("login");
        return View("Logowanie");
    }



}
