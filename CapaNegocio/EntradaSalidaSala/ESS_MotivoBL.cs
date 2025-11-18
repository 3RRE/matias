using CapaDatos.EntradaSalidaSala;
using CapaEntidad.EntradaSalidaSala;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.EntradaSalidaSala {
    public class ESS_MotivoBL {
        private readonly ESS_MotivoDAL _essMotivoDAL;
        public ESS_MotivoBL() {
            _essMotivoDAL = new ESS_MotivoDAL();
        }
        public List<ESS_MotivoEntidad> ListarMotivo() => _essMotivoDAL.ListarMotivo();
        public List<ESS_MotivoEntidad> ListarMotivoPorEstado(int estado) => _essMotivoDAL.ListarMotivoPorEstado(estado);
        public ESS_MotivoEntidad ObtenerMotivoPorId(int id) => _essMotivoDAL.ObtenerMotivoPorId(id);
        public int InsertarMotivo(ESS_MotivoEntidad model) => _essMotivoDAL.InsertarMotivo(model);
        public bool EditarMotivo(ESS_MotivoEntidad model) => _essMotivoDAL.EditarMotivo(model);
    }
}
