using System;

namespace CapaEntidad.GLPI {
    public class GLPI_BaseClass {
        public int Id { get; set; }
        public DateTime FechaRegistro { get; set; }

        public bool Existe() {
            return Id > 0;
        }
    }
}
