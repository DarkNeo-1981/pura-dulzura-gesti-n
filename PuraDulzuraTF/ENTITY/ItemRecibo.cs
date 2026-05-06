using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class ItemRecibo
    {
        public string Concepto { get; set; } // Ej: "Sueldo Básico", "Jubilación", "Horas Extra"
        public string Tipo { get; set; }     // Debe ser "Haber" o "Descuento"
        public decimal Monto { get; set; }   // El valor monetario
    }
}