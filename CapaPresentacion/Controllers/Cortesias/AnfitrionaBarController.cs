using CapaEntidad;
using CapaEntidad.Cortesias;
using CapaNegocio;
using CapaPresentacion.Controllers;
using CapaPresentacion.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Windows.Documents;

namespace CapaPresentacion.Controllers.Cortesias {

    public class AnfitrionaBarController : Controller {

        private readonly ServiceCortesias serviceCortesias;

        public AnfitrionaBarController() {
            serviceCortesias = new ServiceCortesias();
        }

        #region Views
        public ActionResult Index() {
            return View("~/Views/Cortesias/AnfitrionaBar.cshtml");
        }

        #endregion

        [HttpPost]
        public async Task<JsonResult> GetAnfitrionas(int codSala) {
            bool success = false;
            string displayMessage;

            List<CRT_Usuario> listAnfitrionas = new List<CRT_Usuario>();

            try {
                listAnfitrionas = await serviceCortesias.GetList<CRT_Usuario>("GetEmpleadosAnfitriona", codSala);
                success = true;
                displayMessage = success ? "Obtenido correctamente" : "No se pudo obtener";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new {
                success,
                displayMessage,
                data = new {
                    rooms = listAnfitrionas
                }
            });
        }

        [HttpPost]
        public async Task<JsonResult> GetEmpleadosBar(int codSala) {
            bool success = false;
            string message = "Error al obtener empleados";

            List<SEG_UsuarioEntidad> list = new List<SEG_UsuarioEntidad>();

            try {
                list = await serviceCortesias.GetList<SEG_UsuarioEntidad>("GetEmpleadosBar", codSala);

                success = true;
                message = success ? "Obtenido correctamente" : "No se pudo obtener";
            } catch(Exception ex) {
                message = $"{ex.Message}. Llame al administrador";
            }

            return Json(new {
                success,
                message,
                data = list
            });
        }


        [HttpPost]
        public async Task<JsonResult> GetAnfitrionasBar(int codSala, int id) {
            bool success = false;
            string message = "Error al obtener empleados";

            List<CRT_AnfitrionaBar> list = new List<CRT_AnfitrionaBar>();

            try {
                list = await serviceCortesias.GetListById<CRT_AnfitrionaBar>("GetAnfitrionasBar", codSala, id);
                success = true;
                message = success ? "Obtenido correctamente" : "No se pudo obtener";
            } catch(Exception ex) {
                message = $"{ex.Message}. Llame al administrador";
            }

            return Json(new {
                success,
                message,
                data = list
            });
        }

        [HttpPost]
        public async Task<JsonResult> AnfitrionaBarAsginar(int codSala, int usuarioId, List<int> anfitrionas) {
            bool status = false;
            string message = "Se asignaron las anfitrionas al bar";

            if(anfitrionas == null) {
                return Json(new {
                    status,
                    message = "No hay anfitrionas seleccionadas"
                });
            }

            try {
                status = await serviceCortesias.AsignarAnfitrionas("AsignarAnfitrionasBar", codSala, usuarioId, anfitrionas);

            } catch(Exception exception) {
                message = exception.Message + ", Llame Administrador";
            }

            return Json(new {
                status,
                message
            });
        }


    }
}