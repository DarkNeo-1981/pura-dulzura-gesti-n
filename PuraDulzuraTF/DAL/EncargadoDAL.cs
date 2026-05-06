using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using ENTITY; 

namespace DAL
{
    // Se hereda de la clase base genérica EmpleadoBaseDAL<T>
    public class EncargadoDAL : EmpleadoBaseDAL<Encargado>
    {
        // Usamos las propiedades abstractas de la clase base
        protected override string NombreElementoRaiz => "Encargados";
        protected override string NombreElementoSingular => "Encargado";

        public EncargadoDAL() : base("Encargados.xml")
        {
            // El constructor base maneja la inicialización del archivo
        }

        // IMPLEMENTACIÓN REQUERIDA: Mapeo de XML a Entidad
        protected override Encargado MapearXElementAEmpleado(XElement xElement)
        {
            Encargado encargado = new Encargado();

            // Mapeo de propiedades base
            encargado.DNI = (int)xElement.Attribute("DNI");
            encargado.Nombre = (string)xElement.Element("Nombre");
            encargado.Apellido = (string)xElement.Element("Apellido");
            encargado.Email = (string)xElement.Element("Email");

            // Conversiones seguras y obligatorias
            encargado.SueldoBasico = decimal.Parse((string)xElement.Element("SueldoBasico"), System.Globalization.CultureInfo.InvariantCulture);
            encargado.FechaIngreso = DateTime.Parse((string)xElement.Element("FechaIngreso"));
            encargado.Eliminado = bool.Parse((string)xElement.Element("Eliminado"));

            // Campos específicos de Encargado
            if (xElement.Element("Legajo") != null)
            {
                encargado.Legajo = int.Parse((string)xElement.Element("Legajo"));
            }
            if (xElement.Element("CUIL") != null)
            {
                encargado.CUIL = (string)xElement.Element("CUIL");
            }
            if (xElement.Element("Sexo") != null)
            {
                encargado.Sexo = (string)xElement.Element("Sexo");
            }

            // Mapeo DNI_Supervisor (Coherente con la entidad)
            if (xElement.Element("DNI_Supervisor") != null)
            {
                encargado.DNI_Supervisor = int.Parse((string)xElement.Element("DNI_Supervisor"));
            }

            return encargado;
        }

        // ===============
        // MÉTODOS ABM 
        // ===============

        // Firma ESTANDARIZADA con 11 argumentos
        public int Agregar(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            int resultado = 0;
            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                // La verificación de existencia debe ser con el tipo int.
                var DNI_Ya_Utilizado = xmlDoc.Descendants(NombreElementoSingular).Any(x => (int)x.Attribute("DNI") == pDNI);

                if (!DNI_Ya_Utilizado)
                {
                    xmlDoc.Element(NombreElementoRaiz).Add(new XElement(NombreElementoSingular,
                                new XAttribute("DNI", pDNI),
                                new XElement("Legajo", pLegajo),
                                new XElement("Nombre", pNombre),
                                new XElement("Apellido", pApellido),
                                new XElement("Sexo", pSexo),
                                new XElement("Email", pEmail),
                                new XElement("CUIL", pCUIL),
                                new XElement("SueldoBasico", pSueldoBasico.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                                new XElement("FechaIngreso", pFechaIngreso.ToString("yyyy-MM-dd")),
                                new XElement("DNI_Supervisor", pDNI_Superior), // Se guarda el superior
                                new XElement("Eliminado", pEliminado.ToString())));
                    xmlDoc.Save(rutaArchivo);
                    resultado = 1;
                }
            }
            catch (Exception) { return 0; }
            return resultado;
        }

        // Firma ESTANDARIZADA con 11 argumentos
        public int Modificar(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                var EModificar = xmlDoc.Descendants(NombreElementoSingular).FirstOrDefault(e => (int)e.Attribute("DNI") == pDNI);

                if (EModificar != null)
                {
                    if (EModificar.Element("Legajo") == null) EModificar.Add(new XElement("Legajo"));
                    EModificar.Element("Legajo").Value = pLegajo.ToString();

                    EModificar.Element("Nombre").Value = pNombre;
                    EModificar.Element("Apellido").Value = pApellido;

                    if (EModificar.Element("Sexo") == null) EModificar.Add(new XElement("Sexo"));
                    EModificar.Element("Sexo").Value = pSexo;

                    if (EModificar.Element("Email") == null) EModificar.Add(new XElement("Email"));
                    EModificar.Element("Email").Value = pEmail;

                    // CORRECCIÓN CLAVE: Se utiliza DNI_Supervisor
                    if (EModificar.Element("DNI_Supervisor") == null) EModificar.Add(new XElement("DNI_Supervisor"));
                    EModificar.Element("DNI_Supervisor").Value = pDNI_Superior.ToString();

                    if (EModificar.Element("CUIL") == null) EModificar.Add(new XElement("CUIL"));
                    EModificar.Element("CUIL").Value = pCUIL;

                    if (EModificar.Element("SueldoBasico") == null) EModificar.Add(new XElement("SueldoBasico"));
                    EModificar.Element("SueldoBasico").Value = pSueldoBasico.ToString(System.Globalization.CultureInfo.InvariantCulture);

                    if (EModificar.Element("FechaIngreso") == null) EModificar.Add(new XElement("FechaIngreso"));
                    EModificar.Element("FechaIngreso").Value = pFechaIngreso.ToString("yyyy-MM-dd");

                    if (EModificar.Element("Eliminado") == null) EModificar.Add(new XElement("Eliminado"));
                    EModificar.Element("Eliminado").Value = pEliminado.ToString();

                    xmlDoc.Save(rutaArchivo);
                    return 1;
                }
                return 0;
            }
            catch (Exception) { return 0; }
        }
                
