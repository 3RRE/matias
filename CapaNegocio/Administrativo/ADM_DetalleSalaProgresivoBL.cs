using CapaDatos.Administrativo;
using CapaEntidad.Administrativo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Administrativo
{
    public class ADM_DetalleSalaProgresivoBL
    {
        private readonly ADM_DetalleSalaProgresivoDAL _detalleSalaProgresivoDAL = new ADM_DetalleSalaProgresivoDAL();
        public List<ADM_DetalleSalaProgresivoEntidad> GetListadoADM_DetalleSalaProgresivoPorCodSalaProgresivo(int CodSalaProgresivo)
        {
            return _detalleSalaProgresivoDAL.GetListadoADM_DetalleSalaProgresivoPorCodSalaProgresivo(CodSalaProgresivo);
        }
        public bool EditarADM_DetalleSalaProgresivo(ADM_DetalleSalaProgresivoEntidad progresivo)
        {
            return _detalleSalaProgresivoDAL.EditarADM_DetalleSalaProgresivo(progresivo);
        }
        public int GuardarADM_DetalleSalaProgresivo(ADM_DetalleSalaProgresivoEntidad progresivo)
        {
            return _detalleSalaProgresivoDAL.GuardarADM_DetalleSalaProgresivo(progresivo);
        }
    }
}
