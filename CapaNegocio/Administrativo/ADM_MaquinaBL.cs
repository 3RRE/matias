using CapaDatos.Administrativo;
using CapaEntidad.Administrativo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Administrativo
{
    public class ADM_MaquinaBL
    {
        private readonly ADM_MaquinaDAL _maquinaDAL = new ADM_MaquinaDAL();
        public List<ADM_MaquinaEntidad> GetListadoADM_MaquinaPorSala(int CodSala)
        {
            return _maquinaDAL.GetListadoADM_MaquinaPorSala(CodSala);
        }
        public bool EditarADM_Maquina(ADM_MaquinaEntidad maquina)
        {
            return _maquinaDAL.EditarADM_Maquina(maquina);
        }
        public int GuardarADM_Maquina(ADM_MaquinaEntidad maquina)
        {
            return _maquinaDAL.GuardarADM_Maquina(maquina);
        }
    }
}
