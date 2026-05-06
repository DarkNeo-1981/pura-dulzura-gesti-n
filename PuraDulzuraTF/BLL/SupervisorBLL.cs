using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class SupervisorBLL:IPersona
    {
        public int CantidadSupervisores()
        {
            MAPPER.SupervisorMapper map = new MAPPER.SupervisorMapper();
            int resultado = 0;
            resultado = map.CantidadSupervisores();
            map = null;
            return resultado;
        }
        public ENTITY.Supervisor BuscarUno(int pDNI)
        {
            MAPPER.SupervisorMapper map = new MAPPER.SupervisorMapper();
            ENTITY.Supervisor ent = map.Buscar(pDNI);
            map = null;
            return ent;
        }
        public List<ENTITY.Supervisor> BuscarTodos(bool pEliminado)
        {
            MAPPER.SupervisorMapper map = new MAPPER.SupervisorMapper();
            List<ENTITY.Supervisor> l = map.BuscarTodos(pEliminado);
            map = null;
            return l;
        }
        public string[] DevolverDatos(int pDNI)
        {
            ENTITY.Supervisor s = this.BuscarUno(pDNI);
            string[] datos =
                            { s.DNI.ToString(),
                              s.Legajo.ToString(),
                              s.Nombre.ToString(),
                              s.Apellido.ToString(),
                              s.Sexo.ToString(),
                              s.Email.ToString(),
                              s.CUIL.ToString(),
                              s.SueldoBasico.ToString(System.Globalization.CultureInfo.InvariantCulture),
                              s.FechaIngreso.ToString("yyyy-MM-dd"),
                              s.DNI_Supervisor.ToString(),
                              "Supervisor",
                              s.Eliminado.ToString()
                            };
            return datos;
        }
        public int BorrarUno(int pDNI)
        {
            MAPPER.SupervisorMapper map = new MAPPER.SupervisorMapper();
            int resultado = map.BorrarUno(pDNI);
            map = null;
            return resultado;
        }
        public int DeshacerBorrarUno(int pDNI)
        {
            MAPPER.SupervisorMapper map = new MAPPER.SupervisorMapper();
            int resultado = map.DeshacerBorrarUno(pDNI);
            map = null;
            return resultado;
        }
        public int AgregarUno(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            MAPPER.SupervisorMapper map = new MAPPER.SupervisorMapper();
            int resultado = map.AgregarUno(pDNI, pLegajo, pNombre, pApellido, pSexo, pEmail, pCUIL, pSueldoBasico, pFechaIngreso, pDNI_Superior, pEliminado);
            map = null;
            return resultado;
        }

        public int ModificarUno(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            MAPPER.SupervisorMapper map = new MAPPER.SupervisorMapper();
            int resultado = map.ModificarUno(pDNI, pLegajo, pNombre, pApellido, pSexo, pEmail, pCUIL, pSueldoBasico, pFechaIngreso, pDNI_Superior, pEliminado);
            map = null;
            return resultado;
        }
    }
}
