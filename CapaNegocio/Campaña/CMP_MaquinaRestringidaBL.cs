using CapaDatos.Campaña;
using CapaEntidad.Campañas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Campaña
{
    public class CMP_MaquinaRestringidaBL
    {
        private CMP_MaquinaRestringidaDAL maquina_dal = new CMP_MaquinaRestringidaDAL();
        public List<CMP_MaquinaRestringidaEntidad> GetListadoMaquinaRestringidaSala(int CodSala)
        {
            return maquina_dal.GetListadoMaquinaRestringidaSala(CodSala);
        }
        public bool EditarEstadoRestriccionMaquina(CMP_MaquinaRestringidaEntidad maquinaRestringida) {
            return maquina_dal.EditarEstadoRestriccionMaquina(maquinaRestringida);
        }
        public bool GuardaMaquina(CMP_MaquinaRestringidaEntidad maquina) {
            return maquina_dal.GuardaMaquina(maquina);
        }
        public bool GuardarMaquinaMasivo(string values)
        {
            return maquina_dal.GuardarMaquinaMasivo(values);
        }
        public CMP_MaquinaRestringidaEntidad GetMaquinaRestringidaSalaPorSalaYMaquina(int CodSala, string CodMaquina) {
            return maquina_dal.GetMaquinaRestringidaSalaPorSalaYMaquina(CodSala, CodMaquina);
        }
    }
}
