using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    [seguridad]
    public class EstadoServiciosController : Controller
    {
        private EstadoServiciosBL estadoServiciosBL= new EstadoServiciosBL();

        public ActionResult EstadoServiciosVista()
        {
            return View();
        }

        [HttpPost]
        public JsonResult EstadoServiciosListadoJson(int codSala)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            List<EstadoServiciosEntidad> data = new List<EstadoServiciosEntidad>();

            try
            {
                if (codSala == 0)
                {
                    data = estadoServiciosBL.GetEstadoServiciosAll();
                } else
                {
                    data = estadoServiciosBL.GetEstadoServiciosxSala(codSala);
                }
                respuesta= true;

            }catch(Exception ex) { 

                mensaje= ex.Message;
            }

            return Json(new { respuesta, mensaje, data = data.ToList() });
        }



    }
}