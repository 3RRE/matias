using System;

namespace CapaEntidad.TransaccionTarjetaCliente.Entidad {
    public class TTC_TransaccionEntidad {
        public int Id { get; set; }
        public int CodSala { get; set; }
        public int? IdCliente { get; set; }
        public int ItemVoucher { get; set; }
        public string NombreCompletoCliente { get; set; } = string.Empty;
        public string NumeroDocumentoCliente { get; set; } = string.Empty;
        public string MedioPago { get; set; } = string.Empty;
        public string EntidadEmisora { get; set; } = string.Empty;
        public string TipoTarjeta { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string NumeroTarjeta { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public int Caja { get; set; }
        public int Turno { get; set; }
        public DateTime FechaMigracion { get; set; }
    }
}
