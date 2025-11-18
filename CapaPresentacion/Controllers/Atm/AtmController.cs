using CapaEntidad;
using CapaNegocio;
using CapaNegocio.Progresivo;
using CapaPresentacion.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CapaPresentacion.Controllers.Atm
{
    [seguridad]
    public class AtmController : Controller
    {
        public ActionResult UtilitarioAtmVista()
        {
            return View("~/Views/Atm/UtilitarioAtmVista.cshtml");
        }

        public ActionResult ConsultarApertura(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            //var jsonResponse = new object();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                //var stream = client.OpenRead(url);
                //var reader = new StreamReader(stream);
                //response = reader.ReadToEnd();

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                //jsonResponse = JsonConvert.DeserializeObject<object>(response, settings);

            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            var jsonResult = Json(new { data = response }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult ValidarApertura(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            //var jsonResponse = new object();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                //var stream = client.OpenRead(url);
                //var reader = new StreamReader(stream);
                //response = reader.ReadToEnd();

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                //jsonResponse = JsonConvert.DeserializeObject<object>(response, settings);

            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            var jsonResult = Json(new { data = response }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult ModificarApertura(string url)
        {
            var client = new System.Net.WebClient();
            var response = "";

            //var jsonResponse = new object();
            try
            {
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(url, "POST");
                //var stream = client.OpenRead(url);
                //var reader = new StreamReader(stream);
                //response = reader.ReadToEnd();

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                //jsonResponse = JsonConvert.DeserializeObject<object>(response, settings);

            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            var jsonResult = Json(new { data = response }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}