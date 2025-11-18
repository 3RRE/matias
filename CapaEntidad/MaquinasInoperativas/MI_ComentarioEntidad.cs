using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.MaquinasInoperativas {
    public class MI_ComentarioEntidad {

        public int CodComentario { get; set; }
        public string Texto { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int CodUsuario { get; set; }
        public int CodMaquinaInoperativa { get; set; }

        public int EstadoProceso { get; set; }
        public int Estado { get; set; }
        public int CorreoEnviado { get; set; }
         
        public string NombreCompleto;
        public string Nombres;
        public string ApellidosPaterno;
        public string ApellidosMaterno;

    }
}
