using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class JefeBLL:IPersona
    {
        public int CantidadJefes()
        {
            MAPPER.JefeMapper map = new MAPPER.JefeMapper();
            int resultado = 0;
            resultado = map.CantidadJefes();
            map = null;
            return resultado;
        }
        public ENTITY.Jefe BuscarUno(int pDNI)
        {
            MAPPER.JefeMapper map = new MAPPER.JefeMapper();
            ENTITY.Jefe ent = map.Buscar(pDNI);
            map = null;
            return ent;
        }
        public List<ENTITY.Jefe> BuscarTodos(bool pEliminado)
        {
            MAPPER.JefeMapper map = new MAPPER.JefeMapper();
            List<ENTITY.Jefe> l = map.BuscarTodos(pEliminado);
            map = null;
            return l;
        }
        public string[] DevolverDatos(int pDNI)
        {
            ENTITY.Jefe j = this.BuscarUno(pDNI);
            string[] datos =
                            { j.DNI.ToString(),
                              j.Legajo.ToString(),
                              j.Nombre.ToString(),
                              j.Apellido.ToString(),
                              j.Sexo.ToString(),
                              j.Email.ToString(),
                              j.CUIL.ToString(),
                              j.SueldoBasico.ToString(System.Globalization.CultureInfo.InvariantCulture),
                              j.FechaIngreso.ToString("yyyy-MM-dd"),
                              j.DNI_Supervisor.ToString(),
                              "Jefe",
                              j.Eliminado.ToString()
                            };
            return datos;
        }
        public int BorrarUno(int pDNI)
        {
            MAPPER.JefeMapper map = new MAPPER.JefeMapper();
            int resultado = map.BorrarUno(pDNI);
            map = null;
            return resultado;
        }
        public int DeshacerBorrarUno(int pDNI)
        {
            MAPPER.JefeMapper map = new MAPPER.JefeMapper();
            int resultado = map.DeshacerBorrarUno(pDNI);
            map = null;
            return resultado;
        }
        public int AgregarUno(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            MAPPER.JefeMapper map = new MAPPER.JefeMapper();
            int resultado = map.AgregarUno(pDNI, pLegajo, pNombre, pApellido, pSexo, pEmail, pCUIL, pSueldoBasico, pFechaIngreso, pDNI_Superior, pEliminado);
            map = null;
            return resultado;
        }

        public int ModificarUno(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            MAPPER.JefeMapper map = new MAPPER.JefeMapper();
            int resultado = map.ModificarUno(pDNI, pLegajo, pNombre, pApellido, pSexo, pEmail, pCUIL, pSueldoBasico, pFechaIngreso, pDNI_Superior, pEliminado);
            map = null;
            return resultado;
        }
    }
}
