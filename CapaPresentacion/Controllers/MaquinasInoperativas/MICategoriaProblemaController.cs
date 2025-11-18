using CapaDatos.MaquinasInoperativas;
using CapaEntidad;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.MaquinasInoperativas
{
    [seguridad]
    public class MICategoriaProblemaController : Controller {

        private MI_ProblemaBL problemaBL = new MI_ProblemaBL();
        private MI_CategoriaProblemaBL categoriaProblemaBL = new MI_CategoriaProblemaBL();
        public ActionResult ListadoCategoriaProblema() {
            return View("~/Views/MaquinasInoperativas/ListadoCategoriaProblema.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarCategoriaProblemaJson() {
            var errormensaje = "";
            var lista = new List<MI_CategoriaProblemaEntidad>();

            try {

                lista = categoriaProblemaBL.CategoriaProblemaListadoCompletoJson();

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarCategoriaProblemaActiveJson() {
            var errormensaje = "";
            bool respuesta = false;
            var lista = new List<MI_CategoriaProblemaEntidad>();

            try {

                lista = categoriaProblemaBL.CategoriaProblemaListadoActiveJson();
                respuesta = true;

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
                respuesta = false;
            }
            return Json(new { data = lista.ToList(), respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarCategoriaProblemaCodJson(int codCategoriaProblema) {
            var errormensaje = "";
            bool respuesta = false;
            MI_CategoriaProblemaEntidad item = new MI_CategoriaProblemaEntidad();

            try {

                item = categoriaProblemaBL.CategoriaProblemaCodObtenerJson(codCategoriaProblema);
                respuesta = true;

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
                respuesta = false;
            }
            return Json(new { data = item, respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CategoriaProblemaEditarJson(MI_CategoriaProblemaEntidad categoriaProblema) {
            var errormensaje = "";
            bool respuestaConsulta = false;
            categoriaProblema.FechaModificacion = DateTime.Now;
            try {
                respuestaConsulta = categoriaProblemaBL.CategoriaProblemaEditarJson(categoriaProblema);

                if(respuestaConsulta) {

                    errormensaje = "Registro de Categoria Problema Actualizado Correctamente";
                } else {
                    errormensaje = "Error al Actualizar Categoria Problema , LLame Administrador";
                    respuestaConsulta = false;
                }

            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult CategoriaProblemaGuardarJson(MI_CategoriaProblemaEntidad categoriaProblema) {
            var errormensaje = "";
            Int64 respuestaConsulta = 0;
            bool respuesta = false;
            try {
                SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];
                categoriaProblema.FechaRegistro = DateTime.Now;
                categoriaProblema.FechaModificacion = DateTime.Now;
                categoriaProblema.CodUsuario = usuario.UsuarioNombre;

                respuestaConsulta = categoriaProblemaBL.CategoriaProblemaInsertarJson(categoriaProblema);

                if(respuestaConsulta > 0) {
                    respuesta = true;
                    errormensaje = "Registro de Categoria Problema Guardado Correctamente";
                } else {
                    errormensaje = "Error al Crear Categoria Problema , LLame Administrador";
                    respuesta = false;
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuesta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult CategoriaProblemaEliminarJson(int cod) {
            var errormensaje = "";
            bool respuesta = false;


            List<MI_ProblemaEntidad> lista = problemaBL.ProblemaListadoCompletoJson();

            foreach(var item in lista) {
                if(item.CodCategoriaProblema == cod) {
                    errormensaje = "La Categoria Problema se encuentra actualmente asignada.";
                    respuesta = false;
                    return Json(new { respuesta = respuesta, mensaje = errormensaje });
                }
            }

            try {
                respuesta = categoriaProblemaBL.CategoriaProblemaEliminarJson(cod);
                if(respuesta) {
                    respuesta = true;
                    errormensaje = "Se quitó la Categoria Problema Correctamente";
                } else {
                    errormensaje = "Error al Quitar la Categoria Problema , LLame Administrador";
                    respuesta = false;
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult CategoriaProblemaDescargarExcelJson() {
            string fecha = DateTime.Now.ToString("dd_MM_yyyy");
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<MI_CategoriaProblemaEntidad> lista = new List<MI_CategoriaProblemaEntidad>();
            var strElementos = String.Empty;
            var strElementos_ = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;
            try {


                lista = categoriaProblemaBL.CategoriaProblemaListadoCompletoJson();
                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("CategoriaProblema");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;

                    workSheet.Cells[3, 2].Value = "Codigo";
                    workSheet.Cells[3, 3].Value = "Nombre";
                    workSheet.Cells[3, 4].Value = "Descripcion";
                    workSheet.Cells[3, 5].Value = "Fecha Registro";
                    workSheet.Cells[3, 6].Value = "Fecha Modificacion";
                    workSheet.Cells[3, 7].Value = "Codigo Usuario";
                    workSheet.Cells[3, 8].Value = "Estado";

                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach(var registro in lista) {

                        workSheet.Cells[recordIndex, 2].Value = registro.CodCategoriaProblema;
                        workSheet.Cells[recordIndex, 3].Value = registro.Nombre.ToUpper();
                        workSheet.Cells[recordIndex, 4].Value = registro.Descripcion.ToUpper();
                        workSheet.Cells[recordIndex, 5].Value = registro.FechaRegistro.ToString("dd-MM-yyyy hh:mm:ss tt");
                        workSheet.Cells[recordIndex, 6].Value = registro.FechaModificacion.ToString("dd-MM-yyyy hh:mm:ss tt");
                        workSheet.Cells[recordIndex, 7].Value = registro.CodUsuario;
                        workSheet.Cells[recordIndex, 8].Value = registro.Estado == 1 ? "ACTIVO" : "INACTIVO";
                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:H3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:H3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:H3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:H3"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B3:H3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:H3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:H3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:H3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B3:H3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:H3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:H3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:H3"].Style.Border.Bottom.Color.SetColor(colborder);

                    int filasagregadas = 3;
                    total = filasagregadas + total;

                    workSheet.Cells["B4:H" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    /*
                    workSheet.Cells["B2:E2"].Merge = true;
                    workSheet.Cells["B2:E2"].Style.Font.Bold = true;
                    */

                    int filaFooter = total + 1;
                    workSheet.Cells["B" + filaFooter + ":H" + filaFooter].Merge = true;
                    workSheet.Cells["B" + filaFooter + ":H" + filaFooter].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter + ":H" + filaFooter].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter + ":H" + filaFooter].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter + ":H" + filaFooter].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter + ":H" + filaFooter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells["B" + filaFooter + ":H" + filaFooter].Style.Font.Size = 14;
                    workSheet.Cells[filaFooter, 2].Value = "Total : " + (total - filasagregadas) + " Registros";
                    workSheet.Cells[filaFooter, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["B4:H" + total].Style.WrapText = true;

                    int filaultima = total;
                    workSheet.Cells[3, 2, filaultima, 8].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 40;
                    workSheet.Column(4).Width = 40;
                    workSheet.Column(5).Width = 30;
                    workSheet.Column(6).Width = 30;
                    workSheet.Column(7).Width = 18;
                    workSheet.Column(8).Width = 18;
                    excelName = "CategoriaProblema_" + fecha + ".xlsx";
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
    }
}