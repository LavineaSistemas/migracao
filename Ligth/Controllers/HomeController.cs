using System.Diagnostics;
using Ligth.Dao;
using Microsoft.AspNetCore.Mvc;
using Ligth.Models;
using Microsoft.AspNetCore.Authorization;

namespace Ligth.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
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

    public IActionResult Painel()
    {
        var perfil = User.Claims.First(c => c.Type == "perfil").Value;
        if (perfil == "ADM")
        {
            return View();
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }
}