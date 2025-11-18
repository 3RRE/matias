using System;
using System.Collections.Generic;

namespace CapaEntidad.ProgresivoRuleta.Entidades {
    public class PRU_Configuracion {
        public int IdRuleta { get; set; }
        public int CodSala { get; set; }
        public string NombreRuleta { get; set; } = string.Empty;
        public int TotalSlots { get; set; }
        public List<string> SlotHexValues { get; set; } = new List<string>();
        public List<int> SlotHexPositions { get; set; } = new List<int>();
        public string CodMaquinas { get; set; } = string.Empty;
        public string Posiciones { get; set; } = string.Empty;
        public int MinSlotsPlaying { get; set; }
        public decimal CoinInPercent { get; set; }
        public int MinBet { get; set; }
        public string Ip { get; set; }
        public bool StatusOk { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public DateTime FechaRegistro { get; set; }

        public bool Existe() {
            return IdRuleta > 0;
        }
    }
}
