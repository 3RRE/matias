using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.EntradaSalidaSala {
    public class ESS_LogOcurrenciaEntidad {
        public int IdLogOcurrencia { get; set; }
        public int CodSala { get; set; }
        public string NombreSala { get; set; }
        public int IdArea { get; set; }
        public string NombreArea { get; set; }
        public string DescripcionArea { get; set; }
        public int IdTipologia { get; set; }
        public string NombreTipologia { get; set; }
        public string DescripcionTipologia { get; set; }
        public int IdActuante { get; set; }
        public string NombreActuante { get; set; }
        public string DescripcionActuante { get; set; }
        public string Detalle { get; set; }
        public int IdComunicacion { get; set; }
        public string NombreComunicacion { get; set; }
        public string DescripcionComunicacion { get; set; }
        public string AccionEjecutada { get; set; }
        public int IdEstadoOcurrencia { get; set; }
        public string EstadoOcurrencia { get; set; }
        public DateTime FechaSolucion { get; set; }
        public DateTime Fecha { get; set; }
        public string UsuarioRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
    public class ESS_AreaEntidad {
        public int IdArea { get; set; }
        public string Nombre { get; set; }
        public int Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
    }
    public class ESS_TipologiaEntidad {
        public int IdTipologia { get; set; }
        public string Nombre { get; set; }
        public int Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
    }
    public class ESS_ActuanteEntidad {
        public int IdActuante { get; set; }
        public string Nombre { get; set; }
        public int Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
    }
    public class ESS_ComunicacionEntidad {
        public int IdComunicacion { get; set; }
        public string Nombre { get; set; }
        public int Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
    }
    public class ESS_EstadoOcurrenciaEntidad {
        public int IdEstadoOcurrencia { get; set; }
        public string Nombre { get; set; }
        public int Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
    }

}
