using CapaDatos.ControlAcceso;
using CapaEntidad.ControlAcceso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.ControlAcceso
{
    public class CAL_TipoExclusionBL
    {
        CAL_TipoExclusionDAL capaDatos = new CAL_TipoExclusionDAL();

        public List<CAL_TipoExclusionEntidad> TipoExclusionListadoCompletoJson()
        {
            return capaDatos.GetAllTipoExclusion();
        }
    }
}
