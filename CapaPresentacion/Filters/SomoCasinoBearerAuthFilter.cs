using Microsoft.AspNetCore.Http;
using System.Configuration;
using System.Text;
using System.Web.Mvc;

namespace CapaPresentacion.Filters {

    public class SomoCasinoBearerAuthFilter : ActionFilterAttribute {
        private readonly string _staticToken = ConfigurationManager.AppSettings["TokenSomosCasino"];

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            var request = filterContext.HttpContext.Request;
            var authHeader = request.Headers["Authorization"];

            // Si no hay token → 401 Unauthorized
            if(string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ")) {
                filterContext.Result = BuildJsonResponse(StatusCodes.Status403Forbidden, "Token no proporcionado");
                return;
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            // Si el token no coincide → 403 Forbidden
            if(!string.Equals(token, _staticToken)) {
                filterContext.Result = BuildJsonResponse(StatusCodes.Status403Forbidden, "Token inválido");
                return;
            }

            base.OnActionExecuting(filterContext);
        }

        private JsonResult BuildJsonResponse(int statusCode, string message) {
            var response = new JsonResult {
                Data = new { success = false, displayMessage = message },
                ContentEncoding = Encoding.UTF8,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            // Importante: setear el status code en la respuesta HTTP
            System.Web.HttpContext.Current.Response.StatusCode = statusCode;

            return response;
        }
    }
}
