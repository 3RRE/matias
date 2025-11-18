using CapaEntidad;
using CapaEntidad.WhatsApp;
using CapaNegocio;
using CapaNegocio.WhatsApp;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.WhatsApp {
    [seguridad]
    public class InstanciaController : Controller {
        private readonly WSP_InstanciaUltraMsgBL _instanciaBL;
        private readonly SalaBL _salaBL;

        public InstanciaController() {
            _instanciaBL = new WSP_InstanciaUltraMsgBL();
            _salaBL= new SalaBL();
        }

        public ActionResult InstanciaListadoVista() {
            return View();
        }

        public ActionResult InstanciaInsertarVista() {
            return View();
        }

        public ActionResult InstanciaActualizarVista(int id = 0) {
            var instancia = _instanciaBL.ObtenerInstanciaPorIdInstancia(id);
            if(instancia == null || instancia.Instancia == null) {
                return RedirectToAction("InstanciaListadoVista");
            }
            return View(instancia);
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarTodasLasInstanciasJSON() {
            bool success = false;
            List<WSP_InstanciaUltraMsgEntidad> instancias = new List<WSP_InstanciaUltraMsgEntidad>();
            string displayMessage;
            int usuarioId = Convert.ToInt32(Session["UsuarioID"]);

            try {
                instancias = _instanciaBL.ObtenerTodasLasInstancias(usuarioId);
                success = true;
                displayMessage = "Lista de Instancias.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = instancias, displayMessage });
        }

        [HttpPost]
        public JsonResult ObtenerInstanciaPorIdJSON(int idInstancia) {
            bool success = false;
            WSP_InstanciaUltraMsgEntidad instancia = new WSP_InstanciaUltraMsgEntidad();
            string displayMessage;

            try {
                instancia = _instanciaBL.ObtenerInstanciaPorIdInstancia(idInstancia);
                success = instancia.IdInstanciaUltraMsg > 0;
                displayMessage = success ? "Instancia econtrada." : "No se encontro la instancia";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = instancia, displayMessage });
        }

        [HttpPost]
        public JsonResult InsertarInstanciaJSON(WSP_InstanciaUltraMsgEntidad instancia) {
            bool success = false;
            string displayMessage;
            try {
                SalaEntidad sala = _salaBL.ObtenerSalaPorCodigo(instancia.CodSala);
                if(sala.CodSala <= 0) {
                    displayMessage = "No existe la sala seleccionada.";
                    return Json(new { success, displayMessage });
                }

                if(_instanciaBL.SalaTieneInstancia(instancia.CodSala)) {
                    displayMessage = $"La sala {sala.Nombre} ya tiene una instancia.";
                    return Json(new { success, displayMessage });
                }

                success = _instanciaBL.InsertarInstancia(instancia);
                displayMessage = success ? "Instancia agregada correctamente." : "No se pudo agregar la Instancia, llame al Administrador.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult ActualizarInstanciaJSON(WSP_InstanciaUltraMsgEntidad instancia) {
            bool success = false;
            string displayMessage;
            try {
                success = _instanciaBL.ActualizarInstancia(instancia);
                displayMessage = success ? "Instancia actualizada correctamente." : "No se pudo actualizar la Instancia, llame al Administrador.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult EliminarInstanciaJSON(int idInstancia) {
            bool success = false;
            string displayMessage;
            try {
                success = _instanciaBL.EliminarInstancia(idInstancia);
                displayMessage = success ? "Instancia eliminada correctamente" : "No se pudo eliminar la Instancia, llame al Administrador.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }
            return Json(new { success, displayMessage });
        }
    }
}