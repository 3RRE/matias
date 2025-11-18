using CapaDatos.GLPI.Mantenedores;
using CapaEntidad.GLPI.Helper;
using CapaEntidad.GLPI.Mantenedores;
using System.Collections.Generic;
using System.Linq;

namespace CapaNegocio.GLPI.Mantenedores {
    public class GLPI_CategoriaBL {
        private readonly GLPI_CategoriaDAL categoriaDAL;

        public GLPI_CategoriaBL() {
            categoriaDAL = new GLPI_CategoriaDAL();
        }

        public List<GLPI_Categoria> ObtenerCategorias() {
            return categoriaDAL.ObtenerCategorias();
        }

        public List<GLPI_SelectHelper> ObtenerCategoriasSelect() {
            List<GLPI_SelectHelper> options = ObtenerCategorias()
                .Select(x => new GLPI_SelectHelper {
                    IdPadre = x.ObtenerIdPadreSelect(),
                    Valor = x.ObtenerValorSelect(),
                    Texto = x.ObtenerTextoSelect()
                }).ToList();
            return options;
        }

        public GLPI_Categoria ObtenerCategoriaPorId(int id) {
            return categoriaDAL.ObtenerCategoriaPorId(id);
        }

        public bool InsertarCategoria(GLPI_Categoria categoria) {
            return categoriaDAL.InsertarCategoria(categoria) > 0;
        }

        public bool ActualizarCategoria(GLPI_Categoria categoria) {
            return categoriaDAL.ActualizarCategoria(categoria) > 0;
        }

        public bool EliminarCategoria(int id) {
            return categoriaDAL.EliminarCategoria(id) > 0;
        }
    }
}
