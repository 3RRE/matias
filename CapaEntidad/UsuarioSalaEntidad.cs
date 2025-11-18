using System;
namespace CapaEntidad
{
    public class UsuarioSalaEntidad
    {  
        public Int32 UsuarioSalaId { get; set; } 
        public Int32 SalaId { get; set; } 
        public Int32 UsuarioId { get; set; } 
        public DateTime FechaRegistro { get; set; } 
        public bool Estado { get; set; }
    }
    public class EmpleadoUsuarioEntidad
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
        public string NombreEmpleado { get; set; }
        public int UsuarioID { get; set; }
        public int TipoUsuarioID { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioContraseña { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int FailedAttempts { get; set; }
        public int Estado { get; set; }
        public int EstadoContrasena { get; set; }
    }
}
