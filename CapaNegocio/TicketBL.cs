using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class TicketBL
    {
        public TicketDAL ticketDAL = new TicketDAL();
   
        public List<TicketEntidad> TicketDepositoIDListadoJson(int DepositoID)
        {
            return ticketDAL.TicketDepositoIDListadoJson(DepositoID);
        }
        public bool TicketInsertarJson(TicketEntidad ticket)
        {
            return ticketDAL.TicketInsertarJson(ticket);
        }
        public bool TicketEditarJson(TicketEntidad ticket)
        {
            return ticketDAL.TicketEditarJson(ticket);
        }
        public bool TicketATInsertarJson(HistorialTicketAT ticket)
        {
            return ticketDAL.TicketATInsertarJson(ticket);
        }

        public List<HistorialTicketAT> TicketATSalaListarJson(DateTime fechaini, DateTime fechafin, string codsala, string usuario)
        {
            return ticketDAL.TicketATSalaListarJson(fechaini, fechafin, codsala,usuario);
        }

    }
}
