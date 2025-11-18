using CapaEntidad.Reportes.AperturaCaja;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using CapaEntidad;

namespace CapaPresentacion.Reports {
    public class ProgresivoReporte {

        public static MemoryStream ExcelAperturaCajas(List<CabeceraOfflineEntidad> dataReporteAperturaCaja, DateTime fechaIni,DateTime FechaFin, string nombreSala, string nombreProgresivo) {
            MemoryStream memoryStream = new MemoryStream();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using(ExcelPackage excelPackage = new ExcelPackage()) {
                ExcelWorksheet excelNominal = excelPackage.Workbook.Worksheets.Add("Reporte Progresivo");
                WorksheetAperturaCajas(excelNominal, dataReporteAperturaCaja, fechaIni, FechaFin,  nombreSala,  nombreProgresivo);

                excelPackage.SaveAs(memoryStream);
            }

            return memoryStream;
        }

        private static ExcelWorksheet WorksheetAperturaCajas(ExcelWorksheet worksheet, List<CabeceraOfflineEntidad> data, DateTime fechaIni, DateTime FechaFin, string nombreSala, string nombreProgresivo) {
            // parameters
            DateTime currentDate = DateTime.Now;
            List<string> nullDates = new List<string>
            {
                "31-12-1752",
                "01-01-1753"
            };

            // table
            int itemsCount = data.Count;
            int fromRow = 5;
            int toRow = fromRow + itemsCount;
            int fromCol = 2;
            int toCol = fromCol + 5;
            double rowHeight = 20;

            // colors
            Color backgroundColor = ColorTranslator.FromHtml("#002060");
            Color backgroundColorOdd = ColorTranslator.FromHtml("#EEEEEE");
            Color borderColor = ColorTranslator.FromHtml("#484848");

            // header
            worksheet.Row(2).Height = rowHeight + 4;
            worksheet.Cells[2, 2, 2, 7].Merge = true;
            worksheet.Cells[3, 2, 3, 7].Merge = true;
            worksheet.Cells[2, 2, 3, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[2, 2, 3, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[2, 2].Style.Font.Size = 18;
            worksheet.Cells[3, 2].Style.Font.Size = 12;
            worksheet.Cells[2, 2].Style.Font.Bold = true;
            worksheet.Cells[3, 2].Style.Font.Italic = true;
            worksheet.Cells[2, 2].Value = $"Reporte Progresivo "+ nombreProgresivo + ", sala "+nombreSala + " )" ;
            worksheet.Cells[3, 2].Value = "Del " + fechaIni.ToString("dd-MM-yyyy ") +"al "+FechaFin.ToString("dd-MM-yyyy ");

            // head
            worksheet.Cells[fromRow, 2].Value = "Pozo";
            worksheet.Cells[fromRow, 3].Value = "Máquina";
            worksheet.Cells[fromRow, 4].Value = "Fecha";
            worksheet.Cells[fromRow, 5].Value = "Hora";
            worksheet.Cells[fromRow, 6].Value = "Monto";
            worksheet.Cells[fromRow, 7].Value = "Evento";

            // head style
            worksheet.Row(fromRow).Height = rowHeight + 4;
            worksheet.Cells[fromRow, fromCol, fromRow, toCol].Style.Font.Bold = true;
            worksheet.Cells[fromRow, fromCol, fromRow, toCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[fromRow, fromCol, fromRow, toCol].Style.Fill.BackgroundColor.SetColor(backgroundColor);
            worksheet.Cells[fromRow, fromCol, fromRow, toCol].Style.Font.Color.SetColor(Color.White);

            // body
            int recordRow = fromRow + 1;

            foreach(CabeceraOfflineEntidad item in data) {
                worksheet.Row(recordRow).Height = rowHeight;

                if(item.TipoPozo == 1) {
                    worksheet.Cells[recordRow, 2].Value = "Superior";
                }
                else if(item.TipoPozo == 2) {
                    worksheet.Cells[recordRow, 2].Value = "Medio";

                } else if(item.TipoPozo == 3) {
                    worksheet.Cells[recordRow, 2].Value = "Inferior";

                }

                worksheet.Cells[recordRow, 3].Value = item.SlotID;

                if(nullDates.Contains(item.Fecha.ToString("dd-MM-yyyy"))) {
                    worksheet.Cells[recordRow, 4].Value = "";
                } else {
                    worksheet.Cells[recordRow, 4].Value = item.Fecha.ToString("dd-MM-yyyy ");
                }

                if(nullDates.Contains(item.Fecha.ToString("dd-MM-yyyy"))) {
                    worksheet.Cells[recordRow, 5].Value = "";
                } else {
                    worksheet.Cells[recordRow, 5].Value = item.Fecha.ToString("hh:mm:ss tt");
                }

                worksheet.Cells[recordRow, 6].Value = item.Monto;
                worksheet.Cells[recordRow, 7].Value = item.IdCabeceraProgresivo;

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
            worksheet.Row(recordRow).Height = rowHeight + 5;
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

           

            return worksheet;
        }
    }

}