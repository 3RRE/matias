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
    public class AperturaCierreSalaController : Controller {
        ESS_AperturaCierreSalaBL essAperturaCierreSalaBL = new ESS_AperturaCierreSalaBL();
        private readonly SalaBL _salaBl = new SalaBL();

        public ActionResult AperturaCierreSalaVista() {
            return View("~/Views/EntradaSalidaSala/AperturaCierreSala.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult AperturaCierreSalaListarJson(int[] codsala, DateTime fechaini, DateTime fechafin) {
            string mensaje = "No se encontraron registros";
            bool respuesta = false;
            List<ESS_AperturaCierreSalaEntidad> listaESS_AperturaCierreSala = new List<ESS_AperturaCierreSalaEntidad>();

            try {
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                int[] salasBusqueda;
                if(codsala.Contains(-1)) {
                    salasBusqueda = salas.Select(x => x.CodSala).ToArray();
                } else {
                    salasBusqueda = codsala;
                }
                listaESS_AperturaCierreSala = essAperturaCierreSalaBL.ListarRegistroAperturaCierreSala(salasBusqueda, fechaini, fechafin);

                if(listaESS_AperturaCierreSala != null && listaESS_AperturaCierreSala.Count > 0) {
                    mensaje = "Registros obtenidos";
                    respuesta = true;

                    int horasLimite = int.Parse(System.Configuration.ConfigurationManager.AppSettings["FechaLimiteEditarAperturaCierreSala"]);

                    foreach(var registro in listaESS_AperturaCierreSala) {

                        if(registro.Fecha != null && registro.HoraCierre != null) {

                            DateTime fechaCierre = registro.Fecha.Add(registro.HoraCierre);
                            DateTime fechaLimite = fechaCierre.AddHours(horasLimite);


                            registro.FechaLimiteEditarAperturaCierreSala = fechaLimite;
                        } else {
                            registro.FechaLimiteEditarAperturaCierreSala = null;
                        }
                    }
                }
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, comunicarse con el administrador";
                Console.WriteLine(exp.Message);
            }

            var oRespuesta = new {
                mensaje,
                respuesta,
                data = listaESS_AperturaCierreSala
            };

            var serializer = new JavaScriptSerializer {
                MaxJsonLength = Int32.MaxValue
            };

            var result = new ContentResult {
                Content = serializer.Serialize(oRespuesta),
                ContentType = "application/json"
            };

            return result;
        }




        [HttpPost]
        public ActionResult GuardarAperturaCierreSala(ESS_AperturaCierreSalaEntidad aperturaCierre) {
            bool respuesta = false;
            string mensaje = "No se pudo guardar el registro.";

            try {
                aperturaCierre.FechaRegistro = DateTime.Now;
                aperturaCierre.UsuarioRegistro = (string)Session["UsuarioNombre"];

                int idInsertado = essAperturaCierreSalaBL.GuardarRegistroAperturaCierreSala(aperturaCierre);

                if(idInsertado > 0) {
                    respuesta = true;
                    mensaje = "Registro guardado correctamente.";
                }
            } catch(Exception ex) {
                mensaje = $"Error: {ex.Message}";
                Console.WriteLine(ex.StackTrace);
            }

            return Json(new { respuesta, mensaje });
        }


        [HttpPost]
        public ActionResult EditarAperturaCierreSala(ESS_AperturaCierreSalaEntidad registro) {
            bool respuesta = false;
            string mensaje = "No se pudo actualizar los datos";

            try {
                DateTime? fechaCierre = essAperturaCierreSalaBL.ObtenerFechaHoraCierrePorId(registro.IdAperturaCierreSala);

                if(fechaCierre == null) {
                    registro.UsuarioModificacion = (string)Session["UsuarioNombre"];
                    respuesta = essAperturaCierreSalaBL.ActualizarAperturaCierreSala(registro);

                    if(respuesta) {
                        mensaje = "Registro actualizado correctamente.";
                    }
                } else {
                    int horasLimite = int.Parse(System.Configuration.ConfigurationManager.AppSettings["FechaLimiteEditarAperturaCierreSala"]);

                    DateTime fechaLimite = fechaCierre.Value.AddHours(horasLimite);

                    if(DateTime.Now > fechaLimite) {
                        mensaje = "La fecha límite para editar este registro ha sido superada.";
                    } else {
                        registro.UsuarioModificacion = (string)Session["UsuarioNombre"];
                        respuesta = essAperturaCierreSalaBL.ActualizarAperturaCierreSala(registro);

                        if(respuesta) {
                            mensaje = "Registro actualizado correctamente.";
                        }
                    }
                }
            } catch(Exception ex) {
                mensaje = $"Error: {ex.Message}";
            }

            return Json(new { respuesta, mensaje });
        }

        [HttpPost]
        public ActionResult FinalizarAperturaCierreSala(ESS_AperturaCierreSalaEntidad registro) {
            bool respuesta = false;
            string mensaje = "No se pudo actualizar los datos";

            try {
                DateTime? fechaCierre = essAperturaCierreSalaBL.ObtenerFechaHoraCierrePorId(registro.IdAperturaCierreSala);

                if(fechaCierre == null) {
                    registro.UsuarioModificacion = (string)Session["UsuarioNombre"];
                    respuesta = essAperturaCierreSalaBL.FinalizarRegistroAperturaCierreSala(registro);

                    if(respuesta) {
                        mensaje = "Registro actualizado correctamente.";
                    }
                } else {
                    int horasLimite = int.Parse(System.Configuration.ConfigurationManager.AppSettings["FechaLimiteEditarAperturaCierreSala"]);

                    DateTime fechaLimite = fechaCierre.Value.AddHours(horasLimite);

                    if(DateTime.Now > fechaLimite) {
                        mensaje = "La fecha límite para editar este registro ha sido superada.";
                    } else {
                        registro.UsuarioModificacion = (string)Session["UsuarioNombre"];
                        respuesta = essAperturaCierreSalaBL.FinalizarRegistroAperturaCierreSala(registro);

                        if(respuesta) {
                            mensaje = "Registro actualizado correctamente.";
                        }
                    }
                }
            } catch(Exception ex) {
                mensaje = $"Error: {ex.Message}";
            }

            return Json(new { respuesta, mensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public bool EsFechaLimiteSuperada(ESS_AperturaCierreSalaEntidad registro) {
            int horasLimite = int.Parse(System.Configuration.ConfigurationManager.AppSettings["FechaLimiteEditarAperturaCierreSala"]);

            DateTime fechaCierreCompleta = registro.Fecha.Add(registro.HoraCierre);
            DateTime fechaLimite = fechaCierreCompleta.AddHours(horasLimite);

            return DateTime.Now > fechaLimite;
        }
        [HttpPost]
        public ActionResult EliminarAperturaCierreSala(int Id) {
            bool respuesta = false;
            string mensaje = "No se pudo eliminar el registro";

            try {
                respuesta = essAperturaCierreSalaBL.EliminarRegistroAperturaCierreSala(new ESS_AperturaCierreSalaEntidad { IdAperturaCierreSala = Id });

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
        public ActionResult ListarEmpleadoBUKcargo() {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_AperturaCierreSalaPersonaEntidad>();

            try {
                data = essAperturaCierreSalaBL.ListarEmpleadoBUKcargo();
                mensaje = "Listando registros";
                respuesta = true;
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, contacte con administrador";
                respuesta = false;
            }

            return Json(new { mensaje, respuesta, data });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarEmpleadoBUKcargoPrevencionista() {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_AperturaCierreSalaPersonaEntidad>();

            try {
                data = essAperturaCierreSalaBL.ListarEmpleadoBUKcargo().Where(x => x.IdCargo == 74).ToList();
                mensaje = "Listando registros de Prevencionistas";
                respuesta = true;
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, contacte con administrador";
                respuesta = false;
            }

            return Json(new { mensaje, respuesta, data });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarEmpleadoBUKcargoJefeDeSala() {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_AperturaCierreSalaPersonaEntidad>();

            try {
                data = essAperturaCierreSalaBL.ListarEmpleadoBUKcargo().Where(x => x.IdCargo == 62).ToList();
                mensaje = "Listando registros de Jefes de Sala";
                respuesta = true;
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, contacte con administrador";
                respuesta = false;
            }

            return Json(new { mensaje, respuesta, data });
        }



        [HttpPost]
        public ActionResult ReporteAperturaCierreSalaXSalaJsonExcel(int[] codsala, DateTime fechaini, DateTime fechafin) {
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

                List<ESS_AperturaCierreSalaEntidad> lista = essAperturaCierreSalaBL.ListarRegistroAperturaCierreSala(salasBusqueda, fechaini, fechafin);

                lista = lista.OrderByDescending(x => x.IdAperturaCierreSala).ToList();

                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();


                    var workSheet = excel.Workbook.Worksheets.Add("Apertura y Cierre de Sala");
                    workSheet.TabColor = Color.Black;
                    workSheet.DefaultRowHeight = 12;


                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[2, 2, 2, 12].Merge = true;
                    workSheet.Cells[2, 2].Value = "Reporte de Apertura y Cierre de Sala";
                    workSheet.Cells[2, 2].Style.Font.Bold = true;
                    workSheet.Cells[2, 2].Style.Font.Size = 13;
                    workSheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Sala";
                    workSheet.Cells[3, 4].Value = "Fecha";
                    workSheet.Cells[3, 5].Value = "Apertura (Hora)";
                    workSheet.Cells[3, 6].Value = "Apertura (Prevencionista)";
                    workSheet.Cells[3, 7].Value = "Apertura (Jefe de Sala)";
                    workSheet.Cells[3, 8].Value = "Apertura (Observaciones)";
                    workSheet.Cells[3, 9].Value = "Cierre (Hora)";
                    workSheet.Cells[3, 10].Value = "Cierre (Prevencionista)";
                    workSheet.Cells[3, 11].Value = "Cierre (Jefe de Sala)";
                    workSheet.Cells[3, 12].Value = "Cierre (Observaciones)";

                    int recordIndex = 4;
                    foreach(var item in lista) {
                        workSheet.Cells[recordIndex, 2].Value = item.IdAperturaCierreSala;
                        workSheet.Cells[recordIndex, 3].Value = item.Sala;
                        workSheet.Cells[recordIndex, 4].Value = item.Fecha.ToString("dd-MM-yyyy");
                        workSheet.Cells[recordIndex, 5].Value = item.HoraApertura.ToString(@"hh\:mm");
                        workSheet.Cells[recordIndex, 6].Value = item.PrevencionistaApertura;
                        workSheet.Cells[recordIndex, 7].Value = item.JefeSalaApertura;
                        workSheet.Cells[recordIndex, 8].Value = string.IsNullOrEmpty(item.ObservacionesApertura) ? "No definido" : item.ObservacionesApertura;
                        workSheet.Column(8).Width = 40;
                        workSheet.Cells[recordIndex, 8].Style.WrapText = true;



                        //workSheet.Cells[recordIndex, 9].Value = item.HoraCierre.ToString(@"hh\:mm");
                        if(item.HoraCierre == TimeSpan.Zero) {
                            workSheet.Cells[recordIndex, 9].Value = "No definido";
                        } else {
                            workSheet.Cells[recordIndex, 9].Value = item.HoraCierre.ToString(@"hh\:mm");
                        }

                        workSheet.Cells[recordIndex, 10].Value = string.IsNullOrEmpty(item.PrevencionistaCierre) ? "No definido" : item.PrevencionistaCierre;
                        workSheet.Cells[recordIndex, 11].Value = string.IsNullOrEmpty(item.JefeSalaCierre) ? "No definido" : item.JefeSalaCierre;
                        workSheet.Cells[recordIndex, 12].Value = string.IsNullOrEmpty(item.ObservacionesCierre) ? "No definido" : item.ObservacionesCierre;

                        workSheet.Column(12).Width = 40;
                        workSheet.Cells[recordIndex, 12].Style.WrapText = true;

                        recordIndex++;
                    }

                    Color principalHeaderBackground = ColorTranslator.FromHtml("#003268");
                    workSheet.Cells["B3:L3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:L3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:L3"].Style.Fill.BackgroundColor.SetColor(principalHeaderBackground);
                    workSheet.Cells["B3:L3"].Style.Font.Color.SetColor(Color.White);

                    for(int i = 2; i <= 12; i++) {
                        if(i != 8 && i != 12) {
                            workSheet.Column(i).AutoFit();
                        }
                    }

                    excelName = $"ReporteAperturaCierreSala_{fechaini:dd_MM_yyyy}_al_{fechafin:dd_MM_yyyy}.xlsx";

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
        public ActionResult PlantillaAperturaCierreSalaDescargarExcel() {
            string path = Server.MapPath(@"~/Content/ess_excel_plantilla/Plantilla_ESS_AperturaCierreSala.xlsx"); ;
            string base64String = "";
            string excelName = "";
            string mensaje = "";
            bool respuesta = false;
            try {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);

                List<string> listaDatosSala = new List<string>();

                listaDatosSala = salas.Select(x => $"{x.CodSala}-{x.Nombre}").ToList();


                // Abrir el archivo existente
                using(var package = new ExcelPackage(new FileInfo(path))) {
                    var workSheet = package.Workbook.Worksheets[0]; // Seleccionar la primera hoja

                    // Crear campos con validacion de datos
                    var validationSala = workSheet.DataValidations.AddListValidation("B5:B30");
                    validationSala.ShowErrorMessage = true;
                    validationSala.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    validationSala.ErrorTitle = "Valor inválido";
                    validationSala.Error = "Seleccione un valor de la lista.";

                    // Crear lista
                    int startRowSala = 2;
                    for(int i = 0; i < listaDatosSala.Count; i++) {
                        workSheet.Cells[startRowSala + i, 26].Value = listaDatosSala[i];
                    }
                    int endRowSala = startRowSala + listaDatosSala.Count - 1;
                    validationSala.Formula.ExcelFormula = $"$Z${startRowSala}:$Z${endRowSala}";
                    workSheet.Column(26).Hidden = true;

                    // Fin de la Validacion

                    byte[] bin = package.GetAsByteArray();
                    base64String = Convert.ToBase64String(bin);
                    excelName = "Plantilla_AperturaCierreSala_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx";
                    respuesta = true;
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }

            return Json(new { respuesta, mensaje, data = base64String, excelName });
        }


        [HttpPost]
        public ActionResult ImportarExcelAperturaCierreSala() {
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
                var listadoEmpleadobuk = new List<ESS_AperturaCierreSalaPersonaEntidad>();
                var listadoJefe = new List<ESS_AperturaCierreSalaPersonaEntidad>();
                var listadoPrevencionista = new List<ESS_AperturaCierreSalaPersonaEntidad>();

                listadoEmpleadobuk = essAperturaCierreSalaBL.ListarEmpleadoBUKcargo();
                listadoJefe = listadoEmpleadobuk.Where(x => x.IdCargo == 62).ToList();
                listadoPrevencionista = listadoEmpleadobuk.Where(x => x.IdCargo == 74).ToList();

                string fechaHora = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string nombreProcesado = $"Procesado_{fechaHora}_{nombreOriginal}";
                using(var stream = archivoExcel.InputStream) {
                    using(var package = new ExcelPackage(stream)) {
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                        if(worksheet == null)
                            return Json(new { respuesta = false, mensaje = "El archivo no contiene hojas válidas." });

                        int ColumnaObservacionesImportar = 12;
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
                            for(int col = 2; col <= 11; col++) {
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
                            string campoFecha = worksheet.Cells[row, 3].Text.Trim(); // Columna C

                            string campoHoraApertura = worksheet.Cells[row, 4].Text.Trim(); // Columna D
                            string campoJefeSalaApertura = worksheet.Cells[row, 5].Text.Trim(); // Columna E
                            string campoPrevencionistaApertura = worksheet.Cells[row, 6].Text.Trim(); // Columna F
                            string campoObservacionesApertura = worksheet.Cells[row, 7].Text.Trim(); // Columna G

                            string campoHoraCierre = worksheet.Cells[row, 8].Text.Trim(); // Columna H
                            string campoJefeSalaCierre = worksheet.Cells[row, 9].Text.Trim(); // Columna I
                            string campoPrevencionistaCierre = worksheet.Cells[row, 10].Text.Trim(); // Columna J
                            string campoObservacionesCierre = worksheet.Cells[row, 11].Text.Trim(); // Columna K

                            // Validaciones
                            if(string.IsNullOrEmpty(campoSala)) {
                                observacionFila.Add("El campo Sala es obligatorio.");
                                ResaltarCelda(worksheet, row, 2); // Resaltar celda B
                            }
                            if(string.IsNullOrEmpty(campoFecha)) {
                                observacionFila.Add("El campo Fecha de Apertura/Cierre es obligatorio.");
                                ResaltarCelda(worksheet, row, 3); // Resaltar celda C
                            }
                            if(string.IsNullOrEmpty(campoHoraApertura)) {
                                observacionFila.Add("El campo Hora de Apertura es obligatorio.");
                                ResaltarCelda(worksheet, row, 4); // Resaltar celda D
                            }
                            if(string.IsNullOrEmpty(campoJefeSalaApertura)) {
                                observacionFila.Add("El campo Jefe de Sala (Apertura) es obligatorio.");
                                ResaltarCelda(worksheet, row, 5); // Resaltar celda E
                            }
                            if(string.IsNullOrEmpty(campoPrevencionistaApertura)) {
                                observacionFila.Add("El campo Prevencionista (Apertura) es obligatorio.");
                                ResaltarCelda(worksheet, row, 6); // Resaltar celda F
                            }
                            //if(string.IsNullOrEmpty(campoObservacionesApertura)) { 
                            //    observacionFila.Add("El campo Observaciones (Apertura) es obligatorio.");
                            //    ResaltarCelda(worksheet, row, 7); // Resaltar celda G
                            //}
                            if(string.IsNullOrEmpty(campoHoraCierre)) {
                                observacionFila.Add("El campo Hora de Cierre es obligatorio.");
                                ResaltarCelda(worksheet, row, 8); // Resaltar celda H
                            }
                            if(string.IsNullOrEmpty(campoJefeSalaApertura)) {
                                observacionFila.Add("El campo Jefe de Sala (Cierre) es obligatorio.");
                                ResaltarCelda(worksheet, row, 9); // Resaltar celda I
                            }
                            if(string.IsNullOrEmpty(campoPrevencionistaCierre)) {
                                observacionFila.Add("El campo Prevencionista (Cierre) es obligatorio.");
                                ResaltarCelda(worksheet, row, 10); // Resaltar celda J
                            }
                            //if(string.IsNullOrEmpty(campoObservacionesCierre)) { 
                            //    observacionFila.Add("El campo Observaciones (Cierre) es obligatorio.");
                            //    ResaltarCelda(worksheet, row, 11); // Resaltar celda K
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
                                    var registro = new ESS_AperturaCierreSalaEntidad {
                                        CodSala = int.Parse(campoSala.Split('-')[0]),
                                        Sala = campoSala.Split('-')[1],
                                        Fecha = DateTime.Parse(DateTime.Parse(campoFecha).ToString("yyyy-MM-dd")),

                                        HoraApertura = TimeSpan.Parse(DateTime.Parse(campoHoraApertura).ToString("HH:mm:ss")),
                                        JefeSalaApertura = campoJefeSalaApertura,
                                        PrevencionistaApertura = campoPrevencionistaApertura,
                                        ObservacionesApertura = campoObservacionesApertura,

                                        HoraCierre = TimeSpan.Parse(DateTime.Parse(campoHoraCierre).ToString("HH:mm:ss")),
                                        JefeSalaCierre = campoJefeSalaCierre,
                                        PrevencionistaCierre = campoPrevencionistaCierre,
                                        ObservacionesCierre = campoObservacionesCierre,

                                        FechaRegistro = DateTime.Now,
                                        UsuarioRegistro = (string)Session["UsuarioNombre"],
                                    };

                                    var observacionFila_BUK = new List<string>();

                                    var ExisteEmpleadoJefeApertura = listadoJefe.FirstOrDefault(x => x.NumeroDocumento == campoJefeSalaApertura);
                                    var ExisteEmpleadoPrevencionistaApertura = listadoPrevencionista.FirstOrDefault(x => x.NumeroDocumento == campoPrevencionistaApertura);
                                    var ExisteEmpleadoJefeCierre = listadoJefe.FirstOrDefault(x => x.NumeroDocumento == campoJefeSalaCierre);
                                    var ExisteEmpleadoPrevencionistaCierre = listadoPrevencionista.FirstOrDefault(x => x.NumeroDocumento == campoPrevencionistaCierre);

                                    if(ExisteEmpleadoJefeApertura == null) {
                                        observacionFila_BUK.Add("No se encontró al Jefe de Sala (Apertura).");
                                    }
                                    if(ExisteEmpleadoPrevencionistaApertura == null) {
                                        observacionFila_BUK.Add("No se encontró al Prevencionista (Apertura).");
                                    }
                                    if(ExisteEmpleadoJefeCierre == null) {
                                        observacionFila_BUK.Add("No se encontró al Jefe de Sala (Cierre).");
                                    }
                                    if(ExisteEmpleadoPrevencionistaCierre == null) {
                                        observacionFila_BUK.Add("No se encontró al Prevencionista (Cierre).");
                                    }

                                    if(!observacionFila_BUK.Any()) {
                                        //Empleados Apertura
                                        registro.IdJefeSalaApertura = ExisteEmpleadoJefeApertura.IdBuk;
                                        registro.JefeSalaApertura = ExisteEmpleadoJefeApertura.NombreCompleto;

                                        registro.IdPrevencionistaApertura = ExisteEmpleadoPrevencionistaApertura.IdBuk;
                                        registro.PrevencionistaApertura = ExisteEmpleadoPrevencionistaApertura.NombreCompleto;

                                        //Empleados Cierre
                                        registro.IdJefeSalaCierre = ExisteEmpleadoJefeCierre.IdBuk;
                                        registro.JefeSalaCierre = ExisteEmpleadoJefeCierre.NombreCompleto;

                                        registro.IdPrevencionistaCierre = ExisteEmpleadoPrevencionistaCierre.IdBuk;
                                        registro.PrevencionistaCierre = ExisteEmpleadoPrevencionistaCierre.NombreCompleto;

                                        int insertedId = essAperturaCierreSalaBL.GuardarRegistroAperturaCierreSala_Importar(registro);
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
