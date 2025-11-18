using CapaEntidad.GLPI.Enum;
using System;
using System.Collections.Generic;

namespace CapaEntidad.GLPI.Reporte {
    public class GLPI_ReporteFiltro {
        public List<int> CodsSalas { get; set; } = new List<int>();
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public List<GLPI_FaseTicket> FasesTicket { get; set; } = new List<GLPI_FaseTicket>();
    }
}
