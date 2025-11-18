using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ClienteSatisfaccion.Entidad {
    public class PreguntaEntidad {
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
    }
}
