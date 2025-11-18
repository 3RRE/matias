using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.MantenimientoBD
{
    public class DatabaseInformacionEntidad
    {
        public string database_name { get; set; }
        public double log_size_mb { get; set; }
        public double row_size_mb { get; set; }
        public double total_size_mb { get; set; }
    }
}
