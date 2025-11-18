using CapaEntidad;
using CapaEntidad.SatisfaccionCliente.DTO;
using CapaEntidad.SatisfaccionCliente.Enum;
using CapaEntidad.SatisfaccionCliente.Reporte;
using CapaNegocio;
using CapaNegocio.SatisfaccionCliente;
using S3k.Utilitario.Excel;
using S3k.Utilitario.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.SatisfaccionCliente {
    [seguridad(true)]
    public class EscReportesController : Controller {
        private readonly ESC_RespuestaBL respuestaBL;
        private readonly SalaBL salaBL;

        public EscReportesController() {
            respuestaBL = new ESC_RespuestaBL();
            salaBL = new SalaBL();
        }

        #region Views
        public ActionResult RespuestasView() {
            return View("~/Views/SatisfaccionCliente/Reportes/Respuestas.cshtml");
        }
        #endregion

        #region Methods
        [HttpPost]
        public async Task<JsonResult> ObtenerReporteDeRespuestas(ESC_ReporteFiltro filtro) {
            bool success = false;
            string displayMessage;
            Esc_ReportePespuestasChart chart = new Esc_ReportePespuestasChart();
            List<ESC_RespuestaDto> table = new List<ESC_RespuestaDto>();

            chart.Puntajes.AddRange(new List<string> {
                ESC_Puntaje.MuyMalo.GetDisplayText(),
                ESC_Puntaje.Malo.GetDisplayText(),
                ESC_Puntaje.Regular.GetDisplayText(),
                ESC_Puntaje.Bueno.GetDisplayText(),
                ESC_Puntaje.MuyBueno.GetDisplayText()
            });

            try {
                foreach(int codSala in filtro.CodsSala) {
                    SalaEntidad sala = salaBL.ObtenerSalaPorCodigo(codSala);
                    List<ESC_RespuestaDto> respuestasSala = respuestaBL.ObtenerRespuestasPorCodSalaYFechas(codSala, filtro.FechaInicio, filtro.FechaFin);
                    List<ESC_ReportePorcentaje> reportePorcentaje = respuestaBL.ObtenerReportePorcentajes(codSala, filtro.FechaInicio, filtro.FechaFin);

                    ESC_ReporteRespuestasDataSet dataSet = new ESC_ReporteRespuestasDataSet() {
                        Sala = sala.Nombre,
                        Cantidades = reportePorcentaje.Select(x => Convert.ToInt32(x.CantidadRespuestas)).ToList(),
                    };

                    chart.DataSets.Add(dataSet);
                    table.AddRange(respuestasSala);
                }
                success = table.Count > 0;
                displayMessage = success ? "Reporte de respuestas de satisfacción." : "No hay registros.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            object data = new {
                table,
                chart
            };

            JsonResult jsonResult = Json(new { success, displayMessage, data });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region Excel
        [HttpPost]
        public async Task<JsonResult> GenerarExcelReporteDeRespuestas(ESC_ReporteFiltro filtro) {
            bool success = false;
            string displayMessage;

            #region Obtener los datos
            List<ESC_RespuestaDto> data = new List<ESC_RespuestaDto>();

            foreach(int codSala in filtro.CodsSala) {
                SalaEntidad sala = salaBL.ObtenerSalaPorCodigo(codSala);
                List<ESC_RespuestaDto> respuestas = respuestaBL.ObtenerRespuestasPorCodSalaYFechas(codSala, filtro.FechaInicio, filtro.FechaFin);
                data.AddRange(respuestas);
            }

            success = data.Count > 0;
            displayMessage = success ? "Lista de respuestas." : "No se encontraron respuestas con los filtros ingresados.";
            #endregion

            if(!success) {
                return Json(new { success, displayMessage });
            }

            #region Armar DataTable
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Cod. Sala", typeof(int));
            dataTable.Columns.Add("Sala", typeof(string));
            dataTable.Columns.Add("Número Documento", typeof(string));
            dataTable.Columns.Add("Cliente", typeof(string));
            dataTable.Columns.Add("Pregunta", typeof(string));
            dataTable.Columns.Add("Puntaje", typeof(int));
            dataTable.Columns.Add("Puntaje Texto", typeof(string));
            dataTable.Columns.Add("Fecha Registro", typeof(string));

            foreach(ESC_RespuestaDto item in data) {
                dataTable.Rows.Add(
                    item.Pregunta.Sala.CodSala,
                    item.Pregunta.Sala.Nombre,
                    item.Cliente.NumeroDocumento,
                    item.Cliente.NombreCompleto,
                    item.Pregunta.Texto,
                    item.Puntaje,
                    item.Puntaje.GetDisplayText(),
                    item.FechaRegistro.ToString("dd/MM/yyyy HH:MM:ss")
                );
            }
            #endregion

            #region CrearExcel
            try {
                ExportExcel exportExcel = new ExportExcel {
                    FileName = $"Respuestas satisfaccion cliente del {filtro.FechaInicio:dd/MM/yyyy} al {filtro.FechaFin:dd/MM/yyyy}",
                    SheetName = "Respuestas",
                    Data = dataTable,
                    Title = $"Respuestas de clientes",
                    FirstColumNumber = true,
                };

                byte[] excelBytes = ExcelHelper.GenerateExcel(exportExcel);
                displayMessage = success ? "Archivo excel generado correctamente." : "Ocurrio un error al intentar generar el archiv excel.";

                exportExcel.Data = null;

                object obj = new {
                    success,
                    bytes = Convert.ToBase64String(excelBytes),
                    displayMessage,
                    fileInfo = exportExcel
                };

                JsonResult json = Json(obj);
                json.MaxJsonLength = int.MaxValue;
                return json;
            } catch(Exception exp) {
                success = false;
                displayMessage = exp.Message + ". Llame al administrador.";
            }
            #endregion

            JsonResult jsonResult = Json(new { success, data, displayMessage });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion
    }
}