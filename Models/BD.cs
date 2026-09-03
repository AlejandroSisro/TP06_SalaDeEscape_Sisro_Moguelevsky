using System.Diagnostics;
using Dapper;
using Escape.Models;
using Microsoft.AspNetCore.Mvc;

namespace Escape.Controllers;

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
            return RedirectToAction("Sala", new { id = 1 });
        }

        return View("Login");
    }

    [HttpPost]
    public IActionResult Login(string usuario, string contraseña, string sala)
    {
        if (usuario != null && usuario != "" && contraseña != null && contraseña != "")
        {
            BD bd = new BD();

            Usuario existente = bd.ObtenerUsuarioPorNombre(usuario);

            int salaNumero = 1;
            if (sala != null && sala != "")
            {
                int.TryParse(sala, out salaNumero);
                if (salaNumero <= 0)
                {
                    salaNumero = 1;
                }
            }

            if (existente == null)
            {
                Usuario nuevo = new Usuario();
                nuevo.nombreUsuario = usuario;
                nuevo.contraseña = contraseña;
                nuevo.nombre = "";
                nuevo.apellido = "";
                nuevo.IdBendicion = 0;
                nuevo.IdMaldicion = 0;
                nuevo.Sala = salaNumero;

                bd.RegistrarUsuario(nuevo);
            }
            else
            {
                bool valido = bd.ValidarCredenciales(usuario, contraseña);
                if (valido == false)
                {
                    ViewBag.Error = "Usuario o contraseña inválidos";
                    return View("Login");
                }

                existente.Sala = salaNumero;
                bd.ActualizarUsuario(existente);
            }

            HttpContext.Session.SetString("Usuario", usuario);
            HttpContext.Session.SetString("SalaActual", salaNumero.ToString());

            return RedirectToAction("Sala", new { id = 1 });
        }

        ViewBag.Error = "Usuario o contraseña inválidos";
        return View("Login");
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
            ViewBag.Mensaje = "Has purificado la niebla. El camino está libre.";
            ViewBag.Correcto = true;
        }
        else
        {
            ViewBag.Mensaje = "El orden es incorrecto. Intenta de nuevo.";
            ViewBag.Correcto = false;
        }

        return View("Sala2");
    }

    [HttpGet]
    public IActionResult Sala(int id)
    {
        int partidaId = HttpContext.Session.GetString("PartidaId");

        if (partidaId == null || partidaId == "")
        {
            return RedirectToAction("Index");
        }

        int idPartida = 0;
        int.TryParse(partidaId, out idPartida);

        using int connection = GetConnection();

        int partida = connection.QuerySingleOrDefault<dynamic>(
            @"
            SELECT p.Id, p.NombreParticipante, pr.SalaActual
            FROM Partidas p
            LEFT JOIN Progresos pr ON pr.PartidaId = p.Id
            WHERE p.Id = @Id
            ",
            new { Id = idPartida }
        );

        if (partida == null)
        {
            return RedirectToAction("Error");
        }

        if (partida.SalaActual != null && (int)partida.SalaActual != id)
        {
            return RedirectToAction("Error");
        }

        HttpContext.Session.SetString("SalaActual", id.ToString());
        ViewBag.NombreParticipante = partida.NombreParticipante;
        ViewBag.SalaActual = id;

        return View();
    }

    [HttpPost]
    public IActionResult ResponderSala(int id, string respuesta)
    {
        var partidaId = HttpContext.Session.GetString("PartidaId");

        if (partidaId == null || partidaId == "")
        {
            return RedirectToAction("Index");
        }

        int idPartida = 0;
        int.TryParse(partidaId, out idPartida);

        using var connection = GetConnection();

        var partida = connection.QuerySingleOrDefault<dynamic>(
            @"
            SELECT p.Id, pr.SalaActual
            FROM Partidas p
            LEFT JOIN Progresos pr ON pr.PartidaId = p.Id
            WHERE p.Id = @Id
            ",
            new { Id = idPartida }
        );

        if (partida == null)
        {
            return RedirectToAction("Error");
        }

        if (partida.SalaActual != null && (int)partida.SalaActual != id)
        {
            return RedirectToAction("Error");
        }

        connection.Execute(
            @"
            UPDATE Progresos
            SET SalaActual = @SalaActual,
                UltimaRespuesta = @Respuesta,
                FechaActualizacion = GETDATE()
            WHERE PartidaId = @PartidaId;

            IF @@ROWCOUNT = 0
            BEGIN
                INSERT INTO Progresos (PartidaId, SalaActual, UltimaRespuesta, FechaActualizacion)
                VALUES (@PartidaId, @SalaActual, @Respuesta, GETDATE());
            END;
            ",
            new
            {
                PartidaId = idPartida,
                SalaActual = id,
                Respuesta = respuesta
            }
        );

        HttpContext.Session.SetString("SalaActual", id.ToString());

        return RedirectToAction("Sala", new { id });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private SqlConnection GetConnection()
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        return new SqlConnection(connectionString);
    }
}
