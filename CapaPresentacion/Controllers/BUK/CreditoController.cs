using CapaEntidad.BUK;
using CapaEntidad.Response;
using CapaNegocio.BUK;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.BUK {
    [seguridad]
    public class CreditoController : Controller {
        private readonly BUK_CreditoBL _creditoBL;

        public CreditoController() {
            _creditoBL = new BUK_CreditoBL();
        }

        #region VISTAS
        public ActionResult CreditoListadoVista() {
            return View();
        }

        public ActionResult CreditoDetalleVista(int id = 0) {
            BUK_CreditoEntidad credito = _creditoBL.ObtenerCreditoPorId(id);
            if(!credito.Existe()) {
                return RedirectToAction("CreditoListadoVista");
            }
            return View(credito);
        }
        #endregion

        #region METODOS
        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarCreditosJSON() {
            ResponseEntidad<List<BUK_CreditoEntidad>> response = new ResponseEntidad<List<BUK_CreditoEntidad>>();

            try {
                response.data = _creditoBL.ObtenerCreditos();
                response.success = response.data.Count > 0;
                response.displayMessage = response.success ? "Lista de créditos de trabajadores de BUK." : "No hay créditos de trabajadores de BUK.";
            } catch(Exception ex) {
                response.CreateErrorResponse(ex.Message + ". Llame al Administrador.");
            }

            return Json(response);
        }

        [HttpPost]
        public JsonResult ObtenerCreditoPorIdJSON(int idCredito) {
            ResponseEntidad<BUK_CreditoEntidad> response = new ResponseEntidad<BUK_CreditoEntidad>();

            try {
                response.data = _creditoBL.ObtenerCreditoPorId(idCredito);
                response.success = response.data.Existe();
                response.displayMessage = response.success ? "Crédito econtrado." : "No se encontró el crédito buscado.";
            } catch(Exception ex) {
                response.CreateErrorResponse(ex.Message + ". Llame al Administrador.");
            }

            return Json(response);
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ObtenerCreditosDeEmpleado(BUK_CreditoEntidad creditoEntidad) {
            ResponseEntidad<List<BUK_CreditoEntidad>> response = new ResponseEntidad<List<BUK_CreditoEntidad>>();

            try {
                response.data = _creditoBL.ObtenerCreditosDeEmpleadoByEmpresa(creditoEntidad);
                response.success = response.data.Count > 0;
                response.displayMessage = response.success ? "Créditos econtrado." : "No se encontró creditos para el número documento buscado.";
            } catch(Exception ex) {
                response.CreateErrorResponse(ex.Message + ". Llame al Administrador.");
            }

            return Json(response);
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult AnularCreditosEmpleadoJSON(BUK_CreditoEntidad credito) {
            ResponseEntidad<List<BUK_CreditoEntidad>> response = new ResponseEntidad<List<BUK_CreditoEntidad>>();
            response.data = _creditoBL.ObtenerCreditosDeEmpleadoByEmpresa(credito);

            if(response.data.Count == 0) {
                response.CreateErrorResponse("No se encontró creditos para anular.");
                return Json(response);
            }

            try {
                int idModificado = _creditoBL.AnularCredito(credito);
                response.success = idModificado > 0;
                response.displayMessage = response.success ? "Crédito anulado." : "No se encontró el crédito para anular.";
            } catch(Exception ex) {
                response.CreateErrorResponse(ex.Message + ". Llame al Administrador.");
            }

            return Json(response);
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult InsertarCreditoJSON(BUK_CreditoEntidad credito) {
            ResponseEntidad<BUK_CreditoEntidad> response = new ResponseEntidad<BUK_CreditoEntidad>();

            try {
                response.data = credito;
                response.data.IdCredito = _creditoBL.InsertarCredito(credito);
                response.success = response.data.Existe();
                response.displayMessage = response.success ? "Crédito creado correctamente." : "No se pudo crear el crédito.";
            } catch(Exception ex) {
                response.CreateErrorResponse(ex.Message + ". Llame al Administrador.");
            }

            return Json(response);
        }
        #endregion
    }
}