using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class EncargadoBLL
    {
        public int CantidadEncargados()
        {
            MAPPER.EncargadoMapper map = new MAPPER.EncargadoMapper();
            int resultado = 0;
            resultado = map.CantidadEncargados();
            map = null;
            return resultado;
        }
        public ENTITY.Encargado BuscarUno(int pId)
        {
            MAPPER.EncargadoMapper map = new MAPPER.EncargadoMapper();
            ENTITY.Encargado ent = map.Buscar(pId);
            map = null;
            return ent;
        }
        public List<ENTITY.Encargado> BuscarTodos(bool pEliminado)
        {
            MAPPER.EncargadoMapper map = new MAPPER.EncargadoMapper();
            List<ENTITY.Encargado> l = map.BuscarTodos(pEliminado);
            map = null;
            return l;
        }
        public int AgregarUno(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            MAPPER.EncargadoMapper map = new MAPPER.EncargadoMapper();
            int resultado = map.AgregarUno(pDNI, pLegajo, pNombre, pApellido, pSexo, pEmail, pCUIL, pSueldoBasico, pFechaIngreso, pDNI_Superior, pEliminado);
            map = null;
            return resultado;
        }
        public int ModificarUno(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            MAPPER.EncargadoMapper map = new MAPPER.EncargadoMapper();
            int resultado = map.ModificarUno(pDNI, pLegajo, pNombre, pApellido, pSexo, pEmail, pCUIL, pSueldoBasico, pFechaIngreso, pDNI_Superior, pEliminado);
            map = null;
            return resultado;
        }
        public int BorrarUno(int pId)
        {
            MAPPER.EncargadoMapper map = new MAPPER.EncargadoMapper();
            int resultado = map.BorrarUno(pId);
            map = null;
            return resultado;
        }
        public int DeshacerBorrarUno(int pId)
        {
            MAPPER.EncargadoMapper map = new MAPPER.EncargadoMapper();
            int resultado = map.DeshacerBorrarUno(pId);
            map = null;
            return resultado;
        }
    }
}
