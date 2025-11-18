using System.Collections.Generic;
using System.Data;

namespace CapaEntidad.Excel {
    public class HojaExcelReporteAgrupado {
        public DataTable Data { get; set; } = new DataTable();
        public string Nombre { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public List<string> MetaData { get; set; } = new List<string>();
    }
}
