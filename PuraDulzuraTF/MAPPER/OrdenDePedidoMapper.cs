using ENTITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAPPER
{
    public class OrdenDePedido
    {
        private DAL.OrdenDePedidoDAL dal = new DAL.OrdenDePedidoDAL();

        public int CantidadPedidos()
        {
            return ObtenerTodos()?.Count ?? 0;
        }

        public ENTITY.OrdenDePedido Buscar(int id)
        {
            var lista = CastDsToEnt(dal.Buscar_Uno(id));
            return lista.Count > 0 ? lista[0] : null;
        }

        public List<ENTITY.OrdenDePedido> ObtenerTodos()
        {
            return CastDsToEnt(dal.Buscar_Todos());
        }

        public bool Agregar(ENTITY.OrdenDePedido pedido)
        {
            return dal.Agregar(pedido) == 1;
        }

        public bool Eliminar(int id)
        {
            return dal.Eliminar(id) == 1;
        }

        private List<ENTITY.OrdenDePedido> CastDsToEnt(DataTable dt)
        {
            List<ENTITY.OrdenDePedido> lista = new List<ENTITY.OrdenDePedido>();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var ent = new ENTITY.OrdenDePedido
                    {
                        Id = int.Parse(dr["Id"].ToString()),
                        DNI_Vendedor = int.Parse(dr["DNI_Vendedor"].ToString()),
                        DNI_Cliente = int.Parse(dr["DNI_Cliente"].ToString()),
                        FechaDeVenta = dr["FechaDeVenta"].ToString(),
                        Total = decimal.Parse(dr["Total"].ToString()),
                        Eliminado = bool.Parse(dr["Eliminado"].ToString())                        
                    };
                    lista.Add(ent);
                }
            }
            return lista;
        }
    }
}
