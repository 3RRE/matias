using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ClienteSatisfaccion.DTO {
    public class PreguntaDTO {
        public int IdPregunta { get; set; }
        public string Texto { get; set; }
        public int Orden { get; set; }
        public bool Activo { get; set; }
        public List<OpcionDTO> Opciones { get; set; } = new List<OpcionDTO>();
    }

    public class OpcionDTO {
        public int IdOpcion { get; set; }
        public string Texto { get; set; }
        public FlujoPreguntaDTO Flujo { get; set; }
    }

    public class FlujoPreguntaDTO {
        public int IdFlujo { get; set; }
        public int IdPreguntaActual { get; set; }
        public int IdOpcion { get; set; }
        public int? IdPreguntaSiguiente { get; set; }
        public bool EsFinalDeBloque { get; set; }
    }

}
