using CapaDatos.BUK;
using CapaEntidad.BUK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.BUK {
    public class BUK_EmpleadoBL {
        private readonly BUK_EmpleadoDAL _bukEmpleadoDAL = new BUK_EmpleadoDAL();
        public List<BUK_EmpleadoEntidad> ObtenerEmpleadosActivos(int idempresa) => _bukEmpleadoDAL.ObtenerEmpleadosActivos(idempresa);
        public List<BUK_EmpleadoEntidad> ObtenerEmpleadosActivosPorTermino(int idempresa, string term) => _bukEmpleadoDAL.ObtenerEmpleadosActivosPorTermino(idempresa,term);
        public List<BUK_EmpleadoEntidad> ObtenerEmpleadosActivosPorTerminoSinEmpresa(string term) => _bukEmpleadoDAL.ObtenerEmpleadosActivosPorTerminoSinEmpresa(term);
        public List<BUK_EmpleadoEntidad> ObtenerEmpleadosActivosPorTerminoxCargo(string term, int[] idcargo) => _bukEmpleadoDAL.ObtenerEmpleadosActivosPorTerminoxCargo(term,idcargo);
    }
}
