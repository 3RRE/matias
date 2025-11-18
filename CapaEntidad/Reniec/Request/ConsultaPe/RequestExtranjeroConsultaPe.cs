using System;

namespace CapaEntidad.Reniec.Request.ConsultaPe {
    public class RequestExtranjeroConsultaPe {
        public string NumeroDocumento { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
    }
}
