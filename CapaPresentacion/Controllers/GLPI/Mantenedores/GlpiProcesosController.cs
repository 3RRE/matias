using CapaEntidad.GLPI.Mantenedores;
using CapaNegocio.GLPI.Mantenedores;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.GLPI.Mantenedores {
    [seguridad(false)]
    public class GlpiProcesosController : Controller {
        private readonly GLPI_ProcesoBL procesoBL;

        public GlpiProcesosController() {
            procesoBL = new GLPI_ProcesoBL();
        }

        #region Methods
        [HttpPost]
        public JsonResult GetProcesos() {
            bool success = false;
            string displayMessage;
            List<GLPI_Proceso> data = new List<GLPI_Proceso>();

            try {
                data = procesoBL.ObtenerProcesos();
                success = data.Count > 0;
                displayMessage = success ? "Lista de procesos." : "No hay procesos registrados.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult GetProcesoById(int id) {
            bool success = false;
            string displayMessage;
            GLPI_Proceso data = new GLPI_Proceso();

            try {
                data = procesoBL.ObtenerProcesoPorId(id);
                success = data.Existe();
                displayMessage = success ? "Proceso encontrado." : "No se encontró el proceso.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult SaveProceso(GLPI_Proceso proceso) {
            bool success = false;
            string displayMessage;
            bool isEdit = proceso.Existe();

            try {
                success = isEdit ? procesoBL.ActualizarProceso(proceso) : procesoBL.InsertarProceso(proceso);
                displayMessage = success ? "Proceso guardado correctamente." : "No se pudo guardar el proceso.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult DeleteProceso(int id) {
            bool success = false;
            string displayMessage;

            try {
                success = procesoBL.EliminarProceso(id);
                displayMessage = success ? "Proceso eliminado correctamente." : "No se pudo eliminar el proceso.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage });
        }
        #endregion
    }
}
