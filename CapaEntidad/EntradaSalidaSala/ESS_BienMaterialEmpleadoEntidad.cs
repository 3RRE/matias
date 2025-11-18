using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.EntradaSalidaSala {
    public class ESS_BienMaterialEmpleadoEntidad {
        public int IdBienMaterialEmpleado { get; set; }
        public int IdBienMaterial { get; set; }
        public int IdEmpleado { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public int IdTipoDocumentoRegistro { get; set; }
        public string DocumentoRegistro { get; set; }
        public int IdCargo { get; set; }
        public int IdEmpresaExterna { get; set; }
        public string Cargo { get; set; }
        public string NombreCargo { get; set; }

        public string TipoDocumento { get; set; }
        public string NombreDocumentoRegistro { get; set; }
        public string NombreDocumento { get; set; }
        public int IdEmpresa { get; set; }
        public string Empresa { get; set; }
    }
}
