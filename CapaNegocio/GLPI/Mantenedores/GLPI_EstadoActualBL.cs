using CapaDatos.GLPI.Mantenedores;
using CapaEntidad.GLPI.Helper;
using CapaEntidad.GLPI.Mantenedores;
using System.Collections.Generic;
using System.Linq;

namespace CapaNegocio.GLPI.Mantenedores {
    public class GLPI_EstadoActualBL {
        private readonly GLPI_EstadoActualDAL estadoActualDAL;

        public GLPI_EstadoActualBL() {
            estadoActualDAL = new GLPI_EstadoActualDAL();
        }

        public List<GLPI_EstadoActual> ObtenerEstadosActuales() {
            return estadoActualDAL.ObtenerEstadosActuales();
        }

        public List<GLPI_SelectHelper> ObtenerEstadosActualesSelect() {
            List<GLPI_SelectHelper> options = ObtenerEstadosActuales()
                .Select(x => new GLPI_SelectHelper {
                    IdPadre = x.ObtenerIdPadreSelect(),
                    Valor = x.ObtenerValorSelect(),
                    Texto = x.ObtenerTextoSelect()
                }).ToList();
            return options;
        }

        public GLPI_EstadoActual ObtenerEstadoActualPorId(int id) {
            return estadoActualDAL.ObtenerEstadoActualPorId(id);
        }

        public bool InsertarEstadoActual(GLPI_EstadoActual estadoActual) {
            return estadoActualDAL.InsertarEstadoActual(estadoActual) > 0;
        }

        public bool ActualizarEstadoActual(GLPI_EstadoActual estadoActual) {
            return estadoActualDAL.ActualizarEstadoActual(estadoActual) > 0;
        }

        public bool EliminarEstadoActual(int id) {
            return estadoActualDAL.EliminarEstadoActual(id) > 0;
        }
    }
}
