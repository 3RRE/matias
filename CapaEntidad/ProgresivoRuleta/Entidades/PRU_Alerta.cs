using System;

namespace CapaEntidad.ProgresivoRuleta.Entidades {
    public class PRU_Alerta {
        public int Id { get; set; }
        public int CodSala { get; set; }
        public int IdRuleta { get; set; }
        public string Detalle { get; set; }
        public string CodMaquina { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaAlerta { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
