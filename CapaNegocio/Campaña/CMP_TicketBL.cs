using CapaDatos.Campaña;
using CapaEntidad.Campañas;
using CapaEntidad.TITO;
using CapaNegocio.AsistenciaCliente;
using System;
using System.Collections.Generic;

namespace CapaNegocio.Campaña
{
    public class CMP_TicketBL
    {
        private CMP_TicketDAL ticket_dal = new CMP_TicketDAL();
        private readonly AST_ClienteBL _ASTClienteBL = new AST_ClienteBL();

        public List<CMP_TicketEntidad> CMPTicketListadoCompletoJson()
        {
            return ticket_dal.GetListadoTicketcompleto();
        }

        public List<CMP_TicketEntidad> CMPTicketCampañaListadoJson(Int64 id)
        {
            return ticket_dal.GetListadoTicketCampaña(id);
        }

        public CMP_TicketEntidad CMPTIcketIdObtenerJson(int id)
        {
            return ticket_dal.GetTicketID(id);
        }

        public CMP_TicketEntidad CMPTIcketmontonroObtenerJson(int item, string nroticket, double monto)
        {
            return ticket_dal.GetTicketxnromonto(item, nroticket, monto);
        }

        public int TicketInsertarJson(CMP_TicketEntidad ticket)
        {
            return ticket_dal.GuardarTicket(ticket);
        }
        public bool TicketEliminarJson(int id)
        {
            return ticket_dal.eliminarTicket(id);
        }

        public List<CMP_TicketEntidad> GetTicketsEnlazadosItems(int salaId, List<int> D00H_Items)
        {
            return ticket_dal.GetTicketsEnlazadosItems(salaId, D00H_Items);
        }

        public List<CMP_TicketEntidad> GetTicketsEnlazadosItemsV2(int salaId, List<int> D00H_Items)
        {
            return ticket_dal.GetTicketsEnlazadosItemsV2(salaId, D00H_Items);
        }

        public CMP_TicketEntidad GetTicketItem(int item, string nroticket, double monto)
        {
            return ticket_dal.GetTicketItem(item, nroticket, monto);
        }

        public List<Reporte_Detalle_TITO_Caja> ATPGuardarTickets(int usuarioId, int sala_id, int campaniaId, int customer_id, List<Reporte_Detalle_TITO_Caja> tickets)
        {
            return ticket_dal.ATPGuardarTickets(usuarioId, campaniaId, customer_id, tickets);
        }

        public bool ATPActualizarTicketsDiff(int salaId, List<Reporte_Detalle_TITO_Caja> tickets)
        {
            return ticket_dal.ATPActualizarTicketsDiff(salaId, tickets);
        }

        #region Reporte Tickets

        public List<CMP_TicketReporteEntidad> ObtenerTicketsPorSalas(List<int> salaIds, DateTime fromDate, DateTime endDate, int campaniaTipo)
        {
            return ticket_dal.ObtenerTicketsPorSalas(salaIds, fromDate, endDate, campaniaTipo);
        }

        #endregion
    }
}
