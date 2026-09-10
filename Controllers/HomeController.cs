using System.Diagnostics;
using TP06_SalaDeEscape_Sisro_Moguelevsky.Models;
using Microsoft.AspNetCore.Mvc;

namespace TP06_SalaDeEscape_Sisro_Moguelevsky.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration _configuration;

    public HomeController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult Index()
    {
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

    [HttpGet]
    public IActionResult Login()
    {
        string usuario = HttpContext.Session.GetString("Usuario");
        if (usuario != null && usuario != "")
        {
            return RedirectToAction("Sala1");
        }

        return View("Login");
    }

    [HttpPost]
    public IActionResult Login(string usuario, string contrasena)
    {
        if (string.IsNullOrWhiteSpace(usuario))
        {
            ViewBag.Error = "Debe ingresar un nombre de usuario.";
            return View("Login");
        }

        try
        {
            BD bd = new BD();

            Usuario existente = bd.ObtenerUsuarioPorNombre(usuario);
            if (existente == null)
            {
                Usuario nuevo = new Usuario
                {
                    nombreUsuario = usuario,
                    Sala = 1
                };

                bd.RegistrarUsuario(nuevo);
            }

            HttpContext.Session.SetString("Usuario", usuario);

            Usuario u = bd.ObtenerUsuarioPorNombre(usuario);
            int salaActual = 1;
            if (u != null)
            {
                salaActual = u.Sala;
            }

            HttpContext.Session.SetString("SalaActual", salaActual.ToString());
            return RedirectToAction(nameof(Sala1));
        }
        catch
        {
            ViewBag.Error = "No se pudo conectar con la base de datos. Verificá que SQL Server esté activo y que la base exista.";
            return View("Login");
        }
    }

    [HttpGet]
    public IActionResult Medea()
    {
        ViewBag.Mensaje = "";
        ViewBag.Correcto = false;
        return View("Sala2");
    }

    [HttpPost]
    public IActionResult Medea(string ingrediente1, string ingrediente2, string ingrediente3)
    {
        bool correcto = ingrediente1 == "Bronce" && ingrediente2 == "Adamanto" && ingrediente3 == "Colmillos";

        if (correcto)
        {
            BD bd = new BD();
            string usuario = HttpContext.Session.GetString("Usuario");
            if (!string.IsNullOrWhiteSpace(usuario))
            {
                Usuario user = bd.ObtenerUsuarioPorNombre(usuario);
                if (user != null)
                {
                    user.Sala = 3;
                    bd.ActualizarUsuario(user);
                    HttpContext.Session.SetString("SalaActual", "3");
                }
            }

            return RedirectToAction("Sala3");
        }

        ViewBag.Mensaje = "El orden es incorrecto. Intenta de nuevo.";
        ViewBag.Correcto = false;

        return View("Sala2");
    }

    [HttpGet]
    public IActionResult Sala3()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Sala3(int id, string[] ovejas)
    {
        bool correcto = true;
        if (ovejas == null)
        {
            correcto = false;
        }
        else if (ovejas.Length != 3)
        {
            correcto = false;
        }
        else
        {
            bool tieneAzul = false;
            bool tieneVerde = false;
            bool tieneVioleta = false;
            for (int i = 0; i < ovejas.Length; i++)
            {
                string v = ovejas[i];
                if (v == "azul")
                {
                    tieneAzul = true;
                }
                else if (v == "verde")
                {
                    tieneVerde = true;
                }
                else if (v == "violeta")
                {
                    tieneVioleta = true;
                }
            }

            if (tieneAzul == false || tieneVerde == false || tieneVioleta == false)
            {
                correcto = false;
            }
        }

        if (correcto)
        {
            BD bd = new BD();
            string usuario = HttpContext.Session.GetString("Usuario");
            if (!string.IsNullOrWhiteSpace(usuario))
            {
                Usuario user = bd.ObtenerUsuarioPorNombre(usuario);
                if (user != null)
                {
                    user.Sala = 4;
                    bd.ActualizarUsuario(user);
                    HttpContext.Session.SetString("SalaActual", "4");
                }
            }

            return RedirectToAction("Sala4");
        }

        ViewBag.Mensaje = "Las ovejas que seleccionaste no son las correctas. Recuerda: azul, violeta y verde.";
        ViewBag.Correcto = false;

        return View();
    }

    [HttpGet]
    public IActionResult Sala4()
    {
        ViewBag.Mensaje = "";
        ViewBag.Correcto = false;
        return View();
    }

    [HttpPost]
    public IActionResult Sala4(string sentido1, string sentido2, string sentido3, string sentido4)
    {
        bool correcto = sentido1 == "Estrella del Norte" &&
                        sentido2 == "Llama del Este" &&
                        sentido3 == "Profundidad del Sur" &&
                        sentido4 == "Sombra del Oeste";

        if (correcto)
        {
            BD bd = new BD();
            string usuario = HttpContext.Session.GetString("Usuario");
            if (!string.IsNullOrWhiteSpace(usuario))
            {
                Usuario user = bd.ObtenerUsuarioPorNombre(usuario);
                if (user != null)
                {
                    user.Sala = 5; // avanzar a la sala final
                    bd.ActualizarUsuario(user);
                    HttpContext.Session.SetString("SalaActual", "5");
                }
            }

            return RedirectToAction("Sala5");
        }

        ViewBag.Mensaje = "El timón responde con un giro monstruoso: la brújula está desordenada y el remolino se acerca. Revisa la secuencia del marinero.";
        ViewBag.Correcto = false;
        return View();
    }

    [HttpPost]
    public IActionResult Sala1(int elegidoId, int dios1Id, int dios2Id)
    {
        BD bd = new BD();
        Dioses elegido = bd.ObtenerDiosPorId(elegidoId);
        int otroId = dios1Id;
        if (dios1Id == elegidoId)
        {
            otroId = dios2Id;
        }

        Dioses otro = bd.ObtenerDiosPorId(otroId);

        ViewBag.Elegido = elegido;
        ViewBag.Otro = otro;
        ViewBag.ShowRiddle = true;
        ViewBag.Dios1 = elegido;
        ViewBag.Dios2 = otro;

        return View();
    }

    [HttpPost]
    public IActionResult ResponderEsfinge(int elegidoId, int dios1Id, int dios2Id, string respuesta)
    {
        BD bd = new BD();
        Dioses elegido = bd.ObtenerDiosPorId(elegidoId);
        int otroId = dios1Id;
        if (dios1Id == elegidoId)
        {
            otroId = dios2Id;
        }

        Dioses otro = bd.ObtenerDiosPorId(otroId);

        bool correcto = false;
        if (respuesta != null && respuesta != "")
        {
            string r = respuesta.ToLower();
            if (r.Contains("hombre") || r.Contains("el hombre"))
            {
                correcto = true;
            }
        }

        if (correcto)
        {
            string usuario = HttpContext.Session.GetString("Usuario");
            if (usuario != null && usuario != "")
            {
                Usuario user = bd.ObtenerUsuarioPorNombre(usuario);
                if (user != null)
                {
                    user.Sala = 2;
                    bd.ActualizarUsuario(user);
                    HttpContext.Session.SetString("SalaActual", "2");
                }
            }

            return RedirectToAction("Medea");
        }

        ViewBag.Elegido = elegido;
        ViewBag.Otro = otro;
        ViewBag.ShowRiddle = false;
        ViewBag.AnswerCorrect = correcto;
        ViewBag.Dios1 = elegido;
        ViewBag.Dios2 = otro;
        return View("Sala1");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        string requestId = HttpContext.TraceIdentifier;
        if (Activity.Current != null && Activity.Current.Id != null && Activity.Current.Id != "")
        {
            requestId = Activity.Current.Id;
        }

        return View(new ErrorViewModel { RequestId = requestId });
    }

    [HttpGet]
    public IActionResult Sala5()
    {
        ViewBag.Mensaje = "";
        ViewBag.Correcto = false;
        return View();
    }

    [HttpGet]
    public IActionResult Sala1()
    {
        BD bd = new BD();
        List<Dioses> dioses = bd.ObtenerDiosesAleatorios(2);
        if (dioses == null || dioses.Count < 2)
        {
            ViewBag.Mensaje = "No se pudieron cargar los dioses. Intentá recargar la página.";
            ViewBag.Dios1 = null;
            ViewBag.Dios2 = null;
            return View();
        }

        ViewBag.Dios1 = dioses[0];
        ViewBag.Dios2 = dioses[1];
        ViewBag.ShowRiddle = false;
        ViewBag.AnswerCorrect = null;
        return View();
    }

    [HttpPost]
    public IActionResult Sala5(string respuesta)
    {
        bool correctoRiddle = false;
        if (!string.IsNullOrWhiteSpace(respuesta))
        {
            string r = respuesta.ToLower().Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u");
            if (r.Contains("dia") && r.Contains("noche"))
            {
                correctoRiddle = true;
            }
        }

        if (correctoRiddle)
        {
            BD bd = new BD();
            string usuario = HttpContext.Session.GetString("Usuario");
            if (!string.IsNullOrWhiteSpace(usuario))
            {
                Usuario user = bd.ObtenerUsuarioPorNombre(usuario);
                if (user != null)
                {
                    user.Sala = 1; // reinicia para jugar de nuevo
                    bd.ActualizarUsuario(user);
                    HttpContext.Session.SetString("SalaActual", "1");
                }
            }

            return RedirectToAction("Sala1");
        }

        ViewBag.Mensaje = "La respuesta es incorrecta. Intentá otra vez.";
        ViewBag.Correcto = false;
        return View();
    }
}
