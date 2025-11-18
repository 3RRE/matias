using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.EntradaSalidaSala {
    public class ESS_RecaudacionPersonalEntidad {
        public int IdRecaudacionPersonal { get; set; }
        public int CodSala { get; set; }
        public string NombreSala { get; set; }
        public DateTime RecaudacionInicio { get; set; }
        public DateTime RecaudacionFin { get; set; }
        public DateTime EmpadronamientoInicio { get; set; }
        public DateTime EmpadronamientoFin { get; set; }
        public int NumeroClientes { get; set; }
        public string Observaciones { get; set; }
        public string UsuarioRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public List<ESS_RecaudacionPersonalEmpleadoEntidad> Empleados { get; set; } = new List<ESS_RecaudacionPersonalEmpleadoEntidad>();
    }
    public class ESS_RecaudacionPersonalEmpleadoEntidad {
        public int IdRecaudacionPersonalEmpleado { get; set; }
        public int IdRecaudacionPersonal { get; set; }
        public int IdEmpleado { get; set; }
        public int IdEstadoParticipante { get; set; }
        public string EstadoParticipante { get; set; }
        public int? IdFuncion { get; set; }
        public string NombreFuncion { get; set; }
        public string DescripcionFuncion { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public int? IdTipoDocumentoRegistro { get; set; }
        public string DocumentoRegistro { get; set; }
        public int? IdCargo { get; set; }
        public string NombreCargo { get; set; }
        public string TipoDocumento {  get; set; }
        public string Cargo { get; set; } 
        public string NombreDocumentoRegistro { get; set; }
        public int? IdEmpresa { get; set; }
        public string Empresa { get; set; }
    }
    public class ESS_FuncionEntidad {
        public int IdFuncion { get; set; }
        public string Nombre { get; set; }
        public int Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string UsuarioRegistro { get; set; }
    }
    public class ESS_CargoRPEntidad {
        public int IdCargo { get; set; }
        public string Nombre { get; set; }
        public int Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string UsuarioRegistro { get; set; }
    }
    

}
