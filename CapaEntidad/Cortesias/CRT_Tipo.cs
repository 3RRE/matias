using System;

namespace CapaEntidad.Cortesias {
    public class CRT_Tipo {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ImagenUrl { get; set; }
        public int IdUsuario { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }

        public string ImagenBase64 { get; set; }

        public bool Existe() {
            return Id > 0;
        }
    }
}
