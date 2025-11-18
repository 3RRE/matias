using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ControlAcceso
{
    public class CAL_MenorDeEdadEntidad
    {
        public int IdMenor { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Doi { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int Sala { get; set; }
        public string NombreSala { get; set; }
        public int EmpleadoID { get; set; }
        public string NombreEmpleado { get; set; }
        public int Estado { get; set; }
        public int TipoDoi { get; set; }
        public string NombreTipoDoi { get; set; }
    }
}
