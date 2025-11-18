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
using CapaEntidad;
using CapaNegocio;
using CapaDatos;

namespace CapaPresentacion.Controllers.ControlAcceso
{
    [seguridad]
    public class CALBandaController : Controller
    {
        private CAL_BandaBL bandaBL = new CAL_BandaBL();
        private UbigeoBL ubigeoBL = new UbigeoBL();
        private CAL_PersonaProhibidoIngresoBL timadorBL = new CAL_PersonaProhibidoIngresoBL();

        public ActionResult ListadoBanda()
        {
            return View("~/Views/ControlAcceso/ListadoBanda.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarBandaJson()
        {
            var errormensaje = "";
            var lista = new List<CAL_BandaEntidad>();

            try
            {

                lista = bandaBL.BandaListadoCompletoJson();

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarBandaIdJson(int BandaID)
        {
            var errormensaje = "";
            bool respuesta = false;
            CAL_BandaEntidad item = new CAL_BandaEntidad();

            try
            {

                item = bandaBL.BandaIdObtenerJson(BandaID);
                respuesta = true;

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
                respuesta = false;
            }
            return Json(new { data = item, respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult BandaEditarJson(CAL_BandaEntidad banda)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            banda.FechaModificacion = DateTime.Now;
            try
            {
                respuestaConsulta = bandaBL.BandaEditarJson(banda);

                if (respuestaConsulta)
                {

                    errormensaje = "Registro de Banda Actualizado Correctamente";
                }
                else
                {
                    errormensaje = "Error al Actualizar Banda , LLame Administrador";
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
        public ActionResult BandaGuardarJson(CAL_BandaEntidad banda)
        {
            var errormensaje = "";
            Int64 respuestaConsulta = 0;
            bool respuesta = false;
            try
            {
                banda.FechaRegistro = DateTime.Now;
                banda.FechaModificacion = DateTime.Now; 
                respuestaConsulta = bandaBL.BandaInsertarJson(banda);

                if (respuestaConsulta > 0)
                {
                    respuesta = true;
                    errormensaje = "Registro Banda Guardado Correctamente";
                }
                else
                {
                    errormensaje = "Error al crear la Banda , LLame Administrador";
                    respuesta = false;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new {respuesta= respuesta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult BandaEliminarJson(int id)
        {
            var errormensaje = "";
            bool respuesta = false;


            List<CAL_PersonaProhibidoIngresoEntidad> lista = timadorBL.TimadorListadoCompletoJson();

            foreach (var item in lista)
            {
                if (item.BandaID == id)
                {
                    errormensaje = "La banda se encuentra actualmente asignada.";
                    respuesta = false;
                    return Json(new { respuesta = respuesta, mensaje = errormensaje });
                }
            }

            try
            {
                respuesta = bandaBL.BandaEliminarJson(id);
                if (respuesta)
                {
                    respuesta = true;
                    errormensaje = "Se quitó el Banda Correctamente";
                }
                else
                {
                    errormensaje = "error al Quitar el Banda , LLame Administrador";
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
        public ActionResult BandaDescargarExcelJson()
        {
            string fecha = DateTime.Now.ToString("dd_MM_yyyy");
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<CAL_BandaEntidad> lista = new List<CAL_BandaEntidad>();
            var strElementos = String.Empty;
            var strElementos_ = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;
            try
            {


                lista = bandaBL.BandaListadoCompletoJson();
                if (lista.Count > 0)
                {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Banda");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Descripcion";
                    workSheet.Cells[3, 4].Value = "Pais";
                    workSheet.Cells[3, 5].Value = "Departamento";
                    workSheet.Cells[3, 6].Value = "Provincia";
                    workSheet.Cells[3, 7].Value = "Distrito";
                    workSheet.Cells[3, 8].Value = "Estado";
                    workSheet.Cells[3, 9].Value = "Fecha Registro";
                    workSheet.Cells[3, 10].Value = "Fecha Modificacion";

                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach (var registro in lista)
                    {

                        workSheet.Cells[recordIndex, 2].Value = registro.BandaID;
                        workSheet.Cells[recordIndex, 3].Value = registro.Descripcion.ToUpper();
                        workSheet.Cells[recordIndex, 4].Value = registro.Pais.Trim() ==""?"--": registro.Pais.Trim();
                        workSheet.Cells[recordIndex, 5].Value = registro.Departamento == "" ? "--" : registro.Departamento.Trim();
                        workSheet.Cells[recordIndex, 6].Value = registro.Provincia == "" ? "--" : registro.Provincia.Trim();
                        workSheet.Cells[recordIndex, 7].Value = registro.Distrito == "" ? "--" : registro.Distrito.Trim();
                        workSheet.Cells[recordIndex, 8].Value = registro.Estado == 1 ? "ACTIVO" : "INACTIVO";
                        workSheet.Cells[recordIndex, 9].Value = registro.FechaRegistro.ToString("dd-MM-yyyy hh:mm:ss tt");
                        workSheet.Cells[recordIndex, 10].Value = registro.FechaModificacion.ToString("dd-MM-yyyy hh:mm:ss tt");
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
                    workSheet.Column(4).Width = 30;
                    workSheet.Column(5).Width = 30;
                    workSheet.Column(6).Width = 30;
                    workSheet.Column(7).Width = 30;
                    workSheet.Column(8).Width = 18;
                    workSheet.Column(9).Width = 30;
                    workSheet.Column(10).Width = 30;
                    excelName = "Banda_" + fecha + ".xlsx";
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


        [seguridad(false)]
        [HttpPost]
        public ActionResult GetDataSelects(UbigeoEntidad ubigeo)
        {
            List<UbigeoEntidad> listaUbigeo = new List<UbigeoEntidad>();
            List<UbigeoEntidad> listaDepartamentos = new List<UbigeoEntidad>();
            List<UbigeoEntidad> listaProvincias = new List<UbigeoEntidad>();
            List<UbigeoEntidad> listaDistritos = new List<UbigeoEntidad>();
            UbigeoEntidad ubigeoEntidad = new UbigeoEntidad();
            ubigeoEntidad = ubigeoBL.GetDatosUbigeo(ubigeo.CodUbigeo);
            string mensaje = "";
            bool respuesta = false;
            object oRespuesta = new object();
            try
            {

                listaDepartamentos = ubigeoBL.ListadoDepartamento();
                if (ubigeoEntidad.CodUbigeo != 0)
                {
                    listaProvincias = ubigeoBL.GetListadoProvincia(ubigeoEntidad.DepartamentoId);
                    listaDistritos = ubigeoBL.GetListadoDistrito(ubigeoEntidad.ProvinciaId, ubigeoEntidad.DepartamentoId);
                    oRespuesta = new
                    {
                        dataUbigeo = ubigeoEntidad,
                        dataDepartamentos = listaDepartamentos,
                        dataProvincias = listaProvincias,
                        dataDistritos = listaDistritos,
                    };
                }
                else
                {
                    oRespuesta = new
                    {
                        dataUbigeo = ubigeoEntidad,
                    };
                }
                respuesta = true;

                mensaje = "Listando registros";
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                respuesta = false;
            }
            return Json(new { mensaje, respuesta, data = oRespuesta });
        }

        #region Ubigeo
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoDepartamento()
        {
            string mensaje = "";
            bool respuesta = false;
            List<UbigeoEntidad> lista = new List<UbigeoEntidad>();
            try
            {
                lista = ubigeoBL.ListadoDepartamento();
                mensaje = "Listando Registros";
                respuesta = true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                respuesta = false;
            }
            return Json(new { mensaje, respuesta, data = lista });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoProvincia(int DepartamentoID)
        {
            string mensaje = "";
            bool respuesta = false;
            List<UbigeoEntidad> lista = new List<UbigeoEntidad>();
            try
            {
                lista = ubigeoBL.GetListadoProvincia(DepartamentoID);
                mensaje = "Listando Registros";
                respuesta = true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                respuesta = false;
            }
            return Json(new { mensaje, respuesta, data = lista });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoDistrito(int ProvinciaID, int DepartamentoID)
        {
            string mensaje = "";
            bool respuesta = false;
            List<UbigeoEntidad> lista = new List<UbigeoEntidad>();
            try
            {
                lista = ubigeoBL.GetListadoDistrito(ProvinciaID, DepartamentoID);
                mensaje = "Listando Registros";
                respuesta = true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                respuesta = false;
            }
            return Json(new { mensaje, respuesta, data = lista });
        }

        #endregion

    }
}