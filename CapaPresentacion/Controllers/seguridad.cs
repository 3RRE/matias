using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Models;
using CapaPresentacion.Utilitarios;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CapaPresentacion.Controllers
{
    public class seguridad: System.Web.Mvc.AuthorizeAttribute
    {
        public seguridad(bool activa = true)///activa  parámetro que se envia desde el atributo del método =>   [seguridad(activa)]      true por defecto
        {
            activar = activa;
        }
        private bool defaultactivar = true;
        public bool activar
        {
            get
            {
                return defaultactivar;
            }
            set
            {
                defaultactivar = value;
            }
        }
        public string Controlador { get; set; }
        public string Metodo { get; set; }
        public string pos { get; set; }

        #region MÉTODOS SEGURIDAD 
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            var id = (httpContext.Request.RequestContext.RouteData.Values["pos"] as string)
              ??
              (httpContext.Request["pos"] as string);
            if (id == "1")
            {
                return base.AuthorizeCore(httpContext);
            }
            bool authorize = false;
            String Controlador = this.Controlador;
            String Metodo = this.Metodo;

            /*
            if(HttpContext.Current.Session["ROOT_SUPERUSERNAME"] != null)
            {
                return authorize;
            }*/

            if (HttpContext.Current.Session["UsuarioNombre"] != null)
            {
                var UsuarioID = HttpContext.Current.Session["UsuarioID"];
                if (Controlador != null)
                {
                    String busqueda = "";
                    busqueda = funciones.consulta("PermisoUsuario", @"SELECT [WEB_PRolID]
                                                                ,[WEB_PermID]
                                                                ,[WEB_RolID]
                                                                ,[WEB_PRolFechaRegistro]
                                                                FROM [dbo].[WEB_PermisoRol] where WEB_RolID =" + HttpContext.Current.Session["rol"]);
                    if (busqueda.Length < 3)
                    {
                        authorize = false;
                    }
                    else
                    {
                        authorize = true;
                    }
                }
                else { authorize = true; }
            }
            return authorize;
        } 
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var attributosseguridad = filterContext.ActionDescriptor.GetCustomAttributes(typeof(seguridad), true);
            string control = filterContext.Controller.ValueProvider.GetValue("Controller").AttemptedValue;
            string accion = filterContext.Controller.ValueProvider.GetValue("action").AttemptedValue;
            Metodo_atributos metodoObjeto = new SeguridadController().Metodo_Objeto(control, accion);

            if (metodoObjeto.seguridad == false) { return; } ////si seguridad  final del mètodo es false ;  dejar que avance al método



            if (filterContext.HttpContext.Session["UsuarioNombre"] == null)
            {
                if (!filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    var myCookie = new HttpCookie("urlredireccion");
                    DateTime now = DateTime.Now;
                    myCookie.Value = filterContext.HttpContext.Request.Url.LocalPath;// System.Web.HttpContext.Current.Session["Empresa"].ToString();
                    myCookie.Expires = now.AddYears(50);
                    filterContext.HttpContext.Response.Cookies.Add(myCookie);
                }
                HandleUnauthorizedRequest(filterContext);
                //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Login" }));
                return;
            }


            //SEG_AUDITORIA auditoriaobjeto = new SEG_AUDITORIA();
            //auditoriaobjeto.fechaRegistro = DateTime.Now;
            //auditoriaobjeto.usuario = filterContext.HttpContext.Session["UsuarioNombre"].ToString();
            //auditoriaobjeto.proceso = control + "/" + accion;
            //auditoriaobjeto.descripcion = metodoObjeto.descripcion;
            //auditoriaobjeto.subsistema = metodoObjeto.modulo;
            //auditoriaobjeto.usuariodata = Newtonsoft.Json.JsonConvert.SerializeObject(filterContext.HttpContext.Session["usuario"]); ;

            //HttpCookie codSala_cookie = filterContext.HttpContext.Request.Cookies["codSala"];
            //if (codSala_cookie != null)
            //{
            //    if (codSala_cookie.Value!="")
            //    { 
            //        auditoriaobjeto.codSala = Convert.ToInt32(codSala_cookie.Value);
            //    }
            //}

            //string datainicial = "";
            //string datafinal = "";
            //if (filterContext.HttpContext.Request.Cookies["datainicial"] != null)
            //{
            //    datainicial = Newtonsoft.Json.JsonConvert.SerializeObject(filterContext.HttpContext.Request.Cookies["datainicial"].Value);
            //}
            //if (filterContext.HttpContext.Request.Cookies["datafinal"] != null)
            //{
            //    datafinal = Newtonsoft.Json.JsonConvert.SerializeObject(filterContext.HttpContext.Request.Cookies["datafinal"].Value);
            //}

            //auditoriaobjeto.datainicial = datainicial;
            //auditoriaobjeto.datafinal = datafinal;
            //auditoriaobjeto.ip = SeguridadController.GetIPAddress();

            //SEG_AUDITORIABL auditoriabl1 = new SEG_AUDITORIABL();
            //bool guardoauditoria = auditoriabl1.Guardar(auditoriaobjeto);

            //if (codSala_cookie != null)
            //{
            //    filterContext.HttpContext.Response.Cookies.Remove("codSala");
            //    codSala_cookie.Expires = DateTime.Now.AddDays(-10);
            //    codSala_cookie.Value = null;
            //    filterContext.HttpContext.Response.SetCookie(codSala_cookie);
            //}

            //    HttpCookie currentUserCookie = filterContext.HttpContext.Request.Cookies["datainicial"];
            //if (currentUserCookie != null)
            //{
            //    filterContext.HttpContext.Response.Cookies.Remove("datainicial");
            //    currentUserCookie.Expires = DateTime.Now.AddDays(-10);
            //    currentUserCookie.Value = null;
            //    filterContext.HttpContext.Response.SetCookie(currentUserCookie);
            //}
            //currentUserCookie = filterContext.HttpContext.Request.Cookies["datafinal"];
            //if (currentUserCookie != null)
            //{
            //    filterContext.HttpContext.Response.Cookies.Remove("datafinal");
            //    currentUserCookie.Expires = DateTime.Now.AddDays(-10);
            //    currentUserCookie.Value = null;
            //    filterContext.HttpContext.Response.SetCookie(currentUserCookie);
            //}


            HttpCookie token = filterContext.HttpContext.Request.Cookies["token"];

            var tokensesion = filterContext.HttpContext.Session["token"];
            if (token != null && tokensesion != null && tokensesion.ToString() != token.Value)
            {

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Login" }));

                return;
            }

            bool authorize = false;
            String Controlador = this.Controlador;
            String Metodo = this.Metodo;

            if (Controlador == "" || Controlador == null) { Controlador = control; }

            Regex rgx = new Regex(@"Panel");
            if (Metodo == "") { if ((accion == "Panel" || accion == "Detalle") && rgx.IsMatch(accion)) { Metodo = "Listado"; } }

            if (HttpContext.Current.Session["UsuarioNombre"] != null)
            {
                var UsuarioID = HttpContext.Current.Session["UsuarioID"];
                if (Controlador != null)
                {
                    string nombreCompletoControlador = filterContext.Controller.GetType().Name;
                    string query = $@"
                        SELECT [WEB_PRolID], [WEB_RolID], [WEB_PRolFechaRegistro]
                        FROM [dbo].[SEG_PermisoRol]
                        LEFT JOIN [SEG_Permiso] ON [SEG_Permiso].[WEB_PermID] = [SEG_PermisoRol].[WEB_PermID]
                        WHERE [SEG_PermisoRol].WEB_RolID = {HttpContext.Current.Session["rol"]} AND [SEG_Permiso].[WEB_PermNombre]='{accion}' AND [SEG_Permiso].[WEB_PermControlador]='{nombreCompletoControlador}'
                    ";

                    String busqueda = funciones.consulta("PermisoUsuario", query);

                    if (busqueda.Length < 3)
                    {
                        authorize = false;
                    }
                    else
                    {
                        authorize = true;
                    }
                    //  authorize = true;

                }
                else { authorize = true; }
            }
            else
            {
            }
            string estatus = "";
            if (!authorize)
            {
                try
                {
                    HandleUnauthorizedRequest(filterContext);
                }
                catch (Exception ex)
                {
                    estatus = ex.Message;
                }
            }
            else { return; }

        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                if (HttpContext.Current.Session["UsuarioNombre"] != null)
                {
                    var httpContext = context.HttpContext;
                    var request = httpContext.Request;
                    var response = httpContext.Response;
                    var user = httpContext.User;
                    response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    response.SuppressFormsAuthenticationRedirect = true;
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "AccesoNegado" }));
                    response.End();
                    base.HandleUnauthorizedRequest(context);

                }
                else
                {
                    var httpContext = context.HttpContext;
                    var request = httpContext.Request;
                    /* var response = httpContext.Response;
                     var user = httpContext.User;
                     response.StatusCode = 403;
                     response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;

                     response.SuppressFormsAuthenticationRedirect = true;
                     response.End();*/

                    /*context.Result = new JsonResult
                    {
                        Data = "_Logon_",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };*/
                    //context.HttpContext.Response.Clear();

                    context.HttpContext.Response.StatusCode = (int)561;
                    context.HttpContext.Response.SuppressFormsAuthenticationRedirect = false;
                    context.HttpContext.Response.End();
                    base.HandleUnauthorizedRequest(context);
                }

            }
            else
            {
                if (HttpContext.Current.Session["UsuarioNombre"] != null)
                {
                    ErrorCodeUtil errorCodeUtil = new ErrorCodeUtil();
                    var errorCode = "403";
                    var httpContext = context.HttpContext;
                    var viewData = context.Controller.ViewData;
                    List<string> errorData = errorCodeUtil.setPageValues(httpContext, errorCode);                    
                    context.Result = new ViewResult
                    {
                        ViewName = "~/Views/Shared/MensajeCodigo.cshtml",
                        ViewData = viewData
                    };
                    context.Controller.ViewBag.ErrorData = errorData;
                }
                else
                {
                    var httpContext = context.HttpContext;
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Login" }));
                }
            }
        } 
        #endregion

    }
}