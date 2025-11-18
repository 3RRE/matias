using CapaEntidad;
using CapaPresentacion.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaNegocio;
using System.Text.RegularExpressions;
using System.Text;
using System.Web.Script.Serialization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;
using CapaPresentacion.Utilitarios;

namespace CapaPresentacion.Controllers.Soporte
{
    [seguridad]
    public class CallCenterController : Controller
    {

        private SEG_UsuarioBL segUsuarioBl = new SEG_UsuarioBL();
        private SalaBL salaBL = new SalaBL();
        // GET: CallCenter
        public ActionResult CallCenterVista()
        {
            return View("~/Views/CallCenter/CallCenterVista.cshtml");
        }

        public ActionResult CallCenterRegistroVista()
        {
            return View("~/Views/CallCenter/CallCenterRegistroVista.cshtml");
        }

        public ActionResult CallCenterMovimientoAuxiliarCaja()
        {
            return View("~/Views/CallCenter/CallCenterMovimientoAuxiliarCaja.cshtml");
        }

        public ActionResult CallCenterConsultaTicketRegistradoXpuntoVenta()
        {
            return View("~/Views/CallCenter/CallCenterConsultaTicketRegistradoXpuntoVenta.cshtml");
        }

        public ActionResult CallCenterObtenerCajas()
        {
            return View("~/Views/CallCenter/CallCenterObtenerCajas.cshtml");
        }

        public ActionResult CallCenterModificarTickets()
        {
            return View("~/Views/CallCenter/CallCenterModificarTickets.cshtml");
        }
        public ActionResult CallCenterBloquearDesbloquearUsuario()
        {
            return View("~/Views/CallCenter/CallCenterBloquearDesbloquearUsuario.cshtml");
        }

