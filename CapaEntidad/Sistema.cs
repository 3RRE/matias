using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Sistema
    {
        public int SistemaId { get; set; }

        public string Descripcion { get; set; }

        public string Version { get; set; }

        public DateTime? FechaRegistro { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public bool? Estado { get; set; }
        
    }
}
