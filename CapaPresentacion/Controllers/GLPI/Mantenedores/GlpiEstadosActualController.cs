using CapaEntidad.GLPI.Mantenedores;
using CapaNegocio.GLPI.Mantenedores;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.GLPI.Mantenedores {
    [seguridad(false)]
    public class GlpiEstadosActualController : Controller {
        private readonly GLPI_EstadoActualBL estadoActualBL;

        public GlpiEstadosActualController() {
            estadoActualBL = new GLPI_EstadoActualBL();
        }

        #region Methods
        [HttpPost]
        public JsonResult GetEstadosActuales() {
            bool success = false;
            string displayMessage;
            List<GLPI_EstadoActual> data = new List<GLPI_EstadoActual>();

            try {
                data = estadoActualBL.ObtenerEstadosActuales();
                success = data.Count > 0;
                displayMessage = success ? "Lista de estados actuales." : "No hay estados actuales registrados.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult GetEstadoActualById(int id) {
            bool success = false;
            string displayMessage;
            GLPI_EstadoActual data = new GLPI_EstadoActual();

            try {
                data = estadoActualBL.ObtenerEstadoActualPorId(id);
                success = data.Existe();
                displayMessage = success ? "Estado actual encontrado." : "No se encontró el estado actual.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult SaveEstadoActual(GLPI_EstadoActual partida) {
            bool success = false;
            string displayMessage;
            bool isEdit = partida.Existe();

            try {
                success = isEdit ? estadoActualBL.ActualizarEstadoActual(partida) : estadoActualBL.InsertarEstadoActual(partida);
                displayMessage = success ? "Estado actual guardado correctamente." : "No se pudo guardar el estado actual.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult DeleteEstadoActual(int id) {
            bool success = false;
            string displayMessage;

            try {
                success = estadoActualBL.EliminarEstadoActual(id);
                displayMessage = success ? "Estado actual eliminado correctamente." : "No se pudo eliminar el estado actual.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage });
        }
        #endregion
    }
}
