using CapaEntidad.ProgresivoRuleta.Dto;
using CapaNegocio.ProgresivoRuleta;
using CapaPresentacion.Reports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.ProgresivoRuleta {
    [seguridad]
    public class ProgresivoRuletaController : Controller {
        private readonly PRU_RuletaBL _pruRuletaBL = new PRU_RuletaBL();
        private readonly PRU_GanadorBL _pruGanadorBL = new PRU_GanadorBL();
        private readonly PRU_AlertaBL _pruAlertaBL = new PRU_AlertaBL();

        #region Views
        public ActionResult GanadoresPRU() {
            return View("~/Views/ProgresivoRuleta/GanadoresPRU.cshtml");
        }

        public ActionResult AlertasPRU() {
            return View("~/Views/ProgresivoRuleta/AlertasPRU.cshtml");
        }
        #endregion

        #region Methods
        [HttpPost]
        public JsonResult SeleccionarRuletasPRU(int salaId) {
            bool success = false;
            string message = "No hay registros disponibles";

            List<PRU_RuletaSelectDto> data = new List<PRU_RuletaSelectDto>();

            try {
                var items = _pruRuletaBL.SeleccionarRuletasBySalaId(salaId);

                if(items.Count > 0) {
                    success = true;
                    message = "Registros obtenidos";
                    data = items;
                }
            } catch(Exception exception) {
                message = exception.Message;
            }

            return Json(new {
                success,
                message,
                data
            });
        }

        [HttpPost]
        public JsonResult ObtenerGanadoresPRU(int salaId, int? ruletaId, DateTime fechaInicial, DateTime fechaFinal) {
            bool success = false;
            string message = "No hay registros disponibles";

            if(salaId <= 0) {
                return Json(new {
                    success = false,
                    message = "Por favor, seleccione una sala"
                });
            }

            List<PRU_GanadorDto> data = new List<PRU_GanadorDto>();

            try {
                var items = _pruGanadorBL.ObtenerGanadoresByFechas(salaId, ruletaId, fechaInicial, fechaFinal);

                if(items.Count > 0) {
                    success = true;
                    message = "Registros obtenidos";
                    data = items;
                }
            } catch(Exception exception) {
                message = exception.Message;
            }

            var result = Json(new {
                success,
                message,
                data
            });

            result.MaxJsonLength = int.MaxValue;

            return result;
        }

        [HttpPost]
        public JsonResult ObtenerAlertasPRU(int salaId, int? ruletaId, DateTime fechaInicial, DateTime fechaFinal) {
            bool success = false;
            string message = "No hay registros disponibles";

            if(salaId <= 0) {
                return Json(new {
                    success = false,
                    message = "Por favor, seleccione una sala"
                });
            }

            List<PRU_AlertaDto> data = new List<PRU_AlertaDto>();

            try {
                var items = _pruAlertaBL.ObtenerAlertasByFechas(salaId, ruletaId, fechaInicial, fechaFinal);

                if(items.Count > 0) {
                    success = true;
                    message = "Registros obtenidos";
                    data = items;
                }
            } catch(Exception exception) {
                message = exception.Message;
            }

            var result = Json(new {
                success,
                message,
                data
            });

            result.MaxJsonLength = int.MaxValue;

            return result;
        }

        [HttpPost]
        public JsonResult ExcelGanadoresPRU(int salaId, int? ruletaId, DateTime fechaInicial, DateTime fechaFinal) {
            bool success = false;
            string message = "No hay registros disponibles";

            if(salaId <= 0) {
                return Json(new {
                    success = false,
                    message = "Por favor, seleccione una sala"
                });
            }

            DateTime currentDate = DateTime.Now;
            string fileExtension = "xlsx";
            string fileName = $"GanadoresPRU_{salaId}_{currentDate:dd-MM-yyyy}_{currentDate:HHmmss}.{fileExtension}";
            string data = string.Empty;

            try {
                var items = _pruGanadorBL.ObtenerGanadoresByFechas(salaId, ruletaId, fechaInicial, fechaFinal);

                if(items.Count > 0) {
                    MemoryStream stream = PRU_GanadorReport.ExcelGanadores(items);

                    success = true;
                    message = "Excel generado";
                    data = Convert.ToBase64String(stream.ToArray());
                }
            } catch(Exception exception) {
                message = exception.Message;
            }

            return Json(new {
                success,
                message,
                fileName,
                data
            });
        }

        [HttpPost]
        public JsonResult ExcelAlertasPRU(int salaId, int? ruletaId, DateTime fechaInicial, DateTime fechaFinal) {
            bool success = false;
            string message = "No hay registros disponibles";

            if(salaId <= 0) {
                return Json(new {
                    success = false,
                    message = "Por favor, seleccione una sala"
                });
            }

            DateTime currentDate = DateTime.Now;
            string fileExtension = "xlsx";
            string fileName = $"AlertasPRU_{salaId}_{currentDate:dd-MM-yyyy}_{currentDate:HHmmss}.{fileExtension}";
            string data = string.Empty;

            try {
                var items = _pruAlertaBL.ObtenerAlertasByFechas(salaId, ruletaId, fechaInicial, fechaFinal);

                if(items.Count > 0) {
                    MemoryStream stream = PRU_AlertaReport.ExcelAlertas(items);

                    success = true;
                    message = "Registros obtenidos";
                    data = Convert.ToBase64String(stream.ToArray());
                }
            } catch(Exception exception) {
                message = exception.Message;
            }

            return Json(new {
                success,
                message,
                fileName,
                data
            });
        }
        #endregion
    }
}