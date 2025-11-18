using CapaEntidad;
using CapaEntidad.Alertas;
using CapaEntidad.Reportes;
using CapaNegocio;
using CapaPresentacion.Reports;
using Microsoft.AspNet.SignalR.Json;
using Microsoft.AspNet.SignalR.Messaging;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ZedGraph;
using static CapaPresentacion.Controllers.Revisiones.RevisionesController;
using Newtonsoft.Json;

namespace CapaPresentacion.Controllers
{
    [seguridad]
    public class LogAlertaBilleterosController : Controller
    {
        private SalaBL salaBl = new SalaBL();
        private SEG_PermisoRolBL seg_PermisoRolBL = new SEG_PermisoRolBL();
        private string FirebaseKey = ConfigurationManager.AppSettings["firebaseServiceKey"];
        private string PathLogAlertaBilleteros = ConfigurationManager.AppSettings["PathLogAlertaBilleteros"];
        private LogAlertaBilleterosBL logBL = new LogAlertaBilleterosBL();
        private SEG_UsuarioBL usuariobl = new SEG_UsuarioBL();
        // GET: LogAlertaBilleteros
        public ActionResult LogsOnlineVista()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetLogAlertaBilleterosxFiltros(int[] salas,DateTime FechaInicio, DateTime FechaFin,int Tipo = 0)
        {
            var errormensaje = "";
            var lista = new List<LogAlertaBilleterosEntidad>();
            int cantElementos = (salas == null) ? 0 : salas.Length;
            string whereQuery = " ";
            try
            {
                if (cantElementos > 0)
                {
                    whereQuery = " and sal.[CodSala] in(" + "'" + String.Join("','", salas) + "'" + ") ";
                }
                if (Tipo != 0)
                {
                    whereQuery += " and myLog.Tipo=" + Tipo;
                }
                lista = logBL.GetLogAlertaBilleterosxFiltros(FechaInicio, FechaFin, whereQuery).OrderByDescending(x=>x.FechaRegistro).ToList();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }

            var jsonResult = Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }
        [HttpPost]
        public ActionResult GetAlertaBilleteroxId(int Id)
        {
            string mensaje = "";
            bool respuesta = false;
            LogAlertaBilleterosEntidad alerta = new LogAlertaBilleterosEntidad();
            try
            {
                alerta = logBL.GetAlertaBilleteroxId(Id);
                mensaje = "Obteniendo Registro";
                respuesta = true;
            }catch(Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje,respuesta,data=alerta});
        }
        [HttpPost]
        public ActionResult GetLogAlertaBilleterosxFiltrosExcel(int[] salas, DateTime FechaInicio, DateTime FechaFin, int Tipo = 0) {
            bool respuesta = false;
            string base64String = string.Empty;
            string excelName = string.Empty;
            string mensaje = string.Empty;
            var errormensaje = "";
            var lista = new List<LogAlertaBilleterosEntidad>();
            int cantElementos = (salas == null) ? 0 : salas.Length;
            string whereQuery = " ";
            SEG_UsuarioEntidad usuario = new SEG_UsuarioEntidad();

            try
            {
                if (cantElementos > 0)
                {
                    whereQuery = " and sal.[CodSala] in(" + "'" + String.Join("','", salas) + "'" + ") ";
                }
                if (Tipo != 0)
                {
                    whereQuery += " and myLog.Tipo=" + Tipo;
                }
                lista = logBL.GetLogAlertaBilleterosxFiltros(FechaInicio, FechaFin, whereQuery);
                if (lista.Count > 0)
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();
                    usuario = usuariobl.UsuarioEmpleadoIDObtenerJson(Convert.ToInt32(Session["UsuarioID"]));
                    var ws = excel.Workbook.Worksheets.Add("Logs Online");
                    //Cabeceras
                    ws.Cells["B1"].Value = "Reporte Logs Online";
                    ws.Cells["B1:C1"].Style.Font.Bold = true;

                    ws.Cells["B1"].Style.Font.Size = 20;
                    ws.Cells["B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["B1:G1"].Merge = true;
                    ws.Cells["B1:G1"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells["B3"].Value = "Usuario Registro";
                    ws.Cells["B3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["C3"].Value = usuario.UsuarioNombre;
                    ws.Cells["C3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["B3"].Style.Font.Bold = true;

                    ws.Cells["D3"].Value = "Fecha";
                    ws.Cells["D3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["E3"].Value = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss A");
                    ws.Cells["E3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["D3"].Style.Font.Bold = true;

                    //Headers
                    ws.Cells["B5"].Value = "Id";
                    ws.Cells["C5"].Value = "Sala";
                    ws.Cells["D5"].Value = "Fecha Registro";
                    ws.Cells["E5"].Value = "Tipo";
                    ws.Cells["F5"].Value = "Cod. Maquina";
                    ws.Cells["G5"].Value = "Información";

                    ws.Cells["B5:G5"].Style.Font.Bold = true;
                    ws.Cells["B5:G5"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells["B5:G5"].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                    ws.Cells["B5:G5"].Style.Font.Color.SetColor(Color.White);
                    ws.Cells["B5:G5"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    int fila = 6;
                    int total = lista.Count;
                    int inicioGrupo = 0;
                    int finGrupo = 0;
                    foreach (var registro in lista)
                    {
                        string strDetalle = string.Empty;
                        string tipo = string.Empty;
                        string codMaquina = string.Empty;
                        dynamic detalle = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(registro.Descripcion);
                        if (registro.Tipo == 1)
                        {
                            tipo = "Servicio Online";
                        }
                        else if (registro.Tipo == 2)
                        {
                            EVT_EventosOnlineEntidad evento = new EVT_EventosOnlineEntidad();
                            evento = JsonConvert.DeserializeObject<EVT_EventosOnlineEntidad>(registro.Descripcion);
                            tipo = "Evento Tecnologias";
                            codMaquina = detalle.CodMaquina;
                            strDetalle = $@"
                                        Cod_Even_OL:{evento.Cod_Even_OL}; CodTarjeta:{evento.CodTarjeta}{Environment.NewLine}
                                        CodMaquina:{evento.CodMaquina}; Fecha Online:{evento.Fecha.ToString("dd/MM/yyyy HH:mm:ss A")}{Environment.NewLine}
                                        Evento:{evento.Evento}{Environment.NewLine}";
                            string strDispositivo = string.Empty;

                            foreach (var dispositivo in evento.ListaEventoDispositivo)
                            {
                                strDispositivo += $@"- IdOnline:{dispositivo.DispositivoId}, Fecha Cierre: {dispositivo.FechaRegistro}, Maquina Cliente : {dispositivo.DispositivoNombre}, Usuario Cierre: {dispositivo.Usuario}{Environment.NewLine}";
                            }
                            strDetalle += strDispositivo;
                        }
                        else if (registro.Tipo == 3)
                        {
                            AlertBillNotificationReqEntidad alerta = new AlertBillNotificationReqEntidad();
                            alerta = JsonConvert.DeserializeObject<AlertBillNotificationReqEntidad>(registro.Descripcion);
                            tipo = "Alerta Billetero";
                            codMaquina = detalle.CodMaquina;
                        }
                        else
                        {
                            tipo = string.Empty;
                        }
                        ws.Cells[fila, 2].Value = registro.Id;
                        ws.Cells[fila, 3].Value = registro.NombreSala;
                        ws.Cells[fila, 4].Value = registro.FechaRegistro.ToString("dd-MM-yyyy hh:mm:ss");
                        ws.Cells[fila, 5].Value = tipo;
                        ws.Cells[fila, 6].Value = codMaquina;
                        ws.Cells[fila, 7].Value = registro.Preview;
                        fila++;
                        if (registro.Tipo == 2 || registro.Tipo == 3)
                        {
                            inicioGrupo = fila;
                            ws.Cells[fila, 2].Value = "Detalle";
                            ws.Cells[fila, 3].Value = strDetalle;
                            fila++;

                            fila++;
                            finGrupo = fila - 1;
                            for(var i = inicioGrupo; i <= finGrupo; i++) {
                                ws.Row(i).OutlineLevel = 1;
                                ws.Row(i).Collapsed = true;
                            }
                        }
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");
                    int filaFooter_ = fila;
                    ws.Cells["B" + filaFooter_ + ":G" + filaFooter_].Merge = true;
                    ws.Cells["B" + filaFooter_ + ":G" + filaFooter_].Style.Font.Bold = true;
                    ws.Cells["B" + filaFooter_ + ":G" + filaFooter_].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells["B" + filaFooter_ + ":G" + filaFooter_].Style.Fill.BackgroundColor.SetColor(colbackground);
                    ws.Cells["B" + filaFooter_ + ":G" + filaFooter_].Style.Font.Color.SetColor(Color.White);
                    ws.Cells["B" + filaFooter_ + ":G" + filaFooter_].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[filaFooter_, 2].Value = "Total : " + (total) + " Registros";


                    ws.Cells[5, 2, filaFooter_, 7].AutoFilter = true;

                    ws.Column(2).AutoFit();
                    ws.Column(3).Width = 30;
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 20;
                    ws.Column(6).Width = 30;
                    ws.Column(7).Width = 50;
                    ws.Column(7).BestFit=true;

                    excelName = "Reporte" +DateTime.Now+"_.xlsx";
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());

                    mensaje = "Descargando Archivo";
                    respuesta = true;
                }
                
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new
            {
                data = base64String,
                excelName,
                respuesta,
                mensaje
            };
            var result = new ContentResult
            {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;
        }

        #region Logs Reporte Nominal

        [HttpGet]
        public ActionResult LogsReporteNominal()
        {
            return View("~/Views/LogAlertaBilleteros/LogsReporteNominal.cshtml");
        }

        [HttpPost]
        public ActionResult ObtenerLogsReporteNominal(List<int> rooms, DateTime fromDate, DateTime toDate, List<int> types)
        {
            bool success = false;
            string message = "No hay registros";

            List<string> dates = new List<string>();
            List<ALEV_TipoReporteNominalEntidad> data = new List<ALEV_TipoReporteNominalEntidad>();

            try
            {
                List<ALEV_TipoReporteNominalEntidad> tipoReporteNominal = new List<ALEV_TipoReporteNominalEntidad>();

                foreach (int type in types)
                {
                    List<ALEV_ReporteNominalEntidad> reporteNominal = logBL.ObtenerReporteNominal(rooms, fromDate, toDate, type);

                    tipoReporteNominal.Add(new ALEV_TipoReporteNominalEntidad
                    {
                        Tipo = type,
                        Nombre = LogNominalReport.typeLogs.ContainsKey(type) ? LogNominalReport.typeLogs[type] : "",
                        Salas = reporteNominal
                    });
                }

                if(tipoReporteNominal.Any())
                {
                    dates = logBL.ObtenerRangoFechasNominal(fromDate, toDate);
                    data = tipoReporteNominal;

                    success = true;
                    message = "Datos obtenidos";
                }
            }
            catch (Exception exception)
            {
                message = exception.Message.ToString();
            }

            JsonResult jsonResult = Json(new
            {
                success,
                message,
                dates,
                data
            });

            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        [HttpPost]
        public ActionResult ExcelLogsReporteNominal(List<int> rooms, DateTime fromDate, DateTime toDate, List<int> types)
        {
            bool success = false;
            string message = "No hay registros";

            DateTime currentDate = DateTime.Now;
            string fileExtension = "xlsx";
            string fileName = $"{LogNominalReport.defaultTypeLogWs} {fromDate.ToString("dd-MM-yyyy")} al {toDate.ToString("dd-MM-yyyy")} {currentDate.ToString("HHmmss")}.{fileExtension}";

            string data = string.Empty;

            try
            {
                List<ALEV_TipoReporteNominalEntidad> tipoReporteNominal = new List<ALEV_TipoReporteNominalEntidad>();

                foreach (int type in types)
                {
                    string tipoNombre = LogNominalReport.typeLogs.ContainsKey(type) ? LogNominalReport.typeLogs[type] : "";
                    List<ALEV_ReporteNominalEntidad> reporteNominal = logBL.ObtenerReporteNominal(rooms, fromDate, toDate, type);

                    tipoReporteNominal.Add(new ALEV_TipoReporteNominalEntidad
                    {
                        Tipo = type,
                        Nombre = tipoNombre,
                        Salas = reporteNominal
                    });
                }

                if (tipoReporteNominal.Any())
                {
                    dynamic arguments = new
                    {
                        fromDate,
                        toDate
                    };

                    List<string> dates = logBL.ObtenerRangoFechasNominal(fromDate, toDate);

                    MemoryStream memoryStream = LogNominalReport.ExcelLogNominalMultiple(dates, tipoReporteNominal, arguments);
                    data = Convert.ToBase64String(memoryStream.ToArray());

                    success = true;
                    message = "Excel generado";
                }
            }
            catch (Exception exception)
            {
                message = exception.Message.ToString();
            }

            JsonResult jsonResult = Json(new
            {
                success,
                message,
                fileName,
                data
            });

            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ExcelLogsReporteNominalFree(List<int> rooms, DateTime fromDate, DateTime toDate, List<int> types)
        {
            bool success = false;
            string message = "No hay registros";

            DateTime currentDate = DateTime.Now;
            string fileExtension = "xlsx";
            string fileName = $"{LogNominalReport.defaultTypeLogWs} {fromDate.ToString("dd-MM-yyyy")} al {toDate.ToString("dd-MM-yyyy")} {currentDate.ToString("HHmmss")}.{fileExtension}";

            string data = string.Empty;

            try
            {
                List<ALEV_TipoReporteNominalEntidad> tipoReporteNominal = new List<ALEV_TipoReporteNominalEntidad>();

                foreach (int type in types)
                {
                    string tipoNombre = LogNominalReport.typeLogs.ContainsKey(type) ? LogNominalReport.typeLogs[type] : "";
                    List<ALEV_ReporteNominalEntidad> reporteNominal = logBL.ObtenerReporteNominal(rooms, fromDate, toDate, type);

                    tipoReporteNominal.Add(new ALEV_TipoReporteNominalEntidad
                    {
                        Tipo = type,
                        Nombre = tipoNombre,
                        Salas = reporteNominal
                    });
                }

                if (tipoReporteNominal.Any())
                {
                    dynamic arguments = new
                    {
                        fromDate,
                        toDate
                    };

                    List<string> dates = logBL.ObtenerRangoFechasNominal(fromDate, toDate);

                    MemoryStream memoryStream = LogNominalReport.ExcelLogNominalMultiple(dates, tipoReporteNominal, arguments);
                    data = Convert.ToBase64String(memoryStream.ToArray());  

                    success = true;
                    message = "Excel generado";
                }
            }
            catch (Exception exception)
            {
                message = exception.Message.ToString();
            }

            JsonResult jsonResult = Json(new
            {
                success,
                message,
                fileName,
                data
            });

            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        #endregion

        #region Logs Alerta Billeteros

        class EventoEntidad
        {
            int coqMaquina { get; set; }
            int evento { get; set; }
            string fecha { get; set; }
            string hora { get; set; }
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ObtenerSalaLogsFecha(int room, int type, DateTime date)
        {
            bool success = false;
            string message = "No hay registros";

            List<LogAlertaBilleterosEntidad> data = new List<LogAlertaBilleterosEntidad>();

            try
            {
                List<LogAlertaBilleterosEntidad> listLogs = logBL.ObtenerSalaLogsFecha(room, type, date);

                if (listLogs.Any())
                {
                    data = listLogs;

                    success = true;
                    message = "Datos obtenidos";
                }
            }
            catch (Exception exception)
            {
                message = exception.Message.ToString();
            }

            JsonResult jsonResult = Json(new
            {
                success,
                message,
                data
            });

            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult  ObtenerEventosDelDiaPorCodSalaMovil(int codSala) {
            bool success = false;
            string message = "";
            //List<LogAlertaBilleterosEvento> data = new List<LogAlertaBilleterosEvento>();
            List<EVT_EventosOnlineEntidad> listLogs = logBL.ObtenerEventosDelDiaPorCodSalaMovil(codSala);
            List<object> data = new List<object>();
            string nombreSala = salaBl.SalaListaIdJson(codSala).Nombre;
            try
            {
                foreach (var log in listLogs)
                {
                    LogAlertaBilleterosEvento log22 = JsonConvert.DeserializeObject<LogAlertaBilleterosEvento>(log.Evento);
                    object ra = new
                    {
                        log22.CodMaquina,
                        log22.Evento,
                        Fecha = log22.Fecha.ToString("dd/MM/yyyy"),
                        Hora = log22.Fecha.ToString("HH:mm:ss"),
                    };
                    data.Add(ra);
                }
                success = data.Count > 0;
                message = success ? $"Lista de eventos de la sala {nombreSala}." : $"No hay eventos en la sala {nombreSala} en este momento.";
            }
            catch (Exception exception)
            {
                message= $"Error al obtener los eventos de la sala {nombreSala}, {exception.Message}";
            }

            JsonResult jsonResult = Json(new {
                success,
                message,
                data
            });

            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }


        [seguridad(false)]
        [HttpPost]
        public ActionResult ObtenerEventosPorRangoFechaCodSalaMovil(int codSala, DateTime fechaIni, DateTime fechaFin)
        {

            bool success = false;
            string message = "";
            List<EVT_EventosOnlineEntidad> listLogs = logBL.ObtenerEventosPorRangoFechaCodSala(codSala, fechaIni, fechaFin);
            List<object> data = new List<object>();
            string nombreSala = salaBl.SalaListaIdJson(codSala).Nombre;

            try
            {
                foreach (var log in listLogs)
                {
                    LogAlertaBilleterosEvento json = JsonConvert.DeserializeObject<LogAlertaBilleterosEvento>(log.Evento);
                    object ra = new
                    {
                        json.CodMaquina,
                        json.Evento,
                        Fecha = json.Fecha.ToString("dd/MM/yyyy"),
                        Hora = json.Fecha.ToString("HH:mm:ss"),
                    };
                    data.Add(ra);
                }
                success = data.Count > 0;
                message = success ? $"Lista de eventos de la sala {nombreSala} entre {fechaIni.ToString("dd/MM/yyyy")} - {fechaFin.ToString("dd/MM/yyyy")}." : $"No hay eventos en la sala {nombreSala} entre {fechaIni.ToString("dd/MM/yyyy")} - {fechaFin.ToString("dd/MM/yyyy")}.";
            }
            catch (Exception exception)
            {
                message = $"Error al obtener los eventos de la sala {nombreSala}, {exception.Message}";
            }

            JsonResult jsonResult = Json(new
            {
                success,
                message,
                data
            });

            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }
        #endregion
    }
}