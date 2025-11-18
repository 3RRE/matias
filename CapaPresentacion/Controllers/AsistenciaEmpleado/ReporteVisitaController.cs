using CapaEntidad.AsistenciaEmpleado;
using CapaNegocio.AsistenciaEmpleado;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebSockets;

namespace CapaPresentacion.Controllers.AsistenciaEmpleado
{
    [seguridad]
    public class ReporteVisitaController : Controller
    {
        string ubicacionarchivos = ConfigurationManager.AppSettings["PathArchivos"].ToString();
        private EmpleadoVisitaBL empleadoVisitaBL = new EmpleadoVisitaBL();

        public ActionResult ReporteVisita()
        {
            return View("~/Views/AsistenciaEmpleado/ReporteVisita.cshtml");
        }


        [HttpPost]
        public ActionResult GetReporteVisitaJson(int[] ArraySalaId, DateTime fechaIni, DateTime fechaFin)
        {
            string mensaje = "";
            bool respuesta = false;
            int cantElementos = (ArraySalaId == null) ? 0 : ArraySalaId.Length;
            var strElementos = String.Empty;
            List<EmpleadoVisitaEmpleadoEntidad> lista = new List<EmpleadoVisitaEmpleadoEntidad>();

            try
            {

                //string strSalas =  String.Join(",", ArraySalaId);
                if (cantElementos > 0)
                {
                    strElementos = " a.sala_id in(" + "'" + String.Join("','", ArraySalaId) + "'" + ") and ";
                }
                lista = empleadoVisitaBL.VisitaListaxFechabetweenListarJson(fechaIni, fechaFin, strElementos);

            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data = lista.ToList() });
        }

        [HttpPost]
        public ActionResult GetReporteVisitaDetalleJson(Int64 vis_id)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
          
            List<EmpleadoVisitaDetalleEntidad> lista = new List<EmpleadoVisitaDetalleEntidad>();

