using CapaEntidad.SatisfaccionCliente.Enum;

namespace CapaEntidad.SatisfaccionCliente.Entity {
    public class ESC_Respuesta : ESC_BaseClass {
        public int IdRespuestaSala { get; set; }
        public int IdPregunta { get; set; }
        public ESC_Puntaje Puntaje { get; set; }
        public string NumeroDocumento { get; set; }
    }
}
