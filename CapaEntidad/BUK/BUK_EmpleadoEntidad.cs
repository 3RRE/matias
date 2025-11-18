using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.BUK {
    public class BUK_EmpleadoEntidad {
        public int IdBuk { get; set; }
        public string TipoDocumento { get; set; } = string.Empty;
        public string NumeroDocumento { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string ApellidoMaterno { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public int IdCargo { get; set; }
        public string Cargo { get; set; } = string.Empty;
        public int IdEmpresa { get; set; }
        public string Empresa { get; set; } = string.Empty;
        public DateTime? FechaCese { get; set; }
        public bool EstadoCese { get; set; }
    }
}
