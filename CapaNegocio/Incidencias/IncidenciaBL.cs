using CapaDatos.Incidencias;
using CapaEntidad.Incidencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Incidencias
{
    public class IncidenciaBL
    {
        private IncidenciaDAL _incidenciaDal = new IncidenciaDAL();

        public List<IncidenciaEntidad> ListarIncidencias(int idSistema)
        {
            return _incidenciaDal.IncidenciaListado(idSistema);
        }

        public int InsertarIncidencia(IncidenciaEntidad sistema)
        {
            return _incidenciaDal.GuardarIncidencia(sistema);
        }
    }
}
