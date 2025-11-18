namespace CapaEntidad.TransaccionTarjetaCliente.Dto {
    public class TTC_Tarjeta {
        public string MedioPago { get; set; } = string.Empty;
        public string EntidadEmisora { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Numero { get; set; }
    }
}
