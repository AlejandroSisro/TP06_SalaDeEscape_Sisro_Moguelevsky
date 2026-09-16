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

    private void GuardarSalaActualEnSesionYBD(string usuario, int sala)
    {
        int salaValida = sala;
        if (salaValida < 1)
        {
            salaValida = 1;
        }
        else if (salaValida > 5)
        {
            salaValida = 1;
        }

        HttpContext.Session.SetString("SalaActual", salaValida.ToString());

        if (string.IsNullOrWhiteSpace(usuario))
        {
            return;
        }

        try
        {
            BD bd = new BD();
            Usuario usuarioActual = bd.ObtenerUsuarioPorNombre(usuario.Trim());
            if (usuarioActual != null)
            {
                usuarioActual.Sala = salaValida;
                bd.ActualizarUsuario(usuarioActual);
            }
        }
        catch
        {
            // Ignorar error si no se pudo persistir la sala.
        }
    }

    private IActionResult RedirigirUsuarioSegunSala(Usuario usuarioActual)
    {
        if (usuarioActual == null)
        {
            return RedirectToAction(nameof(Index));
        }

        int sala = usuarioActual.Sala;
        if (sala < 1 || sala > 5)
        {
            sala = 1;
            usuarioActual.Sala = 1;
            new BD().ActualizarUsuario(usuarioActual);
        }

        HttpContext.Session.SetString("Usuario", usuarioActual.nombreUsuario);
        HttpContext.Session.SetString("SalaActual", sala.ToString());

        return RedirectToAction(ObtenerAccionPorSala(sala));
    }

    private Usuario ObtenerUsuarioPersistido(string nombreUsuario)
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario))
        {
            return null;
        }

        BD bd = new BD();
        Usuario usuarioActual = bd.ObtenerUsuarioPorNombre(nombreUsuario.Trim());
        if (usuarioActual == null)
        {
            return null;
        }

        if (usuarioActual.Sala < 1 || usuarioActual.Sala > 5)
        {
            usuarioActual.Sala = 1;
            bd.ActualizarUsuario(usuarioActual);
        }

        return usuarioActual;
    }

    private string ObtenerAccionPorSala(int sala)
    {
        return sala switch
        {
            1 => nameof(Sala1),
            2 => nameof(Medea),
            3 => nameof(Sala3),
            4 => nameof(Sala4),
            5 => nameof(Sala5),
            _ => nameof(Sala1)
        };
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Continuar()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Continuar(string usuario)
    {
        string nombreNormalizado = usuario?.Trim();
        if (string.IsNullOrWhiteSpace(nombreNormalizado))
        {
            ViewBag.Error = "Ingresá tu nombre de usuario para continuar.";
            return View();
        }

        Usuario usuarioActual = ObtenerUsuarioPersistido(nombreNormalizado);
        if (usuarioActual == null)
        {
            ViewBag.Error = "Ese usuario no existe. Probá con otro nombre de usuario.";
            return View();
        }

        return RedirigirUsuarioSegunSala(usuarioActual);
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
        if (!string.IsNullOrWhiteSpace(usuario))
        {
            Usuario usuarioActual = ObtenerUsuarioPersistido(usuario);
            if (usuarioActual != null)
            {
                return RedirigirUsuarioSegunSala(usuarioActual);
            }

            HttpContext.Session.Remove("Usuario");
            HttpContext.Session.Remove("SalaActual");
        }

        return View("Login");
    }

    [HttpPost]
    public IActionResult Login(string usuario, string contrasena)
    {
        string nombreNormalizado = usuario?.Trim();
        if (string.IsNullOrWhiteSpace(nombreNormalizado))
        {
            ViewBag.Error = "Debe ingresar un nombre de usuario.";
            return View("Login");
        }

        try
        {
            Usuario usuarioActual = AsegurarUsuarioPersistido(nombreNormalizado, 1, resetSiExiste: true);
            if (usuarioActual == null)
            {
                // No validamos existencia acá. El login solo guarda el nombre y empieza una nueva partida.
                usuarioActual = new Usuario { nombreUsuario = nombreNormalizado, Sala = 1 };
            }

            HttpContext.Session.SetString("Usuario", usuarioActual.nombreUsuario);
            HttpContext.Session.SetString("SalaActual", usuarioActual.Sala.ToString());
            return RedirectToAction(nameof(Sala1));
        }
        catch
        {
            ViewBag.Error = "No se pudo conectar con la base de datos. Intentá nuevamente más tarde.";
            return View("Login");
        }
    }

    [HttpGet]
    public IActionResult Medea()
    {
        string usuario = HttpContext.Session.GetString("Usuario");
        GuardarSalaActualEnSesionYBD(usuario, 2);

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
            string usuario = HttpContext.Session.GetString("Usuario");
            GuardarSalaActualEnSesionYBD(usuario, 3);
            return RedirectToAction("Sala3");
        }

        ViewBag.Mensaje = "El orden es incorrecto. Intenta de nuevo.";
        ViewBag.Correcto = false;

        return View("Sala2");
    }

    [HttpGet]
    public IActionResult Sala2()
    {
        return RedirectToAction(nameof(Medea));
    }

    [HttpGet]
    public IActionResult Sala3()
    {
        string usuario = HttpContext.Session.GetString("Usuario");
        GuardarSalaActualEnSesionYBD(usuario, 3);
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
            string usuario = HttpContext.Session.GetString("Usuario");
            GuardarSalaActualEnSesionYBD(usuario, 4);
            return RedirectToAction("Sala4");
        }

        ViewBag.Mensaje = "Las ovejas que seleccionaste no son las correctas. Recuerda: azul, violeta y verde.";
        ViewBag.Correcto = false;

        return View();
    }

    [HttpGet]
    public IActionResult Sala4()
    {
        string usuario = HttpContext.Session.GetString("Usuario");
        GuardarSalaActualEnSesionYBD(usuario, 4);
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
            string usuario = HttpContext.Session.GetString("Usuario");
            GuardarSalaActualEnSesionYBD(usuario, 5);
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
            GuardarSalaActualEnSesionYBD(usuario, 2);
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
        string usuario = HttpContext.Session.GetString("Usuario");
        GuardarSalaActualEnSesionYBD(usuario, 5);
        ViewBag.Mensaje = "";
        ViewBag.Correcto = false;
        return View();
    }

    [HttpGet]
    public IActionResult Sala1()
    {
        string usuario = HttpContext.Session.GetString("Usuario");
        GuardarSalaActualEnSesionYBD(usuario, 1);

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
            string usuario = HttpContext.Session.GetString("Usuario");
            GuardarSalaActualEnSesionYBD(usuario, 1);
            ViewBag.Mensaje = "EL TIEMPO NO PUEDE SER DETENIDO";
            ViewBag.Correcto = true;
            ViewBag.ShowRestartOverlay = true;
            return View();
        }

        ViewBag.Mensaje = "La respuesta es incorrecta. Intentá otra vez.";
        ViewBag.Correcto = false;
        ViewBag.ShowRestartOverlay = false;
        return View();
    }

    private Usuario AsegurarUsuarioPersistido(string nombreUsuario, int salaInicial = 1, bool resetSiExiste = false)
    {
        string nombreNormalizado = nombreUsuario?.Trim();
        if (string.IsNullOrWhiteSpace(nombreNormalizado))
        {
            return null;
        }

        BD bd = new BD();
        Usuario usuarioActual = bd.ObtenerUsuarioPorNombre(nombreNormalizado);

        if (usuarioActual == null)
        {
            usuarioActual = new Usuario
            {
                nombreUsuario = nombreNormalizado,
                Sala = salaInicial
            };
            bd.RegistrarUsuario(usuarioActual);
            return bd.ObtenerUsuarioPorNombre(nombreNormalizado) ?? usuarioActual;
        }

        usuarioActual.Sala = salaInicial;
        bd.ActualizarUsuario(usuarioActual);
        return usuarioActual;
    }
}
