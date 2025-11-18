using CapaEntidad.GLPI.DTO.Mantenedores;
using System;

namespace CapaEntidad.GLPI.DTO.Global {
    public class GLPI_SeguimientoDto {
        public int Id { get; set; }
        public GLPI_UsuarioDto UsuarioRegistra { get; set; } = new GLPI_UsuarioDto();
        public GLPI_EstadoTicketDto EstadoTicketAnterior { get; set; } = new GLPI_EstadoTicketDto();
        public GLPI_EstadoTicketDto EstadoTicketActual { get; set; } = new GLPI_EstadoTicketDto();
        public string Descripcion { get; set; } = string.Empty;
        public GLPI_ProcesoDto ProcesoAnterior { get; set; } = new GLPI_ProcesoDto();
        public GLPI_ProcesoDto ProcesoActual { get; set; } = new GLPI_ProcesoDto();
        public string Correos { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public string FechaRegistroStr { get => FechaRegistro.ToString("dd/MM/yyyy HH:mm:ss"); }

        public bool Existe() {
            return Id > 0;
        }
    }
}
