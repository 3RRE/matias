using CapaDatos.Ocurrencias;
using CapaEntidad.Ocurrencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Ocurrencias
{
    public class OCU_OcurrenciaBL
    {
        private OCU_OcurrenciaDAL ocurrenciaDAL = new OCU_OcurrenciaDAL();
        public int GuardarOcurrencia(OCU_OcurrenciaEntidad ocurrencia)
        {
            return ocurrenciaDAL.GuardarOcurrencia(ocurrencia);
        }
        public bool EditarEstadoEnvioOcurrencia(OCU_OcurrenciaEntidad ocurrencia)
        {
            return ocurrenciaDAL.EditarEstadoEnvioOcurrencia(ocurrencia);
        }
        public List<OCU_OcurrenciaEntidad> GetListadoOcurrencia(DateTime fechaIni,DateTime fechaFin, string condicion="")
        {
            return ocurrenciaDAL.GetListadoOcurrencia(fechaIni,fechaFin, condicion);
        }
        public OCU_OcurrenciaEntidad GetOcurrenciaId(int ocurrenciaId)
        {
            return ocurrenciaDAL.GetOcurrenciaId(ocurrenciaId);
        }
    }
}
