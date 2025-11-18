using System;

namespace CapaEntidad.ClienteSala {
    public class ClienteSalaEntidad {
        public int CodClie { get; set; }
        public string NomCli { get; set; } = string.Empty;
        public string Apeclie { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Nacionalidad { get; set; } = string.Empty;
        public string Sexo { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public int CodSala { get; set; }
        public int CodEmpresa { get; set; }
        public string Telefono { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public bool EnviaNotificacionWhatsapp { get; set; }
        public bool EnviaNotificacionSms { get; set; }
        public bool EnviaNotificacionEmail { get; set; }
        public bool LlamadaCelular { get; set; }
    }
}
