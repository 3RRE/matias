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
using System.Drawing.Imaging;
using System.Globalization;

namespace CapaPresentacion.Controllers.ControlAcceso
{
    [seguridad]
    public class CALCodigoController : Controller
    {
        private CAL_CodigoBL codigoBL = new CAL_CodigoBL();

        public ActionResult ListadoCodigo()
        {
            return View("~/Views/ControlAcceso/ListadoCodigo.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarCodigoJson()
        {
            var errormensaje = "";
            var lista = new List<CAL_CodigoEntidad>();

            try
            {

                lista = codigoBL.CodigoListadoCompletoJson();

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarCodigoIdJson(int CodigoID)
        {
            var errormensaje = "";
            CAL_CodigoEntidad item = new CAL_CodigoEntidad();

            try
            {

                item = codigoBL.CodigoIdObtenerJson(CodigoID);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = item, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CodigoEditarJson(CAL_CodigoEntidad codigo)
        {
                var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                const string PathInsercion = @"c:/IASRECO/Ludopatas";
                string nombreArchivo = string.Empty;
                CAL_CodigoEntidad codigoAntiguo = codigoBL.CodigoIdObtenerJson(codigo.CodigoID);
                respuestaConsulta = codigoBL.CodigoEditarJson(codigo);

                if (respuestaConsulta)
                {

                    string htmlAdicional = string.Empty;
                    bool archivoVerificado = VerificarArchivo(Path.Combine(PathInsercion, $"{codigoAntiguo.Alerta.ToUpper().Trim()}.png"));
                    if (archivoVerificado)
                    {
                        System.IO.File.Delete(Path.Combine(PathInsercion, $"{codigoAntiguo.Alerta.ToUpper().Trim()}.png"));
                        

                        errormensaje = "Registro de Codigo Actualizado Correctamente Con Imagen  ";
                    }
                    else
                    {
                       
                        errormensaje = "Registro de Codigo Actualizado Correctamente Sin Imagen ";
                    }


                }
                else
                {
                    errormensaje = "Error al Actualizar Codigo , LLame Administrador";
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
        public ActionResult CodigoGuardarJson(CAL_CodigoEntidad codigo)
        {
            var errormensaje = "";
            Int64 respuestaConsulta = 0;
            bool respuesta = false;
            try
            {
                respuestaConsulta = codigoBL.CodigoInsertarJson(codigo);

                if (respuestaConsulta > 0)
                {
                    respuesta = true;
                    errormensaje = "Registro Codigo Guardado Correctamente";
                }
                else
                {
                    errormensaje = "Error al crear la Codigo , LLame Administrador";
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
        public ActionResult CodigoEliminarJson(int id)
        {
            var errormensaje = "";
            bool respuesta = false;
            try
            {
                respuesta = codigoBL.CodigoEliminarJson(id);
                if (respuesta)
                {
                    respuesta = true;
                    errormensaje = "Se quitó el Codigo Correctamente";
                }
                else
                {
                    errormensaje = "error al Quitar el Codigo , LLame Administrador";
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
        public ActionResult CodigoDescargarExcelJson()
        {
            string fecha = DateTime.Now.ToString("dd_MM_yyyy");
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<CAL_CodigoEntidad> lista = new List<CAL_CodigoEntidad>();
            var strElementos = String.Empty;
            var strElementos_ = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;
            try
            {


                lista = codigoBL.CodigoListadoCompletoJson();
                if (lista.Count > 0)
                {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Codigo");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Alerta";
                    workSheet.Cells[3, 4].Value = "Accion";
                    workSheet.Cells[3, 5].Value = "Color";

                    int recordIndex = 4;
                    int total = lista.Count;
                    foreach (var registro in lista)
                    {

                        workSheet.Cells[recordIndex, 2].Value = registro.CodigoID;
                        workSheet.Cells[recordIndex, 3].Value = registro.Alerta;
                        workSheet.Cells[recordIndex, 4].Value = registro.Accion;
                        workSheet.Cells[recordIndex, 5].Value = registro.Color;

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
                    workSheet.Column(3).Width = 24;
                    workSheet.Column(4).Width = 60;
                    workSheet.Column(5).Width = 18;
                    excelName = "Codigo_" + fecha + ".xlsx";
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


        public string generarImagen(string parthinsercion, string texto, string color)
        {
            color = color.Replace("#", "");
            color = "#ff" + color;
            int argb = Int32.Parse(color.Replace("#", ""), NumberStyles.HexNumber);
            Color coloreado = Color.FromArgb(argb);

            Bitmap objBitmap = new Bitmap(1, 1);
            int Width = 0;
            int Height = 0;

            Font objFont = new Font("Arial", 80,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Pixel);

            Graphics objGraphics = Graphics.FromImage(objBitmap);

            Width = (int)objGraphics.MeasureString(texto, objFont).Width;
            Height = (int)objGraphics.MeasureString(texto, objFont).Height;
            objBitmap = new Bitmap(objBitmap, new Size(Width, Height));


            objGraphics = Graphics.FromImage(objBitmap);
            objGraphics.SmoothingMode =
                System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            objGraphics.CompositingQuality =
                System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            objGraphics.InterpolationMode =
                System.Drawing.Drawing2D.InterpolationMode.High;
            objGraphics.TextRenderingHint =
                System.Drawing.Text.TextRenderingHint.AntiAlias;
            objGraphics.DrawString(texto, objFont,
                new SolidBrush(coloreado), 0, 0);
            objGraphics.DrawEllipse(new Pen(new SolidBrush(coloreado), 4), new Rectangle(0, 0, Width, Height));
            objGraphics.Flush();

            objBitmap = RotateImage(objBitmap, -45);
            string nombreArchivo = $@"{texto}.png";
            string rutaImagen = Path.Combine(parthinsercion, nombreArchivo);
            objBitmap.Save(rutaImagen, ImageFormat.Png);
            return nombreArchivo;
        }

        private Bitmap RotateImage(Bitmap bmp, float angle)
        {
            float height = bmp.Height;
            float width = bmp.Width;
            int hypotenuse = System.Convert.ToInt32(System.Math.Floor(Math.Sqrt(height * height + width * width)));
            Bitmap rotatedImage = new Bitmap(hypotenuse * 2, hypotenuse * 2);
            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                g.TranslateTransform((float)rotatedImage.Width / 2, (float)rotatedImage.Height / 2);
                g.RotateTransform(angle);
                g.TranslateTransform(-(float)rotatedImage.Width / 2, -(float)rotatedImage.Height / 2);
                g.DrawImage(bmp, (hypotenuse * 2 - width) / 2, (hypotenuse * 2 - height) / 2, width, height);
            }
            return rotatedImage;
        }
        private bool VerificarArchivo(string path)
        {
            bool respuesta = false;
            try
            {
                if (System.IO.File.Exists(path))
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

    }
}