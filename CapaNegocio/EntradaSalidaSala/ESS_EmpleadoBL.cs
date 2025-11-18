using CapaDatos.BUK;
using CapaDatos.EntradaSalidaSala;
using CapaEntidad.BUK;
using CapaEntidad.EntradaSalidaSala;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.EntradaSalidaSala {
    public class ESS_EmpleadoBL {
        private readonly ESS_EmpleadoDAL _essEmpleadoDAL;
        public ESS_EmpleadoBL() {
            _essEmpleadoDAL = new ESS_EmpleadoDAL();
        }
        public List<ESS_EmpleadoEntidad> ListarEmpleado() => _essEmpleadoDAL.ListarEmpleado();
        public List<ESS_EmpleadoEntidad> ListarEmpleadoPorEstado(int estado) => _essEmpleadoDAL.ListarEmpleadoPorEstado(estado);
        public ESS_EmpleadoEntidad ObtenerEmpleadoPorId(int id) => _essEmpleadoDAL.ObtenerEmpleadoPorId(id);
        public int InsertarEmpleado(ESS_EmpleadoEntidad model) => _essEmpleadoDAL.InsertarEmpleado(model);
        public bool EditarEmpleado(ESS_EmpleadoEntidad model) => _essEmpleadoDAL.EditarEmpleado(model);

        public List<ESS_EmpleadoEntidad> ObtenerEmpleadoExternoActivosPorTermino(int idempresa, string term) => _essEmpleadoDAL.ObtenerEmpleadoExternoActivosPorTermino(idempresa, term);
    }
}
