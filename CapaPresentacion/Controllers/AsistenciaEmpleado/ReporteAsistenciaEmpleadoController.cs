using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilitarios;
using CapaEntidad.AsistenciaEmpleado;
using CapaNegocio.AsistenciaEmpleado;
using ClaseError = CapaDatos.Utilitarios.ClaseError;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace CapaPresentacion.Controllers.AsistenciaEmpleado
{
    [seguridad]

    public class ReporteAsistenciaEmpleadoController : Controller
    {
        ClaseError error = new ClaseError();
        private EmpleadoAsistenciaBL empleadoAsistenciabl = new EmpleadoAsistenciaBL();

        public ActionResult ReporteAsistencia()
        {
            return View("~/Views/AsistenciaEmpleado/ReporteAsistencia.cshtml");
        }

        

        [HttpPost]
        public ActionResult GetListadoAsistenciaSalaFiltros(int[] ArraySalaId, DateTime fechaIni, DateTime fechaFin)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            int cantElementos = (ArraySalaId == null) ? 0 : ArraySalaId.Length;
            var strElementos = String.Empty;
            List<EmpleadoDatosAsistencia> lista = new List<EmpleadoDatosAsistencia>();

            try
            {
                
                //string strSalas =  String.Join(",", ArraySalaId);
                if (cantElementos > 0)
                {
                    strElementos = " loc_id in(" + "'" + String.Join("','", ArraySalaId) + "'" + ") and ";
                }
                var listado = empleadoAsistenciabl.EmpleadoAsistenciaxFechabetweenListarJson(fechaIni, fechaFin, strElementos);

                error = listado.error;
                lista = listado.empleadoAsistencia;
               
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Asistencia Desde: " + fechaIni.ToShortDateString();
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudo Generar el Listado";
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data = lista.ToList() });
        }

        [HttpPost]
        public ActionResult AsistenciaDescargarExcelJson(int[] ArraySalaId, DateTime fechaini, DateTime fechafin)
        {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            int cantElementos = (ArraySalaId == null) ? 0 : ArraySalaId.Length;
            var strElementos = String.Empty;
            List<EmpleadoDatosAsistencia> lista = new List<EmpleadoDatosAsistencia>();
            try
            {
                if (cantElementos > 0)
                {
                    strElementos = " loc_id in(" + "'" + String.Join("','", ArraySalaId) + "'" + ") and ";
                }

                var EmpleadoAsistenciaTupla = empleadoAsistenciabl.EmpleadoAsistenciaxFechabetweenListarJson(fechaini, fechafin, strElementos);
                error = EmpleadoAsistenciaTupla.error;
                lista = EmpleadoAsistenciaTupla.empleadoAsistencia;
                if (error.Key.Equals(string.Empty))
                {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Reporte de Asistencia" + DateTime.Today.ToString("dd-MM-yyyy"));
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //

                    workSheet.Cells["B" + 2 + ":G" + 2].Merge = true;
                    workSheet.Cells[2, 2].Value = "Reporte de Asistencia del : " + fechaini.ToString("dd-MM-yyyy HH:mm tt") + " al : " + fechafin.ToString("dd-MM-yyyy HH:mm tt");
                    workSheet.Cells[2, 2].Style.Font.Bold = true;

                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;

                    workSheet.Cells[3, 2].Value = "Id";
                    workSheet.Cells[3, 3].Value = "Nombre y Apellidos";
                    workSheet.Cells[3, 4].Value = "Local";
                    workSheet.Cells[3, 5].Value = "Fecha";
                    workSheet.Cells[3, 6].Value = "Hora";
                    workSheet.Cells[3, 7].Value = "Estado";
                    //Body of table  
                    //  
                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach (var registro in lista)
                    {
                        //workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                        workSheet.Cells[recordIndex, 2].Value = registro.emp_id;
                        workSheet.Cells[recordIndex, 3].Value = registro.emp_nombre + " " + registro.emp_ape_paterno + " " + registro.emp_ape_materno;
                        workSheet.Cells[recordIndex, 4].Value = registro.local;
                        workSheet.Cells[recordIndex, 5].Value = registro.ema_fecha.ToString("dd-MM-yyyy");
                        workSheet.Cells[recordIndex, 6].Value = registro.ema_fecha.ToString("HH:mm tt");
                        var estado = "Asignado";
                        if (registro.ema_asignado != 1)
                        {
                            estado = "No Asignado";
                        }
                        workSheet.Cells[recordIndex, 7].Value = estado;
                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:G3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:G3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:G3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:G3"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B3:G3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:G3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:G3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:G3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B3:G3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:G3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:G3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:G3"].Style.Border.Bottom.Color.SetColor(colborder);

                    //workSheet.Cells.AutoFitColumns(60, 250);//overloaded min and max lengths
                    workSheet.Cells["B3:B" + total + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["G3:G" + total + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    int filaFooter = total + 4;
                    workSheet.Cells["B" + filaFooter + ":G" + filaFooter].Merge = true;
                    workSheet.Cells["B" + filaFooter + ":G" + filaFooter].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter + ":G" + filaFooter].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter + ":G" + filaFooter].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter + ":G" + filaFooter].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter + ":G" + filaFooter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[filaFooter, 2].Value = "Total : " + total + " Registros";

                    int filaultima = total + 3;
                    //workSheet.Cells["C1:C" + (total + 1)].AutoFilter = true;
                    //workSheet.Cells["D1:D" + (total + 1)].AutoFilter = true;
                    workSheet.Cells[3, 2, filaultima, 7].AutoFilter = true;

                    //workSheet.Column(1).AutoFit();
                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 30;
                    workSheet.Column(4).Width = 30;
                    workSheet.Column(5).Width = 15;
                    workSheet.Column(6).Width = 14;
                    workSheet.Column(7).Width = 20;

                    excelName = DateTime.Today.ToString("dd_MM_yyyy") + "_reporte_asistencia.xlsx";

                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());

                    mensaje = "Descargando Archivo";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
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
