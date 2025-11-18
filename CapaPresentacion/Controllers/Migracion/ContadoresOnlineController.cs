using CapaEntidad.Migracion;
using CapaNegocio.Migracion;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Migracion {

    [seguridad(false)]
    public class ContadoresOnlineController : Controller {
        private readonly ContadoresOnlineBL contadoresOnlineBL;

        public ContadoresOnlineController() {
            contadoresOnlineBL = new ContadoresOnlineBL();
        }

        [HttpPost]
        public JsonResult ObtenerFechaDeUltimoContadorPorCodSala(int codSala) {
            DateTime fecha = contadoresOnlineBL.ObtenerFechaDeUltimoContadorPorCodSala(codSala);
            var result = new { success = true, data = fecha };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarContadoresOnline(List<ContadoresOnline> contadoresOnline) {
            bool success = contadoresOnlineBL.GuardarContadoresOnline(contadoresOnline);
            string displayMessage = success ? $"{contadoresOnline.Count} contadores migrados correctamente." : "Error al migrar los Contadores.";
            var result = new { success, displayMessage };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}