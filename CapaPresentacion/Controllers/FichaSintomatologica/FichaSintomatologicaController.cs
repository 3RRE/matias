using CapaDatos.Utilitarios;
using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.FichaSintomatologica {
    [seguridad]
    public class FichaSintomatologicaController : Controller {
        private readonly SalaBL _salaBl = new SalaBL();
        private readonly EmpresaBL _empresaBl = new EmpresaBL();
        FichaSintomatologicaBL fichaSintomatologicaBL = new FichaSintomatologicaBL();
        SEG_EmpleadoBL _SEG_EmpleadoBL = new SEG_EmpleadoBL();
        UsuarioSalaBL usuarioSalaBL = new UsuarioSalaBL();
        SalaBL salaBL = new SalaBL();
        ClaseError error = new ClaseError();
        private SEG_UsuarioBL segUsuarioBl = new SEG_UsuarioBL();
        string ubicacionarchivos = ConfigurationManager.AppSettings["PathArchivos"].ToString();
        private int CodigoSalaSomosCasino = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoSalaSomosCasino"]);

        public ActionResult FichaVista() {
            List<SalaEntidad> listaSalas = new List<SalaEntidad>();
            listaSalas = _salaBl.ListadoSalaPorUsuario(Convert.ToInt32(Session["UsuarioID"])).Where(x => x.CodSala != CodigoSalaSomosCasino).ToList();
            if(listaSalas.Count() > 1) {
                ViewBag.multiple = true;
            }
            if(listaSalas.Count() == 0) {
                ViewBag.multiple = true;
            }
            if(listaSalas.Count() == 1) {
                ViewBag.multiple = false;
                var sala = listaSalas[0];
                var empresa = _empresaBl.EmpresaListaIdJson(sala.CodEmpresa);
                ViewBag.nombreSala = sala.Nombre;
                ViewBag.idSala = sala.CodSala;
                ViewBag.nombreEmpresa = empresa.RazonSocial;
                ViewBag.direccionEmpresa = empresa.Direccion;
                ViewBag.idEmpresa = empresa.CodEmpresa;
                ViewBag.logoEmpresa = empresa.RutaArchivoLogo;
                ViewBag.fecha = DateTime.Now.ToString("dd/MM/yyyy");
                ViewBag.tiposala = sala.tipo;
                ViewBag.rucEmpresa = empresa.Ruc;
                ViewBag.logoSala = sala.RutaArchivoLogo;
                ViewBag.logoSala = sala.RutaArchivoLogo != "" ? "https://drive.google.com/uc?id=" + sala.RutaArchivoLogo : "";
                return View("~/Views/FichaSintomatologica/FichaVista.cshtml");
            } else {
                return View("~/Views/FichaSintomatologica/FichaMensaje.cshtml");
            }

        }

        public ActionResult FichaReporteVista() {
            return View();
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult FichaObtenerData(string nro_documento) {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            SEG_EmpleadoEntidad empleadoEntidad = new SEG_EmpleadoEntidad();
            FichaSintomatologicaEntidad ficha = new FichaSintomatologicaEntidad();
            string UriSistemaRRHH = ConfigurationManager.AppSettings["UriSistemaReclutamiento"];
            string url = UriSistemaRRHH + "ofisis/PersonaPorDniFechaCeseV2?dni=" + nro_documento + "&val=true";
            string json = "";
            try {
                empleadoEntidad = _SEG_EmpleadoBL.EmpleadoxNroDocumentoFichaSintomatologicaJson(nro_documento);
                if(empleadoEntidad.EmpleadoID == 0) {
                    using(var client = new HttpClient()) {

                        using(var response = client.GetAsync(url).Result) {
                            if(response.IsSuccessStatusCode) {
                                json = response.Content.ReadAsStringAsync().Result;
                                if(json != "{}") {
                                    dynamic jsonObj = JsonConvert.DeserializeObject(json);
                                    var item = jsonObj.data;
                                    if(item.CESE_ESTADO == 1) {
                                        mensaje = "¡Extrabajador!";
                                        DateTime fechaActual = DateTime.Now;
                                        DateTime fechaCese = Convert.ToDateTime(item.FE_CESE_TRAB);
                                        fechaCese = fechaCese.AddMonths(6);
                                        if(fechaActual > fechaCese) {
                                            respuesta = false;

                                        } else {
                                            empleadoEntidad.Nombres = item.NO_TRAB;
                                            empleadoEntidad.ApellidosPaterno = item.NO_APEL_PATE;
                                            empleadoEntidad.ApellidosMaterno = item.NO_APEL_MATE;
                                            empleadoEntidad.Empresa = item.DE_NOMB;
                                            empleadoEntidad.Ruc = item.NU_RUCS;
                                            empleadoEntidad.DOI = (nro_documento).Trim();
                                            empleadoEntidad.Direccion = item.NO_DIRE_TRAB;
                                            empleadoEntidad.Telefono = item.NU_TLF1;
                                            empleadoEntidad.AreaTrabajo = item.DE_PUES_TRAB;
                                            mensaje = "Registro Extrabajador\n(fecha cese en rango)";
                                            respuesta = true;
                                        }

                                    } else {
                                        empleadoEntidad.Nombres = item.NO_TRAB;
                                        empleadoEntidad.ApellidosPaterno = item.NO_APEL_PATE;
                                        empleadoEntidad.ApellidosMaterno = item.NO_APEL_MATE;
                                        empleadoEntidad.DOI = (nro_documento).Trim();
                                        empleadoEntidad.Direccion = item.NO_DIRE_TRAB;
                                        empleadoEntidad.Telefono = item.NU_TLF1;
                                        empleadoEntidad.Empresa = item.DE_NOMB;
                                        empleadoEntidad.Ruc = item.NU_RUCS;
                                        empleadoEntidad.AreaTrabajo = item.DE_PUES_TRAB;
                                        mensaje = "Registro Empleado";
                                        respuesta = true;

                                    }

                                } else {
                                    respuesta = false;
                                    mensaje = "No se encontro registro de Empleado";

                                }
                            }
                        }
                    }
                }
                if(empleadoEntidad.EmpleadoID > 0) {
                    ficha = fichaSintomatologicaBL.FichaSintomatologicaBuscarIngresoJson(empleadoEntidad.EmpleadoID);
                    respuesta = true;
                }


            } catch(Exception ex) {
                mensajeConsola = ex.Message;
                respuesta = false;
            }

            return Json(new { respuesta, mensaje, mensajeConsola, empleadoEntidad, ficha });
        }


        [HttpPost]
        public ActionResult FichaNuevoJson(FichaSintomatologicaEntidad fichaSintomatologica) {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            Int32 id = 0;
            SalaEntidad sala = new SalaEntidad();
            EmpresaEntidad empresa = new EmpresaEntidad();
            var direccionFoto = ubicacionarchivos + "/firmas/";
            try {
                if(!Directory.Exists(direccionFoto)) {
                    Directory.CreateDirectory(direccionFoto);
                }

                SEG_EmpleadoEntidad empleado = new SEG_EmpleadoEntidad();
                empleado.EmpleadoID = fichaSintomatologica.EmpleadoId;
                empleado.AreaTrabajo = fichaSintomatologica.Area;
                empleado.Nombres = fichaSintomatologica.Nombre;
                empleado.ApellidosPaterno = fichaSintomatologica.ApellidoPaterno;
                empleado.ApellidosMaterno = fichaSintomatologica.ApellidoMaterno;
                empleado.Telefono = fichaSintomatologica.Celular;
                empleado.DOI = fichaSintomatologica.DOI;
                empleado.Direccion = fichaSintomatologica.Direccion;
                empleado.Direccion = fichaSintomatologica.Direccion;
                empleado.AreaTrabajo = fichaSintomatologica.Area;
                empleado.Empresa = fichaSintomatologica.Empresa;
                empleado.Ruc = fichaSintomatologica.RUC;
                if(fichaSintomatologica.EmpleadoId == 0) {
                    id = _SEG_EmpleadoBL.EmpleadoGuardarFichaSintomatologicaJson(empleado);
                    empleado.EmpleadoID = id;
                } else {
                    _SEG_EmpleadoBL.EmpleadoActualizarFichaSintomatologicaJson(empleado);
                }
                //Actualizar area de trabajo del empleado

                fichaSintomatologica.EmpleadoId = empleado.EmpleadoID;
                //Registra ingreso o salida
                if(fichaSintomatologica.FichaId == 0) {
                    fichaSintomatologica.FechaIngreso = DateTime.Now;
                    fichaSintomatologica.Activo = true;
                    fichaSintomatologica.FichaId = fichaSintomatologicaBL.FichaSintomatologicaIngresoInsertaridJson(fichaSintomatologica);

                } else {
                    fichaSintomatologica.FechaSalida = DateTime.Now;
                    fichaSintomatologica.Activo = false;
                    fichaSintomatologicaBL.FichaSintomatologicaSalidaModificarJson(fichaSintomatologica);
                }
                string[] cadena = fichaSintomatologica.Firma.Split(',');
                byte[] imagen = Convert.FromBase64String(cadena[1]);
                Bitmap img = new Bitmap(ConvertByteToImg(imagen));
                img.Save(direccionFoto + "_" + fichaSintomatologica.FichaId + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                img.Dispose();
                fichaSintomatologica.Firma = "_" + fichaSintomatologica.FichaId + ".bmp";
                fichaSintomatologicaBL.FichaSintomatologicaImagenModificarJson(fichaSintomatologica);

                mensaje = "Registrado Correctamente";
                respuesta = true;
            } catch(Exception exp) {
                mensaje = exp.Message + ", Llame Administrador";
                respuesta = false;
            }

            return Json(new { respuesta, mensaje, mensajeConsola });
        }


        [HttpPost]
        public ActionResult FichaSintomatologicaListarxSalaFechaJson(int[] codsala, DateTime fechaini, DateTime fechafin) {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            List<FichaSintomatologicaEntidad> listaFichas = new List<FichaSintomatologicaEntidad>();
            int cantElementos = (codsala == null) ? 0 : codsala.Length;
            var strElementos = String.Empty;
            try {
                if(cantElementos > 0) {
                    strElementos = " ficha.CodSala in(" + "'" + String.Join("','", codsala) + "'" + ") and ";
                }
                var fichaSintomatologicaTupla = fichaSintomatologicaBL.FichaSintomatologicaFiltroListarxSalaFechaJson(strElementos, fechaini, fechafin);
                error = fichaSintomatologicaTupla.error;
                listaFichas = fichaSintomatologicaTupla.fichaSintomatologicasLista;
                if(error.Key.Equals(string.Empty)) {
                    mensaje = "Listando";
                    respuesta = true;
                } else {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar";
                }

            } catch(Exception exp) {
                mensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = listaFichas.ToList(), respuesta, mensaje, mensajeConsola });
        }

        [HttpPost]
        public ActionResult ReporteFichaSintomatologicaDescargarExcelJson(int[] codsala, DateTime fechaini, DateTime fechafin) {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<FichaSintomatologicaEntidad> listaFichas = new List<FichaSintomatologicaEntidad>();
            int cantElementos = (codsala == null) ? 0 : codsala.Length;
            var strElementos = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;

            try {

                if(cantElementos > 0) {
                    strElementos = " ficha.CodSala in(" + "'" + String.Join("','", codsala) + "'" + ") and ";
                }
                var fichaSintomatologicaTupla = fichaSintomatologicaBL.FichaSintomatologicaFiltroListarxSalaFechaJson(strElementos, fechaini, fechafin);
                error = fichaSintomatologicaTupla.error;
                listaFichas = fichaSintomatologicaTupla.fichaSintomatologicasLista;


                if(error.Key.Equals(string.Empty)) {
                    for(int i = 0; i < codsala.Length; i++) {
                        var salat = _salaBl.SalaListaIdJson(codsala[i]);
                        nombresala.Add(salat.Nombre);
                    }
                    salasSeleccionadas = String.Join(",", nombresala);

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Fichas de salud");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;

                    //Header of table  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[2, 2].Value = "SALAS : " + salasSeleccionadas;

                    workSheet.Cells[3, 2].Value = "Id";
                    workSheet.Cells[3, 3].Value = "Fecha Ingreso";
                    workSheet.Cells[3, 4].Value = "Fecha Salida";
                    workSheet.Cells[3, 5].Value = "Empresa";
                    workSheet.Cells[3, 6].Value = "Sala";
                    workSheet.Cells[3, 7].Value = "Nro. Doc.";
                    workSheet.Cells[3, 8].Value = "Nombres";
                    workSheet.Cells[3, 9].Value = "Area de Trabajo";

                    //Body of table  
                    int recordIndex = 4;
                    int total = listaFichas.Count;
                    foreach(var registro in listaFichas) {
                        workSheet.Cells[recordIndex, 2].Value = registro.FichaId;
                        workSheet.Cells[recordIndex, 3].Value = registro.Fecha;
                        workSheet.Cells[recordIndex, 4].Value = registro.FechaSalida.ToString("dd/MM/yyyy hh:mm tt");
                        workSheet.Cells[recordIndex, 5].Value = registro.Empresa;
                        workSheet.Cells[recordIndex, 6].Value = registro.Sala;
                        workSheet.Cells[recordIndex, 7].Value = registro.DOI;
                        workSheet.Cells[recordIndex, 8].Value = registro.Nombre + " " + registro.ApellidoPaterno + " " + registro.ApellidoMaterno;
                        workSheet.Cells[recordIndex, 9].Value = registro.Area;

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

                    int filasagregadas = 3;
                    total = filasagregadas + total;

                    workSheet.Cells["B4:B" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    workSheet.Cells["B2:I2"].Merge = true;
                    workSheet.Cells["B2:I2"].Style.Font.Bold = true;

                    int filaFooter_ = total + 1;

                    workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Merge = true;
                    workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[filaFooter_, 2].Value = "Total : " + (total - filasagregadas) + " Registros";

                    int filaultima = total;
                    workSheet.Cells[3, 2, filaultima, 13].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 25;
                    workSheet.Column(4).Width = 25;
                    workSheet.Column(5).Width = 35;
                    workSheet.Column(6).Width = 35;
                    workSheet.Column(7).Width = 35;
                    workSheet.Column(8).Width = 25;
                    workSheet.Column(9).Width = 25;
                    workSheet.Column(10).Width = 25;
                    excelName = "FichasSintmatologicas_" + fechaini.ToString("dd_MM_yyyy") + "_al_" + fechafin.ToString("dd_MM_yyyy") + "_.xlsx";
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());

                    mensaje = "Descargando Archivo";
                    respuesta = true;
                } else {
                    mensajeConsola = error.Value;
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
        public ActionResult FichaSintomatologicaIdObtenerJson(int id) {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            var imagen = string.Empty;
            var direccionFoto = ubicacionarchivos + "/firmas/";
            FichaSintomatologicaEntidad fichaSintomatologica = new FichaSintomatologicaEntidad();
            SalaEntidad sala = new SalaEntidad();
            EmpresaEntidad empresa = new EmpresaEntidad();
            try {
                if(!Directory.Exists(direccionFoto)) {
                    Directory.CreateDirectory(direccionFoto);
                }
                var fichaSintomatologicaTupla = fichaSintomatologicaBL.FichaSintomatologicaIdObtenerJson(id);
                error = fichaSintomatologicaTupla.error;
                fichaSintomatologica = fichaSintomatologicaTupla.fichaSintomatologica;

                sala = _salaBl.SalaListaIdJson(fichaSintomatologica.CodSala);
                empresa = _empresaBl.EmpresaListaIdJson(sala.CodEmpresa);

                if(fichaSintomatologica.Firma.Trim() != "") {
                    if(System.IO.File.Exists(direccionFoto + "" + fichaSintomatologica.Firma)) {
                        byte[] bmp = ImageToBinary(direccionFoto + "" + fichaSintomatologica.Firma);
                        fichaSintomatologica.Firma = Convert.ToBase64String(bmp);
                    }

                } else {
                    fichaSintomatologica.Firma = "";
                }


                if(error.Key.Equals(string.Empty)) {
                    mensaje = "Obteniendo Información de Registro Seleccionado";
                    respuesta = true;
                } else {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudo Obtener La Información de Registro Seleccionado";
                }

            } catch(Exception exp) {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = fichaSintomatologica, imagen, respuesta, mensaje, mensajeConsola, dataSala = sala, dataEmpresa = empresa });
        }

        public ActionResult FichaSintomatologicaRespuestaPDFDescarga(string doc, bool todo = false) {
            var filename = doc + ".pdf";
            var actionPDF = new Rotativa.ActionAsPdf("GenerateRespuestaPDF_", new { doc = doc, todo = todo }) {
                FileName = filename,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = "--load-error-handling ignore",
                PageMargins = { Top = 15, Left = 10, Right = 10 }
            };

            byte[] applicationPDFData = actionPDF.BuildFile(ControllerContext);

            return actionPDF;
        }


        [seguridad(false)]
        public ActionResult GenerateRespuestaPDF_(string doc, bool todo) {
            var imagen = string.Empty;
            FichaSintomatologicaEntidad fichaSintomatologica = new FichaSintomatologicaEntidad();
            SalaEntidad sala = new SalaEntidad();
            string base64String = string.Empty;
            var direccionFoto = ubicacionarchivos + "/firmas/";
            try {
                if(!Directory.Exists(direccionFoto)) {
                    Directory.CreateDirectory(direccionFoto);
                }
                var fichaSintomatologicaTupla = fichaSintomatologicaBL.FichaSintomatologicaIdObtenerJson(Convert.ToInt32(doc));
                error = fichaSintomatologicaTupla.error;
                fichaSintomatologica = fichaSintomatologicaTupla.fichaSintomatologica;
                if(fichaSintomatologica.Firma.Trim() != "") {
                    if(System.IO.File.Exists(direccionFoto + "" + fichaSintomatologica.Firma)) {
                        byte[] bmp = ImageToBinary(direccionFoto + "" + fichaSintomatologica.Firma);
                        fichaSintomatologica.Firma = Convert.ToBase64String(bmp);
                    }

                } else {
                    fichaSintomatologica.Firma = "";
                }
                sala = _salaBl.SalaListaIdJson(fichaSintomatologica.CodSala);
                var empresa = _empresaBl.EmpresaListaIdJson(sala.CodEmpresa);

                imagen = empresa.RutaArchivoLogo;

                if(imagen == "") {
                    ViewBag.imagen = "";
                } else {
                    ViewBag.imagen = imagen;
                }
                if(error.Key.Equals(string.Empty)) {
                    ViewBag.mensaje = "Obteniendo Información de Registro Seleccionado";
                    ViewBag.respuesta = true;
                    ViewBag.ficha = fichaSintomatologica;
                    ViewBag.sala = sala;
                    ViewBag.empresa = empresa;
                } else {
                    ViewBag.mensajeConsola = error.Value;
                    ViewBag.mensaje = "No se Pudo Obtener La Información de Registro Seleccionado";
                    return View("~/Views/Home/Error.cshtml");
                }

                ViewBag.logoSala = sala.RutaArchivoLogo != "" ? "https://drive.google.com/uc?id=" + sala.RutaArchivoLogo : "";
            } catch(Exception exp) {
                ViewBag.mensaje = exp.Message + ", Llame Administrador";
                return View("~/Views/Home/Error.cshtml");
            }
            return View("~/Views/FichaSintomatologica/FichaPDFVista.cshtml");
        }

        [HttpPost]
        public ActionResult GenerarFisicoFichaSaludPdfJson(int[] codsala, DateTime fechaini, DateTime fechafin) {
            bool respuesta = false;
            ActionAsPdf actionPDF;
            DateTime fechaActual = DateTime.Now;
            string filename = fechaActual.Day + "_" + fechaActual.Month + "_" + fechaActual.Year + "_Fisico_Ficha_Salud" + ".pdf";
            //string reclamaciones = " id in('" + String.Join("','", ArrayReclamacionesId) + "')";

            actionPDF = new ActionAsPdf("PdfFicha_SaludMultiple", new { codsala, fechaini, fechafin }) {
                FileName = filename,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = "--load-error-handling ignore",
                PageMargins = { Top = 15, Left = 10, Right = 10 }
            };

            byte[] applicationPDFData = actionPDF.BuildFile(ControllerContext);
            string file = Convert.ToBase64String(applicationPDFData);
            respuesta = true;

            return Json(new { data = file, filename, respuesta });
        }

        [seguridad(false)]
        public ActionResult PdfFicha_SaludMultiple(int[] codsala, DateTime fechaini, DateTime fechafin) {
            List<FichaSintomatologicaEntidadReporte> listaFichas = new List<FichaSintomatologicaEntidadReporte>();
            int cantElementos = (codsala == null) ? 0 : codsala.Length;
            var strElementos = String.Empty;
            try {
                if(cantElementos > 0) {
                    strElementos = " ficha.CodSala in(" + "'" + String.Join("','", codsala) + "'" + ") and ";
                }
                var fichaSintomatologicaTupla = fichaSintomatologicaBL.FichaSintomatologicaListaIdObtenerJson(strElementos, fechaini, fechafin);
                error = fichaSintomatologicaTupla.error;
                listaFichas = fichaSintomatologicaTupla.fichaSintomatologicasLista;
                foreach(var ficha in listaFichas) {

                    ficha.RutaArchivoLogo = ficha.RutaArchivoLogo != "" ? "https://drive.google.com/uc?id=" + ficha.RutaArchivoLogo : "";
                }
                ViewBag.data = listaFichas;
                ViewBag.respuesta = true;
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                ViewBag.respuesta = false;
                ViewBag.data = null;
            }


            return View("~/Views/FichaSintomatologica/FichaPDFVistaMultiple.cshtml");
        }



        [seguridad(false)]
        public static byte[] ImageToBinary(string imagePath) {
            FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, (int)fileStream.Length);
            fileStream.Close();
            return buffer;
        }

        [seguridad(false)]
        public Image ConvertByteToImg(Byte[] img) {
            //Image FetImg;
            //MemoryStream ms = new MemoryStream(img);
            //FetImg = Image.FromStream(ms);
            //ms.Close();
            //return FetImg;
            using(var stream = new MemoryStream(img)) {
                return Image.FromStream(stream);
            }
        }
    }
}