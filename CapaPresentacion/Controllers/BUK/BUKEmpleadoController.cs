using CapaEntidad.BUK;
using CapaNegocio.BUK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.BUK
{
    [seguridad(false)]
    public class BUKEmpleadoController : Controller
    {
        private readonly BUK_EmpleadoBL _bukEmpleadoBL = new BUK_EmpleadoBL();
        [HttpPost]
        public ActionResult ObtenerEmpleadosActivos(int idempresa) {
            bool success = false;
            List<BUK_EmpleadoEntidad> result = new List<BUK_EmpleadoEntidad>();
            string mensaje;

            try {
                result = _bukEmpleadoBL.ObtenerEmpleadosActivos(idempresa);
                success = result.Count > 0;
                mensaje = success ? "Lista de empleados" : "No se encontraton empleados";
            } catch(Exception exp) {
                mensaje = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = result, mensaje });
        }
        [HttpGet]
        public ActionResult ObtenerEmpleadosActivosPorPatron(int idempresa, string term) {
            bool success = false;
          List<BUK_EmpleadoEntidad> result = new List<BUK_EmpleadoEntidad>();
            string mensaje;

            try {
                result = _bukEmpleadoBL.ObtenerEmpleadosActivosPorTermino(idempresa,term);
                success = result.Count > 0;
                mensaje = success ? "Lista de empleados" : "No se encontraton empleados";
            } catch(Exception exp) {
                mensaje = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = result, mensaje },JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ObtenerEmpleadosActivosPorTerminoSinEmpresa(string term) {
            bool success = false;
            List<BUK_EmpleadoEntidad> result = new List<BUK_EmpleadoEntidad>();
            string mensaje;

            try {
                result = _bukEmpleadoBL.ObtenerEmpleadosActivosPorTerminoSinEmpresa(term);
                success = result.Count > 0;
                mensaje = success ? "Lista de empleados" : "No se encontraton empleados";
            } catch(Exception exp) {
                mensaje = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = result, mensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ObtenerEmpleadosActivosPorTerminoxCargo(string term, int[] idcargo) {
            bool success = false;
            List<BUK_EmpleadoEntidad> result = new List<BUK_EmpleadoEntidad>();
            string mensaje;

            try {
                result = _bukEmpleadoBL.ObtenerEmpleadosActivosPorTerminoxCargo(term, idcargo);
                success = result.Count > 0;
                mensaje = success ? "Lista de empleados" : "No se encontraton empleados";
            } catch(Exception exp) {
                mensaje = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = result, mensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}