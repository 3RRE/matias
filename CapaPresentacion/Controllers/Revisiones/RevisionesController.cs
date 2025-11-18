using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilitarios;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using CHANO = System.IO;

namespace CapaPresentacion.Controllers.Revisiones
{
    public class RevisionesController : Controller
    {
        private LogAlertaBilleterosBL logBL = new LogAlertaBilleterosBL();

        // GET: Revisiones
        public ActionResult Index()
        {
            return View("~/Views/Revisiones/Index.cshtml");
        }

        public class Data
        {
            public int CodSala { get; set; }
            public string NombreSala { get; set; }
            public List<Revision> Registros { get; set; }
        }
        public class Revision
        {
            public int CodSala { get; set; }
            public string Descripcion { get; set; }
            public string FechaRegistro { get; set; }
            public int Id { get; set; }
            public string NombreSala { get; set; }
            public bool Peligro { get; set; }
            public string Preview { get; set; }
            public int Tipo { get; set; }


        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ReporteRevisionesJson(List<Data> data)
        {
            string base64String = "";
            string excelName = string.Empty;
            var lista = new List<LogAlertaBilleterosEntidad>();
            List<Data> registrosAgrupados = new List<Data>();
            DateTime fechaActual = DateTime.Now;
            Color alerta = ColorTranslator.FromHtml("#D94E2F");
            Color normal = ColorTranslator.FromHtml("#008f39");
            Color white = ColorTranslator.FromHtml("#FFFFFF");
            Color bgColumn = ColorTranslator.FromHtml("#003268");
            Color bgRow = ColorTranslator.FromHtml("#d1e5ff");
            Color colbackground = ColorTranslator.FromHtml("#003268");
            Color colborder = ColorTranslator.FromHtml("#484848");

            try
            {
                registrosAgrupados = AlertasLog();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excel = new ExcelPackage();


                var workSheet = excel.Workbook.Worksheets.Add("Particiones");

                var celdasParaCombinar = workSheet.Cells["B1:E1"];
                celdasParaCombinar.Merge = true;

                var fechaCelda = workSheet.Cells["B2:E2"];
                fechaCelda.Merge= true;
                fechaCelda.Value= "Fecha de emision del reporte: "+fechaActual.ToString("dd/MM/yyyy HH:mm:ss");
                fechaCelda.Worksheet.Row(fechaCelda.Start.Row).Height = 40.00;
                fechaCelda.Style.Font.Size = 13;
                fechaCelda.Style.Font.Italic= true;
                fechaCelda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Titulo
                celdasParaCombinar.Value = "REPORTE ALERTA BILLETEROS";
                celdasParaCombinar.Style.Font.Size = 30;
                celdasParaCombinar.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                celdasParaCombinar.Worksheet.Row(celdasParaCombinar.Start.Row).Height = 65.00;
                celdasParaCombinar.Style.Font.Bold = true;


                //Leyenda
                workSheet.Cells[33, 2].Value = "Color";
                workSheet.Cells[33, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[33, 2].Style.Fill.BackgroundColor.SetColor(bgColumn);
                workSheet.Cells[33, 2].Style.Font.Color.SetColor(white);
                workSheet.Cells[33, 3].Value = "Descripcion";
                workSheet.Cells[33, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[33, 3].Style.Fill.BackgroundColor.SetColor(bgColumn);
                workSheet.Cells[33, 3].Style.Font.Color.SetColor(white);
                workSheet.Cells[34, 3].Value = "Tiene alertas dentro de 1 día";
                workSheet.Cells[35, 3].Value = "No tiene alertas mas de 1 día";
                workSheet.Cells[34, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[34, 2].Style.Fill.BackgroundColor.SetColor(normal);
                workSheet.Cells[35, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[35, 2].Style.Fill.BackgroundColor.SetColor(alerta);
                workSheet.TabColor = System.Drawing.Color.Black;
                workSheet.DefaultRowHeight = 12;
                //Header of table  
                //  
                workSheet.Row(3).Height = 20;
                workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(3).Style.Font.Bold = true;

                workSheet.Cells[3, 2].Value = "Sala";
                workSheet.Cells[3, 3].Value = "Última actividad del Servicio Online";
                workSheet.Cells[3, 4].Value = "Ultimo evento registrado";
                workSheet.Cells[3, 5].Value = "Última alerta registrada";

                //Body of table  
                //  
                int recordIndex = 4;
                int total = lista.Count;
                foreach (var registro in registrosAgrupados)
                {

                    workSheet.Cells["B" + recordIndex + ":E" + recordIndex].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B" + recordIndex + ":E" + recordIndex].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B" + recordIndex + ":E" + recordIndex].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B" + recordIndex + ":E" + recordIndex].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B" + recordIndex + ":E" + recordIndex].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B" + recordIndex + ":E" + recordIndex].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B" + recordIndex + ":E" + recordIndex].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B" + recordIndex + ":E" + recordIndex].Style.Border.Bottom.Color.SetColor(colborder);

                    workSheet.Row(recordIndex).Height = 20.00;
                    if (recordIndex % 2 == 0)
                    {

                        workSheet.Cells[recordIndex, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[recordIndex, 2].Style.Fill.BackgroundColor.SetColor(bgRow);
                        workSheet.Cells[recordIndex, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[recordIndex, 3].Style.Fill.BackgroundColor.SetColor(bgRow);
                        workSheet.Cells[recordIndex, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[recordIndex, 4].Style.Fill.BackgroundColor.SetColor(bgRow);
                        workSheet.Cells[recordIndex, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[recordIndex, 5].Style.Fill.BackgroundColor.SetColor(bgRow);

                    }
                    workSheet.Cells[recordIndex, 2].Value = registro.NombreSala;
                    foreach (var registroData in registro.Registros)
                    {
                        if (registroData.Tipo == 1)
                        {
                            workSheet.Cells[recordIndex, 3].Value = registroData.FechaRegistro + registroData.Descripcion;

                        }
                        if (registroData.Tipo == 2)
                        {
                            workSheet.Cells[recordIndex, 4].Value = registroData.FechaRegistro;
                            if (registroData.Peligro)
                            {
                                workSheet.Cells[recordIndex, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[recordIndex, 4].Style.Fill.BackgroundColor.SetColor(alerta);
                                workSheet.Cells[recordIndex, 4].Style.Font.Color.SetColor(white);

                            }
                            else
                            {
                                workSheet.Cells[recordIndex, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[recordIndex, 4].Style.Fill.BackgroundColor.SetColor(normal);
                                workSheet.Cells[recordIndex, 4].Style.Font.Color.SetColor(white);

                            }

                        }
                        if (registroData.Tipo == 3)
                        {
                            workSheet.Cells[recordIndex, 5].Value = registroData.FechaRegistro;
                            if (registroData.Peligro)
                            {
                                workSheet.Cells[recordIndex, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[recordIndex, 5].Style.Fill.BackgroundColor.SetColor(alerta);
                                workSheet.Cells[recordIndex, 5].Style.Font.Color.SetColor(white);

                            }
                            else
                            {
                                workSheet.Cells[recordIndex, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[recordIndex, 5].Style.Fill.BackgroundColor.SetColor(normal);
                                workSheet.Cells[recordIndex, 5].Style.Font.Color.SetColor(white);

                            }

                        }
                    }

                    recordIndex++;

                }




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

                workSheet.Column(2).AutoFit();
                workSheet.Column(3).AutoFit();
                workSheet.Column(4).AutoFit();
                workSheet.Column(5).AutoFit();

                workSheet.Column(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Column(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Column(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                workSheet.Column(2).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Column(3).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Column(4).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Column(5).Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                workSheet.Column(17).Width = 35;
                excelName = "ReporteAlertaBilleteros.xlsx";
                var memoryStream = new MemoryStream();
                excel.SaveAs(memoryStream);
                base64String = Convert.ToBase64String(memoryStream.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }



            return Json(new { data = base64String, excelName });

        }


        [seguridad(false)]
        [HttpPost]
        public ActionResult ReporteRevisionesJsonxUsuario(List<Data> data)
        {
            string base64String = "";
            string excelName = string.Empty;
            var lista = new List<LogAlertaBilleterosEntidad>();
            List<Data> registrosAgrupados = new List<Data>();
            DateTime fechaActual = DateTime.Now;
            Color alerta = ColorTranslator.FromHtml("#D94E2F");
            Color normal = ColorTranslator.FromHtml("#008f39");
            Color white = ColorTranslator.FromHtml("#FFFFFF");
            Color bgColumn = ColorTranslator.FromHtml("#003268");
            Color bgRow = ColorTranslator.FromHtml("#d1e5ff");
            Color colbackground = ColorTranslator.FromHtml("#003268");
            Color colborder = ColorTranslator.FromHtml("#484848");

            try
            {
                registrosAgrupados = AlertasLogxUsuario();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excel = new ExcelPackage();


                var workSheet = excel.Workbook.Worksheets.Add("Particiones");

                var celdasParaCombinar = workSheet.Cells["B1:E1"];
                celdasParaCombinar.Merge = true;

                var fechaCelda = workSheet.Cells["B2:E2"];
                fechaCelda.Merge = true;
                fechaCelda.Value = "Fecha de emision del reporte: " + fechaActual.ToString("dd/MM/yyyy HH:mm:ss");
                fechaCelda.Worksheet.Row(fechaCelda.Start.Row).Height = 40.00;
                fechaCelda.Style.Font.Size = 13;
                fechaCelda.Style.Font.Italic = true;
                fechaCelda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Titulo
                celdasParaCombinar.Value = "REPORTE ALERTA BILLETEROS";
                celdasParaCombinar.Style.Font.Size = 30;
                celdasParaCombinar.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                celdasParaCombinar.Worksheet.Row(celdasParaCombinar.Start.Row).Height = 65.00;
                celdasParaCombinar.Style.Font.Bold = true;


                //Leyenda
                workSheet.Cells[33, 2].Value = "Color";
                workSheet.Cells[33, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[33, 2].Style.Fill.BackgroundColor.SetColor(bgColumn);
                workSheet.Cells[33, 2].Style.Font.Color.SetColor(white);
                workSheet.Cells[33, 3].Value = "Descripcion";
                workSheet.Cells[33, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[33, 3].Style.Fill.BackgroundColor.SetColor(bgColumn);
                workSheet.Cells[33, 3].Style.Font.Color.SetColor(white);
                workSheet.Cells[34, 3].Value = "Tiene alertas dentro de 1 día";
                workSheet.Cells[35, 3].Value = "No tiene alertas mas de 1 día";
                workSheet.Cells[34, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[34, 2].Style.Fill.BackgroundColor.SetColor(normal);
                workSheet.Cells[35, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[35, 2].Style.Fill.BackgroundColor.SetColor(alerta);
                workSheet.TabColor = System.Drawing.Color.Black;
                workSheet.DefaultRowHeight = 12;
                //Header of table  
                //  
                workSheet.Row(3).Height = 20;
                workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(3).Style.Font.Bold = true;

                workSheet.Cells[3, 2].Value = "Sala";
                workSheet.Cells[3, 3].Value = "Última actividad del Servicio Online";
                workSheet.Cells[3, 4].Value = "Ultimo evento registrado";
                workSheet.Cells[3, 5].Value = "Última alerta registrada";

                //Body of table  
                //  
                int recordIndex = 4;
                int total = lista.Count;
                foreach (var registro in registrosAgrupados)
                {

                    workSheet.Cells["B" + recordIndex + ":E" + recordIndex].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B" + recordIndex + ":E" + recordIndex].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B" + recordIndex + ":E" + recordIndex].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B" + recordIndex + ":E" + recordIndex].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B" + recordIndex + ":E" + recordIndex].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B" + recordIndex + ":E" + recordIndex].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B" + recordIndex + ":E" + recordIndex].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B" + recordIndex + ":E" + recordIndex].Style.Border.Bottom.Color.SetColor(colborder);

                    workSheet.Row(recordIndex).Height = 20.00;
                    if (recordIndex % 2 == 0)
                    {

                        workSheet.Cells[recordIndex, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[recordIndex, 2].Style.Fill.BackgroundColor.SetColor(bgRow);
                        workSheet.Cells[recordIndex, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[recordIndex, 3].Style.Fill.BackgroundColor.SetColor(bgRow);
                        workSheet.Cells[recordIndex, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[recordIndex, 4].Style.Fill.BackgroundColor.SetColor(bgRow);
                        workSheet.Cells[recordIndex, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[recordIndex, 5].Style.Fill.BackgroundColor.SetColor(bgRow);

                    }
                    workSheet.Cells[recordIndex, 2].Value = registro.NombreSala;
                    foreach (var registroData in registro.Registros)
                    {
                        if (registroData.Tipo == 1)
                        {
                            workSheet.Cells[recordIndex, 3].Value = registroData.FechaRegistro + registroData.Descripcion;

                        }
                        if (registroData.Tipo == 2)
                        {
                            workSheet.Cells[recordIndex, 4].Value = registroData.FechaRegistro;
                            if (registroData.Peligro)
                            {
                                workSheet.Cells[recordIndex, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[recordIndex, 4].Style.Fill.BackgroundColor.SetColor(alerta);
                                workSheet.Cells[recordIndex, 4].Style.Font.Color.SetColor(white);

                            }
                            else
                            {
                                workSheet.Cells[recordIndex, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[recordIndex, 4].Style.Fill.BackgroundColor.SetColor(normal);
                                workSheet.Cells[recordIndex, 4].Style.Font.Color.SetColor(white);

                            }

                        }
                        if (registroData.Tipo == 3)
                        {
                            workSheet.Cells[recordIndex, 5].Value = registroData.FechaRegistro;
                            if (registroData.Peligro)
                            {
                                workSheet.Cells[recordIndex, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[recordIndex, 5].Style.Fill.BackgroundColor.SetColor(alerta);
                                workSheet.Cells[recordIndex, 5].Style.Font.Color.SetColor(white);

                            }
                            else
                            {
                                workSheet.Cells[recordIndex, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[recordIndex, 5].Style.Fill.BackgroundColor.SetColor(normal);
                                workSheet.Cells[recordIndex, 5].Style.Font.Color.SetColor(white);

                            }

                        }
                    }

                    recordIndex++;

                }




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

                workSheet.Column(2).AutoFit();
                workSheet.Column(3).AutoFit();
                workSheet.Column(4).AutoFit();
                workSheet.Column(5).AutoFit();

                workSheet.Column(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Column(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Column(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                workSheet.Column(2).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Column(3).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Column(4).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Column(5).Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                workSheet.Column(17).Width = 35;
                excelName = "ReporteAlertaBilleteros.xlsx";
                var memoryStream = new MemoryStream();
                excel.SaveAs(memoryStream);
                base64String = Convert.ToBase64String(memoryStream.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }



            return Json(new { data = base64String, excelName });

        }



        private List<Data> AlertasLog()
        {
            DateTime fechaActual = DateTime.Now;
            var lista = new List<LogAlertaBilleterosEntidad>();
            List<Data> registrosAgrupados = new List<Data>();


            lista = logBL.ConsultaRegistrosAlertaBilletero().ToList();

            var registrosAgrupadosPorSala = lista.GroupBy(registro => registro.CodSala);

            foreach (var grupo in registrosAgrupadosPorSala)
            {
                
                var registrosRevision = grupo.Select(registro =>
                {
                    TimeSpan diff = fechaActual - registro.FechaRegistro;
                    double diasDiff = diff.TotalDays;
                    return new Revision
                    {
                        Id = Convert.ToInt32(registro.Id),
                        CodSala = registro.CodSala,
                        Descripcion = registro.Descripcion,
                        Tipo = registro.Tipo,
                        FechaRegistro = registro.FechaRegistro.ToString("dd/MM/yyyy HH:mm:ss"),
                        Preview = registro.Preview,
                        NombreSala = registro.NombreSala,
                        Peligro = diasDiff > 1 ? true : false,
                    }; // Conversión explícita a tipo object
                }).ToList();
                var nombreSala = grupo.First().NombreSala;
                
                var data = new Data
                {
                    CodSala = grupo.Key,
                    NombreSala = nombreSala,
                    Registros = registrosRevision
                };
                registrosAgrupados.Add(data);
            }
            return registrosAgrupados;

        }


        private List<Data> AlertasLogxUsuario()
        {
            var usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            DateTime fechaActual = DateTime.Now;
            var lista = new List<LogAlertaBilleterosEntidad>();
            List<Data> registrosAgrupados = new List<Data>();
            List<int> listaSalas = logBL.ConsultaSalasActivasxUsuario(usuarioId).ToList();
            string listaSalasString = string.Join(", ", listaSalas);

            lista = logBL.ConsultaRegistrosAlertaBilleteroxUsuario(listaSalasString.ToString()).ToList();

            var registrosAgrupadosPorSala = lista.GroupBy(registro => registro.CodSala);

            foreach (var grupo in registrosAgrupadosPorSala)
            {

                var registrosRevision = grupo.Select(registro =>
                {
                    TimeSpan diff = fechaActual - registro.FechaRegistro;
                    double diasDiff = diff.TotalDays;
                    return new Revision
                    {
                        Id = Convert.ToInt32(registro.Id),
                        CodSala = registro.CodSala,
                        Descripcion = registro.Descripcion,
                        Tipo = registro.Tipo,
                        FechaRegistro = registro.FechaRegistro.ToString("dd/MM/yyyy HH:mm:ss"),
                        Preview = registro.Preview,
                        NombreSala = registro.NombreSala,
                        Peligro = diasDiff > 1 ? true : false,
                    }; // Conversión explícita a tipo object
                }).ToList();
                var nombreSala = grupo.First().NombreSala;

                var data = new Data
                {
                    CodSala = grupo.Key,
                    NombreSala = nombreSala,
                    Registros = registrosRevision
                };
                registrosAgrupados.Add(data);
            }
            return registrosAgrupados;

        }


        [seguridad(false)]
        [HttpPost]
        public ActionResult ConsultaRegistrosAlertaBilletero()
        {
            var errormensaje = "";
            DateTime fechaActual = DateTime.Now;
            var lista = new List<LogAlertaBilleterosEntidad>();
            List<Data> registrosAgrupados = new List<Data>();

            try
            {
                registrosAgrupados = AlertasLog();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }

            var jsonResult = Json(new { data = registrosAgrupados, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ConsultaRegistrosAlertaBilleteroxUsuario()
        {
            var errormensaje = "";
            DateTime fechaActual = DateTime.Now;
            var lista = new List<LogAlertaBilleterosEntidad>();
            List<Data> registrosAgrupados = new List<Data>();

            try
            {
                registrosAgrupados = AlertasLogxUsuario();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }

            var jsonResult = Json(new { data = registrosAgrupados, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }



        [seguridad(false)]

        [HttpPost]
        public async Task GenerarReportealertaBilletero()
        {
            string mensaje = "";
            //string[] destinatarios = { "deriandar@gmail.com", "jose.ishikawa@designdevsoftware.com", "richard.blanco@designdevsoftware.com" };
            string base64String = "";
            string excelName = string.Empty;
            var lista = new List<LogAlertaBilleterosEntidad>();
            List<Data> registrosAgrupados = new List<Data>();
            DateTime fechaActual = DateTime.Now;
            Color alerta = ColorTranslator.FromHtml("#D94E2F");
            Color normal = ColorTranslator.FromHtml("#008f39");
            Color white = ColorTranslator.FromHtml("#FFFFFF");
            Color bgColumn = ColorTranslator.FromHtml("#003268");
            Color bgRow = ColorTranslator.FromHtml("#d1e5ff");
            Color colbackground = ColorTranslator.FromHtml("#003268");
            Color colborder = ColorTranslator.FromHtml("#484848");


            List<string> destinatarios = new List<string>(); ;
            try
            {
                string listaCorreos = ConfigurationManager.AppSettings["correosAlertas"];
                destinatarios = listaCorreos.Split(',').ToList();
            }
            catch
            {
                destinatarios.Add("deriandar@gmail.com");
            }

            try
            {
                registrosAgrupados = AlertasLog();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excel = new ExcelPackage();

                var workSheet = excel.Workbook.Worksheets.Add("Particiones");
                var celdasParaCombinar = workSheet.Cells["B1:E1"];
                celdasParaCombinar.Merge = true;
                var fechaCelda = workSheet.Cells["B2:E2"];
                fechaCelda.Merge = true;
                fechaCelda.Value = "Fecha de emision del reporte: " + fechaActual.ToString("dd/MM/yyyy HH:mm:ss");
                fechaCelda.Worksheet.Row(fechaCelda.Start.Row).Height = 40.00;
                fechaCelda.Style.Font.Size = 13;
                fechaCelda.Style.Font.Italic = true;
                fechaCelda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Titulo
                celdasParaCombinar.Value = "REPORTE ALERTA BILLETEROS";
                celdasParaCombinar.Style.Font.Size = 30;
                celdasParaCombinar.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                celdasParaCombinar.Worksheet.Row(celdasParaCombinar.Start.Row).Height = 65.00;
                celdasParaCombinar.Style.Font.Bold = true;



                //Leyenda
                workSheet.Cells[33, 2].Value = "Color";
                workSheet.Cells[33, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[33, 2].Style.Fill.BackgroundColor.SetColor(bgColumn);
                workSheet.Cells[33, 2].Style.Font.Color.SetColor(white);
                workSheet.Cells[33, 3].Value = "Descripcion";
                workSheet.Cells[33, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[33, 3].Style.Fill.BackgroundColor.SetColor(bgColumn);
                workSheet.Cells[33, 3].Style.Font.Color.SetColor(white);
                workSheet.Cells[34, 3].Value = "Tiene alertas dentro de 1 día";
                workSheet.Cells[35, 3].Value = "No tiene alertas mas de 1 día";
                workSheet.Cells[34, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[34, 2].Style.Fill.BackgroundColor.SetColor(normal);
                workSheet.Cells[35, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[35, 2].Style.Fill.BackgroundColor.SetColor(alerta);
                workSheet.TabColor = System.Drawing.Color.Black;
                workSheet.DefaultRowHeight = 12;
                //Header of table  
                //  
                workSheet.Row(3).Height = 20;
                workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(3).Style.Font.Bold = true;

                workSheet.Cells[3, 2].Value = "Sala";
                workSheet.Cells[3, 3].Value = "Última actividad del Servicio Online";
                workSheet.Cells[3, 4].Value = "Ultimo evento registrado";
                workSheet.Cells[3, 5].Value = "Última alerta registrada";

                //Body of table  
                //  
                int recordIndex = 4;
                int total = lista.Count;
                foreach (var registro in registrosAgrupados)
                {

                    workSheet.Cells["B"+recordIndex+":E"+recordIndex].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B" + recordIndex + ":E" + recordIndex].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B" + recordIndex + ":E" + recordIndex].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B" + recordIndex + ":E" + recordIndex].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B" + recordIndex + ":E" + recordIndex].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B" + recordIndex + ":E" + recordIndex].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B" + recordIndex + ":E" + recordIndex].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B" + recordIndex + ":E" + recordIndex].Style.Border.Bottom.Color.SetColor(colborder);

                    workSheet.Row(recordIndex).Height = 20.00;
                    if(recordIndex %2 == 0)
                    {
                        
                        workSheet.Cells[recordIndex, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[recordIndex, 2].Style.Fill.BackgroundColor.SetColor(bgRow);
                        workSheet.Cells[recordIndex, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[recordIndex, 3].Style.Fill.BackgroundColor.SetColor(bgRow);
                        workSheet.Cells[recordIndex, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[recordIndex, 4].Style.Fill.BackgroundColor.SetColor(bgRow);
                        workSheet.Cells[recordIndex, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[recordIndex, 5].Style.Fill.BackgroundColor.SetColor(bgRow);

                    }
                    workSheet.Cells[recordIndex, 2].Value = registro.NombreSala;
                    foreach (var registroData in registro.Registros)
                    {
                        if (registroData.Tipo == 1)
                        {
                            workSheet.Cells[recordIndex, 3].Value = registroData.FechaRegistro + registroData.Descripcion;

                        }
                        if (registroData.Tipo == 2)
                        {
                            workSheet.Cells[recordIndex, 4].Value = registroData.FechaRegistro;
                            if (registroData.Peligro)
                            {
                                workSheet.Cells[recordIndex, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[recordIndex, 4].Style.Fill.BackgroundColor.SetColor(alerta);
                                workSheet.Cells[recordIndex, 4].Style.Font.Color.SetColor(white);

                            }
                            else
                            {
                                workSheet.Cells[recordIndex, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[recordIndex, 4].Style.Fill.BackgroundColor.SetColor(normal);
                                workSheet.Cells[recordIndex, 4].Style.Font.Color.SetColor(white);

                            }

                        }
                        if (registroData.Tipo == 3)
                        {
                            workSheet.Cells[recordIndex, 5].Value = registroData.FechaRegistro;
                            if (registroData.Peligro)
                            {
                                workSheet.Cells[recordIndex, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[recordIndex, 5].Style.Fill.BackgroundColor.SetColor(alerta);
                                workSheet.Cells[recordIndex, 5].Style.Font.Color.SetColor(white);

                            }
                            else
                            {
                                workSheet.Cells[recordIndex, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[recordIndex, 5].Style.Fill.BackgroundColor.SetColor(normal);
                                workSheet.Cells[recordIndex, 5].Style.Font.Color.SetColor(white);

                            }

                        }
                    }

                    recordIndex++;

                }
                



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

                workSheet.Column(2).AutoFit();
                workSheet.Column(3).AutoFit();
                workSheet.Column(4).AutoFit();
                workSheet.Column(5).AutoFit();

                workSheet.Column(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Column(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Column(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                workSheet.Column(2).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Column(3).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Column(4).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Column(5).Style.VerticalAlignment = ExcelVerticalAlignment.Center;

               
                

                var memoryStream = new MemoryStream();
                excel.SaveAs(memoryStream);
                base64String = Convert.ToBase64String(memoryStream.ToArray());

                string excelGenerado = ConvertBase64ToExcel(base64String,"ReporteAlertas");
                mensaje = "Correo enviado correctamente";
                string reporteAlertasDia = await ConsultaReporteSalasAlertas();

                string excelGeneradoReporteDia = ConvertBase64ToExcel(reporteAlertasDia, "LogReporteNominal");


                EnviarCorreos(destinatarios, excelGenerado, excelGeneradoReporteDia);

            }
            catch (Exception ex)
            {
                mensaje = "Error al enviar el correo";
                Console.WriteLine(ex.Message);
            }

            //return Json(new { mensaje });

        }

  
        private class RespuestaReporteSalas{
        public bool success { get; set; }
        public string message { get; set; }
        public string fileName { get; set; }
        public string data { get; set; }

        }

        public async Task<string> ConsultaReporteSalasAlertas()
        {
            // URL del endpoint en tu propio servidor
            string url = "http://40.122.134.6/IAS/LogAlertaBilleteros/ExcelLogsReporteNominalFree";
            //string url = "http://localhost:56382/LogAlertaBilleteros/ExcelLogsReporteNominalFree";
            //string url = "http://192.168.1.99/ias/LogAlertaBilleteros/ExcelLogsReporteNominalFree";
            string excelGenerado = "";
            List<int> lista = logBL.ListarSalasActivas().ToList();

            DateTime fromDate = DateTime.Now.AddDays(-2);
            DateTime toDate = DateTime.Now;

            string fromDateStr = fromDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string toDateStr = toDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var data = new
                    {
                        rooms = lista, // Ejemplo de array de enteros
                        fromDate = fromDateStr, // Fecha de inicio
                        toDate = toDateStr, // Fecha de fin
                        types = new int[] { 1,2,3 } // Ejemplo de array de strings
                    };

                    // Serializar los datos a formato JSON
                    string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data);

                    // Convertir los datos serializados a contenido de tipo StringContent
                    StringContent contenido = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    
                    HttpResponseMessage response = await client.PostAsync(url, contenido);

                    // Verificar si la solicitud fue exitosa (código de estado 200 OK)
                    if (response.IsSuccessStatusCode)
                    {
                        // Leer y deserializar la respuesta JSON del servidor
                        string jsonRespuesta = await response.Content.ReadAsStringAsync();
                        RespuestaReporteSalas respuesta = JsonConvert.DeserializeObject<RespuestaReporteSalas>(jsonRespuesta);

                        // Aquí puedes acceder a las propiedades de la respuesta
                        if (respuesta.success)
                        {
                          
                            string fileName = respuesta.fileName;
                            string mensaje = respuesta.message;
                            string dataResponse = respuesta.data;

                            excelGenerado = dataResponse;


                        }
                        else
                        {
                            // La solicitud no fue exitosa
                            Console.WriteLine( "Error al consultar el reporte de alertas por sala dia");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error en la solicitud. Código de estado: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
            return excelGenerado;

        }


     
        public static string ConvertBase64ToExcel(string base64, string nombre)
        {
            string filePath ="";
            try
            {
                Guid guid = Guid.NewGuid();
                DateTime now = DateTime.Now;

                byte[] excelBytes = Convert.FromBase64String(base64);

                string fileDierctory = VerifyDirectory("C:/alertabilltero/");
                string fileName = nombre;
                string fileExtension = "xlsx";

                filePath = fileDierctory + fileName + "." +fileExtension;
                CHANO.File.WriteAllBytes(filePath, excelBytes);
                Console.WriteLine("Excel convertido correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al convertir el archivo a excel. {ex.Message}");
            }
            return filePath;
        }

        private static string VerifyDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            return directoryPath;
        }


        private void EnviarCorreos(List<string> destinatarios, string excelGenerado, string reportAlertaDia)
        {
            List<string> adjuntos = new List<string>();
            SalaEntidad sala = new SalaEntidad();
            Correo correo_enviar = new Correo();

            adjuntos.Add(excelGenerado);
            adjuntos.Add(reportAlertaDia);

            string basepath = Request.Url.Scheme + "://" + ((Request.Url.Authority + Request.ApplicationPath).TrimEnd('/')) + "/";
            sala.RutaArchivoLogo = sala.RutaArchivoLogo != basepath + "Content/assets/images/no_image.jpg" ? "https://drive.google.com/uc?id=" + sala.RutaArchivoLogo : "";
            StringBuilder html = new StringBuilder();

            string htmlEnvio = $@"Reporte Alertas Billeteros";



            var listac = String.Join(",", destinatarios);
            Correo correo_destinatario = new Correo();

            // Adjuntar el archivo al correo electrónico
            correo_destinatario.EnviarCorreo(
            listac,
                     "Reporte Alertas Billeteros",
                        htmlEnvio,
                     true,
                     adjuntos
                     );
        }




    }
}