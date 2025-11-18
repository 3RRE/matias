using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class DepositoEntidad
    {
        public int DepositoID { get; set; }
        public int DepositoSala { get; set; }
        public int Codsala { get; set; }
        public string nombresala { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteApelPat { get; set; }
        public string ClienteApelMat { get; set; }
        public string TipoDocNombre { get; set; }
        public string ClienteNroDoc { get; set; }
        public double Monto { get; set; }
        public string NroTickets { get; set; }
        public string NroOperacion { get; set; }
        public DateTime FechaReg { get; set; }
        public DateTime FechaAct { get; set; }
        public int Estado { get; set; }
        public List<TicketEntidad> Tickets { get; set; }
        //data para nro de cuenta
        public string BancoNombre { get; set; }
        public string NroCuenta { get; set; }
        public string FechaRegString { get; set; }

        public string UsuarioNombreReg { get; set; }
    }

}
