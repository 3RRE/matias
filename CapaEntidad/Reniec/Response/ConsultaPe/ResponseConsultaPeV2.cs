namespace CapaEntidad.Reniec.Response.ConsultaPe {
    public class ResponseConsultaPeV2<T> where T : new() {
        public string type { get; set; } = string.Empty;
        public int status { get; set; }
        public string message { get; set; } = "El DNI no existe o pertenece a un menor de edad.";
        public T data { get; set; } = new T();
    }
}