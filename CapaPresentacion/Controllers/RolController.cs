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
    public class RolController : Controller
    {
        // GET: Rol
        private SEG_RolBL webRolBl = new SEG_RolBL();
        public ActionResult ListadoRol()
        {
            return View("~/Views/Rol/RolListadoVista.cshtml");
        }

        public ActionResult NuevoRol()
        {
            return View("~/Views/Rol/RolNuevoVista.cshtml");
        }
        public ActionResult RegistroRol(string id)
        {
            int sub = Convert.ToInt32(id.Replace("Registro", ""));
            var errormensaje = "";
            var rol = new SEG_RolEntidad();
            try
            {
                rol = webRolBl.GetRolId(sub);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                errormensaje = "Verifique conexion,Llame Administrador";
            }
            ViewBag.rol = rol;
            ViewBag.errormensaje = errormensaje;
            return View("~/Views/Rol/RolRegistroVista.cshtml");
        }


        [HttpPost]
        public ActionResult GetListadoRol()
        {
            var errormensaje = "";
            var lista = new List<SEG_RolEntidad>();
            try
            {
                lista = webRolBl.ListarRol();

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { data = lista.ToList(), mensaje = errormensaje });
            //  var aa = lista.ToList();

        }

        [HttpPost]
        public ActionResult GuardarRol(SEG_RolEntidad rol)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = webRolBl.GuardarRol(rol);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult ActualizarRol(SEG_RolEntidad rol)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = webRolBl.ActualizarRol(rol);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult ActualizarEstadoRol(int rolId, int estado)
        {
            var errormensaje = "Accion realizada Correctamente.";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = webRolBl.ActualizarEstadoRol(rolId, estado);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult DeleteRolId(int rolId)
        {
            var errormensaje = "Accion realizada Correctamente.";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = webRolBl.EliminarRol(rolId);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
    }
}