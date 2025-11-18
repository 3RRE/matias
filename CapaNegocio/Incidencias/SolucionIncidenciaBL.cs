using CapaDatos.Incidencias;
using CapaEntidad.Incidencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Incidencias
{
    public class SolucionIncidenciaBL
    {
        private SolucionIncidenciaDAL _solucionIncidenciaDal = new SolucionIncidenciaDAL();

        public List<SolucionIncidenciaEntidad> ListarSolucionIncidencias(int idIncidencia)
        {
            return _solucionIncidenciaDal.SolucionIncidenciaListado(idIncidencia);
        }

        public int InsertarSolucionIncidencia(SolucionIncidenciaEntidad solucion)
        {
            return _solucionIncidenciaDal.GuardarSolucion(solucion);
        }
    }
}
