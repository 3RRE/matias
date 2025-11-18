using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class SEG_UsuarioEntidad
    {
        public string NombreEmpleado { get; set; }
        public int UsuarioID { get; set; }
        public int EmpleadoID { get; set; }
        public int TipoUsuarioID { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioContraseña { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int FailedAttempts { get; set; }
        public int Estado { get; set; }
        public int EstadoContrasena { get; set; }
        public string MailJob { get; set; }
        public string UsuarioToken { get; set; }
        public string DOI { get; set; }
        public string foto { get; set; }

        public string RolNombre { get; set; }

        

    }
}
