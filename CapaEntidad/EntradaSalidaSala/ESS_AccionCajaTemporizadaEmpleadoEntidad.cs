using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.EntradaSalidaSala {
    public class ESS_AccionCajaTemporizadaEmpleadoEntidad {
        public int? IdAccionCajaTemporizadaEmpleado { get; set; }
        public int? IdAccionCajaTemporizada { get; set; }
        public int? IdEmpleado { get; set; }
        public int IdAutoriza { get; set; }
        public string NombreAutoriza { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public int? IdTipoDocumentoRegistro { get; set; }
        public string DocumentoRegistro { get; set; }
        public int? IdCargo { get; set; }
        public string NombreCargo { get; set; }
        public string TipoDocumento { get; set; }
    }
}
