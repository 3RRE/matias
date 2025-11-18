using CapaDatos.EntradaSalidaSala;
using CapaEntidad.EntradaSalidaSala;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.EntradaSalidaSala {
    public class ESS_EmpresaBL {
        private readonly ESS_EmpresaExternaDAL _essEmpresaDAL;
        public ESS_EmpresaBL() {
            _essEmpresaDAL = new ESS_EmpresaExternaDAL();
        }
        public List<ESS_EmpresaEntidad> ListarEmpresa() => _essEmpresaDAL.ListarEmpresa();
        public List<ESS_EmpresaEntidad> ListarEmpresaPorEstado(int estado) => _essEmpresaDAL.ListarEmpresaPorEstado(estado);
        public ESS_EmpresaEntidad ObtenerEmpresaPorId(int id) => _essEmpresaDAL.ObtenerEmpresaPorId(id);
        public int InsertarEmpresa(ESS_EmpresaEntidad model) => _essEmpresaDAL.InsertarEmpresa(model);
        public bool EditarEmpresa(ESS_EmpresaEntidad model) => _essEmpresaDAL.EditarEmpresa(model);
    }
}