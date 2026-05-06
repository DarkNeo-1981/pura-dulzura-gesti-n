using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class VendedorBLL:IPersona
    {
        public int CantidadVendedores()
        {
            MAPPER.VendedorMapper map = new MAPPER.VendedorMapper();
            int resultado = 0;
            resultado = map.CantidadVendedores();
            map = null;
            return resultado;
        }
        public ENTITY.Vendedor BuscarUno(int pDNI)
        {
            MAPPER.VendedorMapper map = new MAPPER.VendedorMapper();
            ENTITY.Vendedor ent = map.Buscar(pDNI);
            map = null;
            return ent;
        }
        public List<ENTITY.Vendedor> BuscarTodos(bool pEliminado)
        {
            MAPPER.VendedorMapper map = new MAPPER.VendedorMapper();
            List<ENTITY.Vendedor> l = map.BuscarTodos(pEliminado);
            map = null;
            return l;
        }
        public string[] DevolverDatos(int pDNI)
        {
            ENTITY.Vendedor v = this.BuscarUno(pDNI);
            string[] datos =
                            { v.DNI.ToString(),
                              v.Legajo.ToString(),
                              v.Nombre.ToString(),
                              v.Apellido.ToString(),
                              v.Sexo.ToString(),
                              v.Email.ToString(),
                              v.CUIL.ToString(),
                              v.SueldoBasico.ToString(System.Globalization.CultureInfo.InvariantCulture),
                              v.FechaIngreso.ToString("yyyy-MM-dd"),
                              v.DNI_Supervisor.ToString(),
                              "Vendedor",
                              v.Eliminado.ToString()
                            };
            return datos;
        }
        public int BorrarUno(int pDNI)
        {
            MAPPER.VendedorMapper map = new MAPPER.VendedorMapper();
            int resultado = map.BorrarUno(pDNI);
            map = null;
            return resultado;
        }
        public int DeshacerBorrarUno(int pDNI)
        {
            MAPPER.VendedorMapper map = new MAPPER.VendedorMapper();
            int resultado = map.DeshacerBorrarUno(pDNI);
            map = null;
            return resultado;
        }
        public int AgregarUno(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            MAPPER.VendedorMapper map = new MAPPER.VendedorMapper();
            int resultado = map.AgregarUno(pDNI, pLegajo, pNombre, pApellido, pSexo, pEmail, pCUIL, pSueldoBasico, pFechaIngreso, pDNI_Superior, pEliminado);
            map = null;
            return resultado;
        }

        public int ModificarUno(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            MAPPER.VendedorMapper map = new MAPPER.VendedorMapper();
            int resultado = map.ModificarUno(pDNI, pLegajo, pNombre, pApellido, pSexo, pEmail, pCUIL, pSueldoBasico, pFechaIngreso, pDNI_Superior, pEliminado);
            map = null;
            return resultado;
        }
    }
}
