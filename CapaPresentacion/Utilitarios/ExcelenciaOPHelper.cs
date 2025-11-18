using CapaEntidad.ExcelenciaOperativa;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace CapaPresentacion.Utilitarios {
    public class ExcelenciaOPHelper
    {
        public static MemoryStream ExcelTypeJOP(EO_FichaExcelenciaOperativaEntidad ficha)
        {
            MemoryStream memoryStream = new MemoryStream();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using(ExcelPackage excelPackage = new ExcelPackage())
            {

                ExcelWorksheet worksheetJOP = excelPackage.Workbook.Worksheets.Add("EO - JOP");
                WorksheetJOP(worksheetJOP, ficha);

                // Save Excel
                excelPackage.SaveAs(memoryStream);
            }

            return memoryStream;
        }

        public static MemoryStream ExcelTypeGU(EO_FichaExcelenciaOperativaEntidad ficha)
        {
            MemoryStream memoryStream = new MemoryStream();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using(ExcelPackage excelPackage = new ExcelPackage())
            {

                ExcelWorksheet worksheetJOP = excelPackage.Workbook.Worksheets.Add("EO - GU");
                WorksheetGU(worksheetJOP, ficha);
                
                // Save Excel
                excelPackage.SaveAs(memoryStream);
            }

            return memoryStream;
        }

        public static MemoryStream ExcelFisico(List<EO_FichaExcelenciaOperativaEntidad> listaFicha, dynamic parameters)
        {
            MemoryStream memoryStream = new MemoryStream();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using(ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheetEO = excelPackage.Workbook.Worksheets.Add("Fichas Excelencia Operativa");
                WorksheetEO(worksheetEO, listaFicha, parameters);

                foreach(EO_FichaExcelenciaOperativaEntidad ficha in listaFicha)
                {
                    if(ficha.Tipo == 1)
                    {
                        ExcelWorksheet worksheetJOP = excelPackage.Workbook.Worksheets.Add($"FichaId {ficha.FichaId}");
                        WorksheetJOP(worksheetJOP, ficha);
                    }

                    if(ficha.Tipo == 2)
                    {
                        ExcelWorksheet worksheetGU = excelPackage.Workbook.Worksheets.Add($"FichaId {ficha.FichaId}");
                        WorksheetGU(worksheetGU, ficha);
                    }
                }

                // Save Excel
                excelPackage.SaveAs(memoryStream);
            }

            return memoryStream;
        }

        public static MemoryStream ExcelNominal(List<EO_FichaExcelenciaOperativaEntidad> listaFicha, dynamic parameters)
        {
            List<EO_FichaExcelenciaOperativaEntidad> listaFichaV1 = listaFicha.Where(x => x.FichaVersion == 1).ToList();
            List<EO_FichaExcelenciaOperativaEntidad> listaFichaV3 = listaFicha.Where(x => x.FichaVersion == 3).ToList();
            List<EO_FichaExcelenciaOperativaEntidad> listaFichaV4 = listaFicha.Where(x => x.FichaVersion == 4).ToList();
            List<EO_FichaExcelenciaOperativaEntidad> listaFichaV5 = listaFicha.Where(x => x.FichaVersion == 5).ToList();

            MemoryStream memoryStream = new MemoryStream();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                 
                if (listaFichaV5.Any())
                {
                    ExcelWorksheet worksheetEONominalV5 = excelPackage.Workbook.Worksheets.Add("EO Nominal V5");
                    WorksheetEONominal(worksheetEONominalV5, listaFichaV5, parameters, 5);
                }
                if (listaFichaV4.Any())
                {
                    ExcelWorksheet worksheetEONominalV4 = excelPackage.Workbook.Worksheets.Add("EO Nominal V4");
                    WorksheetEONominal(worksheetEONominalV4, listaFichaV4, parameters, 4);
                }
                if (listaFichaV3.Any()) {
                    ExcelWorksheet worksheetEONominalV3 = excelPackage.Workbook.Worksheets.Add("EO Nominal V3");
                    WorksheetEONominal(worksheetEONominalV3, listaFichaV3, parameters, 3);
                }
                if (listaFichaV1.Any()) {
                    ExcelWorksheet worksheetEONominalV1 = excelPackage.Workbook.Worksheets.Add("EO Nominal V1");
                    WorksheetEONominal(worksheetEONominalV1, listaFichaV1, parameters, 1);
                }

                // Save Excel
                excelPackage.SaveAs(memoryStream);
            }

            return memoryStream;
        }

        public static MemoryStream ExcelNominalPuntuacion(List<EO_FichaExcelenciaOperativaEntidad> listaFicha, dynamic parameters)
        {
            List<EO_FichaExcelenciaOperativaEntidad> listaFichaV1 = listaFicha.Where(x => x.FichaVersion == 1).ToList();
            List<EO_FichaExcelenciaOperativaEntidad> listaFichaV3 = listaFicha.Where(x => x.FichaVersion == 3).ToList();
            List<EO_FichaExcelenciaOperativaEntidad> listaFichaV4 = listaFicha.Where(x => x.FichaVersion == 4).ToList();
            List<EO_FichaExcelenciaOperativaEntidad> listaFichaV5 = listaFicha.Where(x => x.FichaVersion == 5).ToList();

            MemoryStream memoryStream = new MemoryStream();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage())
            { 
                
                if (listaFichaV5.Any())
                {
                    ExcelWorksheet worksheetEONominalV5 = excelPackage.Workbook.Worksheets.Add("EO Nominal V5");
                    WorksheetEONominalPuntuacion(worksheetEONominalV5, listaFichaV5, parameters, 5);
                }
                if (listaFichaV4.Any()) {
                    ExcelWorksheet worksheetEONominalV4 = excelPackage.Workbook.Worksheets.Add("EO Nominal V4");
                    WorksheetEONominalPuntuacion(worksheetEONominalV4, listaFichaV4, parameters, 4);
                }
                if (listaFichaV3.Any()) {
                    ExcelWorksheet worksheetEONominalV3 = excelPackage.Workbook.Worksheets.Add("EO Nominal V3");
                    WorksheetEONominalPuntuacion(worksheetEONominalV3, listaFichaV3, parameters, 3);
                }
                if (listaFichaV1.Any()) {
                    ExcelWorksheet worksheetEONominalV1 = excelPackage.Workbook.Worksheets.Add("EO Nominal V1");
                    WorksheetEONominalPuntuacion(worksheetEONominalV1, listaFichaV1, parameters, 1);
                }

                // Save Excel
                excelPackage.SaveAs(memoryStream);
            }

            return memoryStream;
        }

        private static ExcelWorksheet WorksheetJOP(ExcelWorksheet worksheet, EO_FichaExcelenciaOperativaEntidad ficha)
        {
            // Row height
            double rowHeightlHeader = 20;
            double rowHeightHead = 22;
            double rowHeightlBody = 20;
            double rowHeightlFoot = 20;

            // Colors
            Color headBackgroundColor = ColorTranslator.FromHtml("#D9D9D9");
            Color borderColor = ColorTranslator.FromHtml("#242424");

            // Header of table
            worksheet.Row(1).Height = rowHeightlHeader;
            worksheet.Row(2).Height = rowHeightlHeader;
            worksheet.Cells[1, 1, 2, 3].Merge = true;
            worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[1, 1].Style.Font.Bold = true;
            worksheet.Cells[1, 1].Value = "EXCELENCIA OPERATIVA DE SALA - Jefe Operativo";

            // Information
            worksheet.Row(3).Height = rowHeightlHeader + 10;
            worksheet.Cells[3, 1, 3, 3].Merge = true;
            worksheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[3, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[3, 1].Value = $"Fecha : {ficha.Fecha.ToString("dd/MM/yyyy")} / Usuario : {ficha.UsuarioNombre} / Sala : {ficha.SalaNombre}";

            // Start Body
            int bodyFromRow = 4;
            int bodyFromCol = 1;
            int bodyToCol = 3;
            int categoryIndex = bodyFromRow;
            int recordIndex = bodyFromRow + 1;

            foreach(EO_FichaCategoriaEntidad categoria in ficha.Categorias)
            {

                worksheet.Row(categoryIndex).Height = rowHeightHead;
                worksheet.Cells[categoryIndex, bodyFromCol, categoryIndex, 2].Merge = true;
                worksheet.Cells[categoryIndex, bodyFromCol, categoryIndex, bodyToCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[categoryIndex, bodyFromCol, categoryIndex, bodyToCol].Style.Fill.BackgroundColor.SetColor(headBackgroundColor);
                worksheet.Cells[categoryIndex, bodyFromCol, categoryIndex, bodyToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[categoryIndex, bodyFromCol, categoryIndex, bodyToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                // Head Data
                worksheet.Cells[categoryIndex, 1].Value = categoria.Nombre;
                worksheet.Cells[categoryIndex, 3].Value = "Puntuación";

                // Items
                int itemIndex = 1;

                foreach(EO_FichaItemEntidad item in categoria.Items)
                {

                    worksheet.Row(recordIndex).Height = rowHeightlBody;
                    worksheet.Cells[recordIndex, bodyFromCol, recordIndex, bodyToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[recordIndex, bodyFromCol, recordIndex, bodyToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[recordIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Data
                    if (item.TipoRespuesta == 7)
                    {
                        worksheet.Cells[recordIndex, 1].Value = itemIndex;
                        worksheet.Cells[recordIndex, 2, recordIndex, 3].Merge = true;
                        worksheet.Cells[recordIndex, 2].Value = item.Observacion;
                        worksheet.Cells[recordIndex, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    } else
                    {
                        worksheet.Cells[recordIndex, 1].Value = itemIndex;
                        worksheet.Cells[recordIndex, 2].Value = item.Nombre;
                        worksheet.Cells[recordIndex, 3].Value = item.PuntuacionObtenida.ToString("0.00");
                    }
                    
                   

                    itemIndex++;
                    recordIndex++;
                }

                categoryIndex += itemIndex;
                recordIndex++;
            }

            // Body detail
            int bodyToRow = recordIndex - 2;

            // Border
            worksheet.Cells[bodyFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Top.Color.SetColor(borderColor);
            worksheet.Cells[bodyFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Left.Color.SetColor(borderColor);
            worksheet.Cells[bodyFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Right.Color.SetColor(borderColor);
            worksheet.Cells[bodyFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Bottom.Color.SetColor(borderColor);
            // End Body

            // Start Footer
            int detailIndex = recordIndex;
            int detailFromCol = bodyFromCol;
            int detailToCol = 5;

            // Head
            worksheet.Cells[detailIndex, 1].Value = "Detalle";
            worksheet.Cells[detailIndex, 3].Value = "Puntuación Base";
            worksheet.Cells[detailIndex, 4].Value = "Puntuación Obtenida";
            worksheet.Cells[detailIndex, 5].Value = "%";

            int detailRecordIndex = detailIndex + 1;

            foreach(EO_FichaCategoriaEntidad categoria in ficha.Categorias)
            {

                worksheet.Row(detailRecordIndex).Height = rowHeightlBody;
                worksheet.Cells[detailRecordIndex, detailFromCol, detailRecordIndex, 2].Merge = true;

                // Start Data
                worksheet.Cells[detailRecordIndex, 1].Value = categoria.Nombre;
                worksheet.Cells[detailRecordIndex, 3].Value = categoria.PuntuacionBase.ToString("0.00");
                worksheet.Cells[detailRecordIndex, 4].Value = categoria.PuntuacionObtenida.ToString("0.00");
                worksheet.Cells[detailRecordIndex, 5].Value = categoria.Porcentaje;
                // End Data

                // Format
                worksheet.Cells[detailRecordIndex, 5].Style.Numberformat.Format = "0.0%";

                detailRecordIndex++;
            }

            // Foot detail
            int detailFromRow = detailIndex;
            int detailToRow = detailRecordIndex;

            // Data
            worksheet.Cells[detailToRow, 3].Value = ficha.PuntuacionBase.ToString("0.00");
            worksheet.Cells[detailToRow, 4].Value = ficha.PuntuacionObtenida.ToString("0.00");
            worksheet.Cells[detailToRow, 5].Value = ficha.Porcentaje;

            // Table
            worksheet.Row(detailFromRow).Height = rowHeightHead;
            worksheet.Row(detailToRow).Height = rowHeightlFoot;
            worksheet.Cells[detailFromRow, detailFromCol, detailFromRow, 2].Merge = true;
            worksheet.Cells[detailFromRow, detailFromCol, detailFromRow, detailToCol].Style.WrapText = true;
            worksheet.Cells[detailFromRow, detailFromCol, detailFromRow, detailToCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[detailFromRow, detailFromCol, detailFromRow, detailToCol].Style.Fill.BackgroundColor.SetColor(headBackgroundColor);
            worksheet.Cells[detailFromRow, detailFromCol, detailToRow, detailToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[detailFromRow, detailFromCol, detailToRow, detailToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            // Border
            worksheet.Cells[detailFromRow, detailFromCol, detailToRow, detailToCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[detailFromRow, detailFromCol, detailToRow, detailToCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[detailFromRow, detailFromCol, detailToRow, detailToCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[detailFromRow, detailFromCol, detailToRow, detailToCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[detailFromRow, detailFromCol, detailToRow, detailToCol].Style.Border.Top.Color.SetColor(borderColor);
            worksheet.Cells[detailFromRow, detailFromCol, detailToRow, detailToCol].Style.Border.Left.Color.SetColor(borderColor);
            worksheet.Cells[detailFromRow, detailFromCol, detailToRow, detailToCol].Style.Border.Right.Color.SetColor(borderColor);
            worksheet.Cells[detailFromRow, detailFromCol, detailToRow, detailToCol].Style.Border.Bottom.Color.SetColor(borderColor);

            worksheet.Cells[detailToRow, detailFromCol, detailToRow, 2].Style.Border.Top.Style = ExcelBorderStyle.None;
            worksheet.Cells[detailToRow, detailFromCol, detailToRow, 2].Style.Border.Left.Style = ExcelBorderStyle.None;
            worksheet.Cells[detailToRow, detailFromCol, detailToRow, 2].Style.Border.Right.Style = ExcelBorderStyle.None;
            worksheet.Cells[detailToRow, detailFromCol, detailToRow, 2].Style.Border.Bottom.Style = ExcelBorderStyle.None;

            // Format
            worksheet.Cells[detailToRow, detailToCol].Style.Numberformat.Format = "0.0%";

            // End Footer

            // Font Size
            worksheet.Cells[1, 1, detailToRow, detailToCol].Style.Font.Size = 10;

            // Columns Width
            worksheet.Column(1).Width = 6;
            worksheet.Column(2).AutoFit();
            worksheet.Column(3).Width = 12;
            worksheet.Column(4).Width = 12;
            worksheet.Column(5).Width = 12;

            // Rows Custom Height
            worksheet.Row(detailFromRow).CustomHeight = false;

            if(worksheet.Workbook.Worksheets.Count > 1) {
                // Add a button to return to the main sheet in cell D3
                worksheet.Cells[3, 4, 3, 5].Merge = true;
                worksheet.Cells[3, 4].Value = "Regresar a Principal";
                worksheet.Cells[3, 4].Hyperlink = new Uri($"#'{worksheet.Workbook.Worksheets[0].Name}'!A1", UriKind.Relative);
                worksheet.Cells[3, 4].Style.Font.UnderLine = true;
                worksheet.Cells[3, 4].Style.Font.Color.SetColor(Color.Blue);
                worksheet.Cells[3, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[3, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }


            return worksheet;
        }

        private static ExcelWorksheet WorksheetGU(ExcelWorksheet worksheet, EO_FichaExcelenciaOperativaEntidad ficha)
        {
            // Row height
            double rowHeightlHeader = 20;
            double rowHeightHead = 22;
            double rowHeightlBody = 20;
            double rowHeightlFoot = 20;

            // Colors
            Color headBackgroundColor = ColorTranslator.FromHtml("#D9D9D9");
            Color borderColor = ColorTranslator.FromHtml("#242424");

            // Header of table
            worksheet.Row(1).Height = rowHeightlHeader;
            worksheet.Row(2).Height = rowHeightlHeader;
            worksheet.Cells[1, 1, 2, 4].Merge = true;
            worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[1, 1].Style.Font.Bold = true;
            worksheet.Cells[1, 1].Value = "EXCELENCIA OPERATIVA DE SALA - Gerente de Unidad";

            // Information
            worksheet.Row(3).Height = rowHeightlHeader + 10;
            worksheet.Cells[3, 1, 3, 3].Merge = true;
            worksheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[3, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[3, 1].Value = $"Fecha : {ficha.Fecha.ToString("dd/MM/yyyy")} / Usuario : {ficha.UsuarioNombre} / Sala : {ficha.SalaNombre}";

            // Start Body
            int bodyFromRow = 4;
            int bodyFromCol = 1;
            int bodyToCol = 4;
            int categoryIndex = bodyFromRow;
            int recordIndex = bodyFromRow + 1;

            foreach(EO_FichaCategoriaEntidad categoria in ficha.Categorias)
            {

                worksheet.Row(categoryIndex).Height = rowHeightHead;
                worksheet.Cells[categoryIndex, bodyFromCol, categoryIndex, 2].Merge = true;
                worksheet.Cells[categoryIndex, bodyFromCol, categoryIndex, bodyToCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[categoryIndex, bodyFromCol, categoryIndex, bodyToCol].Style.Fill.BackgroundColor.SetColor(headBackgroundColor);
                worksheet.Cells[categoryIndex, bodyFromCol, categoryIndex, bodyToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[categoryIndex, bodyFromCol, categoryIndex, bodyToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                worksheet.Cells[categoryIndex, bodyFromCol, categoryIndex, bodyToCol].Style.WrapText = true;
                worksheet.Row(categoryIndex).CustomHeight = false;

                // Head Data
                worksheet.Cells[categoryIndex, 1].Value = categoria.Nombre;
                worksheet.Cells[categoryIndex, 3].Value = "Puntuación";
                worksheet.Cells[categoryIndex, 4].Value = "Fecha Vencimiento";

                // Items
                int itemIndex = 1;

                foreach(EO_FichaItemEntidad item in categoria.Items)
                {

                    worksheet.Row(recordIndex).Height = rowHeightlBody;
                    worksheet.Cells[recordIndex, bodyFromCol, recordIndex, bodyToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[recordIndex, bodyFromCol, recordIndex, bodyToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[recordIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Start Data
                    worksheet.Cells[recordIndex, 1].Value = itemIndex;
                    worksheet.Cells[recordIndex, 2].Value = item.Nombre;
                    worksheet.Cells[recordIndex, 3].Value = item.PuntuacionObtenida.ToString("0.00");

                    string dateExpiry = "---";

                    if(item.FechaExpiracionActivo == 1)
                    {
                        DateTime temp;
                        string formatDateExpiry = string.Format("{0:dd/MM/yyyy}", item.FechaExpiracion);

                        if(DateTime.TryParse(formatDateExpiry, out temp))
                        {
                            dateExpiry = formatDateExpiry;
                        }
                        else
                        {
                            dateExpiry = "No asignado";
                        }
                    }

                    worksheet.Cells[recordIndex, 4].Value = dateExpiry;
                    // End Data

                    itemIndex++;
                    recordIndex++;
                }

                categoryIndex += itemIndex;
                recordIndex++;
            }

            // Body detail
            int bodyToRow = recordIndex - 2;

            // Border
            worksheet.Cells[bodyFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Top.Color.SetColor(borderColor);
            worksheet.Cells[bodyFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Left.Color.SetColor(borderColor);
            worksheet.Cells[bodyFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Right.Color.SetColor(borderColor);
            worksheet.Cells[bodyFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Bottom.Color.SetColor(borderColor);
            // End Body

            // Start Footer
            int detailIndex = recordIndex;
            int detailFromCol = bodyFromCol;
            int detailToCol = 5;

            // Head
            worksheet.Cells[detailIndex, 1].Value = "Detalle";
            worksheet.Cells[detailIndex, 3].Value = "Puntuación Base";
            worksheet.Cells[detailIndex, 4].Value = "Puntuación Obtenida";
            worksheet.Cells[detailIndex, 5].Value = "%";

            int detailRecordIndex = detailIndex + 1;

            foreach(EO_FichaCategoriaEntidad categoria in ficha.Categorias)
            {

                worksheet.Row(detailRecordIndex).Height = rowHeightlBody;
                worksheet.Cells[detailRecordIndex, detailFromCol, detailRecordIndex, 2].Merge = true;

                // Data
                worksheet.Cells[detailRecordIndex, 1].Value = categoria.Nombre;
                worksheet.Cells[detailRecordIndex, 3].Value = categoria.PuntuacionBase.ToString("0.00");
                worksheet.Cells[detailRecordIndex, 4].Value = categoria.PuntuacionObtenida.ToString("0.00");
                worksheet.Cells[detailRecordIndex, 5].Value = categoria.Porcentaje;

                // Format
                worksheet.Cells[detailRecordIndex, 5].Style.Numberformat.Format = "0.0%";

                detailRecordIndex++;
            }

            // Foot detail
            int detailFromRow = detailIndex;
            int detailToRow = detailRecordIndex;

            // Data
            worksheet.Cells[detailToRow, 3].Value = ficha.PuntuacionBase.ToString("0.00");
            worksheet.Cells[detailToRow, 4].Value = ficha.PuntuacionObtenida.ToString("0.00");
            worksheet.Cells[detailToRow, 5].Value = ficha.Porcentaje;

            // Table
            worksheet.Row(detailFromRow).Height = rowHeightHead;
            worksheet.Row(detailToRow).Height = rowHeightlFoot;
            worksheet.Cells[detailFromRow, detailFromCol, detailFromRow, 2].Merge = true;
            worksheet.Cells[detailFromRow, detailFromCol, detailFromRow, detailToCol].Style.WrapText = true;
            worksheet.Cells[detailFromRow, detailFromCol, detailFromRow, detailToCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[detailFromRow, detailFromCol, detailFromRow, detailToCol].Style.Fill.BackgroundColor.SetColor(headBackgroundColor);
            worksheet.Cells[detailFromRow, detailFromCol, detailToRow, detailToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[detailFromRow, detailFromCol, detailToRow, detailToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            // Border
            worksheet.Cells[detailFromRow, detailFromCol, detailToRow, detailToCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[detailFromRow, detailFromCol, detailToRow, detailToCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[detailFromRow, detailFromCol, detailToRow, detailToCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[detailFromRow, detailFromCol, detailToRow, detailToCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[detailFromRow, detailFromCol, detailToRow, detailToCol].Style.Border.Top.Color.SetColor(borderColor);
            worksheet.Cells[detailFromRow, detailFromCol, detailToRow, detailToCol].Style.Border.Left.Color.SetColor(borderColor);
            worksheet.Cells[detailFromRow, detailFromCol, detailToRow, detailToCol].Style.Border.Right.Color.SetColor(borderColor);
            worksheet.Cells[detailFromRow, detailFromCol, detailToRow, detailToCol].Style.Border.Bottom.Color.SetColor(borderColor);

            worksheet.Cells[detailToRow, detailFromCol, detailToRow, 2].Style.Border.Top.Style = ExcelBorderStyle.None;
            worksheet.Cells[detailToRow, detailFromCol, detailToRow, 2].Style.Border.Left.Style = ExcelBorderStyle.None;
            worksheet.Cells[detailToRow, detailFromCol, detailToRow, 2].Style.Border.Right.Style = ExcelBorderStyle.None;
            worksheet.Cells[detailToRow, detailFromCol, detailToRow, 2].Style.Border.Bottom.Style = ExcelBorderStyle.None;

            // Format
            worksheet.Cells[detailToRow, detailToCol].Style.Numberformat.Format = "0.0%";

            // End Footer

            // Font Size
            worksheet.Cells[1, 1, detailToRow, detailToCol].Style.Font.Size = 10;

            // Columns Width
            worksheet.Column(1).Width = 6;
            worksheet.Column(2).AutoFit();
            worksheet.Column(3).Width = 12;
            worksheet.Column(4).Width = 12;
            worksheet.Column(5).Width = 12;

            // Rows Custom Height
            worksheet.Row(detailFromRow).CustomHeight = false;

            if(worksheet.Workbook.Worksheets.Count > 1) {
                // Add a button to return to the main sheet in cell D3
                worksheet.Cells[3, 4, 3, 5].Merge = true;
                worksheet.Cells[3, 4].Value = "Regresar a Principal";
                worksheet.Cells[3, 4].Hyperlink = new Uri($"#'{worksheet.Workbook.Worksheets[0].Name}'!A1", UriKind.Relative);
                worksheet.Cells[3, 4].Style.Font.UnderLine = true;
                worksheet.Cells[3, 4].Style.Font.Color.SetColor(Color.Blue);
                worksheet.Cells[3, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[3, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            return worksheet;
        }

        private static ExcelWorksheet WorksheetEO(ExcelWorksheet worksheet, List<EO_FichaExcelenciaOperativaEntidad> listaFicha, dynamic parameters)
        {
            // Parameters
            DateTime startDate = Convert.ToDateTime(parameters.startDate);
            DateTime endDate = Convert.ToDateTime(parameters.endDate);

            Dictionary<int, string> typesCollection = new Dictionary<int, string>
            {
                { 1, "Jefe Operativo" },
                { 2, "Gerente de Unidad" }
            };

            // Row height
            double rowHeightlHeader = 20;

            // Colors
            Color headBackgroundColor = ColorTranslator.FromHtml("#003268");
            Color borderColor = ColorTranslator.FromHtml("#074B88");

            // Header
            worksheet.Row(2).Height = rowHeightlHeader;
            worksheet.Row(3).Height = rowHeightlHeader;
            worksheet.Cells[2, 2, 3, 9].Merge = true;
            worksheet.Cells[4, 2, 4, 9].Merge = true;
            worksheet.Cells[2, 2, 4, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[2, 2, 4, 9].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[2, 2].Style.Font.Size = 14;
            worksheet.Cells[4, 2].Style.Font.Size = 12;
            worksheet.Cells[2, 2].Style.Font.Bold = true;

            worksheet.Cells[2, 2].Value = "Fichas de Excelencia Operativa";
            worksheet.Cells[4, 2].Value = $"{startDate.ToString("dd/MM/yyyy")} al {endDate.ToString("dd/MM/yyyy")}";

            // Head
            int bodyFromRow = 6;
            int bodyFromCol = 2;
            int bodyToCol = 9;

            worksheet.Cells[bodyFromRow, 2].Value = "Id";
            worksheet.Cells[bodyFromRow, 3].Value = "Tipo";
            worksheet.Cells[bodyFromRow, 4].Value = "Fecha";
            worksheet.Cells[bodyFromRow, 5].Value = "Puntuación Base";
            worksheet.Cells[bodyFromRow, 6].Value = "Puntuación Obtenida";
            worksheet.Cells[bodyFromRow, 7].Value = "Porcentaje";
            worksheet.Cells[bodyFromRow, 8].Value = "Usuario";
            worksheet.Cells[bodyFromRow, 9].Value = "Sala";

            // Body
            int totalRecords = listaFicha.Count;
            int recordIndex = bodyFromRow + 1;

            foreach(EO_FichaExcelenciaOperativaEntidad ficha in listaFicha) {

                // Data
                worksheet.Cells[recordIndex, 2].Value = ficha.FichaId;
                worksheet.Cells[recordIndex, 3].Value = typesCollection.ContainsKey(ficha.Tipo) ? typesCollection[ficha.Tipo] : "";
                worksheet.Cells[recordIndex, 4].Value = ficha.Fecha.ToString("dd/MM/yyyy");
                worksheet.Cells[recordIndex, 5].Value = ficha.PuntuacionBase.ToString("0.00");
                worksheet.Cells[recordIndex, 6].Value = ficha.PuntuacionObtenida.ToString("0.00");
                worksheet.Cells[recordIndex, 7].Value = ficha.Porcentaje;
                worksheet.Cells[recordIndex, 8].Value = ficha.UsuarioNombre;
                worksheet.Cells[recordIndex, 9].Value = ficha.SalaNombre;

                // Formula
                worksheet.Cells[recordIndex, 2].Hyperlink = new Uri($"#'FichaId {ficha.FichaId}'!A1", UriKind.Relative);
                worksheet.Cells[recordIndex, 2].Style.Font.UnderLine = true;
                worksheet.Cells[recordIndex, 2].Style.Font.Color.SetColor(Color.Blue);

                recordIndex++;
            }

            int footFromRow = bodyFromRow + totalRecords + 1;
            int formatFromRow = bodyFromRow + 1;
            int formatToRow = bodyFromRow + totalRecords;
            

            // Head style
            worksheet.Cells[bodyFromRow, bodyFromCol, bodyFromRow, bodyToCol].Style.Font.Bold = true;
            worksheet.Cells[bodyFromRow, bodyFromCol, bodyFromRow, bodyToCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[bodyFromRow, bodyFromCol, bodyFromRow, bodyToCol].Style.Fill.BackgroundColor.SetColor(headBackgroundColor);
            worksheet.Cells[bodyFromRow, bodyFromCol, bodyFromRow, bodyToCol].Style.Font.Color.SetColor(Color.White);

            // Borders Table
            worksheet.Cells[bodyFromRow, bodyFromCol, footFromRow, bodyToCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, footFromRow, bodyToCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, footFromRow, bodyToCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, footFromRow, bodyToCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, footFromRow, bodyToCol].Style.Border.Top.Color.SetColor(borderColor);
            worksheet.Cells[bodyFromRow, bodyFromCol, footFromRow, bodyToCol].Style.Border.Left.Color.SetColor(borderColor);
            worksheet.Cells[bodyFromRow, bodyFromCol, footFromRow, bodyToCol].Style.Border.Right.Color.SetColor(borderColor);
            worksheet.Cells[bodyFromRow, bodyFromCol, footFromRow, bodyToCol].Style.Border.Bottom.Color.SetColor(borderColor);

            // Alignment
            worksheet.Cells[bodyFromRow, bodyFromCol, footFromRow, bodyToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[bodyFromRow, bodyFromCol, footFromRow, bodyToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[bodyFromRow, bodyFromCol, footFromRow, bodyToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[bodyFromRow, bodyFromCol, footFromRow, bodyToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            // Format
            worksheet.Cells[formatFromRow, 7, formatToRow, 7].Style.Numberformat.Format = "0.0%";

            // Foot
            worksheet.Cells[footFromRow, bodyFromCol, footFromRow, bodyToCol].Merge = true;
            worksheet.Cells[footFromRow, bodyFromCol, footFromRow, bodyToCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[footFromRow, bodyFromCol, footFromRow, bodyToCol].Style.Fill.BackgroundColor.SetColor(headBackgroundColor);
            worksheet.Cells[footFromRow, bodyFromCol, footFromRow, bodyToCol].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells[footFromRow, bodyFromCol, footFromRow, bodyToCol].Style.Font.Bold = true;
            worksheet.Cells[footFromRow, bodyFromCol].Value = $"Total : {totalRecords} Registros";

            // Filters
            worksheet.Cells[bodyFromRow, bodyFromCol, formatToRow, bodyToCol].AutoFilter = true;

            // Columns Width
            worksheet.Column(2).AutoFit();
            worksheet.Column(3).Width = 25;
            worksheet.Column(4).Width = 15;
            worksheet.Column(5).Width = 25;
            worksheet.Column(6).Width = 25;
            worksheet.Column(7).Width = 20;
            worksheet.Column(8).Width = 25;
            worksheet.Column(9).Width = 25;

            // Rows Custom Height
            worksheet.DefaultRowHeight = 15;

            return worksheet;
        }

        private static ExcelWorksheet WorksheetEONominal(ExcelWorksheet worksheet, List<EO_FichaExcelenciaOperativaEntidad> listaFicha, dynamic parameters, int version)
        {
            // Parameters
            int typeCode = parameters.typeCode;
            string roomName = parameters.roomName;
            DateTime startDate = Convert.ToDateTime(parameters.startDate);
            DateTime endDate = Convert.ToDateTime(parameters.endDate);

            string folderPath = System.Web.Hosting.HostingEnvironment.MapPath("~/");
            string fileName = string.Empty;
            string typeName = string.Empty;

            // data items
            List<EO_FichaCategoriaEntidad> categories = new List<EO_FichaCategoriaEntidad>();

            if(typeCode == 1)
            {
                fileName = "Content/data/EOJefeOperativo.json";
                typeName = "Jefe Operativo";

                if (version == 3)
                {
                    fileName = "Content/data/EOJefeOperativoV3.json";
                }
                if (version == 4)
                {
                    fileName = "Content/data/EOJefeOperativoV4.json";
                }
                if (version == 5)
                {
                    fileName = "Content/data/EOJefeOperativoV5.json";
                }
            }

            if (typeCode == 2)
            {
                fileName = "Content/data/EOGerenteUnidad.json";
                typeName = "Gerente de Unidad";
            }

            using(StreamReader streamReader = new StreamReader(Path.Combine(folderPath, fileName)))
            {
                string json = streamReader.ReadToEnd();
                categories = JsonConvert.DeserializeObject<List<EO_FichaCategoriaEntidad>>(json);
            }

            // Row height
            double rowHeightl = 20;

            // Colors
            Color headerBackgroundColor = ColorTranslator.FromHtml("#003268");
            Color headBackgroundColor = ColorTranslator.FromHtml("#D9D9D9");
            Color borderColor = ColorTranslator.FromHtml("#242424");

            // Header
            worksheet.Row(1).Height = rowHeightl + 10;
            worksheet.Cells[1, 1, 1, 2].Merge = true;
            worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[1, 1].Style.Font.Bold = true;
            worksheet.Cells[1, 1].Value = $"Sala : {roomName} / Fecha : {startDate.ToString("dd/MM/yyyy")} al {endDate.ToString("dd/MM/yyyy")}";

            // Head
            worksheet.Row(2).Height = rowHeightl + 5;
            worksheet.Cells[2, 1, 2, 2].Merge = true;
            worksheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[2, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[2, 1].Style.Font.Bold = true;
            worksheet.Cells[2, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[2, 1].Style.Fill.BackgroundColor.SetColor(headerBackgroundColor);
            worksheet.Cells[2, 1].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells[2, 1].Value = $"EXCELENCIA OPERATIVA DE SALA - {typeName}";

            // Body
            int itemFromRow = 3;
            int itemFromCol = 1;
            int itemToCol = 2;

            int recordA = itemFromRow;
            int recordB = itemFromRow + 1;

            foreach(EO_FichaCategoriaEntidad category in categories)
            {
                worksheet.Row(recordA).Height = rowHeightl + 2;
                worksheet.Cells[recordA, itemFromCol, recordA, itemToCol].Merge = true;
                worksheet.Cells[recordA, itemFromCol, recordA, itemToCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[recordA, itemFromCol, recordA, itemToCol].Style.Fill.BackgroundColor.SetColor(headBackgroundColor);
                worksheet.Cells[recordA, itemFromCol, recordA, itemToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[recordA, itemFromCol, recordA, itemToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                // Data
                worksheet.Cells[recordA, itemFromCol].Value = category.Nombre;

                int indexB = 1;

                foreach(EO_FichaItemEntidad item in category.Items)
                {
                    worksheet.Row(recordB).Height = rowHeightl;
                    worksheet.Cells[recordB, itemFromCol, recordB, itemToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[recordB, itemFromCol, recordB, itemToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[recordB, itemToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Data
                    worksheet.Cells[recordB, itemFromCol].Value = indexB;
                    worksheet.Cells[recordB, itemToCol].Value = item.Nombre;

                    indexB++;
                    recordB++;
                }

                recordA += indexB;
                recordB++;
            }

            // foot
            int footFromRow = recordB - 1;
            int footToRow = footFromRow + 2;
            int sheetFromRow = 1;
            int borderFromRow = sheetFromRow + 1;

            // Merge
            worksheet.Cells[footFromRow, itemFromCol, footFromRow, itemToCol].Merge = true;
            worksheet.Cells[footFromRow + 1, itemFromCol, footFromRow + 1, itemToCol].Merge = true;
            worksheet.Cells[footFromRow + 2, itemFromCol, footFromRow + 2, itemToCol].Merge = true;

            // Data
            worksheet.Cells[footFromRow, itemFromCol].Value = "Puntuacion base";
            worksheet.Cells[footFromRow + 1, itemFromCol].Value = "Puntuación obtenida";
            worksheet.Cells[footFromRow + 2, itemFromCol].Value = "Porcentaje";

            // Height
            worksheet.Row(footFromRow).Height = rowHeightl + 2;
            worksheet.Row(footFromRow + 1).Height = rowHeightl + 2;
            worksheet.Row(footFromRow + 2).Height = rowHeightl + 2;

            // Style
            worksheet.Cells[footFromRow, itemFromCol, footToRow, itemToCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[footFromRow, itemFromCol, footToRow, itemToCol].Style.Fill.BackgroundColor.SetColor(headBackgroundColor);
            worksheet.Cells[footFromRow, itemFromCol, footToRow, itemToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            worksheet.Cells[footFromRow, itemFromCol, footToRow, itemToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[footFromRow, itemFromCol, footToRow, itemToCol].Style.Font.Bold = true;

            // Border Table
            worksheet.Cells[borderFromRow, itemFromCol, footToRow, itemToCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[borderFromRow, itemFromCol, footToRow, itemToCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[borderFromRow, itemFromCol, footToRow, itemToCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[borderFromRow, itemFromCol, footToRow, itemToCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[borderFromRow, itemFromCol, footToRow, itemToCol].Style.Border.Top.Color.SetColor(borderColor);
            worksheet.Cells[borderFromRow, itemFromCol, footToRow, itemToCol].Style.Border.Left.Color.SetColor(borderColor);
            worksheet.Cells[borderFromRow, itemFromCol, footToRow, itemToCol].Style.Border.Right.Color.SetColor(borderColor);
            worksheet.Cells[borderFromRow, itemFromCol, footToRow, itemToCol].Style.Border.Bottom.Color.SetColor(borderColor);

            // Font Size
            worksheet.Cells[sheetFromRow, itemFromCol, footToRow, itemToCol].Style.Font.Size = 10;

            // Start scores
            int dayFromRow = 2;
            int dayFromCol = itemToCol + 1;

            int dayRecord = dayFromCol;

            foreach(EO_FichaExcelenciaOperativaEntidad ficha in listaFicha)
            {
                // Data Ficha
                worksheet.Cells[dayFromRow, dayRecord].Value = ficha.Fecha.ToString("dd/MM/yyyy");

                int categoryRecord = dayFromRow + 1;
                int itemRecord = dayFromRow + 2;

                int indexLocated = 0;

                int indexCategory = 1;

                foreach(EO_FichaCategoriaEntidad fichaCategoria in ficha.Categorias)
                {
                    worksheet.Cells[categoryRecord, dayFromCol, categoryRecord, dayRecord].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[categoryRecord, dayFromCol, categoryRecord, dayRecord].Style.Fill.BackgroundColor.SetColor(headBackgroundColor);
                    worksheet.Cells[categoryRecord, dayFromCol, categoryRecord, dayRecord].Value = "Respuesta";

                    int indexItem = 1;

                    foreach(EO_FichaItemEntidad fichaItem in fichaCategoria.Items)
                    {
                        if(string.IsNullOrEmpty(fichaItem.Codigo))
                        {
                            if(typeCode == 1 && indexCategory == 1)
                            {
                                if(indexItem <= 6)
                                {
                                    worksheet.Cells[itemRecord, dayRecord].Value = fichaItem.Respuesta;
                                }
                                else
                                {
                                    worksheet.Cells[itemRecord + 1, dayRecord].Value = fichaItem.Respuesta;
                                }

                                if(fichaCategoria.Items.Count == 11)
                                {
                                    indexLocated = 1;
                                }
                            }
                            else
                            {
                                worksheet.Cells[itemRecord, dayRecord].Value = fichaItem.Respuesta;
                            }
                        }
                        else
                        {
                            if (fichaItem.TipoRespuesta == 7)
                            {
                                if (!string.IsNullOrEmpty(fichaItem.Observacion))
                                {
                                    worksheet.Cells[itemRecord, dayRecord].Value = "Ver Comentario";

                                } else
                                {
                                    worksheet.Cells[itemRecord, dayRecord].Value = "Sin comentario";

                                }
                                if (!string.IsNullOrEmpty(fichaItem.Observacion))
                                {
                                    var comment = worksheet.Cells[itemRecord, dayRecord].AddComment(fichaItem.Observacion, "Nombre de Usuario");

                                    comment.AutoFit = true;
                                    comment.Font.Size = 10;
                                }
                            } else
                            {
                                worksheet.Cells[itemRecord, dayRecord].Value = fichaItem.Respuesta;

                            }
                        }
                        
                        indexItem++;
                        itemRecord++;
                    }

                    indexCategory++;

                    categoryRecord += indexItem;
                    itemRecord++;

                    // index located
                    categoryRecord += indexLocated;
                    itemRecord += indexLocated;
                    indexLocated = 0;
                }

                dayRecord++;
            }
            // End scores

            // Head days style
            int dayToCol = dayRecord - 1;

            worksheet.Cells[dayFromRow, dayFromCol, dayFromRow, dayToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[dayFromRow, dayFromCol, dayFromRow, dayToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[dayFromRow, dayFromCol, dayFromRow, dayToCol].Style.Font.Bold = true;
            worksheet.Cells[dayFromRow, dayFromCol, dayFromRow, dayToCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[dayFromRow, dayFromCol, dayFromRow, dayToCol].Style.Fill.BackgroundColor.SetColor(headerBackgroundColor);
            worksheet.Cells[dayFromRow, dayFromCol, dayFromRow, dayToCol].Style.Font.Color.SetColor(Color.White);

            // Start Total
            int totalFromRow = footFromRow;
            int totalRecord = dayFromCol;

            foreach(EO_FichaExcelenciaOperativaEntidad ficha in listaFicha)
            {
                // Data
                worksheet.Cells[totalFromRow, totalRecord].Value = ficha.PuntuacionBase.ToString("0.00");
                worksheet.Cells[totalFromRow + 1, totalRecord].Value = ficha.PuntuacionObtenida.ToString("0.00");
                worksheet.Cells[totalFromRow + 2, totalRecord].Value = ficha.Porcentaje;

                // Format
                worksheet.Cells[totalFromRow + 2, totalRecord].Style.Numberformat.Format = "0.0%";

                //Columns Width
                worksheet.Column(totalRecord).AutoFit();

                totalRecord++;
            }
            // End Total

            // Foot style
            int totalToRow = totalFromRow + 2;
            int totalToCol = totalRecord - 1;

            worksheet.Cells[totalFromRow, dayFromCol, totalToRow, totalToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[totalFromRow, dayFromCol, totalToRow, totalToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[totalFromRow, dayFromCol, totalToRow, totalToCol].Style.Font.Bold = true;

            // Score styles
            int scoreFromRow = dayFromRow + 1;
            int scoreToRow = totalFromRow - 1;

            worksheet.Cells[scoreFromRow, dayFromCol, scoreToRow, totalToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[scoreFromRow, dayFromCol, scoreToRow, totalToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            // Border Scores
            worksheet.Cells[borderFromRow, dayFromCol, totalToRow, totalToCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[borderFromRow, dayFromCol, totalToRow, totalToCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[borderFromRow, dayFromCol, totalToRow, totalToCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[borderFromRow, dayFromCol, totalToRow, totalToCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[borderFromRow, dayFromCol, totalToRow, totalToCol].Style.Border.Top.Color.SetColor(borderColor);
            worksheet.Cells[borderFromRow, dayFromCol, totalToRow, totalToCol].Style.Border.Left.Color.SetColor(borderColor);
            worksheet.Cells[borderFromRow, dayFromCol, totalToRow, totalToCol].Style.Border.Right.Color.SetColor(borderColor);
            worksheet.Cells[borderFromRow, dayFromCol, totalToRow, totalToCol].Style.Border.Bottom.Color.SetColor(borderColor);

            // Font Size
            worksheet.Cells[borderFromRow, dayFromCol, totalToRow, totalToCol].Style.Font.Size = 10;

            // Columns Width
            worksheet.Column(1).AutoFit();
            worksheet.Column(2).AutoFit();

            return worksheet;
        }

        private static ExcelWorksheet WorksheetEONominalPuntuacion(ExcelWorksheet worksheet, List<EO_FichaExcelenciaOperativaEntidad> listaFicha, dynamic parameters, int version)
        {
            // Parameters
            int typeCode = parameters.typeCode;
            string roomName = parameters.roomName;
            DateTime startDate = Convert.ToDateTime(parameters.startDate);
            DateTime endDate = Convert.ToDateTime(parameters.endDate);

            string folderPath = System.Web.Hosting.HostingEnvironment.MapPath("~/");
            string fileName = string.Empty;
            string typeName = string.Empty;

            // data items
            List<EO_FichaCategoriaEntidad> categories = new List<EO_FichaCategoriaEntidad>();

            if (typeCode == 1)
            {
                fileName = "Content/data/EOJefeOperativo.json";
                typeName = "Jefe Operativo";

                if (version == 3)
                {
                    fileName = "Content/data/EOJefeOperativoV3.json";
                }
                if (version == 4)
                {
                    fileName = "Content/data/EOJefeOperativoV4.json";
                }
                if (version == 5)
                {
                    fileName = "Content/data/EOJefeOperativoV5.json";
                }
            }

            if(typeCode == 2)
            {
                fileName = "Content/data/EOGerenteUnidad.json";
                typeName = "Gerente de Unidad";
            }

            using(StreamReader streamReader = new StreamReader(Path.Combine(folderPath, fileName)))
            {
                string json = streamReader.ReadToEnd();
                categories = JsonConvert.DeserializeObject<List<EO_FichaCategoriaEntidad>>(json);
            }

            // Row height
            double rowHeightl = 20;

            // Colors
            Color headerBackgroundColor = ColorTranslator.FromHtml("#003268");
            Color headBackgroundColor = ColorTranslator.FromHtml("#D9D9D9");
            Color borderColor = ColorTranslator.FromHtml("#242424");

            // Header
            worksheet.Row(1).Height = rowHeightl + 10;
            worksheet.Cells[1, 1, 1, 2].Merge = true;
            worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[1, 1].Style.Font.Bold = true;
            worksheet.Cells[1, 1].Value = $"Sala : {roomName} / Fecha : {startDate.ToString("dd/MM/yyyy")} al {endDate.ToString("dd/MM/yyyy")}";

            // Head
            worksheet.Row(2).Height = rowHeightl + 5;
            worksheet.Cells[2, 1, 2, 2].Merge = true;
            worksheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[2, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[2, 1].Style.Font.Bold = true;
            worksheet.Cells[2, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[2, 1].Style.Fill.BackgroundColor.SetColor(headerBackgroundColor);
            worksheet.Cells[2, 1].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells[2, 1].Value = $"EXCELENCIA OPERATIVA DE SALA - {typeName}";

            // Body
            int itemFromRow = 3;
            int itemFromCol = 1;
            int itemToCol = 2;

            int recordA = itemFromRow;
            int recordB = itemFromRow + 1;

            foreach(EO_FichaCategoriaEntidad category in categories)
            {
                worksheet.Row(recordA).Height = rowHeightl + 2;
                worksheet.Cells[recordA, itemFromCol, recordA, itemToCol].Merge = true;
                worksheet.Cells[recordA, itemFromCol, recordA, itemToCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[recordA, itemFromCol, recordA, itemToCol].Style.Fill.BackgroundColor.SetColor(headBackgroundColor);
                worksheet.Cells[recordA, itemFromCol, recordA, itemToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[recordA, itemFromCol, recordA, itemToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                // Data
                worksheet.Cells[recordA, itemFromCol].Value = category.Nombre;

                int indexB = 1;

                foreach(EO_FichaItemEntidad item in category.Items)
                {
                    worksheet.Row(recordB).Height = rowHeightl;
                    worksheet.Cells[recordB, itemFromCol, recordB, itemToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[recordB, itemFromCol, recordB, itemToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[recordB, itemToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Data
                    worksheet.Cells[recordB, itemFromCol].Value = indexB;
                    worksheet.Cells[recordB, itemToCol].Value = item.Nombre;

                    indexB++;
                    recordB++;
                }

                recordA += indexB;
                recordB++;
            }

            // foot
            int footFromRow = recordB - 1;
            int footToRow = footFromRow + 2;
            int sheetFromRow = 1;
            int borderFromRow = sheetFromRow + 1;

            // Merge
            worksheet.Cells[footFromRow, itemFromCol, footFromRow, itemToCol].Merge = true;
            worksheet.Cells[footFromRow + 1, itemFromCol, footFromRow + 1, itemToCol].Merge = true;
            worksheet.Cells[footFromRow + 2, itemFromCol, footFromRow + 2, itemToCol].Merge = true;

            // Data
            worksheet.Cells[footFromRow, itemFromCol].Value = "Puntuacion base";
            worksheet.Cells[footFromRow + 1, itemFromCol].Value = "Puntuación obtenida";
            worksheet.Cells[footFromRow + 2, itemFromCol].Value = "Porcentaje";

            // Height
            worksheet.Row(footFromRow).Height = rowHeightl + 2;
            worksheet.Row(footFromRow + 1).Height = rowHeightl + 2;
            worksheet.Row(footFromRow + 2).Height = rowHeightl + 2;

            // Style
            worksheet.Cells[footFromRow, itemFromCol, footToRow, itemToCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[footFromRow, itemFromCol, footToRow, itemToCol].Style.Fill.BackgroundColor.SetColor(headBackgroundColor);
            worksheet.Cells[footFromRow, itemFromCol, footToRow, itemToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            worksheet.Cells[footFromRow, itemFromCol, footToRow, itemToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[footFromRow, itemFromCol, footToRow, itemToCol].Style.Font.Bold = true;

            // Border Table
            worksheet.Cells[borderFromRow, itemFromCol, footToRow, itemToCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[borderFromRow, itemFromCol, footToRow, itemToCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[borderFromRow, itemFromCol, footToRow, itemToCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[borderFromRow, itemFromCol, footToRow, itemToCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[borderFromRow, itemFromCol, footToRow, itemToCol].Style.Border.Top.Color.SetColor(borderColor);
            worksheet.Cells[borderFromRow, itemFromCol, footToRow, itemToCol].Style.Border.Left.Color.SetColor(borderColor);
            worksheet.Cells[borderFromRow, itemFromCol, footToRow, itemToCol].Style.Border.Right.Color.SetColor(borderColor);
            worksheet.Cells[borderFromRow, itemFromCol, footToRow, itemToCol].Style.Border.Bottom.Color.SetColor(borderColor);

            // Font Size
            worksheet.Cells[sheetFromRow, itemFromCol, footToRow, itemToCol].Style.Font.Size = 10;

            // Start scores
            int dayFromRow = 2;
            int dayFromCol = itemToCol + 1;

            int dayRecord = dayFromCol;

            foreach(EO_FichaExcelenciaOperativaEntidad ficha in listaFicha)
            {
                // Data Ficha
                worksheet.Cells[dayFromRow, dayRecord].Value = ficha.Fecha.ToString("dd/MM/yyyy");

                int categoryRecord = dayFromRow + 1;
                int itemRecord = dayFromRow + 2;

                int indexLocated = 0;

                int indexCategory = 1;

                foreach(EO_FichaCategoriaEntidad fichaCategoria in ficha.Categorias)
                {
                    worksheet.Cells[categoryRecord, dayFromCol, categoryRecord, dayRecord].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[categoryRecord, dayFromCol, categoryRecord, dayRecord].Style.Fill.BackgroundColor.SetColor(headBackgroundColor);
                    worksheet.Cells[categoryRecord, dayFromCol, categoryRecord, dayRecord].Value = "Puntuación";

                    int indexItem = 1;

                    foreach(EO_FichaItemEntidad fichaItem in fichaCategoria.Items)
                    {
                        if(string.IsNullOrEmpty(fichaItem.Codigo))
                        {
                            if(typeCode == 1 && indexCategory == 1)
                            {
                                if(indexItem <= 6)
                                {
                                    worksheet.Cells[itemRecord, dayRecord].Value = fichaItem.PuntuacionObtenida.ToString("0.00");
                                }
                                else
                                {
                                    worksheet.Cells[itemRecord + 1, dayRecord].Value = fichaItem.PuntuacionObtenida.ToString("0.00");
                                }

                                if(fichaCategoria.Items.Count == 11)
                                {
                                    indexLocated = 1;
                                }
                            }
                            else
                            {
                                worksheet.Cells[itemRecord, dayRecord].Value = fichaItem.PuntuacionObtenida.ToString("0.00");
                            }
                        }
                        else
                        {
                            //worksheet.Cells[itemRecord, dayRecord].Value = fichaItem.PuntuacionObtenida.ToString("0.00");

                            if (fichaItem.TipoRespuesta == 7)
                            {
                                if (!string.IsNullOrEmpty(fichaItem.Observacion))
                                {
                                    worksheet.Cells[itemRecord, dayRecord].Value = "Ver Comentario";

                                } else
                                {
                                    worksheet.Cells[itemRecord, dayRecord].Value = "Sin comentario";

                                }
                                if (!string.IsNullOrEmpty(fichaItem.Observacion))
                                {
                                    var comment = worksheet.Cells[itemRecord, dayRecord].AddComment(fichaItem.Observacion, "Nombre de Usuario");

                                    comment.AutoFit = true;
                                    comment.Font.Size = 10;
                                }
                            } else
                            {
                                worksheet.Cells[itemRecord, dayRecord].Value = fichaItem.PuntuacionObtenida.ToString("0.00");

                            }
                        }

                        indexItem++;
                        itemRecord++;
                    }

                    indexCategory++;

                    categoryRecord += indexItem;
                    itemRecord++;

                    // index located
                    categoryRecord += indexLocated;
                    itemRecord += indexLocated;
                    indexLocated = 0;
                }

                dayRecord++;
            }
            // End scores

            // Head days style
            int dayToCol = dayRecord - 1;

            worksheet.Cells[dayFromRow, dayFromCol, dayFromRow, dayToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[dayFromRow, dayFromCol, dayFromRow, dayToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[dayFromRow, dayFromCol, dayFromRow, dayToCol].Style.Font.Bold = true;
            worksheet.Cells[dayFromRow, dayFromCol, dayFromRow, dayToCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[dayFromRow, dayFromCol, dayFromRow, dayToCol].Style.Fill.BackgroundColor.SetColor(headerBackgroundColor);
            worksheet.Cells[dayFromRow, dayFromCol, dayFromRow, dayToCol].Style.Font.Color.SetColor(Color.White);

            // Start Total
            int totalFromRow = footFromRow;
            int totalRecord = dayFromCol;

            foreach(EO_FichaExcelenciaOperativaEntidad ficha in listaFicha)
            {
                // Data
                worksheet.Cells[totalFromRow, totalRecord].Value = ficha.PuntuacionBase.ToString("0.00");
                worksheet.Cells[totalFromRow + 1, totalRecord].Value = ficha.PuntuacionObtenida.ToString("0.00");
                worksheet.Cells[totalFromRow + 2, totalRecord].Value = ficha.Porcentaje;

                // Format
                worksheet.Cells[totalFromRow + 2, totalRecord].Style.Numberformat.Format = "0.0%";

                //Columns Width
                worksheet.Column(totalRecord).AutoFit();

                totalRecord++;
            }
            // End Total

            // Foot style
            int totalToRow = totalFromRow + 2;
            int totalToCol = totalRecord - 1;

            worksheet.Cells[totalFromRow, dayFromCol, totalToRow, totalToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[totalFromRow, dayFromCol, totalToRow, totalToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[totalFromRow, dayFromCol, totalToRow, totalToCol].Style.Font.Bold = true;

            // Score styles
            int scoreFromRow = dayFromRow + 1;
            int scoreToRow = totalFromRow - 1;

            worksheet.Cells[scoreFromRow, dayFromCol, scoreToRow, totalToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[scoreFromRow, dayFromCol, scoreToRow, totalToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            // Border Scores
            worksheet.Cells[borderFromRow, dayFromCol, totalToRow, totalToCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[borderFromRow, dayFromCol, totalToRow, totalToCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[borderFromRow, dayFromCol, totalToRow, totalToCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[borderFromRow, dayFromCol, totalToRow, totalToCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[borderFromRow, dayFromCol, totalToRow, totalToCol].Style.Border.Top.Color.SetColor(borderColor);
            worksheet.Cells[borderFromRow, dayFromCol, totalToRow, totalToCol].Style.Border.Left.Color.SetColor(borderColor);
            worksheet.Cells[borderFromRow, dayFromCol, totalToRow, totalToCol].Style.Border.Right.Color.SetColor(borderColor);
            worksheet.Cells[borderFromRow, dayFromCol, totalToRow, totalToCol].Style.Border.Bottom.Color.SetColor(borderColor);

            // Font Size
            worksheet.Cells[borderFromRow, dayFromCol, totalToRow, totalToCol].Style.Font.Size = 10;

            // Columns Width
            worksheet.Column(1).AutoFit();
            worksheet.Column(2).AutoFit();

            return worksheet;
        }
    }
}