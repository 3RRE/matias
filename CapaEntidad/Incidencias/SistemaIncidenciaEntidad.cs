using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Incidencias
{
    public  class SistemaIncidenciaEntidad
    {
        public Int64 idSistema { get; set; }
        public String nombre { get; set; }
        public String descripcion { get; set; }
        public DateTime fecha_creacion { get; set; }
    }
}
