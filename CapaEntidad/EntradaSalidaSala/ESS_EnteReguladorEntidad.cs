using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.EntradaSalidaSala {
    public class ESS_EnteReguladorEntidad {

        public int IdEnteRegulador { get; set; }
        public int CodSala { get; set; }
        public String NombreSala { get; set; }
        public int IdMotivo { get; set; }
        public String NombreMotivo { get; set; }
        public String DescripcionMotivo { get; set; }
        public int IdEmpresa { get; set; }
        public String NombreEmpresa { get; set; }
        public String Descripcion { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaSalida { get; set; }
        public String Observaciones { get; set; }
        public String DocReferencia { get; set; }
        public String RutaImagen { get; set; }
        public String NombreAutoriza { get; set; }

        public int IdAutoriza { get; set; }
        public int IdArchivo { get; set; }


        public int? IdEmpleadoSEG { get; set; }
        public string EmpleadoNombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public int? IdTipoDocumentoRegistro { get; set; }
        public string DocumentoRegistro { get; set; }
        public string IdCargo { get; set; }
        public int? Estado { get; set; }



        public string UsuarioRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }



        public List<ESS_BienMaterialEmpleadoEntidad> Empleados { get; set; }
        public List<ESS_EntidadRegularPersonaEntidadPublica> PersonasEntidadPublica { get; set; } = new List<ESS_EntidadRegularPersonaEntidadPublica>();



    }
}


