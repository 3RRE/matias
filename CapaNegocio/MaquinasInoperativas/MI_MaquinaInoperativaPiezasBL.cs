using CapaDatos.MaquinasInoperativas;
using CapaEntidad.MaquinasInoperativas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.MaquinasInoperativas {
    public class MI_MaquinaInoperativaPiezasBL {

        MI_MaquinaInoperativaPiezasDAL capaDatos = new MI_MaquinaInoperativaPiezasDAL();
        public List<MI_MaquinaInoperativaPiezasEntidad> MaquinaInoperativaPiezasListadoCompletoJson() {
            return capaDatos.GetAllMaquinaInoperativaPiezas();
        }
        public List<MI_MaquinaInoperativaPiezasEntidad> MaquinaInoperativaPiezasListadoActiveJson() {
            return capaDatos.GetAllMaquinaInoperativaPiezasActive();
        }
        public List<MI_MaquinaInoperativaPiezasEntidad> MaquinaInoperativaPiezasListadoxMaquinaInoperativaJson(int cod) {
            return capaDatos.GetAllMaquinaInoperativaPiezasxMaquinaInoperativa(cod);
        }
        public MI_MaquinaInoperativaPiezasEntidad MaquinaInoperativaPiezasCodObtenerJson(int cod) {
            return capaDatos.GetCodMaquinaInoperativaPiezas(cod);
        }
        public int MaquinaInoperativaPiezasInsertarJson(MI_MaquinaInoperativaPiezasEntidad Entidad) {
            var cod = capaDatos.InsertarMaquinaInoperativaPiezas(Entidad);

            return cod;
        }
        public bool MaquinaInoperativaPiezasEditarJson(MI_MaquinaInoperativaPiezasEntidad Entidad) {
            var status = capaDatos.EditarMaquinaInoperativaPiezas(Entidad);

            return status;
        }
        public bool MaquinaInoperativaPiezasEliminarJson(int cod) {
            var status = capaDatos.EliminarMaquinaInoperativaPiezas(cod);

            return status;
        }
        public bool MaquinaInoperativaPiezasEliminarxMaquinaJson(int cod) {
            var status = capaDatos.EliminarMaquinaInoperativaPiezasxMaquina(cod);

            return status;
        }
    }
}
