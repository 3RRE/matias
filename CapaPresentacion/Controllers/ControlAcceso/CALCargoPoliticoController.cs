using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaDatos.ControlAcceso;
using CapaNegocio.ControlAcceso;
using CapaEntidad.ControlAcceso;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;

namespace CapaPresentacion.Controllers.ControlAcceso
{
    [seguridad]
    public class CALCargoPoliticoController : Controller
    {
        private CAL_CargoPoliticoBL cargoPoliticoBL = new CAL_CargoPoliticoBL();
        private CAL_PoliticoBL politicoBL = new CAL_PoliticoBL();

        public ActionResult ListadoCargoPolitico()
        {
            return View("~/Views/ControlAcceso/ListadoCargoPolitico.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarCargoPoliticoJson()
        {
            var errormensaje = "";
            var lista = new List<CAL_CargoPoliticoEntidad>();

            try
            {

                lista = cargoPoliticoBL.CargoPoliticoListadoCompletoJson();

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarCargoPoliticoIdJson(int CargoPoliticoID)
        {
            var errormensaje = "";
            CAL_CargoPoliticoEntidad item = new CAL_CargoPoliticoEntidad();

            try
            {

                item = cargoPoliticoBL.CargoPoliticoIdObtenerJson(CargoPoliticoID);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = item, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CargoPoliticoEditarJson(CAL_CargoPoliticoEntidad cargoPolitico)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = cargoPoliticoBL.CargoPoliticoEditarJson(cargoPolitico);

                if (respuestaConsulta)
                {

                    errormensaje = "Registro de Cargo Politico Actualizado Correctamente";
                }
                else
                {
                    errormensaje = "Error al Actualizar Cargo Politico , LLame Administrador";
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
        public ActionResult CargoPoliticoGuardarJson(CAL_CargoPoliticoEntidad cargoPolitico)
        {
            var errormensaje = "";
            Int64 respuestaConsulta = 0;
            bool respuesta = false;
            try
            {
                respuestaConsulta = cargoPoliticoBL.CargoPoliticoInsertarJson(cargoPolitico);

                if (respuestaConsulta > 0)
                {
                    respuesta = true;
                    errormensaje = "Registro Cargo Politico Guardado Correctamente";
                }
                else
                {
                    errormensaje = "Error al crear la Cargo Politico , LLame Administrador";
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
        public ActionResult CargoPoliticoEliminarJson(int id)
        {
            var errormensaje = "";
            bool respuesta = false;


            List<CAL_PoliticoEntidad> lista = politicoBL.PoliticoListadoCompletoJson();

            foreach (var item in lista)
            {
                if (item.CargoPoliticoID == id)
                {
                    errormensaje = "El cargo politico se encuentra actualmente asignado.";
                    respuesta = false;
                    return Json(new { respuesta = respuesta, mensaje = errormensaje });
                }
            }

            try
            {
                respuesta = cargoPoliticoBL.CargoPoliticoEliminarJson(id);
                if (respuesta)
                {
                    respuesta = true;
                    errormensaje = "Se quitó el Cargo Politico Correctamente";
                }
                else
                {
                    errormensaje = "error al Quitar el Cargo Politico , LLame Administrador";
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
        public ActionResult CargoPoliticoDescargarExcelJson()
        {
            string fecha = DateTime.Now.ToString("dd_MM_yyyy");
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<CAL_CargoPoliticoEntidad> lista = new List<CAL_CargoPoliticoEntidad>();
            var strElementos = String.Empty;
            var strElementos_ = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;
            try
            {


                lista = cargoPoliticoBL.CargoPoliticoListadoCompletoJson();
                if (lista.Count > 0)
                {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Cargo Politico");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Nombre";
                    workSheet.Cells[3, 4].Value = "Descripcion";
                    workSheet.Cells[3, 5].Value = "Estado";

                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach (var registro in lista)
                    {

                        workSheet.Cells[recordIndex, 2].Value = registro.CargoPoliticoID;
                        workSheet.Cells[recordIndex, 3].Value = registro.Nombre;
                        workSheet.Cells[recordIndex, 4].Value = registro.Descripcion;
                        workSheet.Cells[recordIndex, 5].Value = registro.Estado == true ? "Activo" : "Inactivo";
                        
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
                    workSheet.Column(3).Width = 44;
                    workSheet.Column(4).Width = 44;
                    workSheet.Column(5).Width = 18;
                    excelName = "CargoPolitico_" + fecha + ".xlsx";
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