using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS
{
    public static class ConvertirListaADatatable
    {
        public static DataTable ListToDatatable<T>(List<T> lista)
        {
            // Crear el DataTable
            DataTable dt = new DataTable();

            // Obtener las propiedades del objeto (las columnas del DataTable)
            var propiedades = typeof(T).GetProperties();

            // Crear las columnas en el DataTable
            foreach (var prop in propiedades)
            {
                // Se manejan los tipos nulos para evitar errores. Si es nulo, usa el tipo base.
                Type colType = prop.PropertyType;
                if (colType.IsGenericType && colType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    colType = Nullable.GetUnderlyingType(colType);
                }
                dt.Columns.Add(prop.Name, colType);
            }

            // Llenar el DataTable con los datos de la lista
            foreach (var item in lista)
            {
                DataRow row = dt.NewRow();
                foreach (var prop in propiedades)
                {
                    row[prop.Name] = prop.GetValue(item, null) ?? DBNull.Value;
                }
                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
