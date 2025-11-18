using CapaDatos.ClienteSatisfaccion.Respuesta;
using CapaDatos.ClienteSatisfaccion.Tablet;
using CapaEntidad.ClienteSatisfaccion.Entidad;
using CapaNegocio.ClienteSatisfaccion.Respuesta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.ClienteSatisfaccion.Tablet {
    public class TabletBL {
        private TabletDAL tabletDAL = new TabletDAL();
        public List<TabletEntidad> ListadoTablets(int salaId) {
            return tabletDAL.ListadoTablets(salaId);
        }

        public bool CrearTablet(TabletEntidad tablet)   {
            return tabletDAL.CrearTablet(tablet);

        }

       public bool EditarTablet(string tablet, bool activo, int idTablet) {
                return tabletDAL.EditarTablet(tablet, activo , idTablet);
        }
        public bool ExisteTabletEnSala(int salaId, int tabletId) {
            return tabletDAL.ExisteTabletEnSala(salaId, tabletId);
        }
    }
}
