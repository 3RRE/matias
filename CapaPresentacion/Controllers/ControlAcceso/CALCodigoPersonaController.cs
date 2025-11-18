using CapaDatos.ControlAcceso;
using CapaEntidad.ControlAcceso;
using CapaNegocio.ControlAcceso;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.ControlAcceso
{
    [seguridad]
    public class CALCodigoPersonaController : Controller
    {

        private CAL_CodigoPersonaBL codigoPersonaBL = new CAL_CodigoPersonaBL();

        public ActionResult ListadoCodigoPersona()
        {
            return View("~/Views/ControlAcceso/ListadoCodigoPersona.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarCodigoPersonaJson()
        {
            var errormensaje = "";
            var lista = new List<CAL_CodigoPersonaEntidad>();

            try
            {

                lista = codigoPersonaBL.CodigoPersonaListadoCompletoJson();

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarCodigoPersonaIdJson(int CodigoPersonaID)
        {
            var errormensaje = "";
            CAL_CodigoPersonaEntidad item = new CAL_CodigoPersonaEntidad();

            try
            {

                item = codigoPersonaBL.CodigoPersonaIdObtenerJson(CodigoPersonaID);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = item, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CodigoPersonaEditarJson(CAL_CodigoPersonaEntidad codigoPersona)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = codigoPersonaBL.CodigoPersonaEditarJson(codigoPersona);

                if (respuestaConsulta)
                {

                    respuestaConsulta = true;
                    errormensaje = "Registro de CodigoPersona Actualizado Correctamente ";


                }
                else
                {
                    errormensaje = "Error al Actualizar CodigoPersona , LLame Administrador";
                    respuestaConsulta = false;
                }

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult CodigoPersonaGuardarJson(CAL_CodigoPersonaEntidad codigoPersona)
        {
            var errormensaje = "";
            Int64 respuestaConsulta = 0;
            bool respuesta = false;
            try
            {
                codigoPersona.Editable = 1;
                respuestaConsulta = codigoPersonaBL.CodigoPersonaInsertarJson(codigoPersona);

                if (respuestaConsulta > 0)
                {
                    respuesta = true;
                    errormensaje = "Registro CodigoPersona Guardado Correctamente";
                }
                else
                {
                    errormensaje = "Error al crear la CodigoPersona , LLame Administrador";
                    respuesta = false;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult CodigoPersonaEliminarJson(int id)
        {
            var errormensaje = "";
            bool respuesta = false;
            try
            {
                respuesta = codigoPersonaBL.CodigoPersonaEliminarJson(id);
                if (respuesta)
                {
                    respuesta = true;
                    errormensaje = "Se quitó el Codigo Persona Correctamente";
                }
                else
                {
                    errormensaje = "error al Quitar el Codigo Persona , LLame Administrador";
                    respuesta = false;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult CodigoPersonaDescargarExcelJson()
        {
            string fecha = DateTime.Now.ToString("dd_MM_yyyy");
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<CAL_CodigoPersonaEntidad> lista = new List<CAL_CodigoPersonaEntidad>();
            var strElementos = String.Empty;
            var strElementos_ = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;
            try
            {


                lista = codigoPersonaBL.CodigoPersonaListadoCompletoJson();
                if (lista.Count > 0)
                {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("CodigoPersona");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "TipoPersona";
                    workSheet.Cells[3, 4].Value = "Codigo Nombre";
                    workSheet.Cells[3, 5].Value = "Editable";

                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach (var registro in lista)
                    {

                        workSheet.Cells[recordIndex, 2].Value = registro.Id;
                        workSheet.Cells[recordIndex, 3].Value = registro.TipoPersona.ToUpper().Trim() == "" ? "--" : registro.TipoPersona.Trim().ToUpper();
                        workSheet.Cells[recordIndex, 4].Value = registro.CodigoNombre.ToUpper().Trim() == "" ? "--" : registro.CodigoNombre.Trim().ToUpper();
                        workSheet.Cells[recordIndex, 5].Value = registro.Editable == 1 ? "ACTIVO" : "INACTIVO";

                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:E3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:E3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:E3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:E3"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B3:E3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:E3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:E3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:E3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B3:E3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:E3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:E3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:E3"].Style.Border.Bottom.Color.SetColor(colborder);

                    int filasagregadas = 3;
                    total = filasagregadas + total;

                    workSheet.Cells["B4:B" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    /*
                    workSheet.Cells["B2:E2"].Merge = true;
                    workSheet.Cells["B2:E2"].Style.Font.Bold = true;
                    */

                    int filaFooter = total + 1;
                    workSheet.Cells["B" + filaFooter + ":E" + filaFooter].Merge = true;
                    workSheet.Cells["B" + filaFooter + ":E" + filaFooter].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter + ":E" + filaFooter].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter + ":E" + filaFooter].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter + ":E" + filaFooter].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter + ":E" + filaFooter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells["B" + filaFooter + ":E" + filaFooter].Style.Font.Size = 14;
                    workSheet.Cells[filaFooter, 2].Value = "Total : " + (total - filasagregadas) + " Registros";
                    workSheet.Cells[filaFooter, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["B4:E" + total].Style.WrapText = true;

                    int filaultima = total;
                    workSheet.Cells[3, 2, filaultima, 5].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 24;
                    workSheet.Column(4).Width = 60;
                    workSheet.Column(5).Width = 18;
                    excelName = "CodigoPersona_" + fecha + ".xlsx";
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());

                    mensaje = "Descargando Archivo";
                    respuesta = true;
                }
                else
                {
                    mensaje = "No se Pudo generar Archivo";
                }

            }
            catch (Exception exp)
            {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });

        }
    }
}