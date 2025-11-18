using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class TecnicoEntidad
    {
        public int TecnicoId { get; set; }
        public int EmpleadoId { get; set; }
        public int AreaTechId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int NivelTecnicoId { get; set; }
        public bool Estado { get; set; }
        public string NombreCompleto { get; set; }
    }
}
