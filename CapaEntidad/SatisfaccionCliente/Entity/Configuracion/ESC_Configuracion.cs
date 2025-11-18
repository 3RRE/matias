using CapaEntidad.SatisfaccionCliente.Enum;

namespace CapaEntidad.SatisfaccionCliente.Entity.Configuracion {
    public class ESC_Configuracion : ESC_BaseClass {
        public int CodSala { get; set; }
        public ESC_TipoValidacionEnvioRespuesta TipoValidacionEnvioRespuesta { get; set; }
        public int TiempoEsperaRespuesta { get; set; } //en minutos
        public string MensajeTiempoEsperaRespuesta { get; set; } = string.Empty;
        public int EnvioMaximoDiario { get; set; }
        public string MensajeEnvioMaximoDiario { get; set; } = string.Empty;
        public int EnvioMaximoMensual { get; set; }
        public string MensajeEnvioMaximoMensual { get; set; } = string.Empty;
        public bool RespuestasAnonimas { get; set; }
        public bool EncuestaActiva { get; set; }
    }
}
