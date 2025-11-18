using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers {
    public class SalaMaestraController : Controller {
        private readonly SalaMaestraBL _salaMaestraBL;

        public SalaMaestraController() {
            _salaMaestraBL = new SalaMaestraBL();
        }

        #region Vistas
        public ActionResult SalaMaestraListadoVista() {
            return View();
        }

        public ActionResult SalaMaestraInsertarVista() {
            return View();
        }

        public ActionResult SalaMaestraActualizarVista(int id = 0) {
            var salaMaestra = _salaMaestraBL.ObtenerSalaMaestraPorCodigo(id);
            if(salaMaestra == null || salaMaestra.Nombre == null) {
                return RedirectToAction("SalaMaestraListadoVista");
            }
            return View(salaMaestra);
        }
        #endregion

        #region Metodos
        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarTodasSalasMaestrasJSON() {
            bool success = false;
            List<SalaMaestraEntidad> salasMaestras = new List<SalaMaestraEntidad>();
            string displayMessage;

            try {
                salasMaestras = _salaMaestraBL.ObtenerTodasLasSalasMaestras();
                success = true;
                displayMessage = "Lista de Salas Maestras.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = salasMaestras.ToList(), displayMessage }, JsonRequestBehavior.AllowGet);
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarTodasSalasMaestrasActivasJSON() {
            bool success = false;
            List<SalaMaestraEntidad> salasMaestras = new List<SalaMaestraEntidad>();
            string displayMessage;

            try {
                salasMaestras = _salaMaestraBL.ObtenerTodasLasSalasMaestrasActivas();
                success = true;
                displayMessage = "Lista de Salas Maestras.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = salasMaestras.ToList(), displayMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerSalaMaestraPorCodigoJSON(int codigoSalaMaestra) {
            bool success = false;
            SalaMaestraEntidad salasMaestra = new SalaMaestraEntidad();
            string displayMessage;

            try {
                salasMaestra = _salaMaestraBL.ObtenerSalaMaestraPorCodigo(codigoSalaMaestra);
                success = true;
                displayMessage = "Sala Maestra encontrada.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = salasMaestra, displayMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SalaMaestraInsertarJSON(SalaMaestraEntidad salaMaestra) {
            bool success = false;
            string displayMessage;
            try {
                success = _salaMaestraBL.InsertarSalaMaestra(salaMaestra);
                displayMessage = success ? "Sala Maestra creada correctamente" : "No se pudo crear la Sala Maestra, llame al Administrador.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult SalaMaestraActualizarJSON(SalaMaestraEntidad salaMaestra) {
            bool success = false;
            string displayMessage;
            try {
                success = _salaMaestraBL.ActualizarSalaMaestra(salaMaestra);
                displayMessage = success ? "Sala Maestra actualizada correctamente" : "No se pudo actualizar la Sala Maestra, llame al Administrador.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult SalaMaestraActualizarEstadoJSON(SalaMaestraEntidad salaMaestra) {
            bool success = false;
            string displayMessage;
            try {
                success = _salaMaestraBL.ActualizarEstadoDeSalaMaestra(salaMaestra.CodSalaMaestra, salaMaestra.Estado);
                displayMessage = success ? "Estado actualizado correctamente" : "No se pudo actualizar el estado, llame al Administrador.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult SalaMaestraEliminarJSON(int codSalaMaestra) {
            bool success = false;
            string displayMessage;
            try {
                success = _salaMaestraBL.EliminarSalaMaestra(codSalaMaestra);
                displayMessage = success ? "Sala Maestra eliminada correctamente" : "No se pudo eliminar la Sala Maestra, llame al Administrador.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }
            return Json(new { success, displayMessage });
        }
        #endregion
    }
}