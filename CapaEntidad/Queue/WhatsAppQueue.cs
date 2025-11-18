namespace CapaEntidad.Queue {
    public class WhatsAppQueue {
        public int IdSistema { get; set; }
        public WhatsAppQueueContentRequest Mensaje { get; set; } = new WhatsAppQueueContentRequest();
    }

    public class WhatsAppQueueContentRequest {
        public string CodigoPais { get; set; } = string.Empty;
        public string NumeroCelular { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
    }
}
