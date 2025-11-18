using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaDatos.ControlAcceso;
using CapaNegocio.ControlAcceso;
using CapaEntidad.ControlAcceso;
using System.Drawing;
using OfficeOpenXml.Style;
using System.IO;
using OfficeOpenXml;

namespace CapaPresentacion.Controllers.ControlAcceso
{
    public class CALTipoTimadorController : Controller
    {
        private CAL_TipoTimadorBL tipoTimadorBL = new CAL_TipoTimadorBL();

        public ActionResult ListadoTipoTimador()
        {
            return View("~/Views/ControlAcceso/ListadoTipoTimador.cshtml");
        }

        [HttpPost]
        public JsonResult ListarTipoTimadorJson()
        {
            var errormensaje = "";
            var lista = new List<CAL_TipoTimadorEntidad>();

            try
            {

                lista = tipoTimadorBL.TipoTimadorListadoCompletoJson();

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}