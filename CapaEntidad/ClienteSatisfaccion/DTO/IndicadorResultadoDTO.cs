using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ClienteSatisfaccion.DTO {
    public class IndicadorResultadoDTO {
        public string Indicador { get; set; }

        public int TotalRespuestas { get; set; }

        // Cantidades por nivel
        public int CantMuyInsatisfecho { get; set; }
        public int CantInsatisfecho { get; set; }
        public int CantNeutral { get; set; }
        public int CantSatisfecho { get; set; }
        public int CantMuySatisfecho { get; set; }

        // Porcentajes por nivel (0..100 con 2 decimales)
        public double PctMuyInsatisfecho { get; set; }
        public double PctInsatisfecho { get; set; }
        public double PctNeutral { get; set; }
        public double PctSatisfecho { get; set; }
        public double PctMuySatisfecho { get; set; }

        // Indicador (MuySatisfecho% - MuyInsatisfecho%) redondeado a entero
        public int IndicadorValor { get; set; }
    }


    public class IndicadorRespuesta {
        public int IdSala { get; set; }
        public DateTime FechaRespuesta { get; set; }
        public int IdTablet { get; set; }
        public string NombreTablet { get; set; }
        public int Valor { get; set; }
        public string Indicador { get; set; }
        public string Pregunta { get; set; }
    }

}
