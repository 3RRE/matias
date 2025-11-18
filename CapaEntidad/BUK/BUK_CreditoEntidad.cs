using System;

namespace CapaEntidad.BUK {
    public class BUK_CreditoEntidad {
        public int IdCredito { get; set; }
        public int IdCreditoBuk { get; set; }
        public string NumeroDocumento { get; set; } = string.Empty;
        public int Anio { get; set; }
        public int Periodo { get; set; }
        public string CodigoEmpresa { get; set; } = string.Empty;
        public string CodigoTipoVale { get; set; }
        public decimal CuotaMensual { get; set; }
        public int CantidadCoutas { get; set; }
        public decimal MontoTotal { get; set; }
        public string DescripcionTipo { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }

        public bool Existe() {
            return IdCredito > 0;
        }
    }
}
