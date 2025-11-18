using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class HistorialSolicitudCambioBL
    {
        private HistorialSolicitudCambioDAL historial = new HistorialSolicitudCambioDAL();

        public List<HistorialSolicitudCambioEntidad> BusquedaHistorialSolicitudCambioJson(int id)
        {
            return historial.BusquedaHistorialSolicitudCambioJson(id);
        }
        public bool RegistrarHistorialSolicitudCambio(HistorialSolicitudCambioEntidad historialentidad)
        {
            return historial.RegistrarHistorialSolicitudCambio(historialentidad);
        }
    }
}
