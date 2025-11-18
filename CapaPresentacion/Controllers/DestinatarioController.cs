using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Models;
using UAParser;

namespace CapaPresentacion.Controllers
{
    [seguridad]
    public class DestinatarioController : Controller
    {        
        private DestinatarioBL destinatarioBl = new DestinatarioBL();
        private Destinatario_DetalleBL destinatarioDetalleBl = new Destinatario_DetalleBL();

        #region Mantenedor Destinatario
        public ActionResult DestinatarioListadoVista()
        {
            return View("~/Views/Destinatario/DestinatarioListadoVista.cshtml");
        }
        public ActionResult DestinatarioInsertarVista()
        {
            return View("~/Views/Destinatario/DestinatarioInsertarVista.cshtml");
        }
        public ActionResult DestinatarioEditarVista(int id)
        {
            DestinatarioEntidad destinatario = new DestinatarioEntidad();
            string errormensaje = "";
            try
            {
                destinatario = destinatarioBl.DestinatarioObtenerJson(id);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            ViewBag.destinatario = destinatario;
            return View("~/Views/Destinatario/DestinatarioEditarVista.cshtml");
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult DestinatarioListadoJson()
        {
            var errormensaje = "";
            var lista = new List<DestinatarioEntidad>();
            try
            {
                lista = destinatarioBl.DestinatarioListadoJson();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult DestinatarioListadoTipoEmailJson(int tipoEmail)
        {
            var errormensaje = "";
            var lista = new List<DestinatarioEntidad>();
            try
            {
                lista = destinatarioBl.DestinatarioListadoTipoEmailJson(tipoEmail);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult DestinatarioInsertarJson(DestinatarioEntidad destinatario)
        {
            var errormensaje = "";
            var response = false;
            try
            {
                response = destinatarioBl.DestinatarioInsertarJson(destinatario);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = response, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult DestinatarioEditarJson(DestinatarioEntidad destinatario)
        {
            var errormensaje = "";
            var response = false;
            try
            {
                response = destinatarioBl.DestinatarioEditarJson(destinatario);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = response, mensaje = errormensaje });
        }

        #endregion

        #region Configuracion Destinatario
        public ActionResult DestinatarioConfiguracionVista()
        {
            return View("~/Views/Destinatario/DestinatarioConfiguracionVista.cshtml");
        }
        public ActionResult DestinatarioDetalleInsertarJson(int tipoEmail, string DestinatariosId)
        {            
            var errormensaje = "";
            var responseDelete = false;
            var responseInsert = true;
            var response= false;
            var destinatarios = DestinatariosId.Split(',');
            try
            {
                responseDelete = destinatarioDetalleBl.DestinatarioDetalleEliminarJson(tipoEmail);
                var i = 0;
                while (responseInsert==true && i< destinatarios.Count() && destinatarios[0]!="")
                {
                    Destinatario_DetalleEntidad destinatarioDetalle = new Destinatario_DetalleEntidad {
                        EmailID = Convert.ToInt32(destinatarios[i]),
                        TipoEmail= tipoEmail
                    };
                    responseInsert = destinatarioDetalleBl.DestinatarioDetalleInsertarJson(destinatarioDetalle);
                    i++;
                }                
                response = responseDelete && responseInsert;
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = response, mensaje = errormensaje });
        }
        #endregion       
    }
}