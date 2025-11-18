using CapaEntidad;
using CapaPresentacion.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaNegocio;
using System.Text.RegularExpressions;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;
using System.Web.Script.Serialization;
using System.Text;
using System.Configuration;

namespace CapaPresentacion.Controllers
{
    [seguridad]
    public class TransferenciasController : Controller
    {
        private TransferenciaBL transferenciaBl = new TransferenciaBL();
        private SalaBL salaBl = new SalaBL();
        private ClienteBl clienteBl = new ClienteBl();
        private BancoCuentaBL bancocuentaBl = new BancoCuentaBL();
        private DepositoBL depositobl = new DepositoBL();
        private TicketBL ticketbl = new TicketBL();
        private SolicitudTransferenciaBL solicitudtransferenciabl = new SolicitudTransferenciaBL();
        private SolicitudTicketBL solicitudticketbl = new SolicitudTicketBL();
        // GET: Transferencias
        public ActionResult Formulario()
        {
            return View("~/Views/Transferencias/TransferenciaFormularioVista.cshtml");
        }

        public ActionResult Reporte()
        {
            return View("~/Views/Transferencias/TransferenciaReporteVista.cshtml");
        }

        public ActionResult ReporteDepositos()
        {
            return View("~/Views/Transferencias/DepositoReporteVista.cshtml");
        }
        public ActionResult Cliente()
        {
            return View("~/Views/Transferencias/ClienteVista.cshtml");
        }

