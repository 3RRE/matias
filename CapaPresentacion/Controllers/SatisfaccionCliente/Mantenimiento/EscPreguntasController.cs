using CapaEntidad;
using CapaEntidad.SatisfaccionCliente.DTO.Mantenedores;
using CapaEntidad.SatisfaccionCliente.Entity.Mantenedores;
using CapaNegocio;
using CapaNegocio.SatisfaccionCliente.Mantenedores;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.SatisfaccionCliente.Mantenimiento {
    [seguridad(true)]
    public class EscPreguntasController : Controller {
        private readonly ESC_PreguntaBL preguntaBL;
        private readonly SalaBL salaBL;

        public EscPreguntasController() {
            preguntaBL = new ESC_PreguntaBL();
            salaBL = new SalaBL();
        }

        #region Views
        public ActionResult PreguntasView() {
            return View("~/Views/SatisfaccionCliente/Preguntas/Preguntas.cshtml");
        }

        public ActionResult AgregarEditarPreguntaView(int id = 0) {
            ESC_PreguntaDto pregunta = id == 0 ? new ESC_PreguntaDto() : preguntaBL.ObtenerPreguntaPorId(id);
            return View("~/Views/SatisfaccionCliente/Preguntas/PreguntasAddEdit.cshtml", pregunta);
        }
        #endregion

        #region Methods
        [seguridad(false)] //debido a que NWO hace uso del metodo
        [HttpPost]
        public JsonResult GetPreguntasByCodSala(int codSala) {
            bool success = false;
            string displayMessage;
            List<ESC_PreguntaDto> data = new List<ESC_PreguntaDto>();

            try {
                SalaEntidad sala = salaBL.ObtenerSalaPorCodigo(codSala);
                if(!sala.Existe()) {
                    success = false;
                    displayMessage = $"No existe sala con el código {codSala}.";
                    return Json(new { success, displayMessage });
                }

                data = preguntaBL.ObtenerPreguntasPorCodSala(codSala);
                success = data.Count > 0;
                displayMessage = success ? $"Preguntas de la sala {sala.Nombre}." : $"La sala {sala.Nombre} aún no tiene preguntas registradas.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult GetPreguntaById(int id) {
            bool success = false;
            string displayMessage;
            ESC_PreguntaDto data = new ESC_PreguntaDto();

            try {
                data = preguntaBL.ObtenerPreguntaPorId(id);
                success = data.Existe();
                displayMessage = success ? "Pregunta encontrada." : "No se encontró la pregunta.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult SavePregunta(ESC_Pregunta pregunta) {
            bool success = false;
            string displayMessage;
            bool isEdit = pregunta.Existe();

            try {
                success = isEdit ? preguntaBL.ActualizarPregunta(pregunta) : preguntaBL.InsertarPregunta(pregunta);
                displayMessage = success ? "Pregunta guardada correctamente." : "No se pudo guardar la pregunta.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult DeletePregunta(int id) {
            bool success = false;
            string displayMessage;

            try {
                success = preguntaBL.EliminarPregunta(id);
                displayMessage = success ? "Pregunta eliminada correctamente." : "No se pudo eliminar la pregunta.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage });
        }
        #endregion
    }
}
