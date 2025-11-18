using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaEntidad.ProgresivoSeguridad;
using CapaNegocio;
using CapaNegocio.SeguridadProgresivo;
using CapaPresentacion.Models;
using UAParser;

namespace CapaPresentacion.Controllers
{
    [seguridad]
    public class UsuarioController : Controller
    {        
        private SEG_UsuarioBL segUsuarioBl = new SEG_UsuarioBL();
        private SalaBL salaBl = new SalaBL();

        //private WEB_ConfiguracionBL webConfiguracionBL = new WEB_ConfiguracionBL();
        private UsuarioEncriptacionBL usuarioEncriptacionBL = new UsuarioEncriptacionBL();
        private SEG_PermisoRolBL webPermisoRolBL = new SEG_PermisoRolBL();
        private SEG_RolUsuarioBL webRolUsuarioBL = new SEG_RolUsuarioBL();
        private SEG_EmpleadoBL empleadoBL = new SEG_EmpleadoBL();
      
        private SEG_PermisoMenuBL webPermisoMenuBl = new SEG_PermisoMenuBL();
        private SEG_Configuracion_SeguridadBL csBl = new SEG_Configuracion_SeguridadBL();

        private SEG_RolBL webRolBL = new SEG_RolBL();
        private SeguridadProgresivoBL segprogresivoBl = new SeguridadProgresivoBL();
        public ActionResult UsuarioListadoVista()
        {
            return View("~/Views/Usuario/UsuarioListadoVista.cshtml");
        }
        public ActionResult UsuarioInsertarVista()
        {
            return View("~/Views/usuario/UsuarioInsertarVista.cshtml");
        }
        public ActionResult CambiarContraseniaVista()
        {
            return View("~/Views/usuario/CambiarContraseniaVista.cshtml");
        }

