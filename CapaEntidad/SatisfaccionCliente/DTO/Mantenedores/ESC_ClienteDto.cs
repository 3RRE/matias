namespace CapaEntidad.SatisfaccionCliente.DTO.Mantenedores {
    public class ESC_ClienteDto {
        public int Id { get; set; }
        public string NumeroDocumento { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string ApellidoMaterno { get; set; } = string.Empty;
        public string NombreCompleto { get => ObtenerNombreCompleto(); }

        private string ObtenerNombreCompleto() {
            string nombreCompleto = $"{Nombres} {ApellidoPaterno} {ApellidoMaterno}".Trim();
            return string.IsNullOrWhiteSpace(nombreCompleto) ? "-" : nombreCompleto;
        }
    }
}
