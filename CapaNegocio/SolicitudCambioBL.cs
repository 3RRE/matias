using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class SolicitudCambioBL
    {
        private SolicitudCambioDAL solicitud = new SolicitudCambioDAL();

        public List<SolicitudCambioEntidad> ListaSolicitudCambioJson()
        {
            return solicitud.ListaSolicitudCambioJson();
        }

        public List<SolicitudCambioEntidad> BuscarSolicitudCambioJson(string id_sala,DateTime fechaInicio, DateTime fechaFin, string tipoCambio, string estadoSolicitudCambioId)
        {
            return solicitud.BuscarSolicitudCambioJson(id_sala,fechaInicio, fechaFin,tipoCambio,estadoSolicitudCambioId);
        }

        public bool GuardarBorradorSolicitudCambioJson(SolicitudCambioEntidad solicitudCambio)
        {
            return solicitud.GuardarBorradorSolicitudCambioJson(solicitudCambio);
        }

        public bool EnviarSolicitudCambioJson(SolicitudCambioEntidad solicitudCambio)
        {
            return solicitud.EnviarSolicitudCambioJson(solicitudCambio);
        }

        public List<SolicitudCambioEntidad> BuscarSolicitudCambioNoComite(string id_sala,DateTime fechaInicio, DateTime fechaFin,int id_empleado, string tipoCambio, string estadoSolicitudCambioId)
        {
            return solicitud.BuscarSolicitudCambioNoComite(id_sala,fechaInicio, fechaFin,id_empleado, tipoCambio, estadoSolicitudCambioId);
        }

        public SolicitudCambioEntidad ObtenerSolicitudIdJson(int id)
        {
            return solicitud.ObtenerSolicitudIdJson(id);
        }

        public bool SolicitudCambioActualizarJson(SolicitudCambioEntidad solicitudcambio)
        {
            return solicitud.SolicitudCambioActualizarJson(solicitudcambio);
        }

        public bool SolicitudCambioEliminarJson(int id)
        {
            return solicitud.SolicitudCambioEliminarJson(id);
        }

        public SolicitudCambioEntidad ObtenerUltimaSolicitudUsuarioLogeado(int EmpleadoId)
        {
            return solicitud.ObtenerUltimaSolicitudUsuarioLogeado(EmpleadoId);
        }
    }
}
