using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class OrdenDePedidoBLL
    {
        OrdenDePedidoDAL dal = new OrdenDePedidoDAL();
        private OrdenDePedidoDAL dalPedido;

        public bool Agregar(OrdenDePedido pedido)
        {
            int resultado = dal.Agregar(pedido);
            return resultado == 1;
        }

        public List<OrdenDePedido> ObtenerTodos()
        {
            List<OrdenDePedido> lista = new List<OrdenDePedido>();
            DataTable dt = dal.Buscar_Todos();

            if (dt == null)
                return lista;

            foreach (DataRow row in dt.Rows)
            {
                if (row["Eliminado"].ToString().ToLower() != "true")
                {
                    OrdenDePedido o = new OrdenDePedido
                    {
                        Id = int.Parse(row["Id"].ToString()),
                        DNI_Vendedor = int.Parse(row["DNI_Vendedor"].ToString()),
                        DNI_Cliente = int.Parse(row["DNI_Cliente"].ToString()),
                        FechaDeVenta = row["FechaDeVenta"].ToString(),
                        Total = decimal.Parse(row["Total"].ToString()),
                        Eliminado = bool.Parse(row["Eliminado"].ToString()),
                        Cobrada = bool.Parse(row["Cobrada"].ToString()),
                        Facturada = bool.Parse(row["Facturada"].ToString())
                    };
                    lista.Add(o);
                }
            }
            return lista;
        }

        public OrdenDePedido ObtenerPorId(int id)
        {
            DataTable dt = dal.Buscar_Uno(id);
            if (dt == null || dt.Rows.Count == 0)
                return null;

            DataRow row = dt.Rows[0];
            OrdenDePedido o = new OrdenDePedido
            {
                Id = int.Parse(row["Id"].ToString()),
                DNI_Vendedor = int.Parse(row["DNI_Vendedor"].ToString()),
                DNI_Cliente = int.Parse(row["DNI_Cliente"].ToString()),
                FechaDeVenta = row["FechaDeVenta"].ToString(),
                Total = decimal.Parse(row["Total"].ToString()),
                Eliminado = row["Eliminado"].ToString().ToLower() == "true"
            };

            return o;
        }

        public bool Eliminar(int id)
        {
            int resultado = dal.Eliminar(id);
            return resultado == 1;
        }

        public int CantidadTotal()
        {
            List<OrdenDePedido> lista = ObtenerTodos();
            return lista.Count;
        }        

        public OrdenDePedidoBLL()
        {
            dalPedido = new OrdenDePedidoDAL();
        }

        public OrdenDePedido BuscarCompleto(int id)
        {
            return dalPedido.BuscarCompleto(id);
        }
        public bool MarcarComoFacturada(int id)
        {
            int resultado = dal.MarcarComoFacturada(id);
            return resultado == 1;
        }

        public bool MarcarComoCobrada(int id)
        {
            int resultado = dal.MarcarComoCobrada(id);
            return resultado == 1;
        }
    }
}

