using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{

    public class DetalleContadoresGame
    {
        public int? CodDetalleContadoresGame { get; set; }
        public int? CodContadoresGame { get; set; }
        public string CodMaquina { get; set; }
        public int? CodSala { get; set; }
        public int? CodEmpresa { get; set; }
        public int? CodMoneda { get; set; }
        public DateTime? FechaOperacion { get; set; }
        public double? CoinInIni { get; set; }
        public double? CoinInFin { get; set; }
        public double? SaldoCoinIn { get; set; }
        public double? CoinOutIni { get; set; }
        public double? CoinOutFin { get; set; }
        public double? SaldoCoinOut { get; set; }
        public double? JackpotIni { get; set; }
        public double? JackpotFin { get; set; }
        public double? SaldoJackpot { get; set; }
        public double? HandPayIni { get; set; }
        public double? HandPayFin { get; set; }
        public double? CancelCreditIni { get; set; }
        public double? CancelCreditFin { get; set; }
        public double? GamesPlayedIni { get; set; }
        public double? GamesPlayedFin { get; set; }
        public double? SaldoGamesPlayed { get; set; }
        public double? ProduccionPorSlot1 { get; set; }
        public double? ProduccionPorSlot2Reset { get; set; }
        public double? ProduccionPorSlot3Rollover { get; set; }
        public double? ProduccionPorSlot4Prueba { get; set; }
        public double? ProduccionTotalPorSlot5Dia { get; set; }
        public double? TipoCambio { get; set; }
        public int? RetiroTemporal { get; set; }
        public double? TiempoJuego { get; set; }

        //CAMPOS FALTANTES?
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Activo { get; set; }
        public bool Estado { get; set; }
        public int CodUsuario { get; set; }

    }
    public class DetalleContadoresGameResponse {
        public int CodDetalleContadoresGame { get; set; }

        public int CodContadoresGame { get; set; }

        public int CodMaquina { get; set; }

        public int CodSala { get; set; }

        public int CodEmpresa { get; set; }

        public int CodMoneda { get; set; }

        public DateTime FechaOperacion { get; set; }

        public decimal CoinInIni { get; set; }

        public decimal CoinInFin { get; set; }

        public decimal CoinOutIni { get; set; }

        public decimal CoinOutFin { get; set; }

        public decimal JackpotIni { get; set; }

        public decimal JackpotFin { get; set; }

        public decimal HandPayIni { get; set; }

        public decimal HandPayFin { get; set; }

        public decimal CancelCreditIni { get; set; }

        public decimal CancelCreditFin { get; set; }

        public decimal GamesPlayedIni { get; set; }

        public decimal GamesPlayedFin { get; set; }

        public decimal ProduccionPorSlot1 { get; set; }

        public decimal ProduccionPorSlot2Reset { get; set; }

        public decimal ProduccionPorSlot3Rollover { get; set; }

        public decimal ProduccionPorSlot4Prueba { get; set; }

        public decimal ProduccionTotalPorSlot5Dia { get; set; }

        public decimal TipoCambio { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool Activo { get; set; }

        public int Estado { get; set; }

        public decimal SaldoCoinIn { get; set; }

        public decimal SaldoCoinOut { get; set; }

        public decimal SaldoJackpot { get; set; }

        public decimal SaldoGamesPlayed { get; set; }

        public string CodUsuario { get; set; }

        public int RetiroTemporal { get; set; }

        public decimal TiempoJuego { get; set; }
        public string CodMaquinaLey { get; set; }
        public string CodAlterno { get; set; }
    }
}
