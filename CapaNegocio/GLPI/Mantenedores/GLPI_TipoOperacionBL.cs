using CapaDatos.GLPI.Mantenedores;
using CapaEntidad.GLPI.Helper;
using CapaEntidad.GLPI.Mantenedores;
using System.Collections.Generic;
using System.Linq;

namespace CapaNegocio.GLPI.Mantenedores {
    public class GLPI_TipoOperacionBL {
        private readonly GLPI_TipoOperacionDAL tipoOperacionDAL;

        public GLPI_TipoOperacionBL() {
            tipoOperacionDAL = new GLPI_TipoOperacionDAL();
        }

        public List<GLPI_TipoOperacion> ObtenerTiposOperacion() {
            return tipoOperacionDAL.ObtenerTiposOperacion();
        }

        public List<GLPI_SelectHelper> ObtenerTiposOperacionSelect() {
            List<GLPI_SelectHelper> options = ObtenerTiposOperacion()
                .Select(x => new GLPI_SelectHelper {
                    IdPadre = x.ObtenerIdPadreSelect(),
                    Valor = x.ObtenerValorSelect(),
                    Texto = x.ObtenerTextoSelect()
                }).ToList();
            return options;
        }

        public GLPI_TipoOperacion ObtenerTipoOperacionPorId(int id) {
            return tipoOperacionDAL.ObtenerTipoOperacionPorId(id);
        }

        public bool InsertarTipoOperacion(GLPI_TipoOperacion tipoOperacion) {
            return tipoOperacionDAL.InsertarTipoOperacion(tipoOperacion) > 0;
        }

        public bool ActualizarTipoOperacion(GLPI_TipoOperacion tipoOperacion) {
            return tipoOperacionDAL.ActualizarTipoOperacion(tipoOperacion) > 0;
        }

        public bool EliminarTipoOperacion(int id) {
            return tipoOperacionDAL.EliminarTipoOperacion(id) > 0;
        }
    }
}
