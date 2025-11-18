using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.MaquinasInoperativas {
    public class MI_MaquinaInoperativaRepuestosEntidad {
        public int CodMaquinaInoperativaRepuestos { get; set; }
        public int CodMaquinaInoperativa { get; set; }
        public int CodRepuesto { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string CodUsuario { get; set; }
        public int Estado { get; set; }
        public string NombreRepuesto { get; set; }
    }
}
