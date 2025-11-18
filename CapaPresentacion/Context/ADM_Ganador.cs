namespace CapaPresentacion.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ADM_Ganador
    {
        [Key]
        public int CodGanador { get; set; }

        public int? CodDetalleSalaProgresivo { get; set; }

        public int? CodMaquina { get; set; }

        public decimal? MontoProgresivo { get; set; }

        public int? SubioPremio { get; set; }

        public DateTime? FechaPremio { get; set; }

        public DateTime? FechaSubida { get; set; }

        public decimal? Token { get; set; }

        public long? AntCoinIn { get; set; }

        public long? AntCoinOut { get; set; }

        public long? AntJackpot { get; set; }

        public long? AntCancelCredits { get; set; }

        public long? AntBill { get; set; }

        public long? AntGamesPlayed { get; set; }

        public long? AntBonus { get; set; }

        public long? AntEftIn { get; set; }

        public long? ActCoinIn { get; set; }

        public long? ActCoinOut { get; set; }

        public long? ActJackpot { get; set; }

        public long? ActCancelCredits { get; set; }

        public long? ActBill { get; set; }

        public long? ActGamesPlayed { get; set; }

        public long? ActBonus { get; set; }

        public long? ActEftIn { get; set; }

        public DateTime? FechaRegistro { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public bool? Activo { get; set; }

        public int? Estado { get; set; }

        [StringLength(100)]
        public string CodUsuario { get; set; }

        public DateTime? FechaOperacion { get; set; }

        public virtual ADM_DetalleSalaProgresivo ADM_DetalleSalaProgresivo { get; set; }

        public virtual ADM_Maquina ADM_Maquina { get; set; }
    }
}
