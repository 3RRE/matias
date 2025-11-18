using System.Collections.Generic;

namespace CapaEntidad.Reportes
{
    public class ALEV_ReporteNominalEntidad
    {
        public int SalaId { get; set; }
        public string Sala { get; set; }
        public int Tipo { get; set; }
        public List<ALEV_LogNominalEntidad> Logs { get; set; }
    }

    public class ALEV_LogNominalEntidad
    {
        public int SalaId { get; set; }
        public string Fecha { get; set; }
        public int Total { get; set; }
    }

    public class ALEV_SalaNominalEntidad
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
    }

    public class ALEV_TipoReporteNominalEntidad
    {
        public int Tipo { get; set; }
        public string Nombre { get; set; }
        public List<ALEV_ReporteNominalEntidad> Salas { get; set; }
    }
}
