
using System;

namespace CapaEntidad.Notificaciones.Entity.Mantenedores {
    public class CredencialCorreo : NotificacionBaseClassMantenedor {
        public int IdSistema { get; set; } 
        public string NombreRemitente { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string ClaveSMTP { get; set; } = string.Empty;
        public string ServidorSMTP { get; set; } = string.Empty;
        public int PuertoSMTP { get; set; }
        public bool SSLHabilitado { get; set; } = true;
        public int CuotaDiaria { get; set; } = 1000;
        public int Prioridad { get; set; }
        public bool Estado { get; set; } = true;

    }
}
