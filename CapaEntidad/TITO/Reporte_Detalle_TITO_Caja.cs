using System;

namespace CapaEntidad.TITO
{
    public class Reporte_Detalle_TITO_Caja
    {
        public DateTime Fecha_Proceso { get; set; }
        public string Fecha_Proceso_Inicio { get; set; }
        public string Fecha_Proceso_desc { get; set; }
        public string Nro_Documento { get; set; }
        public decimal Monto_Dinero { get; set; }
        public string Monto_Dinero_desc { get; set; }
        public decimal IGV { get; set; }
        public string IGV_desc { get; set; }
        public decimal SubTOTAL { get; set; }
        public string SubTOTAL_desc { get; set; }
        public decimal TOTAL { get; set; }
        public string TOTAL_desc { get; set; }
        public string TipoPago_desc { get; set; }
        public string TipoProceso_desc { get; set; }
        public string Descripcion { get; set; }
        public string Cliente { get; set; }
        public int ClienteCodigo { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteApellido { get; set; }
        public string ClienteDni { get; set; }
        public string ClienteGenero { get; set; }
        public string ClienteFechaNacimiento { get; set; }
        public string ClienteCorreo { get; set; }
        public string ClienteTelefono { get; set; }
        public string Turno { get; set; }
        public string Personal { get; set; }
        public string Hora { get; set; }
        public string Ticket { get; set; }
        public string DioAutorizacion { get; set; }
        public int EstadoTiket { get; set; }
        public int idTipoProceso { get; set; }
        public string TipoOrigen { get; set; }
        public string LugarOrigen { get; set; }
        public string Motivo { get; set; }
        public string CodigoExtra { get; set; }
        public int ProcesoNroCaja { get; set; }
        public string ProcesoNroMaquina { get; set; }
        public string FechaApertura { get; set; }
        public int D00H_Item { get; set; }
    }
}
