using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class EstadoSolicitudCambioBL
    {
        private EstadoSolicitudCambioDAL estado = new EstadoSolicitudCambioDAL();

        public List<EstadoSolicitudCambioEntidad> EstadoSolicitudCambioListadoJson()
        {
            return estado.EstadoSolicitudCambioListadoJson();
        }

    }
}
