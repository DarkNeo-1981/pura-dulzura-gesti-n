using BLL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace UI
{
    public partial class Dashboard : Form
    {
        private readonly Color primaryBg = Color.FromArgb(140, 90, 53);   // Marrón principal
        private readonly Color secondaryBg = Color.FromArgb(245, 245, 245); // Fondo claro para charts
        private readonly Color textColor = Color.Black;

        public List<Clientes> ListaClientes;
        public List<Producto> ListaProductos;
        public List<OrdenDePedido> ListaOrdenesPedidos;

        private readonly Color[] seriesColors = new Color[]
        {
            Color.FromArgb(93, 173, 226),
            Color.FromArgb(241, 196, 15),
            Color.FromArgb(46, 204, 113),
            Color.FromArgb(155, 89, 182),
            Color.FromArgb(231, 76, 60),
            Color.FromArgb(52, 152, 219),
            Color.FromArgb(26, 188, 156)
        };

        DateTime FechaInicio;
        DateTime FechaFin;

        // Layouts (runtime)
        private TableLayoutPanel rtMainLayout;
        //private TableLayoutPanel rtTopLayout;
        //private TableLayoutPanel rtMidLayout;
        //private TableLayoutPanel rtBottomLayout;

        // Controles (runtime) - prefijo rt para evitar confusion con Designer
        private DateTimePicker rtDtpInicio;
        private DateTimePicker rtDtpFin;
        private Button rtBtnActualizar;
        private Chart rtChartPedidos;
        private Chart rtChartIngresos;
        private Chart rtChartVendedores;
        private Chart rtChartCobros;
        private Chart rtChartProductosVendidos;
        private Chart rtChartRentabilidad;

        private void Dashboard_Load(object sender, EventArgs e)
        {
            rtDtpInicio.MaximumSize = new Size(200, 30);
            rtDtpFin.MaximumSize = new Size(200, 30);
            DateTime fi = rtDtpInicio.Value.Date;
            DateTime ff = rtDtpFin.Value.Date.AddDays(1).AddTicks(-1);           
        }

        public Dashboard()
        {
            InitializeComponent(); 
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = primaryBg;

            FechaInicio = DateTime.Now.AddMonths(-1);
            FechaFin = DateTime.Now;

            CrearLayoutRuntime();
            ConfigurarChartsRuntime();
            ActualizarDashboard(FechaInicio, FechaFin);
        }

        private void CrearLayoutRuntime()
        {
            // layout principal para toda la ventana
            rtMainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = primaryBg,
                RowCount = 2, // Fila superior para controles, fila inferior para gráficos
                Margin = new Padding(10)
            };
            rtMainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); // Fila para filtros
            rtMainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Fila para gráficos
            this.Controls.Add(rtMainLayout);

            // ==== PANEL SUPERIOR (filtros y botones) ====
            TableLayoutPanel rtTopLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = primaryBg,
                ColumnCount = 6
            };
            rtTopLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            rtTopLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
            rtTopLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            rtTopLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
            rtTopLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            rtTopLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            rtMainLayout.Controls.Add(rtTopLayout, 0, 0);

            // Controles para el panel superior
            Label lblDesde = new Label
            {
                Text = "Desde:",
                ForeColor = Color.White,
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(0, 5, 0, 0),
                AutoSize = true
            };
            rtDtpInicio = new DateTimePicker
            {
                Value = FechaInicio,
                Width = 150,
                Height = 25,
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
            };
            Label lblHasta = new Label
            {
                Text = "Hasta:",
                ForeColor = Color.White,
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 5, 0, 0),
                AutoSize = true
            };
            rtDtpFin = new DateTimePicker
            {
                Value = FechaFin,
                Width = 150,
                Height = 25,
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
            };
            rtBtnActualizar = new Button
            {
                Text = "Actualizar",
                Width = 100,
                Height = 30,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(170, 110, 70),
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
            };
            rtBtnActualizar.Click += btnActualizar_Click;

            Button rtBtnCerrar = new Button
            {
                Text = "Cerrar",
                Width = 100,
                Height = 30,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(170, 110, 70),
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Margin = new Padding(5, 3, 0, 0)
            };
            rtBtnCerrar.Click += (s, e) => this.Close();

            // Se agregan los controles al TableLayoutPanel superior
            rtTopLayout.Controls.Add(lblDesde, 0, 0);
            rtTopLayout.Controls.Add(rtDtpInicio, 1, 0);
            rtTopLayout.Controls.Add(lblHasta, 2, 0);
            rtTopLayout.Controls.Add(rtDtpFin, 3, 0);
            rtTopLayout.Controls.Add(rtBtnActualizar, 4, 0);
            rtTopLayout.Controls.Add(rtBtnCerrar, 5, 0);

            // ==== LAYOUT PARA LOS GRÁFICOS (simétricos) ====
            TableLayoutPanel rtChartsLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = primaryBg,
                RowCount = 2,
                ColumnCount = 3
            };
            rtChartsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            rtChartsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            rtChartsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            rtChartsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            rtChartsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));

            // Agregamos los gráficos al nuevo layout simétrico
            rtChartPedidos = new Chart { Dock = DockStyle.Fill, Margin = new Padding(5) };
            rtChartIngresos = new Chart { Dock = DockStyle.Fill, Margin = new Padding(5) };
            rtChartVendedores = new Chart { Dock = DockStyle.Fill, Margin = new Padding(5) };
            rtChartCobros = new Chart { Dock = DockStyle.Fill, Margin = new Padding(5) };
            rtChartProductosVendidos = new Chart { Dock = DockStyle.Fill, Margin = new Padding(5) };
            rtChartRentabilidad = new Chart { Dock = DockStyle.Fill, Margin = new Padding(5) };

            rtChartsLayout.Controls.Add(rtChartPedidos, 0, 0);
            rtChartsLayout.Controls.Add(rtChartIngresos, 1, 0);
            rtChartsLayout.Controls.Add(rtChartVendedores, 2, 0);
            rtChartsLayout.Controls.Add(rtChartCobros, 0, 1);
            rtChartsLayout.Controls.Add(rtChartProductosVendidos, 1, 1);
            rtChartsLayout.Controls.Add(rtChartRentabilidad, 2, 1);

            rtMainLayout.Controls.Add(rtChartsLayout, 0, 1);
        }

        private void AplicarEstiloBaseChartRuntime(Chart chart)
        {
            chart.ChartAreas.Clear();
            chart.Series.Clear();
            chart.Legends.Clear();

            var ca = new ChartArea();
            chart.ChartAreas.Add(ca);

            var legend = new Legend
            {
                Docking = Docking.Bottom,
                Alignment = StringAlignment.Center,
                BackColor = primaryBg,
                ForeColor = textColor,
                Font = new Font("Segoe UI", 9)
            };
            chart.Legends.Add(legend);

            chart.BackColor = primaryBg;
            ca.BackColor = secondaryBg;
            ca.AxisX.LineColor = textColor;
            ca.AxisY.LineColor = textColor;
            ca.AxisX.LabelStyle.ForeColor = textColor;
            ca.AxisY.LabelStyle.ForeColor = textColor;
            ca.AxisX.MajorGrid.LineColor = Color.FromArgb(200, 200, 200);
            ca.AxisY.MajorGrid.LineColor = Color.FromArgb(200, 200, 200);
        }

        private void ConfigurarChartsRuntime()
        {
            // Pedidos
            AplicarEstiloBaseChartRuntime(rtChartPedidos);
            rtChartPedidos.Titles.Add("Total de Pedidos por Día");
            rtChartPedidos.Titles[0].ForeColor = textColor;
            rtChartPedidos.Titles[0].Font = new Font("Segoe UI", 11, FontStyle.Bold);
            rtChartPedidos.Series.Add("Pedidos");
            rtChartPedidos.Series["Pedidos"].ChartType = SeriesChartType.Column;
            rtChartPedidos.Series["Pedidos"].Color = seriesColors[0];

            // Ingresos
            AplicarEstiloBaseChartRuntime(rtChartIngresos);
            rtChartIngresos.Titles.Add("Ingresos y Pedidos por Día");
            rtChartIngresos.Titles[0].ForeColor = textColor;
            rtChartIngresos.Titles[0].Font = new Font("Segoe UI", 11, FontStyle.Bold);
            rtChartIngresos.Series.Add("Ingresos");
            rtChartIngresos.Series["Ingresos"].ChartType = SeriesChartType.Column;
            rtChartIngresos.Series["Ingresos"].Color = seriesColors[1];
            rtChartIngresos.Series.Add("Pedidos");
            rtChartIngresos.Series["Pedidos"].ChartType = SeriesChartType.Line;
            rtChartIngresos.Series["Pedidos"].YAxisType = AxisType.Secondary;
            rtChartIngresos.Series["Pedidos"].BorderWidth = 3;
            rtChartIngresos.Series["Pedidos"].Color = seriesColors[3];

            // Vendedores
            AplicarEstiloBaseChartRuntime(rtChartVendedores);
            rtChartVendedores.Titles.Add("Ingresos por Vendedor");
            rtChartVendedores.Titles[0].ForeColor = textColor;
            rtChartVendedores.Titles[0].Font = new Font("Segoe UI", 11, FontStyle.Bold);
            rtChartVendedores.Series.Add("Vendedores");
            rtChartVendedores.Series["Vendedores"].ChartType = SeriesChartType.Bar;
            rtChartVendedores.Series["Vendedores"].Color = seriesColors[2];

            // Cobros
            AplicarEstiloBaseChartRuntime(rtChartCobros);
            rtChartCobros.Titles.Add("Órdenes Cobradas vs No Cobradas");
            rtChartCobros.Titles[0].ForeColor = textColor;
            rtChartCobros.Titles[0].Font = new Font("Segoe UI", 11, FontStyle.Bold);
            rtChartCobros.Series.Add("Cobros");
            rtChartCobros.Series["Cobros"].ChartType = SeriesChartType.Doughnut;
            rtChartCobros.Series["Cobros"].IsValueShownAsLabel = true;

            // Productos
            AplicarEstiloBaseChartRuntime(rtChartProductosVendidos);
            rtChartProductosVendidos.Titles.Add("Top 5 Productos Más Vendidos");
            rtChartProductosVendidos.Titles[0].ForeColor = textColor;
            rtChartProductosVendidos.Titles[0].Font = new Font("Segoe UI", 11, FontStyle.Bold);
            rtChartProductosVendidos.Series.Add("Productos");
            rtChartProductosVendidos.Series["Productos"].ChartType = SeriesChartType.Pie;
            rtChartProductosVendidos.Series["Productos"].IsValueShownAsLabel = true;
            rtChartProductosVendidos.Series["Productos"].LabelForeColor = textColor;

            // Rentabilidad
            AplicarEstiloBaseChartRuntime(rtChartRentabilidad);
            rtChartRentabilidad.Titles.Add("Rentabilidad por Producto");
            rtChartRentabilidad.Titles[0].ForeColor = textColor;
            rtChartRentabilidad.Titles[0].Font = new Font("Segoe UI", 11, FontStyle.Bold);
            rtChartRentabilidad.Series.Add("Rentabilidad");
            rtChartRentabilidad.Series["Rentabilidad"].ChartType = SeriesChartType.Bar; // Cambiamos a barras
            rtChartRentabilidad.Series["Rentabilidad"].IsValueShownAsLabel = true;
            rtChartRentabilidad.Series["Rentabilidad"].LabelFormat = "P1"; // Formato de porcentaje con un decimal
        }

        private void ActualizarDashboard(DateTime fechainicio, DateTime fechafin)
        {
            OrdenDePedidoBLL bllPedido = new OrdenDePedidoBLL();
            ProductoBLL bllProducto = new ProductoBLL();
            VendedorBLL bllVendedor = new VendedorBLL();
            List<Vendedor> ListaVendedores = bllVendedor.BuscarTodos(false);

            ListaOrdenesPedidos = bllPedido.ObtenerTodos();
            ListaProductos = bllProducto.ObtenerTodosLosProductos();

            var pedidosFiltrados = ListaOrdenesPedidos
                .Where(p => DateTime.TryParse(p.FechaDeVenta, out DateTime fecha) &&
                            fecha >= fechainicio && fecha <= fechafin)
                .ToList();

            // Pedidos por día
            var pedidosPorDia = pedidosFiltrados
                .GroupBy(p => DateTime.Parse(p.FechaDeVenta).Date)
                .Select(g => new { Fecha = g.Key, Cant = g.Count() })
                .ToList();

            rtChartPedidos.Series["Pedidos"].Points.Clear();
            foreach (var pd in pedidosPorDia)
                rtChartPedidos.Series["Pedidos"].Points.AddXY(pd.Fecha.ToShortDateString(), pd.Cant);

            // Ingresos y pedidos
            var ingresosPorDia = pedidosFiltrados
                .GroupBy(p => DateTime.Parse(p.FechaDeVenta).Date)
                .Select(g => new { Fecha = g.Key, Total = g.Sum(p => p.Total) })
                .ToList();

            rtChartIngresos.Series["Ingresos"].Points.Clear();
            rtChartIngresos.Series["Pedidos"].Points.Clear();
            foreach (var fecha in pedidosPorDia.Select(x => x.Fecha).Union(ingresosPorDia.Select(x => x.Fecha)))
            {
                var ingreso = ingresosPorDia.FirstOrDefault(i => i.Fecha == fecha)?.Total ?? 0;
                var cant = pedidosPorDia.FirstOrDefault(i => i.Fecha == fecha)?.Cant ?? 0;
                rtChartIngresos.Series["Ingresos"].Points.AddXY(fecha.ToShortDateString(), ingreso);
                rtChartIngresos.Series["Pedidos"].Points.AddXY(fecha.ToShortDateString(), cant);
            }

            // Ingresos por vendedor
            var ingresosPorVendedor = pedidosFiltrados
                .GroupBy(p => p.DNI_Vendedor) // g.Key es int
                .Select(g =>
                {
                    // Comparación directa entre dos enteros: v.DNI (int) y g.Key (int)
                    var vendedor = ListaVendedores.FirstOrDefault(v => v.DNI == g.Key);

                    string nombreVendedor = vendedor != null
                        ? $"{vendedor.Nombre} {vendedor.Apellido}"
                        : $"ID Desconocido ({g.Key})";

                    return new { Vendedor = nombreVendedor, Total = g.Sum(p => p.Total) };
                })
                .OrderByDescending(x => x.Total)
                .ToList();

            rtChartVendedores.Series["Vendedores"].Points.Clear();
            foreach (var v in ingresosPorVendedor)
                rtChartVendedores.Series["Vendedores"].Points.AddXY(v.Vendedor, v.Total);

            // Cobros
            int cobrados = pedidosFiltrados.Count(p => p.Cobrada);
            int noCobrados = pedidosFiltrados.Count(p => !p.Cobrada);

            rtChartCobros.Series["Cobros"].Points.Clear();
            rtChartCobros.Series["Cobros"].Points.AddXY("Cobrados", cobrados);
            rtChartCobros.Series["Cobros"].Points.AddXY("No Cobrados", noCobrados);

            // Productos más vendidos
            Dictionary<int, int> productosVendidos = new Dictionary<int, int>();
            foreach (var pedido in pedidosFiltrados)
            {
                var completo = bllPedido.BuscarCompleto(pedido.Id);
                if (completo?.Detalles != null)
                {
                    foreach (var d in completo.Detalles)
                    {
                        if (!productosVendidos.ContainsKey(d.IdProducto))
                            productosVendidos[d.IdProducto] = 0;
                        productosVendidos[d.IdProducto] += d.Cantidad;
                    }
                }
            }

            var top = productosVendidos
                .OrderByDescending(p => p.Value)
                .Take(5)
                .Select(p => new
                {
                    Nombre = ListaProductos.FirstOrDefault(x => x.ID == p.Key)?.Nombre ?? "Desconocido",
                    Cantidad = p.Value
                }).ToList();

            rtChartProductosVendidos.Series["Productos"].Points.Clear();
            foreach (var prod in top)
                rtChartProductosVendidos.Series["Productos"].Points.AddXY(prod.Nombre, prod.Cantidad);

            // Rentabilidad por producto
            var rentabilidadPorProducto = new Dictionary<string, decimal>();

            foreach (var producto in ListaProductos)
            {
                decimal ingresosPorProducto = 0;
                decimal costosPorProducto = 0;

                // Obtener todos los detalles de pedidos de este producto
                var detallesProducto = pedidosFiltrados
                    .SelectMany(p => bllPedido.BuscarCompleto(p.Id)?.Detalles ?? new List<DetalleOrdenDePedido>())
                    .Where(d => d.IdProducto == producto.ID)
                    .ToList();

                foreach (var detalle in detallesProducto)
                {
                    ingresosPorProducto += detalle.Cantidad * detalle.PrecioUnitario;
                    costosPorProducto += detalle.Cantidad * producto.Costo; 
                }

                if (ingresosPorProducto > 0)
                {
                    decimal rentabilidad = (ingresosPorProducto - costosPorProducto) / ingresosPorProducto;
                    rentabilidadPorProducto[producto.Nombre] = rentabilidad;
                }
            }

            // Llenar el gráfico de rentabilidad
            rtChartRentabilidad.Series["Rentabilidad"].Points.Clear();
            foreach (var item in rentabilidadPorProducto.OrderByDescending(x => x.Value).Take(5)) // Mostramos el top 5
            {
                rtChartRentabilidad.Series["Rentabilidad"].Points.AddXY(item.Key, item.Value);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            DateTime fi = rtDtpInicio.Value.Date;
            DateTime ff = rtDtpFin.Value.Date.AddDays(1).AddTicks(-1);
            ActualizarDashboard(fi, ff);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}


