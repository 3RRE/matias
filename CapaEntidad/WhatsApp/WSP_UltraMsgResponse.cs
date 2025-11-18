namespace CapaEntidad.WhatsApp {
    public class WSP_UltraMsgResponse {
        public string sent { get; set; } = string.Empty;
        public string message { get; set; } = string.Empty;
        public int id { get; set; }
        public string error { get; set; } = string.Empty;
    }
}