        // MÉTODOS Borrar y Deshacer_Borrar
        public int Borrar(int pDNI)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                var encargado = xmlDoc.Descendants(NombreElementoSingular).FirstOrDefault(e => (int)e.Attribute("DNI") == pDNI);

                if (encargado != null)
                {
                    if (encargado.Element("Eliminado") == null) encargado.Add(new XElement("Eliminado", "True"));
                    else encargado.Element("Eliminado").Value = "True";

                    xmlDoc.Save(rutaArchivo);
                    return 1;
                }
                return 0;
            }
            catch (Exception) { return 0; }
        }

        public int Deshacer_Borrar(int pDNI)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                var encargado = xmlDoc.Descendants(NombreElementoSingular).FirstOrDefault(e => (int)e.Attribute("DNI") == pDNI);

                if (encargado != null)
                {
                    if (encargado.Element("Eliminado") == null) encargado.Add(new XElement("Eliminado", "False"));
                    else encargado.Element("Eliminado").Value = "False";

                    xmlDoc.Save(rutaArchivo);
                    return 1;
                }
                return 0;
            }
            catch (Exception) { return 0; }
        }        

        // =================================================================
        // MÉTODOS DE CONSULTA DATATABLE (CORREGIDOS PARA EL DGV)
        // =================================================================

        public DataTable Buscar_Uno(int pDNI)
        {
            DataTable dt = CrearEstructuraDataTable();

            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                var encargado = xmlDoc.Descendants(NombreElementoSingular).FirstOrDefault(e => (int)e.Attribute("DNI") == pDNI);

                if (encargado != null)
                {
                    DataRow dr = dt.NewRow();
                    dr["DNI"] = (string)encargado.Attribute("DNI");
                    dr["Legajo"] = (string)encargado.Element("Legajo");
                    dr["Nombre"] = (string)encargado.Element("Nombre");
                    dr["Apellido"] = (string)encargado.Element("Apellido");

                    // Campos adicionales para el DGV
                    dr["Sexo"] = (string)encargado.Element("Sexo");
                    dr["Email"] = (string)encargado.Element("Email");
                    dr["CUIL"] = (string)encargado.Element("CUIL");

                    // Mapea DNI_Supervisor del XML a la columna Superior del DataTable
                    dr["Superior"] = (string)encargado.Element("DNI_Supervisor");
                    dr["Cargo"] = NombreElementoSingular; // "Encargado"

                    dr["Eliminado"] = (string)encargado.Element("Eliminado");
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception) { return null; }
            return dt;
        }

        public DataTable Buscar_Todos(bool pEliminado)
        {
            DataTable dt = CrearEstructuraDataTable();

            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                IEnumerable<XElement> consulta;

                if (pEliminado)
                {
                    consulta = xmlDoc.Descendants(NombreElementoSingular);
                }
                else
                {
                    consulta = xmlDoc.Descendants(NombreElementoSingular).Where(e => (string)e.Element("Eliminado") == "False");
                }

                foreach (XElement xElement in consulta)
                {
                    DataRow dr = dt.NewRow();
                    dr["DNI"] = (string)xElement.Attribute("DNI");
                    dr["Legajo"] = (string)xElement.Element("Legajo");
                    dr["Nombre"] = (string)xElement.Element("Nombre");
                    dr["Apellido"] = (string)xElement.Element("Apellido");

                    // Campos adicionales para el DGV
                    dr["Sexo"] = (string)xElement.Element("Sexo");
                    dr["Email"] = (string)xElement.Element("Email");
                    dr["CUIL"] = (string)xElement.Element("CUIL");

                    // Mapea DNI_Supervisor del XML a la columna Superior del DataTable
                    dr["Superior"] = (string)xElement.Element("DNI_Supervisor");
                    dr["Cargo"] = NombreElementoSingular; // "Encargado"

                    dr["Eliminado"] = (string)xElement.Element("Eliminado");
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception) { return null; }
            return dt;
        }

        // Método auxiliar para crear la estructura del DataTable (para evitar repetición)
        private DataTable CrearEstructuraDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DNI");
            dt.Columns.Add("Legajo");
            dt.Columns.Add("Nombre");
            dt.Columns.Add("Apellido");
            dt.Columns.Add("Sexo");
            dt.Columns.Add("Email");
            dt.Columns.Add("CUIL");
            dt.Columns.Add("Superior"); // DNI_Supervisor en el XML
            dt.Columns.Add("Cargo");
            dt.Columns.Add("Eliminado");
            return dt;
        }

        public int CantidadEncargados()
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                return xmlDoc.Descendants(NombreElementoSingular).Count();
            }
            catch (Exception) { return 0; }
        }
    }
}