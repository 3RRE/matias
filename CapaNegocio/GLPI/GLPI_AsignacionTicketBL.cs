using CapaDatos.GLPI;
using CapaEntidad.GLPI;
using CapaEntidad.GLPI.DTO;
using S3k.Utilitario.GLPI;
using System.Collections.Generic;

namespace CapaNegocio.GLPI {
    public class GLPI_AsignacionTicketBL {
        private readonly GLPI_AsignacionTicketDAL ticketDAL;

        public GLPI_AsignacionTicketBL() {
            ticketDAL = new GLPI_AsignacionTicketDAL();
        }

        public List<GLPI_AsignacionTicketDto> ObtenerAsignacionesTicket() {
            return ticketDAL.ObtenerAsignacionesTicket();
        }

        public List<GLPI_AsignacionTicketDto> ObtenerAsignacionesTicketsPorIdUsuarioAsigna(int idUsuarioAsigna) {
            return ticketDAL.ObtenerAsignacionesTicketsPorIdUsuarioAsigna(idUsuarioAsigna);
        }

        public List<GLPI_AsignacionTicketDto> ObtenerAsignacionesTicketsPorIdUsuarioAsignado(int idUsuarioAsignado) {
            return ticketDAL.ObtenerAsignacionesTicketsPorIdUsuarioAsignado(idUsuarioAsignado);
        }

        public GLPI_AsignacionTicketDto ObtenerAsignacionTicketPorId(int id) {
            return ticketDAL.ObtenerAsignacionTicketPorId(id);
        }

        public GLPI_AsignacionTicketDto ObtenerAsignacionTicketPorIdTicket(int idTicket) {
            return ticketDAL.ObtenerAsignacionTicketPorIdTicket(idTicket);
        }

        public bool InsertarAsignacionTicket(GLPI_AsignacionTicket asignacionTicket) {
            asignacionTicket.Correos = GlpiCorreoHelper.FormarCorreos(asignacionTicket.Destinatarios);
            return ticketDAL.InsertarAsignacionTicket(asignacionTicket) > 0;
        }

        public bool ActualizarAsignacionTicket(GLPI_AsignacionTicket asignacionTicket) {
            asignacionTicket.Correos = GlpiCorreoHelper.FormarCorreos(asignacionTicket.Destinatarios);
            return ticketDAL.ActualizarAsignacionTicket(asignacionTicket) > 0;
        }

        public bool EliminarAsignacionTicket(int id) {
            return ticketDAL.EliminarAsignacionTicket(id) > 0;
        }

        public bool TicketEstaAsignado(int idTicket) {
            return ticketDAL.TicketEstaAsignado(idTicket);
        }
    }
}
