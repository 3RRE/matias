using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.MaquinasInoperativas {
    public class MI_PiezaBL {

        MI_PiezaDAL capaDatos = new MI_PiezaDAL();

        public List<MI_PiezaEntidad> PiezaListadoCompletoJson() {
            return capaDatos.GetAllPieza();
        }
        public List<MI_PiezaEntidad> PiezaListadoActiveJson() {
            return capaDatos.GetAllPiezaActive();
        }
        public List<MI_PiezaEntidad> PiezaListadoxCategoriaJson(int cod) {
            return capaDatos.GetAllPiezaxCategoria(cod);
        }
        public MI_PiezaEntidad PiezaCodObtenerJson(int cod) {
            return capaDatos.GetCodPieza(cod);
        }
        public int PiezaInsertarJson(MI_PiezaEntidad Entidad) {
            var cod = capaDatos.InsertarPieza(Entidad);

            return cod;
        }
        public bool PiezaEditarJson(MI_PiezaEntidad Entidad) {
            var status = capaDatos.EditarPieza(Entidad);

            return status;
        }
        public bool PiezaEliminarJson(int cod) {
            var status = capaDatos.EliminarPieza(cod);

            return status;
        }
    }
}
