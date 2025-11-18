using System.Collections.Generic;

namespace CapaEntidad.GLPI {
    public class GLPI_SeguimientoTicket : GLPI_BaseClass {
        public int IdTicket { get; set; }
        public int IdUsuarioRegistra { get; set; }
        public int IdEstadoTicketAnterior { get; set; }
        public int IdEstadoTicketActual { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int IdProcesoAnterior { get; set; }
        public int IdProcesoActual { get; set; }
        public string Correos { get; set; } = string.Empty;
        public List<string> Destinatarios { get; set; } = new List<string>();
    }
}
