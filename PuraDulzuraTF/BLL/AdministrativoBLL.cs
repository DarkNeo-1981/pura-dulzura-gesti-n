using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class AdministrativoBLL:IPersona
    {
        public int CantidadAdministrativos()
        {
            MAPPER.AdministrativoMapper map = new MAPPER.AdministrativoMapper();
            int resultado = 0;
            resultado = map.CantidadAdministrativos();
            map = null;
            return resultado;
        }
        public ENTITY.Administrativo BuscarUno(int pDNI)
        {
            MAPPER.AdministrativoMapper map = new MAPPER.AdministrativoMapper();
            ENTITY.Administrativo ent = map.Buscar(pDNI);
            map = null;
            return ent;
        }
        public List<ENTITY.Administrativo> BuscarTodos(bool pEliminado)
        {
            MAPPER.AdministrativoMapper map = new MAPPER.AdministrativoMapper();
            List<ENTITY.Administrativo> l = map.BuscarTodos(pEliminado);
            map = null;
            return l;
        }
        public string[] DevolverDatos(int pDNI)
        {
            ENTITY.Administrativo a = this.BuscarUno(pDNI);
            string[] datos =
                            { a.DNI.ToString(),
                              a.Legajo.ToString(),
                              a.Nombre.ToString(),
                              a.Apellido.ToString(),
                              a.Sexo.ToString(),
                              a.Email.ToString(),
                              a.CUIL.ToString(),
                              a.SueldoBasico.ToString(System.Globalization.CultureInfo.InvariantCulture),
                              a.FechaIngreso.ToString("yyyy-MM-dd"),
                              a.DNI_Supervisor.ToString(),
                              "Administrativo",
                              a.Eliminado.ToString()
                            };
            return datos;
        }
        public int BorrarUno(int pDNI)
        {
            MAPPER.AdministrativoMapper map = new MAPPER.AdministrativoMapper();
            int resultado = map.BorrarUno(pDNI);
            map = null;
            return resultado;
        }
        public int DeshacerBorrarUno(int pDNI)
        {
            MAPPER.AdministrativoMapper map = new MAPPER.AdministrativoMapper();
            int resultado = map.DeshacerBorrarUno(pDNI);
            map = null;
            return resultado;
        }
        public int AgregarUno(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Supervisor, bool pEliminado)
        {
            MAPPER.AdministrativoMapper map = new MAPPER.AdministrativoMapper();
            int resultado = map.AgregarUno(pDNI, pLegajo, pNombre, pApellido, pSexo, pEmail, pCUIL, pSueldoBasico, pFechaIngreso, pDNI_Supervisor, pEliminado);
            map = null;
            return resultado;
        }

        public int ModificarUno(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Supervisor, bool pEliminado)

        {
            MAPPER.AdministrativoMapper map = new MAPPER.AdministrativoMapper();
            int resultado = map.ModificarUno(pDNI, pLegajo, pNombre, pApellido, pSexo, pEmail, pCUIL, pSueldoBasico, pFechaIngreso, pDNI_Supervisor, pEliminado);
            map = null;
            return resultado;
        }

    }
}
