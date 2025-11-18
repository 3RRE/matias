namespace CapaPresentacion.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ADM_DetalleContadoresGame
    {
        [Key]
        public int CodDetalleContadoresGame { get; set; }

        public int? CodContadoresGame { get; set; }

        public int? CodMaquina { get; set; }

        public int? CodSala { get; set; }

        public int? CodEmpresa { get; set; }

        public int? CodMoneda { get; set; }

        public DateTime? FechaOperacion { get; set; }

        public decimal? CoinInIni { get; set; }

        public decimal? CoinInFin { get; set; }

        public decimal? CoinOutIni { get; set; }

        public decimal? CoinOutFin { get; set; }

        public decimal? JackpotIni { get; set; }

        public decimal? JackpotFin { get; set; }

        public decimal? HandPayIni { get; set; }

        public decimal? HandPayFin { get; set; }

        public decimal? CancelCreditIni { get; set; }

        public decimal? CancelCreditFin { get; set; }

        public decimal? GamesPlayedIni { get; set; }

        public decimal? GamesPlayedFin { get; set; }

        public decimal? ProduccionPorSlot1 { get; set; }

        public decimal? ProduccionPorSlot2Reset { get; set; }

        public decimal? ProduccionPorSlot3Rollover { get; set; }

        public decimal? ProduccionPorSlot4Prueba { get; set; }

        public decimal? ProduccionTotalPorSlot5Dia { get; set; }

        public decimal? TipoCambio { get; set; }

        public DateTime? FechaRegistro { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public bool? Activo { get; set; }

        public int? Estado { get; set; }

        public decimal? SaldoCoinIn { get; set; }

        public decimal? SaldoCoinOut { get; set; }

        public decimal? SaldoJackpot { get; set; }

        public decimal? SaldoGamesPlayed { get; set; }

        [StringLength(100)]
        public string CodUsuario { get; set; }

        public int? RetiroTemporal { get; set; }

        public decimal? TiempoJuego { get; set; }

        public virtual ADM_Maquina ADM_Maquina { get; set; }
    }
}
