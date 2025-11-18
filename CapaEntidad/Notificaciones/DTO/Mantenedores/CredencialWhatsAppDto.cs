using System;

namespace CapaEntidad.Notificaciones.DTO.Mantenedores {
    public class CredencialWhatsAppDto {
        public int Id { get; set; }
        //public SistemaDto Sistema { get; set; } = new SistemaDto();

        public int IdSistema { get; set; }
        public string NombreSistema { get; set; } = string.Empty; 
        public string UrlBase { get; set; } = string.Empty;
        public string Instancia { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public string FechaRegistroStr { get => FechaRegistro.ToString("dd/MM/yyyy HH:mm:ss"); }
        public DateTime? FechaModificacion { get; set; }
        public string FechaModificacionStr { get => FechaModificacion.HasValue ? FechaModificacion.Value.ToString("dd/MM/yyyy HH:mm:ss") : "N/A"; }

        public bool Existe() {
            return Id > 0;
        }
    }
}
