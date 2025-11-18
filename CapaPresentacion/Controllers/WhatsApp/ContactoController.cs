using CapaEntidad.WhatsApp;
using CapaNegocio.WhatsApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.WhatsApp
{
    [seguridad]
    public class ContactoController : Controller
    {
        WSP_ContactoBL _contactoBL = new WSP_ContactoBL();

        // GET: Contacto
        public ActionResult ContactoListadoVista() {
            return View();
        }

        public ActionResult ContactoInsertarVista() {
            return View();
        }

        public ActionResult ContactoActualizarVista(int id = 0) {
            var contacto = _contactoBL.ObtenerContactoPorIdContacto(id);
            if(contacto == null || contacto.Nombre == null) {
                return RedirectToAction("ContactoListadoVista");
            }
            return View(contacto);
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarTodosLosContactosJSON() {
            bool success = false;
            List<WSP_ContactoEntidad> contactos = new List<WSP_ContactoEntidad>();
            string displayMessage;

            try {
                contactos = _contactoBL.ObtenerTodosLosContactos();
                success = true;
                displayMessage = "Lista de Contactos.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = contactos.ToList(), displayMessage }, JsonRequestBehavior.AllowGet);
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarLosContactosActivosJSON() {
            bool success = false;
            List<WSP_ContactoEntidad> contactos = new List<WSP_ContactoEntidad>();
            string displayMessage;

            try {
                contactos = _contactoBL.ObtenerContactosActivos();
                success = true;
                displayMessage = "Lista de Contactos Activos.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = contactos.ToList(), displayMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerContactoPorIdJSON(WSP_ContactoEntidad contact) {
            bool success = false;
            WSP_ContactoEntidad contacto = new WSP_ContactoEntidad();
            string displayMessage;

            try {
                contacto = _contactoBL.ObtenerContactoPorIdContacto(contact.IdContacto);
                success = true;
                displayMessage = "Contacto econtrado.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = contacto, displayMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertarContactoJSON(WSP_ContactoEntidad contacto) {
            bool success = false;
            string displayMessage;
            try {
                success = _contactoBL.InsertarContacto(contacto);
                displayMessage = success ? "Contacto agregado correctamente." : "No se pudo agregar el Contacto, llame al Administrador.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult ActualizarContactoJSON(WSP_ContactoEntidad contacto) {
            bool success = false;
            string displayMessage;
            try {
                success = _contactoBL.ActualizarContactoPorIdContacto(contacto);
                displayMessage = success ? "Contacto actualizado correctamente." : "No se pudo actualizar el Contacto, llame al Administrador.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult ActualizarEstadoContactoJSON(WSP_ContactoEntidad contacto) {
            bool success = false;
            string displayMessage;
            try {
                success = _contactoBL.ActualizarEstadoDeSalaMaestraPorIdContacto(contacto.IdContacto, contacto.Estado);
                displayMessage = success ? "Estado actualizado correctamente." : "No se pudo actualizar el estado, llame al Administrador.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult EliminarContactoJSON(WSP_ContactoEntidad contacto) {
            bool success = false;
            string displayMessage;
            try {
                success = _contactoBL.EliminarContactoPorIdContacto(contacto.IdContacto);
                displayMessage = success ? "Contacto eliminado correctamente" : "No se pudo eliminar el Contacto, llame al Administrador.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }
            return Json(new { success, displayMessage });
        }
    }
}