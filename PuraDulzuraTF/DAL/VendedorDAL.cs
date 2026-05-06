using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using ENTITY; // Asume que la entidad Vendedor existe

namespace DAL
{
    // Se hereda de la clase base EmpleadoBaseDAL<T>
    public class VendedorDAL : EmpleadoBaseDAL<Vendedor>
    {
        protected override string NombreElementoRaiz => "Vendedores";
        protected override string NombreElementoSingular => "Vendedor";

        public VendedorDAL() : base("Vendedores.xml")
        {
            // El constructor base (EmpleadoBaseDAL) maneja la creación de la carpeta y archivo.
        }

        protected override Vendedor MapearXElementAEmpleado(XElement xElement)
        {
            Vendedor vendedor = new Vendedor();

            // Mapeo de propiedades de EmpleadoBase (Lectura robusta)
            // Siempre deben existir DNI y los campos básicos, si no, fallará
            vendedor.DNI = (int)xElement.Attribute("DNI");
            vendedor.Nombre = (string)xElement.Element("Nombre");
            vendedor.Apellido = (string)xElement.Element("Apellido");
            vendedor.Email = (string)xElement.Element("Email");
            // Conversiones seguras y obligatorias
            vendedor.SueldoBasico = decimal.Parse((string)xElement.Element("SueldoBasico"), System.Globalization.CultureInfo.InvariantCulture);
            vendedor.FechaIngreso = DateTime.Parse((string)xElement.Element("FechaIngreso"));
            vendedor.Eliminado = bool.Parse((string)xElement.Element("Eliminado"));
            if (xElement.Element("Legajo") != null) vendedor.Legajo = int.Parse((string)xElement.Element("Legajo"));
            if (xElement.Element("CUIL") != null) vendedor.CUIL = (string)xElement.Element("CUIL");
            if (xElement.Element("Sexo") != null) vendedor.Sexo = (string)xElement.Element("Sexo");
            if (xElement.Element("DNI_Supervisor") != null) vendedor.DNI_Supervisor = int.Parse((string)xElement.Element("DNI_Supervisor"));
            return vendedor;
        }

        // =================================================================
        // MÉTODOS ABM 
        // =================================================================

        public int Agregar(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                var dniExistente = xmlDoc.Descendants(NombreElementoSingular).Any(x => (int)x.Attribute("DNI") == pDNI);

                if (!dniExistente)
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
                                new XElement("Eliminado", pEliminado.ToString())
                            ));
                    xmlDoc.Save(rutaArchivo);
                    return 1;
                }
                return 0;
            }
            catch (Exception) { return 0; }
        }

        public int Modificar(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                var vendedor = xmlDoc.Descendants(NombreElementoSingular).FirstOrDefault(v => (int)v.Attribute("DNI") == pDNI);

                if (vendedor != null)
                {
                    vendedor.Element("Legajo").Value = pLegajo.ToString();
                    vendedor.Element("Nombre").Value = pNombre;
                    vendedor.Element("Apellido").Value = pApellido;
                    if (vendedor.Element("Sexo") == null) vendedor.Add(new XElement("Sexo"));
                    vendedor.Element("Sexo").Value = pSexo;
                    vendedor.Element("Email").Value = pEmail;
                    if (vendedor.Element("DNI_Supervisor") == null) vendedor.Add(new XElement("DNI_Supervisor"));
                    vendedor.Element("DNI_Supervisor").Value = pDNI_Superior.ToString();
                    if (vendedor.Element("CUIL") == null) vendedor.Add(new XElement("CUIL"));
                    vendedor.Element("CUIL").Value = pCUIL;
                    if (vendedor.Element("SueldoBasico") == null) vendedor.Add(new XElement("SueldoBasico"));
                    vendedor.Element("SueldoBasico").Value = pSueldoBasico.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    if (vendedor.Element("FechaIngreso") == null) vendedor.Add(new XElement("FechaIngreso"));
                    vendedor.Element("FechaIngreso").Value = pFechaIngreso.ToString("yyyy-MM-dd");
                    if (vendedor.Element("Eliminado") == null) vendedor.Add(new XElement("Eliminado"));
                    vendedor.Element("Eliminado").Value = pEliminado.ToString();
                    xmlDoc.Save(rutaArchivo);
                    return 1;
                }
                return 0;
            }
            catch (Exception) { return 0; }
        }

        //--------------------------------------------------------------------------------------------------------------

        public int Borrar(int pDNI)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                var vendedor = xmlDoc.Descendants(NombreElementoSingular).FirstOrDefault(v => (int)v.Attribute("DNI") == pDNI);

                if (vendedor != null)
                {
                    if (vendedor.Element("Eliminado") == null) vendedor.Add(new XElement("Eliminado", "True"));
                    else vendedor.Element("Eliminado").Value = "True";
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
                var vendedor = xmlDoc.Descendants(NombreElementoSingular).FirstOrDefault(v => (int)v.Attribute("DNI") == pDNI);

                if (vendedor != null)
                {
                    if (vendedor.Element("Eliminado") == null) vendedor.Add(new XElement("Eliminado", "False"));
                    else vendedor.Element("Eliminado").Value = "False";
                    xmlDoc.Save(rutaArchivo);
                    return 1;
                }
                return 0;
            }
            catch (Exception) { return 0; }
        }

        // =================================================================
        // MÉTODOS DE CONSULTA DATATABLE (Corregidos para el DGV)
        // =================================================================

        public DataTable Buscar_Uno(int pDNI)
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

            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                var vendedor = xmlDoc.Descendants(NombreElementoSingular).FirstOrDefault(v => (int)v.Attribute("DNI") == pDNI);

                if (vendedor != null)
                {
                    DataRow dr = dt.NewRow();
                    dr["DNI"] = (string)vendedor.Attribute("DNI");
                    dr["Legajo"] = (string)vendedor.Element("Legajo");
                    dr["Nombre"] = (string)vendedor.Element("Nombre");
                    dr["Apellido"] = (string)vendedor.Element("Apellido");
                    dr["Sexo"] = (string)vendedor.Element("Sexo");
                    dr["Email"] = (string)vendedor.Element("Email");
                    dr["CUIL"] = (string)vendedor.Element("CUIL");
                    dr["SueldoBasico"] = (string)vendedor.Element("SueldoBasico");
                    dr["FechaIngreso"] = (string)vendedor.Element("FechaIngreso");
                    dr["Superior"] = (string)vendedor.Element("DNI_Supervisor");
                    dr["Cargo"] = NombreElementoSingular; // "Vendedor"
                    dr["Eliminado"] = (string)vendedor.Element("Eliminado");
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception) { return null; }
            return dt;
        }

        public DataTable Buscar_Todos(bool pEliminado)
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
                    consulta = xmlDoc.Descendants(NombreElementoSingular).Where(v => (string)v.Element("Eliminado") == "False");
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
                    dr["Cargo"] = NombreElementoSingular; // "Vendedor"
                    dr["Eliminado"] = (string)xElement.Element("Eliminado");
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception) { return null; }
            return dt;
        }

        public int CantidadVendedores()
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
