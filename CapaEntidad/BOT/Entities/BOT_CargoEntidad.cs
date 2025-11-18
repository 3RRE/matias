using System;

namespace CapaEntidad.BOT.Entities {
    public class BOT_CargoEntidad {
        public int Id { get; set; }
        public int IdArea { get; set; }
        public string NombreArea { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }

        public BOT_AreaEntidad Area { get; set; } = new BOT_AreaEntidad();

        public bool Existe() {
            return Id > 0;
        }
    }
}
