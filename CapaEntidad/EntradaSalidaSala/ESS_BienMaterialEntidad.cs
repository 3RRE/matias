using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace CapaEntidad.EntradaSalidaSala {
    public class ESS_BienMaterialEntidad {
        //TABLA BIENMATERIAL
        public int IdBienMaterial { get; set; }
        public int CodSala { get; set; }
        public string NombreSala { get; set; }
        public int TipoBienMaterial { get; set; }
        public int IdCategoria { get; set; }
        public int IdCargoExterno { get; set; }
        public string NombreCategoria { get; set; }
        public string DescripcionCategoria { get; set; }
        public string Descripcion {  get; set; }
        public int IdMotivo { get; set; }
        public string NombreMotivo { get; set; }
        public int IdEmpresaExterna { get; set; }
        public int IdEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public string GRRFT { get; set; }
        public string RutaImagen { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaSalida { get; set; }
        public string Observaciones { get; set; }
        public string UsuarioRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public List<ESS_BienMaterialEmpleadoEntidad> Empleados { get; set; } = new List<ESS_BienMaterialEmpleadoEntidad>();

        //agregado para actualizarRegistrobienMaterial
        public int IdSala { get; set; }
        public int HoraIngreso { get; set; }
        public int HoraSalida { get; set; }
        public int IdTipoRegistro { get; set; }
        public int IdEstadoRegistro { get; set; }
        public int IdRegistro { get; set; }
        public int IdTipoBienMaterial { get; set; }
        public int IdArchivo { get; set; }
        public string IdCargo { get; set; }
    }

    public class ESS_EmpleadoEntidad {
        //TABLA BIENMATERIAL
        public int IdEmpleado { get; set; }
        public int IdEmpleadoSEG { get; set; }
        public String Nombre { get; set; }
        public String ApellidoPaterno { get; set; }
        public String ApellidoMaterno { get; set; }
        public int IdCargo { get; set; }
        public int IdEmpresaExterna { get; set; }
        public int IdTipoDocumentoRegistro { get; set; }
        public String TipoDocumento { get; set; }
        public String DocumentoRegistro { get; set; }
        public int IdTabla { get; set; }
        public int CantidadBusqueda { get; set; } 
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
        public int Estado{ get; set; }
        public ESS_CargoEntidad Cargo { get; set; }
        public string NombreCompleto { get; set; }
        public string NombreEmpresaExterna { get; set; }
        public string NombreCargo { get; set; }
        public string NombreDocumento { get; set; }
    }
    public class ESS_cboCatalogoEntidad {
        public Int64 value { get; set; }
        public String label { get; set; }
    }
}
 
   
 