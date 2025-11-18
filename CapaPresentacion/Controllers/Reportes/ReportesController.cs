using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CapaEntidad.TITO;
using CapaNegocio;
using CapaEntidad;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Drawing;
using System.Configuration;
using CapaPresentacion.Models;
using System.Web.Script.Serialization;
using CapaPresentacion.Utilitarios;
using CapaNegocio.AsistenciaCliente;
using CapaEntidad.AsistenciaCliente;
using CapaEntidad.Response;
using System.Threading.Tasks;
using CapaEntidad.Reportes.AperturaCaja;
using CapaPresentacion.Reports;

namespace CapaPresentacion.Controllers.Reportes
{
    [seguridad]
    public class ReportesController : Controller
    {
        private SEG_UsuarioBL segUsuarioBl = new SEG_UsuarioBL();
        private SalaBL salaBl = new SalaBL();
        private AST_ClienteBL ast_ClienteBL = new AST_ClienteBL();
        private readonly ServiceHelper _serviceHelper = new ServiceHelper();

        public ActionResult TITOCajas()
        {
            return View("~/Views/Reportes/TITOCajas.cshtml");
        }

        public ActionResult TITOPromocionales()
        {
            return View("~/Views/Reportes/TITOPromocionales.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarCajas(string roomCode) {
            int status = 0;
            List<Caja> listBox = new List<Caja>();
            SalaEntidad sala = salaBl.SalaListaIdJson(Convert.ToInt32(roomCode));

            if(sala == null) {
                return Json(new {
                    status = status,
                    message = "No se encontro sala, por favor ingrese datos correctos"
                });
            }

            if(string.IsNullOrEmpty(sala.UrlProgresivo)) {
                return Json(new {
                    status,
                    message = $"La Sala {sala.Nombre} no tiene definido el UrlProgresivo"
                });
            }

            try {
                string response = string.Empty;
                string parameters = "roomCode=" + roomCode;
                string uri = $"{sala.UrlProgresivo}/servicio/ListarCajas?{parameters}";

                using(WebClient webClient = new WebClient()) {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    response = webClient.UploadString(uri, "POST");
                }

                var settings = new JsonSerializerSettings {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                listBox = JsonConvert.DeserializeObject<List<Caja>>(response, settings);

                status = 1;

            } catch(Exception ex) {
                return Json(new {
                    status = 2,
                    message = ex.Message.ToString(),
                    data = listBox.ToList()
                });
            }

            return Json(new {
                status,
                message = "Datos obtenidos correctamente",
                data = listBox.ToList()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ListarTITOCajas(ParametersTITOCaja args)
        {
            int status = 0;
            var listReport = new List<Reporte_Detalle_TITO_Caja>();
            DateTime currentDate = DateTime.Now;
            DateTime dateProcessStart = Convert.ToDateTime(args.OpeningDateStart);
            DateTime dateProcessEnd = Convert.ToDateTime(args.OpeningDateEnd);

            SEG_UsuarioEntidad usuario = segUsuarioBl.UsuarioEmpleadoIDObtenerJson(Convert.ToInt32(Session["UsuarioID"]));

            if (usuario == null)
            {
                return Json(new
                {
                    status,
                    message = "No se encontro un usuario, por favor inicie sesión"
                });
            }

            string personalName = usuario.UsuarioNombre;

            SalaEntidad sala = salaBl.ObtenerSalaEmpresa(Convert.ToInt32(args.RoomCode));

            if (sala == null)
            {
                return Json(new
                {
                    status,
                    message = "No se encontro sala, por favor ingrese datos correctos"
                });
            }

            if (string.IsNullOrEmpty(sala.UrlProgresivo))
            {
                return Json(new
                {
                    status,
                    message = $"La Sala {sala.Nombre} no tiene definido el UrlProgresivo"
                });
            }

            if (args.BoxCode.Length <= 0)
            {
                return Json(new
                {
                    status,
                    message = "Por favor, ingrese los códigos de caja a buscar"
                });
            }

            if (args.TurnCode.Length <= 0)
            {
                return Json(new
                {
                    status,
                    message = "Por favor, ingrese los códigos de turno a buscar"
                });
            }

            if (string.IsNullOrEmpty(args.OpeningDateStart))
            {
                return Json(new
                {
                    status,
                    message = "Por favor, ingrese una fecha inicio a buscar"
                });
            }

            if (string.IsNullOrEmpty(args.OpeningDateEnd))
            {
                return Json(new
                {
                    status,
                    message = "Por favor, ingrese una fecha fin a buscar"
                });
            }

            if ((dateProcessEnd - dateProcessStart).TotalDays > 5)
            {
                return Json(new
                {
                    status,
                    message = "Por favor, seleccione fechas de 5 días de diferencia"
                });
            }

            try
            {
                var response = string.Empty;
                string uri = $"{sala.UrlProgresivo}/servicio/ListarDetalleTITOCajas";

                dynamic data = new
                {
                    BoxCode = args.BoxCode,
                    TurnCode = args.TurnCode,
                    OpeningDateStart = args.OpeningDateStart,
                    OpeningDateEnd = args.OpeningDateEnd
                };

                string json = JsonConvert.SerializeObject(data);

                using (MyWebClientInfinite webClient = new MyWebClientInfinite())
                {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";
                    webClient.Headers[HttpRequestHeader.Accept] = "application/json";
                    response = webClient.UploadString(uri, "POST", json);
                }

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                listReport = JsonConvert.DeserializeObject<List<Reporte_Detalle_TITO_Caja>>(response, settings);

                // start change data customers
                List<string> numberDocuments = listReport.Where(item => !string.IsNullOrEmpty(item.ClienteDni.Trim())).Select(item => item.ClienteDni.Trim()).Distinct().ToList();
                List<AST_ClienteEntidad> customers = ast_ClienteBL.GetListaMasivoClientesxNroDoc(numberDocuments);

                listReport.Where(item => !string.IsNullOrEmpty(item.ClienteDni.Trim())).ToList().ForEach(item => {

                    AST_ClienteEntidad customer = customers.Where(customerItem => customerItem.NroDoc.Trim().Equals(item.ClienteDni.Trim())).FirstOrDefault();

                    if(customer != null)
                    {
                        item.ClienteTelefono = string.IsNullOrEmpty(item.ClienteTelefono.Trim()) ? customer.Celular1.Trim() : item.ClienteTelefono.Trim();
                        item.ClienteCorreo = string.IsNullOrEmpty(item.ClienteCorreo.Trim()) || item.ClienteCorreo.Trim().Equals("CORREO@PROVEEDOR.COM") ? customer.Mail.Trim() : item.ClienteCorreo.Trim();
                    }

                });
                // end change data customers

                status = 1;
            }
            catch (Exception ex)
            {
                return Json(new {
                    status = 2,
                    message = ex.Message.ToString(),
                    data = listReport.ToList()
                });
            }

            //keys tipo proceso
            int keyVentas = 1;
            int keyCompras = 2;
            int keyPagos = 5;
            int keyCortesias = 4;

            // totales de ventas = 1, compras = 2, pagos manuales = 5 y cortesía = 4
            int cantidadVentas = listReport.Where(item => item.idTipoProceso == keyVentas).Count();
            int cantidadCompras = listReport.Where(item => item.idTipoProceso == keyCompras).Count();
            int cantidadPagos = listReport.Where(item => item.idTipoProceso == keyPagos).Count();
            int cantidadCortesias = listReport.Where(item => item.idTipoProceso == keyCortesias).Count();
            int cantidadTickets = listReport.Count();

            decimal totalVentas = listReport.Where(item => item.idTipoProceso == keyVentas).Sum(item => item.Monto_Dinero);
            decimal totalCompras = listReport.Where(item => item.idTipoProceso == keyCompras).Sum(item => item.Monto_Dinero);
            decimal totalPagos = listReport.Where(item => item.idTipoProceso == keyPagos).Sum(item => item.Monto_Dinero);
            decimal totalCortesias = listReport.Where(item => item.idTipoProceso == keyCortesias).Sum(item => item.Monto_Dinero);
            decimal totalTickets = listReport.Sum(item => item.Monto_Dinero);

            // summary
            dynamic summary = new
            {
                company = sala.Empresa.RazonSocial,
                room = sala.Nombre,
                printDate = currentDate.ToString("dd/MM/yyyy"),
                printHour = currentDate.ToString("HH:mm:ss"),
                dateProcessStart = dateProcessStart.ToString("dd/MM/yyyy"),
                dateProcessEnd = dateProcessEnd.ToString("dd/MM/yyyy"),
                employee = personalName,
                cantidadVentas,
                cantidadCompras,
                cantidadPagos,
                cantidadCortesias,
                cantidadTickets,
                totalVentas = totalVentas.ToString("#,##0.00"),
                totalCompras = totalCompras.ToString("#,##0.00"),
                totalPagos = totalPagos.ToString("#,##0.00"),
                totalCortesias = totalCortesias.ToString("#,##0.00"),
                totalTickets = totalTickets.ToString("#,##0.00")
            };

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new
            {
                status,
                message = "Datos obtenidos correctamente",
                data = listReport.ToList(),
                summary
            };

            var result = new ContentResult
            {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };

            return result;
            
            /*return Json(new {
                status,
                message = "Datos obtenidos correctamente",
                data = listReport.ToList(),
                summary
            }, JsonRequestBehavior.AllowGet);*/
        }

        [HttpPost]
        public ActionResult ExcelTITOCajas(ParametersTITOCaja args)
        {
            int status = 0;
            DateTime currentDate = DateTime.Now;
            DateTime dateProcessStart = Convert.ToDateTime(args.OpeningDateStart);
            DateTime dateProcessEnd = Convert.ToDateTime(args.OpeningDateEnd);
            
            string message = string.Empty;
            string fileName = $"Reporte_Detallado_de_Tickets_en_Caja_{args.RoomCode}_{dateProcessStart.ToString("dd_MM_yyyy")}-{dateProcessEnd.ToString("dd_MM_yyyy")}_{currentDate.ToString("HHmmss")}.xlsx";
            string base64String = string.Empty;

            SEG_UsuarioEntidad usuario = segUsuarioBl.UsuarioEmpleadoIDObtenerJson(Convert.ToInt32(Session["UsuarioID"]));
            
            if(usuario == null)
            {
                return Json(new
                {
                    status,
                    message = "No se encontro un usuario, por favor inicie sesión"
                });
            }
            
            string personalName = usuario.UsuarioNombre;

            SalaEntidad sala = salaBl.ObtenerSalaEmpresa(Convert.ToInt32(args.RoomCode));

            if (sala == null)
            {
                return Json(new
                {
                    status,
                    message = "No se encontro sala, por favor ingrese datos correctos"
                });
            }

            if (string.IsNullOrEmpty(sala.UrlProgresivo))
            {
                return Json(new
                {
                    status,
                    message = $"La Sala {sala.Nombre} no tiene definido el UrlProgresivo"
                });
            }

            if (args.BoxCode.Length <= 0)
            {
                return Json(new
                {
                    status,
                    message = "Por favor, ingrese los códigos de caja a buscar"
                });
            }

            if (args.TurnCode.Length <= 0)
            {
                return Json(new
                {
                    status,
                    message = "Por favor, ingrese los códigos de turno a buscar"
                });
            }

            if (string.IsNullOrEmpty(args.OpeningDateStart))
            {
                return Json(new
                {
                    status,
                    message = "Por favor, ingrese una fecha inicio a buscar"
                });
            }

            if (string.IsNullOrEmpty(args.OpeningDateEnd))
            {
                return Json(new
                {
                    status,
                    message = "Por favor, ingrese una fecha fin a buscar"
                });
            }

            if ((dateProcessEnd - dateProcessStart).TotalDays > 5)
            {
                return Json(new
                {
                    status,
                    message = "Por favor, seleccione fechas de 5 días de diferencia"
                });
            }

            Dictionary<int, string> statusTiket = new Dictionary<int, string>
            {
                { 0, "ANULADO" },
                { 1, "PENDIENTE" },
                { 2, "PAGADO" },
                { 3, "VENCIDOS" }
            };

            Dictionary<int, string> processTypeCodes = new Dictionary<int, string>
            {
                { 1, "Ventas" },
                { 2, "Compras" },
                { 5, "Pagos Manuales" },
                { 4, "Cortesía" }
            };

            Dictionary<string, string> gendersCodes = new Dictionary<string, string>
            {
                { "F", "Femenino" },
                { "M", "Masculino" }
            };

            try
            {
                var response = string.Empty;
                string uri = $"{sala.UrlProgresivo}/servicio/ListarDetalleTITOCajas";

                dynamic data = new
                {
                    BoxCode = args.BoxCode,
                    TurnCode = args.TurnCode,
                    OpeningDateStart = args.OpeningDateStart,
                    OpeningDateEnd = args.OpeningDateEnd
                };

                string json = JsonConvert.SerializeObject(data);

                using (MyWebClientInfinite webClient = new MyWebClientInfinite())
                {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";
                    webClient.Headers[HttpRequestHeader.Accept] = "application/json";
                    response = webClient.UploadString(uri, "POST", json);
                }

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                List<Reporte_Detalle_TITO_Caja> listReport = JsonConvert.DeserializeObject<List<Reporte_Detalle_TITO_Caja>>(response, settings);

                // start change data customers
                List<string> numberDocuments = listReport.Where(item => !string.IsNullOrEmpty(item.ClienteDni.Trim())).Select(item => item.ClienteDni.Trim()).Distinct().ToList();
                List<AST_ClienteEntidad> customers = ast_ClienteBL.GetListaMasivoClientesxNroDoc(numberDocuments);

                listReport.Where(item => !string.IsNullOrEmpty(item.ClienteDni.Trim())).ToList().ForEach(item => {

                    AST_ClienteEntidad customer = customers.Where(customerItem => customerItem.NroDoc.Trim().Equals(item.ClienteDni.Trim())).FirstOrDefault();

                    if(customer != null)
                    {
                        item.ClienteTelefono = string.IsNullOrEmpty(item.ClienteTelefono.Trim()) ? customer.Celular1.Trim() : item.ClienteTelefono.Trim();
                        item.ClienteCorreo = string.IsNullOrEmpty(item.ClienteCorreo.Trim()) || item.ClienteCorreo.Trim().Equals("CORREO@PROVEEDOR.COM") ? customer.Mail.Trim() : item.ClienteCorreo.Trim();
                    }

                });
                // end change data customers

                // Generated Excel
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excel = new ExcelPackage();

                var workSheet = excel.Workbook.Worksheets.Add("Reporte Detallado");
                workSheet.TabColor = Color.Black;
                workSheet.DefaultRowHeight = 12;

                //Body of table
                int initCol = 2;
                int initRow = 12;
                int recordIndex = initRow + 1;
                int total = listReport.Count;
                int totalRows = initRow + total;
                int footerRow = totalRows + 1;

                //keys tipo proceso
                int keyVentas = 1;
                int keyCompras = 2;
                int keyPagos = 5;
                int keyCortesias = 4;

                // totales de ventas = 1, compras = 2, pagos manuales = 5 y cortesía = 4
                int cantidadVentas = listReport.Where(item => item.idTipoProceso == keyVentas).Count();
                decimal totalVentas = listReport.Where(item => item.idTipoProceso == keyVentas).Sum(item => item.Monto_Dinero);
                int cantidadCompras = listReport.Where(item => item.idTipoProceso == keyCompras).Count();
                decimal totalCompras = listReport.Where(item => item.idTipoProceso == keyCompras).Sum(item => item.Monto_Dinero);
                int cantidadPagos = listReport.Where(item => item.idTipoProceso == keyPagos).Count();
                decimal totalPagos = listReport.Where(item => item.idTipoProceso == keyPagos).Sum(item => item.Monto_Dinero);
                int cantidadCortesias = listReport.Where(item => item.idTipoProceso == keyCortesias).Count();
                decimal totalCortesias = listReport.Where(item => item.idTipoProceso == keyCortesias).Sum(item => item.Monto_Dinero);
                decimal totalTickets = listReport.Sum(item => item.Monto_Dinero);

                // Title of table
                workSheet.Cells["D4:N4"].Merge = true;
                workSheet.Cells[4, 4].Style.Font.Bold = true;
                workSheet.Cells[4, 4].Style.Font.Size = 16;
                workSheet.Cells[4, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[4, 4].Value = "REPORTE DETALLADO DE TICKETS EN CAJA";

                workSheet.Cells["E2:G2"].Merge = true;
                workSheet.Cells["E3:G3"].Merge = true;
                workSheet.Cells["D2:D3"].Style.Font.Bold = true;
                workSheet.Cells[2, 4].Value = "Empresa";
                workSheet.Cells[3, 4].Value = "Sala";
                workSheet.Cells[2, 5].Value = sala.Empresa.RazonSocial; // set Empresa
                workSheet.Cells[3, 5].Value = sala.Nombre; // set Sala

                workSheet.Cells["M2:M3"].Style.Font.Bold = true;
                workSheet.Cells[2, 13].Value = "Fecha Imp.";
                workSheet.Cells[3, 13].Value = "Hora Imp.";
                workSheet.Cells[2, 14].Value = currentDate.ToString("dd/MM/yyyy"); // set Fecha Imp.
                workSheet.Cells[3, 14].Value = currentDate.ToString("HH:mm:ss"); // set Hora Imp.

                workSheet.Cells["E5:G5"].Merge = true;
                workSheet.Cells["E6:G6"].Merge = true;
                workSheet.Cells["D5:D6"].Style.Font.Bold = true;
                workSheet.Cells[5, 4].Value = "Fecha";
                workSheet.Cells[6, 4].Value = "Caja";
                workSheet.Cells[5, 5].Value = $"{dateProcessStart.ToString("dd/MM/yyyy")} - {dateProcessEnd.ToString("dd/MM/yyyy")}"; // set Fecha Proceso
                workSheet.Cells[6, 5].Value = string.Join(", ", args.BoxName); // set Caja

                workSheet.Cells["N5:O5"].Merge = true;
                workSheet.Cells["N6:O6"].Merge = true;
                workSheet.Cells["M5:M6"].Style.Font.Bold = true;
                workSheet.Cells[5, 13].Value = "Turno";
                workSheet.Cells[6, 13].Value = "Personal";
                workSheet.Cells[5, 14].Value = string.Join(", ", args.TurnCode); // set Turno
                workSheet.Cells[6, 14].Value = personalName; // set Personal

                workSheet.Cells["D8:E8"].Merge = true;
                workSheet.Cells["D9:D10"].Style.Font.Bold = true;
                workSheet.Cells[8, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[8, 4].Style.Font.Bold = true;
                workSheet.Cells[8, 4].Value = "Ventas";
                workSheet.Cells[9, 4].Value = "Cantidad";
                workSheet.Cells[10, 4].Value = "Total";
                workSheet.Cells[9, 5].Value = cantidadVentas.ToString(); // set Cantidad Ventas
                workSheet.Cells[10, 5].Value = totalVentas.ToString("#,##0.00"); // set Total Ventas

                workSheet.Cells["G8:H8"].Merge = true;
                workSheet.Cells["G9:G10"].Style.Font.Bold = true;
                workSheet.Cells[8, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[8, 7].Style.Font.Bold = true;
                workSheet.Cells[8, 7].Value = "Compras";
                workSheet.Cells[9, 7].Value = "Cantidad";
                workSheet.Cells[10, 7].Value = "Total";
                workSheet.Cells[9, 8].Value = cantidadCompras.ToString(); // set Cantidad Compras
                workSheet.Cells[10, 8].Value = totalCompras.ToString("#,##0.00"); // set Total Compras

                workSheet.Cells["J8:K8"].Merge = true;
                workSheet.Cells["J9:J10"].Style.Font.Bold = true;
                workSheet.Cells[8, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[8, 10].Style.Font.Bold = true;
                workSheet.Cells[8, 10].Value = "Pagos Manuales";
                workSheet.Cells[9, 10].Value = "Cantidad";
                workSheet.Cells[10, 10].Value = "Total";
                workSheet.Cells[9, 11].Value = cantidadPagos.ToString(); // set Cantidad Pagos Manuales
                workSheet.Cells[10, 11].Value = totalPagos.ToString("#,##0.00"); // set Total Cantidad Pagos Manuales

                workSheet.Cells["M8:N8"].Merge = true;
                workSheet.Cells["M9:M10"].Style.Font.Bold = true;
                workSheet.Cells[8, 13].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[8, 13].Style.Font.Bold = true;
                workSheet.Cells[8, 13].Value = "Cortesía";
                workSheet.Cells[9, 13].Value = "Cantidad";
                workSheet.Cells[10, 13].Value = "Total";
                workSheet.Cells[9, 14].Value = cantidadCortesias.ToString(); // set Cantidad Cortesía
                workSheet.Cells[10, 14].Value = totalCortesias.ToString("#,##0.00"); // set Total Cortesía

                //Header of table  
                workSheet.Row(initRow).Height = 20;
                workSheet.Row(initRow).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(initRow).Style.Font.Bold = true;

                workSheet.Cells[initRow, 2].Value = "DNI";
                workSheet.Cells[initRow, 3].Value = "Cliente";
                workSheet.Cells[initRow, 4].Value = "Género";
                workSheet.Cells[initRow, 5].Value = "F. Nacimiento";
                workSheet.Cells[initRow, 6].Value = "Correo";
                workSheet.Cells[initRow, 7].Value = "Teléfono";
                workSheet.Cells[initRow, 8].Value = "Tipo de Proceso";
                workSheet.Cells[initRow, 9].Value = "Fecha y Hora Ticket";
                workSheet.Cells[initRow, 10].Value = "Tipo Pago";
                workSheet.Cells[initRow, 11].Value = "Tipo Origen";
                workSheet.Cells[initRow, 12].Value = "Lugar Origen";
                workSheet.Cells[initRow, 13].Value = "Código Extra";
                workSheet.Cells[initRow, 14].Value = "Nro. Ticket";
                workSheet.Cells[initRow, 15].Value = "Personal";
                workSheet.Cells[initRow, 16].Value = "Estado";
                workSheet.Cells[initRow, 17].Value = "Promoción";
                workSheet.Cells[initRow, 18].Value = "Monto Din.";

                foreach (Reporte_Detalle_TITO_Caja report in listReport)
                {
                    string dateBirth = string.IsNullOrEmpty(report.ClienteFechaNacimiento) ? "" : Convert.ToDateTime(report.ClienteFechaNacimiento).ToString("dd/MM/yyyy");
                    string customerGender = string.IsNullOrEmpty(report.ClienteGenero) ? "NA" : report.ClienteGenero;

                    workSheet.Cells[recordIndex, 2].Value = report.ClienteDni;
                    workSheet.Cells[recordIndex, 3].Value = report.Cliente;
                    workSheet.Cells[recordIndex, 4].Value = gendersCodes.ContainsKey(customerGender) ? gendersCodes[customerGender] : "";
                    workSheet.Cells[recordIndex, 5].Value = dateBirth.Equals("01/01/1753") ? "" : dateBirth;
                    workSheet.Cells[recordIndex, 6].Value = report.ClienteCorreo;
                    workSheet.Cells[recordIndex, 7].Value = report.ClienteTelefono;
                    workSheet.Cells[recordIndex, 8].Value = processTypeCodes.ContainsKey(report.idTipoProceso) ? processTypeCodes[report.idTipoProceso] : "";
                    workSheet.Cells[recordIndex, 9].Value = report.Fecha_Proceso.ToString("dd/MM/yyyy HH:mm");
                    workSheet.Cells[recordIndex, 10].Value = report.TipoPago_desc;
                    workSheet.Cells[recordIndex, 11].Value = report.TipoOrigen;
                    workSheet.Cells[recordIndex, 12].Value = report.LugarOrigen;
                    workSheet.Cells[recordIndex, 13].Value = report.CodigoExtra;
                    workSheet.Cells[recordIndex, 14].Value = report.Ticket;
                    workSheet.Cells[recordIndex, 15].Value = report.Personal;
                    workSheet.Cells[recordIndex, 16].Value = statusTiket.ContainsKey(report.EstadoTiket) ? statusTiket[report.EstadoTiket] : "";
                    workSheet.Cells[recordIndex, 17].Value = report.Motivo;
                    workSheet.Cells[recordIndex, 18].Value = report.Monto_Dinero.ToString("#,##0.00");

                    recordIndex++;
                }

                Color colbackground = ColorTranslator.FromHtml("#003268");
                Color colborder = ColorTranslator.FromHtml("#074B88");

                workSheet.Cells["B12:R12"].Style.Font.Bold = true;
                workSheet.Cells["B12:R12"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["B12:R12"].Style.Fill.BackgroundColor.SetColor(colbackground);
                workSheet.Cells["B12:R12"].Style.Font.Color.SetColor(Color.White);
                workSheet.Cells["B12:R12"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet.Cells["B12:R12"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                workSheet.Cells["B12:R12"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet.Cells["B12:R12"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                workSheet.Cells["B12:R12"].Style.Border.Top.Color.SetColor(colborder);
                workSheet.Cells["B12:R12"].Style.Border.Left.Color.SetColor(colborder);
                workSheet.Cells["B12:R12"].Style.Border.Right.Color.SetColor(colborder);
                workSheet.Cells["B12:R12"].Style.Border.Bottom.Color.SetColor(colborder);

                workSheet.Cells["B13:B" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["D13:D" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["E13:E" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["F13:F" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["G13:G" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["H13:H" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["I13:I" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["J13:J" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["K13:K" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["L13:L" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["M13:M" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["N13:N" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["O13:O" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["P13:P" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["Q13:Q" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["R13:R" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                workSheet.Cells["B" + footerRow + ":R" + footerRow].Merge = true;
                workSheet.Cells["B" + footerRow + ":R" + footerRow].Style.Font.Bold = true;
                workSheet.Cells["B" + footerRow + ":R" + footerRow].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["B" + footerRow + ":R" + footerRow].Style.Fill.BackgroundColor.SetColor(colbackground);
                workSheet.Cells["B" + footerRow + ":R" + footerRow].Style.Font.Color.SetColor(Color.White);
                workSheet.Cells["B" + footerRow + ":R" + footerRow].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[footerRow, initCol].Value = "Total : " + total + " Registros";
                workSheet.Cells[initRow, initCol, totalRows, 18].AutoFilter = true;

                workSheet.Cells[totalRows + 3, 18].Style.Font.Bold = true;
                workSheet.Cells[totalRows + 3, 18].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                workSheet.Cells[totalRows + 3, 18].Value = totalTickets.ToString("#,##0.00");

                workSheet.Column(1).AutoFit(1);
                workSheet.Column(2).AutoFit();
                workSheet.Column(3).AutoFit();
                workSheet.Column(4).AutoFit();
                workSheet.Column(5).AutoFit();
                workSheet.Column(6).AutoFit();
                workSheet.Column(7).AutoFit();
                workSheet.Column(8).AutoFit();
                workSheet.Column(9).AutoFit();
                workSheet.Column(10).AutoFit();
                workSheet.Column(11).AutoFit();
                workSheet.Column(12).AutoFit();
                workSheet.Column(13).AutoFit();
                workSheet.Column(14).AutoFit();
                workSheet.Column(15).AutoFit();
                workSheet.Column(16).AutoFit();
                workSheet.Column(17).AutoFit();
                workSheet.Column(18).AutoFit();

                var memoryStream = new MemoryStream();
                excel.SaveAs(memoryStream);

                base64String = Convert.ToBase64String(memoryStream.ToArray());

                status = 1;
                message = "Se genero un Archivo Excel";
                // Generated Excel
            }
            catch (Exception ex)
            {
                return Json(new {
                    status = 2,
                    message = message + " " + ex.Message.ToString(),
                    data = base64String,
                    filename = fileName
                });
            }

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new
            {
                status,
                message,
                data = base64String,
                filename = fileName
            };

            var result = new ContentResult
            {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };

            return result;

            /*return Json(new { 
                status,
                message,
                data = base64String,
                filename = fileName
            });*/
        }

        // TITO PROMOCIONALES
        [HttpPost]
        public ActionResult ListarTITOPromocionales(string roomCode, string startDate, string endDate)
        {
            int status = 0;
            var listReport = new List<DetalleMovAuxTitoEntidad>();

            if (string.IsNullOrEmpty(roomCode))
            {
                return Json(new
                {
                    status,
                    message = "Por favor, seleccione una sala"
                });
            }

            if (string.IsNullOrEmpty(startDate))
            {
                return Json(new
                {
                    status,
                    message = "Por favor, ingrese una fecha inicio"
                });
            }

            if (string.IsNullOrEmpty(endDate))
            {
                return Json(new
                {
                    status,
                    message = "Por favor, ingrese una fecha final"
                });
            }

            try
            {
                var response = string.Empty;
                string uriAdministrativo = ConfigurationManager.AppSettings["UriSistemaReclutamiento"].ToString();
                string uri = $"{uriAdministrativo}Administrativo/ListarDetalleMovAuxTitoAdministrativo";

                dynamic data = new
                {
                    CodSala = roomCode,
                    FechaIni = startDate,
                    FechaFin = endDate
                };

                string json = JsonConvert.SerializeObject(data);

                using (WebClient webClient = new WebClient())
                {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";
                    webClient.Headers[HttpRequestHeader.Accept] = "application/json";
                    response = webClient.UploadString(uri, "POST", json);
                }

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                DetalleMovAuxTitoResponse jsonResponse = JsonConvert.DeserializeObject<DetalleMovAuxTitoResponse>(response, settings);

                if (!jsonResponse.respuesta)
                {
                    return Json(new
                    {
                        status,
                        message = "No se encontro datos"
                    });
                }

                listReport = jsonResponse.data;

                status = 1;
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = 2,
                    message = ex.Message.ToString(),
                    data = listReport.ToList()
                });
            }

            var jsonResult = Json(new
            {
                status,
                message = "Datos obtenidos correctamente",
                data = listReport.ToList()
            }, JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        [HttpPost]
        public ActionResult ExcelTITOPromocionales(string roomCode, string startDate, string endDate)
        {
            int status = 0;
            string message = string.Empty;
            DateTime currentDate = DateTime.Now;
            DateTime startDateTime = Convert.ToDateTime(startDate);
            DateTime endDateTime = Convert.ToDateTime(endDate);
            string fileName = "Reporte_TITO_Promocionales_" + roomCode + "_" + startDateTime.ToString("dd_MM_yyyy") + "_" + endDateTime.ToString("dd_MM_yyyy") + "_" + currentDate.ToString("HHmmss") + ".xlsx";
            string base64String = string.Empty;

            if (string.IsNullOrEmpty(roomCode))
            {
                return Json(new
                {
                    status,
                    message = "Por favor, seleccione una sala"
                });
            }

            if (string.IsNullOrEmpty(startDate))
            {
                return Json(new
                {
                    status,
                    message = "Por favor, ingrese una fecha inicio"
                });
            }

            if (string.IsNullOrEmpty(endDate))
            {
                return Json(new
                {
                    status,
                    message = "Por favor, ingrese una fecha final"
                });
            }

            SalaEntidad sala = salaBl.ObtenerSalaEmpresa(Convert.ToInt32(roomCode));

            try
            {
                var response = string.Empty;
                string uriAdministrativo = ConfigurationManager.AppSettings["UriSistemaReclutamiento"].ToString();
                string uri = $"{uriAdministrativo}Administrativo/ListarDetalleMovAuxTitoAdministrativo";

                dynamic data = new
                {
                    CodSala = roomCode,
                    FechaIni = startDate,
                    FechaFin = endDate
                };

                string json = JsonConvert.SerializeObject(data);

                using (WebClient webClient = new WebClient())
                {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";
                    webClient.Headers[HttpRequestHeader.Accept] = "application/json";
                    response = webClient.UploadString(uri, "POST", json);
                }

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                DetalleMovAuxTitoResponse jsonResponse = JsonConvert.DeserializeObject<DetalleMovAuxTitoResponse>(response, settings);

                List<DetalleMovAuxTitoEntidad> listReport = jsonResponse.data;

                // Generated Excel
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excel = new ExcelPackage();

                var workSheet = excel.Workbook.Worksheets.Add("Reporte TITO Promocionales");
                workSheet.TabColor = Color.Black;
                workSheet.DefaultRowHeight = 12;

                //Body of table
                int initCol = 2;
                int initRow = 7;
                int recordIndex = initRow + 1;
                int recordRow = initRow + 1;
                int total = listReport.Count;
                int totalColumns = 7;
                int totalRows = initRow + total;
                int footerRow = totalRows + 1;

                // Title of table
                workSheet.Cells["B2:H2"].Merge = true;
                workSheet.Cells[2, 2].Style.Font.Bold = true;
                workSheet.Cells[2, 2].Style.Font.Size = 16;
                workSheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[2, 2].Value = "REPORTE DE TICKETS PROMOCIONALES";

                workSheet.Cells["B4:B5"].Style.Font.Bold = true;
                workSheet.Cells[4, 2].Value = "Empresa";
                workSheet.Cells[5, 2].Value = "Sala";
                workSheet.Cells[4, 3].Value = sala.Empresa.RazonSocial; // set Empresa
                workSheet.Cells[5, 3].Value = sala.Nombre; // set Sala

                workSheet.Cells["G4:G5"].Style.Font.Bold = true;
                workSheet.Cells[4, 7].Value = "Fecha Inicio";
                workSheet.Cells[5, 7].Value = "Fecha Final";
                workSheet.Cells[4, 8].Value = startDate; // set Fecha Inicio
                workSheet.Cells[5, 8].Value = endDate; // set Fecha Final

                //Header of table  
                workSheet.Row(initRow).Height = 20;
                workSheet.Row(initRow).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(initRow).Style.Font.Bold = true;

                workSheet.Cells[initRow, 2].Value = "Cortesía";
                workSheet.Cells[initRow, 3].Value = "Cortesía No Dest";
                workSheet.Cells[initRow, 4].Value = "Promoción";
                workSheet.Cells[initRow, 5].Value = "Promoción No Dest";
                workSheet.Cells[initRow, 6].Value = "Estado";
                workSheet.Cells[initRow, 7].Value = "Fecha Inicio";
                workSheet.Cells[initRow, 8].Value = "Fecha Final";

                foreach (DetalleMovAuxTitoEntidad report in listReport)
                {
                    workSheet.Cells[recordIndex, 2].Value = report.TitoCortesia.ToString("0.00");
                    workSheet.Cells[recordIndex, 3].Value = report.TitoCortesiaNoDest.ToString("0.00");
                    workSheet.Cells[recordIndex, 4].Value = report.TitoPromocion.ToString("0.00");
                    workSheet.Cells[recordIndex, 5].Value = report.TitoPromocionNoDest.ToString("0.00");
                    workSheet.Cells[recordIndex, 6].Value = report.Estado;
                    workSheet.Cells[recordIndex, 7].Value = report.FechaTicketIni.ToString("dd/MM/yyyy HH:mm:ss");
                    workSheet.Cells[recordIndex, 8].Value = report.FechaTicketFin.ToString("dd/MM/yyyy HH:mm:ss");

                    recordIndex++;
                }

                Color colbackground = ColorTranslator.FromHtml("#003268");
                Color colborder = ColorTranslator.FromHtml("#074B88");

                workSheet.Cells["B" + initRow + ":H" + initRow].Style.Font.Bold = true;
                workSheet.Cells["B" + initRow + ":H" + initRow].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["B" + initRow + ":H" + initRow].Style.Fill.BackgroundColor.SetColor(colbackground);
                workSheet.Cells["B" + initRow + ":H" + initRow].Style.Font.Color.SetColor(Color.White);
                workSheet.Cells["B" + initRow + ":H" + initRow].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet.Cells["B" + initRow + ":H" + initRow].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                workSheet.Cells["B" + initRow + ":H" + initRow].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet.Cells["B" + initRow + ":H" + initRow].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet.Cells["B" + initRow + ":H" + initRow].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                workSheet.Cells["B" + initRow + ":H" + initRow].Style.Border.Top.Color.SetColor(colborder);
                workSheet.Cells["B" + initRow + ":H" + initRow].Style.Border.Left.Color.SetColor(colborder);
                workSheet.Cells["B" + initRow + ":H" + initRow].Style.Border.Right.Color.SetColor(colborder);
                workSheet.Cells["B" + initRow + ":H" + initRow].Style.Border.Bottom.Color.SetColor(colborder);

                workSheet.Cells["B" + recordRow + ":B" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["C" + recordRow + ":C" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["D" + recordRow + ":D" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["E" + recordRow + ":E" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["F" + recordRow + ":F" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["G" + recordRow + ":G" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["H" + recordRow + ":H" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                workSheet.Cells["B" + footerRow + ":H" + footerRow].Merge = true;
                workSheet.Cells["B" + footerRow + ":H" + footerRow].Style.Font.Bold = true;
                workSheet.Cells["B" + footerRow + ":H" + footerRow].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["B" + footerRow + ":H" + footerRow].Style.Fill.BackgroundColor.SetColor(colbackground);
                workSheet.Cells["B" + footerRow + ":H" + footerRow].Style.Font.Color.SetColor(Color.White);
                workSheet.Cells["B" + footerRow + ":H" + footerRow].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[footerRow, initCol].Value = "Total : " + total + " Registros";
                workSheet.Cells[initRow, initCol, totalRows, totalColumns + (initCol - 1)].AutoFilter = true;

                workSheet.Column(2).AutoFit();
                workSheet.Column(3).AutoFit();
                workSheet.Column(4).AutoFit();
                workSheet.Column(5).AutoFit();
                workSheet.Column(6).AutoFit();
                workSheet.Column(7).AutoFit();
                workSheet.Column(8).AutoFit();

                var memoryStream = new MemoryStream();
                excel.SaveAs(memoryStream);

                base64String = Convert.ToBase64String(memoryStream.ToArray());

                status = 1;
                message = "Se genero un Archivo Excel";
                // Generated Excel
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = 2,
                    message = message + " " + ex.Message.ToString(),
                    data = base64String,
                    filename = fileName
                });
            }

            return Json(new
            {
                status,
                message,
                data = base64String,
                filename = fileName
            });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarCajasVpn(string urlPrivada, string urlPublica) {
            int status = 0;
            List<Caja> listBox = new List<Caja>();
            var client = new System.Net.WebClient();
            var response = "";
            object oEnvio = new object();
            try {
                oEnvio = new {
                    ipPrivada = urlPrivada
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(urlPublica, "POST", inputJson);
                listBox = JsonConvert.DeserializeObject<List<Caja>>(response);

                status = 1;
            } catch(Exception ex) {
                return Json(new {
                    status = 2,
                    message = ex.Message.ToString(),
                    data = listBox.ToList()
                });
            }
            return Json(new {
                status,
                message = "Datos obtenidos correctamente",
                data = listBox.ToList()
            }, JsonRequestBehavior.AllowGet);
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarTITOCajasVpn(ParametersTITOCaja args,string urlPublica, string urlPrivada) {
            int status = 0;
            var listReport = new List<Reporte_Detalle_TITO_Caja>();
            DateTime currentDate = DateTime.Now;
            DateTime dateProcessStart = Convert.ToDateTime(args.OpeningDateStart);
            DateTime dateProcessEnd = Convert.ToDateTime(args.OpeningDateEnd);

            SEG_UsuarioEntidad usuario = segUsuarioBl.UsuarioEmpleadoIDObtenerJson(Convert.ToInt32(Session["UsuarioID"]));

            if(usuario == null) {
                return Json(new {
                    status,
                    message = "No se encontro un usuario, por favor inicie sesión"
                });
            }

            string personalName = usuario.UsuarioNombre;
            SalaEntidad sala = salaBl.ObtenerSalaEmpresa(Convert.ToInt32(args.RoomCode));

            if(sala == null) {
                return Json(new {
                    status,
                    message = "No se encontro sala, por favor ingrese datos correctos"
                });
            }

            if(args.BoxCode.Length <= 0) {
                return Json(new {
                    status,
                    message = "Por favor, ingrese los códigos de caja a buscar"
                });
            }

            if(args.TurnCode.Length <= 0) {
                return Json(new {
                    status,
                    message = "Por favor, ingrese los códigos de turno a buscar"
                });
            }

            if(string.IsNullOrEmpty(args.OpeningDateStart)) {
                return Json(new {
                    status,
                    message = "Por favor, ingrese una fecha inicio a buscar"
                });
            }

            if(string.IsNullOrEmpty(args.OpeningDateEnd)) {
                return Json(new {
                    status,
                    message = "Por favor, ingrese una fecha fin a buscar"
                });
            }

            if((dateProcessEnd - dateProcessStart).TotalDays > 5) {
                return Json(new {
                    status,
                    message = "Por favor, seleccione fechas de 5 días de diferencia"
                });
            }

            try {
                var response = string.Empty;

                object data = new {
                    BoxCode = args.BoxCode,
                    TurnCode = args.TurnCode,
                    OpeningDateStart = args.OpeningDateStart,
                    OpeningDateEnd = args.OpeningDateEnd,
                    ipPrivada=urlPrivada
                };

                string json = JsonConvert.SerializeObject(data);

                using(MyWebClientInfinite webClient = new MyWebClientInfinite()) {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";
                    webClient.Headers[HttpRequestHeader.Accept] = "application/json";
                    response = webClient.UploadString(urlPublica, "POST", json);
                }

                var settings = new JsonSerializerSettings {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                listReport = JsonConvert.DeserializeObject<List<Reporte_Detalle_TITO_Caja>>(response, settings);

                // start change data customers
                List<string> numberDocuments = listReport.Where(item => !string.IsNullOrEmpty(item.ClienteDni.Trim())).Select(item => item.ClienteDni.Trim()).Distinct().ToList();
                List<AST_ClienteEntidad> customers = ast_ClienteBL.GetListaMasivoClientesxNroDoc(numberDocuments);

                listReport.Where(item => !string.IsNullOrEmpty(item.ClienteDni.Trim())).ToList().ForEach(item => {

                    AST_ClienteEntidad customer = customers.Where(customerItem => customerItem.NroDoc.Trim().Equals(item.ClienteDni.Trim())).FirstOrDefault();

                    if(customer != null) {
                        item.ClienteTelefono = string.IsNullOrEmpty(item.ClienteTelefono.Trim()) ? customer.Celular1.Trim() : item.ClienteTelefono.Trim();
                        item.ClienteCorreo = string.IsNullOrEmpty(item.ClienteCorreo.Trim()) || item.ClienteCorreo.Trim().Equals("CORREO@PROVEEDOR.COM") ? customer.Mail.Trim() : item.ClienteCorreo.Trim();
                    }

                });
                // end change data customers

                status = 1;
            } catch(Exception ex) {
                return Json(new {
                    status = 2,
                    message = ex.Message.ToString(),
                    data = listReport.ToList()
                });
            }

            //keys tipo proceso
            int keyVentas = 1;
            int keyCompras = 2;
            int keyPagos = 5;
            int keyCortesias = 4;

            // totales de ventas = 1, compras = 2, pagos manuales = 5 y cortesía = 4
            int cantidadVentas = listReport.Where(item => item.idTipoProceso == keyVentas).Count();
            int cantidadCompras = listReport.Where(item => item.idTipoProceso == keyCompras).Count();
            int cantidadPagos = listReport.Where(item => item.idTipoProceso == keyPagos).Count();
            int cantidadCortesias = listReport.Where(item => item.idTipoProceso == keyCortesias).Count();
            int cantidadTickets = listReport.Count();

            decimal totalVentas = listReport.Where(item => item.idTipoProceso == keyVentas).Sum(item => item.Monto_Dinero);
            decimal totalCompras = listReport.Where(item => item.idTipoProceso == keyCompras).Sum(item => item.Monto_Dinero);
            decimal totalPagos = listReport.Where(item => item.idTipoProceso == keyPagos).Sum(item => item.Monto_Dinero);
            decimal totalCortesias = listReport.Where(item => item.idTipoProceso == keyCortesias).Sum(item => item.Monto_Dinero);
            decimal totalTickets = listReport.Sum(item => item.Monto_Dinero);

            // summary
            dynamic summary = new {
                company = sala.Empresa.RazonSocial,
                room = sala.Nombre,
                printDate = currentDate.ToString("dd/MM/yyyy"),
                printHour = currentDate.ToString("HH:mm:ss"),
                dateProcessStart = dateProcessStart.ToString("dd/MM/yyyy"),
                dateProcessEnd = dateProcessEnd.ToString("dd/MM/yyyy"),
                employee = personalName,
                cantidadVentas,
                cantidadCompras,
                cantidadPagos,
                cantidadCortesias,
                cantidadTickets,
                totalVentas = totalVentas.ToString("#,##0.00"),
                totalCompras = totalCompras.ToString("#,##0.00"),
                totalPagos = totalPagos.ToString("#,##0.00"),
                totalCortesias = totalCortesias.ToString("#,##0.00"),
                totalTickets = totalTickets.ToString("#,##0.00")
            };

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new {
                status,
                message = "Datos obtenidos correctamente",
                data = listReport.ToList(),
                summary
            };

            var result = new ContentResult {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };

            return result;

            /*return Json(new {
                status,
                message = "Datos obtenidos correctamente",
                data = listReport.ToList(),
                summary
            }, JsonRequestBehavior.AllowGet);*/
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ExcelTITOCajasVpn(ParametersTITOCaja args,string urlPublica,string urlPrivada) {
            int status = 0;
            DateTime currentDate = DateTime.Now;
            DateTime dateProcessStart = Convert.ToDateTime(args.OpeningDateStart);
            DateTime dateProcessEnd = Convert.ToDateTime(args.OpeningDateEnd);

            string message = string.Empty;
            string fileName = $"Reporte_Detallado_de_Tickets_en_Caja_{args.RoomCode}_{dateProcessStart.ToString("dd_MM_yyyy")}-{dateProcessEnd.ToString("dd_MM_yyyy")}_{currentDate.ToString("HHmmss")}.xlsx";
            string base64String = string.Empty;

            SEG_UsuarioEntidad usuario = segUsuarioBl.UsuarioEmpleadoIDObtenerJson(Convert.ToInt32(Session["UsuarioID"]));

            if(usuario == null) {
                return Json(new {
                    status,
                    message = "No se encontro un usuario, por favor inicie sesión"
                });
            }

            string personalName = usuario.UsuarioNombre;

            SalaEntidad sala = salaBl.ObtenerSalaEmpresa(Convert.ToInt32(args.RoomCode));

            if(sala == null) {
                return Json(new {
                    status,
                    message = "No se encontro sala, por favor ingrese datos correctos"
                });
            }

            if(args.BoxCode.Length <= 0) {
                return Json(new {
                    status,
                    message = "Por favor, ingrese los códigos de caja a buscar"
                });
            }

            if(args.TurnCode.Length <= 0) {
                return Json(new {
                    status,
                    message = "Por favor, ingrese los códigos de turno a buscar"
                });
            }

            if(string.IsNullOrEmpty(args.OpeningDateStart)) {
                return Json(new {
                    status,
                    message = "Por favor, ingrese una fecha inicio a buscar"
                });
            }

            if(string.IsNullOrEmpty(args.OpeningDateEnd)) {
                return Json(new {
                    status,
                    message = "Por favor, ingrese una fecha fin a buscar"
                });
            }

            if((dateProcessEnd - dateProcessStart).TotalDays > 5) {
                return Json(new {
                    status,
                    message = "Por favor, seleccione fechas de 5 días de diferencia"
                });
            }

            Dictionary<int, string> statusTiket = new Dictionary<int, string>
            {
                { 0, "ANULADO" },
                { 1, "PENDIENTE" },
                { 2, "PAGADO" },
                { 3, "VENCIDOS" }
            };

            Dictionary<int, string> processTypeCodes = new Dictionary<int, string>
            {
                { 1, "Ventas" },
                { 2, "Compras" },
                { 5, "Pagos Manuales" },
                { 4, "Cortesía" }
            };

            Dictionary<string, string> gendersCodes = new Dictionary<string, string>
            {
                { "F", "Femenino" },
                { "M", "Masculino" }
            };

            try {
                var response = string.Empty;

                object data = new {
                    BoxCode = args.BoxCode,
                    TurnCode = args.TurnCode,
                    OpeningDateStart = args.OpeningDateStart,
                    OpeningDateEnd = args.OpeningDateEnd,
                    ipPrivada=urlPrivada
                };

                string json = JsonConvert.SerializeObject(data);

                using(MyWebClientInfinite webClient = new MyWebClientInfinite()) {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";
                    webClient.Headers[HttpRequestHeader.Accept] = "application/json";
                    response = webClient.UploadString(urlPublica, "POST", json);
                }

                var settings = new JsonSerializerSettings {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                List<Reporte_Detalle_TITO_Caja> listReport = JsonConvert.DeserializeObject<List<Reporte_Detalle_TITO_Caja>>(response, settings);

                // start change data customers
                List<string> numberDocuments = listReport.Where(item => !string.IsNullOrEmpty(item.ClienteDni.Trim())).Select(item => item.ClienteDni.Trim()).Distinct().ToList();
                List<AST_ClienteEntidad> customers = ast_ClienteBL.GetListaMasivoClientesxNroDoc(numberDocuments);

                listReport.Where(item => !string.IsNullOrEmpty(item.ClienteDni.Trim())).ToList().ForEach(item => {

                    AST_ClienteEntidad customer = customers.Where(customerItem => customerItem.NroDoc.Trim().Equals(item.ClienteDni.Trim())).FirstOrDefault();

                    if(customer != null) {
                        item.ClienteTelefono = string.IsNullOrEmpty(item.ClienteTelefono.Trim()) ? customer.Celular1.Trim() : item.ClienteTelefono.Trim();
                        item.ClienteCorreo = string.IsNullOrEmpty(item.ClienteCorreo.Trim()) || item.ClienteCorreo.Trim().Equals("CORREO@PROVEEDOR.COM") ? customer.Mail.Trim() : item.ClienteCorreo.Trim();
                    }

                });
                // end change data customers

                // Generated Excel
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excel = new ExcelPackage();

                var workSheet = excel.Workbook.Worksheets.Add("Reporte Detallado");
                workSheet.TabColor = Color.Black;
                workSheet.DefaultRowHeight = 12;

                //Body of table
                int initCol = 2;
                int initRow = 12;
                int recordIndex = initRow + 1;
                int total = listReport.Count;
                int totalRows = initRow + total;
                int footerRow = totalRows + 1;

                //keys tipo proceso
                int keyVentas = 1;
                int keyCompras = 2;
                int keyPagos = 5;
                int keyCortesias = 4;

                // totales de ventas = 1, compras = 2, pagos manuales = 5 y cortesía = 4
                int cantidadVentas = listReport.Where(item => item.idTipoProceso == keyVentas).Count();
                decimal totalVentas = listReport.Where(item => item.idTipoProceso == keyVentas).Sum(item => item.Monto_Dinero);
                int cantidadCompras = listReport.Where(item => item.idTipoProceso == keyCompras).Count();
                decimal totalCompras = listReport.Where(item => item.idTipoProceso == keyCompras).Sum(item => item.Monto_Dinero);
                int cantidadPagos = listReport.Where(item => item.idTipoProceso == keyPagos).Count();
                decimal totalPagos = listReport.Where(item => item.idTipoProceso == keyPagos).Sum(item => item.Monto_Dinero);
                int cantidadCortesias = listReport.Where(item => item.idTipoProceso == keyCortesias).Count();
                decimal totalCortesias = listReport.Where(item => item.idTipoProceso == keyCortesias).Sum(item => item.Monto_Dinero);
                decimal totalTickets = listReport.Sum(item => item.Monto_Dinero);

                // Title of table
                workSheet.Cells["D4:N4"].Merge = true;
                workSheet.Cells[4, 4].Style.Font.Bold = true;
                workSheet.Cells[4, 4].Style.Font.Size = 16;
                workSheet.Cells[4, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[4, 4].Value = "REPORTE DETALLADO DE TICKETS EN CAJA";

                workSheet.Cells["E2:G2"].Merge = true;
                workSheet.Cells["E3:G3"].Merge = true;
                workSheet.Cells["D2:D3"].Style.Font.Bold = true;
                workSheet.Cells[2, 4].Value = "Empresa";
                workSheet.Cells[3, 4].Value = "Sala";
                workSheet.Cells[2, 5].Value = sala.Empresa.RazonSocial; // set Empresa
                workSheet.Cells[3, 5].Value = sala.Nombre; // set Sala

                workSheet.Cells["M2:M3"].Style.Font.Bold = true;
                workSheet.Cells[2, 13].Value = "Fecha Imp.";
                workSheet.Cells[3, 13].Value = "Hora Imp.";
                workSheet.Cells[2, 14].Value = currentDate.ToString("dd/MM/yyyy"); // set Fecha Imp.
                workSheet.Cells[3, 14].Value = currentDate.ToString("HH:mm:ss"); // set Hora Imp.

                workSheet.Cells["E5:G5"].Merge = true;
                workSheet.Cells["E6:G6"].Merge = true;
                workSheet.Cells["D5:D6"].Style.Font.Bold = true;
                workSheet.Cells[5, 4].Value = "Fecha";
                workSheet.Cells[6, 4].Value = "Caja";
                workSheet.Cells[5, 5].Value = $"{dateProcessStart.ToString("dd/MM/yyyy")} - {dateProcessEnd.ToString("dd/MM/yyyy")}"; // set Fecha Proceso
                workSheet.Cells[6, 5].Value = string.Join(", ", args.BoxName); // set Caja

                workSheet.Cells["N5:O5"].Merge = true;
                workSheet.Cells["N6:O6"].Merge = true;
                workSheet.Cells["M5:M6"].Style.Font.Bold = true;
                workSheet.Cells[5, 13].Value = "Turno";
                workSheet.Cells[6, 13].Value = "Personal";
                workSheet.Cells[5, 14].Value = string.Join(", ", args.TurnCode); // set Turno
                workSheet.Cells[6, 14].Value = personalName; // set Personal

                workSheet.Cells["D8:E8"].Merge = true;
                workSheet.Cells["D9:D10"].Style.Font.Bold = true;
                workSheet.Cells[8, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[8, 4].Style.Font.Bold = true;
                workSheet.Cells[8, 4].Value = "Ventas";
                workSheet.Cells[9, 4].Value = "Cantidad";
                workSheet.Cells[10, 4].Value = "Total";
                workSheet.Cells[9, 5].Value = cantidadVentas.ToString(); // set Cantidad Ventas
                workSheet.Cells[10, 5].Value = totalVentas.ToString("#,##0.00"); // set Total Ventas

                workSheet.Cells["G8:H8"].Merge = true;
                workSheet.Cells["G9:G10"].Style.Font.Bold = true;
                workSheet.Cells[8, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[8, 7].Style.Font.Bold = true;
                workSheet.Cells[8, 7].Value = "Compras";
                workSheet.Cells[9, 7].Value = "Cantidad";
                workSheet.Cells[10, 7].Value = "Total";
                workSheet.Cells[9, 8].Value = cantidadCompras.ToString(); // set Cantidad Compras
                workSheet.Cells[10, 8].Value = totalCompras.ToString("#,##0.00"); // set Total Compras

                workSheet.Cells["J8:K8"].Merge = true;
                workSheet.Cells["J9:J10"].Style.Font.Bold = true;
                workSheet.Cells[8, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[8, 10].Style.Font.Bold = true;
                workSheet.Cells[8, 10].Value = "Pagos Manuales";
                workSheet.Cells[9, 10].Value = "Cantidad";
                workSheet.Cells[10, 10].Value = "Total";
                workSheet.Cells[9, 11].Value = cantidadPagos.ToString(); // set Cantidad Pagos Manuales
                workSheet.Cells[10, 11].Value = totalPagos.ToString("#,##0.00"); // set Total Cantidad Pagos Manuales

                workSheet.Cells["M8:N8"].Merge = true;
                workSheet.Cells["M9:M10"].Style.Font.Bold = true;
                workSheet.Cells[8, 13].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[8, 13].Style.Font.Bold = true;
                workSheet.Cells[8, 13].Value = "Cortesía";
                workSheet.Cells[9, 13].Value = "Cantidad";
                workSheet.Cells[10, 13].Value = "Total";
                workSheet.Cells[9, 14].Value = cantidadCortesias.ToString(); // set Cantidad Cortesía
                workSheet.Cells[10, 14].Value = totalCortesias.ToString("#,##0.00"); // set Total Cortesía

                //Header of table  
                workSheet.Row(initRow).Height = 20;
                workSheet.Row(initRow).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(initRow).Style.Font.Bold = true;

                workSheet.Cells[initRow, 2].Value = "DNI";
                workSheet.Cells[initRow, 3].Value = "Cliente";
                workSheet.Cells[initRow, 4].Value = "Género";
                workSheet.Cells[initRow, 5].Value = "F. Nacimiento";
                workSheet.Cells[initRow, 6].Value = "Correo";
                workSheet.Cells[initRow, 7].Value = "Teléfono";
                workSheet.Cells[initRow, 8].Value = "Tipo de Proceso";
                workSheet.Cells[initRow, 9].Value = "Fecha y Hora Ticket";
                workSheet.Cells[initRow, 10].Value = "Tipo Pago";
                workSheet.Cells[initRow, 11].Value = "Tipo Origen";
                workSheet.Cells[initRow, 12].Value = "Lugar Origen";
                workSheet.Cells[initRow, 13].Value = "Código Extra";
                workSheet.Cells[initRow, 14].Value = "Nro. Ticket";
                workSheet.Cells[initRow, 15].Value = "Personal";
                workSheet.Cells[initRow, 16].Value = "Estado";
                workSheet.Cells[initRow, 17].Value = "Promoción";
                workSheet.Cells[initRow, 18].Value = "Monto Din.";

                foreach(Reporte_Detalle_TITO_Caja report in listReport) {
                    string dateBirth = string.IsNullOrEmpty(report.ClienteFechaNacimiento) ? "" : Convert.ToDateTime(report.ClienteFechaNacimiento).ToString("dd/MM/yyyy");
                    string customerGender = string.IsNullOrEmpty(report.ClienteGenero) ? "NA" : report.ClienteGenero;

                    workSheet.Cells[recordIndex, 2].Value = report.ClienteDni;
                    workSheet.Cells[recordIndex, 3].Value = report.Cliente;
                    workSheet.Cells[recordIndex, 4].Value = gendersCodes.ContainsKey(customerGender) ? gendersCodes[customerGender] : "";
                    workSheet.Cells[recordIndex, 5].Value = dateBirth.Equals("01/01/1753") ? "" : dateBirth;
                    workSheet.Cells[recordIndex, 6].Value = report.ClienteCorreo;
                    workSheet.Cells[recordIndex, 7].Value = report.ClienteTelefono;
                    workSheet.Cells[recordIndex, 8].Value = processTypeCodes.ContainsKey(report.idTipoProceso) ? processTypeCodes[report.idTipoProceso] : "";
                    workSheet.Cells[recordIndex, 9].Value = report.Fecha_Proceso.ToString("dd/MM/yyyy HH:mm");
                    workSheet.Cells[recordIndex, 10].Value = report.TipoPago_desc;
                    workSheet.Cells[recordIndex, 11].Value = report.TipoOrigen;
                    workSheet.Cells[recordIndex, 12].Value = report.LugarOrigen;
                    workSheet.Cells[recordIndex, 13].Value = report.CodigoExtra;
                    workSheet.Cells[recordIndex, 14].Value = report.Ticket;
                    workSheet.Cells[recordIndex, 15].Value = report.Personal;
                    workSheet.Cells[recordIndex, 16].Value = statusTiket.ContainsKey(report.EstadoTiket) ? statusTiket[report.EstadoTiket] : "";
                    workSheet.Cells[recordIndex, 17].Value = report.Motivo;
                    workSheet.Cells[recordIndex, 18].Value = report.Monto_Dinero.ToString("#,##0.00");

                    recordIndex++;
                }

                Color colbackground = ColorTranslator.FromHtml("#003268");
                Color colborder = ColorTranslator.FromHtml("#074B88");

                workSheet.Cells["B12:R12"].Style.Font.Bold = true;
                workSheet.Cells["B12:R12"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["B12:R12"].Style.Fill.BackgroundColor.SetColor(colbackground);
                workSheet.Cells["B12:R12"].Style.Font.Color.SetColor(Color.White);
                workSheet.Cells["B12:R12"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet.Cells["B12:R12"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                workSheet.Cells["B12:R12"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet.Cells["B12:R12"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                workSheet.Cells["B12:R12"].Style.Border.Top.Color.SetColor(colborder);
                workSheet.Cells["B12:R12"].Style.Border.Left.Color.SetColor(colborder);
                workSheet.Cells["B12:R12"].Style.Border.Right.Color.SetColor(colborder);
                workSheet.Cells["B12:R12"].Style.Border.Bottom.Color.SetColor(colborder);

                workSheet.Cells["B13:B" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["D13:D" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["E13:E" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["F13:F" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["G13:G" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["H13:H" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["I13:I" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["J13:J" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["K13:K" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["L13:L" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["M13:M" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["N13:N" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["O13:O" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["P13:P" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["Q13:Q" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["R13:R" + recordIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                workSheet.Cells["B" + footerRow + ":R" + footerRow].Merge = true;
                workSheet.Cells["B" + footerRow + ":R" + footerRow].Style.Font.Bold = true;
                workSheet.Cells["B" + footerRow + ":R" + footerRow].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["B" + footerRow + ":R" + footerRow].Style.Fill.BackgroundColor.SetColor(colbackground);
                workSheet.Cells["B" + footerRow + ":R" + footerRow].Style.Font.Color.SetColor(Color.White);
                workSheet.Cells["B" + footerRow + ":R" + footerRow].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[footerRow, initCol].Value = "Total : " + total + " Registros";
                workSheet.Cells[initRow, initCol, totalRows, 18].AutoFilter = true;

                workSheet.Cells[totalRows + 3, 18].Style.Font.Bold = true;
                workSheet.Cells[totalRows + 3, 18].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                workSheet.Cells[totalRows + 3, 18].Value = totalTickets.ToString("#,##0.00");

                workSheet.Column(1).AutoFit(1);
                workSheet.Column(2).AutoFit();
                workSheet.Column(3).AutoFit();
                workSheet.Column(4).AutoFit();
                workSheet.Column(5).AutoFit();
                workSheet.Column(6).AutoFit();
                workSheet.Column(7).AutoFit();
                workSheet.Column(8).AutoFit();
                workSheet.Column(9).AutoFit();
                workSheet.Column(10).AutoFit();
                workSheet.Column(11).AutoFit();
                workSheet.Column(12).AutoFit();
                workSheet.Column(13).AutoFit();
                workSheet.Column(14).AutoFit();
                workSheet.Column(15).AutoFit();
                workSheet.Column(16).AutoFit();
                workSheet.Column(17).AutoFit();
                workSheet.Column(18).AutoFit();

                var memoryStream = new MemoryStream();
                excel.SaveAs(memoryStream);

                base64String = Convert.ToBase64String(memoryStream.ToArray());

                status = 1;
                message = "Se genero un Archivo Excel";
                // Generated Excel
            } catch(Exception ex) {
                return Json(new {
                    status = 2,
                    message = message + " " + ex.Message.ToString(),
                    data = base64String,
                    filename = fileName
                });
            }

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new {
                status,
                message,
                data = base64String,
                filename = fileName
            };

            var result = new ContentResult {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };

            return result;

            /*return Json(new { 
                status,
                message,
                data = base64String,
                filename = fileName
            });*/
        }

        #region Apertura Caja

        [HttpGet]
        public ActionResult AperturaCajas()
        {
            return View("~/Views/Reportes/AperturaCaja/AperturaCajas.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public async Task<ActionResult> ListarAperturaCajas(int salaId)
        {
            bool success = false;
            bool inVpn = false;
            string message = "No se encontraron registros";

            if (salaId <= 0)
            {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione una sala"
                });
            }

            List<AperturarCajaEntidad> data = new List<AperturarCajaEntidad>();

            try
            {
                SalaVpnEntidad sala = salaBl.ObtenerSalaVpnPorCodigo(salaId);

                if (sala.CodSala == 0)
                {
                    return Json(new
                    {
                        success,
                        message = "No se ha encontrado datos de la sala"
                    });
                }

                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, new List<string>());

                if (!tcpConnection.IsOpen)
                {
                    return Json(new
                    {
                        success,
                        message = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                    });
                }

                string servicePath = "Servicio/ObtenerAperturaCajas";
                string content = string.Empty;
                string requestUri = string.Empty;

                if (tcpConnection.IsVpn)
                {
                    inVpn = true;

                    object arguments = new
                    {
                        ipPrivada = $"{sala.IpPrivada}:{sala.PuertoServicioWebOnline}/{servicePath}"
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{tcpConnection.Url}/Servicio/VPNGenericoPost";
                }
                else
                {
                    content = JsonConvert.SerializeObject(string.Empty);
                    requestUri = $"{sala.UrlProgresivo}/{servicePath}";
                }

                ResultEntidad result = await _serviceHelper.PostAsync(requestUri, content);

                if (result.success)
                {
                    List<AperturarCajaEntidad> listaAperturas = JsonConvert.DeserializeObject<List<AperturarCajaEntidad>>(result.data) ?? new List<AperturarCajaEntidad>();

                    if (listaAperturas.Any())
                    {
                        listaAperturas.ForEach(item => {
                            item.NombreSala = sala.Nombre;
                            item.NombreEmpresa = sala.NombreEmpresa;
                        });

                        data = listaAperturas;

                        success = true;
                        message = "Registros obtenidos correctamente";
                    }
                }
                else
                {
                    success = false;
                    message = result.message;
                }
            }
            catch (Exception exception)
            {
                success = false;
                message = exception.Message;
            }

            return Json(new
            {
                success,
                message,
                data,
                inVpn
            });
        }

        [seguridad(false)]
        [HttpPost]
        public async Task<ActionResult> ExcelAperturaCajas(int salaId)
        {
            bool success = false;
            bool inVpn = false;
            string message = "No se encontraron registros";

            if (salaId <= 0)
            {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione una sala"
                });
            }

            SalaVpnEntidad sala = salaBl.ObtenerSalaVpnPorCodigo(salaId);

            if (sala.CodSala == 0)
            {
                return Json(new
                {
                    success,
                    message = "No se ha encontrado datos de la sala"
                });
            }

            DateTime currentDate = DateTime.Now;
            string fileExtension = "xlsx";
            string fileName = $"AperturaCajas_{sala.Nombre}_{currentDate.ToString("dd-MM-yyyy")}_{currentDate.ToString("HHmmss")}.{fileExtension}";
            string data = string.Empty;

            try
            {
                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, new List<string>());

                if (!tcpConnection.IsOpen)
                {
                    return Json(new
                    {
                        success,
                        message = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                    });
                }

                string servicePath = "Servicio/ObtenerAperturaCajas";
                string content = string.Empty;
                string requestUri = string.Empty;

                if (tcpConnection.IsVpn)
                {
                    inVpn = true;

                    object arguments = new
                    {
                        ipPrivada = $"{sala.IpPrivada}:{sala.PuertoServicioWebOnline}/{servicePath}"
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{tcpConnection.Url}/Servicio/VPNGenericoPost";
                }
                else
                {
                    content = JsonConvert.SerializeObject(string.Empty);
                    requestUri = $"{sala.UrlProgresivo}/{servicePath}";
                }

                ResultEntidad result = await _serviceHelper.PostAsync(requestUri, content);

                if (result.success)
                {
                    List<AperturarCajaEntidad> listaAperturas = JsonConvert.DeserializeObject<List<AperturarCajaEntidad>>(result.data) ?? new List<AperturarCajaEntidad>();

                    if (listaAperturas.Any())
                    {
                        listaAperturas.ForEach(item => {
                            item.NombreSala = sala.Nombre;
                            item.NombreEmpresa = sala.NombreEmpresa;
                        });

                        MemoryStream excelStream = AperturaCajaReport.ExcelAperturaCajas(listaAperturas);
                        data = Convert.ToBase64String(excelStream.ToArray());

                        success = true;
                        message = "Excel generado";
                    }
                }
                else
                {
                    success = false;
                    message = result.message;
                }
            }
            catch (Exception exception)
            {
                success = false;
                message = exception.Message;
            }

            return Json(new
            {
                success,
                message,
                fileName,
                data,
                inVpn
            });
        }

        #endregion
    }
}