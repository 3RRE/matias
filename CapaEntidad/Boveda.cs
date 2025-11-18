using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Boveda
    {
        public UInt64 BovedaID { get; set; }
        public decimal SaldoInicial { get; set; }
        public DateTime? FechaInicial { get; set; }
        public decimal SaldoFinal { get; set; }
        public decimal SaldoFinalM { get; set; }
        public DateTime? FechaFinal { get; set; }
        public decimal MontoCajonRechazo { get; set; }
        public decimal MontoCajonRechazoM { get; set; }
        public decimal RecargaCajon { get; set; }
        public decimal SalidaCajon { get; set; }
        public decimal SalidaCajonM { get; set; }
        public decimal StackerBilletes { get; set; }
        public decimal StackerBilletesM { get; set; }
        public decimal StackerTITO { get; set; }
        public decimal StackerTITOM { get; set; }
        public decimal MontoImpreso { get; set; }
        public decimal MontoDineroPagado { get; set; }
        public int TurnoOperativo { get; set; }

    }
}
