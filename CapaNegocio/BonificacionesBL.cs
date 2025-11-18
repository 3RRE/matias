using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class BonificacionesBL
    {
        private BonificacionesDAL Bonificacionesdal = new BonificacionesDAL();
        public List<BonificacionesEntidad> BonificacionesBuscarJson(string nrodocumento, string nroticket)
        {
            return Bonificacionesdal.BonificacionesBuscarJson(nrodocumento, nroticket);
        }

        public List<BonificacionesEntidad> BonificacionesListarJson(DateTime fechaini, DateTime fechafin, string codsala)
        {
            return Bonificacionesdal.BonificacionesListarJson(fechaini, fechafin, codsala);
        }

        public int BonificacionesInsertarJson(BonificacionesEntidad bonificacion)
        {
            return Bonificacionesdal.BonificacionesInsertarJson(bonificacion);
        }

        public bool BonificacionesActualizarJson(BonificacionesEntidad bonificacion)
        {
            return Bonificacionesdal.BonificacionesActualizarJson(bonificacion);
        }

    }
}
