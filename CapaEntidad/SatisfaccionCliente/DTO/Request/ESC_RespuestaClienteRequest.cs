using System.Collections.Generic;

namespace CapaEntidad.SatisfaccionCliente.DTO.Request {
    public class ESC_RespuestaClienteRequest {
        public ESC_SalaRequest Sala { get; set; } = new ESC_SalaRequest();
        public ESC_ClienteRequest Cliente { get; set; } = new ESC_ClienteRequest();
        public List<ESC_RespuestaRequest> Respuestas { get; set; } = new List<ESC_RespuestaRequest>();
    }
}
