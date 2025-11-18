using CapaEntidad;
using CapaEntidad.ProgresivoOffline;
using CapaNegocio;
using CapaNegocio.Progresivo;
using CapaPresentacion.Filters;
using CapaPresentacion.Models;
using CapaPresentacion.Utilitarios;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CapaEntidad.Response;
using System.Threading.Tasks;
using CapaPresentacion.Reports;
using System.IO;
using CapaEntidad.WhatsApp;
using CapaNegocio.WhatsApp;

namespace CapaPresentacion.Controllers.Progresivo
{
    [seguridad]
    [TokenProgresivo(true)]
    public class ProgresivoController : Controller
    {

        private ProgresivoBL progesivoBL = new ProgresivoBL();
        private DestinatarioBL destinatarioBL = new DestinatarioBL ();
        private MarcaMaquinaBL marcamaquinabl = new MarcaMaquinaBL();
        private JuegoBL juegobl = new JuegoBL();
        private ModeloMaquinaBL modelomaquinabl = new ModeloMaquinaBL();
        private AumentoCreditoMaquinaBL _aumentoCreditoMaquinaBL=new AumentoCreditoMaquinaBL();
        private readonly SalaBL _salaBl = new SalaBL();
        private readonly ServiceHelper _serviceHelper = new ServiceHelper();
        private WSP_MensajeriaUltraMsgBL wspMensajeriaUltraMsgBL;

        public ActionResult ProgresivoVista()
        {
            return View("~/Views/Progresivo/ProgresivoVista.cshtml");
        }
        public ActionResult ProgresivoRegistroVista()
        {
            //Registrar pozo = 0, entonces no envia la data;
            //string RegistrarPozo = ConfigurationManager.AppSettings["RegistrarPozo"];
            //ViewBag.RegistrarPozo = Convert.ToInt32(RegistrarPozo);
            return View("~/Views/Progresivo/ProgresivoRegistroVista.cshtml");
        }
        public ActionResult MantenimientoProgresivoVista()
        {
            return View("~/Views/Progresivo/MantenimientoProgresivoVista.cshtml");
        }
        public ActionResult ReporteProgresivoVista()
        {
            return View("~/Views/Progresivo/ReporteProgresivoVista.cshtml");
        }
        public ActionResult ProgresivoListadoMaquina()
        {
            return View();
        }
        public ActionResult ProgresivoListadoMarcaModelo() {
            return View();
        }
        public ActionResult ProgresivoRegistrarMaquina(int id,string url,bool consultaVpn=false,string urlPublica="",string urlPrivada="")
        {
            ViewBag.consultaVpn=consultaVpn;
            ViewBag.urlPublica=urlPublica;
            ViewBag.urlPrivada = urlPrivada;
            ViewBag.id_codProgresivo = id;
            ViewBag.url_sala = url;
            return View();
        }

        public ActionResult ProgresivoEditarMaquina(string id, string url, int codProgresivo, bool consultaVpn=false,string urlPublica="",string urlPrivada="")
        {
            ViewBag.consultaVpn = consultaVpn;
            ViewBag.urlPublica = urlPublica;
            ViewBag.urlPrivada = urlPrivada;
            ViewBag.id_codProgresivo = codProgresivo;
            ViewBag.url = url;
            ViewBag.id = id;
            return View();
        }

        public ActionResult ConsultarListaMaquinaProgresivo(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            var jsonResponse = new List<MaquinaProgresivoEntidad>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                jsonResponse = JsonConvert.DeserializeObject<List<MaquinaProgresivoEntidad>>(response, settings);

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

        public ActionResult ConsultarObtenerMaquinaJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            var jsonResponse = new MaquinaProgresivoEntidad();
            try
            {
                client.Headers.Add("content-type", "application/json");                
                response = client.DownloadString(url);
                jsonResponse = JsonConvert.DeserializeObject<MaquinaProgresivoEntidad>(response);

            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse });
        }

