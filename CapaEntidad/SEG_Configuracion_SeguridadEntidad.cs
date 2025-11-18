using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class SEG_Configuracion_SeguridadEntidad
    {
        public int codWebConfiguracionSeguridad { get; set; }
        public string linkInterno { get; set; }
        public string linkExterno { get; set; }
        public int cantidadLetraNombre { get; set; }
        public int cantidadLetraApePaterno { get; set; }
        public int cantidadLetraApeMaterno { get; set; }
        public int cantidadLetraDNI { get; set; }
        public int ordenNombre { get; set; }
        public int ordenApePaterno { get; set; }
        public int ordenApeMaterno { get; set; }
        public int ordenDNI { get; set; }
        public string mensajeEmail { get; set; }
    }
}
