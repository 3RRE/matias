using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaNegocio.ControlAcceso;
using CapaEntidad.ControlAcceso;

namespace CapaPresentacion.Controllers.ControlAcceso
{
    public class CALTipoExclusionController : Controller
    {

        private CAL_TipoExclusionBL tipoExclusionBL = new CAL_TipoExclusionBL();

        public ActionResult ListadoTipoExclusion()
        {
            return View("~/Views/ControlAcceso/ListadoTipoExclusion.cshtml");
        }

        [HttpPost]
        public JsonResult ListarTipoExclusionJson()
        {
            var errormensaje = "";
            var lista = new List<CAL_TipoExclusionEntidad>();

            try
            {

                lista = tipoExclusionBL.TipoExclusionListadoCompletoJson();                                          

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}