        [HttpPost]
        public ActionResult ConsultaTransferenciasPendienteSalaJson(string url,DateTime fechaini,DateTime fechafin)
        {
            var client = new System.Net.WebClient();
            var response = "";

            var jsonResponse = new List<Transferencia>();
            try
            {
                string parameters = "fechaini=" + fechaini + "&fechafin=" + fechafin;

                client.Headers.Add("content-type", "application/x-www-form-urlencoded");
                response = client.UploadString(url, "POST",parameters);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                jsonResponse = JsonConvert.DeserializeObject<List<Transferencia>>(response, settings);
               
            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            var jsonResult = Json(new { data = jsonResponse.ToList() }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult PagarTransferenciaPendiente(string url, Transferencia datapendiente)
        {
            var client = new System.Net.WebClient();
            var response = "";
            string pathArchivos = ConfigurationManager.AppSettings["IASPathImagenVoucher"].ToString();
            bool jsonResponse;
            string extension = string.Empty;
            try
            {
                var pic = System.Web.HttpContext.Current.Request.Files["Image"];
                if (pic.ContentLength > 0 && pic != null)
                {
                    extension = Path.GetExtension(pic.FileName).ToLower();
                    if (extension == ".jpg" || extension == ".png" || extension == ".jpeg")
                    {
                        
                        if (!Directory.Exists(pathArchivos))
                        {
                            Directory.CreateDirectory(pathArchivos);
                        }
                    }
                    else
                    {
                        return Json(new { data = false });
                    }
                }

                datapendiente.Estado = 1;
                datapendiente.FechaReg = DateTime.Now;
                datapendiente.FechaRegString = Convert.ToString(DateTime.Now);
                datapendiente.FechaAct = DateTime.Now;
                datapendiente.UsuarioID = Convert.ToInt32(Session["UsuarioID"]);
                datapendiente.ImagenVoucher = "";
                datapendiente.UsuarioNombreReg = Session["UsuarioNombre"].ToString();
                int idInsertar = transferenciaBl.TransferenciaInsertarJson(datapendiente);

                if (idInsertar > 0)
                {
                    var nombre = string.Empty;
                    string nombreArchivo = "voucher_" + idInsertar;
                    nombre = (nombreArchivo + extension);
                    var imagePath = Path.Combine(pathArchivos, nombre);
                    pic.SaveAs(imagePath);

                    datapendiente.TransferenciaID = idInsertar;
                    string parameters = "TransferenciaID=" + datapendiente.TransferenciaID + "&Estado=1&NroOperacion=" + datapendiente.NroOperacion + "&Observacion=" + datapendiente.Observacion + "&FechaOperacion=" + datapendiente.FechaOperacion;

                    var direccionFoto = imagePath;
                    if (System.IO.File.Exists(direccionFoto))
                    {
                        byte[] bmp = ImageToBinary(direccionFoto);
                        datapendiente.imagenbase64voucher = Convert.ToBase64String(bmp);
                        datapendiente.ImagenVoucher = nombre;
                    }

                    bool voucherTransferencia = transferenciaBl.TransferenciaImagenModificarJson(idInsertar, nombre);
                    bool solicitudEstado = transferenciaBl.SolicitudTransferenciaEstadonModificarJson(datapendiente.SolicitudTransferenciaID);

                    var solicitud = transferenciaBl.SolicitudTransferenciaIDJson(datapendiente.SolicitudTransferenciaID);
                    if (solicitud.SolicitudID > 0)
                    {
                        datapendiente.SolicitudTransferenciaID = solicitud.SolicitudSala;
                    }
                    
                    string inputJson = (new JavaScriptSerializer()).Serialize(datapendiente);
                    client.Headers.Add("content-type", "application/json");
                    client.Encoding = Encoding.UTF8;
                    response = client.UploadString(url, "POST", inputJson);

                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    jsonResponse = true;
                    //jsonResponse = Convert.ToBoolean(response);
                }
                else
                {
                    jsonResponse = false;
                }

            }
            catch (Exception ex)
            {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            var jsonResult = Json(new { data = jsonResponse }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult BuscarTransferenciasJson(DateTime fechaini, DateTime fechafin)
        {
            var errormensaje = "";
            var lista = new List<Transferencia>();
            try
            {
                lista = transferenciaBl.TransferenciaBuscarListarJson(fechaini, fechafin);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        public JsonResult ReporteTransferenciasSalaJson(int[] codsala, DateTime fechaini, DateTime fechafin)
        {
            var errormensaje = "";
            var lista = new List<Transferencia>();
            int cantElementos = (codsala == null) ? 0 : codsala.Length;
            var strElementos = String.Empty;
            double totalmonto = 0;
            try
            {
                if (cantElementos > 0)
                {
                    strElementos = " tr.[Codsala] in(" + "'" + String.Join("','", codsala) + "'" + ") and ";
                }


                lista = transferenciaBl.TransferenciaSalasListarJson(fechaini, fechafin, strElementos);
                totalmonto = lista.Select(x => x.Monto).Sum();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), total = totalmonto, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult ReporteTransferenciaDescargarExcelJson(int[] codsala, DateTime fechaini, DateTime fechafin)
        {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<Transferencia> lista = new List<Transferencia>();
            int cantElementos = (codsala == null) ? 0 : codsala.Length;
            var strElementos = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;
            List<SolicitudTicket> tickets = new List<SolicitudTicket>();
            try
            {
                
 
                if (cantElementos > 0)
                {
                    for (int i = 0; i < codsala.Length; i++)
                    {
                        var salat = salaBl.SalaListaIdJson(codsala[i]);
                        nombresala.Add(salat.Nombre);
                    }
                    salasSeleccionadas = String.Join(",", nombresala);

                    strElementos = " tr.[Codsala] in(" + "'" + String.Join("','", codsala) + "'" + ") and ";
                }

                lista = transferenciaBl.TransferenciaSalasListarJson(fechaini, fechafin, strElementos);
                if (lista.Count>0)
                {
                    double totalmonto = lista.Select(x => x.Monto).Sum();
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Tranferencias");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[2, 2].Value = "SALAS : "+ salasSeleccionadas;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Sala";
                    workSheet.Cells[3, 4].Value = "Fecha Registro";
                    workSheet.Cells[3, 5].Value = "Tipo Doc";
                    workSheet.Cells[3, 6].Value = "Nro Doc";
                    workSheet.Cells[3, 7].Value = "Cliente";
                    workSheet.Cells[3, 8].Value = "Banco";
                    workSheet.Cells[3, 9].Value = "Cuenta";
                    workSheet.Cells[3, 10].Value = "Monto";
                    workSheet.Cells[3, 11].Value = "Nro Operacion";
                    workSheet.Cells[3, 12].Value = "Fecha Operacion";
                    workSheet.Cells[3, 13].Value = "Usu. Reg.";
                    workSheet.Cells[3, 14].Value = "Usu. Sala";
                    workSheet.Cells[3, 15].Value = "Cant. Ticket";
                    workSheet.Cells[3, 16].Value = "Ticket";
                    //Body of table  
                    //  
                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach (var registro in lista)
                    {

                        string registroticket = string.Empty;
                        //workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                        tickets = transferenciaBl.TicketSolicitudIDListadoJson(registro.SolicitudTransferenciaID);
                        if (tickets.Count > 0)
                        {
                            int cantid = tickets.Count;
                            int i = 0;
                            foreach (var regTicket in tickets)
                            {
                                i++;
                                if (i == cantid)
                                {
                                    registroticket += regTicket.NroTicketTito + " - S/." + regTicket.Monto.ToString("#,##0.00");
                                }
                                else
                                {
                                    registroticket += regTicket.NroTicketTito + " - S/." + regTicket.Monto.ToString("#,##0.00") + Environment.NewLine;
                                }

                            }
                        }

                        workSheet.Cells[recordIndex, 2].Value = registro.TransferenciaID;
                        workSheet.Cells[recordIndex, 3].Value = registro.nombresala;
                        workSheet.Cells[recordIndex, 4].Value = registro.FechaReg.ToString("dd-MM-yyyy hh:mm:ss");
                       
                        workSheet.Cells[recordIndex, 5].Value = registro.TipoDocNombre;
                        workSheet.Cells[recordIndex, 6].Value = registro.ClienteNroDoc;
                        workSheet.Cells[recordIndex, 7].Value = registro.ClienteApelPat + " " + registro.ClienteApelMat + " " + registro.ClienteNombre;
                        workSheet.Cells[recordIndex, 8].Value = registro.BancoNombre;
                        workSheet.Cells[recordIndex, 9].Value = registro.NroCuenta;
                        workSheet.Cells[recordIndex, 10].Value = registro.Monto;
                        workSheet.Cells[recordIndex, 11].Value = registro.NroOperacion;
                        workSheet.Cells[recordIndex, 12].Value = registro.FechaOperacion.ToString("dd-MM-yyyy");
                        workSheet.Cells[recordIndex, 13].Value = registro.UsuarioNombre;
                        workSheet.Cells[recordIndex, 14].Value = registro.usuariosala;
                        workSheet.Cells[recordIndex, 15].Value = registro.NroTickets;
                        workSheet.Cells[recordIndex, 16].Value = registroticket;

                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:P3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:P3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:P3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:P3"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B3:P3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:P3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:P3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:P3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B3:P3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:P3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:P3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:P3"].Style.Border.Bottom.Color.SetColor(colborder);

                    //workSheet.Cells.AutoFitColumns(60, 250);//overloaded min and max lengths
                    int filasagregadas = 3;
                    total = filasagregadas + total;

                    workSheet.Cells["B4:B" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["L4:L" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["M4:M" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["N4:N" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["J4:J" + total].Style.Numberformat.Format = "#,##0.00";

                    workSheet.Cells["B2:P2"].Merge = true;
                    workSheet.Cells["B2:P2"].Style.Font.Bold = true;

                    int filaFooter = total + 1;
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Merge = true;
                    workSheet.Cells["B" + filaFooter + ":P" + filaFooter].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter + ":P" + filaFooter].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter + ":P" + filaFooter].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter + ":P" + filaFooter].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells[filaFooter, 2].Value = "Total Monto : ";
                    workSheet.Cells[filaFooter, 10].Value = totalmonto;
                    workSheet.Cells[filaFooter, 10].Style.Numberformat.Format = "#,##0.00";
                    workSheet.Cells["B" + filaFooter + ":P" + filaFooter].Style.Font.Size = 14;

                    workSheet.Cells["B4:K" + total].Style.WrapText = true;

                    int filaFooter_ = total + 2;
                    workSheet.Cells["B" + filaFooter_ + ":P" + filaFooter_].Merge = true;
                    workSheet.Cells["B" + filaFooter_ + ":P" + filaFooter_].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter_ + ":P" + filaFooter_].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter_ + ":P" + filaFooter_].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter_ + ":P" + filaFooter_].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter_ + ":P" + filaFooter_].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[filaFooter_, 2].Value = "Total : " + (total-filasagregadas) + " Registros";

                    int filaultima = total;
                    workSheet.Cells[3, 2, filaultima, 16].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 20;
                    workSheet.Column(4).Width = 25;
                    workSheet.Column(5).Width = 25;
                    workSheet.Column(6).Width = 25; 
                    workSheet.Column(7).Width = 30;
                    workSheet.Column(8).Width = 28;
                    workSheet.Column(9).Width = 27;
                    workSheet.Column(10).Width = 22;
                    workSheet.Column(11).Width = 14;
                    workSheet.Column(12).Width = 20;
                    workSheet.Column(13).Width = 14;

                    workSheet.Column(14).Width = 20;
                    workSheet.Column(15).Width = 20;
                    workSheet.Column(16).Width = 29;
                    excelName = "transferencias_" + fechaini.ToString("dd_MM_yyyy") + "_al_"+ fechafin.ToString("dd_MM_yyyy") + "_transferencias.xlsx";
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

        [HttpPost]
        public ActionResult DetalleTransferenciaIdJson(int id)
        {
            string mensaje = string.Empty;
            string errormensaje = string.Empty;
            bool respuesta = false;
            var registro = new SolicitudTransferencia();
            Transferencia transferencia = new Transferencia();
            List<SolicitudTicket> lista = new List<SolicitudTicket>();
            string pathArchivos = ConfigurationManager.AppSettings["IASPathImagenVoucher"].ToString();
            try
            {
                transferencia = transferenciaBl.TransferenciaIDJson(id);
                
                if (transferencia.TransferenciaID > 0)
                {
                    registro = transferenciaBl.SolicitudTransferenciaIDJson(transferencia.SolicitudTransferenciaID);
                    var nombre = string.Empty;
                    nombre = transferencia.ImagenVoucher;
                    var imagePath = Path.Combine(pathArchivos, nombre);

                    var direccionFoto = imagePath;
                    if (System.IO.File.Exists(direccionFoto))
                    {
                        byte[] bmp = ImageToBinary(direccionFoto);
                        transferencia.imagenbase64voucher = Convert.ToBase64String(bmp);

                    }
                    lista = transferenciaBl.TicketSolicitudIDListadoJson(registro.SolicitudID);
                    
                    respuesta = true;
                }
                else
                {
                    mensaje = "No se encuentró el registro";
                    respuesta = false;

                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = transferencia, lista= lista.ToList(), respuesta, mensaje, errormensaje }); ;
        }


        [HttpPost]
        public JsonResult ReporteDepositosSalaJson(int[] codsala, DateTime fechaini, DateTime fechafin)
        {
            var errormensaje = "";
            var lista = new List<DepositoEntidad>();
            int cantElementos = (codsala == null) ? 0 : codsala.Length;
            var strElementos = String.Empty;
            double totalmonto = 0;
            try
            {
                if (cantElementos > 0)
                {
                    strElementos = " de.[Codsala] in(" + "'" + String.Join("','", codsala) + "'" + ") and ";
                }

                lista = transferenciaBl.DepositosListadoJson(fechaini, fechafin, strElementos);
                totalmonto = lista.Select(x => x.Monto).Sum();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(),total= totalmonto, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult reporteDepositosDescargarExcelJson(int[] codsala, DateTime fechaini, DateTime fechafin)
        {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<DepositoEntidad> lista = new List<DepositoEntidad>();
            int cantElementos = (codsala == null) ? 0 : codsala.Length;
            var strElementos = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;

            List<TicketEntidad> tickets = new List<TicketEntidad>();
           
            try
            {


                if (cantElementos > 0)
                {
                    for (int i = 0; i < codsala.Length; i++)
                    {
                        var salat = salaBl.SalaListaIdJson(codsala[i]);
                        nombresala.Add(salat.Nombre);
                    }
                    salasSeleccionadas = String.Join(",", nombresala);

                    strElementos = " de.[Codsala] in(" + "'" + String.Join("','", codsala) + "'" + ") and ";
                }

                lista = transferenciaBl.DepositosListadoJson(fechaini, fechafin, strElementos);
                if (lista.Count > 0)
                {

                    double totalmonto = lista.Select(x => x.Monto).Sum();
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Depositos");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[2, 2].Value = "SALAS : " + salasSeleccionadas;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Sala";
                    workSheet.Cells[3, 4].Value = "Fecha Registro";
                    workSheet.Cells[3, 5].Value = "Tipo Doc";
                    workSheet.Cells[3, 6].Value = "Nro Doc";
                    workSheet.Cells[3, 7].Value = "Cliente";
                    workSheet.Cells[3, 8].Value = "Monto";
                    workSheet.Cells[3, 9].Value = "Nro Tickets";
                    workSheet.Cells[3, 10].Value = "Nro Operacion";
                    workSheet.Cells[3, 11].Value = "Tickets";
                    //Body of table  
                    //  
                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach (var registro in lista)
                    {
                        string registroticket = string.Empty;
                        //workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                        tickets = ticketbl.TicketDepositoIDListadoJson(registro.DepositoID);

                        if (tickets.Count > 0)
                        {
                            int cantid = tickets.Count;
                            int i = 0;
                            foreach (var regTicket in tickets)
                            {
                                i++;
                                if (i == cantid)
                                {
                                    registroticket += regTicket.NroTicketTito + " - S/." + regTicket.Monto.ToString("#,##0.00");
                                }
                                else
                                {
                                    registroticket += regTicket.NroTicketTito + " - S/." + regTicket.Monto.ToString("#,##0.00") + Environment.NewLine;
                                }
                                
                            }
                        }

                        workSheet.Cells[recordIndex, 2].Value = registro.DepositoID;
                        workSheet.Cells[recordIndex, 3].Value = registro.nombresala;
                        workSheet.Cells[recordIndex, 4].Value = registro.FechaReg.ToString("dd-MM-yyyy hh:mm:ss");

                        workSheet.Cells[recordIndex, 5].Value = registro.TipoDocNombre;
                        workSheet.Cells[recordIndex, 6].Value = registro.ClienteNroDoc;
                        workSheet.Cells[recordIndex, 7].Value = registro.ClienteApelPat + " " + registro.ClienteApelMat + " " + registro.ClienteNombre;
                        workSheet.Cells[recordIndex, 8].Value = registro.Monto;
                        workSheet.Cells[recordIndex, 9].Value = registro.NroTickets;
                        workSheet.Cells[recordIndex, 10].Value = registro.NroOperacion;
                        workSheet.Cells[recordIndex, 11].Value = registroticket;
                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:K3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:K3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:K3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:k3"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B3:K3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:K3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:K3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:K3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B3:K3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:K3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:K3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:K3"].Style.Border.Bottom.Color.SetColor(colborder);

                    //workSheet.Cells.AutoFitColumns(60, 250);//overloaded min and max lengths

                    int filasagregadas = 3;
                    total = filasagregadas + total;

                    workSheet.Cells["B4:B" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["J4:J" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["I4:I" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["H4:H" + total].Style.Numberformat.Format = "#,##0.00";


                    workSheet.Cells["B2:K2"].Merge = true;
                    workSheet.Cells["B2:K2"].Style.Font.Bold = true;

                    int filaFooter = total + 1;
                    workSheet.Cells["B" + filaFooter + ":G" + filaFooter].Merge = true;
                    workSheet.Cells["B" + filaFooter + ":K" + filaFooter].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter + ":K" + filaFooter].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter + ":K" + filaFooter].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter + ":K" + filaFooter].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter + ":G" + filaFooter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells[filaFooter, 2].Value = "Total Monto : ";
                    workSheet.Cells[filaFooter, 8].Value = totalmonto;
                    workSheet.Cells[filaFooter, 8].Style.Numberformat.Format = "#,##0.00";
                    workSheet.Cells["B" + filaFooter + ":K" + filaFooter].Style.Font.Size = 14;

                    workSheet.Cells["B4:K" + total].Style.WrapText = true;

                    int filaFooter_ = total + 2;
                    workSheet.Cells["B" + filaFooter_ + ":K" + filaFooter_].Merge = true;
                    workSheet.Cells["B" + filaFooter_ + ":K" + filaFooter_].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter_ + ":K" + filaFooter_].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter_ + ":K" + filaFooter_].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter_ + ":K" + filaFooter_].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter_ + ":K" + filaFooter_].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[filaFooter_, 2].Value = "Total : " + (total - filasagregadas) + " Registros";

                    int filaultima = total;
                    workSheet.Cells[3, 2, filaultima, 11].AutoFilter = true;

                    

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 20;
                    workSheet.Column(4).Width = 25;
                    workSheet.Column(5).Width = 25;
                    workSheet.Column(6).Width = 25;
                    workSheet.Column(7).Width = 30;
                    workSheet.Column(8).Width = 28;
                    workSheet.Column(9).Width = 20;
                    workSheet.Column(10).Width = 22;
                    workSheet.Column(11).Width = 23;

                    excelName = "depositos_" + fechaini.ToString("dd_MM_yyyy") + "_al_" + fechafin.ToString("dd_MM_yyyy") + "_depositos.xlsx";
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

        [HttpPost]
        public JsonResult BuscarSolicitudesJson()
        {
            var errormensaje = "";
            var lista = new List<SolicitudTransferencia>();
            try
            {
                lista = transferenciaBl.SolicitudTransferenciaActivasListarJson();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult SolicitudIdJson(int id)
        {
            string mensaje = string.Empty;
            string errormensaje = string.Empty;
            bool respuesta = false;
            var registro = new SolicitudTransferencia();
            try
            {
                registro = transferenciaBl.SolicitudTransferenciaIDJson(id);
                if(registro.SolicitudID==0)
                {
                    mensaje = "Solicitud no se encuentra";
                    respuesta = false;
                }
                else
                {
                    if (registro.Estado != 0) {
                        mensaje = "Solicitud ya aprobada";
                        respuesta = false;
                    }
                    else
                    {
                        respuesta = true;
                    }
                    
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = registro, respuesta,mensaje , errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult BuscarClientesJson(string valor)
        {
            var errormensaje = "";
            var lista = new List<Cliente>();
            try
            {
                lista = clienteBl.ClienteBuscarJson(valor);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }


        [seguridad(false)]
        [HttpPost]
        public ActionResult BuscarCuentasClientesJson(int id)
        {
            var errormensaje = "";
            var lista = new List<BancoCuentaEntidad>();
            try
            {
                lista = bancocuentaBl.BancoCuentaclienteidListadoJson(id);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListaClientesJson()
        {
            var errormensaje = "";
            var lista = new List<Cliente>();
            try
            {
                lista = clienteBl.ClienteListadoJson();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult TicketsDepositoJson(int id)
        {
            var errormensaje = "";
            var lista = new List<TicketEntidad>();
            try
            {
                lista = ticketbl.TicketDepositoIDListadoJson(id);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult Sincronizar()
        {
            var errormensaje = "";
            var response = false;
            var lista = new List<Cliente>();
            int cantidad;
            var transferenciaCliente = new List<Cliente>();
            try
            {

                transferenciaCliente = transferenciaBl.TransferenciaClientesListarJson();

                lista = clienteBl.ClienteListadoJson();

                foreach (var item in transferenciaCliente)
                {
                    cantidad = lista.Where(x => x.ClienteNombre == item.ClienteNombre && x.ClienteApelPat==item.ClienteApelPat && x.ClienteApelMat==item.ClienteApelMat && x.ClienteTipoDoc==item.ClienteTipoDoc && x.ClienteNroDoc==item.ClienteNroDoc).Count();

                    if(cantidad==0)
                    {
                        item.FechaReg = DateTime.Now;
                        clienteBl.clienteInsertarJson(item);
                    }

                }

                response = true;
                
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = response, mensaje = errormensaje });
        }

        [HttpPost]
        public JsonResult CuentasClienteJson(string tipodoc, string nrodoc,int id )
        {
            var errormensaje = "";
            //var lista = new List<Transferencia>();
            var lista = new List<BancoCuentaEntidad>();
            try
            {
                //lista = transferenciaBl.TransferenciaCuentaListadoJson(tipodoc, nrodoc);
                lista = bancocuentaBl.BancoCuentaclienteidListadoJson(id);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult ClienteExcelJson()
        {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            var lista = new List<Cliente>();
            try
            {
               
                lista = clienteBl.ClienteListadoJson();
                if (lista.Count > 0)
                {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Tranferencias");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(1).Height = 20;
                    workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(1).Style.Font.Bold = true;

                    workSheet.Cells[1, 2].Value = "ID";
                    workSheet.Cells[1, 3].Value = "Tipo Doc";
                    workSheet.Cells[1, 4].Value = "Nro Doc";
                    workSheet.Cells[1, 5].Value = "Cliente";
                  
                    //Body of table  
                    //  
                    int recordIndex = 2;
                    int total = lista.Count;
                    foreach (var registro in lista)
                    {
                        //workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                        workSheet.Cells[recordIndex, 2].Value = registro.ClienteID;
                        workSheet.Cells[recordIndex, 3].Value = registro.ClienteTipoDoc;
                        workSheet.Cells[recordIndex, 4].Value = registro.ClienteNroDoc;
                        workSheet.Cells[recordIndex, 5].Value = registro.ClienteApelPat + " " + registro.ClienteApelMat + " " + registro.ClienteNombre;
                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B1:E1"].Style.Font.Bold = true;
                    workSheet.Cells["B1:E1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B1:E1"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B1:E1"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B1:E1"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B1:E1"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B1:E1"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B1:E1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B1:E1"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B1:E1"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B1:E1"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B1:E1"].Style.Border.Bottom.Color.SetColor(colborder);

                    //workSheet.Cells.AutoFitColumns(60, 250);//overloaded min and max lengths
                    workSheet.Cells["B2:B" + total + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["E2:E" + total + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    int filaFooter = total + 2;
                    workSheet.Cells["B" + filaFooter + ":E" + filaFooter].Merge = true;
                    workSheet.Cells["B" + filaFooter + ":E" + filaFooter].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter + ":E" + filaFooter].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter + ":E" + filaFooter].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter + ":E" + filaFooter].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter + ":E" + filaFooter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[filaFooter, 2].Value = "Total : " + total + " Registros";

                    int filaultima = total + 1;
                    workSheet.Cells[1, 2, filaultima, 5].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 20;
                    workSheet.Column(4).Width = 20;
                    workSheet.Column(5).Width = 30;

                    excelName =  DateTime.Now.ToString("dd_MM_yyyy") + "_clientes.xlsx";
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
        
        [seguridad(false)]
        public static byte[] ImageToBinary(string imagePath)
        {
            FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, (int)fileStream.Length);
            fileStream.Close();
            return buffer;
        }

        #region Coneccion Online
        [seguridad(false)]
        [HttpPost]
        public ActionResult SincronizarDepositoCliente(DepositoEntidad deposito)
        {
            string mensaje = "";
            bool respuesta = false;
            bool respuestaCliente = false;
            bool respuestaDeposito = false;
            bool respuestaTickets = false;
            bool respuestaDetalleBanco = false;

            int IdCliente = 0;
            int idDeposito = 0;
            try
            {
                //Seccion Cliente
                respuesta = true;
                List<Cliente> listaClientes = new List<Cliente>();
                Cliente cliente = new Cliente();
                Cliente clienteEditar_Insertar = new Cliente();

                clienteEditar_Insertar.ClienteNombre = deposito.ClienteNombre;
                clienteEditar_Insertar.ClienteApelPat = deposito.ClienteApelPat;
                clienteEditar_Insertar.ClienteApelMat = deposito.ClienteApelMat;
                clienteEditar_Insertar.ClienteTipoDoc = deposito.TipoDocNombre;
                clienteEditar_Insertar.ClienteNroDoc = deposito.ClienteNroDoc;

                cliente = clienteBl.ClienteBuscarJson(deposito.ClienteNroDoc).Where(x=>x.ClienteTipoDoc.ToLower().Equals(deposito.TipoDocNombre.ToLower())).FirstOrDefault();
                if (cliente!=null)
                {
                    clienteEditar_Insertar.ClienteID = cliente.ClienteID;
                    clienteEditar_Insertar.FechaReg = cliente.FechaReg;
                    respuestaCliente = clienteBl.ClienteEditarJson(clienteEditar_Insertar);
                    IdCliente = cliente.ClienteID;
                }
                else
                {
                    Cliente clienteConsultaID = new Cliente();
                    clienteEditar_Insertar.FechaReg = DateTime.Now;
                    respuestaCliente = clienteBl.clienteInsertarJson(clienteEditar_Insertar);
                    clienteConsultaID= clienteBl.ClienteBuscarJson(clienteEditar_Insertar.ClienteNroDoc).Where(x => x.ClienteTipoDoc.ToLower()==clienteEditar_Insertar.ClienteTipoDoc.ToLower()).FirstOrDefault();
                    if (clienteConsultaID != null)
                    {
                        IdCliente = clienteConsultaID.ClienteID;
                        respuestaCliente = true;
                    }
                    else
                    {
                        respuestaCliente = false;
                    }
                }
                if (respuestaCliente)
                {
                    //seccion Detalle Banco
                    BancoCuentaEntidad bancoCuenta = new BancoCuentaEntidad();
                    BancoCuentaEntidad bancoCuentaConsulta = new BancoCuentaEntidad();
                    bancoCuenta.Banco = deposito.BancoNombre;
                    bancoCuenta.NroCuenta = deposito.NroCuenta;
                    if (bancoCuenta.Banco != ""&&bancoCuenta.Banco!=null&& bancoCuenta.NroCuenta!=null && bancoCuenta.NroCuenta != "")
                    {
                        bancoCuentaConsulta = bancocuentaBl.BancoCuentaclienteidListadoJson(IdCliente).Where(x => x.NroCuenta == deposito.NroCuenta && x.Banco == deposito.BancoNombre).FirstOrDefault();
                        if (bancoCuentaConsulta!= null)
                        {
                            //editar
                            bancoCuenta.BancoCuentaID = bancoCuentaConsulta.BancoCuentaID;
                            bancoCuenta.ClienteID = bancoCuentaConsulta.ClienteID;
                            respuestaDetalleBanco = bancocuentaBl.BancoCuentaEditarJson(bancoCuenta);
                        }
                        else
                        {
                            //agregar
                            bancoCuenta.ClienteID = IdCliente;
                            respuestaDetalleBanco = bancocuentaBl.BancoCuentaInsertarJson(bancoCuenta);
                        }
                    }
                    else
                    {
                        respuestaDetalleBanco = true;
                    }
                    if (deposito.DepositoID != 0)
                    {
                        //Seccion depositos
                        DepositoEntidad depositoEditar_Insertar = new DepositoEntidad();
                        DepositoEntidad depositoConsulta = new DepositoEntidad();

                        depositoEditar_Insertar.Codsala = deposito.Codsala;
                        depositoEditar_Insertar.ClienteNombre = deposito.ClienteNombre;
                        depositoEditar_Insertar.ClienteApelPat = deposito.ClienteApelPat;
                        depositoEditar_Insertar.ClienteApelMat = deposito.ClienteApelMat;
                        depositoEditar_Insertar.ClienteNroDoc = deposito.ClienteNroDoc;
                        depositoEditar_Insertar.TipoDocNombre = deposito.TipoDocNombre;
                        depositoEditar_Insertar.Monto = deposito.Monto;
                        depositoEditar_Insertar.NroTickets = deposito.NroTickets;
                        depositoEditar_Insertar.Estado = deposito.Estado;
                        depositoEditar_Insertar.NroOperacion = deposito.NroOperacion;
                        depositoEditar_Insertar.FechaReg = Convert.ToDateTime(deposito.FechaRegString);
                        depositoEditar_Insertar.FechaAct = DateTime.Now;
                        depositoEditar_Insertar.DepositoSala = deposito.DepositoID;
                        depositoEditar_Insertar.UsuarioNombreReg = deposito.UsuarioNombreReg;

                        depositoConsulta = depositobl.DepositoListarDepositoSalaJson(deposito.DepositoID).Where(x=>x.Codsala==deposito.Codsala).FirstOrDefault();

                        if (depositoConsulta != null)
                        {
                            depositoEditar_Insertar.DepositoID = depositoConsulta.DepositoID;
                            respuestaDeposito = depositobl.DepositoEditarJson(depositoEditar_Insertar);
                            idDeposito = depositoConsulta.DepositoID;
                        }
                        else
                        {
                            idDeposito = depositobl.DepositoInsertarJson(depositoEditar_Insertar);
                        }
                        if (idDeposito != 0)
                        {
                            respuestaDeposito = true;
                        }
                        else
                        {
                            respuestaDeposito = false;
                        }
                        //Seccion Tickets validar por cantidad de tickets
                        if (respuestaDeposito)
                        {
                            if (deposito.Tickets != null)
                            {
                                int totalTicketsRecibidos = deposito.Tickets.Count;
                                if (totalTicketsRecibidos != 0)
                                {
                                    int contadorTickets = 0;
                                    List<TicketEntidad> ticketConsultaLista = new List<TicketEntidad>();
                                    ticketConsultaLista = ticketbl.TicketDepositoIDListadoJson(idDeposito);
                                    foreach (var item in deposito.Tickets)
                                    {
                                        TicketEntidad ticketConsulta = new TicketEntidad();
                                        TicketEntidad ticketEditar_Insertar = new TicketEntidad();
                                        ticketEditar_Insertar.DepositoID = idDeposito;
                                        ticketEditar_Insertar.FechaReg = item.FechaReg;
                                        ticketEditar_Insertar.Monto = item.Monto;
                                        ticketEditar_Insertar.NroTicketTito = item.NroTicketTito;
                                        ticketConsulta = ticketConsultaLista.Where(x => x.NroTicketTito.Trim().Equals(item.NroTicketTito)).FirstOrDefault();
                                        if (ticketConsulta != null)
                                        {
                                            //Editar
                                            ticketEditar_Insertar.TicketID = ticketConsulta.TicketID;
                                            respuestaTickets = ticketbl.TicketEditarJson(ticketEditar_Insertar);
                                        }
                                        else
                                        {
                                            //Insertar
                                            respuestaTickets = ticketbl.TicketInsertarJson(ticketEditar_Insertar);
                                        }
                                        if (respuestaTickets)
                                        {
                                            contadorTickets++;
                                        }
                                    }
                                    if (contadorTickets == totalTicketsRecibidos)
                                    {
                                        respuestaTickets = true;
                                    }
                                    else
                                    {
                                        respuestaTickets = false;
                                    }
                                }
                                else
                                {
                                    respuestaTickets = true;
                                }
                            }
                            else
                            {
                                respuestaTickets = true;
                            }
                           
                        }
                    }
                    else
                    {
                        respuestaDeposito = true;
                        respuestaTickets = true;
                    }
                }
                if (respuestaCliente && respuestaDeposito && respuestaDetalleBanco && respuestaTickets)
                {
                    respuesta = true;
                }
                else
                {
                    respuesta = false;
                }
            }catch(Exception ex)
            {
                mensaje=ex.Message;
                respuesta = false;
            }
            return Json(respuesta);
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult SincronizarEditarCliente(Cliente cliente)
        {
            bool respuestaCliente = false;
            int IdCliente = 0;
            bool respuesta = false;
            try
            {
                Cliente clienteEditar_Insertar = new Cliente();
                Cliente clienteConsulta = new Cliente();
                clienteEditar_Insertar.ClienteNombre = cliente.ClienteNombre;
                clienteEditar_Insertar.ClienteApelPat = cliente.ClienteApelPat;
                clienteEditar_Insertar.ClienteApelMat = cliente.ClienteApelMat;
                clienteEditar_Insertar.ClienteTipoDoc = cliente.ClienteTipoDoc;
                clienteEditar_Insertar.ClienteNroDoc = cliente.ClienteNroDoc;
                clienteConsulta = clienteBl.ClienteBuscarJson(cliente.ClienteNroDoc).Where(x => x.ClienteTipoDoc.ToLower().Equals(cliente.ClienteTipoDoc.ToLower())).FirstOrDefault();
                if (cliente != null)
                {
                    clienteEditar_Insertar.ClienteID = clienteConsulta.ClienteID;
                    clienteEditar_Insertar.FechaReg = clienteConsulta.FechaReg;
                    respuesta = clienteBl.ClienteEditarJson(clienteEditar_Insertar);
                    
                }
                else
                {
                    Cliente clienteConsultaID = new Cliente();
                    clienteEditar_Insertar.FechaReg = DateTime.Now;
                    respuesta = clienteBl.clienteInsertarJson(clienteEditar_Insertar);
                    //clienteConsultaID = clienteBl.ClienteBuscarJson(clienteEditar_Insertar.ClienteNroDoc).Where(x => x.ClienteTipoDoc.ToLower() == clienteEditar_Insertar.ClienteTipoDoc.ToLower()).FirstOrDefault();
                    //if (clienteConsultaID != null)
                    //{
                    //    IdCliente = clienteConsultaID.ClienteID;
                    //    respuesta = true;
                    //}
                    //else
                    //{
                    //    respuesta= false;
                    //}
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return Json(respuesta);
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult SincronizarEditarTickets(TicketEntidad ticket)
        {
            bool respuesta = false;
            int depositoID = 0;
            try
            {
                DepositoEntidad depositoConsulta = new DepositoEntidad();

                depositoConsulta = depositobl.DepositoListarDepositoSalaJson(ticket.DepositoID).Where(x=>x.Codsala==ticket.codSala).FirstOrDefault();

                if (depositoConsulta != null)
                {
                    //Existe el deposito, entonces se puede insertar el ticket;
                    depositoID = depositoConsulta.DepositoID;
                    List<TicketEntidad> ticketConsultaLista = new List<TicketEntidad>();
                    ticketConsultaLista = ticketbl.TicketDepositoIDListadoJson(depositoID);

                    TicketEntidad ticketConsulta = new TicketEntidad();

                    TicketEntidad ticketEditar_Insertar = new TicketEntidad();

                    ticketEditar_Insertar.DepositoID = depositoID;
                    ticketEditar_Insertar.FechaReg = ticket.FechaReg;
                    ticketEditar_Insertar.Monto = ticket.Monto;
                    ticketEditar_Insertar.NroTicketTito = ticket.NroTicketTito;
                    ticketConsulta = ticketConsultaLista.Where(x => x.NroTicketTito.Trim().Equals(ticket.NroTicketTito)).FirstOrDefault();
                    if (ticketConsulta != null)
                    {
                        //Editar
                        ticketEditar_Insertar.TicketID = ticketConsulta.TicketID;
                        respuesta = ticketbl.TicketEditarJson(ticketEditar_Insertar);
                    }
                    else
                    {
                        //Insertar
                        respuesta = ticketbl.TicketInsertarJson(ticketEditar_Insertar);
                    }
                }
                else
                {
                    respuesta = false;
                }
             
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return Json(respuesta);
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult SincronizarCuentasdeBanco(Cliente cliente)
        {
            bool respuesta = false;
            bool respuestaCliente = false;
            bool respuestaDetalleBanco = false;
            int IdCliente = 0;
            try
            {
                Cliente clienteEditar_Insertar = new Cliente();
                Cliente clienteConsulta = new Cliente();

                clienteConsulta = clienteBl.ClienteBuscarJson(cliente.ClienteNroDoc).Where(x=>x.ClienteTipoDoc==cliente.ClienteTipoDoc).FirstOrDefault();
                if (clienteConsulta != null)
                {
                    cliente.ClienteID = clienteConsulta.ClienteID;
                    cliente.FechaReg = clienteConsulta.FechaReg;
                    respuestaCliente = clienteBl.ClienteEditarJson(cliente);
                    IdCliente = cliente.ClienteID;
                }
                else
                {
                    Cliente clienteConsultaID = new Cliente();
                    cliente.FechaReg = DateTime.Now;
                    respuestaCliente = clienteBl.clienteInsertarJson(cliente);
                    clienteConsultaID = clienteBl.ClienteBuscarJson(cliente.ClienteNroDoc).Where(x => x.ClienteTipoDoc.ToLower() == cliente.ClienteTipoDoc.ToLower()).FirstOrDefault();
                    if (clienteConsultaID != null)
                    {
                        IdCliente = clienteConsultaID.ClienteID;
                        respuestaCliente = true;
                    }
                    else
                    {
                        respuestaCliente = false;
                    }
                }
                if (respuestaCliente)
                {
                    BancoCuentaEntidad bancoCuenta = new BancoCuentaEntidad();
                    BancoCuentaEntidad bancoCuentaConsulta = new BancoCuentaEntidad();
                    bancoCuenta.Banco = cliente.BancoNombre;
                    bancoCuenta.NroCuenta = cliente.NroCuenta;
                    if (bancoCuenta.Banco != "" && bancoCuenta.Banco != null && bancoCuenta.NroCuenta != null && bancoCuenta.NroCuenta != "")
                    {
                        bancoCuentaConsulta = bancocuentaBl.BancoCuentaclienteidListadoJson(IdCliente).Where(x => x.NroCuenta == cliente.NroCuentaAnterior && x.Banco == cliente.BancoNombreAnterior).FirstOrDefault();
                        if (bancoCuentaConsulta != null)
                        {
                            //editar
                            bancoCuenta.BancoCuentaID = bancoCuentaConsulta.BancoCuentaID;
                            bancoCuenta.ClienteID = bancoCuentaConsulta.ClienteID;
                            respuestaDetalleBanco = bancocuentaBl.BancoCuentaEditarJson(bancoCuenta);
                        }
                        else
                        {
                            //agregar
                            bancoCuenta.ClienteID = IdCliente;
                            respuestaDetalleBanco = bancocuentaBl.BancoCuentaInsertarJson(bancoCuenta);
                        }
                    }
                    else
                    {
                        respuestaDetalleBanco = true;
                    }
                }
                if (respuestaCliente && respuestaDetalleBanco)
                {
                    respuesta = true;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return Json(respuesta);
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult SincronizarSolicitudTransferenciaCliente(SolicitudTransferencia solicitud){
            string mensaje = "";
            bool respuesta = false;
            bool respuestaCliente = false;
            bool respuestaSolicitud = false;
            bool respuestaTickets = false;
            bool respuestaDetalleBanco = false;

            int IdCliente = 0;
            int idSolicitud = 0;
            try
            {
                //Seccion Cliente
                respuesta = true;
                List<Cliente> listaClientes = new List<Cliente>();
                Cliente cliente = new Cliente();
                Cliente clienteEditar_Insertar = new Cliente();

                clienteEditar_Insertar.ClienteNombre = solicitud.ClienteNombre;
                clienteEditar_Insertar.ClienteApelPat = solicitud.ClienteApelPat;
                clienteEditar_Insertar.ClienteApelMat = solicitud.ClienteApelMat;
                clienteEditar_Insertar.ClienteTipoDoc = solicitud.TipoDocNombre;
                clienteEditar_Insertar.ClienteNroDoc = solicitud.ClienteNroDoc;

                cliente = clienteBl.ClienteBuscarJson(solicitud.ClienteNroDoc).Where(x => x.ClienteTipoDoc.ToLower().Equals(solicitud.TipoDocNombre.ToLower())).FirstOrDefault();
                if (cliente != null)
                {
                    clienteEditar_Insertar.ClienteID = cliente.ClienteID;
                    clienteEditar_Insertar.FechaReg = cliente.FechaReg;
                    respuestaCliente = clienteBl.ClienteEditarJson(clienteEditar_Insertar);
                    IdCliente = cliente.ClienteID;
                }
                else
                {
                    Cliente clienteConsultaID = new Cliente();
                    clienteEditar_Insertar.FechaReg = DateTime.Now;
                    respuestaCliente = clienteBl.clienteInsertarJson(clienteEditar_Insertar);
                    clienteConsultaID = clienteBl.ClienteBuscarJson(clienteEditar_Insertar.ClienteNroDoc).Where(x => x.ClienteTipoDoc.ToLower() == clienteEditar_Insertar.ClienteTipoDoc.ToLower()).FirstOrDefault();
                    if (clienteConsultaID != null)
                    {
                        IdCliente = clienteConsultaID.ClienteID;
                        respuestaCliente = true;
                    }
                    else
                    {
                        respuestaCliente = false;
                    }
                }
                if (respuestaCliente)
                {
                    //seccion Detalle Banco
                    BancoCuentaEntidad bancoCuenta = new BancoCuentaEntidad();
                    BancoCuentaEntidad bancoCuentaConsulta = new BancoCuentaEntidad();
                    bancoCuenta.Banco = solicitud.BancoNombre;
                    bancoCuenta.NroCuenta = solicitud.NroCuenta;
                    if (bancoCuenta.Banco != "" && bancoCuenta.Banco != null && bancoCuenta.NroCuenta != null && bancoCuenta.NroCuenta != "")
                    {
                        bancoCuentaConsulta = bancocuentaBl.BancoCuentaclienteidListadoJson(IdCliente).Where(x => x.NroCuenta == solicitud.NroCuenta && x.Banco == solicitud.BancoNombre).FirstOrDefault();
                        if (bancoCuentaConsulta != null)
                        {
                            //editar
                            bancoCuenta.BancoCuentaID = bancoCuentaConsulta.BancoCuentaID;
                            bancoCuenta.ClienteID = bancoCuentaConsulta.ClienteID;
                            respuestaDetalleBanco = bancocuentaBl.BancoCuentaEditarJson(bancoCuenta);
                        }
                        else
                        {
                            //agregar
                            bancoCuenta.ClienteID = IdCliente;
                            respuestaDetalleBanco = bancocuentaBl.BancoCuentaInsertarJson(bancoCuenta);
                        }
                    }
                    else
                    {
                        respuestaDetalleBanco = true;
                    }
                    if (solicitud.SolicitudID != 0)
                    {
                        //Seccion depositos
                        SolicitudTransferencia solicitudEditar_Insertar = new SolicitudTransferencia();
                        SolicitudTransferencia solicitudConsulta = new SolicitudTransferencia();

                        solicitudEditar_Insertar.Codsala = solicitud.Codsala;
                        solicitudEditar_Insertar.ClienteNombre = solicitud.ClienteNombre;
                        solicitudEditar_Insertar.ClienteApelPat = solicitud.ClienteApelPat;
                        solicitudEditar_Insertar.ClienteApelMat = solicitud.ClienteApelMat;
                        solicitudEditar_Insertar.ClienteNroDoc = solicitud.ClienteNroDoc;
                        solicitudEditar_Insertar.TipoDocNombre = solicitud.TipoDocNombre;
                        solicitudEditar_Insertar.Monto = solicitud.Monto;
                        solicitudEditar_Insertar.NroTickets = solicitud.NroTickets;
                        solicitudEditar_Insertar.Estado = solicitud.Estado;
                        solicitudEditar_Insertar.FechaReg = DateTime.Now;
                        solicitudEditar_Insertar.SolicitudSala = solicitud.SolicitudID;
                        solicitudEditar_Insertar.BancoNombre = solicitud.BancoNombre==null?"":solicitud.BancoNombre;
                        solicitudEditar_Insertar.NroCuenta = solicitud.NroCuenta==null?"":solicitud.NroCuenta;
                        solicitudEditar_Insertar.UsuarioNombreReg = solicitud.UsuarioNombreReg;
                        solicitudConsulta = solicitudtransferenciabl.SolicitudTransferenciaListarSolicitudSalaJson(solicitud.SolicitudID).Where(x => x.Codsala== solicitud.Codsala).FirstOrDefault();

                        if (solicitudConsulta != null)
                        {
                            solicitudEditar_Insertar.SolicitudID = solicitudConsulta.SolicitudID;
                            respuestaSolicitud = solicitudtransferenciabl.SolicitudTransferenciaEditarJson(solicitudEditar_Insertar);
                            idSolicitud = solicitudConsulta.SolicitudID;
                        }
                        else
                        {
                            idSolicitud = solicitudtransferenciabl.SolicitudTransferenciaInsertar(solicitudEditar_Insertar);
                        }
                        if (idSolicitud != 0)
                        {
                            respuestaSolicitud = true;
                        }
                        else
                        {
                            respuestaSolicitud = false;
                        }
                        //Seccion Tickets validar por cantidad de tickets
                        if (respuestaSolicitud)
                        {
                            if (solicitud.Tickets != null)
                            {
                                int totalTicketsRecibidos = solicitud.Tickets.Count;
                                if (totalTicketsRecibidos != 0)
                                {
                                    int contadorTickets = 0;
                                    List<SolicitudTicket> ticketConsultaLista = new List<SolicitudTicket>();
                                    ticketConsultaLista = solicitudticketbl.SolicitudTicketSolicitudIDListadoJson(idSolicitud);
                                    foreach (var item in solicitud.Tickets)
                                    {
                                        SolicitudTicket ticketConsulta = new SolicitudTicket();
                                        SolicitudTicket ticketEditar_Insertar = new SolicitudTicket();
                                        ticketEditar_Insertar.SolicitudID = idSolicitud;
                                        ticketEditar_Insertar.FechaReg = item.FechaReg;
                                        ticketEditar_Insertar.Monto = item.Monto;
                                        ticketEditar_Insertar.NroTicketTito = item.NroTicketTito;
                                        ticketConsulta = ticketConsultaLista.Where(x => x.NroTicketTito.Trim().Equals(item.NroTicketTito)).FirstOrDefault();
                                        if (ticketConsulta != null)
                                        {
                                            //Editar
                                            ticketEditar_Insertar.SolicitudTicketID = ticketConsulta.SolicitudTicketID;
                                            respuestaTickets = solicitudticketbl.SolicitudTicketEditarJson(ticketEditar_Insertar);
                                        }
                                        else
                                        {
                                            //Insertar
                                            respuestaTickets = solicitudticketbl.SolicitudTicketInsertarJson(ticketEditar_Insertar);
                                        }
                                        if (respuestaTickets)
                                        {
                                            contadorTickets++;
                                        }
                                    }
                                    if (contadorTickets == totalTicketsRecibidos)
                                    {
                                        respuestaTickets = true;
                                    }
                                    else
                                    {
                                        respuestaTickets = false;
                                    }
                                }
                                else
                                {
                                    respuestaTickets = true;
                                }
                            }
                            else
                            {
                                respuestaTickets = true;
                            }

                        }
                    }
                    else
                    {
                        respuestaSolicitud = true;
                        respuestaTickets = true;
                    }
                }
                if (respuestaCliente && respuestaSolicitud && respuestaDetalleBanco && respuestaTickets)
                {
                    respuesta = true;
                }
                else
                {
                    respuesta = false;
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                respuesta = false;
            }
            return Json(respuesta);
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult AnularSolicitudTransferenciaCliente(SolicitudTransferencia solicitud)
        {
            bool respuesta = false;
            string mensaje = "";
            SolicitudTransferencia solicitudConsulta = new SolicitudTransferencia();
            try
            {
                solicitudConsulta = solicitudtransferenciabl.SolicitudTransferenciaListarSolicitudSalaJson(solicitud.SolicitudID).Where(x => x.Codsala == solicitud.SolicitudSala).FirstOrDefault();
                if (solicitudConsulta != null)
                {
                    solicitud.SolicitudID = solicitudConsulta.SolicitudID;
                    respuesta = solicitudtransferenciabl.SolicitudTransferenciaAnularJson(solicitud);
                }
                else
                {
                    //si no esta registrado, no aparecera en el listado,por ende el online se puede anular
                    respuesta = true;
                }
            }
            catch(Exception ex)
            {
                mensaje = ex.Message;
                respuesta = false;
            }
            return Json(respuesta);
        }
        [seguridad(false)]
        [HttpPost]
        public JsonResult SincronizarTransferenciasSalaJson(int[] codsala, DateTime fechaini, DateTime fechafin)
        {
            var errormensaje = "";
            var lista = new List<Transferencia>();
            int cantElementos = (codsala == null) ? 0 : codsala.Length;
            var strElementos = String.Empty;
            try
            {
                if (cantElementos > 0)
                {
                    strElementos = " tr.[Codsala] in(" + "'" + String.Join("','", codsala) + "'" + ") and ";
                }
                lista = transferenciaBl.TransferenciaSalasListarJson(fechaini, fechafin, strElementos);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(),mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult SincronizarVoucherTransaccionJson(int TransferenciaID)
        {
            string mensaje = "";
            Transferencia transferencia = new Transferencia();
            string pathArchivos = ConfigurationManager.AppSettings["IASPathImagenVoucher"].ToString();
            try
            {
                transferencia = transferenciaBl.TransferenciaIDJson(TransferenciaID);
                var nombre = string.Empty;
                nombre = transferencia.ImagenVoucher;
                var imagePath = Path.Combine(pathArchivos, nombre);

                var direccionFoto = imagePath;
                if (System.IO.File.Exists(direccionFoto))
                {
                    byte[] bmp = ImageToBinary(direccionFoto);
                    transferencia.imagenbase64voucher = Convert.ToBase64String(bmp);
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            var jsonResult = Json(new { data = transferencia }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

    }
}
