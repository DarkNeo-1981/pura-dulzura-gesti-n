using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class Clientes
    {
        public int Id {  get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Calle { get; set; } 
        public string Numero { get; set; }
        public string Piso { get; set; }
        public string Depto { get; set; } 
        public string Localidad { get; set; }

        public string NombreCompleto => $"{Apellido}, {Nombre}";
    }
}
