using CapaEntidad.BUK;
using CapaEntidad.EntradaSalidaSala;
using CapaNegocio;
using CapaNegocio.BUK;
using CapaNegocio.EntradaSalidaSala;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using OfficeOpenXml.Style;
using S3k.Utilitario.clases_especial;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CapaPresentacion.Controllers.EntradaSalidaSala {
    [seguridad]
    public class BienesMaterialesController : Controller {
        private readonly SalaBL _salaBl = new SalaBL();
        private readonly EmpresaBL _empresaBl = new EmpresaBL();
        ESS_BienMaterialBL ess_bienmaterialbl = new ESS_BienMaterialBL();
        ESS_CatalogoBL ess_catalogobl = new ESS_CatalogoBL();
        DestinatarioBL destinatariobl = new DestinatarioBL();
        ClaseError error = new ClaseError();
        private readonly ESS_CargoBL _essCargoBL = new ESS_CargoBL();
        private readonly ESS_EmpresaBL _essEmpresaBL = new ESS_EmpresaBL();
        private readonly ESS_MotivoBL _essMotivoBL = new ESS_MotivoBL();
        private readonly ESS_CategoriaBL _essCategoriaBL = new ESS_CategoriaBL();
        private readonly ESS_EmpleadoBL _essEmpleadoBL = new ESS_EmpleadoBL();
        private readonly BUK_EquivalenciaEmpresaBL _equivalenciaEmpresaBL = new BUK_EquivalenciaEmpresaBL();
        private readonly BUK_EmpleadoBL _bukEmpleadoBL = new BUK_EmpleadoBL();


        public ActionResult BienesMaterialesVista() {
            return View("~/Views/EntradaSalidaSala/BienMaterial.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarBienesMaterialesxSalaJson(int[] codsala, int idtipobienmaterial, DateTime fechaini, DateTime fechafin) {
            string mensaje = "No se encontraton registros";
            bool respuesta = false;
            List<ESS_BienMaterialEntidad> listaESS_BienMaterial = new List<ESS_BienMaterialEntidad>();
            try {
                // Directorio de imagen
                string Ruta = "EntradaSalidaSala";
                string Subruta = "BienMaterial";
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
                listaESS_BienMaterial = ess_bienmaterialbl.ListadoBienMaterial(salasBusqueda, idtipobienmaterial, fechaini, fechafin);
                mensaje = "Registros obtenidos";
                respuesta = true;
                string rutaBase = ConfigurationManager.AppSettings["PathWebArchivos"].ToString() + "/";

                foreach(var item in listaESS_BienMaterial) {
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
                data = listaESS_BienMaterial
            };
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var result = new ContentResult {
                Content = serializer.Serialize(oRespuesta),
                ContentType = "application/json"
            };
            return result;
        }

        //[seguridad(false)]
        [HttpPost]
        public ActionResult GuardarBienesMateriales(ESS_BienMaterialEntidad bienmaterial, string Empleados) {
            bool respuesta = false;

            string mensaje = "No se pudo guardar los datos";
            int tamanioMaximo = 4194304;
            string extension = "";
            List<string> extensiones = new List<string>() { ".jpg", ".png", ".jpeg", ".pdf", ".docx" };
            try {
                List<ESS_BienMaterialEmpleadoEntidad> listaEmpleados = JsonConvert.DeserializeObject<List<ESS_BienMaterialEmpleadoEntidad>>(Empleados);

                bienmaterial.Empleados = listaEmpleados;
                string Ruta = "EntradaSalidaSala";
                string Subruta = "BienMaterial";
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
                    }

                }

                bienmaterial.FechaRegistro = DateTime.Now;
                bienmaterial.UsuarioRegistro = (string)Session["UsuarioNombre"];

                int insertedId = ess_bienmaterialbl.GuardarRegistroBienMaterial(bienmaterial);
                if(insertedId > 0) {
                    if(file != null) {
                        if(file.ContentLength > 0) {
                            if(!Directory.Exists(PathPrincipal)) {
                                Directory.CreateDirectory(PathPrincipal);
                            }
                            var nombreArchivoCompleto = string.Empty;
                            string nombreArchivo = $"Imagen_GRRFT_{insertedId}";
                            nombreArchivoCompleto = (nombreArchivo + extension);
                            var imagePath = Path.Combine(PathPrincipal, nombreArchivoCompleto);
                            file.SaveAs(imagePath);
                            ess_bienmaterialbl.ActualizarRutaImagen(insertedId, nombreArchivoCompleto);
                        }
                    }
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
        public ActionResult EditarBienesMateriales(ESS_BienMaterialEntidad bienmaterial, string Empleados) {
            bool respuesta = false;

            string mensaje = "No se pudo actualizar los datos";
            int tamanioMaximo = 4194304;
            string extension = "";
            List<string> extensiones = new List<string>() { ".jpg", ".png", ".jpeg", ".pdf", ".docx" };
            try {
                List<ESS_BienMaterialEmpleadoEntidad> listaEmpleados = JsonConvert.DeserializeObject<List<ESS_BienMaterialEmpleadoEntidad>>(Empleados);

                bienmaterial.Empleados = listaEmpleados;
                string Ruta = "EntradaSalidaSala";
                string Subruta = "BienMaterial";
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

                bienmaterial.FechaModificacion = DateTime.Now;
                bienmaterial.UsuarioModificacion = (string)Session["UsuarioNombre"];

                respuesta = ess_bienmaterialbl.EditarBienesMateriales(bienmaterial);


                if(respuesta) {
                    //Guardar Imagen
                    if(file != null) {

                        if(file.ContentLength > 0) {

                            if(!Directory.Exists(PathPrincipal)) {
                                Directory.CreateDirectory(PathPrincipal);
                            }

                            var nombreArchivoCompleto = string.Empty;
                            string nombreArchivo = $"Imagen_GRRFT_{bienmaterial.IdBienMaterial}";
                            nombreArchivoCompleto = (nombreArchivo + extension);
                            var imagePath = Path.Combine(PathPrincipal, nombreArchivoCompleto);
                            if(System.IO.File.Exists(imagePath)) {
                                System.IO.File.Delete(imagePath);
                            }
                            file.SaveAs(imagePath);
                            ess_bienmaterialbl.ActualizarRutaImagen(bienmaterial.IdBienMaterial, nombreArchivoCompleto);

                        }
                    }

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
        public JsonResult EliminarBienesMateriales(int id) {
            bool respuesta = false;
            string mensaje = "No se pudo eliminar el registro.";

            try {
                respuesta = ess_bienmaterialbl.EliminarRegistroBienMaterial(new ESS_BienMaterialEntidad { IdBienMaterial = id });
                if(respuesta) {
                    mensaje = "Registro eliminado correctamente.";
                }
            } catch(Exception ex) {
                mensaje = "Error al eliminar el registro: " + ex.Message;
            }

            return Json(new { respuesta, mensaje });
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

        #region ESS_EmpresaExterna
        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarEmpresa() {
            string mensaje = string.Empty;
            bool respuesta = false;
            var lista = new List<ESS_EmpresaEntidad>();
            try {

                lista = _essEmpresaBL.ListarEmpresa();
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
        public ActionResult ListarEmpresaPorEstado(int estado = 1) {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_EmpresaEntidad>();
            try {

                data = _essEmpresaBL.ListarEmpresaPorEstado(estado);
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
        public ActionResult ObtenerEmpresaPorId(int id) {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new ESS_EmpresaEntidad();
            try {

                data = _essEmpresaBL.ObtenerEmpresaPorId(id);
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
        public ActionResult InsertarEmpresa(ESS_EmpresaEntidad model) {
            string mensaje = "No se pudo insertar el registro";
            bool respuesta = false;
            try {
                model.FechaRegistro = DateTime.Now;
                model.UsuarioRegistro = (string)Session["UsuarioNombre"];
                int idInsertado = _essEmpresaBL.InsertarEmpresa(model);
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
        public ActionResult EditarEmpresa(ESS_EmpresaEntidad model) {
            string mensaje = "No se pudo editar el registro";
            bool respuesta = false;
            try {
                model.FechaModificacion = DateTime.Now;
                model.UsuarioModificacion = (string)Session["UsuarioNombre"];
                respuesta = _essEmpresaBL.EditarEmpresa(model);
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
            var data = new List<ESS_MotivoEntidad>();
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
            var data = new List<ESS_MotivoEntidad>();
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
        public ActionResult InsertarMotivo(ESS_MotivoEntidad model) {
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
        public ActionResult EditarMotivo(ESS_MotivoEntidad model) {
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
        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarCategoriaPorEstado(int estado = 1) {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_CategoriaEntidad>();
            try {

                data = _essCategoriaBL.ListarCategoriaPorEstado(estado);
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
        public ActionResult InsertarCategoria(ESS_CategoriaEntidad model) {
            string mensaje = "No se pudo insertar el registro";
            bool respuesta = false;
            try {
                model.FechaRegistro = DateTime.Now;
                model.UsuarioRegistro = (string)Session["UsuarioNombre"];
                int idInsertado = _essCategoriaBL.InsertarCategoria(model);
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
        public ActionResult EditarCategoria(ESS_CategoriaEntidad model) {
            string mensaje = "No se pudo editar el registro";
            bool respuesta = false;
            try {
                model.FechaModificacion = DateTime.Now;
                model.UsuarioModificacion = (string)Session["UsuarioNombre"];
                respuesta = _essCategoriaBL.EditarCategoria(model);
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
        [HttpGet]
        public ActionResult ObtenerEmpleadoExternoActivosPorPatron(int idempresa, string term) {
            bool success = false;
            List<ESS_EmpleadoEntidad> result = new List<ESS_EmpleadoEntidad>();
            string mensaje;

            try {
                result = _essEmpleadoBL.ObtenerEmpleadoExternoActivosPorTermino(idempresa, term);
                success = result.Count > 0;
                mensaje = success ? "Lista de empleados" : "No se encontraton empleados";
            } catch(Exception exp) {
                mensaje = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = result, mensaje }, JsonRequestBehavior.AllowGet);
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
        public ActionResult ReporteBienesMaterialesDescargarExcelJson(int[] codsala, int idtipobienmaterial, DateTime fechaIni, DateTime fechaFin) {
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

                List<ESS_BienMaterialEntidad> lista = ess_bienmaterialbl.ListadoBienMaterial(salasBusqueda, idtipobienmaterial, fechaIni, fechaFin);
                lista = lista.OrderByDescending(x => x.IdBienMaterial).ToList();
                if(lista.Count > 0) {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    // Hoja Principal: Bienes Materiales
                    var workSheet = excel.Workbook.Worksheets.Add("BienesMateriales");
                    workSheet.TabColor = Color.Black;
                    workSheet.DefaultRowHeight = 12;

                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[2, 2, 2, 12].Merge = true;
                    workSheet.Cells[2, 2].Value = "Reporte de Bienes Materiales";
                    workSheet.Cells[2, 2].Style.Font.Bold = true;
                    workSheet.Cells[2, 2].Style.Font.Size = 13;
                    workSheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Sala";
                    workSheet.Cells[3, 4].Value = "Tipo Bien Material";
                    workSheet.Cells[3, 5].Value = "Categoría";
                    workSheet.Cells[3, 6].Value = "Descripción";
                    workSheet.Cells[3, 7].Value = "Motivo";
                    workSheet.Cells[3, 8].Value = "Empresa";
                    workSheet.Cells[3, 9].Value = "Fecha Ingreso";
                    workSheet.Cells[3, 10].Value = "Fecha Salida";
                    workSheet.Cells[3, 11].Value = "Observaciones";
                    workSheet.Cells[3, 12].Value = "Empleados";

                    int recordIndex = 4;
                    foreach(var item in lista) {
                        workSheet.Cells[recordIndex, 2].Value = item.IdBienMaterial;
                        workSheet.Cells[recordIndex, 3].Value = item.NombreSala;
                        workSheet.Cells[recordIndex, 4].Value = item.TipoBienMaterial == 1 ? "Interno" : "Externo";
                        //workSheet.Cells[recordIndex, 5].Value = item.NombreCategoria; 
                        workSheet.Cells[recordIndex, 6].Value = item.Descripcion;
                        workSheet.Cells[recordIndex, 7].Value = item.NombreMotivo;
                        workSheet.Cells[recordIndex, 8].Value = item.NombreEmpresa;
                        workSheet.Cells[recordIndex, 9].Value = item.FechaIngreso.ToString("dd-MM-yyyy hh:mm tt");
                        workSheet.Cells[recordIndex, 10].Value = item.FechaSalida.ToString("dd-MM-yyyy") == "01-01-1753" ? "No Definido" : item.FechaSalida.ToString("dd-MM-yyyy hh:mm tt");
                        workSheet.Cells[recordIndex, 11].Value = string.IsNullOrEmpty(item.Observaciones) ? "No definido" : item.Observaciones;

                        if(item.NombreCategoria == "OTROS") {
                            workSheet.Cells[recordIndex, 5].Value = $"{item.NombreCategoria} ({item.DescripcionCategoria})";
                        } else {
                            workSheet.Cells[recordIndex, 5].Value = item.NombreCategoria;
                        }
                        workSheet.Column(5).Width = 30;

                        if(item.Empleados != null && item.Empleados.Any()) {
                            var employeeSheet = excel.Workbook.Worksheets.Add($"Detalle_BienMaterial_{item.IdBienMaterial}");

                            employeeSheet.Cells[1, 2, 1, 8].Merge = true;
                            employeeSheet.Cells[1, 2].Value = $"Detalle : Bien Material N°: {item.IdBienMaterial}";
                            employeeSheet.Cells[1, 2].Style.Font.Bold = true;
                            employeeSheet.Cells[1, 2].Style.Font.Size = 13;
                            employeeSheet.Cells[1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            employeeSheet.Cells[3, 2].Value = "ID";
                            employeeSheet.Cells[3, 3].Value = "Nombre";
                            employeeSheet.Cells[3, 4].Value = "Apellido Paterno";
                            employeeSheet.Cells[3, 5].Value = "Apellido Materno";
                            employeeSheet.Cells[3, 6].Value = "Tipo Documento";
                            employeeSheet.Cells[3, 7].Value = "Documento";
                            employeeSheet.Cells[3, 8].Value = "Cargo";

                            Color headerBackground = ColorTranslator.FromHtml("#003268");
                            employeeSheet.Cells["B3:H3"].Style.Font.Bold = true;
                            employeeSheet.Cells["B3:H3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            employeeSheet.Cells["B3:H3"].Style.Fill.BackgroundColor.SetColor(headerBackground);
                            employeeSheet.Cells["B3:H3"].Style.Font.Color.SetColor(Color.White);
                            employeeSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            int employeeIndex = 4;
                            foreach(var empleado in item.Empleados) {
                                employeeSheet.Cells[employeeIndex, 2].Value = empleado.IdEmpleado;
                                employeeSheet.Cells[employeeIndex, 3].Value = empleado.Nombre;
                                employeeSheet.Cells[employeeIndex, 4].Value = empleado.ApellidoPaterno;
                                employeeSheet.Cells[employeeIndex, 5].Value = empleado.ApellidoMaterno;
                                employeeSheet.Cells[employeeIndex, 6].Value = empleado.TipoDocumento.ToUpper();
                                employeeSheet.Cells[employeeIndex, 7].Value = empleado.DocumentoRegistro;
                                employeeSheet.Cells[employeeIndex, 8].Value = empleado.Cargo;
                                employeeIndex++;
                            }

                            for(int i = 2; i <= 8; i++) {
                                employeeSheet.Column(i).AutoFit();
                            }

                            var hyperlink = new ExcelHyperLink($"#'Detalle_BienMaterial_{item.IdBienMaterial}'!A1", "Ver Detalles");
                            workSheet.Cells[recordIndex, 12].Hyperlink = hyperlink;
                            workSheet.Cells[recordIndex, 12].Style.Font.UnderLine = true;
                            workSheet.Cells[recordIndex, 12].Style.Font.Color.SetColor(Color.Blue);
                        } else {
                            workSheet.Cells[recordIndex, 12].Value = "No hay empleados";
                        }

                        recordIndex++;
                    }

                    Color principalHeaderBackground = ColorTranslator.FromHtml("#003268");
                    workSheet.Cells["B3:L3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:L3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:L3"].Style.Fill.BackgroundColor.SetColor(principalHeaderBackground);
                    workSheet.Cells["B3:L3"].Style.Font.Color.SetColor(Color.White);

                    for(int i = 2; i <= 12; i++) {
                        if(i != 5) {
                            workSheet.Column(i).AutoFit();
                        }
                    }

                    excelName = $"ReporteBienMaterial_{fechaIni:dd_MM_yyyy}_al_{fechaFin:dd_MM_yyyy}.xlsx";
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
        [HttpPost]
        public ActionResult ObtenerPorIdBienesMateriales(int id) {
            // Directorio de imagen
            string Ruta = "EntradaSalidaSala";
            string Subruta = "BienMaterial";
            string PathPrincipal = Path.Combine(Ruta, Subruta);
            // Fin Directorio de imagen

            string mensaje = string.Empty;
            bool respuesta = false;
            string rutaBase = ConfigurationManager.AppSettings["PathWebArchivos"].ToString() + "/";
            var data = new ESS_BienMaterialEntidad();
            try {

                data = ess_bienmaterialbl.GetBienMaterialPorId(id);
                mensaje = "Obteniendo registro";
                respuesta = true;
                if(!string.IsNullOrEmpty(data.RutaImagen)) {
                    var RutaImagen = Path.Combine(PathPrincipal, data.RutaImagen);
                    data.RutaImagen = new Uri(new Uri(rutaBase), RutaImagen).ToString();
                }
                return Json(new { mensaje, respuesta, data });
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, contacte con administrador";
                respuesta = false;
                return Json(new { mensaje, respuesta });
            }
        }
        [seguridad(false)]
        [HttpGet]
        public ActionResult DescargarArchivo(string nombreArchivo) {
            string rutaBase = ConfigurationManager.AppSettings["PathArchivos"].ToString();
            string rutaCompleta = Path.Combine(rutaBase, nombreArchivo);

            if(!System.IO.File.Exists(rutaCompleta)) {
                return HttpNotFound();
            }

            return File(rutaCompleta, "application/octet-stream", nombreArchivo);
        }
        //[seguridad(false)]
        //public FileResult DescargarImagenBienMaterial(string fileName = "") {

        //    //string Ruta = "EntradaSalidaSala";
        //    //string Subruta = "BienMaterial";
        //    //string PathPrincipal = Path.Combine(ConfigurationManager.AppSettings["PathArchivos"].ToString());
        //    string fullName = Path.Combine(ConfigurationManager.AppSettings["PathArchivos"].ToString(), fileName);
        //    byte[] fileBytes = ConvertirArchivo(fullName);
        //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        //}
        //[seguridad(false)]
        //byte[] ConvertirArchivo(string s) {
        //    System.IO.FileStream fs = System.IO.File.OpenRead(s);
        //    byte[] data = new byte[fs.Length];
        //    int br = fs.Read(data, 0, data.Length);
        //    if(br != fs.Length)
        //        throw new System.IO.IOException(s);
        //    return data;
        //}

        [seguridad(false)]
        [HttpPost]
        public ActionResult ProcesarSeccionExcel() {
            bool response = false;
            string errormensaje = "";
            HttpPostedFileBase file = Request.Files["file"];
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try {
                using(var package = new ExcelPackage(file.InputStream)) {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    List<object> formulariosLeidos = new List<object>();

                    // Definir las posiciones iniciales para cada formulario
                    int totalFormularios = 5; // Número de formularios a procesar
                    int filaInicio = 5;
                    int filaFin = 16;
                    int columnaInicio = 2; // B
                    int columnasPorFormulario = 5; // Rango de columnas por formulario

                    for(int i = 0; i < totalFormularios; i++) {
                        // Determinar la columna de inicio y de lectura clave
                        int columnaClave = columnaInicio + 3; // Columna clave (E5, L5, S5, etc.)
                        string valorClave = worksheet.Cells[filaInicio, columnaClave].Value?.ToString().Trim();

                        // Si la celda clave tiene "Sí", procesar el formulario
                        if(valorClave?.ToUpper() == "SI") {
                            List<object> datosFormulario = new List<object>();

                            for(int fila = filaInicio; fila <= filaFin; fila++) {
                                var filaDatos = new {
                                    ColumnaB = worksheet.Cells[fila, columnaInicio].Value?.ToString().Trim(),
                                    ColumnaC = worksheet.Cells[fila, columnaInicio + 1].Value?.ToString().Trim(),
                                    ColumnaD = worksheet.Cells[fila, columnaInicio + 2].Value?.ToString().Trim(),
                                    ColumnaE = worksheet.Cells[fila, columnaInicio + 3].Value?.ToString().Trim(),
                                    ColumnaF = worksheet.Cells[fila, columnaInicio + 4].Value?.ToString().Trim()
                                };

                                datosFormulario.Add(filaDatos);
                            }

                            formulariosLeidos.Add(new {
                                FormularioIndex = i + 1,
                                Datos = datosFormulario
                            });
                        }

                        // Avanzar al siguiente formulario (mover columnas)
                        columnaInicio += columnasPorFormulario + 2; // Avanzar al siguiente bloque
                    }

                    response = true;
                    return Json(new { respuesta = response, formulariosLeidos });
                }
            } catch(Exception ex) {
                return Json(new { respuesta = response, mensaje = $"Error procesando el archivo: {ex.Message}" });
            }
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ReporteBienesMaterialesDescargarExcelJson2(int[] codsala, int idtipobienmaterial, DateTime fechaIni, DateTime fechaFin) {
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

                List<ESS_BienMaterialEntidad> lista = ess_bienmaterialbl.ListadoBienMaterial(salasBusqueda, idtipobienmaterial, fechaIni, fechaFin);
                lista = lista.OrderByDescending(x => x.IdBienMaterial).ToList();

                if(lista.Count > 0) {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    // Hoja Principal: Bienes Materiales
                    var workSheet = excel.Workbook.Worksheets.Add("BienesMateriales");
                    workSheet.TabColor = Color.Black;
                    workSheet.DefaultRowHeight = 12;

                    // Título
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[2, 2, 2, 12].Merge = true;
                    workSheet.Cells[2, 2].Value = "Reporte de Bienes Materiales";
                    workSheet.Cells[2, 2].Style.Font.Bold = true;
                    workSheet.Cells[2, 2].Style.Font.Size = 13;
                    workSheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    // Cabeceras
                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Sala";
                    workSheet.Cells[3, 4].Value = "Tipo Bien Material";
                    workSheet.Cells[3, 5].Value = "Categoría";
                    workSheet.Cells[3, 6].Value = "Descripción";
                    workSheet.Cells[3, 7].Value = "Motivo";
                    workSheet.Cells[3, 8].Value = "Empresa";
                    workSheet.Cells[3, 9].Value = "Fecha Ingreso";
                    workSheet.Cells[3, 10].Value = "Fecha Salida";
                    workSheet.Cells[3, 11].Value = "Observaciones";
                    workSheet.Cells[3, 12].Value = "Empleados";

                    // Datos para lista desplegable
                    var listaDatos = new List<string> { "Dato1", "Dato2", "Dato3" }; // Reemplaza con datos de tu BD.
                    int dataStartRow = 4;
                    for(int i = 0; i < listaDatos.Count; i++) {
                        workSheet.Cells[dataStartRow + i, 14].Value = listaDatos[i]; // Columna N (14) para datos.
                    }

                    // Crear lista desplegable
                    var validation = workSheet.DataValidations.AddListValidation("G4:G100"); // Rango donde aplicar la lista.
                    validation.Formula.ExcelFormula = "$N$4:$N$" + (dataStartRow + listaDatos.Count - 1);
                    validation.ShowErrorMessage = true;
                    validation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    validation.ErrorTitle = "Valor inválido";
                    validation.Error = "Seleccione un valor de la lista.";

                    int recordIndex = 4;
                    foreach(var item in lista) {
                        workSheet.Cells[recordIndex, 2].Value = item.IdBienMaterial;
                        workSheet.Cells[recordIndex, 3].Value = item.NombreSala;
                        workSheet.Cells[recordIndex, 4].Value = item.TipoBienMaterial == 1 ? "Interno" : "Externo";
                        workSheet.Cells[recordIndex, 6].Value = item.Descripcion;
                        recordIndex++;
                    }

                    // Generar archivo Excel
                    byte[] bin = excel.GetAsByteArray();
                    base64String = Convert.ToBase64String(bin);
                    excelName = "Reporte_Bienes_Materiales_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    respuesta = true;
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }

            return Json(new { respuesta, mensaje, base64String, excelName });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult PlantillaBienesMaterialesDescargarExcel() {
            string path = Server.MapPath(@"~/Content/ess_excel_plantilla/Plantilla_ESS_BienMaterial.xlsx");
            string base64String = "";
            string excelName = "";
            string mensaje = "";
            bool respuesta = false;

            try {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                var categorias = _essCategoriaBL.ListarCategoriaPorEstado(1);
                var motivos = _essMotivoBL.ListarMotivoPorEstado(1);
                var EmpresaExterna = _essEmpresaBL.ListarEmpresaPorEstado(1);
                var EmpresaIntera = _equivalenciaEmpresaBL.ObtenerTodasLasEquivalenciasEmpresa();

                List<string> listaDatosSala = salas.Select(x => $"{x.CodSala}-{x.Nombre}").ToList();
                List<string> listaDatosTipoBien = new List<string> { "Interno", "Externo" };
                List<string> listaDatosCategoria = categorias.Select(x => $"{x.IdCategoria}-{x.Nombre}").ToList();
                List<string> listaDatosMotivo = motivos.Select(x => $"{x.IdMotivo}-{x.Nombre}").ToList();
                List<string> listaEmpresaExterna = EmpresaExterna.Select(x => $"{x.IdEmpresaExterna}-{x.Nombre}").ToList();
                List<string> listaEmpresaInterna = EmpresaIntera.Select(x => $"{x.IdEmpresaBuk}-{x.Nombre}").ToList();


                using(var package = new ExcelPackage(new FileInfo(path))) {

                    var workSheet = package.Workbook.Worksheets[0]; // Seleccionar la primera hoja

                    int startRowSala = 2;
                    int startRowTipoBien = 1;
                    int columnStartTipoBien = 27;

                    var validationSala = workSheet.DataValidations.AddListValidation("B5:B30");
                    validationSala.ShowErrorMessage = true;
                    validationSala.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    validationSala.ErrorTitle = "Valor inválido";
                    validationSala.Error = "Seleccione un valor de la lista.";

                    var validationTipoBien = workSheet.DataValidations.AddListValidation("C5:C30");
                    validationTipoBien.ShowErrorMessage = true;
                    validationTipoBien.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    validationTipoBien.ErrorTitle = "Valor inválido";
                    validationTipoBien.Error = "Seleccione un valor de la lista.";
                    validationTipoBien.Formula.ExcelFormula = $"AA2:AA{startRowTipoBien + listaDatosTipoBien.Count}";

                    var validationCategoria = workSheet.DataValidations.AddListValidation("D5:D30");
                    validationCategoria.ShowErrorMessage = true;
                    validationCategoria.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    validationCategoria.ErrorTitle = "Valor inválido";
                    validationCategoria.Error = "Seleccione un valor de la lista.";

                    var validationMotivo = workSheet.DataValidations.AddListValidation("F5:F30");
                    validationMotivo.ShowErrorMessage = true;
                    validationMotivo.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    validationMotivo.ErrorTitle = "Valor inválido";
                    validationMotivo.Error = "Seleccione un valor de la lista.";

                    //var validationEmpresa = workSheet.DataValidations.AddListValidation("E12");
                    //validationEmpresa.ShowErrorMessage = true;
                    //validationEmpresa.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                    //validationEmpresa.ErrorTitle = "Valor inválido";
                    //validationEmpresa.Error = "Seleccione un valor de Empresa."; 
                    //validationEmpresa.Formula.ExcelFormula =
                    //    $"IF(C10=\"Interno\",AB2:AB{startRowTipoBien + listaEmpresaInterna.Count}," +
                    //    $"IF(C10=\"Externo\",AC2:AC{startRowTipoBien + listaEmpresaExterna.Count},\"\"))";


                    for(int j = 0; j < listaDatosSala.Count; j++) {
                        workSheet.Cells[startRowSala + j, 26].Value = listaDatosSala[j];
                    }
                    int endRowSala = startRowSala + listaDatosSala.Count - 1;
                    validationSala.Formula.ExcelFormula = $"$Z${startRowSala}:$Z${endRowSala}";
                    workSheet.Column(26).Hidden = true;


                    workSheet.Cells[startRowTipoBien, columnStartTipoBien].Value = "TipoBien";
                    for(int j = 0; j < listaDatosTipoBien.Count; j++) {
                        workSheet.Cells[startRowTipoBien + j + 1, columnStartTipoBien].Value = listaDatosTipoBien[j];
                    }
                    int endRowTipoBien = startRowTipoBien + 1 + listaDatosTipoBien.Count - 1;
                    validationTipoBien.Formula.ExcelFormula = $"$AA${startRowTipoBien + 1}:$AA${endRowTipoBien}";
                    workSheet.Column(columnStartTipoBien).Hidden = true;

                    //Empresa interna
                    workSheet.Cells[startRowTipoBien, columnStartTipoBien + 1].Value = "Interno";
                    for(int j = 0; j < listaEmpresaInterna.Count; j++) {
                        workSheet.Cells[startRowTipoBien + j + 1, columnStartTipoBien + 1].Value = listaEmpresaInterna[j];
                    }
                    workSheet.Column(columnStartTipoBien + 1).Hidden = true;

                    //Empresa externa
                    for(int j = 0; j < listaEmpresaExterna.Count; j++) {
                        workSheet.Cells[startRowTipoBien + j + 1, columnStartTipoBien + 2].Value = listaEmpresaExterna[j];
                    }
                    workSheet.Column(columnStartTipoBien + 2).Hidden = true;

                    //Categoria
                    for(int j = 0; j < listaDatosCategoria.Count; j++) {
                        workSheet.Cells[startRowTipoBien + j + 1, columnStartTipoBien + 3].Value = listaDatosCategoria[j];
                    }
                    int endRowCategoria = startRowTipoBien + 1 + listaDatosCategoria.Count - 1;
                    validationCategoria.Formula.ExcelFormula = $"$AD${startRowTipoBien + 1}:$AD${endRowCategoria}";
                    workSheet.Column(columnStartTipoBien + 3).Hidden = true;


                    for(int j = 0; j < listaDatosMotivo.Count; j++) {
                        workSheet.Cells[startRowTipoBien + j + 1, columnStartTipoBien + 4].Value = listaDatosMotivo[j];
                    }
                    int endRowMotivo = startRowTipoBien + 1 + listaDatosMotivo.Count - 1;
                    validationMotivo.Formula.ExcelFormula = $"$AE${startRowTipoBien + 1}:$AE${endRowMotivo}";
                    workSheet.Column(columnStartTipoBien + 4).Hidden = true;


                    for(int row = 5; row <= 30; row++) {
                        workSheet.Cells[row, 7].Value = "--Seleccione un Tipo Bien--"; //Mensaje por default

                        var validationEmpresa = workSheet.DataValidations.AddListValidation($"G{row}");
                        validationEmpresa.ShowErrorMessage = true;
                        validationEmpresa.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                        validationEmpresa.ErrorTitle = "Valor inválido";
                        validationEmpresa.Error = "Seleccione un valor de Empresa.";

                        // La fórmula verifica el valor en la columna C de la misma fila
                        validationEmpresa.Formula.ExcelFormula =
                            $"IF(C{row}=\"Interno\",AB2:AB{startRowTipoBien + listaEmpresaInterna.Count}," +
                            $"IF(C{row}=\"Externo\",AC2:AC{startRowTipoBien + listaEmpresaExterna.Count},\"\"))";
                    }

                    byte[] bin = package.GetAsByteArray();
                    base64String = Convert.ToBase64String(bin);
                    excelName = "Plantilla_BienMaterial_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx";
                    respuesta = true;
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }

            return Json(new { respuesta, mensaje, data = base64String, excelName });
        }


        [HttpPost]
        public ActionResult ImportarExcelBienesMateriales() {
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

                var listadoEmpresaInterna = new List<BUK_EquivalenciaEmpresaEntidad>();
                var ListadoEmpresaExterna = new List<ESS_EmpresaEntidad>();
                listadoEmpresaInterna = _equivalenciaEmpresaBL.ObtenerTodasLasEquivalenciasEmpresa();
                ListadoEmpresaExterna = _essEmpresaBL.ListarEmpresaPorEstado(1);





                string fechaHora = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string nombreProcesado = $"Procesado_{fechaHora}_{nombreOriginal}";
                using(var stream = archivoExcel.InputStream) {
                    using(var package = new ExcelPackage(stream)) {
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                        if(worksheet == null)
                            return Json(new { respuesta = false, mensaje = "El archivo no contiene hojas válidas." });

                        int ColumnaObservacionesImportar = 15;
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
                            for(int col = 2; col <= 14; col++) {
                                var valorCelda = worksheet.Cells[row, col].Value?.ToString().Trim();

                                //if (!string.IsNullOrEmpty(valorCelda))
                                //{
                                //    filaVacia = false;
                                //}


                                if(!string.IsNullOrEmpty(valorCelda)) {
                                    filaVacia = false;
                                } else {
                                    if(valorCelda == "--Seleccione un Tipo Bien--") {
                                        filaVacia = false;
                                    } else {
                                        break;
                                    }
                                }
                            }

                            if(filaVacia) {
                                continue;
                            }

                            var observacionFila = new List<string>();

                            string campoSala = worksheet.Cells[row, 2].Text.Trim(); // Columna B
                            string campoTipoBien = worksheet.Cells[row, 3].Text.Trim(); // Columna C
                            string campoCategoria = worksheet.Cells[row, 4].Text.Trim(); // Columna D
                            string campoDescripcion = worksheet.Cells[row, 5].Text.Trim(); // Columna E
                            string campoMotivo = worksheet.Cells[row, 6].Text.Trim(); // Columna F
                            string campoEmpresaoInstitucion = worksheet.Cells[row, 7].Text.Trim(); // Columna G
                            string campoEmpleados = worksheet.Cells[row, 8].Text.Trim(); // Columna H
                            string campoDocumentoReferencia = worksheet.Cells[row, 9].Text.Trim(); // Columna I
                            string campoFechaIngreso = worksheet.Cells[row, 10].Text.Trim(); // Columna J
                            string campoHoraIngreso = worksheet.Cells[row, 11].Text.Trim(); // Columna K
                            string campoFechaSalida = worksheet.Cells[row, 12].Text.Trim(); // Columna L
                            string campoHoraSalida = worksheet.Cells[row, 13].Text.Trim(); // Columna M
                            string campoObservaciones = worksheet.Cells[row, 14].Text.Trim(); // Columna N


                            // Validaciones 
                            if(string.IsNullOrEmpty(campoSala)) {
                                observacionFila.Add("El campo Sala es obligatorio.");
                                ResaltarCelda(worksheet, row, 2); // Resaltar celda B
                            }
                            if(string.IsNullOrEmpty(campoTipoBien)) {
                                observacionFila.Add("El campo Tipo de Bien es obligatorio.");
                                ResaltarCelda(worksheet, row, 3); // Resaltar celda C
                            } else {
                                if(!(campoTipoBien.ToLower() == "interno" || campoTipoBien.ToLower() == "externo")) {
                                    observacionFila.Add("El campo Tipo Bien solo puede ser Interno o Externo.");
                                }
                            }
                            if(string.IsNullOrEmpty(campoCategoria)) {
                                observacionFila.Add("El campo Categoría es obligatorio.");
                                ResaltarCelda(worksheet, row, 4); // Resaltar celda D
                            }
                            if(string.IsNullOrEmpty(campoDescripcion)) {
                                observacionFila.Add("El campo Descripción es obligatorio.");
                                ResaltarCelda(worksheet, row, 5); // Resaltar celda E
                            }
                            if(string.IsNullOrEmpty(campoMotivo)) {
                                observacionFila.Add("El campo Motivo es obligatorio.");
                                ResaltarCelda(worksheet, row, 6); // Resaltar celda F
                            }
                            if(string.IsNullOrEmpty(campoEmpresaoInstitucion)) {
                                observacionFila.Add("El campo Empresa/Institución es obligatorio.");
                                ResaltarCelda(worksheet, row, 7); // Resaltar celda G
                            }
                            if(string.IsNullOrEmpty(campoEmpleados)) {
                                observacionFila.Add("El campo Empleados es obligatorio.");
                                ResaltarCelda(worksheet, row, 8); // Resaltar celda H
                            } else {
                                if(campoEmpleados.Split(',').Any(x => string.IsNullOrEmpty(x.Trim()))) {
                                    observacionFila.Add("El campo Empleados no tiene un formato válido.");
                                    ResaltarCelda(worksheet, row, 8); // Resaltar celda H
                                }
                            }
                            //if(string.IsNullOrEmpty(campoDocumentoReferencia))
                            //    {
                            //    observacionFila.Add("El campo Documento de Referencia es obligatorio.");
                            //    ResaltarCelda(worksheet, row, 9); // Resaltar celda I
                            //}
                            //Fecha Ingreso
                            if(string.IsNullOrEmpty(campoFechaIngreso)) {
                                observacionFila.Add("El campo Fecha de Ingreso es obligatorio.");
                                ResaltarCelda(worksheet, row, 10); // Resaltar celda J
                            } else {
                                if(!DateTime.TryParse(campoFechaIngreso, out _)) {
                                    observacionFila.Add("El campo Fecha de Ingreso no tiene un formato válido.");
                                    ResaltarCelda(worksheet, row, 10); // Resaltar celda J
                                }
                            }
                            if(string.IsNullOrEmpty(campoHoraIngreso)) {
                                observacionFila.Add("El campo Hora de Ingreso es obligatorio.");
                                ResaltarCelda(worksheet, row, 11); // Resaltar celda K
                            } else {
                                if(!TimeSpan.TryParse(campoHoraIngreso, out _)) {
                                    observacionFila.Add("El campo Hora de Ingreso no tiene un formato válido.");
                                    ResaltarCelda(worksheet, row, 11); // Resaltar celda K
                                }
                            }
                            //Fecha Salida
                            if(string.IsNullOrEmpty(campoFechaSalida)) {
                                observacionFila.Add("El campo Fecha de Salida es obligatorio.");
                                ResaltarCelda(worksheet, row, 12); // Resaltar celda L
                            } else {
                                if(!DateTime.TryParse(campoFechaSalida, out _)) {
                                    observacionFila.Add("El campo Fecha de Salida no tiene un formato válido.");
                                    ResaltarCelda(worksheet, row, 12); // Resaltar celda L
                                }
                            }
                            if(string.IsNullOrEmpty(campoHoraSalida)) {
                                observacionFila.Add("El campo Hora de Salida es obligatorio.");
                                ResaltarCelda(worksheet, row, 13); // Resaltar celda M
                            } else {
                                if(!TimeSpan.TryParse(campoHoraSalida, out _)) {
                                    observacionFila.Add("El campo Hora de Salida no tiene un formato válido.");
                                    ResaltarCelda(worksheet, row, 13); // Resaltar celda M
                                }
                            }
                            //Todo OK
                            if(!observacionFila.Any()) {
                                try {
                                    var registro = new ESS_BienMaterialEntidad {
                                        CodSala = int.Parse(campoSala.Split('-')[0]),
                                        NombreSala = campoSala.Split('-')[1],
                                        FechaRegistro = DateTime.Now,
                                        UsuarioRegistro = (string)Session["UsuarioNombre"],
                                        TipoBienMaterial = campoTipoBien.ToLower() == "interno" ? 1 : 2, //Interno o Externo
                                        IdCategoria = int.Parse(campoCategoria.Split('-')[0]),
                                        NombreCategoria = campoCategoria.Split('-')[1],
                                        Descripcion = campoDescripcion,
                                        IdMotivo = int.Parse(campoMotivo.Split('-')[0]),
                                        NombreMotivo = campoMotivo.Split('-')[1],
                                        IdEmpresa = int.Parse(campoEmpresaoInstitucion.Split('-')[0]),
                                        NombreEmpresa = campoEmpresaoInstitucion.Split('-')[1],
                                        GRRFT = campoDocumentoReferencia,
                                        FechaIngreso = DateTime.Parse(DateTime.Parse(campoFechaIngreso).ToString("yyyy-MM-dd")) + TimeSpan.Parse(DateTime.Parse(campoHoraIngreso).ToString("HH:mm:ss")),
                                        FechaSalida = DateTime.Parse(DateTime.Parse(campoFechaSalida).ToString("yyyy-MM-dd")) + TimeSpan.Parse(DateTime.Parse(campoHoraSalida).ToString("HH:mm:ss")),
                                        Observaciones = campoObservaciones
                                    };

                                    var observacionFila_Empleados = new List<string>();
                                    var observacionFila_Empresa_Instituciones = new List<string>();
                                    var listadoEmpresaGeneral = new List<ESS_EmpresaEntidad>();
                                    List<ESS_BienMaterialEmpleadoEntidad> personasEmpleado = new List<ESS_BienMaterialEmpleadoEntidad>();

                                    List<ESS_EmpleadoEntidad> listadoEmpleadosExterno = new List<ESS_EmpleadoEntidad>();
                                    List<BUK_EmpleadoEntidad> listadoEmpleadosInterno = new List<BUK_EmpleadoEntidad>();

                                    List<string> ListaEmpleados = campoEmpleados.Split(',').Select(x => x.Trim()).ToList();

                                    for(int i = 0; i < ListaEmpleados.Count; i++) {
                                        var NumeroDocumento = ListaEmpleados[i].Trim();
                                        if(registro.TipoBienMaterial == 1) {
                                            listadoEmpleadosInterno = _bukEmpleadoBL.ObtenerEmpleadosActivosPorTermino(registro.IdEmpresa, string.Empty);
                                            var ExisteEmpleado = listadoEmpleadosInterno.FirstOrDefault(x => x.NumeroDocumento.Trim() == NumeroDocumento.Trim());

                                            if(ExisteEmpleado == null) {
                                                observacionFila_Empleados.Add($"No se encontró al Empleado {NumeroDocumento}.");
                                            } else {
                                                ESS_BienMaterialEmpleadoEntidad NuevoEmpleado = new ESS_BienMaterialEmpleadoEntidad();

                                                NuevoEmpleado.IdEmpleado = ExisteEmpleado.IdBuk;
                                                NuevoEmpleado.Nombre = ExisteEmpleado.Nombres;
                                                NuevoEmpleado.ApellidoPaterno = ExisteEmpleado.ApellidoPaterno;
                                                NuevoEmpleado.ApellidoMaterno = ExisteEmpleado.ApellidoMaterno;
                                                NuevoEmpleado.DocumentoRegistro = ExisteEmpleado.NumeroDocumento;
                                                NuevoEmpleado.IdCargo = ExisteEmpleado.IdCargo;
                                                NuevoEmpleado.NombreCargo = ExisteEmpleado.Cargo;
                                                NuevoEmpleado.TipoDocumento = ExisteEmpleado.TipoDocumento;
                                                NuevoEmpleado.IdEmpresa = ExisteEmpleado.IdEmpresa;
                                                NuevoEmpleado.Empresa = ExisteEmpleado.Empresa;
                                                NuevoEmpleado.Cargo = ExisteEmpleado.Cargo;
                                                NuevoEmpleado.NombreDocumentoRegistro = ExisteEmpleado.TipoDocumento;

                                                registro.Empleados.Add(NuevoEmpleado);
                                                //registro.Empleados.Add(new ESS_BienMaterialEmpleadoEntidad
                                                //{
                                                //    IdEmpleado = ExisteEmpleado.IdBuk,
                                                //    Nombre = ExisteEmpleado.Nombres,
                                                //    ApellidoPaterno = ExisteEmpleado.ApellidoPaterno,
                                                //    ApellidoMaterno = ExisteEmpleado.ApellidoMaterno,
                                                //    DocumentoRegistro = ExisteEmpleado.NumeroDocumento,
                                                //    IdCargo = ExisteEmpleado.IdCargo,
                                                //    NombreCargo = ExisteEmpleado.Cargo,
                                                //    TipoDocumento = ExisteEmpleado.TipoDocumento,
                                                //    IdEmpresa = ExisteEmpleado.IdEmpresa,
                                                //    Empresa = ExisteEmpleado.Empresa,
                                                //});
                                            }

                                        } else {
                                            listadoEmpleadosExterno = _essEmpleadoBL.ObtenerEmpleadoExternoActivosPorTermino(registro.IdEmpresa, string.Empty);
                                            var ExisteEmpleado = listadoEmpleadosExterno.FirstOrDefault(x => x.DocumentoRegistro.Trim() == NumeroDocumento.Trim());

                                            if(ExisteEmpleado == null) {
                                                observacionFila_Empleados.Add($"No se encontró al Empleado {NumeroDocumento}.");
                                            } else {
                                                registro.Empleados.Add(new ESS_BienMaterialEmpleadoEntidad {
                                                    IdEmpleado = ExisteEmpleado.IdEmpleado,
                                                    Nombre = ExisteEmpleado.Nombre,
                                                    ApellidoPaterno = ExisteEmpleado.ApellidoPaterno,
                                                    ApellidoMaterno = ExisteEmpleado.ApellidoMaterno,
                                                    DocumentoRegistro = ExisteEmpleado.DocumentoRegistro,
                                                    IdCargo = ExisteEmpleado.IdCargo,
                                                    NombreCargo = ExisteEmpleado.NombreCargo,
                                                    TipoDocumento = ExisteEmpleado.TipoDocumento,
                                                    IdEmpresa = ExisteEmpleado.IdEmpresaExterna,
                                                    Empresa = ExisteEmpleado.NombreEmpresaExterna,
                                                    Cargo = ExisteEmpleado.NombreCargo,
                                                    NombreDocumento = ExisteEmpleado.TipoDocumento,
                                                    NombreDocumentoRegistro = ExisteEmpleado.NombreDocumento
                                                });
                                            }
                                        }
                                    }

                                    var ObservacionFinal = new List<string>();

                                    if(registro.Empleados.Count == 0) {
                                        ObservacionFinal.Add("Pero no se registró con ningun Empleado.");
                                    } else {
                                        if(observacionFila_Empleados.Any()) {
                                            ObservacionFinal = observacionFila_Empleados;
                                        }
                                    }

                                    int insertedId = ess_bienmaterialbl.GuardarRegistroBienMaterial(registro);
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
