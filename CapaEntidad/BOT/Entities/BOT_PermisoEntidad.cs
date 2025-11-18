using CapaEntidad.BOT.Enum;
using S3k.Utilitario.Extensions;
using System;

namespace CapaEntidad.BOT.Entities {
    public class BOT_PermisoEntidad {
        public int Id { get; set; }
        public int IdEmpleado { get; set; }
        public int IdCargo { get; set; }
        public BOT_Acciones CodAccion { get; set; }
        public string Accion => CodAccion.GetDisplayText();
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }

        public bool Existe() {
            return Id > 0;
        }
    }
}
