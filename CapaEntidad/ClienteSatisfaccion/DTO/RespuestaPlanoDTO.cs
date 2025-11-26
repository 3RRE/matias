using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ClienteSatisfaccion.DTO {
    public class RespuestaPlanoDTO {
        public int IdRespuestaEncuesta { get; set; }
        public string NroDocumento { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Celular { get; set; }
        public string Correo { get; set; }
        public DateTime FechaRespuesta { get; set; }
        public int IdTablet { get; set; }
        public string NombreTablet { get; set; }
        public int IdPregunta { get; set; }
        public string Pregunta { get; set; }
        public string Indicador { get; set; }
        public bool Multi { get; set; }
        public string TextoOpcion { get; set; }
        public int? ValorOpcion { get; set; }
        public bool TieneComentario { get; set; }
        public string Comentario { get; set; }
    }
}
