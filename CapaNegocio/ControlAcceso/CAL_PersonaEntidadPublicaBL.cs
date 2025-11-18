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
    public class CAL_PersonaEntidadPublicaBL
    {
        CAL_PersonaEntidadPublicaDAL capaDatos = new CAL_PersonaEntidadPublicaDAL();

        public List<CAL_PersonaEntidadPublicaEntidad> PersonaEntidadPublicaListadoCompletoJson()
        {
            return capaDatos.GetAllPersonaEntidadPublica();
        }
        public CAL_PersonaEntidadPublicaEntidad PersonaEntidadPublicaIdObtenerJson(int id)
        {
            return capaDatos.GetIDPersonaEntidadPublica(id);
        }
        public int PersonaEntidadPublicaInsertarJson(CAL_PersonaEntidadPublicaEntidad Entidad)
        {
            var id = capaDatos.InsertarPersonaEntidadPublica(Entidad);

            return id;
        }
        public bool PersonaEntidadPublicaEditarJson(CAL_PersonaEntidadPublicaEntidad Entidad)
        {
            var status = capaDatos.EditarPersonaEntidadPublica(Entidad);

            return status;
        }
        public bool PersonaEntidadPublicaEliminarJson(int id)
        {
            var status = capaDatos.EliminarPersonaEntidadPublica(id);

            return status;
        }
        public CAL_PersonaEntidadPublicaEntidad GetPersonaEntidadPublicaPorDNI(string dni)
        {
            return capaDatos.GetPersonaEntidadPublicaPorDNI(dni);
        }
    }
}
