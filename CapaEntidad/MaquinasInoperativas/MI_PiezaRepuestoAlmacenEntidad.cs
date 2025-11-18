using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.MaquinasInoperativas {
    public class MI_PiezaRepuestoAlmacenEntidad {

        public int CodPiezaRepuestoAlmacen { get; set; }
        public int CodPiezaRepuesto { get; set; }
        public int CodTipo { get; set; }
        public int CodAlmacen { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int Estado { get; set; }


        public string NombrePiezaRepuesto { get; set; }
        public string NombreAlmacen { get; set; }
        public string NombreSala { get; set; }
        public string NombreTipo { get; set; }
        public int CodSala { get; set; }
        public int CodUsuario { get; set; }

    }
}
