using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class ComiteCambios
    {
        public int EmpleadoComiteId { get; set; }

        public int? EmpleadoID { get; set; }
        public string NombreCompleto { get; set; }
        public string Nombres { get; set; }
        public string ApellidosPaterno { get; set; }
        public string ApellidosMaterno { get; set; }

        public bool? Estado { get; set; }
    }
}
