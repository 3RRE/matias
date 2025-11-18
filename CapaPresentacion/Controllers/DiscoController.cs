
using CapaEntidad;
using CapaEntidad.Alertas;
using CapaEntidad.Discos;
using CapaNegocio;
using CapaNegocio.Discos;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Windows.Media.Media3D;
using CapaNegocio.Disco;
using CapaEntidad.Disco;
using System.Configuration;
using CapaPresentacion.Utilitarios;
using CapaNegocio.Alertas;
using System.Net;
using System.Text;
using CapaDatos.Disco;
using CapaEntidad.ContadoresNegativos;
using System.Text.RegularExpressions;
using OfficeOpenXml.Table;
using S3k.Utilitario;
using CapaEntidad.Response;
using System.Threading.Tasks;

namespace CapaPresentacion.Controllers {
    [seguridad]
    public class DiscoController : Controller {
        private readonly DiscoBL _discoBl = new DiscoBL();
        private SalaBL _salaBl = new SalaBL();
        private SEG_CargoBL _cargoBl = new SEG_CargoBL();
        private readonly DiscoCargoConfigBL _discoConfigBL = new DiscoCargoConfigBL();
        private string FirebaseKey = ConfigurationManager.AppSettings["firebaseServiceKey"];
        private readonly LogTransac _log = new LogTransac();
        private string PathLogAlertaBilleteros = ConfigurationManager.AppSettings["PathLogAlertaBilleteros"];
        private ALT_AlertaSalaBL alertaSalaBL = new ALT_AlertaSalaBL();
        private AlertaDiscoBL _alertaDiscoBL = new AlertaDiscoBL();
        private readonly ServiceHelper _serviceHelper = new ServiceHelper();

