using CapaEntidad.GLPI.Mantenedores;
using CapaNegocio.GLPI.Mantenedores;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.GLPI.Mantenedores {
    [seguridad(false)]
    public class GlpiIdentificadoresController : Controller {
        private readonly GLPI_IdentificadorBL identificadorBL;

        public GlpiIdentificadoresController() {
            identificadorBL = new GLPI_IdentificadorBL();
        }

        #region Methods
        [HttpPost]
        public JsonResult GetIdentificadores() {
            bool success = false;
            string displayMessage;
            List<GLPI_Identificador> data = new List<GLPI_Identificador>();

            try {
                data = identificadorBL.ObtenerIdentificadores();
                success = data.Count > 0;
                displayMessage = success ? "Lista de identificadores." : "No hay tipos de identificadores.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult GetIdentificadorById(int id) {
            bool success = false;
            string displayMessage;
            GLPI_Identificador data = new GLPI_Identificador();

            try {
                data = identificadorBL.ObtenerIdentificadorPorId(id);
                success = data.Existe();
                displayMessage = success ? "Identificador encontrado." : "No se encontró el identificador.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult SaveIdentificador(GLPI_Identificador partida) {
            bool success = false;
            string displayMessage;
            bool isEdit = partida.Existe();

            try {
                success = isEdit ? identificadorBL.ActualizarIdentificador(partida) : identificadorBL.InsertarIdentificador(partida);
                displayMessage = success ? "Identificador guardado correctamente." : "No se pudo guardar el identificador.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult DeleteIdentificador(int id) {
            bool success = false;
            string displayMessage;

            try {
                success = identificadorBL.EliminarIdentificador(id);
                displayMessage = success ? "Identificador eliminado correctamente." : "No se pudo eliminar el identificador.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage });
        }
        #endregion
    }
}
