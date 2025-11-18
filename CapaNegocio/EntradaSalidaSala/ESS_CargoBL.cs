using CapaDatos.EntradaSalidaSala;
using CapaEntidad.EntradaSalidaSala;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.EntradaSalidaSala {
    public class ESS_CargoBL {
        private readonly ESS_CargoExternoDAL _essCargoDAL;
        public ESS_CargoBL() {
            _essCargoDAL = new ESS_CargoExternoDAL();
        }
        public List<ESS_CargoEntidad> ListarCargo() => _essCargoDAL.ListarCargo();
        public List<ESS_CargoEntidad> ListarCargoPorEstado(int estado)  => _essCargoDAL.ListarCargoPorEstado(estado);
        public ESS_CargoEntidad ObtenerCargoPorId(int id) => _essCargoDAL.ObtenerCargoPorId(id);
        public int InsertarCargo(ESS_CargoEntidad model) => _essCargoDAL.InsertarCargo(model);
        public bool EditarCargo(ESS_CargoEntidad model) => _essCargoDAL.EditarCargo(model);
    }
}
