using CapaEntidad.GLPI.Mantenedores;
using CapaNegocio.GLPI.Mantenedores;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.GLPI.Mantenedores {
    [seguridad(false)]
    public class GlpiTiposOperacionController : Controller {
        private readonly GLPI_TipoOperacionBL tipoOperacionBL;

        public GlpiTiposOperacionController() {
            tipoOperacionBL = new GLPI_TipoOperacionBL();
        }

        #region Methods
        [HttpPost]
        public JsonResult GetTiposOperacion() {
            bool success = false;
            string displayMessage;
            List<GLPI_TipoOperacion> data = new List<GLPI_TipoOperacion>();

            try {
                data = tipoOperacionBL.ObtenerTiposOperacion();
                success = data.Count > 0;
                displayMessage = success ? "Lista de tipos de operación." : "No hay tipos de operación registrados.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult GetTipoOperacionById(int id) {
            bool success = false;
            string displayMessage;
            GLPI_TipoOperacion data = new GLPI_TipoOperacion();

            try {
                data = tipoOperacionBL.ObtenerTipoOperacionPorId(id);
                success = data.Existe();
                displayMessage = success ? "Tipo de operación encontrada." : "No se encontró el tipo de operación.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult SaveTipoOperacion(GLPI_TipoOperacion tipoOperacion) {
            bool success = false;
            string displayMessage;
            bool isEdit = tipoOperacion.Existe();

            try {
                success = isEdit ? tipoOperacionBL.ActualizarTipoOperacion(tipoOperacion) : tipoOperacionBL.InsertarTipoOperacion(tipoOperacion);
                displayMessage = success ? "Tipo de operación guardada correctamente." : "No se pudo guardar el tipo de operación.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult DeleteTipoOperacion(int id) {
            bool success = false;
            string displayMessage;

            try {
                success = tipoOperacionBL.EliminarTipoOperacion(id);
                displayMessage = success ? "Tipo de operación eliminada correctamente." : "No se pudo eliminar el tipo de operación.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage });
        }
        #endregion
    }
}