using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace BLL
{
    public class UsuariosBLL
    {
        public ENTITY.Usuarios BuscarUno(int pId)
        {
            MAPPER.UsuariosMapper map = new MAPPER.UsuariosMapper();
            ENTITY.Usuarios ent = map.Buscar(pId);
            map = null;
            return ent;
        }
        public ENTITY.Usuarios BuscarUnoPorUsuario(string pUsuario)
        {
            MAPPER.UsuariosMapper map = new MAPPER.UsuariosMapper();
            ENTITY.Usuarios ent = map.Buscar(pUsuario);
            map = null;
            return ent;
        }
        public List<ENTITY.Usuarios> BuscarTodos()
        {
            MAPPER.UsuariosMapper map = new MAPPER.UsuariosMapper();
            List<ENTITY.Usuarios> l = map.Buscar();
            map = null;
            return l;
        }

        public int Borrar(int pId)
        {            
            MAPPER.UsuariosMapper map = new MAPPER.UsuariosMapper();
            int resultado = map.Borrar(pId);
            map = null;

            return resultado;
        }

        public int AgregarUno(string pUsuario, string pClave, int pDNI, int pPermiso)
        {
            MAPPER.UsuariosMapper map = new MAPPER.UsuariosMapper();
            int resultado = map.AgregarUno(pUsuario, pClave, pDNI, pPermiso);
            map = null;
            return resultado;
        }
        public int ModificarUno(int pId, string pUsuario, int pPermiso)
        {
            int resultado;
            if (pUsuario == "admin")
            {
                resultado = 0;                
            }
            else
            {
                MAPPER.UsuariosMapper map = new MAPPER.UsuariosMapper();
                resultado = map.ModificarUno(pId, pUsuario, pPermiso);
                map = null;                
            }
            return resultado;
        }
        public int CambiarClave(int pId, string pClave)
        {
            MAPPER.UsuariosMapper map = new MAPPER.UsuariosMapper();
            int resultado = map.CambiarClave(pId, pClave);
            map = null;
            return resultado;
        }

        public int ReestablecerClave(int pId)
        {
            MAPPER.UsuariosMapper map = new MAPPER.UsuariosMapper();
            int resultado = map.ReestablecerClave(pId);
            map = null;
            return resultado;
        }

    }
}
