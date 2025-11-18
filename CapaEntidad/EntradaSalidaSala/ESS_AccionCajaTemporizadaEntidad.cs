using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.EntradaSalidaSala {

    public class ESS_AccionCajaTemporizadaEntidad {


        public int IdAccionCajaTemporizada { get; set; }
        public int? IdEmpleadoESS { get; set; }
        public int CodSala { get; set; }
        public string NombreSala { get; set; }
        public DateTime Fecha { get; set; }
        public int IdDispositivo { get; set; }
        public string NombreDispositivo { get; set; }
        public string DescripcionDispositivo { get; set; }
        public int IdDeficiencia { get; set; }
        public string NombreDeficiencia { get; set; }
        public string MedidaAdoptada { get; set; }
        public DateTime? FechaSolucion { get; set; }
        public string UsuarioRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int? Estado { get; set; }

        public int IdAutoriza { get; set; }
        public string NombreAutoriza { get; set; }
        public string Cargo { get; set; }
        public string DocumentoRegistro{ get; set; }
        public string TipoDocumento{ get; set; }

        public ESS_DispositivoEntidad Dispositivo { get; set; }
        public ESS_DeficienciaEntidad Deficiencia { get; set; }

        public List<ESS_AccionCajaTemporizadaEmpleadoEntidad> BukEmpleadoEntidad { get; set; }
        public List<ESS_AccionCajaTemporizadaEmpleadoEntidad> Empleados { get; set; }


        public List<ESS_AccionCajaTemporizadaRelacionEntidad> Relaciones { get; set; } = new List<ESS_AccionCajaTemporizadaRelacionEntidad>();


    }



    public class ESS_AccionCajaTemporizadaRelacionEntidad {
        public int IdRelacion { get; set; }
        public string TipoRelacion { get; set; }
        public string Observaciones { get; set; }
    }

    public class ESS_DispositivoEntidad {

        public int IdDispositivo { get; set; }
        public string Nombre { get; set; }
        public int? Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
    }

    public class ESS_DeficienciaEntidad {

        public int IdDeficiencia { get; set; }
        public string Nombre { get; set; }
        public int? Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
    }

    public class BukEmpleadoEntidad {

        public int IdBuk { get; set; }
        public string NombreCompleto { get; set; }
        public string Cargo { get; set; }
    }

    public class ESS_AccionCajaTemporizadaCargoEntidad {
         
        public int? IdAutoriza { get; set; } 
        public string NombreAutoriza { get; set; } 
        public string Cargo { get; set; }
        public string NumeroDocumento { get; set; }

    }



}