using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Data;
using System.IO;

namespace S3k.Utilitario.Excel {
    public static class ExcelHelper {
        public static byte[] GenerateExcel(ExportExcel exportExcel) {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();

            //La hoja de Excel
            var workSheet = excel.Workbook.Worksheets.Add(exportExcel.SheetName);
            workSheet.TabColor = System.Drawing.Color.Blue;

            //Center Column Number
            if(exportExcel.FirstColumNumber) {
                workSheet.Column(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            //El titulo
            int rowTitle = 2;
            int columTitle = 2;
            ExcelRange rangeTitle = workSheet.Cells[rowTitle, columTitle, rowTitle, exportExcel.Data.Columns.Count + columTitle - 1];
            rangeTitle.Merge = true;
            rangeTitle.Value = exportExcel.Title;
            rangeTitle.Style.Font.Bold = true;
            rangeTitle.Style.Font.Size = 20;
            rangeTitle.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            rangeTitle.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            rangeTitle.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(68, 84, 106));
            rangeTitle.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            rangeTitle.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.FromArgb(68, 114, 196));
            workSheet.Row(rowTitle + 1).Height = 11;

            //Tabla
            int tableRow = rowTitle + 2; // Dos filas después del título
            int tableColumn = columTitle;
            var startCell = workSheet.Cells[tableRow, tableColumn].LoadFromDataTable(exportExcel.Data, true);

            int auxTableRow = tableColumn;
            // Iterar a través de las columnas y establecer el formato de celda
            foreach(DataColumn column in exportExcel.Data.Columns) {
                ExcelRange columnCells = workSheet.Cells[tableRow + 1, auxTableRow, startCell.End.Row, auxTableRow];
                var type = column.DataType;
                if(type == typeof(DateTime)) {
                    columnCells.Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";
                } else if(column.DataType == typeof(double) || column.DataType == typeof(decimal)) {
                    columnCells.Style.Numberformat.Format = "#,##0";
                } else if(type == typeof(int)) {
                    columnCells.Style.Numberformat.Format = "0";
                } else if(type == typeof(string)) {
                    columnCells.Style.Numberformat.Format = "@";
                }
                auxTableRow++;
            }

            var tableRange = workSheet.Cells[tableRow, tableColumn, startCell.End.Row, startCell.End.Column];
            var table = workSheet.Tables.Add(tableRange, "Table_1");
            table.TableStyle = TableStyles.Light9;
            table.ShowHeader = true;
            table.ShowFilter = true;
            table.ShowFirstColumn = true;
            tableRange.AutoFitColumns();

            // Fecha de generación
            int rowDate = startCell.End.Row + 2;
            int colDate = 2;
            ExcelRange dateRange = workSheet.Cells[rowDate, colDate, rowDate, colDate + 3];
            dateRange.Merge = true;
            dateRange.Value = "Fecha de Generación: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            dateRange.Style.Font.Color.SetColor(System.Drawing.Color.Gray);
            dateRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            // Cantidad total de registros
            int rowTotalRecords = rowDate + 1;
            int colTotalRecords = 2;
            ExcelRange totalRecordsRange = workSheet.Cells[rowTotalRecords, colTotalRecords, rowTotalRecords, colTotalRecords + 3];
            totalRecordsRange.Merge = true;
            totalRecordsRange.Value = "Cantidad Total de Registros: " + exportExcel.Data.Rows.Count;
            totalRecordsRange.Style.Font.Color.SetColor(System.Drawing.Color.Gray);
            totalRecordsRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            using(MemoryStream memoryStream = new MemoryStream()) {
                excel.SaveAs(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
