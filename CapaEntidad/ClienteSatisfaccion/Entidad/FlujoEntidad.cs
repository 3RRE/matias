using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ClienteSatisfaccion.Entidad {
    public class FlujoEntidad {
        public int IdFlujo { get; set; }
        public int IdTipoEncuesta { get; set; }
        public int IdPreguntaActual { get; set; }
        public int IdOpcion { get; set; }
        public int IdPreguntaSiguiente { get; set; }
    }
}
