using ENTITY;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using MigraDoc.Rendering; // Contiene PdfDocumentRenderer
using System;
using System.Collections.Generic;
using System.IO;
using PdfSharp.Pdf.IO;

namespace Utils
{
    public static class PdfGenerator
    {
        public static string GenerarReciboPDF(ReciboDeSueldo recibo)
        {
            try
            {
                // Crear documento y sección
                Document document = new Document();
                document.Info.Title = "Recibo de Sueldo";
                Section section = document.AddSection();
                section.PageSetup = document.DefaultPageSetup.Clone();
                section.PageSetup.PageFormat = PageFormat.A4;
                section.PageSetup.Orientation = Orientation.Portrait;
                section.PageSetup.TopMargin = "2cm";
                section.PageSetup.BottomMargin = "2cm";
                section.PageSetup.LeftMargin = "2.5cm";
                section.PageSetup.RightMargin = "2.5cm";

                var empleado = recibo.Empleado;
                if (empleado == null) empleado = new EmpleadoBase { DNI = 0, Legajo = 0 };

                // ===========================================
                // 1. MEMBRETE y LOGO (CON BLINDAJE DE ERRORES)
                // ===========================================

                Table logoTable = section.AddTable();
                logoTable.Borders.Width = 0;

                // Columnas: Columna para el logo (5cm), Columna para el título (6cm), Columna de espacio/centrado (5cm)
                logoTable.AddColumn(Unit.FromCentimeter(5));
                logoTable.AddColumn(Unit.FromCentimeter(6));
                logoTable.AddColumn(Unit.FromCentimeter(5));

                Row logoRow = logoTable.AddRow();

                // ------------------------------
                // LÓGICA DE INSERCIÓN DEL LOGO
                // ------------------------------

                // La ruta de ejecución es la carpeta BIN.
                string nombreLogo = "Logo Pura Dulzura.jpg";
                string rutaLogo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", nombreLogo);

                Cell logoCell = logoRow.Cells[0];
                logoCell.VerticalAlignment = VerticalAlignment.Center;
                logoCell.Format.Alignment = ParagraphAlignment.Center;
                logoCell.Shading.Color = Colors.White;

                if (File.Exists(rutaLogo))
                {
                    // Si el archivo existe en la carpeta de ejecución, se inserta
                    Image logo = logoCell.AddImage(rutaLogo);
                    logo.Width = Unit.FromCentimeter(4.0);
                    logo.LockAspectRatio = true;
                }
                else
                {
                    // Si no se encuentra, se muestra un mensaje de error simple (sin la ruta completa)
                    // para evitar el error de 'null' en MigraDoc.
                    logoCell.AddParagraph("LOGO NO ENCONTRADO").Format.Alignment = ParagraphAlignment.Center;
                    logoCell.AddParagraph("Asegúrese de copiar el archivo 'Logo Pura Dulzura.jpg' al directorio de salida.").Format.Alignment = ParagraphAlignment.Center;
                    logoCell.Format.Font.Size = 7;
                    logoCell.Shading.Color = Colors.LightSalmon;
                }

                // Celda 1: Encabezado Centrado
                Paragraph header = logoRow.Cells[1].AddParagraph("PURA DULZURA S.R.L.");
                header.Format.Font.Size = 16;
                header.Format.Font.Bold = true;
                header.Format.Alignment = ParagraphAlignment.Center;
                header.Format.SpaceAfter = "0.2cm";

                // Celda 2: Espacio vacío para centrar

                logoTable.Rows.LeftIndent = 0;

                // Título Secundario Centrado
                Paragraph subHeader = section.AddParagraph("RECIBO DE HABERES");
                subHeader.Format.Font.Size = 12;
                subHeader.Format.Alignment = ParagraphAlignment.Center;
                subHeader.Format.SpaceAfter = "0.5cm";

                // Separador
                section.AddParagraph("").Format.Borders.Bottom = new Border { Width = 1, Color = Colors.Black };
                section.AddParagraph("\n");

                // ===========================================
                // 2. DATOS DEL EMPLEADO
                // ===========================================

                Table datosEmpleado = section.AddTable();
                datosEmpleado.Borders.Width = 0.5;

                datosEmpleado.AddColumn(Unit.FromCentimeter(3.5));
                datosEmpleado.AddColumn(Unit.FromCentimeter(5.5));
                datosEmpleado.AddColumn(Unit.FromCentimeter(3.5));
                datosEmpleado.AddColumn(Unit.FromCentimeter(3.5));

                datosEmpleado.Rows.LeftIndent = Unit.FromCentimeter(0.5);

                // Fila 1
                Row fila1 = datosEmpleado.AddRow();
                fila1.Cells[0].AddParagraph("Empleado:");
                fila1.Cells[1].AddParagraph(empleado?.NombreCompleto ?? "N/A - Nombre Nulo");
                fila1.Cells[2].AddParagraph("DNI:");
                fila1.Cells[3].AddParagraph(empleado?.DNI.ToString() ?? "N/A");

                // Fila 2
                Row fila2 = datosEmpleado.AddRow();
                fila2.Cells[0].AddParagraph("Legajo:");
                fila2.Cells[1].AddParagraph(empleado?.Legajo.ToString() ?? "N/A");
                fila2.Cells[2].AddParagraph("Período:");
                fila2.Cells[3].AddParagraph($"{recibo.Periodo:MM/yyyy}");

                // Fila 3 (Categoría y CUIL)
                Row fila3 = datosEmpleado.AddRow();
                fila3.Cells[0].AddParagraph("Categoría:");
                fila3.Cells[1].AddParagraph(empleado?.Categoria ?? "No Asignada");
                fila3.Cells[2].AddParagraph("CUIL:");
                fila3.Cells[3].AddParagraph(empleado?.CUIL ?? "N/A");

                section.AddParagraph("\n");

                // ===========================================
                // 3. TABLA DE ITEMS (Detalle)
                // ===========================================

                Table tabla = section.AddTable();
                tabla.Borders.Width = 0.75;
                tabla.Borders.Color = Colors.Gray;

                tabla.AddColumn(Unit.FromCentimeter(6)); // Concepto
                tabla.AddColumn(Unit.FromCentimeter(3)); // Haberes
                tabla.AddColumn(Unit.FromCentimeter(3)); // Deducciones
                tabla.AddColumn(Unit.FromCentimeter(3)); // Neto parcial

                tabla.Rows.LeftIndent = Unit.FromCentimeter(0.5);

                Row encabezado = tabla.AddRow();
                encabezado.Shading.Color = Colors.LightGray;
                encabezado.Cells[0].AddParagraph("Concepto");
                encabezado.Cells[1].AddParagraph("Haberes");
                encabezado.Cells[2].AddParagraph("Descuentos");
                encabezado.Cells[3].AddParagraph("Neto");

                foreach (Cell cell in encabezado.Cells)
                {
                    cell.VerticalAlignment = VerticalAlignment.Center;
                    if (cell.Column.Index > 0) cell.Format.Alignment = ParagraphAlignment.Right;
                }

                foreach (var item in recibo.Detalle)
                {
                    Row row = tabla.AddRow();
                    row.Cells[0].AddParagraph(item.Concepto ?? "Concepto Inválido");

                    string tipoItem = item.Tipo ?? string.Empty;

                    if (tipoItem == "Haber")
                    {
                        row.Cells[1].AddParagraph(item.Monto.ToString("C2")).Format.Alignment = ParagraphAlignment.Right;
                        row.Cells[2].AddParagraph("-").Format.Alignment = ParagraphAlignment.Right;
                        row.Cells[3].AddParagraph(item.Monto.ToString("C2")).Format.Alignment = ParagraphAlignment.Right;
                    }
                    else if (tipoItem == "Descuento")
                    {
                        row.Cells[1].AddParagraph("-").Format.Alignment = ParagraphAlignment.Right;
                        row.Cells[2].AddParagraph(item.Monto.ToString("C2")).Format.Alignment = ParagraphAlignment.Right;
                        row.Cells[3].AddParagraph((-item.Monto).ToString("C2")).Format.Alignment = ParagraphAlignment.Right;
                    }
                    else
                    {
                        row.Cells[1].AddParagraph("-").Format.Alignment = ParagraphAlignment.Right;
                        row.Cells[2].AddParagraph("-").Format.Alignment = ParagraphAlignment.Right;
                        row.Cells[3].AddParagraph(item.Monto.ToString("C2")).Format.Alignment = ParagraphAlignment.Right;
                    }
                }

                // ============
                // 4. TOTALES 
                // ============

                section.AddParagraph("\n");
                Table totales = section.AddTable();

                totales.Rows.LeftIndent = Unit.FromCentimeter(1.5);

                totales.AddColumn(Unit.FromCentimeter(10));
                totales.AddColumn(Unit.FromCentimeter(5));

                Row totalRow1 = totales.AddRow();
                totalRow1.Cells[0].AddParagraph("Total Haberes:").Format.Alignment = ParagraphAlignment.Right;
                totalRow1.Cells[1].AddParagraph(recibo.TotalHaberes.ToString("C2")).Format.Alignment = ParagraphAlignment.Right;

                Row totalRow2 = totales.AddRow();
                totalRow2.Cells[0].AddParagraph("Total Descuentos:").Format.Alignment = ParagraphAlignment.Right;
                totalRow2.Cells[1].AddParagraph(recibo.TotalDescuentos.ToString("C2")).Format.Alignment = ParagraphAlignment.Right;

                Row totalRow3 = totales.AddRow();
                totalRow3.Cells[0].AddParagraph("Neto a Cobrar:").Format.Alignment = ParagraphAlignment.Right;
                totalRow3.Cells[1].AddParagraph(recibo.NetoAPagar.ToString("C2")).Format.Alignment = ParagraphAlignment.Right;
                totalRow3.Shading.Color = Colors.LightGray;
                totalRow3.Format.Font.Bold = true;

                // ============
                // 5. FIRMAS 
                // ============

                section.AddParagraph("\n\n");

                Table firmas = section.AddTable();

                firmas.AddColumn(Unit.FromCentimeter(7.5)); // Empleador
                firmas.AddColumn(Unit.FromCentimeter(7.5)); // Empleado
                firmas.Rows.LeftIndent = Unit.FromCentimeter(0.5);

                Row firmaRow = firmas.AddRow();

                // Firma del Empleador (Izquierda)
                firmaRow.Cells[0].AddParagraph("______________________________");
                firmaRow.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                firmaRow.Cells[0].Format.SpaceBefore = "1cm";
                firmaRow.Cells[0].AddParagraph("Firma y Sello del Empleador").Format.Alignment = ParagraphAlignment.Center;

                // Firma del Empleado (Derecha)
                firmaRow.Cells[1].AddParagraph("______________________________");
                firmaRow.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                firmaRow.Cells[1].Format.SpaceBefore = "1cm";
                firmaRow.Cells[1].AddParagraph("Firma del Empleado").Format.Alignment = ParagraphAlignment.Center;


                // Guardar PDF
                string directorio = Path.Combine(Environment.CurrentDirectory, "RecibosGenerados");
                if (!Directory.Exists(directorio))
                    Directory.CreateDirectory(directorio);

                string dniParaArchivo = recibo.Empleado?.DNI.ToString() ?? "NOGET";
                string rutaArchivo = Path.Combine(directorio, $"Recibo_{dniParaArchivo}_{recibo.Periodo:yyyyMM}.pdf");

                // Renderización y guardado
                PdfDocumentRenderer renderer = new PdfDocumentRenderer(true)
                {
                    Document = document
                };
                renderer.RenderDocument();
                renderer.PdfDocument.Save(rutaArchivo);

                return rutaArchivo;
            }
            catch (Exception ex)
            {                
                throw new Exception($"Error al generar PDF (Tipo: {ex.GetType().Name}): {ex.Message}", ex);
            }
        }
    }
}