using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.MaquinasInoperativas {
    public class MI_CategoriaPiezaBL {

        MI_CategoriaPiezaDAL capaDatos = new MI_CategoriaPiezaDAL();

        public List<MI_CategoriaPiezaEntidad> CategoriaPiezaListadoCompletoJson() {
            return capaDatos.GetAllCategoriaPieza();
        }
        public List<MI_CategoriaPiezaEntidad> CategoriaPiezaListadoActiveJson() {
            return capaDatos.GetAllCategoriaPiezaActive();
        }
        public MI_CategoriaPiezaEntidad CategoriaPiezaCodObtenerJson(int cod) {
            return capaDatos.GetCodCategoriaPieza(cod);
        }
        public int CategoriaPiezaInsertarJson(MI_CategoriaPiezaEntidad Entidad) {
            var cod = capaDatos.InsertarCategoriaPieza(Entidad);

            return cod;
        }
        public bool CategoriaPiezaEditarJson(MI_CategoriaPiezaEntidad Entidad) {
            var status = capaDatos.EditarCategoriaPieza(Entidad);

            return status;
        }
        public bool CategoriaPiezaEliminarJson(int cod) {
            var status = capaDatos.EliminarCategoriaPieza(cod);

            return status;
        }
    }
}
