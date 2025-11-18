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
    public class CAL_CargoEntidadBL
    {
        CAL_CargoEntidadDAL capaDatos = new CAL_CargoEntidadDAL();

        public List<CAL_CargoEntidadEntidad> CargoEntidadListadoCompletoJson()
        {
            return capaDatos.GetAllCargoEntidad();
        }
        public CAL_CargoEntidadEntidad CargoEntidadIdObtenerJson(int id)
        {
            return capaDatos.GetIDCargoEntidad(id);
        }
        public int CargoEntidadInsertarJson(CAL_CargoEntidadEntidad Entidad)
        {
            var id = capaDatos.InsertarCargoEntidad(Entidad);

            return id;
        }
        public bool CargoEntidadEditarJson(CAL_CargoEntidadEntidad Entidad)
        {
            var status = capaDatos.EditarCargoEntidad(Entidad);

            return status;
        }
        public bool CargoEntidadEliminarJson(int id)
        {
            var status = capaDatos.EliminarCargoEntidad(id);

            return status;
        }
    }
}
