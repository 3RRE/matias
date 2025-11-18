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
    public class ConfiguracionSeguridadController : Controller
    {
        private ConfiguracionSeguridadBL configuracionseguridadbl = new ConfiguracionSeguridadBL();

        public ActionResult ConfiguracionSeguridadVista()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GuardarConfiguracionSeguridadJson(ConfiguracionSeguridadEntidad cs)
        {
            var errormensaje = "";
            var respuesta = false;

            try
            {
                if (cs.codWebConfiguracionSeguridad == 0)
                {
                    respuesta = configuracionseguridadbl.GuardarConfiguracionSeguridadJson(cs);
                }
                else
                {
                    respuesta = configuracionseguridadbl.ActualizarConfiguracionSeguridadJson(cs);
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }

            return Json(new { data = respuesta, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ObtenerConfiguracionSeguridadJson()
        {
            var errormensaje = "";
            ConfiguracionSeguridadEntidad configuracionSeguridad = null;

            try
            {
                configuracionSeguridad = configuracionseguridadbl.ObtenerConfiguracionSeguridadJson();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }

            return Json(new { data = configuracionSeguridad, mensaje = errormensaje });
        }
    }
}