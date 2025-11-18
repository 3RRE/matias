using CapaEntidad;
using CapaEntidad.SatisfaccionCliente.DTO.Configuracion;
using CapaEntidad.SatisfaccionCliente.Entity.Configuracion;
using CapaNegocio;
using CapaNegocio.SatisfaccionCliente.Configuracion;
using System;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.SatisfaccionCliente.Configuracion {
    [seguridad(true)]
    public class EscConfiguracionesController : Controller {
        private readonly ESC_ConfiguracionBL configuracionBL;
        private readonly SalaBL salaBL;

        public EscConfiguracionesController() {
            configuracionBL = new ESC_ConfiguracionBL();
            salaBL = new SalaBL();
        }

        #region Views
        public ActionResult ConfiguracionView() {
            return View("~/Views/SatisfaccionCliente/Configuracion/Configuracion.cshtml");
        }
        public ActionResult AgregarEditarConfiguracionView(int id = 0) {
            ESC_ConfiguracionDto configuracion = id == 0 ? new ESC_ConfiguracionDto() : configuracionBL.ObtenerConfiguracionPorId(id);
            return View("~/Views/SatisfaccionCliente/Configuracion/ConfiguracionAddEdit.cshtml", configuracion);
        }
        #endregion

        #region Methods
        [seguridad(false)] //debido a que NWO hace uso del metodo
        [HttpPost]
        public JsonResult GetConfiguracionByCodSala(int codSala) {
            bool success = false;
            string displayMessage;
            ESC_ConfiguracionDto data = new ESC_ConfiguracionDto();

            try {
                SalaEntidad sala = salaBL.ObtenerSalaPorCodigo(codSala);
                if(!sala.Existe()) {
                    success = false;
                    displayMessage = $"No existe sala con el código {codSala}.";
                    return Json(new { success, displayMessage });
                }

                data = configuracionBL.ObtenerConfiguracionPorCodSala(codSala);
                success = data.Existe();
                displayMessage = success ? $"Configuración de la sala {sala.Nombre}." : $"La sala {sala.Nombre} aún no tiene registrada su configuración.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult GetConfiguracionById(int id) {
            bool success = false;
            string displayMessage;
            ESC_ConfiguracionDto data = new ESC_ConfiguracionDto();

            try {
                data = configuracionBL.ObtenerConfiguracionPorId(id);
                success = data.Existe();
                displayMessage = success ? "Configuración encontrada." : "No se encontró la configuración.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult SaveConfiguracion(ESC_Configuracion configuracion) {
            bool success = false;
            string displayMessage;
            int id = 0;
            bool isEdit = configuracion.Existe();

            try {
                id = isEdit ? configuracionBL.ActualizarConfiguracion(configuracion) : configuracionBL.InsertarConfiguracion(configuracion);
                success = id > 0;
                displayMessage = success ? "Configuración guardada correctamente." : "No se pudo guardar la configuración.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage, id });
        }
        #endregion
    }
}