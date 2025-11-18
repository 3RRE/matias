using System;

namespace CapaEntidad.ProgresivoRuleta.Dto {
    public class PRU_AlertaDto {
        public int Id { get; set; }
        public PRU_SalaDto Sala { get; set; }
        public PRU_RuletaDto Ruleta { get; set; }
        public string Detalle { get; set; }
        public string CodMaquina { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaAlerta { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
