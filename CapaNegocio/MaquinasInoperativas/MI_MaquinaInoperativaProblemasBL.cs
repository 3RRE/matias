using CapaDatos.MaquinasInoperativas;
using CapaEntidad.MaquinasInoperativas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.MaquinasInoperativas {
    public class MI_MaquinaInoperativaProblemasBL {

        MI_MaquinaInoperativaProblemasDAL capaDatos = new MI_MaquinaInoperativaProblemasDAL();
        public List<MI_MaquinaInoperativaProblemasEntidad> MaquinaInoperativaProblemasListadoCompletoJson() {
            return capaDatos.GetAllMaquinaInoperativaProblemas();
        }
        public List<MI_MaquinaInoperativaProblemasEntidad> MaquinaInoperativaProblemasListadoActiveJson() {
            return capaDatos.GetAllMaquinaInoperativaProblemasActive();
        }
        public List<MI_MaquinaInoperativaProblemasEntidad> MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(int cod) {
            return capaDatos.GetAllMaquinaInoperativaProblemasxMaquinaInoperativa(cod);
        }
        public MI_MaquinaInoperativaProblemasEntidad MaquinaInoperativaProblemasCodObtenerJson(int cod) {
            return capaDatos.GetCodMaquinaInoperativaProblemas(cod);
        }
        public int MaquinaInoperativaProblemasInsertarJson(MI_MaquinaInoperativaProblemasEntidad Entidad) {
            var cod = capaDatos.InsertarMaquinaInoperativaProblemas(Entidad);

            return cod;
        }
        public bool MaquinaInoperativaProblemasEditarJson(MI_MaquinaInoperativaProblemasEntidad Entidad) {
            var status = capaDatos.EditarMaquinaInoperativaProblemas(Entidad);

            return status;
        }
        public bool MaquinaInoperativaProblemasEliminarJson(int cod) {
            var status = capaDatos.EliminarMaquinaInoperativaProblemas(cod);

            return status;
        }
        public bool MaquinaInoperativaProblemasEliminarxMaquinaJson(int cod) {
            var status = capaDatos.EliminarMaquinaInoperativaProblemasxMaquina(cod);

            return status;
        }

        public bool AceptarMaquinaInoperativaProblemas(int cod)
        {
            var status = capaDatos.AceptarMaquinaInoperativaProblemas(cod);

            return status;
        }
        public bool RechazarMaquinaInoperativaProblemas(int cod)
        {
            var status = capaDatos.RechazarMaquinaInoperativaProblemas(cod);

            return status;
        }

        

    }
}
