using CapaEntidad.GLPI.Mantenedores;
using CapaNegocio.GLPI.Mantenedores;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.GLPI.Mantenedores {
    [seguridad(false)]
    public class GlpiCategoriasController : Controller {
        private readonly GLPI_CategoriaBL categoriaBL;

        public GlpiCategoriasController() {
            categoriaBL = new GLPI_CategoriaBL();
        }

        #region Methods
        [HttpPost]
        public JsonResult GetCategorias() {
            bool success = false;
            string displayMessage;
            List<GLPI_Categoria> data = new List<GLPI_Categoria>();

            try {
                data = categoriaBL.ObtenerCategorias();
                success = data.Count > 0;
                displayMessage = success ? "Lista de categorías." : "No hay categorías registradas.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult GetCategoriaById(int id) {
            bool success = false;
            string displayMessage;
            GLPI_Categoria data = new GLPI_Categoria();

            try {
                data = categoriaBL.ObtenerCategoriaPorId(id);
                success = data.Existe();
                displayMessage = success ? "Categoría encontrada." : "No se encontró la categoría.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult SaveCategoria(GLPI_Categoria categoria) {
            bool success = false;
            string displayMessage;
            bool isEdit = categoria.Existe();

            try {
                success = isEdit ? categoriaBL.ActualizarCategoria(categoria) : categoriaBL.InsertarCategoria(categoria);
                displayMessage = success ? "Categoría guardada correctamente." : "No se pudo guardar la categoría.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult DeleteCategoria(int id) {
            bool success = false;
            string displayMessage;

            try {
                success = categoriaBL.EliminarCategoria(id);
                displayMessage = success ? "Categoría eliminada correctamente." : "No se pudo eliminar la categoría.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage });
        }
        #endregion
    }
}
