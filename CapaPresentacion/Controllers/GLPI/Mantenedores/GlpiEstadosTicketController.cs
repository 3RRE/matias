using CapaEntidad.GLPI.Mantenedores;
using CapaNegocio.GLPI.Mantenedores;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.GLPI.Mantenedores {
    [seguridad(false)]
    public class GlpiEstadosTicketController : Controller {
        private readonly GLPI_EstadoTicketBL estadoTicketBL;

        public GlpiEstadosTicketController() {
            estadoTicketBL = new GLPI_EstadoTicketBL();
        }

        #region Methods
        [HttpPost]
        public JsonResult GetEstadosTicket() {
            bool success = false;
            string displayMessage;
            List<GLPI_EstadoTicket> data = new List<GLPI_EstadoTicket>();

            try {
                data = estadoTicketBL.ObtenerEstadosTicket();
                success = data.Count > 0;
                displayMessage = success ? "Lista de estados de ticket." : "No hay estados de ticket registrados.";
            } catch (Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult GetEstadoTicketById(int id) {
            bool success = false;
            string displayMessage;
            GLPI_EstadoTicket data = new GLPI_EstadoTicket();

            try {
                data = estadoTicketBL.ObtenerEstadoTicketPorId(id);
                success = data.Existe();
                displayMessage = success ? "Estado de ticket encontrado." : "No se encontró el estado de ticket.";
            } catch (Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult SaveEstadoTicket(GLPI_EstadoTicket partida) {
            bool success = false;
            string displayMessage;
            bool isEdit = partida.Existe();

            try {
                success = isEdit ? estadoTicketBL.ActualizarEstadoTicket(partida) : estadoTicketBL.InsertarEstadoTicket(partida);
                displayMessage = success ? "Estado de ticket guardado correctamente." : "No se pudo guardar el estado de ticket.";
            } catch (Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }

        //[HttpPost]
        //public JsonResult ActualizarEstado(int id, bool estado) {
        //    bool success = false;
        //    string displayMessage;

        //    try {
        //        success = estadoTicketBL.ActualizarEstado(id, estado);
        //        displayMessage = success ? "Estado actualizado correctamente." : "No se pudo actualizar es estado.";
        //    } catch (Exception ex) {
        //        displayMessage = $"{ex.Message}. Llame al administrador";
        //    }
        //    return Json(new { success, displayMessage });
        //}

        [HttpPost]
        public JsonResult DeleteEstadoTicket(int id) {
            bool success = false;
            string displayMessage;

            try {
                success = estadoTicketBL.EliminarEstadoTicket(id);
                displayMessage = success ? "Estado de ticket eliminado correctamente." : "No se pudo eliminar el estado de ticket.";
            } catch (Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage });
        }
        #endregion
    }
}
