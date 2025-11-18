using CapaDatos.Administrativo;
using CapaEntidad.Administrativo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Administrativo
{
    public class ADM_HistorialMaquinaBL
    {
        private readonly ADM_HistorialMaquinaDAL _historialMaquinaDAL = new ADM_HistorialMaquinaDAL();
        public List<ADM_HistorialMaquinaEntidad> GetListadoADM_HistorialMaquinaPorSala(int CodSala)
        {
            return _historialMaquinaDAL.GetListadoADM_HistorialMaquinaPorSala(CodSala);
        }
        public int GuardarADM_HistorialMaquina(ADM_HistorialMaquinaEntidad historial)
        {
            return _historialMaquinaDAL.GuardarADM_HistorialMaquina(historial);
        }
        public bool EditarADM_HistorialMaquina(ADM_HistorialMaquinaEntidad historial)
        {
            return _historialMaquinaDAL.EditarADM_HistorialMaquina(historial);
        }
    }
}
