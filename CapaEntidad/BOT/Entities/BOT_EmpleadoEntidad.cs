using CapaEntidad.BOT.Enum;
using System;

namespace CapaEntidad.BOT.Entities {
    public class BOT_EmpleadoEntidad {
        public int Id { get; set; }
        public int IdBuk { get; set; }

        //Para el usuario en el bot
        public string NumeroDocumento { get; set; } = string.Empty;
        public string CodigoPaisCelular { get; set; } = string.Empty;
        public string TelefonoParticular { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public BOT_OrigenRegistro OrigenRegistro { get; set; }

        //Informacion del trabajador
        public string TipoDocumento { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string ApellidoMaterno { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public string Nacionalidad { get; set; } = string.Empty;
        public DateTime? FechaNacimiento { get; set; }
        public string EstadoCivil { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Departamento { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public string Distrito { get; set; } = string.Empty;
        public string TelefonoOficina { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string EmailPersonal { get; set; } = string.Empty;
        public string SituacionEducativa { get; set; } = string.Empty;
        public string CodigoFicha { get; set; } = string.Empty;
        public DateTime? FechaIncorporacion { get; set; }
        public string FormaPago { get; set; } = string.Empty;
        public string Banco { get; set; } = string.Empty;
        public string TipoCuenta { get; set; } = string.Empty;
        public string NumeroCuenta { get; set; } = string.Empty;
        public string RegimenPensionario { get; set; } = string.Empty;
        public string MonedaCts { get; set; } = string.Empty;

        //Informacion del trabajo
        public DateTime? FechaInicioTrabajo { get; set; }
        public string TipoContrato { get; set; } = string.Empty;
        public DateTime? FechaFinTrabajo { get; set; }
        public string PlanEps { get; set; } = string.Empty;
        public int IdEmpresaBuk { get; set; }
        public string Empresa { get; set; } = string.Empty;
        public int IdAreaBuk { get; set; }
        public string Area { get; set; } = string.Empty;
        public int IdCargoBuk { get; set; }
        public string Cargo { get; set; } = string.Empty;
        public string Sede { get; set; } = string.Empty;

        //Helper
        public int IdCargo { get; set; }

        public bool Existe() {
            return Id > 0;
        }
    }
}
