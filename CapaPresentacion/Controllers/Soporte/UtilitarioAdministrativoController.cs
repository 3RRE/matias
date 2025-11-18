using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Soporte
{
    [seguridad]
    public class UtilitarioAdministrativoController : Controller
    {     
        public ActionResult UtilitarioAdministrativoVista()
        {
            return View("~/Views/Soporte/UtilitarioAdministrativo/UtilitarioAdministrativoVista.cshtml");
        }

        [HttpPost]
        public ActionResult UtilitarioAdministrativoSincronizarJson(DateTime fechaIni, DateTime fechaFin, string Sala)
        {
            var errormensaje = "";
            int rpta = 0;
            try
            {
                if(string.IsNullOrEmpty(Sala)) {
                    return Json(new { respuesta = false, mensaje = "No se configuró la url de sala" });
                }
                try
                {
                    var fecha1 = Convert.ToDateTime(fechaIni).ToString("MM/dd/yyyy");
                    var fecha2 = Convert.ToDateTime(fechaFin).ToString("MM/dd/yyyy");
                    string url = Sala + @"/servicio/GetAllUtilitario?fechaIni="+fecha1+"&fechaFin="+fecha2;
                    using (var client = new HttpClient())
                    {
                        client.Timeout= TimeSpan.FromMilliseconds(System.Threading.Timeout.Infinite);
                        using (var response = client.PostAsync(url,null).Result)
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                rpta = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                            }
                            else
                            {
                                rpta = 0;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    errormensaje = ex.Message + " ,Llame Administrador";
                }

                if (rpta == 1)
                {
                    errormensaje = "Ok";
                }
                else
                {
                    errormensaje = "Error de conexión";                    
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            bool rpt = errormensaje == "Ok" ? true : false;
            return Json(new { mensaje = errormensaje, respuesta = rpt });
        }
    }
}