        public ActionResult ConsultarListaMarcaMaquinaJson(string url)
        {
            //var errormensaje = "";
            //var lista = new List<MarcaMaquinaEntidad>();
            //try
            //{
            //    lista = marcamaquinabl.MarcaMaquinaListaJson();

            //}
            //catch (Exception exp)
            //{
            //    errormensaje = exp.Message + ", Llame Administrador";
            //}
            //var listado = from list in lista
            //              orderby list.CodMarcaMaquina descending
            //              select new
            //              {
            //                  list.CodMarcaMaquina,
            //                  list.Nombre,
            //                  list.ColorHexa,
            //                  list.Sigla,
            //                  list.FechaRegistro,
            //                  list.FechaModificacion,
            //                  list.Activo,
            //                  EstadoMarcaMaquina = list.Estado.ToString() == "1" ? "Habilitado" : "Deshabilitado",
            //                  list.CodRD,
            //                  list.CodUsuario
            //      //            EstadoNombre = list.EstadoEmpleado.ToString() == "1" ? "Habilitado" : "Deshabilitado"
            //              };
            //return Json(new { data = listado.ToList(), mensaje = errormensaje });
        
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new List<Marca>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.DownloadString(url);
                jsonResponse = JsonConvert.DeserializeObject<List<Marca>>(response);
            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse.ToList() });
        }

        public ActionResult ConsultarListaJuegoMaquinaJson(string url)
        {
            //var errormensaje = "";
            //var lista = new List<JuegoEntidad>();
            //try
            //{
            //    lista = juegobl.ListarJuegoMaquinaJson(id);

            //}
            //catch (Exception exp)
            //{
            //    errormensaje = exp.Message + ", Llame Administrador";
            //}
            //var listado = from list in lista
            //              orderby list.CodJuego descending
            //              select new
            //              {
            //                  list.CodJuego,
            //                  list.CodMarcaMaquina,
            //                  list.Nombre,
            //                  list.JuegosAlternos,
            //                  list.ColorHexa,
            //                  list.Sigla,
            //                  list.FechaRegistro,
            //                  list.FechaModificacion,
            //                  list.Activo,
            //                  EstadoMarcaMaquina = list.Estado.ToString() == "1" ? "Habilitado" : "Deshabilitado",
            //                  list.CodRD,
            //                  list.CodUsuario
            //              };
            //return Json(new { data = listado.ToList(), mensaje = errormensaje });

            //var client = new System.Net.WebClient();
            //var response = "";

            //var jsonResponse = new List<MaquinaProgresivoEntidad>();
            //List<JuegoEntidad> juego_ = new List<JuegoEntidad>();
            //try
            //{
            //    client.Headers.Add("content-type", "application/json");
            //    response = client.UploadString(url, "POST");
            //    var settings = new JsonSerializerSettings
            //    {
            //        NullValueHandling = NullValueHandling.Ignore,
            //        MissingMemberHandling = MissingMemberHandling.Ignore
            //    };
            //    jsonResponse = JsonConvert.DeserializeObject<List<MaquinaProgresivoEntidad>>(response, settings);
            //    if (jsonResponse.Count() > 0)
            //    {
            //        var listaTurno = jsonResponse.GroupBy(x => new{x.Juego,}).Select(group => new {group.Key.Juego,Count = group.Count()}).OrderBy(x => x.Juego).ToList();
            //        foreach(var registro in listaTurno)
            //        {
            //            juego_.Add(new JuegoEntidad { Nombre= registro.Juego });
            //        }
            //    }
            //    foreach (MaquinaProgresivoEntidad registro in jsonResponse){

            //    }

            //}
            //catch (Exception ex)
            //{
            //    response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
            //    return Json(new { mensaje = ex.Message.ToString() });
            //}
            //var jsonResult = Json(new { data = juego_.ToList() }, JsonRequestBehavior.AllowGet);
            //jsonResult.MaxJsonLength = int.MaxValue;
            //return jsonResult;

            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new List<MaquinaJuego>();
            try {
                client.Headers.Add("content-type", "application/json");
                response = client.DownloadString(url);
                jsonResponse = JsonConvert.DeserializeObject<List<MaquinaJuego>>(response);
            } catch(Exception ex) {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse.ToList() });
        }


        public ActionResult ConsultarListaModeloMaquinaJson(string url)
        {
            //var errormensaje = "";
            //var lista = new List<ModeloMaquinaEntidad>();
            //try
            //{
            //    lista = modelomaquinabl.ListaModeloMaquinaJson(id);

            //}
            //catch (Exception exp)
            //{
            //    errormensaje = exp.Message + ", Llame Administrador";
            //}
            //var listado = from list in lista
            //              orderby list.CodModeloMaquina descending
            //              select new
            //              {
            //                  list.CodModeloMaquina,
            //                  list.CodMarcaMaquina,
            //                  list.Nombre,
            //                  list.Ncmod,
            //                  list.Cimod,
            //                  list.ColorHexa,
            //                  list.Sigla,
            //                  list.FechaRegistro,
            //                  list.FechaModificacion,
            //                  list.Activo,
            //                  EstadoMarcaMaquina = list.Estado.ToString() == "1" ? "Habilitado" : "Deshabilitado",
            //                  list.CodRD,
            //                  list.CodUsuario
            //              };
            //return Json(new { data = listado.ToList(), mensaje = errormensaje });

            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new List<ModeloMaquinaEntidad>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.DownloadString(url);
                jsonResponse = JsonConvert.DeserializeObject<List<ModeloMaquinaEntidad>>(response);
            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse.ToList() });
        }

        public ActionResult ConsultarGuardarMaquinaProgresivo(Maquina parametros,string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            try
            {
                string parameters = "Canal="+parametros.Canal+"&Juego="+parametros.Juego+"&Toquen="+ parametros.Toquen.ToString().Replace(',','.') + "&Estado="+parametros.Estado+"&MarcaID="+parametros.MarcaID+"&ModeloID="+parametros.ModeloID+"&codigo_alterno="+parametros.codigo_alterno+ "&SlotID="+parametros.SlotID+ "&nombre_modelo="+parametros.nombre_modelo+"&nombre_marca="+parametros.nombre_marca;
                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    response = wc.UploadString(url,"POST", parameters);
                }
            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = response });
        }

        public ActionResult ConsultarModificarMaquinaProgresivo(Maquina parametros,string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            try
            {
                string parameters = "Canal="+parametros.Canal+"&Juego="+parametros.Juego+"&Toquen="+ parametros.Toquen.ToString().Replace(',','.') + "&Estado="+parametros.Estado+"&MarcaID="+parametros.MarcaID+"&ModeloID="+parametros.ModeloID+"&codigo_alterno="+parametros.codigo_alterno+ "&SlotID="+parametros.SlotID+ "&nombre_modelo="+parametros.nombre_modelo+"&nombre_marca="+parametros.nombre_marca;

                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    response = wc.UploadString(url,"PUT", parameters);
                }
            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = response });
        }

        public ActionResult ConsultarEliminarMaquinaProgresivo(Maquina parametros,string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            try
            {
                string parameters = "Canal=" + parametros.Canal + "&Juego=" + parametros.Juego + "&Toquen=" + parametros.Toquen.ToString().Replace(',', '.') + "&Estado=" + parametros.Estado + "&MarcaID=" + parametros.MarcaID + "&ModeloID=" + parametros.ModeloID + "&codigo_alterno=" + parametros.codigo_alterno + "&SlotID=" + parametros.SlotID + "&nombre_modelo=" + parametros.nombre_modelo + "&nombre_marca=" + parametros.nombre_marca;

                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    response = wc.UploadString(url, "DELETE", parameters);
                }
            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = response });
        }

        public ActionResult ConsultarObtenerMarcaMaquinaProgresivo(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            var jsonResponse = new Marca();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.DownloadString(url);
                jsonResponse = JsonConvert.DeserializeObject<Marca>(response);

            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse });
        }
        public ActionResult ConsultarGuardarMarcaMaquinaProgresivo(string MarcaID,string Nombre,string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            try
            {
                string parameters = "MarcaID=" + MarcaID + "&Nombre=" + Nombre + "&Estado=1";

                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    response = wc.UploadString(url, "POST", parameters);
                }
            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = response });
        }

        public ActionResult ConsultarGuardarModeloMaquinaProgresivo(string ModeloID,string MarcaID,string Nombre,string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            try
            {
                string parameters = "ModeloID="+ ModeloID +"&MarcaID=" + MarcaID + "&Nombre=" + Nombre + "&Estado=1";

                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    response = wc.UploadString(url, "POST", parameters);
                }
            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = response });
        }

        #region Llamadas al Api ServiceWebOnline VISTA GANADORES PROGRESIVO
        [seguridad(false)]
        [HttpPost]
        public ActionResult ProgresivoGanadoresListadoJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new List<GanadorResponse>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                jsonResponse = JsonConvert.DeserializeObject<List<GanadorResponse>>(response);
            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse.ToList() });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ProgresivoGanadoresDetalleContadoresListadoJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new List<DetalleContadores>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                //json = webClient.UploadString(url + "/api/RegistroProgresivo/GuardarDetalles", "PUT", lista_stringjson/* "{some:\"json data\"}"*/);
                jsonResponse = JsonConvert.DeserializeObject<List<DetalleContadores>>(response);
                if(jsonResponse.Count > 0) {
                    jsonResponse.ForEach(x => x.FechaCompleta = new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day, x.Hora.Hour, x.Hora.Minute, x.Hora.Second));
                    jsonResponse = jsonResponse.OrderBy(x => x.FechaCompleta).ToList();
                }
            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse.ToList() });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ProgresivoDetalleContadoresListadoJson(DateTime fechaInicio,DateTime fechaFin,string codMaq,string url) {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new List<DetalleContadores>();
            object oEnvio = new object();
            try {
                oEnvio = new {
                    codMaq = codMaq,
                    fechaIni = fechaInicio.ToString("dd-MM-yyyy HH:mm:ss"),
                    fechaFin = fechaFin.ToString("dd-MM-yyyy HH:mm:ss")
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST",inputJson);
                //json = webClient.UploadString(url + "/api/RegistroProgresivo/GuardarDetalles", "PUT", lista_stringjson/* "{some:\"json data\"}"*/);
                jsonResponse = JsonConvert.DeserializeObject<List<DetalleContadores>>(response);
                //if(jsonResponse.Count > 0) {
                //    jsonResponse.ForEach(x => x.FechaCompleta = new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day, x.Hora.Hour, x.Hora.Minute, x.Hora.Second));
                //    jsonResponse = jsonResponse.OrderBy(x => x.FechaCompleta).ToList();
                //}
            } catch(Exception ex) {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                List<DetalleContadores> lista= new List<DetalleContadores>();
                return Json(new { data=lista });
            }
            return Json(new { data = jsonResponse.ToList() });
        }
        #endregion

        #region Llamadas al Api ServiceWebOnline VISTA REGISTRO PROGRESIVO
        [seguridad(false)]
        [HttpPost]
        public ActionResult ProgresivoListarImagenesJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new List<ListadoImagen>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.DownloadString(url);
                jsonResponse = JsonConvert.DeserializeObject<List<ListadoImagen>>(response);
            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse.ToList() });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ProgresivoActivoObtenerJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new ProgresivoActivo();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                //json = webClient.UploadString(url + "/api/RegistroProgresivo/GuardarDetalles", "PUT", lista_stringjson/* "{some:\"json data\"}"*/);
                jsonResponse = JsonConvert.DeserializeObject<ProgresivoActivo>(response);
            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ProgresivoPozosActualesObtenerJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new List<PozosActuales>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                //json = webClient.UploadString(url + "/api/RegistroProgresivo/GuardarDetalles", "PUT", lista_stringjson/* "{some:\"json data\"}"*/);
                jsonResponse = JsonConvert.DeserializeObject<List<PozosActuales>>(response);
            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse.ToList() });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ProgresivoDetalleListarJson(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new List<ProgresivoDetalle>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                //json = webClient.UploadString(url + "/api/RegistroProgresivo/GuardarDetalles", "PUT", lista_stringjson/* "{some:\"json data\"}"*/);
                jsonResponse = JsonConvert.DeserializeObject<List<ProgresivoDetalle>>(response);
            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse.ToList() });
        }
        [HttpPost]
        public ActionResult ProgresivoGuardarCabeceraJson(List<string> Lista, string url)
        {
            var client = new System.Net.WebClient();
            var response = "";
            try
            {
                client.Headers.Add("content-type", "application/json");
                //response = client.UploadString(url, "POST");          
                string parametros = JsonConvert.SerializeObject(Lista);
                response = client.UploadString(url, "POST", parametros);
                //jsonResponse = JsonConvert.DeserializeObject<List<ProgresivoDetalle>>(response);
            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = response });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ProgresivoDetallesGuardarJson(List<DetalleProgresivo> listaPozos, string url)
        {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new List<string>();
            try
            {
                client.Headers.Add("content-type", "application/json");
                //response = client.UploadString(url, "POST");          
                string parametros = JsonConvert.SerializeObject(listaPozos);
                response = client.UploadString(url, "PUT", parametros);
                jsonResponse = JsonConvert.DeserializeObject<List<string>>(response);
            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ProgresivoPozoInsertarJson(List<string> lista, string url)
        {
            var client = new System.Net.WebClient();
            var response = "";
            try
            {
                client.Headers.Add("content-type", "application/json");
                //response = client.UploadString(url, "POST");          
                string parametros = JsonConvert.SerializeObject(lista);
                response = client.UploadString(url, "POST", parametros);
            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = response });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ProgresivoHistoricoInsertarJson(string objStr, int codSala, int codProgresivo)
        {
            var response = false;
            string error = "";
            try
            {
                var historialProgresivo = new HistorialProgresivoEntidad();
                historialProgresivo.CodSala = codSala;
                historialProgresivo.CodProgresivo = codProgresivo;
                historialProgresivo.UsuarioID = Convert.ToInt32(Session["UsuarioID"]);
                //historialProgresivo.Parametros = new JavaScriptSerializer().Serialize(obj);
                historialProgresivo.Parametros = objStr;
                response = progesivoBL.ProgresivoHistoricoInsertarJson(historialProgresivo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                error = "servidor no disponible";
            }
            return Json(new { data = response, error = error });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ProgresivoCorreoEnviarJson(ProgresivoHistorico obj, String Sala, String Progresivo, List<DetalleProgresivo> listaPozos, List<string> lista)
        {
            var response = false;
            string error = "";
            try
            {
                //Envio Correo
                String correoservidor = ConfigurationManager.AppSettings["correo"];
                String password = ConfigurationManager.AppSettings["password"];
                var listadestinos = new List<DestinatarioEntidad>();
                listadestinos = destinatarioBL.DestinatarioListadoTipoEmailJson(1);
                string correosdestino = string.Join(",", listadestinos.Select(x => x.Email));
                //string correosdestino = "vh.vega@software3000.net";
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(correoservidor);
                mail.To.Add(correosdestino);
                mail.Subject = "Cambio en Configuracion de Progresivo.";

                var strSimbolo = (obj.ProgresivoActivo.Simbolo != lista[7].ToString().Trim()) ? " style='color:red' " : " ";
                var strPagoCaja = (obj.ProgresivoActivo.PagoCaja != ((lista[5].ToString() == "1") ? true : false)) ? " style='color:red' " : " ";
                var strDuracionPantalla = (obj.ProgresivoActivo.DuracionPantalla != Convert.ToInt64(lista[6])) ? " style='color:red' " : " ";
                var strNroJugadores = (obj.ProgresivoActivo.NroJugadores != Convert.ToInt64(lista[3])) ? " style='color:red' " : " ";
                var strNroPozos = (obj.ProgresivoActivo.NroPozos != Convert.ToInt64(lista[1])) ? " style='color:red' " : " ";
                var strProgresivoImagenID = (obj.ProgresivoActivo.ProgresivoImagenID != Convert.ToInt64(lista[4])) ? " style='color:red' " : " ";
                var strEstado = (obj.ProgresivoActivo.Estado != Convert.ToInt64(lista[8])) ? " style='color:red' " : " ";
                var strBaseOculto = (obj.ProgresivoActivo.BaseOculto != ((lista[9].ToString() == "1") ? true : false)) ? " style='color:red' " : " ";
                var strRegHistorico = (obj.ProgresivoActivo.RegHistorico != ((lista[10].ToString() == "1") ? true : false)) ? " style='color:red' " : " ";

                var UsuarioNombre = System.Web.HttpContext.Current.Session["UsuarioNombre"].ToString();
                SEG_EmpleadoEntidad EmpleadoEntidad = (SEG_EmpleadoEntidad)Session["empleado"];
                var EmpleadoNombre = EmpleadoEntidad.Nombres +" "+ EmpleadoEntidad.ApellidosPaterno+" "+ EmpleadoEntidad.ApellidosMaterno;

                var mensaje = @"
                            Usuario: "+UsuarioNombre+@"<br/>
                            Empleado: "+ EmpleadoNombre + @"<br/>
                            Datos de Progresivo.                                
                            <table style='border: 1px solid black;'>
                                <tr><th>Campo</th><th>Valor Anterior</th><th>Valor Actual</th></tr>
                                <tr><td>Sala</td><td>" + Sala + "</td><td>" + Sala + @"</td></tr>
                                <tr><td>Progresivo</td><td>" + Progresivo + "</td><td>" + Progresivo + @"</td></tr>
                                <tr><td>Moneda</td><td>" + obj.ProgresivoActivo.Simbolo + "</td><td " + strSimbolo + ">" + lista[7].ToString().Trim() + @"</td></tr>
                                <tr><td>Lugar Pago</td><td>" + ((obj.ProgresivoActivo.PagoCaja==true)?"Activo":"Inactivo") + "</td><td" + strPagoCaja + ">" + ((lista[5].ToString() == "1") ? "Activo" : "Inactivo") + @"</td></tr>
                                <tr><td>Duracion en Pantalla</td><td> " + obj.ProgresivoActivo.DuracionPantalla + " </td><td" + strDuracionPantalla + ">" + lista[6] + @"</td></tr>
                                <tr><td>Nro. Jugadores</td><td> " + obj.ProgresivoActivo.NroJugadores + " </td><td" + strNroJugadores + ">" + lista[3] + @"</td></tr>
                                <tr><td>Nro. Pozos</td><td> " + obj.ProgresivoActivo.NroPozos + " </td><td" + strNroPozos + ">" + lista[1] + @"</td></tr>
                                <tr><td>Imagen</td><td> " + obj.ProgresivoActivo.ProgresivoImagenID + " </td><td" + strProgresivoImagenID + ">" + lista[4] + @"</td></tr>
                                <tr><td>Estado</td><td> " + ((obj.ProgresivoActivo.Estado == 1) ? "Activo" : "Inactivo") + " </td><td" + strEstado + ">" + ((lista[8].ToString() == "1") ? "Activo" : "Inactivo") + @"</td></tr>
                                <tr><td>Pozo Oculto</td><td> " + ((obj.ProgresivoActivo.BaseOculto==true)?"Activo":"Inactivo") + " </td><td" + strBaseOculto + ">" + ((lista[9].ToString() == "1") ? "Activo" : "Inactivo") + @"</td></tr>
                                <tr><td>Registrar Historico</td><td> " + ((obj.ProgresivoActivo.RegHistorico==true)?"Activo":"Inactivo")+ " </td><td" + strRegHistorico + ">" + ((lista[10].ToString() == "1") ? "Activo" : "Inactivo") + @"</td></tr>
                            </table>";
                if (obj.DetalleProgresivo != null && obj.DetalleProgresivo.Count() >= listaPozos.Count())
                {
                    foreach (var pozo in obj.DetalleProgresivo)
                    {
                        var tpoPozo = "";
                        if (pozo.TipoPozo == 1) { tpoPozo = "POZO SUPERIOR"; }
                        if (pozo.TipoPozo == 2) { tpoPozo = "POZO MEDIO"; }
                        if (pozo.TipoPozo == 3) { tpoPozo = "POZO INFERIOR"; }
                        var mbActual = "";
                        var mmiActual = "";
                        var mmaActual = "";
                        var incpActual = "";
                        var rsaActual = "";
                        var rsjActual = "";
                        var momiActual = "";
                        var momaActual = "";
                        var incpoActual = "";
                        var difActual = "";
                        var poaActual = "";
                        var pozoActual = listaPozos.FirstOrDefault(o => o.TipoPozo == pozo.TipoPozo);
                        if (pozoActual == null)
                        {
                            mbActual = "0";
                            mmiActual = "0";
                            mmaActual = "0";
                            incpActual = "0";
                            rsaActual = "0";
                            rsjActual = "0";
                            momiActual = "0";
                            momaActual = "0";
                            incpoActual = "0";
                            difActual = "0";
                            poaActual = "0";
                        }
                        else
                        {
                            mbActual = pozoActual.MontoBase.ToString();
                            mmiActual = pozoActual.MontoMin.ToString();
                            mmaActual = pozoActual.MontoMax.ToString();
                            incpActual = pozoActual.IncPozo1.ToString();
                            rsaActual = pozoActual.RsApuesta.ToString();
                            rsjActual = pozoActual.RsJugadores.ToString();
                            momiActual = pozoActual.MontoOcMin.ToString();
                            momaActual = pozoActual.MontoOcMax.ToString();
                            incpoActual = pozoActual.IncOcPozo1.ToString();
                            difActual = pozoActual.Dificultad.ToString();
                            poaActual = pozoActual.Actual.ToString();
                        }
                        var strmbActual = (Convert.ToDouble(mbActual) != pozo.MontoBase) ? " style='color:red' " : " ";
                        var strmmiActual = (Convert.ToDouble(mmiActual) != pozo.MontoMin) ? " style='color:red' " : " ";
                        var strmmaActual = (Convert.ToDouble(mmaActual) != pozo.MontoMax) ? " style='color:red' " : " ";
                        var strincpActual = (Convert.ToDouble(incpActual) != pozo.IncPozo1) ? " style='color:red' " : " ";
                        var strrsaActual = (Convert.ToDouble(rsaActual) != pozo.RsApuesta) ? " style='color:red' " : " ";
                        var strrsjActual = (Convert.ToDouble(rsjActual) != pozo.RsJugadores) ? " style='color:red' " : " ";
                        var strmomiActual = (Convert.ToDouble(momiActual) != pozo.MontoOcMin) ? " style='color:red' " : " ";
                        var strmomaActual = (Convert.ToDouble(momaActual) != pozo.MontoOcMax) ? " style='color:red' " : " ";
                        var strincpoActual = (Convert.ToDouble(incpoActual) != pozo.IncOcPozo1) ? " style='color:red' " : " ";
                        var strdifActual = (Convert.ToDouble(difActual) != pozo.Dificultad) ? " style='color:red' " : " ";
                        var strpoaActual = (Convert.ToDouble(poaActual) != pozo.Actual) ? " style='color:red' " : " ";

                        var StrDificultadP = "";
                        switch (pozo.Dificultad)
                        {
                            case 5:
                                StrDificultadP = "Facil";
                                break;
                            case 6:
                                StrDificultadP = "Normal";
                                break;
                            case 7:
                                StrDificultadP = "Dificil";
                                break;
                            default:
                                StrDificultadP = "";
                                break;
                        }
                        switch (difActual)
                        {
                            case "5":
                                difActual = "Facil";
                                break;
                            case "6":
                                difActual = "Normal";
                                break;
                            case "7":
                                difActual = "Dificil";
                                break;
                            default:
                                difActual = "";
                                break;
                        }
                        mensaje = mensaje + @"<h2>" + tpoPozo + @"<h2>
                                    <table style='border: 1px solid black;'>
                                        <tr><th>Campo</th><th>Valor Anterior</th><th>Valor Actual</th></tr>
                                        <tr><td>Premio Base</td><td>" + pozo.MontoBase + @"</td><td " + strmbActual + ">" + mbActual + @"</td></tr>
                                        <tr><td>Premio Minimo</td><td>" + pozo.MontoMin + @"</td><td" + strmmiActual + ">" + mmiActual + @"</td></tr>
                                        <tr><td>Premio Maximo</td><td>" + pozo.MontoMax + @"</td><td" + strmmaActual + ">" + mmaActual + @"</td></tr>
                                        <tr><td>Inc. Pozo</td><td>" + pozo.IncPozo1 + @"</td><td" + strincpActual + ">" + incpActual + @"</td></tr>
                                        <tr><td>Restriccion de Apuesta</td><td>" + pozo.RsApuesta + @"</td><td" + strrsaActual + ">" + rsaActual + @"</td></tr>
                                        <tr><td>Nro. Jugadores</td><td>" + pozo.RsJugadores + @"</td><td" + strrsjActual + ">" + rsjActual + @"</td></tr>
                                        <tr><td>Monto Oculto Minimo</td><td>" + pozo.MontoOcMin + @"</td><td" + strmomiActual + ">" + momiActual + @"</td></tr>
                                        <tr><td>Monto Oculto Maximo</td><td>" + pozo.MontoOcMax + @"</td><td" + strmomaActual + ">" + momaActual + @"</td></tr>
                                        <tr><td>Inc. Pozo Oculto</td><td>" + pozo.IncOcPozo1 + @"</td><td" + strincpoActual + ">" + incpoActual + @"</td></tr>
                                        <tr><td>Dificultad</td><td>" + StrDificultadP + @"</td><td" + strdifActual + ">" + difActual + @"</td></tr>
                                        <tr><td>Pozo Actual</td><td>" + pozo.Actual + @"</td><td" + strpoaActual + ">" + poaActual + @"</td></tr>
                                    </table>";
                    }
                }
                else
                {
                    foreach (var pozo in listaPozos)
                    {
                        var tpoPozo = "";
                        if (pozo.TipoPozo == 1) { tpoPozo = "POZO SUPERIOR"; }
                        if (pozo.TipoPozo == 2) { tpoPozo = "POZO MEDIO"; }
                        if (pozo.TipoPozo == 3) { tpoPozo = "POZO INFERIOR"; }
                        var mbAntiguo = "";
                        var mmiAntiguo = "";
                        var mmaAntiguo = "";
                        var incpAntiguo = "";
                        var rsaAntiguo = "";
                        var rsjAntiguo = "";
                        var momiAntiguo = "";
                        var momaAntiguo = "";
                        var incpoAntiguo = "";
                        var difAntiguo = "";
                        var poaAntiguo = "";

                        var pozoActual = new DetalleProgresivo();
                        if (obj.DetalleProgresivo != null)
                        {
                            pozoActual = obj.DetalleProgresivo.FirstOrDefault(o => o.TipoPozo == pozo.TipoPozo);
                        }
                        else
                        {
                            pozoActual = null;
                        }
                        if (pozoActual == null)
                        {
                            mbAntiguo = "0";
                            mmiAntiguo = "0";
                            mmaAntiguo = "0";
                            incpAntiguo = "0";
                            rsaAntiguo = "0";
                            rsjAntiguo = "0";
                            momiAntiguo = "0";
                            momaAntiguo = "0";
                            incpoAntiguo = "0";
                            difAntiguo = "0";
                            poaAntiguo = "0";
                        }
                        else
                        {
                            mbAntiguo = pozoActual.MontoBase.ToString();
                            mmiAntiguo = pozoActual.MontoMin.ToString();
                            mmaAntiguo = pozoActual.MontoMax.ToString();
                            incpAntiguo = pozoActual.IncPozo1.ToString();
                            rsaAntiguo = pozoActual.RsApuesta.ToString();
                            rsjAntiguo = pozoActual.RsJugadores.ToString();
                            momiAntiguo = pozoActual.MontoOcMin.ToString();
                            momaAntiguo = pozoActual.MontoOcMax.ToString();
                            incpoAntiguo = pozoActual.IncOcPozo1.ToString();
                            difAntiguo = pozoActual.Dificultad.ToString();
                            poaAntiguo = pozoActual.Actual.ToString();
                        }
                        var strmbAntiguo = (Convert.ToDouble(mbAntiguo) != pozo.MontoBase) ? " style='color:red' " : " ";
                        var strmmiAntiguo = (Convert.ToDouble(mmiAntiguo) != pozo.MontoMin) ? " style='color:red' " : " ";
                        var strmmaAntiguo = (Convert.ToDouble(mmaAntiguo) != pozo.MontoMax) ? " style='color:red' " : " ";
                        var strincpAntiguo = (Convert.ToDouble(incpAntiguo) != pozo.IncPozo1) ? " style='color:red' " : " ";
                        var strrsaAntiguo = (Convert.ToDouble(rsaAntiguo) != pozo.RsApuesta) ? " style='color:red' " : " ";
                        var strrsjAntiguo = (Convert.ToDouble(rsjAntiguo) != pozo.RsJugadores) ? " style='color:red' " : " ";
                        var strmomiAntiguo = (Convert.ToDouble(momiAntiguo) != pozo.MontoOcMin) ? " style='color:red' " : " ";
                        var strmomaAntiguo = (Convert.ToDouble(momaAntiguo) != pozo.MontoOcMax) ? " style='color:red' " : " ";
                        var strincpoAntiguo = (Convert.ToDouble(incpoAntiguo) != pozo.IncOcPozo1) ? " style='color:red' " : " ";
                        var strdifAntiguo = (Convert.ToDouble(difAntiguo) != pozo.Dificultad) ? " style='color:red' " : " ";
                        var strpoaAntiguo = (Convert.ToDouble(poaAntiguo) != pozo.Actual) ? " style='color:red' " : " ";

                        var StrDificultadP1 = "";
                        switch (pozo.Dificultad)
                        {
                            case 5:
                                StrDificultadP1 = "Facil";
                                break;
                            case 6:
                                StrDificultadP1 = "Normal";
                                break;
                            case 7:
                                StrDificultadP1 = "Dificil";
                                break;
                            default:
                                StrDificultadP1 = "";
                                break;
                        }
                        switch (difAntiguo)
                        {
                            case "5":
                                difAntiguo = "Facil";
                                break;
                            case "6":
                                difAntiguo = "Normal";
                                break;
                            case "7":
                                difAntiguo = "Dificil";
                                break;
                            default:
                                difAntiguo = "";
                                break;
                        }

                        mensaje = mensaje + @"<h2>" + tpoPozo + @"<h2>
                                    <table style='border: 1px solid black;'>
                                        <tr><th>Campo</th><th>Valor Anterior</th><th>Valor Actual</th></tr>
                                        <tr><td>Premio Base</td><td>" + mbAntiguo + @"</td><td" + strmbAntiguo + ">" + pozo.MontoBase + @"</td></tr>
                                        <tr><td>Premio Minimo</td><td>" + mmiAntiguo + @"</td><td" + strmmiAntiguo + ">" + pozo.MontoMin + @"</td></tr>
                                        <tr><td>Premio Maximo</td><td>" + mmaAntiguo + @"</td><td" + strmmaAntiguo + ">" + pozo.MontoMax + @"</td></tr>
                                        <tr><td>Inc. Pozo</td><td>" + incpAntiguo + @"</td><td" + strincpAntiguo + ">" + pozo.IncPozo1 + @"</td></tr>
                                        <tr><td>Restriccion de Apuesta</td><td>" + rsaAntiguo + @"</td><td" + strrsaAntiguo + ">" + pozo.RsApuesta + @"</td></tr>
                                        <tr><td>Nro. Jugadores</td><td>" + rsjAntiguo + @"</td><td" + strrsjAntiguo + ">" + pozo.RsJugadores + @"</td></tr>
                                        <tr><td>Monto Oculto Minimo</td><td>" + momiAntiguo + @"</td><td" + strmomiAntiguo + ">" + pozo.MontoOcMin + @"</td></tr>
                                        <tr><td>Monto Oculto Maximo</td><td>" + momaAntiguo + @"</td><td" + strmomaAntiguo + ">" + pozo.MontoOcMax + @"</td></tr>
                                        <tr><td>Inc. Pozo Oculto</td><td>" + incpoAntiguo + @"</td><td" + strincpoAntiguo + ">" + pozo.IncOcPozo1 + @"</td></tr>
                                        <tr><td>Dificultad</td><td>" + difAntiguo + @"</td><td" + strdifAntiguo + ">" + StrDificultadP1 + @"</td></tr>
                                        <tr><td>Pozo Actual</td><td>" + poaAntiguo + @"</td><td" + strpoaAntiguo + ">" + pozo.Actual + @"</td></tr>
                                    </table>";
                    }
                }

                mail.Body = mensaje;
                mail.IsBodyHtml = true;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(correoservidor, password);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                response = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                error = "servidor no disponible";
            }
            return Json(new { data = response, error = error });
        }

        [seguridad(false)]
        [HttpPost]
        public async Task<ActionResult> ProgresivoWhatsAppEnviarJson() {
            var response = new ResponseEntidad<WSP_UltraMsgResponse>();
            string error = "";
            try {
                //Enviar WhatsApp

                //var UsuarioNombre = System.Web.HttpContext.Current.Session["UsuarioNombre"].ToString();
                //SEG_EmpleadoEntidad EmpleadoEntidad = (SEG_EmpleadoEntidad)Session["empleado"];
                //var EmpleadoNombre = EmpleadoEntidad.Nombres + " " + EmpleadoEntidad.ApellidosPaterno + " " + EmpleadoEntidad.ApellidosMaterno;

                //var mensaje = @"
                //            Usuario: " + UsuarioNombre + @".
                //            Empleado: " + EmpleadoNombre + @".
                //            Se realizaron cambios en la configuración.";
                var message = @"Se realizaron cambios en la configuración.";
                wspMensajeriaUltraMsgBL = new WSP_MensajeriaUltraMsgBL(61);
                response = await wspMensajeriaUltraMsgBL.SendMessage("51916182084", message);


            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                error = "servidor no disponible";
            }
            return Json(new { data = response, error = error });
        }
        #endregion
        #region Aumento de Creditos
        [seguridad(false)]
        [TokenProgresivo(false)]
        [HttpPost]
        public ActionResult ObtenerUltimoAumento(string CodMaq, int CodSala)
        {
            var result= _aumentoCreditoMaquinaBL.ObtenerUltimoRegistro(CodMaq,CodSala);
            return Json(result);
        }
        [seguridad(false)]
        [TokenProgresivo(false)]
        [HttpPost]
        public ActionResult InsertarAumentoCreditoMaquina(AumentoCreditoMaquinaEntidad item)
        {
            item.UsuarioRegistro = Convert.ToInt32(Session["UsuarioID"]);
            item.FechaRegistro = DateTime.Now;
            item.Hora = item.FechaRegistro.Hour;
            item.Minuto = item.FechaRegistro.Minute;
            item.FechaUltimoAumento = DateTime.Now;
            var result = _aumentoCreditoMaquinaBL.InsertarAumentoCreditoMaquina(item);
            var ultimoAumento = _aumentoCreditoMaquinaBL.ObtenerUltimoRegistro(item.CodMaq, item.CodSala);
            return Json(new { respuesta=result,data=ultimoAumento });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ActualizarCantidadEnAumentoCreditoMaquina(AumentoCreditoMaquinaEntidad item)
        {
            item.FechaUltimoAumento = DateTime.Now;
            var result = _aumentoCreditoMaquinaBL.ActualizarCantidadEnAumentoCreditoMaquina(item);
            var ultimoAumento = _aumentoCreditoMaquinaBL.ObtenerUltimoRegistro(item.CodMaq, item.CodSala);
            return Json(new { respuesta=result,data=ultimoAumento });
        }
        [HttpPost]
        [TokenProgresivo(false)]
        [seguridad(false)]
        public ActionResult AumentarCreditoMaquinaEnServicio(Int32 IdProgresivo, String CodMaquina,  String UrlProgresivoAumento, string IpSignalr)
        {
            bool respuesta = false;
            object oEnvio = new object();
            try
            {
                respuesta = true;
                oEnvio = new
                {
                    CodMaquina = CodMaquina,
                    IdProgresivo = IdProgresivo,
                    IpProgresivo = IpSignalr
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                var client = new MyWebClient();
                var response = "";
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                response = client.UploadString(UrlProgresivoAumento, "POST", inputJson);
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                respuesta = Convert.ToBoolean(jsonObj);
                respuesta = true;
            }
            catch (Exception ex)
            {
                respuesta=false;
            }
            return Json(new {respuesta=respuesta });
        }
        [HttpPost]
        [TokenProgresivo(false)]
        [seguridad(false)]
        public ActionResult AumentarCreditoMaquinaEnServicioVpn(Int32 IdProgresivo, String CodMaquina, String UrlProgresivoAumento, string IpSignalr,string urlPublica,string urlPrivada) {
            bool respuesta = false;
            object oEnvio = new object();
            try {
                respuesta = true;
                oEnvio = new {
                    CodMaquina = CodMaquina,
                    IdProgresivo = IdProgresivo,
                    IpProgresivo = IpSignalr,
                    ipPrivada = urlPrivada
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                var client = new MyWebClient();
                var response = "";
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                response = client.UploadString(urlPublica, "POST", inputJson);
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                respuesta = Convert.ToBoolean(jsonObj);
                respuesta = true;
            } catch(Exception) {
                respuesta = false;
            }
            return Json(new { respuesta = respuesta });
        }
        #endregion
        #region Nuevos Metodos Vista Ganadores Progresivo
        [seguridad(false)]
        [HttpPost]
        public ActionResult ListadoProgresivosVpn(string urlPublica,string urlPrivada) {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new List<WEB_Progresivo>();
            object oEnvio = new object();
            try {
                oEnvio = new { 
                    ipPrivada=urlPrivada
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(urlPublica, "POST",inputJson);
                jsonResponse = JsonConvert.DeserializeObject<List<WEB_Progresivo>>(response);
            } catch(Exception ex) {
            }
            return Json(jsonResponse);
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ProgresivoGanadoresListadoJsonVpn(string urlPublica, string urlPrivada) {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new List<GanadorResponse>();
            try {
                object oEnvio = new {
                    ipPrivada=urlPrivada
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(urlPublica, "POST",inputJson);
                jsonResponse = JsonConvert.DeserializeObject<List<GanadorResponse>>(response);
            } catch(Exception ex) {
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse.ToList() });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ProgresivoGanadoresDetalleContadoresListadoJsonVpn(string urlPublica,string urlPrivada) {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new List<DetalleContadores>();
            try {
                object oEnvio = new {
                    ipPrivada=urlPrivada
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(urlPublica, "POST",inputJson);
                //json = webClient.UploadString(url + "/api/RegistroProgresivo/GuardarDetalles", "PUT", lista_stringjson/* "{some:\"json data\"}"*/);
                jsonResponse = JsonConvert.DeserializeObject<List<DetalleContadores>>(response);
                if(jsonResponse.Count > 0) {
                    jsonResponse.ForEach(x => x.FechaCompleta = new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day, x.Hora.Hour, x.Hora.Minute, x.Hora.Second));
                    jsonResponse = jsonResponse.OrderBy(x => x.FechaCompleta).ToList();
                }
            } catch(Exception ex) {
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse.ToList() });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ProgresivoDetalleContadoresListadoJsonVpn(DateTime fechaInicio, DateTime fechaFin, string codMaq, string urlPublica, string urlPrivada) {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new List<DetalleContadores>();
            object oEnvio = new object();
            try {
                oEnvio = new {
                    codMaq = codMaq,
                    fechaIni = fechaInicio.ToString("dd-MM-yyyy HH:mm:ss"),
                    fechaFin = fechaFin.ToString("dd-MM-yyyy HH:mm:ss"),
                    ipPrivada=urlPrivada
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(urlPublica, "POST", inputJson);
                jsonResponse = JsonConvert.DeserializeObject<List<DetalleContadores>>(response);
              
            } catch(Exception ex) {
            }
            return Json(new { data = jsonResponse.ToList() });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ProgresivoDetalleContadoresListadoJsonV2(DateTime fechaInicio, DateTime fechaFin, string codMaq, string url) {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new List<DetalleContadores>();
            object oEnvio = new object();
            try {
                oEnvio = new {
                    codMaq = codMaq,
                    fechaIni = fechaInicio.ToString("dd-MM-yyyy HH:mm:ss"),
                    fechaFin = fechaFin.ToString("dd-MM-yyyy HH:mm:ss")
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST", inputJson);
                jsonResponse = JsonConvert.DeserializeObject<List<DetalleContadores>>(response);
            } catch(Exception ex) {
                List<DetalleContadores> lista = new List<DetalleContadores>();
                return Json(new { data = lista });
            }
            return Json(new { data = jsonResponse.ToList() });
        }
        #endregion
        #region Nuevos Metodos Vista RegistroProgresivo
        [seguridad(false)]
        [HttpPost]
        public ActionResult ProgresivoListarImagenesJsonVpn(string urlPublica,string urlPrivada) {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new List<ListadoImagen>();
            try {
                object oEnvio =new {
                    urlPrivada=urlPrivada
                };
                string inputJson= (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(urlPublica,"POST",inputJson);
                jsonResponse = JsonConvert.DeserializeObject<List<ListadoImagen>>(response);
            } catch(Exception ex) {
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse.ToList() });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ProgresivoActivoObtenerJsonVpn(string urlPublica,string urlPrivada) {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new ProgresivoActivo();
            try {
                object oEnvio = new {
                    urlPrivada = urlPrivada
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(urlPublica, "POST",inputJson);

                jsonResponse = JsonConvert.DeserializeObject<ProgresivoActivo>(response);
            } catch(Exception ex) {
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ProgresivoPozosActualesObtenerJsonVpn(string urlPublica,string urlPrivada) {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new List<PozosActuales>();
            try {
                object oEnvio = new {
                    urlPrivada = urlPrivada
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(urlPublica, "POST",inputJson);
                jsonResponse = JsonConvert.DeserializeObject<List<PozosActuales>>(response);
            } catch(Exception ex) {
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse.ToList() });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ProgresivoDetalleListarJsonVpn(string urlPublica,string urlPrivada) {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new List<ProgresivoDetalle>();
            try {
                object oEnvio = new {
                    urlPrivada = urlPrivada
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(urlPublica, "POST",inputJson);
                jsonResponse = JsonConvert.DeserializeObject<List<ProgresivoDetalle>>(response);
            } catch(Exception ex) {
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse.ToList() });
        }
        [HttpPost]
        [seguridad(false)]//quitar esto cuando se actualize
        public ActionResult ProgresivoGuardarCabeceraJsonVpn(List<string> Lista, string urlPublica,string urlPrivada) {
            var client = new System.Net.WebClient();
            var response = "";
            try {
                object oEnvio = new {
                    urlPrivada = urlPrivada,
                    ids=Lista
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                //string parametros = JsonConvert.SerializeObject(Lista);
                string responseClient = client.UploadString(urlPublica, "POST", inputJson);

                response = JsonConvert.DeserializeObject<dynamic>(responseClient);

            } catch(Exception ex) {
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = response });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ProgresivoDetallesGuardarJsonVpn(List<DetalleProgresivo> listaPozos, string urlPublica, string urlPrivada) {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new List<string>();
            try {
                object oEnvio = new { 
                    listaPozos=listaPozos,
                    urlPrivada=urlPrivada
                };
                client.Headers.Add("content-type", "application/json");
                string parametros = JsonConvert.SerializeObject(oEnvio);
                response = client.UploadString(urlPublica, "POST", parametros);

                response = response.Replace(@"""[", "[").Replace(@"]""", "]").Replace(@"\", @"").Replace(@"\\", @"").Trim();

                jsonResponse = JsonConvert.DeserializeObject<List<string>>(response);
            } catch(Exception ex) {
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ProgresivoPozoInsertarJsonVpn(List<string> lista, string urlPublica, string urlPrivada) {
            var client = new System.Net.WebClient();
            var response = "";
            try {
                object oEnvio = new {
                    urlPrivada= urlPrivada,
                    id=lista
                };
                client.Headers.Add("content-type", "application/json");
                string parametros = JsonConvert.SerializeObject(oEnvio);
                response = client.UploadString(urlPublica, "POST", parametros);
            } catch(Exception ex) {
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = response });
        }
        #endregion
        #region Nuevos Metodos Listado Progresivos
        [seguridad(false)]
        [HttpPost]
        public ActionResult ConsultarListaMaquinaProgresivoVpn(string urlPublica, string urlPrivada) {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new List<MaquinaProgresivoEntidad>();
            object oEnvio = new object();
            try {
                oEnvio = new {
                    ipPrivada = urlPrivada
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(urlPublica, "POST", inputJson);
                jsonResponse = JsonConvert.DeserializeObject<List<MaquinaProgresivoEntidad>>(response);
            } catch(Exception ex) {
                return Json(new { mensaje = ex.Message.ToString() });
            }
            var jsonResult = Json(new { data = jsonResponse.ToList() }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ConsultarListaMarcaMaquinaJsonVpn(string urlPublica, string urlPrivada) {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new List<Marca>();
            try {
                object oEnvio = new { 
                    ipPrivada=urlPrivada
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(urlPublica,"POST",inputJson);
                jsonResponse = JsonConvert.DeserializeObject<List<Marca>>(response);
            } catch(Exception ex) {
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse.ToList() });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ConsultarListaJuegoMaquinaJsonVpn(string urlPublica, string urlPrivada) {

            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new List<MaquinaJuego>();
            try {
                object oEnvio = new
                {
                    ipPrivada = urlPrivada
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(urlPublica, "POST", inputJson);
                jsonResponse = JsonConvert.DeserializeObject<List<MaquinaJuego>>(response);
            } catch(Exception ex) {
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse.ToList() });

        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ConsultarListaModeloMaquinaJsonVpn(string urlPublica, string urlPrivada) {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new List<ModeloMaquinaEntidad>();
            try {
                object oEnvio = new {
                    ipPrivada=urlPrivada,
                };
                string inputJson=(new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(urlPublica,"POST",inputJson);
                jsonResponse = JsonConvert.DeserializeObject<List<ModeloMaquinaEntidad>>(response);
            } catch(Exception ex) {
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse.ToList() });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ConsultarObtenerMaquinaJsonVpn(string urlPublica, string urlPrivada) {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new MaquinaProgresivoEntidad();
            try {
                object oEnvio = new {
                    ipPrivada=urlPrivada
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(urlPublica,"POST",inputJson);
                jsonResponse = JsonConvert.DeserializeObject<MaquinaProgresivoEntidad>(response);

            } catch(Exception ex) {
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ConsultarGuardarMaquinaProgresivoVpn(Maquina parametros, string urlPublica, string urlPrivada) {
            var client = new System.Net.WebClient();
            var response = "";
            dynamic jsonResponse = "";
            try {
                object oEnvio = new {
                    ipPrivada = urlPrivada,
                    maquina=parametros
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(urlPublica, "POST", inputJson);
                jsonResponse = JsonConvert.DeserializeObject<dynamic>(response);

            } catch(Exception ex) {
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = response });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ConsultarModificarMaquinaProgresivoVpn(Maquina parametros, string urlPublica, string urlPrivada) {
            var client = new System.Net.WebClient();
            var response = "";
            dynamic jsonResponse = "";
            try {
                object oEnvio = new {
                    ipPrivada = urlPrivada,
                    maquina = parametros
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(urlPublica, "POST", inputJson);
                jsonResponse = JsonConvert.DeserializeObject<dynamic>(response);

            } catch(Exception ex) {
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = response });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ConsultarEliminarMaquinaProgresivoVpn(Maquina parametros, string urlPublica, string urlPrivada) {
            var client = new System.Net.WebClient();
            var response = "";
            dynamic jsonResponse = "";
            try {
                object oEnvio = new {
                    ipPrivada = urlPrivada,
                    maquina = parametros
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(urlPublica, "POST", inputJson);
                jsonResponse = JsonConvert.DeserializeObject<dynamic>(response);

            } catch(Exception ex) {
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = response });
        }
        #endregion
        
        [HttpPost]
        [seguridad(false)]
        [TokenProgresivo(false)]
        public ActionResult EchoPingSalas(string[] ips) {
            List<object> result= new List<object>();
            try {
                foreach(var item in ips) {
                    Uri uri = new Uri(item);
                    bool response = EchoPingWithPort(uri.Host,uri.Port);
                    result.Add(new { 
                        uri=item,
                        respuesta=response
                    });
                }
            } catch(Exception) {
                result = new List<object>();
            }                 
            return Json(result);
        }

        [seguridad(false)]
        [TokenProgresivo(false)]
        private bool EchoPingWithPort(string ip, int port) {
            bool ok = true;
            TimeSpan timeout = new TimeSpan(0, 0, 1);
            try {
                using(TcpClient tcp = new TcpClient()) {
                    IAsyncResult result = tcp.BeginConnect(ip, port, null, null);
                    WaitHandle wait = result.AsyncWaitHandle;
                    try {
                        if(!result.AsyncWaitHandle.WaitOne(timeout, false)) {
                            tcp.Close();
                            ok = false;
                        }
                        tcp.EndConnect(result);
                    } catch(Exception) {
                        ok = false;
                    } finally {
                        wait.Close();
                    }
                }
            } catch(Exception) {
                ok = false;
            }
            return ok;
        }

        [HttpPost]
        [seguridad(false)]
        [TokenProgresivo(false)]
        public ActionResult EchoPingSalasUsuario() {
            List<SalaEntidad> listaSalas = new List<SalaEntidad>();
            List<object> result = new List<object>();
            try {
                var usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                listaSalas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                listaSalas = listaSalas.Where(x => !string.IsNullOrEmpty(x.UrlProgresivo)).ToList();
                foreach(var item in listaSalas) {
                    Uri uri = new Uri(item.UrlProgresivo);
                    bool response = EchoPingWithPort(uri.Host, uri.Port);
                    result.Add(new {
                        uri=item.UrlProgresivo,
                        respuesta=response
                    });
                }
            } catch(Exception) {
                result = new List<object>();
            }
            return Json(result);
        }

        #region Progresivos Offline

        [seguridad(false)]
        [TokenProgresivo(false)]
        [HttpPost]
        public ActionResult EnviarUltimaFechaProgresivosOffline(int codSala, int codProgresivo)
        {
            var client = new System.Net.WebClient();
            var response = false;
            var jsonResponse = new CabeceraOfflineEntidad();
            var mensaje = "";
            try
            {

                response = true;
                mensaje = "Hora obtenida";

                jsonResponse = progesivoBL.GetUltimaFechaProgresivoxSala(codSala,codProgresivo);

                if (jsonResponse.Fecha.ToString()== "1/01/0001 00:00:00")
                {
                    return Json(new { respuesta = response, data = "1/01/2020 00:00:00" });
                }

            }
            catch (Exception ex)
            {
                response = false;
                return Json(new { respuesta = response, mensaje = ex.Message.ToString() });
            }
            return Json(new { respuesta = response, data = jsonResponse.Fecha.ToString()});
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ProgresivoOfflineListadoJson(int codSala)
        {
            var client = new System.Net.WebClient();
            var response = false;
            var jsonResponse = new List<ProgresivoOfflineEntidad>();
            try
            {
                jsonResponse = progesivoBL.GetProgresivoOfflinexSalaxProgresivo(codSala);
                response = true;
            }
            catch (Exception ex)
            {
                response = false;
                return Json(new { respuesta = response, mensaje = ex.Message.ToString() });
            }
            return Json(new { respuesta = response, data = jsonResponse.ToList() });
        }

        [HttpPost]
        public ActionResult DetallesContadoresPremioOfflineListadoJson(string id, string id2, int id3, int id4)
        {
            var client = new System.Net.WebClient();
            var response = false;
            var result = new List<DetalleOfflineEntidad>();
            try
            {
                result = progesivoBL.DetallesContadoresPremio(id,id2,id3,id4);

                int fila_contador = 0; int result_count = result.Count();
                foreach (var fila in result)
                {
                    var bonus1_fila = fila.Bonus1;
                    if (fila_contador + 1 == result_count)
                    {
                        fila.Dif_Bonus1 = fila.Bonus1;
                        fila.Dif_Bonus2 = fila.Bonus2;
                    }
                    else
                    {   
                        fila.Dif_Bonus1 = fila.Bonus1 - result[fila_contador + 1].Bonus1;
                        fila.Dif_Bonus2 = fila.Bonus2 - result[fila_contador + 1].Bonus2;
                    }
                    fila_contador++;

                }

                if (result.Count > 0)
                {
                    response = true;
                }

                //DateTime now = DateTime.Now.AddDays(-1);

                //result = result.Where(x => x.Hora.Day>= now.Day).ToList();
                result = result.OrderBy(x => x.Hora).ToList() ;

            }
            catch (Exception ex)
            {
                response = false;
                return Json(new { respuesta = response, mensaje = ex.Message.ToString() });
            }
            return Json(new { respuesta = response, data = result.ToList() });
        }


        [seguridad(false)]
        [HttpPost]
        public ActionResult ExcelProgresivo(int codSala, int codProgresivo, DateTime fechaini, DateTime fechafin) {
            bool success = false;
            var jsonResponse = new List<CabeceraOfflineEntidad>();


            SalaVpnEntidad sala = _salaBl.ObtenerSalaVpnPorCodigo(codSala);


            DateTime currentDate = DateTime.Now;
            string fileExtension = "xlsx";
            string fileName = $"ReporteProgresico{sala.Nombre}_{currentDate.ToString("dd-MM-yyyy")}_{currentDate.ToString("HHmmss")}.{fileExtension}";
            string data = string.Empty;


            TimeSpan diferencia = fechafin - fechaini;

            try {
                if(diferencia.Days > 30) {
                    return Json(new { respuesta = success, mensaje = "La diferencia de días debe ser menor a 30" });

                } else {
                    string nombreSala = progesivoBL.NombreProgresivo(codProgresivo, codSala);
                    jsonResponse = progesivoBL.GetCabeceraOfflinexSalaxProgresivo(codSala, codProgresivo, fechaini, fechafin);
                    MemoryStream excelStream = ProgresivoReporte.ExcelAperturaCajas(jsonResponse, fechaini, fechafin, sala.Nombre, nombreSala);
                    data = Convert.ToBase64String(excelStream.ToArray());
                    success = true;
                }

            } catch(Exception ex) {
                success = false;
                return Json(new { respuesta = success, mensaje = ex.Message.ToString() });
            }

            return Json(new { data = data, fileName, success });


        }


        [HttpPost]
        public ActionResult CabeceraOfflineListadoJson(int codSala, int codProgresivo, DateTime fechaini, DateTime fechafin)
        {
            var client = new System.Net.WebClient();
            var response = false;
            var jsonResponse = new List<CabeceraOfflineEntidad>();
            TimeSpan diferencia = fechafin - fechaini;
            try
            {
                if(diferencia.Days > 30) {
                    return Json(new { respuesta = response, mensaje = "La diferencia de días debe ser menor a 30" });

                } else {
                    jsonResponse = progesivoBL.GetCabeceraOfflinexSalaxProgresivo(codSala, codProgresivo, fechaini, fechafin);
                    response = true;
                }
                
            }
            catch (Exception ex)
            {
                response = false;
                return Json(new { respuesta = response, mensaje = ex.Message.ToString() });
            }

            var oRespuesta = new
            {
                mensaje="Cabeceras Obtenidas correctamente",
                respuesta= response,
                data = jsonResponse
            };
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var result = new ContentResult {
                Content = serializer.Serialize(oRespuesta),
                ContentType = "application/json"
            };
            return result;


            //return Json(new { respuesta = response, data = jsonResponse.ToList() });
        }

        [HttpPost]
        public ActionResult DetalleOfflineListadoJson(int codCabecera)
        {
            var client = new System.Net.WebClient();
            var response = false;
            var jsonResponse = new List<DetalleOfflineEntidad>();
            try
            {
                jsonResponse = progesivoBL.GetDetalleOfflinexCabecera(codCabecera);
                response = true;
            }
            catch (Exception ex)
            {
                response = false;
                return Json(new { respuesta = response, mensaje = ex.Message.ToString() });
            }
            return Json(new { respuesta = response, data = jsonResponse.ToList() });
        }

        [seguridad(false)]
        [TokenProgresivo(false)]
        [HttpPost]
        public ActionResult ProgresivoOfflineRecepcionOnlineJson(List<ProgresivoOfflineEntidad> listaReporteProgresivo)
        {
            var mensaje = "";
            var response = false;
            var jsonResponse = new List<ProgresivoOfflineEntidad>();
            try
            {

                List<ProgresivoOfflineEntidad> listaActual = progesivoBL.GetProgresivoOffline();

                foreach (var item in listaReporteProgresivo)
                {

                    var existe = listaActual.FirstOrDefault(x=>(x.WEB_PrgID == item.WEB_PrgID && x.CodSala==item.CodSala));

                    if (existe == null)
                    {

                        response = progesivoBL.ProgresivoOfflineInsertarJson(item);

                        if (!response)
                        {
                            mensaje = "Error al insertar nuevo progresivo";
                            response = false;
                            return Json(new { respuesta = response, mensaje = mensaje.ToString() });
                        }
                    } else
                    {

                        response = progesivoBL.ProgresivoOfflineActualizarJson(item);
                        if (!response)
                        {
                            mensaje = "Error al actualizar progresivo existente";
                            response = false;
                            return Json(new { respuesta = response, mensaje = mensaje.ToString() });
                        }
                    }


                }

                response = true;
                mensaje = "Generado progresivo";

            }
            catch (Exception ex)
            {
                response = false;
                return Json(new { respuesta = response, mensaje = ex.Message.ToString() });
            }
            return Json(new { respuesta = response, mensaje = mensaje.ToString() });
        }

        [seguridad(false)]
        [TokenProgresivo(false)]
        [HttpPost]
        public ActionResult CabeceraOfflineRecepcionOnlineJson(List<CabeceraOfflineEntidad> listaReporteProgresivo)
        {
            var mensaje = "";
            int idCabecera = 0;
            var response = false;
            var jsonResponse = new List<CabeceraOfflineEntidad>();
            try
            {

                List<CabeceraOfflineEntidad> listaActual = progesivoBL.GetCabeceraOffline();

                foreach (var item in listaReporteProgresivo)
                {

                    item.Fecha = Convert.ToDateTime(item.FechaStr);

                    var existe = listaActual.FirstOrDefault(x => (x.ProgresivoID == item.ProgresivoID && x.Fecha == item.Fecha && x.SlotID == item.SlotID));

                    if (existe == null)
                    {

                        idCabecera = progesivoBL.CabeceraOfflineInsertarJson(item);

                        if (idCabecera==0)
                        {
                            mensaje = "Error al insertar cabecera progresivo";
                            response = false;
                            return Json(new { respuesta = response, mensaje = mensaje.ToString() });
                        }
                    }


                }

                response = true;
                mensaje = "Generado cabeceras progresivo";

            }
            catch (Exception ex)
            {
                response = false;
                return Json(new { respuesta = response, mensaje = ex.Message.ToString() });
            }
            return Json(new { respuesta = response, mensaje = mensaje.ToString() });
        }


        [seguridad(false)]
        [TokenProgresivo(false)]
        [HttpPost]
        public ActionResult DetalleOfflineRecepcionOnlineJson(List<DetalleOfflineEntidad> listaReporteProgresivo)
        {
            var mensaje = "";
            var response = false;
            var jsonResponse = new List<DetalleOfflineEntidad>();
            try
            {

                List<DetalleOfflineEntidad> listaActual = progesivoBL.GetDetalleOffline();

                foreach (var item in listaReporteProgresivo)
                {
                    item.Fecha = Convert.ToDateTime(item.FechaStr);
                    item.Hora = Convert.ToDateTime(item.HoraStr);
                    item.FechaCompleta = item.Hora;
                    var existe = listaActual.FirstOrDefault(x => (Convert.ToInt32(x.CodMaq) == Convert.ToInt32(item.CodMaq) && x.Hora == item.Hora));

                    if (existe == null)
                    {

                        response = progesivoBL.DetalleOfflineInsertarJson(item);



                        if (!response)
                        {
                            mensaje = "Error al insertar detalle progresivo";
                            response = false;
                            return Json(new { respuesta = response, mensaje = mensaje.ToString() });
                        }
                    }


                }

                response = true;
                mensaje = "Generado detalles progresivo";

            }
            catch (Exception ex)
            {
                response = false;
                return Json(new { respuesta = response, mensaje = ex.Message.ToString() });
            }
            return Json(new { respuesta = response, mensaje = mensaje.ToString() });
        }


        [seguridad(false)]
        [TokenProgresivo(false)]
        [HttpPost]
        public ActionResult CabeceraDetalleOfflineRecepcionOnlineJson(List<CabeceraOfflineEntidad> listaReporteProgresivo)
        {
            var mensaje = "";
            int idCabecera = 0;
            bool response = false;
            var jsonResponse = new List<CabeceraOfflineEntidad>();
            try
            {

                List<CabeceraOfflineEntidad> listaActual = progesivoBL.GetCabeceraOffline();

                foreach (var item in listaReporteProgresivo)
                {

                    item.Fecha = Convert.ToDateTime(item.FechaStr);

                    var existe = listaActual.FirstOrDefault(x => (x.ProgresivoID == item.ProgresivoID && x.Fecha == item.Fecha && x.SlotID == item.SlotID));

                    if (existe == null)
                    {

                        idCabecera = progesivoBL.CabeceraOfflineInsertarJson(item);

                        if (idCabecera>0)
                        {

                            foreach(var detalle in item.listaDetalleOfflineEntidad)
                            {
                                detalle.IdCabeceraProgresivo = idCabecera;
                                detalle.Fecha = Convert.ToDateTime(detalle.FechaStr);
                                detalle.Hora = Convert.ToDateTime(detalle.HoraStr);
                                detalle.FechaCompleta = detalle.Hora;

                                response = progesivoBL.DetalleOfflineInsertarJson(detalle);

                                if (!response)
                                {
                                    mensaje = "Error al insertar detalle progresivo";
                                    response = false;
                                    return Json(new { respuesta = response, mensaje = mensaje.ToString() });
                                }

                            }

                        } else
                        {
                            mensaje = "Error al insertar cabecera progresivo";
                            response = false;
                            return Json(new { respuesta = response, mensaje = mensaje.ToString() });
                        }
                    }


                }

                response = true;
                mensaje = "Generado cabeceras y detalles progresivo";

            }
            catch (Exception ex)
            {
                response = false;
                return Json(new { respuesta = response, mensaje = ex.Message.ToString() });
            }
            return Json(new { respuesta = response, mensaje = mensaje.ToString() });
        }


        #endregion
        [seguridad(false)]
        [TokenProgresivo(false)]
        [HttpPost]
        public ActionResult ConsultarEstadoProgresivoSalaBtn1() {
            int status = 0;
            string message = "No se encontraron registros";
            bool inVpn = false;
            //ARREGLAR
            List<dynamic> listaFinal = new List<dynamic>();
            List<SalaEntidad> listaSalas = _salaBl.ListadoSala();
            List<string> urls = listaSalas.Where(x => !string.IsNullOrEmpty(x.UrlProgresivo)).Select(x => x.UrlProgresivo).ToList();

            foreach(var salar in listaSalas) {
                CheckPortHelper checkPortHelperT = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnectionT = checkPortHelperT.TcpUrlMultiple(salar.UrlProgresivo, urls);

                if(!tcpConnectionT.IsOpen) {
                    return Json(new
                    {
                        status = 2,
                        message = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                    });
                }
                List<WEB_Progresivo> listaProgresivo = new List<WEB_Progresivo>();

                try {
                    string response = string.Empty;
                    string uri = $"{salar.UrlProgresivo}/servicio/ListadoProgresivos";

                    dynamic data = new { };

                    if(tcpConnectionT.IsVpn) {
                        inVpn = true;
                        uri = $"{tcpConnectionT.Url}/servicio/listadoprogresivosVpn";

                        data = new
                        {
                            ipPrivada = $"{salar.IpPrivada}:{salar.PuertoServicioWebOnline}/servicio/ListadoProgresivos"
                        };
                    }

                    string json = JsonConvert.SerializeObject(data);

                    using(MyWebClientInfinite webClient = new MyWebClientInfinite()) {
                        webClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";
                        webClient.Headers[HttpRequestHeader.Accept] = "application/json";
                        response = webClient.UploadString(uri, "POST", json);
                    }

                    var settings = new JsonSerializerSettings {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    // Data
                    listaProgresivo = JsonConvert.DeserializeObject<List<WEB_Progresivo>>(response, settings);

                    status = 1;
                    message = "Datos obtenidos correctamente";
                } catch(Exception exception) {
                    message = exception.Message;
                }

                var resultData = new
                {
                    salar.Nombre,
                    status,
                    message,
                    data = listaProgresivo,
                    inVpn
                };

                JavaScriptSerializer serializer = new JavaScriptSerializer {
                    MaxJsonLength = int.MaxValue
                };

                ContentResult result = new ContentResult {
                    Content = serializer.Serialize(resultData),
                    ContentType = "application/json"
                };


                List<dynamic> listaProgresivos = new List<dynamic>();



                foreach(var item in listaProgresivo) {


                    var primerBotonEstado = "";
                    var segundoBotonEstado = "";
                    var webUrl = item.WEB_Url;
                    var apiUrl = $"{webUrl}/api/home/GETpOZOS?flag=true";
                    var apiUrlBotonDos = $"{webUrl}/api/PremiosProgresivo/Obtener_Premios_Progresivo_Dia?maquina=%27%27&TipoPozo=-1&cantidad=100";

                    using(MyWebClientInfinite webClient = new MyWebClientInfinite()) {
                        webClient.Headers[HttpRequestHeader.Accept] = "application/json";

                        try {
                            Uri xd;
                            bool urlValidate = Uri.TryCreate(apiUrl, UriKind.RelativeOrAbsolute, out xd);
                            if(urlValidate) {
                                var response = webClient.DownloadString(apiUrl);
                                primerBotonEstado = "Estado 200";
                            } else {
                                primerBotonEstado = "No existe malUri";
                            }

                        } catch(WebException ex) {
                            if(ex.Response is HttpWebResponse httpResponse) {
                                var statusCode = httpResponse.StatusCode;
                                primerBotonEstado = $"Estado {statusCode}";

                            } else {
                                // Manejar otros casos de excepción si es necesario
                                primerBotonEstado = $"Estado {ex.Message}";

                            }
                        }

                        


                        dynamic DataFinalProgresivo = new
                        {
                            nombreProgresivo = item.WEB_Nombre,
                            urlProgresivo = apiUrl,
                            estadoPrimerBoton = primerBotonEstado,
                            estadoSegundoBoton = segundoBotonEstado,
                        };

                        listaProgresivos.Add(DataFinalProgresivo);
                    }
                }
                dynamic resultDataSalaProgresivo = new
                {
                    salar.Nombre,
                    status,
                    message,
                    data = listaProgresivos,
                    inVpn
                };

                listaFinal.Add(resultDataSalaProgresivo);
            }
            return Json(new { data = listaFinal });
        }

        [seguridad(false)]
        [TokenProgresivo(false)]
        [HttpPost]
        public ActionResult ConsultarEstadoProgresivoSala() {
            int status = 0;
            string message = "No se encontraron registros";
            bool inVpn = false;
          
            List<dynamic> listaFinal = new List<dynamic>();
            List<SalaEntidad> listaSalas = _salaBl.ListadoSala();
            List<string> urls = listaSalas.Where(x => !string.IsNullOrEmpty(x.UrlProgresivo)).Select(x => x.UrlProgresivo).ToList();

            foreach(var salar in listaSalas) {
                CheckPortHelper checkPortHelperT = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnectionT = checkPortHelperT.TcpUrlMultiple(salar.UrlProgresivo, urls);

                if(!tcpConnectionT.IsOpen) {
                    return Json(new
                    {
                        status = 2,
                        message = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                    });
                }
                List<WEB_Progresivo> listaProgresivo = new List<WEB_Progresivo>();

                try {
                    string response = string.Empty;
                    string uri = $"{salar.UrlProgresivo}/servicio/ListadoProgresivos";

                    dynamic data = new { };

                    if(tcpConnectionT.IsVpn) {
                        inVpn = true;
                        uri = $"{tcpConnectionT.Url}/servicio/listadoprogresivosVpn";

                        data = new
                        {
                            ipPrivada = $"{salar.IpPrivada}:{salar.PuertoServicioWebOnline}/servicio/ListadoProgresivos"
                        };
                    }

                    string json = JsonConvert.SerializeObject(data);

                    using(MyWebClientInfinite webClient = new MyWebClientInfinite()) {
                        webClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";
                        webClient.Headers[HttpRequestHeader.Accept] = "application/json";
                        response = webClient.UploadString(uri, "POST", json);
                    }

                    var settings = new JsonSerializerSettings {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    // Data
                    listaProgresivo = JsonConvert.DeserializeObject<List<WEB_Progresivo>>(response, settings);

                    status = 1;
                    message = "Datos obtenidos correctamente";
                } catch(Exception exception) {
                    message = exception.Message;
                }

                var resultData = new
                {
                    salar.Nombre,
                    status,
                    message,
                    data = listaProgresivo,
                    inVpn
                };

                JavaScriptSerializer serializer = new JavaScriptSerializer {
                    MaxJsonLength = int.MaxValue
                };

                ContentResult result = new ContentResult {
                    Content = serializer.Serialize(resultData),
                    ContentType = "application/json"
                };


                List<dynamic> listaProgresivos = new List<dynamic>();

                

                foreach(var item in listaProgresivo) {

                    
                    var primerBotonEstado="";
                    var segundoBotonEstado="";
                    var webUrl = item.WEB_Url;
                    var apiUrl = $"{webUrl}/api/home/GETpOZOS?flag=true";
                    var apiUrlBotonDos = $"{webUrl}/api/PremiosProgresivo/Obtener_Premios_Progresivo_Dia?maquina=%27%27&TipoPozo=-1&cantidad=100";

                    using(MyWebClientInfinite webClient = new MyWebClientInfinite()) {
                        webClient.Headers[HttpRequestHeader.Accept] = "application/json";

                        try {
                            Uri xd;
                            bool urlValidate = Uri.TryCreate(apiUrl, UriKind.RelativeOrAbsolute, out xd);
                            if(urlValidate) {
                                var response = webClient.DownloadString(apiUrl);
                                primerBotonEstado = "Estado 200";
                            } else {
                                primerBotonEstado = "No existe malUri";
                            }
                          
                        } catch(WebException ex) {
                            if(ex.Response is HttpWebResponse httpResponse) {
                                var statusCode = httpResponse.StatusCode;
                                primerBotonEstado = $"Estado {statusCode}";

                            } else {
                                // Manejar otros casos de excepción si es necesario
                                primerBotonEstado = $"Estado {ex.Message}";

                            }
                        }

                        try {
                            Uri xd;
                            bool urlValidate = Uri.TryCreate(apiUrlBotonDos, UriKind.RelativeOrAbsolute, out xd);
                            if(urlValidate) {
                                var response = webClient.DownloadString(apiUrlBotonDos);
                                segundoBotonEstado = "Estado 200";
                            } else {
                                segundoBotonEstado = "No existe malUri";
                            }

                        } catch(WebException ex) {
                            if(ex.Response is HttpWebResponse httpResponse) {
                                var statusCode = httpResponse.StatusCode;
                                segundoBotonEstado = $"Estado {statusCode}";

                            } else {
                                // Manejar otros casos de excepción si es necesario
                                segundoBotonEstado = $"Estado {ex.Message}";

                            }
                        }


                        dynamic DataFinalProgresivo = new
                        {
                            nombreProgresivo =item.WEB_Nombre,
                            urlProgresivo= apiUrl,
                            estadoPrimerBoton = primerBotonEstado,
                            estadoSegundoBoton = segundoBotonEstado,
                        };

                        listaProgresivos.Add(DataFinalProgresivo);
                    }
                }
                dynamic resultDataSalaProgresivo = new
                {
                    salar.Nombre,
                    status,
                    message,
                    data = listaProgresivos,
                    inVpn
                };

                listaFinal.Add(resultDataSalaProgresivo);
            }
            return Json(new {data = listaFinal} );
        }



        #region Mantenimiento Progresivo
        [seguridad(false)]
        [TokenProgresivo(false)]
        [HttpPost]
        public ActionResult ListarProgresivos(int salaId)
        {

            int status = 0;
            string message = "No se encontraron registros";
            int roomCode = salaId;
            int userId = Convert.ToInt32(Session["UsuarioID"]);
            bool inVpn = false;

            SalaEntidad sala = _salaBl.ObtenerSalaPorCodigo(roomCode);

            if (sala.CodSala == 0)
            {
                return Json(new
                {
                    status = 2,
                    message = "No se encontro sala, por favor ingrese datos correctos"
                });
            }

            List<SalaEntidad> listaSalas = _salaBl.ListadoSalaPorUsuario(userId);
            List<string> urls = listaSalas.Where(x => !string.IsNullOrEmpty(x.UrlProgresivo)).Select(x => x.UrlProgresivo).ToList();

            //Check port
            CheckPortHelper checkPortHelper = new CheckPortHelper();
            CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, urls);

            if (!tcpConnection.IsOpen)
            {
                return Json(new
                {
                    status = 2,
                    message = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                });
            }

            List<WEB_Progresivo> listaProgresivo = new List<WEB_Progresivo>();

            try
            {
                string response = string.Empty;
                string uri = $"{sala.UrlProgresivo}/servicio/ListadoProgresivos";

                dynamic data = new {};

                if (tcpConnection.IsVpn)
                {
                    inVpn = true;
                    uri = $"{tcpConnection.Url}/servicio/listadoprogresivosVpn";

                    data = new
                    {
                        ipPrivada = $"{sala.IpPrivada}:{sala.PuertoServicioWebOnline}/servicio/ListadoProgresivos"
                    };
                }

                string json = JsonConvert.SerializeObject(data);

                using (MyWebClientInfinite webClient = new MyWebClientInfinite())
                {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";
                    webClient.Headers[HttpRequestHeader.Accept] = "application/json";
                    response = webClient.UploadString(uri, "POST", json);
                }

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                // Data
                listaProgresivo = JsonConvert.DeserializeObject<List<WEB_Progresivo>>(response, settings);

                status = 1;
                message = "Datos obtenidos correctamente";
            }
            catch (Exception exception)
            {
                message = exception.Message;
            }

            var resultData = new
            {
                status,
                message,
                data = listaProgresivo,
                inVpn
            };

            JavaScriptSerializer serializer = new JavaScriptSerializer
            {
                MaxJsonLength = int.MaxValue
            };

            ContentResult result = new ContentResult
            {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };

            return result;
        }

        [HttpPost]
        public ActionResult ReiniciarProgresivo(int salaId, string urlProgresivo)
        {
            int status = 0;
            string message = "Progresivo no reiniciado";
            int userId = Convert.ToInt32(Session["UsuarioID"]);
            bool inVpn = false;

            SalaEntidad sala = _salaBl.ObtenerSalaPorCodigo(salaId);

            if (sala.CodSala == 0)
            {
                return Json(new
                {
                    status = 2,
                    message = "No se encontro sala, por favor ingrese datos correctos"
                });
            }

            if(string.IsNullOrEmpty(urlProgresivo))
            {
                return Json(new
                {
                    status = 2,
                    message = "Por favor, ingrese Url Progresivo"
                });
            }

            List<SalaEntidad> listaSalas = _salaBl.ListadoSalaPorUsuario(userId);
            List<string> urls = listaSalas.Where(x => !string.IsNullOrEmpty(x.UrlProgresivo)).Select(x => x.UrlProgresivo).ToList();

            //Check port
            CheckPortHelper checkPortHelper = new CheckPortHelper();
            CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, urls);

            if (!tcpConnection.IsOpen)
            {
                return Json(new
                {
                    status = 2,
                    message = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                });
            }

            dynamic data = string.Empty;

            try
            {
                string response = string.Empty;
                string uri = $"{sala.UrlProgresivo}/servicio/ReiniciarProgresivo";

                dynamic args = new {
                    urlProgresivo
                };

                if (tcpConnection.IsVpn)
                {
                    inVpn = true;
                    uri = $"{tcpConnection.Url}/servicio/ReiniciarProgresivo";
                }

                //uri = "http://192.168.1.110:9895/servicio/ReiniciarProgresivo";
                string json = JsonConvert.SerializeObject(args);

                using (MyWebClientInfinite webClient = new MyWebClientInfinite())
                {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";
                    webClient.Headers[HttpRequestHeader.Accept] = "application/json";
                    response = webClient.UploadString(uri, "POST", json);
                }

                // Data
                data = JsonConvert.DeserializeObject<dynamic>(response);

                status = Convert.ToBoolean(data.status) ? 1 : 0;
                message = Convert.ToString(data.message);
                data = Convert.ToString(data.data);
            }
            catch (Exception exception)
            {
                message = exception.Message;
            }

            return Json(new {
                status,
                message,
                data,
                inVpn
            });
        }

        [HttpPost]
        public ActionResult ObtenerFechaProgresivo(int salaId, string urlProgresivo)
        {
            int status = 0;
            string message = "No es posible conectar con el servidor remoto";
            int userId = Convert.ToInt32(Session["UsuarioID"]);
            bool inVpn = false;

            SalaEntidad sala = _salaBl.ObtenerSalaPorCodigo(salaId);

            if (sala.CodSala == 0)
            {
                return Json(new
                {
                    status = 2,
                    message = "No se encontro sala, por favor ingrese datos correctos"
                });
            }

            if (string.IsNullOrEmpty(urlProgresivo))
            {
                return Json(new
                {
                    status = 2,
                    message = "Por favor, ingrese Url Progresivo"
                });
            }

            List<SalaEntidad> listaSalas = _salaBl.ListadoSalaPorUsuario(userId);
            List<string> urls = listaSalas.Where(x => !string.IsNullOrEmpty(x.UrlProgresivo)).Select(x => x.UrlProgresivo).ToList();

            //Check port
            CheckPortHelper checkPortHelper = new CheckPortHelper();
            CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, urls);

            if (!tcpConnection.IsOpen)
            {
                return Json(new
                {
                    status = 2,
                    message = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                });
            }

            dynamic progresivo = new System.Dynamic.ExpandoObject();

            try
            {
                string response = string.Empty;
                string uri = $"{sala.UrlProgresivo}/servicio/ObtenerHoraProgresivo";

                dynamic args = new
                {
                    urlProgresivo
                };

                if (tcpConnection.IsVpn)
                {
                    inVpn = true;
                    uri = $"{tcpConnection.Url}/servicio/ObtenerHoraProgresivo";
                }

                //uri = "http://192.168.1.110:9895/servicio/ObtenerHoraProgresivo";
                string json = JsonConvert.SerializeObject(args);

                using (MyWebClientInfinite webClient = new MyWebClientInfinite())
                {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";
                    webClient.Headers[HttpRequestHeader.Accept] = "application/json";
                    response = webClient.UploadString(uri, "POST", json);
                }

                // Data
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response);

                if (Convert.ToBoolean(data.status))
                {
                    status = 1;
                    message = "Datos obtenidos correctamente";
                    progresivo = new
                    {
                        date = Convert.ToDateTime(data.data.date),
                        dateFormatted = Convert.ToString(data.data.dateFormatted),
                    };
                }
            }
            catch (Exception exception)
            {
                message = exception.Message;
            }

            return Json(new
            {
                status,
                message,
                data = progresivo,
                inVpn
            });
        }

        [HttpPost]
        public ActionResult ActualizarFechaProgresivo(int salaId, string urlProgresivo, string urlFechaHora)
        {
            int status = 0;
            string message = "No se encontraron registros";
            int userId = Convert.ToInt32(Session["UsuarioID"]);
            bool inVpn = false;

            SalaEntidad sala = _salaBl.ObtenerSalaPorCodigo(salaId);

            if (sala.CodSala == 0)
            {
                return Json(new
                {
                    status = 2,
                    message = "No se encontro sala, por favor ingrese datos correctos"
                });
            }

            if (string.IsNullOrEmpty(urlProgresivo))
            {
                return Json(new
                {
                    status = 2,
                    message = "Por favor, ingrese Url Progresivo"
                });
            }

            if (string.IsNullOrEmpty(urlFechaHora))
            {
                return Json(new
                {
                    status = 2,
                    message = "Por favor, ingrese Url Fecha Hora"
                });
            }

            List<SalaEntidad> listaSalas = _salaBl.ListadoSalaPorUsuario(userId);
            List<string> urls = listaSalas.Where(x => !string.IsNullOrEmpty(x.UrlProgresivo)).Select(x => x.UrlProgresivo).ToList();

            //Check port
            CheckPortHelper checkPortHelper = new CheckPortHelper();
            CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, urls);

            if (!tcpConnection.IsOpen)
            {
                return Json(new
                {
                    status = 2,
                    message = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                });
            }

            dynamic data = string.Empty;

            try
            {
                string response = string.Empty;
                string uri = $"{sala.UrlProgresivo}/servicio/CambiarHoraProgresivo";

                dynamic args = new
                {
                    urlProgresivo,
                    urlFechaHora
                };

                if (tcpConnection.IsVpn)
                {
                    inVpn = true;
                    uri = $"{tcpConnection.Url}/servicio/CambiarHoraProgresivo";
                }

                //uri = "http://192.168.1.110:9895/servicio/CambiarHoraProgresivo";
                string json = JsonConvert.SerializeObject(args);

                using (MyWebClientInfinite webClient = new MyWebClientInfinite())
                {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";
                    webClient.Headers[HttpRequestHeader.Accept] = "application/json";
                    response = webClient.UploadString(uri, "POST", json);
                }

                // Data
                data = JsonConvert.DeserializeObject<dynamic>(response);

                status = Convert.ToBoolean(data.status) ? 1 : 0;
                message = Convert.ToString(data.message);
                data = Convert.ToString(data.data);
            }
            catch (Exception exception)
            {
                message = exception.Message;
            }

            return Json(new
            {
                status,
                message,
                data,
                inVpn
            });
        }

        #endregion


        #region Reporte Control Progresivos
        [seguridad(false)]
        [TokenProgresivo(false)]
        public ActionResult ReporteControlProgresivos()
        {
            return View("~/Views/Progresivo/ReporteControlProgresivos.cshtml");
        }
        [seguridad(false)]
        [TokenProgresivo(false)]
        [HttpPost]
        public ActionResult GetOdata(string oDataStr)
        {
            //buscar data de maquina
            string token = GetToken();
            string uri = ConfigurationManager.AppSettings["AdministrativoUri"];
            object oRespuesta = new object();
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);

            //HttpResponseMessage response = client.GetAsync($"odata/empresa?$select=CodEmpresa,RazonSocial&$filter=TipoEmpresa eq 1 or TipoEmpresa eq 3 and Activo eq true&$orderby=RazonSocial").Result;
            HttpResponseMessage response = client.GetAsync(oDataStr).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                return Json(data);
            }
            else
            {
                return Json(false);
            }

        }
        [seguridad(false)]
        [TokenProgresivo(false)]
        private static string GetToken()
        {
            string administrativoUsername = ConfigurationManager.AppSettings["AdministrativoUsername"];
            string administrativoPassword = ConfigurationManager.AppSettings["AdministrativoPassword"];
            string key = administrativoUsername + ":" + administrativoPassword;
            return Encode(key);
        }
        [seguridad(false)]
        [TokenProgresivo(false)]
        private static string Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        [seguridad(false)]
        [TokenProgresivo(false)]
        public ActionResult ConsolidadoControlProgresivos() {
            return View("~/Views/Progresivo/ConsolidadoControlProgresivos.cshtml");
        }
        #endregion


        #region Metodos Para Migrar Data DIEGO - AsignacionMT - DetalleContadoresGame

        [seguridad(false)]
        [TokenProgresivo(false)]
        [HttpPost]
        public ActionResult AsignacionMTRecepcionOnlineJson(List<Asignacion_M_T> listaReporteProgresivo)
        {
            var mensaje = "";
            var response = false;
            var jsonResponse = new List<Asignacion_M_T>();
            try
            {

                List<Asignacion_M_T> listaActual = progesivoBL.GetAsignacion_M_T();

                foreach (var item in listaReporteProgresivo)
                {
                    var existe = listaActual.FirstOrDefault(x => (Convert.ToInt32(x.CodMaq) == Convert.ToInt32(item.CodMaq) && x.COD_SALA == item.COD_SALA));

                    if (existe == null)
                    {
                        
                        int id = progesivoBL.Asignacion_M_TInsertarJson(item);

                        if (id==0)
                        {
                            mensaje = "Error al insertar Asignacion_M_T";
                            response = false;
                            return Json(new { respuesta = response, mensaje = mensaje.ToString() });
                        }
                    }


                }

                response = true;
                mensaje = "Generado Asignacion_M_T";

            }
            catch (Exception ex)
            {
                response = false;
                return Json(new { respuesta = response, mensaje = ex.Message.ToString() });
            }
            return Json(new { respuesta = response, mensaje = mensaje.ToString() });
        }


        [seguridad(false)]
        [TokenProgresivo(false)]
        [HttpPost]
        public ActionResult DetalleContadoresGameRecepcionOnlineJson(List<DetalleContadoresGame> listaReporteProgresivo)
        {
            var mensaje = "";
            var response = false;
            var jsonResponse = new List<DetalleContadoresGame>();
            try
            {

                List<DetalleContadoresGame> listaActual = progesivoBL.GetDetalleContadoresGame();

                foreach (var item in listaReporteProgresivo)
                {
                    //item.Fecha = Convert.ToDateTime(item.FechaStr);
                    //item.Hora = Convert.ToDateTime(item.HoraStr);
                    //item.FechaCompleta = item.Hora;

                    item.FechaRegistro = DateTime.Now;
                    item.FechaModificacion = DateTime.Now;
                    var existe = listaActual.FirstOrDefault(x => (Convert.ToInt32(x.CodSala) == Convert.ToInt32(item.CodSala) && x.CodMaquina == item.CodMaquina && x.FechaOperacion == item.FechaOperacion));

                    if (existe == null)
                    {

                        int id = progesivoBL.DetalleContadoresGameInsertarJson(item);

                        if (id==0)
                        {
                            mensaje = "Error al insertar detalle contador game";
                            response = false;
                            return Json(new { respuesta = response, mensaje = mensaje.ToString() });
                        }
                    }


                }

                response = true;
                mensaje = "Generado detalles contadores game";

            }
            catch (Exception ex)
            {
                response = false;
                return Json(new { respuesta = response, mensaje = ex.Message.ToString() });
            }
            return Json(new { respuesta = response, mensaje = mensaje.ToString() });
        }

        #endregion

        #region Metodos CRUD Marca Modelo Progresivo

        public ActionResult ObtenerMarcaId(string url) {

            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new Marca();
            try {
                client.Headers.Add("content-type", "application/json");
                response = client.DownloadString(url);
                jsonResponse = JsonConvert.DeserializeObject<Marca>(response);
            } catch (Exception ex) {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse });

        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ObtenerMarcaIdVpn(string urlPublica, string urlPrivada) {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new Marca();
            try {
                object oEnvio = new
                {
                    ipPrivada = urlPrivada,
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(urlPublica, "POST", inputJson);
                jsonResponse = JsonConvert.DeserializeObject<Marca>(response);
            } catch (Exception ex) {
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse });
        }

        public ActionResult GuardarMarca(MarcaMaquinaEntidad parametros, string url) {
            var client = new System.Net.WebClient();
            var response = "";

            try {
                string parameters = "Nombre=" + parametros.Nombre + "&Estado="+ parametros.Estado;
                using (WebClient wc = new WebClient()) {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    response = wc.UploadString(url, "POST", parameters);
                }
            } catch (Exception ex) {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = response });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult GuardarMarcaVpn(MarcaMaquinaEntidad parametros, string urlPublica, string urlPrivada) {
            var client = new System.Net.WebClient();
            var response = "";
            dynamic jsonResponse = "";
            try {
                object oEnvio = new
                {
                    ipPrivada = urlPrivada,
                    marca = parametros
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(urlPublica, "POST", inputJson);
                jsonResponse = JsonConvert.DeserializeObject<dynamic>(response);

            } catch (Exception ex) {
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse });
        }

        public ActionResult EditarMarca(MarcaMaquinaEntidad parametros, string url) {
            var client = new System.Net.WebClient();
            var response = "";

            try {
                string parameters = "MarcaID=" + parametros.CodMarcaMaquina + "&Nombre=" + parametros.Nombre + "&Estado=" + parametros.Estado;

                using (WebClient wc = new WebClient()) {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    response = wc.UploadString(url, "PUT", parameters);
                }
            } catch (Exception ex) {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = response });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult EditarMarcaVpn(MarcaMaquinaEntidad parametros, string urlPublica, string urlPrivada) {
            var client = new System.Net.WebClient();
            var response = "";
            dynamic jsonResponse = "";
            try {
                object oEnvio = new
                {
                    ipPrivada = urlPrivada,
                    marca = parametros
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(urlPublica, "POST", inputJson);
                jsonResponse = JsonConvert.DeserializeObject<dynamic>(response);

            } catch (Exception ex) {
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse });
        }


        public ActionResult ObtenerModeloId(string url) {

            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new ModeloMaquinaEntidad();
            try {
                client.Headers.Add("content-type", "application/json");
                response = client.DownloadString(url);
                jsonResponse = JsonConvert.DeserializeObject<ModeloMaquinaEntidad>(response);
            } catch (Exception ex) {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse });

        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ObtenerModeloIdVpn(string urlPublica, string urlPrivada) {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new ModeloMaquinaEntidad();
            try {
                object oEnvio = new
                {
                    ipPrivada = urlPrivada,
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(urlPublica, "POST", inputJson);
                jsonResponse = JsonConvert.DeserializeObject<ModeloMaquinaEntidad>(response);
            } catch (Exception ex) {
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse });
        }



        public ActionResult GuardarModelo(ModeloMaquinaEntidad parametros, string url) {
            var client = new System.Net.WebClient();
            var response = "";

            try {
                string parameters = "MarcaID=" + parametros.CodMarcaMaquina +"&Nombre=" + parametros.Nombre + "&Estado=" + parametros.Estado;
                using (WebClient wc = new WebClient()) {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    response = wc.UploadString(url, "POST", parameters);
                }

                if (response.ToString()=="1") {


                }


            } catch (Exception ex) {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = response });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult GuardarModeloVpn(ModeloMaquinaEntidad parametros, string urlPublica, string urlPrivada) {
            var client = new System.Net.WebClient();
            var response = "";
            dynamic jsonResponse = "";
            try {
                object oEnvio = new
                {
                    ipPrivada = urlPrivada,
                    modelo = parametros
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(urlPublica, "POST", inputJson);
                jsonResponse = JsonConvert.DeserializeObject<dynamic>(response);

            } catch (Exception ex) {
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse });
        }

        public ActionResult EditarModelo(ModeloMaquinaEntidad parametros, string url) {
            var client = new System.Net.WebClient();
            var response = "";

            try {
                string parameters = "MarcaID=" + parametros.CodMarcaMaquina + "&ModeloID=" + parametros.CodModeloMaquina + "&Nombre=" + parametros.Nombre + "&Estado=" + parametros.Estado;

                using (WebClient wc = new WebClient()) {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    response = wc.UploadString(url, "PUT", parameters);
                }
            } catch (Exception ex) {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = response });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult EditarModeloVpn(ModeloMaquinaEntidad parametros, string urlPublica, string urlPrivada) {
            var client = new System.Net.WebClient();
            var response = "";
            dynamic jsonResponse = "";
            try {
                object oEnvio = new
                {
                    ipPrivada = urlPrivada,
                    modelo = parametros
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(urlPublica, "POST", inputJson);
                jsonResponse = JsonConvert.DeserializeObject<dynamic>(response);

            } catch (Exception ex) {
                return Json(new { mensaje = ex.Message.ToString() });
            }
            return Json(new { data = jsonResponse });
        }

        #endregion

        #region Modulo Progresivo V2

        [seguridad(false)]
        [TokenProgresivo(false)]
        [HttpPost]
        public async Task<ActionResult> ObtenerWebProgresivos(int salaId)
        {
            bool success = false;
            bool inVpn = false;
            string message = "No se ha encontrado registros";

            if (salaId <= 0)
            {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione una sala"
                });
            }

            List<WEB_Progresivo> data = new List<WEB_Progresivo>();

            try
            {
                SalaEntidad sala = _salaBl.ObtenerSalaPorCodigo(salaId);

                if (sala.CodSala == 0)
                {
                    return Json(new
                    {
                        success,
                        message = "No se ha encontrado datos de la sala"
                    });
                }

                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, new List<string>());

                if (!tcpConnection.IsOpen)
                {
                    return Json(new
                    {
                        success,
                        message = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                    });
                }

                string servicePath = "Servicio/ObtenerWebProgresivos";
                string content = string.Empty;
                string requestUri = string.Empty;

                if (tcpConnection.IsVpn)
                {
                    inVpn = true;

                    object arguments = new
                    {
                        ipPrivada = $"{sala.IpPrivada}:{sala.PuertoServicioWebOnline}/{servicePath}"
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{tcpConnection.Url}/Servicio/VPNGenericoPost";
                }
                else
                {
                    content = JsonConvert.SerializeObject(string.Empty);
                    requestUri = $"{sala.UrlProgresivo}/{servicePath}";
                }

                ResultEntidad result = await _serviceHelper.PostAsync(requestUri, content);

                if (result.success)
                {
                    List<WEB_Progresivo> listaProgresivos = JsonConvert.DeserializeObject<List<WEB_Progresivo>>(result.data) ?? new List<WEB_Progresivo>();

                    if (listaProgresivos.Any())
                    {
                        data = listaProgresivos;

                        success = true;
                        message = "Registros obtenidos correctamente";
                    }
                }
                else
                {
                    success = false;
                    message = result.message;
                }
            }
            catch (Exception exception)
            {
                success = false;
                message = exception.Message;
            }

            return Json(new
            {
                success,
                message,
                data,
                inVpn
            });
        }

        #endregion
    }
}