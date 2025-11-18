using CapaEntidad.AsistenciaCliente;
using CapaEntidad.Progresivo;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace CapaPresentacion.Utilitarios
{
    public class ReportesHelper
    {
        #region Reporte Empadronamiento Cliente

        public static MemoryStream ExcelEmpadronamientoCliente(List<EMC_EmpadronamientoClienteEntidad> listEmpadronamiento, dynamic parameters)
        {
            MemoryStream memoryStream = new MemoryStream();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet excelEmpadronamiento = excelPackage.Workbook.Worksheets.Add("Empadronamiento Cliente");
                WorksheetEmpadronamientoCliente(excelEmpadronamiento, listEmpadronamiento, parameters);

                // Save Excel
                excelPackage.SaveAs(memoryStream);
            }

            return memoryStream;
        }

        private static ExcelWorksheet WorksheetEmpadronamientoCliente(ExcelWorksheet worksheet, List<EMC_EmpadronamientoClienteEntidad> listEmpadronamiento, dynamic parameters)
        {
            // Parameters
            string roomName = parameters.roomName;
            DateTime fromDate = Convert.ToDateTime(parameters.fromDate);
            DateTime toDate = Convert.ToDateTime(parameters.toDate);

            // Row height
            double rowHeightlHeader = 20;

            // Colors
            Color headBackgroundColor = ColorTranslator.FromHtml("#003268");
            Color borderColorHead = ColorTranslator.FromHtml("#074B88");
            Color borderColorBody = ColorTranslator.FromHtml("#747474");

            // Header
            worksheet.Row(2).Height = rowHeightlHeader + 10;
            worksheet.Cells[2, 2, 2, 10].Merge = true;
            worksheet.Cells[2, 2, 2, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[2, 2, 2, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[2, 2].Style.Font.Size = 12;
            worksheet.Cells[2, 2].Style.Font.Bold = true;
            worksheet.Cells[2, 2].Style.Font.Italic = true;
            worksheet.Cells[2, 2].Value = $"{roomName} del {fromDate.ToString("dd/MM/yyyy")} al {toDate.ToString("dd/MM/yyyy")}";

            // Head
            int bodyFromRow = 4;
            int bodyFromCol = 2;
            int bodyToCol = 10;
            int headColFromRow = bodyFromRow + 1;

            // Head style
            worksheet.Row(bodyFromRow).Height = rowHeightlHeader;
            worksheet.Row(headColFromRow).Height = rowHeightlHeader + 10;
            worksheet.Cells[bodyFromRow, bodyFromCol, bodyFromRow, bodyToCol].Merge = true;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Font.Bold = true;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Fill.BackgroundColor.SetColor(headBackgroundColor);
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Font.Color.SetColor(Color.White);

            // Borders Table Head
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Top.Color.SetColor(borderColorHead);
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Left.Color.SetColor(borderColorHead);
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Right.Color.SetColor(borderColorHead);
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Bottom.Color.SetColor(borderColorHead);

            worksheet.Cells[bodyFromRow, bodyFromCol].Value = "EMPADRONAMIENTO DE CLIENTES";

            worksheet.Cells[headColFromRow, 2].Value = "DNI";
            worksheet.Cells[headColFromRow, 3].Value = "Nombres y Apellidos";
            worksheet.Cells[headColFromRow, 4].Value = "Zona";
            worksheet.Cells[headColFromRow, 5].Value = "Entregó documento de identidad";
            worksheet.Cells[headColFromRow, 6].Value = "Verificación DNI";
            worksheet.Cells[headColFromRow, 7].Value = "Apuesta";
            worksheet.Cells[headColFromRow, 8].Value = "Observaciones";
            worksheet.Cells[headColFromRow, 9].Value = "Estatus";
            worksheet.Cells[headColFromRow, 10].Value = "Hora de salida";

            worksheet.Cells[headColFromRow, 5].Style.WrapText = true;

            // Body
            int totalRecords = listEmpadronamiento.Count();
            int recordRow = headColFromRow + 1;
            int recordIndex = 1;

            foreach (EMC_EmpadronamientoClienteEntidad empadronamiento in listEmpadronamiento)
            {
                worksheet.Row(recordRow).Height = rowHeightlHeader;

                // Data
                worksheet.Cells[recordRow, 2].Value = empadronamiento.NroDoc;
                worksheet.Cells[recordRow, 3].Value = empadronamiento.NombreCompleto;
                worksheet.Cells[recordRow, 4].Value = empadronamiento.ZonaNombreIn;
                worksheet.Cells[recordRow, 5].Value = empadronamiento.entrega_dni ? "SI" : "NO";
                worksheet.Cells[recordRow, 6].Value = empadronamiento.reniec ? "SI" : "NO";
                worksheet.Cells[recordRow, 7].Value = empadronamiento.apuestaImportante.ToString("#,##0.00");
                worksheet.Cells[recordRow, 8].Value = empadronamiento.observacion;
                worksheet.Cells[recordRow, 9].Value = empadronamiento.Estado == 1 ? "Completado" : "Pendiente";
                worksheet.Cells[recordRow, 10].Value = empadronamiento.Estado == 1 ? empadronamiento.FechaSalida.ToString("hh:mm:ss tt") : "---";

                recordRow++;
                recordIndex++;
            }

            int recordFromRow = headColFromRow + 1;
            int bodyToRow = headColFromRow + totalRecords;

            if (totalRecords > 0)
            {
                // Borders Table Body
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Top.Color.SetColor(borderColorBody);
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Left.Color.SetColor(borderColorBody);
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Right.Color.SetColor(borderColorBody);
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Bottom.Color.SetColor(borderColorBody);

                // Alignment Cells
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                // Alignment Columns
                worksheet.Cells[recordFromRow, 2, bodyToRow, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[recordFromRow, 5, bodyToRow, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[recordFromRow, 7, bodyToRow, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells[recordFromRow, 9, bodyToRow, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Filters
                worksheet.Cells[headColFromRow, bodyFromCol, bodyToRow, bodyToCol].AutoFilter = true;
            }

            // Footer style
            int footerFromRow = bodyToRow + 1;

            worksheet.Row(footerFromRow).Height = rowHeightlHeader;
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Merge = true;
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.Font.Bold = true;
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.Fill.BackgroundColor.SetColor(headBackgroundColor);
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.Font.Color.SetColor(Color.White);

            // Borders Table Head
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.Border.Top.Color.SetColor(borderColorHead);
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.Border.Left.Color.SetColor(borderColorHead);
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.Border.Right.Color.SetColor(borderColorHead);
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.Border.Bottom.Color.SetColor(borderColorHead);

            worksheet.Cells[footerFromRow, bodyFromCol].Value = $"Total : {totalRecords} Registros";

            // Columns Width
            worksheet.Column(2).Width = 20;
            worksheet.Column(3).AutoFit();
            worksheet.Column(4).Width = 20;
            worksheet.Column(5).Width = 20;
            worksheet.Column(6).Width = 20;
            worksheet.Column(7).Width = 20;
            worksheet.Column(8).AutoFit();
            worksheet.Column(9).Width = 20;
            worksheet.Column(10).AutoFit();

            return worksheet;
        }

        #endregion

        #region Reporte Alerta Progresivo

        public static MemoryStream ExcelAlertasProgresivo(List<AlertaProgresivoEntidad> listaAlertasProgresivo, dynamic parameters)
        {
            MemoryStream memoryStream = new MemoryStream();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet excelAlertasProgresivo = excelPackage.Workbook.Worksheets.Add("Alertas Registro Progresivo");
                WorksheetAlertasProgresivo(excelAlertasProgresivo, listaAlertasProgresivo, parameters);

                foreach (AlertaProgresivoEntidad alertaProgresivo in listaAlertasProgresivo)
                {
                    ExcelWorksheet excelAlertaProgresivo = excelPackage.Workbook.Worksheets.Add($"ARP {alertaProgresivo.Id}");
                    WorksheetAlertaProgresivo(excelAlertaProgresivo, alertaProgresivo);
                }

                // Save Excel
                excelPackage.SaveAs(memoryStream);
            }

            return memoryStream;
        }

        private static ExcelWorksheet WorksheetAlertasProgresivo(ExcelWorksheet worksheet, List<AlertaProgresivoEntidad> listaAlertasProgresivo, dynamic parameters)
        {
            // Parameters
            string roomName = parameters.roomName;
            DateTime fromDate = Convert.ToDateTime(parameters.fromDate);
            DateTime toDate = Convert.ToDateTime(parameters.toDate);

            // Row height
            double rowHeightlHeader = 20;

            // Colors
            Color headBackgroundColor = ColorTranslator.FromHtml("#003268");
            Color borderColorHead = ColorTranslator.FromHtml("#074B88");
            Color borderColorBody = ColorTranslator.FromHtml("#747474");

            // Header
            worksheet.Row(2).Height = rowHeightlHeader + 10;
            worksheet.Cells[2, 2, 2, 5].Merge = true;
            worksheet.Cells[2, 2, 2, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[2, 2, 2, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[2, 2].Style.Font.Size = 12;
            worksheet.Cells[2, 2].Style.Font.Bold = true;
            worksheet.Cells[2, 2].Style.Font.Italic = true;
            worksheet.Cells[2, 2].Value = $"{roomName} del {fromDate.ToString("dd/MM/yyyy")} al {toDate.ToString("dd/MM/yyyy")}";

            // Head
            int bodyFromRow = 4;
            int bodyFromCol = 2;
            int bodyToCol = 5;
            int headColFromRow = bodyFromRow + 1;

            // Head style
            worksheet.Row(bodyFromRow).Height = rowHeightlHeader;
            worksheet.Row(headColFromRow).Height = rowHeightlHeader + 5;
            worksheet.Cells[bodyFromRow, bodyFromCol, bodyFromRow, bodyToCol].Merge = true;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Font.Bold = true;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Fill.BackgroundColor.SetColor(headBackgroundColor);
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Font.Color.SetColor(Color.White);

            // Borders Table Head
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Top.Color.SetColor(borderColorHead);
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Left.Color.SetColor(borderColorHead);
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Right.Color.SetColor(borderColorHead);
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Bottom.Color.SetColor(borderColorHead);

            worksheet.Cells[bodyFromRow, bodyFromCol].Value = "Alertas Registro Progresivo";

            worksheet.Cells[headColFromRow, 2].Value = "Sala";
            worksheet.Cells[headColFromRow, 3].Value = "Progresivo";
            worksheet.Cells[headColFromRow, 4].Value = "Descripción";
            worksheet.Cells[headColFromRow, 5].Value = "Fecha Registro";

            worksheet.Cells[headColFromRow, 4].Style.WrapText = true;

            // Body
            int totalRecords = listaAlertasProgresivo.Count();
            int recordRow = headColFromRow + 1;
            int recordIndex = 1;

            foreach (AlertaProgresivoEntidad alertaProgresivo in listaAlertasProgresivo)
            {
                worksheet.Row(recordRow).Height = rowHeightlHeader;

                // Data
                worksheet.Cells[recordRow, 2].Value = alertaProgresivo.SalaNombre;
                worksheet.Cells[recordRow, 3].Value = alertaProgresivo.ProgresivoNombre;
                worksheet.Cells[recordRow, 4].Value = alertaProgresivo.Descripcion;
                worksheet.Cells[recordRow, 5].Value = alertaProgresivo.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss tt");
                worksheet.Cells[recordRow, 6].Value = "Ver Detalle";

                // Formula
                worksheet.Cells[recordRow, 6].Hyperlink = new Uri($"#'ARP {alertaProgresivo.Id}'!A1", UriKind.Relative);

                recordRow++;
                recordIndex++;
            }

            int recordFromRow = headColFromRow + 1;
            int bodyToRow = headColFromRow + totalRecords;

            if (totalRecords > 0)
            {
                // Borders Table Body
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Top.Color.SetColor(borderColorBody);
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Left.Color.SetColor(borderColorBody);
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Right.Color.SetColor(borderColorBody);
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Bottom.Color.SetColor(borderColorBody);

                // Alignment Cells
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                // Alignment Columns
                worksheet.Cells[recordFromRow, 2, bodyToRow, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells[recordFromRow, 5, bodyToRow, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Filters
                worksheet.Cells[headColFromRow, bodyFromCol, bodyToRow, bodyToCol].AutoFilter = true;
            }

            // Footer style
            int footerFromRow = bodyToRow + 1;

            worksheet.Row(footerFromRow).Height = rowHeightlHeader;
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Merge = true;
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.Font.Bold = true;
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.Fill.BackgroundColor.SetColor(headBackgroundColor);
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.Font.Color.SetColor(Color.White);

            // Borders Table Head
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.Border.Top.Color.SetColor(borderColorHead);
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.Border.Left.Color.SetColor(borderColorHead);
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.Border.Right.Color.SetColor(borderColorHead);
            worksheet.Cells[footerFromRow, bodyFromCol, footerFromRow, bodyToCol].Style.Border.Bottom.Color.SetColor(borderColorHead);

            worksheet.Cells[footerFromRow, bodyFromCol].Value = $"Total : {totalRecords} Registros";

            // Columns Width
            worksheet.Column(2).Width = 40;
            worksheet.Column(3).Width = 50;
            worksheet.Column(4).Width = 70;
            worksheet.Column(5).Width = 30;
            worksheet.Column(6).AutoFit();

            return worksheet;
        }

        public static ExcelWorksheet WorksheetAlertaProgresivo(ExcelWorksheet worksheet, AlertaProgresivoEntidad alertaProgresivo)
        {
            // dictionary
            Dictionary<int, string> namePozos = new Dictionary<int, string>
            {
                { 1, "Pozo Superior" },
                { 2, "Pozo Medio" },
                { 3, "Pozo Inferior" }
            };

            // Colors
            Color backgroundColorBlue = ColorTranslator.FromHtml("#00226C");
            Color backgroundColorRed = ColorTranslator.FromHtml("#D60F10");
            Color borderColor = ColorTranslator.FromHtml("#000000");
            Color backgroundColorCurrent = ColorTranslator.FromHtml("#AFF4AA");
            Color backgroundColorOld = ColorTranslator.FromHtml("#F4AFAA");

            // Row height
            double rowHeightlHeader = 20;

            // Title
            worksheet.Row(2).Height = rowHeightlHeader + 5;

            worksheet.Cells[2, 2, 2, 5].Merge = true;
            worksheet.Cells[2, 2, 2, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[2, 2, 2, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[2, 2].Style.Font.Size = 12;
            worksheet.Cells[2, 2].Style.Font.Bold = true;
            worksheet.Cells[2, 2].Style.Font.Italic = true;
            worksheet.Cells[2, 2].Value = "Alerta Registro Progresivo";

            // Header
            worksheet.Cells[3, 2, 3, 5].Merge = true;
            worksheet.Cells[4, 2, 4, 5].Merge = true;
            worksheet.Cells[5, 2, 5, 5].Merge = true;
            worksheet.Cells[6, 2, 6, 5].Merge = true;
            worksheet.Cells[3, 2, 6, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[3, 2, 6, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            worksheet.Cells[3, 2].Value = $"Sala: {alertaProgresivo.SalaNombre}";
            worksheet.Cells[4, 2].Value = $"Progresivo: {alertaProgresivo.ProgresivoNombre}";
            worksheet.Cells[5, 2].Value = $"Descripción: {alertaProgresivo.Descripcion}";
            worksheet.Cells[6, 2].Value = $"Fecha: {alertaProgresivo.FechaRegistro.ToString("dd/MM/yyyy HH:mm:ss")}";

            int fromRow = 8;
            int fromCol = 2;
            int fromColPozo = 3;
            int fromCol2 = 0;

            AlertaProgresivoDetalleEntidad detalleActual = alertaProgresivo.Detalle.Where(x => x.ProActual == true).FirstOrDefault();
            AlertaProgresivoDetalleEntidad detalleAnterior = alertaProgresivo.Detalle.Where(x => x.ProActual == false).FirstOrDefault();

            List<string> noCompareProperties = new List<string>
            {
                "Id",
                "FechaRegistro",
                "AlertaId",
                "DetalleId",
                "ProActual",
                "Anterior",
                "ActualOculto",
                "AnteriorOculto",
                "Fecha"
            };

            List<ResultadoEquals> resultadoEquals = new ObjectsHelper().CompareObjects(detalleActual, detalleAnterior, noCompareProperties);
            List<ResultadoEquals> diferentes = resultadoEquals.Where(x => !x.Estado && !x.Campo.Equals("Pozos")).ToList();

            if (detalleActual != null)
            {
                int toCol = detalleActual.Pozos.Count + fromCol;
                fromCol2 = toCol;

                // merge
                worksheet.Cells[fromRow, fromCol, fromRow, toCol].Merge = true;
                worksheet.Cells[fromRow + 1, fromCol, fromRow + 1, toCol].Merge = true;
                worksheet.Cells[fromRow + 2, fromCol, fromRow + 2, toCol].Merge = true;
                worksheet.Cells[fromRow + 3, fromCol, fromRow + 3, toCol].Merge = true;
                worksheet.Cells[fromRow + 4, fromCol, fromRow + 4, toCol].Merge = true;
                worksheet.Cells[fromRow + 5, fromCol, fromRow + 5, toCol].Merge = true;

                // data
                worksheet.Cells[fromRow, fromCol].Value = "Progresivo Actual";
                worksheet.Cells[fromRow + 1, fromCol].Value = $"Moneda: {detalleActual.Simbolo}";
                worksheet.Cells[fromRow + 2, fromCol].Value = $"Duración en pantalla: {detalleActual.DuracionPantalla} segundos";
                worksheet.Cells[fromRow + 3, fromCol].Value = $"Número de jugadores: {detalleActual.NroJugadores}";
                worksheet.Cells[fromRow + 4, fromCol].Value = $"Número de pozos: {detalleActual.NroPozos}";
                worksheet.Cells[fromRow + 5, fromCol].Value = $"Imagen: {detalleActual.ProgresivoImagenNombre}";

                if (diferentes.Any(x => x.Campo.Equals("Simbolo")))
                {
                    worksheet.Cells[fromRow + 1, fromCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[fromRow + 1, fromCol].Style.Fill.BackgroundColor.SetColor(backgroundColorCurrent);
                }

                if (diferentes.Any(x => x.Campo.Equals("DuracionPantalla")))
                {
                    worksheet.Cells[fromRow + 2, fromCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[fromRow + 2, fromCol].Style.Fill.BackgroundColor.SetColor(backgroundColorCurrent);
                }

                if (diferentes.Any(x => x.Campo.Equals("NroJugadores")))
                {
                    worksheet.Cells[fromRow + 3, fromCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[fromRow + 3, fromCol].Style.Fill.BackgroundColor.SetColor(backgroundColorCurrent);
                }

                if (diferentes.Any(x => x.Campo.Equals("NroPozos")))
                {
                    worksheet.Cells[fromRow + 4, fromCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[fromRow + 4, fromCol].Style.Fill.BackgroundColor.SetColor(backgroundColorCurrent);
                }

                if (diferentes.Any(x => x.Campo.Equals("ProgresivoImagenNombre")))
                {
                    worksheet.Cells[fromRow + 5, fromCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[fromRow + 5, fromCol].Style.Fill.BackgroundColor.SetColor(backgroundColorCurrent);
                }

                // indicadores
                int fromRowInd = fromRow + 6;

                worksheet.Cells[fromRowInd + 1, fromCol].Value = "Premio base";
                worksheet.Cells[fromRowInd + 2, fromCol].Value = "Premio mínimo";
                worksheet.Cells[fromRowInd + 3, fromCol].Value = "Premio máximo";
                worksheet.Cells[fromRowInd + 4, fromCol].Value = "Inc. pozo (Ej. 0.01 = 1%)";
                worksheet.Cells[fromRowInd + 5, fromCol].Value = "Restricción de Apuesta (Créditos)";
                worksheet.Cells[fromRowInd + 6, fromCol].Value = "Nro de Jugadores";
                worksheet.Cells[fromRowInd + 7, fromCol].Value = "Monto oculto minimo";
                worksheet.Cells[fromRowInd + 8, fromCol].Value = "Monto oculto máximo";
                worksheet.Cells[fromRowInd + 9, fromCol].Value = "Inc. pozo oculto (Ej. 0.01 = 1%)";
                worksheet.Cells[fromRowInd + 10, fromCol].Value = "Dificultad";
                worksheet.Cells[fromRowInd + 11, fromCol].Value = "Pozo actual";

                // Pozos
                int initPozo = fromColPozo;

                foreach (AlertaProgresivoPozoEntidad pozo in detalleActual.Pozos)
                {
                    worksheet.Cells[fromRowInd, initPozo].Value = namePozos.ContainsKey(pozo.TipoPozo) ? namePozos[pozo.TipoPozo] : "";

                    initPozo++;
                }

                initPozo = fromColPozo;

                foreach (AlertaProgresivoPozoEntidad pozo in detalleActual.Pozos)
                {
                    worksheet.Cells[fromRowInd + 1, initPozo].Value = pozo.MontoBase;

                    if (diferentes.Any(x => x.Campo.Equals("MontoBase") && x.Pozo == pozo.TipoPozo))
                    {
                        worksheet.Cells[fromRowInd + 1, initPozo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[fromRowInd + 1, initPozo].Style.Fill.BackgroundColor.SetColor(backgroundColorCurrent);
                    }

                    initPozo++;
                }

                initPozo = fromColPozo;

                foreach (AlertaProgresivoPozoEntidad pozo in detalleActual.Pozos)
                {
                    worksheet.Cells[fromRowInd + 2, initPozo].Value = pozo.MontoMin;

                    if (diferentes.Any(x => x.Campo.Equals("MontoMin") && x.Pozo == pozo.TipoPozo))
                    {
                        worksheet.Cells[fromRowInd + 2, initPozo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[fromRowInd + 2, initPozo].Style.Fill.BackgroundColor.SetColor(backgroundColorCurrent);
                    }

                    initPozo++;
                }

                initPozo = fromColPozo;

                foreach (AlertaProgresivoPozoEntidad pozo in detalleActual.Pozos)
                {
                    worksheet.Cells[fromRowInd + 3, initPozo].Value = pozo.MontoMax;

                    if (diferentes.Any(x => x.Campo.Equals("MontoMax") && x.Pozo == pozo.TipoPozo))
                    {
                        worksheet.Cells[fromRowInd + 3, initPozo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[fromRowInd + 3, initPozo].Style.Fill.BackgroundColor.SetColor(backgroundColorCurrent);
                    }

                    initPozo++;
                }

                initPozo = fromColPozo;

                foreach (AlertaProgresivoPozoEntidad pozo in detalleActual.Pozos)
                {
                    worksheet.Cells[fromRowInd + 4, initPozo].Value = pozo.IncPozo1;

                    if (diferentes.Any(x => x.Campo.Equals("IncPozo1") && x.Pozo == pozo.TipoPozo))
                    {
                        worksheet.Cells[fromRowInd + 4, initPozo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[fromRowInd + 4, initPozo].Style.Fill.BackgroundColor.SetColor(backgroundColorCurrent);
                    }

                    initPozo++;
                }

                initPozo = fromColPozo;

                foreach (AlertaProgresivoPozoEntidad pozo in detalleActual.Pozos)
                {
                    worksheet.Cells[fromRowInd + 5, initPozo].Value = pozo.RsApuesta;

                    if (diferentes.Any(x => x.Campo.Equals("RsApuesta") && x.Pozo == pozo.TipoPozo))
                    {
                        worksheet.Cells[fromRowInd + 5, initPozo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[fromRowInd + 5, initPozo].Style.Fill.BackgroundColor.SetColor(backgroundColorCurrent);
                    }

                    initPozo++;
                }

                initPozo = fromColPozo;

                foreach (AlertaProgresivoPozoEntidad pozo in detalleActual.Pozos)
                {
                    worksheet.Cells[fromRowInd + 6, initPozo].Value = pozo.RsJugadores;

                    if (diferentes.Any(x => x.Campo.Equals("RsJugadores") && x.Pozo == pozo.TipoPozo))
                    {
                        worksheet.Cells[fromRowInd + 6, initPozo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[fromRowInd + 6, initPozo].Style.Fill.BackgroundColor.SetColor(backgroundColorCurrent);
                    }

                    initPozo++;
                }

                initPozo = fromColPozo;

                foreach (AlertaProgresivoPozoEntidad pozo in detalleActual.Pozos)
                {
                    worksheet.Cells[fromRowInd + 7, initPozo].Value = pozo.MontoOcMin;

                    if (diferentes.Any(x => x.Campo.Equals("MontoOcMin") && x.Pozo == pozo.TipoPozo))
                    {
                        worksheet.Cells[fromRowInd + 7, initPozo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[fromRowInd + 7, initPozo].Style.Fill.BackgroundColor.SetColor(backgroundColorCurrent);
                    }

                    initPozo++;
                }

                initPozo = fromColPozo;

                foreach (AlertaProgresivoPozoEntidad pozo in detalleActual.Pozos)
                {
                    worksheet.Cells[fromRowInd + 8, initPozo].Value = pozo.MontoOcMax;

                    if (diferentes.Any(x => x.Campo.Equals("MontoOcMax") && x.Pozo == pozo.TipoPozo))
                    {
                        worksheet.Cells[fromRowInd + 8, initPozo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[fromRowInd + 8, initPozo].Style.Fill.BackgroundColor.SetColor(backgroundColorCurrent);
                    }

                    initPozo++;
                }

                initPozo = fromColPozo;

                foreach (AlertaProgresivoPozoEntidad pozo in detalleActual.Pozos)
                {
                    worksheet.Cells[fromRowInd + 9, initPozo].Value = pozo.IncOcPozo1;

                    if (diferentes.Any(x => x.Campo.Equals("IncOcPozo1") && x.Pozo == pozo.TipoPozo))
                    {
                        worksheet.Cells[fromRowInd + 9, initPozo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[fromRowInd + 9, initPozo].Style.Fill.BackgroundColor.SetColor(backgroundColorCurrent);
                    }

                    initPozo++;
                }

                initPozo = fromColPozo;

                foreach (AlertaProgresivoPozoEntidad pozo in detalleActual.Pozos)
                {
                    worksheet.Cells[fromRowInd + 10, initPozo].Value = pozo.Dificultad_desc;

                    if (diferentes.Any(x => x.Campo.Equals("Dificultad_desc") && x.Pozo == pozo.TipoPozo))
                    {
                        worksheet.Cells[fromRowInd + 10, initPozo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[fromRowInd + 10, initPozo].Style.Fill.BackgroundColor.SetColor(backgroundColorCurrent);
                    }

                    initPozo++;
                }

                initPozo = fromColPozo;

                foreach (AlertaProgresivoPozoEntidad pozo in detalleActual.Pozos)
                {
                    worksheet.Cells[fromRowInd + 11, initPozo].Value = pozo.Actual;

                    if (diferentes.Any(x => x.Campo.Equals("Actual") && x.Pozo == pozo.TipoPozo))
                    {
                        worksheet.Cells[fromRowInd + 11, initPozo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[fromRowInd + 11, initPozo].Style.Fill.BackgroundColor.SetColor(backgroundColorCurrent);
                    }

                    initPozo++;
                }

                int toRow = fromRow + 17;

                // Header style
                worksheet.Cells[fromRow, fromCol, fromRow, toCol].Style.Font.Bold = true;
                worksheet.Cells[fromRow, fromCol, fromRow, toCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[fromRow, fromCol, fromRow, toCol].Style.Fill.BackgroundColor.SetColor(backgroundColorBlue);
                worksheet.Cells[fromRow, fromCol, fromRow, toCol].Style.Font.Color.SetColor(Color.White);

                // Head style
                worksheet.Cells[fromRowInd, fromCol, fromRowInd, toCol].Style.Font.Bold = true;
                worksheet.Cells[fromRowInd, fromCol, fromRowInd, toCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[fromRowInd, fromCol, fromRowInd, toCol].Style.Fill.BackgroundColor.SetColor(backgroundColorBlue);
                worksheet.Cells[fromRowInd, fromCol, fromRowInd, toCol].Style.Font.Color.SetColor(Color.White);

                // Borders Table
                worksheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Top.Color.SetColor(borderColor);
                worksheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Left.Color.SetColor(borderColor);
                worksheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Right.Color.SetColor(borderColor);
                worksheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Bottom.Color.SetColor(borderColor);

                // Alignment
                worksheet.Cells[fromRow, fromCol, toRow, toCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[fromRow, fromCol, toRow, toCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[fromRow + 1, fromCol, toRow, fromCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells[toRow, fromCol, toRow, toCol].Style.Font.Bold = true;
            }

            if(detalleAnterior != null)
            {
                int fromColTwo = fromCol + fromCol2;
                int toColTwo = detalleAnterior.Pozos.Count + fromColTwo;

                // merge
                worksheet.Cells[fromRow, fromColTwo, fromRow, toColTwo].Merge = true;
                worksheet.Cells[fromRow + 1, fromColTwo, fromRow + 1, toColTwo].Merge = true;
                worksheet.Cells[fromRow + 2, fromColTwo, fromRow + 2, toColTwo].Merge = true;
                worksheet.Cells[fromRow + 3, fromColTwo, fromRow + 3, toColTwo].Merge = true;
                worksheet.Cells[fromRow + 4, fromColTwo, fromRow + 4, toColTwo].Merge = true;
                worksheet.Cells[fromRow + 5, fromColTwo, fromRow + 5, toColTwo].Merge = true;

                // data
                worksheet.Cells[fromRow, fromColTwo].Value = "Progresivo Anterior";
                worksheet.Cells[fromRow + 1, fromColTwo].Value = $"Moneda: {detalleAnterior.Simbolo}";
                worksheet.Cells[fromRow + 2, fromColTwo].Value = $"Duración en pantalla: {detalleAnterior.DuracionPantalla} segundos";
                worksheet.Cells[fromRow + 3, fromColTwo].Value = $"Número de jugadores: {detalleAnterior.NroJugadores}";
                worksheet.Cells[fromRow + 4, fromColTwo].Value = $"Número de pozos: {detalleAnterior.NroPozos}";
                worksheet.Cells[fromRow + 5, fromColTwo].Value = $"Imagen: {detalleAnterior.ProgresivoImagenNombre}";

                if (diferentes.Any(x => x.Campo.Equals("Simbolo")))
                {
                    worksheet.Cells[fromRow + 1, fromColTwo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[fromRow + 1, fromColTwo].Style.Fill.BackgroundColor.SetColor(backgroundColorOld);
                }

                if (diferentes.Any(x => x.Campo.Equals("DuracionPantalla")))
                {
                    worksheet.Cells[fromRow + 2, fromColTwo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[fromRow + 2, fromColTwo].Style.Fill.BackgroundColor.SetColor(backgroundColorOld);
                }

                if (diferentes.Any(x => x.Campo.Equals("NroJugadores")))
                {
                    worksheet.Cells[fromRow + 3, fromColTwo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[fromRow + 3, fromColTwo].Style.Fill.BackgroundColor.SetColor(backgroundColorOld);
                }

                if (diferentes.Any(x => x.Campo.Equals("NroPozos")))
                {
                    worksheet.Cells[fromRow + 4, fromColTwo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[fromRow + 4, fromColTwo].Style.Fill.BackgroundColor.SetColor(backgroundColorOld);
                }

                if (diferentes.Any(x => x.Campo.Equals("ProgresivoImagenNombre")))
                {
                    worksheet.Cells[fromRow + 5, fromColTwo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[fromRow + 5, fromColTwo].Style.Fill.BackgroundColor.SetColor(backgroundColorOld);
                }

                // indicadores
                int fromRowInd = fromRow + 6;

                worksheet.Cells[fromRowInd + 1, fromColTwo].Value = "Premio base";
                worksheet.Cells[fromRowInd + 2, fromColTwo].Value = "Premio mínimo";
                worksheet.Cells[fromRowInd + 3, fromColTwo].Value = "Premio máximo";
                worksheet.Cells[fromRowInd + 4, fromColTwo].Value = "Inc. pozo (Ej. 0.01 = 1%)";
                worksheet.Cells[fromRowInd + 5, fromColTwo].Value = "Restricción de Apuesta (Créditos)";
                worksheet.Cells[fromRowInd + 6, fromColTwo].Value = "Nro de Jugadores";
                worksheet.Cells[fromRowInd + 7, fromColTwo].Value = "Monto oculto minimo";
                worksheet.Cells[fromRowInd + 8, fromColTwo].Value = "Monto oculto máximo";
                worksheet.Cells[fromRowInd + 9, fromColTwo].Value = "Inc. pozo oculto (Ej. 0.01 = 1%)";
                worksheet.Cells[fromRowInd + 10, fromColTwo].Value = "Dificultad";
                worksheet.Cells[fromRowInd + 11, fromColTwo].Value = "Pozo actual";

                // Pozos
                fromColPozo = fromColTwo + 1;
                int initPozo = fromColPozo;

                foreach (AlertaProgresivoPozoEntidad pozo in detalleAnterior.Pozos)
                {
                    worksheet.Cells[fromRowInd, initPozo].Value = namePozos.ContainsKey(pozo.TipoPozo) ? namePozos[pozo.TipoPozo] : "";

                    initPozo++;
                }

                initPozo = fromColPozo;

                foreach (AlertaProgresivoPozoEntidad pozo in detalleAnterior.Pozos)
                {
                    worksheet.Cells[fromRowInd + 1, initPozo].Value = pozo.MontoBase;

                    if (diferentes.Any(x => x.Campo.Equals("MontoBase") && x.Pozo == pozo.TipoPozo))
                    {
                        worksheet.Cells[fromRowInd + 1, initPozo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[fromRowInd + 1, initPozo].Style.Fill.BackgroundColor.SetColor(backgroundColorOld);
                    }

                    initPozo++;
                }

                initPozo = fromColPozo;

                foreach (AlertaProgresivoPozoEntidad pozo in detalleAnterior.Pozos)
                {
                    worksheet.Cells[fromRowInd + 2, initPozo].Value = pozo.MontoMin;

                    if (diferentes.Any(x => x.Campo.Equals("MontoMin") && x.Pozo == pozo.TipoPozo))
                    {
                        worksheet.Cells[fromRowInd + 2, initPozo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[fromRowInd + 2, initPozo].Style.Fill.BackgroundColor.SetColor(backgroundColorOld);
                    }

                    initPozo++;
                }

                initPozo = fromColPozo;

                foreach (AlertaProgresivoPozoEntidad pozo in detalleAnterior.Pozos)
                {
                    worksheet.Cells[fromRowInd + 3, initPozo].Value = pozo.MontoMax;

                    if (diferentes.Any(x => x.Campo.Equals("MontoMax") && x.Pozo == pozo.TipoPozo))
                    {
                        worksheet.Cells[fromRowInd + 3, initPozo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[fromRowInd + 3, initPozo].Style.Fill.BackgroundColor.SetColor(backgroundColorOld);
                    }

                    initPozo++;
                }

                initPozo = fromColPozo;

                foreach (AlertaProgresivoPozoEntidad pozo in detalleAnterior.Pozos)
                {
                    worksheet.Cells[fromRowInd + 4, initPozo].Value = pozo.IncPozo1;

                    if (diferentes.Any(x => x.Campo.Equals("IncPozo1") && x.Pozo == pozo.TipoPozo))
                    {
                        worksheet.Cells[fromRowInd + 4, initPozo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[fromRowInd + 4, initPozo].Style.Fill.BackgroundColor.SetColor(backgroundColorOld);
                    }

                    initPozo++;
                }

                initPozo = fromColPozo;

                foreach (AlertaProgresivoPozoEntidad pozo in detalleAnterior.Pozos)
                {
                    worksheet.Cells[fromRowInd + 5, initPozo].Value = pozo.RsApuesta;

                    if (diferentes.Any(x => x.Campo.Equals("RsApuesta") && x.Pozo == pozo.TipoPozo))
                    {
                        worksheet.Cells[fromRowInd + 5, initPozo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[fromRowInd + 5, initPozo].Style.Fill.BackgroundColor.SetColor(backgroundColorOld);
                    }

                    initPozo++;
                }

                initPozo = fromColPozo;

                foreach (AlertaProgresivoPozoEntidad pozo in detalleAnterior.Pozos)
                {
                    worksheet.Cells[fromRowInd + 6, initPozo].Value = pozo.RsJugadores;

                    if (diferentes.Any(x => x.Campo.Equals("RsJugadores") && x.Pozo == pozo.TipoPozo))
                    {
                        worksheet.Cells[fromRowInd + 6, initPozo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[fromRowInd + 6, initPozo].Style.Fill.BackgroundColor.SetColor(backgroundColorOld);
                    }

                    initPozo++;
                }

                initPozo = fromColPozo;

                foreach (AlertaProgresivoPozoEntidad pozo in detalleAnterior.Pozos)
                {
                    worksheet.Cells[fromRowInd + 7, initPozo].Value = pozo.MontoOcMin;

                    if (diferentes.Any(x => x.Campo.Equals("MontoOcMin") && x.Pozo == pozo.TipoPozo))
                    {
                        worksheet.Cells[fromRowInd + 7, initPozo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[fromRowInd + 7, initPozo].Style.Fill.BackgroundColor.SetColor(backgroundColorOld);
                    }

                    initPozo++;
                }

                initPozo = fromColPozo;

                foreach (AlertaProgresivoPozoEntidad pozo in detalleAnterior.Pozos)
                {
                    worksheet.Cells[fromRowInd + 8, initPozo].Value = pozo.MontoOcMax;

                    if (diferentes.Any(x => x.Campo.Equals("MontoOcMax") && x.Pozo == pozo.TipoPozo))
                    {
                        worksheet.Cells[fromRowInd + 8, initPozo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[fromRowInd + 8, initPozo].Style.Fill.BackgroundColor.SetColor(backgroundColorOld);
                    }

                    initPozo++;
                }

                initPozo = fromColPozo;

                foreach (AlertaProgresivoPozoEntidad pozo in detalleAnterior.Pozos)
                {
                    worksheet.Cells[fromRowInd + 9, initPozo].Value = pozo.IncOcPozo1;

                    if (diferentes.Any(x => x.Campo.Equals("IncOcPozo1") && x.Pozo == pozo.TipoPozo))
                    {
                        worksheet.Cells[fromRowInd + 9, initPozo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[fromRowInd + 9, initPozo].Style.Fill.BackgroundColor.SetColor(backgroundColorOld);
                    }

                    initPozo++;
                }

                initPozo = fromColPozo;

                foreach (AlertaProgresivoPozoEntidad pozo in detalleAnterior.Pozos)
                {
                    worksheet.Cells[fromRowInd + 10, initPozo].Value = pozo.Dificultad_desc;

                    if (diferentes.Any(x => x.Campo.Equals("Dificultad_desc") && x.Pozo == pozo.TipoPozo))
                    {
                        worksheet.Cells[fromRowInd + 10, initPozo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[fromRowInd + 10, initPozo].Style.Fill.BackgroundColor.SetColor(backgroundColorOld);
                    }

                    initPozo++;
                }

                initPozo = fromColPozo;

                foreach (AlertaProgresivoPozoEntidad pozo in detalleAnterior.Pozos)
                {
                    worksheet.Cells[fromRowInd + 11, initPozo].Value = pozo.Actual;

                    if (diferentes.Any(x => x.Campo.Equals("Actual") && x.Pozo == pozo.TipoPozo))
                    {
                        worksheet.Cells[fromRowInd + 11, initPozo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[fromRowInd + 11, initPozo].Style.Fill.BackgroundColor.SetColor(backgroundColorOld);
                    }

                    initPozo++;
                }

                int toRow = fromRow + 17;

                // Header style
                worksheet.Cells[fromRow, fromColTwo, fromRow, toColTwo].Style.Font.Bold = true;
                worksheet.Cells[fromRow, fromColTwo, fromRow, toColTwo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[fromRow, fromColTwo, fromRow, toColTwo].Style.Fill.BackgroundColor.SetColor(backgroundColorRed);
                worksheet.Cells[fromRow, fromColTwo, fromRow, toColTwo].Style.Font.Color.SetColor(Color.White);

                // Head style
                worksheet.Cells[fromRowInd, fromColTwo, fromRowInd, toColTwo].Style.Font.Bold = true;
                worksheet.Cells[fromRowInd, fromColTwo, fromRowInd, toColTwo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[fromRowInd, fromColTwo, fromRowInd, toColTwo].Style.Fill.BackgroundColor.SetColor(backgroundColorRed);
                worksheet.Cells[fromRowInd, fromColTwo, fromRowInd, toColTwo].Style.Font.Color.SetColor(Color.White);

                // Borders Table
                worksheet.Cells[fromRow, fromColTwo, toRow, toColTwo].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[fromRow, fromColTwo, toRow, toColTwo].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[fromRow, fromColTwo, toRow, toColTwo].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[fromRow, fromColTwo, toRow, toColTwo].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[fromRow, fromColTwo, toRow, toColTwo].Style.Border.Top.Color.SetColor(borderColor);
                worksheet.Cells[fromRow, fromColTwo, toRow, toColTwo].Style.Border.Left.Color.SetColor(borderColor);
                worksheet.Cells[fromRow, fromColTwo, toRow, toColTwo].Style.Border.Right.Color.SetColor(borderColor);
                worksheet.Cells[fromRow, fromColTwo, toRow, toColTwo].Style.Border.Bottom.Color.SetColor(borderColor);

                // Alignment
                worksheet.Cells[fromRow, fromColTwo, toRow, toColTwo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[fromRow, fromColTwo, toRow, toColTwo].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[fromRow + 1, fromColTwo, toRow, fromColTwo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells[toRow, fromColTwo, toRow, toColTwo].Style.Font.Bold = true;
            }

            // Columns Width
            worksheet.Column(2).AutoFit();
            worksheet.Column(3).AutoFit();
            worksheet.Column(4).AutoFit();
            worksheet.Column(5).AutoFit();
            worksheet.Column(6).Width = 2.0;
            worksheet.Column(7).AutoFit();
            worksheet.Column(8).AutoFit();
            worksheet.Column(9).AutoFit();
            worksheet.Column(10).AutoFit();

            return worksheet;
        }

        #endregion
    }
}