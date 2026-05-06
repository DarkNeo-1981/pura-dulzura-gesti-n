using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class ConfiguracionMailDTO
    {
        // Propiedades del Servidor SMTP (Nombres Limpios)
        public string Host { get; set; }
        public int Puerto { get; set; }
        public bool UsaSSL { get; set; }

        // Credenciales y Remitente (Nombres Limpios)
        public string EmailRemitente { get; set; }
        public string Password { get; set; }
    }
}
