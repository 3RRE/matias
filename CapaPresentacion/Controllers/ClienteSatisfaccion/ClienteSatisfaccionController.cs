using CapaEntidad.ClienteSatisfaccion;
using CapaNegocio.SatisfaccionCliente.Configuracion;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaNegocio.ClienteSatisfaccion.Preguntas;
using CapaNegocio.ClienteSatisfaccion.Opciones;
using CapaNegocio.ClienteSatisfaccion.Flujo;
using CapaEntidad.ClienteSatisfaccion.Entidad;
using CapaNegocio.ClienteSatisfaccion.Respuesta;
using CapaNegocio.ClienteSatisfaccion.Tablet;
using CapaEntidad.ClienteSatisfaccion.DTO;
using CapaNegocio.ClienteSatisfaccion.Configuracion;

namespace CapaPresentacion.Controllers.ClienteSatisfaccion
{
    [seguridad(false)]
    public class ClienteSatisfaccionController : Controller
    {
        private readonly PreguntasBL preguntasBL;
        private readonly OpcionesBL opcionesBL;
        private readonly FlujoBL flujoBL;
        private readonly RespuestaBL respuestaBL;
        private readonly TabletBL tabletBL;
        private readonly SalaBL salaBl;
        private readonly CSConfiguracionBL confgiBL;

        public ClienteSatisfaccionController() {
            preguntasBL = new PreguntasBL();
            opcionesBL = new OpcionesBL();
            flujoBL = new FlujoBL();
            respuestaBL = new RespuestaBL();
            tabletBL = new TabletBL();
            salaBl = new SalaBL();
            confgiBL = new CSConfiguracionBL();
        }


        #region Views
        [seguridad(false)]
        public ActionResult NPSView() {
            return View("~/Views/ClienteSatisfaccion/Encuesta/NPS.cshtml");
        }

        [seguridad(false)]
        public ActionResult CSATView() {
            return View("~/Views/ClienteSatisfaccion/Encuesta/CSAT.cshtml");
        }


        [seguridad(false)]
        public ActionResult ReportesView() {
            return View("~/Views/ClienteSatisfaccion/Reportes/Reporte.cshtml");
        }

        #endregion


        #region NPSMETHODS

        [HttpPost]
        public JsonResult GetPreguntasNPS() {
            bool success = false;
            string displayMessage;
            EncuestaDTO encuesta = new EncuestaDTO();

            try {
                var preguntas = preguntasBL.ListadoPreguntas(1);
                var opciones = opcionesBL.ListadoOpciones();
                var flujo = flujoBL.ListadoFlujoEncuesta(1);

                var preguntasDTO = preguntas.Select(p => new PreguntasDTO {
                    IdPregunta = p.IdPregunta,
                    IdTipoEncuesta = p.IdTipoEncuesta,
                    Texto = p.Texto,
                    Orden = p.Orden,
                    Random = p.Random,
                    Opciones = opciones
                        .Where(o => o.IdPregunta == p.IdPregunta)
                        .Select(o => new OpcionesDTO {
                            idOpcion = o.IdOpcion,
                            idPregunta = o.IdPregunta,
                            Texto = o.Texto,
                            TieneComentario = o.TieneComentario
                        })
                        .ToList()
                }).ToList();

                var flujoDTO = flujo.Select(f => new FlujoDTO {
                    IdFlujo = f.IdFlujo,
                    IdTipoEncuesta = f.IdTipoEncuesta,
                    IdPreguntaActual = f.IdPreguntaActual,
                    IdOpcion = f.IdOpcion,
                    IdPreguntaSiguiente = f.IdPreguntaSiguiente
                }).ToList();

                encuesta.Preguntas = preguntasDTO;
                encuesta.Flujo = flujoDTO;

                success = encuesta.Preguntas.Any();
                displayMessage = success
                    ? "Se encontraron preguntas con sus opciones y flujo"
                    : "No se encontraron preguntas";

            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data = encuesta });
        }


        public int GuardarEncuestaConPreguntasNormal(RespuestaEncuestaEntidad encuesta, List<RespuestaPreguntaEntidad> respuestas) {
            int idInsertado = 0;
            try {
                encuesta.FechaRespuesta = DateTime.Now;
                // 1. Guardar la encuesta y recuperar su Id
                idInsertado = respuestaBL.GuardarRespuestaEncuesta(encuesta);

                // 2. Guardar cada respuesta ligada al Id de la encuesta
                foreach(var p in respuestas) {
                    p.IdRespuestaEncuesta = idInsertado;
                    respuestaBL.GuardarRespuestaPregunta(p);
                }
            } catch(Exception ex) {
                Console.WriteLine("Error al guardar: " + ex.Message);
                idInsertado = 0;
            }

            return idInsertado;
        }


        #endregion


