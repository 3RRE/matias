using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Usuarios
    {
        public int CodU { get; set; }

        public string Nombre { get; set; }

        public string CU { get; set; }

        public int? Estado { get; set; }

        public string CUcierreCaja { get; set; }

        public string nivel { get; set; }

        public int? Sis { get; set; }

        public int? codPer { get; set; }

        public string Usu_nameexterno { get; set; }

        public bool? IsBloqueado { get; set; }

        public int? ContPassFallado { get; set; }

    }
}
