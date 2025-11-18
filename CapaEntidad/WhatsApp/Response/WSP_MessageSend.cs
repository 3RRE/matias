using System.Collections.Generic;

namespace CapaEntidad.WhatsApp.Response {
    public class WSP_MultipleMessageResponse {
        public string Message { get; set; } = string.Empty;
        public List<WSP_MultipleMessageSala> Casinos { get; set; } = new List<WSP_MultipleMessageSala>();
    }

    public class WSP_MultipleMessageSala {
        public int CodSala { get; set; }
        public List<WSP_UltraMsgResponse> MessagesSend { get; set; } = new List<WSP_UltraMsgResponse>();
    }
}
