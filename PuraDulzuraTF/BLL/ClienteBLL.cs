using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ClienteBLL
    {
        ClienteDAL dal = new ClienteDAL();

        public int Agregar(string nombre, string apellido, string dni, string telefono, string email,
                           string calle, string numero, string piso, string depto, string localidad)
        {
            // Validación de DNI duplicado
            var tabla = dal.Buscar_Todos(true);
            foreach (DataRow row in tabla.Rows)
            {
                if (row["Dni"].ToString() == dni && row["Eliminado"].ToString() == "False")
                {
                    throw new Exception("Ya existe un cliente con ese DNI.");
                }
            }
            return dal.Agregar(nombre, apellido, dni, telefono, email, calle, numero, piso, depto, localidad);
        }

        public int Modificar(int id, string nombre, string apellido, string dni, string telefono, string email,
                              string calle, string numero, string piso, string depto, string localidad)
        {
            return dal.Modificar(id, nombre, apellido, dni, telefono, email, calle, numero, piso, depto, localidad);
        }

        public int Borrar(int id)
        {
            return dal.Borrar(id);
        }

        public int Deshacer_Borrar(int id)
        {
            return dal.Deshacer_Borrar(id);
        }

        public DataTable Buscar_Todos(bool incluirEliminados = false)
        {
            return dal.Buscar_Todos(incluirEliminados);
        }

        public List<Clientes> Buscar_Todos_Lista(bool incluirEliminados = false)
        {
            DataTable dt = dal.Buscar_Todos(incluirEliminados);
            List<Clientes> lista = new List<Clientes>();

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(ClienteMapper.MapFromDataRow(row));
            }
            return lista;
        }

        public int CantidadClientes()
        {
            return dal.CantidadClientes();
        }
    }
}

