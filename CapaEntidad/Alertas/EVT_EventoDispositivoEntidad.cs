using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Alertas
{
    public class EVT_EventoDispositivoEntidad
    {
        public int DispositivoId { get; set; }
        public int EventoId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string DispositivoNombre { get; set; }
        public string Usuario { get; set; }
    }
}
