using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using ENTITY;

namespace DAL
{
    // 1. Se hereda de la clase base genérica EmpleadoBaseDAL<T>
    public class GerenteDAL : EmpleadoBaseDAL<Gerente>
    {
        // 2. Propiedades abstractas de la clase base
        protected override string NombreElementoRaiz => "Gerentes";
        protected override string NombreElementoSingular => "Gerente";

        public GerenteDAL() : base("Gerentes.xml")
        {
            // El constructor base (EmpleadoBaseDAL) maneja la creación de la carpeta y archivo.
        }

        // 3. Implementación requerida de la clase base: Mapear XElement a Entidad
        protected override Gerente MapearXElementAEmpleado(XElement xElement)
        {
            Gerente gerente = new Gerente();

            // Mapeo de propiedades de EmpleadoBase
            gerente.DNI = (int)xElement.Attribute("DNI");
            gerente.Nombre = (string)xElement.Element("Nombre");
            gerente.Apellido = (string)xElement.Element("Apellido");
            gerente.Email = (string)xElement.Element("Email");

            // Verificación y Mapeo de Sexo
            if (xElement.Element("Sexo") != null) gerente.Sexo = (string)xElement.Element("Sexo");

            // Conversiones seguras y obligatorias
            gerente.SueldoBasico = decimal.Parse((string)xElement.Element("SueldoBasico"), System.Globalization.CultureInfo.InvariantCulture);
            gerente.FechaIngreso = DateTime.Parse((string)xElement.Element("FechaIngreso"));
            gerente.Eliminado = bool.Parse((string)xElement.Element("Eliminado"));

            // Mapeo de campos específicos (verificando existencia)
            if (xElement.Element("Legajo") != null)
            {
                gerente.Legajo = int.Parse((string)xElement.Element("Legajo"));
            }
            if (xElement.Element("CUIL") != null)
            {
                gerente.CUIL = (string)xElement.Element("CUIL");
            }

            // El Gerente no tiene superior, por lo que no se mapea DNI_Superior.

            return gerente;
        }

        // ===========
        // MÉTODOS ABM 
        // ===========
                
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
                                // No se guarda DNI_Superior en el XML del Gerente, pero el parámetro se mantiene para uniformidad de la capa BLL.
                                new XElement("Eliminado", pEliminado.ToString())
                            ));
                    xmlDoc.Save(rutaArchivo);
                    return 1;
                }
                return 0;
            }
            catch (Exception) { return 0; }
        }

        // MÉTODO MODIFICAR CORREGIDO: Uniformando los parámetros (11 argumentos)
        public int Modificar(int pDNI, int pLegajo, string pNombre, string pApellido, string pSexo, string pEmail, string pCUIL, decimal pSueldoBasico, DateTime pFechaIngreso, int pDNI_Superior, bool pEliminado)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                // Uso FirstOrDefault para ser más directo.
                var gerente = xmlDoc.Descendants(NombreElementoSingular).FirstOrDefault(g => (int)g.Attribute("DNI") == pDNI);

                if (gerente != null)
                {
                    gerente.Element("Legajo").Value = pLegajo.ToString();
                    gerente.Element("Nombre").Value = pNombre;
                    gerente.Element("Apellido").Value = pApellido;

                    // Asegurar existencia y actualizar Sexo
                    if (gerente.Element("Sexo") == null) gerente.Add(new XElement("Sexo"));
                    gerente.Element("Sexo").Value = pSexo;

                    gerente.Element("Email").Value = pEmail;

                    // Asegurar existencia y actualizar CUIL
                    if (gerente.Element("CUIL") == null) gerente.Add(new XElement("CUIL"));
                    gerente.Element("CUIL").Value = pCUIL;

                    // Asegurar existencia y actualizar SueldoBasico
                    if (gerente.Element("SueldoBasico") == null) gerente.Add(new XElement("SueldoBasico"));
                    gerente.Element("SueldoBasico").Value = pSueldoBasico.ToString(System.Globalization.CultureInfo.InvariantCulture);

                    // Asegurar existencia y actualizar FechaIngreso
                    if (gerente.Element("FechaIngreso") == null) gerente.Add(new XElement("FechaIngreso"));
                    gerente.Element("FechaIngreso").Value = pFechaIngreso.ToString("yyyy-MM-dd");

                    // pDNI_Superior SE OMITE de la modificación para el Gerente.

                    // Asegurar existencia y actualizar Eliminado
                    if (gerente.Element("Eliminado") == null) gerente.Add(new XElement("Eliminado"));
                    gerente.Element("Eliminado").Value = pEliminado.ToString();

                    xmlDoc.Save(rutaArchivo);
                    return 1;
                }
                return 0;
            }
            catch (Exception) { return 0; }
        }

        // =================================================================
        // OTROS MÉTODOS (Borrar y Deshacer_Borrar)
        // =================================================================

        public int Borrar(int pDNI)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(rutaArchivo);
                var gerente = xmlDoc.Descendants(NombreElementoSingular).FirstOrDefault(g => (int)g.Attribute("DNI") == pDNI);

                if (gerente != null)
                {
                    if (gerente.Element("Eliminado") == null) gerente.Add(new XElement("Eliminado", "True"));
                    else gerente.Element("Eliminado").Value = "True";

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
                var gerente = xmlDoc.Descendants(NombreElementoSingular).FirstOrDefault(g => (int)g.Attribute("DNI") == pDNI);

                if (gerente != null)
                {
                    if (gerente.Element("Eliminado") == null) gerente.Add(new XElement("Eliminado", "False"));
                    else gerente.Element("Eliminado").Value = "False";

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
                var gerente = xmlDoc.Descendants(NombreElementoSingular).FirstOrDefault(g => (int)g.Attribute("DNI") == pDNI);

                if (gerente != null)
                {
                    DataRow dr = dt.NewRow();
                    dr["DNI"] = (string)gerente.Attribute("DNI");
                    dr["Legajo"] = (string)gerente.Element("Legajo");
                    dr["Nombre"] = (string)gerente.Element("Nombre");
                    dr["Apellido"] = (string)gerente.Element("Apellido");
                    dr["Sexo"] = (string)gerente.Element("Sexo");
                    dr["Email"] = (string)gerente.Element("Email");
                    dr["CUIL"] = (string)gerente.Element("CUIL");
                    dr["SueldoBasico"] = (string)gerente.Element("SueldoBasico");
                    dr["FechaIngreso"] = (string)gerente.Element("FechaIngreso");
                    // CORRECCIÓN CLAVE: El Gerente no tiene superior, se asigna un valor nulo/cero para la columna.
                    dr["Superior"] = "0";
                    dr["Cargo"] = NombreElementoSingular; // Asigna el cargo

                    dr["Eliminado"] = (string)gerente.Element("Eliminado");
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
                    consulta = xmlDoc.Descendants(NombreElementoSingular).Where(g => (string)g.Element("Eliminado") == "False");
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
                    // CORRECCIÓN CLAVE: El Gerente no tiene superior, se asigna un valor nulo/cero para la columna.
                    dr["Superior"] = "0";
                    dr["Cargo"] = NombreElementoSingular; // Asigna el cargo

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
            dt.Columns.Add("SueldoBasico");
            dt.Columns.Add("FechaIngreso");
            dt.Columns.Add("Superior"); // Columna necesaria para la grilla
            dt.Columns.Add("Cargo");    // Columna necesaria para la grilla
            dt.Columns.Add("Eliminado");
            return dt;
        }

        public int CantidadGerentes()
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
