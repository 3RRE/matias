namespace CapaEntidad.GLPI {
    public class GLPI_CierreTicket : GLPI_BaseClass {
        public int IdTicket { get; set; }
        public int IdUsuarioCierra { get; set; }
        public int IdEstadoTicketAnterior { get; set; }
        public int IdEstadoTicketActual { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int IdUsuarioConfirma { get; set; }
    }
}
