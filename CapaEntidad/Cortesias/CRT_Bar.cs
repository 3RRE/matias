using System;

namespace CapaEntidad.Cortesias {
    public class CRT_Bar {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }

        public bool Existe() {
            return Id > 0;
        }
    }
}
