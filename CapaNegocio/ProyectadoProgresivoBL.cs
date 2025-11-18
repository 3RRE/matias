using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class ProyectadoProgresivoBL
    {
        ProyectadoProgresivoDAL proyectadodal = new ProyectadoProgresivoDAL();

        public List<ProyectadoProgresivoEntidad> ProyectadoProgresivoListadoJson()
        {
            return proyectadodal.ProyectadoProgresivoListadoJson();
        }

        public bool ProyectadoProgresivoInsertarJson(ProyectadoProgresivoEntidad entidad)
        {
            return proyectadodal.ProyectadoProgresivoInsertarJson(entidad);
        }

        public ProyectadoProgresivoEntidad ProyectadoProgresivoObtenerIdJson(int IdProyectadoProgresivo)
        {
            return proyectadodal.ProyectadoProgresivoObtenerIdJson(IdProyectadoProgresivo);
        }

        public bool ProyectadoProgresivoEditarJson(ProyectadoProgresivoEntidad data)
        {
            return proyectadodal.ProyectadoProgresivoEditarJson(data);
        }
    }
}
