using CapaEntidad;
using CapaEntidad.AsistenciaCliente;
using CapaEntidad.Campañas;
using CapaNegocio;
using CapaNegocio.AsistenciaCliente;
using CapaNegocio.Campaña;
using CapaPresentacion.Reports;
using CapaPresentacion.Utilitarios;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CapaPresentacion.Controllers.Campaña
{
    [seguridad]
    public class CampaniaReporteController : Controller
    {
        private CMP_CampañaBL campaniabl = new CMP_CampañaBL();
        private CMP_TicketBL ticketbl = new CMP_TicketBL();
        private CMP_ClienteBL clientebl = new CMP_ClienteBL();
        private SalaBL salaBl = new SalaBL();
        private SEG_UsuarioBL usuariobl = new SEG_UsuarioBL();
        private CMP_CuponesGeneradosBL cuponesBL = new CMP_CuponesGeneradosBL();
        private CMP_DetalleCuponesImpresosBL detalleCuponesImpresosBL = new CMP_DetalleCuponesImpresosBL();
        private AST_ClienteBL ast_ClienteBL = new AST_ClienteBL();
        #region Campania Promocion
        public ActionResult ReporteCampaniaPromocionCliente()
        {
            return View("~/Views/Campania/ReporteCampaniaPromocionClienteVista.cshtml");
        }
        [HttpPost]
        public JsonResult ListarCampaniasxFechasJson(int[] codsala, DateTime fechaini, DateTime fechafin, int tipo = 0)
        {
            var errormensaje = "";
            var lista = new List<CMP_CampañaEntidad>();
            int cantElementos = (codsala == null) ? 0 : codsala.Length;
            var strElementos = String.Empty;
            var strElementos_ = String.Empty;
            try
            {

                if (cantElementos > 0)
                {
                    strElementos = " c.[sala_id] in(" + "'" + String.Join("','", codsala) + "'" + ") and ";
                }
                lista = campaniabl.GetListadoCampaniaxTipoyFechas(strElementos, fechaini, fechafin, tipo);
                if (tipo == 1)
                {
                    lista = lista.Where(x => x.tipo == 1).ToList();
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ReporteCampaniaPromocionExcelJson(Int64 campaniaid)
        {

            var mensaje = "";
            List<CMP_TicketEntidad> listaTickets = new List<CMP_TicketEntidad>();
            List<CMP_ClienteEntidad> listaClientes = new List<CMP_ClienteEntidad>();
            CMP_CampañaEntidad campania = new CMP_CampañaEntidad();
            SalaEntidad sala = new SalaEntidad();
            SEG_UsuarioEntidad usuario = new SEG_UsuarioEntidad();
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            try
            {
                campania = campaniabl.CampañaIdObtenerJson(campaniaid);
                listaClientes = clientebl.GetClientesCampaniaJson(campaniaid);
                listaTickets = ticketbl.CMPTicketCampañaListadoJson(campaniaid);
                if (campania.id != 0)
                {
                    usuario = usuariobl.UsuarioEmpleadoIDObtenerJson(campania.usuario_id);
                    sala = salaBl.SalaListaIdJson(campania.sala_id);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var ws = excel.Workbook.Worksheets.Add("Clientes Campaña");
                    ws.Cells["B1"].Value = "Reporte Campaña Tipo Promoción";
                    ws.Cells["B1:C1"].Style.Font.Bold = true;

                    ws.Cells["B1"].Style.Font.Size = 20;
                    ws.Cells["B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["B1:I1"].Merge = true;
                    ws.Cells["B1:I1"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells["B3"].Value = "Usuario Registro";
                    ws.Cells["B3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["C3"].Value = usuario.UsuarioNombre;
                    ws.Cells["C3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["B3"].Style.Font.Bold = true;

                    ws.Cells["D3"].Value = "Campaña";
                    ws.Cells["D3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["E3"].Value = campania.nombre;
                    ws.Cells["E3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["D3"].Style.Font.Bold = true;

                    ws.Cells["F3"].Value = "Fecha Registro";
                    ws.Cells["F3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["G3"].Value = campania.fechareg.ToString("dd/MM/yyyy hh:mm:ss A");
                    ws.Cells["G3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["F3"].Style.Font.Bold = true;

                    ws.Cells["H3"].Value = "Sala";
                    ws.Cells["H3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["I3"].Value = sala.Nombre;
                    ws.Cells["I3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["H3"].Style.Font.Bold = true;

                    ws.Cells["B4"].Value = "Fecha Inicio";
                    ws.Cells["B4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["C4"].Value = campania.fechaini.ToString("dd/MM/yyyy");
                    ws.Cells["C4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["B4"].Style.Font.Bold = true;

                    ws.Cells["D4"].Value = "Fecha Fin";
                    ws.Cells["D4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["E4"].Value = campania.fechafin.ToString("dd/MM/yyyy");
                    ws.Cells["E4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["D4"].Style.Font.Bold = true;

                    ws.Cells["F4"].Value = "Estado";
                    ws.Cells["F4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["G4"].Value = campania.estado == 0 ? "VENCIDO" : "ACTIVO";
                    ws.Cells["G4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["F4"].Style.Font.Bold = true;
                    int fila = 8;

                    if (listaClientes.Count > 0)
                    {
                        ws.Cells["B6"].Value = "Lista Clientes";
                        ws.Cells["B6:C6"].Style.Font.Bold = true;

                        ws.Cells["B6"].Style.Font.Size = 14;
                        ws.Cells["B6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["B6:F6"].Merge = true;
                        ws.Cells["B6:F6"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        //Seccion Clientes
                        ws.Cells["B7"].Value = "Id";
                        ws.Cells["C7"].Value = "Cliente";
                        ws.Cells["D7"].Value = "Nro. Documento";
                        ws.Cells["E7"].Value = "Fecha Nacimiento";
                        ws.Cells["F7"].Value = "Correo";

                        ws.Cells["B7:F7"].Style.Font.Bold = true;
                        ws.Cells["B7:F7"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells["B7:F7"].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                        ws.Cells["B7:F7"].Style.Font.Color.SetColor(Color.White);
                        ws.Cells["B7:F7"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        foreach (var cliente in listaClientes)
                        {
                            ws.Cells[string.Format("B{0}", fila)].Value = cliente.id;
                            ws.Cells[string.Format("C{0}", fila)].Value = cliente.NombreCompleto;
                            ws.Cells[string.Format("D{0}", fila)].Value = cliente.NroDoc;
                            ws.Cells[string.Format("E{0}", fila)].Value = cliente.FechaNacimiento;
                            ws.Cells[string.Format("F{0}", fila)].Value = cliente.Mail;
                            fila++;
                        }
                        fila++;
                    }
                    if (listaTickets.Count > 0)
                    {
                        ws.Cells[string.Format("B{0}", fila)].Value = "Lista Tickets";
                        ws.Cells[string.Format("B{0}:H{1}", fila, fila)].Style.Font.Bold = true;

                        ws.Cells[string.Format("B{0}", fila)].Style.Font.Size = 14;
                        ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[string.Format("B{0}:H{1}", fila, fila)].Merge = true;
                        ws.Cells[string.Format("B{0}:H{1}", fila, fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        fila++;
                        //Seccion Tickets
                        ws.Cells[string.Format("B{0}", fila)].Value = "Id";
                        ws.Cells[string.Format("C{0}", fila)].Value = "FechaTicket";
                        ws.Cells[string.Format("D{0}", fila)].Value = "NroTicket";
                        ws.Cells[string.Format("E{0}", fila)].Value = "Monto";
                        ws.Cells[string.Format("F{0}", fila)].Value = "Cliente";
                        ws.Cells[string.Format("G{0}", fila)].Value = "NroDoc";
                        ws.Cells[string.Format("H{0}", fila)].Value = "UsuarioReg";

                        ws.Cells[string.Format("B{0}:H{1}", fila, fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:H{1}", fila, fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[string.Format("B{0}:H{1}", fila, fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                        ws.Cells[string.Format("B{0}:H{1}", fila, fila)].Style.Font.Color.SetColor(Color.White);
                        ws.Cells[string.Format("B{0}:H{1}", fila, fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        fila++;
                        foreach (var ticket in listaTickets)
                        {
                            ws.Cells[string.Format("B{0}", fila)].Value = ticket.id;
                            ws.Cells[string.Format("C{0}", fila)].Value = ticket.fechareg.ToString("dd/MM/yyyy");
                            ws.Cells[string.Format("D{0}", fila)].Value = ticket.nroticket;
                            ws.Cells[string.Format("E{0}", fila)].Value = ticket.monto;
                            ws.Cells[string.Format("F{0}", fila)].Value = ticket.NombreCompleto;
                            ws.Cells[string.Format("G{0}", fila)].Value = ticket.NroDoc;
                            ws.Cells[string.Format("H{0}", fila)].Value = ticket.nombre_usuario;
                            fila++;
                        }
                    }

                    fila++;
                    ws.Cells["A:AZ"].AutoFitColumns();

                    excelName = "CampaniaPromocion_" + DateTime.Now.ToUniversalTime() + ".xlsx";
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());
                    mensaje = "Descargando Archivo";
                    respuesta = true;
                }
            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = base64String, excelName, respuesta, mensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ReporteCampaniaPromocionExcelTodosJson(int[] codsala, DateTime fechaini, DateTime fechafin, int tipo = 0)
        {
            var mensaje = "";
            var lista = new List<CMP_CampañaEntidad>();
            int cantElementos = (codsala == null) ? 0 : codsala.Length;
            var strElementos = String.Empty;
            var strElementos_ = String.Empty;
            List<CMP_TicketEntidad> listaTickets = new List<CMP_TicketEntidad>();
            List<CMP_ClienteEntidad> listaClientes = new List<CMP_ClienteEntidad>();
            CMP_CampañaEntidad campania = new CMP_CampañaEntidad();
            SalaEntidad sala = new SalaEntidad();
            SEG_UsuarioEntidad usuario = new SEG_UsuarioEntidad();
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;

            try
            {

                if (cantElementos > 0)
                {
                    strElementos = " c.[sala_id] in(" + "'" + String.Join("','", codsala) + "'" + ") and ";
                }

                lista = campaniabl.GetListadoCampaniaxTipoyFechas(strElementos, fechaini, fechafin, 0);

                if (lista.Any())
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    foreach (var camp in lista)
                    {
                        campania = campaniabl.CampañaIdObtenerJson(camp.id);
                        listaClientes = clientebl.GetClientesCampaniaJson(camp.id);
                        listaTickets = ticketbl.CMPTicketCampañaListadoJson(camp.id);
                        usuario = usuariobl.UsuarioEmpleadoIDObtenerJson(camp.usuario_id);
                        sala = salaBl.SalaListaIdJson(camp.sala_id);

                        var ws = excel.Workbook.Worksheets.Add(campania.nombre + "_" + campania.id);

                        ws.Cells["B1"].Value = "Reporte Campaña Tipo Promoción";
                        ws.Cells["B1:C1"].Style.Font.Bold = true;

                        ws.Cells["B1"].Style.Font.Size = 20;
                        ws.Cells["B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["B1:I1"].Merge = true;
                        ws.Cells["B1:I1"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells["B3"].Value = "Usuario Registro";
                        ws.Cells["B3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["C3"].Value = usuario.UsuarioNombre;
                        ws.Cells["C3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["B3"].Style.Font.Bold = true;

                        ws.Cells["D3"].Value = "Campaña";
                        ws.Cells["D3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["E3"].Value = campania.nombre;
                        ws.Cells["E3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["D3"].Style.Font.Bold = true;

                        ws.Cells["F3"].Value = "Fecha Registro";
                        ws.Cells["F3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["G3"].Value = campania.fechareg.ToString("dd/MM/yyyy hh:mm:ss A");
                        ws.Cells["G3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["F3"].Style.Font.Bold = true;

                        ws.Cells["H3"].Value = "Sala";
                        ws.Cells["H3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["I3"].Value = sala.Nombre;
                        ws.Cells["I3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["H3"].Style.Font.Bold = true;

                        ws.Cells["B4"].Value = "Fecha Inicio";
                        ws.Cells["B4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["C4"].Value = campania.fechaini.ToString("dd/MM/yyyy");
                        ws.Cells["C4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["B4"].Style.Font.Bold = true;

                        ws.Cells["D4"].Value = "Fecha Fin";
                        ws.Cells["D4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["E4"].Value = campania.fechafin.ToString("dd/MM/yyyy");
                        ws.Cells["E4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["D4"].Style.Font.Bold = true;

                        ws.Cells["F4"].Value = "Estado";
                        ws.Cells["F4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["G4"].Value = campania.estado == 0 ? "VENCIDO" : "ACTIVO";
                        ws.Cells["G4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["F4"].Style.Font.Bold = true;

                        int fila = 8;

                        if (listaClientes.Count > 0)
                        {
                            ws.Cells["B6"].Value = "Lista Clientes";
                            ws.Cells["B6:C6"].Style.Font.Bold = true;

                            ws.Cells["B6"].Style.Font.Size = 14;
                            ws.Cells["B6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells["B6:F6"].Merge = true;
                            ws.Cells["B6:F6"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            //Seccion Clientes
                            ws.Cells["B7"].Value = "Id";
                            ws.Cells["C7"].Value = "Cliente";
                            ws.Cells["D7"].Value = "Nro. Documento";
                            ws.Cells["E7"].Value = "Fecha Nacimiento";
                            ws.Cells["F7"].Value = "Correo";

                            ws.Cells["B7:F7"].Style.Font.Bold = true;
                            ws.Cells["B7:F7"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells["B7:F7"].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                            ws.Cells["B7:F7"].Style.Font.Color.SetColor(Color.White);
                            ws.Cells["B7:F7"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            foreach (var cliente in listaClientes)
                            {
                                ws.Cells[string.Format("B{0}", fila)].Value = cliente.id;
                                ws.Cells[string.Format("C{0}", fila)].Value = cliente.NombreCompleto;
                                ws.Cells[string.Format("D{0}", fila)].Value = cliente.NroDoc;
                                ws.Cells[string.Format("E{0}", fila)].Value = cliente.FechaNacimiento;
                                ws.Cells[string.Format("F{0}", fila)].Value = cliente.Mail;

                                fila++;
                            }

                            fila++;
                        }

                        if (listaTickets.Count > 0)
                        {
                            ws.Cells[string.Format("B{0}", fila)].Value = "Lista Tickets";
                            ws.Cells[string.Format("B{0}:H{1}", fila, fila)].Style.Font.Bold = true;

                            ws.Cells[string.Format("B{0}", fila)].Style.Font.Size = 14;
                            ws.Cells[string.Format("B{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[string.Format("B{0}:H{1}", fila, fila)].Merge = true;
                            ws.Cells[string.Format("B{0}:H{1}", fila, fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            fila++;

                            //Seccion Tickets
                            ws.Cells[string.Format("B{0}", fila)].Value = "Id";
                            ws.Cells[string.Format("C{0}", fila)].Value = "FechaTicket";
                            ws.Cells[string.Format("D{0}", fila)].Value = "NroTicket";
                            ws.Cells[string.Format("E{0}", fila)].Value = "Monto";
                            ws.Cells[string.Format("F{0}", fila)].Value = "Cliente";
                            ws.Cells[string.Format("G{0}", fila)].Value = "NroDoc";
                            ws.Cells[string.Format("H{0}", fila)].Value = "UsuarioReg";

                            ws.Cells[string.Format("B{0}:H{1}", fila, fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:H{1}", fila, fila)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[string.Format("B{0}:H{1}", fila, fila)].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                            ws.Cells[string.Format("B{0}:H{1}", fila, fila)].Style.Font.Color.SetColor(Color.White);
                            ws.Cells[string.Format("B{0}:H{1}", fila, fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            fila++;

                            foreach (var ticket in listaTickets)
                            {
                                ws.Cells[string.Format("B{0}", fila)].Value = ticket.id;
                                ws.Cells[string.Format("C{0}", fila)].Value = ticket.fechareg.ToString("dd/MM/yyyy");
                                ws.Cells[string.Format("D{0}", fila)].Value = ticket.nroticket;
                                ws.Cells[string.Format("E{0}", fila)].Value = ticket.monto;
                                ws.Cells[string.Format("F{0}", fila)].Value = ticket.NombreCompleto;
                                ws.Cells[string.Format("G{0}", fila)].Value = ticket.NroDoc;
                                ws.Cells[string.Format("H{0}", fila)].Value = ticket.nombre_usuario;

                                fila++;
                            }
                        }

                        fila++;

                        ws.Cells["A:AZ"].AutoFitColumns();
                    }

                    excelName = "CampaniaPromocion_" + DateTime.Now.ToUniversalTime() + ".xlsx";

                    var memoryStream = new MemoryStream();

                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());
                    mensaje = "Descargando Archivo";
                    respuesta = true;
                }
                else
                {
                    respuesta = false;
                    mensaje = "No hay registros";
                }
            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ", Llame Administrador";
            }

            return Json(new { data = base64String, excelName, respuesta, mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ExcelTicketsPromocionales(List<int> salaIds, string fechaInicio, string fechaFin)
        {
            bool success = false;
            string message = "No se ha encontrado registros";

            if (!salaIds.Any())
            {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione una o varias salas"
                });
            }

            if (string.IsNullOrEmpty(fechaInicio))
            {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione fecha inicio"
                });
            }

            if (string.IsNullOrEmpty(fechaFin))
            {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione fecha final"
                });
            }

            DateTime fromDate = Convert.ToDateTime(fechaInicio);
            DateTime endDate = Convert.ToDateTime(fechaFin);

            DateTime currentDate = DateTime.Now;
            string fileExtension = "xlsx";
            string filename = $"TicketsPromocionales_{fromDate.ToString("dd-MM-yyyy")}_al_{endDate.ToString("dd-MM-yyyy")}_{currentDate.ToString("HHmmss")}.{fileExtension}";
            string data = string.Empty;

            try
            {
                List<CMP_TicketReporteEntidad> listaTickets = ticketbl.ObtenerTicketsPorSalas(salaIds, fromDate, endDate, 0);

                if (listaTickets.Any())
                {
                    object arguments = new
                    {
                        fechaInicio,
                        fechaFin
                    };

                    MemoryStream excelStream = CampaniaReport.ExcelTicketsPromocionales(listaTickets, arguments);

                    data = Convert.ToBase64String(excelStream.ToArray());

                    success = true;
                    message = "Excel Generado";
                }
            }
            catch (Exception exception)
            {
                success = false;
                message = exception.Message;
            }

            JsonResult jsonResult = Json(new
            {
                success,
                message,
                filename,
                data
            });

            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        #endregion
        #region Campania Sorteo Cliente
        public ActionResult ReporteCampaniaSorteoCliente()
        {
            return View("~/Views/Campania/ReporteCampaniaSorteoClienteVista.cshtml");
        }
        [HttpPost]
        public ActionResult ListarCampaniasxClienteJson(int[] codsala, string nrodoc, DateTime fechaIni, DateTime fechaFin, string UrlProgresivoSala)
        {
            List<AST_ClienteEntidad> cliente = new List<AST_ClienteEntidad>();
            List<CMP_CuponesGeneradosEntidad> listacuponesGenerados = new List<CMP_CuponesGeneradosEntidad>();
            List<CMP_ClienteEntidad> listaCmpCliente = new List<CMP_ClienteEntidad>();
            string clienteIdQueryPromocion = string.Empty;
            string clienteIdQuerySorteo = string.Empty;
            string mensaje = string.Empty;
            bool respuesta = false;
            List<CMP_SesionCuponesClienteEntidad> listaSesiones = new List<CMP_SesionCuponesClienteEntidad>();
            List<int> listaIdsCampanias = new List<int>();
            var listaCampanias = new List<CMP_CampañaEntidad>();
            try
            {
                cliente = ast_ClienteBL.GetListaClientesxNroDoc(nrodoc);
                if (cliente.Count > 0)
                {
                    clienteIdQueryPromocion = " cli.cliente_id in (" + String.Join(",", cliente.Select(x => x.Id)) + ") and camp.sala_id in (" + String.Join(",", codsala) + ")";
                    clienteIdQuerySorteo = " cli.ClienteId in (" + String.Join(",", cliente.Select(x => x.Id)) + ") and camp.sala_id in (" + String.Join(",", codsala) + ")";
                    //Lista CMP_Cliente
                    listaCmpCliente = clientebl.GetClientesCampaniaxCliente(clienteIdQueryPromocion, fechaIni, fechaFin);
                    //Lista Cupones Generados
                    //listacuponesGenerados = cuponesBL.GetListadoCuponesxCliente(clienteIdQuerySorteo, fechaIni, fechaFin);
                    listaSesiones = ListarSesionesPorFechasJsonServicio(UrlProgresivoSala,fechaIni,fechaFin,nrodoc);
                    listaIdsCampanias = listaSesiones.Where(x => x.CampaniaId != 0).Select(x => x.CampaniaId).Distinct().ToList();
                    foreach (var campania in listaIdsCampanias)
                    {
                        var camp = campaniabl.CampañaIdObtenerJson(campania);
                        listaCampanias.Add(camp);
                    }
                    //generar listacupones generados
                    foreach(var ses in listaSesiones)
                    {
                        var campania = listaCampanias.Where(x => x.id.Equals(Convert.ToInt64(ses.CampaniaId))).FirstOrDefault();
                        if (campania != null)
                        {
                            listacuponesGenerados.Add(new CMP_CuponesGeneradosEntidad {
                                CampaniaNombre=campania.nombre,
                                NombreCompleto=ses.NombreCliente,
                                NroDoc=ses.NroDocumento,
                                ClienteId=ses.ClienteId,
                                CampaniaId=ses.CampaniaId,
                                TipoCampania="Sorteo",
                                SerieIni=ses.SerieIni,
                                SerieFin=ses.SerieFin,
                                CantidadCupones=ses.CantidadCupones,
                                Fecha=ses.Fecha
                            });
                        }
                    }
                    foreach (var cmpcliente in listaCmpCliente)
                    {
                        listacuponesGenerados.Add(new CMP_CuponesGeneradosEntidad
                        {
                            CampaniaNombre = cmpcliente.CampaniaNombre,
                            NombreCompleto = cmpcliente.NombreCompleto,
                            NroDoc = cmpcliente.NroDoc,
                            ClienteId = cmpcliente.cliente_id,
                            CampaniaId = cmpcliente.campania_id,
                            TipoCampania = "Promocion",
                        });
                    }
                    respuesta = true;
                    mensaje = "Listando Registros";
                }
                else
                {
                    mensaje = "No se encontraron clientes con el nro. de documento " + nrodoc;
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data = listacuponesGenerados });
        }
        [HttpPost]
        public ActionResult ReporteCampaniaClienteExcelJson(int[] codsala, string nrodoc, DateTime fechaIni, DateTime fechaFin,string UrlProgresivoSala)
        {
            List<AST_ClienteEntidad> cliente = new List<AST_ClienteEntidad>();
            List<CMP_CuponesGeneradosEntidad> listacuponesGenerados = new List<CMP_CuponesGeneradosEntidad>();
            List<CMP_ClienteEntidad> listaCmpCliente = new List<CMP_ClienteEntidad>();
            string clienteIdQueryPromocion = string.Empty;
            string clienteIdQuerySorteo = string.Empty;
            string mensaje = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;

            List<CMP_SesionCuponesClienteEntidad> listaSesiones = new List<CMP_SesionCuponesClienteEntidad>();
            List<int> listaIdsCampanias = new List<int>();
            var listaCampanias = new List<CMP_CampañaEntidad>();
            try
            {
                cliente = ast_ClienteBL.GetListaClientesxNroDoc(nrodoc);
                if (cliente.Count > 0)
                {
                    clienteIdQueryPromocion = " cli.cliente_id in (" + String.Join(",", cliente.Select(x => x.Id)) + ") and camp.sala_id in (" + String.Join(",", codsala) + ")";
                    clienteIdQuerySorteo = " cli.ClienteId in (" + String.Join(",", cliente.Select(x => x.Id)) + ") and camp.sala_id in (" + String.Join(",", codsala) + ")";
                    //Lista CMP_Cliente
                    listaCmpCliente = clientebl.GetClientesCampaniaxCliente(clienteIdQueryPromocion, fechaIni, fechaFin);
                    //Lista Cupones Generados
                    //listacuponesGenerados = cuponesBL.GetListadoCuponesxCliente(clienteIdQuerySorteo, fechaIni, fechaFin);
                    listaSesiones = ListarSesionesPorFechasJsonServicio(UrlProgresivoSala, fechaIni, fechaFin, nrodoc);
                    listaIdsCampanias = listaSesiones.Where(x => x.CampaniaId != 0).Select(x => x.CampaniaId).Distinct().ToList();
                    foreach(var campania in listaIdsCampanias)
                    {
                        var camp = campaniabl.CampañaIdObtenerJson(campania);
                        listaCampanias.Add(camp);
                    }
                    //generar listacupones generados
                    foreach(var ses in listaSesiones)
                    {
                        var campania = listaCampanias.Where(x => x.id.Equals(Convert.ToInt64(ses.CampaniaId))).FirstOrDefault();
                        if (campania != null)
                        {
                            listacuponesGenerados.Add(new CMP_CuponesGeneradosEntidad
                            {
                                CampaniaNombre = campania.nombre,
                                NombreCompleto = ses.NombreCliente,
                                NroDoc = ses.NroDocumento,
                                ClienteId = ses.ClienteId,
                                CampaniaId = ses.CampaniaId,
                                TipoCampania = "Sorteo",
                                SerieIni = ses.SerieIni,
                                SerieFin = ses.SerieFin,
                                CantidadCupones = ses.CantidadCupones,
                                Fecha = ses.Fecha
                            });
                        }
                    }
                    foreach (var cmpcliente in listaCmpCliente)
                    {
                        listacuponesGenerados.Add(new CMP_CuponesGeneradosEntidad
                        {
                            CampaniaNombre = cmpcliente.CampaniaNombre,
                            NombreCompleto = cmpcliente.NombreCompleto,
                            NroDoc = cmpcliente.NroDoc,
                            ClienteId = cmpcliente.cliente_id,
                            CampaniaId = cmpcliente.campania_id,
                            TipoCampania = "Promocion",
                        });
                    }
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();
                    var ws = excel.Workbook.Worksheets.Add("Clientes Campaña");

                    if (listacuponesGenerados.Count > 0)
                    {

                        ws.Cells["B1"].Value = "Reporte Campaña Cliente";
                        ws.Cells["B1:C1"].Style.Font.Bold = true;

                        ws.Cells["B1"].Style.Font.Size = 20;
                        ws.Cells["B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["B1:K1"].Merge = true;
                        ws.Cells["B1:K1"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells["B2"].Value = "Lista Clientes";
                        ws.Cells["B2:K2"].Style.Font.Bold = true;

                        ws.Cells["B2"].Style.Font.Size = 14;
                        ws.Cells["B2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["B2:K2"].Merge = true;
                        ws.Cells["B2:K2"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        //Seccion Clientes
                        ws.Cells["B7"].Value = "Nombre";
                        ws.Cells["C7"].Value = "Nro Doc.";
                        ws.Cells["D7"].Value = "Campaña";
                        ws.Cells["E7"].Value = "Tipo";
                        ws.Cells["F7"].Value = "Fecha";
                        ws.Cells["G7"].Value = "Maquina";
                        ws.Cells["H7"].Value = "SerieIni";
                        ws.Cells["I7"].Value = "SerieFin";
                        ws.Cells["J7"].Value = "Cantidad Cupones";
                        ws.Cells["K7"].Value = "Parametro";

                        ws.Cells["B7:K7"].Style.Font.Bold = true;
                        ws.Cells["B7:K7"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells["B7:K7"].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                        ws.Cells["B7:K7"].Style.Font.Color.SetColor(Color.White);
                        ws.Cells["B7:K7"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        int fila = 8;
                        foreach (var cupon in listacuponesGenerados)
                        {
                            ws.Cells[string.Format("B{0}", fila)].Value = cupon.NombreCompleto;
                            ws.Cells[string.Format("C{0}", fila)].Value = cupon.NroDoc;
                            ws.Cells[string.Format("D{0}", fila)].Value = cupon.CampaniaNombre;
                            ws.Cells[string.Format("E{0}", fila)].Value = cupon.TipoCampania;
                            ws.Cells[string.Format("F{0}", fila)].Value = cupon.TipoCampania == "Sorteo" ? cupon.Fecha.ToString("dd/MM/yyyy") : "";
                            ws.Cells[string.Format("G{0}", fila)].Value = cupon.TipoCampania == "Sorteo" ? cupon.SlotId : "";
                            ws.Cells[string.Format("H{0}", fila)].Value = cupon.TipoCampania == "Sorteo" ? cupon.SerieIni : "";
                            ws.Cells[string.Format("I{0}", fila)].Value = cupon.TipoCampania == "Sorteo" ? cupon.SerieFin : "";
                            ws.Cells[string.Format("J{0}", fila)].Value = cupon.TipoCampania == "Sorteo" ? cupon.CantidadCupones.ToString() : "";
                            ws.Cells[string.Format("K{0}", fila)].Value = cupon.TipoCampania == "Sorteo" ? cupon.Parametro.ToString() : "";
                            fila++;
                        }
                        fila++;
                        ws.Cells["A:AZ"].AutoFitColumns();

                    }

                    excelName = "CampaniaPromocion_" + DateTime.Now.ToUniversalTime() + ".xlsx";
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());
                    mensaje = "Descargando Archivo";
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { data = base64String, excelName, respuesta, mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Campania Sorteo
        public ActionResult ReporteCampaniaCliente()
        {
            return View("~/Views/Campania/ReporteCampaniaClienteVista.cshtml");
        }
        [HttpPost]
        public JsonResult ListarCampaniaSorteoPorFechasJson(int[] codsala, DateTime fechaIni, DateTime fechaFin, string UrlProgresivoSala)
        {
            var errormensaje = "";
            var lista = new List<CMP_SesionCuponesClienteEntidad>();
            int cantElementos = (codsala == null) ? 0 : codsala.Length;
            var strElementos = String.Empty;
            var strElementos_ = String.Empty;
            List<int> listaIdsCampanias = new List<int>();
            var listaCampanias = new List<CMP_CampañaEntidad>();
            try
            {
                if (cantElementos > 0)
                {
                    strElementos = " c.[sala_id] in(" + "'" + String.Join("','", codsala) + "'" + ") ";
                }
                //lista = campaniabl.GetListadoCampaniaSorteoReporte(strElementos, fechaini, fechafin);
                lista = ListarSesionesPorFechasJsonServicio(UrlProgresivoSala,fechaIni, fechaFin);
                listaIdsCampanias = lista.Where(x=>x.CampaniaId!=0).Select(x => x.CampaniaId).Distinct().ToList();
                foreach(var campania in listaIdsCampanias)
                {
                    var camp = campaniabl.CampañaIdObtenerJson(campania);
                    listaCampanias.Add(camp);
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaCampanias.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ReporteCampaniaSorteoExcelJson(int campaniaid, DateTime fechaIni, DateTime fechaFin, string UrlProgresivoSala)
        {

            var mensaje = "";

            CMP_CampañaEntidad campania = new CMP_CampañaEntidad();
            SalaEntidad sala = new SalaEntidad();
            SEG_UsuarioEntidad usuario = new SEG_UsuarioEntidad();
            List<CMP_CuponesGeneradosEntidad> listaCupones = new List<CMP_CuponesGeneradosEntidad>();
            List<CMP_DetalleCuponesImpresosEntidad> listaCuponesImpresos = new List<CMP_DetalleCuponesImpresosEntidad>();
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            string whereQuery = string.Empty;
            List<CMP_CuponesGeneradosEntidad> listaCuponesServicio = new List<CMP_CuponesGeneradosEntidad>();
            try
            {
                campania = campaniabl.CampañaIdObtenerJson(campaniaid);
                //listaCupones = cuponesBL.GetListadoCuponesxCampaniaFecha(campaniaid, fechaIni, fechaFin, true);
                //listaCupones = listaCupones.ToList();
                //whereQuery = " where CgId in (" + String.Join(",", listaCupones.Select(x => x.CgId)) + ") ";
                //listaCuponesImpresos = detalleCuponesImpresosBL.GetListadoDetalleCuponImpresoExcel(whereQuery);
                //foreach (var cupon in listaCupones)
                //{
                //    listaCuponesImpresos = detalleCuponesImpresosBL.GetListadoDetalleCuponImpreso(cupon.CgId);
                //    cupon.DetalleCuponesImpresos = listaCuponesImpresos;
                //}
                if (campania.id != 0)
                {
                    listaCuponesServicio = ListarSesionCuponesClientePorFechaYCampaniaId(campaniaid, UrlProgresivoSala, fechaIni, fechaFin);

                    string QueryContadores = $@" where SesionId in ({String.Join(",",listaCuponesServicio.Select(x=>x.SesionId).Distinct().ToList())}) ";
                    var listaContadoresServicio = CMPContadoresOnlineReporteExcel(campaniaid,UrlProgresivoSala,fechaIni,fechaFin);

                    //string QueryDetalleGenerados = $@" where Cod_Cont in ({String.Join(",",listaContadoresServicio.Select(x=>x.Cod_Cont).Distinct().ToList())})";
                    //var listaDetalleGeneradoServicio = ListarDetalleCuponesGeneradosPorQuery(UrlProgresivoSala, QueryDetalleGenerados);


                    usuario = usuariobl.UsuarioEmpleadoIDObtenerJson(campania.usuario_id);
                    sala = salaBl.SalaListaIdJson(campania.sala_id);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var ws = excel.Workbook.Worksheets.Add("Clientes Campaña");
                    ws.Cells["B1"].Value = "Reporte Campaña Tipo Sorteo";
                    ws.Cells["B1:C1"].Style.Font.Bold = true;

                    ws.Cells["B1"].Style.Font.Size = 20;
                    ws.Cells["B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["B1:I1"].Merge = true;
                    ws.Cells["B1:I1"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ws.Cells["B3"].Value = "Usuario Registro";
                    ws.Cells["B3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["C3"].Value = usuario.UsuarioNombre;
                    ws.Cells["C3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["B3"].Style.Font.Bold = true;

                    ws.Cells["D3"].Value = "Campaña";
                    ws.Cells["D3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["E3"].Value = campania.nombre;
                    ws.Cells["E3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["D3"].Style.Font.Bold = true;

                    ws.Cells["F3"].Value = "Fecha Registro";
                    ws.Cells["F3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["G3"].Value = campania.fechareg.ToString("dd/MM/yyyy hh:mm:ss A");
                    ws.Cells["G3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["F3"].Style.Font.Bold = true;

                    ws.Cells["H3"].Value = "Sala";
                    ws.Cells["H3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["I3"].Value = sala.Nombre;
                    ws.Cells["I3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["H3"].Style.Font.Bold = true;

                    ws.Cells["B4"].Value = "Fecha Inicio";
                    ws.Cells["B4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["C4"].Value = campania.fechaini.ToString("dd/MM/yyyy");
                    ws.Cells["C4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["B4"].Style.Font.Bold = true;

                    ws.Cells["D4"].Value = "Fecha Fin";
                    ws.Cells["D4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["E4"].Value = campania.fechafin.ToString("dd/MM/yyyy");
                    ws.Cells["E4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["D4"].Style.Font.Bold = true;

                    ws.Cells["F4"].Value = "Estado";
                    ws.Cells["F4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["G4"].Value = campania.estado == 0 ? "VENCIDO" : "ACTIVO";
                    ws.Cells["G4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells["F4"].Style.Font.Bold = true;

                    ws.Cells["B7"].Value = "SesionId";
                    ws.Cells["C7"].Value = "Maquina";
                    ws.Cells["D7"].Value = "Nombre Completo";
                    ws.Cells["E7"].Value = "Nro. Documento";
                    ws.Cells["F7"].Value = "Cantidad Cupones";
                    ws.Cells["G7"].Value = "Serie Ini";
                    ws.Cells["H7"].Value = "Serie Fin";
                    ws.Cells["I7"].Value = "Fecha";
                    ws.Cells["J7"].Value = "Mail";

                    ws.Cells["B7:J7"].Style.Font.Bold = true;
                    ws.Cells["B7:J7"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells["B7:J7"].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                    ws.Cells["B7:J7"].Style.Font.Color.SetColor(Color.White);
                    ws.Cells["B7:J7"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    int fila = 8, inicioGrupo = 0, finGrupo = 0;
                    foreach (var cupon in listaCuponesServicio)
                    {
                        ws.Cells[string.Format("B{0}", fila)].Value = cupon.SesionId;
                        ws.Cells[string.Format("C{0}", fila)].Value = cupon.SlotId;
                        ws.Cells[string.Format("D{0}", fila)].Value = cupon.NombreCompleto;
                        ws.Cells[string.Format("E{0}", fila)].Value = cupon.NroDoc;
                        ws.Cells[string.Format("F{0}", fila)].Value = cupon.CantidadCupones;
                        ws.Cells[string.Format("G{0}", fila)].Value = cupon.SerieIni;
                        ws.Cells[string.Format("H{0}", fila)].Value = cupon.SerieFin;
                        ws.Cells[string.Format("I{0}", fila)].Value = cupon.Fecha.ToString("dd/MM/yyyy");
                        ws.Cells[string.Format("J{0}", fila)].Value = cupon.Mail;
                        //Styles
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        fila++;

                        //cupon.DetalleCuponesImpresos = listaCuponesImpresos.Where(x => x.CgId == cupon.CgId).ToList();
                        //Detalle
                        cupon.DetalleCuponesImpresos = listaContadoresServicio.Where(x => x.SesionId == cupon.SesionId).ToList();
                        if (cupon.DetalleCuponesImpresos.Count > 0)
                        {
                            inicioGrupo = fila;
                            //Cabeceras
                            ws.Cells[string.Format("C{0}", inicioGrupo)].Value = "Maquina";
                            ws.Cells[string.Format("D{0}", inicioGrupo)].Value = "Serie Ini";
                            ws.Cells[string.Format("E{0}", inicioGrupo)].Value = "Serie Fin";
                            ws.Cells[string.Format("F{0}", inicioGrupo)].Value = "Cantidad Cupones";
                            ws.Cells[string.Format("G{0}", inicioGrupo)].Value = "Coin Out Ini";
                            ws.Cells[string.Format("H{0}", inicioGrupo)].Value = "Coin Out Fin";
                            ws.Cells[string.Format("I{0}", inicioGrupo)].Value = "Current Credits";
                            ws.Cells[string.Format("J{0}", inicioGrupo)].Value = "Monto";
                            ws.Cells[string.Format("K{0}", inicioGrupo)].Value = "Token";
                            ws.Cells[string.Format("L{0}", inicioGrupo)].Value = "Coin Out Parametro";
                            ws.Cells[string.Format("M{0}", inicioGrupo)].Value = "Fecha Reg.";
                            ws.Cells[string.Format("C{0}:M{0}", inicioGrupo)].Style.Font.Bold = true;
                            ws.Cells[string.Format("C{0}:M{0}", inicioGrupo)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[string.Format("C{0}:M{0}", inicioGrupo)].Style.Fill.BackgroundColor.SetColor(Color.DarkRed);
                            ws.Cells[string.Format("C{0}:M{0}", inicioGrupo)].Style.Font.Color.SetColor(Color.White);
                            fila++;
                            //Datos Detalle
                            foreach (var detalle in cupon.DetalleCuponesImpresos)
                            {
                                ws.Cells[string.Format("C{0}", fila)].Value = detalle.CodMaq;
                                ws.Cells[string.Format("D{0}", fila)].Value = detalle.SerieIni;
                                ws.Cells[string.Format("E{0}", fila)].Value = detalle.SerieFin;
                                ws.Cells[string.Format("F{0}", fila)].Value = detalle.CantidadCuponesImpresos;
                                ws.Cells[string.Format("G{0}", fila)].Value = detalle.CoinOutAnterior;
                                ws.Cells[string.Format("H{0}", fila)].Value = detalle.CoinOut;
                                ws.Cells[string.Format("I{0}", fila)].Value = detalle.CurrentCredits;
                                ws.Cells[string.Format("J{0}", fila)].Value = detalle.Monto;
                                ws.Cells[string.Format("K{0}", fila)].Value = detalle.Token;
                                ws.Cells[string.Format("L{0}", fila)].Value = detalle.CoinOutIas;
                                ws.Cells[string.Format("M{0}", fila)].Value = detalle.FechaRegistro.ToString("dd/MM/yyyy");
                                fila++;
                            }
                            //Agregar una fila extra en el detalle para que no se confundan con el collapse
                            fila++;
                            finGrupo = fila - 1;
                            for (var i = inicioGrupo; i <= finGrupo; i++)
                            {
                                ws.Row(i).OutlineLevel = 1;
                                ws.Row(i).Collapsed = true;
                            }
                        }
                        else
                        {
                            ws.Cells[string.Format("C{0}", fila)].Value = "No se encontraron detalles para este registro";
                            ws.Cells[string.Format("C{0}:M{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("C{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[string.Format("C{0}:M{0}", fila)].Merge = true;
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            //Agregar una fila extra en el detalle para que no se confundan con el collapse
                            fila++;
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;

                            fila++;
                        }
                        //Fin Detalle
                    }

                    fila++;
                    ws.Cells["A:AZ"].AutoFitColumns();

                    excelName = "CampaniaSorteo_" + DateTime.Now.ToUniversalTime() + ".xlsx";
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());
                    mensaje = "Descargando Archivo";
                    respuesta = true;
                }
            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new
            {
                data = base64String,
                excelName,
                respuesta,
                mensaje
            };
            var result = new ContentResult
            {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;

            //return Json(new { data = base64String, excelName, respuesta, mensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ReporteCampaniaSorteoExcelTodosJson(int[] ListaCampaniaIds,string UrlProgresivoSala)
        {
            var errormensaje = "";
            var listaCampanias = new List<CMP_CampañaEntidad>();
            var strElementos = String.Empty;
            var strElementos_ = String.Empty;
            var mensaje = "";

            CMP_CampañaEntidad campania = new CMP_CampañaEntidad();
            SalaEntidad sala = new SalaEntidad();
            SEG_UsuarioEntidad usuario = new SEG_UsuarioEntidad();
            List<CMP_CuponesGeneradosEntidad> listaCupones = new List<CMP_CuponesGeneradosEntidad>();
            List<CMP_DetalleCuponesImpresosEntidad> listaCuponesImpresos = new List<CMP_DetalleCuponesImpresosEntidad>();
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            string whereQuery = string.Empty;
            List<CMP_CuponesGeneradosEntidad> listaCuponesServicio = new List<CMP_CuponesGeneradosEntidad>();
            List<CMP_DetalleCuponesImpresosEntidad> listaContadoresServicio = new List<CMP_DetalleCuponesImpresosEntidad>();
            try
            {

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excel = new ExcelPackage();
                String QuerySesiones = $@" where CampaniaId in ({String.Join(",",ListaCampaniaIds.Distinct().ToList())})";
                listaCuponesServicio = ListarSesionCuponesClientePorQuery(QuerySesiones, UrlProgresivoSala);

                if (listaCuponesServicio.Count > 0)
                {
                    string QueryContadores = $@" where SesionId in ({String.Join(",",listaCuponesServicio.Select(x=>x.SesionId).Distinct().ToList())})";
                    listaContadoresServicio = ListarContadoresOnlineWebPorQuery(UrlProgresivoSala,QueryContadores);
                }
                foreach (var camp in ListaCampaniaIds)
                {
                    campania = campaniabl.CampañaIdObtenerJson(camp);
                    listaCupones = listaCuponesServicio.Where(x => x.CampaniaId == campania.id).ToList();
                    List<long> listaIdsCupones = new List<long>();
                    listaIdsCupones = listaCupones.Select(x => x.SesionId).ToList(); ;
                  
                    if (campania.id != 0)
                    {
                        usuario = usuariobl.UsuarioEmpleadoIDObtenerJson(campania.usuario_id);
                        sala = salaBl.SalaListaIdJson(campania.sala_id);


                        var ws = excel.Workbook.Worksheets.Add(campania.nombre + "_" + campania.id);
                        ws.Cells["B1"].Value = "Reporte Campaña Tipo Sorteo";
                        ws.Cells["B1:C1"].Style.Font.Bold = true;

                        ws.Cells["B1"].Style.Font.Size = 20;
                        ws.Cells["B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["B1:I1"].Merge = true;
                        ws.Cells["B1:I1"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        ws.Cells["B3"].Value = "Usuario Registro";
                        ws.Cells["B3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["C3"].Value = usuario.UsuarioNombre;
                        ws.Cells["C3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["B3"].Style.Font.Bold = true;

                        ws.Cells["D3"].Value = "Campaña";
                        ws.Cells["D3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["E3"].Value = campania.nombre;
                        ws.Cells["E3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["D3"].Style.Font.Bold = true;

                        ws.Cells["F3"].Value = "Fecha Registro";
                        ws.Cells["F3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["G3"].Value = campania.fechareg.ToString("dd/MM/yyyy hh:mm:ss A");
                        ws.Cells["G3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["F3"].Style.Font.Bold = true;

                        ws.Cells["H3"].Value = "Sala";
                        ws.Cells["H3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["I3"].Value = sala.Nombre;
                        ws.Cells["I3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["H3"].Style.Font.Bold = true;

                        ws.Cells["B4"].Value = "Fecha Inicio";
                        ws.Cells["B4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["C4"].Value = campania.fechaini.ToString("dd/MM/yyyy");
                        ws.Cells["C4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["B4"].Style.Font.Bold = true;

                        ws.Cells["D4"].Value = "Fecha Fin";
                        ws.Cells["D4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["E4"].Value = campania.fechafin.ToString("dd/MM/yyyy");
                        ws.Cells["E4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["D4"].Style.Font.Bold = true;

                        ws.Cells["F4"].Value = "Estado";
                        ws.Cells["F4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["G4"].Value = campania.estado == 0 ? "VENCIDO" : "ACTIVO";
                        ws.Cells["G4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["F4"].Style.Font.Bold = true;

                        ws.Cells["B7"].Value = "CgId";
                        ws.Cells["C7"].Value = "Maquina";
                        ws.Cells["D7"].Value = "Nombre Completo";
                        ws.Cells["E7"].Value = "Nro. Documento";
                        ws.Cells["F7"].Value = "Cantidad Cupones";
                        ws.Cells["G7"].Value = "Serie Ini";
                        ws.Cells["H7"].Value = "Serie Fin";
                        ws.Cells["I7"].Value = "Fecha";
                        ws.Cells["J7"].Value = "Mail";

                        ws.Cells["B7:J7"].Style.Font.Bold = true;
                        ws.Cells["B7:J7"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells["B7:J7"].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                        ws.Cells["B7:J7"].Style.Font.Color.SetColor(Color.White);
                        ws.Cells["B7:J7"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        int fila = 8, inicioGrupo = 0, finGrupo = 0;
                        foreach (var cupon in listaCupones)
                        {
                            ws.Cells[string.Format("B{0}", fila)].Value = cupon.CgId;
                            ws.Cells[string.Format("C{0}", fila)].Value = cupon.SlotId;
                            ws.Cells[string.Format("D{0}", fila)].Value = cupon.NombreCompleto;
                            ws.Cells[string.Format("E{0}", fila)].Value = cupon.NroDoc;
                            ws.Cells[string.Format("F{0}", fila)].Value = cupon.CantidadCupones;
                            ws.Cells[string.Format("G{0}", fila)].Value = cupon.SerieIni;
                            ws.Cells[string.Format("H{0}", fila)].Value = cupon.SerieFin;
                            ws.Cells[string.Format("I{0}", fila)].Value = cupon.Fecha.ToString("dd/MM/yyyy");
                            ws.Cells[string.Format("J{0}", fila)].Value = cupon.Mail;
                            //Styles
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                            ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            fila++;

                            //listaCuponesImpresos = listaContadoresServicio.Where(x => x.SesionId =).ToList();

                            cupon.DetalleCuponesImpresos = listaContadoresServicio.Where(x => x.SesionId == cupon.SesionId).ToList();
                            //Detalle
                            if (cupon.DetalleCuponesImpresos.Count > 0)
                            {
                                inicioGrupo = fila;
                                //Cabeceras
                                ws.Cells[string.Format("C{0}", inicioGrupo)].Value = "Maquina";
                                ws.Cells[string.Format("D{0}", inicioGrupo)].Value = "Serie Ini";
                                ws.Cells[string.Format("E{0}", inicioGrupo)].Value = "Serie Fin";
                                ws.Cells[string.Format("F{0}", inicioGrupo)].Value = "Cantidad Cupones";
                                ws.Cells[string.Format("G{0}", inicioGrupo)].Value = "Coin Out Ini";
                                ws.Cells[string.Format("H{0}", inicioGrupo)].Value = "Coin Out Fin";
                                ws.Cells[string.Format("I{0}", inicioGrupo)].Value = "Current Credits";
                                ws.Cells[string.Format("J{0}", inicioGrupo)].Value = "Monto";
                                ws.Cells[string.Format("K{0}", inicioGrupo)].Value = "Token";
                                ws.Cells[string.Format("L{0}", inicioGrupo)].Value = "Coin Out Parametro";
                                ws.Cells[string.Format("M{0}", inicioGrupo)].Value = "Fecha Reg.";
                                ws.Cells[string.Format("C{0}:M{0}", inicioGrupo)].Style.Font.Bold = true;
                                ws.Cells[string.Format("C{0}:M{0}", inicioGrupo)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[string.Format("C{0}:M{0}", inicioGrupo)].Style.Fill.BackgroundColor.SetColor(Color.DarkRed);
                                ws.Cells[string.Format("C{0}:M{0}", inicioGrupo)].Style.Font.Color.SetColor(Color.White);
                                fila++;
                                //Datos Detalle
                                foreach (var detalle in cupon.DetalleCuponesImpresos)
                                {
                                    ws.Cells[string.Format("C{0}", fila)].Value = detalle.CodMaq;
                                    ws.Cells[string.Format("D{0}", fila)].Value = detalle.SerieIni;
                                    ws.Cells[string.Format("E{0}", fila)].Value = detalle.SerieFin;
                                    ws.Cells[string.Format("F{0}", fila)].Value = detalle.CantidadCuponesImpresos;
                                    ws.Cells[string.Format("G{0}", fila)].Value = detalle.CoinOutAnterior;
                                    ws.Cells[string.Format("H{0}", fila)].Value = detalle.CoinOut;
                                    ws.Cells[string.Format("I{0}", fila)].Value = detalle.CurrentCredits;
                                    ws.Cells[string.Format("J{0}", fila)].Value = detalle.Monto;
                                    ws.Cells[string.Format("K{0}", fila)].Value = detalle.Token;
                                    ws.Cells[string.Format("L{0}", fila)].Value = detalle.CoinOutIas;
                                    ws.Cells[string.Format("M{0}", fila)].Value = detalle.FechaRegistro.ToString("dd/MM/yyyy");
                                    fila++;
                                }
                                //Agregar una fila extra en el detalle para que no se confundan con el collapse
                                fila++;
                                finGrupo = fila - 1;
                                for (var i = inicioGrupo; i <= finGrupo; i++)
                                {
                                    ws.Row(i).OutlineLevel = 1;
                                    ws.Row(i).Collapsed = true;
                                }
                            }
                            else
                            {
                                ws.Cells[string.Format("C{0}", fila)].Value = "No se encontraron detalles para este registro";
                                ws.Cells[string.Format("C{0}:M{0}", fila)].Style.Font.Bold = true;
                                ws.Cells[string.Format("C{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Cells[string.Format("C{0}:M{0}", fila)].Merge = true;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;
                                //Agregar una fila extra en el detalle para que no se confundan con el collapse
                                fila++;
                                ws.Row(fila).OutlineLevel = 1;
                                ws.Row(fila).Collapsed = true;

                                fila++;
                            }
                            //Fin Detalle
                        }

                        fila++;
                        ws.Cells["A:AZ"].AutoFitColumns();
                    }
                }
                excelName = "CampaniaSorteo_" + DateTime.Now.ToUniversalTime() + ".xlsx";
                var memoryStream = new MemoryStream();
                excel.SaveAs(memoryStream);
                base64String = Convert.ToBase64String(memoryStream.ToArray());
                mensaje = "Descargando Archivo";
                respuesta = true;
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new
            {
                data = base64String,
                excelName,
                respuesta,
                mensaje
            };
            var result = new ContentResult
            {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;
            //return Json(new { data = listaCampanias.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        [HttpPost]
        public ActionResult CampaniaTicketsObtenerJson(Int64 campaniaid)
        {
            var errormensaje = "";
            var tickets = new List<CMP_TicketEntidad>();
            try
            {
                tickets = ticketbl.CMPTicketCampañaListadoJson(campaniaid);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = tickets, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult ClientesCampaniaIDObtenerJson(Int64 id)
        {
            var errormensaje = "";

            var campania = new List<CMP_ClienteEntidad>();
            try
            {
                campania = clientebl.GetClientesCampaniaJson(id);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = campania, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult GetListadoCuponesxCampaniaReporte(int CampaniaId, DateTime fechaInicio, DateTime fechaFin,string UrlProgresivoSala)
        {
            string mensaje = "";
            List<CMP_CuponesGeneradosEntidad> lista = new List<CMP_CuponesGeneradosEntidad>();

            bool respuesta = false;
            try
            {
                //lista = cuponesBL.GetListadoCuponesxCampania(CampaniaId);
                //var query = from l in lista
                //            where l.Fecha >= fechaInicio
                //            && l.Fecha <= fechaFin
                //            select l;
                //lista = query.ToList();
                lista = ListarSesionCuponesClientePorFechaYCampaniaId(CampaniaId, UrlProgresivoSala, fechaInicio, fechaFin);
                respuesta = true;
                mensaje = "Listando Registros";
            }
            catch (Exception exp)
            {
                mensaje = exp.Message;
            }
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var result = new ContentResult
            {
                Content = serializer.Serialize(new { mensaje,respuesta,data=lista}),
                ContentType = "application/json"
            };
            return result;
            //return Json(new { respuesta , mensaje ,data=listaCupones });
        }
        #region Consultas al Servicio
        [seguridad(false)]
        public List<CMP_SesionCuponesClienteEntidad> ListarSesionesPorFechasJsonServicio(string UrlProgresivoSala, DateTime fechaInicio,DateTime fechaFin, string nroDocumento = "")
        {
            List<CMP_SesionCuponesClienteEntidad> listaSesiones = new List<CMP_SesionCuponesClienteEntidad>();
            object oEnvio = new object();
            if(string.IsNullOrEmpty(UrlProgresivoSala)) {
                return (listaSesiones);
            }
            try
            {
                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                oEnvio = new
                {
                    fechaIni=fechaInicio,
                    fechaFin=fechaFin,
                    nroDocumento = nroDocumento

                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                string mensaje = "";
                bool respuesta = false;
                var client = new System.Net.WebClient();
                var response = "";
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                string url = UrlProgresivoSala;
                url += "/servicio/ListarSesionesPorFechasJson";
                response = client.UploadString(url, "POST", inputJson);
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                mensaje = jsonObj.mensaje;
                respuesta = Convert.ToBoolean(jsonObj.respuesta);
                if (respuesta)
                {
                    var items = jsonObj.data;
                    foreach (var myItem in items)
                    {
                        CMP_SesionCuponesClienteEntidad campania = new CMP_SesionCuponesClienteEntidad() {
                            SesionId = ManejoNulos.ManageNullInteger64(myItem.SesionId),
                            CodMaquina =ManejoNulos.ManageNullStr(myItem.CodMaquina),
                            Terminado =ManejoNulos.ManageNullInteger(myItem.Terminado),
                            Fecha = ManejoNulos.ManageNullDate(myItem.Fecha),
                            ClienteId = ManejoNulos.ManageNullInteger( myItem.ClienteId),
                            NombreCliente = ManejoNulos.ManageNullStr( myItem.NombreCliente),
                            NombreSala = ManejoNulos.ManageNullStr( myItem.NombreSala),
                            NroDocumento = ManejoNulos.ManageNullStr( myItem.NroDocumento),
                            Estado_Envio = ManejoNulos.ManageNullInteger( myItem.Estado_Envio),
                            UsuarioIdIAS = ManejoNulos.ManageNullInteger( myItem.UsuarioIdIAS),
                            Prefijo = ManejoNulos.ManageNullStr( myItem.Prefijo),
                            CoinOutIAS = ManejoNulos.ManageNullDouble( myItem.CoinOutIAS),
                            TopeCuponesxJugada = ManejoNulos.ManageNullInteger( myItem.TopeCuponesxJugada),
                            ParametrosImpresion = ManejoNulos.ManageNullStr(myItem.ParametrosImpresion),
                            CantidadCupones = ManejoNulos.ManageNullInteger(myItem.CantidadCupones),
                            CantidadJugadas = ManejoNulos.ManageNullInteger(myItem.CantidadJugadas),
                            CampaniaId = ManejoNulos.ManageNullInteger(myItem.CampaniaId),
                            Correo = ManejoNulos.ManageNullStr(myItem.Correo),
                            SerieIni = ManejoNulos.ManageNullStr(myItem.SerieIni),
                            SerieFin = ManejoNulos.ManageNullStr(myItem.SerieFin),
                        };
                        listaSesiones.Add(campania);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                listaSesiones.Clear();
            }
            return listaSesiones;
        }
        [seguridad(false)]
        public List<CMP_CuponesGeneradosEntidad> ListarSesionCuponesClientePorFechaYCampaniaId(int campaniaId, string UrlProgresivoSala,DateTime fechaIni,DateTime fechaFin)
        {
            List<CMP_CuponesGeneradosEntidad> listaRespuesta = new List<CMP_CuponesGeneradosEntidad>();
            object oEnvio = new object();
            if(string.IsNullOrEmpty(UrlProgresivoSala)) {
                return (listaRespuesta);
            }
            try
            {
                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                oEnvio = new
                {
                    CampaniaId = campaniaId,
                    fechaIni=fechaIni,
                    fechaFin=fechaFin,
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                string mensaje = "";
                bool respuesta = false;
                var client = new System.Net.WebClient();
                var response = "";
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                string url = UrlProgresivoSala;
                url += "/servicio/ListarSesionCuponesClientePorFechaYCampaniaId";
                response = client.UploadString(url, "POST", inputJson);
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                mensaje = jsonObj.mensaje;
                respuesta = Convert.ToBoolean(jsonObj.respuesta);
                if (respuesta)
                {
                    var items = jsonObj.data;
                    foreach (var myItem in items)
                    {
                        List<CMP_DetalleCuponesGeneradosEntidad> listaDetalles = new List<CMP_DetalleCuponesGeneradosEntidad>();
                        CMP_CuponesGeneradosEntidad contador = new CMP_CuponesGeneradosEntidad()
                        {
                            CgId = myItem.CgId,
                            CampaniaId = myItem.CampaniaId,
                            ClienteId = myItem.ClienteId,
                            ApelPat = string.Empty,
                            ApelMat = string.Empty,
                            Nombre = string.Empty,
                            NombreCompleto = myItem.NombreCliente,
                            Mail = myItem.Correo,
                            FechaNacimiento = DateTime.Now,
                            NroDoc = myItem.NroDocumento,
                            CodSala = 0,
                            nombreSala = myItem.NombreSala,
                            UsuarioId = myItem.UsuarioIdIAS,
                            UsuarioNombre = myItem.UsuarioNombre,
                            SlotId = myItem.CodMaquina,
                            Juego = string.Empty,
                            Marca = string.Empty,
                            Modelo = string.Empty,
                            Win = 0,
                            Parametro = 0,
                            ValorJuego = myItem.CoinOutIAS,
                            CantidadCupones = myItem.CantidadCupones,
                            SaldoCupIni = 0,
                            SaldoCupFin = 0,
                            SerieIni = myItem.SerieIni,
                            SerieFin = myItem.SerieFin,
                            Fecha = myItem.Fecha,
                            Estado = myItem.Terminado,
                            SesionId = myItem.SesionId
                        };
                        listaRespuesta.Add(contador);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return listaRespuesta;
        }
        [seguridad(false)]
        public List<CMP_DetalleCuponesImpresosEntidad> ListarContadoresOnlineWebPorQuery(string UrlProgresivoSala,string Query)
        {
            List<CMP_DetalleCuponesImpresosEntidad> listaRespuesta = new List<CMP_DetalleCuponesImpresosEntidad>();
            object oEnvio = new object();
            if(string.IsNullOrEmpty(UrlProgresivoSala)) {
                return (listaRespuesta);
            }
            try
            {
                oEnvio = new
                {
                    query = Query
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                string mensaje = "";
                bool respuesta = false;
                var client = new System.Net.WebClient();
                var response = "";
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                string url = UrlProgresivoSala;
                url += "/servicio/ListarContadoresOnlineWebPorQuery";
                response = client.UploadString(url, "POST", inputJson);
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                mensaje = jsonObj.mensaje;
                respuesta = Convert.ToBoolean(jsonObj.respuesta);
                if (respuesta)
                {
                    var items = jsonObj.data;
                    foreach (var myItem in items)
                    {
                        List<CMP_DetalleCuponesImpresosEntidad> listaDetalles = new List<CMP_DetalleCuponesImpresosEntidad>();
                        CMP_DetalleCuponesImpresosEntidad contador = new CMP_DetalleCuponesImpresosEntidad()
                        {
                            DetImpId = 0,
                            CgId = 0,
                            CodSala = 0,
                            SerieIni = ManejoNulos.ManageNullStr(myItem.SerieIni),
                            SerieFin = ManejoNulos.ManageNullStr( myItem.SerieFin),
                            CantidadCuponesImpresos =ManejoNulos.ManageNullInteger( myItem.CantidadCupones),
                            UltimoCuponImpreso = string.Empty,
                            CoinOutIas =ManejoNulos.ManageNullDouble(  myItem.CoinOutIas),
                            CodMaq = ManejoNulos.ManageNullStr( myItem.CodMaq),
                            CoinOutAnterior = ManejoNulos.ManageNullDouble( myItem.CoinOutAnterior),
                            CoinOut = ManejoNulos.ManageNullDouble( myItem.CoinOut),
                            CurrentCredits = ManejoNulos.ManageNullDouble( myItem.CurrentCredits),
                            Monto = ManejoNulos.ManageNullDecimal( myItem.Monto),
                            Token =ManejoNulos.ManageNullDecimal(  myItem.Token),
                            FechaRegistro = ManejoNulos.ManageNullDate( myItem.FechaRegistro),
                            id = 0,
                            HandPay =ManejoNulos.ManageNullDouble( myItem.HandPay),
                            JackPot = ManejoNulos.ManageNullDouble( myItem.JackPot),
                            HandPayAnterior =ManejoNulos.ManageNullDouble( myItem.HandPayAnterior),
                            JackPotAnterior = ManejoNulos.ManageNullDouble( myItem.JackPotAnterior),
                            Cod_Cont = ManejoNulos.ManageNullInteger64( myItem.Cod_Cont),
                            SesionId=ManejoNulos.ManageNullInteger64(myItem.SesionId)
                        };
                        listaRespuesta.Add(contador);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return listaRespuesta;
        }
        [seguridad(false)]
        public List<CMP_CuponesGeneradosEntidad> ListarSesionCuponesClientePorQuery(string Query, string UrlProgresivoSala)
        {
            List<CMP_CuponesGeneradosEntidad> listaRespuesta = new List<CMP_CuponesGeneradosEntidad>();
            object oEnvio = new object();
            if(string.IsNullOrEmpty(UrlProgresivoSala)) {
                return (listaRespuesta);
            }
            try
            {
                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                oEnvio = new
                {
                    query=Query
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                string mensaje = "";
                bool respuesta = false;
                var client = new System.Net.WebClient();
                var response = "";
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                string url = UrlProgresivoSala;
                url += "/servicio/ListarSesionCuponesClientePorQuery";
                response = client.UploadString(url, "POST", inputJson);
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                mensaje = jsonObj.mensaje;
                respuesta = Convert.ToBoolean(jsonObj.respuesta);
                if (respuesta)
                {
                    var items = jsonObj.data;
                    foreach (var myItem in items)
                    {
                        List<CMP_DetalleCuponesGeneradosEntidad> listaDetalles = new List<CMP_DetalleCuponesGeneradosEntidad>();
                        CMP_CuponesGeneradosEntidad contador = new CMP_CuponesGeneradosEntidad()
                        {
                            CgId = myItem.CgId,
                            CampaniaId = myItem.CampaniaId,
                            ClienteId = myItem.ClienteId,
                            ApelPat = string.Empty,
                            ApelMat = string.Empty,
                            Nombre = string.Empty,
                            NombreCompleto = myItem.NombreCliente,
                            Mail = myItem.Correo,
                            FechaNacimiento = DateTime.Now,
                            NroDoc = myItem.NroDocumento,
                            CodSala = 0,
                            nombreSala = myItem.NombreSala,
                            UsuarioId = myItem.UsuarioIdIAS,
                            UsuarioNombre = myItem.UsuarioNombre,
                            SlotId = myItem.CodMaquina,
                            Juego = string.Empty,
                            Marca = string.Empty,
                            Modelo = string.Empty,
                            Win = 0,
                            Parametro = 0,
                            ValorJuego = myItem.CoinOutIAS,
                            CantidadCupones = myItem.CantidadCupones,
                            SaldoCupIni = 0,
                            SaldoCupFin = 0,
                            SerieIni = myItem.SerieIni,
                            SerieFin = myItem.SerieFin,
                            Fecha = myItem.Fecha,
                            Estado = myItem.Terminado,
                            SesionId = myItem.SesionId,
                        };
                        listaRespuesta.Add(contador);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return listaRespuesta;
        }

        [seguridad(false)]
        public List<CMP_DetalleCuponesImpresosEntidad> CMPContadoresOnlineReporteExcel(int campaniaId, string UrlProgresivoSala, DateTime fechaIni, DateTime fechaFin)
        {
            List<CMP_DetalleCuponesImpresosEntidad> listaRespuesta = new List<CMP_DetalleCuponesImpresosEntidad>();
            object oEnvio = new object();
            if(string.IsNullOrEmpty(UrlProgresivoSala)) {
                return (listaRespuesta);
            }
            try
            {
                oEnvio = new
                {
                    CampaniaId = campaniaId,
                    fechaIni = fechaIni,
                    fechaFin = fechaFin,
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                string mensaje = "";
                bool respuesta = false;
                var client = new MyWebClient();
                var response = "";
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                string url = UrlProgresivoSala;
                url += "/servicio/CMPContadoresOnlineReporteExcel";
                response = client.UploadString(url, "POST", inputJson);
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                mensaje = jsonObj.mensaje;
                respuesta = Convert.ToBoolean(jsonObj.respuesta);
                if (respuesta)
                {
                    var items = jsonObj.data;
                    foreach (var myItem in items)
                    {
                        List<CMP_DetalleCuponesImpresosEntidad> listaDetalles = new List<CMP_DetalleCuponesImpresosEntidad>();
                        CMP_DetalleCuponesImpresosEntidad contador = new CMP_DetalleCuponesImpresosEntidad()
                        {
                            DetImpId = 0,
                            CgId = 0,
                            CodSala = 0,
                            SerieIni = ManejoNulos.ManageNullStr(myItem.SerieIni),
                            SerieFin = ManejoNulos.ManageNullStr(myItem.SerieFin),
                            CantidadCuponesImpresos = ManejoNulos.ManageNullInteger(myItem.CantidadCupones),
                            UltimoCuponImpreso = string.Empty,
                            CoinOutIas = ManejoNulos.ManageNullDouble(myItem.CoinOutIas),
                            CodMaq = ManejoNulos.ManageNullStr(myItem.CodMaq),
                            CoinOutAnterior = ManejoNulos.ManageNullDouble(myItem.CoinOutAnterior),
                            CoinOut = ManejoNulos.ManageNullDouble(myItem.CoinOut),
                            CurrentCredits = ManejoNulos.ManageNullDouble(myItem.CurrentCredits),
                            Monto = ManejoNulos.ManageNullDecimal(myItem.Monto),
                            Token = ManejoNulos.ManageNullDecimal(myItem.Token),
                            FechaRegistro = ManejoNulos.ManageNullDate(myItem.FechaRegistro),
                            id = 0,
                            HandPay = ManejoNulos.ManageNullDouble(myItem.HandPay),
                            JackPot = ManejoNulos.ManageNullDouble(myItem.JackPot),
                            HandPayAnterior = ManejoNulos.ManageNullDouble(myItem.HandPayAnterior),
                            JackPotAnterior = ManejoNulos.ManageNullDouble(myItem.JackPotAnterior),
                            Cod_Cont = ManejoNulos.ManageNullInteger64(myItem.Cod_Cont),
                            SesionId = ManejoNulos.ManageNullInteger64(myItem.SesionId)
                        };
                        listaRespuesta.Add(contador);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                listaRespuesta = new List<CMP_DetalleCuponesImpresosEntidad>();
            }
            return listaRespuesta;
        }
        #endregion
    }
}