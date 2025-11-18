using System;

namespace CapaEntidad.Campañas {
    public class CMP_ClienteEntidad {
        public Int64 id { get; set; }
        public Int64 campania_id { get; set; }
        public Int64 cliente_id { get; set; }
        //---------------------------------------
        public string ApelPat { get; set; }
        public string ApelMat { get; set; }
        public string Nombre { get; set; }
        public string NombreCompleto { get; set; }
        public string NroDoc { get; set; }
        public int TipoDocumentoId { get; set; }
        public string TipoDocumento { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int SalaId { get; set; }
        public string NombreSala { get; set; }
        public Int64 CMPcliente_id { get; set; }
        public string Mail { get; set; }
        public string CodigoPais { get; set; }
        public string NumeroCelular { get; set; }
        //------------------------------------------
        public string CampaniaNombre { get; set; }
        public DateTime fecha_reg { get; set; }
        public string Codigo { get; set; }
        public bool CodigoCanjeado { get; set; }
        public DateTime FechaGeneracionCodigo { get; set; }
        public DateTime FechaExpiracionCodigo { get; set; }
        public DateTime FechaCanjeoCodigo { get; set; }
        public bool CodigoExpirado { get; set; }
        public bool CodigoEnviado { get; set; }
        public string ProcedenciaRegistro { get; set; }
        public string Nacionalidad { get; set; }
        public int Edad { get => CalcularEdad(); }
        public string CodigoCanjeableEn { get; set; }
        public bool CodigoCanjeableMultiplesSalas { get; set; }
        public int CodigoCanjeadoEn { get; set; }
        public decimal MontoRecargado { get; set; }
        public string MontoRecargadoStr { get { return MontoRecargado.ToString("0.00"); } }

        public bool EsPosibleEnviarMensajeWhatsApp() {
            return
                !string.IsNullOrEmpty(CodigoPais) &&
                !string.IsNullOrEmpty(NumeroCelular) &&
                CodigoPais.Length >= 1 &&
                NumeroCelular.Length >= 7;
        }

        public bool CodigoEstaExpirado() {
            DateTime fechaExpiracion = new DateTime(FechaExpiracionCodigo.Year, FechaExpiracionCodigo.Month, FechaExpiracionCodigo.Day, FechaExpiracionCodigo.Hour, FechaExpiracionCodigo.Minute, 0);
            DateTime fechaActual = DateTime.Now;
            return fechaActual > fechaExpiracion;
        }

        public int CalcularEdad() {
            DateTime ahora = DateTime.Now;
            int edad = ahora.Year - FechaNacimiento.Year;
            // Ajustar la edad si aún no ha cumplido años en el año actual
            if(FechaNacimiento > ahora.AddYears(-edad)) {
                edad--;
            }
            return edad;
        }

        public string ObtenerMensajeFormateadoParaEnvio(string mensajePlantilla, CMP_CampañaEntidad campania, CMP_ClienteEntidad campaniaCliente) {
            string mensaje = string.Empty;
            mensaje = string.Format(
                mensajePlantilla,
                campaniaCliente.Codigo,
                campaniaCliente.NombreSala,
                campaniaCliente.FechaExpiracionCodigo.ToString("dd/MM/yyyy HH:mm"),
                !string.IsNullOrEmpty(campaniaCliente.Nombre) ? campaniaCliente.Nombre : campaniaCliente.NombreCompleto,
                campaniaCliente.NombreCompleto,
                campania.duracionCodigoDias,
                campania.duracionCodigoHoras,
                campania.duracionReactivacionCodigoDias,
                campania.duracionReactivacionCodigoHoras,
                campaniaCliente.FechaExpiracionCodigo.AddDays(campania.duracionReactivacionCodigoDias).AddHours(campania.duracionReactivacionCodigoHoras).ToString("dd/MM/yyyy HH:mm"),
                campaniaCliente.MontoRecargado.ToString("0.00")
            );
            return mensaje;
        }
    }
}
