using CapaDatos.Utilitarios;
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
    public class LogsOcurrenciasController : Controller {
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
        private readonly ESS_LogOcurrenciaBL _essLogOcurrenciaBL = new ESS_LogOcurrenciaBL();
        public ActionResult LogsOcurrenciasVista() {
            return View("~/Views/EntradaSalidaSala/LogOcurrencia.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarLogsOcurrenciaxSalaJson(int[] codsala, DateTime fechaini, DateTime fechafin) {
            string mensaje = "No se encontraton registros";
            bool respuesta = false;
            List<ESS_LogOcurrenciaEntidad> listaESS_LogOcurrencia = new List<ESS_LogOcurrenciaEntidad>();
            try {
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                int[] salasBusqueda;
                if(codsala.Contains(-1)) {
                    salasBusqueda = salas.Select(x => x.CodSala).ToArray();
                } else {
                    salasBusqueda = codsala;
                }
                listaESS_LogOcurrencia = _essLogOcurrenciaBL.ListadoLogsOcurrencia(salasBusqueda, fechaini, fechafin);
                mensaje = "Registros obtenidos";
                respuesta = true;
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, comunicarse con administrador";
            }
            var oRespuesta = new {
                mensaje,
                respuesta,
                data = listaESS_LogOcurrencia
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
        public ActionResult GuardarLogsOcurrencias(ESS_LogOcurrenciaEntidad ingresosalidagu) {
            bool respuesta = false;

            string mensaje = "No se pudo guardar los datos";

            try {
                ingresosalidagu.FechaRegistro = DateTime.Now;
                ingresosalidagu.UsuarioRegistro = (string)Session["UsuarioNombre"];

                int insertedId = _essLogOcurrenciaBL.GuardarLogOcurrencia(ingresosalidagu);
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
        public ActionResult EditarLogsOcurrencias(ESS_LogOcurrenciaEntidad ingresosalidagu) {
            bool respuesta = false;
            string mensaje = "No se pudo actualizar los datos";

            try {

                ingresosalidagu.FechaModificacion = DateTime.Now;
                ingresosalidagu.UsuarioModificacion = (string)Session["UsuarioNombre"];

                respuesta = _essLogOcurrenciaBL.ActualizarLogOcurrencia(ingresosalidagu);
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
        public JsonResult EliminarLogsOcurrencias(int id) {
            bool respuesta = false;
            string mensaje = "No se pudo eliminar el registro.";

            try {
                respuesta = _essLogOcurrenciaBL.EliminarLogOcurrencia(id);
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
        public ActionResult ListarAreaPorEstado(int estado = 1) {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_AreaEntidad>();
            try {

                data = _essLogOcurrenciaBL.ListarAreaPorEstado(estado);
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
        public ActionResult ListarTipologiaPorEstado(int estado = 1) {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_TipologiaEntidad>();
            try {

                data = _essLogOcurrenciaBL.ListarTipologiaPorEstado(estado);
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
        public ActionResult ListarActuantePorEstado(int estado = 1) {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_ActuanteEntidad>();
            try {

                data = _essLogOcurrenciaBL.ListarActuantePorEstado(estado);
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
        public ActionResult ListarComunicacionPorEstado(int estado = 1) {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_ComunicacionEntidad>();
            try {

                data = _essLogOcurrenciaBL.ListarComunicacionPorEstado(estado);
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
        public ActionResult ListarEstadoOcurrenciaPorEstado(int estado = 1) {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_EstadoOcurrenciaEntidad>();
            try {

                data = _essLogOcurrenciaBL.ListarEstadoOcurrenciaPorEstado(estado);
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
        public ActionResult ReporteLogsOcurrenciasDescargarExcelJson(int[] codSala, DateTime fechaIni, DateTime fechaFin) {
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
                List<ESS_LogOcurrenciaEntidad> lista = _essLogOcurrenciaBL.ListadoLogsOcurrencia(salasBusqueda, fechaIni, fechaFin);
                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using(ExcelPackage excel = new ExcelPackage()) {

                        var workSheet = excel.Workbook.Worksheets.Add("Log de Ocurrencia");
                        workSheet.TabColor = Color.Black;
                        workSheet.DefaultRowHeight = 12;

                        // Encabezado principal
                        workSheet.Row(3).Height = 20;
                        workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet.Row(3).Style.Font.Bold = true;
                        workSheet.Cells[2, 1, 2, 11].Merge = true;
                        workSheet.Cells[2, 1].Value = "Reporte de Log de Ocurrencia";
                        workSheet.Cells[2, 1].Style.Font.Bold = true;
                        workSheet.Cells[2, 1].Style.Font.Size = 13;
                        workSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;



                        workSheet.Cells[3, 1].Value = "ID";
                        workSheet.Cells[3, 2].Value = "Sala";
                        workSheet.Cells[3, 3].Value = "Área";
                        workSheet.Cells[3, 4].Value = "Tipología";
                        workSheet.Cells[3, 5].Value = "Actuante";
                        workSheet.Cells[3, 6].Value = "Comunicacion";
                        workSheet.Cells[3, 7].Value = "Detalle";
                        workSheet.Cells[3, 8].Value = "Acción Ejecutada";
                        workSheet.Cells[3, 9].Value = "Estado Ocurrencia";
                        workSheet.Cells[3, 10].Value = "Fecha Ocurrencia";
                        workSheet.Cells[3, 11].Value = "Fecha Solución";
                        workSheet.Column(7).Width = 40;
                        workSheet.Column(8).Width = 40;

                        int rowIndex = 4;
                        foreach(var item in lista) {
                            workSheet.Cells[rowIndex, 1].Value = item.IdLogOcurrencia;
                            workSheet.Cells[rowIndex, 2].Value = item.NombreSala;
                            workSheet.Cells[rowIndex, 3].Value = item.IdArea == -1 ? "(" + item.NombreArea + ") " + item.DescripcionArea : item.NombreArea;
                            workSheet.Cells[rowIndex, 4].Value = item.IdTipologia == -1 ? "(" + item.NombreTipologia + ") " + item.DescripcionTipologia : item.NombreTipologia;
                            workSheet.Cells[rowIndex, 5].Value = item.IdActuante == -1 ? "(" + item.NombreActuante + ") " + item.DescripcionActuante : item.NombreActuante;
                            workSheet.Cells[rowIndex, 6].Value = item.IdComunicacion == -1 ? "(" + item.NombreComunicacion + ") " + item.DescripcionComunicacion : item.NombreComunicacion;
                            workSheet.Cells[rowIndex, 7].Value = item.Detalle;
                            workSheet.Cells[rowIndex, 8].Value = item.AccionEjecutada;
                            workSheet.Cells[rowIndex, 9].Value = item.EstadoOcurrencia;
                            workSheet.Cells[rowIndex, 10].Value = item.Fecha.ToString("dd-MM-yyyy") == "01-01-1753" ? "No definido" : item.Fecha.ToString("dd-MM-yyyy hh:mm tt");
                            workSheet.Cells[rowIndex, 11].Value = item.FechaSolucion.ToString("dd-MM-yyyy") == "01-01-1753" ? "No definido" : item.FechaSolucion.ToString("dd-MM-yyyy hh:mm tt");
                            rowIndex++;
                        }

                        Color principalHeaderBackground = ColorTranslator.FromHtml("#003268");
                        workSheet.Cells["A3:K3"].Style.Font.Bold = true;
                        workSheet.Cells["A3:K3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells["A3:K3"].Style.Fill.BackgroundColor.SetColor(principalHeaderBackground);
                        workSheet.Cells["A3:K3"].Style.Font.Color.SetColor(Color.White);

                        for(int i = 1; i <= 11; i++) {
                            if(i != 7 || i != 8) {
                                workSheet.Column(i).AutoFit();
                            }
                        }
                        excelName = $"ReporteLogsOcurrencia_{fechaIni:dd_MM_yyyy}_al_{fechaFin:dd_MM_yyyy}.xlsx";


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


        #region ESS_Area
        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarArea() {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_AreaEntidad>();
            try {
                data = _essLogOcurrenciaBL.ListarArea();
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
        public ActionResult InsertarArea(ESS_AreaEntidad model) {
            string mensaje = "No se pudo insertar el registro";
            bool respuesta = false;
            try {
                model.FechaRegistro = DateTime.Now;
                model.UsuarioRegistro = (string)Session["UsuarioNombre"];
                int idInsertado = _essLogOcurrenciaBL.InsertarArea(model);
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
        public ActionResult EditarArea(ESS_AreaEntidad model) {
            string mensaje = "No se pudo editar el registro";
            bool respuesta = false;
            try {
                model.FechaModificacion = DateTime.Now;
                model.UsuarioModificacion = (string)Session["UsuarioNombre"];
                respuesta = _essLogOcurrenciaBL.EditarArea(model);
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
        #endregion

        #region ESS_Tipologia
        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarTipologia() {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_TipologiaEntidad>();
            try {
                data = _essLogOcurrenciaBL.ListarTipologia();
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
        public ActionResult InsertarTipologia(ESS_TipologiaEntidad model) {
            string mensaje = "No se pudo insertar el registro";
            bool respuesta = false;
            try {
                model.FechaRegistro = DateTime.Now;
                model.UsuarioRegistro = (string)Session["UsuarioNombre"];
                int idInsertado = _essLogOcurrenciaBL.InsertarTipologia(model);
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
        public ActionResult EditarTipologia(ESS_TipologiaEntidad model) {
            string mensaje = "No se pudo editar el registro";
            bool respuesta = false;
            try {
                model.FechaModificacion = DateTime.Now;
                model.UsuarioModificacion = (string)Session["UsuarioNombre"];
                respuesta = _essLogOcurrenciaBL.EditarTipologia(model);
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
        #endregion 

        #region ESS_Actuante
        [seguridad(false)]

        [HttpPost]
        public ActionResult ListarActuante() {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_ActuanteEntidad>();
            try {
                data = _essLogOcurrenciaBL.ListarActuante();
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
        public ActionResult InsertarActuante(ESS_ActuanteEntidad model) {
            string mensaje = "No se pudo insertar el registro";
            bool respuesta = false;
            try {
                model.FechaRegistro = DateTime.Now;
                model.UsuarioRegistro = (string)Session["UsuarioNombre"];
                int idInsertado = _essLogOcurrenciaBL.InsertarActuante(model);
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
        public ActionResult EditarActuante(ESS_ActuanteEntidad model) {
            string mensaje = "No se pudo editar el registro";
            bool respuesta = false;
            try {
                model.FechaModificacion = DateTime.Now;
                model.UsuarioModificacion = (string)Session["UsuarioNombre"];
                respuesta = _essLogOcurrenciaBL.EditarActuante(model);
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
        #endregion
        #region ESS_Comunicacion
        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarComunicacion() {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_ComunicacionEntidad>();
            try {
                data = _essLogOcurrenciaBL.ListarComunicacion();
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
        public ActionResult InsertarComunicacion(ESS_ComunicacionEntidad model) {
            string mensaje = "No se pudo insertar el registro";
            bool respuesta = false;
            try {
                model.FechaRegistro = DateTime.Now;
                model.UsuarioRegistro = (string)Session["UsuarioNombre"];
                int idInsertado = _essLogOcurrenciaBL.InsertarComunicacion(model);
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
        public ActionResult EditarComunicacion(ESS_ComunicacionEntidad model) {
            string mensaje = "No se pudo editar el registro";
            bool respuesta = false;
            try {
                model.FechaModificacion = DateTime.Now;
                model.UsuarioModificacion = (string)Session["UsuarioNombre"];
                respuesta = _essLogOcurrenciaBL.EditarComunicacion(model);
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
        #endregion


        [seguridad(false)]
        [HttpPost]
        public ActionResult PlantillaLogsOcurrenciasDescargarExcel() {
            string path = Server.MapPath(@"~/Content/ess_excel_plantilla/Plantilla_ESS_LogOcurrencia.xlsx"); ;
            string base64String = "";
            string excelName = "";
            string mensaje = "";
            bool respuesta = false;
            try {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                var areas = _essLogOcurrenciaBL.ListarAreaPorEstado(1);
                var tipologias = _essLogOcurrenciaBL.ListarTipologiaPorEstado(1);
                var actuantes = _essLogOcurrenciaBL.ListarActuantePorEstado(1);
                var comunicaciones = _essLogOcurrenciaBL.ListarComunicacionPorEstado(1);
                var estadoOcurrencias = _essLogOcurrenciaBL.ListarEstadoOcurrenciaPorEstado(1);
                List<string> listaDatosSala = new List<string>();
                List<string> listaDatosArea = new List<string>();
                List<string> listaDatosTipologia = new List<string>();
                List<string> listaDatosActuante = new List<string>();
                List<string> listaDatosComunicacion = new List<string>();
                List<string> listaDatosEstadoOcurrencia = new List<string>();

                listaDatosSala = salas.Select(x => $"{x.CodSala}-{x.Nombre}").ToList();
                listaDatosArea = areas.Select(x => $"{x.IdArea}-{x.Nombre}").ToList();
                listaDatosTipologia = tipologias.Select(x => $"{x.IdTipologia}-{x.Nombre}").ToList();
                listaDatosActuante = actuantes.Select(x => $"{x.IdActuante}-{x.Nombre}").ToList();
                listaDatosComunicacion = comunicaciones.Select(x => $"{x.IdComunicacion}-{x.Nombre}").ToList();
                listaDatosEstadoOcurrencia = estadoOcurrencias.Select(x => $"{x.IdEstadoOcurrencia}-{x.Nombre}").ToList();

                // Abrir el archivo existente
                using(var package = new ExcelPackage(new FileInfo(path))) {
                    var workSheet = package.Workbook.Worksheets[0]; // Seleccionar la primera hoja

                    // Crear campos con validacion de datos
                    var validationSala = workSheet.DataValidations.AddListValidation("B5:B30");
                    validationSala.ShowErrorMessage = true;
                    validationSala.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    validationSala.ErrorTitle = "Valor inválido";
                    validationSala.Error = "Seleccione un valor de la lista.";

                    var validationArea = workSheet.DataValidations.AddListValidation("E5:E30");
                    validationArea.ShowErrorMessage = true;
                    validationArea.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    validationArea.ErrorTitle = "Valor inválido";
                    validationArea.Error = "Seleccione un valor de la lista.";

                    var validationTipologia = workSheet.DataValidations.AddListValidation("F5:F30");
                    validationTipologia.ShowErrorMessage = true;
                    validationTipologia.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    validationTipologia.ErrorTitle = "Valor inválido";
                    validationTipologia.Error = "Seleccione un valor de la lista.";

                    var validationActuante = workSheet.DataValidations.AddListValidation("G5:G30");
                    validationActuante.ShowErrorMessage = true;
                    validationActuante.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    validationActuante.ErrorTitle = "Valor inválido";
                    validationActuante.Error = "Seleccione un valor de la lista.";

                    var validationComunicacion = workSheet.DataValidations.AddListValidation("H5:H30");
                    validationComunicacion.ShowErrorMessage = true;
                    validationComunicacion.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    validationComunicacion.ErrorTitle = "Valor inválido";
                    validationComunicacion.Error = "Seleccione un valor de la lista.";

                    var validationEstadoOcurrencia = workSheet.DataValidations.AddListValidation("I5:I30");
                    validationEstadoOcurrencia.ShowErrorMessage = true;
                    validationEstadoOcurrencia.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    validationEstadoOcurrencia.ErrorTitle = "Valor inválido";
                    validationEstadoOcurrencia.Error = "Seleccione un valor de la lista.";

                    // Crear lista
                    int startRowSala = 2;
                    for(int i = 0; i < listaDatosSala.Count; i++) {
                        workSheet.Cells[startRowSala + i, 26].Value = listaDatosSala[i];
                    }
                    int endRowSala = startRowSala + listaDatosSala.Count - 1;
                    validationSala.Formula.ExcelFormula = $"$Z${startRowSala}:$Z${endRowSala}";
                    workSheet.Column(26).Hidden = true;

                    int startRowArea = 2;
                    for(int i = 0; i < listaDatosArea.Count; i++) {
                        workSheet.Cells[startRowArea + i, 27].Value = listaDatosArea[i];
                    }
                    int endRowArea = startRowArea + listaDatosArea.Count - 1;
                    validationArea.Formula.ExcelFormula = $"$AA${startRowArea}:$AA${endRowArea}";
                    workSheet.Column(27).Hidden = true;

                    int startRowTipologia = 2;
                    for(int i = 0; i < listaDatosTipologia.Count; i++) {
                        workSheet.Cells[startRowTipologia + i, 28].Value = listaDatosTipologia[i];
                    }
                    int endRowTipologia = startRowTipologia + listaDatosTipologia.Count - 1;
                    validationTipologia.Formula.ExcelFormula = $"$AB${startRowTipologia}:$AB${endRowTipologia}";
                    workSheet.Column(28).Hidden = true;

                    int startRowActuante = 2;
                    for(int i = 0; i < listaDatosActuante.Count; i++) {
                        workSheet.Cells[startRowActuante + i, 29].Value = listaDatosActuante[i];
                    }
                    int endRowActuante = startRowActuante + listaDatosActuante.Count - 1;
                    validationActuante.Formula.ExcelFormula = $"$AC${startRowActuante}:$AC${endRowActuante}";
                    workSheet.Column(29).Hidden = true;

                    int startRowComunicacion = 2;
                    for(int i = 0; i < listaDatosComunicacion.Count; i++) {
                        workSheet.Cells[startRowComunicacion + i, 30].Value = listaDatosComunicacion[i];
                    }
                    int endRowComunicacion = startRowComunicacion + listaDatosComunicacion.Count - 1;
                    validationComunicacion.Formula.ExcelFormula = $"$AD${startRowComunicacion}:$AD${endRowComunicacion}";
                    workSheet.Column(30).Hidden = true;

                    int startRowEstadoOcurrencia = 2;
                    for(int i = 0; i < listaDatosEstadoOcurrencia.Count; i++) {
                        workSheet.Cells[startRowEstadoOcurrencia + i, 31].Value = listaDatosEstadoOcurrencia[i];
                    }
                    int endRowEstadoOcurrencia = startRowEstadoOcurrencia + listaDatosEstadoOcurrencia.Count - 1;
                    validationEstadoOcurrencia.Formula.ExcelFormula = $"$AE${startRowEstadoOcurrencia}:$AE${endRowEstadoOcurrencia}";
                    workSheet.Column(31).Hidden = true;

                    // Crear lista desplegable para Salas
                    // Fin de la Validacion

                    byte[] bin = package.GetAsByteArray();
                    base64String = Convert.ToBase64String(bin);
                    excelName = "Plantilla_LogsOcurrencias_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx";
                    respuesta = true;
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }

            return Json(new { respuesta, mensaje, data = base64String, excelName });
        }


        [HttpPost]
        public ActionResult ImportarExcelLogsOcurrencias() {
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

                        //Eliminar la columna de observaciones
                        worksheet.DeleteColumn(14);
                        //Agregar una columna despues de observaciones
                        worksheet.InsertColumn(14, 1);

                        // Agregar y formatear la columna de observaciones
                        worksheet.Cells[3, 14, 4, 14].Merge = true;
                        worksheet.Cells[3, 14].Value = "Observación de la importación";
                        worksheet.Cells[3, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[3, 14].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 192, 0));
                        worksheet.Cells[3, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[3, 14].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        worksheet.Cells[3, 14].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        worksheet.Cells[4, 14].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        //modelTable.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        for(int row = 5; row <= 31; row++) {
                            bool filaVacia = true;
                            Console.WriteLine($"Procesando fila: {row}");

                            for(int col = 2; col <= 13; col++) {
                                var valorCelda = worksheet.Cells[row, col].Value?.ToString().Trim();
                                Console.WriteLine($"Fila {row}, Columna {col}, Valor: {valorCelda}");

                                if(!string.IsNullOrEmpty(valorCelda)) {
                                    filaVacia = false;
                                    break;
                                }
                            }

                            if(filaVacia) {
                                Console.WriteLine($"Fila {row} está vacía. Continuando...");
                                continue;
                            }


                            var observacionFila = new List<string>();

                            string campoSala = worksheet.Cells[row, 2].Text.Trim(); // Columna B
                            string campoFechaOcurrencia = worksheet.Cells[row, 3].Text.Trim(); // Columna C
                            string campoHoraOcurrencia = worksheet.Cells[row, 4].Text.Trim(); // Columna D
                            string campoArea = worksheet.Cells[row, 5].Text.Trim(); // Columna E
                            string campoTipologia = worksheet.Cells[row, 6].Text.Trim(); // Columna F
                            string campoActuante = worksheet.Cells[row, 7].Text.Trim(); // Columna G
                            string campoComunicacion = worksheet.Cells[row, 8].Text.Trim(); // Columna H
                            string campoEstadoOcurrencia = worksheet.Cells[row, 9].Text.Trim(); // Columna I
                            string campoDetalle = worksheet.Cells[row, 10].Text.Trim(); // Columna J
                            string campoAccionEjecutada = worksheet.Cells[row, 11].Text.Trim(); // Columna K
                            string campoFechaSolucion = worksheet.Cells[row, 12].Text.Trim(); // Columna L
                            string campoHoraSolucion = worksheet.Cells[row, 13].Text.Trim(); // Columna M

                            // Validaciones
                            if(string.IsNullOrEmpty(campoSala)) {
                                observacionFila.Add("El campo Sala es obligatorio.");
                                ResaltarCelda(worksheet, row, 2); // Resaltar celda B
                            }
                            if(string.IsNullOrEmpty(campoFechaOcurrencia)) {
                                observacionFila.Add("El campo Fecha de Ocurrencia es obligatorio.");
                                ResaltarCelda(worksheet, row, 3); // Resaltar celda C
                            }
                            if(string.IsNullOrEmpty(campoHoraOcurrencia)) {
                                observacionFila.Add("El campo Hora de Ocurrencia es obligatorio.");
                                ResaltarCelda(worksheet, row, 4); // Resaltar celda D
                            }
                            if(string.IsNullOrEmpty(campoArea)) {
                                observacionFila.Add("El campo Área es obligatorio.");
                                ResaltarCelda(worksheet, row, 5); // Resaltar celda E
                            }
                            if(string.IsNullOrEmpty(campoTipologia)) {
                                observacionFila.Add("El campo Tipología es obligatorio.");
                                ResaltarCelda(worksheet, row, 6); // Resaltar celda F
                            }
                            if(string.IsNullOrEmpty(campoActuante)) {
                                observacionFila.Add("El campo Actuante es obligatorio.");
                                ResaltarCelda(worksheet, row, 7); // Resaltar celda G
                            }
                            if(string.IsNullOrEmpty(campoComunicacion)) {
                                observacionFila.Add("El campo Comunicación es obligatorio.");
                                ResaltarCelda(worksheet, row, 8); // Resaltar celda H
                            }
                            if(string.IsNullOrEmpty(campoEstadoOcurrencia)) {
                                observacionFila.Add("El campo Estado de Ocurrencia es obligatorio.");
                                ResaltarCelda(worksheet, row, 9); // Resaltar celda I
                            }
                            //if (string.IsNullOrEmpty(campoDetalle))
                            //{
                            //    observacionFila.Add("El campo Detalle es obligatorio.");
                            //    ResaltarCelda(worksheet, row, 10); // Resaltar celda J
                            //}
                            //if (string.IsNullOrEmpty(campoAccionEjecutada))
                            //{
                            //    observacionFila.Add("El campo Acción Ejecutada es obligatorio.");
                            //    ResaltarCelda(worksheet, row, 11); // Resaltar celda K
                            //}
                            if(string.IsNullOrEmpty(campoFechaSolucion)) {
                                observacionFila.Add("El campo Fecha de Solución es obligatorio.");
                                ResaltarCelda(worksheet, row, 12); // Resaltar celda L
                            }
                            if(string.IsNullOrEmpty(campoHoraSolucion)) {
                                observacionFila.Add("El campo Hora de Solución es obligatorio.");
                                ResaltarCelda(worksheet, row, 13); // Resaltar celda M
                            }

                            //Todo OK
                            if(!observacionFila.Any()) {
                                try {
                                    var registro = new ESS_LogOcurrenciaEntidad {
                                        CodSala = int.Parse(campoSala.Split('-')[0]),
                                        NombreSala = campoSala.Split('-')[1],
                                        Fecha = DateTime.Parse(DateTime.Parse(campoFechaOcurrencia).ToString("yyyy-MM-dd"))
                                                        + TimeSpan.Parse(DateTime.Parse(campoHoraOcurrencia).ToString("HH:mm:ss")),

                                        IdArea = int.Parse(campoArea.Split('-')[0]),
                                        NombreArea = campoArea.Split('-')[1],

                                        IdTipologia = int.Parse(campoTipologia.Split('-')[0]),
                                        NombreTipologia = campoTipologia.Split('-')[1],

                                        IdActuante = int.Parse(campoActuante.Split('-')[0]),
                                        NombreActuante = campoActuante.Split('-')[1],

                                        IdComunicacion = int.Parse(campoComunicacion.Split('-')[0]),
                                        NombreComunicacion = campoComunicacion.Split('-')[1],

                                        Detalle = campoDetalle,
                                        AccionEjecutada = campoAccionEjecutada,

                                        IdEstadoOcurrencia = int.Parse(campoEstadoOcurrencia.Split('-')[0]),
                                        FechaSolucion = DateTime.Parse(DateTime.Parse(campoFechaSolucion).ToString("yyyy-MM-dd"))
                                                        + TimeSpan.Parse(DateTime.Parse(campoHoraSolucion).ToString("HH:mm:ss")),

                                        UsuarioRegistro = (string)Session["UsuarioNombre"]
                                    };

                                    int insertedId = _essLogOcurrenciaBL.GuardarLogOcurrencia(registro);
                                    if(insertedId > 0) {
                                        string msg = "Registro guardado con éxito.";
                                        observaciones.Add(new { Fila = row, Observaciones = msg });

                                        var richText = worksheet.Cells[row, 14].RichText;
                                        var prefijo = richText.Add("✔ ");
                                        prefijo.Color = System.Drawing.Color.Green;
                                        var mensajetexto = richText.Add(msg);
                                        mensajetexto.Color = System.Drawing.Color.Black;
                                    } else {
                                        string msg = "No se pudo guardar el registro.";
                                        observaciones.Add(new { Fila = row, Observaciones = msg });

                                        var richText = worksheet.Cells[row, 14].RichText;
                                        var prefijo = richText.Add("❌ ");
                                        prefijo.Color = System.Drawing.Color.Red;
                                        var mensajetexto = richText.Add(msg);
                                        mensajetexto.Color = System.Drawing.Color.Black;
                                    }
                                } catch(Exception ex) {
                                    string msg = "No se pudo insertar este registro debido a un error en la conversión de datos.";
                                    observaciones.Add(new { Fila = row, Observaciones = msg });

                                    var richText = worksheet.Cells[row, 14].RichText;
                                    var prefijo = richText.Add("❌ ");
                                    prefijo.Color = System.Drawing.Color.Red;
                                    var mensajetexto = richText.Add(msg);
                                    mensajetexto.Color = System.Drawing.Color.Black;


                                }
                            } else {
                                string obsMsg = string.Join(" | ", observacionFila);
                                observaciones.Add(new { Fila = row, Observaciones = obsMsg });

                                var richText = worksheet.Cells[row, 14].RichText;
                                var prefijo = richText.Add("❌ ");
                                prefijo.Color = System.Drawing.Color.Red;
                                var mensajetexto = richText.Add(obsMsg);
                                mensajetexto.Color = System.Drawing.Color.Black;
                            }
                        }

                        worksheet.Column(14).AutoFit();
                        worksheet.Column(14).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
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