        public ActionResult ConfiguracionAlertaDisco() {
            return View("~/Views/Disco/ConfiguracionAlertaDisco.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult CargoSalaListarJson() {
            var errormensaje = "";
            var lista = new List<DiscoCargoConfigEntidad>();
            var listaCargo = new List<SEG_CargoEntidad>();
            var listaSala = new List<SalaEntidad>();
            List<object> listaFinal = new List<object>();
            try {
                lista = _discoConfigBL.ListadoDiscoCargoConfig();
                listaCargo = _cargoBl.CargoListarJson();
                listaSala = _salaBl.ListadoSala();

                foreach(var registro in listaSala) {
                    List<object> cargos = new List<object>();
                    foreach(var registrocargo in listaCargo) {
                        var reg = lista.Where(x => x.cargo_id == registrocargo.CargoID && x.sala_id == registro.CodSala).FirstOrDefault();
                        if(reg == null) {
                            registrocargo.alt_id = 0;
                        } else {
                            registrocargo.alt_id = reg.idDiscoConfigCargo;
                        }

                        cargos.Add(new {
                            registrocargo.CargoID,
                            registrocargo.Descripcion,
                            registrocargo.Estado,
                            registrocargo.alt_id
                        });
                    }
                    //var registr = listaalerta.Where(x => x.cargo_id == registro.CargoID && x.sala_id==registro.).FirstOrDefault();
                    //registro.alt_id = registr.alt_id;
                    listaFinal.Add(new {
                        registro.CodSala,
                        registro.Nombre,
                        registro.NombreCorto,
                        cargos
                    });
                }



            } catch(Exception exp) {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = listaFinal.ToList(), mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult DiscoCargoGuardarJson(DiscoCargoConfigEntidad discoCargo) {
            var errormensaje = "";
            Int64 respuestaConsulta = 0;
            bool respuesta = false;
            try {
                discoCargo.fechaRegistro=DateTime.Now;
                //alertacargo.usuario_id = Convert.ToInt32(Session["UsuarioID"]);
                respuestaConsulta = _discoConfigBL.DiscoCargoConfInsertarJson(discoCargo);

                if(respuestaConsulta > 0) {
                    respuesta = true;
                    errormensaje = "Registro Guardado Correctamente";
                } else {
                    errormensaje = "error al Guardar Registro , LLame Administrador";
                    respuesta = false;
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, id = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult QuitarDiscoCargoJson(int alt_id) {
            var errormensaje = "";
            bool respuesta = false;
            try {
                respuesta = _discoConfigBL.DiscoCargoConfEliminarJson(alt_id);
                if(respuesta) {
                    respuesta = true;
                    errormensaje = "Se quitó Alerta Correctamente";
                } else {
                    errormensaje = "error al Quitar Alerta , LLame Administrador";
                    respuesta = false;
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje });
        }







        public ActionResult ListadoDiscos() {
            return View("~/Views/disco/ListadoDiscos.cshtml");
        }

       

        [HttpPost]
        public JsonResult ListadoDisco(string codSala, DateTime fechaIni, DateTime fechaFin) {

            var errormensaje = "";
            var lista = new List<DiscoEntidad>();
            try {
                lista = _discoBl.ListadoDisco(Convert.ToInt32(codSala), fechaIni, fechaFin).OrderByDescending(x => x.fechaRegistro).ToList();
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";    
            }
            return Json(new { data = lista, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ListadoDiscosSalas(List<int> codSalas, DateTime fechaIni, DateTime fechaFin) {
           
            List<string> codSalasStrings = codSalas.Select(c => c.ToString()).ToList();
            string codSalaString = string.Join(",", codSalasStrings);

            var errormensaje = "";
            var lista = new List<DiscoEntidad>();
            try {
                lista = _discoBl.ListadoDiscoAll(codSalaString, fechaIni, fechaFin).OrderByDescending(x => x.fechaRegistro).ToList();
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }


        [seguridad(false)]
        [HttpPost]
        public JsonResult AgregarDiscosSala(DiscoEntidad[] nuevoDisco) {
            var errormensaje = "";
            Int64 alt_id = 0;
            bool respuesta = false;
               var titulo = "¡Alertas Discos!";
            var servidorKey = FirebaseKey;
            DateTime fecha = DateTime.Now;
            int sala=0;


            int CodSalaNuevoDisco = nuevoDisco[0].codSala;
            int condicionDisco = ManejoNulos.ManageNullInteger(nuevoDisco[0].condicionDisco);
            //string CodSalaNuevoDisco = nuevoDisco[0].codSala;
            DateTime FechaRegistroNuevoDisco = nuevoDisco[0].fechaRegistro;
            DiscoEntidad ultimoRegistro = _discoBl.ObtenerUltimoRegistro(CodSalaNuevoDisco);

            int lista;
           List<AlertaDiscoEntidad> devices = new List<AlertaDiscoEntidad>();
            List<string> correos = new List<string>();

            SalaEntidad datosSala = new SalaEntidad();
            TimeSpan diferencia = FechaRegistroNuevoDisco.Subtract(ultimoRegistro.fechaRegistro);

            if (diferencia.TotalMinutes >5) {
                try
                {
                    foreach (DiscoEntidad disk in nuevoDisco)
                    {
                        lista = _discoBl.AgregarDiscos(disk);
                        sala = disk.codSala;
                    }

                    datosSala = _salaBl.ListadoSala().Where(x => x.CodSala == sala).First();

                    //_log.escribir_logOK(PathLogAlertaBilleteros, "Sala:" + sala.Nombre + " CodSala:" + sala.CodSala);
                    devices = _alertaDiscoBL.AlertaDisco_xdevicesListado(sala);
                    correos = _alertaDiscoBL.AlertaDiscosCorreosListado(sala);

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
                        errormensaje = "Se detecto bajo espacio en uno de los discos de la sala " + datosSala.Nombre;
                    }

                    respuesta = true;
                    string[] dispositivos = devices.Select(x => x.id).ToArray();

                    if (dispositivos.Length > 0)
                    {
                        EnvioFirebase(servidorKey, dispositivos, errormensaje, titulo);

                    }
                    if (correos.Count > 0)
                    {
                        EnviarCorreos(correos.ToArray(), sala, nuevoDisco, condicionDisco);

                    }
                    devices.Clear();


                }
                catch (Exception exp)
                {
                    errormensaje = exp.Message + "Error,Llame Administrador";
                }
            }
           
            return Json(new { data = nuevoDisco, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }


       
        [HttpPost]
        public ActionResult ConsultaDiscosActivos(string urlPublica, string urlPrivada) {
            var client = new System.Net.WebClient();
            var response = "";
            var jsonResponse = new List<DiscoEntidad>();
            object oEnvio = new object();
            try {
                oEnvio = new {
                    ipPrivada = urlPrivada
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                response = client.UploadString(urlPublica, "POST", inputJson);
                jsonResponse = JsonConvert.DeserializeObject<List<DiscoEntidad>>(response);
            } catch(Exception ex) {
            }
            return Json(jsonResponse);
        }



        [HttpPost]
        public ActionResult ReporteDiscosDescargarExcelJson(int codsala, DateTime fechaini, DateTime fechafin) {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<DiscoEntidad> lista = new List<DiscoEntidad>();
            List<ALT_AlertaSalaEntidad> tickets = new List<ALT_AlertaSalaEntidad>();
            string nombreSala = _salaBl.ListadoSala().First(x => x.CodSala == codsala).Nombre;

            Color alerta = ColorTranslator.FromHtml("#D94E2F");
            Color normal = ColorTranslator.FromHtml("#32CD5B");
            //string nombreSala=_salaBl.ObtenerNombreSala(codsala);
            try {

                //lista = alertaSalaBL.ALT_AlertaSala_xsala_idListado(strElementos, fechaini, fechafin);
                lista = _discoBl.ListadoDisco(codsala, fechaini, fechafin);
                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Particiones");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[2, 2].Value = "SALA : " + nombreSala;

                    workSheet.Cells[3, 2].Value = "Nombre";
                    workSheet.Cells[3, 3].Value = "Seudonimo";
                    workSheet.Cells[3, 4].Value = "Sistema Disco";
                    workSheet.Cells[3, 5].Value = "Tipo Disco";
                    workSheet.Cells[3, 6].Value = "Espacio Libre";
                    workSheet.Cells[3, 7].Value = "Espacio en uso";
                    workSheet.Cells[3, 8].Value = "Espacio Total";
                    workSheet.Cells[3, 9].Value = "Fecha de registro";

                    //Body of table  
                    //  
                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach(var registro in lista) {
                        string nuevaCadena = registro.capacidadLibre.Substring(0, registro.capacidadLibre.Length - 2);

                        workSheet.Cells[recordIndex, 2].Value = registro.nombreDisco;
                        workSheet.Cells[recordIndex, 3].Value = registro.seudonimoDisco;
                        workSheet.Cells[recordIndex, 4].Value = registro.sistemaDisco;
                        workSheet.Cells[recordIndex, 5].Value = registro.tipoDisco;

                        workSheet.Cells[recordIndex, 6].Value = registro.capacidadLibre;
                        if(Convert.ToInt32(nuevaCadena) <= 5) {
                            workSheet.Cells[recordIndex, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheet.Cells[recordIndex, 6].Style.Fill.BackgroundColor.SetColor(alerta);
                        } else {
                            workSheet.Cells[recordIndex, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheet.Cells[recordIndex, 6].Style.Fill.BackgroundColor.SetColor(normal);
                        }

                        workSheet.Cells[recordIndex, 7].Value = registro.capacidadEnUso;
                        workSheet.Cells[recordIndex, 8].Value = registro.capacidadTotal;
                        workSheet.Cells[recordIndex, 9].Value = registro.fechaRegistro.ToString("dd-MM-yyyy hh:mm:ss tt");
                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    // Supongamos que tienes datos en el rango B3:E12
                    int celdaInicial = 3;
                    int celdaFinal = celdaInicial + lista.Count;
                    var dataRange = workSheet.Cells[$"B{celdaInicial}:I{celdaFinal}"];

                    // Agregar una tabla con estilo al rango de datos
                    var table = workSheet.Tables.Add(dataRange, "TablaDisco");
                    table.TableStyle = TableStyles.Light6; // Puedes cambiar el estilo según tus preferencias
                    table.ShowHeader = true; // Mostrar el encabezado de la tabla
                    table.ShowFilter = true;

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
                    //workSheet.Cells[3, 2, filaultima, 10].AutoFilter = true;

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
                    excelName = "ReporteDiscos_" + fechaini.ToString("dd_MM_yyyy") + "_al_" + fechafin.ToString("dd_MM_yyyy") + "_.xlsx";
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


        private void EnviarCorreos(string[] destinatarios, int codSala, DiscoEntidad[] discoEntidad, int condicionDisco)
        {
            SalaEntidad sala = new SalaEntidad();
            Correo correo_enviar = new Correo();
            sala = _salaBl.SalaListaIdJson(codSala);
            string basepath = Request.Url.Scheme + "://" + ((Request.Url.Authority + Request.ApplicationPath).TrimEnd('/')) + "/";
            sala.RutaArchivoLogo = sala.RutaArchivoLogo != basepath + "Content/assets/images/no_image.jpg" ? "https://drive.google.com/uc?id=" + sala.RutaArchivoLogo : "";
            //Envio de Email a cliente y otros destinatarios
            //string srcLogoEmpresa = empresa.RutaArchivoLogo == string.Empty ? basepath + "Content/assets/images/no_image.jpg" : basepath + "Uploads/LogosEmpresas/" + empresa.RutaArchivoLogo;
            StringBuilder html = new StringBuilder();
            string color = "";
            foreach (var elemento in discoEntidad)
            {



                html.Append("<tr> ");
                string capacidadLibreNumerico = Regex.Replace(elemento.capacidadLibre, "[^0-9]", "");
                if (int.TryParse(capacidadLibreNumerico, out int capacidadLibre) && capacidadLibre < condicionDisco)
                {
                    html.Append("<td style='border: 1px solid black; padding: 5px; background-color: red;color:white' >");
                    html.Append(elemento.ipServidor);
                    html.Append("</td>");
                    html.Append("<td style='border: 1px solid black; padding: 5px; background-color: red;color:white' >");
                    html.Append(elemento.nombreDisco);
                    html.Append("</td>");
                    html.Append("<td style='border: 1px solid black; padding: 5px; background-color: red;color:white' >");
                    html.Append(elemento.capacidadEnUso);
                    html.Append("</td>");
                    html.Append("<td style='border: 1px solid black; padding: 5px; background-color: red;color:white' >");
                    html.Append(elemento.capacidadLibre);
                    html.Append("</td>");
                    html.Append("<td style='border: 1px solid black; padding: 5px; background-color: red;color:white' >");
                    html.Append(elemento.capacidadTotal);
                    html.Append("</td>");
                }
                else
                {
                    html.Append("<td style='border: 1px solid black; padding: 5px;'>");
                    html.Append(elemento.ipServidor);
                    html.Append("</td>");
                    html.Append("<td style='border: 1px solid black; padding: 5px;' >");
                    html.Append(elemento.nombreDisco);
                    html.Append("</td>");
                    html.Append("<td style='border: 1px solid black; padding: 5px;' >");
                    html.Append(elemento.capacidadEnUso);
                    html.Append("</td>");
                    html.Append("<td style='border: 1px solid black; padding: 5px;' >");
                    html.Append(elemento.capacidadLibre);
                    html.Append("</td>");
                    html.Append("<td style='border: 1px solid black; padding: 5px;' >");
                    html.Append(elemento.capacidadTotal);
                    html.Append("</td>");
                }
               

                html.Append("</tr>");
            }


            string htmlEnvio = $@"
                                     <div style='background: rgb(250,251,63);
                                                   background-image: linear-gradient(to top, #0c2c5c, #053a84, #0f48ac, #2955d6, #4960ff);width: 100%;padding:25px;'>
                                            <table style='border-radius:5px; display: table;margin:0 auto; background:#fff;padding:20px;'>
                                                <tbody style='width:100%'>
                                                <tr>
                                                    <td colspan='6'>
                                                        <div style='border-radius:5px;text-align: center;font-family: Helvetica, Arial, sans-serif;  color: #fff; width:100%;background:#0C2C5C;padding:5px;'>
                                                            <h1>Estado de discos</h1>
                                                        </div>
                                                    </td>
                                                </tr>
                                               
                                                <tr >
                                                    <td colspan='6'>
                                                      
                                                             <div style='text-align: center;font-family: Helvetica, Arial, sans-serif;color: #000000;'>
                                                                <h3  style='margin-bottom: unset;   font-weight: bold;'>Sala</h3>
                                                                <h1
                                                                style='font-size:35px;margin:unset;font-weight: bold;'>{sala.Nombre}</h1>
                                                                <h3  style='margin-top:unset; font-weight: bold;'>{discoEntidad[0].ipServidor}</h3>

                                                            </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan='6'>
                                                            <div style='font-family: Helvetica, Arial, sans-serif;color: #000000;'>
                                                                <h3 style='font-weight: lighter;'>Se detecto poca capacidad libre en uno de los discos</h3>
                                                                
                                                            </div>
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <tr>
                                                        <th style=""background-color: #ccc; font-weight: bold; text-align: center; border: 1px solid black; padding: 5px;"">Ip del servidor</th>
                                                        <th style=""background-color: #ccc; font-weight: bold; text-align: center; border: 1px solid black; padding: 5px;"">Nombre del disco</th>
                                                        <th style=""background-color: #ccc; font-weight: bold; text-align: center; border: 1px solid black; padding: 5px;"">Capacidad en uso</th>                                                    
                                                        <th style=""background-color: #ccc; font-weight: bold; text-align: center; border: 1px solid black; padding: 5px;"">Capacidad Libre</th>                                                       
                                                        <th style=""background-color: #ccc; font-weight: bold; text-align: center; border: 1px solid black; padding: 5px;"">Capacidad Total</th>                                                        

                                                    </tr>
                                                     {html}
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
                                                        <td style = 'border: 1px solid black; padding: 5px;background-color:red;' ></td>
                                                        <td style = 'border: 1px solid black; padding: 5px;' > Capacidad libre menor a {condicionDisco}GB </td>
                                               </ tr >
                                                    
                                                <tr>
                                                    <td colspan='6'>
                                                            <div style='text-align: right;font-family: Helvetica, Arial, sans-serif;color: #000000;'>
                                                                <h3>Fecha: {discoEntidad[0].fechaRegistro}</h3>
                                                                
                                                            </div>
                                                    </td>
                                                </tr>
                                               
                                                </tbody>
                                            </table>
                                        </div>
                                    ";

            /*leyenda correo disco*/
           

            var listac = String.Join(",", destinatarios);
            Correo correo_destinatario = new Correo();
            correo_destinatario.EnviarCorreo(
            listac,
                     "Alerta Espacio de Discos - "+sala.Nombre,
                        htmlEnvio,
                     true
                     );
        }


        public void EnvioFirebase(string servidorKey, String[] DeviceToken, string msg, string title) {

            try {
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
                var payload = new {
                    //to = "fcd2PSqhBGc:APA91bHA8z7G22s0cEWxsuPmMPXmJptMJ2S5-dToF-BtZxyHpo50sskedHiZliox6CJy1vDRZk6zlNFHsiosUdX62D4mhqMuOG3GnI4O96xxH0CJvtcodR8PVsoUh7DGVQUVN-mu5BpW",
                    registration_ids = DeviceToken,
                    priority = "high",
                    content_available = true,
                    data = new {
                        body = msg,
                        title = title,
                        badge = 1
                    },
                };
                string postbody = JsonConvert.SerializeObject(payload).ToString();
                Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
                tRequest.ContentLength = byteArray.Length;
                using(Stream dataStream = tRequest.GetRequestStream()) {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using(WebResponse tResponse = tRequest.GetResponse()) {
                        using(Stream dataStreamResponse = tResponse.GetResponseStream()) {
                            if(dataStreamResponse != null) using(StreamReader tReader = new StreamReader(dataStreamResponse)) {
                                    String sResponseFromServer = tReader.ReadToEnd();
                                    _log.escribir_logOK(PathLogAlertaBilleteros, "Respuesta Firebase:" + sResponseFromServer);
                                }
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                _log.escribir_logOK(PathLogAlertaBilleteros, "Respuesta Firebase" + ex.Message);
            }
        }


        public ActionResult ListadoEspacioDiscoBDs() {
            return View("~/Views/disco/ListadoEspacioDiscoBDs.cshtml");
        }

        public ActionResult ListadoEspacioDiscoBDsAzure() {
            return View("~/Views/disco/ListadoEspacioDiscoBDsAzure.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> ConsultaEspacioDiscoBDs(int salaId) {
            bool success = false;
            bool inVpn = false;
            string message = "No se encontraron registros";

            if(salaId <= 0) {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione una sala"
                });
            }

            List<EspacioDiscoBD> data = new List<EspacioDiscoBD>();

            var _salaBL = new SalaBL();

            try {
                SalaEntidad sala = _salaBL.ObtenerSalaPorCodigo(salaId);
                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, new List<string>());

                if(!tcpConnection.IsOpen) {
                    return Json(new
                    {
                        success,
                        message = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                    });
                }

                string servicePath = "servicio/EspacioLogsConsulta";
                string content = string.Empty;
                string requestUri = string.Empty;

                if(tcpConnection.IsVpn) {
                    inVpn = true;

                    object arguments = new
                    {
                        ipPrivada = $"{sala.IpPrivada}:{sala.PuertoServicioWebOnline}/{servicePath}",
                        salaId
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{tcpConnection.Url}/servicio/VPNGenericoPost";
                } else {
                    object arguments = new
                    {
                        salaId
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{sala.UrlProgresivo}/{servicePath}";
                }

                ResultEntidad result = await _serviceHelper.PostAsync(requestUri, content);

                data = JsonConvert.DeserializeObject<List<EspacioDiscoBD>>(result.data);

                if(data.Count > 0) {
                    
                    success = true;
                    message = "Registros obtenidos correctamente";
                }


            } catch(Exception exception) {
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

        [HttpPost]
        public async Task<ActionResult> LimpiarLogsConsulta(int salaId, string nombreBD, string nombreLog) {
            bool success = false;
            bool inVpn = false;
            string message = "No se logro limpiar el log";

            if(salaId <= 0) {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione una sala"
                });
            }

            List<dynamic> data = new List<dynamic>();

            var _salaBL = new SalaBL();

            try {
                SalaEntidad sala = _salaBL.ObtenerSalaPorCodigo(salaId);
                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, new List<string>());

                if(!tcpConnection.IsOpen) {
                    return Json(new
                    {
                        success,
                        message = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                    });
                }

                string servicePath = "servicio/LimpiarLogsConsulta";
                string content = string.Empty;
                string requestUri = string.Empty;

                if(tcpConnection.IsVpn) {
                    inVpn = true;

                    object arguments = new
                    {
                        ipPrivada = $"{sala.IpPrivada}:{sala.PuertoServicioWebOnline}/{servicePath}",
                        salaId,
                        nombreBD, 
                        nombreLog
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{tcpConnection.Url}/servicio/VPNGenericoPost";
                } else {
                    object arguments = new
                    {
                        salaId,
                        nombreBD,
                        nombreLog
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{sala.UrlProgresivo}/{servicePath}";
                }

                ResultEntidad result = await _serviceHelper.PostAsync(requestUri, content);

                data = JsonConvert.DeserializeObject<List<dynamic>>(result.data);

                success = true;
                message = "Log limpiado correctamente";


            } catch(Exception exception) {
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

        [HttpPost]
        public JsonResult ListadoBDsAzure() {

            bool respuesta = false;
            var errormensaje = "Listado correctamente";
            var lista = new List<EspacioDiscoBD>();
            try {
                lista = _discoBl.ListadoBDsAzure();
                respuesta = true;
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta, data = lista, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LimpiarLogBDAzure(string nombreBD, string nombreLog) {

            bool respuesta = false;
            string errormensaje = "";
            try {
                respuesta = _discoBl.LimpiarLogBDAzure(nombreBD,nombreLog);
                errormensaje = "Se limpio el log de la bd "+nombreBD+" correctamente.";
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

    }
}