        #region CSATMETHODS
        [HttpPost]
        public JsonResult GetPreguntasCSAT() {
            bool success = false;
            string displayMessage;
            EncuestaDTO encuesta = new EncuestaDTO();
            var hoy = DateTime.Now.Date;

            try {
                var preguntas = preguntasBL.ListadoPreguntas(1);
                var opciones = opcionesBL.ListadoOpciones();
                var flujo = flujoBL.ListadoFlujoEncuesta(1);
                var rand = new Random();

                // 🔹 Construir todas las preguntas con sus opciones
                var preguntasDTO = preguntas.Select(p => new PreguntasDTO {
                    IdPregunta = p.IdPregunta,
                    IdTipoEncuesta = p.IdTipoEncuesta,
                    Texto = p.Texto,
                    Orden = p.Orden,
                    Random = p.Random,
                    Activo = p.Activo,
                    Multi=p.Multi,
                    FechaFin = p.FechaFin,
                    FechaInicio= p.FechaInicio,
                    Opciones = opciones
                        .Where(o => o.IdPregunta == p.IdPregunta)
                        .Select(o => new OpcionesDTO {
                            idOpcion = o.IdOpcion,
                            idPregunta = o.IdPregunta,
                            Texto = o.Texto,
                            TieneComentario = o.TieneComentario
                        })
                        .ToList()
                }).ToList();

                // 🔹 Dividir preguntas fijas y aleatorias
                var preguntasRandom = preguntasDTO
                    .Where(p => p.Random
                             && p.Activo
                             && p.FechaInicio != null
                             && p.FechaFin != null
                             && hoy >= p.FechaInicio.Value.Date
                             && hoy <= p.FechaFin.Value.Date)
                    .ToList();
                var preguntasFijas = preguntasDTO.Where(p => !p.Random).ToList();

                // 🔹 Seleccionar 4 aleatorias del grupo random
                if(preguntasRandom.Count > 2) {
                    preguntasRandom = preguntasRandom
                        .OrderBy(x => rand.Next())
                        .Take(2)
                        .ToList();
                }

                // 🔹 Unir y ordenar por Orden
                preguntasDTO = preguntasFijas
                    .Concat(preguntasRandom)
                    .OrderBy(p => p.Orden)
                    .ToList();

                // 🔹 Armar flujo DTO
                var flujoDTO = flujo.Select(f => new FlujoDTO {
                    IdFlujo = f.IdFlujo,
                    IdTipoEncuesta = f.IdTipoEncuesta,
                    IdPreguntaActual = f.IdPreguntaActual,
                    IdOpcion = f.IdOpcion,
                    IdPreguntaSiguiente = f.IdPreguntaSiguiente
                }).ToList();

                // 🔹 Asignar a encuesta
                encuesta.Preguntas = preguntasDTO;
                encuesta.Flujo = flujoDTO;

                success = encuesta.Preguntas.Any();
                displayMessage = success
                    ? "Se encontraron preguntas con sus opciones y flujo"
                    : "No se encontraron preguntas";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data = encuesta });
        }


        public ActionResult ValidarDatosEncuesta(int salaId, int tabletId) {
            bool success = false;
            string displayMessage = "Validación no ejecutada";

            try {
                bool existe = tabletBL.ExisteTabletEnSala(salaId, tabletId);
                var sala = salaBl.ObtenerSalaPorCodigo(salaId);
                var data = new {
                    existe,
                    nombre=sala.Nombre,
                    nombreCorto=sala.NombreCorto
                };
                if(existe) {
                   success = true;
                    displayMessage = "La tablet existe en la sala";
                    return Json(new { success, displayMessage, data });
                } else {
                    displayMessage = "La tablet no existe en la sala";
                    return Json(new { success, displayMessage, data });
                }
            } catch(Exception ex) {
                return Json(new { success, displayMessage });
            }

        }

        public ActionResult VerificarConfiguracionCliente(int idSala, string nroDoc) {
            bool success = false;
            string displayMessage = "";

            try {
                var configuraciones = confgiBL.ListadoConfiguracionesPorSala(idSala);
                var clientePorDia = configuraciones
                    .FirstOrDefault(x => x.IdConfiguracion == 1);

                if(clientePorDia != null) {
                    if(clientePorDia.ValorBit) {
                        //  Validación desactivada → permitir siempre
                        success = true;
                        displayMessage = "Validación desactivada. No puede responder.";
                    } else {
                        // Validación activada → revisar si ya respondió hoy
                        bool puedeResponder = confgiBL.PuedeResponderEncuesta(idSala, nroDoc);

                        if(puedeResponder) {
                            success = true;
                            displayMessage = "Puede responder la encuesta.";
                        } else {
                            success = false;
                            displayMessage = "Ya respondió la encuesta hoy.";
                        }
                    }
                } else {
                    success = false;
                    displayMessage = "No se encontró configuración con Id = 1.";
                }
            } catch(Exception ex) {
                success = false;
                displayMessage = "Error: " + ex.Message;
            }

            return Json(new { success, displayMessage }, JsonRequestBehavior.AllowGet);
        }




        #endregion




    }
}