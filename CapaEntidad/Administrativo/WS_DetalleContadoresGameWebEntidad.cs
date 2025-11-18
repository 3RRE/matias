using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Administrativo
{
    public class WS_DetalleContadoresGameWebEntidad
    {
        public int CodDetalleContadoresGame { get; set; }

        public string CodMaquina { get; set; }

        public int CodSala { get; set; }

        public int CodEmpresa { get; set; }

        public int CodMoneda { get; set; }

        public DateTime FechaOperacion { get; set; }

        public decimal CoinIn { get; set; }

        public decimal CoinOut { get; set; }

        public decimal Jackpot { get; set; }

        public decimal HandPay { get; set; }

        public decimal CancelCredit { get; set; }

        public decimal GamesPlayed { get; set; }

        public decimal TipoCambio { get; set; }
        public DateTime Hora { get; set; }

    }
}
