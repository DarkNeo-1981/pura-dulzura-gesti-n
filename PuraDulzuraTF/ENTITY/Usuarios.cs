using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class Usuarios
    {
        public int Id {get;set;}
        public string Usuario {get;set;}
        public string Clave {get;set;}
        public int DNI {get;set;}
        public Permiso Permiso {get;set;}

        public string NombreCompleto => Usuario; //esto es para los cmb de abrirpedidos
    }
}
