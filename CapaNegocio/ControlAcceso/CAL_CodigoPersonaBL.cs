using CapaDatos.ControlAcceso;
using CapaEntidad.ControlAcceso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.ControlAcceso
{
    public class CAL_CodigoPersonaBL
    {
        CAL_CodigoPersonaDAL capaDatos = new CAL_CodigoPersonaDAL();

        public List<CAL_CodigoPersonaEntidad> CodigoPersonaListadoCompletoJson()
        {
            return capaDatos.GetAllCodigoPersona();
        }
        public CAL_CodigoPersonaEntidad CodigoPersonaIdObtenerJson(int id)
        {
            return capaDatos.GetIDCodigoPersona(id);
        }
        public int CodigoPersonaInsertarJson(CAL_CodigoPersonaEntidad Entidad)
        {
            var id = capaDatos.InsertarCodigoPersona(Entidad);

            return id;
        }
        public bool CodigoPersonaEditarJson(CAL_CodigoPersonaEntidad Entidad)
        {
            var status = capaDatos.EditarCodigoPersona(Entidad);

            return status;
        }
        public bool CodigoPersonaEliminarJson(int id)
        {
            var status = capaDatos.EliminarCodigoPersona(id);

            return status;
        }
    }
}
