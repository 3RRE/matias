using CapaDatos.GLPI;
using CapaEntidad.GLPI;

namespace CapaNegocio.GLPI {
    public class GLPI_CierreTicketBL {
        private readonly GLPI_CierreTicketDAL cierreTicketDAL;

        public GLPI_CierreTicketBL() {
            cierreTicketDAL = new GLPI_CierreTicketDAL();
        }

        public bool InsertarCierreTicket(GLPI_CierreTicket cierreTicket) {
            return cierreTicketDAL.InsertarCierreTicket(cierreTicket) > 0;
        }
        
        public bool ConfirmarCierreTicket(int idUsuario, int idTicket) {
            return cierreTicketDAL.ConfirmarCierreTicket(idUsuario, idTicket) > 0;
        }

        public bool TicketEstaCerrado(int idTicket) {
            return cierreTicketDAL.TicketEstaCerrado(idTicket);
        }
    }
}
