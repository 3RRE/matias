using System.Collections.Generic;

namespace CapaEntidad.ProgresivoRuleta.Config {
    public class Pru_MisteryConfig {
        public int TotalSlots { get; set; }
        public List<string> SlotHexValues { get; set; } = new List<string>();
        public List<int> SlotHexPositions { get; set; } = new List<int>();
        public int MinSlotsPlaying { get; set; }
        public decimal CoinInPercent { get; set; }
        public int MinBet { get; set; }
        public string Ip { get; set; } = string.Empty;
        public bool StatusOk { get; set; } = true;
        public string HoraInicio { get; set; } = string.Empty;
        public string HoraFin { get; set; } = string.Empty;

        public bool Existe() {
            return TotalSlots > 0;
        }
    }
}
