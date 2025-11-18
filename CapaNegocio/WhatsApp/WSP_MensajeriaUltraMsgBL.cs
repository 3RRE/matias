using CapaEntidad;
using CapaEntidad.Response;
using CapaEntidad.WhatsApp;
using Newtonsoft.Json;
using S3k.Utilitario.WhatsApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace CapaNegocio.WhatsApp {
    public class WSP_MensajeriaUltraMsgBL {
        private readonly HttpClient httpClient;
        private readonly string InstanciaApiWhatsApp;
        private readonly string TokenApiWhatsApp;
        private readonly WSP_InstanciaUltraMsgBL wspInstanciaUltraMsgBL;
        public readonly bool IsPosibleUseServiceWhatsApp;
        private readonly SalaEntidad sala;
        private readonly SalaBL salaBl;

        public WSP_MensajeriaUltraMsgBL(int codSala) {
            salaBl = new SalaBL();

            httpClient = new HttpClient();
            wspInstanciaUltraMsgBL = new WSP_InstanciaUltraMsgBL();

            WSP_InstanciaUltraMsgEntidad instancia = wspInstanciaUltraMsgBL.ObtenerInstanciaPorCodSala(codSala);
            IsPosibleUseServiceWhatsApp = instancia.IdInstanciaUltraMsg > 0;
            sala = salaBl.SalaListaIdJson(codSala);

            if(IsPosibleUseServiceWhatsApp) {
                httpClient.BaseAddress = new Uri(instancia.UrlBase);
                this.InstanciaApiWhatsApp = instancia.Instancia;
                this.TokenApiWhatsApp = instancia.Token;
            }
        }

        public async Task<ResponseEntidad<WSP_UltraMsgResponse>> SendMessage(string phoneNumber, string message) {
            ResponseEntidad<WSP_UltraMsgResponse> response = new ResponseEntidad<WSP_UltraMsgResponse>();

            if(!IsPosibleUseServiceWhatsApp) {
                response.success = false;
                response.displayMessage = $"No es posible enviar mensaje por WhatsApp debido a que no se encontró una instancia registrada para la sala {sala.Nombre} en la base de datos.";
                return response;
            }

            //string url = $"{InstanciaApiWhatsApp}/messages/chat?token={TokenApiWhatsApp}&to={phoneNumber}&body={message}&priority=1";
            string url = $"{InstanciaApiWhatsApp}/messages/chat";

            //creando objeto para la peticion
            WSP_UltraMsgRequest requestBody = new WSP_UltraMsgRequest {
                token = TokenApiWhatsApp,
                to = phoneNumber,
                body = message,
                priority = "1"
            };
            var formData = UtilitarioWhatsApp.ObjectToFormUrlEncodedContent(requestBody);

            try {
                var responseServer = await httpClient.PostAsync(url, formData);
                var contentResponse = await responseServer.Content.ReadAsStringAsync();
                response.data = JsonConvert.DeserializeObject<WSP_UltraMsgResponse>(contentResponse);
                response.success = Convert.ToBoolean(response.data.sent) && response.data.message.ToLower().Equals("ok");
                response.displayMessage = response.success ? $"Mensaje enviado por WhatsApp a {phoneNumber}." : $"No se pudo enviar el mensaje por WhatsApp a {phoneNumber}, es probable que la sesión de la cuenta de WhatsApp este cerrada. Avisar al Administrador.";
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Ocurrio un error al momento de realizar el envio de mensaje por WhatsApp.";
                response.errorMessage.Add(ex.Message);
            }

            return response;
        }

        public async Task<ResponseEntidad<List<WSP_UltraMsgResponse>>> SendMessageMultiple(List<string> phoneNumbers, string message) {
            ResponseEntidad<List<WSP_UltraMsgResponse>> response = new ResponseEntidad<List<WSP_UltraMsgResponse>>();

            if(!IsPosibleUseServiceWhatsApp) {
                response.success = false;
                response.displayMessage = "No es posible enviar mensaje por WhatsApp debido a que no se encontró una instancia registrada en la base de datos.";
                return response;
            }

            //string url = $"{InstanciaApiWhatsApp}/messages/chat?token={TokenApiWhatsApp}&to={phones}&body={message}&priority=1";
            string url = $"{InstanciaApiWhatsApp}/messages/chat";

            //creando objeto para la peticion
            string phones = string.Join(",", phoneNumbers);
            WSP_UltraMsgRequest requestBody = new WSP_UltraMsgRequest {
                token = TokenApiWhatsApp,
                to = phones,
                body = message,
                priority = "1"
            };
            var formData = UtilitarioWhatsApp.ObjectToFormUrlEncodedContent(requestBody);

            try {
                var responseServer = await httpClient.PostAsync(url, formData);
                var contentResponse = await responseServer.Content.ReadAsStringAsync();
                response.data = phoneNumbers.Count > 1 ? JsonConvert.DeserializeObject<List<WSP_UltraMsgResponse>>(contentResponse) : new List<WSP_UltraMsgResponse> { JsonConvert.DeserializeObject<WSP_UltraMsgResponse>(contentResponse) };
                response.success = response.data.Any(x => Convert.ToBoolean(x.sent));
                int mensajesCorrectos = response.data.Count(x => Convert.ToBoolean(x.sent));
                int mensajesIncorrectos = response.data.Count(x => !Convert.ToBoolean(x.sent));
                response.displayMessage = response.success ? $"{mensajesCorrectos} mensajes enviados, {mensajesIncorrectos} no enviados." : $"No se logro enviar ni un mensaje por WhatsApp.";
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Ocurrio un error al momento de realizar el envio de mensaje por WhatsApp.";
                response.errorMessage.Add(ex.Message);
            }

            return response;
        }

        public async Task<ResponseEntidad<List<WSP_UltraMsgResponse>>> SendMessageMultipleWhitImage(List<string> phoneNumbers, string message, HttpPostedFileBase image) {
            ResponseEntidad<List<WSP_UltraMsgResponse>> response = new ResponseEntidad<List<WSP_UltraMsgResponse>>();

            if(!IsPosibleUseServiceWhatsApp) {
                response.success = false;
                response.displayMessage = "No es posible enviar mensaje por WhatsApp debido a que no se encontró una instancia registrada en la base de datos.";
                return response;
            }

            //string url = $"{InstanciaApiWhatsApp}/messages/chat?token={TokenApiWhatsApp}&to={phones}&body={message}&priority=1";
            string url = $"{InstanciaApiWhatsApp}/messages/image";

            //creando objeto para la peticion
            string phones = string.Join(",", phoneNumbers);
            WSP_UltraMsgRequest requestBody = new WSP_UltraMsgRequest {
                token = TokenApiWhatsApp,
                to = phones,
                image = UtilitarioWhatsApp.ImageToBase64(image),
                caption = message,
                priority = "10"
            };
            var formData = UtilitarioWhatsApp.ObjectToDictionary(requestBody);
            var encodedItems = formData.Select(i => WebUtility.UrlEncode(i.Key) + "=" + WebUtility.UrlEncode(i.Value));
            var encodedContent = new StringContent(String.Join("&", encodedItems), null, "application/x-www-form-urlencoded");

            try {
                var responseServer = await httpClient.PostAsync(url, encodedContent);
                var contentResponse = await responseServer.Content.ReadAsStringAsync();
                response.data = phoneNumbers.Count > 1 ? JsonConvert.DeserializeObject<List<WSP_UltraMsgResponse>>(contentResponse) : new List<WSP_UltraMsgResponse> { JsonConvert.DeserializeObject<WSP_UltraMsgResponse>(contentResponse) };
                response.success = response.data.Any(x => Convert.ToBoolean(x.sent));
                int mensajesCorrectos = response.data.Count(x => Convert.ToBoolean(x.sent));
                int mensajesIncorrectos = response.data.Count(x => !Convert.ToBoolean(x.sent));
                response.displayMessage = response.success ? $"{mensajesCorrectos} mensajes enviados, {mensajesIncorrectos} no enviados." : $"No se logro enviar ni un mensaje por WhatsApp.";
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Ocurrio un error al momento de realizar el envio de mensaje con imagen por WhatsApp.";
                response.errorMessage.Add(ex.Message);
            }

            return response;
        }
    }
}
