using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.MaquinasInoperativas {
    public class MI_RepuestoEntidad {
        public int CodRepuesto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public float CostoReferencial { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string CodUsuario { get; set; }
        public int CodCategoriaRepuesto { get; set; }
        public string NombreCategoriaRepuesto { get; set; }
        public int Estado { get; set; }
    }
}
