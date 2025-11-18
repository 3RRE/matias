using System;
using System.Collections.Generic;

namespace CapaEntidad.ProgresivoRuleta.Dto {
    public class PRU_ConfiguracionDto {
        public int IdRuleta { get; set; }
        public PRU_SalaDto Sala { get; set; } = new PRU_SalaDto();
        public string NombreRuleta { get; set; } = string.Empty;
        public int TotalSlots { get; set; }
        public string CodMaquinas { get; set; } = string.Empty;
        public string Posiciones { get; set; } = string.Empty;
        public List<string> SlotHexValues { get; set; } = new List<string>();
        public List<int> SlotHexPositions { get; set; } = new List<int>();
        public int MinSlotsPlaying { get; set; }
        public decimal CoinInPercent { get; set; }
        public int MinBet { get; set; }
        public string Ip { get; set; } = string.Empty;
        public bool StatusOk { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public string HoraInicioStr => HoraInicio.ToString(@"hh\:mm");
        public TimeSpan HoraFin { get; set; }
        public string HoraFinStr => HoraFin.ToString(@"hh\:mm");

        public bool Existe() {
            return IdRuleta > 0;
        }
    }
}
