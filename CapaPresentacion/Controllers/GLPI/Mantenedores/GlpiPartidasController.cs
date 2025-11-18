using CapaEntidad.GLPI.Mantenedores;
using CapaNegocio.GLPI.Mantenedores;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.GLPI.Mantenedores {
    [seguridad(false)]
    public class GlpiPartidasController : Controller {
        private readonly GLPI_PartidaBL partidaBL;

        public GlpiPartidasController() {
            partidaBL = new GLPI_PartidaBL();
        }

        #region Methods
        [HttpPost]
        public JsonResult GetPartidas() {
            bool success = false;
            string displayMessage;
            List<GLPI_Partida> data = new List<GLPI_Partida>();

            try {
                data = partidaBL.ObtenerPartidas();
                success = data.Count > 0;
                displayMessage = success ? "Lista de partidas." : "No hay partidas registradas.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult GetPartidaById(int id) {
            bool success = false;
            string displayMessage;
            GLPI_Partida data = new GLPI_Partida();

            try {
                data = partidaBL.ObtenerPartidaPorId(id);
                success = data.Existe();
                displayMessage = success ? "Partida encontrada." : "No se encontró la partida.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult SavePartida(GLPI_Partida partida) {
            bool success = false;
            string displayMessage;
            bool isEdit = partida.Existe();

            try {
                success = isEdit ? partidaBL.ActualizarPartida(partida) : partidaBL.InsertarPartida(partida);
                displayMessage = success ? "Partida guardada correctamente." : "No se pudo guardar la partida.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult DeletePartida(int id) {
            bool success = false;
            string displayMessage;

            try {
                success = partidaBL.EliminarPartida(id);
                displayMessage = success ? "Partida eliminada correctamente." : "No se pudo eliminar la partida.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage });
        }
        #endregion
    }
}
