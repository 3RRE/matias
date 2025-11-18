using CapaDatos.GLPI.Mantenedores;
using CapaEntidad.GLPI.Helper;
using CapaEntidad.GLPI.Mantenedores;
using System.Collections.Generic;
using System.Linq;

namespace CapaNegocio.GLPI.Mantenedores {
    public class GLPI_ClasificacionProblemaBL {
        private readonly GLPI_ClasificacionProblemaDAL clasificacionProblemaDAL;

        public GLPI_ClasificacionProblemaBL() {
            clasificacionProblemaDAL = new GLPI_ClasificacionProblemaDAL();
        }

        public List<GLPI_ClasificacionProblema> ObtenerClasificacionProblemas() {
            return clasificacionProblemaDAL.ObtenerClasificacionProblemas();
        }

        public List<GLPI_SelectHelper> ObtenerClasificacionProblemasSelect() {
            List<GLPI_SelectHelper> options = ObtenerClasificacionProblemas()
                .Select(x => new GLPI_SelectHelper {
                    IdPadre = x.ObtenerIdPadreSelect(),
                    Valor = x.ObtenerValorSelect(),
                    Texto = x.ObtenerTextoSelect()
                }).ToList();
            return options;
        }

        public GLPI_ClasificacionProblema ObtenerClasificacionProblemaPorId(int id) {
            return clasificacionProblemaDAL.ObtenerClasificacionProblemaPorId(id);
        }

        public bool InsertarClasificacionProblema(GLPI_ClasificacionProblema clasificacionProblema) {
            return clasificacionProblemaDAL.InsertarClasificacionProblema(clasificacionProblema) > 0;
        }

        public bool ActualizarClasificacionProblema(GLPI_ClasificacionProblema clasificacionProblema) {
            return clasificacionProblemaDAL.ActualizarClasificacionProblema(clasificacionProblema) > 0;
        }

        public bool EliminarClasificacionProblema(int id) {
            return clasificacionProblemaDAL.EliminarClasificacionProblema(id) > 0;
        }
    }
}
