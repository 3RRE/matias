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
    public class CAL_CargoPoliticoBL
    {
        CAL_CargoPoliticoDAL capaDatos = new CAL_CargoPoliticoDAL();

        public List<CAL_CargoPoliticoEntidad> CargoPoliticoListadoCompletoJson()
        {
            return capaDatos.GetAllCargoPolitico();
        }
        public CAL_CargoPoliticoEntidad CargoPoliticoIdObtenerJson(int id)
        {
            return capaDatos.GetIDCargoPolitico(id);
        }
        public int CargoPoliticoInsertarJson(CAL_CargoPoliticoEntidad Entidad)
        {
            var id = capaDatos.InsertarCargoPolitico(Entidad);

            return id;
        }
        public bool CargoPoliticoEditarJson(CAL_CargoPoliticoEntidad Entidad)
        {
            var status = capaDatos.EditarCargoPolitico(Entidad);

            return status;
        }
        public bool CargoPoliticoEliminarJson(int id)
        {
            var status = capaDatos.EliminarCargoPolitico(id);

            return status;
        }

    }
}
