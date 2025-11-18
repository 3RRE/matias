using CapaEntidad.ControlAcceso.HistorialLudopata.Enum;
using System;

namespace CapaEntidad.ControlAcceso.HistorialLudopata.Dto {
    public class CAL_HistorialLudopataDto {
        public int Id { get; set; }
        public int IdLudopata { get; set; }
        public string NumeroDocumento { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string ApellidoMaterno { get; set; } = string.Empty;
        public CAL_TipoMovimientoHistorialLudopata TipoMovimiento { get; set; }
        public string TipoMovimientoStr { get; set; } = string.Empty;
        public CAL_TipoRegistroHistorialLudopata TipoRegistro { get; set; }
        public string TipoRegistroStr { get; set; } = string.Empty;
        public string NombresEmpleado { get; set; } = string.Empty;
        public string ApellidoPaternoEmpleado { get; set; } = string.Empty;
        public string ApellidoMaternoEmpleado { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }

        public string ObtenerNombreCompletoCliente() {
            return $"{Nombres} {ApellidoPaterno} {ApellidoMaterno}".Trim();
        }

        public string ObtenerNombreCompletoEmpleado() {
            return $"{NombresEmpleado} {ApellidoPaternoEmpleado} {ApellidoMaternoEmpleado}".Trim();
        }
    }
}
