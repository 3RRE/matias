using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ClienteSatisfaccion.DTO {
    public class PreguntaIndicadorDTO {
        public int IdPregunta { get; set; }
        public string Pregunta { get; set; }
        public string Indicador { get; set; }
        public int Orden { get; set; }
        public bool Multi { get; set; }
    }
}
