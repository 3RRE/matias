namespace CapaPresentacion.Utilitarios {
    public class ApiReniecResponse {
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Nombre { get; set; }
        public string NombreCompleto { get; set; }
        public string DNI { get; set; }
        public string ErrorMensaje { get; set; }
        public bool Respuesta { get; set; }
        public string FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public string EstadoCivil { get; set; }
        public string Direccion { get; set; }
        public string Ubigeo { get; set; }
        public string Distrito { get; set; }
        public string Provincia { get; set; }
        public string Departamento { get; set; }
        public string Restrinccion { get; set; }
        public string Foto { get; set; }
        public API_USADA API_USADA { get; set; }
        public ApiReniecResponse() {
            this.ApellidoPaterno = string.Empty;
            this.ApellidoMaterno = string.Empty;
            this.Nombre = string.Empty;
            this.NombreCompleto = string.Empty;
            this.DNI = string.Empty;
            this.ErrorMensaje = string.Empty;
            this.Respuesta = false;
            this.FechaNacimiento = string.Empty;
            this.Genero = string.Empty;
            this.EstadoCivil = string.Empty;
            this.Direccion = string.Empty;
            this.Ubigeo = string.Empty;
            this.Distrito = string.Empty;
            this.Provincia = string.Empty;
            this.Departamento = string.Empty;
            this.Restrinccion = string.Empty;
            this.Foto = string.Empty;
            this.API_USADA = API_USADA.NINGUNA;
        }
    }

    public enum API_USADA { NINGUNA, APIS_PERU, CONSULTA_PE, CONSULTA_PE_V2, API_DNI }
}