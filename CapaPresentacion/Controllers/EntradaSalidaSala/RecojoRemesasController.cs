using CapaEntidad.EntradaSalidaSala;
using CapaNegocio;
using CapaNegocio.EntradaSalidaSala;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using OfficeOpenXml.Style;
using S3k.Utilitario.clases_especial;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CapaPresentacion.Controllers.EntradaSalidaSala {
    [seguridad]
    public class RecojoRemesasController : Controller {
        private readonly SalaBL _salaBl = new SalaBL();
        ClaseError error = new ClaseError();
        ESS_RecojoRemesasBL _essRecojoRemesasBL = new ESS_RecojoRemesasBL();

        public ActionResult RecojoRemesasVista() {
            return View("~/Views/EntradaSalidaSala/RecojoRemesas.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarRecojoRemesasxSalaJson(int[] codsala, DateTime fechaini, DateTime fechafin) {
            string mensaje = "No se encontraron registros";
            bool respuesta = false;
            List<ESS_RecojoRemesaEntidad> listaRecojoRemesas = new List<ESS_RecojoRemesaEntidad>();
            try {
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                int[] salasBusqueda;
                if(codsala.Contains(-1)) {
                    salasBusqueda = salas.Select(x => x.CodSala).ToArray();
                } else {
                    salasBusqueda = codsala;
                }
                listaRecojoRemesas = _essRecojoRemesasBL.ListadoRecojoRemesa(salasBusqueda, fechaini, fechafin);
                mensaje = "Registros obtenidos";
                respuesta = true;
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, comunicarse con administrador";
            }
            var oRespuesta = new {
                mensaje,
                respuesta,
                data = listaRecojoRemesas
            };
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var result = new ContentResult {
                Content = serializer.Serialize(oRespuesta),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult GuardarRecojoRemesas(ESS_RecojoRemesaEntidad recojoRemesa) {
            bool respuesta = false;
            string mensaje = "No se pudo guardar los datos";
            try {
                recojoRemesa.FechaRegistro = DateTime.Now;
                recojoRemesa.UsuarioRegistro = (string)Session["UsuarioNombre"];

                int insertedId = _essRecojoRemesasBL.GuardarRegistroRecojoRemesa(recojoRemesa);
                if(insertedId > 0) {
                    respuesta = true;
                    mensaje = "Los datos se han guardado";
                }
            } catch(Exception exception) {
                mensaje = "Ha ocurrido un error, comunicarse con administrador";
            }

            return Json(new {
                respuesta,
                mensaje
            });
        }

        [HttpPost]
        public ActionResult EditarRecojoRemesas(ESS_RecojoRemesaEntidad recojoRemesa) {
            bool respuesta = false;
            string mensaje = "No se pudo actualizar los datos";
            try {
                recojoRemesa.FechaModificacion = DateTime.Now;
                recojoRemesa.UsuarioModificacion = (string)Session["UsuarioNombre"];

                respuesta = _essRecojoRemesasBL.ActualizarRegistroRecojoRemesa(recojoRemesa);
                if(respuesta) {
                    mensaje = "Los datos se han actualizado";
                }
            } catch(Exception exception) {
                mensaje = "Ha ocurrido un error, comunicarse con administrador";
            }

            return Json(new {
                respuesta,
                mensaje
            });
        }

        [HttpPost]
        public JsonResult EliminarRecojoRemesas(int id) {
            bool respuesta = false;
            string mensaje = "No se pudo eliminar el registro.";
            try {
                respuesta = _essRecojoRemesasBL.EliminarRegistroRecojoRemesa(id);
                if(respuesta) {
                    mensaje = "Registro eliminado correctamente.";
                }
            } catch(Exception ex) {
                mensaje = "Error al eliminar el registro: " + ex.Message;
            }

            return Json(new { respuesta, mensaje });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarEstadoFotocheck() {
            string mensaje = "No se encontraron registros";
            bool respuesta = false;
            List<ESS_EstadoFotocheckEntidad> listaEstadoFotocheck = new List<ESS_EstadoFotocheckEntidad>();
            try {
                listaEstadoFotocheck = _essRecojoRemesasBL.ListadoEstadoFotocheck();
                mensaje = "Registros obtenidos";
                respuesta = true;
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, comunicarse con administrador";
            }
            var oRespuesta = new {
                mensaje,
                respuesta,
                data = listaEstadoFotocheck
            };
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var result = new ContentResult {
                Content = serializer.Serialize(oRespuesta),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult ReporteRecojoRemesasDescargarExcelJson(int[] codSala, DateTime fechaIni, DateTime fechaFin) {
            string mensaje = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;

            try {
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                int[] salasBusqueda;
                if(codSala.Contains(-1)) {
                    salasBusqueda = salas.Select(x => x.CodSala).ToArray();
                } else {
                    salasBusqueda = codSala;
                }
                List<ESS_RecojoRemesaEntidad> lista = _essRecojoRemesasBL.ListadoRecojoRemesa(salasBusqueda, fechaIni, fechaFin);
                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using(ExcelPackage excel = new ExcelPackage()) {

                        var workSheet = excel.Workbook.Worksheets.Add("Recojo de Remesas");
                        workSheet.TabColor = Color.Black;
                        workSheet.DefaultRowHeight = 12;

                        // Encabezado principal
                        workSheet.Row(3).Height = 20;
                        workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet.Row(3).Style.Font.Bold = true;
                        workSheet.Cells[2, 1, 2, 10].Merge = true;
                        workSheet.Cells[2, 1].Value = "Reporte de Recojo de Remesas";
                        workSheet.Cells[2, 1].Style.Font.Bold = true;
                        workSheet.Cells[2, 1].Style.Font.Size = 13;
                        workSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;



                        workSheet.Cells[3, 1].Value = "ID";
                        workSheet.Cells[3, 2].Value = "Sala";
                        workSheet.Cells[3, 3].Value = "Personal";
                        workSheet.Cells[3, 4].Value = "DNI/C.E.";
                        workSheet.Cells[3, 5].Value = "Codigo Personal";
                        workSheet.Cells[3, 6].Value = "Estado Fotocheck";
                        workSheet.Cells[3, 7].Value = "Placa/Rodaje";
                        workSheet.Cells[3, 8].Value = "Fecha Ingreso";
                        workSheet.Cells[3, 9].Value = "Fecha Salida";
                        workSheet.Cells[3, 10].Value = "Observaciones";
                        workSheet.Column(10).Width = 80;

                        int rowIndex = 4;
                        foreach(var item in lista) {
                            workSheet.Cells[rowIndex, 1].Value = item.IdRecojoRemesa;
                            workSheet.Cells[rowIndex, 2].Value = item.Sala;
                            workSheet.Cells[rowIndex, 3].Value = item.NombreCompletoPersonal;
                            workSheet.Cells[rowIndex, 4].Value = item.TipoDocumentoRegistro + ' ' + item.DocumentoRegistro;
                            workSheet.Cells[rowIndex, 5].Value = item.CodigoPersonal;
                            workSheet.Cells[rowIndex, 6].Value = item.IdEstadoFotocheck == -1 ? "OTROS (" + item.OtroEstadoFotocheck + ")" : item.EstadoFotocheck;

                            workSheet.Cells[rowIndex, 7].Value = item.PlacaRodaje;
                            workSheet.Cells[rowIndex, 8].Value = item.FechaIngreso.ToString("dd-MM-yyyy") == "01-01-1753" ? "No definido" : item.FechaIngreso.ToString("dd-MM-yyyy hh:mm tt");
                            workSheet.Cells[rowIndex, 9].Value = item.FechaSalida.ToString("dd-MM-yyyy") == "01-01-1753" ? "No definido" : item.FechaSalida.ToString("dd-MM-yyyy hh:mm tt");
                            workSheet.Cells[rowIndex, 10].Value = item.Observaciones;
                            rowIndex++;
                        }

                        Color principalHeaderBackground = ColorTranslator.FromHtml("#003268");
                        workSheet.Cells["A3:J3"].Style.Font.Bold = true;
                        workSheet.Cells["A3:J3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells["A3:J3"].Style.Fill.BackgroundColor.SetColor(principalHeaderBackground);
                        workSheet.Cells["A3:J3"].Style.Font.Color.SetColor(Color.White);

                        for(int i = 1; i <= 10; i++) {
                            workSheet.Column(i).AutoFit();
                        }
                        excelName = $"ReporteRecojoRemesas_{fechaIni:dd_MM_yyyy}_al_{fechaFin:dd_MM_yyyy}.xlsx";


                        using(var memoryStream = new MemoryStream()) {
                            excel.SaveAs(memoryStream);
                            base64String = Convert.ToBase64String(memoryStream.ToArray());
                        }

                        mensaje = "Archivo generado correctamente";
                        respuesta = true;
                    }
                } else {
                    mensaje = "No se encontraron datos para el reporte.";
                }
            } catch(Exception ex) {
                mensaje = $"Error al generar el archivo: {ex.Message}";
                respuesta = false;
            }


            return Json(new { data = base64String, excelName, respuesta, mensaje });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarRecojoRemesaPersonal() {
            string mensaje = "No se encontraron registros";
            bool respuesta = false;
            List<ESS_RecojoRemesaPersonalEntidad> listaRecojoRemesasPersonal = new List<ESS_RecojoRemesaPersonalEntidad>();
            try {
                listaRecojoRemesasPersonal = _essRecojoRemesasBL.ListadoRecojoRemesaPersonal();
                mensaje = "Registros obtenidos";
                respuesta = true;
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, comunicarse con administrador";
            }
            var oRespuesta = new {
                mensaje,
                respuesta,
                data = listaRecojoRemesasPersonal
            };
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var result = new ContentResult {
                Content = serializer.Serialize(oRespuesta),
                ContentType = "application/json"
            };
            return result;
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult GuardarRecojoRemesasPersonal(ESS_RecojoRemesaPersonalEntidad personal) {
            bool respuesta = false;
            int _id = 0;
            string mensaje = "No se pudo guardar los datos";
            try {
                personal.FechaRegistro = DateTime.Now;
                personal.UsuarioRegistro = (string)Session["UsuarioNombre"];
                int ExistePersonal = _essRecojoRemesasBL.ExisteRegistroPersonal(personal.IdTipoDocumentoRegistro, personal.DocumentoRegistro);
                if(ExistePersonal <= 0) {
                    int insertedId = _essRecojoRemesasBL.GuardarRegistroRecojoRemesaPersonal(personal);
                    if(insertedId > 0) {
                        respuesta = true;
                        _id = insertedId;
                        mensaje = "Los datos se han guardado";
                    }
                } else {
                    respuesta = false;
                    mensaje = "Usuario ya registrado";
                }

            } catch(Exception exception) {
                mensaje = "Ha ocurrido un error, comunicarse con administrador";
            }

            return Json(new {
                respuesta,
                mensaje,
                _id

            });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult PlantillaRecojoRemesasDescargarExcel() {
            string path = Server.MapPath(@"~/Content/ess_excel_plantilla/Plantilla_ESS_RecojoRemesas.xlsx"); ;
            string base64String = "";
            string excelName = "";
            string mensaje = "";
            bool respuesta = false;
            try {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                var estadofotochecks = _essRecojoRemesasBL.ListadoEstadoFotocheck();

                List<string> listaDatosSala = new List<string>();
                List<string> listaDatosEstadoFotocheck = new List<string>();

                listaDatosSala = salas.Select(x => $"{x.CodSala}-{x.Nombre}").ToList();
                listaDatosEstadoFotocheck = estadofotochecks.Select(x => $"{x.IdEstadoFotocheck}-{x.Nombre}").ToList();

                // Abrir el archivo existente
                using(var package = new ExcelPackage(new FileInfo(path))) {
                    var workSheet = package.Workbook.Worksheets[0]; // Seleccionar la primera hoja

                    // Crear campos con validacion de datos
                    var validationSala = workSheet.DataValidations.AddListValidation("B5:B30");
                    validationSala.ShowErrorMessage = true;
                    validationSala.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    validationSala.ErrorTitle = "Valor inválido";
                    validationSala.Error = "Seleccione un valor de la lista.";

                    var validationEstadoFotocheck = workSheet.DataValidations.AddListValidation("E5:E30");
                    validationEstadoFotocheck.ShowErrorMessage = true;
                    validationEstadoFotocheck.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    validationEstadoFotocheck.ErrorTitle = "Valor inválido";
                    validationEstadoFotocheck.Error = "Seleccione un valor de la lista.";


                    // Crear lista
                    int startRowSala = 2;
                    for(int i = 0; i < listaDatosSala.Count; i++) {
                        workSheet.Cells[startRowSala + i, 26].Value = listaDatosSala[i];
                    }
                    int endRowSala = startRowSala + listaDatosSala.Count - 1;
                    validationSala.Formula.ExcelFormula = $"$Z${startRowSala}:$Z${endRowSala}";
                    workSheet.Column(26).Hidden = true;

                    int startRowDispositivo = 2;
                    for(int i = 0; i < listaDatosEstadoFotocheck.Count; i++) {
                        workSheet.Cells[startRowDispositivo + i, 27].Value = listaDatosEstadoFotocheck[i];
                    }
                    int endRowDispositivo = startRowDispositivo + listaDatosEstadoFotocheck.Count - 1;
                    validationEstadoFotocheck.Formula.ExcelFormula = $"$AA${startRowDispositivo}:$AA${endRowDispositivo}";
                    workSheet.Column(27).Hidden = true;

                    // Fin de la Validacion

                    byte[] bin = package.GetAsByteArray();
                    base64String = Convert.ToBase64String(bin);
                    excelName = "Plantilla_RecojoRemesas_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx";
                    respuesta = true;
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }

            return Json(new { respuesta, mensaje, data = base64String, excelName });
        }

        [HttpPost]
        public ActionResult ImportarExcelRecojoRemesas() {
            var observaciones = new List<object>();
            string mensaje = "";
            bool respuesta = false;

            try {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                HttpPostedFileBase archivoExcel = Request.Files["file"];
                string nombreOriginal = Request.Form["fileName"];

                // Validar archivo
                if(archivoExcel == null || archivoExcel.ContentLength == 0) {
                    mensaje = "El archivo no es válido.";
                    return Json(new { respuesta = false, mensaje });
                }
                List<ESS_RecojoRemesaPersonalEntidad> listaRecojoRemesasPersonal = new List<ESS_RecojoRemesaPersonalEntidad>();
                listaRecojoRemesasPersonal = _essRecojoRemesasBL.ListadoRecojoRemesaPersonal();

                string fechaHora = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string nombreProcesado = $"Procesado_{fechaHora}_{nombreOriginal}";
                using(var stream = archivoExcel.InputStream) {
                    using(var package = new ExcelPackage(stream)) {
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                        if(worksheet == null)
                            return Json(new { respuesta = false, mensaje = "El archivo no contiene hojas válidas." });

                        int ColumnaObservacionesImportar = 11;
                        //Eliminar la columna de observaciones
                        worksheet.DeleteColumn(ColumnaObservacionesImportar);
                        //Agregar una columna despues de observaciones
                        worksheet.InsertColumn(ColumnaObservacionesImportar, 1);

                        // Agregar y formatear la columna de observaciones
                        worksheet.Cells[3, ColumnaObservacionesImportar, 4, ColumnaObservacionesImportar].Merge = true;
                        worksheet.Cells[3, ColumnaObservacionesImportar].Value = "Observación de la importación";
                        worksheet.Cells[3, ColumnaObservacionesImportar].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[3, ColumnaObservacionesImportar].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 192, 0));
                        worksheet.Cells[3, ColumnaObservacionesImportar].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[3, ColumnaObservacionesImportar].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        worksheet.Cells[3, ColumnaObservacionesImportar].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        worksheet.Cells[4, ColumnaObservacionesImportar].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        //modelTable.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        for(int row = 5; row <= 30; row++) {
                            bool filaVacia = true;
                            for(int col = 2; col <= 10; col++) {
                                var valorCelda = worksheet.Cells[row, col].Value?.ToString().Trim();

                                if(!string.IsNullOrEmpty(valorCelda)) {
                                    filaVacia = false;
                                    break;
                                }
                            }

                            if(filaVacia) {
                                continue;
                            }

                            var observacionFila = new List<string>();

                            string campoSala = worksheet.Cells[row, 2].Text.Trim(); // Columna B
                            string campoPersonal = worksheet.Cells[row, 3].Text.Trim(); // Columna C
                            string campoPlacaRodaje = worksheet.Cells[row, 4].Text.Trim(); // Columna D
                            string campoEstadoFotocheck = worksheet.Cells[row, 5].Text.Trim(); // Columna E
                            string campoFechaIngreso = worksheet.Cells[row, 6].Text.Trim(); // Columna F
                            string campoHoraIngreso = worksheet.Cells[row, 7].Text.Trim(); // Columna G
                            string campoFechaSalida = worksheet.Cells[row, 8].Text.Trim(); // Columna H
                            string campoHoraSalida = worksheet.Cells[row, 9].Text.Trim(); // Columna I
                            string campoObservaciones = worksheet.Cells[row, 10].Text.Trim(); // Columna J

                            // Validaciones
                            if(string.IsNullOrEmpty(campoSala)) {
                                observacionFila.Add("El campo Sala es obligatorio.");
                                ResaltarCelda(worksheet, row, 2); // Resaltar celda B
                            }
                            if(string.IsNullOrEmpty(campoPersonal)) {
                                observacionFila.Add("El campo Personal es obligatorio.");
                                ResaltarCelda(worksheet, row, 3); // Resaltar celda C
                            }
                            if(string.IsNullOrEmpty(campoPlacaRodaje)) {
                                observacionFila.Add("El campo Placa/Rodaje es obligatorio.");
                                ResaltarCelda(worksheet, row, 4); // Resaltar celda D
                            }
                            if(string.IsNullOrEmpty(campoEstadoFotocheck)) {
                                observacionFila.Add("El campo Estado Fotocheck es obligatorio.");
                                ResaltarCelda(worksheet, row, 5); // Resaltar celda E
                            }
                            if(string.IsNullOrEmpty(campoFechaIngreso)) {
                                observacionFila.Add("El campo Fecha de Ingreso es obligatorio.");
                                ResaltarCelda(worksheet, row, 6); // Resaltar celda F
                            } else if(!DateTime.TryParse(campoFechaIngreso, out DateTime fechaIngreso)) {
                                observacionFila.Add("El campo Fecha de Ingreso no tiene un formato válido.");
                                ResaltarCelda(worksheet, row, 6); // Resaltar celda F
                            }
                            if(string.IsNullOrEmpty(campoHoraIngreso)) {
                                observacionFila.Add("El campo Hora de Ingreso es obligatorio.");
                                ResaltarCelda(worksheet, row, 7); // Resaltar celda 
                            } else if(!TimeSpan.TryParse(campoHoraIngreso, out TimeSpan horaIngreso)) {
                                observacionFila.Add("El campo Hora de Ingreso no tiene un formato válido.");
                                ResaltarCelda(worksheet, row, 7); // Resaltar celda 
                            }
                            if(string.IsNullOrEmpty(campoFechaSalida)) {
                                observacionFila.Add("El campo Fecha de Salida es obligatorio.");
                                ResaltarCelda(worksheet, row, 8); // Resaltar celda
                            } else if(!DateTime.TryParse(campoFechaSalida, out DateTime fechaSalida)) {
                                observacionFila.Add("El campo Fecha de Ingreso no tiene un formato válido.");
                                ResaltarCelda(worksheet, row, 8); // Resaltar celda
                            }

                            if(string.IsNullOrEmpty(campoHoraSalida)) {
                                observacionFila.Add("El campo Hora de Salida es obligatorio.");
                                ResaltarCelda(worksheet, row, 9); // Resaltar celda
                            } else if(!TimeSpan.TryParse(campoHoraSalida, out TimeSpan horaSalida)) {
                                observacionFila.Add("El campo Hora de Salida no tiene un formato válido.");
                            }
                            //if (string.IsNullOrEmpty(campoObservaciones))
                            //{
                            //    observacionFila.Add("El campo Observaciones es obligatorio.");
                            //}

                            //Todo OK
                            if(!observacionFila.Any()) {
                                try {
                                    var registro = new ESS_RecojoRemesaEntidad {
                                        CodSala = int.Parse(campoSala.Split('-')[0]),
                                        Sala = campoSala.Split('-')[1],
                                        NombreCompletoPersonal = campoPersonal,
                                        PlacaRodaje = campoPlacaRodaje,
                                        IdEstadoFotocheck = int.Parse(campoEstadoFotocheck.Split('-')[0]),
                                        EstadoFotocheck = campoEstadoFotocheck.Split('-')[1],
                                        FechaIngreso = DateTime.Parse(DateTime.Parse(campoFechaIngreso).ToString("yyyy-MM-dd"))
                                                        + TimeSpan.Parse(DateTime.Parse(campoHoraIngreso).ToString("HH:mm:ss")),
                                        FechaSalida = DateTime.Parse(DateTime.Parse(campoFechaSalida).ToString("yyyy-MM-dd"))
                                        + TimeSpan.Parse(DateTime.Parse(campoHoraSalida).ToString("HH:mm:ss")),
                                        Observaciones = campoObservaciones,
                                        FechaRegistro = DateTime.Now,
                                        UsuarioRegistro = (string)Session["UsuarioNombre"],
                                    };

                                    var PersonalExiste = listaRecojoRemesasPersonal.Where(x => x.DocumentoRegistro == campoPersonal.Trim() || x.CodigoPersonal == campoPersonal.Trim()).FirstOrDefault();
                                    if(PersonalExiste == null) {
                                        string msg = "No se pudo encontrar al personal Autoriza.";
                                        observaciones.Add(new { Fila = row, Observaciones = msg });

                                        var richText = worksheet.Cells[row, ColumnaObservacionesImportar].RichText;
                                        var prefijo = richText.Add("❌ ");
                                        prefijo.Color = System.Drawing.Color.Red;
                                        var mensajetexto = richText.Add(msg);
                                        mensajetexto.Color = System.Drawing.Color.Black;
                                    } else {
                                        registro.IdPersonal = PersonalExiste.IdRecojoRemesaPersonal;

                                        int insertedId = _essRecojoRemesasBL.GuardarRegistroRecojoRemesa(registro);
                                        if(insertedId > 0) {
                                            string msg = "Registro guardado con éxito.";
                                            observaciones.Add(new { Fila = row, Observaciones = msg });

                                            var richText = worksheet.Cells[row, ColumnaObservacionesImportar].RichText;
                                            var prefijo = richText.Add("✔ ");
                                            prefijo.Color = System.Drawing.Color.Green;
                                            var mensajetexto = richText.Add(msg);
                                            mensajetexto.Color = System.Drawing.Color.Black;
                                        } else {
                                            string msg = "No se pudo guardar el registro.";
                                            observaciones.Add(new { Fila = row, Observaciones = msg });

                                            var richText = worksheet.Cells[row, ColumnaObservacionesImportar].RichText;
                                            var prefijo = richText.Add("❌ ");
                                            prefijo.Color = System.Drawing.Color.Red;
                                            var mensajetexto = richText.Add(msg);
                                            mensajetexto.Color = System.Drawing.Color.Black;
                                        }
                                    }

                                } catch(Exception ex) {
                                    string msg = "No se pudo insertar este registro debido a un error en la conversión de datos.";
                                    observaciones.Add(new { Fila = row, Observaciones = msg });

                                    var richText = worksheet.Cells[row, ColumnaObservacionesImportar].RichText;
                                    var prefijo = richText.Add("❌ ");
                                    prefijo.Color = System.Drawing.Color.Red;
                                    var mensajetexto = richText.Add(msg);
                                    mensajetexto.Color = System.Drawing.Color.Black;

                                }
                            } else {
                                string obsMsg = string.Join(" | ", observacionFila);
                                observaciones.Add(new { Fila = row, Observaciones = obsMsg });

                                var richText = worksheet.Cells[row, ColumnaObservacionesImportar].RichText;
                                var prefijo = richText.Add("❌ ");
                                prefijo.Color = System.Drawing.Color.Red;
                                var mensajetexto = richText.Add(obsMsg);
                                mensajetexto.Color = System.Drawing.Color.Black;
                            }
                        }

                        worksheet.Column(ColumnaObservacionesImportar).AutoFit();
                        worksheet.Column(ColumnaObservacionesImportar).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        // Guardar el archivo Excel modificado
                        using(var modifiedStream = new MemoryStream()) {
                            package.SaveAs(modifiedStream);
                            var base64Excel = Convert.ToBase64String(modifiedStream.ToArray());

                            respuesta = true;
                            return Json(new {
                                respuesta,
                                mensaje = "Archivo procesado con éxito.",
                                observaciones,
                                excelModificado = base64Excel,
                                nombreArchivo = nombreProcesado
                            });
                        }
                    }
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }

            return Json(new { respuesta, mensaje, observaciones });
        }

        private void ResaltarCelda(ExcelWorksheet worksheet, int row, int col) {
            var cell = worksheet.Cells[row, col];

            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 246, 221)); // Amarillo pálido

            cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }

    }
}
