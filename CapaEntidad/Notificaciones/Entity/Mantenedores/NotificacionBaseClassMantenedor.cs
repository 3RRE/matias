using System;

namespace CapaEntidad.Notificaciones.Entity.Mantenedores {
    public class NotificacionBaseClassMantenedor {
        public int Id { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FehaModificacion { get; set; }
        public bool Existe() {
            return Id > 0;
        }
    }
}
