using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.MaquinasInoperativas {
    public class MI_AlmacenEntidad {
        public int CodAlmacen { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int CodSala { get; set; }
        public string NombreSala { get; set; }
        public int Estado { get; set; }
    }
}