            try
            {

              
                lista = empleadoVisitaBL.VisitaListaDetalleJson(vis_id);

            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data = lista.ToList() });
        }

        [HttpPost]
        public ActionResult GetReporteVisitaDetalleIdJson(Int64 visd_id)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            string imagen = "";
            EmpleadoVisitaDetalleEntidad registro = new EmpleadoVisitaDetalleEntidad();
            var direccion = ubicacionarchivos + "/reporteVisita/";
            try
            {


                registro = empleadoVisitaBL.visitadetalleId(visd_id);
                byte[] bmp = ImageToBinary(direccion + "" + registro.imagen);
                imagen = Convert.ToBase64String(bmp);
                mensaje = "Detalle Reporte";
                respuesta = true;

            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            JsonResult response = Json(new { mensaje, respuesta, data = imagen });
            response.MaxJsonLength= int.MaxValue;

            return response;
        }

        [HttpPost]
        public ActionResult VisitaDescargarExcelJson(int[] ArraySalaId, DateTime fechaini, DateTime fechafin)
        {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            int cantElementos = (ArraySalaId == null) ? 0 : ArraySalaId.Length;
            var strElementos = String.Empty;
            List<EmpleadoVisitaEmpleadoEntidad> lista = new List<EmpleadoVisitaEmpleadoEntidad>();

            try
            {
                if (cantElementos > 0)
                {
                    strElementos = " a.sala_id in(" + "'" + String.Join("','", ArraySalaId) + "'" + ") and ";
                }
                lista = empleadoVisitaBL.VisitaListaxFechabetweenListarJson(fechaini, fechafin, strElementos);

                if (lista.Count>0)
                {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Reporte de Visita " + DateTime.Today.ToString("dd-MM-yyyy"));
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //

                    workSheet.Cells["B" + 2 + ":G" + 2].Merge = true;
                    workSheet.Cells[2, 2].Value = "Reporte de Visita del : " + fechaini.ToString("dd-MM-yyyy") + " al : " + fechafin.ToString("dd-MM-yyyy");
                    workSheet.Cells[2, 2].Style.Font.Bold = true;

                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;

                    workSheet.Cells[3, 2].Value = "Id";
                    workSheet.Cells[3, 3].Value = "Emp. ID";
                    workSheet.Cells[3, 4].Value = "Nombre y Apellidos";
                    workSheet.Cells[3, 5].Value = "Titulo";
                    workSheet.Cells[3, 6].Value = "Fecha";
                    workSheet.Cells[3, 7].Value = "Hora";
                    workSheet.Cells[3, 8].Value = "Imei";
                    //Body of table  
                    //  
                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach (var registro in lista)
                    {
                        //workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                        workSheet.Cells[recordIndex, 2].Value = registro.vis_id;
                        workSheet.Cells[recordIndex, 3].Value = registro.empleado_id;
                        workSheet.Cells[recordIndex, 4].Value = registro.emp_nombre + " " + registro.emp_ape_paterno + " " + registro.emp_ape_materno;
                        workSheet.Cells[recordIndex, 5].Value = registro.titulo;
                        workSheet.Cells[recordIndex, 6].Value = registro.fechaRegistro.ToString("dd-MM-yyyy");
                        workSheet.Cells[recordIndex, 7].Value = registro.fechaRegistro.ToString("HH:mm tt");
                        workSheet.Cells[recordIndex, 8].Value = registro.imei;
                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:H3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:H3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:H3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:H3"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B3:H3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:H3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:H3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:H3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B3:H3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:H3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:H3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:H3"].Style.Border.Bottom.Color.SetColor(colborder);

                    //workSheet.Cells.AutoFitColumns(60, 250);//overloaded min and max lengths
                    workSheet.Cells["B3:B" + total + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["G3:H" + total + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    int filaFooter = total + 4;
                    workSheet.Cells["B" + filaFooter + ":H" + filaFooter].Merge = true;
                    workSheet.Cells["B" + filaFooter + ":H" + filaFooter].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter + ":H" + filaFooter].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter + ":H" + filaFooter].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter + ":H" + filaFooter].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter + ":H" + filaFooter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[filaFooter, 2].Value = "Total : " + total + " Registros";

                    int filaultima = total + 3;
                    //workSheet.Cells["C1:C" + (total + 1)].AutoFilter = true;
                    //workSheet.Cells["D1:D" + (total + 1)].AutoFilter = true;
                    workSheet.Cells[3, 2, filaultima, 8].AutoFilter = true;

                    //workSheet.Column(1).AutoFit();
                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).AutoFit();
                    workSheet.Column(4).Width = 30;
                    workSheet.Column(5).Width = 25;
                    workSheet.Column(6).Width = 30;
                    workSheet.Column(7).Width = 25;
                    workSheet.Column(8).Width = 20;
                    excelName = DateTime.Today.ToString("dd_MM_yyyy") + "_reporte_visita.xlsx";

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
        // GET: ReporteVisita
        [HttpPost]
        public ActionResult VisitaInsertarJson(EmpleadoVisitaEntidad reporte, List<EmpleadoVisitaDetalleEntidad> detalle, String imei)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            Int64 idReporte = 0;
            var direccion = ubicacionarchivos+"/reporteVisita/";
           
            try
            {              
                if (!Directory.Exists(direccion))
                {
                    Directory.CreateDirectory(direccion);
                }

                reporte.fechaRegistro = DateTime.Now;
                idReporte = empleadoVisitaBL.EmpleadoVisitaInsertarJson(reporte);
                

                if (idReporte>0)
                {
                   
                    var carpeta = "_" + idReporte + "_" + DateTime.Now.ToString("dd_MM_yyyy") + "/";
                    var direccionReporte = direccion + "/" + carpeta;
                    if (!Directory.Exists(direccionReporte))
                    {
                        Directory.CreateDirectory(direccionReporte);
                    }

                    var index = 0;
                    foreach (var item in detalle)
                    {
                        index++;
                        var nombreArchivo = "_" + index + "_" + idReporte + ".bmp";
                        item.visita_id = idReporte;
                        item.imagen = carpeta + nombreArchivo;
                        item.fechaRegistro = reporte.fechaRegistro;
                        var DetalleTupla = empleadoVisitaBL.EmpleadoVisitaDetalleInsertarJson(item);

                        byte[] imagen = Convert.FromBase64String(item.imagen_str.Trim());
                        Bitmap imgcelular = new Bitmap(ConvertByteToImg(imagen));
                        imgcelular.Save(direccionReporte + nombreArchivo, System.Drawing.Imaging.ImageFormat.Bmp);
                        imgcelular.Dispose();

                    }
                    mensaje = "Visita Registrado.";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = "";
                    mensaje = "No se Pudo Registrar La Visita";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta, mensaje, mensajeConsola });
        }

        public Image ConvertByteToImg(Byte[] img)
        {
            //Image FetImg;
            //MemoryStream ms = new MemoryStream(img);
            //FetImg = Image.FromStream(ms);
            //ms.Close();
            //return FetImg;
            using (var stream = new MemoryStream(img))
            {
                return Image.FromStream(stream);
            }
        }



        [seguridad(false)]
        [HttpPost]
        public ActionResult pruebaprint()
        {
            var errormensaje = "";
            bool respuesta = false;
            string ubicacionarchivos = ConfigurationManager.AppSettings["PathArchivos"].ToString();
            try
            {
                string ipAddress = "172.21.0.252";
                int port = 9100;

                string zplImageData = string.Empty;
                string filePath = ubicacionarchivos + "/rostros/descarga.jpg";
                byte[] binaryData = System.IO.File.ReadAllBytes(filePath);
                foreach (Byte b in binaryData)
                {
                    string hexRep = String.Format("{0:X}", b);
                    if (hexRep.Length == 1)
                        hexRep = "0" + hexRep;
                    zplImageData += hexRep;
                }
                string zplToSend = "^XA" + "^FO50" + "50^GFA,120000,120000,100" + binaryData.Length + ",," + zplImageData + "^XZ";
                string printImage = "^XA^FO115,50^IME:LOGO.PNG^FS^XZ";

                System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
                client.Connect(ipAddress, port);

                // Write ZPL String to connection
                System.IO.StreamWriter writer = new System.IO.StreamWriter(client.GetStream(), Encoding.UTF8);
               // writer.Write(zplToSend);
                writer.Flush();
                writer.Write(printImage);
                writer.Flush();
                // Close Connection
                writer.Close();
                client.Close();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje });
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
    }
}