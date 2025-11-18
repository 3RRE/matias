using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.EntradaSalidaSala {
    public class ESS_VisitasSupervisorEntidad {
        public int IdVisitaSupervisor { get; set; }
        public int CodSala { get; set; }
        public string NombreSala { get; set; }
        public int IdMotivo { get; set; }
        public string Nombre { get; set; }
        public string OtroMotivo { get; set; }
        public string Observaciones { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaSalida { get; set; }
        public string UsuarioRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public List<ESS_VisitaSupervisorDetalleEntidad> Empleados { get; set; }

    }

    public class ESS_VisitaSupervisorMotivoEntidad {
        public int IdMotivo { get; set; }
        public string Nombre { get; set; }
        public int Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
    }

    public class ESS_VisitaSupervisorDetalleEntidad {
        public int IdVisitaSupervisorDetalle { get; set; }
        public int IdVisitaSupervisor { get; set; }
        public int IdSupervisor { get; set; }
        public string UsuarioRegistro { get; set; }
        public string UsuarioModificacion { get; set; }

        public int IdEmpleado { get; set; }
        public string Nombre { get; set; }
        public string NombreCompleto { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string DocumentoRegistro { get; set; }
        public int IdCargo { get; set; }
        public string TipoDocumento { get; set; }
        public string Cargo { get; set; }


    }
}
