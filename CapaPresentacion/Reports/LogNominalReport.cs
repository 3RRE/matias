using CapaEntidad.Reportes;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace CapaPresentacion.Reports
{
    public class LogNominalReport
    {
        public readonly static string defaultTypeLogWs = "RNL";

        public readonly static Dictionary<int, string> typeLogsWs = new Dictionary<int, string>
        {
            { 1, "RNLSO" },
            { 2, "RNLEV" },
            { 3, "RNLAB" }
        };

        public readonly static Dictionary<int, string> typeLogs = new Dictionary<int, string>
        {
            { 1, "Servicio Online" },
            { 2, "Eventos" },
            { 3, "Alerta Billeteros" }
        };

        public static MemoryStream ExcelLogNominal(List<string> dates, List<ALEV_ReporteNominalEntidad> reporteNominal, dynamic parameters)
        {
            MemoryStream memoryStream = new MemoryStream();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            int type = Convert.ToInt32(parameters.type);
            string typeLogWs = typeLogsWs.ContainsKey(type) ? typeLogsWs[type] : defaultTypeLogWs;

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet excelNominal = excelPackage.Workbook.Worksheets.Add(typeLogWs);
                WorksheetLogNominal(excelNominal, dates, reporteNominal, parameters);

                excelPackage.SaveAs(memoryStream);
            }

            return memoryStream;
        }

        public static MemoryStream ExcelLogNominalMultiple(List<string> dates, List<ALEV_TipoReporteNominalEntidad> tipoReporteNominal, dynamic parameters)
        {
            MemoryStream memoryStream = new MemoryStream();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                foreach(ALEV_TipoReporteNominalEntidad reporte in tipoReporteNominal)
                {
                    string typeLogWs = typeLogsWs.ContainsKey(reporte.Tipo) ? typeLogsWs[reporte.Tipo] : defaultTypeLogWs;

                    dynamic arguments = new
                    {
                        fromDate = Convert.ToDateTime(parameters.fromDate),
                        toDate = Convert.ToDateTime(parameters.toDate),
                        type = reporte.Tipo
                    };

                    ExcelWorksheet excelNominal = excelPackage.Workbook.Worksheets.Add(typeLogWs);
                    WorksheetLogNominal(excelNominal, dates, reporte.Salas, arguments);
                }

                excelPackage.SaveAs(memoryStream);
            }

            return memoryStream;
        }

        private static ExcelWorksheet WorksheetLogNominal(ExcelWorksheet worksheet, List<string> dates, List<ALEV_ReporteNominalEntidad> reporteNominal, dynamic parameters)
        {
            // parameters
            DateTime fromDate = Convert.ToDateTime(parameters.fromDate);
            DateTime toDate = Convert.ToDateTime(parameters.toDate);
            int type = Convert.ToInt32(parameters.type);

            string typeLog = typeLogs.ContainsKey(type) ? typeLogs[type] : "";

            // table
            int roomsCount = reporteNominal.Count;
            int datesCount = dates.Count;
            int fromRow = 5;
            int toRow = fromRow + roomsCount;
            int fromCol = 2;
            int toCol = fromCol + datesCount;
            double rowHeight = 20;

            // colors
            Color backgroundColor = ColorTranslator.FromHtml("#002060");
            Color backgroundColorOn = ColorTranslator.FromHtml("#C9F7C6");
            Color backgroundColorOff = ColorTranslator.FromHtml("#F4AFAA");
            Color backgroundColorOdd = ColorTranslator.FromHtml("#EEEEEE");
            Color borderColor = ColorTranslator.FromHtml("#484848");

            // header
            worksheet.Row(2).Height = rowHeight + 5;
            worksheet.Cells[2, 2, 2, 5].Merge = true;
            worksheet.Cells[3, 2, 3, 5].Merge = true;
            worksheet.Cells[2, 2, 3, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[2, 2, 3, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[2, 2].Style.Font.Size = 12;
            worksheet.Cells[2, 2].Style.Font.Bold = true;
            worksheet.Cells[3, 2].Style.Font.Italic = true;
            worksheet.Cells[2, 2].Value = $"Reporte Nominal Logs {typeLog}";
            worksheet.Cells[3, 2].Value = $"{fromDate.ToString("dd/MM/yyyy")} al {toDate.ToString("dd/MM/yyyy")}";

            // head style
            worksheet.Row(fromRow).Height = rowHeight + 5;
            worksheet.Cells[fromRow, fromCol, fromRow, toCol].Style.Font.Bold = true;
            worksheet.Cells[fromRow, fromCol, fromRow, toCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[fromRow, fromCol, fromRow, toCol].Style.Fill.BackgroundColor.SetColor(backgroundColor);
            worksheet.Cells[fromRow, fromCol, fromRow, toCol].Style.Font.Color.SetColor(Color.White);

            // body
            worksheet.Cells[fromRow, fromCol].Value = "SALAS";

            int dateCol = fromCol + 1;

            foreach(string date in dates)
            {
                worksheet.Cells[fromRow, dateCol].Value = date;

                worksheet.Column(dateCol).AutoFit();

                dateCol++;
            }

            int recordRow = fromRow + 1;

            foreach (ALEV_ReporteNominalEntidad nominal in reporteNominal)
            {
                worksheet.Row(recordRow).Height = rowHeight;

                worksheet.Cells[recordRow, fromCol].Value = nominal.Sala;

                if (recordRow % 2 == 0)
                {
                    worksheet.Cells[recordRow, fromCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[recordRow, fromCol].Style.Fill.BackgroundColor.SetColor(backgroundColorOdd);
                }

                int logCol = fromCol + 1;

                foreach (ALEV_LogNominalEntidad log in nominal.Logs)
                {
                    worksheet.Cells[recordRow, logCol].Value = log.Total;

                    worksheet.Cells[recordRow, logCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[recordRow, logCol].Style.Fill.BackgroundColor.SetColor(backgroundColorOn);

                    if (log.Total == 0)
                    {
                        
                        worksheet.Cells[recordRow, logCol].Style.Fill.BackgroundColor.SetColor(backgroundColorOff);
                    }

                    logCol++;
                }

                recordRow++;
            }

            // borders
            worksheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Top.Color.SetColor(borderColor);
            worksheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Left.Color.SetColor(borderColor);
            worksheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Right.Color.SetColor(borderColor);
            worksheet.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Bottom.Color.SetColor(borderColor);

            // alignment
            worksheet.Cells[fromRow, fromCol, toRow, toCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[fromRow, fromCol, toRow, toCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[fromRow, fromCol, toRow, fromCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[fromRow, fromCol, fromRow, toCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            // columns width
            worksheet.Column(1).Width = 3;
            worksheet.Column(fromCol).AutoFit();

            return worksheet;
        }
    }
}