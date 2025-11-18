using CapaEntidad.ClienteSatisfaccion;
using CapaEntidad.ClienteSatisfaccion.DTO;
using CapaEntidad.ClienteSatisfaccion.Entidad;
using CapaNegocio.ClienteSatisfaccion.Configuracion;
using CapaNegocio.ClienteSatisfaccion.Flujo;
using CapaNegocio.ClienteSatisfaccion.Opciones;
using CapaNegocio.ClienteSatisfaccion.Preguntas;
using CapaNegocio.ClienteSatisfaccion.Respuesta;
using CapaNegocio.ClienteSatisfaccion.Tablet;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.ClienteSatisfaccion
{
    [seguridad]
    public class ConfigClienteSatisfaccionController : Controller
    {
        private readonly PreguntasBL preguntasBL;
        private readonly OpcionesBL opcionesBL;
        private readonly FlujoBL flujoBL;
        private readonly RespuestaBL respuestaBL;
        private readonly TabletBL tabletBL;
        private CSConfiguracionBL configuracionBL = new CSConfiguracionBL();
        // GET: ConfigClienteSatisfaccion
        public ConfigClienteSatisfaccionController() {
            preguntasBL = new PreguntasBL();
            opcionesBL = new OpcionesBL();
            flujoBL = new FlujoBL();
            respuestaBL = new RespuestaBL();
            tabletBL = new TabletBL();
            configuracionBL=new CSConfiguracionBL();
        }


        #region Views
      
        public ActionResult ReportesView() {
            return View("~/Views/ClienteSatisfaccion/Reportes/ReporteEncuesta.cshtml");
        }
        public ActionResult PreguntasView() {
            return View("~/Views/ClienteSatisfaccion/Reportes/PreguntasEncuesta.cshtml");
        }
        public ActionResult TabletsView() {
            return View("~/Views/ClienteSatisfaccion/Reportes/Tablets.cshtml");
        }
        public ActionResult FlujoView() {
            return View("~/Views/ClienteSatisfaccion/Reportes/FlujoPregunta.cshtml");
        }
        public ActionResult ConfiguracionUsuarioView() {
            return View("~/Views/ClienteSatisfaccion/Reportes/ConfiguracionUsuarioView.cshtml");
        }
        #endregion

        #region Methods

        [HttpPost]
        public ActionResult ObtenerPreguntaAtributos() {
            bool success = false;
            string displayMessage;
            try {
                var preguntasAtributo = preguntasBL.ObtenerPreguntasAtributo();

                success = true;
                displayMessage = "Se obtuvieron datos correctamente.";
                return Json(new { success, displayMessage, data = preguntasAtributo });
            } catch(Exception ex) {
                displayMessage = $"Error: {ex.Message}";
                return Json(new { success, displayMessage });
            }
        }

        [HttpPost]
        public ActionResult DataReportesEncuesta(DateTime fechaInicio, DateTime fechaFin) {
            bool success = false;
            string displayMessage;

            try {
                // Normalizar a días completos
                DateTime ini = fechaInicio.Date; // 00:00:00
                DateTime fin = fechaFin.Date;    // 00:00:00

                // Garantizar que ini <= fin
                if(ini > fin)
                    (ini, fin) = (fin, ini);

                // Cantidad de días (inclusive)
                int dias = (fin - ini).Days + 1;

                // Periodo anterior del mismo largo
                DateTime iniAnt = ini.AddDays(-dias);
                DateTime finAnt = fin.AddDays(-dias);

                // 3) Traer indicadores
                var npsActual = respuestaBL.ObtenerNPSIndicador(ini, fin);
                var npsAnterior = respuestaBL.ObtenerNPSIndicador(iniAnt, finAnt);
                var npsMensual = respuestaBL.ObtenerNPSMensual(ini, fin); // lista para gráfico

                // 4) Helpers para diferencias y tendencias
                Func<double, double, double> delta = (a, b) => Math.Round((a - b), 2);
                Func<int, string> trendInt = d => d > 0 ? "INCREMENTO" : (d < 0 ? "DECREMENTO" : "IGUAL");
                Func<double, string> trendDbl = d => d > 0 ? "INCREMENTO" : (d < 0 ? "DECREMENTO" : "IGUAL");

                // 5) Calcular deltas/tendencias (con null-safety por si el repo devuelve null)
                int npsAct = npsActual?.NPS ?? 0;
                int npsAnt = npsAnterior?.NPS ?? 0;

                double pctDetAct = npsActual?.PctDetractores ?? 0.0;
                double pctPasAct = npsActual?.PctPasivos ?? 0.0;
                double pctProAct = npsActual?.PctPromotores ?? 0.0;

                double pctDetAnt = npsAnterior?.PctDetractores ?? 0.0;
                double pctPasAnt = npsAnterior?.PctPasivos ?? 0.0;
                double pctProAnt = npsAnterior?.PctPromotores ?? 0.0;

                int npsDelta = npsAct - npsAnt;

                var deltas = new {
                    npsDelta,
                    npsTrend = trendInt(npsDelta),

                    pctDetractoresDelta = delta(pctDetAct, pctDetAnt),
                    pctDetractoresTrend = trendDbl(delta(pctDetAct, pctDetAnt)),

                    pctPasivosDelta = delta(pctPasAct, pctPasAnt),
                    pctPasivosTrend = trendDbl(delta(pctPasAct, pctPasAnt)),

                    pctPromotoresDelta = delta(pctProAct, pctProAnt),
                    pctPromotoresTrend = trendDbl(delta(pctProAct, pctProAnt))
                };

                var payload = new {
                    periodoActual = new { fechaInicio = ini, fechaFin = fin },
                    periodoAnterior = new { fechaInicio = iniAnt, fechaFin = finAnt },
                    nps = new {
                        npsActual = npsActual,
                        npsAnterior = npsAnterior,
                        deltas,
                    },
                    // Serie mensual para gráfico
                    npsMensual = npsMensual
                };


                success = true;
                displayMessage = "OK";

                return Json(new { success, displayMessage, data = payload });
            } catch(Exception ex) {
                displayMessage = $"Error: {ex.Message}";
                return Json(new { success, displayMessage });
            }
        }



        [HttpPost]
        public ActionResult DataReportesEncuestaCSAT(DateTime fechaInicio, DateTime fechaFin, int salaId) {
            bool success = false;
            string displayMessage;

            try {
                // Normalizar fechas
                DateTime ini = fechaInicio.Date;
                DateTime fin = fechaFin.Date;

                if(ini > fin)
                    (ini, fin) = (fin, ini);

                // Cantidad de días en el rango
                int dias = (fin - ini).Days + 1;

                // Periodo anterior (misma cantidad de días antes)
                DateTime iniAnt = ini.AddDays(-dias);
                DateTime finAnt = fin.AddDays(-dias);

                // === Traer indicadores desde la capa BL ===
                var csatActual = respuestaBL.ObtenerCSATIndicador(ini, fin, salaId);
                var csatAnterior = respuestaBL.ObtenerCSATIndicador(iniAnt, finAnt, salaId);
                var csatMensual = respuestaBL.ObtenerCSATMensual(ini, fin); // lista para gráfico

                // === Helpers ===
                Func<double, double, double> delta = (a, b) => Math.Round((a - b), 2);
                Func<double, string> trendDbl = d => d > 0 ? "INCREMENTO" : (d < 0 ? "DECREMENTO" : "IGUAL");

                // === Valores actuales y anteriores (null safety) ===
                double csatAct = csatActual?.CSAT ?? 0.0;
                double csatAnt = csatAnterior?.CSAT ?? 0.0;
                double csatDelta = delta(csatAct, csatAnt);

                // === Porcentajes individuales ===
                double muyInsAct = csatActual?.MuyInsatisfecho ?? 0.0;
                double insAct = csatActual?.Insatisfecho ?? 0.0;
                double neuAct = csatActual?.Neutral ?? 0.0;
                double satAct = csatActual?.Satisfecho ?? 0.0;
                double muySatAct = csatActual?.MuySatisfecho ?? 0.0;

                double muyInsAnt = csatAnterior?.MuyInsatisfecho ?? 0.0;
                double insAnt = csatAnterior?.Insatisfecho ?? 0.0;
                double neuAnt = csatAnterior?.Neutral ?? 0.0;
                double satAnt = csatAnterior?.Satisfecho ?? 0.0;
                double muySatAnt = csatAnterior?.MuySatisfecho ?? 0.0;

                // === Deltas ===
                var deltas = new {
                    csatDelta,
                    csatTrend = trendDbl(csatDelta),

                    muyInsatisfechoDelta = delta(muyInsAct, muyInsAnt),
                    muyInsatisfechoTrend = trendDbl(delta(muyInsAct, muyInsAnt)),

                    insatisfechoDelta = delta(insAct, insAnt),
                    insatisfechoTrend = trendDbl(delta(insAct, insAnt)),

                    neutralDelta = delta(neuAct, neuAnt),
                    neutralTrend = trendDbl(delta(neuAct, neuAnt)),

                    satisfechoDelta = delta(satAct, satAnt),
                    satisfechoTrend = trendDbl(delta(satAct, satAnt)),

                    muySatisfechoDelta = delta(muySatAct, muySatAnt),
                    muySatisfechoTrend = trendDbl(delta(muySatAct, muySatAnt))
                };

                // === Armar payload ===
                var payload = new {
                    periodoActual = new { fechaInicio = ini, fechaFin = fin },
                    periodoAnterior = new { fechaInicio = iniAnt, fechaFin = finAnt },
                    csat = new {
                        csatActual = csatActual,
                        csatAnterior = csatAnterior,
                        deltas,
                    },
                    // Serie mensual para gráfico
                    csatMensual = csatMensual
                };

                success = true;
                displayMessage = "OK";

                return Json(new { success, displayMessage, data = payload });
            } catch(Exception ex) {
                displayMessage = $"Error: {ex.Message}";
                return Json(new { success, displayMessage });
            }
        }


        [HttpPost]
        public ActionResult DataReportesIndicador(DateTime fechaInicio, DateTime fechaFin, string indicador) {
            bool success = false;
            string displayMessage;

            try {
                // 1) Normalizar fechas (solo fecha, sin hora)
                DateTime ini = fechaInicio.Date;
                DateTime fin = fechaFin.Date;

                if(ini > fin)
                    (ini, fin) = (fin, ini);

                // 2) Calcular periodo anterior con la misma cantidad de días
                int dias = (fin - ini).Days + 1; // inclusivo
                DateTime iniAnt = ini.AddDays(-dias);
                DateTime finAnt = fin.AddDays(-dias);

                // 3) Llamadas BL
                var indicadorActual = respuestaBL.ObtenerIndicador(indicador, ini, fin)
                                       ?? new IndicadorResultadoDTO();
                var indicadorAnterior = respuestaBL.ObtenerIndicador(indicador, iniAnt, finAnt)
                                       ?? new IndicadorResultadoDTO();
                var indicadorDiario = respuestaBL.ObtenerIndicadorDiario(ini, fin, indicador)
                                       ?? new List<IndicadorDiarioDTO>();

                // 4) Helpers
                Func<double, double, double> delta = (a, b) => Math.Round(a - b, 2);
                Func<double, string> trend = d => d > 0 ? "INCREMENTO" : (d < 0 ? "DECRECIMIENTO" : "IGUAL");

                // 5) Actual vs Anterior (IndicadorValor es INT, pero para delta usamos double)
                

                if(indicadorActual.IndicadorValor < 0) {
                    indicadorActual.IndicadorValor = 0;
                }
                if(indicadorAnterior.IndicadorValor < 0) {
                    indicadorAnterior.IndicadorValor = 0;
                }
                double valAct = indicadorActual.IndicadorValor;
                double valAnt = indicadorAnterior.IndicadorValor;
                double valDelta = delta(valAct, valAnt);

                // Porcentajes principales (usados para deltas)
                double pctMuySatAct = indicadorActual.PctMuySatisfecho;
                double pctMuySatAnt = indicadorAnterior.PctMuySatisfecho;

                double pctMuyInsAct = indicadorActual.PctMuyInsatisfecho;
                double pctMuyInsAnt = indicadorAnterior.PctMuyInsatisfecho;

                var deltas = new {
                    // Indicador (entero) – delta como double con 2 dec
                    valorDelta = valDelta,
                    valorTrend = trend(valDelta),

                    // Porcentajes (2 decimales)
                    pctMuySatisfechoDelta = delta(pctMuySatAct, pctMuySatAnt),
                    pctMuySatisfechoTrend = trend(delta(pctMuySatAct, pctMuySatAnt)),

                    pctMuyInsatisfechoDelta = delta(pctMuyInsAct, pctMuyInsAnt),
                    pctMuyInsatisfechoTrend = trend(delta(pctMuyInsAct, pctMuyInsAnt))
                };

                var payload = new {
                    periodoActual = new { fechaInicio = ini, fechaFin = fin },
                    periodoAnterior = new { fechaInicio = iniAnt, fechaFin = finAnt },
                    indicador = new {
                        indicadorActual,
                        indicadorAnterior,
                        deltas
                    },
                    indicadorDiario
                };

                success = true;
                displayMessage = "OK";
                return Json(new { success, displayMessage, data = payload });
            } catch(Exception ex) {
                displayMessage = $"Error: {ex.Message}";
                return Json(new { success, displayMessage });
            }
        }



        [HttpPost]
        public ActionResult DataNCSAT(DateTime fechaInicio, DateTime fechaFin, int salaId) {
            bool success = false;
            string displayMessage;

            try {
                // Normalizar fechas
                DateTime ini = fechaInicio.Date;
                DateTime fin = fechaFin.Date;

                if(ini > fin)
                    (ini, fin) = (fin, ini);

                // Cantidad de días en el rango
                int dias = (fin - ini).Days + 1;

                // Periodo anterior (misma cantidad de días antes)
                DateTime iniAnt = ini.AddDays(-dias);
                DateTime finAnt = fin.AddDays(-dias);

                // === Traer indicadores desde la capa BL ===
                var dataCsatActual = respuestaBL.ObtenerListaCSATIRespuestas(ini, fin, salaId);
                var dataCsatAnterior = respuestaBL.ObtenerListaCSATIRespuestas(iniAnt, finAnt, salaId);
                var csatDiario = respuestaBL.ObtenerCSATDiario(ini, fin, salaId); // lista para gráfico
                var respuestasUnicas = respuestaBL.ObtenerCantidadRespuestasAtributos(salaId, fechaInicio, fechaFin, "CSAT");

                // === Armar payload ===
                var payload = new {
                    periodoActual = new { fechaInicio = ini, fechaFin = fin },
                    periodoAnterior = new { fechaInicio = iniAnt, fechaFin = finAnt },
                    csat = new {
                        dataCsatActual,
                        dataCsatAnterior,
                    },
                    respuestasUnicas,
                    // Serie mensual para gráfico
                    csatDiario
                };

                success = true;
                displayMessage = "OK";

                return Json(new { success, displayMessage, data = payload });
            } catch(Exception ex) {
                displayMessage = $"Error: {ex.Message}";
                return Json(new { success, displayMessage });
            }
        }

        [HttpPost]
        public ActionResult DataNPS(DateTime fechaInicio, DateTime fechaFin, int salaId) {
            bool success = false;
            string displayMessage;

            try {
                // Normalizar a días completos
                DateTime ini = fechaInicio.Date; // 00:00:00
                DateTime fin = fechaFin.Date;    // 00:00:00

                // Garantizar que ini <= fin
                if(ini > fin)
                    (ini, fin) = (fin, ini);

                // Cantidad de días (inclusive)
                int dias = (fin - ini).Days + 1;

                // Periodo anterior del mismo largo
                DateTime iniAnt = ini.AddDays(-dias);
                DateTime finAnt = fin.AddDays(-dias);

                // 3) Traer indicadores
                var npsActual = respuestaBL.ObtenerNPSRespuestas(ini, fin, salaId);
                var npsAnterior = respuestaBL.ObtenerNPSRespuestas(iniAnt, finAnt, salaId);
                var npsMensual = respuestaBL.ObtenerNPSMensual(ini, fin); // lista para gráfico
                var respuestasUnicas = respuestaBL.ObtenerCantidadRespuestasAtributos(salaId, fechaInicio, fechaFin, "NPS");


                var payload = new {
                    periodoActual = new { fechaInicio = ini, fechaFin = fin },
                    periodoAnterior = new { fechaInicio = iniAnt, fechaFin = finAnt },
                    nps = new {
                        npsActual,
                        npsAnterior
                    },
                    respuestasUnicas,
                    npsMensual = npsMensual
                };


                success = true;
                displayMessage = "OK";

                return Json(new { success, displayMessage, data = payload });
            } catch(Exception ex) {
                displayMessage = $"Error: {ex.Message}";
                return Json(new { success, displayMessage });
            }
        }

        [HttpPost]
        public ActionResult DataIndicador(DateTime fechaInicio, DateTime fechaFin, string indicador, int salaId) {
            bool success = false;
            string displayMessage;

            try {
                // 1) Normalizar fechas (solo fecha, sin hora)
                DateTime ini = fechaInicio.Date;
                DateTime fin = fechaFin.Date;

                if(ini > fin)
                    (ini, fin) = (fin, ini);

                // 2) Calcular periodo anterior con la misma cantidad de días
                int dias = (fin - ini).Days + 1; // inclusivo
                DateTime iniAnt = ini.AddDays(-dias);
                DateTime finAnt = fin.AddDays(-dias);

                // 3) Llamadas BL
                var indicadorActual = respuestaBL.ObtenerListaIndicadorRespuestas( ini, fin,indicador,salaId);
                var indicadorAnterior = respuestaBL.ObtenerListaIndicadorRespuestas( iniAnt, finAnt, indicador,salaId);
                var indicadorDiario = respuestaBL.ObtenerIndicadorDiario(ini, fin, indicador) ?? new List<IndicadorDiarioDTO>();


                var payload = new {
                    periodoActual = new { fechaInicio = ini, fechaFin = fin },
                    periodoAnterior = new { fechaInicio = iniAnt, fechaFin = finAnt },
                    indicador = new {
                        indicadorActual,
                        indicadorAnterior,
                        indicadorDiario
                    },
                    indicadorDiario
                };

                success = true;
                displayMessage = "OK";
                return Json(new { success, displayMessage, data = payload });
            } catch(Exception ex) {
                displayMessage = $"Error: {ex.Message}";
                return Json(new { success, displayMessage });
            }
        }

        [HttpPost]
        public ActionResult ObtenerTabletsSala(int salaId) {
            bool success = false;
            string displayMessage;
            try {
                var tablets = tabletBL.ListadoTablets(salaId);

                success = true;
                displayMessage = "OK";
                return Json(new { success, displayMessage, data = tablets });
            } catch(Exception ex) {
                displayMessage = $"Error: {ex.Message}";
                return Json(new { success, displayMessage });
            }
        }

        [HttpPost]
        public ActionResult CrearTablet(TabletEntidad tablet) {
            bool success = false;
            string displayMessage;

            try {
                success = tabletBL.CrearTablet(tablet);
                displayMessage = success ? "Tablet creada correctamente" : "No se pudo crear la tablet";
            } catch(Exception ex) {
                displayMessage = $"Error: {ex.Message}";
            }

            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public ActionResult EditarTablet(TabletEntidad tablet) {
            bool success = false;
            string displayMessage;

            try {
                success = tabletBL.EditarTablet(tablet.Nombre, tablet.Activa, tablet.IdTablet);
                displayMessage = success ? "Tablet actualizada correctamente" : "No se pudo actualizar la tablet";
            } catch(Exception ex) {
                displayMessage = $"Error: {ex.Message}";
            }

            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public ActionResult ExportarEncuestasExcel(int salaId, DateTime fechaInicio, DateTime fechaFin) {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;

            try {
                var preguntas = respuestaBL.ObtenerPreguntasIndicadores();
                var encuestas = respuestaBL.ObtenerRespuestasEncuestas(fechaInicio, fechaFin, salaId);

                if(encuestas != null && encuestas.Any()) {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using(var excel = new ExcelPackage()) {
                        var ws = excel.Workbook.Worksheets.Add("Encuestas");
                        ws.DefaultRowHeight = 18;

                        int row = 1;

                        // === Título Principal ===
                        ws.Cells[row, 1].Value = "Reporte de Encuestas";
                        ws.Cells[row, 1, row, 8].Merge = true;
                        ws.Cells[row, 1, row, 8].Style.Font.Bold = true;
                        ws.Cells[row, 1, row, 8].Style.Font.Size = 16;
                        ws.Cells[row, 1, row, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells[row, 1, row, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        ws.Cells[row, 1, row, 8].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[row, 1, row, 8].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146)); // azul oscuro
                        ws.Cells[row, 1, row, 8].Style.Font.Color.SetColor(System.Drawing.Color.White);

                        row++;

                        // === Rango de Fechas ===
                        ws.Cells[row, 1].Value = $"Sala ID: {salaId}   |   Rango: {fechaInicio:dd/MM/yyyy} al {fechaFin:dd/MM/yyyy}";
                        ws.Cells[row, 1, row, 8].Merge = true;
                        ws.Cells[row, 1, row, 8].Style.Font.Italic = true;
                        ws.Cells[row, 1, row, 8].Style.Font.Size = 12;
                        ws.Cells[row, 1, row, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells[row, 1, row, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        row += 2; // dejar una fila en blanco

                        // === Encabezados fijos ===
                        string[] headersFijos = new[] {
                    "Nro Documento","Nombres","Teléfono","Correo",
                    "Id Tablet","Nombre Tablet","Valor NPS","Clasificación NPS"
                };

                        int col = 1;
                        for(int i = 0; i < headersFijos.Length; i++) {
                            ws.Cells[row, col].Value = headersFijos[i];
                            col++;
                        }

                        // === Encabezados dinámicos ===
                        foreach(var preg in preguntas) {
                            ws.Cells[row, col].Value = preg.Pregunta;
                            col++;
                        }

                        // === Estilos de cabecera ===
                        using(var range = ws.Cells[row, 1, row, col - 1]) {
                            range.Style.Font.Bold = true;
                            range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(79, 129, 189)); // azul corporativo
                            range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        }

                        // === Datos ===
                        int startDataRow = row + 1;
                        var grupos = encuestas.GroupBy(e => e.IdRespuestaEncuesta);
                        foreach(var grupo in grupos) {
                            var first = grupo.First();

                            ws.Cells[startDataRow, 1].Value = first.NroDocumento;
                            ws.Cells[startDataRow, 2].Value = first.Nombre;
                            ws.Cells[startDataRow, 3].Value = first.Celular;
                            ws.Cells[startDataRow, 4].Value = first.Correo;
                            ws.Cells[startDataRow, 5].Value = first.IdTablet;
                            ws.Cells[startDataRow, 6].Value = first.NombreTablet;

                            // NPS
                            var nps = grupo.FirstOrDefault(r => r.Indicador == "NPS");
                            int valorNps = nps?.ValorOpcion ?? 0;
                            ws.Cells[startDataRow, 7].Value = valorNps;

                            string tipoCliente = "";
                            if(valorNps >= 4)
                                tipoCliente = "Promotor";
                            else if(valorNps == 3)
                                tipoCliente = "Pasivo";
                            else if(valorNps > 0)
                                tipoCliente = "Detractor";
                            ws.Cells[startDataRow, 8].Value = tipoCliente;

                            // Respuestas dinámicas
                            col = 9;
                            foreach(var preg in preguntas) {
                                var respuestasPregunta = grupo.Where(r => r.IdPregunta == preg.IdPregunta).ToList();
                                string textoCelda = "";
                                if(respuestasPregunta.Any()) {
                                    if(preg.Multi) {
                                        textoCelda = string.Join(", ", respuestasPregunta.Select(r =>
                                            r.TieneComentario && !string.IsNullOrEmpty(r.Comentario)
                                                ? $"{r.TextoOpcion} ({r.Comentario})"
                                                : r.TextoOpcion
                                        ));
                                    } else {
                                        var r = respuestasPregunta.First();
                                        textoCelda = r.TieneComentario && !string.IsNullOrEmpty(r.Comentario)
                                            ? $"{r.TextoOpcion} ({r.Comentario})"
                                            : r.TextoOpcion;
                                    }
                                }

                                ws.Cells[startDataRow, col].Value = textoCelda;
                                col++;
                            }

                            startDataRow++;
                        }

                        // Ajustes generales
                        ws.Cells[ws.Dimension.Address].AutoFitColumns();
                        ws.View.FreezePanes(row + 1, 1); // congela después de cabecera

                        excelName = $"Encuestas_Sala{salaId}_{fechaInicio:dd_MM_yyyy}_al_{fechaFin:dd_MM_yyyy}.xlsx";
                        var memoryStream = new MemoryStream();
                        excel.SaveAs(memoryStream);
                        base64String = Convert.ToBase64String(memoryStream.ToArray());

                        mensaje = "Descargando Archivo";
                        respuesta = true;
                    }
                } else {
                    mensaje = "No se encontraron encuestas en el rango seleccionado";
                }
            } catch(Exception ex) {
                respuesta = false;
                mensaje = ex.Message + ", contacte al administrador";
            }

            return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });
        }



        //Preguntas

        [HttpPost]
        public ActionResult ObtenerPreguntas(int tipoEncuesta) {
            bool success = false;
            string displayMessage;
            try {
                // 🔹 Obtener entidades desde la capa BL
                var preguntas = preguntasBL.ListadoPreguntas(tipoEncuesta);
                var opciones = opcionesBL.ListadoOpciones();

                // 🔹 Mapear a DTO y agrupar opciones
                var preguntasDto = preguntas.Select(p => new PreguntasDTO {
                    IdPregunta = p.IdPregunta,
                    IdTipoEncuesta = p.IdTipoEncuesta,
                    Texto = p.Texto,
                    Orden = p.Orden,
                    Random = p.Random,
                    Multi = p.Multi,
                    Activo = p.Activo,
                    Indicador=p.Indicador,
                    FechaFin = p.FechaFin,
                    FechaInicio=p.FechaInicio,
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

                success = true;
                displayMessage = "OK";
                return Json(new { success, displayMessage, data = preguntasDto });
            } catch(Exception ex) {
                displayMessage = $"Error: {ex.Message}";
                return Json(new { success, displayMessage });
            }
        }


        // ============================
        // 📌 PREGUNTAS
        // ============================

        // 🔹 CREAR PREGUNTA NORMAL
        [HttpPost]
        public ActionResult CrearPreguntaNormal(PreguntaEntidad model) {
            try {
                model.Orden = 0;
                model.Random = false;
                model.Indicador = "";
                model.Activo = true;

                int id = preguntasBL.CrearPregunta(model);
                return Json(new { success = true, idPregunta = id });
            } catch(Exception ex) {
                return Json(new { success = false, displayMessage = $"Error: {ex.Message}" });
            }
        }

        // 🔹 CREAR PREGUNTA ATRIBUTO (con opciones automáticas)
       [HttpPost]
public ActionResult CrearPreguntaAtributo(PreguntaEntidad model) {
    try {
        if (string.IsNullOrEmpty(model.Indicador))
            return Json(new { success = false, displayMessage = "Debe ingresar el nombre del atributo (Indicador)." });

        // 🔹 Forzar a mayúsculas
        model.Indicador = model.Indicador.Trim().ToUpper();

        // 🔹 Verificar duplicados
        var preguntas = preguntasBL.ListadoPreguntas(1);
        if (preguntas.Any(p => p.Indicador.ToUpper() == model.Indicador)) {
            return Json(new { success = false, displayMessage = $"El nombre del atributo '{model.Indicador}' ya existe." });
        }

        model.Multi = false;
        model.Activo = true;

        int idPregunta = preguntasBL.CrearPregunta(model);

        // Opciones estándar
        var opciones = new Dictionary<string, int> {
            { "Muy insatisfecho", 1 },
            { "Insatisfecho", 2 },
            { "Neutral", 3 },
            { "Satisfecho", 4 },
            { "Muy satisfecho", 5 }
        };

        foreach (var kvp in opciones) {
            var opcion = new OpcionEntidad {
                IdPregunta = idPregunta,
                Texto = kvp.Key,
                TieneComentario = false,
                Valor = kvp.Value
            };
            opcionesBL.CrearOpcion(opcion);
        }

        return Json(new { success = true, idPregunta });
    } catch (Exception ex) {
        return Json(new { success = false, displayMessage = $"Error: {ex.Message}" });
    }
}


        // 🔹 EDITAR PREGUNTA (CSAT, NPS, Normal, Atributo)
        [HttpPost]
        public ActionResult EditarPregunta(PreguntaEntidad model) {
            try {
                bool ok = preguntasBL.EditarPregunta(model);
                return Json(new { success = ok });
            } catch(Exception ex) {
                return Json(new { success = false, displayMessage = $"Error: {ex.Message}" });
            }
        }

        // 🔹 ELIMINAR PREGUNTA (solo Normales y Atributo)
        [HttpPost]
        public ActionResult EliminarPregunta(int idPregunta, string tipo) {
            try {
                if(tipo == "CSAT" || tipo == "NPS") {
                    return Json(new { success = false, displayMessage = "No se pueden eliminar preguntas CSAT o NPS" });
                }

                bool ok = preguntasBL.EliminarPregunta(idPregunta);
                return Json(new { success = ok });
            } catch(Exception ex) {
                return Json(new { success = false, displayMessage = $"Error: {ex.Message}" });
            }
        }

        // ============================
        // 📌 OPCIONES
        // ============================

        // 🔹 CREAR OPCIÓN
        [HttpPost]
        public ActionResult CrearOpcion(OpcionEntidad model) {
            try {
                int id = opcionesBL.CrearOpcion(model);
                return Json(new { success = true, idOpcion = id });
            } catch(Exception ex) {
                return Json(new { success = false, displayMessage = $"Error: {ex.Message}" });
            }
        }

        // 🔹 EDITAR OPCIÓN
        [HttpPost]
        public ActionResult EditarOpcion(OpcionEntidad model) {
            try {
                bool ok = opcionesBL.EditarOpcion(model);
                return Json(new { success = ok });
            } catch(Exception ex) {
                return Json(new { success = false, displayMessage = $"Error: {ex.Message}" });
            }
        }

        // 🔹 ELIMINAR OPCIÓN
        [HttpPost]
        public ActionResult EliminarOpcion(int idOpcion) {
            try {
                bool ok = opcionesBL.EliminarOpcion(idOpcion);
                return Json(new { success = ok });
            } catch(Exception ex) {
                return Json(new { success = false, displayMessage = $"Error: {ex.Message}" });
            }
        }

        [HttpPost]
        public ActionResult TogglePregunta(int idPregunta) {
            bool success = false;
            string displayMessage = "";

            try {
                success = preguntasBL.TogglePregunta(idPregunta);
                displayMessage = success ? "Estado actualizado correctamente" : "No se pudo actualizar el estado";
            } catch(Exception ex) {
                displayMessage = $"Error: {ex.Message}";
            }

            return Json(new { success, displayMessage });
        }


        // ============================
        // 📌 COMENTARIOS
        // ============================
        [HttpPost]
        public ActionResult ListarEncuestadosConComentarios(int salaId, DateTime fechaInicio, DateTime fechaFin) {
            try {
                // 1. Traer encuestados (NPS)
                var encuestados = respuestaBL.ObtenerEncuestados(salaId, fechaInicio, fechaFin);

                // 2. Traer atributos (preguntas sin indicador)
                var atributos = respuestaBL.ObtenerRespuestasAtributos(salaId, fechaInicio, fechaFin);

                // 3. Unir por IdRespuestaEncuesta y armar JSON limpio
                var data = encuestados.Select(e => new
                {
                    e.IdRespuestaEncuesta,
                    e.IdSala,
                    e.IdTablet,
                    e.NroDocumento,
                    e.TipoDocumento,
                    FechaRespuesta = e.FechaRespuesta.ToString("yyyy-MM-dd HH:mm:ss"),
                    e.Nombre,
                    e.Correo,
                    e.Celular,
                    e.PreguntaNPS,
                    e.ValorRespuesta,
                    e.Clasificacion,

                    Comentarios = atributos
                        .Where(a => a.IdRespuestaEncuesta == e.IdRespuestaEncuesta) // 👈 ahora sí funciona
                        .Select(a => new {
                            a.IdPregunta,
                            a.Pregunta,
                            OpcionTexto = a.Opcion,
                            a.Comentario
                        })
                        .ToList()
                })
                .OrderBy(e => e.FechaRespuesta)
                .ThenBy(e => e.IdRespuestaEncuesta)
                .ToList();

                return Json(new {
                    success = true,
                    displayMessage = "OK",
                    data
                });
            } catch(Exception ex) {
                return Json(new {
                    success = false,
                    displayMessage = $"Error: {ex.Message}"
                });
            }
        }
        [HttpPost]
        public ActionResult ObtenerPreguntasConOpcionesYFlujo() {
            try {
                var preguntas = preguntasBL.ObtenerPreguntasConOpcionesYFlujo();

                return Json(new {
                    success = true,
                    displayMessage = "OK",
                    data = preguntas
                });
            } catch(Exception ex) {
                return Json(new {
                    success = false,
                    displayMessage = "Error: " + ex.Message
                });
            }
        }
        [HttpPost]
        public ActionResult ListadoConfiguraciones(int idSala) {
            bool success = false;
            string displayMessage = "";
            List<CSConfiguracionEntidad> data = new List<CSConfiguracionEntidad>();

            try {
                data = configuracionBL.ListadoConfiguracionesPorSala(idSala);
                success = true;
                displayMessage = "Configuraciones obtenidas correctamente";
            } catch(Exception ex) {
                displayMessage = $"Error: {ex.Message}";
            }

            return Json(new { success, displayMessage, data }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ActualizarValorBit(int idConfiguracion, bool nuevoValor, int idSala) {
            bool success = false;
            string displayMessage = "";

            try {
                success = configuracionBL.ActualizarValorBit(idConfiguracion, nuevoValor, idSala);
                displayMessage = success
                    ? "Estado actualizado correctamente"
                    : "No se pudo actualizar el estado";
            } catch(Exception ex) {
                displayMessage = $"Error: {ex.Message}";
            }

            return Json(new { success, displayMessage });
        }

        #endregion

    }
}