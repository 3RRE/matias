namespace CapaEntidad.Reniec.Response.ConsultaPe {
    public class ResponseExtranjeroConsultaPe {
        public string numero_de_documento { get; set; }
        public string calidad_migratoria { get; set; }
        public string nombres { get; set; }
        public string nacionalidad { get; set; }
        public string apellido_paterno { get; set; }
        public string apellido_materno { get; set; }
        public string fecha_nacimiento { get; set; }
        public string fecha_expiracion_residencia { get; set; }
        public string fecha_expiracion_carnet { get; set; }
        public string fecha_ultima_emision_carnet { get; set; }

        public string nombre_completo { get => $"{nombres} {apellido_paterno} {apellido_materno}"; }

        public bool Existe() {
            return !string.IsNullOrEmpty(numero_de_documento);
        }
    }
}