using System;

namespace CapaEntidad.BOT.Entities {
    public class BOT_AreaEntidad {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }

        public bool Existe() {
            return Id > 0;
        }
    }
}
