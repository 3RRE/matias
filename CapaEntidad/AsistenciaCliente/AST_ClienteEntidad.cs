using System;
using System.Collections.Generic;

namespace CapaEntidad.AsistenciaCliente {
    public class AST_ClienteEntidad {
        public int Id { get; set; }
        public string NroDoc { get; set; }
        public string Nombre { get; set; }
        public string ApelPat { get; set; }
        public string ApelMat { get; set; }
        public string Genero { get; set; }
        public string CodigoPais { get; set; }
        public string Celular1 { get; set; }
        public string Celular2 { get; set; }
        public string Mail { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int Edad { get; set; }
        public int AsistioDespuesCuarentena { get; set; }
        public string UbigeoRegDepartamento { get; set; }
        public string UbigeoRegProvincia { get; set; }
        public string UbigeoRegDistrito { get; set; }
        public string UbigeoProcDepartamento { get; set; }
        public string UbigeoProcProvincia { get; set; }
        public string UbigeoProcDistrito { get; set; }
        public string Estado { get; set; }
        public int usuario_reg { get; set; }

        public string PaisId { get; set; }
        public bool Ciudadano { get; set; }
        public int CodigoUbigeo { get; set; }

        public bool EnviaNotificacionWhatsapp { get; set; }
        public bool EnviaNotificacionSms { get; set; }
        public bool EnviaNotificacionEmail { get; set; }
        public bool LlamadaCelular { get; set; }
        public bool EsLudopata { get; set; }

        //Relaciones
        public int TipoDocumentoId { get; set; }
        public int UbigeoProcedenciaId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string TipoRegistro { get; set; }
        //Entidades Relacionadas
        public AST_TipoDocumentoEntidad TipoDocumento { get; set; }
        public UbigeoEntidad UbigeoProcedencia { get; set; }
        //public SalaEntidad SalaRegistro{ get; set; }
        public List<AST_ClienteSalaEntidad> ListaClienteSala { get; set; }
        public AST_ClienteSalaEntidad ClienteSala { get; set; }
        public AST_ClienteEntidad() {
            this.TipoDocumento = new AST_TipoDocumentoEntidad();
            this.UbigeoProcedencia = new UbigeoEntidad();
            //this.SalaRegistro = new SalaEntidad();
            this.ListaClienteSala = new List<AST_ClienteSalaEntidad>();
            this.ClienteSala = new AST_ClienteSalaEntidad();
            this.fecha_emision = new DateTime();
        }
        public int SalaId { get; set; }
        public string NombreCompleto { get; set; }

        public int sala_vacunacion { get; set; }
        public int nro_dosis { get; set; }
        public DateTime fecha_ultima_dosis { get; set; }
        public DateTime fecha_emision { get; set; }

        public string descripcion { get; set; }
        public Boolean entrega_dni { get; set; }
        public Boolean reniec { get; set; }

        // static
        public static readonly string Ubigeo_Pais_Id = "PE";

        //migracion
        public string NombreUbigeo { get; set; }
        public string NombreTipoDocumento { get; set; }
        public string NombreGenero { get; set; }
        public string NombreSala { get; set; }

        public bool Existe() {
            return Id != 0;
        }

        public bool EsMayorDeEdad() {
            return CalcularEdad() >= 18;
        }

        public int CalcularEdad() {
            DateTime ahora = DateTime.Now;
            int edad = ahora.Year - FechaNacimiento.Year;

            // Ajustar la edad si aún no ha cumplido años en el año actual
            if(FechaNacimiento > ahora.AddYears(-edad)) {
                edad--;
            }

            return edad;
        }
    }

    public class AST_ClienteSala {
        public int codSala { get; set; }
        public int IdCliente { get; set; }
        public string NombreSala { get; set; }
        public string TipoDocumento { get; set; }
        public string NroDoc { get; set; }
        public string NombreCliente { get; set; }
        public int cantDosis { get; set; }
        public string Celular { get; set; }
        public string Mail { get; set; }
        public string TipoRegistro { get; set; }
        public bool EnviaNotificacionWhatsapp { get; set; }
        public bool EnviaNotificacionSms { get; set; }
        public bool EnviaNotificacionEmail { get; set; }
        public bool LlamadaCelular { get; set; }
        public bool EsLudopata { get; set; }
        public bool EsProhibido { get; set; }
        public bool EsRobaStacker { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaRegistro { get; set; }
    }

