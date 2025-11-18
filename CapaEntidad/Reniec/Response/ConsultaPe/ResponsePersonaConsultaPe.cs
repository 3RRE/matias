namespace CapaEntidad.Reniec.Response.ConsultaPe {
    public class ResponsePersonaConsultaPe {
        public string dni { get; set; } = string.Empty;
        public string nombres { get; set; } = string.Empty;
        public string apellido_paterno { get; set; } = string.Empty;
        public string apellido_materno { get; set; } = string.Empty;
        public string caracter_verificacion { get; set; } = string.Empty;
        public string caracter_verificacion_anterior { get; set; } = string.Empty;

        public string nombre_completo { get => $"{nombres} {apellido_paterno} {apellido_materno}"; }

        public bool Existe() {
            return !string.IsNullOrEmpty(dni);
        }
    }
}