        [HttpPost]
        public ActionResult UsuarioCambiarContrasenia(SEG_UsuarioEntidad usuario)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                usuario.FailedAttempts = 0;
                usuario.UsuarioContraseña = PasswordHashTool.PasswordHashManager.CreateHash(usuario.UsuarioContraseña);
                respuestaConsulta = segUsuarioBl.UsuarioCambiarContrasenia(usuario);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        public ActionResult UsuarioRegistroVista(string id)
        {
            int sub = Convert.ToInt32(id.Replace("Registro", ""));
            var errormensaje = "";
            var usuario = new SEG_UsuarioEntidad();
            try
            {
                usuario = segUsuarioBl.UsuarioEmpleadoIDObtenerJson(sub);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }            
            ViewBag.usuario = usuario;
            ViewBag.errormensaje = errormensaje;
            return View("~/Views/Usuario/UsuarioRegistroVista.cshtml");
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult UsuarioListadoJson()
        {
            var errormensaje = "";
            var lista = new List<SEG_UsuarioEntidad>();
            var listaRol = new List<SEG_RolEntidad>();
            var listaRolUsuario = new List<SEG_RolUsuarioEntidad>();
            try
            {
                lista = segUsuarioBl.UsuarioListadoJson();
                listaRol = webRolBL.ListarRol().OrderBy(x => x.WEB_RolNombre).ToList();
                listaRolUsuario = webRolUsuarioBL.ListarRolUsuarios();

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }

            return Json(new { data = lista.ToList(), roles = listaRol, rolUsuarios = listaRolUsuario.ToList(), mensaje = errormensaje });
            //  var aa = lista.ToList();

        }

        [HttpPost]
        public ActionResult UsuarioEmpleadoIDObtenerJson(int usuarioId)
        {
            var errormensaje = "";
            var usuario = new SEG_UsuarioEntidad();
            try
            {
                usuario = segUsuarioBl.UsuarioEmpleadoIDObtenerJson(usuarioId);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = usuario, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult UsuarioCoincidenciaObtenerJson(string usuarioNombre, int usuarioID, int condicion)
        {
            var errormensaje = "";
            var usuario = new SEG_UsuarioEntidad();
            try
            {
                usuario = segUsuarioBl.UsuarioCoincidenciaObtenerJson(usuarioNombre, usuarioID, condicion);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = usuario, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult UsuarioGuardarJson(SEG_UsuarioEntidad usuario)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                var usuarioPass = usuario.UsuarioContraseña;
                usuario.FechaRegistro = DateTime.Now;
                usuario.FailedAttempts = 0;
                usuario.EstadoContrasena = 1;
                usuario.Estado = 1;
                usuario.UsuarioContraseña = PasswordHashTool.PasswordHashManager.CreateHash(usuario.UsuarioContraseña);
                respuestaConsulta = segUsuarioBl.UsuarioGuardarJson(usuario);

                if (respuestaConsulta == true)
                {
                    UsuarioCorreoEnviarJsonAsync(usuario);
                    //Envio Correo
                    var empleado = empleadoBL.EmpleadoIdObtenerJson(usuario.EmpleadoID);
                    var configuracionSeguridad = csBl.ConfiguracionSeguridadObtenerJson();
                    String correoservidor = ConfigurationManager.AppSettings["correo"];
                    String password = ConfigurationManager.AppSettings["password"];
                    var session = Session;
                    string correosdestino = empleado.MailJob;
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    mail.From = new MailAddress(correoservidor);
                    mail.To.Add(correosdestino);
                    mail.Subject = "Creacion de Usuario.";
                    mail.Body = configuracionSeguridad.mensajeEmail +
                                "<h2 style='color: #153643; font-family: Arial, sans-serif; font-size: 20px;'>Usuario: " + usuario.UsuarioNombre + " </h2>" +
                                "<h2 style='color: #153643; font-family: Arial, sans-serif; font-size: 20px;'>Contraseña: " + usuarioPass + " </h2>" +
                                "<h2 style='color: #153643; font-family: Arial, sans-serif; font-size: 20px;'>Link Interno: " + configuracionSeguridad.linkInterno + " </h2>" +
                                "<h2 style='color: #153643; font-family: Arial, sans-serif; font-size: 20px;'>Link Externo: " + configuracionSeguridad.linkExterno + " </h2>";
                    mail.IsBodyHtml = true;
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(correoservidor, password);
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
                }
                else
                {
                    errormensaje = "error al crear el usuario , LLame Administrador";
                    respuestaConsulta = false;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult UsuarioActualizarJson(SEG_UsuarioEntidad usuario)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                //usuario.UsuarioContraseña = PasswordHashTool.PasswordHashManager.CreateHash(usuario.UsuarioContraseña);
                respuestaConsulta = segUsuarioBl.UsuarioActualizarJson(usuario);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ValidacionLogin(string usuLogin, string usuPassword)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            bool OlvidoContrasena = false;
            var usuario = new SEG_UsuarioEntidad();
           
            var permisoRol = new List<SEG_PermisoRolEntidad>();
            var rolUsuario = new SEG_RolUsuarioEntidad();
            var empleado = new SEG_EmpleadoEntidad();

            try
            {
                usuario = segUsuarioBl.UsuarioCoincidenciaObtenerJson(usuLogin, 0, 0);
                if (usuario.UsuarioNombre != null)
                {
                    if (usuario.UsuarioID != 0) {

                        if (usuario.FailedAttempts == 5)
                        {
                            errormensaje = "Usuario DesHabilitado ,Comuniquese con el Administrador";
                            return Json(new { respuesta = respuestaConsulta, CambioContrasena = false, mensaje = errormensaje, OlvidoContrasena = false });
                        }

                        respuestaConsulta = PasswordHashTool.PasswordHashManager.ValidatePassword(usuPassword, usuario.UsuarioContraseña);
                        if (respuestaConsulta == false)
                        {
                            int intento = usuario.FailedAttempts + 1;
                            usuario.FailedAttempts = intento;
                            bool res = segUsuarioBl.UsuarioActualizarJson(usuario);
                            if (intento == 5)
                            {
                                errormensaje = "Contraseña no Coincide, Usuario Deshabilitado (Demasiados intentos Permitidos)";
                            }
                            else
                            {
                                errormensaje = "Contraseña no Coincide, Tienes " + (5 - intento) + " Intento(s) mas";
                                OlvidoContrasena = true;
                            }
                            
                        }
                        else
                        {
                            if (usuario.EstadoContrasena == 1)
                            {
                                errormensaje = usuario.UsuarioNombre;
                                Session["UsuarioIDCambioContrasena"] = usuario.UsuarioID;
                                return Json(new { respuesta = respuestaConsulta, CambioContrasena = true, mensaje = errormensaje, OlvidoContrasena = false });
                            }
                        

                            usuario.FailedAttempts = 0;
                            segUsuarioBl.UsuarioActualizarJson(usuario);

                            rolUsuario = webRolUsuarioBL.GetRolUsuarioId(usuario.UsuarioID);
                            int rol = rolUsuario.WEB_RolID;
                            permisoRol = webPermisoRolBL.GetPermisoRol(rol);
                            empleado = empleadoBL.EmpleadoIdObtenerJson(usuario.EmpleadoID);
                            usuario.NombreEmpleado = empleado.ApellidosPaterno + " " + empleado.ApellidosMaterno + " " + empleado.Nombres;
                            var clientInfo = Parser.GetDefault().Parse(Request.Headers["User-Agent"].ToString());
                            var userAgent = clientInfo.UserAgent.ToString().Replace(":", ".");
                            var ipAddress = Request.GetOwinContext().Request.RemoteIpAddress.Replace(":", ".");
                            var identifier = RandomString.GetString(8);
                            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(identifier + ":" + ipAddress + ":" + userAgent + ":" + RandomString.GetString(8)));

                            Session["token"] = token;

                            HttpCookie myCookie = new HttpCookie("token");
                            DateTime now = DateTime.Now;
                            myCookie.Value = token;
                            myCookie.Expires = now.AddYears(50);
                            Response.Cookies.Add(myCookie);

                            Signalr_usuarioEntidad sgn_registro = new Signalr_usuarioEntidad();
                            sgn_registro.usuario_id = Convert.ToInt32(usuario.UsuarioID);
                            sgn_registro.sgn_conection_id = "";
                            sgn_registro.sgn_fechaUpdate = DateTime.Now;
                            Int64 sgn_id = segprogresivoBl.GuardarSignalr_returnID(sgn_registro);

                            HttpCookie myCookieUsuario = new HttpCookie("c_id_sgn");
                            DateTime nowc = DateTime.Now;
                            myCookieUsuario.Value = Convert.ToString(sgn_id);
                            myCookieUsuario.Expires = nowc.AddYears(50);
                            Response.Cookies.Add(myCookieUsuario);


                            Session["rol"] = rol;
                            Session["usuario"] = usuario;
                            Session["UsuarioID"] = usuario.UsuarioID;
                            Session["UsuarioNombre"] = usuario.UsuarioNombre;
                    
                            Session["permisos"] = permisoRol;
                            Session["empleado"] = empleado;
                            errormensaje = "Bienvenido, " + usuario.UsuarioNombre;
                            segUsuarioBl.clrEnabled();
                        }
                    }
                    else
                    {
                        errormensaje = usuario.UsuarioNombre;
                    }
                }
                else
                {
                    respuestaConsulta = false;
                    errormensaje = "Usuario No Encontrado";
                }

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, CambioContrasena = false, mensaje = errormensaje, OlvidoContrasena = OlvidoContrasena });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListadoMenus()
        {
            var errormensaje = "";
            var resultado = new List<dynamic>();
            var listaxMenuPrincipal = new List<SEG_PermisoMenuEntidad>();
            try
            {

                listaxMenuPrincipal = webPermisoMenuBl.GetPermisoMenuRolId(Convert.ToInt32(Session["rol"]));

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Comuniquese con el Administrador";
            }


            return Json(new { dataResultado = listaxMenuPrincipal.ToList(), mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult UsuarioGenerarJson(int EmpleadoID)
        {
            var errormensaje = "";
            var usuario = new SEG_UsuarioEntidad();
            var configuracionSeguridad = new SEG_Configuracion_SeguridadEntidad();
            var empleado = new SEG_EmpleadoEntidad();
            var UsuarioNombre = "";
            var UsuarioContraseña = "";
            try
            {
                configuracionSeguridad = csBl.ConfiguracionSeguridadObtenerJson();
                empleado = empleadoBL.EmpleadoIdObtenerJson(EmpleadoID);
                List<EmpleadoConfiguracionSeguridad> empleadoConfiguracionSeguridad = new List<EmpleadoConfiguracionSeguridad>();
                empleadoConfiguracionSeguridad.Add(new EmpleadoConfiguracionSeguridad() { orden = configuracionSeguridad.ordenNombre, cantidadLetras = configuracionSeguridad.cantidadLetraNombre, valor = Regex.Replace(empleado.Nombres, @"\s+", "") });
                empleadoConfiguracionSeguridad.Add(new EmpleadoConfiguracionSeguridad() { orden = configuracionSeguridad.ordenApePaterno, cantidadLetras = configuracionSeguridad.cantidadLetraApePaterno, valor = empleado.ApellidosPaterno });
                empleadoConfiguracionSeguridad.Add(new EmpleadoConfiguracionSeguridad() { orden = configuracionSeguridad.ordenApeMaterno, cantidadLetras = configuracionSeguridad.cantidadLetraApeMaterno, valor = empleado.ApellidosMaterno });
                empleadoConfiguracionSeguridad.Add(new EmpleadoConfiguracionSeguridad() { orden = configuracionSeguridad.ordenDNI, cantidadLetras = configuracionSeguridad.cantidadLetraDNI, valor = empleado.DOI });
                empleadoConfiguracionSeguridad = empleadoConfiguracionSeguridad.OrderBy(o => o.orden).ToList();
                foreach (var item in empleadoConfiguracionSeguridad)
                {
                    var itemString = "";
                    if (item.valor.Length <= item.cantidadLetras)
                    {
                        itemString = item.valor;
                    }
                    else
                    {
                        itemString = item.valor.Substring(0, item.cantidadLetras);
                    }
                    UsuarioNombre += itemString;
                }
                var random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                UsuarioContraseña = new string(Enumerable.Repeat(chars, 6)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }
            return Json(new { UsuarioNombre = UsuarioNombre, UsuarioContraseña = UsuarioContraseña, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult CambiarContrasena(string usuPassword)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = segUsuarioBl.CambiarContrasena(PasswordHashTool.PasswordHashManager.CreateHash(usuPassword), Convert.ToInt32(Session["UsuarioIDCambioContrasena"]));
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult RestablecerContrasena(string cuenta, string email)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                var usuario = segUsuarioBl.UsuarioCoincidenciaObtenerJson(cuenta, 0, 0);
                if (usuario.UsuarioNombre != null)
                {
                    var empleado = empleadoBL.EmpleadoIdObtenerJson(usuario.EmpleadoID);
                    if (empleado.MailJob == email)
                    {
                        var random = new Random();
                        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                        var UsuarioContraseña = new string(Enumerable.Repeat(chars, 6)
                          .Select(s => s[random.Next(s.Length)]).ToArray());
                        var usuarioPass = UsuarioContraseña;
                        UsuarioContraseña = PasswordHashTool.PasswordHashManager.CreateHash(UsuarioContraseña);
                        respuestaConsulta = segUsuarioBl.RestablecerContrasena(UsuarioContraseña, usuario.UsuarioID);

                        //Envio Correo
                        var configuracionSeguridad = csBl.ConfiguracionSeguridadObtenerJson();
                        String correoservidor = ConfigurationManager.AppSettings["correo"];
                        String password = ConfigurationManager.AppSettings["password"];
                        var session = Session;
                        string correosdestino = empleado.MailJob;
                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                        mail.From = new MailAddress(correoservidor);
                        mail.To.Add(correosdestino);
                        mail.Subject = "Restalecer Contraseña.";
                        mail.Body = "<h2 style='color: #153643; font-family: Arial, sans-serif; font-size: 20px;'> Se restableció la contraseña.</h2>" +
                                    "<h2 style='color: #153643; font-family: Arial, sans-serif; font-size: 20px;'>Usuario: " + usuario.UsuarioNombre + " </h2>" +
                                    "<h2 style='color: #153643; font-family: Arial, sans-serif; font-size: 20px;'>Contraseña: " + usuarioPass + " </h2>" +
                                    "<h2 style='color: #153643; font-family: Arial, sans-serif; font-size: 20px;'>Link Interno: " + configuracionSeguridad.linkInterno + " </h2>" +
                                    "<h2 style='color: #153643; font-family: Arial, sans-serif; font-size: 20px;'>Link Externo: " + configuracionSeguridad.linkExterno + " </h2>";
                        mail.IsBodyHtml = true;
                        SmtpServer.Port = 587;
                        SmtpServer.Credentials = new System.Net.NetworkCredential(correoservidor, password);
                        SmtpServer.EnableSsl = true;
                        SmtpServer.Send(mail);
                    }
                    else
                    {
                        errormensaje = "El Correo Electronico No Coincide.";
                        return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
                    }
                }
                else
                {
                    errormensaje = "Usuario No Encontrado.";
                    return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
       
        [HttpPost]
        public ActionResult UsuarioBloquearJson(int UsuarioID)
        {
            var respuestaConsulta = segUsuarioBl.UsuarioBloquearJson(new SEG_UsuarioEntidad { UsuarioID = UsuarioID });
            return Json(respuestaConsulta);
        }
     
        [HttpPost]
        public ActionResult UsuarioDesbloquearJson(int UsuarioID)
        {
            var respuestaConsulta = segUsuarioBl.UsuarioDesbloquearJson(new SEG_UsuarioEntidad { UsuarioID = UsuarioID });
            return Json(respuestaConsulta);
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult UsuarioActualizarIdJason(int UsuarioID, string UsuarioNombre)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = segUsuarioBl.UsuarioActualizarUsuarioNombreJson(UsuarioID, UsuarioNombre);
                if (respuestaConsulta == true)
                {
                    //Envio Correo
                    var usuario = segUsuarioBl.UsuarioObtenerEmpleadoUsuarioIdJson(UsuarioID);
                    String correoservidor = ConfigurationManager.AppSettings["correo"];
                    String password = ConfigurationManager.AppSettings["password"];
                    string correosdestino = usuario.MailJob;
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    mail.From = new MailAddress(correoservidor);
                    mail.To.Add(correosdestino);
                    mail.Subject = "Cambio Nombre de Usuario.";
                    mail.Body = "Su nombre de usuario fue cambiado." +
                                "<h2 style='color: #153643; font-family: Arial, sans-serif; font-size: 20px;'>Usuario: " + usuario.UsuarioNombre + " </h2>";
                    mail.IsBodyHtml = true;
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(correoservidor, password);
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
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
        public async System.Threading.Tasks.Task<ActionResult> UsuarioCorreoEnviarJsonAsync(SEG_UsuarioEntidad usuario)
        {
            var response = false;
            string error = "";
            try
            {
                var empleado = empleadoBL.EmpleadoIdObtenerJson(usuario.EmpleadoID);
                //Envio Correo
                String correoservidor = ConfigurationManager.AppSettings["correo"];
                String password = ConfigurationManager.AppSettings["password"];
                var correosdestino = ConfigurationManager.AppSettings["destinatarios"]; 
                //string correosdestino = "vh.vega@software3000.net";
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(correoservidor);
                mail.To.Add(correosdestino);
                mail.Subject = "Creación de Usuario";

                var UsuarioNombre = System.Web.HttpContext.Current.Session["UsuarioNombre"].ToString();
                SEG_EmpleadoEntidad EmpleadoEntidad = (SEG_EmpleadoEntidad)Session["empleado"];
                var EmpleadoNombre = EmpleadoEntidad.Nombres + " " + EmpleadoEntidad.ApellidosPaterno + " " + EmpleadoEntidad.ApellidosMaterno;
                var empleadoCreadoNombre = empleado.Nombres + " " + empleado.ApellidosPaterno + " " + empleado.ApellidosPaterno;
                var contenido = @"<div>Nuevo Usuario </div>" +
                     " <table width= 50% bgcolor= #f6f8f1 border=0 cellpadding=0 cellspacing=0>   " +

                     "<tr> " +
                     "<tr> " +
                     " <td style= 'background: #69c3c3'; 'font - size: 24'>   </td> " +
                     " <td style= 'background: #69c3c3'; 'font - size: 24'> USUARIO LOGUEADO  </td>" +
                     " <td style= 'background: #69c3c3'; 'font - size: 24'> USUARIO CREADO </td>" +
                     " </tr>  " +
                     " <td style= 'background: #69c3c3'; 'font - size: 24'> Empleado </td> " +
                     " <td style= 'background: #edbff'; 'font - size: 24'> " + EmpleadoNombre + " </td> " +
                     " <td style= 'background: #ecc9c9'; 'font - size: 24'> " + empleadoCreadoNombre + " </td> " +
                     " </tr>  " + " " +
                    " </tr>  " +
                     " <td style= 'background: #69c3c3'; 'font - size: 24'> Usuario </td> " +
                     " <td style= 'background: #edbff'; 'font - size: 24'> " + UsuarioNombre + " </td> " +
                     " <td style= 'background: #ecc9c9'; 'font - size: 24'> " + usuario.UsuarioNombre + " </td> " +
                     " </tr>  " +
                     " </table> " +
                     "<div><br> </br> <font size = 2> <font color = #0f243e >Atte.</font><br><font color= #0f243e>" +
                                " Adm. Sys IAS.</font><p>" +
                                "</font></div> <div><font size= 2 ><font color= #0f243e >no responder a este mensaje, esta cuenta no es monitoreada.</font></font></div> ";
                 
                mail.Body = contenido;
                mail.IsBodyHtml = true;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(correoservidor, password);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                response = true;
                try
                {
                    await Task.Run(async () =>
                    {
                        SmtpServer.SendCompleted += (s, e) => {
                            SmtpServer.Dispose();
                            mail.Dispose();
                        };
                        SmtpServer.SendAsync(mail, null);
                    });
                    return Json(new { data = response, error = error });
                }
                catch (Exception ex)
                {
                    var targetSite = ex.TargetSite.Name + ex.TargetSite.Module;
                    var source = ex.Source.ToString();
                    var message = ex.Message.ToString();
                    throw;
                } 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                error = "servidor no disponible";
            } 
            return Json(new { data = response, error = error });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ValidacionLoginPrograma(string Usuario, string Password)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            var usuario = new UsuarioEncriptacionEntidad();
            try
            {
                usuario = usuarioEncriptacionBL.UsuarioCoincidenciaJsonPrograma(Usuario);
                if (usuario.UsuarioNombre != null)
                {
                    if (usuario.Id != 0)
                    {
                        //respuestaConsulta = PasswordHashTool.PasswordHashManager.ValidatePassword(Password, usuario.UsuarioPassword);
                        respuestaConsulta = Password == usuario.UsuarioPassword ? true : false;
                        if (respuestaConsulta == false)
                        {
                            errormensaje = "Contraseña no Coincide";

                        }
                        else
                        {

                            if (DateTime.Now >= usuario.FechaIni && DateTime.Now <= usuario.FechaFin)
                            {
                                errormensaje = usuario.Id.ToString();// "Bienvenido, " + usuario.UsuarioNombre;
                            }
                            else
                            {
                                var UsuarioEncriptacionContraseña = "";
                                var random = new Random();
                                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                                UsuarioEncriptacionContraseña = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
                                usuario.FechaIni = DateTime.Now;
                                usuario.FechaFin = DateTime.Now.AddHours(24);
                                usuario.FechaRegistro = DateTime.Now;
                                usuario.UsuarioPassword = UsuarioEncriptacionContraseña;
                                usuarioEncriptacionBL.UsuarioEncriptacionRenovarContraseniaJson(usuario);///UPDATE [dbo].[UsuarioEncriptacion]
                                respuestaConsulta = false;
                                errormensaje = "Contraseña Incorrecta";
                            }
                        }
                    }
                    else
                    {
                        errormensaje = usuario.UsuarioNombre;///Exception message
                    }
                }
                else
                {
                    respuestaConsulta = false;
                    errormensaje = "Usuario No Encontrado";
                }

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [seguridad(false)]
        public ActionResult InsertarHistoriaPrograma(int UsuarioEncriptacionID, int TipoAcceso)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            bool insercion = false;
            try
            {
                insercion = usuarioEncriptacionBL.UsuarioEncriptacionHistorialInsertar(UsuarioEncriptacionID, TipoAcceso);

            }
            catch (Exception exp)
            {
                insercion = false;
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = insercion, mensaje = errormensaje });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult UsuarioEditarTokenAccesoIntranetJson(List<SEG_UsuarioEntidad> listaTokens)
        {
            bool response = false;
            string respuestaEdicion = "";
            string errormensaje = "";
            List<dynamic> listaDevuelta = new List<dynamic>();
            SEG_UsuarioEntidad usuario = new SEG_UsuarioEntidad();
            try
            {
                if (listaTokens.Count > 0)
                {
                    foreach (var m in listaTokens)
                    {
                        usuario.UsuarioID = m.UsuarioID;
                        usuario.UsuarioToken = m.UsuarioToken;
                        var respuestaEdicionTupla = segUsuarioBl.UsuarioEditarTokenAccesoIntranetJson(usuario);
                        if (respuestaEdicionTupla.error.Key.Equals(string.Empty))
                        {
                            respuestaEdicion = respuestaEdicionTupla.respuesta;
                            listaDevuelta.Add(new
                            {
                                UsuarioID = m.UsuarioID,
                                TokenAntiguo = m.UsuarioToken,
                                TokenNuevo = respuestaEdicion,
                                Sistema = "IAS"
                            });
                        }
                    }
                    errormensaje = "Tokens Editados, Mostrando Resultados";
                }
                else
                {
                    errormensaje = "Se ha enviado una lista Vacia";
                }
                response = true;
            }
            catch (Exception ex)
            {
                errormensaje = ex.Message;
            }
            return Json(new { respuesta = response, mensaje = errormensaje, listaDevuelta = listaDevuelta.ToList() });
        }


        [seguridad(false)]
        [HttpPost]
        public ActionResult UsuarioListadoxSalaJson() {
            var errormensaje = "";
            var lista = new List<SEG_UsuarioEntidad>();
            var listaRol = new List<SEG_RolEntidad>();
            var listaRolUsuario = new List<SEG_RolUsuarioEntidad>();
            var listaSala = new List<SalaEntidad>();

            var usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            listaSala = salaBl.ListadoSalaPorUsuario(usuarioId);

            string querySalas = "";
            bool primero = true;

            foreach(var item in listaSala) {
                if(primero) {
                    querySalas = querySalas + item.CodSala.ToString();
                    primero = false;
                } else {
                    querySalas = querySalas + "," + item.CodSala.ToString();
                }
            }

            try {
                lista = segUsuarioBl.UsuarioListadoxSalasJson(querySalas);
                listaRol = webRolBL.ListarRol().OrderBy(x => x.WEB_RolNombre).ToList();
                listaRolUsuario = webRolUsuarioBL.ListarRolUsuarios();

            } catch(Exception exp) {
                errormensaje = exp.Message + ", Llame Administrador";
            }

            return Json(new { data = lista.ToList(), roles = listaRol, rolUsuarios = listaRolUsuario.ToList(), mensaje = errormensaje });
            //  var aa = lista.ToList();

        }

    }
}