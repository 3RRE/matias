using System.Collections.Generic;
using System.Web;

namespace CapaEntidad.WhatsApp {
    public class WSP_MensajeMultipleEntidad {
        public string message { get; set; }
        public string ids { get; set; }
        public List<WSP_ClienteSala> clients { get; set; }
        public List<string> phones { get; set; }
        public HttpPostedFileBase image { get; set; }
        public bool withImage { get; set; }
    }

    public class WSP_ClienteSala {
        public int idCliente { get; set; }
        public int codSala { get; set; }
    }
}
