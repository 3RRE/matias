using CapaEntidad.AsistenciaCliente;
using CapaEntidad.ControlAcceso.HistorialLudopata.Dto;
using CapaEntidad.Excel;
using CapaEntidad.TransaccionTarjetaCliente.Dto;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace CapaNegocio.Excel {
    public static class ExcelHelper {
        #region Cliente Sala Global
        public static void AddColumnsClienteSalaGlobal(DataTable dataTable, bool showContactInfo = true) {
            dataTable.Columns.Add("Cod. Sala");
            dataTable.Columns.Add("Sala");
            dataTable.Columns.Add("Tipo Documento");
            dataTable.Columns.Add("Nro. Documento");
            dataTable.Columns.Add("Cliente");
            if(showContactInfo) {
                dataTable.Columns.Add("Celular");
                dataTable.Columns.Add("Correo");
            }
            dataTable.Columns.Add("Procedencia");
            dataTable.Columns.Add("Tipo de registro");
            dataTable.Columns.Add("Notif. WhatsApp");
            dataTable.Columns.Add("Notif. SMS");
            dataTable.Columns.Add("Llamada");
            dataTable.Columns.Add("Notif. Email");
            dataTable.Columns.Add("F. Nacimiento");
            dataTable.Columns.Add("F. Registro");
        }

        public static void AddDataClienteSalaGlobal(DataTable dataTable, List<AST_ClienteSalaGlobal> clientes, bool showInfoContact = true) {
            foreach(AST_ClienteSalaGlobal item in clientes) {
                List<object> row = new List<object> {
                    item.codSala,
                    item.NombreSala,
                    item.TipoDocumento,
                    item.NroDoc,
                    item.NombreCliente
                };

                if(showInfoContact) {
                    row.Add(item.Celular);
                    row.Add(item.Mail);
                }

                row.AddRange(new List<object> {
                    item.Nacionalidad,
                    item.TipoRegistro,
                    item.EnviaNotificacionWhatsapp ? "Sí" : "No",
                    item.EnviaNotificacionSms ? "Sí" : "No",
                    item.LlamadaCelular ? "Sí" : "No",
                    item.EnviaNotificacionEmail ? "Sí" : "No",
                    item.FechaNacimiento.ToString("dd-MM-yyyy"),
                    item.FechaRegistro.ToString("dd-MM-yyyy hh:mm:ss")
                });

                dataTable.Rows.Add(row.ToArray());
            }
        }
        #endregion

        #region Cliente Sala Global
        public static void AddColumnsClienteSala(DataTable dataTable, bool showContactInfo = true) {
            dataTable.Columns.Add("Cod. Sala");
            dataTable.Columns.Add("Sala");
            dataTable.Columns.Add("Tipo Documento");
            dataTable.Columns.Add("Nro. Documento");
            dataTable.Columns.Add("Cliente");
            dataTable.Columns.Add("Cant. Dosis");
            if(showContactInfo) {
                dataTable.Columns.Add("Celular");
                dataTable.Columns.Add("Correo");
            }
            dataTable.Columns.Add("Notif. WhatsApp");
            dataTable.Columns.Add("Notif. SMS");
            dataTable.Columns.Add("Llamada");
            dataTable.Columns.Add("Notif. Email");
            dataTable.Columns.Add("F. Nacimiento");
            dataTable.Columns.Add("F. Registro");
        }

        public static void AddDataClienteSala(DataTable dataTable, List<AST_ClienteSala> clientes, bool showInfoContact = true) {
            foreach(AST_ClienteSala item in clientes) {
                List<object> row = new List<object> {
                    item.codSala,
                    item.NombreSala,
                    item.TipoDocumento,
                    item.NroDoc,
                    item.NombreCliente,
                    item.cantDosis
                };

                if(showInfoContact) {
                    row.Add(item.Celular);
                    row.Add(item.Mail);
                }

                row.AddRange(new List<object> {
                    item.EnviaNotificacionWhatsapp ? "Sí" : "No",
                    item.EnviaNotificacionSms ? "Sí" : "No",
                    item.LlamadaCelular ? "Sí" : "No",
                    item.EnviaNotificacionEmail ? "Sí" : "No",
                    item.FechaNacimiento.ToString("dd-MM-yyyy"),
                    item.FechaRegistro.ToString("dd-MM-yyyy hh:mm:ss")
                });

                dataTable.Rows.Add(row.ToArray());
            }
        }
        #endregion

        #region Historial Ludopata
        public static void AddColumnsHistorialLudopata(DataTable dataTable) {
            dataTable.Columns.Add("Número Documento");
            dataTable.Columns.Add("Persona");
            dataTable.Columns.Add("Clasificación");
            dataTable.Columns.Add("Tipo Registro");
            dataTable.Columns.Add("Usuario Registra");
            dataTable.Columns.Add("Fecha Registro");
        }

        public static void AddDataHistorialLudopata(DataTable dataTable, List<CAL_HistorialLudopataDto> historialLudopata) {
            foreach(CAL_HistorialLudopataDto item in historialLudopata) {
                List<object> row = new List<object> {
                    item.NumeroDocumento,
                    item.ObtenerNombreCompletoCliente(),
                    item.TipoMovimientoStr,
                    item.TipoRegistroStr,
                    item.ObtenerNombreCompletoEmpleado(),
                    item.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss")
                };
                dataTable.Rows.Add(row.ToArray());
            }
        }
        #endregion

        #region Transacciones de Tarjeta de Clientes
        public static void AddColumnsTransaccionesClienteTarjeta(DataTable dataTable) {
            dataTable.Columns.Add("ITEM", typeof(int));
            dataTable.Columns.Add("CLIENTE", typeof(string));
            dataTable.Columns.Add("DOC. IDENTIDAD", typeof(string));
            dataTable.Columns.Add("MEDIO DE PAGO", typeof(string));
            dataTable.Columns.Add("ENTIDAD EMISORA", typeof(string));
            dataTable.Columns.Add("TIPO", typeof(string));
            dataTable.Columns.Add("MONTO", typeof(decimal));
            dataTable.Columns.Add("NRO. TARJETA", typeof(string));
            dataTable.Columns.Add("FECHA", typeof(string));
            dataTable.Columns.Add("CAJA", typeof(int));
            dataTable.Columns.Add("TURNO", typeof(int));
        }

        public static void AddDataTransaccionesClienteTarjeta(DataTable dataTable, List<TTC_TransaccionDto> transaccionesCliente) {
            foreach(TTC_TransaccionDto item in transaccionesCliente) {
                List<object> row = new List<object> {
                    item.ItemVoucher,
                    item.Cliente.NombreCompleto,
                    item.Cliente.NumeroDocumento,
                    item.Tarjeta.MedioPago,
                    item.Tarjeta.EntidadEmisora,
                    item.Tarjeta.Tipo,
                    item.Monto,
                    item.Tarjeta.Numero,
                    item.FechaRegistroStr,
                    item.Caja.Numero,
                    item.Caja.Turno,
                };
                dataTable.Rows.Add(row.ToArray());
            }
        }
        #endregion

        #region Generate File
        public static ExcelPackage GenerarExcel(List<HojaExcelReporteAgrupado> sheets) {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();
            Color backgroundColor = ColorTranslator.FromHtml("#003268");

            foreach(HojaExcelReporteAgrupado sheet in sheets) {
                ExcelWorksheet workSheet = excel.Workbook.Worksheets.Add(sheet.Nombre);
                workSheet.TabColor = ColorTranslator.FromHtml(sheet.Color);

                int startColumn = 2;
                int endColumn = startColumn + sheet.Data.Columns.Count - 1;
                int startRow = 2;

                #region MetaData
                int rowMetaData = startRow;
                foreach(string data in sheet.MetaData) {
                    workSheet.Cells[rowMetaData, startColumn, rowMetaData, endColumn].Merge = true;
                    workSheet.Cells[rowMetaData, startColumn, rowMetaData, endColumn].Style.Font.Bold = true;
                    workSheet.Cells[rowMetaData, startColumn, rowMetaData, endColumn].Style.Font.Color.SetColor(Color.Gray);
                    workSheet.Row(rowMetaData).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    workSheet.Row(rowMetaData).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    workSheet.Cells[rowMetaData, startColumn].Value = data;
                    rowMetaData++;
                }
                #endregion

                #region Titulo
                int rowTitle = rowMetaData;
                if(!string.IsNullOrWhiteSpace(sheet.Titulo)) {
                    rowTitle++;
                    workSheet.Row(rowTitle).Height = 35;
                    workSheet.Row(rowTitle).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(rowTitle).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    workSheet.Row(rowTitle).Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(68, 84, 106));
                    workSheet.Row(rowTitle).Style.Font.Size = 20;
                    workSheet.Row(rowTitle).Style.Font.Bold = true;

                    workSheet.Cells[rowTitle, startColumn, rowTitle, endColumn].Merge = true;
                    workSheet.Cells[rowTitle, startColumn].Value = sheet.Titulo;
                    rowTitle++;
                }
                #endregion

                #region Table
                int rowTable = rowTitle + 1;
                ExcelRangeBase startCell = workSheet.Cells[rowTable, startColumn].LoadFromDataTable(sheet.Data, true);
                ExcelRange tableRange = workSheet.Cells[rowTable, startColumn, startCell.End.Row, startCell.End.Column];
                ExcelTable table = workSheet.Tables.Add(tableRange, $"Table_{workSheet.Name.Replace(" ", "_")}");
                table.TableStyle = TableStyles.Light9;
                if(sheet.Data.Rows.Count > 0) {
                    table.ShowHeader = true;
                    table.ShowFilter = true;
                    tableRange.AutoFitColumns();
                }

                if(sheet.Data.Rows.Count > 0) {
                    int columnIndex = startColumn;
                    foreach(DataColumn column in sheet.Data.Columns) {
                        ExcelRange columnCells = workSheet.Cells[rowTable + 1, columnIndex, startCell.End.Row, columnIndex];
                        Type type = column.DataType;
                        if(type == typeof(DateTime)) {
                            columnCells.Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";
                        } else if(column.DataType == typeof(double) || column.DataType == typeof(decimal)) {
                            columnCells.Style.Numberformat.Format = "#,##0.00";
                        } else if(type == typeof(int)) {
                            columnCells.Style.Numberformat.Format = "#,##0";
                        } else if(type == typeof(string)) {
                            columnCells.Style.Numberformat.Format = "@";
                        }
                        columnIndex++;
                    }
                }
                #endregion

                #region Style Header Table
                workSheet.Row(rowTable).Height = 20;
                workSheet.Row(rowTable).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(rowTable).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Row(rowTable).Style.Font.Bold = true;

                workSheet.Cells[rowTable, startColumn, rowTable, endColumn].Style.Font.Bold = true;
                workSheet.Cells[rowTable, startColumn, rowTable, endColumn].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[rowTable, startColumn, rowTable, endColumn].Style.Fill.BackgroundColor.SetColor(backgroundColor);
                workSheet.Cells[rowTable, startColumn, rowTable, endColumn].Style.Font.Color.SetColor(Color.White);
                #endregion

                #region Footer Table
                int rowFooter = rowTable + sheet.Data.Rows.Count + 1;
                workSheet.Cells[rowFooter, startColumn, rowFooter, endColumn].Merge = true;
                workSheet.Cells[rowFooter, startColumn, rowFooter, endColumn].Style.Font.Bold = true;
                workSheet.Cells[rowFooter, startColumn, rowFooter, endColumn].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[rowFooter, startColumn, rowFooter, endColumn].Style.Fill.BackgroundColor.SetColor(backgroundColor);
                workSheet.Cells[rowFooter, startColumn, rowFooter, endColumn].Style.Font.Color.SetColor(Color.White);
                workSheet.Cells[rowFooter, startColumn, rowFooter, endColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[rowFooter, startColumn].Value = $"Total: {sheet.Data.Rows.Count} registros";
                #endregion

                #region Configuration Sheet
                for(int i = startColumn; i <= endColumn; i++) {
                    workSheet.Column(i).AutoFit();
                }
                #endregion
            }

            return excel;
        }
        #endregion                
    }
}
