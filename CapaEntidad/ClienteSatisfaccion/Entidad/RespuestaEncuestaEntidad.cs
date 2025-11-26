using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ClienteSatisfaccion.Entidad {
    public class RespuestaEncuestaEntidad {
        public int IdRespuestaEncuesta { get; set; }
        public int IdSala { get; set; }
        public int IdTablet{ get; set; }
        public string NroDocumento{ get; set; }
        public int TipoDocumento { get; set; }
        public DateTime FechaRespuesta { get; set; }
        public int IdTipoEncuesta { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Celular { get; set; }
        public string Codigo { get; set; }


    }
}
