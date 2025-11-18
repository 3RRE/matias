using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class TicketEntidad
    {
        public int TicketID { get; set; }
        public string NroTicketTito { get; set; }
        public int DepositoID { get; set; }
        public DateTime FechaReg { get; set; }
        public double Monto { get; set; }
        public int codSala { get; set; }
    }
}
