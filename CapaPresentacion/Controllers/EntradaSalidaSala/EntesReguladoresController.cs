using CapaEntidad.EntradaSalidaSala;
using CapaNegocio;
using CapaNegocio.AsistenciaCliente;
using CapaNegocio.ControlAcceso;
using CapaNegocio.EntradaSalidaSala;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using OfficeOpenXml.Style;
using S3k.Utilitario.clases_especial;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;


namespace CapaPresentacion.Controllers.EntradaSalidaSala {
    [seguridad]
    public class EntesReguladoresController : Controller {
        private readonly SalaBL _salaBl = new SalaBL();
        private readonly EmpresaBL _empresaBl = new EmpresaBL();
        ESS_EnteReguladorBL ess_entereguladorbl = new ESS_EnteReguladorBL();
        ClaseError error = new ClaseError();
        private readonly ESS_CargoBL _essCargoBL = new ESS_CargoBL();
        private readonly ESS_EnteReguladorMotivoBL _essMotivoBL = new ESS_EnteReguladorMotivoBL();
        private readonly ESS_CategoriaBL _essCategoriaBL = new ESS_CategoriaBL();
        private readonly ESS_EmpleadoBL _essEmpleadoBL = new ESS_EmpleadoBL();
        private CAL_EntidadPublicaBL _entidadPublicaBL = new CAL_EntidadPublicaBL();
        private AST_TipoDocumentoBL ast_TipoDocumentoBL = new AST_TipoDocumentoBL();
        private readonly ESS_AccionCajaTemporizadaBL _essAccionCajaTemporizadaBL = new ESS_AccionCajaTemporizadaBL();

