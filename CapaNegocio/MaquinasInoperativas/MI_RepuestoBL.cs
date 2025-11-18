using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.MaquinasInoperativas {
    public class MI_RepuestoBL {

        MI_RepuestoDAL capaDatos = new MI_RepuestoDAL();

        public List<MI_RepuestoEntidad> RepuestoListadoCompletoJson() {
            return capaDatos.GetAllRepuesto();
        }
        public List<MI_RepuestoEntidad> RepuestoListadoActiveJson() {
            return capaDatos.GetAllRepuestoActive();
        }
        public List<MI_RepuestoEntidad> RepuestoListadoxCategoriaJson(int cod) {
            return capaDatos.GetAllRepuestoxCategoria(cod);
        }
        public MI_RepuestoEntidad RepuestoCodObtenerJson(int cod) {
            return capaDatos.GetCodRepuesto(cod);
        }
        public int RepuestoInsertarJson(MI_RepuestoEntidad Entidad) {
            var cod = capaDatos.InsertarRepuesto(Entidad);

            return cod;
        }
        public bool RepuestoEditarJson(MI_RepuestoEntidad Entidad) {
            var status = capaDatos.EditarRepuesto(Entidad);

            return status;
        }
        public bool RepuestoEliminarJson(int cod) {
            var status = capaDatos.EliminarRepuesto(cod);

            return status;
        }
    }
}
