using CapaEntidad.Notificaciones.DTO.Mantenedores;
using CapaNegocio;
using CapaNegocio.Notificaciones.Mantenedores;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Notificaciones.Mantenedores {
    [seguridad(true)]
    public class SistemaController : Controller {
        private readonly SistemaBL sistemaBL;
        private readonly SalaBL salaBL;

        public SistemaController() {
            sistemaBL = new SistemaBL();
            salaBL = new SalaBL();
        }

        #region Views
        public ActionResult SistemaView() {
            return View("~/Views/Notificaciones/Sistema/Sistema.cshtml");
        }

        public ActionResult AgregarEditarSistemaView(int id = 0) {
            SistemaDto sistema = id == 0 ? new SistemaDto() : sistemaBL.ObtenerSistemaPorId(id);
            return View("~/Views/Notificaciones/Sistema/SistemaAddEdit.cshtml", sistema);
        }
        #endregion

        #region Methods
        [seguridad(false)] //debido a que se usa al cargar la vista
        [HttpPost]
        public JsonResult ObtenerSistemas() {
            bool success = false;
            string displayMessage;
            List<SistemaDto> data = new List<SistemaDto>();

            try {
                data = sistemaBL.ObtenerSistemas();
                success = data.Count > 0;
                displayMessage = success ? "Lista de sistemas." : "No hay sistemas registrados.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult ObtenerSistemaPorId(int id) {
            bool success = false;
            string displayMessage;
            SistemaDto data = new SistemaDto();

            try {
                data = sistemaBL.ObtenerSistemaPorId(id);
                success = data.Existe();
                displayMessage = success ? "Sistema encontrado." : "No se encontró el sistema.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult SaveSistema(CapaEntidad.Notificaciones.Entity.Mantenedores.Sistema sistema) {
            bool success = false;
            string displayMessage;
            bool isEdit = sistema.Existe();

            try {
                success = isEdit ? sistemaBL.ActualizarSistema(sistema) : sistemaBL.InsertarSistema(sistema);
                displayMessage = success ? "Sistema guardado correctamente." : "No se pudo guardar el sistema.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador.";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult EliminarSistema(int id) {
            bool success = false;
            string displayMessage;

            try {
                success = sistemaBL.EliminarSistema(id);
                displayMessage = success ? "Sistema eliminado correctamente." : "No se pudo elimianr el sistema.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador.";
            }

            return Json(new { success, displayMessage });
        }
        #endregion
    }
}
