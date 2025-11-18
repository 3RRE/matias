using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class NivelTecnicoBL
    {
        public NivelTecnicoDAL nivelTecnicoDAL = new NivelTecnicoDAL();
        public List<NivelTecnicoEntidad> NivelTecnicoListarJson()
        {
            return nivelTecnicoDAL.NivelTecnicoListarJson();
        }
    }
}
