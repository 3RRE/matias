using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CapaPresentacion
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "ReclamacionesSala",
                url: "ReclamacionesSala",
                defaults: new { controller = "Reclamacion", action = "ReclamacionInicioVista" }
            );

            routes.MapRoute(
                name: "ReclamacionesNuevo",
                url: "ReclamacionesNuevo",
                defaults: new { controller = "Reclamacion", action = "ReclamacionNuevoVista" , id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ReclamacionDetalle",
                url: "ReclamacionDetalle/{doc}",
                defaults: new { controller = "Reclamacion", action = "ReclamacionDetalle", doc = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ReclamacionDetalleRespuesta",
                url: "ReclamacionDetalleRespuesta/{doc}",
                defaults: new { controller = "Reclamacion", action = "ReclamacionDetalleRespuesta", doc = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "Documento",
              url: "Documento/{doc}",
              defaults: new { controller = "Reclamacion", action = "reclamacionDoc", doc = UrlParameter.Optional }
            );

            routes.MapRoute(
                 name: "DocumentoRespuestaLegal",
                 url: "DocumentoRespuestaLegal/{doc}",
                 defaults: new { controller = "Reclamacion", action = "reclamacionDocRespuestaLegal", doc = UrlParameter.Optional }
             );

            routes.MapRoute(
                name: "HistoricoMaquinaInoperativa",
                url: "MIMaquinaInoperativa/HistoricoMaquinaInoperativa/{id}/{id2}",
                defaults: new { controller = "MIMaquinaInoperativa", action = "HistoricoMaquinaInoperativa", id = UrlParameter.Optional, id2 = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "DetalleHistoricoMaquinaInoperativa",
                url: "MaquinasInoperativasV2/DetalleHistoricoMaquinaInoperativa/{id}/{id2}",
                defaults: new { controller = "MaquinasInoperativasV2", action = "DetalleHistoricoMaquinaInoperativa", id = UrlParameter.Optional, id2 = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Login", id = UrlParameter.Optional }
            );



        }
    }
}
