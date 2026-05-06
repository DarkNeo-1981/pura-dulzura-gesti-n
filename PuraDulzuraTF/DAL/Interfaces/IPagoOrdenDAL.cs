using System.Xml.Linq; 
using System.Linq;

namespace DAL.Interfaces
{
    public interface IPagoOrdenDAL
    {        
        int Agregar(XElement pagoElement);        
        IQueryable<XElement> TraerPagosPorOrdenId(int idOrden);
        decimal ObtenerMontoTotalPagado(int idOrden);
    }
}