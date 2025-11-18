using CapaEntidad.ProgresivoRuleta.Dto;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace CapaPresentacion.Reports {
    public class PRU_GanadorReport {
        public static MemoryStream ExcelGanadores(List<PRU_GanadorDto> items) {
            MemoryStream memoryStream = new MemoryStream();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using(ExcelPackage excelPackage = new ExcelPackage()) {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Ganadores");

                WorksheetGanadores(worksheet, items);

                excelPackage.SaveAs(memoryStream);
            }

            return memoryStream;
        }

        private static ExcelWorksheet WorksheetGanadores(ExcelWorksheet worksheet, List<PRU_GanadorDto> items) {
            // parameters
            DateTime currentDate = DateTime.Now;

            List<string> nullDates = new List<string>
            {
                "31-12-1752",
                "01-01-1753"
            };

            // table
            int itemsCount = items.Count;
            int fromRow = 5;
            int toRow = fromRow + itemsCount;
            int fromCol = 2;
            int toCol = fromCol + 6;
            double rowHeight = 20;

            // colors
            Color backgroundColor = ColorTranslator.FromHtml("#002060");
            Color backgroundColorOdd = ColorTranslator.FromHtml("#EEEEEE");
            Color borderColor = ColorTranslator.FromHtml("#484848");

            // header
            worksheet.Row(2).Height = rowHeight + 5;
            worksheet.Cells[2, 2, 2, 7].Merge = true;
            worksheet.Cells[3, 2, 3, 7].Merge = true;
            worksheet.Cells[2, 2, 3, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[2, 2, 3, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[2, 2].Style.Font.Size = 18;
            worksheet.Cells[3, 2].Style.Font.Size = 12;
            worksheet.Cells[2, 2].Style.Font.Bold = true;
            worksheet.Cells[3, 2].Style.Font.Italic = true;
            worksheet.Cells[2, 2].Value = $"Ganadores Progresivo Ruleta";
            worksheet.Cells[3, 2].Value = currentDate.ToString("dd/MM/yyyy HH:mm:ss");

            // head
            worksheet.Cells[fromRow, 2].Value = "Id";
            worksheet.Cells[fromRow, 3].Value = "Sala";
            worksheet.Cells[fromRow, 4].Value = "Ruleta";
            worksheet.Cells[fromRow, 5].Value = "Cod. Maquina";
            worksheet.Cells[fromRow, 6].Value = "Monto";
            worksheet.Cells[fromRow, 7].Value = "Fecha y Hora";
            worksheet.Cells[fromRow, 8].Value = "Acreditado";

            // head style
            worksheet.Row(fromRow).Height = rowHeight + 6;
            worksheet.Cells[fromRow, fromCol, fromRow, toCol].Style.Font.Bold = true;
            worksheet.Cells[fromRow, fromCol, fromRow, toCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[fromRow, fromCol, fromRow, toCol].Style.Fill.BackgroundColor.SetColor(backgroundColor);
            worksheet.Cells[fromRow, fromCol, fromRow, toCol].Style.Font.Color.SetColor(Color.White);

            // body
            int recordRow = fromRow + 1;

            foreach(var item in items) {
                worksheet.Row(recordRow).Height = rowHeight;

                worksheet.Cells[recordRow, 2].Value = item.Id;
                worksheet.Cells[recordRow, 3].Value = item.Sala.Nombre;
                worksheet.Cells[recordRow, 4].Value = item.Ruleta.Nombre;
                worksheet.Cells[recordRow, 5].Value = item.CodMaquina;
                worksheet.Cells[recordRow, 6].Value = item.Monto.ToString("F2");

                if(nullDates.Contains(item.FechaGanador.ToString("dd-MM-yyyy"))) {
                    worksheet.Cells[recordRow, 7].Value = "";
                } else {
                    worksheet.Cells[recordRow, 7].Value = item.FechaGanador.ToString("dd/MM/yyyy HH:mm:ss");
                }

                worksheet.Cells[recordRow, 8].Value =
                    item.EsAcreditado == 1 ? "Ganador" :
                    item.EsAcreditado == 2 ? "Ganador 2" :
                    "";


                if(recordRow % 2 == 0) {
                    worksheet.Cells[recordRow, fromCol, recordRow, toCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[recordRow, fromCol, recordRow, toCol].Style.Fill.BackgroundColor.SetColor(backgroundColorOdd);
                }

                recordRow++;
            }

            // filters
            worksheet.Cells[fromRow, fromCol, toRow, toCol].AutoFilter = true;

            // footer
            worksheet.Cells[recordRow, fromCol, recordRow, toCol].Merge = true;
            worksheet.Cells[recordRow, fromCol].Value = $"Total : {itemsCount} Registros";

            // footer style
            worksheet.Row(recordRow).Height = rowHeight + 6;
            worksheet.Cells[recordRow, fromCol, recordRow, toCol].Style.Font.Bold = true;
            worksheet.Cells[recordRow, fromCol, recordRow, toCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[recordRow, fromCol, recordRow, toCol].Style.Fill.BackgroundColor.SetColor(backgroundColor);
            worksheet.Cells[recordRow, fromCol, recordRow, toCol].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells[recordRow, fromCol, recordRow, toCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[recordRow, fromCol, recordRow, toCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

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
            worksheet.Cells[fromRow, 3, toRow, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            // columns width
            worksheet.Column(1).Width = 3;
            worksheet.Column(2).AutoFit();
            worksheet.Column(3).AutoFit();
            worksheet.Column(4).AutoFit();
            worksheet.Column(5).AutoFit();
            worksheet.Column(6).AutoFit();
            worksheet.Column(7).AutoFit();
            worksheet.Column(8).AutoFit();

            return worksheet;
        }
    }
}