using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class EmpleadoBase
    {
        public int DNI { get; set; }
        public string CUIL { get; set; }
        public int Legajo { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NombreCompleto
        {
            get { return $"{Nombre} {Apellido}"; }
        }
        public string Sexo { get; set; }    
        public string Email { get; set; }
        public decimal SueldoBasico { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string Categoria { get; set; }
        public bool Eliminado { get; set; }
    }
}
