using CapaNegocio.Disco;
using CapaNegocio.Discos;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaDatos.Imei;
using CapaNegocio.Imei;
using CapaEntidad.Disco;
using CapaEntidad;
using CapaEntidad.Imei;
using CapaEntidad.Discos;
using CapaNegocio.AsistenciaEmpleado;

namespace CapaPresentacion.Controllers.Imei
{
    [seguridad]
    public class ControlImeiController : Controller
    {
        private readonly ControlImeiBL _controlImeiBl = new ControlImeiBL();
        private EmpleadoDispositivoBL empleadoDispositivobl = new EmpleadoDispositivoBL();

        // GET: ControlImei
        public ActionResult ImeiIndex() {
            return View("~/Views/Imei/ImeiControlVista.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarPendientesImei() {
            var errormensaje = "";
            var lista = new List<ControlImeiEntidad>();

            try {
                lista = _controlImeiBl.ListadoControlImei().ToList();
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }

            return Json(new { data = lista, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RechazarControlImei(int idControlImei) {
            var mensaje = "";
            bool respuesta = true;
            
            try {
                respuesta = _controlImeiBl.RechazarImei(idControlImei);
                mensaje = "Se rechazo correctamente";
            } catch(Exception exp) {
                mensaje = exp.Message + ",Llame Administrador";
            }

            return Json(new { data = respuesta,  mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult AceptarControlImei(string usu_imei,int empleadoId, int idControlImei) {
            bool respuesta=false;
            string mensaje="";

            if(!string.IsNullOrEmpty(usu_imei)) {

                var empleadoDispositivo = empleadoDispositivobl.EmpleadoDispositivoemp_IdObtenerJson(empleadoId);
                if(empleadoDispositivo != null) {
                    empleadoDispositivo.emd_imei = usu_imei;

                    if(empleadoDispositivo.emd_id > 0) {

                        bool respo = empleadoDispositivobl.EmpleadoDispositivoEditarJson(empleadoDispositivo);
                        respuesta= respo;
                        mensaje = "Se realizo el cambio correctamente";
                        _controlImeiBl.AceptarImei(idControlImei);
                    } else {
                        empleadoDispositivo.emp_id = empleadoId;
                        empleadoDispositivo.emd_estado = 1;
                        Int64 id = empleadoDispositivobl.EmpleadoDispositivoInsertarJson(empleadoDispositivo);
                        mensaje = "Se registro correctamente";
                    }
                }

            }

            return Json(new { respuesta, mensaje });
        }

    }
}