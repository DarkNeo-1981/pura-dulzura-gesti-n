using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL.Interfaces
{
    public interface IOrdenCompraDAL
    {        
        XElement TraerElementoPorId(int idOrden);
        IQueryable<XElement> TraerTodosElementos();
        int Agregar(XElement ordenElement); // Recibe XElement
        void Modificar(int id, XElement elementoModificado); // Recibe XElement
        void ActualizarEstadoPago(int idOrden, string nuevoEstadoPago);        
        int CambiarEstado(int idOrden, string nuevoEstado);
        string ObtenerEstadoPagoActual(int idOrden);
    }
}
