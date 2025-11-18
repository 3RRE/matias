using CapaEntidad.Reportes.ReporteSunat;
using CapaNegocio;
//using CapaNegocio.ReporteSunat;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace CapaPresentacion.Controllers.Reportes
{
    public class ReporteSunatController : Controller
    {
        //private ReporteSunatBL _sunat = new ReporteSunatBL();
        // GET: RporteSunat
        public ActionResult Index()
        {
            return View("~/Views/ReporteSunat/Index.cshtml");
        }


        public  ActionResult ListadoReporteSunat(string fechaIni, string fechaFin) {


            List<ReporteSunatEntidad> reporteSunat = new List<ReporteSunatEntidad>();

            //reporteSunat =  _sunat.reporteSunatxFecha(fechaIni,fechaFin);

            List<SunatJsonEntidad> data3 = new List<SunatJsonEntidad>();

            List<ReporteSunatEntidad> reporteSunat2 = new List<ReporteSunatEntidad>();

            foreach (var item in reporteSunat)
            {
                string jsonTrama = item.trama;
                SunatJsonEntidad sunatJson = JsonConvert.DeserializeObject<SunatJsonEntidad>(jsonTrama);
                item.trama = sunatJson;

                reporteSunat2.Add(item);

                //data3.Add(sunatJson);
            }

            return Json(new { data = reporteSunat2, mensaje = "errormensaje" });

        }
        public ActionResult Edit(string id)
        {

            return Json(new { id = id }); 
        }

        //public class SunatEntidad
        //{
        //    public DateTime fecha;
        //    public string trama;
        //    public int cereo;
        //    public int idconsunat;
        //    public bool envio;
        //    public string idCereo;
        //    public DateTime Fecha_Proceso;
        //    public DateTime FechaEnvio;
        //    public string motivo;
        //    public int idConfSunat;
        //    public bool bandbusq;
        //}
        //public class SunatJsonEntidad
        //{
        //    public string Nombre;
        //    public string Tipo;
        //}
    }
}