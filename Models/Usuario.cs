using System;

namespace TP06_SalaDeEscape_Sisro_Moguelevsky.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string nombreUsuario { get; set; }
        public string contraseña { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public int IdBendicion { get; set; }
        public int IdMaldicion { get; set; }
        public int Sala { get; set; }
    }
}
