using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos.ControlAcceso;
using CapaEntidad.ControlAcceso;

namespace CapaNegocio.ControlAcceso
{
    public class CAL_BandaBL
    {
        CAL_BandaDAL capaDatos = new CAL_BandaDAL();

        public List<CAL_BandaEntidad> BandaListadoCompletoJson()
        {
            return capaDatos.GetAllBanda();
        }
        public CAL_BandaEntidad BandaIdObtenerJson(int id)
        {
            return capaDatos.GetIDBanda(id);
        }
        public int BandaInsertarJson(CAL_BandaEntidad Entidad)
        {
            var id = capaDatos.InsertarBanda(Entidad);

            return id;
        }
        public bool BandaEditarJson(CAL_BandaEntidad Entidad)
        {
            var status = capaDatos.EditarBanda(Entidad);

            return status;
        }
        public bool BandaEliminarJson(int id)
        {
            var status = capaDatos.EliminarBanda(id);

            return status;
        }
    }
}
