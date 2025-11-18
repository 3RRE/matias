using System;

namespace CapaEntidad.Notificaciones.DTO.Mantenedores {
    public class SistemaDto {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public DateTime? FechaModificacion { get; set; }
        public string FechaRegistroStr { get => FechaRegistro.ToString("dd/MM/yyyy HH:mm:ss"); }
        public string FechaModificacionStr { get => FechaModificacion.HasValue ? FechaModificacion.Value.ToString("dd/MM/yyyy HH:mm:ss") : "N/A"; }

        public bool Existe() {
            return Id > 0;
        }
    }
}
