using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ClienteSatisfaccion.DTO {
    public class NpsResultadoDTO {
        // Cantidades
        public int TotalRespuestas { get; set; }
        public int CantDetractores { get; set; }
        public int CantPasivos { get; set; }
        public int CantPromotores { get; set; }

        // Porcentajes
        public double PctDetractores { get; set; }
        public double PctPasivos { get; set; }
        public double PctPromotores { get; set; }

        // Indicador
        public int NPS { get; set; }
    }

}
