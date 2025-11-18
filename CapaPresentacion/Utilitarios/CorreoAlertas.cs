using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Web;
using System.IO;
using CapaNegocio.Alertas;

namespace CapaPresentacion.Utilitarios
{

    public class CorreoAlertas {
        private ALT_AlertaSalaBL alertaSalaBL = new ALT_AlertaSalaBL();
        private SmtpClient cliente;
        private MailMessage email;
        private string _HOST = ConfigurationManager.AppSettings["host"];
        private string _PORT = ConfigurationManager.AppSettings["port"];
        private string _ENABLESSL = ConfigurationManager.AppSettings["enableSSL"];
        private string _USER;
        private string _PASWWORD;

        public CorreoAlertas(string user, string password) {
                _USER = user;
                _PASWWORD = password;

            cliente = new SmtpClient(_HOST, Int32.Parse(_PORT)) {
                EnableSsl = Boolean.Parse(_ENABLESSL),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_USER, _PASWWORD)
            };
        }
        public void EnviarCorreo(string destinatario, string asunto, string mensaje, bool esHtlm = false, List<string> adjuntos = null) {
            email = new MailMessage(_USER, destinatario, asunto, mensaje);
            email.IsBodyHtml = esHtlm;
            email.BodyEncoding = System.Text.Encoding.UTF8;
            email.SubjectEncoding = System.Text.Encoding.Default;
            if(adjuntos != null) {
                foreach(var adjunto in adjuntos) {
                    Attachment attachment = new Attachment(adjunto, MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.FileName = Path.GetFileName(adjunto);
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    email.Attachments.Add(attachment);
                    //email.Attachments.Add(new Attachment(adjunto));
                }
            }
            cliente.Send(email);
            alertaSalaBL.AgregarContador();

        }
        public void EnviarCorreo(MailMessage message) {
            cliente.Send(message);
        }

    }
}