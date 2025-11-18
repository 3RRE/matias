using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class SolicitudTransferenciaBL
    {
        public SolicitudTransferenciaDAL solicitudtransferenciadal= new SolicitudTransferenciaDAL();
        public int SolicitudTransferenciaInsertar(SolicitudTransferencia solicitud)
        {
            return solicitudtransferenciadal.SolicitudTransferenciaInsertarJson(solicitud);
        }
        public List<SolicitudTransferencia> SolicitudTransferenciaListarSolicitudSalaJson(int SolicitudSala)
        {
            return solicitudtransferenciadal.SolicitudTransferenciaListarSolicitudTransferenciaSalaJson(SolicitudSala);
        }
        public bool SolicitudTransferenciaEditarJson(SolicitudTransferencia solicitud)
        {
            return solicitudtransferenciadal.SolicitudTransferenciaEditarJson(solicitud);
        }
        public bool SolicitudTransferenciaAnularJson(SolicitudTransferencia solicitud)
        {
            return solicitudtransferenciadal.SolicitudTransferenciaAnularJson(solicitud);
        }
    }
}
