using CapaEntidad.GLPI.DTO.Global;

namespace CapaEntidad.GLPI.DTO {
    public class GLPI_AsignacionTicketDto : GLPI_AsignacionDto {
        public GLPI_TicketDto Ticket { get; set; } = new GLPI_TicketDto();
    }
}
