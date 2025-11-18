using System.Collections.Generic;

namespace CapaEntidad.SatisfaccionCliente.Reporte {
    public class Esc_ReportePespuestasChart {
        public List<string> Puntajes { get; set; } = new List<string>();
        public List<ESC_ReporteRespuestasDataSet> DataSets { get; set; } = new List<ESC_ReporteRespuestasDataSet>();
    }

    public class ESC_ReporteRespuestasDataSet {
        public string Sala { get; set; }
        public List<int> Cantidades { get; set; } = new List<int>();
    }
}
