using CapaDatos.MaquinasInoperativas;
using CapaEntidad.MaquinasInoperativas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.MaquinasInoperativas {
    public class MI_CorreoBL {
        MI_CorreoDAL capaDatos = new MI_CorreoDAL();
        public List<MI_CorreoEntidad> GetCorreosxMaquina(int codMaquinaInoperativa) {
            var lista = capaDatos.GetCorreosxMaquina(codMaquinaInoperativa);
            return lista;
        }
        public bool AgregarCorreo(MI_CorreoEntidad correo) {
            var status = capaDatos.AgregarCorreo(correo);
            return status;
        }
        public bool ActulizarCantEnviosCorreo(int codCorreo) {
            var status = capaDatos.ActulizarCantEnviosCorreo(codCorreo);
            return status;
        }

    }
}
