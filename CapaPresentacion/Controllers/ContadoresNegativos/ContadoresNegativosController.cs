using CapaEntidad.Alertas;
using CapaEntidad.ContadoresNegativos;
using CapaNegocio;
using CapaNegocio.ContadoresNegativos;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad.Disco;
using CapaEntidad.Discos;
using CapaEntidad;
using CapaNegocio.Disco;
using CapaNegocio.Discos;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using CapaPresentacion.Utilitarios;
using System.Configuration;
using CapaDatos.ContadoresNegativos;
using CapaDatos;
using CapaEntidad.AsistenciaCliente;
using CapaEntidad.ControlAcceso;
using System.Net.Mail;
using System.Threading.Tasks;
using CapaEntidad.Ocurrencias;
using S3k.Utilitario;
using CapaNegocio.Reclamaciones;
using CapaPresentacion.Models;
using System.Collections;

namespace CapaPresentacion.Controllers.ContadoresNegativos
{
    [seguridad]
    public class ContadoresNegativosController : Controller
    {
        private ContadoresNegativosBL _contadoresBl = new ContadoresNegativosBL();
        private AlertaContadoresNegativosBL _alertaContador = new AlertaContadoresNegativosBL();
        private SalaBL _salaBl = new SalaBL();
        private EmpresaBL _empresa = new EmpresaBL();
        private ContadorNegativoConfigBL _contadorNegativoConfigBl = new ContadorNegativoConfigBL();
        private SEG_CargoBL _cargoBl = new SEG_CargoBL();

        private readonly LogTransac _log = new LogTransac();
        private string PathLogAlertaBilleteros = ConfigurationManager.AppSettings["PathLogAlertaBilleteros"];
        private string FirebaseKey = ConfigurationManager.AppSettings["firebaseServiceKey"];

