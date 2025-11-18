namespace CapaEntidad.Reniec.Response.ApisPeru {
    public class ResponseApisPeru {
        public bool success { get; set; }
        public string dni { get; set; } = string.Empty;
        public string nombres { get; set; } = string.Empty;
        public string apellidoPaterno { get; set; } = string.Empty;
        public string apellidoMaterno { get; set; } = string.Empty;
        public int codVerifica { get; set; }
        public string codVerificaLetra { get; set; } = string.Empty;
    }
}