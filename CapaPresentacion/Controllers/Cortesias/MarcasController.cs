using CapaEntidad.Cortesias;
using CapaNegocio.Cortesias;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Cortesias {
    public class MarcasController : Controller {
        private readonly CRT_MarcaBL marcaBL;

        public MarcasController() {
            marcaBL = new CRT_MarcaBL();
        }

        #region Views
        public ActionResult Index() {
            return View("~/Views/Cortesias/Marca/Index.cshtml");
        }

        public ActionResult Guardar(int id = 0) {
            CRT_Marca marca = id == 0 ? new CRT_Marca() : marcaBL.ObtenerMarcaPorId(id);
            return View("~/Views/Cortesias/Marca/AddEdit.cshtml", marca);
        }
        #endregion

        #region Methods
        [HttpPost]
        public JsonResult GetMarcas() {
            bool success = false;
            string displayMessage;
            List<CRT_Marca> data = new List<CRT_Marca>();

            try {
                data = marcaBL.ObtenerMarcas();
                success = data.Count > 0;
                displayMessage = success ? "Lista de marcas." : "No hay marcas registradas.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult GetMarcaById(int id) {
            bool success = false;
            string displayMessage;
            CRT_Marca data = new CRT_Marca();

            try {
                data = marcaBL.ObtenerMarcaPorId(id);
                success = data.Existe();
                displayMessage = success ? "Marca encontrada." : "No se encontró la marca.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult SaveMarca(CRT_Marca marca) {
            bool success = false;
            string displayMessage;
            bool isEdit = marca.Existe();

            try {
                marca.IdUsuario = Convert.ToInt32(Session["UsuarioID"]);
                success = isEdit ? marcaBL.ActualizarMarca(marca) : marcaBL.InsertarMarca(marca);
                displayMessage = success ? "Marca guardada correctamente." : "No se pudo guardar la marca.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult DeleteMarca(int id) {
            bool success = false;
            string displayMessage;

            try {
                success = marcaBL.EliminarMarca(id);
                displayMessage = success ? "Marca eliminada correctamente." : "No se pudo eliminar la marca.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage });
        }
        #endregion
    }
}
