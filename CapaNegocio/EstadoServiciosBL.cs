using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class EstadoServiciosBL
    {
        private EstadoServiciosDAL estadoServiciosDAL = new EstadoServiciosDAL();

        public List<EstadoServiciosEntidad> GetEstadoServiciosAll()
        {
            return estadoServiciosDAL.GetEstadoServiciosAll();
        }
        public List<EstadoServiciosEntidad> GetEstadoServiciosxSala(int codSala)
        {
            return estadoServiciosDAL.GetEstadoServiciosxSala(codSala);
        }

    }
}
