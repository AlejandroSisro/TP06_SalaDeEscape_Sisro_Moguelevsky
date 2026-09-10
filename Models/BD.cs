using Microsoft.Data.SqlClient;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TP06_SalaDeEscape_Sisro_Moguelevsky.Models
{
    public class BD
    {
        private readonly string _connectionString;
        private static readonly List<Dioses> DiosesFallback = new List<Dioses>
        {
            new Dioses { IdDios = 1, Nombre = "Zeus", FotoDios = "Zeus.png", Dialogo = "¡Contemplá el poder del firmamento, pariente! Los demás te ofrecerán trucos sutiles, pero cuando el Tiempo te pisa los talones, nada supera a la fuerza de la tormenta. Elegí mi rayo: fulminaremos los obstáculos y ralentizaremos los mecanismos de tus enemigos. Dejá que mis hermanos duden, nosotros golpeamos primero." },
            new Dioses { IdDios = 2, Nombre = "Poseidon", FotoDios = "Poseidon.png", Dialogo = "¡Ja! No te dejes engañar por promesas elegantes, pequeña. Solo la fuerza de una marea implacable puede arrastrar la resistencia del enemigo. Con mi bendición, aturdiremos el compás del peligro y empujaremos las respuestas hacia vos como un naufragio en la costa. ¡Elegime y que rujan las olas!" },
            new Dioses { IdDios = 3, Nombre = "Apolo", FotoDios = "Apolo.png", Dialogo = "¡Saludos, estrella de la noche! El camino hacia la cumbre es oscuro y lleno de desvíos engañosos, pero mi luz puede disipar cualquier sombra. Mi bendición iluminará el camino correcto y te dará más tiempo para memorizar los patrones del enemigo. Dejá que la claridad guíe tus pasos." },
            new Dioses { IdDios = 4, Nombre = "Hera", FotoDios = "Hera.png", Dialogo = "El linaje y el orden deben prevalecer ante el caos de Cronos. Las deidades menores te ofrecerán libertades efímeras, pero mi lazo soberano te otorga verdadero control. Con mi bendición, ataremos los elementos del acertijo para que un acierto debilite el resto de las trabas. Someté el nivel a tu voluntad." },
            new Dioses { IdDios = 5, Nombre = "Demeter", FotoDios = "Demeter.png", Dialogo = "El invierno no conoce la piedad, y tus enemigos tampoco deberían conocerla. Mientras los jóvenes del Olimpo derrochan palabras, mi escarcha congelará sus pretensiones. Si elegís mi favor, congelaremos los temporizadores del nivel, dándote la fría calma que necesitás para pensar sin presiones. Soportá la tormenta." },
            new Dioses { IdDios = 6, Nombre = "Hefesto", FotoDios = "Hefesto.png", Dialogo = "Los discursos bonitos no rompen cadenas, muchacha; el metal al rojo vivo sí. Mientras los demás te dan bendiciones intangibles, yo te ofrezco ingeniería pura y pesada. Mi favor destruirá una de las sub-fases más molestas de un solo golpe de mi martillo. Dejá la magia a un lado y elegí la fuerza del yunque." },
            new Dioses { IdDios = 7, Nombre = "Hestia", FotoDios = "Hestia.png", Dialogo = "En medio de la guerra y el caos, la llama del hogar es lo único que permanece puro. El fuego de los demás consume, pero el mío purifica y desgasta la resistencia de las trampas. Con mi bendición, consumiremos los errores del tablero, permitiéndote fallar sin sufrir el castigo completo del enemigo. Mantené la llama encendida." },
            new Dioses { IdDios = 8, Nombre = "Ares", FotoDios = "Ares.png", Dialogo = "¡La diplomacia ha terminado! Esta sala es un campo de batalla y la única salida es a través de la ruina de sus defensas. Olvidate de la paciencia o la lógica; mi bendición te otorgará una furia bélica que forzará la apertura de los candados reduciendo los requisitos del puzzle. Elegí la guerra y abrite paso." },
            new Dioses { IdDios = 9, Nombre = "Hermes", FotoDios = "Hermes.png", Dialogo = "¡Hola, hola! No hay tiempo que perder, ¡el reloj corre rapidísimo! Las demás deidades se toman demasiadas pausas para actuar, pero mi poder es inmediato. Si me elegís, te daré la agilidad mental necesaria para adelantarte a las trampas y reintentar tus movimientos antes de que el servidor registre un fallo. ¡Apuremos el paso!" },
            new Dioses { IdDios = 10, Nombre = "Selene", FotoDios = "Selene.png", Dialogo = "La Luna observa todo desde lo alto, criatura de la noche, y conoce los secretos que los dioses del día ignoran. Mi luz argéntea no te dará fuerza, sino metamorfosis. Al invocarme, activaremos una habilidad oculta que alterará temporalmente las reglas de la sala a tu favor. Confiá en la plata de la noche." },
            new Dioses { IdDios = 11, Nombre = "Artemisa", FotoDios = "Artemisa.png", Dialogo = "No necesitás discursos largos ni templos ostentosos. Lo que necesitás es precisión implacable. Mientras los demás discuten en sus tronos, mi flecha va directo al punto crítico. Elegí mi favor y perforaremos las sub-fases más molestas, dándote un escape rápido. Movete rápido, elígeme." },
            new Dioses { IdDios = 12, Nombre = "Atenea", FotoDios = "Atenea.png", Dialogo = "La fuerza sin estrategia no es más que un despliegue vacío. Mis parientes te ofrecen caos, pero yo te ofrezco la verdad oculta tras el velo. Si aceptás mi escudo, descartaremos el error y traeremos claridad a tu mente para mirar a través de las trampas. Elegí la razón; la victoria se planifica." }
        };

        public BD()
        {
            _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Hades2SalaEscape;Trusted_Connection=True;TrustServerCertificate=True;";
        }

        public List<Dioses> ObtenerTodosLosDioses()
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                string query = "SELECT IdDios, Nombre, FotoDios, Dialogo FROM Dioses";
                List<Dioses> resultado = connection.Query<Dioses>(query).ToList();

                if (resultado == null || resultado.Count == 0)
                {
                    return new List<Dioses>(DiosesFallback);
                }

                return resultado;
            }
            catch
            {
                return new List<Dioses>(DiosesFallback);
            }
        }

        public Dioses ObtenerDiosPorId(int idDios)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                string query = "SELECT IdDios, Nombre, FotoDios, Dialogo FROM Dioses WHERE IdDios = @pId";
                Dioses dios = connection.QueryFirstOrDefault<Dioses>(query, new { pId = idDios });
                return dios ?? DiosesFallback.FirstOrDefault(d => d.IdDios == idDios);
            }
            catch
            {
                return DiosesFallback.FirstOrDefault(d => d.IdDios == idDios);
            }
        }

        public List<Dioses> ObtenerDiosesAleatorios(int cantidad)
        {
            List<Dioses> resultado = new List<Dioses>();
            List<Dioses> todos = ObtenerTodosLosDioses();
            if (todos == null || todos.Count == 0)
            {
                return resultado;
            }

            Random rnd = new Random();
            HashSet<int> indices = new HashSet<int>();
            int max = todos.Count;
            while (indices.Count < cantidad && indices.Count < max)
            {
                int i = rnd.Next(0, max);
                if (!indices.Contains(i))
                {
                    indices.Add(i);
                    resultado.Add(todos[i]);
                }
            }

            return resultado;
        }

        public List<Usuario> ObtenerTodosLosUsuarios()
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                string query = "SELECT Id, nombreUsuario, Sala FROM Usuario";
                return connection.Query<Usuario>(query).ToList();
            }
            catch
            {
                return new List<Usuario>();
            }
        }

        public Usuario ObtenerUsuarioPorNombre(string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                return null;
            }

            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                string query = @"SELECT Id, nombreUsuario, Sala FROM Usuario WHERE nombreUsuario = @pNombreUsuario";
                return connection.QueryFirstOrDefault<Usuario>(query, new { pNombreUsuario = nombreUsuario });
            }
            catch
            {
                return null;
            }
        }

        public bool ValidarCredenciales(string nombreUsuario, string contraseña)
        {
            return true;
        }

        public void RegistrarUsuario(Usuario usuario)
        {
            if (usuario == null || string.IsNullOrWhiteSpace(usuario.nombreUsuario))
            {
                return;
            }

            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                string query = @"INSERT INTO Usuario (nombreUsuario, Sala) VALUES (@pNombreUsuario, @pSala)";
                connection.Execute(query, new
                {
                    pNombreUsuario = usuario.nombreUsuario,
                    pSala = usuario.Sala
                });
            }
            catch
            {
                // Ignorar fallo de inserción si la base no está disponible.
            }
        }

        public void ActualizarUsuario(Usuario usuario)
        {
            if (usuario == null || string.IsNullOrWhiteSpace(usuario.nombreUsuario))
            {
                return;
            }

            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                string query = @"UPDATE Usuario SET Sala = @pSala WHERE nombreUsuario = @pNombreUsuario";
                connection.Execute(query, new
                {
                    pNombreUsuario = usuario.nombreUsuario,
                    pSala = usuario.Sala
                });
            }
            catch
            {
                // Ignorar fallo de actualización si la base no está disponible.
            }
        }

        public void ActualizarSalaUsuario(string nombreUsuario, int sala)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                return;
            }

            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                string query = @"UPDATE Usuario SET Sala = @pSala WHERE nombreUsuario = @pNombreUsuario";
                connection.Execute(query, new { pNombreUsuario = nombreUsuario, pSala = sala });
            }
            catch
            {
                // Ignorar fallo de actualización si la base no está disponible.
            }
        }

        public void EliminarUsuario(string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                return;
            }

            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                string query = @"DELETE FROM Usuario WHERE nombreUsuario = @pNombreUsuario";
                connection.Execute(query, new { pNombreUsuario = nombreUsuario });
            }
            catch
            {
                // Ignorar fallo de borrado si la base no está disponible.
            }
        }

        private bool TieneConexionDisponible()
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