        public ActionResult EntesReguladoresVista() {
            return View("~/Views/EntradaSalidaSala/EntesReguladores.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult EnteReguladorListarxSalaFechaJson(int[] codsala, DateTime fechaini, DateTime fechafin) {
            string mensaje = "No se encontraron registros";
            bool respuesta = false;
            List<ESS_EnteReguladorEntidad> listaESS_EnteRegulador = new List<ESS_EnteReguladorEntidad>();
            try {

                // Directorio de imagen
                string Ruta = "EntradaSalidaSala";
                string Subruta = "EnteRegulador";
                string PathPrincipal = Path.Combine(Ruta, Subruta);
                // Fin Directorio de imagen
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                int[] salasBusqueda;
                if(codsala.Contains(-1)) {
                    salasBusqueda = salas.Select(x => x.CodSala).ToArray();
                } else {
                    salasBusqueda = codsala;
                }
                listaESS_EnteRegulador = ess_entereguladorbl.ListadoEnteRegulador(salasBusqueda, fechaini, fechafin);
                mensaje = "Registros obtenidos";
                respuesta = true;
                string rutaBase = ConfigurationManager.AppSettings["PathWebArchivos"].ToString() + "/";

                foreach(var item in listaESS_EnteRegulador) {
                    if(!string.IsNullOrEmpty(item.RutaImagen)) {
                        var RutaImagen = Path.Combine(PathPrincipal, item.RutaImagen);
                        item.RutaImagen = new Uri(new Uri(rutaBase), RutaImagen).ToString();
                    }
                }
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, comunicarse con administrador";
            }
            var oRespuesta = new {
                mensaje,
                respuesta,
                data = listaESS_EnteRegulador
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
        public ActionResult GuardarEntesReguladores(ESS_EnteReguladorEntidad enteregulador, string PersonasEntidadPublica) {
            bool respuesta = false;
            string mensaje = "No se pudo guardar los datos";
            int tamanioMaximo = 4194304;
            string extension = "";
            List<string> extensiones = new List<string>() { ".jpg", ".png", ".jpeg", ".pdf", ".docx" };

            try {
                enteregulador.PersonasEntidadPublica = JsonConvert.DeserializeObject<List<ESS_EntidadRegularPersonaEntidadPublica>>(PersonasEntidadPublica);

                string Ruta = "EntradaSalidaSala";
                string Subruta = "EnteRegulador";
                string PathPrincipal = Path.Combine(ConfigurationManager.AppSettings["PathArchivos"].ToString(), Ruta, Subruta);
                HttpPostedFileBase file = Request.Files[0];

                if(file != null) {
                    if(file.ContentLength > 0) {
                        if(file.ContentLength >= tamanioMaximo) {
                            return Json(new { respuesta = false, mensaje = "El tamaño del archivo no debe ser superior a 4mb" });
                        }
                        extension = Path.GetExtension(file.FileName).ToLower();
                        if(!extensiones.Contains(extension)) {
                            return Json(new { respuesta = false, mensaje = "Solo se aceptan archivos con extension .jpg,.png, .jpeg, .pdf o .docx" });
                        }
                    }

                }
                enteregulador.FechaRegistro = DateTime.Now;
                enteregulador.UsuarioRegistro = (string)Session["UsuarioNombre"];


                int idInsertado = ess_entereguladorbl.GuardarRegistroEnteRegulador(enteregulador);

                if(idInsertado > 0) {
                    if(file != null) {
                        if(file.ContentLength > 0) {
                            if(!Directory.Exists(PathPrincipal)) {
                                Directory.CreateDirectory(PathPrincipal);
                            }
                            var nombreArchivoCompleto = string.Empty;
                            string nombreArchivo = $"Imagen_DOC_REF_{idInsertado}";
                            nombreArchivoCompleto = (nombreArchivo + extension);
                            var imagePath = Path.Combine(PathPrincipal, nombreArchivoCompleto);
                            file.SaveAs(imagePath);
                            ess_entereguladorbl.ActualizarRutaImagen(idInsertado, nombreArchivoCompleto);
                        }
                    }
                    respuesta = true;
                    mensaje = "Los datos se han guardado";
                }
            } catch(Exception exception) {
                mensaje = $"Error: {exception.Message}";

                Console.WriteLine(exception.StackTrace);
            }

            return Json(new { respuesta, mensaje });
        }

        [HttpPost]
        public ActionResult EditarEntesReguladores(ESS_EnteReguladorEntidad enteregulador, string PersonasEntidadPublica) {
            bool respuesta = false;

            string mensaje = "No se pudo actualizar los datos";
            int tamanioMaximo = 4194304;
            string extension = "";
            List<string> extensiones = new List<string>() { ".jpg", ".png", ".jpeg", ".pdf", ".docx" };

            try {

                if(!string.IsNullOrEmpty(PersonasEntidadPublica)) {
                    enteregulador.PersonasEntidadPublica = JsonConvert.DeserializeObject<List<ESS_EntidadRegularPersonaEntidadPublica>>(PersonasEntidadPublica);
                }
                string Ruta = "EntradaSalidaSala";
                string Subruta = "EnteRegulador";
                string PathPrincipal = Path.Combine(ConfigurationManager.AppSettings["PathArchivos"].ToString(), Ruta, Subruta);
                HttpPostedFileBase file = Request.Files[0];

                if(file != null) {
                    if(file.ContentLength > 0) {
                        if(file.ContentLength >= tamanioMaximo) {
                            return Json(new { respuesta = false, mensaje = "El tamaño del archivo no debe ser superior a 4mb" });
                        }
                        extension = Path.GetExtension(file.FileName).ToLower();
                        if(!extensiones.Contains(extension)) {
                            return Json(new { respuesta = false, mensaje = "Solo se aceptan imagenes con extension .jpg,.png, .jpeg,.pdf o .docx" });
                        }
                        if(!Directory.Exists(PathPrincipal)) {
                            Directory.CreateDirectory(PathPrincipal);
                        }
                    }
                }
                enteregulador.FechaModificacion = DateTime.Now;
                enteregulador.UsuarioModificacion = (string)Session["UsuarioNombre"];


                respuesta = ess_entereguladorbl.ActualizarRegistroEnteRegulador(enteregulador);


                if(respuesta) {
                    if(file != null) {

                        if(file.ContentLength > 0) {

                            if(!Directory.Exists(PathPrincipal)) {
                                Directory.CreateDirectory(PathPrincipal);
                            }
                            var nombreArchivoCompleto = string.Empty;
                            string nombreArchivo = $"Imagen_DOC_REF_{enteregulador.IdEnteRegulador}";
                            nombreArchivoCompleto = (nombreArchivo + extension);
                            var imagePath = Path.Combine(PathPrincipal, nombreArchivoCompleto);
                            if(System.IO.File.Exists(imagePath)) {
                                System.IO.File.Delete(imagePath);
                            }
                            file.SaveAs(imagePath);
                            ess_entereguladorbl.ActualizarRutaImagen(enteregulador.IdEnteRegulador, nombreArchivoCompleto);
                        }
                    }
                    respuesta = true;
                    mensaje = "Los datos se han actualizado";
                }
            } catch(Exception exception) {
                mensaje = $"Error: {exception.Message}";
                Console.WriteLine(exception.StackTrace);
            }

            return Json(new { respuesta, mensaje });
        }



        [HttpPost]
        public JsonResult EliminarEntesReguladores(int id) {
            bool respuesta = false;
            string mensaje = "No se pudo eliminar el registro.";

            try {
                //respuesta = ess_entereguladorbl.EliminarRegistroEnteRegulador(new ESS_EnteReguladorEntidad { IdEnteRegulador = id });
                respuesta = ess_entereguladorbl.EliminarRegistroEnteRegulador(id);
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
        public ActionResult FinalizarHoraRegistroEnteRegulador(int identeregulador, string horaSalida) {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try {
                DateTime horaFinalizada;
                if(DateTime.TryParse(horaSalida, out horaFinalizada)) {
                    respuestaConsulta = ess_entereguladorbl.FinalizarHoraRegistroEnteRegulador(identeregulador, horaFinalizada);
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



        #region ESS_Cargo
        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarCargo() {
            string mensaje = string.Empty;
            bool respuesta = false;
            var lista = new List<ESS_CargoEntidad>();
            try {

                lista = _essCargoBL.ListarCargo();
                mensaje = "Listando registros";
                respuesta = true;
                return Json(new { mensaje, respuesta, data = lista });
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, contacte con administrador";
                respuesta = false;
                return Json(new { mensaje, respuesta });
            }
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarCargoPorEstado(int estado = 1) {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_CargoEntidad>();
            try {

                data = _essCargoBL.ListarCargoPorEstado(estado);
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
        public ActionResult ObtenerCargoPorId(int id) {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new ESS_CargoEntidad();
            try {

                data = _essCargoBL.ObtenerCargoPorId(id);
                mensaje = "Registro Obtenido";
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
        public ActionResult InsertarCargo(ESS_CargoEntidad model) {
            string mensaje = "No se pudo insertar el registro";
            bool respuesta = false;
            try {
                model.FechaRegistro = DateTime.Now;
                model.UsuarioRegistro = (string)Session["UsuarioNombre"];
                int idInsertado = _essCargoBL.InsertarCargo(model);
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
        public ActionResult EditarCargo(ESS_CargoEntidad model) {
            string mensaje = "No se pudo editar el registro";
            bool respuesta = false;
            try {
                model.FechaModificacion = DateTime.Now;
                model.UsuarioModificacion = (string)Session["UsuarioNombre"];
                respuesta = _essCargoBL.EditarCargo(model);
                if(respuesta == true) {
                    mensaje = "Registro editado";
                    respuesta = true;
                }
                return Json(new { mensaje, respuesta });
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, contacte con administrador";
                respuesta = false;
                return Json(new { mensaje, respuesta });
            }
        }
        #endregion
        #region Motivo
        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarMotivo() {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_EnteReguladorMotivoEntidad>();
            try {

                data = _essMotivoBL.ListarMotivo();
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
        public ActionResult ListarMotivoPorEstado(int estado = 1) {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_EnteReguladorMotivoEntidad>();
            try {

                data = _essMotivoBL.ListarMotivoPorEstado(estado);
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
        public ActionResult InsertarMotivo(ESS_EnteReguladorMotivoEntidad model) {
            string mensaje = "No se pudo insertar el registro";
            bool respuesta = false;
            try {
                model.FechaRegistro = DateTime.Now;
                model.UsuarioRegistro = (string)Session["UsuarioNombre"];
                int idInsertado = _essMotivoBL.InsertarMotivo(model);
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
        public ActionResult EditarMotivo(ESS_EnteReguladorMotivoEntidad model) {
            string mensaje = "No se pudo editar el registro";
            bool respuesta = false;
            try {
                model.FechaModificacion = DateTime.Now;
                model.UsuarioModificacion = (string)Session["UsuarioNombre"];
                respuesta = _essMotivoBL.EditarMotivo(model);
                if(respuesta == true) {
                    mensaje = "Registro editado";
                    respuesta = true;
                }
                return Json(new { mensaje, respuesta });
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, contacte con administrador";
                respuesta = false;
                return Json(new { mensaje, respuesta });
            }
        }

        #endregion
        #region Categoria
        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarCategoria() {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_CategoriaEntidad>();
            try {

                data = _essCategoriaBL.ListarCategoria();
                mensaje = "Listando registros";
                respuesta = true;
                return Json(new { mensaje, respuesta, data });
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, contacte con administrador";
                respuesta = false;
                return Json(new { mensaje, respuesta });
            }
        }

        #endregion
        #region Empleado
        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarEmpleado() {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_EmpleadoEntidad>();
            try {

                data = _essEmpleadoBL.ListarEmpleado();
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
        public ActionResult ListarEmpleadoPorEstado(int estado = 1) {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_EmpleadoEntidad>();
            try {

                data = _essEmpleadoBL.ListarEmpleadoPorEstado(estado);
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
        public ActionResult InsertarEmpleado(ESS_EmpleadoEntidad model) {
            string mensaje = "No se pudo insertar el registro";
            bool respuesta = false;
            try {
                model.FechaRegistro = DateTime.Now;
                model.UsuarioRegistro = (string)Session["UsuarioNombre"];
                int idInsertado = _essEmpleadoBL.InsertarEmpleado(model);
                if(idInsertado > 0) {
                    mensaje = "Registro insertado";
                    respuesta = true;
                }
                if(idInsertado == -1) {
                    mensaje = "No se puede insertar, registro duplicado";
                    respuesta = false;
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
        public ActionResult EditarEmpleado(ESS_EmpleadoEntidad model) {
            string mensaje = "No se pudo editar el registro";
            bool respuesta = false;
            try {
                model.FechaModificacion = DateTime.Now;
                model.UsuarioModificacion = (string)Session["UsuarioNombre"];
                respuesta = _essEmpleadoBL.EditarEmpleado(model);
                if(respuesta == true) {
                    mensaje = "Registro editado";
                    respuesta = true;
                }
                return Json(new { mensaje, respuesta });
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, contacte con administrador";
                respuesta = false;
                return Json(new { mensaje, respuesta });
            }
        }
        #endregion


        [HttpPost]
        public ActionResult ReporteEntesReguladoresDescargarExcelJson(int[] codsala, DateTime fechaini, DateTime fechafin) {
            string mensaje = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;

            var usuarioId = (int)Session["UsuarioID"];
            var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
            int[] salasBusqueda;
            if(codsala.Contains(-1)) {
                salasBusqueda = salas.Select(x => x.CodSala).ToArray();
            } else {
                salasBusqueda = codsala;
            }


            List<ESS_EnteReguladorEntidad> lista = ess_entereguladorbl.ListadoEnteRegulador(salasBusqueda, fechaini, fechafin);

            lista = lista.OrderByDescending(x => x.FechaRegistro).ToList();

            try {
                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();


                    var workSheet = excel.Workbook.Worksheets.Add("Entes Reguladores");
                    workSheet.TabColor = Color.Black;
                    workSheet.DefaultRowHeight = 9;


                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[2, 2, 2, 14].Merge = true;
                    workSheet.Cells[2, 2].Value = "Reporte de Entes Reguladores";
                    workSheet.Cells[2, 2].Style.Font.Bold = true;
                    workSheet.Cells[2, 2].Style.Font.Size = 13;
                    workSheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Sala";
                    workSheet.Cells[3, 4].Value = "Fecha";
                    workSheet.Cells[3, 5].Value = "Descripcion";
                    workSheet.Cells[3, 6].Value = "Motivo I/S";
                    workSheet.Cells[3, 7].Value = "Empresa / Institucion";
                    workSheet.Cells[3, 8].Value = "Documento Referencia";
                    workSheet.Cells[3, 9].Value = "Ingreso";
                    workSheet.Cells[3, 10].Value = "Salida";
                    workSheet.Cells[3, 11].Value = "Observaciones";
                    workSheet.Cells[3, 12].Value = "Autoriza";
                    workSheet.Cells[3, 13].Value = "Persona Entidad Publica";

                    int recordIndex = 4;
                    foreach(var item in lista) {
                        workSheet.Cells[recordIndex, 2].Value = item.IdEnteRegulador;
                        workSheet.Cells[recordIndex, 3].Value = item.NombreSala;
                        workSheet.Cells[recordIndex, 4].Value = item.FechaRegistro.ToString("dd-MM-yyyy hh:mm tt");
                        workSheet.Cells[recordIndex, 5].Value = item.Descripcion;
                        workSheet.Column(5).Width = 40;

                        //workSheet.Cells[recordIndex, 6].Value = item.NombreMotivo;
                        workSheet.Cells[recordIndex, 7].Value = item.NombreEmpresa;
                        workSheet.Cells[recordIndex, 8].Value = string.IsNullOrEmpty(item.DocReferencia) ? "No definido" : item.DocReferencia;
                        workSheet.Cells[recordIndex, 9].Value = item.FechaIngreso.ToString("dd-MM-yyyy hh:mm tt");
                        workSheet.Cells[recordIndex, 10].Value = item.FechaSalida.ToString("dd-MM-yyyy") == "01-01-1753" ? "No Definido" : item.FechaSalida.ToString("dd-MM-yyyy hh:mm tt");


                        workSheet.Cells[recordIndex, 11].Value = string.IsNullOrEmpty(item.Observaciones) ? "No definido" : item.Observaciones;
                        workSheet.Column(11).Width = 30;

                        workSheet.Cells[recordIndex, 12].Value = item.NombreAutoriza;


                        if(item.NombreMotivo == "OTROS") {
                            workSheet.Cells[recordIndex, 6].Value = $"{item.NombreMotivo} ({item.DescripcionMotivo})";
                        } else {
                            workSheet.Cells[recordIndex, 6].Value = item.NombreMotivo;
                        }
                        workSheet.Column(6).Width = 30;



                        if(item.PersonasEntidadPublica != null && item.PersonasEntidadPublica.Any()) {
                            var employeeSheet = excel.Workbook.Worksheets.Add($"Detalle_PersonaEntidad_{item.IdEnteRegulador}");
                            employeeSheet.Cells[1, 2, 1, 8].Merge = true;
                            employeeSheet.Cells[1, 2].Value = $"Detalle: Persona Entidad Publica del Registro N°: {item.IdEnteRegulador}";
                            employeeSheet.Cells[1, 2].Style.Font.Bold = true;
                            employeeSheet.Cells[1, 2].Style.Font.Size = 13;
                            employeeSheet.Cells[1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            employeeSheet.Cells[3, 2].Value = "ID";
                            employeeSheet.Cells[3, 3].Value = "Nombres";
                            employeeSheet.Cells[3, 4].Value = "Apellidos";
                            employeeSheet.Cells[3, 5].Value = "Entidad Publica";
                            employeeSheet.Cells[3, 6].Value = "DNI";
                            employeeSheet.Cells[3, 7].Value = "Cargo";
                            //employeeSheet.Cells[3, 8].Value = "Fecha de Registro";

                            Color headerBackground = ColorTranslator.FromHtml("#003268");
                            employeeSheet.Cells["B3:G3"].Style.Font.Bold = true;
                            employeeSheet.Cells["B3:G3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            employeeSheet.Cells["B3:G3"].Style.Fill.BackgroundColor.SetColor(headerBackground);
                            employeeSheet.Cells["B3:G3"].Style.Font.Color.SetColor(Color.White);
                            employeeSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            int employeeIndex = 4;
                            foreach(var empleado in item.PersonasEntidadPublica) {
                                employeeSheet.Cells[employeeIndex, 2].Value = empleado.PersonaEntidadPublicaID;
                                employeeSheet.Cells[employeeIndex, 3].Value = empleado.Nombres;
                                employeeSheet.Cells[employeeIndex, 4].Value = empleado.Apellidos;
                                employeeSheet.Cells[employeeIndex, 5].Value = empleado.EntidadPublicaNombre;
                                employeeSheet.Cells[employeeIndex, 6].Value = empleado.Dni;
                                employeeSheet.Cells[employeeIndex, 7].Value = empleado.CargoEntidadNombre;
                                //employeeSheet.Cells[employeeIndex, 8].Value = empleado.FechaRegistro;
                                employeeIndex++;
                            }

                            for(int i = 2; i <= 7; i++) {
                                employeeSheet.Column(i).AutoFit();
                            }

                            var hyperlink = new ExcelHyperLink($"#'Detalle_PersonaEntidad_{item.IdEnteRegulador}'!A1", "Ver Detalles");
                            workSheet.Cells[recordIndex, 13].Hyperlink = hyperlink;
                            workSheet.Cells[recordIndex, 13].Style.Font.UnderLine = true;
                            workSheet.Cells[recordIndex, 13].Style.Font.Color.SetColor(Color.Blue);
                        } else {
                            workSheet.Cells[recordIndex, 13].Value = "No hay personas entidad publica";
                        }

                        recordIndex++;
                    }

                    Color principalHeaderBackground = ColorTranslator.FromHtml("#003268");
                    workSheet.Cells["B3:M3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:M3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:M3"].Style.Fill.BackgroundColor.SetColor(principalHeaderBackground);
                    workSheet.Cells["B3:M3"].Style.Font.Color.SetColor(Color.White);

                    for(int i = 2; i <= 14; i++) {
                        if(i != 6 && i != 11) {
                            workSheet.Column(i).AutoFit();
                        }
                    }

                    // Nombre del archivo
                    excelName = $"EnteRegulador_{fechaini:dd_MM_yyyy}_al_{fechafin:dd_MM_yyyy}.xlsx";

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


        [HttpGet]
        public ActionResult ObtenerPersonasActivasPorPatronEntesReguladores(int entidadPublicaID, string term) {
            bool success = false;
            List<ESS_EnteReguladorPersonaEntidadPublicaEntidad> result = new List<ESS_EnteReguladorPersonaEntidadPublicaEntidad>();
            string mensaje;

            try {
                // Llama al método de la capa de negocio (BL) para obtener los resultados
                result = ess_entereguladorbl.ObtenerPersonasActivasPorTermino(entidadPublicaID, term);
                success = result.Count > 0;
                mensaje = success ? "Lista de personas activas de la entidad pública" : "No se encontraron personas";
            } catch(Exception exp) {
                mensaje = exp.Message + ". Llame al Administrador.";
            }

            // Devuelve el resultado como un JSON
            return Json(new { success, data = result, mensaje }, JsonRequestBehavior.AllowGet);
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult PlantillaEntesReguladoresDescargarExcel() {
            string path = Server.MapPath(@"~/Content/ess_excel_plantilla/Plantilla_ESS_EntesReguladores.xlsx");
            string base64String = "";
            string excelName = "";
            string mensaje = "";
            bool respuesta = false;

            try {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                var motivos = _essMotivoBL.ListarMotivoPorEstado(1);
                var instituciones = _entidadPublicaBL.EntidadPublicaListadoCompletoJson();
                var tipodocumentos = ast_TipoDocumentoBL.GetListadoTipoDocumento();


                List<string> listaDatosSala = salas.Select(x => $"{x.CodSala}-{x.Nombre}").ToList();
                List<string> listaDatosMotivo = motivos.Select(x => $"{x.IdMotivo}-{x.Nombre}").ToList();
                List<string> listaDatosInstitucion = instituciones.Select(x => $"{x.EntidadPublicaID}-{x.Nombre}").ToList();
                List<string> listaDatosTipoDocumentos = tipodocumentos.Select(x => $"{x.Id}-{x.Nombre.ToUpper()}").ToList();


                using(var package = new ExcelPackage(new FileInfo(path))) {

                    var workSheetOriginal = package.Workbook.Worksheets[0];


                    var workSheet = package.Workbook.Worksheets[0]; // Seleccionar la primera hoja


                    int startRowSala = 2;
                    int startRowTipoBien = 1;
                    int columnStartTipoBien = 27;

                    var validationSala = workSheet.DataValidations.AddListValidation("B5:B30");
                    validationSala.ShowErrorMessage = true;
                    validationSala.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    validationSala.ErrorTitle = "Valor inválido";
                    validationSala.Error = "Seleccione un valor de la lista.";

                    var validationMotivo = workSheet.DataValidations.AddListValidation("E5:E30");
                    validationMotivo.ShowErrorMessage = true;
                    validationMotivo.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    validationMotivo.ErrorTitle = "Valor inválido";
                    validationMotivo.Error = "Seleccione un valor de la lista.";

                    var validationInstitucion = workSheet.DataValidations.AddListValidation("F5:F30");
                    validationInstitucion.ShowErrorMessage = true;
                    validationInstitucion.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    validationInstitucion.ErrorTitle = "Valor inválido";
                    validationInstitucion.Error = "Seleccione un valor de la lista.";


                    //Sala
                    for(int j = 0; j < listaDatosSala.Count; j++) {
                        workSheet.Cells[startRowSala + j, 26].Value = listaDatosSala[j];
                    }
                    int endRowSala = startRowSala + listaDatosSala.Count - 1;
                    validationSala.Formula.ExcelFormula = $"$Z${startRowSala}:$Z${endRowSala}";
                    workSheet.Column(26).Hidden = true;

                    //Motivo 
                    for(int j = 0; j < listaDatosMotivo.Count; j++) {
                        workSheet.Cells[startRowTipoBien + j + 1, columnStartTipoBien].Value = listaDatosMotivo[j];
                    }
                    int endRowMotivo = startRowTipoBien + 1 + listaDatosMotivo.Count - 1;
                    validationMotivo.Formula.ExcelFormula = $"$AA${startRowTipoBien + 1}:$AA${endRowMotivo}";
                    workSheet.Column(columnStartTipoBien).Hidden = true;

                    //Institucion
                    for(int j = 0; j < listaDatosInstitucion.Count; j++) {
                        workSheet.Cells[startRowTipoBien + j + 1, columnStartTipoBien + 1].Value = listaDatosInstitucion[j];
                    }
                    int endRowInstitucion = startRowTipoBien + 1 + listaDatosInstitucion.Count - 1;
                    validationInstitucion.Formula.ExcelFormula = $"$AB${startRowTipoBien + 1}:$AB${endRowInstitucion}";
                    workSheet.Column(columnStartTipoBien + 1).Hidden = true;


                    byte[] bin = package.GetAsByteArray();
                    base64String = Convert.ToBase64String(bin);
                    excelName = "Plantilla_EnteRegulador_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx";
                    respuesta = true;
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }

            return Json(new { respuesta, mensaje, data = base64String, excelName });
        }

        [HttpPost]
        public ActionResult ImportarExcelEntesReguladores() {
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
                var listadoJefeyGerente = new List<ESS_AccionCajaTemporizadaCargoEntidad>();
                listadoJefeyGerente = _essAccionCajaTemporizadaBL.ListarEmpleadosGerentesYJefes();


                string fechaHora = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string nombreProcesado = $"Procesado_{fechaHora}_{nombreOriginal}";
                using(var stream = archivoExcel.InputStream) {
                    using(var package = new ExcelPackage(stream)) {
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                        if(worksheet == null)
                            return Json(new { respuesta = false, mensaje = "El archivo no contiene hojas válidas." });

                        int ColumnaObservacionesImportar = 14;
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
                            for(int col = 2; col <= 13; col++) {
                                var valorCelda = worksheet.Cells[row, col].Value?.ToString().Trim();
                                if(valorCelda == "--Seleccione un Tipo Bien--") {
                                    valorCelda = string.Empty;
                                }
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
                            string campoAutoriza = worksheet.Cells[row, 3].Text.Trim(); // Columna C
                            string campoDescripcion = worksheet.Cells[row, 4].Text.Trim(); // Columna D
                            string campoMotivo = worksheet.Cells[row, 5].Text.Trim(); // Columna E
                            string campoInstitucion = worksheet.Cells[row, 6].Text.Trim(); // Columna F
                            string campoEmpleados = worksheet.Cells[row, 7].Text.Trim(); // Columna G
                            string campoDocumentoReferencia = worksheet.Cells[row, 8].Text.Trim(); // Columna H
                            string campoFechaIngreso = worksheet.Cells[row, 9].Text.Trim(); // Columna I
                            string campoHoraIngreso = worksheet.Cells[row, 10].Text.Trim(); // Columna J
                            string campoFechaSalida = worksheet.Cells[row, 11].Text.Trim(); // Columna K
                            string campoHoraSalida = worksheet.Cells[row, 12].Text.Trim(); // Columna L
                            string campoObservaciones = worksheet.Cells[row, 13].Text.Trim(); // Columna M

                            // Validaciones
                            if(string.IsNullOrEmpty(campoSala)) {
                                observacionFila.Add("El campo Sala es obligatorio.");
                                ResaltarCelda(worksheet, row, 2); // Resaltar celda B
                            }
                            if(string.IsNullOrEmpty(campoAutoriza)) {
                                observacionFila.Add("El campo Autoriza es obligatorio.");
                                ResaltarCelda(worksheet, row, 3); // Resaltar celda C
                            }
                            if(string.IsNullOrEmpty(campoDescripcion)) {
                                observacionFila.Add("El campo Descripción es obligatorio.");
                                ResaltarCelda(worksheet, row, 4); // Resaltar celda D
                            }
                            if(string.IsNullOrEmpty(campoMotivo)) {
                                observacionFila.Add("El campo Motivo es obligatorio.");
                                ResaltarCelda(worksheet, row, 5); // Resaltar celda E
                            }
                            if(string.IsNullOrEmpty(campoInstitucion)) {
                                observacionFila.Add("El campo Empresa/Institución es obligatorio.");
                                ResaltarCelda(worksheet, row, 6); // Resaltar celda F
                            }
                            if(string.IsNullOrEmpty(campoEmpleados)) {
                                observacionFila.Add("El campo Empleados es obligatorio.");
                                ResaltarCelda(worksheet, row, 7); // Resaltar celda G
                            } else {
                                if(campoEmpleados.Split(',').Any(x => string.IsNullOrEmpty(x.Trim()))) {
                                    observacionFila.Add("El campo Empleados no tiene un formato válido.");
                                    ResaltarCelda(worksheet, row, 7); // Resaltar celda G
                                }
                            }
                            if(string.IsNullOrEmpty(campoDocumentoReferencia)) {
                                observacionFila.Add("El campo Documento de Referencia es obligatorio.");
                                ResaltarCelda(worksheet, row, 8); // Resaltar celda H
                            }
                            if(string.IsNullOrEmpty(campoFechaIngreso)) {
                                observacionFila.Add("El campo Fecha de Ingreso es obligatorio.");
                                ResaltarCelda(worksheet, row, 9); // Resaltar celda I
                            } else {
                                if(!DateTime.TryParse(campoFechaIngreso, out _)) {
                                    observacionFila.Add("El campo Fecha de Ingreso no tiene un formato válido.");
                                    ResaltarCelda(worksheet, row, 9); // Resaltar celda I
                                }
                            }

                            if(string.IsNullOrEmpty(campoHoraIngreso)) {
                                observacionFila.Add("El campo Hora de Ingreso es obligatorio.");
                                ResaltarCelda(worksheet, row, 10); // Resaltar celda J
                            } else {
                                if(!TimeSpan.TryParse(campoHoraIngreso, out _)) {
                                    observacionFila.Add("El campo Hora de Ingreso no tiene un formato válido.");
                                    ResaltarCelda(worksheet, row, 10); // Resaltar celda J
                                }
                            }

                            if(string.IsNullOrEmpty(campoFechaSalida)) {
                                observacionFila.Add("El campo Fecha de Salida es obligatorio.");
                                ResaltarCelda(worksheet, row, 11); // Resaltar celda K
                            } else {
                                if(!DateTime.TryParse(campoFechaSalida, out _)) {
                                    observacionFila.Add("El campo Fecha de Salida no tiene un formato válido.");
                                    ResaltarCelda(worksheet, row, 11); // Resaltar celda K
                                }
                            }

                            if(string.IsNullOrEmpty(campoHoraSalida)) {
                                observacionFila.Add("El campo Hora de Salida es obligatorio.");
                                ResaltarCelda(worksheet, row, 12); // Resaltar celda L
                            } else {
                                if(!TimeSpan.TryParse(campoHoraSalida, out _)) {
                                    observacionFila.Add("El campo Hora de Salida no tiene un formato válido.");
                                    ResaltarCelda(worksheet, row, 12); // Resaltar celda L
                                }
                            }
                            if(string.IsNullOrEmpty(campoObservaciones)) {
                                observacionFila.Add("El campo Observaciones es obligatorio.");
                                ResaltarCelda(worksheet, row, 13); // Resaltar celda M
                            }

                            //Todo OK
                            if(!observacionFila.Any()) {
                                try {
                                    var registro = new ESS_EnteReguladorEntidad {
                                        CodSala = int.Parse(campoSala.Split('-')[0]),
                                        NombreSala = campoSala.Split('-')[1],
                                        FechaRegistro = DateTime.Now,
                                        UsuarioRegistro = (string)Session["UsuarioNombre"],
                                        //IdAutoriza = campoAutoriza,
                                        Descripcion = campoDescripcion,
                                        IdMotivo = int.Parse(campoMotivo.Split('-')[0]),
                                        NombreMotivo = campoMotivo.Split('-')[1],
                                        IdEmpresa = int.Parse(campoInstitucion.Split('-')[0]),
                                        NombreEmpresa = campoInstitucion.Split('-')[1],
                                        //Empleados = campoEmpleados,
                                        DocReferencia = campoDocumentoReferencia,
                                        FechaIngreso = DateTime.Parse(DateTime.Parse(campoFechaIngreso).ToString("yyyy-MM-dd")) + TimeSpan.Parse(DateTime.Parse(campoHoraIngreso).ToString("HH:mm:ss")),
                                        FechaSalida = DateTime.Parse(DateTime.Parse(campoFechaSalida).ToString("yyyy-MM-dd")) + TimeSpan.Parse(DateTime.Parse(campoHoraSalida).ToString("HH:mm:ss")),
                                        Observaciones = campoObservaciones
                                    };

                                    var observacionFila_BUK = new List<string>();
                                    var observacionFila_Empleados = new List<string>();

                                    //Validar al personal que autoriza
                                    var ExisteEmpleadoAutoriza = listadoJefeyGerente.FirstOrDefault(x => x.NumeroDocumento == campoAutoriza);
                                    if(ExisteEmpleadoAutoriza == null) {
                                        observacionFila_BUK.Add("No se encontró al Personal Autoriza.");
                                    }
                                    //Validar a los empleados de instituciones
                                    List<ESS_EnteReguladorPersonaEntidadPublicaEntidad> listadoPersonal_Institucion = new List<ESS_EnteReguladorPersonaEntidadPublicaEntidad>();
                                    listadoPersonal_Institucion = ess_entereguladorbl.ObtenerPersonasActivasPorEntidadPublica(registro.IdEmpresa);
                                    List<string> ListaEmpleados = campoEmpleados.Split(',').Select(x => x.Trim()).ToList();

                                    for(int i = 0; i < ListaEmpleados.Count; i++) {
                                        var NumeroDocumento = ListaEmpleados[i].Trim();
                                        var ExisteEmpleado = listadoPersonal_Institucion.FirstOrDefault(x => x.Dni.Trim() == NumeroDocumento.Trim());
                                        //var ExisteEmpleado = listadoPersonal_Institucion.FirstOrDefault(x => x.Dni == NumeroDocumento);

                                        if(ExisteEmpleado == null) {
                                            observacionFila_Empleados.Add($"No se encontró al Empleado {NumeroDocumento}.");
                                        } else {
                                            ESS_EntidadRegularPersonaEntidadPublica NuevoEmpleado = new ESS_EntidadRegularPersonaEntidadPublica();
                                            NuevoEmpleado.PersonaEntidadPublicaID = ExisteEmpleado.PersonaEntidadPublicaID;
                                            NuevoEmpleado.Nombres = ExisteEmpleado.Nombres;
                                            NuevoEmpleado.Apellidos = ExisteEmpleado.Apellidos;
                                            NuevoEmpleado.IdEntidadPublica = (int)ExisteEmpleado.EntidadPublicaID;
                                            NuevoEmpleado.EntidadPublicaNombre = ExisteEmpleado.EntidadPublicaNombre;
                                            NuevoEmpleado.Dni = ExisteEmpleado.Dni;
                                            NuevoEmpleado.IdCargoEntidad = (int)ExisteEmpleado.CargoEntidadID;
                                            NuevoEmpleado.CargoEntidadNombre = ExisteEmpleado.CargoEntidadNombre;
                                            var TipoDocumento = (int)ExisteEmpleado.TipoDOI;
                                            NuevoEmpleado.TipoDOI = TipoDocumento.ToString();
                                            NuevoEmpleado.FechaRegistro = DateTime.Now;

                                            registro.PersonasEntidadPublica.Add(NuevoEmpleado);
                                        }
                                    }

                                    if(!observacionFila_BUK.Any()) {
                                        var ObservacionFinal = new List<string>();
                                        registro.IdAutoriza = (int)ExisteEmpleadoAutoriza.IdAutoriza;
                                        registro.NombreAutoriza = ExisteEmpleadoAutoriza.NombreAutoriza;

                                        if(registro.PersonasEntidadPublica.Count == 0) {
                                            ObservacionFinal.Add("Pero no se registró con ningun Empleado.");
                                        } else {
                                            if(observacionFila_Empleados.Any()) {
                                                ObservacionFinal = observacionFila_Empleados;
                                            }
                                        }

                                        int insertedId = ess_entereguladorbl.GuardarRegistroEnteRegulador_ImportarExcel(registro);
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

                                    } else {
                                        string obsMsg = string.Join(" | ", observacionFila_BUK) + (observacionFila_Empleados.Count == 0 ? "" : " | " + string.Join(" | ", observacionFila_Empleados));
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
