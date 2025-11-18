using CapaEntidad.Notificaciones.DTO.Mantenedores;
using CapaEntidad.Notificaciones.Entity.Mantenedores;
using CapaNegocio.Notificaciones.Mantenedores;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Notificaciones.Mantenedores {
    [seguridad(true)]
    public class CredencialWhatsAppController : Controller {
        private readonly CredencialWhatsAppBL credencialBL;
        private readonly SistemaBL sistemaBL;

        public CredencialWhatsAppController() {
            credencialBL = new CredencialWhatsAppBL();
            sistemaBL = new SistemaBL();
        }

        #region Views
        public ActionResult CredencialWhatsAppView() {
            return View("~/Views/Notificaciones/CredencialWhatsApp/CredencialWhatsApp.cshtml");
        }

        public ActionResult AgregarEditarCredencialWhatsAppView(int id = 0) {
            CredencialWhatsAppDto credencial = id == 0 ? new CredencialWhatsAppDto() : credencialBL.ObtenerCredencialWhatsAppPorId(id);
            return View("~/Views/Notificaciones/CredencialWhatsApp/CredencialWhatsAppAddEdit.cshtml", credencial);
        }
        #endregion

        #region Methods
        [HttpPost]
        public JsonResult ObtenerCredencialesWhatsAppPorSistema(int IdSistema) {
            bool success = false;
            string displayMessage;
            List<CredencialWhatsAppDto> data = new List<CredencialWhatsAppDto>();

            try {
                SistemaDto sistema = sistemaBL.ObtenerSistemaPorId(IdSistema);
                if(!sistema.Existe()) {
                    success = false;
                    displayMessage = $"No hay lista de Credenciales de WhatsApp {IdSistema}.";
                    return Json(new { success, displayMessage });
                }
                data = credencialBL.ObtenerCredencialesWhatsAppPorSistema(IdSistema);
                success = data.Count > 0;
                displayMessage = success ? "Lista de Credenciales de WhatsApp." : "No hay lista de Credenciales de WhatsApp.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage, data });
        }

        [seguridad(false)] //debido a que se usa al cargar la vista
        [HttpPost]
        public JsonResult ObtenerCredencialesWhatsApp() {
            bool success = false;
            string displayMesasge;
            List<CredencialWhatsAppDto> data = new List<CredencialWhatsAppDto>();

            try {
                data = credencialBL.ObtenerCredencialesWhatsApp();
                success = data.Count > 0;
                displayMesasge = success ? "Lista de Credenciales de WhatsApp." : "No hay lista de Credenciales de WhatsApp registrados.";
            } catch(Exception ex) {
                displayMesasge = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMesasge, data });
        }

        [HttpPost]
        public JsonResult SaveCredencialWhatsApp(CredencialWhatsAppDto credencialDto) {
            bool success = false;
            string displayMessage;
            bool isEdit = credencialDto.Existe();

            try {
                var credencial = new CredencialWhatsApp {
                    Id = credencialDto.Id,
                    IdSistema = credencialDto.IdSistema,
                    UrlBase = credencialDto.UrlBase,
                    Instancia = credencialDto.Instancia,
                    Token = credencialDto.Token
                };

                success = isEdit ? credencialBL.ActualizarCredencialWhatsApp(credencial) : credencialBL.InsertarCredencialWhatsApp(credencial);
                displayMessage = success ? "Credencial guardada correctamente." : "Ya existe una credencial para el sistema seleccionado.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador.";
            }

            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult EliminarCredencialWhatsApp(int id) {
            bool success = false;
            string displayMessage;

            try {
                success = credencialBL.EliminarCredencialWhatsApp(id);
                displayMessage = success ? "Credencial eliminado correctamente." : "No se pudo eliminar la credencial.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador.";
            }
            return Json(new { success, displayMessage });
        }
        #endregion
    }
}
