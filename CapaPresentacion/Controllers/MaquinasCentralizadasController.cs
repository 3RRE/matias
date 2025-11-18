using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static CapaPresentacion.Controllers.Revisiones.RevisionesController;
using System.Configuration;
using CapaEntidad;
using System.Web.Script.Serialization;
using CapaDatos.MaquinasInoperativas;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.IO;
using System.Drawing;
using System.Web.WebPages;
using Microsoft.Win32;
using System.Text;

namespace CapaPresentacion.Controllers
{
    public class MaquinasCentralizadasController : Controller
    {
        public ActionResult MaquinasCentralizadasVista() {
            return View("~/Views/MaquinasCentralizadas/ReporteMaquinasCentralizadasVista.cshtml");
        }

        public ActionResult ListadoMaquinasCentralizadasJson(int codSala,DateTime fechaIni, DateTime fechaFin) {

            List<MaquinasCentralizadasEntidad> data = new List<MaquinasCentralizadasEntidad>();
            bool respuesta = false;
            string UrlSistemaReclutamiento = string.Empty;


            string url = UrlSistemaReclutamiento + "AnexoG/ListadoMaquinasActivasReportexSala?CodSala=" + codSala;
            string observacionReporte = "Maquinas obtenidas correctamente.";

            try {
                TimeSpan diferencia = fechaFin - fechaIni;
                if(diferencia.Days > 60) {
                    return Json(new { respuesta, mensaje = "La diferencia de días debe ser menor a 60" });

                } 

            } catch(Exception ex) {
                return Json(new { respuesta, mensaje = ex.Message.ToString() });
            }

            try {
                //buscar data de maquina
                string mensaje = "Registros obtenidos correctamente.";
                string uri = string.Empty;
                string token = GetToken();
                try {
                    uri = ConfigurationManager.AppSettings["AdministrativoUri"].ToString();
                } catch {
                    return Json(new { mensaje = "No existe la key AdministrativoUri", respuesta }, JsonRequestBehavior.AllowGet);
                }
                HttpClient client = new HttpClient {
                    BaseAddress = new Uri(uri)
                };
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);

                var obj = new
                {
                    Sala = codSala,
                    FechaIni = fechaIni,
                    FechaFin = fechaFin,
                };

                string json = JsonConvert.SerializeObject(obj);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync("api/reporte/Reporte8152Completo", httpContent).Result;
                if(response.IsSuccessStatusCode) {
                    var respJson = response.Content.ReadAsStringAsync().Result;
                    var resp = JsonConvert.DeserializeObject<ResponseMaquinasCentralizadasEntidad>(respJson);
                    if(resp.respuesta) {

                        data = resp.data;
                        respuesta = resp.respuesta;
                    } else {
                        observacionReporte = "No se encontraron datos";
                    }
                }
            } catch(Exception e) {
                data = new List<MaquinasCentralizadasEntidad>();
                observacionReporte = "ERROR - No se pudo conectar - " + e.ToString();
            }

            //SERVER
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new
            {
                mensaje = observacionReporte,
                data,
                respuesta
            };
            var result = new ContentResult {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };

            return result;
        }


