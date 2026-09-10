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
            new Dioses { IdDios = 1, Nombre = "Zeus", FotoDios = "Zeus.png", Dialogo = "El firmamento responde a mi voluntad." },
            new Dioses { IdDios = 2, Nombre = "Poseidon", FotoDios = "Poseidon.png", Dialogo = "Las olas rompen cualquier obstáculo." },
            new Dioses { IdDios = 3, Nombre = "Apolo", FotoDios = "Apolo.png", Dialogo = "La luz guía a quien sabe escuchar." },
            new Dioses { IdDios = 4, Nombre = "Hera", FotoDios = "Hera.png", Dialogo = "El orden prevalece sobre el caos." },
            new Dioses { IdDios = 5, Nombre = "Demeter", FotoDios = "Demeter.png", Dialogo = "La tierra guarda secretos bajo su hielo." },
            new Dioses { IdDios = 6, Nombre = "Hefesto", FotoDios = "Hefesto.png", Dialogo = "El metal se dobla ante la fuerza." },
            new Dioses { IdDios = 7, Nombre = "Hestia", FotoDios = "Hestia.png", Dialogo = "La llama del hogar nunca se apaga." },
            new Dioses { IdDios = 8, Nombre = "Ares", FotoDios = "Ares.png", Dialogo = "La guerra abre caminos donde la calma no puede." },
            new Dioses { IdDios = 9, Nombre = "Hermes", FotoDios = "Hermes.png", Dialogo = "La velocidad es el camino más corto." },
            new Dioses { IdDios = 10, Nombre = "Selene", FotoDios = "Selene.png", Dialogo = "La luna revela lo que el día oculta." },
            new Dioses { IdDios = 11, Nombre = "Artemisa", FotoDios = "Artemisa.png", Dialogo = "La flecha más precisa siempre triunfa." },
            new Dioses { IdDios = 12, Nombre = "Atenea", FotoDios = "Atenea.png", Dialogo = "La sabiduría es la verdadera arma." }
        };

        public BD()
        {
            _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Hades2SalaEscape;Trusted_Connection=True;TrustServerCertificate=True;";
        }

        public List<Dioses> ObtenerTodosLosDioses()
        {
            try
            {
                if (!TieneConexionDisponible())
                {
                    return new List<Dioses>(DiosesFallback);
                }

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
                if (!TieneConexionDisponible())
                {
                    return DiosesFallback.FirstOrDefault(d => d.IdDios == idDios);
                }

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
            if (!TieneConexionDisponible())
            {
                return new List<Usuario>();
            }

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
            if (string.IsNullOrWhiteSpace(nombreUsuario) || !TieneConexionDisponible())
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
            if (usuario == null || string.IsNullOrWhiteSpace(usuario.nombreUsuario) || !TieneConexionDisponible())
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
            if (usuario == null || string.IsNullOrWhiteSpace(usuario.nombreUsuario) || !TieneConexionDisponible())
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
            if (string.IsNullOrWhiteSpace(nombreUsuario) || !TieneConexionDisponible())
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
            if (string.IsNullOrWhiteSpace(nombreUsuario) || !TieneConexionDisponible())
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
