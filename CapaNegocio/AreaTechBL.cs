using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class AreaTechBL
    {
        public AreaTechDAL areatechdal = new AreaTechDAL();
        public List<AreaTechEntidad> AreaTechListarJson()
        {
            return areatechdal.AreaTechListarJson();
        }
    }
}
