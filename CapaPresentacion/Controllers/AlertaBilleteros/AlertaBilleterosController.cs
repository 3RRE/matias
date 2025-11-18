using CapaEntidad;
using CapaEntidad.Alertas;
using CapaEntidad.Response;
using CapaNegocio;
using CapaNegocio.Alertas;
using CapaPresentacion.Utilitarios;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.AlertaBilleteros {
    [seguridad]
    public class AlertaBilleterosController : Controller {
        private SalaBL _salaBl = new SalaBL();
        private readonly LogTransac _log = new LogTransac();
        private SEG_CargoBL cargoBl = new SEG_CargoBL();
        private ALT_AlertaSalaBL alertaSalaBL = new ALT_AlertaSalaBL();
        private ALT_AlertaCargoConfBL alertaCargoConfBL = new ALT_AlertaCargoConfBL();
        private SEG_RolUsuarioBL seg_rol_usuarioBL = new SEG_RolUsuarioBL();
        private SalaBL salaBl = new SalaBL();
        private SEG_PermisoRolBL seg_PermisoRolBL = new SEG_PermisoRolBL();
        private string FirebaseKey = ConfigurationManager.AppSettings["firebaseServiceKey"];
        private string PathLogAlertaBilleteros = ConfigurationManager.AppSettings["PathLogAlertaBilleteros"];
        private LogAlertaBilleterosBL logBL = new LogAlertaBilleterosBL();
        private readonly ServiceHelper _serviceHelper = new ServiceHelper();
        private string url_api_firebase = ConfigurationManager.AppSettings["url_api_firebase"];
        private int CodigoSalaSomosCasino = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoSalaSomosCasino"]);

        public ActionResult ReporteAlertaBilleterosVista() {
            return View("~/Views/AlertaBilleteros/ReporteAlertaBilleterosVista.cshtml");
        }

        public ActionResult ConfiguracionAlertaCargoVista() {
            return View("~/Views/AlertaBilleteros/ConfiguracionAlertaCargoVista.cshtml");
        }

        [HttpPost]
        public JsonResult ListarAlertasxFechaJson(int[] codsala, DateTime fechaini, DateTime fechafin) {
            var errormensaje = "";
            var lista = new List<ALT_AlertaSalaEntidad>();
            int cantElementos = (codsala == null) ? 0 : codsala.Length;
            var strElementos = String.Empty;
            try {
                if(cantElementos > 0) {
                    strElementos = " CodSala in(" + "'" + String.Join("','", codsala) + "'" + ") and ";
                }
                lista = alertaSalaBL.ALT_AlertaSala_xsala_idListado(strElementos, fechaini, fechafin);

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            var jsonResult = Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }


        [HttpPost]
        public ActionResult ReporteAlertasSalaDescargarExcelJson(int[] codsala, DateTime fechaini, DateTime fechafin) {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<ALT_AlertaSalaEntidad> lista = new List<ALT_AlertaSalaEntidad>();
            int cantElementos = (codsala == null) ? 0 : codsala.Length;
            var strElementos = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;
            List<ALT_AlertaSalaEntidad> tickets = new List<ALT_AlertaSalaEntidad>();
            double totalmonto = 0;
            try {


                if(cantElementos > 0) {
                    for(int i = 0; i < codsala.Length; i++) {
                        var salat = salaBl.SalaListaIdJson(codsala[i]);
                        nombresala.Add(salat.Nombre);
                    }
                    salasSeleccionadas = String.Join(",", nombresala);

                    strElementos = " CodSala in(" + "'" + String.Join("','", codsala) + "'" + ") and ";
                }

                lista = alertaSalaBL.ALT_AlertaSala_xsala_idListado(strElementos, fechaini, fechafin);
                if(lista.Count > 0) {

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

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "CodEmpresa";
                    workSheet.Cells[3, 4].Value = "NombreEmpresa";
                    workSheet.Cells[3, 5].Value = "CodSala";
                    workSheet.Cells[3, 6].Value = "NombreSala";
                    workSheet.Cells[3, 7].Value = "CodMaquina";
                    workSheet.Cells[3, 8].Value = "Juego";
                    workSheet.Cells[3, 9].Value = "fecha_registro";
                    workSheet.Cells[3, 10].Value = "fecha_termino";
                    workSheet.Cells[3, 11].Value = "cod_tipo_alerta";
                    workSheet.Cells[3, 12].Value = "descripcion_alerta";
                    workSheet.Cells[3, 13].Value = "ColorAlerta";
                    workSheet.Cells[3, 14].Value = "contador_bill_parcial";
                    workSheet.Cells[3, 15].Value = "contador_bill_billetero";
                    workSheet.Cells[3, 16].Value = "estado";
                    workSheet.Cells[3, 17].Value = "alts_fechareg";
                    //Body of table  
                    //  
                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach(var registro in lista) {
                        workSheet.Cells[recordIndex, 2].Value = registro.alts_id;
                        workSheet.Cells[recordIndex, 3].Value = registro.CodEmpresa;
                        workSheet.Cells[recordIndex, 4].Value = registro.NombreEmpresa;
                        workSheet.Cells[recordIndex, 5].Value = registro.CodSala;
                        workSheet.Cells[recordIndex, 6].Value = registro.NombreSala;
                        workSheet.Cells[recordIndex, 7].Value = registro.CodMaquina;
                        workSheet.Cells[recordIndex, 8].Value = registro.Juego;
                        workSheet.Cells[recordIndex, 9].Value = registro.fecha_registro;
                        workSheet.Cells[recordIndex, 10].Value = registro.fecha_termino;
                        workSheet.Cells[recordIndex, 11].Value = registro.cod_tipo_alerta;
                        workSheet.Cells[recordIndex, 12].Value = registro.descripcion_alerta;
                        workSheet.Cells[recordIndex, 13].Value = registro.ColorAlerta;
                        workSheet.Cells[recordIndex, 14].Value = registro.contador_bill_parcial;
                        workSheet.Cells[recordIndex, 15].Value = registro.contador_bill_billetero;
                        workSheet.Cells[recordIndex, 16].Value = (registro.estado == 1) ? "Activo" : "Desactivado";
                        workSheet.Cells[recordIndex, 17].Value = registro.alts_fechareg.ToString("dd-MM-yyyy hh:mm:ss tt");
                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:Q3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:Q3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:Q3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:Q3"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B3:Q3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:Q3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:Q3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:Q3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B3:Q3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:Q3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:Q3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:Q3"].Style.Border.Bottom.Color.SetColor(colborder);

                    //workSheet.Cells.AutoFitColumns(60, 250);//overloaded min and max lengths
                    int filasagregadas = 3;
                    total = filasagregadas + total;
                    workSheet.Cells["N4:N" + total].Style.Numberformat.Format = "#,##0.00";
                    workSheet.Cells["M4:M" + total].Style.Numberformat.Format = "#,##0.00";
                    workSheet.Cells["B4:B" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["B2:Q2"].Merge = true;
                    workSheet.Cells["B2:Q2"].Style.Font.Bold = true;

                    int filaFooter = total + 1;
                    workSheet.Cells["B" + filaFooter + ":M" + filaFooter].Merge = true;
                    workSheet.Cells["B" + filaFooter + ":Q" + filaFooter].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter + ":Q" + filaFooter].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter + ":Q" + filaFooter].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter + ":Q" + filaFooter].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter + ":Q" + filaFooter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells[filaFooter, 2].Value = "Total Monto : ";
                    workSheet.Cells[filaFooter, 10].Value = totalmonto;
                    workSheet.Cells[filaFooter, 13].Style.Numberformat.Format = "#,##0.00";
                    workSheet.Cells[filaFooter, 14].Style.Numberformat.Format = "#,##0.00";
                    workSheet.Cells["B" + filaFooter + ":Q" + filaFooter].Style.Font.Size = 14;

                    workSheet.Cells["B4:Q" + total].Style.WrapText = true;

                    int filaFooter_ = total + 2;
                    workSheet.Cells["B" + filaFooter_ + ":Q" + filaFooter_].Merge = true;
                    workSheet.Cells["B" + filaFooter_ + ":Q" + filaFooter_].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter_ + ":Q" + filaFooter_].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter_ + ":Q" + filaFooter_].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter_ + ":Q" + filaFooter_].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter_ + ":Q" + filaFooter_].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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
                    excelName = "ReporteAlertas_" + fechaini.ToString("dd_MM_yyyy") + "_al_" + fechafin.ToString("dd_MM_yyyy") + "_.xlsx";
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
        [HttpPost]
        public ActionResult CargoSalaListarJson() {
            var errormensaje = "";
            var lista = new List<ALT_AlertaCargoConfEntidad>();
            var listaCargo = new List<SEG_CargoEntidad>();
            var listaSala = new List<SalaEntidad>();
            List<object> listaFinal = new List<object>();
            try {
                lista = alertaCargoConfBL.ALT_AlertaCargoConf_Listado();
                listaCargo = cargoBl.CargoListarJson();
                listaSala = salaBl.ListadoSala();

                foreach(var registro in listaSala) {
                    List<object> cargos = new List<object>();
                    foreach(var registrocargo in listaCargo) {
                        var reg = lista.Where(x => x.cargo_id == registrocargo.CargoID && x.sala_id == registro.CodSala).FirstOrDefault();
                        if(reg == null) {
                            registrocargo.alt_id = 0;
                        } else {
                            registrocargo.alt_id = reg.alt_id;
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
        public ActionResult AlertaCargoGuardarJson(ALT_AlertaCargoConfEntidad alertacargo, int tipo) {
            var errormensaje = "";
            Int64 respuestaConsulta = 0;
            bool respuesta = false;
            try {
                alertacargo.alt_fechareg = DateTime.Now;
                //alertacargo.usuario_id = Convert.ToInt32(Session["UsuarioID"]);
                respuestaConsulta = alertaCargoConfBL.ALT_AlertaCargoConfInsertarJson(alertacargo, tipo);

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
        public ActionResult QuitarAlertaCargoJson(int alt_id) {
            var errormensaje = "";
            bool respuesta = false;
            try {
                respuesta = alertaCargoConfBL.ALT_AlertaCargoConfEliminarJson(alt_id);
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
        [HttpPost]
        public ActionResult QuitarAlertaCargoJsonV2(int sala_id, int cargo_id) {
            var errormensaje = "";
            bool respuesta = false;
            try {
                respuesta = alertaCargoConfBL.EliminarCargoAlertaSala(sala_id, cargo_id);
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

        [seguridad(false)]
        [HttpPost]
        public ActionResult AlertasrecepcionOnlineJson(List<ALT_AlertaSalaEntidad> alertas, SalaEntidad sala) {
            var errormensaje = "";
            Int64 alt_id = 0;
            bool respuesta = false;
            var titulo = "¡Alertas Sala!";
            var servidorKey = FirebaseKey;
            List<ALT_AlertaDeviceEntidad> devices = new List<ALT_AlertaDeviceEntidad>();
            try {
                _log.escribir_logOK(PathLogAlertaBilleteros, "Sala:" + sala.Nombre + " CodSala:" + sala.CodSala);
                DateTime fechaini = DateTime.Now.AddDays(-2);
                DateTime fecha = DateTime.Now;

                //codsala = alertas.Select(z => z.CodSala).First();
                var listaalertas = alertaSalaBL.ALT_AlertaSala_xsala_idFechaListado(Convert.ToInt32(sala.CodSala), fechaini, fecha);
                devices = alertaSalaBL.ALT_AlertaSala_xdevicesListado(Convert.ToInt32(sala.CodSala));
                if(alertas == null) {
                    foreach(var registro in listaalertas) {
                        ALT_AlertaSalaEntidad nuevo = new ALT_AlertaSalaEntidad();
                        nuevo.fecha_termino = fecha.ToString("dd-MM-yyyy hh:mm:ss tt");
                        nuevo.alts_id = registro.alts_id;
                        nuevo.estado = 0;
                        //var buscar = alertas.Where(x => x.AlertaID == registro.AlertaID).FirstOrDefault();
                        alertaSalaBL.ALT_AlertasalaEditarJson(nuevo);
                    }
                    respuesta = true;
                    errormensaje = "Se Desactivaron todas las alertas de la Sala:" + sala.Nombre;
                    string[] dispositivos_ = devices.Select(x => x.id).ToArray();
                    if(dispositivos_.Count() > 0) {
                        EnvioFirebase(servidorKey, dispositivos_, errormensaje, titulo, NotificationType.alerta);
                    }
                    devices.Clear();
                    respuesta = false;
                    return Json(new { respuesta, titulo, devices, servidorKey, mensaje = errormensaje });
                }

                foreach(var registro in listaalertas) {
                    ALT_AlertaSalaEntidad nuevo = new ALT_AlertaSalaEntidad();
                    nuevo.fecha_termino = fecha.ToString("dd-MM-yyyy hh:mm:ss tt");
                    nuevo.alts_id = registro.alts_id;
                    nuevo.estado = 0;
                    var buscar = alertas.Where(x => x.AlertaID == registro.AlertaID).FirstOrDefault();
                    if(buscar == null) {
                        alertaSalaBL.ALT_AlertasalaEditarJson(nuevo);
                    }
                }
                Int64 envio = 0;
                foreach(var registro in alertas) {
                    registro.alts_fechareg = fecha;
                    var existe = alertaSalaBL.ALT_AlertasalaAlertaIdObtenerJson(registro.AlertaID, Convert.ToInt32(sala.CodSala));
                    if(existe.alts_id > 0) {

                    } else {
                        alt_id = alertaSalaBL.ALT_AlertasalaInsertarJson(registro);
                        if(alt_id > 0) {
                            envio++;
                        }

                    }

                }

                if(devices.Count == 0) {
                    errormensaje = "No se encontraron dispositivos para envio , LLame Administrador";
                    respuesta = false;
                    return Json(new { respuesta, mensaje = errormensaje });
                }

                respuesta = true;
                errormensaje = "Se acaban de detectar alertas nuevas en máquinas ó cambios en los estados de las alertas por favor verifique";
                string[] dispositivos = devices.Select(x => x.id).ToArray();
                //_log.escribir_logOK(PathLogAlertaBilleteros, "Inicio Alerta");
                //_log.escribir_logOK(PathLogAlertaBilleteros, "Ids Alertas Sala: " + String.Join(" - ", alertas.Select(x => x.AlertaID).ToArray()) );
                //_log.escribir_logOK(PathLogAlertaBilleteros, "Variable envio : " + envio );
                //_log.escribir_logOK(PathLogAlertaBilleteros, "Dispositivos Enviados:" + String.Join("-- ",dispositivos));
                //_log.escribir_logOK(PathLogAlertaBilleteros, "Servidor Key:" + servidorKey);

                EnvioFirebase(servidorKey, dispositivos, errormensaje, titulo, NotificationType.alerta);
                devices.Clear();
                respuesta = false;
                //_log.escribir_logOK(PathLogAlertaBilleteros, "Termino Alerta");

            } catch(Exception exp) {
                respuesta = false;
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, titulo, devices, servidorKey, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult consultaonlinealertas(int estado, int usuarioId) {
            bool respuesta = false;
            var errormensaje = "";
            var response = "";
            var jsonResponse = new List<AlertBillNotificationReqEntidad>();

            SalaEntidad sala = new SalaEntidad();

            bool inVpn = false;

            try {
                WebClient client = new WebClient();
                string ruta = "/servicio/listadoAlertas?estado=" + estado;

                List<SalaEntidad> listaSalas = salaBl.ListadoSalaPorUsuario(usuarioId).Where(x => x.CodSala != CodigoSalaSomosCasino).ToList();

                if(listaSalas.Count != 1) {
                    respuesta = false;
                    errormensaje = "Usted debe tener asignado solo una sala para poder realizar la acción";
                } else {
                    SalaEntidad sala_unica = listaSalas.FirstOrDefault();

                    sala = salaBl.SalaListaIdJson(sala_unica.CodSala);

                    // start
                    if(sala.CodSala == 0) {
                        return Json(new {
                            respuesta = false,
                            mensaje = "No se encontró sala"
                        });
                    }

                    // salas sin vpn
                    string roomCode = Convert.ToString(sala.CodSala);
                    string roomsWithoutVPN = Convert.ToString(ValidationsHelper.GetValueAppSettingDB("ABILL_RoomsWithoutVPN_IAS", string.Empty));
                    List<string> listRoomsWVPN = string.IsNullOrEmpty(roomsWithoutVPN) ? new List<string>() : roomsWithoutVPN?.Replace(" ", "").Split(',').ToList();

                    if(listRoomsWVPN.Contains(roomCode)) {
                        List<AlertBillNotificationReqEntidad> listAlertaBilleteros = alertaSalaBL.ListarAlertBillNotificationSala(roomCode, estado, 1);

                        if(listAlertaBilleteros.Any()) {
                            jsonResponse = listAlertaBilleteros;

                            respuesta = true;
                            errormensaje = "Listado Alertas";
                        } else {
                            respuesta = false;
                            errormensaje = "No se encontro Registros";
                        }

                        JsonResult resultJSON = Json(new {
                            respuesta,
                            data = jsonResponse.ToList(),
                            mensaje = errormensaje,
                            inVpn
                        }, JsonRequestBehavior.AllowGet);

                        resultJSON.MaxJsonLength = int.MaxValue;

                        return resultJSON;
                    }
                    // salas sin vpn

                    //Check port
                    CheckPortHelper checkPortHelper = new CheckPortHelper();
                    CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, new List<string>());

                    if(!tcpConnection.IsOpen) {
                        return Json(new {
                            respuesta = false,
                            mensaje = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                        });
                    }

                    if(tcpConnection.IsVpn) {
                        if(string.IsNullOrEmpty(sala.IpPrivada) || sala.PuertoServicioWebOnline == 0) {
                            return Json(new {
                                respuesta = false,
                                mensaje = "Configurar correctamente la ip privada de la sala"
                            });
                        }

                        inVpn = true;
                        string endPoint = $"{tcpConnection.Url}/servicio/VPNGenericoPost";

                        object data = new {
                            ipPrivada = $"{sala.IpPrivada}:{sala.PuertoServicioWebOnline}{ruta}"
                        };

                        string json = JsonConvert.SerializeObject(data);

                        client.Headers.Add("content-type", "application/json");
                        client.Encoding = Encoding.UTF8;
                        response = client.UploadString(endPoint, "POST", json);
                    } else {
                        string endPoint = $"{sala.UrlProgresivo}{ruta}";

                        client.Headers.Add("content-type", "application/json");
                        client.Encoding = Encoding.UTF8;
                        response = client.UploadString(endPoint, "POST");
                    }
                    // end

                    var settings = new JsonSerializerSettings {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    jsonResponse = JsonConvert.DeserializeObject<List<AlertBillNotificationReqEntidad>>(response, settings);

                    if(jsonResponse.Count > 0) {
                        foreach(var registro in jsonResponse) {
                            DateTime oDate = Convert.ToDateTime(registro.fecha_registro);
                            registro.fecha_registro = oDate.ToString("dd-MM-yyyy hh:mm:ss tt");
                        }

                        errormensaje = "Listado Alertas";
                        respuesta = true;
                    } else {
                        errormensaje = "No se encontro Registros";
                        respuesta = false;
                    }
                }

            } catch(Exception ex) {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();

                return Json(new { respuesta, mensaje = ex.Message.ToString() });
            }

            var jsonResult = Json(new { respuesta, data = jsonResponse.ToList(), sala, mensaje = errormensaje, inVpn }, JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }


        [seguridad(false)]
        [HttpPost]
        public ActionResult consultaonlinealertasxSala(int estado, int codSala) {
            bool respuesta = false;
            var errormensaje = "";
            var response = "";
            var jsonResponse = new List<AlertBillNotificationReqEntidad>();

            bool inVpn = false;

            try {
                WebClient client = new WebClient();
                string ruta = "/servicio/listadoAlertas?estado=" + estado;

                SalaEntidad sala = salaBl.SalaListaIdJson(codSala);

                // start
                if(sala.CodSala == 0) {
                    return Json(new {
                        respuesta = false,
                        mensaje = "No se encontró sala"
                    });
                }

                // salas sin vpn
                string roomCode = Convert.ToString(codSala);
                string roomsWithoutVPN = Convert.ToString(ValidationsHelper.GetValueAppSettingDB("ABILL_RoomsWithoutVPN_IAS", string.Empty));
                List<string> listRoomsWVPN = string.IsNullOrEmpty(roomsWithoutVPN) ? new List<string>() : roomsWithoutVPN?.Replace(" ", "").Split(',').ToList();

                if(listRoomsWVPN.Contains(roomCode)) {
                    List<AlertBillNotificationReqEntidad> listAlertaBilleteros = alertaSalaBL.ListarAlertBillNotificationSala(roomCode, estado, 1);

                    if(listAlertaBilleteros.Any()) {
                        jsonResponse = listAlertaBilleteros;

                        respuesta = true;
                        errormensaje = "Listado Alertas";
                    } else {
                        respuesta = false;
                        errormensaje = "No se encontro Registros";
                    }

                    JsonResult resultJSON = Json(new {
                        respuesta,
                        data = jsonResponse.ToList(),
                        mensaje = errormensaje,
                        inVpn
                    }, JsonRequestBehavior.AllowGet);

                    resultJSON.MaxJsonLength = int.MaxValue;

                    return resultJSON;
                }
                // salas sin vpn

                //Check port
                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, new List<string>());

                if(!tcpConnection.IsOpen) {
                    return Json(new {
                        respuesta = false,
                        mensaje = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                    });
                }

                if(tcpConnection.IsVpn) {
                    if(string.IsNullOrEmpty(sala.IpPrivada) || sala.PuertoServicioWebOnline == 0) {
                        return Json(new {
                            respuesta = false,
                            mensaje = "Configurar correctamente la ip privada de la sala"
                        });
                    }

                    inVpn = true;
                    string endPoint = $"{tcpConnection.Url}/servicio/VPNGenericoPost";

                    object data = new {
                        ipPrivada = $"{sala.IpPrivada}:{sala.PuertoServicioWebOnline}{ruta}"
                    };

                    string json = JsonConvert.SerializeObject(data);

                    client.Headers.Add("content-type", "application/json");
                    client.Encoding = Encoding.UTF8;
                    response = client.UploadString(endPoint, "POST", json);
                } else {
                    string endPoint = $"{sala.UrlProgresivo}{ruta}";

                    client.Headers.Add("content-type", "application/json");
                    client.Encoding = Encoding.UTF8;
                    response = client.UploadString(endPoint, "POST");
                }
                // end

                var settings = new JsonSerializerSettings {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                jsonResponse = JsonConvert.DeserializeObject<List<AlertBillNotificationReqEntidad>>(response, settings);

                if(jsonResponse.Count > 0) {
                    foreach(var registro in jsonResponse) {
                        DateTime oDate = Convert.ToDateTime(registro.fecha_registro);
                        registro.fecha_registro = oDate.ToString("dd-MM-yyyy hh:mm:ss tt");
                    }

                    errormensaje = "Listado Alertas";
                    respuesta = true;
                } else {
                    errormensaje = "No se encontro Registros";
                    respuesta = false;
                }
            } catch(Exception ex) {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();

                return Json(new {
                    respuesta,
                    mensaje = ex.Message.ToString()
                });
            }

            var jsonResult = Json(new {
                respuesta,
                data = jsonResponse.ToList(),
                mensaje = errormensaje,
                inVpn
            }, JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        public enum NotificationType {
            alerta = 1, evento = 2
        }
        public class MessageRequest_new {
            public string Titulo { get; set; }
            public string Mensaje { get; set; }
            public List<string> DeviceTokens { get; set; } // List of device tokens for multiple recipients
            public NotificationType NotificationType { get; set; }
            public int CodSala { get; set; }
        }
        [seguridad(false)]
        public void EnvioFirebase(string servidorKey, String[] DeviceToken, string msg, string title, NotificationType notificationType, int codSala = 0) {

            //DeviceToken = ["fv3-LKWzSWmY3aETFLtsUa:APA91bGR95t15NmW16tHG4C_dokFzKVCW_LMhpE4vV6LbPuxxe-fCo9VYmn3NN7A363f3SuhQqdgiiMDmSk1p9ZlpTgqmT7y8XJdS6IOwXtR3DhDaaBsygQ"];
            var request = new MessageRequest_new {
                Titulo = title,
                Mensaje = msg,
                NotificationType = notificationType,
                DeviceTokens = DeviceToken.ToList(),
                CodSala = codSala
            };
            string jsonRequest = JsonConvert.SerializeObject(request);

            string url_api = url_api_firebase + "/Notificaciones/enviar_devicetokens";
            using(var client = new WebClient()) {
                try {
                    client.Encoding = System.Text.Encoding.UTF8;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var response = client.UploadString(url_api, "POST", jsonRequest);
                    _log.escribir_logOK(PathLogAlertaBilleteros, response.ToString());
                } catch(WebException ex) {
                    _log.escribir_logOK(PathLogAlertaBilleteros, "Error Firebase Api en " + url_api + " , datos : " + jsonRequest + " : " + ex.Message);
                } catch(Exception ex) {
                    _log.escribir_logOK(PathLogAlertaBilleteros, "Error Firebase Api en " + url_api + " , datos : " + jsonRequest + " : " + ex.Message);
                }
            }
            //ANT.
            //try
            //{
            //    //var serverKey = "AAAAuNqaZi0:APA91bHevFUNteMjQNHBNIC6I2WvlvwLv7thv92a1WPKfiA-dxMiMZ3YaVsf2aZ2PFN5ytBM1JNQIWevFjB5mH3FgZeIrRWGjHKQcXnPvYwuujd8dD16CISrid5XE1-MjyaO01wQFvWQ";
            //    var serverKey = servidorKey;
            //    //DeviceToken = "eiXYDwYt7_w:APA91bHJLSV2CmV5BdkTHZagnvLTuSJ7PbpI-zuLb5vaBhY3bytyD0tenGA0L-aRjOgNZsugUS6uS6RB_wPkD7LGIeY5FlbNZI5XuSpmvhXQNguzio8hWLYEwi3hRamitqFWqE7p72VB";
            //    WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            //    tRequest.Method = "post";
            //    //serverKey - Key from Firebase cloud messaging server  
            //    tRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
            //    //Sender Id - From firebase project setting  
            //    tRequest.Headers.Add(string.Format("Sender: id={0}", String.Join(",", DeviceToken)));
            //    tRequest.ContentType = "application/json";
            //    var payload = new {
            //        //to = "fcd2PSqhBGc:APA91bHA8z7G22s0cEWxsuPmMPXmJptMJ2S5-dToF-BtZxyHpo50sskedHiZliox6CJy1vDRZk6zlNFHsiosUdX62D4mhqMuOG3GnI4O96xxH0CJvtcodR8PVsoUh7DGVQUVN-mu5BpW",
            //        registration_ids = DeviceToken,
            //        priority = "high",
            //        content_available = true,
            //        data = new {
            //            notificationType,
            //            body = msg,
            //            title = title,
            //            badge = 1,
            //            codSala
            //        },
            //    };
            //    string postbody = JsonConvert.SerializeObject(payload).ToString();
            //    Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
            //    tRequest.ContentLength = byteArray.Length;
            //    using(Stream dataStream = tRequest.GetRequestStream()) {
            //        dataStream.Write(byteArray, 0, byteArray.Length);
            //        using(WebResponse tResponse = tRequest.GetResponse()) {
            //            using(Stream dataStreamResponse = tResponse.GetResponseStream()) {
            //                if(dataStreamResponse != null)
            //                    using(StreamReader tReader = new StreamReader(dataStreamResponse)) {
            //                        String sResponseFromServer = tReader.ReadToEnd();
            //                        _log.escribir_logOK(PathLogAlertaBilleteros, "Respuesta Firebase:" + sResponseFromServer);
            //                    }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    _log.escribir_logOK(PathLogAlertaBilleteros, "Respuesta Firebase" + ex.Message);
            //}
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult DesactivarAlertas(int codSala, int usuario_id) {
            SalaEntidad sala = new SalaEntidad();
            List<SalaEntidad> listaSalas = new List<SalaEntidad>();
            var errormensaje = "";
            var client = new System.Net.WebClient();
            var response = "";
            var uri = "/servicio/DesactivarAlertasBDtec";
            //var rutaticketregistrado = "/servicio/ConsultaTicketRegistradoCodCliente?nroTicket=" + nroticket;
            var jsonResponse = new List<dynamic>();
            bool respuesta = false;

            bool inVpn = false;

            try {
                var rol = seg_rol_usuarioBL.GetRolUsuarioId(usuario_id);

                string accion = "MobileSalaDescativarAlertasJson";
                var permiso = seg_PermisoRolBL.GetPermisoRolUsuario(rol.WEB_RolID, accion);

                if(permiso.Count == 0) {
                    respuesta = false;
                    errormensaje = "No tiene Permiso para Descativar Alertas";
                    return Json(new { respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
                }

                sala = salaBl.SalaListaIdJson(codSala);

                // start
                if(sala.CodSala == 0) {
                    return Json(new {
                        respuesta = false,
                        mensaje = "No se encontró sala"
                    });
                }

                //Check port
                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, new List<string>());

                if(!tcpConnection.IsOpen) {
                    return Json(new {
                        respuesta = false,
                        mensaje = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                    });
                }

                if(tcpConnection.IsVpn) {
                    if(string.IsNullOrEmpty(sala.IpPrivada) || sala.PuertoServicioWebOnline == 0) {
                        return Json(new {
                            respuesta = false,
                            mensaje = "Configurar correctamente la ip privada de la sala"
                        });
                    }

                    inVpn = true;
                    string endPoint = $"{tcpConnection.Url}/servicio/VPNGenericoPost";

                    object data = new {
                        ipPrivada = $"{sala.IpPrivada}:{sala.PuertoServicioWebOnline}{uri}"
                    };

                    string json = JsonConvert.SerializeObject(data);

                    client.Headers.Add("content-type", "application/json; charset=utf-8");
                    response = client.UploadString(endPoint, "POST", json);
                } else {
                    string endPoint = $"{sala.UrlProgresivo}{uri}";

                    client.Headers.Add("content-type", "application/json; charset=utf-8");
                    response = client.UploadString(endPoint, "POST");
                }
                // end

                //var settings = new JsonSerializerSettings
                //{
                //    NullValueHandling = NullValueHandling.Ignore,
                //    MissingMemberHandling = MissingMemberHandling.Ignore
                //};
                if((response.ToString().ToUpper().Contains("REGISTROS"))) {
                    errormensaje = "Alertas Desactivadas";
                    respuesta = true;
                } else {
                    errormensaje = "No se pudo desactivar las alertas";
                }
            } catch(Exception ex) {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { mensaje = ex.Message.ToString() });
            }
            var jsonResult = Json(new { respuesta, mensaje = errormensaje, inVpn }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult EscribirLog() {
            string mensaje = "";
            bool respuesta = true;
            try {
                _log.escribir_logOK(PathLogAlertaBilleteros, "Test Log correcto");
                respuesta = true;
                mensaje = "Testenando Log";
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult consultaPermisoalertas(int usuario_id) {
            SalaEntidad sala = new SalaEntidad();
            var errormensaje = "";

            bool respuesta = false;
            SalaEntidad sala_unica = new SalaEntidad();
            try {
                var rol = seg_rol_usuarioBL.GetRolUsuarioId(usuario_id);

                string accion = "MobileSalaMultipleJson";
                var permiso = seg_PermisoRolBL.GetPermisoRolUsuario(rol.WEB_RolID, accion);

                if(permiso.Count == 0) {
                    respuesta = false;
                    errormensaje = "No tiene Permiso para multi salas";
                    return Json(new { respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
                } else {
                    respuesta = true;
                }

            } catch(Exception ex) {

                return Json(new { respuesta, mensaje = ex.Message.ToString() });
            }

            return Json(new { respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet); ;
        }

        [HttpPost]
        public JsonResult MobileSalaMultipleJson() {
            bool respuesta = true;
            return Json(new { respuesta }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MobileSalaDescativarAlertasJson() {
            bool respuesta = true;
            return Json(new { respuesta }, JsonRequestBehavior.AllowGet);
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListadoTodosSalaJsonExterno(int usuario_id) {
            bool respuesta = false;
            var errormensaje = "Lista de salas de usuario";
            var lista = new List<SalaEntidad>();
            try {

                lista = salaBl.ListadoSalaPorUsuario(usuario_id);
                if(lista == null) {
                    errormensaje = "No se encontraron registros de Sala";
                } else {
                    if(lista.Count > 0) {
                        respuesta = true;
                    } else {
                        errormensaje = "No se encontro Registros de sala";
                    }
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult RecepcionarEstadoSistemaNewWebOnline(int CodSala, string Descripcion, string InfoAdicional = "") {
            object oRespuesta = new object();
            bool respuesta = false;
            Dictionary<string, string> myDictionary = new Dictionary<string, string>();
            try {
                myDictionary.Add("Descripcion", Descripcion);
                myDictionary.Add("InfoAdicional", InfoAdicional);
                LogAlertaBilleterosEntidad logEntidad = new LogAlertaBilleterosEntidad();
                logEntidad.CodSala = Convert.ToInt32(CodSala);
                logEntidad.Descripcion = Descripcion;
                logEntidad.Tipo = Convert.ToInt32(TipoLog.EventoServicioOnline);
                logEntidad.Descripcion = Newtonsoft.Json.JsonConvert.SerializeObject(myDictionary);
                logEntidad.FechaRegistro = DateTime.Now;
                logEntidad.Preview = Descripcion;
                //int longString = Descripcion.Length;
                //logEntidad.Preview = longString < 20 ? Descripcion.Substring(0, longString) : Descripcion.Substring(0, 20);
                int idInsertado = logBL.GuardarLogAlerta(logEntidad);
                respuesta = true;
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            oRespuesta = new {
                respuesta
            };
            return Json(oRespuesta);
        }
        [seguridad(false)]
        public ActionResult RecepcionarInformacionEventos(List<EVT_EventosOnlineEntidad> listaEventos, int CodSala) {
            DateTime fechaActual = DateTime.Now;
            List<ALT_AlertaDeviceEntidad> devices = new List<ALT_AlertaDeviceEntidad>();
            List<string> correos = new List<string>();
            ContadorCorreoAlertaEntidad contadorSala = alertaSalaBL.ObtenerValorContador();
            bool respuesta = false;
            string inQuery = string.Empty;
            List<LogAlertaBilleterosEntidad> listaEventosConsulta = new List<LogAlertaBilleterosEntidad>();
            List<string> listaStringInsertar = new List<string>();
            List<Int64> listaInsertados = new List<long>();
            string ConsultaInsertar = string.Empty;
            Dictionary<string, string> myDictionary = new Dictionary<string, string>();
            devices = alertaSalaBL.ALT_AlertaSala_xdevicesListado(Convert.ToInt32(CodSala)).Where(x => x.tipo == 2 || x.tipo == 3).ToList();
            correos = alertaSalaBL.AlertaEventosCorreos(CodSala);
            var servidorKey = FirebaseKey;
            var titulo = "¡Alerta Evento!";
            SalaEntidad datosSala = new SalaEntidad();

            datosSala = _salaBl.ListadoSala().Where(x => x.CodSala == CodSala).First();
            string errormensaje = "";
            //List<EVT_EventosOnlineEntidad> listaEventosCorreos = new List<EVT_EventosOnlineEntidad>();
            try {
                if(listaEventos.Count > 0) {
                    inQuery = " Cod_Even_OL in (" + string.Join(",", listaEventos.Select(x => x.Cod_Even_OL)) + ") and CodSala= " + CodSala;
                    listaEventosConsulta = logBL.GetLogsxCod_Even_OL(inQuery);
                    //Remover registros repetidos
                    if(listaEventosConsulta.Count > 0) {
                        foreach(var evt in listaEventosConsulta) {
                            var eventoConsulta = listaEventos.Where(x => x.Cod_Even_OL == evt.Cod_Even_OL).SingleOrDefault();
                            if(eventoConsulta != null) {
                                listaEventos.Remove(eventoConsulta);
                            }
                        }
                    }
                    foreach(var eventoInsertar in listaEventos) {
                        //myDictionary.Add("Cod_Even_OL", eventoInsertar.Cod_Even_OL.ToString());
                        //myDictionary.Add("Fecha", eventoInsertar.Fecha.ToLocalTime().ToString("yyyy-MM-dd hh:mm:ss"));
                        //myDictionary.Add("CodTarjeta", eventoInsertar.CodTarjeta);
                        //myDictionary.Add("CodMaquina", eventoInsertar.CodMaquina);
                        //myDictionary.Add("Cod_Evento", eventoInsertar.Cod_Evento.ToString());
                        //myDictionary.Add("Evento", eventoInsertar.Evento);
                        //DateTime fechaRegistroEvento = eventoInsertar.Fecha.ToLocalTime();
                        //eventoInsertar.Fecha = fechaRegistroEvento;
                        LogAlertaBilleterosEntidad logEntidad = new LogAlertaBilleterosEntidad();
                        logEntidad.CodSala = Convert.ToInt32(CodSala);
                        logEntidad.Tipo = Convert.ToInt32(TipoLog.EventoOnlineTecnologias);
                        logEntidad.Descripcion = Newtonsoft.Json.JsonConvert.SerializeObject(eventoInsertar);
                        logEntidad.FechaRegistro = DateTime.Now;
                        logEntidad.Cod_Even_OL = eventoInsertar.Cod_Even_OL;
                        logEntidad.Preview = eventoInsertar.Evento;
                        //int longString = eventoInsertar.Evento.Length;
                        //logEntidad.Preview = longString < 20 ? eventoInsertar.Evento.Substring(0, longString) : eventoInsertar.Evento.Substring(0, 20);
                        int idInsertado = logBL.GuardarLogAlerta(logEntidad);
                        /*EVT_EventosOnlineEntidad eventoCorreo = new EVT_EventosOnlineEntidad();
                        eventoCorreo = eventoInsertar;


                        if(eventoInsertar.Fecha.Hour == logEntidad.FechaRegistro.Hour) {
                            eventoCorreo.Fecha = eventoInsertar.Fecha;

                        } else if(eventoInsertar.Fecha.Hour +1 == logEntidad.FechaRegistro.Hour) {
                            eventoCorreo.Fecha = eventoInsertar.Fecha;
                        } else {
                            DateTime adjustedDateTime = eventoInsertar.Fecha.AddHours(-5);
                            eventoCorreo.Fecha = adjustedDateTime;
                        }
                        listaEventosCorreos.Add(eventoCorreo);*/

                    }

                    string[] dispositivos_ = devices.Select(x => x.id).ToArray();

                    if(dispositivos_.Count() > 0) {
                        string[] example = { "cN3tMlCTT1K9ucV3VaGKkQ:APA91bEKF54SRCsPJZ0EYgPHjdgYyU0HgADTiFdFzFuvW5CYdPlxAQOp9r2AHm6LuwGPPt5xNnJ00TDzsJXLw4FdQTduTRwkOnnNW3xz6A1uKO2ZHJ9S1YpiSqG3bqMVUW-N9QEmFqnQ" };
                        errormensaje = "Sala " + datosSala.Nombre + ", maq(s): " + String.Join(", ", listaEventos.Select(x => x.CodMaquina).ToList());
                        EnvioFirebase(servidorKey, example, errormensaje, titulo, NotificationType.evento, CodSala);
                    }
                    if(correos.Count > 0) {

                        if(contadorSala.Fecha != fechaActual.Date) {
                            alertaSalaBL.ResetearContador(fechaActual.Date);
                            contadorSala.Contador = 0;

                        }

                        //if(contadorSala.Contador > 1800) {
                        //    EnviarCorreos(correos.ToArray(), CodSala, listaEventosCorreos, "notificaciones2@gladcon.com", "*n0t.2021gL4-");
                        //} else {
                        //    EnviarCorreos(correos.ToArray(), CodSala, listaEventosCorreos, "development@gladcon.com", "*dev.Fr3d-");
                        //    alertaSalaBL.AgregarContador(contadorSala.Contador + 1);
                        //}

                        //if(contadorSala.Contador > 3600) {
                        //    // Colocar el tercer correo
                        //} else if(contadorSala.Contador > 450) {
                        //    EnviarCorreos(correos.ToArray(), CodSala, listaEventosCorreos, "development@gladcon.com", "iouohxllstreqqdf");

                        //}
                        //else {
                        //    EnviarCorreos(correos.ToArray(), CodSala, listaEventosCorreos, "notificaciones2@gladcon.com", "hrrlznqabdzvfcak");
                        //}


                    }
                    //if(_hubContext != null) {
                    //    _hubContext.Clients.All.EnviarMensaje();
                    //}



                    devices.Clear();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return Json(new { respuesta });
        }


        [seguridad(false)]
        public ActionResult ConsultarSalasUsuario() {
            int codUser = Convert.ToInt32(Session["UsuarioID"]);
            bool respuesta = false;
            string mensaje = "";
            List<LogAlertaBilleterosEntidad> ultimasAlertasPorSalaUsuario = new List<LogAlertaBilleterosEntidad>();
            try {
                List<int> codSalas = salaBl.ListadoIdSala(codUser);
                string cadenaResultado = string.Join(",", codSalas);
                ultimasAlertasPorSalaUsuario = logBL.GetTop10Alerta(cadenaResultado);
                if(ultimasAlertasPorSalaUsuario.Count == 0) {
                    mensaje = "Not tiene asignadas salas";
                    respuesta = false;

                } else {
                    mensaje = "Ultimos eventos registrados";
                    respuesta = true;

                }
            } catch(Exception e) {

                respuesta = false;
                mensaje = e.Message;
            }
            return Json(new { respuesta, mensaje, data = ultimasAlertasPorSalaUsuario });
        }


        [seguridad(false)]
        public ActionResult RecepcionarInformacionCierreEventos(EVT_EventosOnlineEntidad evento, int CodSala) {
            bool respuesta = false;
            Dictionary<string, string> DiccionarioCabecera = new Dictionary<string, string>();


            EVT_EventosOnlineEntidad eventoConsulta = new EVT_EventosOnlineEntidad();
            LogAlertaBilleterosEntidad logConsulta = new LogAlertaBilleterosEntidad();

            try {
                logConsulta = logBL.GetLogAlertaBilleteroPorCodEvenOL(evento.Cod_Even_OL, CodSala, (int)TipoLog.EventoOnlineTecnologias);

                LogAlertaBilleterosEntidad logEntidad = new LogAlertaBilleterosEntidad();
                logEntidad.Descripcion = Newtonsoft.Json.JsonConvert.SerializeObject(evento);
                if(logConsulta.Id != 0) {
                    //Editar
                    logEntidad.Id = logConsulta.Id;
                    respuesta = logBL.EditarLogAlertaBilletero(logEntidad);
                } else {
                    //Insertar
                    //DateTime fechaRegistroEvento = evento.Fecha.ToLocalTime();
                    //evento.Fecha = fechaRegistroEvento;
                    logEntidad.CodSala = Convert.ToInt32(CodSala);
                    logEntidad.Tipo = Convert.ToInt32(TipoLog.EventoOnlineTecnologias);
                    logEntidad.FechaRegistro = DateTime.Now;
                    logEntidad.Cod_Even_OL = evento.Cod_Even_OL;
                    logEntidad.Preview = evento.Evento;
                    //int longString = evento.Evento.Length;
                    //logEntidad.Preview = longString < 20 ? evento.Evento.Substring(0, longString) : evento.Evento.Substring(0, 20);
                    int IdInsertado = logBL.GuardarLogAlerta(logEntidad);
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return Json(new { respuesta });
        }
        [seguridad(false)]
        public ActionResult RecepcionarInformacionCierreAlertas(AlertBillNotificationReqEntidad alerta, int CodSala) {
            bool respuesta = false;

            EVT_EventosOnlineEntidad eventoConsulta = new EVT_EventosOnlineEntidad();
            LogAlertaBilleterosEntidad logConsulta = new LogAlertaBilleterosEntidad();

            try {
                logConsulta = logBL.GetLogAlertaBilleteroPorCodEvenOL(alerta.AlertaID, CodSala, (int)TipoLog.AlertaBilletero);
                LogAlertaBilleterosEntidad logEntidad = new LogAlertaBilleterosEntidad();
                logEntidad.Descripcion = Newtonsoft.Json.JsonConvert.SerializeObject(alerta);
                if(logConsulta.Id != 0) {
                    //Editar
                    logEntidad.Id = logConsulta.Id;
                    respuesta = logBL.EditarLogAlertaBilletero(logEntidad);
                } else {
                    //Insertar
                    alerta.fecha_registro = alerta.fecha_registro;
                    logEntidad.CodSala = Convert.ToInt32(CodSala);
                    logEntidad.Tipo = Convert.ToInt32(TipoLog.AlertaBilletero);
                    logEntidad.FechaRegistro = DateTime.Now;
                    logEntidad.Cod_Even_OL = alerta.AlertaID;
                    logEntidad.Preview = alerta.descripcion_alerta;
                    //int longString = evento.Evento.Length;
                    //logEntidad.Preview = longString < 20 ? evento.Evento.Substring(0, longString) : evento.Evento.Substring(0, 20);
                    int IdInsertado = logBL.GuardarLogAlerta(logEntidad);
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return Json(new { respuesta });
        }
        [seguridad(false)]
        public ActionResult RecepcionarInformacionAlertas(List<AlertBillNotificationReqEntidad> listaAlertas, int CodSala) {
            bool respuesta = false;
            string inQuery = string.Empty;
            List<LogAlertaBilleterosEntidad> listaEventosConsulta = new List<LogAlertaBilleterosEntidad>();
            List<string> listaStringInsertar = new List<string>();
            List<Int64> listaInsertados = new List<long>();
            string ConsultaInsertar = string.Empty;
            Dictionary<string, string> myDictionary = new Dictionary<string, string>();
            try {
                if(listaAlertas.Count > 0) {
                    inQuery = " Cod_Even_OL in (" + string.Join(",", listaAlertas.Select(x => x.AlertaID)) + ") and CodSala= " + CodSala;
                    listaEventosConsulta = logBL.GetLogsxCod_Even_OL(inQuery);
                    //Remover registros repetidos
                    if(listaEventosConsulta.Count > 0) {
                        foreach(var evt in listaEventosConsulta) {
                            var eventoConsulta = listaAlertas.Where(x => x.AlertaID == evt.Cod_Even_OL).SingleOrDefault();
                            if(eventoConsulta != null) {
                                listaAlertas.Remove(eventoConsulta);
                            }
                        }
                    }
                    foreach(var alertaInsertar in listaAlertas) {
                        LogAlertaBilleterosEntidad logEntidad = new LogAlertaBilleterosEntidad();
                        logEntidad.CodSala = Convert.ToInt32(CodSala);
                        logEntidad.Tipo = Convert.ToInt32(TipoLog.AlertaBilletero);
                        logEntidad.Descripcion = Newtonsoft.Json.JsonConvert.SerializeObject(alertaInsertar);
                        logEntidad.FechaRegistro = DateTime.Now;
                        logEntidad.Cod_Even_OL = alertaInsertar.AlertaID;
                        logEntidad.Preview = alertaInsertar.descripcion_alerta;
                        int idInsertado = logBL.GuardarLogAlerta(logEntidad);
                    }
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return Json(new { respuesta });
        }




        /////////////////////////////////////////////////////ENVIO DE CORREOS/////////////////////////////////////////////////////////////

        [seguridad(false)]
        public ActionResult CambiarHoraEnvioCorreo(string nuevaHora) {
            bool respuesta = false;
            return Json(new { respuesta });
        }

        [seguridad(false)]
        public ActionResult CrearExcelAlertas(int codSala, List<String> listaEventos, List<String> listaAlertas) {
            bool respuesta = false;
            return Json(new { respuesta });
        }

        [seguridad(false)]
        public ActionResult EnviarCorreoAlertasDestinatarios() {

            List<LogAlertaBilleterosEntidad> alertaEventos = logBL.ObtenerAlertasEventos();
            List<LogAlertaBilleterosEntidad> alertaBilleteros = logBL.ObtenerAlertasBilleteros();

            bool respuesta = false;
            return Json(new { respuesta });
        }

        [seguridad(false)]
        public ActionResult EnviarCorreoAlertaDestinatario() {
            bool respuesta = false;
            return Json(new { respuesta });
        }

        [seguridad(false)]
        public ActionResult CambiarHoraEnvioCorreo() {
            bool respuesta = false;
            return Json(new { respuesta });
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////version 2 servicio gladconnewwebonline
        ///

        [seguridad(false)]
        [HttpPost]
        public ActionResult AlertasrecepcionOnlineGladconJson(List<ALT_AlertaSalaEntidad> alertas, SalaEntidad sala) {
            DateTime fechaActual = DateTime.Now;
            ContadorCorreoAlertaEntidad contadorSala = alertaSalaBL.ObtenerValorContador();
            List<string> correos = new List<string>();
            var errormensaje = "";
            Int64 alt_id = 0;
            bool respuesta = false;
            var titulo = "¡Alerta Billeteros!";
            var servidorKey = FirebaseKey;
            DateTime fecha = DateTime.Now;
            List<ALT_AlertaDeviceEntidad> devices = new List<ALT_AlertaDeviceEntidad>();
            try {
                correos = alertaSalaBL.AlertBilleterosCorreos(Convert.ToInt32(alertas[0].CodSala));
                _log.escribir_logOK(PathLogAlertaBilleteros, "Sala:" + sala.Nombre + " CodSala:" + sala.CodSala);
                devices = alertaSalaBL.ALT_AlertaSala_xdevicesListado(Convert.ToInt32(sala.CodSala)).Where(x => x.tipo == 1 || x.tipo == 3).ToList();
                List<string> cod_maquinas = new List<string>();
                string maquinas = string.Empty;
                foreach(var registro in alertas) {
                    registro.fecha_registro = Convert.ToDateTime(registro.fecha_registro).ToString("dd/MM/yyyy hh:mm:ss tt");
                    registro.alts_fechareg = fecha;
                    var existe = alertaSalaBL.ALT_AlertasalaAlertaIdObtenerJson(registro.AlertaID, Convert.ToInt32(sala.CodSala));
                    if(existe.alts_id > 0) {

                    } else {
                        alt_id = alertaSalaBL.ALT_AlertasalaInsertarJson(registro);
                        if(alt_id > 0) {
                            cod_maquinas.Add(registro.CodMaquina);
                        }
                    }

                }

                if(cod_maquinas.Count() > 0) {
                    maquinas = String.Join(",", cod_maquinas);
                    errormensaje = $"Sala {sala.Nombre} en máq(s) ({maquinas}) ó cambios en los estados de las alertas, por favor verifique";
                } else {
                    errormensaje = "Se acaban de detectar alertas nuevas en máquina(s) ó cambios en los estados de las alertas por favor verifique";
                }
                if(devices.Count == 0) {
                    errormensaje = "No se encontraron dispositivos para envio , LLame Administrador";
                    respuesta = false;
                    return Json(new { respuesta, mensaje = errormensaje });
                }
                if(correos.Count > 0) {
                    //EnviarCorreosBilleteros(correos.ToArray(), Convert.ToInt32(alertas[0].CodSala), alertas);
                    if(contadorSala.Fecha != fechaActual.Date) {
                        alertaSalaBL.ResetearContador(fechaActual.Date);
                        contadorSala.Contador = 0;
                    }

                    //if(contadorSala.Contador > 3600) {
                    //    // Colocar el tercer correo
                    //} else if(contadorSala.Contador > 1800) {
                    //    EnviarCorreosBilleteros(correos.ToArray(), Convert.ToInt32(alertas[0].CodSala), alertas, "development@gladcon.com", "iouohxllstreqqdf");
                    //} else {
                    //    EnviarCorreosBilleteros(correos.ToArray(), Convert.ToInt32(alertas[0].CodSala), alertas, "notificaciones2@gladcon.com", "hrrlznqabdzvfcak");

                    //}

                }
                respuesta = true;
                if(cod_maquinas.Count() > 0) {
                    string[] dispositivos = devices.Select(x => x.id).ToArray();
                    //_log.escribir_logOK(PathLogAlertaBilleteros, "Inicio Alerta");
                    //_log.escribir_logOK(PathLogAlertaBilleteros, "Ids Alertas Sala: " + String.Join(" - ", alertas.Select(x => x.AlertaID).ToArray()) );
                    //_log.escribir_logOK(PathLogAlertaBilleteros, "Variable envio : " + envio );
                    //_log.escribir_logOK(PathLogAlertaBilleteros, "Dispositivos Enviados:" + String.Join("-- ",dispositivos));
                    //_log.escribir_logOK(PathLogAlertaBilleteros, "Servidor Key:" + servidorKey);

                    EnvioFirebase(servidorKey, dispositivos, errormensaje, titulo, NotificationType.alerta);
                    devices.Clear();
                    //_log.escribir_logOK(PathLogAlertaBilleteros, "Termino Alerta");
                }


            } catch(Exception exp) {
                respuesta = false;
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, titulo, devices, servidorKey, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult AlertasrecepcionOnlineTerminadasGladconJson(List<ALT_AlertaSalaEntidad> alertas, SalaEntidad sala) {
            var errormensaje = "";
            bool respuesta = false;
            var titulo = "¡Termino de Alertas Sala!";
            var servidorKey = FirebaseKey;
            List<ALT_AlertaDeviceEntidad> devices = new List<ALT_AlertaDeviceEntidad>();
            List<string> cod_maquinas = new List<string>();
            try {
                _log.escribir_logOK(PathLogAlertaBilleteros, "Sala:" + sala.Nombre + " CodSala:" + sala.CodSala);


                DateTime fechaini = DateTime.Now.AddDays(-2);
                DateTime fecha = DateTime.Now;

                //codsala = alertas.Select(z => z.CodSala).First();
                var listaalertas = alertaSalaBL.ALT_AlertaSala_xsala_idFechaListado(Convert.ToInt32(sala.CodSala), fechaini, fecha);

                foreach(var registro in listaalertas) {
                    if(registro.fecha_termino == null || registro.fecha_termino == "") {
                        ALT_AlertaSalaEntidad nuevo = new ALT_AlertaSalaEntidad();
                        nuevo.fecha_termino = fecha.ToString("dd-MM-yyyy hh:mm:ss tt");
                        nuevo.alts_id = registro.alts_id;
                        nuevo.estado = 0;
                        //var buscar = alertas.Where(x => x.AlertaID == registro.AlertaID).FirstOrDefault();
                        alertaSalaBL.ALT_AlertasalaEditarJson(nuevo);
                        cod_maquinas.Add(registro.CodMaquina);
                    }

                }

                devices = alertaSalaBL.ALT_AlertaSala_xdevicesListado(Convert.ToInt32(sala.CodSala));

                respuesta = devices.Count > 0;
                errormensaje = devices.Count > 0 ? $"Se desactivaron todas las alertas ({alertas.Count}) de la sala: {sala.Nombre}" : "No se encontraron dispositivos para el envío de notificaciones, LLame al Administrador";

                if(!respuesta) {
                    return Json(new { respuesta, mensaje = errormensaje });
                }

                if(cod_maquinas.Count > 0) {
                    string[] dispositivos = devices.Select(x => x.id).ToArray();
                    EnvioFirebase(servidorKey, dispositivos, errormensaje, titulo, NotificationType.alerta);
                    devices.Clear();
                }


            } catch(Exception exp) {
                respuesta = false;
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, titulo, devices, servidorKey, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult consultaonlinealertasV2(int estado, int usuarioId) {
            SalaEntidad sala = new SalaEntidad();
            var errormensaje = "";
            var client = new System.Net.WebClient();
            var response = "";
            var ruta = "/alertas/GetAllAlertasActivas?estado=" + estado;
            var jsonResponse = new List<AlertBillNotificationReqEntidad>();
            bool respuesta = false;
            List<SalaEntidad> listaSalas = new List<SalaEntidad>();
            SalaEntidad sala_unica = new SalaEntidad();
            try {
                listaSalas = salaBl.ListadoSalaPorUsuario(usuarioId).Where(x => x.CodSala != CodigoSalaSomosCasino).ToList();
                if(listaSalas.Count != 1) {
                    respuesta = false;
                    errormensaje = "Usted debe tener asignado solo una sala para poder realizar la acción";

                } else {
                    sala_unica = listaSalas.FirstOrDefault();

                    sala = salaBl.SalaListaIdJson(sala_unica.CodSala);
                    if(sala.UrlProgresivo == "") {
                        return Json(new { respuesta = false, mensaje = "No se configuró la url de sala" });
                    }
                    ruta = "http://localhost:5135" + ruta;
                    client.Headers.Add("content-type", "application/json");
                    client.Encoding = Encoding.UTF8;
                    response = client.UploadString(ruta, "GET");
                    var settings = new JsonSerializerSettings {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    jsonResponse = JsonConvert.DeserializeObject<List<AlertBillNotificationReqEntidad>>(response, settings);
                    if(jsonResponse.Count > 0) {
                        foreach(var registro in jsonResponse) {
                            DateTime oDate = Convert.ToDateTime(registro.fecha_registro);
                            registro.fecha_registro = oDate.ToString("dd-MM-yyyy hh:mm:ss tt");

                        }


                        errormensaje = "Listado Alertas";
                        respuesta = true;
                    } else {
                        errormensaje = "No se encontro Registros";
                        respuesta = false;
                    }
                }



            } catch(Exception ex) {
                response = " - " + ex.Message.ToString() + "\n ----------- InnerException=\n" + ex.InnerException + "\n --------------- Stack: ------------\n" + ex.StackTrace.ToString();
                return Json(new { respuesta, mensaje = ex.Message.ToString() });
            }
            var jsonResult = Json(new { respuesta, data = jsonResponse.ToList(), sala, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ConsultarAlertasCargo(int codSala) {
            int tipo = 0;
            string mensaje = "";
            try {
                tipo = alertaSalaBL.ConsultarAlertasCargo(codSala);
                if(tipo == 0) {
                    mensaje = "No tiene cargos asignados a esta sala";
                } else {
                    mensaje = "Consulta exitosa";

                }
            } catch(Exception ex) {
                mensaje = "Consulta exitosa" + ex;

            }
            return Json(new { tipo, mensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult CambiarTipoAlerta(int codSala, int tipo) {
            bool respuesta = false;
            string mensaje = "";
            try {
                respuesta = alertaSalaBL.CambiarTipoAlerta(codSala, tipo);
                if(respuesta) {
                    mensaje = "Se actualizo correctamente";
                } else {
                    mensaje = "No se actualizo";
                }
            } catch(Exception ex) {
                mensaje = "Error" + ex;

            }
            return Json(new { respuesta, mensaje });
        }


        private void EnviarCorreosBilleteros(string[] destinatarios, int codSala, List<ALT_AlertaSalaEntidad> discoEntidad, string usuario, string password) {
            SalaEntidad sala = new SalaEntidad();
            Correo correo_enviar = new Correo();
            sala = _salaBl.SalaListaIdJson(codSala);
            string basepath = Request.Url.Scheme + "://" + ((Request.Url.Authority + Request.ApplicationPath).TrimEnd('/')) + "/";
            sala.RutaArchivoLogo = sala.RutaArchivoLogo != basepath + "Content/assets/images/no_image.jpg" ? "https://drive.google.com/uc?id=" + sala.RutaArchivoLogo : "";
            //Envio de Email a cliente y otros destinatarios
            //string srcLogoEmpresa = empresa.RutaArchivoLogo == string.Empty ? basepath + "Content/assets/images/no_image.jpg" : basepath + "Uploads/LogosEmpresas/" + empresa.RutaArchivoLogo;
            StringBuilder html = new StringBuilder();
            string color = "";
            DateTime dataTime = DateTime.Now;

            foreach(var elemento in discoEntidad) {



                html.Append("<tr> ");

                html.Append("<td style='border: 1px solid black; padding: 5px; ' >");
                html.Append(elemento.CodMaquina);
                html.Append("</td>");
                html.Append("<td style='border: 1px solid black; padding: 5px; ' >");
                html.Append(elemento.descripcion_alerta);
                html.Append("</td>");
                html.Append("<td style='border: 1px solid black; padding: 5px; ' >");
                html.Append(elemento.contador_bill_billetero);
                html.Append("</td>");
                html.Append("<td style='border: 1px solid black; padding: 5px; ' >");
                html.Append(elemento.fecha_registro);
                html.Append("</td>");

                //html.Append("<td style='border: 1px solid black; padding: 5px; background-color: red;color:white' >");
                //html.Append(elemento.capacidadTotal);
                html.Append("</td>");


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
                                                            <h1>Alerta Billeteros</h1>
                                                        </div>
                                                    </td>
                                                </tr>
                                               
                                                <tr >
                                                    <td colspan='6'>
                                                             <div style='text-align: center;font-family: Helvetica, Arial, sans-serif;color: #000000;'>
                                                                <h3  style='margin-bottom: unset;   font-weight: bold;'>Sala</h3>
                                                                <h1
                                                                style='font-size:35px;margin:unset;font-weight: bold;'>{sala.Nombre}</h1>

                                                            </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan='6'>
                                                            <div style='font-family: Helvetica, Arial, sans-serif;color: #000000;'>
                                                                <h3 style='font-weight: lighter;'>Lista de alerta billeteros</h3>
                                                                
                                                            </div>
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <tr>
                                                        <th style=""background-color: #ccc; font-weight: bold; text-align: center; border: 1px solid black; padding: 5px;"">Código máquina</th>
                                                        <th style=""background-color: #ccc; font-weight: bold; text-align: center; border: 1px solid black; padding: 5px;"">Nivel alerta</th>
                                                        <th style=""background-color: #ccc; font-weight: bold; text-align: center; border: 1px solid black; padding: 5px;"">Total billetero</th>                                                    
                                                        <th style=""background-color: #ccc; font-weight: bold; text-align: center; border: 1px solid black; padding: 5px;"">Fecha registro</th>                                                       

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
                                                    
                                                       
                                               </ tr >
                                                    
                                                <tr>
                                                    <td colspan='6'>
                                                            <div style='text-align: right;font-family: Helvetica, Arial, sans-serif;color: #000000;'>
                                                                <h3>Fecha: {dataTime}</h3>
                                                                
                                                            </div>
                                                    </td>
                                                </tr>
                                               
                                                </tbody>
                                            </table>
                                        </div>
                                    ";

            /*leyenda correo disco*/


            var listac = String.Join(",", destinatarios);
            CorreoAlertas correo_destinatario = new CorreoAlertas(usuario, password);
            correo_destinatario.EnviarCorreo(
            listac,
                     "Alerta Billeteros - " + sala.Nombre,
                        htmlEnvio,
                     true
                     );
        }

        private void EnviarCorreos(string[] destinatarios, int codSala, List<EVT_EventosOnlineEntidad> discoEntidad, string correoUsuario, string passUusario) {

            SalaEntidad sala = new SalaEntidad();
            Correo correo_enviar = new Correo();
            sala = _salaBl.SalaListaIdJson(codSala);
            string basepath = Request.Url.Scheme + "://" + ((Request.Url.Authority + Request.ApplicationPath).TrimEnd('/')) + "/";
            sala.RutaArchivoLogo = sala.RutaArchivoLogo != basepath + "Content/assets/images/no_image.jpg" ? "https://drive.google.com/uc?id=" + sala.RutaArchivoLogo : "";
            //Envio de Email a cliente y otros destinatarios
            //string srcLogoEmpresa = empresa.RutaArchivoLogo == string.Empty ? basepath + "Content/assets/images/no_image.jpg" : basepath + "Uploads/LogosEmpresas/" + empresa.RutaArchivoLogo;
            StringBuilder html = new StringBuilder();
            string color = "";

            DateTime dataTime = DateTime.Now;
            foreach(var elemento in discoEntidad) {



                html.Append("<tr> ");

                html.Append("<td style='border: 1px solid black; padding: 5px; ' >");
                html.Append(elemento.CodMaquina);
                html.Append("</td>");
                html.Append("<td style='border: 1px solid black; padding: 5px; ' >");
                html.Append(elemento.Cod_Even_OL);
                html.Append("</td>");
                html.Append("<td style='border: 1px solid black; padding: 5px; ' >");
                html.Append(elemento.Evento);
                html.Append("</td>");
                html.Append("<td style='border: 1px solid black; padding: 5px; ' >");
                html.Append(elemento.Fecha);
                html.Append("</td>");
                //html.Append("<td style='border: 1px solid black; padding: 5px; background-color: red;color:white' >");
                //html.Append(elemento.capacidadTotal);
                html.Append("</td>");


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
                                                            <h1>Alerta Eventos</h1>
                                                        </div>
                                                    </td>
                                                </tr>
                                               
                                                <tr >
                                                    <td colspan='6'>
                                                             <div style='text-align: center;font-family: Helvetica, Arial, sans-serif;color: #000000;'>
                                                                <h3  style='margin-bottom: unset;   font-weight: bold;'>Sala</h3>
                                                                <h1
                                                                style='font-size:35px;margin:unset;font-weight: bold;'>{sala.Nombre}</h1>

                                                            </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan='6'>
                                                            <div style='font-family: Helvetica, Arial, sans-serif;color: #000000;'>
                                                                <h3 style='font-weight: lighter;'>Lista de eventos registrados</h3>
                                                                
                                                            </div>
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <tr>
                                                        <th style=""background-color: #ccc; font-weight: bold; text-align: center; border: 1px solid black; padding: 5px;"">Código máquina</th>
                                                        <th style=""background-color: #ccc; font-weight: bold; text-align: center; border: 1px solid black; padding: 5px;"">Código evento ol</th>
                                                        <th style=""background-color: #ccc; font-weight: bold; text-align: center; border: 1px solid black; padding: 5px;"">Evento</th>                                                    
                                                        <th style=""background-color: #ccc; font-weight: bold; text-align: center; border: 1px solid black; padding: 5px;"">Fecha registro</th>                                                       

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
                                                    
                                                       
                                               </ tr >
                                                    
                                                <tr>
                                                    <td colspan='6'>
                                                            <div style='text-align: right;font-family: Helvetica, Arial, sans-serif;color: #000000;'>
                                                                <h3>Fecha: {dataTime}</h3>
                                                                
                                                            </div>
                                                    </td>
                                                </tr>
                                               
                                                </tbody>
                                            </table>
                                        </div>
                                    ";

            /*leyenda correo disco*/


            var listac = String.Join(",", destinatarios);
            CorreoAlertas correo_destinatario = new CorreoAlertas(correoUsuario, passUusario);
            correo_destinatario.EnviarCorreo(
            listac,
                     "Alerta Eventos - " + sala.Nombre,
                        htmlEnvio,
                     true
                     );
        }

        #region Destinatarios Online

        [seguridad(false)]
        [HttpPost]
        public ActionResult ObtenerDestinatariosOnline(int salaId) {
            bool success = false;
            string message = "No se encontraron destinatarios";

            List<WEB_DestinatarioEntidad> data = new List<WEB_DestinatarioEntidad>();

            try {
                List<WEB_DestinatarioEntidad> webDestinatarios = alertaSalaBL.ObtenerDestinatariosOnline(salaId);

                if(webDestinatarios.Any()) {
                    data = webDestinatarios;

                    success = true;
                    message = "Destinatarios obtenidos";
                }
            } catch(Exception exception) {
                message = exception.Message;
            }

            return Json(new {
                success,
                message,
                data
            });
        }

        [seguridad(false)]
        [HttpPost]
        public async Task<ActionResult> SincronizarDestinatariosOnline(int salaId) {
            bool success = false;
            bool inVpn = false;
            string message = "Destinatarios no sincronizados";

            if(salaId <= 0) {
                return Json(new {
                    success,
                    message = "Por favor, seleccione una sala"
                });
            }

            try {
                SalaEntidad sala = _salaBl.ObtenerSalaPorCodigo(salaId);
                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, new List<string>());

                if(!tcpConnection.IsOpen) {
                    return Json(new {
                        success,
                        message = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                    });
                }

                List<WEB_DestinatarioEntidad> webDestinatarios = alertaSalaBL.ObtenerDestinatariosOnline(salaId);

                string servicePath = "servicio/SincronizarDestinatariosOnline";
                string content = string.Empty;
                string requestUri = string.Empty;

                if(tcpConnection.IsVpn) {
                    inVpn = true;

                    object arguments = new {
                        ipPrivada = $"{sala.IpPrivada}:{sala.PuertoServicioWebOnline}/{servicePath}",
                        destinatarios = webDestinatarios
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{tcpConnection.Url}/servicio/VPNGenericoPost";
                } else {
                    object arguments = new {
                        destinatarios = webDestinatarios
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{sala.UrlProgresivo}/{servicePath}";
                }

                ResultEntidad result = await _serviceHelper.PostAsync(requestUri, content);

                if(result.success) {
                    success = true;
                    message = result.message;
                }
            } catch(Exception exception) {
                message = exception.Message;
            }

            return Json(new {
                success,
                message,
                inVpn
            });
        }

        #endregion

    }
}