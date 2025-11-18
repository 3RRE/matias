using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.EntradaSalidaSala {
     
    public class ESS_IngresoSalidaGUEntidad {
        //TABLA ESS_IngresoSalidaGU 
        public int IdIngresoSalidaGU { get; set; }
        public int CodSala { get; set; }
        public string NombreSala { get; set; }
        public DateTime Fecha { get; set; } 
        public string Descripcion { get; set; }
        public int? IdMotivo { get; set; }
        public string NombreMotivo { get; set; }
        public string DescripcionMotivo { get; set; }
        public DateTime HoraIngreso { get; set; }
        public DateTime HoraSalida { get; set; }
        public string Observaciones { get; set; }
        public string UsuarioRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
        public string Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; } 
        public List<ESS_IngresoSalidaGUEmpleadoEntidad> Empleados { get; set; } 
         
    }

}
