using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class TransferenciaBL
    {
        private TransferenciaDAL TransferenciaDal = new TransferenciaDAL();
        public List<Transferencia> TransferenciaListadoJson(DateTime fechaini, DateTime fechafin, int codsala)
        {
            return TransferenciaDal.TransferenciaListarJson( fechaini,  fechafin, codsala);
        }

        public List<Transferencia> TransferenciaSalasListarJson(DateTime fechaini, DateTime fechafin, string codsala)
        {
            return TransferenciaDal.TransferenciaSalasListarJson(fechaini, fechafin, codsala);
        }
        public List<Transferencia> TransferenciaBuscarListarJson(DateTime fechaini, DateTime fechafin)
        {
            return TransferenciaDal.TransferenciaBuscarListarJson(fechaini, fechafin);
        }
        public Transferencia TransferenciaIDJson(int transferenciaID)
        {
            return TransferenciaDal.TransferenciaIDJson(transferenciaID);
        }

        public int TransferenciaInsertarJson(Transferencia transferencia)
        {
            return TransferenciaDal.TransferenciaInsertarJson(transferencia);
        }

        public bool TransferenciaImagenModificarJson(int transferenciaID, string imagen)
        {
            return TransferenciaDal.TransferenciaImagenModificarJson(transferenciaID, imagen);
        }
        
        public List<Cliente> TransferenciaClientesListarJson()
        {
            return TransferenciaDal.TransferenciaClientesListarJson();
        }

        public List<Transferencia> TransferenciaCuentaListadoJson(string tipodoc, string nrodoc)
        {
            return TransferenciaDal.TransferenciaCuentasListarJson(tipodoc, nrodoc);
        }

        public List<DepositoEntidad> DepositosListadoJson(DateTime fechaini, DateTime fechafin, string codsala)
        {
            return TransferenciaDal.DepositoListarJson(fechaini, fechafin, codsala);
        }

        public List<TicketEntidad> TransferenciaCuentaListadoJson()
        {
            return TransferenciaDal.TicketsDepositosListarJson();
        }

        public SolicitudTransferencia SolicitudTransferenciaIDJson(int SolicitudID)
        {
            return TransferenciaDal.SolicitudTransferenciaIDJson(SolicitudID);
        }

        public List<SolicitudTransferencia> SolicitudTransferenciaActivasListarJson()
        {
            return TransferenciaDal.SolicitudTransferenciaActivasListarJson();
        }

        public bool SolicitudTransferenciaEstadonModificarJson(int SolicitudID)
        {
            return TransferenciaDal.SolicitudTransferenciaEstadonModificarJson(SolicitudID);
        }

        public List<SolicitudTicket> TicketSolicitudIDListadoJson(int SolicitudID)
        {
            return TransferenciaDal.TicketSolicitudIDListadoJson(SolicitudID);
        }
    }
}
