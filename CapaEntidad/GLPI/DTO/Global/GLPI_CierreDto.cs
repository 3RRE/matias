using CapaEntidad.GLPI.DTO.Mantenedores;
using System;

namespace CapaEntidad.GLPI.DTO.Global {
    public class GLPI_CierreDto {
        public int Id { get; set; }
        public GLPI_UsuarioDto UsuarioCierra { get; set; } = new GLPI_UsuarioDto();
        public GLPI_EstadoTicketDto EstadoAnterior { get; set; } = new GLPI_EstadoTicketDto();
        public GLPI_EstadoTicketDto EstadoActual { get; set; } = new GLPI_EstadoTicketDto();
        public string Descripcion { get; set; } = string.Empty;
        public GLPI_UsuarioDto UsuarioConfirma { get; set; } = new GLPI_UsuarioDto();
        public DateTime FechaRegistro { get; set; }
        public string FechaRegistroStr { get => FechaRegistro.ToString("dd/MM/yyyy HH:mm:ss"); }

        public bool Existe() {
            return Id > 0;
        }
    }
}
