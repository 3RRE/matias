using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaEntidad.AsistenciaEmpleado
{
    public class EmpleadoDispositivo
    {
        public int emd_id { get; set; }
        public string emd_imei { get; set; }
        public int emp_id { get; set; }
        public int emd_estado { get; set; }
        public string emd_firebaseid { get; set; }
        
    }
}