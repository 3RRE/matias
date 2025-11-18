using CapaEntidad.GLPI.DTO.Mantenedores;
using System;

namespace CapaEntidad.GLPI.DTO.Global {
    public class GLPI_AsignacionDto {
        public int Id { get; set; }
        public GLPI_UsuarioDto UsuarioAsigna { get; set; } = new GLPI_UsuarioDto();
        public GLPI_EstadoTicketDto EstadoTicket { get; set; } = new GLPI_EstadoTicketDto();
        public GLPI_UsuarioDto UsuarioAsignado { get; set; } = new GLPI_UsuarioDto();
        public DateTime FechaTentativaTermino { get; set; }
        public string FechaTentativaTerminoStr { get => FechaTentativaTermino.ToString("dd/MM/yyyy HH:mm:ss"); }
        public string Correos { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public string FechaRegistroStr { get => FechaRegistro.ToString("dd/MM/yyyy HH:mm:ss"); }

        public bool Existe() {
            return Id > 0;
        }
    }
}
