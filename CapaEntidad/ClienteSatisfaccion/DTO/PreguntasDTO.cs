using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ClienteSatisfaccion {

    public class EncuestaDTO{
        public List<PreguntasDTO> Preguntas { get; set; }
        public List<FlujoDTO> Flujo { get; set; }

    }
    public class PreguntasDTO {
        public int IdPregunta { get; set; }
        public int IdTipoEncuesta { get; set; }
        public string Texto { get; set; }
        public string Indicador { get; set; }
        public int Orden { get; set; }
        public bool Random { get; set; }
        public bool Multi { get; set; }
        public bool Activo { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public List<OpcionesDTO> Opciones { get; set; }
    }

    public class OpcionesDTO {
        public int idOpcion { get; set; }
        public int idPregunta { get; set; }
        public string Texto { get; set; }
        public bool TieneComentario { get; set; }
    }


    public class FlujoDTO {
        public int IdFlujo { get; set; }
        public int IdTipoEncuesta { get; set; }
        public int IdPreguntaActual { get; set; }
        public int IdOpcion { get; set; }
        public int IdPreguntaSiguiente { get; set; }
    }
}
