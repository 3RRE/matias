using CapaEntidad.ControlAcceso;
using CapaNegocio.ControlAcceso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.ControlAcceso
{
    public class CALContactoController : Controller
    {
        private CAL_ContactoBL contactoBL = new CAL_ContactoBL();

        [HttpPost]
        public JsonResult InsertarContacto(CAL_ContactoEntidad contacto)
        {
            string errormensaje = "No se pudo insertar el registro";
            int idinsertado = 0;
            bool respuesta = false;
            try
            {

                idinsertado= contactoBL.InsertarContacto(contacto);
                if (idinsertado > 0)
                {
                    errormensaje = "Registro insertado";
                    respuesta = true;
                }
                else
                {
                    errormensaje = "No se pudo insertar el registro";
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { mensaje = errormensaje,respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateContacto(CAL_ContactoEntidad contacto)
        {
            string errormensaje = "No se pudo editar el registro";
            bool respuesta = false;
            try
            {

                respuesta = contactoBL.UpdateContacto(contacto);
                errormensaje = "Registro editado";

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { mensaje = errormensaje,respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateContactoInLudopatas(int ContactoID,int LudopataID)
        {
            string errormensaje = "No se pudo editar el registro";
            bool respuesta = false;
            try
            {

                respuesta = contactoBL.UpdateContactoInLudopatas(ContactoID, LudopataID);
                errormensaje = "Registro editado";

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { mensaje = errormensaje,respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetContactoByID(int ContactoID)
        {
            string errormensaje = "No se pudo obtener el registro";
            CAL_ContactoEntidad contacto = new CAL_ContactoEntidad();
            bool respuesta = false;
            try
            {

                contacto = contactoBL.GetContactoByID(ContactoID);
                respuesta = true;
                errormensaje = "Obteniendo registro";
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { mensaje = errormensaje,respuesta,data=contacto }, JsonRequestBehavior.AllowGet);
        }
    }
}