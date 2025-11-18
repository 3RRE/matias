using CapaEntidad;
using CapaEntidad.Cortesias;
using CapaPresentacion.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Cortesias {

    public class MaquinaAnfitrionaController : Controller {

        private readonly ServiceCortesias serviceCortesias;

        public MaquinaAnfitrionaController() {
            serviceCortesias = new ServiceCortesias();
        }

        #region Views
        public ActionResult Index() {
            return View("~/Views/Cortesias/MaquinaAnfitriona.cshtml");
        }

        #endregion

        [HttpPost]
        public async Task<JsonResult> GetMaquinas(int codSala) {
            bool success = false;
            string displayMessage;

            List<CRT_Maquina> listMaquinas = new List<CRT_Maquina>();
            List<CRT_Zona> listZonas = new List<CRT_Zona>();

            try {
                List<CRT_Maquina> data = await serviceCortesias.GetList<CRT_Maquina>("GetMaquinasZPI", codSala);

                listMaquinas = data;

                List<int> zonas = data.Select(x => x.Zona).Distinct().ToList();

                foreach(var zona in zonas) {
                    CRT_Zona item = new CRT_Zona();
                    item.Id = zona;
                    item.Nombre = data.Where(x=>x.Zona==zona).First().NombreZona;
                    listZonas.Add(item);
                }

                success = true;
                displayMessage = success ? "Obtenido correctamente" : "No se pudo obtener";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new {
                success,
                displayMessage,
                data = new {
                    companies = listZonas,
                    rooms = listMaquinas
                }
            });
        }

        [HttpPost]
        public async Task<JsonResult> GetEmpleadosAnfitriona(int codSala) {
            bool success = false;
            string message = "Error al obtener empleados";

            List<SEG_UsuarioEntidad> list = new List<SEG_UsuarioEntidad>();

            try {
                list= await serviceCortesias.GetList<SEG_UsuarioEntidad>("GetEmpleadosAnfitriona", codSala);

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
        public async Task<JsonResult> GetMaquinasAnfitriona(int codSala, int id) {
            bool success = false;
            string message = "Error al obtener empleados";

            List<CRT_MaquinaAnfitriona> list = new List<CRT_MaquinaAnfitriona>();

            try {
                list = await serviceCortesias.GetListById<CRT_MaquinaAnfitriona>("GetMaquinasAnfitriona", codSala, id);
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
        public async Task<JsonResult> MaquinaAnfitrionaAsginar(int codSala, int usuarioId, List<string> maquinas) {
            bool status = false;
            string message = "Se asignaron las maquinas";

            if(maquinas == null) {
                return Json(new {
                    status,
                    message = "No hay maquinas seleccionadas"
                });
            }

            try {
                status = await serviceCortesias.AsignarMaquinas("AsignarMaquinasAnfitriona", codSala, usuarioId, maquinas);

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