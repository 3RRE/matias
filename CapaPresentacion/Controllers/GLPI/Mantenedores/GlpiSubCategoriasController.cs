using CapaEntidad.GLPI.Mantenedores;
using CapaNegocio.GLPI.Mantenedores;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.GLPI.Mantenedores {
    [seguridad(false)]
    public class GlpiSubCategoriasController : Controller {
        private readonly GLPI_SubCategoriaBL categoriaBL;

        public GlpiSubCategoriasController() {
            categoriaBL = new GLPI_SubCategoriaBL();
        }

        #region Methods
        [HttpPost]
        public JsonResult GetSubCategorias() {
            bool success = false;
            string displayMessage;
            List<GLPI_SubCategoria> data = new List<GLPI_SubCategoria>();

            try {
                data = categoriaBL.ObtenerSubCategorias();
                success = data.Count > 0;
                displayMessage = success ? "Lista de sub categorías." : "No hay sub categorías registradas.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult GetSubCategoriaById(int id) {
            bool success = false;
            string displayMessage;
            GLPI_SubCategoria data = new GLPI_SubCategoria();

            try {
                data = categoriaBL.ObtenerSubCategoriaPorId(id);
                success = data.Existe();
                displayMessage = success ? "Sub categoría encontrada." : "No se encontró la sub categoría.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult SaveSubCategoria(GLPI_SubCategoria categoria) {
            bool success = false;
            string displayMessage;
            bool isEdit = categoria.Existe();

            try {
                success = isEdit ? categoriaBL.ActualizarSubCategoria(categoria) : categoriaBL.InsertarSubCategoria(categoria);
                displayMessage = success ? "Sub categoría guardada correctamente." : "No se pudo guardar la sub categoría.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult DeleteSubCategoria(int id) {
            bool success = false;
            string displayMessage;

            try {
                success = categoriaBL.EliminarSubCategoria(id);
                displayMessage = success ? "Sub categoría eliminada correctamente." : "No se pudo eliminar la sub categoría.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage });
        }
        #endregion
    }
}
