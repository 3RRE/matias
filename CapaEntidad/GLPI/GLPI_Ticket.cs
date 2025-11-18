using CapaEntidad.GLPI.Enum;
using System;
using System.Collections.Generic;

namespace CapaEntidad.GLPI {
    public class GLPI_Ticket : GLPI_BaseClass {
        public int CodSala { get; set; }
        public int IdUsuarioSolicitante { get; set; }
        public int IdTipoOperacion { get; set; }
        public int IdNivelAtencion { get; set; }
        public int IdSubCategoria { get; set; }
        public int IdClasificacionProblema { get; set; }
        public int IdEstadoActual { get; set; }
        public int IdIdentificador { get; set; }    
        public string Descripcion { get; set; } = string.Empty;
        public string Adjunto { get; set; } = string.Empty;
        public string Correos { get; set; } = string.Empty;
        public GLPI_FaseTicket CodigoFaseTicket { get; set; }
        public string Destinatarios { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
    }
}
