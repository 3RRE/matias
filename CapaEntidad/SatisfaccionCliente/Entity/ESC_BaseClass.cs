using System;

namespace CapaEntidad.SatisfaccionCliente.Entity {
    public class ESC_BaseClass {
        public int Id { get; set; }
        public DateTime FechaRegistro { get; set; }

        public bool Existe() {
            return Id > 0;
        }
    }
}
