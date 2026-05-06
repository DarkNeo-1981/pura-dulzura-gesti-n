using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using ENTITY;

namespace DAL
{
    // Herencia de la clase base genérica EmpleadoBaseDAL<T>
    public class AdministrativoDAL : EmpleadoBaseDAL<Administrativo>
    {
        // Propiedades de la clase base
        protected override string NombreElementoRaiz => "Administrativos";
        protected override string NombreElementoSingular => "Administrativo";

        public AdministrativoDAL() : base("Administrativos.xml")
        {
            // El constructor base maneja la inicialización del archivo
        }

        // IMPLEMENTACIÓN REQUERIDA: Mapeo de XML a Entidad
        protected override Administrativo MapearXElementAEmpleado(XElement xElement)
        {
            Administrativo administrativo = new Administrativo();

            // Mapeo de propiedades de EmpleadoBase
            administrativo.DNI = (int)xElement.Attribute("DNI");
            administrativo.Nombre = (string)xElement.Element("Nombre");
            administrativo.Apellido = (string)xElement.Element("Apellido");
            administrativo.Email = (string)xElement.Element("Email");
            // Leer Sexo
            if (xElement.Element("Sexo") != null)
            {
                administrativo.Sexo = (string)xElement.Element("Sexo");
            }
            // Conversiones seguras
            administrativo.SueldoBasico = decimal.Parse((string)xElement.Element("SueldoBasico"), System.Globalization.CultureInfo.InvariantCulture);
            administrativo.FechaIngreso = DateTime.Parse((string)xElement.Element("FechaIngreso"));
            administrativo.Eliminado = bool.Parse((string)xElement.Element("Eliminado"));

            // Mapeo de campos específicos
            if (xElement.Element("Legajo") != null)
            {
                administrativo.Legajo = int.Parse((string)xElement.Element("Legajo"));
            }
            if (xElement.Element("CUIL") != null)
            {
                administrativo.CUIL = (string)xElement.Element("CUIL");
            }
            if (xElement.Element("DNI_Supervisor") != null)
            {
                administrativo.DNI_Supervisor = int.Parse((string)xElement.Element("DNI_Supervisor"));
            }

            return administrativo;
        }

        // =================================================================
        // MÉTODOS ABM
        // =================================================================

