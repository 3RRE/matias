using System.Web.Mvc;

namespace CapaPresentacion.Controllers.EntradaSalidaSala {
    [seguridad(true)]
    public class ReporteConsolidadoController : Controller {
        public ActionResult ReporteConsolidadoVista() {
            return View("~/Views/EntradaSalidaSala/ReporteConsolidado.cshtml");
        }
    }
}
