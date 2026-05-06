using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class TiposNovedad
{
    // Haberes (Suman al Sueldo Bruto)
    public const string HoraExtra = "HORAEXTRA";
    public const string Premio = "PREMIO";
    public const string OtrosHaberes = "OTROSHABERES";

    // Descuentos (Restan del Sueldo Bruto / Neto)
    public const string Adelanto = "ADELANTO";
    public const string Ausencia = "AUSENCIA";
    public const string OtrosDescuentos = "OTROSDESCUENTOS";

    // Lista para llenar el ComboBox
    public static List<string> ObtenerListaParaUI()
    {
        return new List<string>
        {
            "Horas Extra",        // Mapea a HORAEXTRA
            "Premio",             // Mapea a PREMIO
            "Adelanto de Sueldo", // Mapea a ADELANTO (Descuento)
            "Ausencia / Días",    // Mapea a AUSENCIA (Descuento)
            "Otros Haberes",      // Mapea a OTROSHABERES
            "Otros Descuentos"    // Mapea a OTROSDESCUENTOS (Descuento)
        };
    }
}