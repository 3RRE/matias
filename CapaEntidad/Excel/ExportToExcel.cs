using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;

namespace CapaEntidad.Excel {
    public class ExportToExcel {
        public int CodSala { get; set; }

        public string Title { get; set; }

        public string FileName { get; set; }

        public string JsonContent { get; set; }

        public string IgnoredColumns { get; set; }

        public static byte[] ExportExcel(DataTable data, string heading = "", string sheetName = "Hoja 1") {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using(var package = new ExcelPackage()) {
                ExcelWorksheet worksheet = CreateSheet(package, sheetName);
                int startFromRow = string.IsNullOrEmpty(heading) ? 2 : 5;
                worksheet.Cells["A" + startFromRow].LoadFromDataTable(data, true, TableStyles.Light11);

                int colIndex = 1;
                foreach(DataColumn col in data.Columns) {
                    ExcelRange cells = worksheet.Cells[worksheet.Dimension.Start.Row + 1, colIndex, worksheet.Dimension.End.Row, colIndex];
                    if(col.DataType == typeof(DateTime) || col.DataType == typeof(DateTime?)) {
                        cells.Style.Numberformat.Format = "dd/MM/yyyy HH:mm:ss";
                    }
                    worksheet.Column(colIndex).AutoFit();
                    colIndex++;
                }

                if(!string.IsNullOrEmpty(heading)) {
                    worksheet.InsertColumn(1, 1);
                    worksheet.Column(1).Width = 5;
                    worksheet.Cells["B1"].Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    worksheet.Cells["B3"].Value = heading.ToUpper();
                    worksheet.Cells["B3"].Style.Font.Bold = true;
                    worksheet.Cells["B3"].Style.Font.Size = 12;
                    worksheet.Cells["B3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[3, 2, 3, data.Columns.Count + 1].Merge = true;
                    worksheet.Column(1).AutoFit();
                }

                using(MemoryStream memoryStream = new MemoryStream()) {
                    package.SaveAs(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

        public static byte[] ExportExcel(IList data, Type type, string title, string sheetName = "Hoja 1", params string[] ignoredColumns) {
            return ExportExcel(ConvertToDataTable(data, type, ignoredColumns), title, sheetName);
        }

        private static ExcelWorksheet CreateSheet(ExcelPackage p, string sheetName) {
            //p.Workbook.Worksheets.Add(sheetName);
            //ExcelWorksheet ws = p.Workbook.Worksheets[1];
            ExcelWorksheet ws = p.Workbook.Worksheets.Add(sheetName);
            ws.Name = sheetName; //Setting Sheet's name
            ws.Cells.Style.Font.Size = 9; //Default font size for whole sheet
            //ws.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet
            //ws.Cells.Style.Font.Name = "Courier New";
            ws.Cells.Style.Font.Name = "Arial";
            return ws;
        }

        public static DataTable ConvertToDataTable(IList data, Type type, params string[] ignoredColumns) {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(type);
            DataTable table = new DataTable();
            for(int i = 0; i < props.Count; i++) {
                PropertyDescriptor prop = props[i];
                if(!ignoredColumns.Contains(props[i].Name)) {
                    table.Columns.Add(prop.Name, prop.PropertyType);
                }
            }

            object[] values = new object[table.Columns.Count];
            foreach(var item in data) {
                int j = 0;
                for(int i = 0; i < props.Count; i++) {
                    // removed ignored columns
                    if(!ignoredColumns.Contains(props[i].Name)) {
                        var value = props[i].GetValue(item);
                        switch(props[i].Name.ToLower()) {
                            case "activo":
                                values[j] = Convert.ToBoolean(value) ? "ACTIVO" : "NO ACTIVO";
                                break;
                            case "transferenciaelectronica":
                                values[j] = bool.TryParse(value?.ToString(), out bool result) ? (result ? "ACTIVO" : "NO ACTIVO") : value;
                                break;
                            default:
                                values[j] = value;
                                break;
                        }
                        j++;
                    }
                }

                table.Rows.Add(values);
            }
            return table;
        }
    }
}
