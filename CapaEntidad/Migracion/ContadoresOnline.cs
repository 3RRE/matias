using System;

namespace CapaEntidad.Migracion {
    public class ContadoresOnline {
        public long Cod_Cont_OL { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Hora { get; set; }
        public string CodMaq { get; set; }
        public string CodMaqMin { get; set; }
        public string CodTarjeta { get; set; }
        public double CoinIn { get; set; }
        public double CoinOut { get; set; }
        public double HandPay { get; set; }
        public double CurrentCredits { get; set; }
        public decimal Monto { get; set; }
        public double? EftIn { get; set; }
        public double? EftOut { get; set; }
        public double? CancelCredits { get; set; }
        public double? Jackpot { get; set; }
        public double? GamesPlayed { get; set; }
        public double? TrueIn { get; set; }
        public double? TrueOut { get; set; }
        public double? TotalDrop { get; set; }
        public double? Bill { get; set; }
        public long? Bill1 { get; set; }
        public long? Bill2 { get; set; }
        public long? Bill5 { get; set; }
        public long? Bill10 { get; set; }
        public long? Bill20 { get; set; }
        public long? Bill50 { get; set; }
        public long? Bill100 { get; set; }
        public string NroTiket { get; set; }
        public double? TicketIn { get; set; }
        public double? TicketOut { get; set; }
        public double? TicketBonusIn { get; set; }
        public double? TicketBonusOut { get; set; }
        public long? MontoTiket { get; set; }
        public decimal? Progresivo { get; set; }
        public string Enviado { get; set; }
        public int? codevento { get; set; }
        public int? codcli { get; set; }
        public int? codsuper { get; set; }
        public int? CodPer { get; set; }
        public int? CodAtencion { get; set; }
        public int? CodCuadre { get; set; }
        public decimal? PreCredito { get; set; }
        public decimal? Token { get; set; }
        public string crc { get; set; }
        public string PorD { get; set; }
        public string Tficha { get; set; }
        public double? tmpebw { get; set; }
        public double? tapebw { get; set; }
        public double? tappw { get; set; }
        public double? tmppw { get; set; }
        public string Empresa { get; set; }
        public string Sala { get; set; }
    }
}
