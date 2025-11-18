using CapaDatos.ClienteSatisfaccion.Flujo;
using CapaDatos.ClienteSatisfaccion.Opciones;
using CapaEntidad.ClienteSatisfaccion.Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.ClienteSatisfaccion.Flujo {
    public class FlujoBL {
        private FlujoDAL flujoDAL = new FlujoDAL();

        public List<FlujoEntidad> ListadoFlujoEncuesta(int TipoEncuesta) {
            return flujoDAL.ListadoFlujoEncuesta(TipoEncuesta);
        }
    }
}
