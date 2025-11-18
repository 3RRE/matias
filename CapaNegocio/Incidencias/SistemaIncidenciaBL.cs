using CapaDatos.Incidencias;
using CapaEntidad.Incidencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Incidencias
{
    public class SistemaIncidenciaBL
    {
        private SistemaIncidenciaDAL _sistemaIncidenciaDal = new SistemaIncidenciaDAL();

        public List <SistemaIncidenciaEntidad> ListarSistemasIncidencias()
        {
            return _sistemaIncidenciaDal.SistemaIncidenciaListado();
        }

        public int InsertarSistemaIncidencia(SistemaIncidenciaEntidad sistema)
        {
            return _sistemaIncidenciaDal.GuardarSistemaIncidencia(sistema);
        }
    }
}
