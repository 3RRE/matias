using CapaDatos.Cortesias;
using CapaEntidad.Cortesias;
using System.Collections.Generic;

namespace CapaNegocio.Cortesias {
    public class CRT_SubTipoBL {

        private readonly CRT_SubTipoDAL subTipoDAL;

        public CRT_SubTipoBL() {
            subTipoDAL = new CRT_SubTipoDAL();
        }

        public List<CRT_SubTipo> ObtenerSubTipos() {
            return subTipoDAL.ObtenerSubTipos();
        }

        public List<CRT_SubTipo> ObtenerSubTiposPorIdsTipo(List<int> idsTipo) {
            string idsTipoStr = string.Join(",", idsTipo);
            return subTipoDAL.ObtenerSubTiposPorIdsTipo(idsTipoStr);
        }

        public CRT_SubTipo ObtenerSubTipoPorId(int id) {
            return subTipoDAL.ObtenerSubTipoPorId(id);
        }

        public bool InsertarSubTipo(CRT_SubTipo subTipo) {
            return subTipoDAL.InsertarSubTipo(subTipo) != 0;
        }

        public bool ActualizarSubTipo(CRT_SubTipo subTipo) {
            return subTipoDAL.ActualizarSubTipo(subTipo) != 0;
        }

        public bool EliminarSubTipo(int id) {
            return subTipoDAL.EliminarSubTipo(id) != 0;
        }
    }
}
