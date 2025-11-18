using CapaEntidad.ControlAcceso;
using CapaEntidad;
using CapaNegocio.ControlAcceso;
using ImageResizer;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.ControlAcceso
{
    [seguridad]
    public class CALRobaStackersBilleteroController : Controller
    {

        private CAL_RobaStackersBilleteroBL robaStackersBilleteroBL = new CAL_RobaStackersBilleteroBL();

        public ActionResult ListadoRobaStackersBilletero() {
            return View("~/Views/ControlAcceso/ListadoRobaStackersBilletero.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarRobaStackersBilleteroJson() {
            bool respuesta = false;
            var errormensaje = "";
            var lista = new List<CAL_RobaStackersBilleteroEntidad>();
            string PathArchivos = ConfigurationManager.AppSettings["PathArchivos"].ToString() + "/Ludopatas/";

            try {


                lista = robaStackersBilleteroBL.RobaStackersBilleteroListadoCompletoJson();
                string PathImagenesLudopatas = Path.Combine(ConfigurationManager.AppSettings["UriImagenesLudopatas"].ToString());


                foreach(var item in lista) {


                    bool fotoVerificada = VerificarArchivo(Path.Combine(PathArchivos, "profile/thumb/", $"{item.Foto}"));
                    if(fotoVerificada) {
                        item.Foto = PathImagenesLudopatas + "profile/thumb/" + Convert.ToString(item.Foto);
                    } else {
                        item.Foto = PathImagenesLudopatas + "profile/thumb/default_image_profile.jpg";
                    }

                    item.NombreCompleto = item.Nombre + " " + item.ApellidoPaterno + " " + item.ApellidoMaterno;
                    item.EmpleadoNombreCompleto = item.EmpleadoNombres + " " + item.EmpleadoApellidoPaterno;
                }
                respuesta = true;
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
                respuesta = false;
            }
            return Json(new { data = lista.ToList(), respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarRobaStackersBilleteroIdJson(int RobaStackersBilleteroID) {
            bool respuesta = false;
            var errormensaje = "";
            CAL_RobaStackersBilleteroEntidad item = new CAL_RobaStackersBilleteroEntidad();

            try {

                item = robaStackersBilleteroBL.RobaStackersBilleteroIdObtenerJson(RobaStackersBilleteroID);
                respuesta = true;

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
                respuesta = false;
            }
            return Json(new { data = item, respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RobaStackersBilleteroEditarJson(CAL_RobaStackersBilleteroEntidad robaStackersBilletero) {
            var errormensaje = "";
            bool respuestaConsulta = false;
            var empleado = new SEG_EmpleadoEntidad();
            empleado = (SEG_EmpleadoEntidad)Session["empleado"];
            robaStackersBilletero.EmpleadoID = empleado.EmpleadoID;
            CAL_RobaStackersBilleteroEntidad robaStackersBilleteroBusqueda = new CAL_RobaStackersBilleteroEntidad();
            try {
                string PathPrincipal = Path.Combine(ConfigurationManager.AppSettings["PathArchivos"].ToString(), "Ludopatas");
                HttpPostedFileBase file = Request.Files["File"];

                robaStackersBilleteroBusqueda = robaStackersBilleteroBL.GetRobaStackersBilleteroPorDNI(robaStackersBilletero.DNI);
                if(robaStackersBilleteroBusqueda.RobaStackersBilleteroID != robaStackersBilletero.RobaStackersBilleteroID) {
                    errormensaje = "El numero de DNI ya existe";
                    respuestaConsulta = false;
                    return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
                }

                //generar nombre de archivo
                var inicial = string.Empty;
                var objCadena = robaStackersBilletero.Nombre.Split(" ".ToArray());
                if(objCadena.Length > 0) {
                    inicial = objCadena.Where(objeto => !string.IsNullOrWhiteSpace(objeto))
                        .Aggregate(inicial, (current, objeto) => current + objeto[0].ToString());
                }

                objCadena = robaStackersBilletero.ApellidoPaterno.Split(" ".ToArray());
                if(objCadena.Length > 0) {
                    inicial = objCadena.Where(objeto => !string.IsNullOrWhiteSpace(objeto))
                        .Aggregate(inicial, (current, objeto) => current + objeto[0].ToString());
                }

                var imageNameFormNew = inicial + "_" + robaStackersBilletero.RobaStackersBilleteroID + "_rsb.jpg";
                var imageNameFormOld = robaStackersBilletero.Foto;

                if(robaStackersBilletero.Foto != null && robaStackersBilletero.Foto != string.Empty) {
                    robaStackersBilletero.Foto = imageNameFormNew;
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
                        var nombreFoto = robaStackersBilletero.Iniciales + "_" + robaStackersBilletero.RobaStackersBilleteroID +
                                   "_rsb.jpg";
                        //Manipula y Guarda Imagen
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

                        robaStackersBilletero.Imagen = ruta;
                        robaStackersBilletero.Foto = nombreFoto;
                    }
                }
                respuestaConsulta = robaStackersBilleteroBL.RobaStackersBilleteroEditarJson(robaStackersBilletero);

                if(respuestaConsulta) {

                    errormensaje = "Registro de Roba Stackers Billetero Actualizado Correctamente";
                } else {
                    errormensaje = "Error al Actualizar Roba Stackers Billetero , LLame Administrador";
                    respuestaConsulta = false;
                }

            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult RobaStackersBilleteroGuardarJson(CAL_RobaStackersBilleteroEntidad robaStackersBilletero) {
            var errormensaje = "";
            int IdInsertado = 0;
            bool respuesta = false;
            string ruta = "";
            string nombreFoto = "";
            var empleado = new SEG_EmpleadoEntidad();
            empleado = (SEG_EmpleadoEntidad)Session["empleado"];
            robaStackersBilletero.EmpleadoID = empleado.EmpleadoID;
            try {
                string PathPrincipal = Path.Combine(ConfigurationManager.AppSettings["PathArchivos"].ToString(), "Ludopatas");
                HttpPostedFileBase file = Request.Files["File"];
                robaStackersBilletero.FechaRegistro = DateTime.Now;
                robaStackersBilletero.FechaInscripcion = DateTime.Now;


                if(file != null) {
                    if(file.ContentLength > 0) {
                        if(!Directory.Exists(PathPrincipal)) {
                            respuesta = false;
                            errormensaje = "No se encontro el directorio.";
                            return Json(new { respuesta, mensaje = errormensaje });

                        }
                    }
                }

                var robaStackersBilleteroBusqueda = robaStackersBilleteroBL.GetRobaStackersBilleteroPorDNI(robaStackersBilletero.DNI);
                if(robaStackersBilleteroBusqueda.RobaStackersBilleteroID != robaStackersBilletero.RobaStackersBilleteroID) {
                    errormensaje = "El numero de DNI ya existe";
                    return Json(new { respuesta = false, mensaje = errormensaje });
                }

                IdInsertado = robaStackersBilleteroBL.RobaStackersBilleteroInsertarJson(robaStackersBilletero);
                if(IdInsertado > 0) {
                    robaStackersBilletero.RobaStackersBilleteroID = IdInsertado;
                    if(file != null) {
                        if(file.ContentLength > 0) {
                            //if (!Directory.Exists(PathPrincipal))
                            //{

                            //}
                            //tamaños Imagen ( Basado en tamaños de imagen de usuario usados en linkedin  http://www.linkedin.com/) :
                            //                  miniThumb (20px alto * 20px ancho)
                            //                  thumb (65px alto * 65px ancho)
                            //                  standard (200px alto * 200px ancho)
                            //                  extendido (356px alto * 356px ancho)

                            nombreFoto = robaStackersBilletero.Iniciales + "_" +
                            robaStackersBilletero.RobaStackersBilleteroID + "_rsb.jpg";
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

                    robaStackersBilletero.Foto = nombreFoto;
                    robaStackersBilletero.Imagen = ruta;
                    robaStackersBilleteroBL.RobaStackersBilleteroEditarJson(robaStackersBilletero);

                    respuesta = true;
                    errormensaje = "Registro Roba Stackers Billetero Guardado Correctamente";
                } else {
                    errormensaje = "Error al crear el PRoba Stackers Billetero , LLame Administrador";
                    respuesta = false;
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult RobaStackersBilleteroEliminarJson(int id) {
            var errormensaje = "";
            bool respuesta = false;
            try {
                respuesta = robaStackersBilleteroBL.RobaStackersBilleteroEliminarJson(id);
                if(respuesta) {
                    respuesta = true;
                    errormensaje = "Se quitó el Roba Stackers Billetero Correctamente";
                } else {
                    errormensaje = "error al Quitar el Roba Stackers Billetero , LLame Administrador";
                    respuesta = false;
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult RobaStackersBilleteroDescargarExcelJson() {
            string fecha = DateTime.Now.ToString("dd_MM_yyyy");
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<CAL_RobaStackersBilleteroEntidad> lista = new List<CAL_RobaStackersBilleteroEntidad>();
            var strElementos = String.Empty;
            var strElementos_ = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;
            try {


                lista = robaStackersBilleteroBL.RobaStackersBilleteroListadoCompletoJson();
                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Roba Stackers Billetero");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Nombre Completo";
                    workSheet.Cells[3, 4].Value = "DNI";
                    workSheet.Cells[3, 5].Value = "Usuario";
                    workSheet.Cells[3, 6].Value = "Sala";
                    workSheet.Cells[3, 7].Value = "Observacion";
                    workSheet.Cells[3, 8].Value = "Estado";
                    workSheet.Cells[3, 9].Value = "Fecha Registro";

                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach(var registro in lista) {

                        registro.NombreCompleto = registro.Nombre + " " + registro.ApellidoPaterno + " " + registro.ApellidoMaterno;
                        registro.EmpleadoNombreCompleto = registro.EmpleadoNombres + " " + registro.EmpleadoApellidoPaterno;
                        workSheet.Cells[recordIndex, 2].Value = registro.RobaStackersBilleteroID;
                        workSheet.Cells[recordIndex, 3].Value = registro.NombreCompleto.ToUpper();
                        workSheet.Cells[recordIndex, 4].Value = registro.DNI.ToUpper();
                        workSheet.Cells[recordIndex, 5].Value = registro.EmpleadoNombreCompleto.ToUpper();
                        workSheet.Cells[recordIndex, 6].Value = registro.SalaNombre.ToUpper();
                        workSheet.Cells[recordIndex, 7].Value = registro.Observacion.Trim() == "" ? "--" : registro.Observacion.Trim().ToUpper();
                        workSheet.Cells[recordIndex, 8].Value = registro.Estado == 1 ? "ACTIVO" : "INACTIVO";
                        workSheet.Cells[recordIndex, 9].Value = registro.FechaRegistro.ToString("dd-MM-yyyy hh:mm:ss tt");

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

                    workSheet.Cells["B4:I" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    /*
                    workSheet.Cells["B2:E2"].Merge = true;
                    workSheet.Cells["B2:E2"].Style.Font.Bold = true;
                    */

                    int filaFooter = total + 1;
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Merge = true;
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells["B" + filaFooter + ":I" + filaFooter].Style.Font.Size = 14;
                    workSheet.Cells[filaFooter, 2].Value = "Total : " + (total - filasagregadas) + " Registros";
                    workSheet.Cells[filaFooter, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["B4:I" + total].Style.WrapText = true;

                    int filaultima = total;
                    workSheet.Cells[3, 2, filaultima, 9].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 40;
                    workSheet.Column(4).Width = 30;
                    workSheet.Column(5).Width = 40;
                    workSheet.Column(6).Width = 30;
                    workSheet.Column(7).Width = 40;
                    workSheet.Column(8).Width = 18;
                    workSheet.Column(9).Width = 30;
                    excelName = "RobaStackersBilletero_" + fecha + ".xlsx";
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
    }
}