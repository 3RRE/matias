using CapaDatos.ClienteSatisfaccion.Opciones;
using CapaEntidad.ClienteSatisfaccion.Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.ClienteSatisfaccion.Opciones {
    public class OpcionesBL {
      private OpcionesDAL opcionDAL = new OpcionesDAL();

        public List<OpcionEntidad> ListadoOpciones() {
            return opcionDAL.ListadoOpciones();
        }
        public int CrearOpcion(OpcionEntidad opcion) {
            return opcionDAL.CrearOpcion(opcion);
        }

        public bool EditarOpcion(OpcionEntidad opcion) {
            return opcionDAL.EditarOpcion(opcion);
        }

        public bool EliminarOpcion(int idOpcion) {
            return opcionDAL.EliminarOpcion(idOpcion);
        }
    }
}
