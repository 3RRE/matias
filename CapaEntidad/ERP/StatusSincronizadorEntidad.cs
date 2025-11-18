using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ERP {
    public class StatusEnvio {
        public DateTime fechaoperacion { get; set; }
        public int contadores { get; set; }
        public int cuadreticket { get; set; }
        public int caja { get; set; }
        public int enviado { get; set; }
        public DateTime fechacierre { get; set; }

    }
    public class StatusSincroTablaComparacion
    {
        public string nameTable { get; set; }
        public int countData { get; set; }
        public int countTable { get; set; }

    }
}
