using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos.ControlAcceso;
using CapaEntidad.ControlAcceso;

namespace CapaNegocio.ControlAcceso
{
    public class CAL_TipoTimadorBL
    {
        CAL_TipoTimadorDAL capaDatos = new CAL_TipoTimadorDAL();

        public List<CAL_TipoTimadorEntidad> TipoTimadorListadoCompletoJson()
        {
            return capaDatos.GetAllTipoTimador();
        }
    }
}
