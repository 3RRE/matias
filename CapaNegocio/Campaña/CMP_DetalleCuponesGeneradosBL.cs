using CapaDatos.Campaña;
using CapaEntidad.Campañas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Campaña
{
    public class CMP_DetalleCuponesGeneradosBL
    {
        CMP_DetalleCuponesGeneradosDAL detalleBL = new CMP_DetalleCuponesGeneradosDAL();
        public List<CMP_DetalleCuponesGeneradosEntidad> GetListadoDetalleCuponGenerado(Int64 DetImId)
        {
            return detalleBL.GetListadoDetalleCuponGenerado(DetImId);
        }

        public Int64 CuponesTotalJson(Int64 codsala)
        {
            return detalleBL.CuponesTotalJson(codsala);
        }
        public CMP_DetalleCuponesGeneradosEntidad GetDetalleCuponGeneradoId(Int64 DetGenId)
        {
            return detalleBL.GetDetalleCuponGeneradoId(DetGenId);
        }
        public int GuardarDetalleCuponGenerado(CMP_DetalleCuponesGeneradosEntidad cupon)
        {
            return detalleBL.GuardarDetalleCuponGenerado(cupon);
        }
        public bool EditarDetalleCuponGenerado(CMP_DetalleCuponesGeneradosEntidad cupon)
        {
            return detalleBL.EditarDetalleCuponGenerado(cupon);
        }
        public CMP_DetalleCuponesGeneradosEntidad GetDetalleCuponGeneradoPorSerie(string Serie)
        {
            return detalleBL.GetDetalleCuponGeneradoPorSerie(Serie);
        }
        public bool AumentarCantidadImpresionesDetalleCuponGeneradoPorDetGenId(string InQuery,int UsuarioId)
        {
            return detalleBL.AumentarCantidadImpresionesDetalleCuponGeneradoPorDetGenId(InQuery,UsuarioId);
        }
        public CMP_DetalleCuponesGeneradosEntidad GetUltimoDetalleCuponGeneradoPorSerieySala(int CodSala) {
            return detalleBL.GetUltimoDetalleCuponGeneradoPorSerieySala(CodSala);
        }
    }
}
