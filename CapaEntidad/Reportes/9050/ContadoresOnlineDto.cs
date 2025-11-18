using System;

namespace CapaEntidad.Reportes._9050 {
    public class ContadoresOnlineDto {
        public int CodContadoresOnline { get; set; }
        public string CodMaq { get; set; }
        public string NroSerie { get; set; }
        public string Sala { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public DateTime? FechaHora { get; set; }
        public DateTime? Fecha { get; set; }
        public int? Hora { get; set; }
        public double CoinIn { get; set; }
        public double CoinOut { get; set; }
        public double CurrentCredits { get; set; }
        public double CancelCredits { get; set; }
        public double GamesPlayed { get; set; }
        public double Jackpot { get; set; }
        //public double Bill { get; set; }
        public double Win { get; set; }
        public decimal Token { get; set; }
        public double Promedio { get; set; }
        public DateTime? FechaReal { get; set; }
        public string Formula { get; set; }
    }
}
