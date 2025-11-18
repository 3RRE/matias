using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaDatos.ControlAcceso;
using CapaNegocio.ControlAcceso;
using CapaEntidad.ControlAcceso;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml;
using System.IO;

namespace CapaPresentacion.Controllers.ControlAcceso
{
    [seguridad]
    public class CALEntidadPublicaController : Controller
    {

        private CAL_EntidadPublicaBL entidadPublicaBL = new CAL_EntidadPublicaBL();
        private CAL_PersonaEntidadPublicaBL personaEntidadPublicaBL = new CAL_PersonaEntidadPublicaBL();

        public ActionResult ListadoEntidadPublica()
        {
            return View("~/Views/ControlAcceso/ListadoEntidadPublica.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarEntidadPublicaJson()
        {
            var errormensaje = "";
            var lista = new List<CAL_EntidadPublicaEntidad>();

            try
            {

                lista = entidadPublicaBL.EntidadPublicaListadoCompletoJson();

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarEntidadPublicaIdJson(int EntidadPublicaID)
        {
            var errormensaje = "";
            CAL_EntidadPublicaEntidad item = new CAL_EntidadPublicaEntidad();

            try
            {

                item = entidadPublicaBL.EntidadPublicaIdObtenerJson(EntidadPublicaID);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = item, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EntidadPublicaEditarJson(CAL_EntidadPublicaEntidad entidadPublica)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                entidadPublica.FechaModificacion = DateTime.Now;
                respuestaConsulta = entidadPublicaBL.EntidadPublicaEditarJson(entidadPublica);

                if (respuestaConsulta)
                {

                    errormensaje = "Registro de Entidad Publica Actualizado Correctamente";
                }
                else
                {
                    errormensaje = "Error al Actualizar Entidad Publica , LLame Administrador";
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
        public ActionResult EntidadPublicaGuardarJson(CAL_EntidadPublicaEntidad entidadPublica)
        {
            var errormensaje = "";
            Int64 respuestaConsulta = 0;
            bool respuesta = false;
            try
            {
                entidadPublica.FechaRegistro = DateTime.Now;
                respuestaConsulta = entidadPublicaBL.EntidadPublicaInsertarJson(entidadPublica);

                if (respuestaConsulta > 0)
                {
                    respuesta = true;
                    errormensaje = "Registro Entidad Publica Guardado Correctamente";
                }
                else
                {
                    errormensaje = "Error al crear la Entidad Publica , LLame Administrador";
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
        public ActionResult EntidadPublicaEliminarJson(int id)
        {
            var errormensaje = "";
            bool respuesta = false;

            List<CAL_PersonaEntidadPublicaEntidad> lista = personaEntidadPublicaBL.PersonaEntidadPublicaListadoCompletoJson();

            foreach (var item in lista)
            {
                if (item.EntidadPublicaID == id)
                {
                    errormensaje = "La entidad publica se encuentra actualmente asignada.";
                    respuesta = false;
                    return Json(new { respuesta = respuesta, mensaje = errormensaje });
                }
            }
            try
            {
                respuesta = entidadPublicaBL.EntidadPublicaEliminarJson(id);
                if (respuesta)
                {
                    respuesta = true;
                    errormensaje = "Se quitó el Entidad Publica Correctamente";
                }
                else
                {
                    errormensaje = "error al Quitar el Entidad Publica , LLame Administrador";
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
        public ActionResult EntidadPublicaDescargarExcelJson()
        {
            string fecha = DateTime.Now.ToString("dd_MM_yyyy");
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<CAL_EntidadPublicaEntidad> lista = new List<CAL_EntidadPublicaEntidad>();
            var strElementos = String.Empty;
            var strElementos_ = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;
            try
            {


                lista = entidadPublicaBL.EntidadPublicaListadoCompletoJson();
                if (lista.Count > 0)
                {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Entidad Publica");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Nombre";
                    workSheet.Cells[3, 4].Value = "Estado";
                    workSheet.Cells[3, 5].Value = "Fecha Registro";
                    workSheet.Cells[3, 6].Value = "Fecha Modificacion";

                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach (var registro in lista)
                    {

                        workSheet.Cells[recordIndex, 2].Value = registro.EntidadPublicaID;
                        workSheet.Cells[recordIndex, 3].Value = registro.Nombre;
                        workSheet.Cells[recordIndex, 4].Value = registro.Estado == 1 ? "Activo" : "Inactivo";
                        workSheet.Cells[recordIndex, 5].Value = registro.FechaRegistro.ToString("dd-MM-yyyy hh:mm:ss tt");
                        workSheet.Cells[recordIndex, 6].Value = registro.FechaModificacion==null?"NULL": registro.FechaModificacion.ToString("dd-MM-yyyy hh:mm:ss tt");

                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:F3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:F3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:F3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:F3"].Style.Font.Color.SetColor(Color.White);
                                        
                    workSheet.Cells["B3:F3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:F3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:F3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:F3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                        
                    workSheet.Cells["B3:F3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:F3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:F3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:F3"].Style.Border.Bottom.Color.SetColor(colborder);

                    int filasagregadas = 3;
                    total = filasagregadas + total;

                    workSheet.Cells["B4:B" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    /*
                    workSheet.Cells["B2:E2"].Merge = true;
                    workSheet.Cells["B2:E2"].Style.Font.Bold = true;
                    */

                    int filaFooter = total + 1;
                    workSheet.Cells["B" + filaFooter + ":F" + filaFooter].Merge = true;
                    workSheet.Cells["B" + filaFooter + ":F" + filaFooter].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter + ":F" + filaFooter].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter + ":F" + filaFooter].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter + ":F" + filaFooter].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter + ":F" + filaFooter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells["B" + filaFooter + ":F" + filaFooter].Style.Font.Size = 14;
                    workSheet.Cells[filaFooter, 2].Value = "Total : " + (total - filasagregadas) + " Registros";
                    workSheet.Cells[filaFooter, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["B4:F" + total].Style.WrapText = true;

                    int filaultima = total;
                    workSheet.Cells[3, 2, filaultima, 6].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 44;
                    workSheet.Column(4).Width = 18;
                    workSheet.Column(5).Width = 30;
                    workSheet.Column(6).Width = 30;
                    excelName = "EntidadPublica_" + fecha + ".xlsx";
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