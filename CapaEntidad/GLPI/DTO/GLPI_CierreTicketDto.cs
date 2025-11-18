using CapaEntidad.GLPI.DTO.Global;

namespace CapaEntidad.GLPI.DTO {
    public class GLPI_CierreTicketDto : GLPI_CierreDto {
        public GLPI_TicketDto Ticket { get; set; } = new GLPI_TicketDto();
    }
}
