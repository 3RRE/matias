using System;

namespace CapaEntidad.Cortesias {
    public class CRT_Producto {
        public int Id { get; set; }
        public int IdSubTipo { get; set; }
        public int IdMarca { get; set; }
        public string Nombre { get; set; }
        public string ImagenUrl { get; set; }
        public int IdUsuario { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }

        public string NombreSubTipo { get; set; }
        public string NombreTipo { get; set; }
        public string NombreMarca { get; set; }
        public int IdTipo { get; set; }
        public string ImagenBase64 { get; set; }

        public bool Existe() {
            return Id > 0;
        }
    }


    public class CRT_ProductoSala {
        public int Id { get; set; }
        public int IdSubTipo { get; set; }
        public int IdMarca { get; set; }
        public string Nombre { get; set; }
        public string ImagenUrl { get; set; }
        public int IdUsuario { get; set; }
        public bool Estado { get; set; }
        public int Cantidad { get; set; }
        public int CantidadPendiente { get; set; }
        public decimal Precio { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }


        public string NombreSubTipo { get; set; }
        public string NombreTipo { get; set; }
        public string NombreMarca { get; set; }
        public bool isChecked { get; set; }

        public bool Existe() {
            return Id > 0;
        }

    }

}
