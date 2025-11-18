using CapaEntidad;
using CapaEntidad.ControlAcceso;
using CapaNegocio;
using CapaNegocio.ControlAcceso;
using ImageResizer;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.ControlAcceso {
    [seguridad]
    public class CALPersonaProhibidoIngresoController : Controller {
        private readonly CAL_PersonaProhibidoIngresoBL timadorBL;
        private readonly CAL_PersonaProhibidoIngresoIncidenciaIncidenciaBL timadorIncidenciaBL;
        private readonly SalaBL salaBl;

        public CALPersonaProhibidoIngresoController() {
            timadorBL = new CAL_PersonaProhibidoIngresoBL();
            timadorIncidenciaBL = new CAL_PersonaProhibidoIngresoIncidenciaIncidenciaBL();
            salaBl = new SalaBL();
        }

        public ActionResult ListadoPersonaProhibidoIngreso() {
            return View("~/Views/ControlAcceso/ListadoPersonaProhibidoIngreso.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarPersonaProhibidoIngresoJson() {
            bool respuesta = false;
            string errormensaje = "";
            List<CAL_PersonaProhibidoIngresoEntidad> lista = new List<CAL_PersonaProhibidoIngresoEntidad>();
            string PathArchivos = ConfigurationManager.AppSettings["PathArchivos"].ToString() + "/Ludopatas/";

            try {


                lista = timadorBL.TimadorListadoCompletoJson();
                string PathImagenesLudopatas = Path.Combine(ConfigurationManager.AppSettings["UriImagenesLudopatas"].ToString());


                foreach(CAL_PersonaProhibidoIngresoEntidad item in lista) {
                    List<int> codsSalas = salaBl.ObtenerCodsSalasDeSesion(Session);
                    List<CAL_PersonaProhibidoIngresoIncidenciaEntidad> incidencias = timadorIncidenciaBL.GetAllTimadorIncidenciaxTimadorActivo(item.TimadorID, codsSalas);

                    item.SalaNombreCompuesto = string.Join(" - ", incidencias.Select(x => x.SalaNombre.ToString()).ToList());


                    bool fotoVerificada = VerificarArchivo(Path.Combine(PathArchivos, "profile/thumb/", $"{item.Foto}"));
                    if(fotoVerificada) {
                        item.Foto = PathImagenesLudopatas + "profile/thumb/" + Convert.ToString(item.Foto);
                    } else {
                        item.Foto = PathImagenesLudopatas + "profile/thumb/default_image_profile.jpg";
                    }
                    /*
                    if (item.Imagen == "")
                    {
                        item.Imagen = "iVBORw0KGgoAAAANSUhEUgAAAEEAAABBCAYAAACO98lFAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAcBSURBVHhe7ZvZT1VXFMbvf2CsPjAPDiBEBYxyLzFGEqM28Ult9AFpoCFRqk1K4hDhKoKKgPOMdUyFJgVaFbUJtEWlFSsqqMURBwRlUJDBoX1b3d8ix1izTbn77HO4VR4+48S+a//OGvc+1zFmzBiyWqGhoeTn50dOp5OysrKptraOurp76PVff1N6RgYNHz6c/z0oKEj681bLUgjYvK+vL7lcLtqQm0f1N27y5tvan9KjpmZ6/KSFrl67Tt8XF9OWLVtp7tzPKDAwkEaPHi1dzypZAmHUqFG8eafTRfn5G+nmrdvU3dNLrW3tvHlDjY+a6ElLK3U+76Ke3hfUKuBkrsliEFgjIiJCur5uaYUQHh5O/v7+FBkZSatXZ9KNm7f6NvfO5t8nAIGn7Ny5i71oxIgRtoDQAgGGjhw5kgEkJCTQ7+erpU++P2p+/IS6BbiDhw71gRCyGoRpCDAwODiYf79DPMH2p8/oWUendIP9VVPzY4ZYVPQdhYWFUWhIiKUgTEGAYcjokyZNovKKCnZ9JDvZxjwVQPS+eEnFJaUcZvAKq0CYgoCYjY6OpnNVVQxAthmzwrrFJSV9HmERCGUIeDrwgv0HDtKLl6+oUbIBXQKIb48Wct6BZPaYkTIEPJX4+Hi6/+AhZ3WZ8bqE0OgRoVGw7xvOP/AKmU2qUoYQEBBAKSkp9PRZBxspM16nUDW6unspI8MtPDBQapOqlCGgHK7OzKTnXd1So60Qqk519QXOCzq7SmUI6PU3btrEzY3MYCuEdrvm0mWaMGECd5Qyu1RkKhx27dptqyeg+cKs4XLFaU2QyhDQ3+/ZW2A7hNq6qxQb6xx4CCiPIaKLKyws4uFHZrAVQjd6XuSE8ePHD3xOAASUqsNHjtgPQcwl48aN847EiJyw0+acgHJ89lwVT6lhYV4CYffuPbZD+OXXSvZEnQ2TMgT0CZs3bxEQ7CuR6BNOnf6Jy6NXQECfsD4nx9Y+oaPzOZWU/sCDW7g3QPD18aGVK9NtDQd81v79B/joTmaTqpQgwBXRJ6zJyhJxau4AxRNhULsmmqX58+a/OcjRISUIMCAnZ4Mwqs2W4ckQDmZfvnpNW7du43CU2aYijyHAC9CtlZdX2JoPDOEzs9eu9Q4IZWUnOVHJDLVSgOB2uz9eCAg9fObXaWlcomX2qUgZwsmTpwYEAsbp5OQvRGIOkNqnIiUIqNPHT5TZDgGnSzjNnj17ttZ7S48hQGiZ0S3anRgB4MHDRpo+fTpPsTLbVKQEIVBASEz8nKc6O0skQgHnCV5xsgQDoqKi+Iod/Tzqt8xo3cLR/t69BRQkGjWZXapSggChY1y8eDFPdpCVILB2R2cXVZ45S9HRMZyTZDapShkChAw9T7SwF2tqlC5f+yuEwcWaS+SMjeWEqPsWyhQEaOjQobR8xQpLkyQGp0OHD2sfnAyZhoCmZcmSJZZOk1i7oGAfVyWZDWZlGgIMS1iwwNJKAQgbcnO9FwLqNep2Y1OTtmv5dwUIS5cu09oqvy3TENBB4uS3vOJn6hTG6q4SxrywIDGRK5LMBrMyDQGCcXPmzGFP0H1DjfJ76vRpioiMtOytNi0QIGRut3sV30Poyg2GFyxcuEis7yP9XB3SBgFPCTX86NFC0UXqG6wANTU1lfwsKo+QNgiQj48PLVu+XBiur1wiKaanZ9CwT4aJ6uCvdWYwpA0CXrUbMmQIuVet0tozYHy+fecuv66TlJRMY8eO5VMlnHPiEkZmi6cyDQGv7Riv7mZlZ/PVuc7kiGqDhAuwSJLVF/6g3Lw8mjnzU/5sADF7Q60MAf0BDJgyZQrl5W/kt1e7enqppbXdsmEKiRJNGVr0h42P6PjxE6Jb/YpHa/YOkZNUvMNjCMaTx+a3bd9Od+42KL+9akbwDiRNqO7qNbZl1qxZbJ+/AOJJ7ug3BGPzkydP5nP/uw33ePMtrW1SI+0UHgBsQf7A2Wdq6pf8DgPshd2y/byt/4SA2R2LxcXF8ZGa8eS9YfPvChAM77h0+QpfEMFjESp9ZxDyEfy9EFD3/fx8KSYmhtatW//mdX1v3LxMOIOAvQ3CYxEqEydOZBiynCGFgKYHGTctLY2u1NbxYnbHvC7hoeGNWOSNpKQkBoF88fbBzL8gYBiC60+bNo3KRGwhCyMbyxb/vwnlFXvJy8/n0EB1M0AwBPwBdIwDkoZ79xmAVecDAyGj38B3KUpKSik6KooHP+zdgV9ABe6/bfsO6hQDC+LJqlo/0MKDRXjjiylTp8ZzeDgQ/ygnPx47xrGDDCv74Q9N6ECv/1lPM2bMIAeyf2XlGcu+r+CtgqcjT9TX3yDHuarfuK7K/uOHLoBA9XB8yPHfXzlkf/mxaRCC0CAEoUEIQoMQhAYhCA1CaGqmfwAhD5w7zXu82QAAAABJRU5ErkJggg==";
                    }*/

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
        public JsonResult ListarPersonaProhibidoIngresoIdJson(int TimadorID) {
            bool respuesta = false;
            string errormensaje = "";
            CAL_PersonaProhibidoIngresoEntidad item = new CAL_PersonaProhibidoIngresoEntidad();

            try {

                item = timadorBL.TimadorIdObtenerJson(TimadorID);
                respuesta = true;

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
                respuesta = false;
            }
            return Json(new { data = item, respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PersonaProhibidoIngresoEditarJson(CAL_PersonaProhibidoIngresoEntidad timador) {
            var errormensaje = "";
            bool respuestaConsulta = false;
            var empleado = new SEG_EmpleadoEntidad();
            empleado = (SEG_EmpleadoEntidad)Session["empleado"];
            timador.EmpleadoID = empleado.EmpleadoID;
            CAL_PersonaProhibidoIngresoEntidad timadorBusqueda = new CAL_PersonaProhibidoIngresoEntidad();
            try {
                string PathPrincipal = Path.Combine(ConfigurationManager.AppSettings["PathArchivos"].ToString(), "Ludopatas");
                HttpPostedFileBase file = Request.Files["File"];

                timadorBusqueda = timadorBL.GetTimadorPorDNI(timador.DNI);
                if(timadorBusqueda.TimadorID != timador.TimadorID) {
                    errormensaje = "El numero de DNI ya existe";
                    respuestaConsulta = false;
                    return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
                }


                //generar nombre de archivo
                var inicial = string.Empty;
                var objCadena = timador.Nombre.Split(" ".ToArray());
                if(objCadena.Length > 0) {
                    inicial = objCadena.Where(objeto => !string.IsNullOrWhiteSpace(objeto))
                        .Aggregate(inicial, (current, objeto) => current + objeto[0].ToString());
                }

                objCadena = timador.ApellidoPaterno.Split(" ".ToArray());
                if(objCadena.Length > 0) {
                    inicial = objCadena.Where(objeto => !string.IsNullOrWhiteSpace(objeto))
                        .Aggregate(inicial, (current, objeto) => current + objeto[0].ToString());
                }

                var imageNameFormNew = inicial + "_" + timador.TimadorID + "_tim.jpg";
                var imageNameFormOld = timador.Foto;

                if(timador.Foto != null && timador.Foto != string.Empty) {
                    timador.Foto = imageNameFormNew;
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
                        var nombreFoto = timador.Iniciales + "_" + timador.TimadorID +
                                   "_tim.jpg";
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

                        timador.Imagen = ruta;
                        timador.Foto = nombreFoto;
                    }
                }
                respuestaConsulta = timadorBL.TimadorEditarJson(timador);

                if(respuestaConsulta) {

                    errormensaje = "Registro de Persona Prohibido Ingreso Actualizado Correctamente";
                } else {
                    errormensaje = "Error al Actualizar Persona Prohibido Ingreso , LLame Administrador";
                    respuestaConsulta = false;
                }

            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult PersonaProhibidoIngresoGuardarJson(CAL_PersonaProhibidoIngresoEntidad timador) {
            var errormensaje = "";
            int IdInsertado = 0;
            bool respuesta = false;
            string ruta = "";
            string nombreFoto = "";
            var empleado = new SEG_EmpleadoEntidad();
            empleado = (SEG_EmpleadoEntidad)Session["empleado"];
            timador.EmpleadoID = empleado.EmpleadoID;
            try {
                string PathPrincipal = Path.Combine(ConfigurationManager.AppSettings["PathArchivos"].ToString(), "Ludopatas");
                HttpPostedFileBase file = Request.Files["File"];
                timador.FechaRegistro = DateTime.Now;
                timador.FechaInscripcion = DateTime.Now;


                if(file != null) {
                    if(file.ContentLength > 0) {
                        if(!Directory.Exists(PathPrincipal)) {
                            respuesta = false;
                            errormensaje = "No se encontro el directorio.";
                            return Json(new { respuesta, mensaje = errormensaje });

                        }
                    }
                }

                var timadorBusqueda = timadorBL.GetTimadorPorDNI(timador.DNI);
                if(timadorBusqueda.TimadorID != timador.TimadorID) {
                    errormensaje = "El numero de DNI ya existe";
                    return Json(new { respuesta = false, mensaje = errormensaje });
                }

                IdInsertado = timadorBL.TimadorInsertarJson(timador);
                if(IdInsertado > 0) {
                    timador.TimadorID = IdInsertado;
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

                            nombreFoto = timador.Iniciales + "_" +
                            timador.TimadorID + "_tim.jpg";
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

                    timador.Foto = nombreFoto;
                    timador.Imagen = ruta;
                    timadorBL.TimadorEditarJson(timador);


                    //Insertar Incidencia
                    CAL_PersonaProhibidoIngresoIncidenciaEntidad incidencia = new CAL_PersonaProhibidoIngresoIncidenciaEntidad();

                    incidencia.TimadorID = timador.TimadorID;
                    incidencia.Estado = 1;
                    incidencia.FechaRegistro = DateTime.Now;
                    incidencia.EmpleadoID = timador.EmpleadoID;
                    incidencia.CodSala = timador.CodSala;
                    incidencia.Observacion = timador.Observacion;
                    incidencia.SustentoLegal = timador.SustentoLegal;

                    IdInsertado = timadorIncidenciaBL.InsertarTimadorIncidencia(incidencia);

                    if(IdInsertado > 0) {
                        respuesta = true;
                        errormensaje = "Registro Persona Prohibido Ingreso Guardado Correctamente";
                    } else {
                        errormensaje = "Error al crear la incidencia de Persona Prohibido Ingreso , LLame Administrador";
                        respuesta = false;
                    }

                } else {
                    errormensaje = "Error al crear el Persona Prohibido Ingreso , LLame Administrador";
                    respuesta = false;
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult PersonaProhibidoIngresoEliminarJson(int id) {
            var errormensaje = "";
            bool respuesta = false;
            try {
                respuesta = timadorBL.TimadorEliminarJson(id);
                if(respuesta) {
                    respuesta = true;
                    errormensaje = "Se quitó el Persona Prohibido Ingreso Correctamente";
                } else {
                    errormensaje = "error al Quitar el Persona Prohibido Ingreso , LLame Administrador";
                    respuesta = false;
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult PersonaProhibidoIngresoDescargarExcelJson() {
            string fecha = DateTime.Now.ToString("dd_MM_yyyy");
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<CAL_PersonaProhibidoIngresoEntidad> lista = new List<CAL_PersonaProhibidoIngresoEntidad>();
            string strElementos = String.Empty;
            string strElementos_ = String.Empty;
            List<dynamic> nombresala = new List<dynamic>();
            string salasSeleccionadas = String.Empty;


            //Nuevo Metodo Excel con Collapse
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();
            var ws = excel.Workbook.Worksheets.Add("Listado Persona Prohibido Ingreso");
            ws.Cells["B4"].Value = "Listado Persona Prohibido Ingreso";
            ws.Cells["B4:J4"].Style.Font.Bold = true;

            ws.Cells["B4"].Style.Font.Size = 20;
            ws.Cells["B4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["B4:J4"].Merge = true;
            ws.Cells["B4:J4"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            int fila = 8, inicioGrupo = 0, finGrupo = 0;

            ws.Cells["B7"].Value = "ID";
            ws.Cells["C7"].Value = "Nombre Completo";
            ws.Cells["D7"].Value = "DOI";
            ws.Cells["E7"].Value = "Usuario";
            ws.Cells["F7"].Value = "Sala";
            ws.Cells["G7"].Value = "Tipo Persona Prohibido Ingreso";
            ws.Cells["H7"].Value = "Tipo DOI";
            ws.Cells["I7"].Value = "Estado";
            ws.Cells["J7"].Value = "Fecha Registro";


            ws.Cells["B7:J7"].Style.Font.Bold = true;
            ws.Cells["B7:J7"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["B7:J7"].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            ws.Cells["B7:J7"].Style.Font.Color.SetColor(Color.White);
            ws.Cells["B7:J7"].Style.Border.BorderAround(ExcelBorderStyle.Thin);


            try {

                lista = timadorBL.TimadorListadoCompletoJson();

                if(lista.Count > 0) {
                    foreach(CAL_PersonaProhibidoIngresoEntidad timador in lista) {
                        List<int> codsSalas = salaBl.ObtenerCodsSalasDeSesion(Session);
                        List<CAL_PersonaProhibidoIngresoIncidenciaEntidad> listaIncidencia = timadorIncidenciaBL.GetAllTimadorIncidenciaxTimador(timador.TimadorID, codsSalas);

                        timador.NombreCompleto = timador.Nombre + " " + timador.ApellidoPaterno + " " + timador.ApellidoMaterno;
                        timador.EmpleadoNombreCompleto = timador.EmpleadoNombres + " " + timador.EmpleadoApellidoPaterno;
                        timador.SalaNombreCompuesto = String.Join(" - ", listaIncidencia.Select(x => x.SalaNombre.ToString()).ToList());

                        ws.Cells[string.Format("B{0}", fila)].Value = timador.TimadorID;
                        ws.Cells[string.Format("C{0}", fila)].Value = timador.NombreCompleto.ToUpper(); ;
                        ws.Cells[string.Format("D{0}", fila)].Value = timador.DNI.ToUpper();
                        ws.Cells[string.Format("E{0}", fila)].Value = timador.EmpleadoNombreCompleto.ToUpper();
                        ws.Cells[string.Format("F{0}", fila)].Value = timador.SalaNombreCompuesto.ToUpper();
                        ws.Cells[string.Format("G{0}", fila)].Value = timador.TipoTimadorID == 1 ? "Interno" : "Externo";
                        ws.Cells[string.Format("H{0}", fila)].Value = timador.TipoDOI == 1 ? "DNI" : "Otros";
                        ws.Cells[string.Format("I{0}", fila)].Value = timador.Estado == 1 ? "ACTIVO" : "INACTIVO";
                        ws.Cells[string.Format("J{0}", fila)].Value = timador.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");

                        //Styles
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Bold = true;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.PatternType = ExcelFillStyle.Gray125;
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Font.Color.SetColor(Color.Black);
                        ws.Cells[string.Format("B{0}:J{0}", fila)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        fila++;




                        if(listaIncidencia.Count > 0) {
                            inicioGrupo = fila;
                            //Cabeceras
                            ws.Cells[string.Format("C{0}", inicioGrupo)].Value = "ID";
                            ws.Cells[string.Format("D{0}", inicioGrupo)].Value = "CodSala";
                            ws.Cells[string.Format("E{0}", inicioGrupo)].Value = "Observacion";
                            ws.Cells[string.Format("F{0}", inicioGrupo)].Value = "SustentoLegal";
                            ws.Cells[string.Format("G{0}", inicioGrupo)].Value = "EmpleadoID";
                            ws.Cells[string.Format("H{0}", inicioGrupo)].Value = "Estado";
                            ws.Cells[string.Format("I{0}", inicioGrupo)].Value = "Fecha Registro";
                            ws.Cells[string.Format("C{0}:I{0}", inicioGrupo)].Style.Font.Bold = true;
                            ws.Cells[string.Format("C{0}:I{0}", inicioGrupo)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[string.Format("C{0}:I{0}", inicioGrupo)].Style.Fill.BackgroundColor.SetColor(Color.DarkRed);
                            ws.Cells[string.Format("C{0}:I{0}", inicioGrupo)].Style.Font.Color.SetColor(Color.White);
                            fila++;
                            foreach(var detalle in listaIncidencia) {

                                detalle.EmpleadoNombreCompleto = detalle.EmpleadoNombres + " " + detalle.EmpleadoApellidoPaterno;

                                ws.Cells[string.Format("C{0}", fila)].Value = detalle.TimadorIncidenciaID;
                                ws.Cells[string.Format("D{0}", fila)].Value = detalle.SalaNombre.ToUpper();
                                ws.Cells[string.Format("E{0}", fila)].Value = detalle.Observacion.Trim() == "" ? "--" : timador.Observacion.Trim().ToUpper();
                                ws.Cells[string.Format("F{0}", fila)].Value = detalle.SustentoLegal == 1 ? "CON SUSTENTO LEGAL" : "SIN SUSTENTO LEGAL";
                                ws.Cells[string.Format("G{0}", fila)].Value = detalle.EmpleadoNombreCompleto.ToUpper();
                                ws.Cells[string.Format("H{0}", fila)].Value = detalle.Estado == 1 ? "ACTIVO" : "INACTIVO";
                                ws.Cells[string.Format("I{0}", fila)].Value = detalle.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss");
                                fila++;
                            }
                            fila++;
                            finGrupo = fila - 1;
                            for(var i = inicioGrupo; i <= finGrupo; i++) {
                                ws.Row(i).OutlineLevel = 1;
                                ws.Row(i).Collapsed = true;
                            }
                        } else {
                            ws.Cells[string.Format("D{0}", fila)].Value = "No se encontraron incidencias para esta persona";
                            ws.Cells[string.Format("D{0}:I{0}", fila)].Style.Font.Bold = true;
                            ws.Cells[string.Format("D{0}", fila)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells[string.Format("D{0}:I{0}", fila)].Merge = true;
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;
                            //Agregar una fila extra en el detalle para que no se confundan con el collapse
                            fila++;
                            ws.Row(fila).OutlineLevel = 1;
                            ws.Row(fila).Collapsed = true;

                            fila++;
                        }

                    }
                    fila++;
                    ws.Cells["A:AZ"].AutoFitColumns();

                    excelName = "PersonaProhibidoIngreso_" + fecha + ".xlsx";
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

        #region Incidencias
        [HttpPost]
        public JsonResult ListarIncidenciasxPersonaProhibidoIngreso(int TimadorID) {
            bool respuesta = false;
            string errormensaje = "";
            List<CAL_PersonaProhibidoIngresoIncidenciaEntidad> lista = new List<CAL_PersonaProhibidoIngresoIncidenciaEntidad>();

            try {
                List<int> codsSalas = salaBl.ObtenerCodsSalasDeSesion(Session);
                lista = timadorIncidenciaBL.GetAllTimadorIncidenciaxTimador(TimadorID, codsSalas);
                respuesta = true;
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
                respuesta = false;
            }
            object response = new {
                data = lista,
                respuesta,
                mensaje = errormensaje
            };
            return Json(response);
        }

        [HttpPost]
        public JsonResult ListarIncidenciaId(int id) {
            bool respuesta = false;
            var errormensaje = "";
            CAL_PersonaProhibidoIngresoIncidenciaEntidad item = new CAL_PersonaProhibidoIngresoIncidenciaEntidad();

            try {

                item = timadorIncidenciaBL.GetIDTimadorIncidencia(id);
                respuesta = true;

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
                respuesta = false;
            }
            return Json(new { data = item, respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult IncidenciaEditarJson(CAL_PersonaProhibidoIngresoIncidenciaEntidad incidencia) {
            var errormensaje = "";
            bool respuestaConsulta = false;
            var empleado = new SEG_EmpleadoEntidad();
            empleado = (SEG_EmpleadoEntidad)Session["empleado"];
            incidencia.EmpleadoID = empleado.EmpleadoID;
            incidencia.FechaRegistro = DateTime.Now;
            CAL_PersonaProhibidoIngresoEntidad timadorBusqueda = new CAL_PersonaProhibidoIngresoEntidad();
            try {

                respuestaConsulta = timadorIncidenciaBL.EditarTimadorIncidencia(incidencia);

                if(respuestaConsulta) {

                    errormensaje = "Registro de Incidencia Actualizado Correctamente";
                } else {
                    errormensaje = "Error al Actualizar Incidencia, LLame Administrador";
                    respuestaConsulta = false;
                }

            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult IncidenciaGuardarJson(CAL_PersonaProhibidoIngresoIncidenciaEntidad incidencia) {
            string errormensaje = string.Empty;
            int IdInsertado = 0;
            bool respuesta = false;
            SEG_EmpleadoEntidad empleado = new SEG_EmpleadoEntidad();
            empleado = (SEG_EmpleadoEntidad)Session["empleado"];
            incidencia.EmpleadoID = empleado.EmpleadoID;
            try {
                incidencia.FechaRegistro = DateTime.Now;

                List<int> codsSalas = new List<int> { incidencia.CodSala };
                List<CAL_PersonaProhibidoIngresoIncidenciaEntidad> incidenciaBusqueda = timadorIncidenciaBL.GetAllTimadorIncidenciaxTimador(incidencia.TimadorID, codsSalas);

                if(incidenciaBusqueda.Count == 0) {
                    IdInsertado = timadorIncidenciaBL.InsertarTimadorIncidencia(incidencia);
                    respuesta = IdInsertado > 0;
                    errormensaje = respuesta ? "Registro Incidencia Creado Correctamente" : "Error al crear la incidencia. LLame Administrador";
                } else {
                    errormensaje = "No puede registrar otra incidencia en la misma sala para esta persona.";
                    return Json(new { respuesta = false, mensaje = errormensaje });
                }

            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult IncidenciaEliminarJson(int id) {
            var errormensaje = "";
            bool respuesta = false;
            try {
                respuesta = timadorIncidenciaBL.EliminarTimadorIncidencia(id);
                if(respuesta) {
                    respuesta = true;
                    errormensaje = "Se quitó la incidencia Correctamente";
                } else {
                    errormensaje = "error al Quitar la incidencia , LLame Administrador";
                    respuesta = false;
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje });
        }
        #endregion
    }
}