    public class AST_FiltroCliente {
        private string _numeroDocumento = string.Empty;
        private string _nombres = string.Empty;
        private string _apellidoPaterno = string.Empty;
        private string _apellidoMaterno = string.Empty;

        public string NumeroDocumento {
            get => _numeroDocumento?.Trim();
            set => _numeroDocumento = value;
        }

        public string Nombres {
            get => _nombres?.Trim();
            set => _nombres = value;
        }

        public string ApellidoPaterno {
            get => _apellidoPaterno?.Trim();
            set => _apellidoPaterno = value;
        }

        public string ApellidoMaterno {
            get => _apellidoMaterno?.Trim();
            set => _apellidoMaterno = value;
        }
    }

    public class AST_ClienteSalaGlobal {
        public int codSala { get; set; }
        public string NombreSala { get; set; }
        public string TipoDocumento { get; set; }
        public string NroDoc { get; set; }
        public string NombreCliente { get; set; }
        public int cantDosis { get; set; }
        public string Celular { get; set; }
        public string Mail { get; set; }
        public string Nacionalidad { get; set; }
        public string PaisId { get; set; }
        public string TipoRegistro { get; set; }
        public bool EnviaNotificacionWhatsapp { get; set; }
        public bool EnviaNotificacionSms { get; set; }
        public bool EnviaNotificacionEmail { get; set; }
        public bool LlamadaCelular { get; set; }
        public bool EsLudopata { get; set; }
        public bool EsProhibido { get; set; }
        public bool EsRobaStacker { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaRegistro { get; set; }
    }

    public class EMP_respuestaAPICONSULTA {
        public string ApellidoMaterno { get; set; }
        public string ApellidoPaterno { get; set; }
        public string Nombre { get; set; }
        public string NombreCompleto { get; set; }
        public string DNI { get; set; }
        public Int32 cliente_id { get; set; }
    }
    public class AST_ClienteMigracion {
        public int IdIas { get; set; }
        public string NroDoc { get; set; }
        public string Nombre { get; set; }
        public string ApelPat { get; set; }
        public string ApelMat { get; set; }
        public string Celular1 { get; set; }
        public string Celular2 { get; set; }
        public string Mail { get; set; }
        public DateTime FechaNacimiento { get; set; }
        //Relaciones
        public DateTime FechaRegistro { get; set; }
        public string NombreCompleto { get; set; }
        public string NombreUbigeo { get; set; }
        public string NombreTipoDocumento { get; set; }
        public string NombreGenero { get; set; }
        public string NombreSala { get; set; }
    }
    public class AST_ClienteERP {
        public int CodCliente { get; set; }
        public int CodTipoDOI { get; set; }
        public int CodOperadorMovil { get; set; }
        public int CodUbigeo { get; set; }
        public string Ubigeo { get; set; }
        public string TipoCliente { get; set; }
        public string FrecuenciaCliente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string ApellidoMaterno { get; set; }
        public string ApellidoPaterno { get; set; }
        public string TipoDOI { get; set; }
        public string DOI { get; set; }
        public string Genero { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string OperadorMovil { get; set; }
        public string Movil { get; set; }
        public string MailPersonal { get; set; }
        public string MailJob { get; set; }
        public string RutaArchivo { get; set; }
        public int CodTipoCliente { get; set; }
        public int CodFrecuenciaCliente { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string CodUsuario { get; set; }
        public int Estado { get; set; }
        public bool Activo { get; set; }
        public int CodSala { get; set; }
    }

    public class AST_ClienteCortesia {
        public string NroDoc { get; set; }
        public string NombreCompleto { get; set; }
    }

}
