using CapaEntidad;
using CapaEntidad.BOT.Entities;
using CapaNegocio;
using CapaNegocio.Cortesias;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.BOT {
    [seguridad(false)]
    public class BotContactosController : BaseController {
        private readonly BOT_ContactoBL contactoBL;
        private readonly BOT_CargoBL cargoBL;
        private readonly EmpresaBL empresaBL;

        public BotContactosController() {
            contactoBL = new BOT_ContactoBL();
            cargoBL = new BOT_CargoBL();
            empresaBL = new EmpresaBL();
        }

        #region Views
        public ActionResult Index() {
            return View("~/Views/Bot/Contacto/Index.cshtml");
        }

        public ActionResult Guardar(int id = 0) {
            BOT_ContactoEntidad contacto = id == 0 ? new BOT_ContactoEntidad() : contactoBL.ObtenerContactoPorId(id);
            return View("~/Views/Bot/Contacto/AddEdit.cshtml", contacto);
        }
        #endregion

        #region Methods
        [HttpPost]
        public JsonResult GetContactos() {
            bool success = false;
            string displayMessage;
            List<BOT_ContactoEntidad> data = new List<BOT_ContactoEntidad>();

            try {
                data = contactoBL.ObtenerContactos();
                success = data.Count > 0;
                displayMessage = success ? "Lista de contactos." : "No hay contactos registrados.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return JsonResponse(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult GetContactoById(int id) {
            bool success = false;
            string displayMessage;
            BOT_ContactoEntidad data = new BOT_ContactoEntidad();

            try {
                data = contactoBL.ObtenerContactoPorId(id);
                success = data.Existe();
                displayMessage = success ? "Contacto encontrado." : "No se encontró el contacto.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return JsonResponse(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult SaveContacto(BOT_ContactoEntidad contacto) {
            bool success = false;
            string displayMessage = "";
            bool isEdit = contacto.Existe();

            try {
                BOT_CargoEntidad cargo = cargoBL.ObtenerCargoPorId(contacto.IdCargo);
                if(!cargo.Existe()) {
                    success = false;
                    displayMessage = $"No existe el cargo con código '{contacto.IdCargo}' al que intenta vincular al contacto.";
                    return JsonResponse(new { success, displayMessage });
                }

                success = isEdit ? contactoBL.ActualizarContacto(contacto) : contactoBL.InsertarContacto(contacto);
                displayMessage = success ? "Contacto guardado correctamente." : "No se pudo guardar el contacto.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return JsonResponse(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult DeleteContacto(int id) {
            bool success = false;
            string displayMessage;

            try {
                success = contactoBL.EliminarContacto(id);
                displayMessage = success ? "Contacto eliminado correctamente." : "No se pudo eliminar el contacto.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return JsonResponse(new { success, displayMessage });
        }
        #endregion
    }
}
