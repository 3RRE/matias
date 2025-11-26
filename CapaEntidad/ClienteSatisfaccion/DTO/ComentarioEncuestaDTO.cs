using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ClienteSatisfaccion.DTO {
    public class EncuestadoDTO {
        public int IdRespuestaEncuesta { get; set; }
        public int IdSala { get; set; }
        public int IdTablet { get; set; }
        public string NroDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public DateTime FechaRespuesta { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Celular { get; set; }
        public string PreguntaNPS { get; set; }
        public int? ValorRespuesta { get; set; }
        public string Clasificacion { get; set; }

        // 👇 Atributos y comentarios adicionales
        public List<RespuestaAtributoDTO> Comentarios { get; set; } = new List<RespuestaAtributoDTO>();
    }


    public class RespuestaAtributoDTO {
        public int IdRespuestaEncuesta { get; set; }
        public int IdPregunta { get; set; }
        public string Pregunta { get; set; }
        public string Opcion { get; set; }
        public string Comentario { get; set; }
    }


    public class PreguntaIndicador {
        public string Pregunta { get; set; }
        public string Indicador { get; set; }
    }

    public class SubPreguntaNPS {
        public int IdPreguntaNps { get; set; }
        public int IdSubPregunta { get; set; }
        public int IdOpcion { get; set; }
        public string TextoOpcion { get; set; }
        public int ValorOpcion { get; set; }
    }
}
