using CapaEntidad.GLPI.Mantenedores;
using CapaNegocio.GLPI.Mantenedores;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.GLPI.Mantenedores {
    [seguridad(false)]
    public class GlpiClasificacionProblemasController : Controller {
        private readonly GLPI_ClasificacionProblemaBL clasificacionProblemaBL;

        public GlpiClasificacionProblemasController() {
            clasificacionProblemaBL = new GLPI_ClasificacionProblemaBL();
        }

        #region Methods
        [HttpPost]
        public JsonResult GetClasificacionProblemas() {
            bool success = false;
            string displayMessage;
            List<GLPI_ClasificacionProblema> data = new List<GLPI_ClasificacionProblema>();

            try {
                data = clasificacionProblemaBL.ObtenerClasificacionProblemas();
                success = data.Count > 0;
                displayMessage = success ? "Lista de clasificaciones de problemas." : "No hay clasificaciones de problemas registradas.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult GetClasificacionProblemaById(int id) {
            bool success = false;
            string displayMessage;
            GLPI_ClasificacionProblema data = new GLPI_ClasificacionProblema();

            try {
                data = clasificacionProblemaBL.ObtenerClasificacionProblemaPorId(id);
                success = data.Existe();
                displayMessage = success ? "Clasificación de problema encontrada." : "No se encontró la clasificación de problema.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult SaveClasificacionProblema(GLPI_ClasificacionProblema clasificacionProblema) {
            bool success = false;
            string displayMessage;
            bool isEdit = clasificacionProblema.Existe();

            try {
                success = isEdit ? clasificacionProblemaBL.ActualizarClasificacionProblema(clasificacionProblema) : clasificacionProblemaBL.InsertarClasificacionProblema(clasificacionProblema);
                displayMessage = success ? "Clasificación de problema guardada correctamente." : "No se pudo guardar la clasificación de problema.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult DeleteClasificacionProblema(int id) {
            bool success = false;
            string displayMessage;

            try {
                success = clasificacionProblemaBL.EliminarClasificacionProblema(id);
                displayMessage = success ? "Clasificación de problema eliminada correctamente." : "No se pudo eliminar la clasificación de problema.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage });
        }
        #endregion
    }
}
