using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad {
    public class EVT_DispositivoEntidad {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int Estado { get; set; }
        public string Navegador { get; set; }
        public bool EstadoPing { get; set; }
    }
}
