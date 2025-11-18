using CapaEntidad.Campañas;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace CapaPresentacion.Reports
{
    public class CampaniaReport
    {
        public static MemoryStream ExcelTicketsPromocionales(List<CMP_TicketReporteEntidad> listaTickets, dynamic parameters)
        {
            MemoryStream memoryStream = new MemoryStream();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet excelTickets = excelPackage.Workbook.Worksheets.Add("Tickets Promocionales");

                WorksheetTicketsPromocionales(excelTickets, listaTickets, parameters);

                excelPackage.SaveAs(memoryStream);
            }

            return memoryStream;
        }

        private static ExcelWorksheet WorksheetTicketsPromocionales(ExcelWorksheet worksheet, List<CMP_TicketReporteEntidad> items, dynamic parameters)
        {
            // parameters
            DateTime fromDate = Convert.ToDateTime(parameters.fechaInicio);
            DateTime endDate = Convert.ToDateTime(parameters.fechaFin);

            // table
            int itemsCount = items.Count;
            int fromRow = 5;
            int toRow = fromRow + itemsCount;
            int fromCol = 2;
            int toCol = fromCol + 10;
            double rowHeight = 20;

            // colors
            Color backgroundColor = ColorTranslator.FromHtml("#002060");
            Color backgroundColorOdd = ColorTranslator.FromHtml("#EEEEEE");
            Color borderColor = ColorTranslator.FromHtml("#484848");

            // header
            worksheet.Row(2).Height = rowHeight + 5;
            worksheet.Cells[2, 2, 2, 12].Merge = true;
            worksheet.Cells[3, 2, 3, 12].Merge = true;
            worksheet.Cells[2, 2, 3, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[2, 2, 3, 12].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[2, 2].Style.Font.Size = 18;
            worksheet.Cells[3, 2].Style.Font.Size = 12;
            worksheet.Cells[2, 2].Style.Font.Bold = true;
            worksheet.Cells[3, 2].Style.Font.Italic = true;
            worksheet.Cells[2, 2].Value = "Reporte Tickets Promocionales";
            worksheet.Cells[3, 2].Value = $"{fromDate.ToString("dd/MM/yyyy")} al {endDate.ToString("dd/MM/yyyy")}";

            // head
            worksheet.Cells[fromRow, 2].Value = "Id";
            worksheet.Cells[fromRow, 3].Value = "Item";
            worksheet.Cells[fromRow, 4].Value = "Fecha";
            worksheet.Cells[fromRow, 5].Value = "Hora";
            worksheet.Cells[fromRow, 6].Value = "NroTicket";
            worksheet.Cells[fromRow, 7].Value = "Monto";
            worksheet.Cells[fromRow, 8].Value = "Cliente";
            worksheet.Cells[fromRow, 9].Value = "NroDoc";
            worksheet.Cells[fromRow, 10].Value = "Campaña";
            worksheet.Cells[fromRow, 11].Value = "Sala";
            worksheet.Cells[fromRow, 12].Value = "Usuario";

            // head style
            worksheet.Row(fromRow).Height = rowHeight + 5;
            worksheet.Cells[fromRow, fromCol, fromRow, toCol].Style.Font.Bold = true;
            worksheet.Cells[fromRow, fromCol, fromRow, toCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[fromRow, fromCol, fromRow, toCol].Style.Fill.BackgroundColor.SetColor(backgroundColor);
            worksheet.Cells[fromRow, fromCol, fromRow, toCol].Style.Font.Color.SetColor(Color.White);

            // body
            int recordRow = fromRow + 1;

            foreach (CMP_TicketReporteEntidad item in items)
            {
                worksheet.Row(recordRow).Height = rowHeight;

                worksheet.Cells[recordRow, 2].Value = item.Id;
                worksheet.Cells[recordRow, 3].Value = item.Item;
                worksheet.Cells[recordRow, 4].Value = item.FechaInicio.ToString("dd/MM/yyyy");
                worksheet.Cells[recordRow, 5].Value = item.FechaInicio.ToString("HH:mm:ss");
                worksheet.Cells[recordRow, 6].Value = item.NroTicket;
                worksheet.Cells[recordRow, 7].Value = item.Monto;
                worksheet.Cells[recordRow, 8].Value = item.ClienteNombres;
                worksheet.Cells[recordRow, 9].Value = item.ClienteDOI;
                worksheet.Cells[recordRow, 10].Value = item.CampaniaNombre;
                worksheet.Cells[recordRow, 11].Value = item.SalaNombre;
                worksheet.Cells[recordRow, 12].Value = item.UsuarioNombre;

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
            worksheet.Cells[fromRow, 8, toRow, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[fromRow, 10, toRow, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[fromRow, 11, toRow, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

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
            worksheet.Column(10).AutoFit();
            worksheet.Column(11).AutoFit();
            worksheet.Column(12).AutoFit();

            return worksheet;
        }
    }
}