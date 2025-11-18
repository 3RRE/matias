using System.Web.Mvc;

namespace CapaPresentacion.Controllers.AsistenciaCliente {
    public class PermisosClienteController : Controller {
        public JsonResult VerInfoContactoCliente() {
            return Json(new { success = true });
        }
    }
}