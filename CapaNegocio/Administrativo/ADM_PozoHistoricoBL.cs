using CapaDatos.Administrativo;
using CapaEntidad.Administrativo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Administrativo
{
    public class ADM_PozoHistoricoBL
    {
        private readonly ADM_PozoHistoricoDAL _pozoHistoricoDAL = new ADM_PozoHistoricoDAL();
        public List<ADM_PozoHistoricoEntidad> GetListadoADM_PozoHistoricoPorCodDetalleSalaProgresivoYFecha(int CodDetalleSalaProgresivo, DateTime FechaOperacion)
        {
            return _pozoHistoricoDAL.GetListadoADM_PozoHistoricoPorCodDetalleSalaProgresivoYFecha(CodDetalleSalaProgresivo, FechaOperacion);
        }
        public bool EditarADM_PozoHistorico(ADM_PozoHistoricoEntidad progresivo)
        {
            return _pozoHistoricoDAL.EditarADM_PozoHistorico(progresivo);
        }
        public int GuardarADM_PozoHistorico(ADM_PozoHistoricoEntidad progresivo)
        {
            return _pozoHistoricoDAL.GuardarADM_PozoHistorico(progresivo);
        }

        public List<ADM_PozoHistoricoEntidad> GetHistoricoPorDetalle(int codDetalleSalaProgresivo) { 
            return _pozoHistoricoDAL.GetHistoricoPorDetalle(codDetalleSalaProgresivo);
        }

    }
}
