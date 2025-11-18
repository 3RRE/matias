using CapaDatos.GLPI.Mantenedores;
using CapaEntidad.GLPI.Helper;
using CapaEntidad.GLPI.Mantenedores;
using System.Collections.Generic;
using System.Linq;

namespace CapaNegocio.GLPI.Mantenedores {
    public class GLPI_SubCategoriaBL {
        private readonly GLPI_SubCategoriaDAL subCategoriaDAL;

        public GLPI_SubCategoriaBL() {
            subCategoriaDAL = new GLPI_SubCategoriaDAL();
        }

        public List<GLPI_SubCategoria> ObtenerSubCategorias() {
            return subCategoriaDAL.ObtenerSubCategorias();
        }

        public List<GLPI_SelectHelper> ObtenerSubCategoriasSelect() {
            List<GLPI_SelectHelper> options = ObtenerSubCategorias()
                .Select(x => new GLPI_SelectHelper {
                    IdPadre = x.ObtenerIdPadreSelect(),
                    Valor = x.ObtenerValorSelect(),
                    Texto = x.ObtenerTextoSelect()
                }).ToList();
            return options;
        }

        public GLPI_SubCategoria ObtenerSubCategoriaPorId(int id) {
            return subCategoriaDAL.ObtenerSubCategoriaPorId(id);
        }

        public bool InsertarSubCategoria(GLPI_SubCategoria subCategoria) {
            return subCategoriaDAL.InsertarSubCategoria(subCategoria) > 0;
        }

        public bool ActualizarSubCategoria(GLPI_SubCategoria subCategoria) {
            return subCategoriaDAL.ActualizarSubCategoria(subCategoria) > 0;
        }

        public bool EliminarSubCategoria(int id) {
            return subCategoriaDAL.EliminarSubCategoria(id) > 0;
        }
    }
}
