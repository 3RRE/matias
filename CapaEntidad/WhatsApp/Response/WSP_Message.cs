using System;

namespace CapaEntidad.WhatsApp.Response {
    public class WSP_Message {
        public string id { get; set; }
        public int idContact { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string message { get; set; }
        public bool state { get; set; }
        public bool hasImage { get; set; }
        public Int64 timestamp { get; set; }
    }
}
