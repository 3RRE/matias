using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class SEG_PermisoEntidad
    {
        public int WEB_PermID { get; set; }
        public string WEB_PermNombre { get; set; }
        public string WEB_PermNombreR { get; set; }
        public string WEB_PermTipo { get; set; }
        public string WEB_PermControlador { get; set; }
        public string WEB_PermDescripcion { get; set; }
        public string WEB_ModuloNombre { get; set; }
        public string WEB_PermEstado { get; set; }
        public DateTime WEB_PermFechaRegistro { get; set; }
    }
}