        [HttpPost]
        public ActionResult ConsultaObtenerUsuariosListadoJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            var jsonResponse = new List<Usuarios>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                jsonResponse = JsonConvert.DeserializeObject<List<Usuarios>>(response, settings);

            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse.ToList() });
        }
        [HttpPost]
        public ActionResult ConsultaTicketRegistradoMovAuxiliarXMaqAlternoAndItemCajaJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            var jsonResponse = new List<Detalle_movaux_por_maquinaEntidad>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                jsonResponse = JsonConvert.DeserializeObject<List<Detalle_movaux_por_maquinaEntidad>>(response, settings);

            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse.ToList() });
        }

        [HttpPost]
        public ActionResult ConsultaTicketRegistradoMovAuxiliarListadoJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            var jsonResponse = new List<Detalle_movaux_por_maquinaEntidad>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                //jsonResponse = JsonConvert.DeserializeObject<List<Detalle_movaux_por_maquinaEntidad>>(response);
                jsonResponse = JsonConvert.DeserializeObject<List<Detalle_movaux_por_maquinaEntidad>>(response, settings);

            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse.ToList() });
        }

        public ActionResult ConsultaObtenerProcesosTitoJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            var jsonResponse = new List<ProcesosTitoEntidad>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                jsonResponse = JsonConvert.DeserializeObject<List<ProcesosTitoEntidad>>(response, settings);

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

        public ActionResult ConsultaObtenerDetalle_movaux_por_maquinaJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            var jsonResponse = new List<Detalle_movaux_por_maquinaEntidad>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                jsonResponse = JsonConvert.DeserializeObject<List<Detalle_movaux_por_maquinaEntidad>>(response, settings);

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

        public ActionResult ConsultaObtenerDet0001TTO_00HJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            var jsonResponse = new List<Det0001TTO_00H>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                jsonResponse = JsonConvert.DeserializeObject<List<Det0001TTO_00H>>(response, settings);

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

        public ActionResult ConsultaObtenerDet0001TTOJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            var jsonResponse = new List<Det0001TTOEntidad>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                jsonResponse = JsonConvert.DeserializeObject<List<Det0001TTOEntidad>>(response, settings);

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

        public ActionResult ConsultaEliminarProcesoTitoJson(string url)
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
        public ActionResult ConsultaEliminarDet0001TTO_00HJson(string url)
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

        public ActionResult ConsultaEliminarDet0001TTOJson(string url)
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

        public ActionResult ConsultaModificarProcesoTitoJson(string url)
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
        public ActionResult ConsultaModificarDet0001TTO_00HJson(string url)
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
        public ActionResult ConsultaModificarDetalle_movaux_por_maquinaJson(string url)
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
        public ActionResult ConsultaModificarDet0001TTOJson(string url)
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
        public ActionResult ConsultaTicketRegistradoXpuntoVentaListadoJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            var jsonResponse = new List<Det0001TTO_00H>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                jsonResponse = JsonConvert.DeserializeObject<List<Det0001TTO_00H>>(response, settings);

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

        [HttpPost]
        public ActionResult ConsultaObtenerCajasXFechaInicioJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

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
        public ActionResult ConsultaObtenerResumenSalaJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            var jsonResponse = new List<ResumenSalaEntidad>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                jsonResponse = JsonConvert.DeserializeObject<List<ResumenSalaEntidad>>(response, settings);

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

        public ActionResult ConsultaObtenerResumenSalaIdJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            var jsonResponse = new List<ResumenSalaEntidad>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                jsonResponse = JsonConvert.DeserializeObject<List<ResumenSalaEntidad>>(response, settings);

            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse.ToList() });
        }

        public ActionResult ConsultaModificarResumenSalaJson(string url)
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

        public ActionResult ConsultaMemoria_Bajar1_InicioJson(string url)
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

        public ActionResult ConsultaMemoria_Bajar1_FinJson(string url)
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

        public ActionResult ConsultaReducirLogJson(string url)
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

        public ActionResult ConsultaObtenerClientesPtkJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            var jsonResponse = new List<ClientesPtkEntidad>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                jsonResponse = JsonConvert.DeserializeObject<List<ClientesPtkEntidad>>(response, settings);

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

        public ActionResult ConsultaObtenerPromocionSorteoActivoJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            var jsonResponse = new List<PromocionSorteoEntidad>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                jsonResponse = JsonConvert.DeserializeObject<List<PromocionSorteoEntidad>>(response, settings);

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

        public ActionResult ConsultaAdicionarPuntoCuponJson(string url)
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

        public ActionResult ConsultaRestarPuntoCuponJson(string url)
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

        public ActionResult CallCenterReducirMemoriaVista()
        {
            return View();
        }

        public ActionResult CallCenterSoportePlayerTrackingVista()
        {
            return View();
        }

        public ActionResult TicketRegistradoVista()
        {
            return View();
        }

        public ActionResult VerificarComportamientoMaquinaVista()
        {
            return View();
        }

        public ActionResult TicketEmitidoPorMontoVista()
        {
            return View();
        }
        public ActionResult CallCenterResumenSalaVista()
        {
            return View();
        }
        public ActionResult CallCenterModificarResumenSalaVista(string id, string ip)
        {
            ViewBag.Id = id;
            ViewBag.Ip = ip;
            return View();
        }
        public ActionResult TicketBolsaVista()
        {
            return View();
        }
        [HttpPost]
        public ActionResult InsertaDetalleTipoFicha(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";
            var response1 = false;
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                response = Regex.Replace(response, "[^0-9]", "");
                var respon = Int32.Parse(response);
                response1 = respon >= 0 ? true : false;

            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }

            return Json(new { data = response1 });
        }
        [HttpPost]
        public ActionResult ObtenerSeguimientoPuntoCuponGenerado(string url)
        {
            var client = new System.Net.WebClient { Encoding = System.Text.Encoding.UTF8 };
            var response = "";

            var jsonResponse = new List<SeguimientoPuntoCuponGeneradoEntidad>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                jsonResponse = JsonConvert.DeserializeObject<List<SeguimientoPuntoCuponGeneradoEntidad>>(response, settings);

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
        [HttpPost]
        public ActionResult ConsultaVerificarComportamientoMaquinaJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            var jsonResponse = new List<ContadoresOnline>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                jsonResponse = JsonConvert.DeserializeObject<List<ContadoresOnline>>(response, settings);

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
        [HttpPost]
        public ActionResult ConsultaTicketEmitidoPorMontoJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            var jsonResponse = new List<Det0001TTO_00H>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                jsonResponse = JsonConvert.DeserializeObject<List<Det0001TTO_00H>>(response, settings);

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
        [HttpPost]
        public ActionResult ConsultaTicketRegistradoJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            var jsonResponse = new List<Det0001TTO_00H>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                jsonResponse = JsonConvert.DeserializeObject<List<Det0001TTO_00H>>(response, settings);

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
        [HttpPost]
        public ActionResult ModificarProcedimientoAlmacenadoJson(string url)
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
        public ActionResult RestaurarProcedimientoAlmacenadoJson(string url)
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
        public ActionResult ConsultaTicketRegistradoPorFechasJson(string url,DateTime fechaIni,DateTime fechaFin)
        {
            var client = new MyWebClient();
            var response = "";

            var jsonResponse = new List<Det0001TTO_00H>();
            object oEnvio = new object();
            try
            {
                oEnvio = new
                {
                    fechaIni = fechaIni,
                    fechaFin= fechaFin,
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;

                response = client.UploadString(url, "POST",inputJson);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                jsonResponse = JsonConvert.DeserializeObject<List<Det0001TTO_00H>>(response, settings);

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
        [HttpPost]
        public ActionResult ConsultaTicketRegistradosPorFechasExcel(string url,DateTime fechaIni,DateTime fechaFin, int codSala)
        {
            var client = new MyWebClient();
            var response = "";
            string base64String = "";
            string mensaje = string.Empty;
            bool respuesta = false;
            string excelName = string.Empty;
            var jsonResponse = new List<Det0001TTO_00H>();
            object oEnvio = new object();
            try
            {
                oEnvio = new
                {
                    fechaIni = fechaIni,
                    fechaFin = fechaFin,
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;

                response = client.UploadString(url, "POST", inputJson);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                jsonResponse = JsonConvert.DeserializeObject<List<Det0001TTO_00H>>(response, settings);

                if (jsonResponse.Count > 0)
                {
                    SalaEntidad sala = salaBL.SalaListaIdJson(codSala);
                    SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];
                    //Creacion del Excel
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Tickets Bolsa");

                    workSheet.Cells["B1"].Value = "Reporte Tickets Bolsa - Sala : "+sala.Nombre;
                    workSheet.Cells["B1:C1"].Style.Font.Bold = true;

                    workSheet.Cells["B1"].Style.Font.Size = 20;
                    workSheet.Cells["B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["B1:I1"].Merge = true;
                    workSheet.Cells["B1:I1"].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                    workSheet.Cells["B2"].Value = "Usuario :";
                    workSheet.Cells["B2"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells["C2"].Value = usuario.UsuarioNombre;
                    workSheet.Cells["C2"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells["B2"].Style.Font.Bold = true;

                    workSheet.Cells["D2"].Value = "Fecha Reporte :";
                    workSheet.Cells["D2"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells["E2"].Value = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                    workSheet.Cells["E2"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells["D2"].Style.Font.Bold = true;

                    workSheet.Cells["F2"].Value = "Fecha Inicio :";
                    workSheet.Cells["F2"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells["G2"].Value = fechaIni.ToString("dd/MM/yyyy");
                    workSheet.Cells["G2"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells["F2"].Style.Font.Bold = true;

                    workSheet.Cells["H2"].Value = "Fecha Fin :";
                    workSheet.Cells["H2"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells["I2"].Value = fechaFin.ToString("dd/MM/yyyy");
                    workSheet.Cells["I2"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells["H2"].Style.Font.Bold = true;

                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[3, 2].Value = "Item";
                    workSheet.Cells[3, 3].Value = "Fecha Apertura";
                    workSheet.Cells[3, 4].Value = "Tipo Venta";
                    workSheet.Cells[3, 5].Value = "Punto Venta";
                    workSheet.Cells[3, 6].Value = "Tito Fecha Ini";
                    workSheet.Cells[3, 7].Value = "Tito Nro. Ticket";
                    workSheet.Cells[3, 8].Value = "Tito Monto Ticket";
                    workSheet.Cells[3, 9].Value = "Tito Monto Ticket No Cobrable";
                    workSheet.Cells[3, 10].Value = "Tipo Venta Fin";
                    workSheet.Cells[3, 11].Value = "Punto Venta Fin";
                    workSheet.Cells[3, 12].Value = "Tito Fecha Fin";
                    workSheet.Cells[3, 13].Value = "Cod. Cliente";
                    workSheet.Cells[3, 14].Value = "Tipo Ticket";
                    workSheet.Cells[3, 15].Value = "Estado";
                    workSheet.Cells[3, 16].Value = "Id. Tipo Moneda";
                    workSheet.Cells[3, 17].Value = "Motivo";
                    workSheet.Cells[3, 18].Value = "Id. Tipo Pago";
                    workSheet.Cells[3, 19].Value = "Tipo Proceso";
                    workSheet.Cells[3, 20].Value = "R. Estado";
                    workSheet.Cells[3, 21].Value = "Tipo Ingreso";
                    workSheet.Cells[3, 22].Value = "Fecha Apertura Real";
                    workSheet.Cells[3, 23].Value = "Punto Venta Min.";
                    workSheet.Cells[3, 24].Value = "Fecha Recreativa";
                    workSheet.Cells[3, 25].Value = "Turno";
                    workSheet.Cells[3, 26].Value = "Cod. Caja";
                    workSheet.Cells[3, 27].Value = "Player Tracking";
                    //Body of table  
                    int recordIndex = 4;
                    int total = jsonResponse.Count;
                    foreach (var registro in jsonResponse)
                    {
                        string TipoTicket = string.Empty;
                        string EstadoTicket = string.Empty;
                        switch (registro.Estado)
                        {
                            case 1:
                                EstadoTicket = "NO COBRADO";
                                break;
                            case 2:
                                EstadoTicket = "COBRADO";
                                break;
                            case 3:
                                EstadoTicket = "VENCIDO";
                                break;
                            default:
                                EstadoTicket = "ANULADO";
                                break;
                        }
                        switch (registro.tipo_ticket)
                        {
                            case "1":
                                TipoTicket = "NORMAL";
                                break;
                            case "2":
                                TipoTicket = "PAGO ANUAL";
                                break;
                            case "3":
                                TipoTicket = "";
                                break;
                            case "4":
                                TipoTicket = "PROMOCIONAL";
                                break;
                            default:
                                TipoTicket = "PROMOCIONAL";
                                break;
                        }
                        workSheet.Cells[recordIndex, 2].Value = registro.Item;
                        workSheet.Cells[recordIndex, 3].Value = registro.Fecha_Apertura.ToString("dd/MM/yyyy hh:mm tt");
                        workSheet.Cells[recordIndex, 4].Value = registro.Tipo_venta;
                        workSheet.Cells[recordIndex, 5].Value = registro.Punto_venta;
                        workSheet.Cells[recordIndex, 6].Value = registro.Tito_fechaini.ToString("dd/MM/yyyy hh:mm tt");
                        workSheet.Cells[recordIndex, 7].Value = registro.Tito_NroTicket;
                        workSheet.Cells[recordIndex, 8].Value = registro.Tito_MontoTicket;
                        workSheet.Cells[recordIndex, 9].Value = registro.Tito_MTicket_NoCobrable;
                        workSheet.Cells[recordIndex, 10].Value = registro.Tipo_venta_fin;
                        workSheet.Cells[recordIndex, 11].Value = registro.Punto_venta_fin;
                        workSheet.Cells[recordIndex, 12].Value = registro.Tito_fechafin.ToString("dd/MM/yyyy hh:mm tt");
                        workSheet.Cells[recordIndex, 13].Value = registro.codclie;
                        workSheet.Cells[recordIndex, 14].Value = TipoTicket;
                        workSheet.Cells[recordIndex, 15].Value = EstadoTicket;
                        workSheet.Cells[recordIndex, 16].Value = registro.IdTipoMoneda;
                        workSheet.Cells[recordIndex, 17].Value = registro.Motivo;
                        workSheet.Cells[recordIndex, 18].Value = registro.IdTipoPago;
                        workSheet.Cells[recordIndex, 19].Value = registro.Tipo_Proceso;
                        workSheet.Cells[recordIndex, 20].Value = registro.r_Estado;
                        workSheet.Cells[recordIndex, 21].Value = registro.Tipo_Ingreso;
                        workSheet.Cells[recordIndex, 22].Value = Convert.ToDateTime(registro.Fecha_Apertura_Real).ToString("dd/MM/yyyy hh:mm tt");
                        workSheet.Cells[recordIndex, 23].Value = registro.PuntoVentaMin;
                        workSheet.Cells[recordIndex, 24].Value = registro.fecha_reactiva.ToString("dd/MM/yyyy hh:mm tt");
                        workSheet.Cells[recordIndex, 25].Value = registro.turno;
                        workSheet.Cells[recordIndex, 26].Value = registro.codCaja;
                        workSheet.Cells[recordIndex, 27].Value = registro.player_tracking;

                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:AA3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:AA3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:AA3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:AA3"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B3:AA3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:AA3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:AA3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:AA3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B3:AA3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:AA3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:AA3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:AA3"].Style.Border.Bottom.Color.SetColor(colborder);

                    int filaFooter_ = recordIndex;
                    workSheet.Cells["B" + filaFooter_ + ":Z" + filaFooter_].Merge = true;
                    workSheet.Cells["B" + filaFooter_ + ":Z" + filaFooter_].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter_ + ":Z" + filaFooter_].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter_ + ":Z" + filaFooter_].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter_ + ":Z" + filaFooter_].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter_ + ":Z" + filaFooter_].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[filaFooter_, 2].Value = "Total : " + (total) + " Registros";


                    workSheet.Cells[3, 2, filaFooter_, 27].AutoFilter = true;
                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 30;
                    workSheet.Column(4).Width = 15;
                    workSheet.Column(5).Width = 30;
                    workSheet.Column(6).Width = 30;
                    workSheet.Column(7).Width = 30;
                    workSheet.Column(10).Width = 15;
                    workSheet.Column(11).Width = 30;
                    workSheet.Column(12).Width = 30;
                    workSheet.Column(17).Width = 30;
                    workSheet.Column(22).Width = 30;
                    workSheet.Column(23).Width = 30;
                    workSheet.Column(24).Width = 30;

                    excelName = "ticketsbolsa_" + fechaIni.ToString("dd_MM_yyyy") + "_al_" + fechaFin.ToString("dd_MM_yyyy") + "_Soporte_"+sala.Nombre+".xlsx";
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());
                    //Termino excel

                    mensaje = "Descargando Archivo";
                    respuesta = true;
                }
                else
                {
                    mensaje = "No se encontraron registros";
                }
               
            }
            catch (Exception ex)
            {
                respuesta = false;
                mensaje = ex.Message + ", Llame Administrador";
            }
            var jsonResult = Json(new { data = base64String, excelName, respuesta  }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}