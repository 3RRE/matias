namespace CapaEntidad.WhatsApp {
    public class WSP_UltraMsgRequest {
        public string token { get; set; } = string.Empty;
        public string to { get; set; } = string.Empty;
        public string image { get; set; } = string.Empty;
        public string body { get; set; } = string.Empty;
        public string caption { get; set; } = string.Empty;
        public string priority { get; set; } = string.Empty;
        public string referenceId { get; set; } = string.Empty;
        public string nocache { get; set; } = string.Empty;
        public string msgId { get; set; } = string.Empty;
        public string mentions { get; set; } = string.Empty;
    }
}
