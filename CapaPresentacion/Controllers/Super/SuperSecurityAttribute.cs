using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CapaPresentacion.Controllers.Super
{
    public class SuperSecurityAttribute : ActionFilterAttribute
    {
        public bool authorized { get; set; }

        public SuperSecurityAttribute(bool authorized = true)
        {
            this.authorized = authorized;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (authorized)
            {
                if (string.IsNullOrEmpty(filterContext.HttpContext.Session["ROOT_SUPERUSERNAME"] as string))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{
                        { "controller", "Super" },
                        { "action", "Login" }
                    });
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(filterContext.HttpContext.Session["ROOT_SUPERUSERNAME"] as string))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{
                        { "controller", "Super" },
                        { "action", "control" }
                    });
                }
            }
        }
    }
}