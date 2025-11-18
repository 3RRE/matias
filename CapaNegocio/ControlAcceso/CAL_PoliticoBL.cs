using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos.ControlAcceso;
using CapaEntidad.ControlAcceso;
using Newtonsoft.Json;

namespace CapaNegocio.ControlAcceso
{
    public class CAL_PoliticoBL
    {
        CAL_PoliticoDAL capaDatos = new CAL_PoliticoDAL();

        public List<CAL_PoliticoEntidad> PoliticoListadoCompletoJson()
        {
            return capaDatos.GetAllPolitico();
        }
        public CAL_PoliticoEntidad PoliticoIdObtenerJson(int id)
        {
            return capaDatos.GetIDPolitico(id);
        }
        public int PoliticoInsertarJson(CAL_PoliticoEntidad Entidad)
        {
            var id = capaDatos.InsertarPolitico(Entidad);

            return id;
        }
        public bool PoliticoEditarJson(CAL_PoliticoEntidad Entidad)
        {
            var status = capaDatos.EditarPolitico(Entidad);

            return status;
        }
        public bool PoliticoEliminarJson(int id)
        {
            var status = capaDatos.EliminarPolitico(id);

            return status;
        }
        public CAL_PoliticoEntidad GetPoliticoPorDNI(string dni)
        {
            return capaDatos.GetPoliticoPorDNI(dni);
        }
    }
}
