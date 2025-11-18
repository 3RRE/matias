namespace CapaEntidad.Cortesias.Reporte {
    public class CRT_ReportePedido {
        public int CodSala { get; set; }
        public string Sala { get; set; } = string.Empty;
        public int IdPedido { get; set; }
        public string Productos { get; set; } = string.Empty;
        public string CodMaquina { get; set; } = string.Empty;
        public int Zona { get; set; }
        public string NombreZona { get; set; } = string.Empty;
        public int Posicion { get; set; }
        public int Isla { get; set; }
        public string NombreIsla { get; set; } = string.Empty;
        public string Anfitriona { get; set; } = string.Empty;
        public string NumeroDocumentoCliente { get; set; } = string.Empty;
        public string NombreCliente { get; set; } = string.Empty;

        public bool Existe() {
            return IdPedido > 0;
        }
    }
}
