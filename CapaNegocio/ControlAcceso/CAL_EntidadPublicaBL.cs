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
    public class CAL_EntidadPublicaBL
    {

        CAL_EntidadPublicaDAL capaDatos = new CAL_EntidadPublicaDAL();

        public List<CAL_EntidadPublicaEntidad> EntidadPublicaListadoCompletoJson()
        {
            return capaDatos.GetAllEntidadPublica();
        }
        public CAL_EntidadPublicaEntidad EntidadPublicaIdObtenerJson(int id)
        {
            return capaDatos.GetIDEntidadPublica(id);
        }
        public int EntidadPublicaInsertarJson(CAL_EntidadPublicaEntidad Entidad)
        {
            var id = capaDatos.InsertarEntidadPublica(Entidad);

            return id;
        }
        public bool EntidadPublicaEditarJson(CAL_EntidadPublicaEntidad Entidad)
        {
            var status = capaDatos.EditarEntidadPublica(Entidad);

            return status;
        }
        public bool EntidadPublicaEliminarJson(int id)
        {
            var status = capaDatos.EliminarEntidadPublica(id);

            return status;
        }
    }
}
