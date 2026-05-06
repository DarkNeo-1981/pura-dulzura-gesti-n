using BLL;
using ENTITY;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

public class FacturaHelper
{
    private readonly ClienteServiceBLL clienteService = new ClienteServiceBLL();
    private readonly ProductoServiceBLL productoService = new ProductoServiceBLL();

    private const string NombreNegocio = "Pura Dulzura S.R.L.";
    private const string CUITNegocio = "27-30925687-0";
    private const string DireccionNegocio = "Calle 57A número 1803, Hudson, Berazategui \r\n";
    private const string TelefonoNegocio = "113792-8983";
    private const string CorreoNegocio = "Desi0000@hotmail.com ";
    private const string TipoFactura = "B";
    private static int _ultimoNumeroFactura = 1000;
    private static string _ultimoCAE = "12345678901234";

  
    public void GenerarFacturaPDF(OrdenDePedido orden)
    {
        var cliente = clienteService.BuscarPorDNI(orden.DNI_Cliente);
        if (cliente == null)
        {
            MessageBox.Show("Cliente no encontrado", "Error");
            return;
        }

        // Incrementar número de factura para cada nueva generación
        _ultimoNumeroFactura++;
        string numeroFactura = $"0001-{_ultimoNumeroFactura.ToString("D8")}"; // Formato serie-numero (ej: 0001-00001001)

        string nombreArchivo = $"Factura_{orden.Id}.pdf";
        string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Facturas");
        Directory.CreateDirectory(ruta);
        string rutaCompleta = Path.Combine(ruta, nombreArchivo);

        Document doc = new Document(PageSize.A4);
        PdfWriter.GetInstance(doc, new FileStream(rutaCompleta, FileMode.Create));
        doc.Open();

        // --- CABECERA DE FACTURA (DATOS DEL NEGOCIO Y TIPO/NUMERO) ---
        // Tabla para organizar la cabecera
        PdfPTable headerTable = new PdfPTable(2);
        headerTable.WidthPercentage = 100;
        headerTable.SetWidths(new float[] { 3f, 2f }); // Proporción de columnas

        // Columna Izquierda (Datos del Negocio)
        PdfPCell cellNegocio = new PdfPCell();
        cellNegocio.Border = iTextSharp.text.Rectangle.NO_BORDER;
        cellNegocio.AddElement(new Paragraph(NombreNegocio, FontFactory.GetFont("Arial", 12, Font.BOLD)));
        cellNegocio.AddElement(new Paragraph($"CUIT: {CUITNegocio}", FontFactory.GetFont("Arial", 10)));
        cellNegocio.AddElement(new Paragraph($"Dirección: {DireccionNegocio}", FontFactory.GetFont("Arial", 10)));
        cellNegocio.AddElement(new Paragraph($"Teléfono: {TelefonoNegocio}", FontFactory.GetFont("Arial", 10)));
        cellNegocio.AddElement(new Paragraph($"Email: {CorreoNegocio}\n\n", FontFactory.GetFont("Arial", 10)));
        headerTable.AddCell(cellNegocio);

        // Columna Derecha (Tipo de Factura, Número, Fecha)
        PdfPCell cellFacturaInfo = new PdfPCell();
        cellFacturaInfo.Border = iTextSharp.text.Rectangle.BOX; 
        cellFacturaInfo.BorderWidth = 1f;
        Font fontFacturaTipo = FontFactory.GetFont("Arial", 18, Font.BOLD, BaseColor.RED);
        cellFacturaInfo.AddElement(new Paragraph($"FACTURA {TipoFactura}", fontFacturaTipo)); // Resalta el tipo
        cellFacturaInfo.AddElement(new Paragraph($"Número: {numeroFactura}", FontFactory.GetFont("Arial", 12, Font.BOLD)));
        cellFacturaInfo.AddElement(new Paragraph($"Fecha de Emisión: {DateTime.Now.ToString("dd/MM/yyyy")}\n", FontFactory.GetFont("Arial", 10)));
        headerTable.AddCell(cellFacturaInfo);

        doc.Add(headerTable);
        doc.Add(new Paragraph("\n")); // Espacio después de la cabecera

        // --- DATOS DEL CLIENTE ---
        doc.Add(new Paragraph("DATOS DEL CLIENTE:", FontFactory.GetFont("Arial", 10, Font.BOLD)));
        doc.Add(new Paragraph($"Cliente: {cliente.NombreCompleto}", FontFactory.GetFont("Arial", 10)));
        doc.Add(new Paragraph($"DNI: {cliente.Dni}", FontFactory.GetFont("Arial", 10)));
        doc.Add(new Paragraph($"Dirección: {cliente.Calle} {cliente.Numero}, {cliente.Localidad}\n", FontFactory.GetFont("Arial", 10)));
        // Fecha de la orden (si es diferente de la fecha de emisión)
        doc.Add(new Paragraph($"Fecha de la Orden: {orden.FechaDeVenta}\n\n", FontFactory.GetFont("Arial", 10)));

        // --- DETALLES DE LA ORDEN (TU TABLA EXISTENTE) ---
        PdfPTable tabla = new PdfPTable(4);
        tabla.WidthPercentage = 100;
        tabla.AddCell(new PdfPCell(new Phrase("Producto", FontFactory.GetFont("Arial", 10, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = BaseColor.LIGHT_GRAY });
        tabla.AddCell(new PdfPCell(new Phrase("Cantidad", FontFactory.GetFont("Arial", 10, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = BaseColor.LIGHT_GRAY });
        tabla.AddCell(new PdfPCell(new Phrase("Precio Unitario", FontFactory.GetFont("Arial", 10, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = BaseColor.LIGHT_GRAY });
        tabla.AddCell(new PdfPCell(new Phrase("Subtotal", FontFactory.GetFont("Arial", 10, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = BaseColor.LIGHT_GRAY });

        foreach (var detalle in orden.Detalles)
        {
            var producto = productoService.BuscarPorId(detalle.IdProducto);
            if (producto == null) continue;

            // Celda para Producto 
            PdfPCell cellProducto = new PdfPCell(new Phrase(producto.Nombre, FontFactory.GetFont("Arial", 9)));
            cellProducto.HorizontalAlignment = Element.ALIGN_LEFT;
            tabla.AddCell(cellProducto);

            // Celda para Cantidad
            PdfPCell cellCantidad = new PdfPCell(new Phrase(detalle.Cantidad.ToString(), FontFactory.GetFont("Arial", 9)));
            cellCantidad.HorizontalAlignment = Element.ALIGN_RIGHT; // <-- Acá se aplica la alineación a la CELDA
            tabla.AddCell(cellCantidad); // <-- Y acá se añade la CELDA a la tabla

            // Celda para Precio Unitario
            PdfPCell cellPrecioUnitario = new PdfPCell(new Phrase(detalle.PrecioUnitario.ToString("C2", CultureInfo.GetCultureInfo("es-AR")), FontFactory.GetFont("Arial", 9)));
            cellPrecioUnitario.HorizontalAlignment = Element.ALIGN_RIGHT; // <-- Lo mismo acá
            tabla.AddCell(cellPrecioUnitario);

            // Celda para Subtotal
            PdfPCell cellSubtotal = new PdfPCell(new Phrase(detalle.Subtotal.ToString("C2", CultureInfo.GetCultureInfo("es-AR")), FontFactory.GetFont("Arial", 9)));
            cellSubtotal.HorizontalAlignment = Element.ALIGN_RIGHT; // <-- Y acá
            tabla.AddCell(cellSubtotal);
        }

        doc.Add(tabla);
        doc.Add(new Paragraph("\n"));

        // --- TOTAL Y PIE DE PÁGINA (CAE) ---
        doc.Add(new Paragraph($"TOTAL: {orden.Total.ToString("C2", CultureInfo.GetCultureInfo("es-AR"))}", FontFactory.GetFont("Arial", 12, Font.BOLD)) { Alignment = Element.ALIGN_RIGHT });
        doc.Add(new Paragraph("\n"));
        doc.Add(new Paragraph($"CAE: {_ultimoCAE} ", FontFactory.GetFont("Arial", 9, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
        doc.Add(new Paragraph($"Vencimiento CAE: {DateTime.Now.AddDays(10).ToString("dd/MM/yyyy")}", FontFactory.GetFont("Arial", 8)) { Alignment = Element.ALIGN_CENTER }); // Ejemplo de vencimiento


        doc.Close();

        MessageBox.Show("Factura generada con éxito", "OK");
        System.Diagnostics.Process.Start(rutaCompleta);        
    }
}