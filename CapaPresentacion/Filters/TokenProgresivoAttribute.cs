using CapaEntidad;
using CapaEntidad.ProgresivoSeguridad;
using CapaNegocio;
using CapaNegocio.SeguridadProgresivo;
using CapaPresentacion.Utilitarios;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Filters
{
    public class TokenProgresivoAttribute : ActionFilterAttribute
    {
        private bool Authorized { get; set; }
        private string Token { get; set; }
        private string ID_SGN { get; set; }
        public readonly static string KEY_TOKEN_PROGRESIVO = "KEY_TOKEN_PROGRESIVO";
        public readonly static string C_ID_SGN_PROGRESIVO = "c_id_sgn";
        public readonly static string KEY_SGN_ROOM_ID = "KEY_SGN_ROOM_ID";

        private SEG_PermisoRolBL seg_PermisoRolBL = new SEG_PermisoRolBL();

        public TokenProgresivoAttribute(bool authorized = true)
        {
            Authorized = authorized;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (Authorized)
            {
                string accion = "ProgresivoAutorizado";
                int sessionRolId = (int)filterContext.HttpContext.Session["rol"];
                List<SEG_PermisoRolEntidad> permiso = seg_PermisoRolBL.GetPermisoRolUsuario(sessionRolId, accion);

                Token = filterContext.HttpContext.Session[KEY_TOKEN_PROGRESIVO] as string;
                ID_SGN = filterContext.HttpContext.Request.Cookies[C_ID_SGN_PROGRESIVO].Value;
                bool authTokenValid = IsRequestTokenValid();

                if (permiso.Count > 0)
                {
                    authTokenValid = true;
                }

                if (!authTokenValid)
                {
                    ErrorCodeUtil errorCodeUtil = new ErrorCodeUtil();
                    var errorCode = "401";
                    var httpContext = filterContext.HttpContext;
                    var viewData = filterContext.Controller.ViewData;
                    List<string> errorData = errorCodeUtil.setPageValues(httpContext, errorCode);
                    filterContext.Result = new ViewResult
                    {
                        ViewName = "~/Views/Shared/MensajeCodigo.cshtml",
                        ViewData = viewData
                    };
                    filterContext.Controller.ViewBag.ErrorData = errorData;
                }
            }
        }

        private bool IsRequestTokenValid()
        {
            bool response = false;

            string sgn_id = ID_SGN;
            SeguridadProgresivoBL segprogresivoBl = new SeguridadProgresivoBL();
            Signalr_usuarioEntidad sgn_registro = segprogresivoBl.GetSignalr_usuarioId(Convert.ToInt64(sgn_id));
            string tokenUsuario = sgn_registro.sgn_token;

            if (!string.IsNullOrEmpty(tokenUsuario) && tokenUsuario.Equals(Token))
            {
                response = true;
            }

            // free is true
            // response = true;

            return response;
        }
    }
}