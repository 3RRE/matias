using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Windows.Documents;

namespace CapaPresentacion.Controllers.Ubigeo {
    public class UbigeoController : Controller {
        private readonly UbigeoBL ubigeoBL;

        public UbigeoController() {
            ubigeoBL = new UbigeoBL();
        }

        [HttpGet]
        public JsonResult GetPaises() {
            bool success;
            string displayMessage;
            List<UbigeoEntidad> paises = new List<UbigeoEntidad>();
            try {
                paises = ubigeoBL.ListaPaisesConCodigoTelefonico();
                success = paises.Count > 0;
                displayMessage = success ? "Lista de paises." : "No hay paises.";
            } catch(Exception ex) {
                success = false;
                displayMessage = ex.Message;
            }
            return Json(new { success, displayMessage, data = paises }, JsonRequestBehavior.AllowGet);
        }
    }
}