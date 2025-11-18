using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Cortesias {

    public class CRT_Sala {
        public int SalId { get; set; }
        public int EmpresaId { get; set; }
        public string SalCodigo { get; set; }
        public string SalNombre { get; set; }
        public string SalDireccion { get; set; }
        public string SalCorreo { get; set; }
        public DateTime SalCreated { get; set; }
        public string SalUsuario { get; set; }
        public string SalContrasenia { get; set; }
        public bool SalEstado { get; set; }
        public double? SalLatitud { get; set; }
        public double? SalLongitud { get; set; }
    }
    public class CRT_Empleado {
        public int EmpIdBuk { get; set; }
        public string Empresa { get; set; }
        public string Sede { get; set; }
        public string EmpNombre { get; set; }
        public string EmpApePaterno { get; set; }
        public string EmpApeMaterno { get; set; }
        public string EmpNroDocumento { get; set; }
        public string CoEmpr { get; set; }
        public string CoSede { get; set; }
        public string Puesto { get; set; }
        public bool EmpEstado { get; set; }
        public string EmpCorreo { get; set; }
        public DateTime? EmpCesado { get; set; }
    }

}
