using CapaDatos.Cortesias;
using CapaEntidad.Cortesias;
using System.Collections.Generic;

namespace CapaNegocio.Cortesias {
    public class CRT_TipoBL {

        private readonly CRT_TipoDAL tipoDAL;

        public CRT_TipoBL() {
            tipoDAL = new CRT_TipoDAL();
        }

        public List<CRT_Tipo> ObtenerTipos() {
            return tipoDAL.ObtenerTipos();
        }

        public CRT_Tipo ObtenerTipoPorId(int id) {
            return tipoDAL.ObtenerTipoPorId(id);
        }

        public bool InsertarTipo(CRT_Tipo tipo) {
            return tipoDAL.InsertarTipo(tipo) != 0;
        }

        public bool ActualizarTipo(CRT_Tipo tipo) {
            return tipoDAL.ActualizarTipo(tipo) != 0;
        }

        public bool EliminarTipo(int id) {
            return tipoDAL.EliminarTipo(id) != 0;
        }
    }
}
