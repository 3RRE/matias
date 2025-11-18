using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.MaquinasInoperativas {
    public class MI_MaquinaInoperativaProblemasEntidad {
        public int CodMaquinaInoperativaProblemas { get; set; }
        public int CodMaquinaInoperativa { get; set; }
        public int CodProblema { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string CodUsuario { get; set; }
        public int Estado { get; set; }
        public string NombreProblema { get; set; }
        public string DescripcionProblema { get; set; }
        //plus
        public int CodCategoriaProblema { get; set; }
        public string NombreRepuesto { get; set; }
    }
}
