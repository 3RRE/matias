using CapaDatos.GLPI.Mantenedores;
using CapaEntidad.GLPI.Helper;
using CapaEntidad.GLPI.Mantenedores;
using System.Collections.Generic;
using System.Linq;

namespace CapaNegocio.GLPI.Mantenedores {
    public class GLPI_UsuarioBL {
        private readonly GLPI_UsuarioDAL usuarioDAL;

        public GLPI_UsuarioBL() {
            usuarioDAL = new GLPI_UsuarioDAL();
        }

        public List<GLPI_Usuario> ObtenerUsuariosPorAccion(string accion) {
            return usuarioDAL.ObtenerUsuariosPorAccion(accion);
        }

        public List<GLPI_SelectHelper> ObtenerUsuariosPorAccionSelect(string accion) {
            List<GLPI_SelectHelper> options = ObtenerUsuariosPorAccion(accion)
                .Select(x => new GLPI_SelectHelper {
                    IdPadre = x.ObtenerIdPadreSelect(),
                    Valor = x.ObtenerValorSelect(),
                    Texto = x.ObtenerTextoSelect()
                }).ToList();
            return options;
        }
    }
}
