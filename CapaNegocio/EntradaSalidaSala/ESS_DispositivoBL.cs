using CapaDatos.EntradaSalidaSala;
using CapaEntidad.EntradaSalidaSala;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.EntradaSalidaSala {
    public class ESS_DispositivoBL {
        private readonly ESS_DispositivoDAL _essDispositivoDAL;
        public ESS_DispositivoBL() {
            _essDispositivoDAL = new ESS_DispositivoDAL();
        }
        public List<ESS_DispositivoEntidad> ListarDispositivo() => _essDispositivoDAL.ListarDispositivo();
        public List<ESS_DispositivoEntidad> ListarDispositivoPorEstado(int estado) => _essDispositivoDAL.ListarDispositivoPorEstado(estado);
        public ESS_DispositivoEntidad ObtenerDispositivoPorId(int id) => _essDispositivoDAL.ObtenerDispositivoPorId(id);
        public int InsertarDispositivo(ESS_DispositivoEntidad model) => _essDispositivoDAL.InsertarDispositivo(model);
        public bool EditarDispositivo(ESS_DispositivoEntidad model) => _essDispositivoDAL.EditarDispositivo(model);
    }
}
