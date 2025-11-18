using CapaDatos.GLPI;
using CapaEntidad.GLPI;
using CapaEntidad.GLPI.DTO;
using CapaEntidad.GLPI.DTO.Global;
using S3k.Utilitario.GLPI;
using System.Collections.Generic;

namespace CapaNegocio.GLPI {
    public class GLPI_SeguimientoTicketBL {
        private readonly GLPI_SeguimientoTicketDAL seguimientoTicketDAL;

        public GLPI_SeguimientoTicketBL() {
            seguimientoTicketDAL = new GLPI_SeguimientoTicketDAL();
        }

        public List<GLPI_SeguimientoTicketDto> ObtenerSeguimientoTicketPorIdTicket(int idTicket) {
            return seguimientoTicketDAL.ObtenerSeguimientoTicketPorIdTicket(idTicket);
        }

        public List<GLPI_SeguimientoDto> ObtenerSeguimientosPorIdTicket(int idTicket) {
            return seguimientoTicketDAL.ObtenerSeguimientosPorIdTicket(idTicket);
        }

        public GLPI_SeguimientoTicket ObtenerUltimoSeguimientoDeTicket(int idTicket) {
            return seguimientoTicketDAL.ObtenerUltimoSeguimientoDeTicket(idTicket);
        }

        public bool InsertarSeguimientoTicket(GLPI_SeguimientoTicket seguimientoTicket) {
            seguimientoTicket.Correos = GlpiCorreoHelper.FormarCorreos(seguimientoTicket.Destinatarios);
            return seguimientoTicketDAL.InsertarSeguimientoTicket(seguimientoTicket) > 0;
        }

        public bool TicketTieneSeguimiento(int idTicket) {
            return seguimientoTicketDAL.TicketTieneSeguimiento(idTicket);
        }
    }
}
