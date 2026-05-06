using ENTITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAPPER
{
    public class UsuariosMapper    
    {
        public Usuarios Buscar(int pId)
        {
            DAL.UsuariosDAL op = new DAL.UsuariosDAL();
            List<Usuarios> l = new List<Usuarios>();
            l = castDstoEnt(op.Buscar_Uno(pId));
            if (l.Count > 0) return l[0];
            else return null;
        }
        public Usuarios Buscar(string pUsuario)
        {
            DAL.UsuariosDAL op = new DAL.UsuariosDAL();
            List<Usuarios> l = new List<Usuarios>();
            l = castDstoEnt(op.Buscar_Uno(pUsuario));
            if (l.Count > 0) return l[0];
            else return null;
        }
        public List<Usuarios> Buscar()
        {
            DAL.UsuariosDAL op = new DAL.UsuariosDAL();
            List<Usuarios> l = new List<Usuarios>();
            l = castDstoEnt(op.Buscar_Todos());
            if (l.Count > 0) return l;
            else return null;
        }
        public int Borrar(int pId)
        {
            DAL.UsuariosDAL op = new DAL.UsuariosDAL();
            int resultado = op.Borrar(pId);
            return resultado;
        }
        public int AgregarUno(string pUsuario, string pClave, int pDNI, int pPermiso)
        {
            DAL.UsuariosDAL op = new DAL.UsuariosDAL();
            int resultado = op.Agregar(pUsuario, pClave, pDNI, pPermiso);
            return resultado;
        }
        public int ModificarUno(int pId, string pUsuario, int pPermiso)
        {
            DAL.UsuariosDAL op = new DAL.UsuariosDAL();
            int resultado = op.Modificar(pId, pUsuario, pPermiso);
            return resultado;
        }
        public int CambiarClave(int pId, string pClave)
        {
            DAL.UsuariosDAL op = new DAL.UsuariosDAL();
            int resultado = op.CambiarClave(pId, pClave);
            return resultado;
        }

        public int ReestablecerClave(int pId)
        {
            DAL.UsuariosDAL op = new DAL.UsuariosDAL();
            int resultado = op.ReestablecerClave(pId);
            return resultado;
        }

        private List<Usuarios> castDstoEnt(DataTable dt)
        {
            List<Usuarios> Lista_Op = new List<Usuarios>();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PermisosMapper mapP = new PermisosMapper();
                    Usuarios op = new Usuarios();
                    op.Id = int.Parse(dr["Id"].ToString());
                    op.Usuario = dr["Usuario"].ToString();
                    op.Clave = dr["Clave"].ToString();
                    op.DNI = int.Parse(dr["DNI"].ToString());
                    op.Permiso = mapP.Buscar(int.Parse(dr["Permiso"].ToString()));
                    Lista_Op.Add(op);
                }
            }
            return Lista_Op;
        }
    }
}