        // Firma ESTANDARIZADA con los 11 argumentos
        public int Agregar(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                // CORRECCIÓN: Se utiliza la conversión a int para la comparación
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
                                new XElement("DNI_Supervisor", pDNI_Superior),
                                new XElement("Eliminado", pEliminado.ToString())));
                    xmlDoc.Save(rutaArchivo);
                    return 1;
                }
                return 0;
            }
            catch (Exception) { return 0; }
        }

        // Firma ESTANDARIZADA con los 11 argumentos
        public int Modificar(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);

                var EModificar = xmlDoc.Descendants(NombreElementoSingular).FirstOrDefault(a => (int)a.Attribute("DNI") == pDNI);

                if (EModificar != null)
                {
                    // Asegurar existencia y actualizar Legajo
                    if (EModificar.Element("Legajo") == null) EModificar.Add(new XElement("Legajo"));
                    EModificar.Element("Legajo").Value = pLegajo.ToString();

                    EModificar.Element("Nombre").Value = pNombre;
                    EModificar.Element("Apellido").Value = pApellido;

                    // Asegurar existencia y actualizar Sexo
                    if (EModificar.Element("Sexo") == null) EModificar.Add(new XElement("Sexo"));
                    EModificar.Element("Sexo").Value = pSexo;

                    // Asegurar existencia y actualizar Email
                    if (EModificar.Element("Email") == null) EModificar.Add(new XElement("Email"));
                    EModificar.Element("Email").Value = pEmail;

                    // Asegurar existencia y actualizar DNI_Supervisor
                    if (EModificar.Element("DNI_Supervisor") == null) EModificar.Add(new XElement("DNI_Supervisor"));
                    EModificar.Element("DNI_Supervisor").Value = pDNI_Superior.ToString();

                    // Asegurar existencia y actualizar CUIL
                    if (EModificar.Element("CUIL") == null) EModificar.Add(new XElement("CUIL"));
                    EModificar.Element("CUIL").Value = pCUIL;

                    // Asegurar existencia y actualizar SueldoBasico
                    if (EModificar.Element("SueldoBasico") == null) EModificar.Add(new XElement("SueldoBasico"));
                    EModificar.Element("SueldoBasico").Value = pSueldoBasico.ToString(System.Globalization.CultureInfo.InvariantCulture);

                    // Asegurar existencia y actualizar FechaIngreso
                    if (EModificar.Element("FechaIngreso") == null) EModificar.Add(new XElement("FechaIngreso"));
                    EModificar.Element("FechaIngreso").Value = pFechaIngreso.ToString("yyyy-MM-dd");

                    // Asegurar existencia y actualizar Eliminado
                    if (EModificar.Element("Eliminado") == null) EModificar.Add(new XElement("Eliminado"));
                    EModificar.Element("Eliminado").Value = pEliminado.ToString();

                    xmlDoc.Save(rutaArchivo);
                    return 1;
                }
                return 0;
            }
            catch (Exception) { return 0; }
        }

        // Métodos Borrar y Deshacer_Borrar

        public int Borrar(int pDNI)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                var administrativo = xmlDoc.Descendants(NombreElementoSingular).FirstOrDefault(a => (int)a.Attribute("DNI") == pDNI);

                if (administrativo != null)
                {
                    if (administrativo.Element("Eliminado") == null) administrativo.Add(new XElement("Eliminado", "True"));
                    else administrativo.Element("Eliminado").Value = "True";

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
                var administrativo = xmlDoc.Descendants(NombreElementoSingular).FirstOrDefault(a => (int)a.Attribute("DNI") == pDNI);

                if (administrativo != null)
                {
                    if (administrativo.Element("Eliminado") == null) administrativo.Add(new XElement("Eliminado", "False"));
                    else administrativo.Element("Eliminado").Value = "False";

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

        // Método auxiliar para crear la estructura del DataTable completa
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
            dt.Columns.Add("SueldoBasico"); 
            dt.Columns.Add("FechaIngreso"); 
            dt.Columns.Add("Superior");  
            dt.Columns.Add("Cargo");     
            dt.Columns.Add("Eliminado");
            return dt;
        }

        public DataTable Buscar_Uno(int pDNI)
        {
            DataTable dt = CrearEstructuraDataTable();

            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                var xElement = xmlDoc.Descendants(NombreElementoSingular).FirstOrDefault(a => (int)a.Attribute("DNI") == pDNI);

                if (xElement != null)
                {
                    DataRow dr = dt.NewRow();
                    dr["DNI"] = (string)xElement.Attribute("DNI");
                    dr["Legajo"] = (string)xElement.Element("Legajo");
                    dr["Nombre"] = (string)xElement.Element("Nombre");
                    dr["Apellido"] = (string)xElement.Element("Apellido");
                    dr["Sexo"] = (string)xElement.Element("Sexo");
                    dr["Email"] = (string)xElement.Element("Email");
                    dr["CUIL"] = (string)xElement.Element("CUIL");
                    dr["SueldoBasico"] = (string)xElement.Element("SueldoBasico");
                    dr["FechaIngreso"] = (string)xElement.Element("FechaIngreso");
                    dr["Superior"] = (string)xElement.Element("DNI_Supervisor");
                    dr["Cargo"] = NombreElementoSingular; // "Administrativo"
                    dr["Eliminado"] = (string)xElement.Element("Eliminado");
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
                    // Si pEliminado es false, solo trae los NO eliminados
                    consulta = xmlDoc.Descendants(NombreElementoSingular).Where(a => (string)a.Element("Eliminado") == "False");
                }

                foreach (XElement xElement in consulta)
                {
                    DataRow dr = dt.NewRow();
                    dr["DNI"] = (string)xElement.Attribute("DNI");
                    dr["Legajo"] = (string)xElement.Element("Legajo");
                    dr["Nombre"] = (string)xElement.Element("Nombre");
                    dr["Apellido"] = (string)xElement.Element("Apellido");
                    dr["Sexo"] = (string)xElement.Element("Sexo");
                    dr["Email"] = (string)xElement.Element("Email");
                    dr["CUIL"] = (string)xElement.Element("CUIL");
                    dr["SueldoBasico"] = (string)xElement.Element("SueldoBasico");
                    dr["FechaIngreso"] = (string)xElement.Element("FechaIngreso");
                    dr["Superior"] = (string)xElement.Element("DNI_Supervisor");
                    dr["Cargo"] = NombreElementoSingular; // "Administrativo"
                    dr["Eliminado"] = (string)xElement.Element("Eliminado");
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception) { return null; }
            return dt;
        }

        public int CantidadAdministrativos()
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