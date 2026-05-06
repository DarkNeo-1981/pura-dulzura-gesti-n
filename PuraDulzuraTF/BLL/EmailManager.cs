using ENTITY; 
using SERVICIOS;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace BLL
{
    public class EmailManager
    {        
        public void EnviarReciboPorEmail(ConfiguracionMailDTO config, ReciboDeSueldo recibo, string rutaPDF, string emailEmpleado)
        {
            string host = config.Host; 
            int port = config.Puerto; 
            string senderEmail = config.EmailRemitente;
            bool enableSsl = config.UsaSSL;
            
            string senderPassword;
            try
            {
                // Verifica si existe la contraseña encriptada. Si está vacía, no hay credencial para desencriptar.
                if (string.IsNullOrEmpty(config.Password)) 
                {
                    throw new Exception("La contraseña no está configurada o se encuentra vacía. Por favor, guarde la configuración de correo.");
                }                
                senderPassword = Encriptacion.Desencriptar(config.Password); 
            }
            catch (Exception ex)
            {
                throw new Exception($"Error de seguridad al obtener la contraseña: {ex.Message}");
            }

            // VALIDAR QUE EXISTA EL PDF
            if (!File.Exists(rutaPDF))
                throw new FileNotFoundException($"No se encontró el archivo PDF en la ruta: {rutaPDF}");

            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(senderEmail, "Pura Dulzura | Sistema de Nómina");
                    mail.To.Add(emailEmpleado);
                    mail.Subject = $"Recibo de Sueldo - Período {recibo.Periodo:MM/yyyy}";
                    mail.Body = $@"
                        Estimado/a empleado/a,<br><br>
                        Adjuntamos su recibo de sueldo correspondiente al período 
                        <strong>{recibo.Periodo:MMMM yyyy}</strong>.<br><br>
                        Saludos cordiales,<br>
                        <b>El equipo de Administración</b>.
                    ";
                    mail.IsBodyHtml = true;
                    mail.Attachments.Add(new Attachment(rutaPDF));

                    using (SmtpClient smtp = new SmtpClient(host, port)) 
                    {
                        // contraseña DESENCRIPTADA
                        smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                        smtp.EnableSsl = enableSsl; 
                        smtp.Timeout = 15000;
                        smtp.Send(mail);
                    }
                }

                // ELIMINAR ARCHIVO TEMPORAL DESPUÉS DEL ENVÍO EXITOSO
                if (File.Exists(rutaPDF))
                    File.Delete(rutaPDF);
            }
            catch (SmtpException smtpEx)
            {
                if (File.Exists(rutaPDF))
                    File.Delete(rutaPDF);

                // mensaje de error para el usuario final
                throw new Exception($"Error de conexión o credenciales SMTP: {smtpEx.Message}\nVerifique la configuración de Host, Puerto y la Contraseña de Aplicación.");
            }
            catch (Exception ex)
            {
                if (File.Exists(rutaPDF))
                    File.Delete(rutaPDF);

                throw new Exception($"Error al enviar el correo al empleado {recibo.DNI_Empleado} ({emailEmpleado}): {ex.Message}");
            }
        }
    }
}
