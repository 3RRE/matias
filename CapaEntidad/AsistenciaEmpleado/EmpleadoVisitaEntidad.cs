using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.AsistenciaEmpleado
{
    public class EmpleadoVisitaEntidad
    {
        public Int64 vis_id { get; set; }
        public int empleado_id { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public string imei { get; set; }
        public DateTime fechaRegistro { get; set; }
        public int sala_id { get; set; }
    }

    public class EmpleadoVisitaEmpleadoEntidad
    {
        public Int64 vis_id { get; set; }
        public int empleado_id { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public string imei { get; set; }
        public DateTime fechaRegistro { get; set; }
        public int sala_id { get; set; }

        public string emp_nombre { get; set; }
        public string emp_ape_paterno { get; set; }
        public string emp_ape_materno { get; set; }

    }

 
}
