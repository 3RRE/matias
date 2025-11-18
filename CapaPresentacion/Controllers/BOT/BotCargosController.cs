using CapaEntidad.BOT.Entities;
using CapaNegocio.Cortesias;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.BOT {
    [seguridad(false)]
    public class BotCargosController : BaseController {
        private readonly BOT_CargoBL cargoBL;
        private readonly BOT_AreaBL areaBL;

        public BotCargosController() {
            cargoBL = new BOT_CargoBL();
            areaBL = new BOT_AreaBL();
        }

        #region Views
        public ActionResult Index() {
            return View("~/Views/Bot/Cargo/Index.cshtml");
        }

        public ActionResult Guardar(int id = 0) {
            BOT_CargoEntidad cargo = id == 0 ? new BOT_CargoEntidad() : cargoBL.ObtenerCargoPorId(id);
            return View("~/Views/Bot/Cargo/AddEdit.cshtml", cargo);
        }
        #endregion

        #region Methods
        [HttpPost]
        public JsonResult GetCargos() {
            bool success = false;
            string displayMessage;
            List<BOT_CargoEntidad> data = new List<BOT_CargoEntidad>();

            try {
                data = cargoBL.ObtenerCargos();
                success = data.Count > 0;
                displayMessage = success ? "Lista de cargos." : "No hay cargos registrados.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return JsonResponse(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult GetCargoById(int id) {
            bool success = false;
            string displayMessage;
            BOT_CargoEntidad data = new BOT_CargoEntidad();

            try {
                data = cargoBL.ObtenerCargoPorId(id);
                success = data.Existe();
                displayMessage = success ? "Cargo encontrado." : "No se encontró el cargo.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return JsonResponse(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult SaveCargo(BOT_CargoEntidad cargo) {
            bool success = false;
            string displayMessage = "";
            bool isEdit = cargo.Existe();

            try {
                BOT_AreaEntidad area = areaBL.ObtenerAreaPorId(cargo.IdArea);
                if(!area.Existe()) {
                    success = false;
                    displayMessage = $"No existe el área con código '{cargo.IdArea}' al que intenta vincular al cargo.";
                    return Json(new { success, displayMessage });
                }

                success = isEdit ? cargoBL.ActualizarCargo(cargo) : cargoBL.InsertarCargo(cargo);
                displayMessage = success ? "Cargo guardado correctamente." : "No se pudo guardar el cargo.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return JsonResponse(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult DeleteCargo(int id) {
            bool success = false;
            string displayMessage;

            try {
                success = cargoBL.EliminarCargo(id);
                displayMessage = success ? "Cargo eliminado correctamente." : "No se pudo eliminar el cargo.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return JsonResponse(new { success, displayMessage });
        }
        #endregion
    }
}
