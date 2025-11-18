using CapaEntidad.Campañas;
using CapaEntidad.TITO;
using CapaNegocio.Campaña;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace CapaPresentacion.Utilitarios {
    public class AuditoriaTITOPromoHelper
    {
        private readonly int _salaId;
        private readonly List<Reporte_Detalle_TITO_Caja> _listTitos;
        private List<CMP_TicketEntidad> _listTickets;
        private readonly CMP_TicketBL _CMP_TicketBL = new CMP_TicketBL();

        public AuditoriaTITOPromoHelper(int salaId, List<Reporte_Detalle_TITO_Caja> listTitos)
        {
            _salaId = salaId;
            _listTitos = listTitos;
            _listTickets = GetTicketsEnlazados();

            UpdateTicketsDiff();
        }

        public List<CMP_TicketEntidad> GetTicketsEnlazados()
        {
            List<int> D00H_Items = _listTitos.Where(item => item.D00H_Item > 0).Select(item => item.D00H_Item).ToList();

            return _CMP_TicketBL.GetTicketsEnlazadosItemsV2(_salaId, D00H_Items);
        }

        public List<CMP_TicketEntidad> TicketsLinked()
        {
            List<CMP_TicketEntidad> listLinkedTickets = _listTickets.Where(ticket => _listTitos.Any(tito => 
                Convert.ToDateTime(tito.FechaApertura) == ticket.FechaApertura && tito.Ticket == ticket.nroticket && decimal.ToDouble(tito.Monto_Dinero) == ticket.monto
            )).ToList();

            return listLinkedTickets;
        }

        public List<Reporte_Detalle_TITO_Caja> TicketsDiff()
        {
            List<Reporte_Detalle_TITO_Caja> listTitoDiff = _listTitos.Where(tito => !_listTickets.Any(ticket => 
                ticket.FechaApertura == Convert.ToDateTime(tito.FechaApertura) && ticket.nroticket == tito.Ticket && ticket.monto == decimal.ToDouble(tito.Monto_Dinero)
            )).ToList();

            return listTitoDiff;
        }

        public IEnumerable<dynamic> GroupTicketsBox()
        {
            // data group Generados en Caja
            var dataBox = _listTitos.GroupBy(group => Convert.ToDateTime(group.FechaApertura).ToString("dd/MM/yyyy")).Select(group => new {
                transactionDate = group.Key,
                quantity = group.Count(),
                amount = group.Sum(item => item.Monto_Dinero),
                items = group.ToList()
            }).ToList();

            return dataBox;
        }

        public IEnumerable<dynamic> GroupTicketsIas()
        {
            List<CMP_TicketEntidad> listTicketsLinked = TicketsLinked();

            // data group Enlazados en IAS
            var dataIAS = listTicketsLinked.GroupBy(group => group.FechaApertura.ToString("dd/MM/yyyy")).Select(group => new {
                transactionDate = group.Key,
                quantity = group.Count(),
                amount = group.Sum(item => item.monto),
                items = group.ToList()
            }).ToList();

            return dataIAS;
        }

        public IEnumerable<dynamic> GroupTicketsDiff()
        {
            List<Reporte_Detalle_TITO_Caja> listTicketsDiff = TicketsDiff();

            // data group Diferencia
            var dataDiff = listTicketsDiff.GroupBy(group => Convert.ToDateTime(group.FechaApertura).ToString("dd/MM/yyyy")).Select(group => new {
                transactionDate = group.Key,
                quantity = group.Count(),
                amount = group.Sum(item => item.Monto_Dinero),
                items = group.ToList()
            }).ToList();

            return dataDiff;
        }

        public IEnumerable<dynamic> Titos()
        {
            IEnumerable<dynamic> dataBox = GroupTicketsBox();

            // data Auditoria TITO Promocionales
            IEnumerable<dynamic> dataTitos = dataBox.Select(item => new {
                transactionDate = item.transactionDate,

                quantityBox = item.quantity,
                amountBox = item.amount,
                itemsBox = item.items,

                quantityIas = GetDataIas(item.transactionDate).quantity,
                amountIas = GetDataIas(item.transactionDate).amount,
                itemsIas = GetDataIas(item.transactionDate).items,

                quantityDiff = GetDataDiff(item.transactionDate).quantity,
                amountDiff = GetDataDiff(item.transactionDate).amount,
                itemsDiff = GetDataDiff(item.transactionDate).items

            }).ToList();

            return dataTitos;
        }

        private object GetDataIas(dynamic transactionDate)
        {
            IEnumerable<dynamic> dataIAS = GroupTicketsIas();

            object objectIas = dataIAS.Where(ias => ias.transactionDate == transactionDate).Select(ias => new {
                ias.quantity,
                ias.amount,
                ias.items
            }).FirstOrDefault();

            return objectIas ?? new {
                quantity = 0,
                amount = 0,
                items = new List<dynamic>()
            };
        }

        private object GetDataDiff(dynamic transactionDate)
        {
            IEnumerable<dynamic> dataDiff = GroupTicketsDiff();

            object objectDiff = dataDiff.Where(diff => diff.transactionDate == transactionDate).Select(diff => new {
                diff.quantity,
                diff.amount,
                diff.items
            }).FirstOrDefault();

            return objectDiff ?? new {
                quantity = 0,
                amount = 0,
                items = new List<dynamic>()
            };
        }

        private void UpdateTicketsDiff()
        {
            List<Reporte_Detalle_TITO_Caja> listTicketsDiff = TicketsDiff();

            listTicketsDiff = listTicketsDiff.Where(diff => _listTickets.Any(ticket => ticket.item == diff.D00H_Item)).ToList();

            if (listTicketsDiff.Count > 0)
            {
                bool updated = _CMP_TicketBL.ATPActualizarTicketsDiff(_salaId, listTicketsDiff);

                if (updated)
                {
                    _listTickets = GetTicketsEnlazados();
                }
            }
        }

        // Statics
        public static MemoryStream ExcelFisico(IEnumerable<dynamic> listTitos, dynamic parameters)
        {
            MemoryStream memoryStream = new MemoryStream();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using(ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet excelWorksheetATP = excelPackage.Workbook.Worksheets.Add("ATP");
                WorksheetATP(excelWorksheetATP, listTitos, parameters);

                foreach(dynamic tito in listTitos)
                {
                    DateTime transactionDate = Convert.ToDateTime(tito.transactionDate);

                    ExcelWorksheet excelWorksheetTITO = excelPackage.Workbook.Worksheets.Add($"ATP {transactionDate.ToString("dd-MM-yyyy")}");
                    WorksheetTITO(excelWorksheetTITO, tito, parameters);
                }

                // Save Excel
                excelPackage.SaveAs(memoryStream);
            }

            return memoryStream;
        }

        // Workshets
        private static ExcelWorksheet WorksheetATP(ExcelWorksheet worksheet, IEnumerable<dynamic> listTitos, dynamic parameters)
        {
            // Parameters
            string roomName = parameters.roomName;
            DateTime fromDate = Convert.ToDateTime(parameters.fromDate);
            DateTime toDate = Convert.ToDateTime(parameters.toDate);

            // Row height
            double rowHeightlHeader = 20;

            // Colors
            Color headBackgroundColor = ColorTranslator.FromHtml("#003268");
            Color borderColorHead = ColorTranslator.FromHtml("#074B88");
            Color borderColorBody = ColorTranslator.FromHtml("#747474");

            // Header
            worksheet.Row(2).Height = rowHeightlHeader;
            worksheet.Row(3).Height = rowHeightlHeader;
            worksheet.Row(4).Height = rowHeightlHeader;
            worksheet.Cells[2, 2, 3, 9].Merge = true;
            worksheet.Cells[4, 2, 4, 9].Merge = true;
            worksheet.Cells[2, 2, 4, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[2, 2, 4, 9].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[2, 2].Style.Font.Size = 14;
            worksheet.Cells[4, 2].Style.Font.Size = 12;
            worksheet.Cells[2, 2].Style.Font.Bold = true;
            worksheet.Cells[4, 2].Style.Font.Italic = true;

            worksheet.Cells[2, 2].Value = "Auditoría Titos Promocionales";
            worksheet.Cells[4, 2].Value = $"{roomName} del {fromDate.ToString("dd/MM/yyyy")} al {toDate.ToString("dd/MM/yyyy")}";

            // Head
            int bodyFromRow = 6;
            int bodyFromCol = 2;
            int bodyToCol = 9;
            int headColFromRow = bodyFromRow + 1;

            // Head style
            worksheet.Row(bodyFromRow).Height = rowHeightlHeader;
            worksheet.Row(headColFromRow).Height = rowHeightlHeader + 5;
            worksheet.Cells[bodyFromRow, 2, bodyFromRow, 3].Merge = true;
            worksheet.Cells[bodyFromRow, 4, bodyFromRow, 5].Merge = true;
            worksheet.Cells[bodyFromRow, 6, bodyFromRow, 7].Merge = true;
            worksheet.Cells[bodyFromRow, 8, bodyFromRow, 9].Merge = true;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Font.Bold = true;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Fill.BackgroundColor.SetColor(headBackgroundColor);
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Font.Color.SetColor(Color.White);

            // Borders Table Head
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Top.Color.SetColor(borderColorHead);
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Left.Color.SetColor(borderColorHead);
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Right.Color.SetColor(borderColorHead);
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Bottom.Color.SetColor(borderColorHead);

            worksheet.Cells[bodyFromRow, 4].Value = "Generados en Caja";
            worksheet.Cells[bodyFromRow, 6].Value = "Enlazados en IAS";
            worksheet.Cells[bodyFromRow, 8].Value = "Diferencia";

            worksheet.Cells[headColFromRow, 2].Value = "Item";
            worksheet.Cells[headColFromRow, 3].Value = "Fecha Operación";
            worksheet.Cells[headColFromRow, 4].Value = "Cantidad";
            worksheet.Cells[headColFromRow, 5].Value = "Importe Soles";
            worksheet.Cells[headColFromRow, 6].Value = "Cantidad";
            worksheet.Cells[headColFromRow, 7].Value = "Importe Soles";
            worksheet.Cells[headColFromRow, 8].Value = "Cantidad";
            worksheet.Cells[headColFromRow, 9].Value = "Importe Soles";

            // Body
            int totalRecords = listTitos.Count();
            int recordRow = headColFromRow + 1;
            int recordIndex = 1;

            foreach(dynamic tito in listTitos)
            {
                worksheet.Row(recordRow).Height = rowHeightlHeader;
                worksheet.Cells[recordRow, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[recordRow, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                // Data
                DateTime transactionDate = Convert.ToDateTime(tito.transactionDate);

                worksheet.Cells[recordRow, 2].Value = recordIndex;
                worksheet.Cells[recordRow, 3].Value = tito.transactionDate;
                worksheet.Cells[recordRow, 4].Value = tito.quantityBox;
                worksheet.Cells[recordRow, 5].Value = tito.amountBox.ToString("#,##0.00");
                worksheet.Cells[recordRow, 6].Value = tito.quantityIas;
                worksheet.Cells[recordRow, 7].Value = tito.amountIas.ToString("#,##0.00");
                worksheet.Cells[recordRow, 8].Value = tito.quantityDiff;
                worksheet.Cells[recordRow, 9].Value = tito.amountDiff.ToString("#,##0.00");
                worksheet.Cells[recordRow, 10].Value = "Ver Detalle";

                // Formula
                worksheet.Cells[recordRow, 10].Hyperlink = new Uri($"#'ATP {transactionDate.ToString("dd-MM-yyyy")}'!A1", UriKind.Relative);

                recordRow++;
                recordIndex++;
            }

            int recordFromRow = headColFromRow + 1;
            int bodyToRow = headColFromRow + totalRecords;

            if(totalRecords > 0)
            {
                // Borders Table Body
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Top.Color.SetColor(borderColorBody);
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Left.Color.SetColor(borderColorBody);
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Right.Color.SetColor(borderColorBody);
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Bottom.Color.SetColor(borderColorBody);

                // Alignment
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                // Filters
                worksheet.Cells[headColFromRow, bodyFromCol, bodyToRow, bodyToCol].AutoFilter = true;
            }

            // Columns Width
            worksheet.Column(2).AutoFit();
            worksheet.Column(3).Width = 20;
            worksheet.Column(4).Width = 20;
            worksheet.Column(5).Width = 20;
            worksheet.Column(6).Width = 20;
            worksheet.Column(7).Width = 20;
            worksheet.Column(8).Width = 20;
            worksheet.Column(9).Width = 20;
            worksheet.Column(10).Width = 15;

            return worksheet;
        }

        private static ExcelWorksheet WorksheetTITO(ExcelWorksheet worksheet, dynamic tito, dynamic parameters)
        {
            // Parameters
            string roomName = parameters.roomName;
            DateTime transactionDate = Convert.ToDateTime(tito.transactionDate);

            // Row height
            double rowHeightlHeader = 20;

            // Colors
            Color headBackgroundColorBox = ColorTranslator.FromHtml("#1762A3");
            Color borderColorHeadBox = ColorTranslator.FromHtml("#86AED0");
            Color headBackgroundColorIas = ColorTranslator.FromHtml("#52AF52");
            Color borderColorHeadIas = ColorTranslator.FromHtml("#A5D6A5");
            Color headBackgroundColorDiff = ColorTranslator.FromHtml("#D4881B");
            Color borderColorHeadDiff = ColorTranslator.FromHtml("#EAC288");
            Color borderColorBody = ColorTranslator.FromHtml("#747474");

            // Header
            worksheet.Row(2).Height = rowHeightlHeader;
            worksheet.Row(3).Height = rowHeightlHeader;
            worksheet.Row(4).Height = rowHeightlHeader;
            worksheet.Cells[2, 2, 3, 9].Merge = true;
            worksheet.Cells[4, 2, 4, 9].Merge = true;
            worksheet.Cells[2, 2, 4, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[2, 2, 4, 9].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[2, 2].Style.Font.Size = 14;
            worksheet.Cells[4, 2].Style.Font.Size = 12;
            worksheet.Cells[2, 2].Style.Font.Bold = true;
            worksheet.Cells[4, 2].Style.Font.Italic = true;

            worksheet.Cells[2, 2].Value = "Auditoría Titos Promocionales";
            worksheet.Cells[4, 2].Value = $"{roomName} {transactionDate.ToString("dd/MM/yyyy")}";

            // GENERADOS EN CAJA

            // Head
            int bodyFromRow = 6;
            int bodyFromCol = 2;
            int bodyToCol = 9;
            int headColFromRow = bodyFromRow + 1;

            // Head style
            worksheet.Row(bodyFromRow).Height = rowHeightlHeader;
            worksheet.Row(headColFromRow).Height = rowHeightlHeader + 5;

            worksheet.Cells[bodyFromRow, bodyFromCol, bodyFromRow, bodyToCol].Merge = true;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Font.Bold = true;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Fill.BackgroundColor.SetColor(headBackgroundColorBox);
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Font.Color.SetColor(Color.White);

            // Borders Table Head
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Top.Color.SetColor(borderColorHeadBox);
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Left.Color.SetColor(borderColorHeadBox);
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Right.Color.SetColor(borderColorHeadBox);
            worksheet.Cells[bodyFromRow, bodyFromCol, headColFromRow, bodyToCol].Style.Border.Bottom.Color.SetColor(borderColorHeadBox);

            worksheet.Cells[bodyFromRow, 2].Value = "Generados en Caja";

            worksheet.Cells[headColFromRow, 2].Value = "Fecha Apertura";
            worksheet.Cells[headColFromRow, 3].Value = "Fecha Ticket";
            worksheet.Cells[headColFromRow, 4].Value = "Nro";
            worksheet.Cells[headColFromRow, 5].Value = "Monto";
            worksheet.Cells[headColFromRow, 6].Value = "Cliente";
            worksheet.Cells[headColFromRow, 7].Value = "Nro Doc";
            worksheet.Cells[headColFromRow, 8].Value = "Teléfono";
            worksheet.Cells[headColFromRow, 9].Value = "Usuario Reg";

            // Body
            int totalRecords = tito.itemsBox.Count;
            int recordRow = headColFromRow + 1;

            foreach(dynamic item in tito.itemsBox)
            {
                worksheet.Row(recordRow).Height = rowHeightlHeader;

                // Data
                DateTime fechaApertura = Convert.ToDateTime(item.FechaApertura);
                DateTime fechaProcesoInicio = Convert.ToDateTime(item.Fecha_Proceso_Inicio);

                worksheet.Cells[recordRow, 2].Value = fechaApertura.ToString("dd/MM/yyyy HH:mm:ss");
                worksheet.Cells[recordRow, 3].Value = fechaProcesoInicio.ToString("dd/MM/yyyy HH:mm:ss");
                worksheet.Cells[recordRow, 4].Value = item.Ticket;
                worksheet.Cells[recordRow, 5].Value = item.Monto_Dinero.ToString("#,##0.00");
                worksheet.Cells[recordRow, 6].Value = item.Cliente;
                worksheet.Cells[recordRow, 7].Value = item.ClienteDni;
                worksheet.Cells[recordRow, 8].Value = item.ClienteTelefono;
                worksheet.Cells[recordRow, 9].Value = item.Personal;

                recordRow++;
            }

            int recordFromRow = headColFromRow + 1;
            int bodyToRow = headColFromRow + totalRecords;

            if(totalRecords > 0)
            {
                // Borders Table Body
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Top.Color.SetColor(borderColorBody);
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Left.Color.SetColor(borderColorBody);
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Right.Color.SetColor(borderColorBody);
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.Border.Bottom.Color.SetColor(borderColorBody);

                // Alignment
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[recordFromRow, bodyFromCol, bodyToRow, bodyToCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            // Columns Width
            worksheet.Column(1).Width = 2;
            worksheet.Column(2).AutoFit();
            worksheet.Column(3).AutoFit();
            worksheet.Column(4).AutoFit();
            worksheet.Column(5).AutoFit();
            worksheet.Column(6).AutoFit();
            worksheet.Column(7).AutoFit();
            worksheet.Column(8).AutoFit();
            worksheet.Column(9).AutoFit();

            // END GENERADOS EN CAJA

            // ENLAZADOS EN IAS

            Dictionary<int, string> instancesCollection = new Dictionary<int, string>
            {
                { 1, "Procedimiento Manual" },
                { 2, "Instancia por Auditoria" }
            };

            // Head
            int bodyFromRowIas = 6;
            int bodyFromColIas = 11;
            int bodyToColIas = 20;
            int headColFromRowIas = bodyFromRowIas + 1;

            // Head style
            worksheet.Cells[bodyFromRowIas, bodyFromColIas, bodyFromRowIas, bodyToColIas].Merge = true;
            worksheet.Cells[bodyFromRowIas, bodyFromColIas, headColFromRowIas, bodyToColIas].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[bodyFromRowIas, bodyFromColIas, headColFromRowIas, bodyToColIas].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[bodyFromRowIas, bodyFromColIas, headColFromRowIas, bodyToColIas].Style.Font.Bold = true;
            worksheet.Cells[bodyFromRowIas, bodyFromColIas, headColFromRowIas, bodyToColIas].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[bodyFromRowIas, bodyFromColIas, headColFromRowIas, bodyToColIas].Style.Fill.BackgroundColor.SetColor(headBackgroundColorIas);
            worksheet.Cells[bodyFromRowIas, bodyFromColIas, headColFromRowIas, bodyToColIas].Style.Font.Color.SetColor(Color.White);

            // Borders Table Head
            worksheet.Cells[bodyFromRowIas, bodyFromColIas, headColFromRowIas, bodyToColIas].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRowIas, bodyFromColIas, headColFromRowIas, bodyToColIas].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRowIas, bodyFromColIas, headColFromRowIas, bodyToColIas].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRowIas, bodyFromColIas, headColFromRowIas, bodyToColIas].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRowIas, bodyFromColIas, headColFromRowIas, bodyToColIas].Style.Border.Top.Color.SetColor(borderColorHeadIas);
            worksheet.Cells[bodyFromRowIas, bodyFromColIas, headColFromRowIas, bodyToColIas].Style.Border.Left.Color.SetColor(borderColorHeadIas);
            worksheet.Cells[bodyFromRowIas, bodyFromColIas, headColFromRowIas, bodyToColIas].Style.Border.Right.Color.SetColor(borderColorHeadIas);
            worksheet.Cells[bodyFromRowIas, bodyFromColIas, headColFromRowIas, bodyToColIas].Style.Border.Bottom.Color.SetColor(borderColorHeadIas);

            worksheet.Cells[bodyFromRowIas, 11].Value = "Enlazados en IAS";

            worksheet.Cells[headColFromRowIas, 11].Value = "Fecha Apertura";
            worksheet.Cells[headColFromRowIas, 12].Value = "Fecha Ticket";
            worksheet.Cells[headColFromRowIas, 13].Value = "Nro";
            worksheet.Cells[headColFromRowIas, 14].Value = "Monto";
            worksheet.Cells[headColFromRowIas, 15].Value = "Instancia";
            worksheet.Cells[headColFromRowIas, 16].Value = "Campaña";
            worksheet.Cells[headColFromRowIas, 17].Value = "Cliente";
            worksheet.Cells[headColFromRowIas, 18].Value = "Nro Doc";
            worksheet.Cells[headColFromRowIas, 19].Value = "Teléfono";
            worksheet.Cells[headColFromRowIas, 20].Value = "Usuario Reg";

            // Body
            int totalRecordsIas = tito.itemsIas.Count;
            int recordRowIas = headColFromRowIas + 1;

            foreach(dynamic item in tito.itemsIas)
            {
                // Data
                worksheet.Cells[recordRowIas, 11].Value = item.FechaApertura.ToString("dd/MM/yyyy HH:mm:ss");
                worksheet.Cells[recordRowIas, 12].Value = item.fecharegsala.ToString("dd/MM/yyyy HH:mm:ss");
                worksheet.Cells[recordRowIas, 13].Value = item.nroticket;
                worksheet.Cells[recordRowIas, 14].Value = item.monto.ToString("#,##0.00");
                worksheet.Cells[recordRowIas, 15].Value = instancesCollection.ContainsKey(item.estado) ? instancesCollection[item.estado] : "";
                worksheet.Cells[recordRowIas, 16].Value = item.Campania.nombre;
                worksheet.Cells[recordRowIas, 17].Value = item.NombreCompleto;
                worksheet.Cells[recordRowIas, 18].Value = item.NroDoc;
                worksheet.Cells[recordRowIas, 19].Value = item.Celular1;
                worksheet.Cells[recordRowIas, 20].Value = item.nombre_usuario;

                recordRowIas++;
            }

            int recordFromRowIas = headColFromRowIas + 1;
            int bodyToRowIas = headColFromRowIas + totalRecordsIas;

            if(totalRecordsIas > 0)
            {
                // Borders Table Body
                worksheet.Cells[recordFromRowIas, bodyFromColIas, bodyToRowIas, bodyToColIas].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[recordFromRowIas, bodyFromColIas, bodyToRowIas, bodyToColIas].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[recordFromRowIas, bodyFromColIas, bodyToRowIas, bodyToColIas].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[recordFromRowIas, bodyFromColIas, bodyToRowIas, bodyToColIas].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[recordFromRowIas, bodyFromColIas, bodyToRowIas, bodyToColIas].Style.Border.Top.Color.SetColor(borderColorBody);
                worksheet.Cells[recordFromRowIas, bodyFromColIas, bodyToRowIas, bodyToColIas].Style.Border.Left.Color.SetColor(borderColorBody);
                worksheet.Cells[recordFromRowIas, bodyFromColIas, bodyToRowIas, bodyToColIas].Style.Border.Right.Color.SetColor(borderColorBody);
                worksheet.Cells[recordFromRowIas, bodyFromColIas, bodyToRowIas, bodyToColIas].Style.Border.Bottom.Color.SetColor(borderColorBody);

                // Alignment
                worksheet.Cells[recordFromRowIas, bodyFromColIas, bodyToRowIas, bodyToColIas].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[recordFromRowIas, bodyFromColIas, bodyToRowIas, bodyToColIas].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            // Columns Width
            worksheet.Column(10).Width = 2;
            worksheet.Column(11).AutoFit();
            worksheet.Column(12).AutoFit();
            worksheet.Column(13).AutoFit();
            worksheet.Column(14).AutoFit();
            worksheet.Column(15).AutoFit();
            worksheet.Column(16).AutoFit();
            worksheet.Column(17).AutoFit();
            worksheet.Column(18).AutoFit();
            worksheet.Column(19).AutoFit();
            worksheet.Column(20).AutoFit();

            // END ENLAZADOS EN IAS

            // DIFERENCIA

            // Head
            int bodyFromRowDiff = 6;
            int bodyFromColDiff = 22;
            int bodyToColDiff = 29;
            int headColFromRowDiff = bodyFromRowDiff + 1;

            // Head style
            worksheet.Cells[bodyFromRowDiff, bodyFromColDiff, bodyFromRowDiff, bodyToColDiff].Merge = true;
            worksheet.Cells[bodyFromRowDiff, bodyFromColDiff, headColFromRowDiff, bodyToColDiff].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[bodyFromRowDiff, bodyFromColDiff, headColFromRowDiff, bodyToColDiff].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[bodyFromRowDiff, bodyFromColDiff, headColFromRowDiff, bodyToColDiff].Style.Font.Bold = true;
            worksheet.Cells[bodyFromRowDiff, bodyFromColDiff, headColFromRowDiff, bodyToColDiff].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[bodyFromRowDiff, bodyFromColDiff, headColFromRowDiff, bodyToColDiff].Style.Fill.BackgroundColor.SetColor(headBackgroundColorDiff);
            worksheet.Cells[bodyFromRowDiff, bodyFromColDiff, headColFromRowDiff, bodyToColDiff].Style.Font.Color.SetColor(Color.White);

            // Borders Table Head
            worksheet.Cells[bodyFromRowDiff, bodyFromColDiff, headColFromRowDiff, bodyToColDiff].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRowDiff, bodyFromColDiff, headColFromRowDiff, bodyToColDiff].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRowDiff, bodyFromColDiff, headColFromRowDiff, bodyToColDiff].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRowDiff, bodyFromColDiff, headColFromRowDiff, bodyToColDiff].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[bodyFromRowDiff, bodyFromColDiff, headColFromRowDiff, bodyToColDiff].Style.Border.Top.Color.SetColor(borderColorHeadDiff);
            worksheet.Cells[bodyFromRowDiff, bodyFromColDiff, headColFromRowDiff, bodyToColDiff].Style.Border.Left.Color.SetColor(borderColorHeadDiff);
            worksheet.Cells[bodyFromRowDiff, bodyFromColDiff, headColFromRowDiff, bodyToColDiff].Style.Border.Right.Color.SetColor(borderColorHeadDiff);
            worksheet.Cells[bodyFromRowDiff, bodyFromColDiff, headColFromRowDiff, bodyToColDiff].Style.Border.Bottom.Color.SetColor(borderColorHeadDiff);

            worksheet.Cells[bodyFromRowDiff, 22].Value = "Diferencia";

            worksheet.Cells[headColFromRowDiff, 22].Value = "Fecha Apertura";
            worksheet.Cells[headColFromRowDiff, 23].Value = "Fecha Ticket";
            worksheet.Cells[headColFromRowDiff, 24].Value = "Nro";
            worksheet.Cells[headColFromRowDiff, 25].Value = "Monto";
            worksheet.Cells[headColFromRowDiff, 26].Value = "Cliente";
            worksheet.Cells[headColFromRowDiff, 27].Value = "Nro Doc";
            worksheet.Cells[headColFromRowDiff, 28].Value = "Teléfono";
            worksheet.Cells[headColFromRowDiff, 29].Value = "Usuario Reg";

            // Body
            int totalRecordsDiff = tito.itemsDiff.Count;
            int recordRowDiff = headColFromRowDiff + 1;

            foreach(dynamic item in tito.itemsDiff)
            {
                // Data
                DateTime fechaApertura = Convert.ToDateTime(item.FechaApertura);
                DateTime fechaProcesoInicio = Convert.ToDateTime(item.Fecha_Proceso_Inicio);

                worksheet.Cells[recordRowDiff, 22].Value = fechaApertura.ToString("dd/MM/yyyy HH:mm:ss");
                worksheet.Cells[recordRowDiff, 23].Value = fechaProcesoInicio.ToString("dd/MM/yyyy HH:mm:ss");
                worksheet.Cells[recordRowDiff, 24].Value = item.Ticket;
                worksheet.Cells[recordRowDiff, 25].Value = item.Monto_Dinero.ToString("#,##0.00");
                worksheet.Cells[recordRowDiff, 26].Value = item.Cliente;
                worksheet.Cells[recordRowDiff, 27].Value = item.ClienteDni;
                worksheet.Cells[recordRowDiff, 28].Value = item.ClienteTelefono;
                worksheet.Cells[recordRowDiff, 29].Value = item.Personal;

                recordRowDiff++;
            }

            int recordFromRowDiff = headColFromRowDiff + 1;
            int bodyToRowDiff = headColFromRowDiff + totalRecordsDiff;

            if(totalRecordsDiff > 0)
            {
                // Borders Table Body
                worksheet.Cells[recordFromRowDiff, bodyFromColDiff, bodyToRowDiff, bodyToColDiff].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[recordFromRowDiff, bodyFromColDiff, bodyToRowDiff, bodyToColDiff].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[recordFromRowDiff, bodyFromColDiff, bodyToRowDiff, bodyToColDiff].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[recordFromRowDiff, bodyFromColDiff, bodyToRowDiff, bodyToColDiff].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[recordFromRowDiff, bodyFromColDiff, bodyToRowDiff, bodyToColDiff].Style.Border.Top.Color.SetColor(borderColorBody);
                worksheet.Cells[recordFromRowDiff, bodyFromColDiff, bodyToRowDiff, bodyToColDiff].Style.Border.Left.Color.SetColor(borderColorBody);
                worksheet.Cells[recordFromRowDiff, bodyFromColDiff, bodyToRowDiff, bodyToColDiff].Style.Border.Right.Color.SetColor(borderColorBody);
                worksheet.Cells[recordFromRowDiff, bodyFromColDiff, bodyToRowDiff, bodyToColDiff].Style.Border.Bottom.Color.SetColor(borderColorBody);

                // Alignment
                worksheet.Cells[recordFromRowDiff, bodyFromColDiff, bodyToRowDiff, bodyToColDiff].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[recordFromRowDiff, bodyFromColDiff, bodyToRowDiff, bodyToColDiff].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            // Columns Width
            worksheet.Column(21).Width = 2;
            worksheet.Column(22).AutoFit();
            worksheet.Column(23).AutoFit();
            worksheet.Column(24).AutoFit();
            worksheet.Column(25).AutoFit();
            worksheet.Column(26).AutoFit();
            worksheet.Column(27).AutoFit();
            worksheet.Column(28).AutoFit();
            worksheet.Column(29).AutoFit();

            // END DIFERENCIA

            return worksheet;
        }
    }
}