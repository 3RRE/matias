using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ClienteSatisfaccion.DTO {
    public class NpsRespuestaDTO {
        public int IdSala { get; set; }
        public DateTime FechaRespuesta { get; set; }
        public int IdTablet { get; set; }
        public string NombreTablet { get; set; }
        public int Valor { get; set; }
    }
}
