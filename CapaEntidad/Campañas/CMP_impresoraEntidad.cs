using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Campañas
{
    public class CMP_impresoraEntidad
    {
        public Int64 id { get; set; }
        public Int64 sala_id { get; set; }
        public string sala_nombre { get; set; }
        public string nombre { get; set; }
        public string ip { get; set; }
        public string puerto { get; set; }
        public Int32 estado { get; set; }
    }
}
