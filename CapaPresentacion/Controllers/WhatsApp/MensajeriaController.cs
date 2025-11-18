using CapaEntidad.Response;
using CapaEntidad.WhatsApp;
using CapaEntidad.WhatsApp.Response;
using CapaNegocio.WhatsApp;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CapaPresentacion.Controllers.WhatsApp {
    [seguridad]
    public class MensajeriaController : Controller {
        private WSP_MensajeEnviadoBL mensajeEnviadoBL = new WSP_MensajeEnviadoBL();
        private WSP_ContactoBL contactoBL = new WSP_ContactoBL();
        private string UriServerWhatsApp = ConfigurationManager.AppSettings["UriServerWhatsApp"];

        // GET: Mensajeria
        public ActionResult Index() {
            return View();
        }

        public JsonResult EnviarMensajeMultiple(WSP_MensajeMultipleEntidad mensaje) {

            var contactos = contactoBL.ObtenerContactosPorIdsContacto(mensaje.clients.Select(x => x.idCliente).ToList());
            mensaje.phones = contactos.Select(x => x.NumeroCompleto.ToString()).ToList();

            var uri = $"{UriServerWhatsApp}/api/message/multiple";
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new ResponseEntidad<WSP_MultipleMessageResponse>();

            var body = new {
                message = mensaje.message,
                phones = mensaje.phones
            };

            try {
                string inputJson = (new JavaScriptSerializer()).Serialize(body);
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                response = client.UploadString(uri, "POST", inputJson);
                jsonResponse = JsonConvert.DeserializeObject<ResponseEntidad<WSP_MultipleMessageResponse>>(response);
                ////TODO: ACA GUARDO EN LA BD
                //foreach(var item in jsonResponse.data.messages) {
                //    var idContact = contactos.Where(x => x.NumeroCompleto.Equals(item.to)).FirstOrDefault().IdContacto;
                //    item.idContact = idContact;
                //}
                //mensajeEnviadoBL.InsertarMensaje(jsonResponse.data.messages);
            } catch(Exception ex) {
                return Json(new { successs = false, displayMessage = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }

            var jsonResult = Json(jsonResponse, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }
    }
}