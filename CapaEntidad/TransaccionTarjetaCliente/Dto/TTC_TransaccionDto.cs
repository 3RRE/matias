using System;

namespace CapaEntidad.TransaccionTarjetaCliente.Dto {
    public class TTC_TransaccionDto {
        public int Id { get; set; }
        public int ItemVoucher { get; set; }
        public TTC_SalaDto Sala { get; set; } = new TTC_SalaDto();
        public TTC_Cliente Cliente { get; set; } = new TTC_Cliente();
        public TTC_Tarjeta Tarjeta { get; set; } = new TTC_Tarjeta();
        public TTC_CajaDto Caja { get; set; } = new TTC_CajaDto();
        public decimal Monto { get; set; }
        public string MontoStr { get => $"S/{Monto:0.00}"; }
        public DateTime FechaRegistro { get; set; }
        public string FechaRegistroStr { get => FechaRegistro.ToString("dd/MM/yyyy HH:mm:ss"); }
    }
}
