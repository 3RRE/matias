using System;

namespace CapaEntidad.WhatsApp {
    public class WSP_MensajeEnviadoEntidad {
        public int IdMensajeEnviado { get; set; }
        public int IdContacto { get; set; }
        public string CodigoMensaje { get; set; }
        public string Desde { get; set; }
        public string Hacia { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaEnvio { get; set; }
        public bool Estado { get; set; }
        public string NombreDestinatarioContacto { get; set; }
        public string NumeroContacto { get; set; }
        public string CodigoPaisContacto { get; set; }
    }
}
