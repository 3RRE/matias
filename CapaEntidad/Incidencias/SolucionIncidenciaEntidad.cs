using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Incidencias
{
    public class SolucionIncidenciaEntidad
    {
        public Int64 idSolucion { get; set; }
        public String nombre { get; set; }
        public String descripcion { get; set; }
        public Int64 idIncidencia { get; set; }
        public DateTime fecha_registro { get; set; }
    }
}
