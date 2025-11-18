using CapaDatos.Utilitarios;
using CapaEntidad.BUK;
using CapaEntidad.EntradaSalidaSala;
using CapaNegocio;
using CapaNegocio.BUK;
using CapaNegocio.EntradaSalidaSala;
using Newtonsoft.Json;
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
    public class IngresosSalidasGUController : Controller {

        private readonly SalaBL _salaBl = new SalaBL();
        private readonly EmpresaBL _empresaBl = new EmpresaBL();
        ESS_BienMaterialBL ess_bienmaterialbl = new ESS_BienMaterialBL();
        ESS_CatalogoBL ess_catalogobl = new ESS_CatalogoBL();
        DestinatarioBL destinatariobl = new DestinatarioBL();
        ClaseError error = new ClaseError();
        private readonly ESS_CargoBL _essCargoBL = new ESS_CargoBL();
        private readonly ESS_MotivoBL _essMotivoBL = new ESS_MotivoBL();
        private readonly ESS_CategoriaBL _essCategoriaBL = new ESS_CategoriaBL();
        private readonly ESS_EmpleadoBL _essEmpleadoBL = new ESS_EmpleadoBL();
        private readonly ESS_IngresoSalidaGUBL _essIngresoSalidaGUBL = new ESS_IngresoSalidaGUBL();
        private readonly BUK_EmpleadoBL _bukEmpleadoBL = new BUK_EmpleadoBL();
        public ActionResult IngresosSalidasGUVista() {
            return View("~/Views/EntradaSalidaSala/IngresoSalidaGU.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarIngresosSalidasGUxSalaJson(int[] codsala, DateTime fechaini, DateTime fechafin) {
            string mensaje = "No se encontraton registros";
            bool respuesta = false;
            List<ESS_IngresoSalidaGUEntidad> listaESS_IngresoSalidaGU = new List<ESS_IngresoSalidaGUEntidad>();
            try {
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                int[] salasBusqueda;
                if(codsala.Contains(-1)) {
                    salasBusqueda = salas.Select(x => x.CodSala).ToArray();
                } else {
                    salasBusqueda = codsala;
                }
                listaESS_IngresoSalidaGU = _essIngresoSalidaGUBL.ListadoIngresoSalida(salasBusqueda, fechaini, fechafin);
                mensaje = "Registros obtenidos";
                respuesta = true;
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, comunicarse con administrador";
            }
            var oRespuesta = new {
                mensaje,
                respuesta,
                data = listaESS_IngresoSalidaGU
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
        public ActionResult GuardarIngresosSalidasGU(ESS_IngresoSalidaGUEntidad ingresosalidagu, string Empleados) {
            bool respuesta = false;

            string mensaje = "No se pudo guardar los datos";

            try {
                ingresosalidagu.FechaRegistro = DateTime.Now;
                ingresosalidagu.UsuarioRegistro = (string)Session["UsuarioNombre"];
                List<ESS_IngresoSalidaGUEmpleadoEntidad> listaEmpleados = JsonConvert.DeserializeObject<List<ESS_IngresoSalidaGUEmpleadoEntidad>>(Empleados);
                ingresosalidagu.Empleados = listaEmpleados;

                int insertedId = _essIngresoSalidaGUBL.GuardarIngresoSalidaGU(ingresosalidagu);
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
        public ActionResult EditarIngresosSalidasGU(ESS_IngresoSalidaGUEntidad ingresosalidagu, string Empleados) {
            bool respuesta = false;
            string mensaje = "No se pudo actualizar los datos";

            try {
                List<ESS_IngresoSalidaGUEmpleadoEntidad> listaEmpleados = JsonConvert.DeserializeObject<List<ESS_IngresoSalidaGUEmpleadoEntidad>>(Empleados);
                ingresosalidagu.Empleados = listaEmpleados;
                ingresosalidagu.FechaModificacion = DateTime.Now;
                ingresosalidagu.UsuarioModificacion = (string)Session["UsuarioNombre"];

                respuesta = _essIngresoSalidaGUBL.ActualizarIngresoSalidaGU(ingresosalidagu);
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
        public JsonResult EliminarIngresosSalidasGU(int id) {
            bool respuesta = false;
            string mensaje = "No se pudo eliminar el registro.";

            try {
                respuesta = _essIngresoSalidaGUBL.EliminarIngresoSalidaGU(id);
                if(respuesta) {
                    mensaje = "Registro eliminado correctamente.";
                }
            } catch(Exception ex) {
                mensaje = "Error al eliminar el registro: " + ex.Message;
            }

            return Json(new { respuesta, mensaje });
        }

        [HttpPost]
        public ActionResult ReporteIngresosSalidasGUDescargarExcelJson(int[] codsala, DateTime fechaIni, DateTime fechaFin) {
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
                List<ESS_IngresoSalidaGUEntidad> lista = _essIngresoSalidaGUBL.ListadoIngresoSalida(salasBusqueda, fechaIni, fechaFin);

                lista = lista.OrderByDescending(x => x.IdIngresoSalidaGU).ToList();

                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();


                    var workSheet = excel.Workbook.Worksheets.Add("Ingresos y Salidas");
                    workSheet.TabColor = Color.Black;
                    workSheet.DefaultRowHeight = 12;


                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[2, 2, 2, 11].Merge = true;
                    workSheet.Cells[2, 2].Value = "Reporte de Ingresos y Salidas";
                    workSheet.Cells[2, 2].Style.Font.Bold = true;
                    workSheet.Cells[2, 2].Style.Font.Size = 13;
                    workSheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Sala";
                    workSheet.Cells[3, 4].Value = "Descripción";
                    workSheet.Cells[3, 5].Value = "Motivo";
                    workSheet.Cells[3, 6].Value = "Fecha";
                    workSheet.Cells[3, 7].Value = "Hora Ingreso";
                    workSheet.Cells[3, 8].Value = "Hora Salida";
                    workSheet.Cells[3, 9].Value = "Estado";
                    workSheet.Cells[3, 10].Value = "Observaciones";
                    workSheet.Cells[3, 11].Value = "Gerente de Unidad";

                    int recordIndex = 4;
                    foreach(var item in lista) {
                        workSheet.Cells[recordIndex, 2].Value = item.IdIngresoSalidaGU;
                        workSheet.Cells[recordIndex, 3].Value = item.NombreSala;
                        workSheet.Cells[recordIndex, 4].Value = item.Descripcion;
                        //workSheet.Cells[recordIndex, 5].Value = item.NombreMotivo;
                        workSheet.Cells[recordIndex, 6].Value = item.FechaRegistro.ToString("dd-MM-yyyy");
                        workSheet.Cells[recordIndex, 7].Value = item.HoraIngreso.ToString("dd-MM-yyyy") == "01-01-1753" ? "No definido" : item.HoraIngreso.ToString("dd-MM-yyyy hh:mm tt");
                        workSheet.Cells[recordIndex, 8].Value = item.HoraSalida.ToString("dd-MM-yyyy") == "01-01-1753" ? "No definido" : item.HoraSalida.ToString("dd-MM-yyyy hh:mm tt");
                        workSheet.Cells[recordIndex, 9].Value = item.Estado;
                        workSheet.Cells[recordIndex, 10].Value = string.IsNullOrEmpty(item.Observaciones) ? "No definido" : item.Observaciones;
                        if(item.Empleados != null && item.Empleados.Any()) {
                            foreach(var empleado in item.Empleados) {
                                workSheet.Cells[recordIndex, 11].Value = $"{empleado.Nombre} {empleado.ApellidoPaterno} {empleado.ApellidoMaterno}";
                            }
                        } else {
                            workSheet.Cells[recordIndex, 11].Value = "No hay gerente";
                        }

                        if(item.NombreMotivo == "OTROS") {
                            workSheet.Cells[recordIndex, 5].Value = $"({item.NombreMotivo}) {item.DescripcionMotivo}";
                        } else {
                            workSheet.Cells[recordIndex, 5].Value = item.NombreMotivo;
                        }
                        workSheet.Column(5).Width = 30;

                        //    if(item.Empleados != null && item.Empleados.Any()) {
                        //        var employeeSheet = excel.Workbook.Worksheets.Add($"Detalle_IngresoSalida_{item.IdIngresoSalidaGU}");
                        //        employeeSheet.Cells[1, 2, 1, 8].Merge = true;
                        //        employeeSheet.Cells[1, 2].Value = $"Detalle: Ingreso/Salida N°: {item.IdIngresoSalidaGU}";
                        //        employeeSheet.Cells[1, 2].Style.Font.Bold = true;
                        //        employeeSheet.Cells[1, 2].Style.Font.Size = 13;
                        //        employeeSheet.Cells[1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //        employeeSheet.Cells[3, 2].Value = "ID";
                        //        employeeSheet.Cells[3, 3].Value = "Nombre";
                        //        employeeSheet.Cells[3, 4].Value = "Apellido Paterno";
                        //        employeeSheet.Cells[3, 5].Value = "Apellido Materno";
                        //        employeeSheet.Cells[3, 6].Value = "Tipo Documento";
                        //        employeeSheet.Cells[3, 7].Value = "Documento";
                        //        employeeSheet.Cells[3, 8].Value = "Cargo";

                        //        Color headerBackground = ColorTranslator.FromHtml("#003268");
                        //        employeeSheet.Cells["B3:H3"].Style.Font.Bold = true;
                        //        employeeSheet.Cells["B3:H3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //        employeeSheet.Cells["B3:H3"].Style.Fill.BackgroundColor.SetColor(headerBackground);
                        //        employeeSheet.Cells["B3:H3"].Style.Font.Color.SetColor(Color.White);
                        //        employeeSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //        int employeeIndex = 4;
                        //        foreach(var empleado in item.Empleados) {
                        //            employeeSheet.Cells[employeeIndex, 2].Value = empleado.IdEmpleado;
                        //            employeeSheet.Cells[employeeIndex, 3].Value = empleado.Nombre;
                        //            employeeSheet.Cells[employeeIndex, 4].Value = empleado.ApellidoPaterno;
                        //            employeeSheet.Cells[employeeIndex, 5].Value = empleado.ApellidoMaterno;
                        //            employeeSheet.Cells[employeeIndex, 6].Value = empleado.TipoDocumento.ToUpper();
                        //            employeeSheet.Cells[employeeIndex, 7].Value = empleado.DocumentoRegistro.ToUpper();
                        //            employeeSheet.Cells[employeeIndex, 8].Value = empleado.NombreCargo;
                        //            employeeIndex++;
                        //        }

                        //        for(int i = 2; i <= 8; i++) {
                        //            employeeSheet.Column(i).AutoFit();
                        //        }

                        //        var hyperlink = new ExcelHyperLink($"#'Detalle_IngresoSalida_{item.IdIngresoSalidaGU}'!A1", "Ver Detalles");
                        //        workSheet.Cells[recordIndex, 11].Hyperlink = hyperlink;
                        //        workSheet.Cells[recordIndex, 11].Style.Font.UnderLine = true;
                        //        workSheet.Cells[recordIndex, 11].Style.Font.Color.SetColor(Color.Blue);
                        //    } else {
                        //        workSheet.Cells[recordIndex, 11].Value = "No hay empleados";
                        //    }

                        recordIndex++;
                    }

                    Color principalHeaderBackground = ColorTranslator.FromHtml("#003268");
                    workSheet.Cells["B3:K3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:K3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:K3"].Style.Fill.BackgroundColor.SetColor(principalHeaderBackground);
                    workSheet.Cells["B3:K3"].Style.Font.Color.SetColor(Color.White);

                    for(int i = 2; i <= 11; i++) {
                        if(i != 5) {
                            workSheet.Column(i).AutoFit();
                        }
                    }

                    // Nombre del archivo
                    excelName = $"ReporteIngresoSalida_{fechaIni:dd_MM_yyyy}_al_{fechaFin:dd_MM_yyyy}.xlsx";

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
        public ActionResult ListarMotivoGUPorEstado(int estado = 1) {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_MotivoGUEntidad>();
            try {

                data = _essIngresoSalidaGUBL.ListarMotivoGUPorEstado(estado);
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
        public ActionResult ListarMotivo() {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_MotivoGUEntidad>();
            try {
                data = _essIngresoSalidaGUBL.ListarMotivo();
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
        public ActionResult InsertarMotivo(ESS_MotivoGUEntidad model) {
            string mensaje = "No se pudo insertar el registro";
            bool respuesta = false;
            try {
                model.FechaRegistro = DateTime.Now;
                model.UsuarioRegistro = (string)Session["UsuarioNombre"];
                int idInsertado = _essIngresoSalidaGUBL.InsertarMotivo(model);
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
        public ActionResult EditarMotivo(ESS_MotivoGUEntidad model) {
            string mensaje = "No se pudo editar el registro";
            bool respuesta = false;
            try {
                model.FechaModificacion = DateTime.Now;
                model.UsuarioModificacion = (string)Session["UsuarioNombre"];
                respuesta = _essIngresoSalidaGUBL.EditarMotivo(model);
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
        public ActionResult PlantillaIngresosSalidasGUDescargarExcel() {
            string path = Server.MapPath(@"~/Content/ess_excel_plantilla/Plantilla_ESS_IngresoSalidaGU.xlsx"); ;
            string base64String = "";
            string excelName = "";
            string mensaje = "";
            bool respuesta = false;
            try {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                var motivos = _essIngresoSalidaGUBL.ListarMotivoGUPorEstado(1);

                List<string> listaDatosSala = new List<string>();
                List<string> listaDatosMotivo = new List<string>();

                listaDatosSala = salas.Select(x => $"{x.CodSala}-{x.Nombre}").ToList();
                listaDatosMotivo = motivos.Select(x => $"{x.IdMotivo}-{x.Nombre}").ToList();

                // Abrir el archivo existente
                using(var package = new ExcelPackage(new FileInfo(path))) {
                    var workSheet = package.Workbook.Worksheets[0]; // Seleccionar la primera hoja

                    // Crear campos con validacion de datos
                    var validationSala = workSheet.DataValidations.AddListValidation("B5:B30");
                    validationSala.ShowErrorMessage = true;
                    validationSala.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    validationSala.ErrorTitle = "Valor inválido";
                    validationSala.Error = "Seleccione un valor de la lista.";

                    var validationMotivo = workSheet.DataValidations.AddListValidation("H5:H30");
                    validationMotivo.ShowErrorMessage = true;
                    validationMotivo.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    validationMotivo.ErrorTitle = "Valor inválido";
                    validationMotivo.Error = "Seleccione un valor de la lista.";


                    // Crear lista
                    int startRowSala = 2;
                    for(int i = 0; i < listaDatosSala.Count; i++) {
                        workSheet.Cells[startRowSala + i, 26].Value = listaDatosSala[i];
                    }
                    int endRowSala = startRowSala + listaDatosSala.Count - 1;
                    validationSala.Formula.ExcelFormula = $"$Z${startRowSala}:$Z${endRowSala}";
                    workSheet.Column(26).Hidden = true;

                    int startRowMotivo = 2;
                    for(int i = 0; i < listaDatosMotivo.Count; i++) {
                        workSheet.Cells[startRowMotivo + i, 27].Value = listaDatosMotivo[i];
                    }
                    int endRowMotivo = startRowMotivo + listaDatosMotivo.Count - 1;
                    validationMotivo.Formula.ExcelFormula = $"$AA${startRowMotivo}:$AA${endRowMotivo}";
                    workSheet.Column(27).Hidden = true;


                    // Fin de la Validacion

                    byte[] bin = package.GetAsByteArray();
                    base64String = Convert.ToBase64String(bin);
                    excelName = "Plantilla_IngresosSalidasGU_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx";
                    respuesta = true;
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }

            return Json(new { respuesta, mensaje, data = base64String, excelName });
        }




        [HttpPost]
        public ActionResult ImportarExcelIngresosSalidasGU() {
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
                List<BUK_EmpleadoEntidad> listadoGerenteUnidad = new List<BUK_EmpleadoEntidad>();
                listadoGerenteUnidad = _bukEmpleadoBL.ObtenerEmpleadosActivosPorTerminoxCargo(string.Empty, new int[] { 44 });

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
                        worksheet.Cells[4, ColumnaObservacionesImportar].Value = "Observación de la importación";
                        worksheet.Cells[4, ColumnaObservacionesImportar].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[4, ColumnaObservacionesImportar].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 192, 0));
                        worksheet.Cells[4, ColumnaObservacionesImportar].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[4, ColumnaObservacionesImportar].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
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
                            string campoGU = worksheet.Cells[row, 3].Text.Trim(); // Columna C
                            string campoFechaIngreso = worksheet.Cells[row, 4].Text.Trim(); // Columna D
                            string campoHoraIngreso = worksheet.Cells[row, 5].Text.Trim(); // Columna E
                            string campoFechaSalida = worksheet.Cells[row, 6].Text.Trim(); // Columna F
                            string campoHoraSalida = worksheet.Cells[row, 7].Text.Trim(); // Columna G
                            string campoMotivo = worksheet.Cells[row, 8].Text.Trim(); // Columna H
                            string campoDescripcion = worksheet.Cells[row, 9].Text.Trim(); // Columna I
                            string campoObservaciones = worksheet.Cells[row, 10].Text.Trim(); // Columna J

                            // Validaciones
                            if(string.IsNullOrEmpty(campoSala)) {
                                observacionFila.Add("El campo Sala es obligatorio.");
                                ResaltarCelda(worksheet, row, 2); // Resaltar celda B
                            }
                            if(string.IsNullOrEmpty(campoGU)) {
                                observaciones.Add("El campo Gerente de Unidad es obligatorio.");
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
                            if(string.IsNullOrEmpty(campoMotivo)) {
                                observacionFila.Add("El campo Motivo es obligatorio.");
                                ResaltarCelda(worksheet, row, 8); // Resaltar celda H
                            }
                            if(string.IsNullOrEmpty(campoDescripcion)) {
                                observacionFila.Add("El campo Descripción es obligatorio.");
                                ResaltarCelda(worksheet, row, 9); // Resaltar celda I
                            }
                            //if(string.IsNullOrEmpty(campoObservaciones)) { 
                            //    observacionFila.Add("El campo Observaciones es obligatorio.");
                            //    ResaltarCelda(worksheet, row, 10); // Resaltar celda J
                            //}


                            //if (!string.IsNullOrEmpty(campoFecha))
                            //{
                            //    if (!DateTime.TryParse(campoFecha, out _))
                            //    {
                            //        observacionFila.Add("El campo Fecha de Apertura no tiene un formato válido.");
                            //        ResaltarCelda(worksheet, row, 3); // Resaltar celda C
                            //    }
                            //} 

                            //Todo OK
                            if(!observacionFila.Any()) {
                                try {
                                    var registro = new ESS_IngresoSalidaGUEntidad {
                                        CodSala = int.Parse(campoSala.Split('-')[0]),
                                        NombreSala = campoSala.Split('-')[1],

                                        HoraIngreso = DateTime.Parse(DateTime.Parse(campoFechaIngreso).ToString("yyyy-MM-dd")) + TimeSpan.Parse(DateTime.Parse(campoHoraIngreso).ToString("HH:mm:ss")),
                                        HoraSalida = DateTime.Parse(DateTime.Parse(campoFechaSalida).ToString("yyyy-MM-dd")) + TimeSpan.Parse(DateTime.Parse(campoHoraSalida).ToString("HH:mm:ss")),
                                        IdMotivo = int.Parse(campoMotivo.Split('-')[0]),
                                        NombreMotivo = campoMotivo.Split('-')[1],

                                        Descripcion = campoDescripcion,
                                        Observaciones = campoObservaciones,
                                        FechaRegistro = DateTime.Now,
                                        UsuarioRegistro = (string)Session["UsuarioNombre"],
                                    };

                                    var observacionFila_BUK = new List<string>();
                                    var ExisteGerenteUnidad = listadoGerenteUnidad.FirstOrDefault(x => x.NumeroDocumento == campoGU);
                                    if(ExisteGerenteUnidad == null) {
                                        observacionFila_BUK.Add("No se encontró al Gerente Unidad.");
                                    }

                                    if(!observacionFila_BUK.Any()) {
                                        registro.Empleados = new List<ESS_IngresoSalidaGUEmpleadoEntidad>
                                        {
                                            new ESS_IngresoSalidaGUEmpleadoEntidad
                                            {
                                                IdEmpleado = ExisteGerenteUnidad.IdBuk,
                                                Nombre = ExisteGerenteUnidad.Nombres,
                                                ApellidoPaterno = ExisteGerenteUnidad.ApellidoPaterno,
                                                ApellidoMaterno = ExisteGerenteUnidad.ApellidoMaterno,
                                                TipoDocumento = ExisteGerenteUnidad.TipoDocumento,
                                                DocumentoRegistro = ExisteGerenteUnidad.NumeroDocumento,
                                                NombreCargo = "Gerente de Unidad"
                                            }
                                        };

                                        int insertedId = _essIngresoSalidaGUBL.GuardarIngresoSalidaGU(registro);
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

                                    } else {
                                        string obsMsg = string.Join(" | ", observacionFila_BUK);
                                        observaciones.Add(new { Fila = row, Observaciones = obsMsg });

                                        var richText = worksheet.Cells[row, ColumnaObservacionesImportar].RichText;
                                        var prefijo = richText.Add("❌ ");
                                        prefijo.Color = System.Drawing.Color.Red;
                                        var mensajetexto = richText.Add(obsMsg);
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