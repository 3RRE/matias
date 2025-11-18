using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class SolicitudTicketBL
    {
        public SolicitudTicketDAL solicitudticketDAL = new SolicitudTicketDAL();

        public List<SolicitudTicket> SolicitudTicketSolicitudIDListadoJson(int SolicitudID)
        {
            return solicitudticketDAL.SolicitudTicketSolicitudIDListadoJson(SolicitudID);
        }
        public bool SolicitudTicketInsertarJson(SolicitudTicket solicitud)
        {
            return solicitudticketDAL.SolicitudTicketInsertarJson(solicitud);
        }
        public bool SolicitudTicketEditarJson(SolicitudTicket solicitud)
        {
            return solicitudticketDAL.SolicitudTicketEditarJson(solicitud);
        }
    }
}
