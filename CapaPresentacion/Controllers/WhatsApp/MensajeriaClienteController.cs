using CapaEntidad;
using CapaEntidad.AsistenciaCliente;
using CapaEntidad.Response;
using CapaEntidad.WhatsApp;
using CapaEntidad.WhatsApp.Response;
using CapaNegocio;
using CapaNegocio.AsistenciaCliente;
using CapaNegocio.WhatsApp;
using CapaPresentacion.Utilitarios;
using Newtonsoft.Json;
using S3k.Utilitario.WhatsApp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.WhatsApp {
    [seguridad]
    public class MensajeriaClienteController : Controller {
        private readonly string UriServerWhatsApp = string.Empty;
        private readonly int MaxSizeImage = 0;
        private readonly int MaxMessageByRequestWhatsApp = 0;
        private readonly int MaxLengthBase64 = 0;
        private readonly WSP_MensajeriaClienteBL _mensajeriaClienteBL;
        private readonly AST_ClienteBL ast_ClienteBL;
        private readonly SalaBL _salaBL;

        private AST_ClienteEntidad cliente = new AST_ClienteEntidad();
        private WSP_MensajeriaUltraMsgBL mensajeriaUltraMsgBL;

        public MensajeriaClienteController() {
            UriServerWhatsApp = ConfigurationManager.AppSettings["UriServerWhatsApp"];
            _mensajeriaClienteBL = new WSP_MensajeriaClienteBL();
            _salaBL = new SalaBL();
            ast_ClienteBL = new AST_ClienteBL();
            MaxSizeImage = Convert.ToInt32(ValidationsHelper.GetValueAppSettingDB("WSP_MAX_IMAGE_SIZE", 16));
            MaxMessageByRequestWhatsApp = Convert.ToInt32(ValidationsHelper.GetValueAppSettingDB("WSP_MAX_MESSAGE_BY_REQUEST", 400));
            MaxLengthBase64 = Convert.ToInt32(ValidationsHelper.GetValueAppSettingDB("WSP_MAX_LENGTH_BASE64", 10000000));
        }

        #region Vistas
        public ActionResult Index() {
            return View();
        }

        public ActionResult ActualizarContactoClienteVista(int id) {
            var cliente = ast_ClienteBL.GetClienteID(id);
            if(cliente == null || cliente.Id <= 0) {
                return RedirectToAction("Index");
            }
            return View(cliente);
        }
        #endregion

        #region Metodos
        [HttpPost]
        public JsonResult FiltrarClienteJson(string[] filtroSala, string[] filtroTipoCliente, string[] filtroTipoFrecuencia, string[] filtroTipoJuego) {
            bool success = false;
            List<WSP_MensajeriaClienteEntidad> clientes = new List<WSP_MensajeriaClienteEntidad>();
            string displayMessage;

            try {
                clientes = _mensajeriaClienteBL.ObtenerClientesPorFiltro(filtroSala, filtroTipoCliente, filtroTipoFrecuencia, filtroTipoJuego);
                success = true;
                displayMessage = "Lista de clientes filtrados.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }

            var jsonResult = Json(new { success, data = clientes, displayMessage }, JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        [HttpPost]
        public async Task<JsonResult> EnviarMensajeMultiple(WSP_MensajeMultipleEntidad mensaje) {
            ResponseEntidad<WSP_MultipleMessageResponse> response = new ResponseEntidad<WSP_MultipleMessageResponse>();
            response.data.Message = mensaje.message;
            response.success = true;

            if(mensaje.withImage) {
                //Se valida que el archivo subido sea el correcto y cumpla todas las validaciones
                StatusImage estado = UtilitarioWhatsApp.ValidateImage(mensaje.image, MaxSizeImage, MaxLengthBase64);
                if(estado != StatusImage.Ok) {
                    response.success = false;
                    switch(estado) {
                        case StatusImage.Empty: response.displayMessage = "No se adjunto ni una imagen al mensaje."; break;
                        case StatusImage.ErrorType: response.displayMessage = "El tipo de imagen no esta permitido."; break;
                        case StatusImage.ErrorExtension: response.displayMessage = "La extensión de la imagen no es la correcta."; break;
                        case StatusImage.ErrorSize: response.displayMessage = $"El tamaño de la imagen supera el máximo permitido ({MaxSizeImage}MB)"; break;
                        case StatusImage.ErrorLengthBase64: response.displayMessage = $"La imagen supera la longitud de BASE 64 permitido"; break;
                    }
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }

            mensaje.clients = JsonConvert.DeserializeObject<List<WSP_ClienteSala>>(mensaje.ids);
            var agrupado = mensaje.clients.GroupBy(x => x.codSala);

            response.displayMessage = "Mensaje enviado a:<ul>";

            foreach(var sala in agrupado) {
                //se genera la instancia para enviar mensaje con el codigo de la sala
                mensajeriaUltraMsgBL = new WSP_MensajeriaUltraMsgBL(sala.Key);

                SalaEntidad salaEntidad = _salaBL.ObtenerSalaPorCodigo(sala.Key);

                WSP_MultipleMessageSala multipleSalaSend = new WSP_MultipleMessageSala {
                    CodSala = sala.Key
                };

                if(!mensajeriaUltraMsgBL.IsPosibleUseServiceWhatsApp) {
                    response.data.Casinos.Add(multipleSalaSend);
                    continue;
                }

                //se obtiene la lista de clientes seleccionados
                string ids = String.Join(",", sala.Select(x => x.idCliente));
                string whereQuery = $"WHERE cliente.Id IN ({ids})";
                var clientes = ast_ClienteBL.GetListadoClienteFiltrados(whereQuery).ToList();

                int batchSize = MaxMessageByRequestWhatsApp;
                int batchQuantity = (int)Math.Ceiling(Convert.ToDouble(clientes.Count) / batchSize);
                int batchsShipped = 0;

                for(int i = 0; i < clientes.Count; i += batchSize) {
                    List<AST_ClienteEntidad> batchClientes = clientes.Skip(i).Take(batchSize).ToList();
                    if(batchClientes.Count > 0) {
                        batchsShipped++;
                        //se obtiene la lista de celulares de los clientes, segun las reglas de negocio
                        mensaje.phones = _mensajeriaClienteBL.ObtenerNumerosClientes(clientes);
                        try {
                            var responseSendMessages = mensaje.withImage ? await mensajeriaUltraMsgBL.SendMessageMultipleWhitImage(mensaje.phones, mensaje.message, mensaje.image) : await mensajeriaUltraMsgBL.SendMessageMultiple(mensaje.phones, mensaje.message);
                            response.success &= responseSendMessages.success;
                            multipleSalaSend.MessagesSend.AddRange(responseSendMessages.data);
                        } catch {
                            response.success = false;
                        }
                    }
                }
                int cantidadEnviados = multipleSalaSend.MessagesSend.Count(x => Convert.ToBoolean(x.sent));
                string aux = cantidadEnviados == 1 ? "cliente" : "clientes";
                response.displayMessage += $"<li>{cantidadEnviados} {aux} de {salaEntidad.Nombre}.</li>";
                response.data.Casinos.Add(multipleSalaSend);
            }

            response.displayMessage += "</ul>";

            var result = Json(response, JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;

            return result;
        }
        #endregion
    }
}
