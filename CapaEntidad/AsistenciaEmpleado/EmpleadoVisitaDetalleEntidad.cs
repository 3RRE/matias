using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.AsistenciaEmpleado
{
    public class EmpleadoVisitaDetalleEntidad
    {
		public Int64 visd_id { get; set; }
		public Int64 visita_id { get; set; }
		public string nombre { get; set; }
		public string descripcion { get; set; }
		public string imagen { get; set; }
		public string imagen_str { get; set; }
		public DateTime fechaRegistro { get; set; }
	}
}
