using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Incidencias
{
    public class IncidenciaEntidad
    {
        public Int64 idIncidencia { get; set; }
        public String titulo { get; set; }
        public String descripcion { get; set; }
        public Int64 idSistema { get; set; }
        public DateTime fecha_registro { get; set; }
    }
}
