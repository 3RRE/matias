using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ClienteSatisfaccion.Entidad {
    public class OpcionEntidad {
        public int IdOpcion { get; set; }
        public int IdPregunta { get; set; }
        public string Texto { get; set; }
        public bool TieneComentario { get; set; }
        public int? Valor { get; set; }

    }
}
