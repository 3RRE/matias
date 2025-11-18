using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class SolicitudCambioEntidad
    {
        public int SolicitudId { set; get; }
        public string SistemaDescripcion { set; get; }
        public int ModuloId { set; get; }
        public string ModuloDescripcion { set; get; }
        public int TipoCambioId { set; get; }
        public string TipoCambioDescripcion { set; get; }
        public int ImpactoId { set; get; }
        public string ImpactoDescripcion { set; get; }
        public int SolicitanteId { set; get; }
        public string SolicitanteDescripcion { set; get; }
        public int EstadoSolicitudCambioId { set; get; }
        public string EstadoSolicitudCambioDescripcion { set; get; }
        public int SalaId { set; get; }
        public string SalaDescripcion { set; get; }
        public string Descripcion { set; get; }
        public DateTime FechaRegistro { get; set; }
        public string Observacion { get; set; }
    }
}
