using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class ClientesPtkEntidad
    {
        public int ClienteID { get; set; }
        public string Cliente { get; set; }
    }

    public class Cliente
    {
        public int ClienteID { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteApelPat { get; set; }
        public string ClienteApelMat { get; set; }
        public string ClienteTipoDoc { get; set; }
        public string ClienteNroDoc { get; set; }
        public DateTime FechaReg { get; set; }
        public DateTime FechaAct { get; set; }
        public string NroCuenta { get; set; }
        public string BancoNombre { get; set; }
        public string NroCuentaAnterior { get; set; }
        public string BancoNombreAnterior { get; set; }
    }

}
