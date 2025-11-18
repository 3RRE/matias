using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace CapaEntidad.EntradaSalidaSala {
    public class ESS_RecojoRemesaEntidad {
        public int IdRecojoRemesa { get; set; }
        public int CodSala { get; set; }
        public string Sala { get; set; }
        public int IdPersonal { get; set; }
        public string NombreCompletoPersonal { get; set; }
        public string TipoDocumentoRegistro { get; set; }
        public string DocumentoRegistro { get; set; }
        public string CodigoPersonal { get; set; }
        public int IdEstadoFotocheck { get; set; }
        public string EstadoFotocheck { get; set; }
        public string OtroEstadoFotocheck { get; set; }
        public string PlacaRodaje { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaSalida { get; set; }
        public string Observaciones { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
    }

    public class ESS_EstadoFotocheckEntidad {
        public int IdEstadoFotocheck { get; set; }
        public string Nombre { get; set; }
        public int Estado { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }

    }
    public class ESS_RecojoRemesaPersonalEntidad {
        public int IdRecojoRemesaPersonal { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string DocumentoRegistro { get; set; }
        public int IdTipoDocumentoRegistro { get; set; }
        public string TipoDocumentoRegistro { get; set; }
        public string CodigoPersonal { get; set; }
        public int Estado { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
  
//CREATE TABLE ESS_RecojoRemesaPersonal(
//    IdRecojoRemesaPersonal[int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
//    Nombres varchar(50) NULL,
//	ApellidoPaterno varchar(50) NULL,
//	ApellidoMaterno varchar(50) NULL,
//	DocumentoRegistro varchar(50) NULL,
//	IdTipoDocumentoRegistro int NULL,
//    CodigoPersonal varchar(50) NULL,
//	Estado int default 1,
	
//	UsuarioRegistro[varchar] (50) NULL,
//	FechaRegistro datetime NULL,
//	UsuarioModificacion[varchar] (50) NULL,
//	FechaModificacion datetime null,
//)