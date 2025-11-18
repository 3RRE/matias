using CapaDatos.EntradaSalidaSala;
using CapaEntidad.EntradaSalidaSala;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.EntradaSalidaSala {
    public class ESS_EnteReguladorMotivoBL {
        private readonly ESS_EnteReguladorMotivoDAL _essMotivoDAL;
        public ESS_EnteReguladorMotivoBL() {
            _essMotivoDAL = new ESS_EnteReguladorMotivoDAL();
        }
        public List<ESS_EnteReguladorMotivoEntidad> ListarMotivo() => _essMotivoDAL.ListarMotivo();
        public List<ESS_EnteReguladorMotivoEntidad> ListarMotivoPorEstado(int estado) => _essMotivoDAL.ListarMotivoPorEstado(estado);
        public ESS_EnteReguladorMotivoEntidad ObtenerMotivoPorId(int id) => _essMotivoDAL.ObtenerMotivoPorId(id);
        public int InsertarMotivo(ESS_EnteReguladorMotivoEntidad model) => _essMotivoDAL.InsertarMotivo(model);
        public bool EditarMotivo(ESS_EnteReguladorMotivoEntidad model) => _essMotivoDAL.EditarMotivo(model);
    }
}
