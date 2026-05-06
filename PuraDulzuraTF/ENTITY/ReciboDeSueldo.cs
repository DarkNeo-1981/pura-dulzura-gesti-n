using System;
using System.Collections.Generic;

namespace ENTITY
{
    public class ReciboDeSueldo
    {
        // Propiedades de identificación
        public int Id { get; set; }
        public int DNI_Empleado { get; set; }
        public EmpleadoBase Empleado { get; set; }
        public DateTime Periodo { get; set; }
        public DateTime FechaEmision { get; set; }

        // Propiedades de Totales (Necesarias para la DAL y el BLL)
        public decimal TotalHaberes { get; set; }
        public decimal TotalDescuentos { get; set; }
        public decimal Bruto { get; set; }
        public decimal NetoAPagar { get; set; }

        // Detalle para el DataGridView y la persistencia
        public List<ItemRecibo> Detalle { get; set; } = new List<ItemRecibo>();
    }
}