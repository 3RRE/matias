using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Imei {
    public class ControlImeiEntidad {

        public int IdControlImei { get; set; }
        public int IdEmpleado { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreEmpleado { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Cargo { get; set; }
        public string Imei { get; set; }
        public int Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
}
}