        // GET: ContadoresNegativos
        public ActionResult ContadoresNegativosListado()
        {
            return View("~/Views/contadores/ContadoresNegativos.cshtml");
        }
        public ActionResult ConfiguracionAlertaContadorNegativo()
        {
            return View("~/Views/contadores/ConfiguracionAlertaContadorNegativo.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ContadorNegativoListarJson()
        {
            var errormensaje = "";
            var lista = new List<ContadorNegativoConfigEntidad>();
            var listaCargo = new List<SEG_CargoEntidad>();
            var listaSala = new List<SalaEntidad>();
            List<object> listaFinal = new List<object>();

            try
            {
                lista = _contadorNegativoConfigBl.ListadoContadorNegativoCargoConfig();
                listaCargo = _cargoBl.CargoListarJson();
                listaSala = _salaBl.ListadoSala();

                foreach (var registro in listaSala)
                {
                    List<object> cargos = new List<object>();
                    foreach (var registrocargo in listaCargo)
                    {
                        var reg = lista.Where(x => x.cargo_id == registrocargo.CargoID && x.sala_id == registro.CodSala).FirstOrDefault();
                        if (reg == null)
                        {
                            registrocargo.alt_id = 0;
                        }
                        else
                        {
                            registrocargo.alt_id = reg.idContadorConfigCargo;
                        }

                        cargos.Add(new
                        {
                            registrocargo.CargoID,
                            registrocargo.Descripcion,
                            registrocargo.Estado,
                            registrocargo.alt_id
                        });
                    }
                    //var registr = listaalerta.Where(x => x.cargo_id == registro.CargoID && x.sala_id==registro.).FirstOrDefault();
                    //registro.alt_id = registr.alt_id;
                    listaFinal.Add(new
                    {
                        registro.CodSala,
                        registro.Nombre,
                        registro.NombreCorto,
                        cargos
                    });
                }



            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = listaFinal.ToList(), mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult ContadoresNegativosCargoGuardarJson(ContadorNegativoConfigEntidad discoCargo)
        {
            var errormensaje = "";
            Int64 respuestaConsulta = 0;
            bool respuesta = false;
            try
            {
                discoCargo.fechaRegistro = DateTime.Now;
                //alertacargo.usuario_id = Convert.ToInt32(Session["UsuarioID"]);
                respuestaConsulta = _contadorNegativoConfigBl.ContadorNegativoConfigInsertarJson(discoCargo);

                if (respuestaConsulta > 0)
                {
                    respuesta = true;
                    errormensaje = "Registro Guardado Correctamente";
                }
                else
                {
                    errormensaje = "error al Guardar Registro , LLame Administrador";
                    respuesta = false;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, id = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult QuitarContadoresNegativosJson(int alt_id)
        {
            var errormensaje = "";
            bool respuesta = false;
            try
            {
                respuesta = _contadorNegativoConfigBl.ContadorNegativoConfigEliminarJson(alt_id);
                if (respuesta)
                {
                    respuesta = true;
                    errormensaje = "Se quitó Alerta Correctamente";
                }
                else
                {
                    errormensaje = "error al Quitar Alerta , LLame Administrador";
                    respuesta = false;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje });
        }








        [HttpPost]
        public JsonResult ListarContadoresNegativosFecha(int[] codsala, DateTime fechaini, DateTime fechafin)
        {
            var errormensaje = "";
            var lista = new List<ContadoresNegativosEntidad>();
            int cantElementos = (codsala == null) ? 0 : codsala.Length;
            var strElementos = String.Empty;
            try
            {
                if (cantElementos > 0)
                {
                    strElementos = " CodSala in(" + "'" + String.Join("','", codsala) + "'" + ") and ";
                }
                lista = _contadoresBl.ListadoContadoresNegativos(strElementos, fechaini, fechafin);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            var result = new JsonResult();
            result.MaxJsonLength = int.MaxValue;
            result.Data = new { data = lista.ToList(), mensaje = errormensaje };
            //return Json(new { data = lista.ToList(), mensaje = errormensaje });
            return result;
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult AgregarContadorNegativo(List<ContadoresNegativosVM> contadorNegativo)
        {
            var errormensaje = "";
            Int64 alt_id = 0;
            bool respuesta = false;
            var titulo = "¡Contador Negativo!";
            var servidorKey = FirebaseKey;
            DateTime fecha = DateTime.Now;
            int sala = 0;
            sala = contadorNegativo[0].CodSala;

            SalaEntidad datosSala = new SalaEntidad();
            EmpresaEntidad empresa = new EmpresaEntidad();

            datosSala = _salaBl.ListadoSala().Where(x => x.CodSala == sala).First();
            empresa = _empresa.ListadoEmpresa().Where(x => x.CodEmpresa== datosSala.CodEmpresa).First();
            List<ContadoresNegativosEntidad> registroContadorLista = new List<ContadoresNegativosEntidad>();
            List<string> correos = new List<string>();

            //ContadoresNegativosEntidad registroContador = new ContadoresNegativosEntidad();
            foreach (var registroContador in contadorNegativo)
            {
                ContadoresNegativosEntidad nuevoContador = new ContadoresNegativosEntidad();
                nuevoContador.CodEmpresa = registroContador.CodEmpresa;
                nuevoContador.NombreEmpresa = empresa.RazonSocial;
                nuevoContador.Descripcion = registroContador.Descripcion;
                nuevoContador.CodSala = registroContador.CodSala;
                nuevoContador.NombreSala = datosSala.Nombre;
                nuevoContador.CodMaquina = registroContador.CodMaquina;
                nuevoContador.FechaRegistroSala = Convert.ToDateTime(registroContador.FechaRegistroSala);
                nuevoContador.FechaRegistro = Convert.ToDateTime(registroContador.FechaRegistro);
                nuevoContador.CodigoId = registroContador.CodigoId;

                registroContadorLista.Add(nuevoContador);
            }


            int lista;
            List<AlertaContadorNegativoEntidad> devices = new List<AlertaContadorNegativoEntidad>();
         
                try
                {
                        foreach (ContadoresNegativosEntidad registro in registroContadorLista)
                        {
                            lista = _contadoresBl.AgregarContadorNegativo(registro);
                            //sala = disk.codSala;
                        }

                 string nombresMaquinas = string.Join(", ", registroContadorLista.Select(p => p.CodMaquina));

                //datosSala = _salaBl.ListadoSala().Where(x => x.CodSala == sala).First();
                //_log.escribir_logOK(PathLogAlertaBilleteros, "Sala:" + sala.Nombre + " CodSala:" + sala.CodSala);
                    devices = _alertaContador.AlertaContador_xdevicesListado(sala);
                    correos = _alertaContador.AlertaContadorCorreosListado(sala);
                    List<string> cod_maquinas = new List<string>();
                    string maquinas = string.Empty;

                    if (devices.Count == 0)
                    {
                        errormensaje = "No se encontraron dispositivos para envio , LLame Administrador";
                        respuesta = false;
                    }
                    else
                    {
                        respuesta = true;
                        errormensaje = "Se detecto un contador negativo en la sala  " + datosSala.Nombre + " maquinas : "+ nombresMaquinas;
                    }

                    respuesta = true;
                    string[] dispositivos = devices.Select(x => x.id).ToArray();
                    //string[] correos = devices.Select(x => x.mailJob).ToArray();
                    if(dispositivos.Length > 0)
                {
                    EnvioFirebase(servidorKey, dispositivos, errormensaje, titulo);
                }
                if (correos.Count > 0)
                {
                    EnviarCorreos(correos.ToArray(), sala, registroContadorLista);

                }
                devices.Clear();


            }
            catch (Exception exp)
                {
                    errormensaje = exp.Message + "Error,Llame al Administrador";
                }

            return Json(new { data = registroContadorLista, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult ReporteContadoresNegativosExcel(int[] codsala, DateTime fechaini, DateTime fechafin)
        {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<ContadoresNegativosEntidad> lista = new List<ContadoresNegativosEntidad>();
            int cantElementos = (codsala == null) ? 0 : codsala.Length;
            var strElementos = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;
            List<ContadoresNegativosEntidad> tickets = new List<ContadoresNegativosEntidad>();
            double totalmonto = 0;
            try
            {


                if (cantElementos > 0)
                {
                    for (int i = 0; i < codsala.Length; i++)
                    {
                        var salat = _salaBl.SalaListaIdJson(codsala[i]);
                        nombresala.Add(salat.Nombre);
                    }
                    salasSeleccionadas = String.Join(",", nombresala);

                    strElementos = " CodSala in(" + "'" + String.Join("','", codsala) + "'" + ") and ";
                }

                lista = _contadoresBl.ListadoContadoresNegativos(strElementos, fechaini, fechafin);
                if (lista.Count > 0)
                {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Alerta Billeteros");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[2, 2].Value = "SALAS : " + salasSeleccionadas;

                    workSheet.Cells[3, 2].Value = "idContadorNegativo";
                    workSheet.Cells[3, 3].Value = "CodEmpresa";
                    workSheet.Cells[3, 4].Value = "NombreEmpresa";
                    workSheet.Cells[3, 5].Value = "CodSala";
                    workSheet.Cells[3, 6].Value = "NombreSala";
                    workSheet.Cells[3, 7].Value = "CodMaquina";
                    workSheet.Cells[3, 8].Value = "FechaRegistroSala";
                    workSheet.Cells[3, 9].Value = "Descripcion";
                    workSheet.Cells[3, 10].Value = "FechaRegistro";

                    //Body of table  
                    //  
                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach (var registro in lista)
                    {
                        workSheet.Cells[recordIndex, 2].Value = registro.IdContadorNegativo;
                        workSheet.Cells[recordIndex, 3].Value = registro.CodEmpresa;
                        workSheet.Cells[recordIndex, 4].Value = registro.NombreEmpresa;
                        workSheet.Cells[recordIndex, 5].Value = registro.CodSala;
                        workSheet.Cells[recordIndex, 6].Value = registro.NombreSala;
                        workSheet.Cells[recordIndex, 7].Value = registro.CodMaquina;
                        workSheet.Cells[recordIndex, 8].Value = registro.FechaRegistroSala.ToString("dd-MM-yyyy hh:mm:ss tt");
                        workSheet.Cells[recordIndex, 9].Value = registro.Descripcion;
                        workSheet.Cells[recordIndex, 10].Value = registro.FechaRegistro.ToString("dd-MM-yyyy hh:mm:ss tt");
                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:J3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:J3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:J3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:J3"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B3:J3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:J3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:J3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:J3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B3:J3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:J3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:J3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:J3"].Style.Border.Bottom.Color.SetColor(colborder);

                    //workSheet.Cells.AutoFitColumns(60, 250);//overloaded min and max lengths
                    int filasagregadas = 3;
                    total = filasagregadas + total;
                    workSheet.Cells["N4:N" + total].Style.Numberformat.Format = "#,##0.00";
                    workSheet.Cells["M4:M" + total].Style.Numberformat.Format = "#,##0.00";
                    workSheet.Cells["B4:B" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["B2:J2"].Merge = true;
                    workSheet.Cells["B2:J2"].Style.Font.Bold = true;

                    int filaFooter = total + 1;
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Merge = true;
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells[filaFooter, 13].Style.Numberformat.Format = "#,##0.00";
                    workSheet.Cells[filaFooter, 14].Style.Numberformat.Format = "#,##0.00";
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Style.Font.Size = 14;

                    workSheet.Cells["B4:Q" + total].Style.WrapText = true;

                    int filaFooter_ = total + 2;
                    workSheet.Cells["B" + filaFooter_ + ":J" + filaFooter_].Merge = true;
                    workSheet.Cells["B" + filaFooter_ + ":J" + filaFooter_].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter_ + ":J" + filaFooter_].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter_ + ":J" + filaFooter_].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter_ + ":J" + filaFooter_].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter_ + ":J" + filaFooter_].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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
                   
                    excelName = "ReporteAlertas_" + fechaini.ToString("dd_MM_yyyy") + "_al_" + fechafin.ToString("dd_MM_yyyy") + "_.xlsx";
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


       
        public void EnvioFirebase(string servidorKey, String[] DeviceToken, string msg, string title)
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


        private void EnviarCorreos(string[] destinatarios, int codSala, List<ContadoresNegativosEntidad> contadorNegativo)
        {
            SalaEntidad sala = new SalaEntidad();
            Correo correo_enviar = new Correo();
            sala = _salaBl.SalaListaIdJson(codSala);
            string basepath = Request.Url.Scheme + "://" + ((Request.Url.Authority + Request.ApplicationPath).TrimEnd('/')) + "/";
            sala.RutaArchivoLogo = sala.RutaArchivoLogo != basepath + "Content/assets/images/no_image.jpg" ? "https://drive.google.com/uc?id=" + sala.RutaArchivoLogo : "";
            //Envio de Email a cliente y otros destinatarios
            //string srcLogoEmpresa = empresa.RutaArchivoLogo == string.Empty ? basepath + "Content/assets/images/no_image.jpg" : basepath + "Uploads/LogosEmpresas/" + empresa.RutaArchivoLogo;
            StringBuilder html = new StringBuilder();
            foreach (var elemento in contadorNegativo)
            {

                html.Append("<tr>");
                html.Append("<td style='border: 1px solid black; padding: 5px;' >");
                html.Append(elemento.CodMaquina);
                html.Append("</td>");
                html.Append("<td style='border: 1px solid black; padding: 5px;' >");
                html.Append(elemento.Descripcion);
                html.Append("</td>");
                html.Append("</tr>");
            }


            string htmlEnvio = $@"
                                     <div style='background: rgb(250,251,63);
                                                   background-image: linear-gradient(to top, #0c2c5c, #053a84, #0f48ac, #2955d6, #4960ff);width: 100%;padding:25px;'>
                                            <table style='border-radius:5px; max-width: 600px; display: table;margin:0 auto; background:#fff;padding:20px;'>
                                                <tbody>
                                                <tr>
                                                    <td colspan='2'>
                                                        <div style='border-radius:5px;text-align: center;font-family: Helvetica, Arial, sans-serif;  color: #fff; width:100%;background:#0C2C5C;padding:5px;'>
                                                            <h1>CONTADOR NEGATIVO</h1>
                                                             <p>Se registro un contador negativo</p>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr >
                                                    <td colspan='2'>
                                                       
                                                             <div style='text-align: center;font-family: Helvetica, Arial, sans-serif;color: #000000;'>
                                                                <h3>Empresa</h3>
                                                                <p
                                                                margin-bottom: 0px;'>{contadorNegativo[0].NombreEmpresa}</p>
                                                            </div>
                                                    </td>
                                                </tr>
                                                <tr >
                                                    <td colspan='2'>
                                                      
                                                             <div style='text-align: center;font-family: Helvetica, Arial, sans-serif;color: #000000;'>
                                                                <h3>Sala</h3>
                                                                <p
                                                                margin-bottom: 0px;'>{sala.Nombre}</p>
                                                            </div>
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <tr>
                                                        <th style=""background-color: #ccc; font-weight: bold; text-align: center; border: 1px solid black; padding: 5px;"">Código Máquina</th>
                                                        <th style=""background-color: #ccc; font-weight: bold; text-align: center; border: 1px solid black; padding: 5px;"">Descripción</th>
                                                    </tr>
                                                     {html}
                                                </tr>
                                                    <td colspan='2'>
                                                            <div style='text-align: right;font-family: Helvetica, Arial, sans-serif;color: #000000;'>
                                                                <h3>Fecha: {contadorNegativo[0].FechaRegistroSala}</h3>
                                                                
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
                         "CONTADOR NEGATIVO " ,
                            htmlEnvio,
                         true
                         );
            }

       
    }
}