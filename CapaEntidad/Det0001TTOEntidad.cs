using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Det0001TTOEntidad
    {
        public int Item { get; set; }
        public string Tito_NroTicket { get; set; }
        public double Tito_MontoTicket { get; set; }
        public double Tito_MTicket_NoCobrable { get; set; }
        public int Estado { get; set; }
        public int IdTipoMoneda { get; set; }
        public int IdTipoPago { get; set; }
        public DateTime FechaReg { get; set; }
        public string tipo_ticket { get; set; }
        public string PuntoVenta { get; set; }
        public string PuntoVentaMin { get; set; }
        public int Bloqueo { get; set; }
        public string Binario { get; set; }
        public string impreso { get; set; }
    }
}
