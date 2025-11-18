using CapaEntidad.ControlAcceso;
using CapaEntidad;
using ImageResizer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaNegocio.ControlAcceso;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;

namespace CapaPresentacion.Controllers.ControlAcceso
{
    [seguridad]
    public class CALMenorDeEdadController : Controller
    {
        private CAL_MenorDeEdadBL menorEdadBL = new CAL_MenorDeEdadBL();

        public ActionResult Index()
        {
            return View("~/Views/ControlAcceso/ListaRegistrosMenorDeEdad.cshtml");
        }
        [HttpPost]
        public ActionResult listarMenoresDeEdad()
        {
            bool respuesta = false;
            var errormensaje = "";
            var lista = new List<CAL_MenorDeEdadEntidad>();

            try
            {
                lista = menorEdadBL.ListarMenorEdad();
                respuesta = true;
            } catch(Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
                respuesta = false;
            }
            return Json(new { data = lista.ToList(), respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult MenorDeEdadGuardarJson(CAL_MenorDeEdadEntidad menorEdadControlAcceso)
        {
            var errormensaje = "";
            int IdInsertado = 0;
            bool respuesta = false;
            var empleado = new SEG_EmpleadoEntidad();
            empleado = (SEG_EmpleadoEntidad)Session["empleado"];
            menorEdadControlAcceso.EmpleadoID = empleado.EmpleadoID;

            try
            {
                menorEdadControlAcceso.FechaRegistro = DateTime.Now;
                var menorEdadBusqueda = menorEdadBL.GetMenorEdadPorDNI(menorEdadControlAcceso.Doi);
                if(menorEdadBusqueda.IdMenor != menorEdadControlAcceso.IdMenor)
                {
                    errormensaje = "El numero de DNI ya existe";
                    return Json(new { respuesta = false, mensaje = errormensaje });
                }

                IdInsertado = menorEdadBL.InsertarMenorEdad(menorEdadControlAcceso);
                if(IdInsertado > 0)
                {
                    menorEdadControlAcceso.IdMenor = IdInsertado;
                    respuesta = true;
                    errormensaje = "Registro Menor de edad Guardado Correctamente";
                } else
                {
                    errormensaje = "Error al crear el registro de Menor de edad , LLame Administrador";
                    respuesta = false;
                }
            } catch(Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje });
        }


        public ActionResult EditarRegistroMenorEdad(CAL_MenorDeEdadEntidad registro)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            var empleado = new SEG_EmpleadoEntidad();
            empleado = (SEG_EmpleadoEntidad)Session["empleado"];
            registro.EmpleadoID = empleado.EmpleadoID;
            CAL_MenorDeEdadEntidad registroMenorEdad = new CAL_MenorDeEdadEntidad();
            try
            {


                registroMenorEdad = menorEdadBL.GetMenorEdadPorDNI(registro.Doi);
                if(registroMenorEdad.IdMenor != 0)
                {
                    if(registroMenorEdad.IdMenor != registro.IdMenor)
                    {
                        errormensaje = "El numero de DNI ya existe";
                        respuestaConsulta = false;
                        return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
                    }
                }
               


                respuestaConsulta = menorEdadBL.EditarMenorEdad(registro);

                if(respuestaConsulta)
                {

                    errormensaje = "Registro de menor de edad Actualizado Correctamente";
                } else
                {
                    errormensaje = "Error al Actualizar registro de menor de edad , LLame Administrador";
                    respuestaConsulta = false;
                }

            } catch(Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }


        public JsonResult ListarMenrEdadIdJson(int id)
        {
            bool respuesta = false;
            var errormensaje = "";
            CAL_MenorDeEdadEntidad item = new CAL_MenorDeEdadEntidad();

            try
            {

                item = menorEdadBL.GetMenorEdadPorId(id);
                respuesta = true;

            } catch(Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
                respuesta = false;
            }
            return Json(new { data = item, respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult RegistroMenorDeEdadExcel()
        {
            string fecha = DateTime.Now.ToString("dd_MM_yyyy");
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<CAL_MenorDeEdadEntidad> lista = new List<CAL_MenorDeEdadEntidad>();
            var strElementos = String.Empty;
            var strElementos_ = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;
            try
            {


                lista = menorEdadBL.ListarMenorEdad();
                if(lista.Count > 0)
                {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Menores de edad");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Nombre";
                    workSheet.Cells[3, 4].Value = "Apellido Paterno";
                    workSheet.Cells[3, 5].Value = "Apellido Materno";
                    workSheet.Cells[3, 6].Value = "Documento";
                    workSheet.Cells[3, 7].Value = "Sala";
                    workSheet.Cells[3, 8].Value = "Fecha de registro";
                    workSheet.Cells[3, 9].Value = "Estado";
                    workSheet.Cells[3, 10].Value = "Tipo de documento";

                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach(var registro in lista)
                    {

                        workSheet.Cells[recordIndex, 2].Value = registro.IdMenor;
                        workSheet.Cells[recordIndex, 3].Value = registro.Nombre.ToUpper();
                        workSheet.Cells[recordIndex, 4].Value = registro.ApellidoPaterno.ToUpper();
                        workSheet.Cells[recordIndex, 5].Value = registro.ApellidoMaterno.ToUpper();
                        workSheet.Cells[recordIndex, 6].Value = registro.Doi.ToUpper();
                        workSheet.Cells[recordIndex, 7].Value = registro.NombreSala.ToUpper();
                        workSheet.Cells[recordIndex, 8].Value = registro.FechaRegistro.ToString("dd-MM-yyyy hh:mm:ss tt");
                        workSheet.Cells[recordIndex, 9].Value = registro.Estado == 1 ? "ACTIVO" : "INACTIVO";
                        workSheet.Cells[recordIndex, 10].Value = registro.NombreTipoDoi;

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

                    workSheet.Cells["B4:I" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

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
                    workSheet.Cells["B4:I" + total].Style.WrapText = true;

                    int filaultima = total;
                    workSheet.Cells[3, 2, filaultima, 9].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 40;
                    workSheet.Column(4).Width = 30;
                    workSheet.Column(5).Width = 40;
                    workSheet.Column(6).Width = 30;
                    workSheet.Column(7).Width = 40;
                    workSheet.Column(8).Width = 28;
                    workSheet.Column(9).Width = 30;
                    workSheet.Column(10).Width = 40;
                    excelName = "Reporte menores de edad_" + fecha + ".xlsx";
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());

                    mensaje = "Descargando Archivo";
                    respuesta = true;
                } else
                {
                    mensaje = "No se Pudo generar Archivo";
                }

            } catch(Exception exp)
            {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = base64String, excelName, respuesta, mensaje, mensajeConsola });

        }
    }
}