using S3k.Utilitario.Helper;
using System.Text;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers {
    public abstract class BaseController : Controller {
        protected JsonResult JsonResponse(object data, HttpStatusCodes statusCode = HttpStatusCodes.Ok) {
            Response.StatusCode = (int)statusCode;
            Response.TrySkipIisCustomErrors = true;

            JsonResult result = Json(data, JsonRequestBehavior.AllowGet);
            result.ContentType = "application/json";
            result.ContentEncoding = Encoding.UTF8;
            result.MaxJsonLength = int.MaxValue;

            return result;
        }
    }
}