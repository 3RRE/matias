using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaDatos.ControlAcceso;
using CapaNegocio.ControlAcceso;
using CapaEntidad.ControlAcceso;
using System.Drawing;
using OfficeOpenXml.Style;
using System.IO;
using OfficeOpenXml;

namespace CapaPresentacion.Controllers.ControlAcceso
{
    [seguridad]
    public class CALPoliticoController : Controller
    {
        private CAL_PoliticoBL politicoBL = new CAL_PoliticoBL();

        public ActionResult ListadoPolitico()
        {
            return View("~/Views/ControlAcceso/ListadoPolitico.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarPoliticoJson()
        {
            var errormensaje = "";
            var lista = new List<CAL_PoliticoEntidad>();

            try
            {

                lista = politicoBL.PoliticoListadoCompletoJson();

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarPoliticoIdJson(int PoliticoID)
        {
            var errormensaje = "";
            bool respuesta = false;
            CAL_PoliticoEntidad item = new CAL_PoliticoEntidad();

            try
            {

                item = politicoBL.PoliticoIdObtenerJson(PoliticoID);
                respuesta = true;

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
                respuesta = false;
            }
            return Json(new { data = item, respuesta= respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PoliticoEditarJson(CAL_PoliticoEntidad politico)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            CAL_PoliticoEntidad politicoBusqueda = new CAL_PoliticoEntidad();
            try
            {
                
                politicoBusqueda = politicoBL.GetPoliticoPorDNI(politico.Dni);
                if (politicoBusqueda.PoliticoID != politico.PoliticoID)
                {
                    errormensaje = "El numero de DNI ya existe";
                    respuestaConsulta = false;
                    return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
                }

                respuestaConsulta = politicoBL.PoliticoEditarJson(politico);

                if (respuestaConsulta)
                {

                    errormensaje = "Registro de Politico Actualizado Correctamente";
                }
                else
                {
                    errormensaje = "Error al Actualizar Politico , LLame Administrador";
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
        public ActionResult PoliticoGuardarJson(CAL_PoliticoEntidad politico)
        {
            var errormensaje = "";
            Int64 respuestaConsulta = 0;
            bool respuesta = false;
            try
            {
                politico.FechaRegistro = DateTime.Now;
                respuestaConsulta = politicoBL.PoliticoInsertarJson(politico);

                if (respuestaConsulta > 0)
                {
                    respuesta = true;
                    errormensaje = "Registro Politico Guardado Correctamente";
                }
                else
                {
                    errormensaje = "Error al crear la Politico , LLame Administrador";
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
        public ActionResult PoliticoEliminarJson(int id)
        {
            var errormensaje = "";
            bool respuesta = false;
            try
            {
                respuesta = politicoBL.PoliticoEliminarJson(id);
                if (respuesta)
                {
                    respuesta = true;
                    errormensaje = "Se quitó el Politico Correctamente";
                }
                else
                {
                    errormensaje = "error al Quitar el Politico , LLame Administrador";
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
        public ActionResult PoliticoDescargarExcelJson()
        {
            string fecha = DateTime.Now.ToString("dd_MM_yyyy");
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<CAL_PoliticoEntidad> lista = new List<CAL_PoliticoEntidad>();
            var strElementos = String.Empty;
            var strElementos_ = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;
            try
            {


                lista = politicoBL.PoliticoListadoCompletoJson();
                if (lista.Count > 0)
                {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Politico");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Nombres";
                    workSheet.Cells[3, 4].Value = "Apellidos";
                    workSheet.Cells[3, 5].Value = "DNI";
                    workSheet.Cells[3, 6].Value = "Entidad Estatal";
                    workSheet.Cells[3, 7].Value = "Meses";
                    workSheet.Cells[3, 8].Value = "Cargo Politico";
                    workSheet.Cells[3, 9].Value = "Estado";
                    workSheet.Cells[3, 10].Value = "Fecha Registro";

                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach (var registro in lista)
                    {

                        workSheet.Cells[recordIndex, 2].Value = registro.PoliticoID;
                        workSheet.Cells[recordIndex, 3].Value = registro.Nombres.ToUpper();
                        workSheet.Cells[recordIndex, 4].Value = registro.Apellidos.ToUpper();
                        workSheet.Cells[recordIndex, 5].Value = registro.Dni;
                        workSheet.Cells[recordIndex, 6].Value = registro.EntidadEstatal.ToUpper();
                        workSheet.Cells[recordIndex, 7].Value = registro.Meses;
                        workSheet.Cells[recordIndex, 8].Value = registro.cargoPoliticoNombre.ToUpper();
                        workSheet.Cells[recordIndex, 9].Value = registro.Estado == true ? "ACTIVO" : "INACTIVO";
                        workSheet.Cells[recordIndex, 10].Value = registro.FechaRegistro.ToString("dd-MM-yyyy hh:mm:ss tt");

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
                    workSheet.Column(4).Width = 40;
                    workSheet.Column(5).Width = 30;
                    workSheet.Column(6).Width = 30;
                    workSheet.Column(7).Width = 18;
                    workSheet.Column(8).Width = 30;
                    workSheet.Column(9).Width = 18;
                    workSheet.Column(10).Width = 30;
                    excelName = "Politico_" + fecha + ".xlsx";
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