using ENTITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ClienteMapper
{
    public static Clientes MapFromDataRow(DataRow row)
    {
        return new Clientes
        {
            Id = int.Parse(row["Id"].ToString()),
            Nombre = row["Nombre"].ToString(),
            Apellido = row["Apellido"].ToString(),
            Dni = row["Dni"].ToString(),
            Telefono = row["Telefono"].ToString(),
            Email = row["Email"].ToString(),
            Calle = row["Calle"].ToString(),
            Numero = row["Numero"].ToString(),
            Depto = row["Depto"].ToString(),
            Piso = row["Piso"].ToString(),
            Localidad = row["Localidad"].ToString(),
        };
    }
}
