using CapaEntidad.ControlAcceso;
using CapaEntidad.ControlAcceso.Filtro;
using CapaNegocio;
using CapaNegocio.ControlAcceso;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CapaPresentacion.Controllers.ControlAcceso {
    [seguridad]
    public class CALAuditoriaController : Controller {
        private readonly CAL_AuditoriaBL auditoriaBL;
        private readonly SalaBL salaBL;

        public CALAuditoriaController() {
            auditoriaBL = new CAL_AuditoriaBL();
            salaBL = new SalaBL();
        }

        public ActionResult ListadoAuditoria() {
            return View("~/Views/ControlAcceso/ListadoAuditoria.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarAuditoriaJson() {
            var errormensaje = "";
            var lista = new List<CAL_AuditoriaEntidad>();

            try {
                lista = auditoriaBL.AuditoriaListadoCompletoJson();
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AuditoriaDescargarExcelJson() {
            string fecha = DateTime.Now.ToString("dd_MM_yyyy");
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<CAL_AuditoriaEntidad> lista = new List<CAL_AuditoriaEntidad>();
            var strElementos = String.Empty;
            var strElementos_ = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;

            try {
                lista = auditoriaBL.AuditoriaListadoCompletoJson();
                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Auditoria");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Usuario";
                    workSheet.Cells[3, 4].Value = "DNI";
                    workSheet.Cells[3, 5].Value = "Tipo";
                    workSheet.Cells[3, 6].Value = "Nombre";
                    workSheet.Cells[3, 7].Value = "Sala";
                    workSheet.Cells[3, 8].Value = "Fecha";

                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach(var registro in lista) {

                        workSheet.Cells[recordIndex, 2].Value = registro.idAuditoria;
                        workSheet.Cells[recordIndex, 3].Value = registro.usuario;
                        workSheet.Cells[recordIndex, 4].Value = registro.dni.ToUpper().Trim() == "" ? "--" : registro.dni.Trim().ToUpper(); ;
                        workSheet.Cells[recordIndex, 5].Value = registro.tipo.ToUpper().Trim() == "" ? "--" : registro.tipo.Trim().ToUpper(); ;
                        workSheet.Cells[recordIndex, 6].Value = registro.nombre.ToUpper().Trim() == "" ? "--" : registro.nombre.Trim().ToUpper(); ;
                        workSheet.Cells[recordIndex, 7].Value = registro.sala.ToUpper().Trim() == "" ? "--" : registro.sala.Trim().ToUpper(); ;
                        workSheet.Cells[recordIndex, 8].Value = registro.fecha.ToString("dd-MM-yyyy hh:mm:ss tt"); ;

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

                    workSheet.Cells["B4:B" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

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
                    workSheet.Column(3).Width = 24;
                    workSheet.Column(4).Width = 24;
                    workSheet.Column(5).Width = 24;
                    workSheet.Column(6).Width = 24;
                    workSheet.Column(7).Width = 24;
                    workSheet.Column(8).Width = 32;
                    excelName = "Auditoria_" + fecha + ".xlsx";
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


        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarAuditoriaSalasJson(int[] ArraySalaId, DateTime fechaIni, DateTime fechaFin) {
            string mensaje = "";
            bool respuesta = false;
            int cantElementos = (ArraySalaId == null) ? 0 : ArraySalaId.Length;
            var strElementos = String.Empty;
            List<CAL_AuditoriaEntidad> lista = new List<CAL_AuditoriaEntidad>();

            try {

                //string strSalas =  String.Join(",", ArraySalaId);
                if(cantElementos > 0) {
                    strElementos = " sala.CodSala in(" + "'" + String.Join("','", ArraySalaId) + "'" + ") and ";
                }
                lista = auditoriaBL.GetAuditoriaSala(strElementos, fechaIni, fechaFin).OrderByDescending(x => x.fecha).ToList();

                mensaje = "Listando registros";
                respuesta = true;

            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            var oRespuesta = new {
                mensaje,
                respuesta,
                data = lista
            };
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var result = new ContentResult {
                Content = serializer.Serialize(oRespuesta),
                ContentType = "application/json"
            };
            return result;


            //return Json(new { mensaje, respuesta, data = lista});
        }


        [HttpPost]
        public ActionResult AuditoriaSalasDescargarExcelJson(int[] ArraySalaId, DateTime fechaIni, DateTime fechaFin) {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<CAL_AuditoriaEntidad> lista = new List<CAL_AuditoriaEntidad>();
            int cantElementos = (ArraySalaId == null) ? 0 : ArraySalaId.Length;
            var strElementos = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;
            try {


                if(cantElementos > 0) {
                    for(int i = 0; i < ArraySalaId.Length; i++) {
                        var salat = salaBL.SalaListaIdJson(ArraySalaId[i]);
                        nombresala.Add(salat.Nombre);
                    }
                    salasSeleccionadas = String.Join(",", nombresala);
                    strElementos = " sala.CodSala in(" + "'" + String.Join("','", ArraySalaId) + "'" + ") and ";
                }
                lista = auditoriaBL.GetAuditoriaSala(strElementos, fechaIni, fechaFin);
                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("AuditoriaLudopatas");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Cells[2, 2].Value = "SALAS : " + salasSeleccionadas;

                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Usuario";
                    workSheet.Cells[3, 4].Value = "DNI";
                    workSheet.Cells[3, 5].Value = "Tipo";
                    workSheet.Cells[3, 6].Value = "Nombre";
                    workSheet.Cells[3, 7].Value = "Sala";
                    workSheet.Cells[3, 8].Value = "Observacion";
                    workSheet.Cells[3, 9].Value = "Fecha";
                    //Body of table  
                    int recordIndex = 4;
                    int total = lista.Count;

                    foreach(var registro in lista) {
                        workSheet.Cells[recordIndex, 2].Value = registro.idAuditoria;
                        workSheet.Cells[recordIndex, 3].Value = registro.usuarioNombre.ToUpper().Trim();
                        workSheet.Cells[recordIndex, 4].Value = registro.dni.ToUpper().Trim() == "" ? "--" : registro.dni.Trim().ToUpper(); ;
                        workSheet.Cells[recordIndex, 5].Value = registro.tipo.ToUpper().Trim() == "" ? "--" : registro.tipo.Trim().ToUpper(); ;
                        workSheet.Cells[recordIndex, 6].Value = registro.nombre.ToUpper().Trim() == "" ? "--" : registro.nombre.Trim().ToUpper();
                        workSheet.Cells[recordIndex, 7].Value = registro.sala.ToUpper().Trim() == "" ? "--" : registro.sala.Trim().ToUpper(); ;
                        workSheet.Cells[recordIndex, 8].Value = registro.observacion.ToUpper().Trim() == "" ? "--" : registro.observacion.Trim().ToUpper(); ;
                        workSheet.Cells[recordIndex, 9].Value = registro.fecha.ToString("dd-MM-yyyy hh:mm:ss tt");
                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:I3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:I3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:I3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:I3"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B3:I3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:I3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:I3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:I3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B3:I3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:I3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:I3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:I3"].Style.Border.Bottom.Color.SetColor(colborder);

                    int filaFooter_ = recordIndex;
                    workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Merge = true;
                    workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter_ + ":I" + filaFooter_].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[filaFooter_, 2].Value = "Total : " + (total) + " Registros";

                    workSheet.Cells[3, 2, filaFooter_, 9].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 30;
                    workSheet.Column(4).Width = 18;
                    workSheet.Column(5).Width = 35;
                    workSheet.Column(6).Width = 35;
                    workSheet.Column(7).Width = 15;
                    workSheet.Column(8).Width = 70;
                    workSheet.Column(9).Width = 33;

                    excelName = "AuditoriaLudopatas_del_" + fechaIni.ToString("dd_MM_yyyy") + "_al_" + fechaFin.ToString("dd_MM_yyyy") + "_.xlsx";
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());

                    mensaje = "Descargando Archivo";
                    respuesta = true;
                } else {
                    mensaje = "No se encontraron registros";
                }

            } catch(Exception exp) {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
            }

            JsonResult json = Json(new { data = base64String, excelName, respuesta });
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        #region Methods Migration DWH
        [HttpPost]
        [seguridad(false)]
        public JsonResult ObtenerIngresosClientesSalasParaDwh(CAL_IngresoClienteSalaDwhFiltro filtro) {
            List<CAL_AuditoriaEntidad> transacciones = new List<CAL_AuditoriaEntidad>();
            List<int> ids = new List<int>();
            string displayMessage = string.Empty;
            bool success = false;
            try {
                transacciones = auditoriaBL.ObtenerIngresosClientesSalasParaDwh(filtro);
                success = transacciones.Count > 0;
                displayMessage = success ? $"{transacciones.Count} ingresos de clientes a sala para migrar." : "No se encontraron registros de ingresos de clientes a sala para migrar.";
            } catch(Exception ex) {
                displayMessage = $"Error al obtener los ingresos de clientes a sala para migrar. {ex.Message}";
                success = false;
                auditoriaBL.ActualizarEstadoMigracionesDwh(ids, null);
            }

            JsonResult jsonResult = Json(new { success, displayMessage, data = transacciones });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        [seguridad(false)]
        public JsonResult MarcarComoMigrados(List<int> ids, DateTime fechaMigracionDwh) {
            string displayMessage = string.Empty;
            bool success = false;

            if(ids.Count == 0) {
                success = false;
                displayMessage = "Tiene que incluir al menos un id de ingreso de cliente a sala.";
                return Json(new { success, displayMessage });
            }

            try {
                auditoriaBL.ActualizarEstadoMigracionesDwh(ids, fechaMigracionDwh);
                success = true;
                displayMessage = $"{ids.Count} ingreso(s) de cliente(s) a sala marcada(s) como migrado a Data Warehouse.";
            } catch(Exception ex) {
                displayMessage = $"Error al intentar revertir los estados de migración a Data Warehouse. {ex.Message}";
                success = false;
            }

            return Json(new { success, displayMessage });
        }

        [HttpPost]
        [seguridad(false)]
        public JsonResult RevertirEstadoMigracion(List<int> ids) {
            string displayMessage = string.Empty;
            bool success = false;

            if(ids.Count == 0) {
                success = false;
                displayMessage = "Tiene que incluir al menos un id de ingreso de cliente a sala.";
                return Json(new { success, displayMessage });
            }

            try {
                auditoriaBL.ActualizarEstadoMigracionesDwh(ids, null);
                success = true;
                displayMessage = $"Estado de migración a Data Warehouse revertido correctamente.";
            } catch(Exception ex) {
                displayMessage = $"Error al intentar revertir los estados de migración a Data Warehouse. {ex.Message}";
                success = false;
            }

            return Json(new { success, displayMessage });
        }
        #endregion
    }
}