using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ClienteSatisfaccion.DTO {
    public class CsatMensualDTO {
        public int Anio { get; set; }
        public int Mes { get; set; }
        public string PeriodoMes { get; set; }

        public int TotalRespuestas { get; set; }

        // Cantidades
        public int CantMuyInsatisfecho { get; set; }
        public int CantInsatisfecho { get; set; }
        public int CantNeutral { get; set; }
        public int CantSatisfecho { get; set; }
        public int CantMuySatisfecho { get; set; }

        // Porcentajes
        public double MuyInsatisfecho { get; set; }
        public double Insatisfecho { get; set; }
        public double Neutral { get; set; }
        public double Satisfecho { get; set; }
        public double MuySatisfecho { get; set; }

        // Indicador
        public double CSAT { get; set; }
    }
}
