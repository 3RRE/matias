using CapaEntidad;
using CapaEntidad.ProgresivoRuleta.Config;
using CapaEntidad.ProgresivoRuleta.Dto;
using CapaEntidad.ProgresivoRuleta.Entidades;
using CapaEntidad.ProgresivoRuleta.Filtro;
using CapaNegocio;
using CapaNegocio.ProgresivoRuleta;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.ProgresivoRuleta {
    [seguridad]
    public class PruConfiguracionesController : Controller {
        private readonly PRU_ConfiguracionBL configuracionBL;
        private readonly SalaBL salaBL;

        public PruConfiguracionesController() {
            configuracionBL = new PRU_ConfiguracionBL();
            salaBL = new SalaBL();
        }

        #region Views
        public ActionResult ProgresivoRuletaConfiguracionListadoVista() {
            return View("~/Views/ProgresivoRuleta/Configuracion/Index.cshtml");
        }

        public ActionResult ProgresivoRuletaConfiguracionInsertarVista() {
            return View("~/Views/ProgresivoRuleta/Configuracion/Insertar.cshtml");
        }

        public ActionResult ProgresivoRuletaConfiguracionActualizarVista(int id = 0) {
            PRU_ConfiguracionDto configuracion = configuracionBL.ObtenerConfiguracionPorId(id);
            if(!configuracion.Existe()) {
                return RedirectToAction("ProgresivoRuletaConfiguracionListadoVista");
            }
            return View("~/Views/ProgresivoRuleta/Configuracion/Actualizar.cshtml", configuracion);
        }
        #endregion

        #region Methods
        [HttpGet]
        public JsonResult ObtenerConfiguraciones() {
            bool success = false;
            string displayMessage;
            List<PRU_ConfiguracionDto> data = new List<PRU_ConfiguracionDto>();

            try {
                data = configuracionBL.ObtenerConfiguraciones();
                success = data.Count > 0;
                displayMessage = success ? "Lista de configuraciones de progresivos de ruletas." : "No hay configuraciones de progresivos de ruletas.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerConfiguracionesPorCodSala(int codSala) {
            bool success = false;
            string displayMessage;
            List<PRU_ConfiguracionDto> data = new List<PRU_ConfiguracionDto>();

            try {
                SalaEntidad sala = salaBL.SalaListaIdJson(codSala);
                if(!sala.Existe()) {
                    success = false;
                    displayMessage = $"No existe sala con código {codSala}.";
                    return Json(new { success, displayMessage });
                }

                data = configuracionBL.ObtenerConfiguracionesPorCodSala(codSala);
                success = data.Count > 0;
                displayMessage = success ? $"Lista de configuraciones de ruleta de la sala {sala.Nombre}." : $"No hay configuraciones de progresivo ruleta para la sala {sala.Nombre}.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpGet]
        public JsonResult ObtenerConfiguracionPorId(int id) {
            bool success = false;
            string displayMessage;
            PRU_ConfiguracionDto data = new PRU_ConfiguracionDto();

            try {
                data = configuracionBL.ObtenerConfiguracionPorId(id);
                success = data.Existe();
                displayMessage = success ? "Configuración de configuración de ruleta." : "No se encontró la configuración de progresivo de ruleta.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data }, JsonRequestBehavior.AllowGet);
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ObtenerConfiguracionPorFiltro(PRU_Filtro filtro) {
            bool success = false;
            string displayMessage;
            Pru_MisteryConfig data = new Pru_MisteryConfig();

            try {
                SalaEntidad sala = salaBL.SalaListaIdJson(filtro.CodSala);
                if(!sala.Existe()) {
                    success = false;
                    displayMessage = $"No existe sala con código {filtro.CodSala}.";
                    Response.StatusCode = 400;
                    return Json(new { success, displayMessage });
                }

                data = configuracionBL.ObtenerConfiguracionPorFiltro(filtro);
                success = data.Existe();
                Response.StatusCode = success ? 200 : 400;
                displayMessage = success ? "Configuración de configuración de ruleta." : "No se encontró la configuración de progresivo de ruleta.";
            } catch(Exception ex) {
                Response.StatusCode = 500;
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            JsonResult jsonResult = success ? Json(new { success, displayMessage, data }) : Json(new { success, displayMessage });
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        [HttpPost]
        public JsonResult CrearConfiguracion(PRU_Configuracion configuracion) {
            bool success = false;
            string displayMessage;

            try {
                SalaEntidad sala = salaBL.SalaListaIdJson(configuracion.CodSala);
                if(!sala.Existe()) {
                    success = false;
                    displayMessage = $"No existe sala con código {configuracion.CodSala}.";
                    return Json(new { success, displayMessage });
                }

                success = configuracionBL.InsertarConfiguracion(configuracion);
                displayMessage = success ? "Configuración de progresivo de ruleta creada correctamente." : "No se pudo crear la configuración de progresivo de ruleta.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult ActualizarConfiguracion(PRU_Configuracion configuracion) {
            bool success = false;
            string displayMessage;

            try {
                success = configuracionBL.ActualizarConfiguracion(configuracion);
                displayMessage = success ? "Configuración de progresivo de ruleta creada correctamente." : "No se pudo crear la configuración de progresivo de ruleta.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }
        #endregion
    }
}
