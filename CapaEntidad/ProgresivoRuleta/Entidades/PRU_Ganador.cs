using System;

namespace CapaEntidad.ProgresivoRuleta.Entidades {
    public class PRU_Ganador {
        public int Id { get; set; }
        public int CodSala { get; set; }
        public string CodMaquina { get; set; }
        public int IdRuleta { get; set; }
        public decimal Monto { get; set; }
        public int EsAcreditado { get; set; } = 1;
        public DateTime FechaGanador { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
