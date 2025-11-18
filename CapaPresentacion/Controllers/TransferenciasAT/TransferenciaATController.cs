using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CapaPresentacion.Controllers.TransferenciasAT
{
    [seguridad]
    public class TransferenciaATController : Controller
    {
        private TicketBL ticketbl = new TicketBL();
        private SalaBL salaBl = new SalaBL();
        private SEG_PermisoRolBL seg_PermisoRolBL = new SEG_PermisoRolBL();
        // GET: TransferenciaAT
        public ActionResult FormularioRegistro()
        {
            return View("~/Views/TransferenciasAT/FormularioRegistroVista.cshtml");
        }

        [HttpPost]
        public ActionResult ConsultadataTicket(string url, string nroticket)
        {
            var client = new System.Net.WebClient();
            var response = "";
            Det0001TTO_00H dataticket = new Det0001TTO_00H();
            var jsonResponse = new objetoAT();
            bool permiso = true;
            try
            {

                if (url == ""){
                    return Json(new { respuesta = false,mensaje="Configure URL de Sala" });
                }

                string accion = "PermisoVerDetalleTicket";
                var permisol = seg_PermisoRolBL.GetPermisoRolUsuario((int)Session["rol"], accion);
                if (permisol.Count == 0)
                {
                    permiso = false;
                }

                dataticket.Tito_NroTicket = nroticket;
                string parameters = (new JavaScriptSerializer()).Serialize(dataticket);

                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                response = client.UploadString(url, "POST", parameters);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                jsonResponse = JsonConvert.DeserializeObject<objetoAT>(response, settings);
            }
            catch (Exception ex)
            {
                jsonResponse.respuesta = false;
                jsonResponse.mensaje = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { data= jsonResponse });
            }
            var jsonResult = Json(new { data = jsonResponse,permiso }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        [HttpPost]
        public ActionResult CobrarTicket(string url, Det0001TTO_00H ticket)
        {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new objetoAT();
            try
            {
                ticket.Punto_venta_fin = "31";
                if (url == "")
                {
                    return Json(new { respuesta = false, mensaje = "Configure URL de Sala" });
                }

                string parameters = (new JavaScriptSerializer()).Serialize(ticket);

                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                response = client.UploadString(url, "POST", parameters);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                jsonResponse = JsonConvert.DeserializeObject<objetoAT>(response, settings);
                if (jsonResponse.respuesta)
                {
                    HistorialTicketAT historialticket = new HistorialTicketAT();
                    historialticket.Item= ticket.Item;
                    historialticket.Tito_NroTicket = ticket.Tito_NroTicket;
                    historialticket.Tito_MontoTicket = ticket.Tito_MontoTicket; 
                    historialticket.MaquinaCaja = ticket.MaquinaCaja;
                    historialticket.tipo_ticket = ticket.tipo_ticket;
                    historialticket.Tito_fechaini = ticket.Tito_fechaini;
                    historialticket.juego = ticket.juego;
                    historialticket.marca = ticket.marca;
                    historialticket.codAperturaCajaIni = ticket.codAperturaCajaIni;
                    historialticket.CodigoMaquina = ticket.CodigoMaquina;
                    historialticket.IdTipoMoneda = ticket.IdTipoMoneda;
                    historialticket.fecharegistro = DateTime.Now;
                    historialticket.CodSala = ticket.CodSala;
                    historialticket.UsuarioID= Convert.ToInt32(Session["UsuarioID"]);

                    var insertarticketHistorial = ticketbl.TicketATInsertarJson(historialticket);
                }
            }
            catch (Exception ex)
            {
                jsonResponse.respuesta = false;
                jsonResponse.mensaje = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { data = jsonResponse });
            }
            var jsonResult = Json(new { data = jsonResponse }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult ReporteFormularioRegistro()
        {
            return View("~/Views/TransferenciasAT/ReporteFormularioRegistroVista.cshtml");
        }

        [HttpPost]
        public JsonResult reporteFormularioRegistroJson(int[] codsala, DateTime fechaini, DateTime fechafin)
        {
            var errormensaje = "";
            var lista = new List<HistorialTicketAT>();
            int cantElementos = (codsala == null) ? 0 : codsala.Length;
            var strElementos = String.Empty;
            var strUsuario = String.Empty;
            double totalmonto = 0;
            try
            {

                string accion = "PermisoVistaReporteUsuario";
                var permiso = seg_PermisoRolBL.GetPermisoRolUsuario((int)Session["rol"], accion);
                if (permiso.Count==0)
                {
                    strUsuario = " h.UsuarioID = "+(int)Session["UsuarioID"]+" and";
                }

                if (cantElementos > 0)
                {
                    strElementos = " h.[Codsala] in(" + "'" + String.Join("','", codsala) + "'" + ") and ";
                }

                lista = ticketbl.TicketATSalaListarJson(fechaini, fechafin, strElementos, strUsuario);
                totalmonto = lista.Select(x => x.Tito_MontoTicket).Sum();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), total = totalmonto, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult reporteFormularioRegistroDescargarExcelJson(int[] codsala, DateTime fechaini, DateTime fechafin)
        {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<HistorialTicketAT> lista = new List<HistorialTicketAT>();
            int cantElementos = (codsala == null) ? 0 : codsala.Length;
            var strElementos = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;
            List<SolicitudTicket> tickets = new List<SolicitudTicket>();
            var strUsuario = String.Empty;
            try
            {
                string accion = "PermisoVistaReporteUsuario";
                var permiso = seg_PermisoRolBL.GetPermisoRolUsuario((int)Session["rol"], accion);
                if (permiso.Count == 0)
                {
                    strUsuario = " h.UsuarioID = " + (int)Session["UsuarioID"] + " and";
                }

                if (cantElementos > 0)
                {
                    for (int i = 0; i < codsala.Length; i++)
                    {
                        var salat = salaBl.SalaListaIdJson(codsala[i]);
                        nombresala.Add(salat.Nombre);
                    }
                    salasSeleccionadas = String.Join(",", nombresala);

                    strElementos = " h.[Codsala] in(" + "'" + String.Join("','", codsala) + "'" + ") and ";
                }

                lista = ticketbl.TicketATSalaListarJson(fechaini, fechafin, strElementos, strUsuario);
                if (lista.Count > 0)
                {
                    double totalmonto = lista.Select(x => x.Tito_MontoTicket).Sum();
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("HistorialTicketsCobradosAT");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[2, 2].Value = "SALAS : " + salasSeleccionadas;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Fecha Registro";
                    workSheet.Cells[3, 4].Value = "Sala";
                    workSheet.Cells[3, 5].Value = "Nro. Ticket";
                    workSheet.Cells[3, 6].Value = "Monto";
                    workSheet.Cells[3, 7].Value = "Fecha Ticket";
                    workSheet.Cells[3, 8].Value = "Maquina";
                    workSheet.Cells[3, 9].Value = "Usu. Reg.";
                    
                    //Body of table  
                    //  
                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach (var registro in lista)
                    {

                        workSheet.Cells[recordIndex, 2].Value = registro.id;
                        workSheet.Cells[recordIndex, 3].Value = registro.fecharegistro.ToString("dd-MM-yyyy hh:mm:ss");
                        workSheet.Cells[recordIndex, 4].Value = registro.nombreSala;
                        workSheet.Cells[recordIndex, 5].Value = registro.Tito_NroTicket;
                        workSheet.Cells[recordIndex, 6].Value = registro.Tito_MontoTicket;
                        workSheet.Cells[recordIndex, 7].Value = registro.Tito_fechaini.ToString("dd-MM-yyyy hh:mm:ss");
                        workSheet.Cells[recordIndex, 8].Value = registro.CodigoMaquina;
                        workSheet.Cells[recordIndex, 9].Value = registro.UsuarioNombre;


                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:I3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:I3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:I3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:I3"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B3:I3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:I3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:I3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:I3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B3:I3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:I3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:I3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:I3"].Style.Border.Bottom.Color.SetColor(colborder);

                    //workSheet.Cells.AutoFitColumns(60, 250);//overloaded min and max lengths
                    int filasagregadas = 3;
                    total = filasagregadas + total;

                    workSheet.Cells["B4:B" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["I4:I" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                   
                    workSheet.Cells["F4:F" + total].Style.Numberformat.Format = "#,##0.00";

                    workSheet.Cells["B2:I2"].Merge = true;
                    workSheet.Cells["B2:I2"].Style.Font.Bold = true;

                    int filaFooter = total + 1;
                    workSheet.Cells["B" + filaFooter + ":E" + filaFooter].Merge = true;
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter + ":F" + filaFooter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells[filaFooter, 2].Value = "Total Monto : ";
                    workSheet.Cells[filaFooter, 6].Value = totalmonto;
                    workSheet.Cells[filaFooter, 6].Style.Numberformat.Format = "#,##0.00";
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.Font.Size = 14;

                  
                    int filaFooter_ = total + 2;
                    workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Merge = true;
                    workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[filaFooter_, 2].Value = "Total : " + (total - filasagregadas) + " Registros";

                    int filaultima = total;
                    workSheet.Cells[3, 2, filaultima, 9].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 20;
                    workSheet.Column(4).Width = 25;
                    workSheet.Column(5).Width = 25;
                    workSheet.Column(6).Width = 25;
                    workSheet.Column(7).Width = 30;
                    workSheet.Column(8).Width = 28;
                    workSheet.Column(9).Width = 27;
                    excelName = "HistorialTicketsCobradosAT_" + fechaini.ToString("dd_MM_yyyy") + "_al_" + fechafin.ToString("dd_MM_yyyy") + "_.xlsx";
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());

                    mensaje = "Descargando Archivo";
                    respuesta = true;
                }
                else
                {
                    mensaje = "No se Pudo generar Archivo";
                }

            }
            catch (Exception exp)
            {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });

        }

        public bool PermisoVistaReporteUsuario()
        {
            return true;
        }

        public bool PermisoVerDetalleTicket()
        {
            return true;
        }
    }
}