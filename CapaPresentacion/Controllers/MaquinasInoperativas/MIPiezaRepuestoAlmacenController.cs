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
using CapaEntidad.MaquinasInoperativas;
using CapaNegocio.MaquinasInoperativas;

namespace CapaPresentacion.Controllers.MaquinasInoperativas
{
    [seguridad]
    public class MIPiezaRepuestoAlmacenController : Controller
    {

        MI_PiezaRepuestoAlmacenBL piezaRepuestoAlmacenBL = new MI_PiezaRepuestoAlmacenBL();

        public ActionResult ListadoPiezaRepuestoAlmacen() {
            return View("~/Views/MaquinasInoperativas/ListadoPiezaRepuestoAlmacen.cshtml");
        }


        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarPiezaRepuestoAlmacenJson() {
            var errormensaje = "";
            var lista = new List<MI_PiezaRepuestoAlmacenEntidad>();

            try {

                lista = piezaRepuestoAlmacenBL.PiezaRepuestoAlmacenListadoCompletoJson();

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarPiezaRepuestoAlmacenActiveJson() {
            var errormensaje = "";
            bool respuesta = false;
            var lista = new List<MI_PiezaRepuestoAlmacenEntidad>();

            try {

                lista = piezaRepuestoAlmacenBL.PiezaRepuestoAlmacenListadoActiveJson();
                respuesta = true;

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
                respuesta = false;
            }
            return Json(new { data = lista.ToList(), respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarPiezaRepuestoAlmacenCodJson(int codPiezaRepuestoAlmacen) {
            var errormensaje = "";
            bool respuesta = false;
            MI_PiezaRepuestoAlmacenEntidad item = new MI_PiezaRepuestoAlmacenEntidad();

            try {

                item = piezaRepuestoAlmacenBL.PiezaRepuestoAlmacenCodObtenerJson(codPiezaRepuestoAlmacen);
                respuesta = true;

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
                respuesta = false;
            }
            return Json(new { data = item, respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PiezaRepuestoAlmacenEditarJson(MI_PiezaRepuestoAlmacenEntidad piezaRepuestoAlmacen) {
            var errormensaje = "";
            bool respuestaConsulta = false;
            piezaRepuestoAlmacen.FechaModificacion = DateTime.Now;
            if(piezaRepuestoAlmacen.Cantidad <= 0) { piezaRepuestoAlmacen.Estado = 0; } else { piezaRepuestoAlmacen.Estado = 1; };
            try {
                respuestaConsulta = piezaRepuestoAlmacenBL.PiezaRepuestoAlmacenEditarJson(piezaRepuestoAlmacen);

                if(respuestaConsulta) {

                    errormensaje = "Registro de Categoria Pieza Actualizado Correctamente";
                } else {
                    errormensaje = "Error al Actualizar Categoria Pieza , LLame Administrador";
                    respuestaConsulta = false;
                }

            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult PiezaRepuestoAlmacenGuardarJson(MI_PiezaRepuestoAlmacenEntidad piezaRepuestoAlmacen) {
            var errormensaje = "";
            Int64 respuestaConsulta = 0;
            bool respuesta = false;
            try {
                SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];
                piezaRepuestoAlmacen.FechaRegistro = DateTime.Now;
                piezaRepuestoAlmacen.FechaModificacion = DateTime.Now;
                piezaRepuestoAlmacen.Estado = 1;

                bool existe = piezaRepuestoAlmacenBL.RevisarExistenciaPiezaRepuestoAlmacen(piezaRepuestoAlmacen);
                if (!existe)
                {
                    respuestaConsulta = piezaRepuestoAlmacenBL.PiezaRepuestoAlmacenInsertarJson(piezaRepuestoAlmacen);
                    if (respuestaConsulta > 0)
                    {
                        respuesta = true;
                        errormensaje = "Registro de Categoria Pieza Guardado Correctamente";
                    }
                    else
                    {
                        errormensaje = "Error al Crear Categoria Pieza , LLame Administrador";
                        respuesta = false;
                    }
                } else
                {

                    respuesta = false;
                    errormensaje = "Ya existe ese repuesto en almacen, no se puede crear";
                }

            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuesta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult PiezaRepuestoAlmacenEliminarJson(int cod) {
            var errormensaje = "";
            bool respuesta = false;



            try {
                respuesta = piezaRepuestoAlmacenBL.PiezaRepuestoAlmacenEliminarJson(cod);
                if(respuesta) {
                    respuesta = true;
                    errormensaje = "Se quitó la Categoria Pieza Correctamente";
                } else {
                    errormensaje = "Error al Quitar la Categoria Pieza , LLame Administrador";
                    respuesta = false;
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult PiezaRepuestoAlmacenDescargarExcelJson() {
            string fecha = DateTime.Now.ToString("dd_MM_yyyy");
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<MI_PiezaRepuestoAlmacenEntidad> lista = new List<MI_PiezaRepuestoAlmacenEntidad>();
            var strElementos = String.Empty;
            var strElementos_ = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;
            try {


                lista = piezaRepuestoAlmacenBL.PiezaRepuestoAlmacenListadoCompletoJson();
                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("PiezaRepuestoAlmacen");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;

                    workSheet.Cells[3, 2].Value = "Codigo";
                    workSheet.Cells[3, 3].Value = "Sala";
                    workSheet.Cells[3, 4].Value = "Almacen";
                    workSheet.Cells[3, 5].Value = "Repuesto";
                    workSheet.Cells[3, 6].Value = "Cantidad";
                    workSheet.Cells[3, 7].Value = "Fecha Registro";
                    workSheet.Cells[3, 8].Value = "Estado";

                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach(var registro in lista) {

                        workSheet.Cells[recordIndex, 2].Value = registro.CodPiezaRepuestoAlmacen;
                        workSheet.Cells[recordIndex, 3].Value = registro.NombreSala;
                        workSheet.Cells[recordIndex, 4].Value = registro.NombreAlmacen;
                        workSheet.Cells[recordIndex, 5].Value = registro.NombrePiezaRepuesto;
                        workSheet.Cells[recordIndex, 6].Value = registro.Cantidad;
                        workSheet.Cells[recordIndex, 7].Value = registro.FechaRegistro.ToString("dd-MM-yyyy hh:mm:ss tt");
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
                    workSheet.Column(5).Width = 60;
                    workSheet.Column(6).Width = 18;
                    workSheet.Column(7).Width = 40;
                    workSheet.Column(8).Width = 18;
                    excelName = "PiezaRepuestoAlmacen_" + fecha + ".xlsx";
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


        [HttpPost]
        public ActionResult EditarCantidadPiezaRepuestoAlmacen(MI_PiezaRepuestoAlmacenEntidad piezaRepuestoAlmacen) {
            var errormensaje = "";
            bool respuestaConsulta = false;
            piezaRepuestoAlmacen.FechaModificacion = DateTime.Now;
            try {
                respuestaConsulta = piezaRepuestoAlmacenBL.EditarCantidadPiezaRepuestoAlmacen(piezaRepuestoAlmacen);

                if(respuestaConsulta) {

                    errormensaje = "Registro de Categoria Pieza Actualizado Correctamente";
                } else {
                    errormensaje = "Error al Actualizar Categoria Pieza , LLame Administrador";
                    respuestaConsulta = false;
                }

            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }


        [HttpPost]
        public JsonResult ListarPiezaRepuestoAlmacenxTipoJson(int codTipo) {
            var errormensaje = "";
            var lista = new List<MI_PiezaRepuestoAlmacenEntidad>();

            try {


                lista = piezaRepuestoAlmacenBL.GetAllPiezaRepuestoAlmacenxTipo(codTipo);

            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarPiezaRepuestoAlmacenxTipoxAlmacenJson(int codTipo, int codAlmacen) {
            var errormensaje = "";
            List<MI_PiezaRepuestoAlmacenEntidad> lista = new List<MI_PiezaRepuestoAlmacenEntidad>();

            try {

                string queryTipo = "";

                if(codTipo == 0) {
                    queryTipo = "0";
                    lista = piezaRepuestoAlmacenBL.GetAllPiezaRepuestoAlmacenxAlmacen(codAlmacen);
                }
                if(codTipo==1) {
                    queryTipo = "1";
                    lista = piezaRepuestoAlmacenBL.GetAllPiezaRepuestoAlmacenxPiezaxAlmacen( codAlmacen);
                }
                if (codTipo == 2) {
                    queryTipo = "2";
                    lista = piezaRepuestoAlmacenBL.GetAllPiezaRepuestoAlmacenxRepuestoxAlmacen( codAlmacen);
                }


            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }


    }




}