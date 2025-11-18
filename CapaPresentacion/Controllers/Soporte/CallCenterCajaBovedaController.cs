using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Soporte
{
    [seguridad]
    public class CallCenterCajaBovedaController : Controller
    {
        // GET: CallCenterCajaBoveda
        private SEG_UsuarioBL segUsuarioBl = new SEG_UsuarioBL();
        public ActionResult CallCenterCajaBovedaVista()
        {
            return View("~/Views/CallCenterCajaBoveda/CallCenterCajaBovedaVista.cshtml");
        }

        public ActionResult CallCenterResumenSalaBovedaVista()
        {
            return View("~/Views/CallCenterCajaBoveda/CallCenterResumenSalaBovedaVista.cshtml");
        }


        public ActionResult CallCenterModificarResumenSalaBovedaVista(string id, string ip,string url_boveda)
        {
            ViewBag.Id = id;
            ViewBag.Ip = ip;
            ViewBag.UrlBoveda = url_boveda;
            return View("~/Views/CallCenterCajaBoveda/CallCenterModificarResumenSalaBovedaVista.cshtml");            
        }

        [HttpPost]
        public ActionResult ConsultaObtenerCajasXFechaInicioJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";
            int usuarioId = 0;
            SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];
            usuarioId = usuario.UsuarioID;
            url = url + "&usuarioId=" + usuarioId;
            var jsonResponse = new List<ModificarCaja>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                jsonResponse = JsonConvert.DeserializeObject<List<ModificarCaja>>(response, settings);

            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            var jsonResult = Json(new { data = jsonResponse.ToList() }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult ConsultaObtenerModificarCaja(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");

            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = response });
        }
        [HttpPost]
        public ActionResult ModificarProcedimientoAlmacenadoJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";
            int usuarioId = 0;
            SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];
            usuarioId = usuario.UsuarioID;
            url = url + "&usuarioId=" + usuarioId;
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = response });
        }
        [HttpPost]
        public ActionResult RestaurarProcedimientoAlmacenadoJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";
            int usuarioId = 0;
            SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];
            usuarioId = usuario.UsuarioID;
            //url = url + "&usuarioId=" + usuarioId;
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = response });
        }
        public static string GetIPAddress()
        {
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            string IPAddress = " ";
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress;
        }
        [HttpPost]
        public ActionResult RegistrarDatosAuditoriaBDTEC_IAS(string url, string Proceso_Eje, string Descrip_Proceso)
        {
            var client = new System.Net.WebClient();
            var response = "";
            int usuarioId = 0;
            SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];
            usuarioId = usuario.UsuarioID;
            
            string FechaReg = DateTime.Now.ToString();
            int CodUsuario = 0;
            string Usuario_SiS = usuario.UsuarioNombre;
            //Proceso_Eje = "Listar";
            //Descrip_Proceso = "";
            string Usuario_Host = "IAS";
            string IP_Maquina = GetIPAddress();
            string Hos_Maquina = Newtonsoft.Json.JsonConvert.SerializeObject(Session["usuario"]);
            string Dato_Anterior = "";
            int CodFormulario = 0;
            int Sistema = 0;
            url = url + "FechaReg='" + FechaReg+ "'&CodUsuario="+ CodUsuario
                + "&Usuario_SiS='" + Usuario_SiS
                + "'&Proceso_Eje='" + Proceso_Eje
                + "'&Descrip_Proceso='" + Descrip_Proceso
                + "'&Usuario_Host='" + Usuario_Host
                + "'&IP_Maquina='" + IP_Maquina
                + "'&Hos_Maquina='" + Hos_Maquina
                + "'&Dato_Anterior='" + Dato_Anterior
                + "'&CodFormulario=" + CodFormulario
                + "&Sistema=" + Sistema;

            //string FechaReg, int CodUsuario,
            //string Usuario_SiS, string Proceso_Eje,
            //string Descrip_Proceso, string Usuario_Host,
            //string IP_Maquina, string Hos_Maquina,
            //string Dato_Anterior, string CodFormulario,
            //string Sistema)

            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = response });
        }
    }
}