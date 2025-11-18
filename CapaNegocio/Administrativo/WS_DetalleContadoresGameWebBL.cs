using CapaDatos.Administrativo;
using CapaEntidad.Administrativo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Administrativo
{
    public class WS_DetalleContadoresGameWebBL
    {
        private readonly WS_DetalleContadoresGameWebDAL _detalleContadoresGameDAL = new WS_DetalleContadoresGameWebDAL();
        public List<WS_DetalleContadoresGameWebEntidad> GetListadoWS_DetalleContadoresGamePorFechaOperacion(int CodSala, DateTime FechaOperacion)
        {
            return _detalleContadoresGameDAL.GetListadoWS_DetalleContadoresGamePorFechaOperacion(CodSala, FechaOperacion);
        }
        public WS_DetalleContadoresGameWebEntidad GetWS_DetalleContadoresGamePorHoraYCodMaq(int CodSala,string CodMaq, DateTime Hora)
        {
            return _detalleContadoresGameDAL.GetWS_DetalleContadoresGamePorHoraYCodMaq(CodSala, CodMaq,Hora);
        }
        public int GuardarWS_DetalleContadoresGameWeb(WS_DetalleContadoresGameWebEntidad contador)
        {
            return _detalleContadoresGameDAL.GuardarWS_DetalleContadoresGameWeb(contador);
        }
        public bool EliminarWS_DetalleContadoresGameWebPorFecha(int CodSala,DateTime fechaOperacion)
        {
            return _detalleContadoresGameDAL.EliminarWS_DetalleContadoresGameWebPorFecha(CodSala,fechaOperacion);
		}
		public DateTime GetHoraDetalleContadoresGameWeb(int CodSala, DateTime FechaOperacion)
		{
			return _detalleContadoresGameDAL.GetHoraDetalleContadoresGameWeb(CodSala, FechaOperacion);
		}
		

	}
}