        [HttpPost]
        public ActionResult ListadoMaquinasCentralizadasExcelJson(List<MaquinasCentralizadasEntidad> lista) {


            string fecha = DateTime.Now.ToString("dd_MM_yyyy");
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            var strElementos = String.Empty;
            var strElementos_ = String.Empty;
            var salasSeleccionadas = String.Empty;
            try {

                List<MaquinasCentralizadasEntidad> data = new List<MaquinasCentralizadasEntidad>();
                data = lista;
                //string UrlSistemaReclutamiento = string.Empty;

                //try {
                //    UrlSistemaReclutamiento = ConfigurationManager.AppSettings["UriSistemaReclutamiento"].ToString();
                //    string url = UrlSistemaReclutamiento + "AnexoG/ListadoMaquinasActivasReportexSala?CodSala=" + codSala;
                //    using(var client = new HttpClient()) {
                //        using(var response = client.GetAsync(url).Result) {
                //            if(response.IsSuccessStatusCode) {
                //                var json = response.Content.ReadAsStringAsync().Result;
                //                ResponseMaquinasCentralizadasEntidad jsonObj = JsonConvert.DeserializeObject<ResponseMaquinasCentralizadasEntidad>(json);
                //                respuesta = jsonObj.respuesta;
                //                if(respuesta) {
                //                    data = jsonObj.data;
                //                    if(nombresala.IsEmpty() && data.Count>0) {
                //                        nombresala = data.First().sala;
                //                    }
                //                }
                //            } 
                //        }
                //    }
                //} catch(Exception e) {
                //    data = new List<MaquinasCentralizadasEntidad>();
                //}

                if(data.Count > 0) {

                    var nombresala = lista.First().sala;
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add(nombresala);
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;

                    workSheet.Cells[3, 2].Value = "SALA";
                    workSheet.Cells[3, 3].Value = "TIPO MAQUINA";
                    workSheet.Cells[3, 4].Value = "CODIGO";
                    workSheet.Cells[3, 5].Value = "SERIE";
                    workSheet.Cells[3, 6].Value = "ZONA";
                    workSheet.Cells[3, 7].Value = "ISLA";   
                    workSheet.Cells[3, 8].Value = "POSICION";
                    workSheet.Cells[3, 9].Value = "MARCA";
                    workSheet.Cells[3, 10].Value = "MODELO COMERCIAL";
                    workSheet.Cells[3, 11].Value = "CODIGO DE MODELO";
                    workSheet.Cells[3, 12].Value = "JUEGO";
                    workSheet.Cells[3, 13].Value = "PROGRESIVO";
                    workSheet.Cells[3, 14].Value = "PROPIETARIO";
                    workSheet.Cells[3, 15].Value = "TIPO DE CONTRATO";
                    workSheet.Cells[3, 16].Value = "MONEDA";
                    workSheet.Cells[3, 17].Value = "MEDIO JUEGO";
                    workSheet.Cells[3, 18].Value = "TOKEN";
                    workSheet.Cells[3, 19].Value = "DIAS TRABAJADOS";
                    workSheet.Cells[3, 20].Value = "COIN IN";
                    workSheet.Cells[3, 21].Value = "A COIN IN";
                    workSheet.Cells[3, 22].Value = "WIN";
                    workSheet.Cells[3, 23].Value = "MEDIA";
                    workSheet.Cells[3, 24].Value = "HOLD";
                    workSheet.Cells[3, 25].Value = "AVBET X GAME";
                    workSheet.Cells[3, 26].Value = "GAMES PLAYED";
                    workSheet.Cells[3, 27].Value = "TIEMPO JUEGO";
                    workSheet.Cells[3, 28].Value = "MISTERY";
                    workSheet.Cells[3, 29].Value = "TIPO PROGRESIVO";
                    workSheet.Cells[3, 30].Value = "POZOS";
                    workSheet.Cells[3, 31].Value = "PORCENTAJE DEVOLUCION";
                    workSheet.Cells[3, 32].Value = "RETORNO TEORICO";
                    workSheet.Cells[3, 33].Value = "INCREMENTO PROGRESIVO";
                    workSheet.Cells[3, 34].Value = "ESTADO";

                    int recordIndex = 4;
                    int total = data.Count;
                    foreach(var registro in data) {
                        workSheet.Cells[recordIndex, 2].Value = registro.sala;
                        workSheet.Cells[recordIndex, 3].Value = registro.tipo_maquina.ToUpper();
                        workSheet.Cells[recordIndex, 4].Value = registro.codigo;
                        workSheet.Cells[recordIndex, 5].Value = registro.serie;
                        workSheet.Cells[recordIndex, 6].Value = registro.zona.ToUpper();
                        workSheet.Cells[recordIndex, 7].Value = registro.isla;
                        workSheet.Cells[recordIndex, 8].Value = registro.posicion;
                        workSheet.Cells[recordIndex, 9].Value = registro.marca.ToUpper();
                        workSheet.Cells[recordIndex, 10].Value = registro.modelo_comercial.ToUpper();
                        workSheet.Cells[recordIndex, 11].Value = registro.codigo_modelo.ToUpper();
                        workSheet.Cells[recordIndex, 12].Value = registro.juego.ToUpper();
                        workSheet.Cells[recordIndex, 13].Value = registro.progresivo.ToUpper();
                        workSheet.Cells[recordIndex, 14].Value = registro.propietario.ToUpper();
                        workSheet.Cells[recordIndex, 15].Value = registro.tipo_contrato;
                        workSheet.Cells[recordIndex, 16].Value = registro.moneda;
                        workSheet.Cells[recordIndex, 17].Value = registro.medio_juego;
                        workSheet.Cells[recordIndex, 18].Value = Math.Round(registro.token,2);
                        workSheet.Cells[recordIndex, 19].Value = Math.Round(registro.dias_trabajados,0);
                        workSheet.Cells[recordIndex, 20].Value = Math.Round(registro.coin_in, 2);
                        workSheet.Cells[recordIndex, 21].Value = Math.Round(registro.a_coin_in, 2);
                        workSheet.Cells[recordIndex, 22].Value = Math.Round(registro.win, 2);
                        workSheet.Cells[recordIndex, 23].Value = Math.Round(registro.media, 2);
                        workSheet.Cells[recordIndex, 24].Value = Math.Round(registro.hold, 2);
                        workSheet.Cells[recordIndex, 25].Value = Math.Round(registro.avbet_x_game, 2);
                        workSheet.Cells[recordIndex, 26].Value = Math.Round(registro.games_played, 0);
                        workSheet.Cells[recordIndex, 27].Value = Math.Round(registro.tiempo_juego, 2);
                        workSheet.Cells[recordIndex, 28].Value = registro.mistery;
                        workSheet.Cells[recordIndex, 29].Value = registro.tipo_progresivo;
                        workSheet.Cells[recordIndex, 30].Value = registro.pozos;
                        workSheet.Cells[recordIndex, 31].Value = Math.Round(registro.porcentaje_teorico, 2)+"%";
                        workSheet.Cells[recordIndex, 32].Value = registro.rtp_retorno_teorico + "%";
                        workSheet.Cells[recordIndex, 33].Value = registro.incremento_progresivo + "%";
                        workSheet.Cells[recordIndex, 34].Value = "ACTIVO";
                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:AH3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:AH3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:AH3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:AH3"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B3:AH3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:AH3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:AH3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:AH3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B3:AH3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:AH3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:AH3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:AH3"].Style.Border.Bottom.Color.SetColor(colborder);

                    int filasagregadas = 3;
                    total = filasagregadas + total;

                    workSheet.Cells["B4:AH" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    int filaFooter = total + 1;
                    workSheet.Cells["B" + filaFooter + ":AH" + filaFooter].Merge = true;
                    workSheet.Cells["B" + filaFooter + ":AH" + filaFooter].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter + ":AH" + filaFooter].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter + ":AH" + filaFooter].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter + ":AH" + filaFooter].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter + ":AH" + filaFooter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells["B" + filaFooter + ":AH" + filaFooter].Style.Font.Size = 14;
                    workSheet.Cells[filaFooter, 2].Value = "Total : " + (total - filasagregadas) + " Registros";
                    workSheet.Cells[filaFooter, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["B4:AH" + total].Style.WrapText = true;

                    int filaultima = total;
                    workSheet.Cells[3, 2, filaultima, 34].AutoFilter = true;

                    workSheet.Column(2).Width = 30;
                    workSheet.Column(3).Width = 30;
                    workSheet.Column(4).Width = 30;
                    workSheet.Column(5).Width = 30;
                    workSheet.Column(6).Width = 30;
                    workSheet.Column(7).Width = 30;
                    workSheet.Column(8).Width = 30;
                    workSheet.Column(9).Width = 30;
                    workSheet.Column(10).Width = 30;
                    workSheet.Column(11).Width = 30;
                    workSheet.Column(12).Width = 40;
                    workSheet.Column(13).Width = 40;
                    workSheet.Column(14).Width = 40;
                    workSheet.Column(15).Width = 30;
                    workSheet.Column(16).Width = 30;
                    workSheet.Column(17).Width = 30;
                    workSheet.Column(18).Width = 30;
                    workSheet.Column(19).Width = 30;
                    workSheet.Column(20).Width = 30;
                    workSheet.Column(21).Width = 30;
                    workSheet.Column(22).Width = 30;
                    workSheet.Column(23).Width = 30;
                    workSheet.Column(24).Width = 30;
                    workSheet.Column(25).Width = 30;
                    workSheet.Column(26).Width = 30;
                    workSheet.Column(27).Width = 30;
                    workSheet.Column(28).Width = 30;
                    workSheet.Column(29).Width = 30;
                    workSheet.Column(30).Width = 30;
                    workSheet.Column(31).Width = 30;
                    workSheet.Column(32).Width = 30;
                    workSheet.Column(33).Width = 30;
                    workSheet.Column(34).Width = 30;
                    excelName = "MaquinasCentralizadas_"+nombresala+"_" + fecha + ".xlsx";
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());

                    mensaje = "Descargando Archivo";
                    respuesta = true;
                } else {
                    mensaje = "No se Pudo generar Archivo";
                }

            } catch(Exception exp) {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });

        }



        [seguridad(false)]
        public static string GetToken() {
            string administrativoUsername = ConfigurationManager.AppSettings["AdministrativoUsername"];
            string administrativoPassword = ConfigurationManager.AppSettings["AdministrativoPassword"];
            string key = administrativoUsername + ":" + administrativoPassword;
            return Encode(key);
        }
        [seguridad(false)]
        public static string Encode(string plainText) {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        [seguridad(false)]
        public static string Decode(string base64EncodedData) {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}