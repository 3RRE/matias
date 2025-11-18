using CapaEntidad.ControlAcceso.HistorialLudopata.Enum;
using System;

namespace CapaEntidad.ControlAcceso.HistorialLudopata {
    public class CAL_HistorialLudopata {
        public int Id { get; set; }
        public int IdLudopata { get; set; }
        public CAL_TipoMovimientoHistorialLudopata TipoMovimiento { get; set; }
        public CAL_TipoRegistroHistorialLudopata TipoRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
