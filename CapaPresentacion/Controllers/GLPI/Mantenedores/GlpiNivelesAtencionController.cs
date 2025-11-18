using CapaEntidad.GLPI.Mantenedores;
using CapaNegocio.GLPI.Mantenedores;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.GLPI.Mantenedores {
    [seguridad(false)]
    public class GlpiNivelesAtencionController : Controller {
        private readonly GLPI_NivelAtencionBL niveleAtencionBL;

        public GlpiNivelesAtencionController() {
            niveleAtencionBL = new GLPI_NivelAtencionBL();
        }

        #region Methods
        [HttpPost]
        public JsonResult GetNivelesAtencion() {
            bool success = false;
            string displayMessage;
            List<GLPI_NivelAtencion> data = new List<GLPI_NivelAtencion>();

            try {
                data = niveleAtencionBL.ObtenerNivelesAtencion();
                success = data.Count > 0;
                displayMessage = success ? "Lista de niveles de atención." : "No hay niveles de atención registrados.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult GetNivelAtencionById(int id) {
            bool success = false;
            string displayMessage;
            GLPI_NivelAtencion data = new GLPI_NivelAtencion();

            try {
                data = niveleAtencionBL.ObtenerNivelAtencionPorId(id);
                success = data.Existe();
                displayMessage = success ? "Nivel de atención encontrado." : "No se encontró el nivel de atención.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult SaveNivelAtencion(GLPI_NivelAtencion niveleAtencion) {
            bool success = false;
            string displayMessage;
            bool isEdit = niveleAtencion.Existe();

            try {
                success = isEdit ? niveleAtencionBL.ActualizarNivelAtencion(niveleAtencion) : niveleAtencionBL.InsertarNivelAtencion(niveleAtencion);
                displayMessage = success ? "Nivel de atención guardado correctamente." : "No se pudo guardar el nivel de atención.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult DeleteNivelAtencion(int id) {
            bool success = false;
            string displayMessage;

            try {
                success = niveleAtencionBL.EliminarNivelAtencion(id);
                displayMessage = success ? "Nivel de atención eliminado correctamente." : "No se pudo eliminar el nivel de atención.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage });
        }
        #endregion
    }
}
