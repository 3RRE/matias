using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Campañas
{
    public class CMP_impresora_usuarioEntidad
    {
        public Int64 id { get; set; }
        public Int64 usuario_id { get; set; }
        public string UsuarioNombre { get; set; }
        public Int64 impresora_id { get; set; }
        public string nombre { get; set; }
        public string ip { get; set; }
    
    }
}
