using CapaEntidad.Cortesias;
using CapaNegocio.Cortesias;
using CapaPresentacion.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Cortesias {
    public class AnfitrionasController : Controller {
        private readonly CRT_AnfitrionaBL anfitrionaBL;
        private readonly ServiceCortesias serviceCortesias;
        
        public AnfitrionasController() {
            anfitrionaBL = new CRT_AnfitrionaBL();
            serviceCortesias = new ServiceCortesias();
        }

        #region Views
        public ActionResult Index() {
            return View("~/Views/Cortesias/Anfitrionas.cshtml");
        }

        #endregion

        #region Methods
        [HttpPost]
        public JsonResult GetSalas() {
            bool success = false;
            string displayMessage;
            List<CRT_Sala> data = new List<CRT_Sala>();

            try {
                data = anfitrionaBL.GetSalas();
                success = data.Count > 0;
                displayMessage = success ? "Obtenido correctamente" : "No se encontraron salas";
            }
            catch (Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult GetAnfitrionasBySala(string empresa, string sala) {
            bool success = false;
            string displayMessage;
            List<CRT_Empleado> data = new List<CRT_Empleado>();

            try {
                data = anfitrionaBL.GetAnfitrionasBySala(empresa,sala);
                success = data.Count > 0;
                displayMessage = success ? "Obtenido correctamente" : "No se encontraron anfitrionas";
            }
            catch (Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }


        [HttpPost]
        public async Task<JsonResult> CreateAnfitriona(string empresa, string sala, string id) {
            bool success = false;
            string displayMessage;
            List<CRT_Empleado> data = new List<CRT_Empleado>();

            try {

                var anfitriona = anfitrionaBL.GetAnfitrionasByNroDoc(id);
                var salas = anfitrionaBL.GetSalasByCod(empresa,sala);
                success = await serviceCortesias.CreateAnfitriona("CreateAnfitriona", salas.First().CodSala, anfitriona);
                displayMessage = success ? "Creado Correctamente" : "Ya existe su usuario";
            }
            catch (Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        #endregion
    }
}
