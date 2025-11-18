using CapaEntidad.Cortesias;
using CapaPresentacion.Utilitarios;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Cortesias {
    public class BaresController : Controller {
        private readonly ServiceCortesias serviceCortesias;

        public BaresController() {
            serviceCortesias = new ServiceCortesias();
        }

        #region Views
        public ActionResult Index() {
            return View("~/Views/Cortesias/Bar/Index.cshtml");
        }

        public async Task<ActionResult> Guardar(int codSala, int id) {
            CRT_Bar bar = await serviceCortesias.GetItemById<CRT_Bar>("GetBarById", codSala, id);
            bar = bar ?? new CRT_Bar();
            //bar ??= new CRT_Bar();
            return View("~/Views/Cortesias/Bar/AddEdit.cshtml", bar);
        }
        #endregion

        #region Methods
        [HttpPost]
        public async Task<JsonResult> GetBares(int codSala) {
            bool success = false;
            string displayMessage;
            List<CRT_Bar> data = new List<CRT_Bar>();

            try {
                data = await serviceCortesias.GetList<CRT_Bar>("GetBares", codSala);
                success = data.Count > 0;
                displayMessage = success ? "Obtenido correctamente" : "No se pudo obtener";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public async Task<JsonResult> GetBarById(int codSala, int id) {
            bool success = false;
            string displayMessage;
            CRT_Bar data = new CRT_Bar();

            try {
                data = await serviceCortesias.GetItemById<CRT_Bar>("GetBarById", codSala, id);
                success = data.Existe();
                displayMessage = success ? "Obtenido correctamente" : "No se pudo obtener";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public async Task<JsonResult> SaveBar(int codSala, CRT_Bar item) {
            bool success = false;
            string displayMessage;

            try {
                success = await serviceCortesias.CreateOrUpdate<CRT_Bar>("SaveBar", codSala, item);
                displayMessage = success ? "Guardado correctamente" : "No se pudo guardar";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public async Task<JsonResult> DeleteBar(int codSala, int id) {
            bool success = false;
            string displayMessage;

            try {
                success = await serviceCortesias.Delete<CRT_Bar>("DeleteBar", codSala, id);
                displayMessage = success ? "Eliminado correctamente" : "No se pudo eliminar";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage });
        }
        #endregion
    }
}
