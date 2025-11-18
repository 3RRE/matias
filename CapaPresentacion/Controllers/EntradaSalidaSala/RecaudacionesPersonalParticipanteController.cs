using CapaEntidad.BUK;
using CapaEntidad.EntradaSalidaSala;
using CapaNegocio;
using CapaNegocio.AsistenciaCliente;
using CapaNegocio.BUK;
using CapaNegocio.EntradaSalidaSala;
using Newtonsoft.Json;
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
    public class RecaudacionesPersonalParticipanteController : Controller {
        private readonly SalaBL _salaBl = new SalaBL();
        private readonly EmpresaBL _empresaBl = new EmpresaBL();
        ESS_CatalogoBL ess_catalogobl = new ESS_CatalogoBL();
        DestinatarioBL destinatariobl = new DestinatarioBL();
        ClaseError error = new ClaseError();
        private readonly ESS_CargoBL _essCargoBL = new ESS_CargoBL();
        private readonly ESS_MotivoBL _essMotivoBL = new ESS_MotivoBL();
        private readonly ESS_CategoriaBL _essCategoriaBL = new ESS_CategoriaBL();
        private readonly ESS_EmpleadoBL _essEmpleadoBL = new ESS_EmpleadoBL();
        private readonly ESS_RecaudacionPersonalBL _essRecaudacionPersonalBL = new ESS_RecaudacionPersonalBL();
        private AST_TipoDocumentoBL ast_TipoDocumentoBL = new AST_TipoDocumentoBL();
        private readonly BUK_EmpleadoBL _bukEmpleadoBL = new BUK_EmpleadoBL();
        public ActionResult RecaudacionesPersonalParticipanteVista() {
            return View("~/Views/EntradaSalidaSala/RecaudacionPersonal.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarRecaudacionesPersonalParticipantexSalaJson(int[] codsala, DateTime fechaini, DateTime fechafin) {
            string mensaje = "No se encontraton registros";
            bool respuesta = false;
            List<ESS_RecaudacionPersonalEntidad> listaESS_RecaudacionPersonal = new List<ESS_RecaudacionPersonalEntidad>();
            try {
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                int[] salasBusqueda;
                if(codsala.Contains(-1)) {
                    salasBusqueda = salas.Select(x => x.CodSala).ToArray();
                } else {
                    salasBusqueda = codsala;
                }
                listaESS_RecaudacionPersonal = _essRecaudacionPersonalBL.ListadoRecaudacionPersonal(salasBusqueda, fechaini, fechafin);
                mensaje = "Registros obtenidos";
                respuesta = true;
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, comunicarse con administrador";
            }
            var oRespuesta = new {
                mensaje,
                respuesta,
                data = listaESS_RecaudacionPersonal
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
        public ActionResult GuardarRecaudacionesPersonalParticipante(ESS_RecaudacionPersonalEntidad recaudacionpersonal, string Empleados) {
            bool respuesta = false;

            string mensaje = "No se pudo guardar los datos";

            try {
                List<ESS_RecaudacionPersonalEmpleadoEntidad> listaEmpleados = JsonConvert.DeserializeObject<List<ESS_RecaudacionPersonalEmpleadoEntidad>>(Empleados);
                recaudacionpersonal.Empleados = listaEmpleados;
                recaudacionpersonal.FechaRegistro = DateTime.Now;
                recaudacionpersonal.UsuarioRegistro = (string)Session["UsuarioNombre"];

                int insertedId = _essRecaudacionPersonalBL.GuardarRecaudacionPersonal(recaudacionpersonal);
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
        public ActionResult EditarRecaudacionesPersonalParticipante(ESS_RecaudacionPersonalEntidad recaudacionpersonal, string Empleados) {
            bool respuesta = false;
            string mensaje = "No se pudo actualizar los datos";

            try {
                List<ESS_RecaudacionPersonalEmpleadoEntidad> listaEmpleados = JsonConvert.DeserializeObject<List<ESS_RecaudacionPersonalEmpleadoEntidad>>(Empleados);
                recaudacionpersonal.Empleados = listaEmpleados;
                recaudacionpersonal.FechaModificacion = DateTime.Now;
                recaudacionpersonal.UsuarioModificacion = (string)Session["UsuarioNombre"];

                respuesta = _essRecaudacionPersonalBL.ActualizarRecaudacionPersonal(recaudacionpersonal);
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
        public JsonResult EliminarRecaudacionesPersonalParticipante(int id) {
            bool respuesta = false;
            string mensaje = "No se pudo eliminar el registro.";

            try {
                respuesta = _essRecaudacionPersonalBL.EliminarRecaudacionPersonal(id);
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
        public ActionResult ListarFuncionPorEstado(int estado = 1) {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_FuncionEntidad>();
            try {

                data = _essRecaudacionPersonalBL.ListarFuncionPorEstado(estado);
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
        public ActionResult ListarCargoRPPorEstado(int estado = 1) {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_CargoRPEntidad>();
            try {

                data = _essRecaudacionPersonalBL.ListarCargoRPPorEstado(estado);
                mensaje = "Listando registros";
                respuesta = true;
                return Json(new { mensaje, respuesta, data });
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, contacte con administrador";
                respuesta = false;
                return Json(new { mensaje, respuesta });
            }
        }
        [HttpPost]
        public ActionResult ReporteRecaudacionesPersonalParticipanteDescargarExcelJson(int[] codsala, DateTime fechaIni, DateTime fechaFin) {
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
                List<ESS_RecaudacionPersonalEntidad> lista = _essRecaudacionPersonalBL.ListadoRecaudacionPersonal(salasBusqueda, fechaIni, fechaFin);

                if(lista.Count > 0) {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Recaudación Personal");
                    workSheet.TabColor = Color.Black;
                    workSheet.DefaultRowHeight = 12;

                    // Encabezado principal
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[2, 2, 2, 11].Merge = true;
                    workSheet.Cells[2, 2].Value = "Reporte de Recaudación Personal";
                    workSheet.Cells[2, 2].Style.Font.Bold = true;
                    workSheet.Cells[2, 2].Style.Font.Size = 13;
                    workSheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    // Encabezado de columnas
                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Sala";
                    workSheet.Cells[3, 4].Value = "Recaudación Inicio";
                    workSheet.Cells[3, 5].Value = "Recaudación Fin";
                    workSheet.Cells[3, 6].Value = "Empadronamiento Inicio";
                    workSheet.Cells[3, 7].Value = "Empadronamiento Fin";
                    workSheet.Cells[3, 8].Value = "Número Clientes";
                    workSheet.Cells[3, 9].Value = "Observaciones";
                    workSheet.Cells[3, 10].Value = "Empleados";

                    int recordIndex = 4;
                    foreach(var item in lista) {
                        workSheet.Cells[recordIndex, 2].Value = item.IdRecaudacionPersonal;
                        workSheet.Cells[recordIndex, 3].Value = item.NombreSala;
                        workSheet.Cells[recordIndex, 4].Value = item.RecaudacionInicio.ToString("dd-MM-yyyy hh:mm tt");
                        workSheet.Cells[recordIndex, 5].Value = item.RecaudacionFin.ToString("dd-MM-yyyy") == "01-01-1753" ? "No definido" : item.RecaudacionFin.ToString("dd-MM-yyyy hh:mm tt");
                        workSheet.Cells[recordIndex, 6].Value = item.EmpadronamientoInicio.ToString("dd-MM-yyyy") == "01-01-1753" ? "No definido" : item.EmpadronamientoInicio.ToString("dd-MM-yyyy hh:mm tt");
                        workSheet.Cells[recordIndex, 7].Value = item.EmpadronamientoFin.ToString("dd-MM-yyyy") == "01-01-1753" ? "No definido" : item.EmpadronamientoFin.ToString("dd-MM-yyyy hh:mm tt");

                        workSheet.Cells[recordIndex, 8].Value = item.NumeroClientes;
                        workSheet.Cells[recordIndex, 9].Value = item.Observaciones;

                        // Crear hoja de empleados si existen
                        if(item.Empleados != null && item.Empleados.Any()) {
                            var employeeSheet = excel.Workbook.Worksheets.Add($"Detalle_Recaudacion_{item.IdRecaudacionPersonal}");
                            employeeSheet.Cells[1, 2, 1, 10].Merge = true;
                            employeeSheet.Cells[1, 2].Value = $"Detalle: Recaudación Personal N°: {item.IdRecaudacionPersonal}";
                            employeeSheet.Cells[1, 2].Style.Font.Bold = true;
                            employeeSheet.Cells[1, 2].Style.Font.Size = 13;
                            employeeSheet.Cells[1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            // Encabezado empleados
                            employeeSheet.Cells[3, 2].Value = "ID";
                            employeeSheet.Cells[3, 3].Value = "Nombre";
                            employeeSheet.Cells[3, 4].Value = "Apellido Paterno";
                            employeeSheet.Cells[3, 5].Value = "Apellido Materno";
                            employeeSheet.Cells[3, 6].Value = "Tipo Documento";
                            employeeSheet.Cells[3, 7].Value = "Documento";
                            employeeSheet.Cells[3, 8].Value = "Estado Participante";
                            employeeSheet.Cells[3, 9].Value = "Cargo";
                            employeeSheet.Cells[3, 10].Value = "Función";

                            Color headerBackground = ColorTranslator.FromHtml("#003268");
                            employeeSheet.Cells["B3:J3"].Style.Font.Bold = true;
                            employeeSheet.Cells["B3:J3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            employeeSheet.Cells["B3:J3"].Style.Fill.BackgroundColor.SetColor(headerBackground);
                            employeeSheet.Cells["B3:J3"].Style.Font.Color.SetColor(Color.White);
                            employeeSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            int employeeIndex = 4;
                            foreach(var empleado in item.Empleados) {
                                employeeSheet.Cells[employeeIndex, 2].Value = empleado.IdEmpleado;
                                employeeSheet.Cells[employeeIndex, 3].Value = empleado.Nombre;
                                employeeSheet.Cells[employeeIndex, 4].Value = empleado.ApellidoPaterno;
                                employeeSheet.Cells[employeeIndex, 5].Value = empleado.ApellidoMaterno;
                                employeeSheet.Cells[employeeIndex, 6].Value = empleado.TipoDocumento.ToUpper();
                                employeeSheet.Cells[employeeIndex, 7].Value = empleado.DocumentoRegistro;
                                employeeSheet.Cells[employeeIndex, 8].Value = empleado.EstadoParticipante;
                                employeeSheet.Cells[employeeIndex, 9].Value = empleado.NombreCargo;
                                employeeSheet.Cells[employeeIndex, 10].Value = empleado.IdEstadoParticipante == 1 ? empleado.IdFuncion == -1 ? "(OTROS) " + empleado.DescripcionFuncion : empleado.NombreFuncion : "N/A";
                                employeeIndex++;
                            }
                            for(int i = 2; i <= 10; i++) {
                                employeeSheet.Column(i).AutoFit();
                            }

                            // Agregar hipervínculo
                            var hyperlink = new ExcelHyperLink($"#'Detalle_Recaudacion_{item.IdRecaudacionPersonal}'!A1", "Ver Detalles");
                            workSheet.Cells[recordIndex, 10].Hyperlink = hyperlink;
                            workSheet.Cells[recordIndex, 10].Style.Font.UnderLine = true;
                            workSheet.Cells[recordIndex, 10].Style.Font.Color.SetColor(Color.Blue);
                        } else {
                            workSheet.Cells[recordIndex, 10].Value = "No hay empleados";
                        }

                        recordIndex++;
                    }

                    Color principalHeaderBackground = ColorTranslator.FromHtml("#003268");
                    workSheet.Cells["B3:J3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:J3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:J3"].Style.Fill.BackgroundColor.SetColor(principalHeaderBackground);
                    workSheet.Cells["B3:J3"].Style.Font.Color.SetColor(Color.White);

                    for(int i = 2; i <= 10; i++) {
                        workSheet.Column(i).AutoFit();
                    }

                    // Nombre del archivo
                    excelName = $"ReporteRecaudacionPersonal_{fechaIni:dd_MM_yyyy}_al_{fechaFin:dd_MM_yyyy}.xlsx";

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
        public ActionResult ListarFuncion() {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_FuncionEntidad>();
            try {
                data = _essRecaudacionPersonalBL.ListarFuncion();
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
        public ActionResult InsertarFuncion(ESS_FuncionEntidad model) {
            string mensaje = "No se pudo insertar el registro";
            bool respuesta = false;
            try {
                model.FechaRegistro = DateTime.Now;
                model.UsuarioRegistro = (string)Session["UsuarioNombre"];
                int idInsertado = _essRecaudacionPersonalBL.InsertarFuncion(model);
                if(idInsertado > 0) {
                    mensaje = "Registro insertado";
                    respuesta = true;
                }
                return Json(new { mensaje, respuesta });
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, contacte con administrador";
                respuesta = false;
                return Json(new { mensaje, respuesta });
            }
        }
        [seguridad(false)]

        [HttpPost]
        public ActionResult EditarFuncion(ESS_FuncionEntidad model) {
            string mensaje = "No se pudo editar el registro";
            bool respuesta = false;
            try {
                model.FechaModificacion = DateTime.Now;
                model.UsuarioModificacion = (string)Session["UsuarioNombre"];
                respuesta = _essRecaudacionPersonalBL.EditarFuncion(model);
                if(respuesta) {
                    mensaje = "Registro editado";
                }
                return Json(new { mensaje, respuesta });
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, contacte con administrador";
                respuesta = false;
                return Json(new { mensaje, respuesta });
            }
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult PlantillaRecaudacionesPersonalParticipanteDescargarExcel() {
            string path = Server.MapPath(@"~/Content/ess_excel_plantilla/Plantilla_ESS_RecaudacionPersonalParticipante.xlsx");
            string base64String = "";
            string excelName = "";
            string mensaje = "";
            bool respuesta = false;

            try {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                var funciones = _essRecaudacionPersonalBL.ListarFuncionPorEstado(1);
                funciones.Add(new ESS_FuncionEntidad { IdFuncion = -1, Nombre = "OTROS" });
                var tipodocumentos = ast_TipoDocumentoBL.GetListadoTipoDocumento();


                List<string> listaDatosSala = salas.Select(x => $"{x.CodSala}-{x.Nombre}").ToList();
                List<string> listaDatosFunciones = funciones.Select(x => $"{x.IdFuncion}-{x.Nombre}").ToList();
                List<string> listaDatosTipoDocumentos = tipodocumentos.Select(x => $"{x.Id}-{x.Nombre.ToUpper()}").ToList();


                using(var package = new ExcelPackage(new FileInfo(path))) {
                    var workSheet = package.Workbook.Worksheets[0];


                    int startRowSala = 2;
                    int startRowFuncion = 5;
                    int startRowTipoBien = 1;
                    int columnStartTipoBien = 27;

                    var validationSala = workSheet.DataValidations.AddListValidation("E5:E30");
                    validationSala.ShowErrorMessage = true;
                    validationSala.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    validationSala.ErrorTitle = "Valor inválido";
                    validationSala.Error = "Seleccione un valor de la lista.";

                    for(int i = 0; i < funciones.Count; i++) {
                        var fila = startRowFuncion + i;
                        var cellB = workSheet.Cells[$"B{fila}"];
                        var cellC = workSheet.Cells[$"C{fila}"];

                        cellB.Value = funciones[i].IdFuncion;
                        cellC.Value = funciones[i].Nombre;

                        foreach(var cell in new[] { cellB, cellC }) {
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.Border.Top.Color.SetColor(Color.Black);
                            cell.Style.Border.Bottom.Color.SetColor(Color.Black);
                            cell.Style.Border.Left.Color.SetColor(Color.Black);
                            cell.Style.Border.Right.Color.SetColor(Color.Black);
                        }

                    }
                    //Sala
                    for(int j = 0; j < listaDatosSala.Count; j++) {
                        workSheet.Cells[startRowSala + j, 26].Value = listaDatosSala[j];
                    }
                    int endRowSala = startRowSala + listaDatosSala.Count - 1;
                    validationSala.Formula.ExcelFormula = $"$Z${startRowSala}:$Z${endRowSala}";
                    workSheet.Column(26).Hidden = true;

                    workSheet.Column(columnStartTipoBien).Hidden = true;



                    byte[] bin = package.GetAsByteArray();
                    base64String = Convert.ToBase64String(bin);
                    excelName = "Plantilla_RecaudacionPersonal_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx";
                    respuesta = true;
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }

            return Json(new { respuesta, mensaje, data = base64String, excelName });
        }


        [HttpPost]
        public ActionResult ImportarExcelRecaudacionesPersonalParticipante() {
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


                var funciones = _essRecaudacionPersonalBL.ListarFuncionPorEstado(1);
                funciones.Add(new ESS_FuncionEntidad { IdFuncion = -1, Nombre = "OTROS" });
                List<ESS_FuncionEntidad> listaFunciones = funciones.Select(x => new ESS_FuncionEntidad { IdFuncion = x.IdFuncion, Nombre = x.Nombre }).ToList();



                string fechaHora = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string nombreProcesado = $"Procesado_{fechaHora}_{nombreOriginal}";
                using(var stream = archivoExcel.InputStream) {
                    using(var package = new ExcelPackage(stream)) {
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                        if(worksheet == null)
                            return Json(new { respuesta = false, mensaje = "El archivo no contiene hojas válidas." });

                        int ColumnaObservacionesImportar = 19;
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
                            for(int col = 5; col <= 18; col++) {
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

                            string campoSala = worksheet.Cells[row, 5].Text.Trim(); // Columna E
                            string campoFechaRecaudacionInicio = worksheet.Cells[row, 6].Text.Trim(); // Columna F
                            string campoHoraRecaudacionInicio = worksheet.Cells[row, 7].Text.Trim(); // Columna G
                            string campoFechaRecaudacionFin = worksheet.Cells[row, 8].Text.Trim(); // Columna H
                            string campoHoraRecaudacionFin = worksheet.Cells[row, 9].Text.Trim(); // Columna I
                            string campoFechaEmpadronamientoInicio = worksheet.Cells[row, 10].Text.Trim(); // Columna J
                            string campoHoraEmpadronamientoInicio = worksheet.Cells[row, 11].Text.Trim(); // Columna K
                            string campoFechaEmpadronamientoFin = worksheet.Cells[row, 12].Text.Trim(); // Columna L
                            string campoHoraEmpadronamientoFin = worksheet.Cells[row, 13].Text.Trim(); // Columna M
                            string campoNumeroClientes = worksheet.Cells[row, 14].Text.Trim(); // Columna N
                            string campoEmpleadosParticipantes = worksheet.Cells[row, 15].Text.Trim(); // Columna O
                            string campoEmpleadosParticipantesFuncion = worksheet.Cells[row, 16].Text.Trim(); // Columna P
                            string campoEmpleadosNoParticipantes = worksheet.Cells[row, 17].Text.Trim(); // Columna Q
                            string campoObservaciones = worksheet.Cells[row, 18].Text.Trim(); // Columna R


                            // Validaciones
                            if(string.IsNullOrEmpty(campoSala)) {
                                observacionFila.Add("El campo Sala es obligatorio.");
                                ResaltarCelda(worksheet, row, 5); // Resaltar celda E
                            }
                            if(string.IsNullOrEmpty(campoFechaRecaudacionInicio)) { observacionFila.Add("El campo Fecha de Recaudación Inicio es obligatorio."); ResaltarCelda(worksheet, row, 6); } else if(!DateTime.TryParse(campoFechaRecaudacionInicio, out _)) { observacionFila.Add("El campo Fecha de Recaudación Inicio no es una fecha válida."); ResaltarCelda(worksheet, row, 6); }
                            if(string.IsNullOrEmpty(campoHoraRecaudacionInicio)) { observacionFila.Add("El campo Hora de Recaudación Inicio es obligatorio."); ResaltarCelda(worksheet, row, 7); } else if(!TimeSpan.TryParse(campoHoraRecaudacionInicio, out _)) { observacionFila.Add("El campo Hora de Recaudación Inicio no es una hora válida."); ResaltarCelda(worksheet, row, 7); }

                            if(string.IsNullOrEmpty(campoFechaRecaudacionFin)) { observacionFila.Add("El campo Fecha de Recaudación Fin es obligatorio."); ResaltarCelda(worksheet, row, 8); } else if(!DateTime.TryParse(campoFechaRecaudacionFin, out _)) { observacionFila.Add("El campo Fecha de Recaudación Fin no es una fecha válida."); ResaltarCelda(worksheet, row, 8); }
                            if(string.IsNullOrEmpty(campoHoraRecaudacionFin)) { observacionFila.Add("El campo Hora de Recaudación Fin es obligatorio."); ResaltarCelda(worksheet, row, 9); } else if(!TimeSpan.TryParse(campoHoraRecaudacionFin, out _)) { observacionFila.Add("El campo Hora de Recaudación Fin no es una hora válida."); ResaltarCelda(worksheet, row, 9); }

                            if(string.IsNullOrEmpty(campoFechaEmpadronamientoInicio)) { observacionFila.Add("El campo Fecha de Empadronamiento Inicio es obligatorio."); ResaltarCelda(worksheet, row, 10); } else if(!DateTime.TryParse(campoFechaEmpadronamientoInicio, out _)) { observacionFila.Add("El campo Fecha de Empadronamiento Inicio no es una fecha válida."); ResaltarCelda(worksheet, row, 10); }
                            if(string.IsNullOrEmpty(campoHoraEmpadronamientoInicio)) { observacionFila.Add("El campo Hora de Empadronamiento Inicio es obligatorio."); ResaltarCelda(worksheet, row, 11); } else if(!TimeSpan.TryParse(campoHoraEmpadronamientoInicio, out _)) { observacionFila.Add("El campo Hora de Empadronamiento Inicio no es una hora válida."); ResaltarCelda(worksheet, row, 11); }

                            if(string.IsNullOrEmpty(campoFechaEmpadronamientoFin)) { observacionFila.Add("El campo Fecha de Empadronamiento Fin es obligatorio."); ResaltarCelda(worksheet, row, 12); } else if(!DateTime.TryParse(campoFechaEmpadronamientoFin, out _)) { observacionFila.Add("El campo Fecha de Empadronamiento Fin no es una fecha válida."); ResaltarCelda(worksheet, row, 12); }
                            if(string.IsNullOrEmpty(campoHoraEmpadronamientoFin)) { observacionFila.Add("El campo Hora de Empadronamiento Fin es obligatorio."); ResaltarCelda(worksheet, row, 13); } else if(!TimeSpan.TryParse(campoHoraEmpadronamientoFin, out _)) { observacionFila.Add("El campo Hora de Empadronamiento Fin no es una hora válida."); ResaltarCelda(worksheet, row, 13); }

                            if(string.IsNullOrEmpty(campoNumeroClientes)) { observacionFila.Add("El campo Número de Clientes es obligatorio."); ResaltarCelda(worksheet, row, 14); }

                            if(string.IsNullOrEmpty(campoEmpleadosParticipantes)) { observacionFila.Add("El campo Empleados Participantes es obligatorio."); ResaltarCelda(worksheet, row, 15); } else if(!campoEmpleadosParticipantes.Split(',').Any()) { observacionFila.Add("El campo Empleados Participantes no tiene un formato válido."); ResaltarCelda(worksheet, row, 15); }

                            if(string.IsNullOrEmpty(campoEmpleadosParticipantesFuncion)) { observacionFila.Add("El campo Empleados Participantes Función es obligatorio."); ResaltarCelda(worksheet, row, 16); } else if(!campoEmpleadosParticipantesFuncion.Split(',').Any()) { observacionFila.Add("El campo Empleados Participantes Función no tiene un formato válido."); ResaltarCelda(worksheet, row, 16); }

                            if(string.IsNullOrEmpty(campoEmpleadosNoParticipantes)) { observacionFila.Add("El campo Empleados No Participantes es obligatorio."); ResaltarCelda(worksheet, row, 17); } else if(!campoEmpleadosNoParticipantes.Split(',').Any()) { observacionFila.Add("El campo Empleados No Participantes no tiene un formato válido."); ResaltarCelda(worksheet, row, 17); }

                            if(campoEmpleadosParticipantes.Split(',').Any() && campoEmpleadosParticipantesFuncion.Split(',').Any()) {
                                if(campoEmpleadosParticipantes.Split(',').Count() != campoEmpleadosParticipantesFuncion.Split(',').Count()) {
                                    observacionFila.Add("El campo Empleados Participantes y Empleados Participantes Función no tienen la misma cantidad de registros.");
                                }
                            }
                            //Todo OK
                            if(!observacionFila.Any()) {
                                try {
                                    var registro = new ESS_RecaudacionPersonalEntidad {
                                        CodSala = int.Parse(campoSala.Split('-')[0]),
                                        NombreSala = campoSala.Split('-')[1],
                                        RecaudacionInicio = DateTime.Parse(DateTime.Parse(campoFechaRecaudacionInicio).ToString("yyyy-MM-dd")) + TimeSpan.Parse(DateTime.Parse(campoHoraRecaudacionInicio).ToString("HH:mm:ss")),
                                        RecaudacionFin = DateTime.Parse(DateTime.Parse(campoFechaRecaudacionFin).ToString("yyyy-MM-dd")) + TimeSpan.Parse(DateTime.Parse(campoHoraRecaudacionFin).ToString("HH:mm:ss")),
                                        EmpadronamientoInicio = DateTime.Parse(DateTime.Parse(campoFechaEmpadronamientoInicio).ToString("yyyy-MM-dd")) + TimeSpan.Parse(DateTime.Parse(campoHoraEmpadronamientoInicio).ToString("HH:mm:ss")),
                                        EmpadronamientoFin = DateTime.Parse(DateTime.Parse(campoFechaEmpadronamientoFin).ToString("yyyy-MM-dd")) + TimeSpan.Parse(DateTime.Parse(campoHoraEmpadronamientoFin).ToString("HH:mm:ss")),
                                        NumeroClientes = int.Parse(campoNumeroClientes),
                                        Observaciones = campoObservaciones,
                                        FechaRegistro = DateTime.Now,
                                        UsuarioRegistro = (string)Session["UsuarioNombre"],

                                    };

                                    var observacionFila_Empleados = new List<string>();
                                    var ObservacionFinal = new List<string>();

                                    List<string> ListaEmpleadosParticipantes = campoEmpleadosParticipantes.Split(',').Select(x => x.Trim()).ToList();
                                    List<string> ListaEmpleadosParticipantesFuncion = campoEmpleadosParticipantesFuncion.Split(',').Select(x => x.Trim()).ToList();
                                    //Añadiendo los empleados PARTICIPANTES
                                    for(int i = 0; i < ListaEmpleadosParticipantes.Count; i++) {
                                        var NumeroDocumento = ListaEmpleadosParticipantes[i].Trim();
                                        List<BUK_EmpleadoEntidad> listadoEmpleado_por_numerodocumento = new List<BUK_EmpleadoEntidad>();
                                        listadoEmpleado_por_numerodocumento = _bukEmpleadoBL.ObtenerEmpleadosActivosPorTerminoSinEmpresa(NumeroDocumento);
                                        //el primer empleado de mi lista listadoemplead_por_numerodocumento
                                        var ExisteEmpleado = listadoEmpleado_por_numerodocumento.FirstOrDefault(x => x.NumeroDocumento.Trim() == NumeroDocumento.Trim());

                                        int IdFuncion = int.Parse(ListaEmpleadosParticipantesFuncion[i]);
                                        var ExisteFuncion = listaFunciones.FirstOrDefault(x => x.IdFuncion == IdFuncion);

                                        if(ExisteEmpleado == null) {
                                            observacionFila_Empleados.Add($"No se encontró al Empleado {NumeroDocumento}.");
                                        } else {
                                            ESS_RecaudacionPersonalEmpleadoEntidad NuevoEmpleado = new ESS_RecaudacionPersonalEmpleadoEntidad();
                                            if(ExisteFuncion == null) {
                                                NuevoEmpleado.IdFuncion = 0;
                                            } else {
                                                NuevoEmpleado.IdFuncion = ExisteFuncion.IdFuncion;
                                                NuevoEmpleado.NombreFuncion = ExisteFuncion.Nombre;
                                            }

                                            NuevoEmpleado.IdEmpleado = ExisteEmpleado.IdBuk;
                                            NuevoEmpleado.TipoDocumento = ExisteEmpleado.TipoDocumento;
                                            NuevoEmpleado.DocumentoRegistro = ExisteEmpleado.NumeroDocumento;
                                            NuevoEmpleado.Nombre = ExisteEmpleado.Nombres;
                                            NuevoEmpleado.ApellidoPaterno = ExisteEmpleado.ApellidoPaterno;
                                            NuevoEmpleado.ApellidoMaterno = ExisteEmpleado.ApellidoMaterno;
                                            NuevoEmpleado.Cargo = ExisteEmpleado.Cargo;
                                            NuevoEmpleado.IdCargo = ExisteEmpleado.IdCargo;
                                            NuevoEmpleado.IdEstadoParticipante = 1;
                                            NuevoEmpleado.EstadoParticipante = "Participante";
                                            NuevoEmpleado.Empresa = ExisteEmpleado.Empresa;
                                            NuevoEmpleado.IdEmpresa = ExisteEmpleado.IdEmpresa;

                                            registro.Empleados.Add(NuevoEmpleado);
                                        }
                                    }

                                    List<string> ListaEmpleadosNoParticipantes = campoEmpleadosNoParticipantes.Split(',').Select(x => x.Trim()).ToList();

                                    for(int i = 0; i < ListaEmpleadosNoParticipantes.Count; i++) {
                                        var NumeroDocumento = ListaEmpleadosNoParticipantes[i].Trim();
                                        List<BUK_EmpleadoEntidad> listadoEmpleado_por_numerodocumento = new List<BUK_EmpleadoEntidad>();
                                        listadoEmpleado_por_numerodocumento = _bukEmpleadoBL.ObtenerEmpleadosActivosPorTerminoSinEmpresa(NumeroDocumento);
                                        var ExisteEmpleado = listadoEmpleado_por_numerodocumento.FirstOrDefault(x => x.NumeroDocumento.Trim() == NumeroDocumento.Trim());

                                        if(ExisteEmpleado == null) {
                                            observacionFila_Empleados.Add($"No se encontró al Empleado {NumeroDocumento}.");
                                        } else {
                                            ESS_RecaudacionPersonalEmpleadoEntidad NuevoEmpleado = new ESS_RecaudacionPersonalEmpleadoEntidad();

                                            NuevoEmpleado.IdEmpleado = ExisteEmpleado.IdBuk;
                                            NuevoEmpleado.TipoDocumento = ExisteEmpleado.TipoDocumento;
                                            NuevoEmpleado.DocumentoRegistro = ExisteEmpleado.NumeroDocumento;
                                            NuevoEmpleado.Nombre = ExisteEmpleado.Nombres;
                                            NuevoEmpleado.ApellidoPaterno = ExisteEmpleado.ApellidoPaterno;
                                            NuevoEmpleado.ApellidoMaterno = ExisteEmpleado.ApellidoMaterno;
                                            NuevoEmpleado.Cargo = ExisteEmpleado.Cargo;
                                            NuevoEmpleado.IdCargo = ExisteEmpleado.IdCargo;
                                            NuevoEmpleado.IdEstadoParticipante = 2;
                                            NuevoEmpleado.EstadoParticipante = "No Participante";
                                            NuevoEmpleado.Empresa = ExisteEmpleado.Empresa;
                                            NuevoEmpleado.IdEmpresa = ExisteEmpleado.IdEmpresa;
                                            NuevoEmpleado.IdFuncion = 0;

                                            registro.Empleados.Add(NuevoEmpleado);
                                        }
                                    }
                                    if(registro.Empleados.Count == 0) {
                                        observacionFila_Empleados.Add("No se registró con ningun Empleado.");
                                    } else {
                                        if(observacionFila_Empleados.Any()) {
                                            ObservacionFinal = observacionFila_Empleados;
                                        }
                                    }

                                    int insertedId = _essRecaudacionPersonalBL.GuardarRecaudacionPersonal(registro);
                                    if(insertedId > 0) {
                                        string msg = "Registro guardado con éxito." + (ObservacionFinal.Count == 0 ? "" : " | " + string.Join(" | ", ObservacionFinal));
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

                        worksheet.Column(ColumnaObservacionesImportar).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        worksheet.Column(ColumnaObservacionesImportar).AutoFit();
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