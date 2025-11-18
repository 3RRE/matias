using CapaDatos.MaquinasInoperativas;
using CapaEntidad.MaquinasInoperativas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.MaquinasInoperativas {
    public class MI_ComentarioBL {

        MI_ComentarioDAL capaDatos = new MI_ComentarioDAL();
        public List<MI_ComentarioEntidad> GetAllComentariosxMaquina(int cod) {
            return capaDatos.GetAllComentariosxMaquina(cod);
        }
        public MI_ComentarioEntidad GetComentarioxCod(int cod) {
            return capaDatos.GetComentarioxCod(cod);
        }
        public int ComentarioInsertarJson(MI_ComentarioEntidad Entidad) {
            var cod = capaDatos.InsertarComentario(Entidad);
            return cod;
        }
        public bool ComentarioEliminarJson(int cod) {
            var status = capaDatos.EliminarComentario(cod);
            return status;
        }

    }
}
