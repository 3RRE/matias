using CapaDatos.EntradaSalidaSala;
using CapaEntidad.EntradaSalidaSala;
using CapaNegocio;
using CapaNegocio.EntradaSalidaSala;
using Newtonsoft.Json;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Drawing;
namespace CapaPresentacion.Controllers.EntradaSalidaSala
{
    public class VisitasSupervisorController : Controller
    {
        ESS_VisitasSupervisorBL ess__visitasSupervisorBL = new ESS_VisitasSupervisorBL();
        private readonly SalaBL _salaBl = new SalaBL();

        public ActionResult VisitasSupervisorVista() {
            return View("~/Views/EntradaSalidaSala/VisitasSupervisor.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult VisitasSupervisorListarxSalaFechaJson(int[] codsala, DateTime fechaini, DateTime fechafin) {
            string mensaje = "No se encontraton registros";
            bool respuesta = false;
            List<ESS_VisitasSupervisorEntidad> listaESS_VisitasSupervisor = new List<ESS_VisitasSupervisorEntidad>();
            try {
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                int[] salasBusqueda;
                if(codsala.Contains(-1)) {
                    salasBusqueda = salas.Select(x => x.CodSala).ToArray();
                } else {
                    salasBusqueda = codsala;
                }
                listaESS_VisitasSupervisor = ess__visitasSupervisorBL.ListadoVisitasSupervisor(salasBusqueda, fechaini, fechafin);
                mensaje = "Registros obtenidos";
                respuesta = true;
           
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, comunicarse con administrador";
            }
            var oRespuesta = new {
                mensaje,
                respuesta,
                data = listaESS_VisitasSupervisor
            };
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var result = new ContentResult {
                Content = serializer.Serialize(oRespuesta),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public JsonResult EliminarVisitasSupervisor(int id) {
            bool respuesta = false;
            string mensaje = "No se pudo eliminar el registro.";

            try {
                respuesta = ess__visitasSupervisorBL.EliminarVisitasSupervisor(id);
                if(respuesta) {
                    mensaje = "Registro eliminado correctamente.";
                }
            } catch(Exception ex) {
                mensaje = "Error al eliminar el registro: " + ex.Message;
            }

            return Json(new { respuesta, mensaje });
        }

        [HttpPost]
        public ActionResult EditarVisitasSupervisor(ESS_VisitasSupervisorEntidad visitasSupervisor, string Empleados) {
            bool respuesta = false;
            string mensaje = "No se pudo actualizar los datos";

            try {
                List<ESS_VisitaSupervisorDetalleEntidad> listaEmpleados = JsonConvert.DeserializeObject<List<ESS_VisitaSupervisorDetalleEntidad>>(Empleados);
                visitasSupervisor.Empleados = listaEmpleados;
                visitasSupervisor.FechaModificacion = DateTime.Now;
                visitasSupervisor.UsuarioModificacion = (string)Session["UsuarioNombre"];

                respuesta = ess__visitasSupervisorBL.EditarVisitasSupervisor(visitasSupervisor);
                if(respuesta) {
                    respuesta = true;
                    mensaje = "Los datos se han actualizado";
                }
            } catch(Exception exception) {
                mensaje = "Ha ocurrido un error, comunicarse con administrador";
            }

            return Json(new {
                respuesta,
                mensaje
            });
        }

        [HttpPost]
        public ActionResult GuardarVisitasSupervisor(ESS_VisitasSupervisorEntidad visitasSupervisor, string Empleados) {
            bool respuesta = false;

            string mensaje = "No se pudo guardar los datos";

            try {
                List<ESS_VisitaSupervisorDetalleEntidad> listaEmpleados = JsonConvert.DeserializeObject<List<ESS_VisitaSupervisorDetalleEntidad>>(Empleados);

                visitasSupervisor.Empleados = listaEmpleados;
              
                visitasSupervisor.FechaRegistro = DateTime.Now;
                visitasSupervisor.UsuarioRegistro = (string)Session["UsuarioNombre"];

                int insertedId = ess__visitasSupervisorBL.GuardarVisitasSupervisor(visitasSupervisor);
                if(insertedId > 0) {
                    respuesta = true;
                    mensaje = "Los datos se han guardado";
                }
            } catch(Exception exception) {
                mensaje = "Ha ocurrido un error, comunicarse con administrador";
            }
            return Json(new {
                respuesta,
                mensaje
            });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarMotivoPorEstado(int estado = 1) {
            string mensaje = string.Empty;
            bool respuesta = false;
            var data = new List<ESS_VisitaSupervisorMotivoEntidad>();
            try {

                data = ess__visitasSupervisorBL.ListarMotivoPorEstado(estado);
                mensaje = "Listando registros";
                respuesta = true;
                return Json(new { mensaje, respuesta, data });
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, contacte con administrador";
                respuesta = false;
                return Json(new { mensaje, respuesta });
            }
        }

        [HttpPost]
        public ActionResult ReporteVisitasSupervisorDescargarExcelJson(int[] codsala, DateTime fechaini, DateTime fechafin) {
            string mensaje = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;

            var usuarioId = (int)Session["UsuarioID"];
            var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
            int[] salasBusqueda;
            if(codsala.Contains(-1)) {
                salasBusqueda = salas.Select(x => x.CodSala).ToArray();
            } else {
                salasBusqueda = codsala;
            }

            List<ESS_VisitasSupervisorEntidad> lista = ess__visitasSupervisorBL.ListadoVisitasSupervisor(salasBusqueda, fechaini, fechafin);


            lista = lista.OrderByDescending(x => x.FechaRegistro).ToList();

            try {
                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();


                    var workSheet = excel.Workbook.Worksheets.Add("Visitas Supervisor");
                    workSheet.TabColor = Color.Black;
                    workSheet.DefaultRowHeight = 9;


                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[2, 2, 2, 9].Merge = true;
                    workSheet.Cells[2, 2].Value = "Reporte de Visitas Supervisor";
                    workSheet.Cells[2, 2].Style.Font.Bold = true;
                    workSheet.Cells[2, 2].Style.Font.Size = 13;
                    workSheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Sala";
                    workSheet.Cells[3, 4].Value = "Fecha Registro";
                    workSheet.Cells[3, 5].Value = "Motivo";
                    workSheet.Cells[3, 6].Value = "Fecha Ingreso";
                    workSheet.Cells[3, 7].Value = "Fecha Salida";
                    workSheet.Cells[3, 8].Value = "Observaciones";
                    workSheet.Cells[3, 9].Value = "Supervisa";

                    int recordIndex = 4;
                    foreach(var item in lista) {
                        workSheet.Cells[recordIndex, 2].Value = item.IdVisitaSupervisor;
                        workSheet.Cells[recordIndex, 3].Value = item.NombreSala;
                        workSheet.Cells[recordIndex, 4].Value = item.FechaRegistro.ToString("dd-MM-yyyy hh:mm tt");
                        workSheet.Column(5).Width = 30;
                        workSheet.Cells[recordIndex, 6].Value = item.FechaIngreso.ToString("dd-MM-yyyy hh:mm tt");
                        workSheet.Cells[recordIndex, 7].Value = item.FechaSalida.ToString("dd-MM-yyyy") == "01-01-1753" ? "No Definido" : item.FechaSalida.ToString("dd-MM-yyyy hh:mm tt");
                        workSheet.Cells[recordIndex, 8].Value = string.IsNullOrEmpty(item.Observaciones) ? "No definido" : item.Observaciones;
                        workSheet.Column(8).Width = 30;

                        if(item.IdMotivo == -1) {
                            workSheet.Cells[recordIndex, 5].Value = $"Otro: {item.OtroMotivo}";
                        } else {
                            workSheet.Cells[recordIndex, 5].Value = item.Nombre;
                        }

                        if(item.Empleados != null && item.Empleados.Any()) {
                            var employeeSheet = excel.Workbook.Worksheets.Add($"Detalle_VisitasSupervisa_{item.IdVisitaSupervisor}");
                            employeeSheet.Cells[1, 2, 1, 8].Merge = true;
                            employeeSheet.Cells[1, 2].Value = $"Detalle: Persona Visitas Supervisa N°: {item.IdVisitaSupervisor}";
                            employeeSheet.Cells[1, 2].Style.Font.Bold = true;
                            employeeSheet.Cells[1, 2].Style.Font.Size = 13;
                            employeeSheet.Cells[1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            employeeSheet.Cells[3, 2].Value = "ID";
                            employeeSheet.Cells[3, 3].Value = "Nombre";
                            employeeSheet.Cells[3, 4].Value = "Cargo";
                            employeeSheet.Cells[3, 5].Value = "Tipo Documento";
                            employeeSheet.Cells[3, 6].Value = "Documento Registro";

                            Color headerBackground = ColorTranslator.FromHtml("#003268");
                            employeeSheet.Cells["B3:F3"].Style.Font.Bold = true;
                            employeeSheet.Cells["B3:F3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            employeeSheet.Cells["B3:F3"].Style.Fill.BackgroundColor.SetColor(headerBackground);
                            employeeSheet.Cells["B3:F3"].Style.Font.Color.SetColor(Color.White);
                            employeeSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            int employeeIndex = 4;
                            foreach(var empleado in item.Empleados) {
                                employeeSheet.Cells[employeeIndex, 2].Value = empleado.IdEmpleado;
                                employeeSheet.Cells[employeeIndex, 3].Value = empleado.NombreCompleto;
                                employeeSheet.Cells[employeeIndex, 4].Value = empleado.Cargo;
                                employeeSheet.Cells[employeeIndex, 5].Value = empleado.TipoDocumento.ToUpper();
                                employeeSheet.Cells[employeeIndex, 6].Value = empleado.DocumentoRegistro;
                                employeeIndex++;
                            }

                            for(int i = 2; i <= 6; i++) {
                                employeeSheet.Column(i).AutoFit();
                            }

                            var hyperlink = new ExcelHyperLink($"#'Detalle_VisitasSupervisa_{item.IdVisitaSupervisor}'!A1", "Ver Detalles");
                            workSheet.Cells[recordIndex, 9].Hyperlink = hyperlink;
                            workSheet.Cells[recordIndex, 9].Style.Font.UnderLine = true;
                            workSheet.Cells[recordIndex, 9].Style.Font.Color.SetColor(Color.Blue);
                        } else {
                            workSheet.Cells[recordIndex, 9].Value = "No hay visitas de supervisor";
                        }

                        recordIndex++;
                    }

                    Color principalHeaderBackground = ColorTranslator.FromHtml("#003268");
                    workSheet.Cells["B3:I3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:I3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:I3"].Style.Fill.BackgroundColor.SetColor(principalHeaderBackground);
                    workSheet.Cells["B3:I3"].Style.Font.Color.SetColor(Color.White);

                    for(int i = 2; i <= 9; i++) {
                        if(i != 5 && i != 8) {
                            workSheet.Column(i).AutoFit();
                        }
                    }

                    // Nombre del archivo
                    excelName = $"VisitasSupervisa_{fechaini:dd_MM_yyyy}_al_{fechafin:dd_MM_yyyy}.xlsx";

                    using(var memoryStream = new MemoryStream()) {
                        excel.SaveAs(memoryStream);
                        base64String = Convert.ToBase64String(memoryStream.ToArray());
                    }

                    mensaje = "Archivo generado correctamente";
                    respuesta = true;
                } else {
                    mensaje = "No se encontraron datos para el reporte.";
                }
            } catch(Exception ex) {
                mensaje = $"Error al generar el archivo: {ex.Message}";
                respuesta = false;
            }

            return Json(new { data = base64String, excelName, respuesta, mensaje });
        }

    }
}
