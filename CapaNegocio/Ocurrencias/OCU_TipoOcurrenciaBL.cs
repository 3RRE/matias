using CapaDatos.Ocurrencias;
using CapaEntidad.Ocurrencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Ocurrencias
{
    public class OCU_TipoOcurrenciaBL
    {
        private OCU_TipoOcurrenciaDAL tipoOcurrenciaDAL = new OCU_TipoOcurrenciaDAL();
        public List<OCU_TipoOcurrenciaEntidad> GetListadoTipoOcurrencia()
        {
            return tipoOcurrenciaDAL.GetListadoTipoOcurrencia();
        }
        public int GuardarTipoOcurrencia(OCU_TipoOcurrenciaEntidad tipoOcurrencia)
        {
            return tipoOcurrenciaDAL.GuardarTipoOcurrencia(tipoOcurrencia);
        }
        public bool EditarTipoOcurrencia(OCU_TipoOcurrenciaEntidad tipoOcurrencia)
        {
            return tipoOcurrenciaDAL.EditarTipoOcurrencia(tipoOcurrencia);
        }
        public bool EditarEstadoTipoOcurrencia(OCU_TipoOcurrenciaEntidad tipoOcurrencia)
        {
            return tipoOcurrenciaDAL.EditarEstadoTipoOcurrencia(tipoOcurrencia);
        }
        public OCU_TipoOcurrenciaEntidad GetTipoOcurrenciaID(int TipoOcurrenciaId)
        {
            return tipoOcurrenciaDAL.GetTipoOcurrenciaID(TipoOcurrenciaId);
        }
    }
}
