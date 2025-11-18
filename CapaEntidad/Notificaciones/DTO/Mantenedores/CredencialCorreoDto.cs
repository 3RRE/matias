
using System;

namespace CapaEntidad.Notificaciones.DTO.Mantenedores {
    public class CredencialCorreoDto {
        public int Id { get; set; }
        public int IdSistema { get; set; }
        public string NombreSistema { get; set; } = string.Empty; 
        public string NombreRemitente { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string ClaveSMTP { get; set; } = string.Empty;
        public string ServidorSMTP { get; set; } = string.Empty;
        public int PuertoSMTP { get; set; }
        public bool SSLHabilitado { get; set; } = true;
        public string SSLHabilitadoStr { get => SSLHabilitado ? "Sí" : "No"; }
        public int CuotaDiaria { get; set; } = 1000;
        public int Prioridad { get; set; }
        public bool Estado { get; set; } = true;
        public string EstadoStr { get => Estado ? "Activo" : "Inactivo"; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public string FechaRegistroStr { get => FechaRegistro.ToString("dd/MM/yyyy HH:mm:ss"); }
        public DateTime? FechaModificacion { get; set; }
        public string FechaModificacionStr { get => FechaModificacion.HasValue ? FechaModificacion.Value.ToString("dd/MM/yyyy HH:mm:ss") : "N/A"; }

        public bool Existe() {
            return Id > 0;
        }
    }
}
