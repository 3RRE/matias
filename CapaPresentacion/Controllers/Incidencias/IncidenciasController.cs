using CapaEntidad.Disco;
using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad.Incidencias;
using CapaNegocio.Incidencias;
using CapaEntidad.Discos;
using CapaNegocio.Discos;
using CapaNegocio.Disco;

namespace CapaPresentacion.Controllers.Incidencias
{
    [seguridad]
    public class IncidenciasController : Controller
    {

        private readonly IncidenciaBL _incidenciaBL = new IncidenciaBL();
        private readonly SolucionIncidenciaBL _solucionIncidenciaBL = new SolucionIncidenciaBL();
        private readonly SistemaIncidenciaBL _sistemaIncidenciaBL = new SistemaIncidenciaBL();
        // GET: Incidencias
        public ActionResult Index()
        {
            return View("~/Views/Incidencias/Index.cshtml");
        }




        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarSistemaIncidencia()
        {
            var errormensaje = "";
            var lista = new List<SistemaIncidenciaEntidad>();
            try
            {
                lista = _sistemaIncidenciaBL.ListarSistemasIncidencias().ToList();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult AgregarSistemaIncidencia(SistemaIncidenciaEntidad nuevoSistema)
        {
            string errormensaje = "";
            bool respuesta = false;
            nuevoSistema.fecha_creacion = DateTime.Now;

            try
            {
                    int sistema = _sistemaIncidenciaBL.InsertarSistemaIncidencia(nuevoSistema);
                    errormensaje = "Sistema agregado correctamente";
                    respuesta = true;
                }
                catch (Exception exp)
                {
                    errormensaje = exp.Message + "Error,Llame Administrador";
                }

            return Json(new { data = nuevoSistema, mensaje = errormensaje, respuesta }, JsonRequestBehavior.AllowGet);
        }


        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarIncidencia(int idSistema)
        {
            var errormensaje = "";
            var lista = new List<IncidenciaEntidad>();
            try
            {
                lista = _incidenciaBL.ListarIncidencias(idSistema).ToList();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult AgregarIncidencia(IncidenciaEntidad nuevoIncidencia)
        {
            string errormensaje = "";
            bool respuesta = false;
            nuevoIncidencia.fecha_registro = DateTime.Now;
            try
            {
                int sistema = _incidenciaBL.InsertarIncidencia(nuevoIncidencia);
                errormensaje = "Incidencia agregada correctamente";
                respuesta = true;
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + "Error,Llame Administrador";
            }

            return Json(new { data = nuevoIncidencia, mensaje = errormensaje, respuesta }, JsonRequestBehavior.AllowGet);
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarSolucionIncidencia(int idIncidencia)
        {
            var errormensaje = "";
            var lista = new List<SolucionIncidenciaEntidad>();
            try
            {
                lista = _solucionIncidenciaBL.ListarSolucionIncidencias(idIncidencia).ToList();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult AgregarSolucionIncidencia(SolucionIncidenciaEntidad nuevaSolucion)
        {
            string errormensaje = "";
            bool respuesta = false;
            nuevaSolucion.fecha_registro = DateTime.Now;

            try
            {
                int sistema = _solucionIncidenciaBL.InsertarSolucionIncidencia(nuevaSolucion);
                errormensaje = "Incidencia agregada correctamente";
                respuesta = true;
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + "Error,Llame Administrador";
            }

            return Json(new { data = nuevaSolucion, mensaje = errormensaje, respuesta }, JsonRequestBehavior.AllowGet);
        }
    }
}