using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace CapaPresentacion.Utilitarios
{
    public class Correo
    {
        private SmtpClient cliente;  
        private MailMessage email;
        private string _HOST = ConfigurationManager.AppSettings["host"];
        private string _PORT = ConfigurationManager.AppSettings["port"];
        private string _ENABLESSL = ConfigurationManager.AppSettings["enableSSL"];
        private string _USER = ConfigurationManager.AppSettings["correo"];
        private string _PASWWORD = ConfigurationManager.AppSettings["password"];

        public Correo()
        {
            
            cliente = new SmtpClient(_HOST, Int32.Parse(_PORT))
            {
                EnableSsl = Boolean.Parse(_ENABLESSL),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_USER, _PASWWORD)
            };
        }     
        public void EnviarCorreo(string destinatario, string asunto, string mensaje, bool esHtlm = false, List<string> adjuntos=null)
        {
            email = new MailMessage(_USER, destinatario, asunto, mensaje);
            email.IsBodyHtml = esHtlm;
            email.BodyEncoding = System.Text.Encoding.UTF8;
            email.SubjectEncoding = System.Text.Encoding.Default;
            if (adjuntos!=null)
            {
                foreach(var adjunto in adjuntos)
                {
                    Attachment attachment = new Attachment(adjunto, MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.FileName = Path.GetFileName(adjunto);
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    email.Attachments.Add(attachment);
                    //email.Attachments.Add(new Attachment(adjunto));
                }
            }
            cliente.Send(email);
        }
        public void EnviarCorreo(MailMessage message)
        {
            cliente.Send(message);
        }
      
    }
}