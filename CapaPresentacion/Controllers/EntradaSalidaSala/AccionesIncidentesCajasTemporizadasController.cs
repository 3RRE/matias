

using CapaEntidad.EntradaSalidaSala;
using CapaNegocio;
using CapaNegocio.EntradaSalidaSala;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using OfficeOpenXml.Style;
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
    public class AccionesIncidentesCajasTemporizadasController : Controller {
        private readonly ESS_AccionCajaTemporizadaBL _essAccionCajaTemporizadaBL = new ESS_AccionCajaTemporizadaBL();
        private readonly ESS_DeficienciaBL _essDeficienciaBL = new ESS_DeficienciaBL();
        private readonly ESS_DispositivoBL _essDispositivoBL = new ESS_DispositivoBL();
        private readonly SalaBL _salaBl = new SalaBL();


        public ActionResult AccionesIncidentesCajasTemporizadasVista() {
            return View("~/Views/EntradaSalidaSala/AccionesIncidentesCajasTemporizadas.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult AccionCajaTemporizadaListarxSalaFechaJson(int[] codsala, DateTime fechaini, DateTime fechafin) {
            string mensaje = "No se encontraton registros";
            bool respuesta = false;
            List<ESS_AccionCajaTemporizadaEntidad> listaESS_AccionCajaTemporizada = new List<ESS_AccionCajaTemporizadaEntidad>();
            try {
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                int[] salasBusqueda;
                if(codsala.Contains(-1)) {
                    salasBusqueda = salas.Select(x => x.CodSala).ToArray();
                } else {
                    salasBusqueda = codsala;
                }
                listaESS_AccionCajaTemporizada = _essAccionCajaTemporizadaBL.ListadoAccionCajaTemporizada(salasBusqueda, fechaini, fechafin);
                mensaje = "Registros obtenidos";
                respuesta = true;
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, comunicarse con administrador";
            }
            var oRespuesta = new {
                mensaje,
                respuesta,
                data = listaESS_AccionCajaTemporizada
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
        public ActionResult GuardarAccionesIncidentesCajasTemporizadas(ESS_AccionCajaTemporizadaEntidad accioncajatemporizada) {
            bool respuesta = false;
            string mensaje = "No se pudo guardar los datos";

            try {
                accioncajatemporizada.FechaRegistro = DateTime.Now;
                //accioncajatemporizada.Fecha = DateTime.Now;
                accioncajatemporizada.UsuarioRegistro = (string)Session["UsuarioNombre"];

                //List<ESS_AccionCajaTemporizadaEmpleadoEntidad> listaEmpleados = JsonConvert.DeserializeObject<List<ESS_AccionCajaTemporizadaEmpleadoEntidad>>(Empleados);
                //accioncajatemporizada.Empleados = listaEmpleados;


                int insertedId = _essAccionCajaTemporizadaBL.GuardarAccionCajaTemporizada(accioncajatemporizada);
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
        public ActionResult EditarAccionesIncidentesCajasTemporizadas(ESS_AccionCajaTemporizadaEntidad accioncajatemporizada) {
            bool respuesta = false;
            string mensaje = "No se pudo actualizar los datos";

            try {
                //List<ESS_AccionCajaTemporizadaEmpleadoEntidad> listaEmpleados = JsonConvert.DeserializeObject<List<ESS_AccionCajaTemporizadaEmpleadoEntidad>>(Empleados);
                //accioncajatemporizada.Empleados = listaEmpleados;
                accioncajatemporizada.FechaModificacion = DateTime.Now;
                accioncajatemporizada.UsuarioModificacion = (string)Session["UsuarioNombre"];

                respuesta = _essAccionCajaTemporizadaBL.ActualizarAccionCajaTemporizada(accioncajatemporizada);
                if(respuesta) {
                    respuesta = true;
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
        public JsonResult EliminarAccionesIncidentesCajasTemporizadas(int id) {
            bool respuesta = false;
            string mensaje = "No se pudo eliminar el registro.";

            try {
                respuesta = _essAccionCajaTemporizadaBL.EliminarAccionCajaTemporizada(id);
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
        public ActionResult FinalizarHoraRegistroAccionCajaTemporizada(int idaccioncajatemporizada, string horaSalida) {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try {
                DateTime horaFinalizada;
                if(DateTime.TryParse(horaSalida, out horaFinalizada)) {
                    respuestaConsulta = _essAccionCajaTemporizadaBL.FinalizarHoraRegistroAccionCajaTemporizada(idaccioncajatemporizada, horaFinalizada);
                    if(respuestaConsulta) {
                        errormensaje = "Registro finalizado";
                    } else {
                        errormensaje = "Error al actualizar bien material, llame al administrador";
                        respuestaConsulta = false;
                    }
                } else {
                    errormensaje = "La hora ingresada no es válida.";
                    respuestaConsulta = false;
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarDeficienciaPorEstado(int estado = 1) {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_DeficienciaEntidad>();
            try {

                data = _essDeficienciaBL.ListarDeficienciaPorEstado(estado);
                mensaje = "Listando registros";
                respuesta = true;
                return Json(new { mensaje, respuesta, data });
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, contacte con administrador";
                respuesta = false;
                return Json(new { mensaje, respuesta });
            }
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarDispositivoPorEstado(int estado = 1) {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_DispositivoEntidad>();
            try {

                data = _essDispositivoBL.ListarDispositivoPorEstado(estado);
                mensaje = "Listando registros";
                respuesta = true;
                return Json(new { mensaje, respuesta, data });
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, contacte con administrador";
                respuesta = false;
                return Json(new { mensaje, respuesta });
            }
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarEmpleadosGerentesYJefes() {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_AccionCajaTemporizadaCargoEntidad>();

            try {
                data = _essAccionCajaTemporizadaBL.ListarEmpleadosGerentesYJefes();
                mensaje = "Listando registros";
                respuesta = true;
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, contacte con administrador";
                respuesta = false;
            }

            return Json(new { mensaje, respuesta, data });
        }

        [HttpPost]
        public ActionResult ReporteAccionesIncidentesCajasTemporizadasExcelJson(int[] codsala, DateTime fechaini, DateTime fechafin) {
            string mensaje = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;



            try {

                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                int[] salasBusqueda;
                if(codsala.Contains(-1)) {
                    salasBusqueda = salas.Select(x => x.CodSala).ToArray();
                } else {
                    salasBusqueda = codsala;
                }

                List<ESS_AccionCajaTemporizadaEntidad> lista = _essAccionCajaTemporizadaBL.ListadoAccionCajaTemporizada(salasBusqueda, fechaini, fechafin);

                lista = lista.OrderByDescending(x => x.Fecha).ToList();

                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();


                    var workSheet = excel.Workbook.Worksheets.Add("Acciones Incidentes de Cajas Temporizadas");
                    workSheet.TabColor = Color.Black;
                    workSheet.DefaultRowHeight = 9;


                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[2, 2, 2, 11].Merge = true;
                    workSheet.Cells[2, 2].Value = "Reporte de Acciones Incidentes de Cajas Temporizadas";
                    workSheet.Cells[2, 2].Style.Font.Bold = true;
                    workSheet.Cells[2, 2].Style.Font.Size = 13;
                    workSheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Sala";
                    workSheet.Cells[3, 4].Value = "Fecha";
                    workSheet.Cells[3, 5].Value = "Autoriza";
                    workSheet.Cells[3, 6].Value = "Dispositivos";
                    workSheet.Cells[3, 7].Value = "Deficiencias";
                    workSheet.Cells[3, 8].Value = "Medidas Adoptadas";
                    workSheet.Cells[3, 9].Value = "FechaSolucion";

                    int recordIndex = 4;
                    foreach(var item in lista) {

                        workSheet.Cells[recordIndex, 2].Value = item.IdAccionCajaTemporizada;
                        workSheet.Cells[recordIndex, 3].Value = item.NombreSala;
                        workSheet.Cells[recordIndex, 4].Value = item.Fecha.ToString("dd-MM-yyyy hh:mm tt");
                        workSheet.Cells[recordIndex, 5].Value = item.NombreAutoriza;
                        //workSheet.Cells[recordIndex, 6].Value = item.NombreDispositivo;
                        workSheet.Cells[recordIndex, 7].Value = item.NombreDeficiencia;
                        workSheet.Cells[recordIndex, 8].Value = string.IsNullOrEmpty(item.MedidaAdoptada) ? "No definido" : item.MedidaAdoptada;
                        workSheet.Column(8).Width = 30;

                        workSheet.Cells[recordIndex, 9].Value = (item.FechaSolucion ?? DateTime.MinValue).ToString("dd-MM-yyyy") == "01-01-1753" ? "No Definido" : (item.FechaSolucion ?? DateTime.MinValue).ToString("dd-MM-yyyy hh:mm tt");

                        if(item.NombreDispositivo == "OTROS") {
                            workSheet.Cells[recordIndex, 6].Value = $"{item.NombreDispositivo} ({item.DescripcionDispositivo})";
                        } else {
                            workSheet.Cells[recordIndex, 6].Value = item.NombreDispositivo;
                        }
                        workSheet.Column(6).Width = 30;

                        recordIndex++;
                    }

                    Color principalHeaderBackground = ColorTranslator.FromHtml("#003268");
                    workSheet.Cells["B3:I3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:I3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:I3"].Style.Fill.BackgroundColor.SetColor(principalHeaderBackground);
                    workSheet.Cells["B3:I3"].Style.Font.Color.SetColor(Color.White);

                    for(int i = 2; i <= 11; i++) {
                        if(i != 8 && i != 6) {
                            workSheet.Column(i).AutoFit();
                        }
                    }

                    // Nombre del archivo
                    excelName = $"ReporteAccionCajaTemporizada_{fechaini:dd_MM_yyyy}_al_{fechafin:dd_MM_yyyy}.xlsx";

                    using(var memoryStream = new MemoryStream()) {
                        excel.SaveAs(memoryStream);
                        base64String = Convert.ToBase64String(memoryStream.ToArray());
                    }

                    mensaje = "Archivo generado correctamente";
                    respuesta = true;
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
        public ActionResult PlantillaAccionesIncidentesCajasTemporizadasDescargarExcel() {
            string path = Server.MapPath(@"~/Content/ess_excel_plantilla/Plantilla_ESS_AccionesIncidentesCajasTemporizadas.xlsx"); ;
            string base64String = "";
            string excelName = "";
            string mensaje = "";
            bool respuesta = false;
            try {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                var dispositivos = _essDispositivoBL.ListarDispositivoPorEstado(1);
                var deficiencias = _essDeficienciaBL.ListarDeficienciaPorEstado(1);

                List<string> listaDatosSala = new List<string>();
                List<string> listaDatosDispositivo = new List<string>();
                List<string> listaDatosDeficiencia = new List<string>();

                listaDatosSala = salas.Select(x => $"{x.CodSala}-{x.Nombre}").ToList();
                listaDatosDispositivo = dispositivos.Select(x => $"{x.IdDispositivo}-{x.Nombre}").ToList();
                listaDatosDeficiencia = deficiencias.Select(x => $"{x.IdDeficiencia}-{x.Nombre}").ToList();

                // Abrir el archivo existente
                using(var package = new ExcelPackage(new FileInfo(path))) {
                    var workSheet = package.Workbook.Worksheets[0]; // Seleccionar la primera hoja

                    // Crear campos con validacion de datos
                    var validationSala = workSheet.DataValidations.AddListValidation("B5:B30");
                    validationSala.ShowErrorMessage = true;
                    validationSala.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    validationSala.ErrorTitle = "Valor inválido";
                    validationSala.Error = "Seleccione un valor de la lista.";

                    var validationDispositivo = workSheet.DataValidations.AddListValidation("H5:H30");
                    validationDispositivo.ShowErrorMessage = true;
                    validationDispositivo.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    validationDispositivo.ErrorTitle = "Valor inválido";
                    validationDispositivo.Error = "Seleccione un valor de la lista.";

                    var validationDeficiencia = workSheet.DataValidations.AddListValidation("I5:I30");
                    validationDeficiencia.ShowErrorMessage = true;
                    validationDeficiencia.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    validationDeficiencia.ErrorTitle = "Valor inválido";
                    validationDeficiencia.Error = "Seleccione un valor de la lista.";

                    // Crear lista
                    int startRowSala = 2;
                    for(int i = 0; i < listaDatosSala.Count; i++) {
                        workSheet.Cells[startRowSala + i, 26].Value = listaDatosSala[i];
                    }
                    int endRowSala = startRowSala + listaDatosSala.Count - 1;
                    validationSala.Formula.ExcelFormula = $"$Z${startRowSala}:$Z${endRowSala}";
                    workSheet.Column(26).Hidden = true;

                    int startRowDispositivo = 2;
                    for(int i = 0; i < listaDatosDispositivo.Count; i++) {
                        workSheet.Cells[startRowDispositivo + i, 27].Value = listaDatosDispositivo[i];
                    }
                    int endRowDispositivo = startRowDispositivo + listaDatosDispositivo.Count - 1;
                    validationDispositivo.Formula.ExcelFormula = $"$AA${startRowDispositivo}:$AA${endRowDispositivo}";
                    workSheet.Column(27).Hidden = true;

                    int startRowDeficiencia = 2;
                    for(int i = 0; i < listaDatosDeficiencia.Count; i++) {
                        workSheet.Cells[startRowDeficiencia + i, 28].Value = listaDatosDeficiencia[i];
                    }
                    int endRowDeficiencia = startRowDeficiencia + listaDatosDeficiencia.Count - 1;
                    validationDeficiencia.Formula.ExcelFormula = $"$AB${startRowDeficiencia}:$AB${endRowDeficiencia}";
                    workSheet.Column(28).Hidden = true;

                    // Fin de la Validacion

                    byte[] bin = package.GetAsByteArray();
                    base64String = Convert.ToBase64String(bin);
                    excelName = "Plantilla_AccionIncidenteCajaTemporizada_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx";
                    respuesta = true;
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }

            return Json(new { respuesta, mensaje, data = base64String, excelName });
        }

        [HttpPost]
        public ActionResult ImportarExcelAccionesIncidentesCajasTemporizadas() {
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
                            string campoPersonaAutoriza = worksheet.Cells[row, 3].Text.Trim(); // Columna C
                            string campoFechaIngreso = worksheet.Cells[row, 4].Text.Trim(); // Columna D
                            string campoHoraIngreso = worksheet.Cells[row, 5].Text.Trim(); // Columna E
                            string campoFechaSalida = worksheet.Cells[row, 6].Text.Trim(); // Columna F
                            string campoHoraSalida = worksheet.Cells[row, 7].Text.Trim(); // Columna G
                            string campoDispositivo = worksheet.Cells[row, 8].Text.Trim(); // Columna H
                            string campoDeficiencia = worksheet.Cells[row, 9].Text.Trim(); // Columna I
                            string campoMedidaAdoptada = worksheet.Cells[row, 10].Text.Trim(); // Columna J


                            // Validaciones
                            if(string.IsNullOrEmpty(campoSala)) {
                                observacionFila.Add("El campo Sala es obligatorio.");
                                ResaltarCelda(worksheet, row, 2); // Resaltar celda B
                            }
                            if(string.IsNullOrEmpty(campoPersonaAutoriza)) {
                                observacionFila.Add("El campo Persona Autoriza es obligatorio.");
                                ResaltarCelda(worksheet, row, 3); // Resaltar celda C
                            }
                            if(string.IsNullOrEmpty(campoFechaIngreso)) {
                                observacionFila.Add("El campo Fecha de Ingreso es obligatorio.");
                                ResaltarCelda(worksheet, row, 4); // Resaltar celda D
                            }
                            if(string.IsNullOrEmpty(campoHoraIngreso)) {
                                observacionFila.Add("El campo Hora de Ingreso es obligatorio.");
                                ResaltarCelda(worksheet, row, 5); // Resaltar celda E
                            }
                            if(string.IsNullOrEmpty(campoFechaSalida)) {
                                observacionFila.Add("El campo Fecha de Salida es obligatorio.");
                                ResaltarCelda(worksheet, row, 6); // Resaltar celda F
                            }
                            if(string.IsNullOrEmpty(campoHoraSalida)) {
                                observacionFila.Add("El campo Hora de Salida es obligatorio.");
                                ResaltarCelda(worksheet, row, 7); // Resaltar celda G
                            }
                            if(string.IsNullOrEmpty(campoDispositivo)) {
                                observacionFila.Add("El campo Dispositivo es obligatorio.");
                                ResaltarCelda(worksheet, row, 8); // Resaltar celda H
                            }
                            if(string.IsNullOrEmpty(campoDeficiencia)) {
                                observacionFila.Add("El campo Deficiencia es obligatorio.");
                                ResaltarCelda(worksheet, row, 9); // Resaltar celda I
                            }
                            if(string.IsNullOrEmpty(campoMedidaAdoptada)) {
                                observacionFila.Add("El campo Medida Adoptada es obligatorio.");
                                ResaltarCelda(worksheet, row, 10); // Resaltar celda J
                            }

                            //Todo OK
                            if(!observacionFila.Any()) {
                                try {
                                    var registro = new ESS_AccionCajaTemporizadaEntidad {
                                        CodSala = int.Parse(campoSala.Split('-')[0]),
                                        NombreSala = campoSala.Split('-')[1],
                                        Fecha = DateTime.Parse(DateTime.Parse(campoFechaIngreso).ToString("yyyy-MM-dd"))
                                                        + TimeSpan.Parse(DateTime.Parse(campoHoraIngreso).ToString("HH:mm:ss")),
                                        FechaSolucion = DateTime.Parse(DateTime.Parse(campoFechaSalida).ToString("yyyy-MM-dd"))
                                                        + TimeSpan.Parse(DateTime.Parse(campoHoraSalida).ToString("HH:mm:ss")),
                                        IdDispositivo = int.Parse(campoDispositivo.Split('-')[0]),
                                        NombreDispositivo = campoDispositivo.Split('-')[1],
                                        IdDeficiencia = int.Parse(campoDeficiencia.Split('-')[0]),
                                        NombreDeficiencia = campoDeficiencia.Split('-')[1],
                                        MedidaAdoptada = campoMedidaAdoptada,
                                        FechaRegistro = DateTime.Now,
                                        UsuarioRegistro = (string)Session["UsuarioNombre"],
                                    };

                                    var EmpleadoNUKExiste = _essAccionCajaTemporizadaBL.ObtenerEmpleadoPorDocumentoBUK(campoPersonaAutoriza);
                                    if(EmpleadoNUKExiste == null) {
                                        string msg = "No se pudo encontrar al personal Autoriza.";
                                        observaciones.Add(new { Fila = row, Observaciones = msg });

                                        var richText = worksheet.Cells[row, ColumnaObservacionesImportar].RichText;
                                        var prefijo = richText.Add("❌ ");
                                        prefijo.Color = System.Drawing.Color.Red;
                                        var mensajetexto = richText.Add(msg);
                                        mensajetexto.Color = System.Drawing.Color.Black;
                                    } else {
                                        registro.IdAutoriza = EmpleadoNUKExiste.IdBuk;
                                        registro.NombreAutoriza = EmpleadoNUKExiste.NombreCompleto;

                                        int insertedId = _essAccionCajaTemporizadaBL.GuardarAccionCajaTemporizadafromImportar(registro);
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
