using System.Reflection.Metadata;
using System.Net;
using Internal;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Silownia.Models;
using MvcUzytkownik.Data;
using Microsoft.Extensions.Logging;
using System.Linq;
using Silownia.Models;
using System.Collections.Generic;
using System;
using BCrypt.Net;


namespace Silownia.Controllers;

[Route("/api/[controller]")]
public class LogowanieController : Controller
{
    private readonly ILogger<LogowanieController> _logger;
    private readonly MvcUzytkownikContext _context;

    public LogowanieController(ILogger<LogowanieController> logger, MvcUzytkownikContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Route("/api/[controller]/Login")]
    public IActionResult Login(){
        return View("Login");
    }

    [HttpPost("Login")]
    public IActionResult Login(string email, string password)
    {
        // Perform login logic here
        // You can validate the username and password against your authentication system/database
        // Example login logic:
        Console.WriteLine(_context.Uzytkownik.Count());
        Console.WriteLine("email: " + email);
        var user = _context.Uzytkownik.FirstOrDefault(u => u.Email == email);
        if(user == null)
        {
            ViewBag.ErrorMessage = "Invalid username or password";
            return View("Login");
        }

        Console.WriteLine("password: " + password);
        Console.WriteLine("user.Haslo: " + user.Haslo);
        if(BCrypt.Net.BCrypt.Verify(password, user.Haslo) == false)
        {
            ViewBag.ErrorMessage = "Invalid username or password";
            return View("Login");
        }
         // Successful login

        // Add user to session
        HttpContext.Session.SetString("userId", user.Id.ToString());
        HttpContext.Session.SetString("isAdmin", user.CzyAdministrator.ToString());

        bool loggedIn = true;
        bool isAdmin = user.CzyAdministrator;
        ViewBag.LoggedIn = loggedIn;
        ViewBag.Admin = isAdmin;

        return RedirectToAction("Index", "Home"); // Redirect to the welcome page after successful login
        }


    [HttpGet]
    [Route("/api/[controller]/Register")]
    public IActionResult Register()
    {
        return View("Register");
    }

    [HttpPost("Register")]
    public IActionResult Register(string email, string password, string confirmPassword, string firstName, string lastName)
    {
        // Perform registration logic here
        // You can validate the username and password and save them to your authentication system/database
        if(email.Length < 5 || password.Length < 5)
        {
            ViewBag.ErrorMessage = "email and password must be at least 5 characters long";
            return View("Register");
        }

        if(email.Contains("@") == false)
        {
            ViewBag.ErrorMessage = "email must contain @";
            return View("Register");
        }

        if(password != confirmPassword)
        {
            ViewBag.ErrorMessage = "passwords do not match";
            return View("Register");
        }

        if(firstName.Length < 2 || lastName.Length < 2)
        {
            ViewBag.ErrorMessage = "first name and last name must be at least 2 characters long";
            return View("Register");
        }

        if(_context.Uzytkownik.Any(u => u.Email == email))
        {
            ViewBag.ErrorMessage = "email already exists";
            return View("Register");
        }

        Uzytkownik uzytkownik = new Uzytkownik();
        uzytkownik.Id = _context.Uzytkownik.Count() + 1;
        uzytkownik.Email = email;

        // Hash the password before saving it to the database
        // Example hashing logic:
        uzytkownik.Haslo = BCrypt.Net.BCrypt.HashPassword(password);

        uzytkownik.Imie = firstName;
        uzytkownik.Nazwisko = lastName;

        if(_context.Uzytkownik.Count() == 0)
        {
            uzytkownik.CzyAdministrator = true;
        }
        else
        {
            uzytkownik.CzyAdministrator = false;
        }
        _context.Uzytkownik.Add(uzytkownik);

        _context.SaveChanges();

        // Successful registration pop up
        ViewBag.SuccessMessage = "Registration successful";

        var user = _context.Uzytkownik.FirstOrDefault(u => u.Email == email);

        // add user to session
        HttpContext.Session.SetString("userId", user.Id.ToString());
        HttpContext.Session.SetString("isAdmin", user.CzyAdministrator.ToString());


        // Example registration logic:
        // Save the username and password to the database

        // Redirect to the login page after successful registration
        return RedirectToAction("Index", "Home");
    }

    [HttpPost("Logout")]
    public IActionResult Logout()
    {
        // Perform logout logic here
        // Example logout logic:
        HttpContext.Session.Remove("userId"); // Remove the user from the session

        return RedirectToAction("Index", "Home"); // Redirect to the home page after successful logout
    }
}
