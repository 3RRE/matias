using CapaDatos.EntradaSalidaSala;
using CapaEntidad.EntradaSalidaSala;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.EntradaSalidaSala {
    public class ESS_DeficienciaBL {
        private readonly ESS_DeficienciaDAL _essDeficienciaDAL;
        public ESS_DeficienciaBL() {
            _essDeficienciaDAL = new ESS_DeficienciaDAL();
        }
        public List<ESS_DeficienciaEntidad> ListarDeficiencia() => _essDeficienciaDAL.ListarDeficiencia();
        public List<ESS_DeficienciaEntidad> ListarDeficienciaPorEstado(int estado) => _essDeficienciaDAL.ListarDeficienciaPorEstado(estado);
        public ESS_DeficienciaEntidad ObtenerDeficienciaPorId(int id) => _essDeficienciaDAL.ObtenerDeficienciaPorId(id);
        public int InsertarDeficiencia(ESS_DeficienciaEntidad model) => _essDeficienciaDAL.InsertarDeficiencia(model);
        public bool EditarDeficiencia(ESS_DeficienciaEntidad model) => _essDeficienciaDAL.EditarDeficiencia(model);
    }
}
