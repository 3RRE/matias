using System.Collections.Generic;

namespace CapaEntidad.Reportes._9050 {
    public class ReporteGerenciaDto {
        public List<ReporteGerenciaCabeceraDto> cabecera { get; set; }
        public List<ContadoresOnlineDto> detalle { get; set; }
    }
}
