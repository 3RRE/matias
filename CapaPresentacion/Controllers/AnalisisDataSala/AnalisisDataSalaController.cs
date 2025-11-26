
using CapaNegocio.AnalisisDataSala;
using System;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.AnalisisDataSala {
    
    [seguridad(false)]
    public class AnalisisDataSalaController : Controller {
        private readonly AnalisisDataSalaBL _analisisBl = new AnalisisDataSalaBL();
        /// <summary>
        /// tiempo juego
        /// </summary>
        /// <returns></returns>
        [seguridad]
        public ActionResult DashBoardTiempoJuego() {
            return View();
        }


        /// <summary>
        /// Endpoint para los KPIs Generales (Widget 1)
        /// </summary>
        [HttpGet]
        public JsonResult GetKpiGeneral(string codSala, DateTime fecha) {
            try {
                var data = _analisisBl.GetKpiGeneral(codSala, fecha);
                return Json(new { success = true, data = data }, JsonRequestBehavior.AllowGet);
            } catch (Exception ex) {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Endpoint para el Gráfico de Barras de Horas (Widget 2)
        /// </summary>
        [HttpGet]
        public JsonResult GetUtilizacionPorHora(string codSala, DateTime fecha) {
            try {
                var data = _analisisBl.GetUtilizacionPorHora(codSala, fecha);
                return Json(new { success = true, data = data }, JsonRequestBehavior.AllowGet);
            } catch (Exception ex) {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Endpoint para el Ranking de Máquinas (Widget 3)
        /// </summary>
        [HttpGet]
        public JsonResult GetRankingMaquinas(string codSala, DateTime fecha) {
            try {
                var data = _analisisBl.GetRankingMaquinas(codSala, fecha);
                return Json(new { success = true, data = data }, JsonRequestBehavior.AllowGet);
            } catch (Exception ex) {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Endpoint para la Gráfica Gantt de 1 Máquina (Widget 4)
        /// </summary>
        [HttpGet]
        public JsonResult GetTimelineMaquina(string codSala, DateTime fecha, string codMaq) {
            try {
                var data = _analisisBl.GetTimelineMaquina(codSala, fecha, codMaq);
                return Json(new { success = true, data = data }, JsonRequestBehavior.AllowGet);
            } catch (Exception ex) {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // -----------------------------------------------------------------
        // --- ¡NUEVOS ENDPOINTS JSON PARA HIT FREQUENCY! ---
        // -----------------------------------------------------------------

        [seguridad]
        public ActionResult DashBoardHeatFrecuency() {
            return View();
        }

        /// <summary>
        /// Endpoint para los KPIs Generales (HF)
        /// </summary>
        [HttpGet]
        public JsonResult GetHitFrecGeneral(string codSala, DateTime fecha) {
            try {
                var data = _analisisBl.GetHitFrecGeneral(codSala, fecha);
                return Json(new { success = true, data = data }, JsonRequestBehavior.AllowGet);
            } catch (Exception ex) {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Endpoint para el Ranking de Máquinas (HF)
        /// </summary>
        [HttpGet]
        public JsonResult GetHitFrecPorMaquina(string codSala, DateTime fecha) {
            try {
                var data = _analisisBl.GetHitFrecPorMaquina(codSala, fecha);
                return Json(new { success = true, data = data }, JsonRequestBehavior.AllowGet);
            } catch (Exception ex) {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Endpoint para el Log de Sustento (HF)
        /// </summary>
        [HttpGet]
        public JsonResult GetHitFrecLogDetallado(string codSala, DateTime fecha, string codMaq) {
            try {
                var data = _analisisBl.GetHitFrecLogDetallado(codSala, fecha, codMaq);
                return Json(new { success = true, data = data }, JsonRequestBehavior.AllowGet);
            } catch (Exception ex) {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }

};
