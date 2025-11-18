using CapaEntidad.Reportes.AperturaCaja;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace CapaPresentacion.Reports
{
    public class AperturaCajaReport
    {
        public readonly static Dictionary<int, string> typeBoxes = new Dictionary<int, string>
        {
            { 5, "Fichas" },
            { 12, "ATM" }
        };

        public static MemoryStream ExcelAperturaCajas(List<AperturarCajaEntidad> dataReporteAperturaCaja)
        {
            MemoryStream memoryStream = new MemoryStream();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet excelNominal = excelPackage.Workbook.Worksheets.Add("Reporte Apertura Cajas");
                WorksheetAperturaCajas(excelNominal, dataReporteAperturaCaja);

                excelPackage.SaveAs(memoryStream);
            }

            return memoryStream;
        }

        private static ExcelWorksheet WorksheetAperturaCajas(ExcelWorksheet worksheet, List<AperturarCajaEntidad> data)
        {
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
            int toCol = fromCol + 7;
            double rowHeight = 20;

            // colors
            Color backgroundColor = ColorTranslator.FromHtml("#002060");
            Color backgroundColorOdd = ColorTranslator.FromHtml("#EEEEEE");
            Color borderColor = ColorTranslator.FromHtml("#484848");

            // header
            worksheet.Row(2).Height = rowHeight + 5;
            worksheet.Cells[2, 2, 2, 9].Merge = true;
            worksheet.Cells[3, 2, 3, 9].Merge = true;
            worksheet.Cells[2, 2, 3, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[2, 2, 3, 9].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[2, 2].Style.Font.Size = 18;
            worksheet.Cells[3, 2].Style.Font.Size = 12;
            worksheet.Cells[2, 2].Style.Font.Bold = true;
            worksheet.Cells[3, 2].Style.Font.Italic = true;
            worksheet.Cells[2, 2].Value = $"Reporte Apertura Caja";
            worksheet.Cells[3, 2].Value = currentDate.ToString("dd/MM/yyyy HH:mm:ss");

            // head
            worksheet.Cells[fromRow, 2].Value = "Código Caja";
            worksheet.Cells[fromRow, 3].Value = "Empresa";
            worksheet.Cells[fromRow, 4].Value = "Sala";
            worksheet.Cells[fromRow, 5].Value = "Fecha Apertura";
            worksheet.Cells[fromRow, 6].Value = "Fecha Cierre";
            worksheet.Cells[fromRow, 7].Value = "Item";
            worksheet.Cells[fromRow, 8].Value = "Turno";
            worksheet.Cells[fromRow, 9].Value = "Tipo Caja";

            // head style
            worksheet.Row(fromRow).Height = rowHeight + 5;
            worksheet.Cells[fromRow, fromCol, fromRow, toCol].Style.Font.Bold = true;
            worksheet.Cells[fromRow, fromCol, fromRow, toCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[fromRow, fromCol, fromRow, toCol].Style.Fill.BackgroundColor.SetColor(backgroundColor);
            worksheet.Cells[fromRow, fromCol, fromRow, toCol].Style.Font.Color.SetColor(Color.White);

            // body
            int recordRow = fromRow + 1;

            foreach (AperturarCajaEntidad item in data)
            {
                worksheet.Row(recordRow).Height = rowHeight;

                worksheet.Cells[recordRow, 2].Value = item.CodCaja;
                worksheet.Cells[recordRow, 3].Value = item.NombreEmpresa;
                worksheet.Cells[recordRow, 4].Value = item.NombreSala;

                if (nullDates.Contains(item.FechaApertura.ToString("dd-MM-yyyy")))
                {
                    worksheet.Cells[recordRow, 5].Value = "";
                }
                else
                {
                    worksheet.Cells[recordRow, 5].Value = item.FechaApertura.ToString("dd-MM-yyyy hh:mm:ss tt");
                }

                if (nullDates.Contains(item.FechaCierre.ToString("dd-MM-yyyy")))
                {
                    worksheet.Cells[recordRow, 6].Value = "";
                }
                else
                {
                    worksheet.Cells[recordRow, 6].Value = item.FechaCierre.ToString("dd-MM-yyyy hh:mm:ss tt");
                }

                worksheet.Cells[recordRow, 7].Value = item.Item;
                worksheet.Cells[recordRow, 8].Value = item.Turno;
                worksheet.Cells[recordRow, 9].Value = typeBoxes.ContainsKey(item.TipoCaja) ? typeBoxes[item.TipoCaja] : "Otros";

                if (recordRow % 2 == 0)
                {
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
            worksheet.Column(8).AutoFit();
            worksheet.Column(9).AutoFit();

            return worksheet;
        }
    }
}