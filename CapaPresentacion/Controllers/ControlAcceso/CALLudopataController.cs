using CapaEntidad;
using CapaEntidad.AsistenciaCliente;
using CapaEntidad.ControlAcceso;
using CapaEntidad.ControlAcceso.HistorialLudopata;
using CapaEntidad.ControlAcceso.HistorialLudopata.Enum;
using CapaEntidad.ControlAcceso.Ludopata.Dto;
using CapaNegocio;
using CapaNegocio.AsistenciaCliente;
using CapaNegocio.ControlAcceso;
using CapaPresentacion.Models;
using ImageResizer;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.ControlAcceso {
    [seguridad]
    public class CALLudopataController : Controller {
        private readonly CAL_LudopataBL ludopataBL;
        private readonly CAL_ContactoBL contactoBL;
        private readonly UbigeoBL ubigeoBL;
        private readonly AST_TipoDocumentoBL ast_TipoDocumentoBL;
        private readonly AST_ClienteBL ast_ClienteBL;
        private readonly DestinatarioBL destinatariobl;
        private readonly SalaBL salaBL;
        private readonly SEG_UsuarioBL seg_usuarioBL;
        private readonly CAL_HistorialLudopataBL historialLudopataBL;

        public CALLudopataController() {
            ludopataBL = new CAL_LudopataBL();
            contactoBL = new CAL_ContactoBL();
            ubigeoBL = new UbigeoBL();
            ast_TipoDocumentoBL = new AST_TipoDocumentoBL();
            ast_ClienteBL = new AST_ClienteBL();
            destinatariobl = new DestinatarioBL();
            salaBL = new SalaBL();
            seg_usuarioBL = new SEG_UsuarioBL();
            historialLudopataBL = new CAL_HistorialLudopataBL();
        }

        public ActionResult ListadoLudopata() {
            return View("~/Views/ControlAcceso/ListadoLudopata.cshtml");
        }
        public ActionResult ReporteLudopataCliente() {
            return View("~/Views/ControlAcceso/ReporteLudopataCliente.cshtml");
        }

        [seguridad(false)]
        [HttpGet]
        public JsonResult ListarLudopataJson() {
            string displayMessage = string.Empty;
            bool success = false;

            List<CAL_LudopataDto> ludopatas = new List<CAL_LudopataDto>();
            try {
                ludopatas = ludopataBL.GetLudopatasActivos();
                success = ludopatas.Count > 0;
                displayMessage = success ? "Lista de ludópatas." : "No hay ludópatas registrados.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador.";
            }

            JsonResult jsonResult = Json(new { success, records = ludopatas.Count, displayMessage, data = ludopatas }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult ListarLudopataIdJson(int LudopataID) {
            var errormensaje = "";
            CAL_LudopataEntidad item = new CAL_LudopataEntidad();
            UbigeoEntidad ubigeo = new UbigeoEntidad();
            bool respuesta = false;
            try {

                item = ludopataBL.LudopataIdObtenerJson(LudopataID);
                if(item.LudopataID > 0) {
                    ubigeo = ubigeoBL.GetDatosUbigeo(item.CodUbigeo);
                    item.Ubigeo = ubigeo;
                    respuesta = true;
                    errormensaje = "Registro Obtenido";
                } else {
                    errormensaje = "No se pudo obtener el registro";
                }

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = item, mensaje = errormensaje, respuesta }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LudopataEditarJson(CAL_LudopataEntidad ludopata) {
            var errormensaje = "";
            bool respuestaConsulta = false;
            CAL_LudopataEntidad ludopataBusqueda = new CAL_LudopataEntidad();
            try {
                string PathPrincipal = Path.Combine(ConfigurationManager.AppSettings["PathArchivos"].ToString(), "Ludopatas");
                HttpPostedFileBase file = Request.Files["File"];

                ludopataBusqueda = ludopataBL.GetLudopataPorDNI(ludopata.DNI);
                if(ludopataBusqueda.LudopataID != ludopata.LudopataID) {
                    errormensaje = "El numero de DNI ya existe";
                    respuestaConsulta = false;
                    return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
                }

                ludopataBusqueda = ludopataBL.LudopataIdObtenerJson(ludopata.LudopataID);
                if(ludopataBusqueda.LudopataID <= 0) {
                    errormensaje = $"No existe ludópata con ID {ludopata.LudopataID}.";
                    respuestaConsulta = false;
                    return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
                }

                //generar nombre de archivo
                var inicial = string.Empty;
                var objCadena = ludopata.Nombre.Split(" ".ToArray());
                if(objCadena.Length > 0) {
                    inicial = objCadena.Where(objeto => !string.IsNullOrWhiteSpace(objeto))
                        .Aggregate(inicial, (current, objeto) => current + objeto[0].ToString());
                }

                objCadena = ludopata.ApellidoPaterno.Split(" ".ToArray());
                if(objCadena.Length > 0) {
                    inicial = objCadena.Where(objeto => !string.IsNullOrWhiteSpace(objeto))
                        .Aggregate(inicial, (current, objeto) => current + objeto[0].ToString());
                }

                var imageNameFormNew = inicial + "_" + ludopata.LudopataID + "_ludo.jpg";
                var imageNameFormOld = ludopata.Foto;

                if(ludopata.Foto != null && ludopata.Foto != string.Empty) {
                    ludopata.Foto = imageNameFormNew;
                }
                //reemplazando archivos
                if(imageNameFormOld != null) {
                    var combinePathNew = Path.Combine(PathPrincipal, "profile/miniThumb", imageNameFormNew);
                    var combinePathOld = Path.Combine(PathPrincipal, "profile/miniThumb", imageNameFormOld);

                    if(System.IO.File.Exists(combinePathOld)) {
                        System.IO.File.Move(combinePathOld, combinePathNew);
                    }
                    combinePathNew = Path.Combine(PathPrincipal, "profile/thumb", imageNameFormNew);
                    combinePathOld = Path.Combine(PathPrincipal, "profile/thumb", imageNameFormOld);

                    if(System.IO.File.Exists(combinePathOld)) {
                        System.IO.File.Move(combinePathOld, combinePathNew);
                    }

                    combinePathNew = Path.Combine(PathPrincipal, "profile/standard", imageNameFormNew);
                    combinePathOld = Path.Combine(PathPrincipal, "profile/standard", imageNameFormOld);

                    if(System.IO.File.Exists(combinePathOld)) {
                        System.IO.File.Move(combinePathOld, combinePathNew);
                    }
                    combinePathNew = Path.Combine(PathPrincipal, "profile/extend", imageNameFormNew);
                    combinePathOld = Path.Combine(PathPrincipal, "profile/extend", imageNameFormOld);

                    if(System.IO.File.Exists(combinePathOld)) {
                        System.IO.File.Move(combinePathOld, combinePathNew);
                    }
                }
                if(file != null) {
                    if(file.ContentLength > 0) {
                        //tamaños Imagen ( Basado en tamaños de imagen de usuario usados en linkedin  http://www.linkedin.com/) :
                        //                  miniThumb (20px alto * 20px ancho)
                        //                  thumb (65px alto * 65px ancho)
                        //                  standard (200px alto * 200px ancho)
                        //                  extendido (356px alto * 356px ancho)
                        var nombreFoto = ludopata.Iniciales + "_" + ludopata.LudopataID +
                                   "_ludo.jpg";
                        //Manipula y Imagen
                        var combinePath = Path.Combine(PathPrincipal, "profile/thumb", nombreFoto);
                        if(System.IO.File.Exists(combinePath)) {
                            System.IO.File.Delete(combinePath);
                        }

                        //ImageName

                        //Manipulación de Imagen y Guardar

                        //Manipula y Guarda Imagen
                        combinePath = Path.Combine(PathPrincipal, "profile/miniThumb", nombreFoto);
                        if(System.IO.File.Exists(combinePath)) {
                            System.IO.File.Delete(combinePath);
                        }

                        ImageBuilder.Current.Build(new ImageJob(file, combinePath,
                            new Instructions("width=20;height=20;crop=auto;cache=no")));

                        combinePath = Path.Combine(PathPrincipal, "profile/thumb", nombreFoto);
                        if(System.IO.File.Exists(combinePath)) {
                            System.IO.File.Delete(combinePath);
                        }

                        ImageBuilder.Current.Build(new ImageJob(file, combinePath,
                            new Instructions("width=60;height=60;crop=auto;cache=no")));
                        var ruta = ConvertirImagenBase64(combinePath);

                        combinePath = Path.Combine(PathPrincipal, "profile/standard", nombreFoto);
                        if(System.IO.File.Exists(combinePath)) {
                            System.IO.File.Delete(combinePath);
                        }

                        ImageBuilder.Current.Build(new ImageJob(file, combinePath,
                            new Instructions("width=200;height=200;crop=auto;cache=no")));


                        combinePath = Path.Combine(PathPrincipal, "profile/extend", nombreFoto);
                        if(System.IO.File.Exists(combinePath)) {
                            System.IO.File.Delete(combinePath);
                        }

                        ImageBuilder.Current.Build(new ImageJob(file, combinePath,
                            new Instructions("width=356;height=356;crop=auto;cache=no")));

                        ludopata.Imagen = ruta;
                        ludopata.Foto = nombreFoto;
                    }
                }
                respuestaConsulta = ludopataBL.LudopataEditarJson(ludopata);

                if(respuestaConsulta) {
                    bool seCambiaEstado = ludopata.Estado != ludopataBusqueda.Estado;
                    if(seCambiaEstado) {
                        int idUsuario = Convert.ToInt32(Session["UsuarioID"]);
                        CAL_TipoMovimientoHistorialLudopata tipoMovimeinto = ludopataBusqueda.Estado == 1 && ludopata.Estado == 0 ? CAL_TipoMovimientoHistorialLudopata.Sale : CAL_TipoMovimientoHistorialLudopata.Entra;
                        CAL_HistorialLudopata historialLudopata = new CAL_HistorialLudopata() {
                            IdLudopata = ludopata.LudopataID,
                            TipoMovimiento = tipoMovimeinto,
                            TipoRegistro = CAL_TipoRegistroHistorialLudopata.Manual,
                            IdUsuario = idUsuario
                        };
                        historialLudopataBL.InsertarHistorialLudopata(historialLudopata);
                    }

                    errormensaje = "Registro de Ludopata Actualizado Correctamente";
                } else {
                    errormensaje = "Error al Actualizar Ludopata , LLame Administrador";
                    respuestaConsulta = false;
                }

            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult LudopataGuardarJson(CAL_LudopataEntidad ludopata) {
            var errormensaje = "";
            int IdInsertado = 0;
            bool respuesta = false;
            string ruta = "";
            string nombreFoto = "";
            SEG_UsuarioEntidad usuario = new SEG_UsuarioEntidad();
            try {
                string PathPrincipal = Path.Combine(ConfigurationManager.AppSettings["PathArchivos"].ToString(), "Ludopatas");
                HttpPostedFileBase file = Request.Files["File"];
                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                usuario = seg_usuarioBL.UsuarioEmpleadoIDObtenerJson(usuarioId);
                List<SalaEntidad> listaSalasUsuario = salaBL.ListadoSalaPorUsuario(usuarioId);
                if(listaSalasUsuario.Count < 0) {
                    respuesta = false;
                    errormensaje = "Este usuario debe tener seleccionado salas";
                    return Json(new { respuesta, mensaje = errormensaje });
                }
                string stringSalasUsuario = String.Join(",", listaSalasUsuario.Select(x => x.Nombre));
                if(file != null) {
                    if(file.ContentLength > 0) {
                        if(!Directory.Exists(PathPrincipal)) {
                            respuesta = false;
                            errormensaje = "No se encontro el directorio.";
                            return Json(new { respuesta, mensaje = errormensaje });

                        }
                    }
                }
                ludopata.UsuarioRegistro = usuarioId;
                ludopata.SalasUsuario = stringSalasUsuario;
                ludopata.NombreUsuarioRegistro = usuario.UsuarioNombre;
                //validar dni repetido
                var ludopataBusqueda = ludopataBL.GetLudopataPorDNI(ludopata.DNI);
                if(ludopataBusqueda.LudopataID != ludopata.LudopataID) {
                    errormensaje = "El numero de DNI ya existe";
                    return Json(new { respuesta = false, mensaje = errormensaje });
                }

                IdInsertado = ludopataBL.LudopataInsertarJson(ludopata);

                if(IdInsertado > 0) {
                    CAL_HistorialLudopata historialLudopata = new CAL_HistorialLudopata() {
                        IdLudopata = IdInsertado,
                        TipoMovimiento = CAL_TipoMovimientoHistorialLudopata.Entra,
                        TipoRegistro = CAL_TipoRegistroHistorialLudopata.Manual,
                        IdUsuario = usuarioId
                    };
                    historialLudopataBL.InsertarHistorialLudopata(historialLudopata);

                    ludopata.LudopataID = IdInsertado;
                    if(file != null) {
                        if(file.ContentLength > 0) {
                            //tamaños Imagen ( Basado en tamaños de imagen de usuario usados en linkedin  http://www.linkedin.com/) :
                            //                  miniThumb (20px alto * 20px ancho)
                            //                  thumb (65px alto * 65px ancho)
                            //                  standard (200px alto * 200px ancho)
                            //                  extendido (356px alto * 356px ancho)

                            nombreFoto = ludopata.Iniciales + "_" +
                            ludopata.LudopataID + "_ludo.jpg";
                            //Manipula y Guarda Imagen
                            var combinePath = Path.Combine(PathPrincipal, "profile/miniThumb", nombreFoto);
                            ImageBuilder.Current.Build(new ImageJob(file, combinePath,
                                new Instructions("width=20;height=20;crop=auto;cache=no")));

                            combinePath = Path.Combine(PathPrincipal, "profile/thumb", nombreFoto);
                            ImageBuilder.Current.Build(new ImageJob(file, combinePath,
                                new Instructions("width=60;height=60crop=auto;cache=no")));

                            ruta = ConvertirImagenBase64(combinePath);

                            combinePath = Path.Combine(PathPrincipal, "profile/standard", nombreFoto);
                            ImageBuilder.Current.Build(new ImageJob(file, combinePath,
                                new Instructions("width=200;height=200;crop=auto;cache=no")));

                            combinePath = Path.Combine(PathPrincipal, "profile/extend", nombreFoto);
                            ImageBuilder.Current.Build(new ImageJob(file, combinePath,
                                new Instructions("width=356;height=356;crop=auto;cache=no")));
                        }

                    }
                    CAL_ContactoEntidad contacto = new CAL_ContactoEntidad {
                        NombreContacto = ludopata.NombreContacto,
                        ApellidoPaternoContacto = ludopata.ApellidoPaternoContacto,
                        ApellidoMaternoContacto = ludopata.ApellidoMaternoContacto,
                        TelefonoContacto = ludopata.Telefono,
                        CelularContacto = ludopata.CelularContacto
                    };

                    int ContactoID = new CAL_ContactoBL().InsertarContacto(contacto);
                    ludopata.ContactoID = ContactoID;

                    ludopata.Foto = nombreFoto;
                    ludopata.Imagen = ruta;
                    ludopataBL.LudopataEditarJson(ludopata);
                    AST_ClienteEntidad ClienteExistente = BuscarLudopataEnClientes(ludopata.DNI);
                    if(ClienteExistente.Id > 0) {
                        //EnviarCorreoLudopataExistente(ludopata,ClienteExistente);
                        Task.Run(() => {
                            Task oResp = EnviarCorreoAsync(ludopata, ClienteExistente);
                        });
                    }

                    respuesta = true;
                    errormensaje = "Registro Ludopata Guardado Correctamente";
                } else {
                    errormensaje = "Error al crear la Ludopata , LLame Administrador";
                    respuesta = false;
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult LudopataEliminarJson(int id, int ContactoID) {
            var errormensaje = "";
            bool respuesta = false;
            try {
                CAL_LudopataEntidad ludopataVerificacion = ludopataBL.LudopataIdObtenerJson(id);
                if(ludopataVerificacion.Estado == 0) {
                    respuesta = false;
                    errormensaje = "El registro de ludopata ya se encuentra inactivo.";
                    return Json(new { respuesta, mensaje = errormensaje });
                }

                int idUsuario = Convert.ToInt32(Session["UsuarioID"]);
                respuesta = ludopataBL.ModificarEstadoLudopata(id, false);
                if(respuesta) {
                    CAL_HistorialLudopata historialLudopata = new CAL_HistorialLudopata() {
                        IdLudopata = id,
                        TipoMovimiento = CAL_TipoMovimientoHistorialLudopata.Sale,
                        TipoRegistro = CAL_TipoRegistroHistorialLudopata.Manual,
                        IdUsuario = idUsuario
                    };
                    historialLudopataBL.InsertarHistorialLudopata(historialLudopata);
                    errormensaje = "Se quitó el ludópata correctamente";
                } else {
                    errormensaje = "Error al quitar el ludópata, LLame Administrador";
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult LudopataDescargarExcelJson() {
            string fecha = DateTime.Now.ToString("dd_MM_yyyy");
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<CAL_LudopataEntidad> lista = new List<CAL_LudopataEntidad>();
            var strElementos = String.Empty;
            var strElementos_ = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;
            try {


                lista = ludopataBL.LudopataListadoCompletoJson();
                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Ludopata");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Nombre Completo";
                    workSheet.Cells[3, 4].Value = "Tipo documento";
                    workSheet.Cells[3, 5].Value = "DOI";
                    workSheet.Cells[3, 6].Value = "Telefono";
                    workSheet.Cells[3, 7].Value = "Tipo Exclusion";
                    workSheet.Cells[3, 8].Value = "Codigo Registro";
                    workSheet.Cells[3, 9].Value = "Estado";
                    workSheet.Cells[3, 10].Value = "Fecha Inscripcion";

                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach(var registro in lista) {

                        registro.NombreCompleto = registro.Nombre + " " + registro.ApellidoPaterno + " " + registro.ApellidoMaterno;
                        workSheet.Cells[recordIndex, 2].Value = registro.LudopataID;
                        workSheet.Cells[recordIndex, 3].Value = registro.NombreCompleto.ToUpper();
                        workSheet.Cells[recordIndex, 4].Value = registro.DOINombre.ToUpper();
                        workSheet.Cells[recordIndex, 5].Value = registro.DNI.ToUpper();
                        workSheet.Cells[recordIndex, 6].Value = registro.Telefono.ToUpper().Trim() == "" ? "--" : registro.Telefono.Trim().ToUpper();
                        workSheet.Cells[recordIndex, 7].Value = registro.TipoExclusion == 1 ? "PERSONAL" : "FAMILIAR";
                        workSheet.Cells[recordIndex, 8].Value = registro.CodRegistro.Trim() == "" ? "--" : registro.CodRegistro.Trim().ToUpper();
                        workSheet.Cells[recordIndex, 9].Value = registro.Estado == 1 ? "ACTIVO" : "INACTIVO";
                        workSheet.Cells[recordIndex, 10].Value = registro.FechaInscripcion.ToString("dd-MM-yyyy hh:mm:ss tt");

                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:J3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:J3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:J3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:J3"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B3:J3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:J3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:J3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:J3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B3:J3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:J3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:J3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:J3"].Style.Border.Bottom.Color.SetColor(colborder);

                    int filasagregadas = 3;
                    total = filasagregadas + total;

                    workSheet.Cells["B4:J" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    /*
                    workSheet.Cells["B2:E2"].Merge = true;
                    workSheet.Cells["B2:E2"].Style.Font.Bold = true;
                    */

                    int filaFooter = total + 1;
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Merge = true;
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells["B" + filaFooter + ":J" + filaFooter].Style.Font.Size = 14;
                    workSheet.Cells[filaFooter, 2].Value = "Total : " + (total - filasagregadas) + " Registros";
                    workSheet.Cells[filaFooter, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["B4:J" + total].Style.WrapText = true;

                    int filaultima = total;
                    workSheet.Cells[3, 2, filaultima, 10].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 40;
                    workSheet.Column(4).Width = 30;
                    workSheet.Column(5).Width = 40;
                    workSheet.Column(6).Width = 30;
                    workSheet.Column(7).Width = 30;
                    workSheet.Column(8).Width = 40;
                    workSheet.Column(9).Width = 18;
                    workSheet.Column(10).Width = 30;
                    excelName = "Ludopata_" + fecha + ".xlsx";
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());

                    mensaje = "Descargando Archivo";
                    respuesta = true;
                } else {
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
        public JsonResult ListarLudopataServerJson(DtParameters dtParameters) {
            var errormensaje = "";
            var lista = new List<CAL_LudopataEntidad>();
            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = "";
            List<dynamic> registro = new List<dynamic>();
            int pageSize, skip;
            int recordsTotal = 0;
            int recordsFiltered = 0;
            string PathArchivos = ConfigurationManager.AppSettings["PathArchivos"].ToString() + "/Ludopatas/";
            try {

                if(dtParameters.Order != null) {
                    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower();
                } else {
                    orderCriteria = "LudopataID";
                    orderAscendingDirection = "asc";
                }
                string whereQuery = "";
                if(!string.IsNullOrEmpty(searchBy) && !(string.IsNullOrWhiteSpace(searchBy))) {
                    //Campos de la Tabla CMP_Sesion_Cupones_Cliente
                    string[] values = { "lud.Nombre", "lud.ApellidoPaterno", "lud.ApellidoMaterno", "doi.DESCRIPCION", "lud.DNI", "lud.CodRegistro", "lud.FechaInscripcion", "CONCAT(lud.Nombre, ' ', lud.ApellidoPaterno, ' ', lud.ApellidoMaterno)" };
                    List<string> listaWhere = new List<string>();
                    foreach(var value in values) {
                        listaWhere.Add($@" {value} like '%{searchBy}%' ");
                    }
                    whereQuery += $@" where ( {String.Join(" or ", listaWhere)} )";
                }
                pageSize = dtParameters.Length;
                skip = dtParameters.Start;

                recordsFiltered = ludopataBL.ObtenerTotalRegistrosFiltrados(whereQuery);
                whereQuery += $@" order by {orderCriteria} {orderAscendingDirection} offset {skip} rows fetch next {pageSize} rows only;";
                lista = ludopataBL.GetAllLudopataFiltrados(whereQuery);
                recordsTotal = ludopataBL.ObtenerTotalRegistros();

                string PathImagenesLudopatas = Path.Combine(ConfigurationManager.AppSettings["UriImagenesLudopatas"].ToString());

                foreach(var item in lista) {


                    bool fotoVerificada = VerificarArchivo(Path.Combine(PathArchivos, "profile/thumb/", $"{item.Foto}"));
                    if(fotoVerificada) {
                        item.Foto = PathImagenesLudopatas + "profile/thumb/" + Convert.ToString(item.Foto);
                    } else {
                        item.Foto = PathImagenesLudopatas + "profile/thumb/default_image_profile.jpg";
                    }

                    //item.Imagen = "";
                    item.NombreCompleto = item.Nombre + " " + item.ApellidoPaterno + " " + item.ApellidoMaterno;
                }

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { draw = dtParameters.Draw, recordsFiltered = recordsFiltered, recordsTotal = recordsTotal, data = lista, mensaje = "Listando Registros", });
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ObtenerDataSelects(UbigeoEntidad ubigeo) {
            List<UbigeoEntidad> listaUbigeo = new List<UbigeoEntidad>();
            List<AST_TipoDocumentoEntidad> listaTipoDocumento = new List<AST_TipoDocumentoEntidad>();
            List<UbigeoEntidad> listaProvincias = new List<UbigeoEntidad>();
            List<UbigeoEntidad> listaDistritos = new List<UbigeoEntidad>();
            string mensaje = "";
            bool respuesta = false;
            object oRespuesta = new object();
            try {
                listaUbigeo = ubigeoBL.ListadoDepartamento();
                listaTipoDocumento = ast_TipoDocumentoBL.GetListadoTipoDocumento();
                if(ubigeo.CodUbigeo != 0) {
                    listaProvincias = ubigeoBL.GetListadoProvincia(ubigeo.DepartamentoId);
                    listaDistritos = ubigeoBL.GetListadoDistrito(ubigeo.ProvinciaId, ubigeo.DepartamentoId);
                    oRespuesta = new {
                        dataUbigeo = listaUbigeo,
                        dataTipoDocumento = listaTipoDocumento,
                        dataProvincias = listaProvincias,
                        dataDistritos = listaDistritos,
                    };
                } else {
                    oRespuesta = new {
                        dataUbigeo = listaUbigeo,
                        dataTipoDocumento = listaTipoDocumento,
                    };
                }
                respuesta = true;
                mensaje = "Listando registros";
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data = oRespuesta });
        }
        private string ConvertirImagenBase64(string rutaImagen = "") {
            string base64String = string.Empty;
            try {
                if(!rutaImagen.Equals(string.Empty)) {
                    if(System.IO.File.Exists(rutaImagen)) {
                        byte[] imagebytes = System.IO.File.ReadAllBytes(rutaImagen);
                        base64String = Convert.ToBase64String(imagebytes);
                    } else {
                        base64String = string.Empty;
                    }
                } else {
                    base64String = string.Empty;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                base64String = string.Empty;
            }
            return base64String;
        }
        private bool VerificarArchivo(string path) {
            bool respuesta = false;
            try {
                if(System.IO.File.Exists(path)) {
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        private AST_ClienteEntidad BuscarLudopataEnClientes(string DNI) {
            AST_ClienteEntidad clienteConsulta = new AST_ClienteEntidad();
            try {
                clienteConsulta = ast_ClienteBL.GetClientexNroDoc(DNI);
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                clienteConsulta = new AST_ClienteEntidad();
            }
            return clienteConsulta;
        }
        private async Task<bool> EnviarCorreoAsync(CAL_LudopataEntidad ludopata, AST_ClienteEntidad clienteConsulta) {
            bool respuesta = false;
            SmtpClient cliente;
            MailMessage email;
            List<DestinatarioEntidad> listaDestinatarios = new List<DestinatarioEntidad>();
            try {
                string _HOST = ConfigurationManager.AppSettings["host"];
                string _PORT = ConfigurationManager.AppSettings["port"];
                string _ENABLESSL = ConfigurationManager.AppSettings["enableSSL"];
                string _USER = ConfigurationManager.AppSettings["correo"];
                string _PASWWORD = ConfigurationManager.AppSettings["password"];
                listaDestinatarios = destinatariobl.DestinatarioListadoTipoEmailJson(3).Where(x => x.estado == 1).ToList();
                string destinatarios = String.Join(",", listaDestinatarios.Select(x => x.Email));
                cliente = new SmtpClient(_HOST, Convert.ToInt32(_PORT)) {
                    EnableSsl = Boolean.Parse(_ENABLESSL),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_USER, _PASWWORD)
                };
                ludopata.Nombre = (ludopata.Nombre == null) ? "" : ludopata.Nombre;
                ludopata.ApellidoPaterno = (ludopata.ApellidoPaterno == null) ? "" : ludopata.ApellidoPaterno;
                ludopata.ApellidoMaterno = (ludopata.ApellidoMaterno == null) ? "" : ludopata.ApellidoMaterno;
                String htmlEnvio = $@"
                        <div style='background: rgb(250,251,63);
                                    background: radial-gradient(circle, rgba(250,251,63,1) 0%, rgba(255,162,0,1) 100%);width: 100%;'>
                            <table style='max-width: 500px; display: table;margin:0 auto; padding: 25px;'>
                                <tbody>
                                <tr>
                                    <td colspan='2'>
                                        <div style='text-align: center;font-family: Helvetica, Arial, sans-serif; font-size: 18px; color: #000000;'>
                                            <h3>MÓDULO CONTROL DE ACCESO</h3>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan='2'>
                                            <div style='font-family: Helvetica, Arial, sans-serif;color: #000000;'>
                                                <p style=' margin-top: 0px;margin-bottom: 0px;'>Buenos dias, se ha registrado un ludopata nuevo, con registro existente en clientes</p>
                                                <p style=' margin-top: 10px;margin-bottom: 0px;'><strong>Nombre Ludopata:</strong> {ludopata.Nombre.ToUpper()} {ludopata.ApellidoPaterno.ToUpper()} {ludopata.ApellidoMaterno.ToUpper()}</p>
                                                <p style=' margin-top: 10px;margin-bottom: 0px;'><strong>DNI:</strong> {ludopata.DNI}</p>
                                                <p style=' margin-top: 10px;margin-bottom: 0px;'><strong>Usuario que Registró:</strong> {ludopata.NombreUsuarioRegistro}</p>
                                                <p style=' margin-top: 10px;margin-bottom: 0px;'><strong>Salas del Usuario:</strong> {ludopata.SalasUsuario}</p>
                                                <p style=' margin-top: 10px;margin-bottom: 0px;'>Gracias. </p>
                                            </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan='2'>
                                        <div style='text-align: center;font-family: Helvetica, Arial, sans-serif; font-size: 15px; color: #000000;'>
                                            <p style='font-size: 13px;'><strong>GLADCON GROUP</strong></p>
                                        </div>
                                    </td>
                                </tr>
                                </tbody>
                            </table>
                        </div>
                    "
               ;

                email = new MailMessage(_USER, destinatarios, "Modulo Control Accesos - IAS. Ludopata registrado", htmlEnvio) {
                    IsBodyHtml = true,
                    BodyEncoding = System.Text.Encoding.UTF8,
                    SubjectEncoding = System.Text.Encoding.Default
                };
                ludopata.FechaEnvioCorreo = DateTime.Now;
                await cliente.SendMailAsync(email);
                ludopataBL.EditarFechaEnvioCorreo(ludopata);
                respuesta = true;
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ReporteLudopatasCliente() {
            string mensaje = string.Empty;
            bool respuesta = false;
            List<CAL_LudopataEntidad> lista = new List<CAL_LudopataEntidad>();
            try {
                lista = ludopataBL.ReporteLudopatasClientes();
                mensaje = "Listando Registros";
                respuesta = true;
            } catch(Exception ex) {
                mensaje = ex.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje, respuesta }, JsonRequestBehavior.AllowGet);
        }
    }
}