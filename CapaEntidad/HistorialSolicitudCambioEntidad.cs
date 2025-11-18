using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class HistorialSolicitudCambioEntidad
    {
        public int HistorialId { set; get; }
        public int SolicitudId { set; get; }
        public int EstadoSolicitudId { set; get; }
        public DateTime FechaRegistro { set; get; }
        public int AprobadorPor { set; get; }
        public DateTime FechaRespuesta { set; get; }
        public string Observacion { set; get; }
        public string EstadoNuevo { set; get; }
    }
}
