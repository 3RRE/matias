using CapaEntidad.SatisfaccionCliente.Enum;

namespace CapaEntidad.SatisfaccionCliente.Reporte {
    public class ESC_ReportePorcentaje {
        public ESC_Puntaje Puntaje { get; set; }
        public int TotalRespuestas { get; set; }
        public int CantidadRespuestas { get; set; }
        public double Porcentaje { get; set; }
    }
}
