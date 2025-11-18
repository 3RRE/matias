using CapaDatos.Administrativo;
using CapaEntidad.Administrativo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Administrativo
{
    public class ADM_SalaProgresivoBL
    {
        private readonly ADM_SalaProgresivoDAL _salaProgresivoDAL = new ADM_SalaProgresivoDAL();
        public List<ADM_SalaProgresivoEntidad> GetListadoADM_SalaProgresivoPorSala(int CodSala)
        {
            return _salaProgresivoDAL.GetListadoADM_SalaProgresivoPorSala(CodSala);
        }
        public bool EditarADM_SalaProgresivo(ADM_SalaProgresivoEntidad progresivo)
        {
            return _salaProgresivoDAL.EditarADM_SalaProgresivo(progresivo);
        }
        public int GuardarADM_SalaProgresivo(ADM_SalaProgresivoEntidad progresivo)
        {
            return _salaProgresivoDAL.GuardarADM_SalaProgresivo(progresivo);
        }
        public List<ADM_SalaProgresivoEntidad> GetListadoADM_SalaProgresivoPorQuery(string whereQuery, IDictionary<string, DateTime> fechaParametros=null)
        {
            return _salaProgresivoDAL.GetListadoADM_SalaProgresivoPorQuery(whereQuery, fechaParametros);
        }
    }
}
