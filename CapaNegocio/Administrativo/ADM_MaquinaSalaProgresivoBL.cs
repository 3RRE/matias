using CapaDatos.Administrativo;
using CapaEntidad.Administrativo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Administrativo
{
    public class ADM_MaquinaSalaProgresivoBL
    {
        private readonly ADM_MaquinaSalaProgresivoDAL _maquinaSalaProgresivoDAL = new ADM_MaquinaSalaProgresivoDAL();
        public List<ADM_MaquinaSalaProgresivoEntidad> GetListadoADM_MaquinaSalaProgresivoPorCodSalaProgresivo(int CodSalaProgresivo)
        {
            return _maquinaSalaProgresivoDAL.GetListadoADM_MaquinaSalaProgresivoPorCodSalaProgresivo(CodSalaProgresivo);
        }
        public bool EditarADM_MaquinaSalaProgresivo(ADM_MaquinaSalaProgresivoEntidad progresivo)
        {
            return _maquinaSalaProgresivoDAL.EditarADM_MaquinaSalaProgresivo(progresivo);
        }
        public int GuardarADM_MaquinaSalaProgresivo(ADM_MaquinaSalaProgresivoEntidad progresivo)
        {
            return _maquinaSalaProgresivoDAL.GuardarADM_MaquinaSalaProgresivo(progresivo);
        }
        public ADM_MaquinaSalaProgresivoEntidad GetADM_MaquinaSalaProgresivoPorCodSalaProgresivoyCodMaquina(int CodSalaProgresivo, int CodMaquina)
        {
            return _maquinaSalaProgresivoDAL.GetADM_MaquinaSalaProgresivoPorCodSalaProgresivoyCodMaquina(CodSalaProgresivo, CodMaquina);
        }

        public List<ADM_MaquinaEntidad> GetMaquinasActivasSinAsignacion(int? codSala = null) {
            return _maquinaSalaProgresivoDAL.GetMaquinasActivasSinAsignacion();
        }
    }
}
