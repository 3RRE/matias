using CapaDatos.Campaña;
using CapaEntidad.Campañas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Campaña
{
    public class CMP_CuponesGeneradosBL
    {
        CMP_CuponesGeneradosDAL cupoDal = new CMP_CuponesGeneradosDAL();
        public List<CMP_CuponesGeneradosEntidad> GetListadoCupones()
        {
            return cupoDal.GetListadoCupones();
        }

        public CMP_CuponesGeneradosEntidad GetCuponGeneradoId(Int64 CgId)
        {
            return cupoDal.GetCuponGeneradoId(CgId);
        }
        public int GuardarCuponGenerado(CMP_CuponesGeneradosEntidad cupon)
        {
            return cupoDal.GuardarCuponGenerado(cupon);
        }
        public bool EditarCuponGenerado(CMP_CuponesGeneradosEntidad cupon)
        {
            return cupoDal.EditarCuponGenerado(cupon);
        }
        public bool EditarEstadoCuponGenerado(CMP_CuponesGeneradosEntidad cupon)
        {
            return cupoDal.EditarEstadoCuponGenerado(cupon);
        }
        public bool EditarCuponGeneradoSeries(CMP_CuponesGeneradosEntidad cupon)
        {
            return cupoDal.EditarCuponGeneradoSeries(cupon);
        }
        public bool EditarCantidadCuponGenerados(CMP_CuponesGeneradosEntidad cupon)
        {
            return cupoDal.EditarCantidadCuponGenerados(cupon);
        }
        public List<CMP_CuponesGeneradosEntidad> GetListadoCuponesxCampania(Int64 CampaniaId, bool detalle=false)
        {
            return cupoDal.GetListadoCuponesxCampania(CampaniaId,detalle);
        }
        public List<CMP_CuponesGeneradosEntidad> GetListadoCuponesxCampaniaFecha(Int64 CampaniaId, DateTime fechaInicio, DateTime fechaFin, bool detalle = false)
        {
            return cupoDal.GetListadoCuponesxCampaniaFecha(CampaniaId, fechaInicio, fechaFin, detalle);
        }
        public List<CMP_CuponesGeneradosEntidad> GetListadoCuponesxCliente(string whereQuery, DateTime fechaIni, DateTime fechaFin) {
            return cupoDal.GetListadoCuponesxCliente(whereQuery,fechaIni,fechaFin);
        }
    }
}
