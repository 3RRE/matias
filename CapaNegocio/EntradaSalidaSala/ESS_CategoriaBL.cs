using CapaDatos.EntradaSalidaSala;
using CapaEntidad.EntradaSalidaSala;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.EntradaSalidaSala {
    public class ESS_CategoriaBL {
        private readonly ESS_CategoriaDAL _essCategoriaDAL;
        public ESS_CategoriaBL() {
            _essCategoriaDAL = new ESS_CategoriaDAL();
        }
        public List<ESS_CategoriaEntidad> ListarCategoria() => _essCategoriaDAL.ListarCategoria();
        public List<ESS_CategoriaEntidad> ListarCategoriaPorEstado(int estado) => _essCategoriaDAL.ListarCategoriaPorEstado(estado);
        public ESS_CategoriaEntidad ObtenerCategoriaPorId(int id) => _essCategoriaDAL.ObtenerCategoriaPorId(id);
        public int InsertarCategoria(ESS_CategoriaEntidad model) => _essCategoriaDAL.InsertarCategoria(model);
        public bool EditarCategoria(ESS_CategoriaEntidad model) => _essCategoriaDAL.EditarCategoria(model);
    }
}
