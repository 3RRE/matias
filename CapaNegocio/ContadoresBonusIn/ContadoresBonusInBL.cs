using CapaDatos.ContadoresBonusIn;
using CapaEntidad.ContadoresBonusIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.ContadoresBonusIn {
    public class ContadoresBonusInBL {

        private ContadoresBonusInDAL contadoresBonusInDAL = new ContadoresBonusInDAL();

        public ContadoresBonusInCompleto ObtenerUltimoContadorBonusIn(int codSala) {
            return contadoresBonusInDAL.ObtenerUltimoContadorBonusIn(codSala);
        }
        public Int64 GuardarContadoresBonusIn(ContadoresBonusInCompleto contador) {
            return contadoresBonusInDAL.GuardarContadoresBonusIn(contador);
        }

        public List<ContadoresBonusInCompleto> BuscarSiExisteContadorBonusIn(int codSala, float tmpebw, string codmaq) {
            return contadoresBonusInDAL.BuscarSiExisteContadorBonusIn(codSala,tmpebw,codmaq);
        }
        public List<ContadoresBonusInCompleto> ObtenerListadoContadorBonusInFiltroFechas(int codSala, DateTime fechaIni, DateTime fechaFin) {
            return contadoresBonusInDAL.ObtenerListadoContadorBonusInFiltroFechas(codSala, fechaIni, fechaFin);
        }
        public List<ContadoresBonusInCompleto> ObtenerListadoDetalleContadorBonusInFiltroFechas(int codSala,string codMaq, DateTime fechaIni, DateTime fechaFin) {
            return contadoresBonusInDAL.ObtenerListadoDetalleContadorBonusInFiltroFechas(codSala,codMaq, fechaIni, fechaFin);
        }
        public List<ContadoresBonusInCompleto> ObtenerContadoresBonusInEnviarTotalBonusInJson(int codSala,string codMaq, DateTime fechaIni, DateTime fechaFin) {
            return contadoresBonusInDAL.ObtenerContadoresBonusInEnviarTotalBonusInJson(codSala,codMaq, fechaIni,fechaFin);
        }
        public ContadoresBonusInCompleto ObtenerAntContadoresBonusInEnviarTotalBonusInJson(int codSala,string codMaq, DateTime fecha) {
            return contadoresBonusInDAL.ObtenerAntContadoresBonusInEnviarTotalBonusInJson(codSala,codMaq, fecha);
		}
		public Int64 GuardarContadoresOnlineBoton(ContadoresOnlineBoton contador)
		{
			return contadoresBonusInDAL.GuardarContadoresOnlineBoton(contador);
		}
		public DateTime ObtenerHoraUltimoContadorBoton(int codSala)
		{
			return contadoresBonusInDAL.ObtenerHoraUltimoContadorBoton(codSala);
		}
		public DateTime GetHoraContadoresOnlineBoton(int codSala, DateTime fecha, string order)
		{
			return contadoresBonusInDAL.GetHoraContadoresOnlineBoton(codSala,fecha,order);
		}
		public DateTime GetHoraMaquinaContadoresOnlineBoton(int codSala,string codMaq, DateTime fecha, string order)
		{
			return contadoresBonusInDAL.GetHoraMaquinaContadoresOnlineBoton(codSala, codMaq,fecha, order);
		}
		public List<ContadoresOnlineBoton> GetHoraContadoresOnlineBotonAllMaquina(int codSala, DateTime fecha)
		{
			return contadoresBonusInDAL.GetHoraContadoresOnlineBotonAllMaquina(codSala, fecha);
		}
        public List<ContadoresBonusInCompleto> ObtenerListadoContadoreBonusInPorRango(int codSala, DateTime fechaIni, DateTime fechaFin) { 
            return contadoresBonusInDAL.ObtenerListadoContadoreBonusInPorRango(codSala , fechaIni, fechaFin);   
        }
        public List<ContadoresOnlineBoton> GetContadoresOnlineBotonPorRangoFechas(int CodSala, DateTime fechaIni, DateTime fechaFin) {
            return contadoresBonusInDAL.GetContadoresOnlineBotonPorRangoFechas(CodSala,fechaIni,fechaFin);
        }
    }
}
