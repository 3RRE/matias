using System;

namespace CapaEntidad.ProgresivoRuleta.Dto {
    public class PRU_GanadorDto {
        public int Id { get; set; }
        public PRU_SalaDto Sala { get; set; }
        public PRU_RuletaDto Ruleta { get; set; }
        public string CodMaquina { get; set; }
        public decimal Monto { get; set; }
        public int EsAcreditado { get; set; }
        public DateTime FechaGanador { get; set; }
        public DateTime FechaRegistro { get; set; }

        public bool Existe() {
            return Id > 0;
        }
    }
}
