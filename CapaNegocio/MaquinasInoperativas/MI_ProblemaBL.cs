using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.MaquinasInoperativas {
    public class MI_ProblemaBL {

        MI_ProblemaDAL capaDatos = new MI_ProblemaDAL();

        public List<MI_ProblemaEntidad> ProblemaListadoCompletoJson() {
            return capaDatos.GetAllProblema();
        }
        public List<MI_ProblemaEntidad> ProblemaListadoActiveJson() {
            return capaDatos.GetAllProblemaActive();
        }
        public List<MI_ProblemaEntidad> ProblemaListadoxCategoriaJson(int cod) {
            return capaDatos.GetAllProblemaxCategoria(cod);
        }
        public List<MI_ProblemaEntidad> ProblemaListadoxCategoriaListaJson(string lista) {
            return capaDatos.GetAllProblemaxCategoriaLista(lista);
        }
        public MI_ProblemaEntidad ProblemaCodObtenerJson(int cod) {
            return capaDatos.GetCodProblema(cod);
        }
        public int ProblemaInsertarJson(MI_ProblemaEntidad Entidad) {
            var cod = capaDatos.InsertarProblema(Entidad);

            return cod;
        }
        public bool ProblemaEditarJson(MI_ProblemaEntidad Entidad) {
            var status = capaDatos.EditarProblema(Entidad);

            return status;
        }
        public bool ProblemaEliminarJson(int cod) {
            var status = capaDatos.EliminarProblema(cod);

            return status;
        }
    }
}
