using CapaEntidad.BUK;
using CapaEntidad.BUK.Response;
using CapaEntidad.Response;
using CapaNegocio.BUK;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.BUK {
    [seguridad]
    public class EquivalenciaEmpresaController : Controller {

        private readonly BUK_EquivalenciaEmpresaBL _equivalenciaEmpresaBL;

        public EquivalenciaEmpresaController() {
            _equivalenciaEmpresaBL = new BUK_EquivalenciaEmpresaBL();
        }

        public ActionResult EquivalenciaEmpresaListadoVista() {
            return View();
        }

        public ActionResult EquivalenciaEmpresaInsertarVista() {
            return View();
        }

        public ActionResult EquivalenciaEmpresaActualizarVista(int id = 0) {
            var equivalenciaEmpresa = _equivalenciaEmpresaBL.ObtenerEquivalenciaEmpresaPorIdEquivalenciaEmpresa(id);
            if(equivalenciaEmpresa == null || equivalenciaEmpresa.Nombre == null) {
                return RedirectToAction("EquivalenciaEmpresaListadoVista");
            }
            return View(equivalenciaEmpresa);
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarTodasLasEquivalenciasEmpresaJSON() {
            bool success = false;
            List<BUK_EquivalenciaEmpresaEntidad> equivalenciasEmpresa = new List<BUK_EquivalenciaEmpresaEntidad>();
            string displayMessage;

            try {
                equivalenciasEmpresa = _equivalenciaEmpresaBL.ObtenerTodasLasEquivalenciasEmpresa();
                success = equivalenciasEmpresa.Count > 0;
                displayMessage = success ? "Lista de equivalencias de empresas de BUK." : "No hay equivalencias de empresas de BUK";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = equivalenciasEmpresa, displayMessage });
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarTodasLasEquivalenciasEmpresaActivasJSON() {
            bool success = false;
            List<BUK_EquivalenciaEmpresaEntidad> equivalenciasEmpresa = new List<BUK_EquivalenciaEmpresaEntidad>();
            string displayMessage;

            try {
                equivalenciasEmpresa = _equivalenciaEmpresaBL.ObtenerTodasLasEquivalenciasEmpresaActivas();
                success = equivalenciasEmpresa.Count > 0;
                displayMessage = success ? "Lista de equivalencias de empresas de activas BUK." : "No hay equivalencias de empresas activas de BUK";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = equivalenciasEmpresa, displayMessage });
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarTodasLasEquivalenciasEmpresaCorrectasJSON() {
            bool success = false;
            List<BUK_EquivalenciaEmpresaEntidad> equivalenciasEmpresa = new List<BUK_EquivalenciaEmpresaEntidad>();
            string displayMessage;

            try {
                equivalenciasEmpresa = _equivalenciaEmpresaBL.ObtenerTodasLasEquivalenciasEmpresaCorrectas();
                success = equivalenciasEmpresa.Count > 0;
                displayMessage = success ? "Lista de equivalencias de empresas de BUK." : "No hay equivalencias de empresas de BUK";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = equivalenciasEmpresa, displayMessage });
        }

        [HttpPost]
        public JsonResult ObtenerEquivalenciaEmpresaPorIdJSON(int idEquivalenciaEmpresa) {
            bool success = false;
            BUK_EquivalenciaEmpresaEntidad equivalenciaEmpresa = new BUK_EquivalenciaEmpresaEntidad();
            string displayMessage;

            try {
                equivalenciaEmpresa = _equivalenciaEmpresaBL.ObtenerEquivalenciaEmpresaPorIdEquivalenciaEmpresa(idEquivalenciaEmpresa);
                success = equivalenciaEmpresa.Existe();
                displayMessage = success ? "Equivalencia de empresa econtrada." : "No se encontró la equivalencia de empresa";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = equivalenciaEmpresa, displayMessage });
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ObtenerEquivalenciaEmpresaPorCodEmpresaOfisisJSON(string codEmpresaOfisis) {
            bool success = false;
            BUK_EquivalenciaEmpresaEntidad equivalenciaEmpresa = new BUK_EquivalenciaEmpresaEntidad();
            string displayMessage;

            try {
                equivalenciaEmpresa = _equivalenciaEmpresaBL.ObtenerEquivalenciaEmpresaPorCodEmpresaOfisis(codEmpresaOfisis);
                success = equivalenciaEmpresa.Existe();
                displayMessage = success ? "Equivalencia de empresa econtrada." : $"No se encontró la equivalencia de empresa con codigo ofisis {codEmpresaOfisis}.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = equivalenciaEmpresa, displayMessage });
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ObtenerEquivalenciaEmpresaPorIdEmpresaBukJSON(int idEmpresaBuk) {
            bool success = false;
            BUK_EquivalenciaEmpresaEntidad equivalenciaEmpresa = new BUK_EquivalenciaEmpresaEntidad();
            string displayMessage;

            try {
                equivalenciaEmpresa = _equivalenciaEmpresaBL.ObtenerEquivalenciaEmpresaPorIdEmpresaBuk(idEmpresaBuk);
                success = equivalenciaEmpresa.Existe();
                displayMessage = success ? "Equivalencia de empresa econtrada." : $"No se encontró la equivalencia de empresa con ID BUK {idEmpresaBuk}.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = equivalenciaEmpresa, displayMessage });
        }

        [HttpPost]
        public JsonResult InsertarEquivalenciaEmpresaJSON(BUK_EquivalenciaEmpresaEntidad equivalenciaEmpresa) {
            bool success = false;
            string displayMessage;
            try {
                if(equivalenciaEmpresa.Equals("0")) {
                    displayMessage = $"Cod. OFISIS debe ser un número mayor a cero (0).";
                    return Json(new { success, displayMessage });
                }

                if(equivalenciaEmpresa.IdEmpresaBuk == 0) {
                    displayMessage = $"Id BUK debe ser un número mayor a cero (0).";
                    return Json(new { success, displayMessage });
                }

                BUK_EquivalenciaEmpresaEntidad empresaVerificacion = _equivalenciaEmpresaBL.ObtenerEquivalenciaEmpresaPorCodEmpresaOfisis(equivalenciaEmpresa.CodEmpresaOfisis);
                if(empresaVerificacion.Existe()) {
                    displayMessage = $"Ya existe una equivalencia de empresa con Cod. Ofisis {equivalenciaEmpresa.CodEmpresaOfisis}.";
                    return Json(new { success, displayMessage });
                }

                empresaVerificacion = _equivalenciaEmpresaBL.ObtenerEquivalenciaEmpresaPorIdEmpresaBuk(equivalenciaEmpresa.IdEmpresaBuk);
                if(empresaVerificacion.Existe()) {
                    displayMessage = $"Ya existe una equivalencia de empresa con Id BUK {equivalenciaEmpresa.IdEmpresaBuk}.";
                    return Json(new { success, displayMessage });
                }

                success = _equivalenciaEmpresaBL.InsertarEquivalenciaEmpresa(equivalenciaEmpresa);
                displayMessage = success ? "Equivalencia de empresa agregada correctamente." : "No se pudo agregar la equivalencia de empresa, llame al Administrador.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult ActualizarEquivalenciaEmpresaJSON(BUK_EquivalenciaEmpresaEntidad equivalenciaEmpresa) {
            bool success = false;
            string displayMessage;
            try {
                if(equivalenciaEmpresa.Equals("0")) {
                    displayMessage = $"Cod. OFISIS debe ser un número mayor a cero (0).";
                    return Json(new { success, displayMessage });
                }

                if(equivalenciaEmpresa.IdEmpresaBuk == 0) {
                    displayMessage = $"Id BUK debe ser un número mayor a cero (0).";
                    return Json(new { success, displayMessage });
                }

                BUK_EquivalenciaEmpresaEntidad empresaVerificacion = _equivalenciaEmpresaBL.ObtenerEquivalenciaEmpresaPorCodEmpresaOfisis(equivalenciaEmpresa.CodEmpresaOfisis);
                if(empresaVerificacion.Existe() && empresaVerificacion.IdEquivalenciaEmpresa != equivalenciaEmpresa.IdEquivalenciaEmpresa) {
                    displayMessage = $"Ya existe una equivalencia de empresa con Cod. Ofisis {equivalenciaEmpresa.CodEmpresaOfisis}.";
                    return Json(new { success, displayMessage });
                }

                empresaVerificacion = _equivalenciaEmpresaBL.ObtenerEquivalenciaEmpresaPorIdEmpresaBuk(equivalenciaEmpresa.IdEmpresaBuk);
                if(empresaVerificacion.Existe() && empresaVerificacion.IdEquivalenciaEmpresa != equivalenciaEmpresa.IdEquivalenciaEmpresa) {
                    displayMessage = $"Ya existe una equivalencia de empresa con Id BUK {equivalenciaEmpresa.IdEmpresaBuk}.";
                    return Json(new { success, displayMessage });
                }

                success = _equivalenciaEmpresaBL.ActualizarEquivalenciaEmpresa(equivalenciaEmpresa);
                displayMessage = success ? "Equivalencia de empresa actualizada correctamente." : "No se pudo actualizar la equivalencia de empresa, llame al Administrador.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult EliminarEquivalenciaEmpresaJSON(int idEquivalenciaEmpresa) {
            bool success = false;
            string displayMessage;
            try {
                success = _equivalenciaEmpresaBL.EliminarEquivalenciaEmpresa(idEquivalenciaEmpresa);
                displayMessage = success ? "Equivalencia de empresa eliminada correctamente" : "No se pudo eliminar la equivalencia de empresa, llame al Administrador.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }
            return Json(new { success, displayMessage });
        }

        #region Sincronizador
        [seguridad(false)]
        public JsonResult SincronizarEmpresas(List<BUK_EquivalenciaEmpresaEntidad> empresas) {
            ResponseEntidad<BUK_EquivalenciaEmpresaResponse> response = new ResponseEntidad<BUK_EquivalenciaEmpresaResponse>();
            try {
                response = _equivalenciaEmpresaBL.SincronizarEmpresas(empresas);
            } catch(Exception ex) {
                response.displayMessage = $"Error al intentar sincronizar empresas. {ex.Message}";
            }
            return Json(response);
        }
        #endregion
    }
}