using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ClienteSatisfaccion.DTO {
    public class NpsMensualDTO {
        public int Anio { get; set; }
        public int Mes { get; set; }
        public string PeriodoMes { get; set; }
        public int TotalRespuestas { get; set; }
        public int NPS { get; set; }
    }
}
