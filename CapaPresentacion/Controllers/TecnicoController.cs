using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class TecnicoController : Controller
    {
        // GET: Tecnico
        private TecnicoBL tecnicobl = new TecnicoBL();
        private AreaTechBL areatechbl = new AreaTechBL();
        private NivelTecnicoBL nivelTecnicoBL = new NivelTecnicoBL();
        private SEG_EmpleadoBL sEG_EmpleadoBL = new SEG_EmpleadoBL();
        public ActionResult TecnicoListarVista()
        {
            return View();
        }

        public ActionResult TecnicoInsertarVista()
        {
            return View();
        }
        public ActionResult TecnicoEditarVista(string id)
        {
            int TecnicoId = Convert.ToInt32(id.Replace("Registro", ""));

            var errormensaje = "";
            var tecnico = new TecnicoEntidad();
            try
            {
                tecnico = tecnicobl.TecnicoIdObtenerJson(TecnicoId);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            ViewBag.Tecnico = tecnico;
            return View();
        }

        [HttpPost]
        public ActionResult TecnicoListarNombreEmpleadoJson()
        {
            var errormensaje = "";
            var lista = new List<TecnicoEntidad>();
            try
            {
                lista = tecnicobl.TecnicoListarJson();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult TecnicoInsertarJson(TecnicoEntidad tecnico)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                tecnico.FechaRegistro = DateTime.Now;
                respuestaConsulta = tecnicobl.TecnicoGuardarJson(tecnico);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult TecnicoEditarJson(TecnicoEntidad tecnico)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                tecnico.FechaRegistro = DateTime.Now;
                respuestaConsulta = tecnicobl.TecnicoEditarJson(tecnico);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult AreaTechListarJson()
        {
            var errormensaje = "";
            var lista = new List<AreaTechEntidad>();
            try
            {
                lista = areatechbl.AreaTechListarJson();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult NivelTecnicoListarJson()
        {
            var errormensaje = "";
            var lista = new List<NivelTecnicoEntidad>();
            try
            {
                lista = nivelTecnicoBL.NivelTecnicoListarJson();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult ActualizarEstadoTecnico(int TecnicoId, string TecnicoEstado)
        {
            int Estado = Convert.ToInt32(TecnicoEstado);
            var msj = "Accion realizada Correctamente.";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = tecnicobl.ActualizarEstadoTecnico(TecnicoId, Estado);
            }
            catch (Exception exp)
            {
                msj = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = msj });
        }

    }
}