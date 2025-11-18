using System;

namespace CapaEntidad.BOT.Entities {
    public class BOT_ContactoEntidad {
        public int Id { get; set; }
        public int IdCargo { get; set; }
        public string NombreArea { get; set; } = string.Empty;
        public string NombreCargo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string CodigoPaisCelular { get; set; } = string.Empty;
        public string Celular { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }

        public bool Existe() {
            return Id > 0;
        }

        public string ObtenerCelularCompleto() {
            return $"{CodigoPaisCelular}{Celular}".Trim();
        }
    }
}
