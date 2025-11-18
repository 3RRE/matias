using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.MaquinasInoperativas {
    public class MI_MaquinaInoperativaPiezasEntidad {
        public int CodMaquinaInoperativaPiezas { get; set; }
        public int CodMaquinaInoperativa { get; set; }
        public int CodPieza { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string CodUsuario { get; set; }
        public int Estado { get; set; }
        public string NombrePieza { get; set; }
        public string DescripcionPieza { get; set; }

    }
}
