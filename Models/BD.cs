using Microsoft.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace Escape.Models
{
    public class BD
    {
        private string _connectionString = @"Server=.;Database=EscapeDB;User Id=alumno;Password=123456;TrustServerCertificate=True;";

        public List<Usuario> ObtenerTodosLosUsuarios()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, nombreUsuario, contraseña, nombre, apellido, ISNULL(IdBendicion,0) AS IdBendicion, ISNULL(IdMaldicion,0) AS IdMaldicion, ISNULL(Sala,1) AS Sala FROM Usuario";
                return connection.Query<Usuario>(query).ToList();
            }
        }

        public Usuario ObtenerUsuarioPorNombre(string nombreUsuario)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT Id, nombreUsuario, contraseña, nombre, apellido, ISNULL(IdBendicion,0) AS IdBendicion, ISNULL(IdMaldicion,0) AS IdMaldicion, ISNULL(Sala,1) AS Sala FROM Usuario 
                                 WHERE nombreUsuario = @pNombreUsuario";
                return connection.QueryFirstOrDefault<Usuario>(query, new { pNombreUsuario = nombreUsuario });
            }
        }

        public bool ValidarCredenciales(string nombreUsuario, string contraseña)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT Id, nombreUsuario, contraseña, nombre, apellido, ISNULL(IdBendicion,0) AS IdBendicion, ISNULL(IdMaldicion,0) AS IdMaldicion, ISNULL(Sala,1) AS Sala FROM Usuario 
                                 WHERE nombreUsuario = @pNombreUsuario 
                                 AND contraseña = @pContraseña";
                Usuario usuario = connection.QueryFirstOrDefault<Usuario>(query, new { pNombreUsuario = nombreUsuario, pContraseña = contraseña });
                return usuario != null;
            }
        }

        public void RegistrarUsuario(Usuario usuario)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Usuario 
                                 (nombreUsuario, contraseña, nombre, apellido, IdBendicion, IdMaldicion, Sala) 
                                 VALUES 
                                 (@pNombreUsuario, @pContraseña, @pNombre, @pApellido, @pIdBendicion, @pIdMaldicion, @pSala)";

                connection.Execute(query, new
                {
                    pNombreUsuario = usuario.nombreUsuario,
                    pContraseña = usuario.contraseña,
                    pNombre = usuario.nombre,
                    pApellido = usuario.apellido,
                    pIdBendicion = usuario.IdBendicion == 0 ? (object)DBNull.Value : usuario.IdBendicion,
                    pIdMaldicion = usuario.IdMaldicion == 0 ? (object)DBNull.Value : usuario.IdMaldicion,
                    pSala = usuario.Sala
                });
            }
        }

        public void ActualizarUsuario(Usuario usuario)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Usuario 
                                 SET contraseña = @pContraseña,
                                     nombre = @pNombre,
                                     apellido = @pApellido,
                                     IdBendicion = @pIdBendicion,
                                     IdMaldicion = @pIdMaldicion,
                                     Sala = @pSala
                                 WHERE nombreUsuario = @pNombreUsuario";

                connection.Execute(query, new
                {
                    pNombreUsuario = usuario.nombreUsuario,
                    pContraseña = usuario.contraseña,
                    pNombre = usuario.nombre,
                    pApellido = usuario.apellido,
                    pIdBendicion = usuario.IdBendicion == 0 ? (object)DBNull.Value : usuario.IdBendicion,
                    pIdMaldicion = usuario.IdMaldicion == 0 ? (object)DBNull.Value : usuario.IdMaldicion,
                    pSala = usuario.Sala
                });
            }
        }

        public void EliminarUsuario(string nombreUsuario)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM Usuario 
                                 WHERE nombreUsuario = @pNombreUsuario";
                connection.Execute(query, new { pNombreUsuario = nombreUsuario });
            }
        }
    }
}
