using CapaPresentacion.Filters;
using CapaPresentacion.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    [seguridad(false)]
    public class HomeController : Controller
    {

        [seguridad(false)]
        public ActionResult About()
        {
            return View("~/Views/Home/About.cshtml");
        }

        [seguridad(false)]
        public ActionResult Salir()
        {
            ViewBag.uri = ConfigurationManager.AppSettings["ServiceUri"];
            ViewBag.LinkService = ConfigurationManager.AppSettings["LinkServicio"];
            Session["permisos"] = null;
            Session["empleado"] = null;
            Session["Empresa"] = null;
            Session["Sala"] = null;
            Session["UsuarioID"] = null;
            Session["UsuarioNombre"] = null;
            Session["token"] = null;
            Session["usuario"] = null;
            Session.Remove(TokenProgresivoAttribute.KEY_TOKEN_PROGRESIVO);
            Session.Remove(TokenProgresivoAttribute.KEY_SGN_ROOM_ID);
            funciones.BorrarCookie("token");
            funciones.BorrarCookie("codSala");
            funciones.BorrarCookie("datainicial");
            funciones.BorrarCookie("datafinal");
            funciones.BorrarCookie("c_id_sgn");
            return View("~/Views/Home/Login.cshtml");
        }
        // [sinseguridad]
        //  [seguridad]

      //  [seguridad(false)]
        public ActionResult Login()
        {
            ViewBag.uri = ConfigurationManager.AppSettings["ServiceUri"];
            ViewBag.LinkService = ConfigurationManager.AppSettings["LinkServicio"];

            if (Session["UsuarioNombre"] != null)
            {
                ViewBag.Message = "Panel Seguridad";

                return View("~/Views/Home/Index.cshtml");
            }
            else
            {
                ViewBag.Message = "Login De Acceso";

                string ubicacion_archivo = Server.MapPath(@"~/Content/controlador.txt");
                try
                {
                    string primeracargada = System.IO.File.ReadAllText(ubicacion_archivo);
                    if (primeracargada == "1")
                    {
                        var bbb = new SeguridadController().updateMethod();
                        System.IO.File.WriteAllText(ubicacion_archivo, "0");
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }


                return View("~/Views/Home/Login.cshtml");


            }

        }
        [seguridad(false)]
        public ActionResult CambioContrasena()
        {
            return View("~/Views/Home/CambioContrasena.cshtml");
        }

        [seguridad(false)]
        public ActionResult RestablecerContrasena()
        {
            return View("~/Views/Home/RestablecerContrasena.cshtml");
        }

        [seguridad(false)]

        public ActionResult AccesoNegado()
        {
            return View("~/Views/Home/accesonegado.cshtml");
        }


        [seguridad(false)]

        public ActionResult DashBoard()
        {
           
            if (Session["UsuarioNombre"] != null)
            {
                ViewBag.Message = "Panel Seguridad";

                return View("~/Views/Home/Index.cshtml");
            }
            else
            {
                ViewBag.uri = ConfigurationManager.AppSettings["ServiceUri"];
                ViewBag.LinkService = ConfigurationManager.AppSettings["LinkServicio"];
                ViewBag.Message = "Login De Acceso";
                return View("~/Views/Home/Login.cshtml");


            }
        }
    }
}