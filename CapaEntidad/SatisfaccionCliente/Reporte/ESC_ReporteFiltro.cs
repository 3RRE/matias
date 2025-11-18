using System;
using System.Collections.Generic;

namespace CapaEntidad.SatisfaccionCliente.Reporte {
    public class ESC_ReporteFiltro {
        public List<int> CodsSala { get; set; } = new List<int>();
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public bool TieneSalas() {
            return CodsSala.Count > 0;
        }
    }
}
