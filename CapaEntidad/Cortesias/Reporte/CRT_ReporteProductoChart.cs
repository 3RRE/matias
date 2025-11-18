using System.Collections.Generic;

namespace CapaEntidad.Cortesias.Reporte {
    public class CRT_ReporteProductoChart {
        public List<string> Productos { get; set; }
        public List<CRT_ReporteProductoDataSet> DataSets { get; set; } = new List<CRT_ReporteProductoDataSet>();
    }

    public class CRT_ReporteProductoDataSet {
        public string Sala { get; set; }
        public List<int> Cantidades { get; set;} =new List<int>();
    }
}
