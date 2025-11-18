using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ClienteSatisfaccion.DTO {
    public class IndicadorDiarioDTO {
        public DateTime Fecha { get; set; }
        public int TotalRespuestas { get; set; }

        // Cantidades
        public int CantMuyInsatisfecho { get; set; }
        public int CantInsatisfecho { get; set; }
        public int CantNeutral { get; set; }
        public int CantSatisfecho { get; set; }
        public int CantMuySatisfecho { get; set; }

        // Porcentajes (0..100, 2 decimales)
        public double PctMuyInsatisfecho { get; set; }
        public double PctInsatisfecho { get; set; }
        public double PctNeutral { get; set; }
        public double PctSatisfecho { get; set; }
        public double PctMuySatisfecho { get; set; }

        // Indicador (%MuySatisfecho - %MuyInsatisfecho)
        public int IndicadorValor { get; set; }
    }

}
