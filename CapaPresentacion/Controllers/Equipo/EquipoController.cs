using CapaEntidad;
using CapaEntidad.Alertas;
using CapaEntidad.Disco;
using CapaEntidad.Discos;
using CapaEntidad.Equipo;
using CapaNegocio;
using CapaNegocio.Disco;
using CapaNegocio.Discos;
using CapaNegocio.Equipo;
using CapaPresentacion.Utilitarios;
using Newtonsoft.Json;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Equipo
{
    [seguridad]
    public class EquipoController : Controller
    {
        private readonly EquipoBL _equipoBl = new EquipoBL();
        private string PathLogAlertaBilleteros = ConfigurationManager.AppSettings["PathLogAlertaBilleteros"];
        private readonly LogTransac _log = new LogTransac();
        private string FirebaseKey = ConfigurationManager.AppSettings["firebaseServiceKey"];
        private SalaBL _salaBl = new SalaBL();
        private AlertaDiscoBL _alertaDiscoBL = new AlertaDiscoBL();

        // GET: Equipo
        public ActionResult EquipoIndex()
        {
            return View("~/Views/Equipo/EquipoControl.cshtml");

        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult AgregarInfoEquipoSala(EquipoEntidad nuevoEquipoInfo)
        {
            var errormensaje = "";
            Int64 alt_id = 0;
            bool respuesta = false;
            var titulo = "¡Alertas RAM!";
            var servidorKey = FirebaseKey;
            DateTime fecha = DateTime.Now;
            int sala = 0;
            int lista;
            List<AlertaDiscoEntidad> devices = new List<AlertaDiscoEntidad>();
            List<string> correos = new List<string>();

            int salaNuevoEquipo = nuevoEquipoInfo.CodSala;
            DateTime fechaNuevoEquipo = nuevoEquipoInfo.FechaRegistro;
            EquipoEntidad ultimoRegistro = _equipoBl.ObtenerUltimoRegistro(salaNuevoEquipo);

            TimeSpan diferencia = fechaNuevoEquipo.Subtract(ultimoRegistro.FechaRegistro);

            SalaEntidad datosSala = new SalaEntidad();

            if (diferencia.TotalMinutes > 5)
            {
                try
                {

                    lista = _equipoBl.AgregarEquipoInfo(nuevoEquipoInfo);
                    datosSala = _salaBl.ListadoSala().Where(x => x.CodSala == salaNuevoEquipo).First();

                    devices = _alertaDiscoBL.AlertaDisco_xdevicesListado(salaNuevoEquipo);
                    correos = _alertaDiscoBL.AlertaDiscosCorreosListado(datosSala.CodSala);
                    List<string> cod_maquinas = new List<string>();
                    string maquinas = string.Empty;

                    if (devices.Count == 0)
                    {
                        errormensaje = "No se encontraron dispositivos para envio , LLame al Administrador";
                        respuesta = false;
                    }
                    else
                    {
                        respuesta = true;
                        errormensaje = "Se detecto alto uso de RAM de la sala  " + datosSala.Nombre;
                    }
                    respuesta = true;
                    string[] dispositivos = devices.Select(x => x.id).ToArray();

                    if (dispositivos.Length > 0)
                    {
                        EnvioFirebase(servidorKey, dispositivos, errormensaje, titulo);

                    }
                    if (correos.Count > 0)
                    {
                        EnviarCorreos(correos.ToArray(), datosSala.CodSala, nuevoEquipoInfo);

                    }
                    devices.Clear();
                }
                catch (Exception exp)
                {
                    errormensaje = exp.Message + "Error,Llame Administrador";
                }
            }

            return Json(new { data = nuevoEquipoInfo, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListadoEquipoInfo(string codSala, DateTime fechaIni, DateTime fechaFin)
        {
            var errormensaje = "";
            var lista = new List<EquipoEntidad>();

            try
            {
                lista = _equipoBl.ListadoEquiposInfo(Convert.ToInt32(codSala), fechaIni, fechaFin).OrderByDescending(x => x.FechaRegistro).ToList();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }

            return Json(new { data = lista, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReporteEquipoDescargarExcelJson(int codsala, DateTime fechaini, DateTime fechafin)
        {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<EquipoEntidad> lista = new List<EquipoEntidad>();
            string nombreSala = _salaBl.ListadoSala().First(x => x.CodSala == codsala).Nombre;

            Color alerta = ColorTranslator.FromHtml("#D94E2F");
            Color normal = ColorTranslator.FromHtml("#32CD5B");
            //string nombreSala=_salaBl.ObtenerNombreSala(codsala);

            try
            {
                //lista = alertaSalaBL.ALT_AlertaSala_xsala_idListado(strElementos, fechaini, fechafin);
                lista = _equipoBl.ListadoEquiposInfo(codsala, fechaini, fechafin);
                if (lista.Count > 0)
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Equipo");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[2, 2].Value = "SALA : " + nombreSala;

                    workSheet.Cells[3, 2].Value = "Ip";
                    workSheet.Cells[3, 3].Value = "Capacidad RAM";
                    workSheet.Cells[3, 4].Value = "Capacidad RAM libre";
                    workSheet.Cells[3, 5].Value = "Capacidad RAM ocupada";
                    workSheet.Cells[3, 6].Value = "Porcentaje uso de RAM";
                    workSheet.Cells[3, 7].Value = "Porcentaje de uso de la CPU";
                    workSheet.Cells[3, 8].Value = "Procesos de la CPU";
                    workSheet.Cells[3, 9].Value = "Fecha de registro";

                    //Body of table  
                    //  
                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach (var registro in lista)
                    {

                        workSheet.Cells[recordIndex, 2].Value = registro.IpEquipo;
                        workSheet.Cells[recordIndex, 3].Value = registro.MemoriaTotal;
                        workSheet.Cells[recordIndex, 4].Value = registro.MemoriaDisponible;
                        workSheet.Cells[recordIndex, 5].Value = registro.MemoriaUsada;
                        workSheet.Cells[recordIndex, 6].Value = registro.PorcentajeUsoRam;
                        workSheet.Cells[recordIndex, 7].Value = registro.PorcentajeCpu;
                        workSheet.Cells[recordIndex, 8].Value = registro.ProcesosCpu;
                        workSheet.Cells[recordIndex, 9].Value = registro.FechaRegistro.ToString("dd-MM-yyyy hh:mm:ss tt");
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
                    workSheet.Cells["N4:N" + total].Style.Numberformat.Format = "#,##0.00";
                    workSheet.Cells["M4:M" + total].Style.Numberformat.Format = "#,##0.00";
                    workSheet.Cells["B4:B" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["B2:I2"].Merge = true;
                    workSheet.Cells["B2:I2"].Style.Font.Bold = true;

                    int filaFooter = total + 1;
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Merge = true;
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    //workSheet.Cells[filaFooter, 2].Value = "Total Monto : ";
                    workSheet.Cells[filaFooter, 13].Style.Numberformat.Format = "#,##0.00";
                    workSheet.Cells[filaFooter, 14].Style.Numberformat.Format = "#,##0.00";
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.Font.Size = 14;

                    workSheet.Cells["B4:I" + total].Style.WrapText = true;

                    int filaFooter_ = total + 2;
                    workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Merge = true;
                    workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[filaFooter_, 2].Value = "Total : " + (total - filasagregadas) + " Registros";

                    int filaultima = total;
                    workSheet.Cells[3, 2, filaultima, 10].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 20;
                    workSheet.Column(4).Width = 25;
                    workSheet.Column(5).Width = 25;
                    workSheet.Column(6).Width = 25;
                    workSheet.Column(7).Width = 24;
                    workSheet.Column(8).Width = 24;
                    workSheet.Column(9).Width = 27;
                    workSheet.Column(10).Width = 35;
                    workSheet.Column(11).Width = 25;
                    workSheet.Column(12).Width = 25;
                    workSheet.Column(13).Width = 24;
                    workSheet.Column(14).Width = 24;
                    workSheet.Column(15).Width = 27;
                    workSheet.Column(16).Width = 20;
                    workSheet.Column(17).Width = 35;
                    excelName = "ReporteEstadoEquipo_" + fechaini.ToString("dd_MM_yyyy") + "_al_" + fechafin.ToString("dd_MM_yyyy") + "_.xlsx";
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

        private void EnviarCorreos(string[] destinatarios, int codSala, EquipoEntidad equipoRam)
        {
            SalaEntidad sala = new SalaEntidad();
            Correo correo_enviar = new Correo();
            sala = _salaBl.SalaListaIdJson(codSala);
            string basepath = Request.Url.Scheme + "://" + ((Request.Url.Authority + Request.ApplicationPath).TrimEnd('/')) + "/";
            sala.RutaArchivoLogo = sala.RutaArchivoLogo != basepath + "Content/assets/images/no_image.jpg" ? "https://drive.google.com/uc?id=" + sala.RutaArchivoLogo : "";
            //Envio de Email a cliente y otros destinatarios
            //string srcLogoEmpresa = empresa.RutaArchivoLogo == string.Empty ? basepath + "Content/assets/images/no_image.jpg" : basepath + "Uploads/LogosEmpresas/" + empresa.RutaArchivoLogo;
           


            string htmlEnvio = $@"
                                     <div style='background: rgb(250,251,63);
                                                   background-image: linear-gradient(to top, #0c2c5c, #053a84, #0f48ac, #2955d6, #4960ff);width: 100%;padding:25px;'>
                                            <table style='border-radius:5px;display: table;margin:0 auto; background:#fff;padding:20px;'>
                                                <tbody>
                                                <tr>
                                                    <td colspan='7'>
                                                        <div style='border-radius:5px;text-align: center;font-family: Helvetica, Arial, sans-serif;  color: #fff; width:100%;background:#0C2C5C;padding:5px;'>
                                                            <h1>Estado de memoria Ram</h1>
                                                        </div>
                                                    </td>
                                                </tr>
                                               
                                                <tr >
                                                    <td colspan='7'>
                                                      
                                                             <div style='text-align: center;font-family: Helvetica, Arial, sans-serif;color: #000000;'>
                                                                <h3 style='margin-bottom: unset;   font-weight: bold;'>Sala</h3>
                                                                <h1 style='font-size:35px;margin:unset;font-weight: bold;'>{sala.Nombre}</h1>
                                                                <h3  style='margin-top:unset; font-weight: bold;'>{equipoRam.IpEquipo}</h3>

                                                            </div>
                                                    </td>
                                                </tr>
<tr>
                                                    <td colspan='7'>
                                                            <div style='font-family: Helvetica, Arial, sans-serif;color: #000000;'>
                                                                <h3 style='font-weight: lighter;'>Se detecto uso excesivo de la memoria RAM</h3>
                                                                
                                                            </div>
                                                    </td>
                                                </tr>
                                                 <tr colspan='7'>
                                                    <tr>
                                                        <th style=""background-color: #ccc; font-weight: bold; text-align: center; border: 1px solid black; padding: 5px;"">Ip equipo</th>
                                                        <th style=""background-color: #ccc; font-weight: bold; text-align: center; border: 1px solid black; padding: 5px;""> RAM usada</th>
                                                        <th style=""background-color: #ccc; font-weight: bold; text-align: center; border: 1px solid black; padding: 5px;"">RAM disponible</th>
                                                        <th style=""background-color: #ccc; font-weight: bold; text-align: center; border: 1px solid black; padding: 5px;"">RAM total</th> 
                                                        <th style=""background-color: #ccc; font-weight: bold; text-align: center; border: 1px solid black; padding: 5px;"">RAM %</th> 
                                                        <th style=""background-color: #ccc; font-weight: bold; text-align: center; border: 1px solid black; padding: 5px;""> CPU %</th>  
                                                        <th style="" background-color: #ccc; font-weight: bold; text-align: center; border: 1px solid black; padding: 5px;"">Procesos CPU</th> 

                                                    </tr>
                                                     <tr>
                                                        <td style='border: 1px solid black; padding: 5px;background-color: red;color:white;' >{equipoRam.IpEquipo}</td>
                                                        <td style='border: 1px solid black; padding: 5px;background-color: red;color:white;' >{equipoRam.MemoriaUsada}</td>
                                                        <td style='border: 1px solid black; padding: 5px;background-color: red;color:white;' >{equipoRam.MemoriaDisponible}</td>
                                                        <td style='border: 1px solid black; padding: 5px;background-color: red;color:white;' >{equipoRam.MemoriaTotal}</td>
                                                        <td style='border: 1px solid black; padding: 5px;background-color: red;color:white;' >{equipoRam.PorcentajeUsoRam}</td>
                                                        <td style='border: 1px solid black; padding: 5px;background-color: red;color:white;' >{equipoRam.PorcentajeCpu}</td>
                                                        <td style='border: 1px solid black; padding: 5px;background-color: red;color:white;' >{equipoRam.ProcesosCpu}</td>


                                                    </tr>
                                                </tr>
                                                <tr>
                                                    <td colspan='6'>
                                                            <div style='height:20px;'>
                                                                
                                                            </div>
                                                    </td>
                                                </tr>
                                                <tr>                                               
                                                    <tr>
                                                       <th style=""background-color: #ccc; font-weight: bold; text-align: center; border: 1px solid black; padding: 5px;"">
                                                       Color
                                                       </th>
                                                        <th style=""background-color: #ccc; font-weight: bold; text-align: center; border: 1px solid black; padding: 5px;"">
                                                       Descripción
                                                       </th>
                                                    </tr>
                                                       <td style='border: 1px solid black; padding: 5px;background-color:red;'></td>
                                                       <td style='border: 1px solid black; padding: 5px;'>Porcentaje de uso de la memoria RAM excede al 80%</td>
                                                    </tr>
                                                <tr>
                                                    <td colspan='7'>
                                                            <div style='text-align: right;font-family: Helvetica, Arial, sans-serif;color: #000000;'>
                                                                <h3>Fecha: {equipoRam.FechaRegistro}</h3>
                                                                
                                                            </div>
                                                    </td>
                                                </tr>
                                               
                                                </tbody>
                                            </table>
                                        </div>
                                    ";


            var listac = String.Join(",", destinatarios);
            Correo correo_destinatario = new Correo();
            correo_destinatario.EnviarCorreo(
            listac,
                     "ESTADO EQUIPO ",
                        htmlEnvio,
                     true
                     );
        }

        private void EnvioFirebase(string servidorKey, String[] DeviceToken, string msg, string title)
        {

            try
            {
                //var serverKey = "AAAAuNqaZi0:APA91bHevFUNteMjQNHBNIC6I2WvlvwLv7thv92a1WPKfiA-dxMiMZ3YaVsf2aZ2PFN5ytBM1JNQIWevFjB5mH3FgZeIrRWGjHKQcXnPvYwuujd8dD16CISrid5XE1-MjyaO01wQFvWQ";
                var serverKey = servidorKey;
                //DeviceToken = "eiXYDwYt7_w:APA91bHJLSV2CmV5BdkTHZagnvLTuSJ7PbpI-zuLb5vaBhY3bytyD0tenGA0L-aRjOgNZsugUS6uS6RB_wPkD7LGIeY5FlbNZI5XuSpmvhXQNguzio8hWLYEwi3hRamitqFWqE7p72VB";
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                //serverKey - Key from Firebase cloud messaging server  
                tRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
                //Sender Id - From firebase project setting  
                tRequest.Headers.Add(string.Format("Sender: id={0}", String.Join(",", DeviceToken)));
                tRequest.ContentType = "application/json";
                var payload = new
                {
                    //to = "fcd2PSqhBGc:APA91bHA8z7G22s0cEWxsuPmMPXmJptMJ2S5-dToF-BtZxyHpo50sskedHiZliox6CJy1vDRZk6zlNFHsiosUdX62D4mhqMuOG3GnI4O96xxH0CJvtcodR8PVsoUh7DGVQUVN-mu5BpW",
                    registration_ids = DeviceToken,
                    priority = "high",
                    content_available = true,
                    data = new
                    {
                        body = msg,
                        title = title,
                        badge = 1
                    },
                };
                string postbody = JsonConvert.SerializeObject(payload).ToString();
                Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    String sResponseFromServer = tReader.ReadToEnd();
                                    _log.escribir_logOK(PathLogAlertaBilleteros, "Respuesta Firebase:" + sResponseFromServer);
                                }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.escribir_logOK(PathLogAlertaBilleteros, "Respuesta Firebase" + ex.Message);
            }
        }
    }
}