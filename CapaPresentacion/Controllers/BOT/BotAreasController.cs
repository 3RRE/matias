using CapaEntidad.BOT.Entities;
using CapaNegocio.Cortesias;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.BOT {
    [seguridad(false)]
    public class BotAreasController : BaseController {
        private readonly BOT_AreaBL areaBL;

        public BotAreasController() {
            areaBL = new BOT_AreaBL();
        }

        #region Views
        public ActionResult Index() {
            return View("~/Views/Bot/Area/Index.cshtml");
        }

        public ActionResult Guardar(int id = 0) {
            BOT_AreaEntidad area = id == 0 ? new BOT_AreaEntidad() : areaBL.ObtenerAreaPorId(id);
            return View("~/Views/Bot/Area/AddEdit.cshtml", area);
        }
        #endregion

        #region Methods
        [HttpPost]
        public JsonResult GetAreas() {
            bool success = false;
            string displayMessage;
            List<BOT_AreaEntidad> data = new List<BOT_AreaEntidad>();

            try {
                data = areaBL.ObtenerAreas();
                success = data.Count > 0;
                displayMessage = success ? "Lista de areas." : "No hay areas registradas.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return JsonResponse(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult GetAreaById(int id) {
            bool success = false;
            string displayMessage;
            BOT_AreaEntidad data = new BOT_AreaEntidad();

            try {
                data = areaBL.ObtenerAreaPorId(id);
                success = data.Existe();
                displayMessage = success ? "Área encontrada." : "No se encontró el área.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return JsonResponse(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult SaveArea(BOT_AreaEntidad area) {
            bool success = false;
            string displayMessage;
            bool isEdit = area.Existe();

            try {
                success = isEdit ? areaBL.ActualizarArea(area) : areaBL.InsertarArea(area);
                displayMessage = success ? "Área guardada correctamente." : "No se pudo guardar el área.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return JsonResponse(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult DeleteArea(int id) {
            bool success = false;
            string displayMessage;

            try {
                success = areaBL.EliminarArea(id);
                displayMessage = success ? "Área eliminada correctamente." : "No se pudo eliminar el área.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return JsonResponse(new { success, displayMessage });
        }
        #endregion
    }
}
