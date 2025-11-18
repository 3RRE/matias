using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Campañas
{
    public class CMP_Campania_ParametrosEntidad
    {
        public Int64 id { get; set; }
        public int sala_id { get; set; }
        public string sala_nombre { get; set; }
        public int usuario_id { get; set; }
        public int condicion_juego { get; set; }
        public int condicion_tipo { get; set; }
        public DateTime fecha_reg { get; set; }
        
       
    }
}
