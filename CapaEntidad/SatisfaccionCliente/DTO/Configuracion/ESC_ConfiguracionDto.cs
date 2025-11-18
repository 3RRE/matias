using CapaEntidad.SatisfaccionCliente.DTO.Mantenedores;
using CapaEntidad.SatisfaccionCliente.Enum;

namespace CapaEntidad.SatisfaccionCliente.DTO.Configuracion {
    public class ESC_ConfiguracionDto {
        public int Id { get; set; }
        public ESC_SalaDto Sala { get; set; } = new ESC_SalaDto();
        public ESC_TipoValidacionEnvioRespuesta TipoValidacionEnvioRespuesta { get; set; }
        public int TiempoEsperaRespuesta { get; set; }
        public string MensajeTiempoEsperaRespuesta { get; set; } = string.Empty;
        public int EnvioMaximoDiario { get; set; }
        public string MensajeEnvioMaximoDiario { get; set; } = string.Empty;
        public int EnvioMaximoMensual { get; set; }
        public string MensajeEnvioMaximoMensual { get; set; } = string.Empty;
        public bool RespuestasAnonimas { get; set; }
        public bool EncuestaActiva { get; set; }

        public bool Existe() {
            return Id > 0;
        }
    }
}
