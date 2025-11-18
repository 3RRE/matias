using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.MaquinasInoperativas {
    public class MI_CategoriaRepuestoBL {

        MI_CategoriaRepuestoDAL capaDatos = new MI_CategoriaRepuestoDAL();

        public List<MI_CategoriaRepuestoEntidad> CategoriaRepuestoListadoCompletoJson() {
            return capaDatos.GetAllCategoriaRepuesto();
        }
        public List<MI_CategoriaRepuestoEntidad> CategoriaRepuestoListadoActiveJson() {
            return capaDatos.GetAllCategoriaRepuestoActive();
        }
        public MI_CategoriaRepuestoEntidad CategoriaRepuestoCodObtenerJson(int cod) {
            return capaDatos.GetCodCategoriaRepuesto(cod);
        }
        public int CategoriaRepuestoInsertarJson(MI_CategoriaRepuestoEntidad Entidad) {
            var cod = capaDatos.InsertarCategoriaRepuesto(Entidad);

            return cod;
        }
        public bool CategoriaRepuestoEditarJson(MI_CategoriaRepuestoEntidad Entidad) {
            var status = capaDatos.EditarCategoriaRepuesto(Entidad);

            return status;
        }
        public bool CategoriaRepuestoEliminarJson(int cod) {
            var status = capaDatos.EliminarCategoriaRepuesto(cod);

            return status;
        }
    }
}
