using CapaEntidad.Response;
using CapaEntidad.WhatsApp;
using CapaEntidad.WhatsApp.Response;
using CapaNegocio.WhatsApp;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.WhatsApp {
    [seguridad]
    public class WhatsAppController : Controller {
        private WSP_MensajeriaUltraMsgBL wspMensajeriaUltraMsgBL;
        private readonly WSP_InstanciaUltraMsgBL wspInstanciaUltraMsgBL;

        private string UriServerWhatsApp = ConfigurationManager.AppSettings["UriServerWhatsApp"];
        private string PathStartServiceWhatsApp = ConfigurationManager.AppSettings["PathStartServiceWhatsApp"];
        private string PathStopServiceWhatsApp = ConfigurationManager.AppSettings["PathStopServiceWhatsApp"];
        private string PathService = ConfigurationManager.AppSettings["PathService"];
        private bool runBatAsAdmin = true;

        public WhatsAppController() {
            wspInstanciaUltraMsgBL = new WSP_InstanciaUltraMsgBL();
        }

        #region WhatsApp Api Propia
        public JsonResult Login() {
            var uri = $"{UriServerWhatsApp}/api/session/login";
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new ResponseEntidad<WSP_Login>();

            try {
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                response = client.DownloadString(uri);
                jsonResponse = JsonConvert.DeserializeObject<ResponseEntidad<WSP_Login>>(response);
            } catch(Exception ex) {
                return Json(new { successs = false, displayMessage = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }

            var jsonResult = Json(jsonResponse, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        public JsonResult Logout() {
            var uri = $"{UriServerWhatsApp}/api/session/logout";
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new ResponseEntidad<object>();

            try {
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                response = client.DownloadString(uri);
                jsonResponse = JsonConvert.DeserializeObject<ResponseEntidad<object>>(response);
            } catch(Exception ex) {
                return Json(new { successs = false, displayMessage = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }

            var jsonResult = Json(jsonResponse, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;


            return jsonResult;
        }

        public JsonResult IniciarServicio() {
            var success = false;
            var displayMessage = string.Empty;
            var ExitCode = 1001;
            var StandardOutput = string.Empty;
            var StandardError = string.Empty;
            var AsAdmin = runBatAsAdmin;

            Process process = new Process();

            try {
                if(runBatAsAdmin) {
                    process.StartInfo.FileName = PathStartServiceWhatsApp;
                } else {
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = $"/c {PathStartServiceWhatsApp}";
                }

                process.StartInfo.WorkingDirectory = PathService;
                process.StartInfo.UseShellExecute = runBatAsAdmin;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = !runBatAsAdmin;
                process.StartInfo.RedirectStandardError = !runBatAsAdmin;
                process.StartInfo.Verb = runBatAsAdmin ? "runas" : string.Empty;
                process.Start();

                process.WaitForExit();

                success = process.ExitCode == 0;
                displayMessage = success ? "Servicio encendido correctamente." : $"No se pudo encender el servicio.";

                ExitCode = process.ExitCode;
                if(!runBatAsAdmin) {
                    StandardOutput = process.StandardOutput.ReadToEnd();
                    StandardError = process.StandardError.ReadToEnd();
                }
            } catch(Exception ex) {
                success = false;
                displayMessage = $"Error al intentar encender el servicio. {ex.Message}";
            } finally {
                process.Close();
                process.Dispose();
            }

            var data = new {
                ExitCode,
                StandardOutput,
                StandardError,
                AsAdmin
            };

            return Json(new { success, data, displayMessage }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DetenerServicio() {
            var success = false;
            var displayMessage = string.Empty;
            var ExitCode = 1001;
            var StandardOutput = string.Empty;
            var StandardError = string.Empty;
            var AsAdmin = runBatAsAdmin;

            Process process = new Process();

            try {
                if(runBatAsAdmin) {
                    process.StartInfo.FileName = PathStopServiceWhatsApp;
                } else {
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = $"/c \"{PathStopServiceWhatsApp}\"";
                }

                process.StartInfo.WorkingDirectory = PathService;
                process.StartInfo.UseShellExecute = runBatAsAdmin;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = !runBatAsAdmin;
                process.StartInfo.RedirectStandardError = !runBatAsAdmin;
                process.StartInfo.Verb = runBatAsAdmin ? "runas" : string.Empty;
                process.Start();

                process.WaitForExit();

                success = process.ExitCode == 0;
                displayMessage = success ? "Servicio apagado correctamente." : $"No se pudo apagar el servicio.";

                ExitCode = process.ExitCode;
                if(!runBatAsAdmin) {
                    StandardOutput = process.StandardOutput.ReadToEnd();
                    StandardError = process.StandardError.ReadToEnd();
                }
            } catch(Exception ex) {
                success = false;
                displayMessage = $"Error al intentar apagar el servicio. {ex.Message}";
            } finally {
                process.Close();
                process.Dispose();
            }

            var data = new {
                ExitCode,
                StandardOutput,
                StandardError,
                AsAdmin
            };

            return Json(new { success, data, displayMessage }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerUrlWhatsAppAPI() {
            var jsonResult = Json(new { url = UriServerWhatsApp }, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        #endregion

        #region WhatsApp Api UltraMSG
        [seguridad(false)]
        [HttpPost]
        public async Task<JsonResult> SendMessage(int codSala, string phoneNumber, string message) {
            wspMensajeriaUltraMsgBL = new WSP_MensajeriaUltraMsgBL(codSala);
            ResponseEntidad<WSP_UltraMsgResponse> response = await wspMensajeriaUltraMsgBL.SendMessage(phoneNumber, message);
            return Json(response);
        }
        #endregion  
    }
}