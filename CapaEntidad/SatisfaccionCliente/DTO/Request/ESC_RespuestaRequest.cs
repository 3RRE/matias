using CapaEntidad.SatisfaccionCliente.Enum;

namespace CapaEntidad.SatisfaccionCliente.DTO.Request {
    public class ESC_RespuestaRequest {
        public int IdRespuestaSala { get; set; }
        public int IdPregunta { get; set; }
        public ESC_Puntaje Puntaje { get; set; }
    }
}
