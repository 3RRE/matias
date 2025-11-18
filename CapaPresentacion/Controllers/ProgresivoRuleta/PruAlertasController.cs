using CapaEntidad.ProgresivoRuleta.Entidades;
using CapaNegocio.ProgresivoRuleta;
using System;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.ProgresivoRuleta {
    public class PruAlertasController : Controller {
        private readonly PRU_AlertaBL alertaBL;

        public PruAlertasController() {
            alertaBL = new PRU_AlertaBL();
        }

        #region Methods
        [seguridad(false)]
        [HttpPost]
        public JsonResult CrearAlerta(PRU_Alerta alerta) {
            bool success = false;
            string displayMessage;

            try {
                success = alertaBL.InsertarAlerta(alerta);
                displayMessage = success ? "Alerta creada correctamente." : "No se pudo crear la alerta.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }
        #endregion
    }
}
