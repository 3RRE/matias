using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CapaNegocio;
using System.IO.Ports;
using System.Reflection;
using CapaEntidad;

namespace CapaPresentacion.Controllers
{
    [seguridad]
    public class RolUsuarioController : Controller
    {
        private SEG_RolUsuarioBL web_RolUsuarioBL = new SEG_RolUsuarioBL();
        private SEG_RolBL webRolBl = new SEG_RolBL();
        private SEG_UsuarioBL segUsuarioBl = new SEG_UsuarioBL();
        private SEG_RolUsuarioBL webRolUsuarioBl = new SEG_RolUsuarioBL();

        [HttpPost]
        public ActionResult GuardarRolUsuario(SEG_RolUsuarioEntidad rolUsuario)
        {
            var errormensaje = "Accion realizada Correctamente.";
            bool respuestaConsulta = false;
            bool deleteUsuario = false;
            try
            {
                deleteUsuario = web_RolUsuarioBL.EliminarRolUsuario(rolUsuario.UsuarioID);
                rolUsuario.WEB_RUsuFechaRegistro = DateTime.Now;
                respuestaConsulta = web_RolUsuarioBL.GuardarRolUsuario(rolUsuario);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListadoRolUsuario()
        {
            var errormensaje = "";
            var lista = new List<SEG_RolEntidad>();
            try
            {
                lista = webRolBl.ListarRol().OrderBy(x => x.WEB_RolNombre).ToList();

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = lista, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListadoTableUsuarioAsignarRol()
        {
            var errormensaje = "";
            var listaRol = new List<SEG_RolEntidad>();
            var listaUsu = new List<SEG_UsuarioEntidad>();
            var listaRolUsuario = new List<SEG_RolUsuarioEntidad>();
            try
            {
                listaRol = webRolBl.ListarRol().OrderBy(x => x.WEB_RolNombre).ToList();
                listaUsu = segUsuarioBl.UsuarioListadoJson();
                listaRolUsuario = webRolUsuarioBl.ListarRolUsuarios();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { roles = listaRol, usuarios = listaUsu.ToList(), rolUsuarios = listaRolUsuario.ToList(), mensaje = errormensaje });
        }
    }
}