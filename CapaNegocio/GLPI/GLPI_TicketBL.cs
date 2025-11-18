using CapaDatos.GLPI;
using CapaEntidad.GLPI;
using CapaEntidad.GLPI.DTO;
using CapaEntidad.GLPI.Enum;
using CapaEntidad.GLPI.Reporte;
using S3k.Utilitario.GLPI;
using System.Collections.Generic;
using System.Linq;

namespace CapaNegocio.GLPI {
    public class GLPI_TicketBL {
        private readonly GLPI_TicketDAL ticketDAL;
        private readonly GLPI_SeguimientoTicketBL seguimientoTicketBL;

        public GLPI_TicketBL() {
            ticketDAL = new GLPI_TicketDAL();
            seguimientoTicketBL = new GLPI_SeguimientoTicketBL();
        }

        public List<GLPI_TicketDto> ObtenerTicketsReportes(GLPI_ReporteFiltro filtro) {
            string codsSalas = string.Join(", ", filtro.CodsSalas);
            string codsFases = string.Join(", ", filtro.FasesTicket.Select(x => ((int)x).ToString()));
            List<GLPI_TicketDto> tickets = ticketDAL.ObtenerTicketsReportes(filtro.FechaInicio, filtro.FechaFin, codsSalas, codsFases);
            foreach(GLPI_TicketDto ticket in tickets) {
                ticket.Seguimientos = seguimientoTicketBL.ObtenerSeguimientosPorIdTicket(ticket.Id);
            }
            return tickets;
        }

        public List<GLPI_TicketDto> ObtenerTicketsPorIdUsuarioSolicitante(int idUsuario) {
            List<GLPI_TicketDto> tickets = ticketDAL.ObtenerTicketsPorIdUsuarioSolicitante(idUsuario);
            foreach(GLPI_TicketDto ticket in tickets) {
                ticket.Seguimientos = seguimientoTicketBL.ObtenerSeguimientosPorIdTicket(ticket.Id);
            }
            return tickets;
        }

        public List<GLPI_TicketDto> ObtenerTicketsPorIdUsuarioAsigna(int idUsuario) {
            List<GLPI_TicketDto> tickets = ticketDAL.ObtenerTicketsPorIdUsuarioAsigna(idUsuario);
            foreach(GLPI_TicketDto ticket in tickets) {
                ticket.Seguimientos = seguimientoTicketBL.ObtenerSeguimientosPorIdTicket(ticket.Id);
            }
            return tickets;
        }

        public List<GLPI_TicketDto> ObtenerTicketsPorIdUsuarioAsignado(int idUsuario) {
            List<GLPI_TicketDto> tickets = ticketDAL.ObtenerTicketsPorIdUsuarioAsignado(idUsuario);
            foreach(GLPI_TicketDto ticket in tickets) {
                ticket.Seguimientos = seguimientoTicketBL.ObtenerSeguimientosPorIdTicket(ticket.Id);
            }
            return tickets;
        }
        public List<GLPI_TicketDto> ObtenerTicketsPorCodsSalaYFase(List<int> codsSala, GLPI_FaseTicket faseTicket)
        {
            string codsSalasStr = string.Join(",", codsSala);
            List<GLPI_TicketDto> tickets = ticketDAL.ObtenerTicketsPorCodsSalaYFase(codsSalasStr, faseTicket);
            foreach (GLPI_TicketDto ticket in tickets)
            {
                ticket.Seguimientos = seguimientoTicketBL.ObtenerSeguimientosPorIdTicket(ticket.Id);
            }
            return tickets;
        }

        public GLPI_TicketDto ObtenerTicketPorId(int id) {
            GLPI_TicketDto ticket = ticketDAL.ObtenerTicketPorId(id);
            ticket.Seguimientos = seguimientoTicketBL.ObtenerSeguimientosPorIdTicket(ticket.Id);
            return ticket;
        }

        public int InsertarTicket(GLPI_Ticket ticket) {
            ticket.Correos = GlpiCorreoHelper.FormarCorreos(GlpiCorreoHelper.FormarCorreos(ticket.Destinatarios));
            return ticketDAL.InsertarTicket(ticket);
        }

        public bool TicketEstaEnFase(int idTicket, GLPI_FaseTicket faseTicket) {
            return ticketDAL.TicketEstaEnFase(idTicket, faseTicket);
        }

        public int ActualizarTicket(GLPI_Ticket ticket) {
            ticket.Correos = GlpiCorreoHelper.FormarCorreos(GlpiCorreoHelper.FormarCorreos(ticket.Destinatarios));
            return ticketDAL.ActualizarTicket(ticket);
        }

        public bool ActualizarFaseTicketPorIdTicket(int idTicket, GLPI_FaseTicket faseTicket) {
            return ticketDAL.ActualizarFaseTicketPorIdTicket(idTicket, faseTicket) > 0;
        }

        public bool EliminarTicket(int id) {
            return ticketDAL.EliminarTicket(id) > 0;
        }
    }
}
