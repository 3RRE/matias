using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ClienteSatisfaccion.Entidad {
    public class RespuestaPreguntaEntidad {
        public int IdRespuestaPregunta { get; set; }
        public int IdRespuestaEncuesta { get; set; }
        public int IdPregunta { get; set; }
        public int IdOpcion { get; set; }
        public string Comentario { get; set; }
    }
}
