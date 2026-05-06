using ENTITY;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.Properties;

namespace UI
{
    public partial class ConfiguracionMail : Form
    {
        private List<MailProvider> _providers;
        private const string MANUAL_OPTION = "Otro/Corporativo (Manual)";
        Menu Principal;
        public ConfiguracionMail()
        {
            InitializeComponent();            
            txtContraseña.UseSystemPasswordChar = true;
        }

        private void ConfiguracionMail_Load(object sender, EventArgs e)
        {
            Principal = (Menu)this.MdiParent;

            // 1. Inicializar la lista de proveedores
            _providers = new List<MailProvider>
            {
                new MailProvider { Name = "Gmail", Host = "smtp.gmail.com", Ports = new List<int> { 587, 465 }, DefaultSsl = true },
                new MailProvider { Name = "Yahoo", Host = "smtp.mail.yahoo.com", Ports = new List<int> { 587, 465 }, DefaultSsl = true },
                new MailProvider { Name = "Outlook/Hotmail", Host = "smtp-mail.outlook.com", Ports = new List<int> { 587 }, DefaultSsl = true },
                new MailProvider { Name = MANUAL_OPTION, Host = "", Ports = new List<int> { 587, 465, 25 }, DefaultSsl = true }
            };

            // 2. Llenar ComboBox de Proveedores y SSL
            cmbProveedor.DataSource = _providers;
            cmbSSL.Items.Add(true);
            cmbSSL.Items.Add(false);
            cmbSSL.SelectedIndex = 0; // Por defecto True

            // 3. Establecer el evento de cambio
            cmbProveedor.SelectedIndexChanged += cmbProveedor_SelectedIndexChanged;

            // 4. Cargar configuraciones guardadas
            // Se ejecuta la carga al final para que la lógica de selección del proveedor
            // dispare cmbProveedor_SelectedIndexChanged y luego podamos SOBREESCRIBIR
            // el Host si es la opción manual.
            CargarConfiguracionGuardada();
        }

        private void cmbProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            MailProvider selectedProvider = cmbProveedor.SelectedItem as MailProvider;

            if (selectedProvider == null) return;

            bool isManual = selectedProvider.Name == MANUAL_OPTION;

            // Lógica para el Host
            if (!isManual)
            {
                txtHost.Text = selectedProvider.Host;
            }
            // Importante: Si es manual, se deja el valor que tenga (ya sea el guardado o vacío).
            txtHost.ReadOnly = !isManual; // Solo editable si es la opción manual
            txtHost.BackColor = isManual ? System.Drawing.SystemColors.Window : System.Drawing.SystemColors.Control; // Pista visual

            // Lógica para los Puertos
            cmbPort.Items.Clear();
            foreach (int port in selectedProvider.Ports)
            {
                cmbPort.Items.Add(port);
            }
            if (cmbPort.Items.Count > 0)
            {
                cmbPort.SelectedIndex = 0;
            }

            // Lógica para SSL
            cmbSSL.SelectedItem = selectedProvider.DefaultSsl;
        }

        private void CargarConfiguracionGuardada()
        {
            // 1. Cargar Email y Host guardado
            txtEmail.Text = Settings.Default.SmtpEmail;
            string savedHost = Settings.Default.SmtpHost;

            // 2. Intentar seleccionar el Proveedor
            var providerToSelect = _providers.FirstOrDefault(p => p.Host.Equals(savedHost, StringComparison.OrdinalIgnoreCase));

            if (providerToSelect != null)
            {
                // Si es un proveedor predefinido, lo seleccionamos. Esto dispara cmbProveedor_SelectedIndexChanged.
                cmbProveedor.SelectedItem = providerToSelect;
            }
            else if (!string.IsNullOrEmpty(savedHost))
            {
                // Si el host guardado no es uno de los predefinidos, seleccionamos 'Manual'.
                // Esto DISPARA cmbProveedor_SelectedIndexChanged, el cual vacía txtHost si es manual.
                cmbProveedor.SelectedItem = _providers.FirstOrDefault(p => p.Name == MANUAL_OPTION);

                // CORRECCIÓN: Volvemos a cargar el Host guardado DESPUÉS de que
                // cmbProveedor_SelectedIndexChanged ha configurado los campos para la opción manual.
                txtHost.Text = savedHost;
            }

            // 3. Cargar Puerto y SSL
            int savedPort = Settings.Default.SmtpPort;
            bool savedSsl = Settings.Default.SmtpEnableSsl;

            // Si hay un puerto guardado y existe en la lista de puertos actual, lo seleccionamos.
            if (savedPort > 0 && cmbPort.Items.Contains(savedPort))
            {
                cmbPort.SelectedItem = savedPort;
            }

            if (cmbSSL.Items.Contains(savedSsl))
            {
                cmbSSL.SelectedItem = savedSsl;
            }

            // 4. La contraseña se deja vacía por seguridad.
            txtContraseña.Text = string.Empty;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // --- 1. VALIDACIONES BÁSICAS ---
            if (string.IsNullOrWhiteSpace(txtHost.Text) ||
                cmbPort.SelectedItem == null ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                cmbSSL.SelectedItem == null)
            {
                MessageBox.Show("Por favor, complete todos los campos obligatorios (Host, Puerto, Email, SSL).", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(cmbPort.SelectedItem.ToString(), out int portValue))
            {
                MessageBox.Show("El puerto seleccionado no es un número válido.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validar Contraseña: Requerida si se cambia o si NUNCA se ha guardado.
            if (string.IsNullOrEmpty(txtContraseña.Text) && string.IsNullOrEmpty(Settings.Default.SmtpPasswordEncrypted))
            {
                MessageBox.Show("Debe ingresar la Contraseña o 'App Password' para el correo.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // --- 2. PERSISTENCIA DE LA CONFIGURACIÓN ---

            // Guardar valores no sensibles
            Settings.Default.SmtpHost = txtHost.Text.Trim();
            Settings.Default.SmtpPort = portValue;
            Settings.Default.SmtpEmail = txtEmail.Text.Trim();
            Settings.Default.SmtpEnableSsl = (bool)cmbSSL.SelectedItem;

            // Lógica para la Contraseña (SOLO se guarda si el usuario escribió algo)
            if (!string.IsNullOrEmpty(txtContraseña.Text))
            {
                try
                {
                    // SE USA LA CLASE DE ENCRIPTACIÓN
                    string encryptedPass = Encriptacion.Encriptar(txtContraseña.Text);
                    Settings.Default.SmtpPasswordEncrypted = encryptedPass;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al encriptar la contraseña: {ex.Message}", "Error de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // --- 3. GUARDAR EN DISCO ---
            Settings.Default.Save();
            MessageBox.Show("Configuración de correo guardada con éxito.", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Principal.AgregarALaBitacora("Se ha realizado la configuracion del e-mail");
            this.Close();
        }
    }
}
