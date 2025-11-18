using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Utilitarios
{
    public class Excel
    {

        private static ExcelWorksheet CreateSheet(ExcelPackage p, string sheetName)
        {
            ExcelWorksheet ws = p.Workbook.Worksheets.Add(sheetName);
            ws.Name = sheetName; //Setting Sheet's name
            ws.Cells.Style.Font.Size = 9; //Default font size for whole sheet
            ws.Cells.Style.Font.Name = "Arial";
            return ws;
        }
        public static void GenerarBordesCelda(ExcelRange celda)
        {
            celda.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            celda.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            celda.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            celda.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            celda.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }


        public static void CeldaAlinear(ExcelRange celda,string align)
        {
            switch (align)
            {
                case "LEFT":
                    celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    break;
                case "CENTER":
                    celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    break;
                case "RIGHT":
                    celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    break;
                default:
                    celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    break;
            }
        }


        public static byte[] ExportarExcel(JObject data)
        {
            var tituloreporte = data["tituloreporte"];
            var fecha = data["fecha"];
            var tablas = data["Tablas"];
            var visibles = data["visibles"];
            var invisibles = data["invisibles"];
            var tabladefinicioncolumnas = data["tabladefinicioncolumnas"];


            if (data["totalMonto"] == null)
            {
                data["totalMonto"] = "";
            }
            foreach (var tabla in tablas)
            {


            }
            var datos = data["Tablas"][0];


            var cabeceras = data["Cabecera"];


            byte[] result = null;
            var items = data["Tablas"][0];
            if (items != null && items.Any())
            {
                var table = new DataTable();
                var firstElement = (JObject)items.First();
                var properties = firstElement.Properties();

                foreach (var prop in properties)
                {
                    var a = prop.Value.Type;
                    string tipo = a.ToString();

                    if (tabladefinicioncolumnas != null && (string)tabladefinicioncolumnas != "")
                    {
                        foreach (var tipocol in JArray.Parse(tabladefinicioncolumnas.ToString()))
                        {
                            if ((string)tipocol["nombre"] == prop.Name)
                            {
                                tipo = (string)tipocol["tipo"];
                            }
                        }
                    }
                    switch (tipo)
                    {
                        case "String":
                            if (prop.Name == "Cantidad")
                            {
                                table.Columns.Add(new DataColumn(prop.Name, typeof(Int32)));
                            }
                            else
                            {
                                table.Columns.Add(new DataColumn(prop.Name, typeof(string)));
                            }
                            break;
                        case "Date": table.Columns.Add(new DataColumn(prop.Name, typeof(DateTime))); ; break;
                        case "decimal": table.Columns.Add(new DataColumn(prop.Name, typeof(decimal))); break;
                        case "Float": table.Columns.Add(new DataColumn(prop.Name, typeof(decimal))); break;
                        case "Integer": table.Columns.Add(new DataColumn(prop.Name, typeof(Int32))); ; break;
                        default: table.Columns.Add(new DataColumn(prop.Name, typeof(string))); break;
                    }
                }
                int rownro = table.Columns.IndexOf("RowNumber");
                if (rownro != -1)
                {
                    table.Columns.Remove("RowNumber");
                    table.Columns.Remove("TotalDisplayRows");
                    table.Columns.Remove("TotalRows");
                }
                if (invisibles != null && invisibles.Count()>0)
                {
                    var invisiblesobj = JArray.Parse(invisibles.ToString());

                    var invi = new List<string>();

                    foreach (var invisislb in invisiblesobj)
                    {
                        invi.Add((table.Columns[Convert.ToInt32(invisislb)]).ToString());
                    }

                    foreach (var invisislb in invi)
                    {
                        table.Columns.Remove(invisislb.ToString());
                    }

                }

                var columns = table.Columns;
                foreach (var item in items)
                {
                    DataRow row = table.NewRow();
                    int i = 0;
                    foreach (DataColumn col in columns)
                    {

                        /*   if (col.ColumnName != "RowNumber" && col.ColumnName != "TotalDisplayRows" && col.ColumnName != "TotalRows")
                           { */
                        if (col.DataType == typeof(System.DateTime))
                        {
                            DateTime fechaa = Convert.ToDateTime("1970-01-01");
                            if ((DateTime)item[col.ToString()] < fechaa)
                            {
                                row[i] = DBNull.Value;
                            }
                            else
                            {
                                row[i] = item[col.ToString()];
                            }
                            // row[i] = (DateTime)item[col.ToString()] < fechaa ?  DBNull.Value : item[col.ToString()];
                            //row[i] = item[col.ToString()];
                            //row[i] = ((DateTime)item[col.ToString()]).ToString("d");
                        }
                        else if (col.DataType == typeof(System.String))
                        {
                            if (col.ToString() == "Cantidad")
                            {
                                // row[i] = (Int32)item[col.ToString()];
                                row[i] = Double.Parse((string)item[col.ToString()]);


                            }
                            else
                            {
                                row[i] = (string)item[col.ToString()];
                            }

                        }

                        else if (col.DataType == typeof(System.Int32))
                        {
                            row[i] = (Int32)item[col.ToString()];

                        }
                        else
                        {


                            row[i] = Double.Parse((string)item[col.ToString()]);
                        }

                        i++;

                        //}

                    }
                    table.Rows.Add(row);
                }

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = CreateSheet(package, "Hoja 1");
                    //int startFromRow = 7;

                    int startFromRow = cabeceras != null ? cabeceras.Count() + 3 : 7;

                    int comienzoy = startFromRow;

                    var cant = table.Columns.Count;
                    var celdas = worksheet.Cells[comienzoy, 1, comienzoy, cant<4?6:cant];
                    celdas.Value = tituloreporte;
                    celdas.Merge = true;
                    celdas.Style.Font.Bold = true;
                    celdas.Style.Font.Size = 16;
                    comienzoy++; comienzoy++;

                    foreach (var cab in cabeceras)
                    {
                        worksheet.Cells[comienzoy, 1].Value = (string)cab["nombre"];
                        var rangeCells = worksheet.Cells[comienzoy, 1, comienzoy, 1];
                        rangeCells.Style.Font.Bold = true;

                        worksheet.Cells[comienzoy, 2].Value = (string)cab["valor"];
                        worksheet.Cells[comienzoy, 2, comienzoy, 6].Merge = true;

                            worksheet.Cells[comienzoy, 2, comienzoy, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; // Alignment Left
                            worksheet.Cells[comienzoy, 2, comienzoy, 6].Style.Font.Size = 11;

                        comienzoy++;

                    }
                    comienzoy++;
                    startFromRow = comienzoy;


                    worksheet.Cells["A" + startFromRow].LoadFromDataTable(table, true);
                    int ccc = 1;
                    foreach (DataColumn col in table.Columns)
                    {
                        var cels = worksheet.Cells[startFromRow, ccc];
                        worksheet.Cells[startFromRow, ccc].Value = (worksheet.Cells[startFromRow, ccc].Value).ToString().Replace("_", " ").ToUpper();
                        cels.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        ccc++;

                    }

                    int filacomienzocomprimido = table.Rows.Count + startFromRow + 3;
                    //   worksheet.Cells["D" + filacomienzocomprimido.ToString()].LoadFromDataTable(tableComprimido, true);
                    // autofit width of cells with small content
                    int colIndex = 1;
                    int rowsLengthFin = table.Rows.Count + startFromRow;
                    foreach (DataColumn col in table.Columns)
                    {

                        ExcelRange cells = worksheet.Cells[worksheet.Dimension.Start.Row, colIndex, rowsLengthFin, colIndex];
                        //
                        worksheet.Cells[worksheet.Dimension.Start.Row, colIndex, rowsLengthFin, colIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        worksheet.Cells[worksheet.Dimension.Start.Row, colIndex, rowsLengthFin, colIndex].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[worksheet.Dimension.Start.Row, colIndex, rowsLengthFin, colIndex].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[worksheet.Dimension.Start.Row, colIndex, rowsLengthFin, colIndex].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[worksheet.Dimension.Start.Row, colIndex, rowsLengthFin, colIndex].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        //
                        //format cell for number
                        var column = col.ColumnName;
                        if (col.DataType == typeof(System.DateTime))
                        {
                            //cells.Style.Numberformat.Format = "yyyy-mm-dd";
                            cells.Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;

                            cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            // cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        }
                        else if (col.DataType == typeof(System.String))
                        {
                            if (col.ToString() == "Cantidad")
                            {
                                cells.Style.Numberformat.Format = "#,##0.00";
                                var cellAddress = new ExcelAddress(startFromRow + 1, colIndex + 1, rowsLengthFin + 1, colIndex + 1);//menores a 0color rojo
                                var cf = worksheet.ConditionalFormatting.AddLessThan(cellAddress);
                                cf.Formula = "0";
                                cf.Style.Font.Color.Color = Color.Red;
                                worksheet.Cells[rowsLengthFin + 1, colIndex].Formula = "SUM(" + cells[startFromRow + 1, colIndex].Address + ":" + cells[rowsLengthFin, colIndex].Address + ")";
                                worksheet.Cells[rowsLengthFin + 1, colIndex].Style.Font.Bold = true;
                                worksheet.Cells[rowsLengthFin + 1, colIndex].Style.Numberformat.Format = "#,##0.00";
                            }

                        }
                        else if (col.DataType == typeof(System.Single))
                        {
                            cells.Style.Numberformat.Format = "#,##0.00";
                            var cellAddress = new ExcelAddress(startFromRow + 1, colIndex + 1, rowsLengthFin + 1, colIndex + 1);//menores a 0color rojo
                            var cf = worksheet.ConditionalFormatting.AddLessThan(cellAddress);
                            cf.Formula = "0";
                            cf.Style.Font.Color.Color = Color.Red;
                            worksheet.Cells[rowsLengthFin + 1, colIndex].Formula = "SUM(" + cells[startFromRow + 1, colIndex].Address + ":" + cells[rowsLengthFin, colIndex].Address + ")";
                            worksheet.Cells[rowsLengthFin + 1, colIndex].Style.Font.Bold = true;
                            worksheet.Cells[rowsLengthFin + 1, colIndex].Style.Numberformat.Format = "#,##0.00";

                        }

                        else if (col.DataType == typeof(System.Decimal))
                        {
                            bool sumarcolumna = false;
                            if (tabladefinicioncolumnas != null)
                            {
                                foreach (var columnprop in JArray.Parse(tabladefinicioncolumnas.ToString()))
                                {
                                    if ((string)columnprop["nombre"] == column)
                                    {
                                        if ((string)columnprop["sumar"] == "true")
                                        {
                                            sumarcolumna = true;
                                        }

                                    }
                                };
                            }
                            cells.Style.Numberformat.Format = "#,##0.00";
                            var cellAddress = new ExcelAddress(startFromRow + 1, colIndex + 1, rowsLengthFin + 1, colIndex + 1);//menores a 0color rojo
                            var cf = worksheet.ConditionalFormatting.AddLessThan(cellAddress);
                            cf.Formula = "0";
                            cf.Style.Font.Color.Color = Color.Red;
                            if (sumarcolumna)
                            {
                                worksheet.Cells[rowsLengthFin + 1, colIndex].Formula = "SUM(" + cells[startFromRow + 1, colIndex].Address + ":" + cells[rowsLengthFin, colIndex].Address + ")";
                                worksheet.Cells[rowsLengthFin + 1, colIndex].Style.Font.Bold = true;
                                worksheet.Cells[rowsLengthFin + 1, colIndex].Style.Numberformat.Format = "#,##0.00";
                            }
                        }
                        else if (col.DataType == typeof(System.Int32))
                        {
                            bool sumarcolumna = false;
                            if (tabladefinicioncolumnas != null)
                            {
                                foreach (var columnprop in JArray.Parse(tabladefinicioncolumnas.ToString()))
                                {
                                    if ((string)columnprop["nombre"] == column)
                                    {
                                        if ((string)columnprop["sumar"] == "true")
                                        {
                                            sumarcolumna = true;
                                        }
                                    }
                                };
                            }
                            if (sumarcolumna)
                            {
                                worksheet.Cells[rowsLengthFin + 1, colIndex].Formula = "SUM(" + cells[startFromRow + 1, colIndex].Address + ":" + cells[rowsLengthFin, colIndex].Address + ")";
                                worksheet.Cells[rowsLengthFin + 1, colIndex].Style.Font.Bold = true;
                                //  worksheet.Cells[rowsLengthFin + 1, colIndex].Style.Numberformat.Format = "###0";
                            }

                        }
                        else
                        {
                            if (col.ToString() == "Cantidad")
                            {
                                cells.Style.Numberformat.Format = "#,##0";
                                var cellAddress = new ExcelAddress(startFromRow + 1, colIndex + 1, rowsLengthFin + 1, colIndex + 1);//menores a 0color rojo
                                var cf = worksheet.ConditionalFormatting.AddLessThan(cellAddress);
                                cf.Formula = "0";
                                cf.Style.Font.Color.Color = Color.Red;
                                worksheet.Cells[rowsLengthFin + 1, colIndex].Formula = "SUM(" + cells[startFromRow + 1, colIndex].Address + ":" + cells[rowsLengthFin, colIndex].Address + ")";
                                worksheet.Cells[rowsLengthFin + 1, colIndex].Style.Font.Bold = true;
                                worksheet.Cells[rowsLengthFin + 1, colIndex].Style.Numberformat.Format = "#,##0";

                            }
                            else
                            {
                                cells.Style.Numberformat.Format = "#,##0.00";
                            }
                        }

                        if (column.IndexOf("FECHA", StringComparison.CurrentCultureIgnoreCase) != -1)
                        {

                            cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        };

                        package.Workbook.Calculate();
                        worksheet.Column(colIndex).AutoFit();

                        colIndex++;

                    }

                  

                    worksheet.Column(1).AutoFit();
                    worksheet.InsertColumn(1, 1);
                    worksheet.Column(1).Width = 5;
                    //set background color white
                    worksheet.Cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells.Style.Fill.BackgroundColor.SetColor(Color.White);
                    //format header table
                    using (var range = worksheet.Cells[startFromRow, 2, startFromRow, table.Columns.Count + 1])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        range.Style.ShrinkToFit = false;
                    }

                    result = package.GetAsByteArray();
                }
            }

            return result;
        }

        public static List<columnasexcel> getcolumnasexcel(JObject primerelemento, string tabladefinicioncolumnas)
        {
            var ColumnasExcelLista = new List<columnasexcel>();
            var propiedades = primerelemento.Properties();
            foreach (var prop in propiedades)
            {
                var columna_Excel = new columnasexcel();
                var a = prop.Value.Type;
                string colnombre = prop.Name;
                string tipo = a.ToString().ToUpper();
                if (tipo == "NULL") {
                    tipo = "STRING";
                }
                string alinear = "LEFT";
                string sumar = "false";

                string formato = "";

                if (tabladefinicioncolumnas != null && (string)tabladefinicioncolumnas != "")
                {
                    foreach (var tipocol in JArray.Parse(tabladefinicioncolumnas.ToString()))
                    {
                        if (((string)tipocol["nombre"]).ToUpper() == (prop.Name).ToUpper())
                        {
                            tipo = (string)tipocol["tipo"];
                            alinear = (string)tipocol["alinear"];
                            sumar = (string)tipocol["sumar"];

                            formato = (string)tipocol["formato"];
                        }
                    }
                }
                
                if (formato == null)
                {
                    if (tipo.ToUpper() == "DECIMAL") { formato = "#,##0.00"; }
                    else if (tipo.ToUpper() == "FLOAT") { formato = "#,##0.00"; }
                    else if (tipo.ToUpper() == "INTEGER") { formato = "###0"; }
                    else { formato = ""; }
                }
                //tipo = tipo.ToUpper();
                columna_Excel.nombre = colnombre;
                columna_Excel.tipo = tipo;
                columna_Excel.alinear = alinear;
                columna_Excel.formato = formato;
                columna_Excel.sumar = sumar;
                ColumnasExcelLista.Add(columna_Excel);
            }
            return ColumnasExcelLista;
        }

        public static byte[] ExportarExcelNuevo(JObject data)
        {
            var tituloreporte = data["tituloreporte"];
            var fecha = data["fecha"];
            var tablas = data["Tablas"];
            var tabladefinicioncolumnas = data["tabladefinicioncolumnas"];
            var aoHeader = data["tablaaoHeader"];

            var datos = data["Tablas"][0];
            var cabeceras = data["Cabecera"];

            //var colorthcabecera = data["colorthcabecera"];

            byte[] result = null;
            var tablaprimera = data["Tablas"][0];
            if (tablaprimera != null && tablaprimera.Any())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = CreateSheet(package, "Hoja 1");
                    ExcelRange celda;
                    int comienzoy = 2;
                    worksheet.Cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells.Style.Fill.BackgroundColor.SetColor(Color.White);

                    celda = worksheet.Cells[comienzoy,1,comienzoy,7];
                    celda.Value = (string)tituloreporte;
                    celda.Merge = true;
                    celda.Style.Font.Bold = true;
                    celda.Style.Font.Size=16;
                    celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    comienzoy++;
                    comienzoy++;
                    foreach (var cab in cabeceras)
                    {
                        worksheet.Cells[comienzoy, 1].Value = (string)cab["nombre"];
                        var rangeCells = worksheet.Cells[comienzoy, 1, comienzoy, 1];
                        rangeCells.Style.Font.Bold = true;

                        worksheet.Cells[comienzoy, 2].Value = (string)cab["valor"];
                        worksheet.Cells[comienzoy, 2, comienzoy, 6].Merge = true;
                        worksheet.Cells[comienzoy, 2, comienzoy, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; // Alignment Left
                        worksheet.Cells[comienzoy, 2, comienzoy, 6].Style.Font.Size = 11;
                        comienzoy++;
                    }
                    int comienzox = 1;
                    comienzoy++;
                    ///NUEVOOOOOOOO
                    //foreach (var TABLA in tablas)
                    //{
                    comienzox = 1;
                    var tabladatos = datos;//tablaactual["datos"];
                    var primerelemento = (JObject)tabladatos.First();
                    var propiedades = primerelemento.Properties();
                    comienzox = 1;
                    //var color_thcabecera = Color.FromName((string)colorthcabecera);

                    var Columnas = getcolumnasexcel(primerelemento,(string)tabladefinicioncolumnas);
                    comienzoy++;

                    ///nuevo headers
                    if (aoHeader != null && (string)aoHeader != "")
                    {
                        foreach (var trfila in JArray.Parse(aoHeader.ToString()))/////3  trs filas de header
                        {
                            var nrofila = (string)trfila["nrofila"];
                            var ths = trfila["ths"];
                            int contadorcolumnas = 1;
                            foreach (var th in JArray.Parse(ths.ToString()))/////27  ths de tr  
                            {
                                var th_text = (string)th["text"];
                                var colspan = (int)th["colspan"]; ////columnas para derecha
                                var rowspan = (int)th["rowspan"];///filas par abajo
                                celda = worksheet.Cells[comienzoy, contadorcolumnas, comienzoy + rowspan - 1, contadorcolumnas + colspan - 1];
                                contadorcolumnas = contadorcolumnas + colspan;
                                celda.Value = th_text;
                                GenerarBordesCelda(celda);
                                celda.Style.Font.Bold = true;
                                celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                celda.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                                celda.Style.ShrinkToFit = false;
                                celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                celda.Style.ShrinkToFit = false;
                            }
                            comienzoy++;
                        }
                    }
                    ////fin nuevo header
                    int COMIENZOTABLADATOSY = comienzoy;

                    int finfilasdatos = tabladatos.Count() + COMIENZOTABLADATOSY;
                    int iii = 0;
                    //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                    //sw.Start();
                    String nombrecolumna=string.Empty;
                    columnasexcel columna_objeto;

                    var TipoCelda = new Dictionary<String, Func<JToken, object>>() {
                                { "DECIMAL", x =>(decimal)x },
                                { "FLOAT",   x =>(decimal)x },
                                { "STRING",  x =>  (string)x },
                                { "DATE",    x =>  (string)x },
                                { "INTEGER", x =>(Int64)x}
                            };

                    foreach (var fila in tabladatos)///llenar datos tabla
                    {
                       comienzox = 1;
                        //if (iii== 200)
                        //{
                        //    sw.Stop();
                        //    Console.WriteLine(sw.ElapsedMilliseconds);
                        //}
                        foreach (var prop in propiedades)
                        {
                            nombrecolumna = prop.Name;
                            columna_objeto = Columnas.Where(x => x.nombre == nombrecolumna).FirstOrDefault();
                            celda = worksheet.Cells[comienzoy, comienzox];
                            var valor_celda = TipoCelda[columna_objeto.tipo](fila[prop.Name]);
                            celda.Value = valor_celda;
                            comienzox++;
                        }///FIN FOR EACH PROPIEDADES =>  columnas 
                        comienzoy++;
                        iii++;
                    }////fin datos tabla   FILAS

                    ////COLUMNAS
                    string tipo, formato, sumar,alinear = string.Empty;
                    int columna_actual = 1;
                    foreach (var col in Columnas)
                    {
                        tipo = col.tipo;
                        formato = col.formato;
                        sumar = col.sumar;
                        alinear = col.alinear;
                        //celda = worksheet.Cells[finfilasdatos, comienzox];
                         celda = worksheet.Cells[COMIENZOTABLADATOSY, columna_actual, finfilasdatos-1, columna_actual];
                        CeldaAlinear(celda, alinear);
                        GenerarBordesCelda(celda);

                        if (tipo == "DECIMAL" || tipo == "FLOAT")
                        {
                            celda.Style.Numberformat.Format = formato;
                            worksheet.ConditionalFormatting.AddLessThan(celda).Formula = "0";
                        }
                        else if (tipo == "INTEGER")
                        {
                            worksheet.ConditionalFormatting.AddLessThan(celda).Formula = "0";
                            celda.Style.Numberformat.Format = formato;
                        }

                        if (Convert.ToBoolean(sumar) == true)
                        {
                            celda = worksheet.Cells[finfilasdatos, columna_actual];
                            celda.Formula = "SUM(" + worksheet.Cells[COMIENZOTABLADATOSY, columna_actual].Address + ":" + worksheet.Cells[finfilasdatos - 1, columna_actual].Address + ")";
                            celda.Style.Font.Bold = true;
                            celda.Style.Numberformat.Format = formato;

                        }
                        columna_actual++;
                    }
                    ////
                    //}
                    /// FIN NUEVOOOOOOOOOOOO


                    //worksheet.Column(1).AutoFit();
                    int columna = 1;
                    foreach (var prop in propiedades)
                    {
                        worksheet.Column(columna).AutoFit();
                        columna++;

                    }
                    worksheet.InsertColumn(1, 1);
                    //worksheet.Column(1).Width = 5;
                    result = package.GetAsByteArray();
                }
            }
            return result;
        }






    }
}
