using CapaDatos.GLPI.Mantenedores;
using CapaEntidad.GLPI.Helper;
using CapaEntidad.GLPI.Mantenedores;
using System.Collections.Generic;
using System.Linq;

namespace CapaNegocio.GLPI.Mantenedores {
    public class GLPI_EstadoTicketBL {
        private readonly GLPI_EstadoTicketDAL estadoTicketDAL;

        public GLPI_EstadoTicketBL() {
            estadoTicketDAL = new GLPI_EstadoTicketDAL();
        }

        public List<GLPI_EstadoTicket> ObtenerEstadosTicket() {
            return estadoTicketDAL.ObtenerEstadosTicket();
        }

        public List<GLPI_SelectHelper> ObtenerEstadosTicketSelect() {
            List<GLPI_SelectHelper> options = ObtenerEstadosTicket()
                .Select(x => new GLPI_SelectHelper {
                    IdPadre = x.ObtenerIdPadreSelect(),
                    Valor = x.ObtenerValorSelect(),
                    Texto = x.ObtenerTextoSelect(),
                    Estado = x.ObtenerEstadoSelect()
                }).ToList();
            return options;
        }

        public GLPI_EstadoTicket ObtenerEstadoTicketPorId(int id) {
            return estadoTicketDAL.ObtenerEstadoTicketPorId(id);
        }

        public bool InsertarEstadoTicket(GLPI_EstadoTicket estadoTicket) {
            return estadoTicketDAL.InsertarEstadoTicket(estadoTicket) > 0;
        }

        public bool ActualizarEstadoTicket(GLPI_EstadoTicket estadoTicket) {
            return estadoTicketDAL.ActualizarEstadoTicket(estadoTicket) > 0;
        }

        //public bool ActualizarEstado(int id, bool estado) {
        //    return estadoTicketDAL.ActualizarEstado(id, estado) > 0;
        //}

        public bool EliminarEstadoTicket(int id) {
            return estadoTicketDAL.EliminarEstadoTicket(id) > 0;
        }
    }
}
