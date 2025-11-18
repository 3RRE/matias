using System.Collections.Generic;
using System.IO;

namespace CapaEntidad.Queue {
    public class EmailQueue {
        public int IdSistema { get; set; }
        public EmailQueueContentRequest Mensaje { get; set; } = new EmailQueueContentRequest();
    }

    public class EmailQueueContentRequest {
        public string Asunto { get; set; } = string.Empty;
        public string Cuerpo { get; set; } = string.Empty;
        public bool EsHtml { get; set; }
        public List<string> Destinatarios { get; set; } = new List<string>();
        public List<string> DestinatariosConCopia { get; set; } = new List<string>();
        public List<string> DestinatariosConCopiaOculta { get; set; } = new List<string>();
        public List<EmailQueueAttachment> Adjuntos { get; set; } = new List<EmailQueueAttachment>();
    }

    public class EmailQueueAttachment {
        public MemoryStream Archivo { get; set; } = new MemoryStream();
        public string Nombre { get; set; } = string.Empty;
    }
}
