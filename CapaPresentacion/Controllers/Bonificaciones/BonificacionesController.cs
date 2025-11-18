using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CapaPresentacion.Controllers.Bonificaciones
{
    [seguridad]
    public class BonificacionesController : Controller
    {
        private BonificacionesBL bonificacionesBL = new BonificacionesBL();
        private SalaBL salaBl = new SalaBL();
        public ActionResult FormularioRegistro()
        {
            return View("~/Views/Bonificaciones/FormularioRegistroVista.cshtml");
        }

        [HttpPost]
        public JsonResult BuscarTicketSolicitud(string nrodocumento,string nroticket)
        {
            var errormensaje = "";
            var lista = new List<BonificacionesEntidad>();        
            double totalmonto = 0;
            bool respuesta = false;
            try
            {
                lista = bonificacionesBL.BonificacionesBuscarJson(nrodocumento, nroticket);
                totalmonto = lista.Select(x => x.bon_monto).Sum();
                if (lista.Count <= 0)
                {
                    respuesta = false;
                    errormensaje = "No se encontro Registro";
                }
                else
                {
                   
                    respuesta = true;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), total = totalmonto,respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CobrarTicket(BonificacionesEntidad bonificaciones)
        {
            var errormensaje = "";
            var response = false;
            try
            {
                bonificaciones.bon_fecharegistro = DateTime.Now;
                bonificaciones.bon_estado = 1;
                bonificaciones.UsuarioID= Convert.ToInt32(Session["UsuarioID"]);
                bonificaciones.CodSala = bonificaciones.CodSala;
                response = bonificacionesBL.BonificacionesActualizarJson(bonificaciones);
                if (!response)
                {
                    errormensaje = "Error al Registrar Pago";
                }
                else
                {
                    errormensaje = "Pago Registrado";
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = response, mensaje = errormensaje });
        }

        public ActionResult ReporteFormularioRegistro()
        {
            return View("~/Views/Bonificaciones/ReporteFormularioRegistroVista.cshtml");
        }

        [HttpPost]
        public JsonResult reporteFormularioRegistroJson(DateTime fechaini, DateTime fechafin, int[] codsala)
        {
            var errormensaje = "";
            var lista = new List<BonificacionesEntidad>();
            double totalmonto = 0;
            try
            {
                int cantElementos = (codsala == null) ? 0 : codsala.Length;
                var strElementos = String.Empty;
                if (cantElementos > 0)
                {
                    strElementos = " b.[CodSala] in(" + "'" + String.Join("','", codsala) + "'" + ") and ";
                }
                lista = bonificacionesBL.BonificacionesListarJson(fechaini, fechafin, strElementos);
                totalmonto = lista.Select(x => x.bon_monto).Sum();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), total = totalmonto, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult reporteFormularioRegistroDescargarExcelJson(int[] CodSala, DateTime fechaini, DateTime fechafin)
        {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<BonificacionesEntidad> lista = new List<BonificacionesEntidad>();
            int cantElementos = (CodSala == null) ? 0 : CodSala.Length;
            var strElementos = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;
          
            var strUsuario = String.Empty;
            try
            {


                if (cantElementos > 0)
                {
                    for (int i = 0; i < CodSala.Length; i++)
                    {
                        var salat = salaBl.SalaListaIdJson(CodSala[i]);
                        nombresala.Add(salat.Nombre);
                    }
                    salasSeleccionadas = String.Join(",", nombresala);

                    strElementos = " b.[CodSala] in(" + "'" + String.Join("','", CodSala) + "'" + ") and ";
                }

                lista = bonificacionesBL.BonificacionesListarJson(fechaini, fechafin, strElementos);
                if (lista.Count > 0)
                {
                    double totalmonto = lista.Select(x => x.bon_monto).Sum();
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Bonificaciones");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[2, 2].Value = "SALAS : " + salasSeleccionadas;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Fecha";
                    workSheet.Cells[3, 4].Value = "Sede";
                    workSheet.Cells[3, 5].Value = "Nro. Documento";
                    workSheet.Cells[3, 6].Value = "Empleado";
                    workSheet.Cells[3, 7].Value = "Nro. Ticket";
                    workSheet.Cells[3, 8].Value = "Monto";
                    workSheet.Cells[3, 9].Value = "Fecha Pago";           
                    workSheet.Cells[3, 10].Value = "Usu. Pago";
                    workSheet.Cells[3, 11].Value = "Estado";
                    //Body of table  
                    //  
                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach (var registro in lista)
                    {

                        workSheet.Cells[recordIndex, 2].Value = registro.bon_id;
                        workSheet.Cells[recordIndex, 3].Value = registro.bon_fecha.ToString("dd-MM-yyyy");
                        workSheet.Cells[recordIndex, 4].Value = registro.nombresala;
                        workSheet.Cells[recordIndex, 5].Value = registro.bon_documento;
                        workSheet.Cells[recordIndex, 6].Value = registro.bon_apepaterno+" "+registro.bon_apematerno+" "+registro.bon_nombre;
                        workSheet.Cells[recordIndex, 7].Value = registro.bon_ticket;
                        workSheet.Cells[recordIndex, 8].Value = registro.bon_monto;
                        workSheet.Cells[recordIndex, 9].Value = registro.bon_fecharegistro.ToString("dd-MM-yyyy hh:mm:ss");
                        workSheet.Cells[recordIndex, 10].Value = registro.nombreusuario;
                        var estado = "Pendiente";
                        if (registro.bon_estado == 1)
                        {
                            estado = "Pagado";
                        }
                        workSheet.Cells[recordIndex, 11].Value = estado;

                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:K3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:K3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:K3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:K3"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B3:K3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:K3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:K3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:K3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B3:K3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:K3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:K3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:K3"].Style.Border.Bottom.Color.SetColor(colborder);

                    //workSheet.Cells.AutoFitColumns(60, 250);//overloaded min and max lengths
                    int filasagregadas = 3;
                    total = filasagregadas + total;

                    workSheet.Cells["B4:B" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["E4:E" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["K4:K" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["G4:G" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["H4:H" + total].Style.Numberformat.Format = "#,##0.00";

                    workSheet.Cells["B2:K2"].Merge = true;
                    workSheet.Cells["B2:K2"].Style.Font.Bold = true;

                    int filaFooter = total + 1;
                    workSheet.Cells["B" + filaFooter + ":G" + filaFooter].Merge = true;
                    workSheet.Cells["B" + filaFooter + ":K" + filaFooter].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter + ":K" + filaFooter].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter + ":K" + filaFooter].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter + ":K" + filaFooter].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter + ":F" + filaFooter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells[filaFooter, 2].Value = "Total Monto : ";
                    workSheet.Cells[filaFooter, 8].Value = totalmonto;
                    workSheet.Cells[filaFooter, 8].Style.Numberformat.Format = "#,##0.00";
                    workSheet.Cells["B" + filaFooter + ":K" + filaFooter].Style.Font.Size = 14;


                    int filaFooter_ = total + 2;
                    workSheet.Cells["B" + filaFooter_ + ":K" + filaFooter_].Merge = true;
                    workSheet.Cells["B" + filaFooter_ + ":K" + filaFooter_].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter_ + ":K" + filaFooter_].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter_ + ":K" + filaFooter_].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter_ + ":K" + filaFooter_].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter_ + ":K" + filaFooter_].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[filaFooter_, 2].Value = "Total : " + (total - filasagregadas) + " Registros";

                    int filaultima = total;
                    workSheet.Cells[3, 2, filaultima, 9].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 20;
                    workSheet.Column(4).Width = 25;
                    workSheet.Column(5).Width = 25;
                    workSheet.Column(6).Width = 32;
                    workSheet.Column(7).Width = 28;
                    workSheet.Column(8).Width = 25;
                    workSheet.Column(9).Width = 27;
                    workSheet.Column(10).Width = 25;
                    workSheet.Column(11).Width = 24;
                    excelName = "ReporteBonificaciones_" + fechaini.ToString("dd_MM_yyyy") + "_al_" + fechafin.ToString("dd_MM_yyyy") + "_.xlsx";
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