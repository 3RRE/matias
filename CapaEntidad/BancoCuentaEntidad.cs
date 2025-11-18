using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class BancoCuentaEntidad
    {
        public Int64 BancoCuentaID { get; set; }
        public string Banco { get; set; }
        public int ClienteID { get; set; }
        public string NroCuenta { get; set; }
    }
}
