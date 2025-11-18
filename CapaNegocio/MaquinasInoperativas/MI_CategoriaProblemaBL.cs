using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.MaquinasInoperativas {
    public class MI_CategoriaProblemaBL {

        MI_CategoriaProblemaDAL capaDatos = new MI_CategoriaProblemaDAL();

        public List<MI_CategoriaProblemaEntidad> CategoriaProblemaListadoCompletoJson() {
            return capaDatos.GetAllCategoriaProblema();
        }
        public List<MI_CategoriaProblemaEntidad> CategoriaProblemaListadoActiveJson() {
            return capaDatos.GetAllCategoriaProblemaActive();
        }
        public MI_CategoriaProblemaEntidad CategoriaProblemaCodObtenerJson(int cod) {
            return capaDatos.GetCodCategoriaProblema(cod);
        }
        public int CategoriaProblemaInsertarJson(MI_CategoriaProblemaEntidad Entidad) {
            var cod = capaDatos.InsertarCategoriaProblema(Entidad);

            return cod;
        }
        public bool CategoriaProblemaEditarJson(MI_CategoriaProblemaEntidad Entidad) {
            var status = capaDatos.EditarCategoriaProblema(Entidad);

            return status;
        }
        public bool CategoriaProblemaEliminarJson(int cod) {
            var status = capaDatos.EliminarCategoriaProblema(cod);

            return status;
        }
    }
}
