using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class ModuloController : Controller
    {
        private ModuloBL modulobl = new ModuloBL();
        public ActionResult ModuloListarVista(int SistemaId)
        {
            ViewBag.SistemaId = SistemaId;
            return View();
        }
        public ActionResult ModuloEditarVista(int ModuloId)
        {
            ViewBag.ModuloId = ModuloId;
            return View();
        }
        public ActionResult ModuloListarJson(int SistemaId)
        {
            var errormensaje = "";
            var lista = new List<ModuloEntidad>();
            try
            {
                //lista = modulobl.ModuloListarJson(SistemaId);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }
    }
}