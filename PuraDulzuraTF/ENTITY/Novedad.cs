using System;

namespace ENTITY
{
    // AÑADIR ESTA INTERFAZ: Obliga a definir un criterio de ordenamiento.
    public class Novedad : IComparable<Novedad>
    {
        public int Id { get; set; }
        public int DNI_Empleado { get; set; }
        public DateTime Periodo { get; set; }
        public string TipoNovedad { get; set; }
        public decimal Valor { get; set; }
        public bool EsDescuento { get; set; }
        public string Observacion { get; set; }

        // AÑADIR ESTE MÉTODO: El criterio de ordenamiento (ej. por DNI y Período)
        public int CompareTo(Novedad other)
        {
            if (other == null) return 1;

            // Ordenar por DNI_Empleado primero
            int dniComparison = this.DNI_Empleado.CompareTo(other.DNI_Empleado);
            if (dniComparison != 0)
            {
                return dniComparison;
            }

            // Si los DNI son iguales, ordenar por Período
            return this.Periodo.CompareTo(other.Periodo);
        }
    }
}
