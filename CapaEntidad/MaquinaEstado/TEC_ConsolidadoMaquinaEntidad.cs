using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.MaquinaEstado {
    public class TEC_ConsolidadoMaquinaEntidad {

        public int SalaId { get; set; }
        public string Sala { get; set; }
        public int TotalConectadas { get; set; }
        public int TotalDesconectadas { get; set; }
        public int TotalMaquinas { get; set; }
       // public DateTime FechaRegistro { get; set; }
    }
}
