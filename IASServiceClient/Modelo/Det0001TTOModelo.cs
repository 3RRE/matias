using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class Det0001TTOModelo
    {
        public string RazonSocial { get; set; }
        public string Sala { get; set; }
        public string Item { get; set; } 
        public string Tito_NroTicket { get; set ;}
        public string Tito_NroTicket_Ant { get; set ;}
        public string Tito_MontoTicket { get; set ;}
        public string Tito_MontoTicket_Ant { get; set ;}
        public string Tito_MTicket_NoCobrable { get; set ;}
        public string Tito_MTicket_NoCobrable_Ant { get; set ;}
        public string Estado { get; set ;}
        public string Estado_Ant { get; set ;} //0 ANULADO   ,   1 COBRABLE
        public string PuntoVenta { get; set ;}
        public string PuntoVenta_Ant { get; set ;}
    }
}
