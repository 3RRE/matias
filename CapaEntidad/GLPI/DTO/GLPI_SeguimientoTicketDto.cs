using CapaEntidad.GLPI.DTO.Global;

namespace CapaEntidad.GLPI.DTO {
    public class GLPI_SeguimientoTicketDto : GLPI_SeguimientoDto {
        public GLPI_TicketDto Ticket { get; set; } = new GLPI_TicketDto();
    }
}
