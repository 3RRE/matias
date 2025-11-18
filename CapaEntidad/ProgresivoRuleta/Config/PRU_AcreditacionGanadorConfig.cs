using System;

namespace CapaEntidad.ProgresivoRuleta.Config {
    public class PRU_AcreditacionGanadorConfig {
        public int CodSala { get; set; }
        public int IdRuleta { get; set; }
        public string Detalle { get; set; } = string.Empty;
        public DateTime FechaAlerta { get; set; }
        public int EsAcreditado { get; set; } = 0;
    }

    public class ContadorMaquinaDto {
        public Int64 CodContOL { get; set; }
        public DateTime Fecha { get; set; }
        public string Hora { get; set; }
        public string CodMaq { get; set; }
        public decimal CoinIn { get; set; }
        public decimal CoinOut { get; set; }
        public decimal Monto { get; set; }
        public decimal EftIn { get; set; }
        public decimal Token { get; set; }

    }
}
