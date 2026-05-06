using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS
{
    public class DatosFijos
    {
        public static List<string> ObtenerCondicionesIVA()
        {
            return new List<string>
            {
                "IVA Responsable Inscripto",
                "IVA Exento",
                "Monotributo",
                "Consumidor Final"
            };
        }

        public static List<KeyValuePair<bool, string>> ObtenerEstadosLogicos()
        {
            return new List<KeyValuePair<bool, string>>
            {
                new KeyValuePair<bool, string>(true, "Activo"),
                new KeyValuePair<bool, string>(false, "Inactivo")
            };
        }

        public static List<string> ObtenerProductos()
        {
            return new List<string>
            {
                "Harina 000",
                "Harina 0000",
                "Harina Leudante",
                "Azucar",
                "Dulce de Leche",
                "Dulce de Leche repostero",
                "Chocolate covertura",
                "Cholate Blanco",
                "Levadura",
                "Leche",
                "Dulce de Batata",
                "Dulce de Membrillo",
                "Chips de Chocolate",
            };
        }
    }
}

