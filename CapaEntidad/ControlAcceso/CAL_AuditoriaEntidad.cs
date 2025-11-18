using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ControlAcceso
{
    public class CAL_AuditoriaEntidad
    {
        //BD
        public int idAuditoria { get; set; }
        public DateTime fecha { get; set; }
        public int usuario { get; set; }
        public string usuarioNombre { get; set; }
        public string dni { get; set; }
        public string tipo { get; set; }
        public string nombre { get; set; }
        public string sala { get; set; }
        public string codigo { get; set; }
        public string observacion { get; set; }


        //Datos


        public long BusquedaId { get; set; }
        public string TipoCliente { get; set; }
        public string Dni { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string FechaRegistroString { get; set; }
        public int UsuarioId { get; set; }
        public int SalaId { get; set; }
        public string Cliente { get; set; }
        public string Empleado { get; set; }
        public string RazonSocial { get; set; }
        public string NombreUsuario { get; set; }
        public string username { get; set; }
        public string NombreSala { get; set; }

    }
}
