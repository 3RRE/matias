using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.MaquinasInoperativas {
    public class MI_PiezaEntidad {
        public int CodPieza { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string CodUsuario { get; set; }
        public int CodCategoriaPieza { get; set; }
        public string NombreCategoriaPieza { get; set; }
        public int Estado { get; set; }
    }
}
