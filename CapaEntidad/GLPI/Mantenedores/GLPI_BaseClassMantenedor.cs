using System;

namespace CapaEntidad.GLPI.Mantenedores {
    public abstract class GLPI_BaseClassMantenedor {
        public int Id { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public bool Existe() {
            return Id > 0;
        }

        public string ObtenerValorSelect() {
            return Id.ToString();
        }

        public abstract string ObtenerIdPadreSelect();
        public abstract string ObtenerTextoSelect();
        //public abstract string ObtenerColorSelect();
        public abstract bool ObtenerEstadoSelect();

    }
}
