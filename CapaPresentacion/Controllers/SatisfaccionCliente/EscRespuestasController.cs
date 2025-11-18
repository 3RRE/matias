using CapaEntidad;
using CapaEntidad.Response;
using CapaEntidad.SatisfaccionCliente.DTO;
using CapaEntidad.SatisfaccionCliente.DTO.Configuracion;
using CapaEntidad.SatisfaccionCliente.DTO.Request;
using CapaEntidad.SatisfaccionCliente.Enum;
using CapaNegocio;
using CapaNegocio.SatisfaccionCliente;
using CapaNegocio.SatisfaccionCliente.Configuracion;
using CapaNegocio.SatisfaccionCliente.Mantenedores;
using S3k.Utilitario.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.SatisfaccionCliente {
    [seguridad(true)]
    public class EscRespuestasController : Controller {
        private readonly ESC_PreguntaBL preguntaBL;
        private readonly ESC_RespuestaBL respuestaBL;
        private readonly SalaBL salaBL;
        private readonly ESC_ConfiguracionBL configuracionBL;

        public EscRespuestasController() {
            preguntaBL = new ESC_PreguntaBL();
            respuestaBL = new ESC_RespuestaBL();
            salaBL = new SalaBL();
            configuracionBL = new ESC_ConfiguracionBL();
        }

        #region Methods
        [HttpPost]
        public JsonResult GetRespuestasPorCodSala(int codSala) {
            bool success = false;
            string displayMessage;
            List<ESC_RespuestaDto> data = new List<ESC_RespuestaDto>();

            try {
                SalaEntidad sala = salaBL.ObtenerSalaPorCodigo(codSala);
                if(!sala.Existe()) {
                    success = false;
                    displayMessage = $"No existe sala con el código {codSala}.";
                    return Json(new { success, displayMessage });
                }

                data = respuestaBL.ObtenerRespuestasPorCodSala(codSala);
                success = data.Count > 0;
                displayMessage = success ? $"Respuestas de satisfacción de la sala {sala.Nombre}." : $"Aun no hay respuestas de satisfacción para la sala {sala.Nombre}.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult GetRespuestaById(int id) {
            bool success = false;
            string displayMessage;
            ESC_RespuestaDto data = new ESC_RespuestaDto();

            try {
                data = respuestaBL.ObtenerRespuestaPorId(id);
                success = data.Existe();
                displayMessage = success ? "Respuesta encontrada." : "No se encontró la respuesta.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [seguridad(false)] //debido a que NWO hace uso del metodo
        [HttpPost]
        public JsonResult SaveRespuesta(ESC_RespuestaClienteRequest respuestaCliente) {
            bool success = false;
            string displayMessage;
            ResponseCodeToast codeToast = ResponseCodeToast.Success;

            try {
                int codSala = respuestaCliente.Sala.CodSala;
                string numeroDocumento = respuestaCliente.Cliente.NumeroDocumento;
                bool enviaNumeroDocumento = !string.IsNullOrWhiteSpace(numeroDocumento);

                #region Verificar Sala
                SalaEntidad sala = salaBL.ObtenerSalaPorCodigo(codSala);
                if(!sala.Existe()) {
                    success = false;
                    displayMessage = $"No existe sala con el código {codSala}.";
                    codeToast = ResponseCodeToast.Warning;
                    return Json(new { success, displayMessage, codeToast });
                }
                #endregion

                #region Verificar Configuracion
                ESC_ConfiguracionDto configuracion = configuracionBL.ObtenerConfiguracionPorCodSala(codSala);
                if(!configuracion.EncuestaActiva) {
                    success = false;
                    displayMessage = $"Por el momento no es están aceptando respuestas.";
                    codeToast = ResponseCodeToast.Warning;
                    return Json(new { success, displayMessage, codeToast });
                }

                List<int> idsPreguntas = respuestaCliente.Respuestas.Select(x => x.IdPregunta).ToList();
                bool preguntasValidas = preguntaBL.PreguntasSonDeSala(codSala, idsPreguntas);
                if(!preguntasValidas) {
                    success = false;
                    displayMessage = "Las preguntas enviadas no corresponden a la sala";
                    codeToast = ResponseCodeToast.Warning;
                    return Json(new { success, displayMessage, codeToast });
                }

                if(!enviaNumeroDocumento && !configuracion.RespuestasAnonimas) {
                    success = false;
                    displayMessage = $"Tiene que ingresar su número de documento para poder enviar su respuesta.";
                    codeToast = ResponseCodeToast.Warning;
                    return Json(new { success, displayMessage });
                }

                if(enviaNumeroDocumento) {
                    switch(configuracion.TipoValidacionEnvioRespuesta) {
                        case ESC_TipoValidacionEnvioRespuesta.PorFecha:
                            int cantidadRedspuestasDia = respuestaBL.ObtenerCantidadRespuestasDeClientePorFechaDeSala(codSala, numeroDocumento, DateTime.Now.Date);
                            if(cantidadRedspuestasDia >= configuracion.EnvioMaximoDiario) {
                                success = false;
                                displayMessage = configuracion.MensajeEnvioMaximoDiario;
                                codeToast = ResponseCodeToast.Warning;
                                return Json(new { success, displayMessage, codeToast });
                            }
                            break;
                        case ESC_TipoValidacionEnvioRespuesta.PorTiempo:
                            ESC_RespuestaDto ultimaRespuesta = respuestaBL.ObtenerUltimaRespuestaClienteDeSala(codSala, numeroDocumento);
                            if(ultimaRespuesta.Existe()) {
                                bool puedeEnviarRespuesta = respuestaBL.ClientePuedeEnviarRespuestaPorTiempo(configuracion, ultimaRespuesta);
                                if(!puedeEnviarRespuesta) {
                                    success = false;
                                    displayMessage = configuracion.MensajeTiempoEsperaRespuesta;
                                    codeToast = ResponseCodeToast.Warning;
                                    return Json(new { success, displayMessage, codeToast });
                                }
                            }
                            break;
                        default:
                            break;
                    }

                    int cantidadRespuestasMes = respuestaBL.ObtenerCantidadRespuestasDeClientePorMesDeSala(codSala, numeroDocumento, DateTime.Now.Month);
                    if(cantidadRespuestasMes >= configuracion.EnvioMaximoMensual) {
                        success = false;
                        displayMessage = configuracion.MensajeEnvioMaximoMensual;
                        codeToast = ResponseCodeToast.Warning;
                        return Json(new { success, displayMessage, codeToast });
                    }
                }
                #endregion

                success = respuestaBL.InsertarRespuestas(respuestaCliente) > 0;
                displayMessage = success ? "Respuesta guardada correctamente." : "No se pudo guardar la respuesta.";
                codeToast = success ? ResponseCodeToast.Success : ResponseCodeToast.Error;
            } catch(Exception ex) {
                codeToast = ResponseCodeToast.Error;
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage, codeToast = codeToast.GetDisplayText() });
        }
        #endregion
    }
}
