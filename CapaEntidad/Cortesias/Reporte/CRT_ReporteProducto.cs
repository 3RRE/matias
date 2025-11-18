namespace CapaEntidad.Cortesias.Reporte {
    public class CRT_ReporteProducto {
        public int CodSala { get; set; }
        public string Sala { get; set; } = string.Empty;
        public int IdTipo { get; set; }
        public string NombreTipo { get; set; } = string.Empty;
        public int IdSubTipo { get; set; }
        public string NombreSubTipo { get; set; } = string.Empty;
        public int IdMarca { get; set; }
        public string NombreMarca { get; set; } = string.Empty;
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }

        public bool Existe() {
            return IdProducto > 0;
        }
    }
}
