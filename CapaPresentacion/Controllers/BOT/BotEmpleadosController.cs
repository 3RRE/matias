using CapaEntidad.BOT.Entities;
using CapaNegocio;
using CapaNegocio.Cortesias;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.BOT {
    [seguridad(false)]
    public class BotEmpleadosController : BaseController {
        private readonly BOT_EmpleadoBL empleadoBL;
        private readonly BOT_CargoBL cargoBL;
        private readonly EmpresaBL empresaBL;

        public BotEmpleadosController() {
            empleadoBL = new BOT_EmpleadoBL();
            cargoBL = new BOT_CargoBL();
            empresaBL = new EmpresaBL();
        }

        #region Views
        public ActionResult Index() {
            return View("~/Views/Bot/Empleado/Index.cshtml");
        }

        public ActionResult Guardar(int id = 0) {
            BOT_EmpleadoEntidad empleado = id == 0 ? new BOT_EmpleadoEntidad() : empleadoBL.ObtenerEmpleadoPorId(id);
            return View("~/Views/Bot/Empleado/AddEdit.cshtml", empleado);
        }
        #endregion

        #region Methods
        [HttpPost]
        public JsonResult GetEmpleados() {
            bool success = false;
            string displayMessage;
            List<BOT_EmpleadoEntidad> data = new List<BOT_EmpleadoEntidad>();

            try {
                data = empleadoBL.ObtenerEmpleados();
                success = data.Count > 0;
                displayMessage = success ? "Lista de empleados." : "No hay empleados registrados.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return JsonResponse(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult GetEmpleadoById(int id) {
            bool success = false;
            string displayMessage;
            BOT_EmpleadoEntidad data = new BOT_EmpleadoEntidad();

            try {
                data = empleadoBL.ObtenerEmpleadoPorId(id);
                success = data.Existe();
                displayMessage = success ? "Empleado encontrado." : "No se encontró el empleado.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return JsonResponse(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult GetEmpleadoByDocumentNumber(string documentNumber) {
            bool success = false;
            string displayMessage;
            BOT_EmpleadoEntidad data = new BOT_EmpleadoEntidad();

            try {
                data = empleadoBL.ObtenerEmpleadoPorNumeroDocumento(documentNumber);
                success = data.Existe();
                displayMessage = success ? "Empleado encontrado." : $"No se encontró empleado con número de documento '{documentNumber}'.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return JsonResponse(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult SaveEmpleado(BOT_EmpleadoEntidad empleado) {
            bool success = false;
            string displayMessage = "";
            bool isEdit = empleado.Existe();

            try {
                success = isEdit ? empleadoBL.ActualizarEmpleado(empleado) : empleadoBL.InsertarEmpleado(empleado);
                displayMessage = success ? "Empleado guardado correctamente." : "No se pudo guardar el empleado.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return JsonResponse(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult DeleteEmpleado(int id) {
            bool success = false;
            string displayMessage;

            try {
                success = empleadoBL.EliminarEmpleado(id);
                displayMessage = success ? "Empleado eliminado correctamente." : "No se pudo eliminar el empleado.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return JsonResponse(new { success, displayMessage });
        }
        #endregion
    }
}
