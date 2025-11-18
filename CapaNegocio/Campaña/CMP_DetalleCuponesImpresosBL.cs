using CapaDatos.Campaña;
using CapaEntidad.Campañas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Campaña
{
    public class CMP_DetalleCuponesImpresosBL
    {
        public CMP_DetalleCuponesImpresosDAL detalleDAL = new CMP_DetalleCuponesImpresosDAL();
        public List<CMP_DetalleCuponesImpresosEntidad> GetListadoDetalleCuponImpreso(Int64 CgId) {
            return detalleDAL.GetListadoDetalleCuponImpreso(CgId);
        }
        public CMP_DetalleCuponesImpresosEntidad GetDetalleCuponImpresoId(Int64 DetImpId) {
            return detalleDAL.GetDetalleCuponImpresoId(DetImpId);
        }
        public Int64 GuardarDetalleCuponImpreso(CMP_DetalleCuponesImpresosEntidad cupon)
        {
            return detalleDAL.GuardarDetalleCuponImpreso(cupon);
        }
        public bool EditarDetalleCuponImpreso(CMP_DetalleCuponesImpresosEntidad cupon)
        {
            return detalleDAL.EditarDetalleCuponImpreso(cupon);
        }
        public List<CMP_DetalleCuponesImpresosEntidad> GetListadoDetalleCuponImpresoExcel(string whereQuery) {
            return detalleDAL.GetListadoDetalleCuponImpresoExcel(whereQuery);
        }
    }
}
