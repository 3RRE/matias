using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class SEG_EmpleadoEntidad
    {
        public string NombreCompleto { get; set; }
        public int EmpleadoID { get; set; }
        public string Nombres { get; set; }
        public string ApellidosPaterno { get; set; }
        public string ApellidosMaterno { get; set; }
        public int CargoID { get; set; }
        public string CargoNombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Direccion { get; set; }
        public int DOIID { get; set; }
        public string DOIIDNombre { get; set; }
        public string DOI { get; set; }
        public string Telefono { get; set; }
        public string Movil { get; set; }
        public string Genero { get; set; }
        public string MailJob { get; set; }
        public string MailPersonal { get; set; }
        public int EstadoEmpleado { get; set; }
        public DateTime FechaAlta { get; set; }
        public string emp_foto { get; set; }
        public string emp_foto_estado { get; set; }
        public string UsuarioNombre { get; set; }
        public int UsuarioID { get; set; }
        public string AreaTrabajo { get; set; }
        public string Empresa { get; set; }
        public string Ruc { get; set; }
    }

    public class EmpleadoEncriptacion : SEG_EmpleadoEntidad
    {
        public string UsuarioNombre { get; set; }
        public string UsuarioPassword { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public int IdUsuarioEncriptacion { get; set; }
        public bool Estado { get; set; }
    }

    public class sp_SiscopBD
    {
        public string terminal { get; set; }
        public string nroDocumento { get; set; }
        public DateTime fechahora { get; set; }
        public int tarjetaProximidad { get; set; }
        public int tipoBusqueda { get; set; }

    }

    public class empleadoBD
    {
        public string CO_TRAB { get; set; }
        public string NO_APEL_PATE { get; set; }
        public string NO_APEL_MATE { get; set; }
        public string NO_TRAB { get; set; }
        public string NO_DIRE_MAI1 { get; set; }
        public DateTime FE_INGR_CORP { get; set; }
    }

    public class ApiFotosData {
        public double distance { get; set; }
        public string name { get; set; }
        public double percentage { get; set; }
    }
}
