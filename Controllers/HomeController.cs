using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TP06_SalaDeEscape_Sisro_Moguelevsky.Models;

namespace TP06_SalaDeEscape_Sisro_Moguelevsky.Controllers;

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

    [HttpGet]
    public IActionResult Medea()
    {
        ViewBag.Mensaje = "";
        ViewBag.Correcto = false;
        return View();
    }

    [HttpPost]
    public IActionResult Medea(string ingrediente1, string ingrediente2, string ingrediente3)
    {
        if (ingrediente1 == null || ingrediente2 == null || ingrediente3 == null ||
            ingrediente1 == "" || ingrediente2 == "" || ingrediente3 == "")
        {
            ViewBag.Mensaje = "Debes elegir los tres ingredientes antes de activar el caldero.";
            ViewBag.Correcto = false;
            return View();
        }

        if (ingrediente1 == "Mandrágora" && ingrediente2 == "Sombra" && ingrediente3 == "Polvo de Hueso")
        {
            ViewBag.Mensaje = "¡Correcto! La niebla se disipa y el camino queda abierto.";
            ViewBag.Correcto = true;
            return View();
        }

        ViewBag.Mensaje = "La mezcla está mal ordenada. El caldero burbujea con furia.";
        ViewBag.Correcto = false;
        return View();
    }

    public IActionResult Continuar()
    {
        return View();
    }

    public IActionResult Historia()
    {
        return View();
    }

    public IActionResult Integrantes()
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
}
