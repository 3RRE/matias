using System;

namespace CapaEntidad.SatisfaccionCliente.Entity.Mantenedores {
    public class ESC_BaseClassMantenedor {
        public int Id { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public bool Existe() {
            return Id > 0;
        }
    }
}
