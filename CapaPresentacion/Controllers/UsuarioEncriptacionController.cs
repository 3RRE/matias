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
    public class UsuarioEncriptacionController : Controller
    {
        // GET: UsuarioEncriptacion
        private UsuarioEncriptacionBL usuarioEncriptacionModel = new UsuarioEncriptacionBL();
        private SEG_EmpleadoBL empleadoModel = new SEG_EmpleadoBL();
        private TecnicoBL tecnicobl = new TecnicoBL();

        public ActionResult UsuarioEncriptacionListarVista()
        {
            return View();
        }

        public ActionResult UsuarioEncriptacionInsertarVista()
        {
            return View();
        }

        public ActionResult UsuarioEncriptacionEditarVista(int Id)
        {
            var errormensaje = "";
            var usuario = new UsuarioEncriptacionEntidad();
            try
            {
                usuario = usuarioEncriptacionModel.UsuarioEncriptacionIDObtenerJson(Id);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            ViewBag.Usuario = usuario;
            ViewBag.errormensaje = errormensaje;
            return View();
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult TecnicosEncriptacionListarJson()
        {
            var errormensaje = "";
            var lista = new List<EmpleadoEncriptacion>();
            var lista_revisada = new List<EmpleadoEncriptacion>();
            bool respuestaConsulta = false;
            var UsuarioEncriptacionContraseña = "";
            var usuario_encriptacion = new UsuarioEncriptacionEntidad();
            try
            {
                lista = empleadoModel.TecnicosEncriptacionListarJson();

                for (int i = 0; i < lista.Count; i++)
                {
                    if (DateTime.Now >= lista[i].FechaIni && DateTime.Now <= lista[i].FechaFin)
                    {
                    }
                    else
                    {
                        var random = new Random();
                        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                        UsuarioEncriptacionContraseña = new string(Enumerable.Repeat(chars, 6)
                          .Select(s => s[random.Next(s.Length)]).ToArray());
                        usuario_encriptacion.EmpleadoId = lista[i].EmpleadoID; ;
                        usuario_encriptacion.FechaIni = DateTime.Now;
                        usuario_encriptacion.FechaFin = DateTime.Now.AddHours(24);
                        usuario_encriptacion.FechaRegistro = DateTime.Now;
                        usuario_encriptacion.UsuarioPassword = UsuarioEncriptacionContraseña;
                        respuestaConsulta = usuarioEncriptacionModel.UsuarioEncriptacionRenovarContraseniaJson(usuario_encriptacion);
                    }
                }
                lista_revisada = empleadoModel.TecnicosEncriptacionListarJson();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista_revisada.ToList(), mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult VerificarEmpleadoUsuarioEncriptacionJson(int EmpleadoId)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            var usuario_encriptacion = new UsuarioEncriptacionEntidad();
            try
            {
                usuario_encriptacion = usuarioEncriptacionModel.UsuarioEncriptacionIDObtenerJson(EmpleadoId);
                if (usuario_encriptacion.Id > 0)
                {
                    respuestaConsulta = true;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult UsuarioEncriptacionRenovarContraseniaJson(int EmpleadoId)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            var UsuarioEncriptacionContraseña = "";
            var usuario_encriptacion = new UsuarioEncriptacionEntidad();
            try
            {
                var random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                UsuarioEncriptacionContraseña = new string(Enumerable.Repeat(chars, 6)
                  .Select(s => s[random.Next(s.Length)]).ToArray());

                usuario_encriptacion.EmpleadoId = EmpleadoId;
                usuario_encriptacion.FechaIni = DateTime.Now;
                usuario_encriptacion.FechaFin = DateTime.Now.AddHours(24);
                usuario_encriptacion.FechaRegistro = DateTime.Now;
                usuario_encriptacion.UsuarioPassword = UsuarioEncriptacionContraseña;
                respuestaConsulta = usuarioEncriptacionModel.UsuarioEncriptacionRenovarContraseniaJson(usuario_encriptacion);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult UsuarioEncriptacionInsertarJson(UsuarioEncriptacionEntidad usuarioencriptacion)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            var UsuarioEncriptacionContraseña = "";
            var usuario_encriptacion = new UsuarioEncriptacionEntidad();
            try
            {
                usuario_encriptacion = usuarioEncriptacionModel.VerificarUsuarioNombreEncriptacionJson(usuarioencriptacion.UsuarioNombre);

                if (usuario_encriptacion.Id > 0)
                {
                    respuestaConsulta = false;
                    errormensaje = "Este nombre de Usuario esta siendo utilizado, intente con otro , Llame Administrador";
                }
                else
                {
                    var random = new Random();
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    UsuarioEncriptacionContraseña = new string(Enumerable.Repeat(chars, 6)
                      .Select(s => s[random.Next(s.Length)]).ToArray());
                    usuarioencriptacion.UsuarioPassword = UsuarioEncriptacionContraseña;
                    usuarioencriptacion.FechaRegistro = DateTime.Now;
                    respuestaConsulta = usuarioEncriptacionModel.UsuarioEncriptacionInsertarJson(usuarioencriptacion);
                    respuestaConsulta = true;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult UsuarioEncriptacionEditarJson(UsuarioEncriptacionEntidad usuarioencriptacion)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            var usuario_encriptacion = new UsuarioEncriptacionEntidad();
            try
            {
                usuario_encriptacion = usuarioEncriptacionModel.VerificarUsuarioNombreEncriptacionJson(usuarioencriptacion.UsuarioNombre);

                if (usuario_encriptacion.Id > 0)
                {
                    if (usuario_encriptacion.Id == usuarioencriptacion.Id)
                    {
                        respuestaConsulta = usuarioEncriptacionModel.UsuarioEncriptacionEditarJson(usuarioencriptacion);
                        respuestaConsulta = true;
                    }
                    else
                    {
                        respuestaConsulta = false;
                        errormensaje = "Este nombre de Usuario esta siendo utilizado, intente con otro , Llame Administrador";
                    }
                }
                else
                {
                    respuestaConsulta = usuarioEncriptacionModel.UsuarioEncriptacionEditarJson(usuarioencriptacion);
                    respuestaConsulta = true;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult TecnicoListarJson()
        {
            var errormensaje = "";
            var lista = new List<TecnicoUsuarioEncriptado>();
            try
            {
                lista = usuarioEncriptacionModel.TecnicoListarJson();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }

        //[HttpPost]
        //public ActionResult ObtenerUsuarioNombreTecnicoIDJson(int TecnicoID)
        //{
        //    var errormensaje = "";
        //    var tecnico = new TecnicoEntidad();
        //    var empleado = new SEG_EmpleadoEntidad();
        //    try
        //    {
        //        tecnico = tecnicobl.TecnicoIdObtenerJson(TecnicoID);
        //        empleado = empleadoModel.EmpleadoIdObtenerJson(tecnico.EmpleadoId);
        //    }
        //    catch (Exception exp)
        //    {
        //        errormensaje = exp.Message + ",Llame Administrador";
        //    }

        //    return Json(new { data = empleado, mensaje = errormensaje });
        //}


    }
}