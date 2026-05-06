using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAPPER
{
    public class EncargadoMapper
    {
        public int CantidadEncargados()
        {
            DAL.EncargadoDAL enc = new DAL.EncargadoDAL();
            int resultado = 0;
            resultado = enc.CantidadEncargados();
            return resultado;
        }
        public ENTITY.Encargado Buscar(int pId)
        {
            DAL.EncargadoDAL enc = new DAL.EncargadoDAL();
            List<ENTITY.Encargado> l = new List<ENTITY.Encargado>();
            l = castDstoEnt(enc.Buscar_Uno(pId));
            if (l.Count > 0) return l[0];
            else return null;
        }
        public List<ENTITY.Encargado> BuscarTodos(bool pEliminado)
        {
            DAL.EncargadoDAL enc = new DAL.EncargadoDAL();
            List<ENTITY.Encargado> l = new List<ENTITY.Encargado>();
            l = castDstoEnt(enc.Buscar_Todos(pEliminado));
            if (l.Count > 0) return l;
            else return null;
        }
        public int BorrarUno(int pId)
        {
            DAL.EncargadoDAL enc = new DAL.EncargadoDAL();
            int resultado = enc.Borrar(pId);
            return resultado;
        }
        public int DeshacerBorrarUno(int pId)
        {
            DAL.EncargadoDAL enc = new DAL.EncargadoDAL();
            int resultado = enc.Deshacer_Borrar(pId);
            return resultado;
        }
        public int AgregarUno(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            DAL.EncargadoDAL enc = new DAL.EncargadoDAL();
            int resultado = enc.Agregar(pDNI, pLegajo, pNombre, pApellido, pSexo, pEmail, pCUIL, pSueldoBasico, pFechaIngreso, pDNI_Superior, pEliminado);
            return resultado;
        }
        public int ModificarUno(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            DAL.EncargadoDAL enc = new DAL.EncargadoDAL();            
            int resultado = enc.Modificar(pDNI, pLegajo, pNombre, pApellido, pSexo, pEmail, pCUIL, pSueldoBasico, pFechaIngreso, pDNI_Superior, pEliminado);
            return resultado;
        }
        private List<ENTITY.Encargado> castDstoEnt(DataTable dt)
        {
            List<ENTITY.Encargado> l = new List<ENTITY.Encargado>();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ENTITY.Encargado ent = new ENTITY.Encargado();
                    ent.Id = int.Parse(dr["Id"].ToString());
                    ent.Nombre = dr["Nombre"].ToString();
                    ent.Apellido = dr["Apellido"].ToString();
                    ent.Eliminado = bool.Parse(dr["Eliminado"].ToString());
                    l.Add(ent);
                }
            }
            return l;
        }
    }
}
