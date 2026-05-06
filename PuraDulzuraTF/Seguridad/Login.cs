using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using ENTITY;

namespace Seguridad
{
    public class Login
    {
        public Usuarios ValidarUsuario(string pUsuario, string pClave)
        {
            string claveEncriptada = SERVICIOS.Encriptacion.Encriptar(pClave);
            BLL.UsuariosBLL Usuario = new BLL.UsuariosBLL();
            Usuarios Usuario_Entidad = Usuario.BuscarUnoPorUsuario(pUsuario);
            if (Usuario_Entidad != null && claveEncriptada == Usuario_Entidad.Clave)
            {
                return Usuario_Entidad;
            }
            return null;
        }
    }
}
