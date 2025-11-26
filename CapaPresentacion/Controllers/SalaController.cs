using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilitarios;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers {
    [seguridad]
    public class SalaController : Controller {
        private readonly SalaBL _salaBl = new SalaBL();
        private readonly UbigeoBL ubigeoBL = new UbigeoBL();
        private readonly EmpresaBL empresaBL = new EmpresaBL();
        private readonly SalaMaestraBL salaMaestraBL = new SalaMaestraBL();
        public ActionResult SalaVista() {
            return View();
        }
        public ActionResult SalaCamposProgresivoVista() {
            return View();
        }
        public ActionResult SalaInsertarVista() {
            return View();
        }
        [HttpPost]
        public JsonResult ListadoSala() {
            var errormensaje = "";
            var lista = new List<SalaEntidad>();
            try {
                lista = _salaBl.ListadoSala();
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListadoSalaActivasSinSeguridad() {
            var errormensaje = "";
            var lista = new List<SalaEntidad>();
            try {
                lista = _salaBl.ListadoSala();
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SalaModificarVista(string id) {
            int sub = Convert.ToInt32(id);
            var errormensaje = "";
            var sala = new SalaEntidad();
            UbigeoEntidad ubigeo = new UbigeoEntidad();
            try {
                sala = _salaBl.SalaListaIdJson(sub);
                sala.Empresa = empresaBL.EmpresaListaIdJson(sala.CodEmpresa);
                sala.RutaArchivoLogoAnt = sala.RutaArchivoLogo;
                ubigeo = ubigeoBL.GetDatosUbigeo(sala.CodUbigeo);

                var correos = sala.correo.Split(',').ToList();
                sala.correo = correos[0];
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                errormensaje = "Verifique conexion,Llame Administrador";
            }
            ViewBag.sala = sala;
            ViewBag.errormensaje = errormensaje;
            ViewBag.ubigeo = ubigeo;
            return View();
        }

        [HttpPost]
        public JsonResult SalaModificarJson(SalaEntidad sala) {
            var errormensaje = "";
            bool respuestaConsulta = false;
            HttpPostedFileBase file = Request.Files[0];
            GoogleDriveApiHelperV2 helper = new GoogleDriveApiHelperV2();
            int tamanioMaximo = 4194304;
            string extension = "";
            try {
                if(file.ContentLength > 0 && file != null) {
                    if(file.ContentLength <= tamanioMaximo) {
                        extension = Path.GetExtension(file.FileName).ToLower();
                        if(extension == ".jpg" || extension == ".png" || extension == ".jpeg") {

                            try {


                                //insertar en drive
                                UserCredential credential;

                                credential = helper.GetCredentials();

                                // Create Drive API service.
                                var service = new DriveService(new BaseClientService.Initializer() {
                                    HttpClientInitializer = credential,
                                    ApplicationName = "Sistema IAS",
                                });
                                //convertir imagen a base64
                                System.IO.Stream fs = file.InputStream;
                                System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                                Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                                string fileId = helper.UploadBase64FileInFolder(service, base64String);
                                sala.RutaArchivoLogo = fileId;

                                //Eliminar imagen de drive
                                if(sala.RutaArchivoLogoAnt != null && sala.RutaArchivoLogoAnt != "") {
                                    string resp = helper.DeleteFile(service, sala.RutaArchivoLogoAnt);
                                }

                            } catch(Exception ex) {
                                sala.RutaArchivoLogo = null;
                                errormensaje = "Google Drive token vencido.";
                            }

                        } else {
                            errormensaje = "Solo se permiten archivos .jpg, .png o jpeg.";
                        }
                    }
                } else {
                    sala.RutaArchivoLogo = sala.RutaArchivoLogoAnt;
                }

                SalaEntidad salaAux = new SalaEntidad();
                salaAux = _salaBl.SalaListaIdJson(sala.CodSala);
                var correos = salaAux.correo.Split(',').ToList();
                correos[0] = sala.correo;
                sala.correo = String.Join(",", correos);

                respuestaConsulta = _salaBl.SalaModificarJson(sala);
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        //[HttpPost]
        //public ActionResult ListadoSalaPorUsuarioJson(int id)
        //{
        //    var errormensaje = "";
        //    var lista = new List<SalaEntidad>();
        //    try
        //    {
        //        lista = _salaBL.ListadoSalaPorUsuario(id);
        //    }
        //    catch (Exception exp)
        //    {
        //        errormensaje = exp.Message + ", Llame Administrador";
        //    }
        //    return Json(new { data = lista.ToList(), mensaje = errormensaje });
        //}

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListadoSalaPorUsuarioJson() {
            var usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            var errormensaje = "";
            var lista = new List<SalaEntidad>();
            try {

                lista = _salaBl.ListadoSalaPorUsuario(usuarioId);
            } catch(Exception exp) {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListadoSalaPorUsuarioOfisisJson() {
            var usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            var errormensaje = "";
            var lista = new List<SalaEntidad>();
            try {

                lista = _salaBl.ListadoSalaPorUsuarioOfisis(usuarioId);
            } catch(Exception exp) {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult InsertarSalaJson(SalaEntidad sala) {
            string mensaje = "No se pudo insertar el registro";
            bool respuesta = false;
            //int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            int tamanioMaximo = 4194304;
            string extension = "";
            string rutaInsertar = "";
            HttpPostedFileBase file = Request.Files[0];
            GoogleDriveApiHelperV2 helper = new GoogleDriveApiHelperV2();
            try {
                //
                if(file.ContentLength > 0 && file != null) {
                    if(file.ContentLength <= tamanioMaximo) {
                        extension = Path.GetExtension(file.FileName).ToLower();
                        if(extension == ".jpg" || extension == ".png" || extension == ".jpeg") {


                            try {


                                //insertar en drive
                                UserCredential credential;

                                credential = helper.GetCredentials();

                                // Create Drive API service.
                                var service = new DriveService(new BaseClientService.Initializer() {
                                    HttpClientInitializer = credential,
                                    ApplicationName = "Sistema IAS",
                                });
                                //string base64String = @"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";
                                //Convertir a base64
                                System.IO.Stream fs = file.InputStream;
                                System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                                Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);

                                string fileId = helper.UploadBase64FileInFolder(service, base64String);
                                sala.RutaArchivoLogo = fileId;

                            } catch(Exception ex) {
                                sala.RutaArchivoLogo = null;
                                mensaje = " - Google Drive token vencido.";
                            }
                        } else {
                            mensaje = "Solo se permiten archivos .jpg, .png o jpeg.";
                        }
                    }
                } else {
                    //mensaje = "Debe seleccionar Un logo para la Empresa";
                    //return Json(new { respuesta, mensaje });
                    mensaje = "Sala sin logo";
                }
                //
                sala.FechaRegistro = DateTime.Now;
                sala.FechaModificacion = DateTime.Now;
                sala.Estado = 1;
                sala.Activo = true;
                respuesta = _salaBl.InsertarSalaJson(sala);
                if(respuesta) {
                    mensaje = "Registro Insertado" + mensaje;
                }
            } catch(Exception ex) {
                mensaje += ex.Message;
            }
            return Json(new { respuesta, mensaje });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetDataSelects(UbigeoEntidad ubigeo) {
            List<UbigeoEntidad> listaUbigeo = new List<UbigeoEntidad>();
            List<EmpresaEntidad> listaEmpresas = new List<EmpresaEntidad>();
            List<SalaMaestraEntidad> listaSalasMaestras = new List<SalaMaestraEntidad>();
            int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            string mensaje = "No se pudieron listar los registros";
            bool respuesta = false;
            object oRespuesta = new object();
            List<UbigeoEntidad> listaProvincias = new List<UbigeoEntidad>();
            List<UbigeoEntidad> listaDistritos = new List<UbigeoEntidad>();
            try {

                listaUbigeo = ubigeoBL.ListadoDepartamento();
                listaEmpresas = empresaBL.ListadoEmpresa();
                listaSalasMaestras = salaMaestraBL.ObtenerTodasLasSalasMaestras();
                if(ubigeo.CodUbigeo != 0) {
                    listaProvincias = ubigeoBL.GetListadoProvincia(ubigeo.DepartamentoId);
                    listaDistritos = ubigeoBL.GetListadoDistrito(ubigeo.ProvinciaId, ubigeo.DepartamentoId);
                    oRespuesta = new {
                        dataUbigeo = listaUbigeo,
                        dataEmpresas = listaEmpresas,
                        dataProvincias = listaProvincias,
                        dataDistritos = listaDistritos,
                        dataSalasMaestras = listaSalasMaestras
                    };
                } else {
                    oRespuesta = new {
                        dataUbigeo = listaUbigeo,
                        dataEmpresas = listaEmpresas,
                        dataSalasMaestras = listaSalasMaestras
                    };
                }

                respuesta = true;
                mensaje = "Listando registros";
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data = oRespuesta });
        }
        #region Ubigeo
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoDepartamento() {
            string mensaje = "";
            bool respuesta = false;
            List<UbigeoEntidad> lista = new List<UbigeoEntidad>();
            try {
                lista = ubigeoBL.ListadoDepartamento();
                mensaje = "Listando Registros";
                respuesta = true;
            } catch(Exception ex) {
                mensaje = ex.Message;
                respuesta = false;
            }
            return Json(new { mensaje, respuesta, data = lista });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoProvincia(int DepartamentoID) {
            string mensaje = "";
            bool respuesta = false;
            List<UbigeoEntidad> lista = new List<UbigeoEntidad>();
            try {
                lista = ubigeoBL.GetListadoProvincia(DepartamentoID);
                mensaje = "Listando Registros";
                respuesta = true;
            } catch(Exception ex) {
                mensaje = ex.Message;
                respuesta = false;
            }
            return Json(new { mensaje, respuesta, data = lista });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoDistrito(int ProvinciaID, int DepartamentoID) {
            string mensaje = "";
            bool respuesta = false;
            List<UbigeoEntidad> lista = new List<UbigeoEntidad>();
            try {
                lista = ubigeoBL.GetListadoDistrito(ProvinciaID, DepartamentoID);
                mensaje = "Listando Registros";
                respuesta = true;
            } catch(Exception ex) {
                mensaje = ex.Message;
                respuesta = false;
            }
            return Json(new { mensaje, respuesta, data = lista });
        }

        #endregion
        [seguridad(false)]
        [HttpPost]
        public JsonResult ListadoTodosSala() {
            var errormensaje = "";
            var lista = new List<SalaEntidad>();
            try {
                lista = _salaBl.ListadoTodosSala();
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }
        [seguridad(false)]
        [HttpPost]
        public JsonResult SalaModificarEstadoJson(SalaEntidad sala) {
            var errormensaje = "";
            bool respuesta = false;
            try {
                respuesta = _salaBl.SalaModificarEstadoJson(sala.CodSala, sala.Estado);
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ListadoTodosSalaExportarExcel() {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<SalaEntidad> lista = new List<SalaEntidad>();
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;
            try {


                lista = _salaBl.ListadoTodosSala();
                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Salas");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[3, 2].Value = "Codigo Sala";
                    workSheet.Cells[3, 3].Value = "Nombre";
                    workSheet.Cells[3, 4].Value = "Nombre Corto";
                    workSheet.Cells[3, 5].Value = "Url Progresivo";
                    workSheet.Cells[3, 6].Value = "Url Sala Online";
                    workSheet.Cells[3, 7].Value = "Estado";
                    //Body of table  
                    int recordIndex = 4;
                    int total = lista.Count;

                    foreach(var registro in lista) {
                        workSheet.Cells[recordIndex, 2].Value = registro.CodSala;
                        workSheet.Cells[recordIndex, 3].Value = registro.Nombre;
                        workSheet.Cells[recordIndex, 4].Value = registro.NombreCorto;
                        workSheet.Cells[recordIndex, 5].Value = registro.UrlProgresivo;
                        workSheet.Cells[recordIndex, 6].Value = registro.UrlSalaOnline;
                        workSheet.Cells[recordIndex, 7].Value = registro.Estado == 1 ? "ACTIVO" : "INACTIVO";
                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:G3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:G3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:G3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:G3"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B3:G3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:G3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:G3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:G3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B3:G3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:G3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:G3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:G3"].Style.Border.Bottom.Color.SetColor(colborder);

                    int filaFooter_ = recordIndex;
                    workSheet.Cells["B" + filaFooter_ + ":G" + filaFooter_].Merge = true;
                    workSheet.Cells["B" + filaFooter_ + ":G" + filaFooter_].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter_ + ":G" + filaFooter_].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter_ + ":G" + filaFooter_].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter_ + ":G" + filaFooter_].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter_ + ":G" + filaFooter_].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[filaFooter_, 2].Value = "Total : " + (total) + " Registros";

                    workSheet.Cells[3, 2, filaFooter_, 7].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 30;
                    workSheet.Column(4).Width = 30;
                    workSheet.Column(5).Width = 30;
                    workSheet.Column(6).Width = 30;
                    workSheet.Column(7).Width = 15;

                    excelName = "Excel_" + DateTime.Now.ToString("dd_MM_yyyy") + "_ListadoTodosSala.xlsx";
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());
                    mensaje = "Descargando Archivo";
                    respuesta = true;
                } else {
                    mensaje = "No se encontraron registros";
                }

            } catch(Exception exp) {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = base64String, excelName, respuesta });
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListadoTodosSalaJsonExterno() {
            bool respuesta = false;
            var errormensaje = "";
            var lista = new List<SalaEntidad>();
            try {
                lista = _salaBl.ListadoTodosSalaActivosOrderJson();
                if(lista == null) {
                    errormensaje = "No se asigno sala a Usuario";
                } else {
                    if(lista.Count > 0) {
                        respuesta = true;
                    } else {
                        errormensaje = "No se asigno sala a Usuario";
                    }
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ListadoSalaPorUsuarioJsonExterno(int usuario_id) {
            bool respuesta = false;
            var errormensaje = "";
            var lista = new List<SalaEntidad>();
            try {

                lista = _salaBl.ListadoSalaPorUsuario(usuario_id);
                if(lista == null) {
                    errormensaje = "No se asigno sala a Usuario";
                } else {
                    if(lista.Count > 0) {
                        respuesta = true;
                    } else {
                        errormensaje = "No se asigno sala a Usuario";
                    }
                }

            } catch(Exception exp) {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta, mensaje = errormensaje });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetImgPorIdDrive(string RutaArchivoLogo) {
            GoogleDriveApiHelperV2 helper = new GoogleDriveApiHelperV2();
            bool respuesta = false;
            string mensaje = string.Empty;
            string base64String = string.Empty;
            try {
                //insertar en drive
                UserCredential credential;

                credential = helper.GetCredentials();

                // Create Drive API service.
                var service = new DriveService(new BaseClientService.Initializer() {
                    HttpClientInitializer = credential,
                    ApplicationName = "Sistema IAS",
                });
                base64String = helper.DownloadFile(service, RutaArchivoLogo);
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data = base64String });
        }




        [seguridad(false)]
        [HttpPost]
        public JsonResult ListadoSalaCamposProgresivo() {
            var errormensaje = "";
            var lista = new List<SalaEntidad>();
            try {
                lista = _salaBl.ListadoCamposProgresivoSalas();
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }



        [seguridad(false)]
        public JsonResult SalaModificarCamposProgresivoJson(int salaId, string nameQuery, string value) {
            var errormensaje = "";
            bool respuesta = false;

            UbigeoEntidad ubigeo = new UbigeoEntidad();
            try {
                respuesta = _salaBl.SalaModificarCamposProgresivoJson(salaId, nameQuery, value);

            } catch(Exception ex) {
                errormensaje = ex.Message + ",Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }


        private bool EchoPingWithPort(string ip, int port) {
            bool ok = true;
            TimeSpan timeout = new TimeSpan(0, 0, 1);
            try {
                using(TcpClient tcp = new TcpClient()) {
                    IAsyncResult result = tcp.BeginConnect(ip, port, null, null);
                    WaitHandle wait = result.AsyncWaitHandle;
                    try {
                        if(!result.AsyncWaitHandle.WaitOne(timeout, false)) {
                            tcp.Close();
                            ok = false;
                        }
                    } catch(Exception) {
                        ok = false;
                        tcp.EndConnect(result);
                    } finally {
                        wait.Close();
                    }
                }
            } catch(Exception) {
                ok = false;
            }
            return ok;
        }

        [HttpPost]
        [seguridad(false)]
        public ActionResult EchoPingSalasUsuario() {
            List<SalaEntidad> listaSalas = new List<SalaEntidad>();
            List<object> result = new List<object>();
            try {
                var usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                listaSalas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                listaSalas = listaSalas.Where(x => !string.IsNullOrEmpty(x.UrlProgresivo)).ToList();
                foreach(var item in listaSalas) {
                    Uri uri = new Uri(item.UrlProgresivo);
                    bool response = EchoPingWithPort(uri.Host, uri.Port);
                    result.Add(new {
                        uri = item.UrlProgresivo,
                        respuesta = response
                    });
                }
            } catch(Exception) {
                result = new List<object>();
            }
            return Json(result);
        }


        public ActionResult SalaEchoPingVista() {
            return View();
        }

        [seguridad(false)]
        public JsonResult ListadoPingIpPublica() {

            var errormensaje = "";
            var lista = new List<SalaEntidad.PingIpPublica>();


            try {

                lista = _salaBl.ListadoIpPublicaSalas();

                lista = lista.Where(x => !string.IsNullOrEmpty(x.IpPublica)).ToList();

                foreach(var item in lista) {

                    Uri uri = new Uri(item.IpPublica);

                    item.Puerto9895 = EchoPingWithPort(uri.Host, 9895);

                    item.Puerto2020 = EchoPingWithPort(uri.Host, 2020);

                    item.Puerto8081 = EchoPingWithPort(uri.Host, 8081);

                    item.IpPublica = uri.Host;

                }


            } catch(Exception ex) {
                errormensaje = ex.Message + ",Llame Administrador";
            }

            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [seguridad(false)]
        public JsonResult ListadoPingIpPrivada(string ipSala) {

            //ipSala = "200.60.148.21";
            ipSala = "http://" + ipSala.Trim() + ":9895";
            //ipSala = "http://localhost:9895";


            var errormensaje = "";
            var lista = new List<SalaEntidad.PingIpPrivada>();
            var listaRespuesta = new List<SalaEntidad.PingIpPrivada>();
            bool respuesta = false;

            try {

                lista = _salaBl.ListadoIpPrivadaSalas();

                lista = lista.Where(x => !string.IsNullOrEmpty(x.IpPrivada)).ToList();


                try {

                    var client = new System.Net.WebClient();
                    var response = "";
                    var ruta = "/servicio/ListadoPingIpPrivada";
                    var jsonResponse = new List<SalaEntidad.PingIpPrivada>();



                    ruta = ipSala + ruta;
                    client.Headers.Add("content-type", "application/json");
                    client.Encoding = Encoding.UTF8;

                    string parameters = JsonConvert.SerializeObject(lista);
                    using(WebClient wc = new WebClient()) {
                        wc.Headers.Add("content-type", "application/json");
                        response = wc.UploadString(ruta, "POST", parameters);
                    }
                    var settings = new JsonSerializerSettings {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    jsonResponse = JsonConvert.DeserializeObject<List<SalaEntidad.PingIpPrivada>>(response, settings);
                    listaRespuesta = JsonConvert.DeserializeObject<List<SalaEntidad.PingIpPrivada>>(response, settings);
                    respuesta = true;
                } catch(Exception ex) {

                    errormensaje = "No se pudo revisar las Ips Privadas, intente de nuevo.";
                }



            } catch(Exception ex) {
                errormensaje = ex.Message + ",Llame Administrador";
            }

            return Json(new { data = listaRespuesta.ToList(), mensaje = errormensaje, respuesta = respuesta }, JsonRequestBehavior.AllowGet);
        }


        [seguridad(false)]
        public JsonResult ListadoDispositivos(string urlPublica, string urlPrivada, int tipo = 1) {

            urlPublica = "http://" + urlPublica.Trim() + ":9895";
            urlPrivada = "http://" + urlPrivada.Trim() + ":9895";


            var errormensaje = "";
            var lista = new List<EVT_DispositivoEntidad>();
            var listaRespuesta = new List<EVT_DispositivoEntidad>();
            bool respuesta = false;

            try {



                try {

                    var response = "";
                    var ruta = "/servicio/BuscarListadoDispositivosProgresivosUrlPrivada";
                    var jsonResponse = new List<EVT_DispositivoEntidad>();
                    ruta = urlPublica + ruta;

                    var parameters = JsonConvert.SerializeObject(new {
                        urlPrivada,
                        tipo
                    });

                    using(WebClient wc = new WebClient()) {
                        wc.Headers.Add("content-type", "application/json");
                        response = wc.UploadString(ruta, "POST", parameters);
                    }
                    var settings = new JsonSerializerSettings {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    listaRespuesta = JsonConvert.DeserializeObject<List<EVT_DispositivoEntidad>>(response, settings);
                    respuesta = true;

                    if(listaRespuesta == null) {
                        errormensaje = "No respondio la Ip Privada " + urlPrivada;
                        respuesta = false;
                        return Json(new { data = lista.ToList(), mensaje = errormensaje, respuesta = respuesta }, JsonRequestBehavior.AllowGet);
                    }

                } catch(Exception ex) {

                    errormensaje = "No se pudo revisar los dispositivos, intente de nuevo.";
                }



            } catch(Exception ex) {
                errormensaje = ex.Message + ",Llame Administrador";
            }

            return Json(new { data = listaRespuesta.ToList(), mensaje = errormensaje, respuesta = respuesta }, JsonRequestBehavior.AllowGet);
        }


        [seguridad(false)]
        public JsonResult ListadoProgresivos(string urlPublica, string urlPrivada, int tipo = 2) {

            urlPublica = "http://" + urlPublica.Trim() + ":9895";
            urlPrivada = "http://" + urlPrivada.Trim() + ":9895";


            var errormensaje = "";
            var lista = new List<WEB_Progresivo>();
            var listaRespuesta = new List<WEB_Progresivo>();
            bool respuesta = false;

            try {


                var response = "";
                var ruta = "/servicio/BuscarListadoDispositivosProgresivosUrlPrivada";
                var jsonResponse = new List<WEB_Progresivo>();


                ruta = urlPublica + ruta;

                var parameters = JsonConvert.SerializeObject(new {
                    urlPrivada,
                    tipo
                });

                try {


                    using(WebClient wc = new WebClient()) {
                        wc.Headers.Add("content-type", "application/json");
                        response = wc.UploadString(ruta, "POST", parameters);
                    }

                    var settings = new JsonSerializerSettings {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    listaRespuesta = JsonConvert.DeserializeObject<List<WEB_Progresivo>>(response, settings);
                    respuesta = true;

                    if(listaRespuesta == null) {
                        errormensaje = "No respondio la Ip Privada " + urlPrivada;
                        respuesta = false;
                        return Json(new { data = lista.ToList(), mensaje = errormensaje, respuesta = respuesta }, JsonRequestBehavior.AllowGet);
                    }

                } catch(Exception ex) {

                    errormensaje = "No se pudo revisar los progresivos, intente de nuevo.";
                }



            } catch(Exception ex) {
                errormensaje = ex.Message + ",Llame Administrador";
            }

            return Json(new { data = listaRespuesta.ToList(), mensaje = errormensaje, respuesta = respuesta }, JsonRequestBehavior.AllowGet);
        }


        [seguridad(false)]
        [HttpPost]
        public JsonResult ListadoIpsSalas() {
            var errormensaje = "";
            var lista = new List<SalaEntidad>();
            try {
                lista = _salaBl.ListadoIpsSalas();
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ConfiguracionCorreosSala() {
            return View("~/Views/Sala/ConfiguracionCorreosSala.cshtml");
        }

        public JsonResult ListadoCorreosSala() {
            string errorMessage = "Lista de los correos de salas obtenidos exitosamente.";
            bool status = true;
            List<CorreoSala> listaCorreos = new List<CorreoSala>();
            try {
                listaCorreos = _salaBl.ObtenerCorreosSala();
                if(listaCorreos.Count == 0) {
                    errorMessage = "No se encontraron registros.";
                }

            } catch(Exception exp) {
                errorMessage = exp.Message + ",Llame Administrador";
                status = false;
            }
            return Json(new { data = listaCorreos.ToList(), message = errorMessage, status }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerdetalleCorreoSala(int salaId) {
            string errorMessage = "Se obtuvo el registro exitosamente.";
            bool status = true;
            CorreoSala correoSala = new CorreoSala();
            try {
                correoSala = _salaBl.ObtenerDetalleCorreosSala(salaId);

            } catch(Exception exp) {
                errorMessage = exp.Message + ",Llame Administrador";
                status = false;
            }
            return Json(new { data = correoSala, message = errorMessage, status }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ActualizarCorreoSala(CorreoSala data) {
            string errorMessage = "Se actualizo correo correctamente.";
            bool status = true;

            try {
                status = _salaBl.ActualizarCorreoSala(data);

            } catch(Exception exp) {
                errorMessage = exp.Message + ",Llame Administrador";
                status = false;
            }
            return Json(new { message = errorMessage, status }, JsonRequestBehavior.AllowGet);
        }

        #region Sala Maestra
        [HttpPost]
        public JsonResult ObtenerSalasDeSalaMaestraPorCodigoSalaMaestra(int codSalaMaestra) {
            bool success = false;
            List<SalaEntidad> salas = new List<SalaEntidad>();
            string displayMessage;

            try {
                salas = _salaBl.ObtenerTodasLasSalasDeSalaMaestraPorCodigoSalaMaestra(codSalaMaestra);
                success = salas.Count > 0;
                displayMessage = salas.Count > 0 ? "Lista Salas de Sala Maestra." : "La Sala Maestra Seleccionada no tiene salas asignadas.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = salas.ToList(), displayMessage }, JsonRequestBehavior.AllowGet);
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListadoSalaMaestraPorUsuarioJson() {
            var usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            var errormensaje = "";
            var lista = new List<SalaEntidad>();
            try {
                lista = _salaBl.ListadoSalaMaestraPorUsuario(usuarioId);
            } catch(Exception exp) {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }
        #endregion

        [HttpPost]
        public ActionResult ActualizaHoraApertura(long salaId, string horaApertura) {
            bool status = false;
            string message = "No se pudo actualizar la hora de apertura";

            try {
                bool updated = _salaBl.ActualizarHoraApertura(salaId, horaApertura);

                if(updated) {
                    status = true;
                    message = "La hora de apertura se ha actualizado";
                }
            } catch(Exception exception) {
                message = exception.Message;
            }

            return Json(new {
                status,
                message
            });
        }
    }
}