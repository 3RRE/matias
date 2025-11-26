using CapaDatos.ClienteSatisfaccion.Flujo;
using CapaDatos.ClienteSatisfaccion.JefeSala;
using CapaEntidad.ClienteSatisfaccion.Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.ClienteSatisfaccion.JefeSala {
    public class CSJefeSalaBL {
        private CsJefeSalaDAL csJefeSalaDAL = new CsJefeSalaDAL();

        public List<CSJefeSalaEntidad> ListarJefesSala(int salaId)  {
            return csJefeSalaDAL.ListarJefesSala(salaId);
        }
        public bool EliminarJefeSala(int idJefeSala) {
            return csJefeSalaDAL.EliminarJefeSala(idJefeSala);
        }

        public int CrearJefeSala(CSJefeSalaEntidad jefeSala) { 
            return csJefeSalaDAL.CrearJefeSala(jefeSala);
        }
        public bool EditarJefeSala(CSJefeSalaEntidad jefeSala) {
            return csJefeSalaDAL.EditarJefeSala(jefeSala);
        }

    }
}
