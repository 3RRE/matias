using CapaEntidad;
using CapaEntidad.Ocurrencias;
using CapaNegocio;
using CapaNegocio.Ocurrencias;
using CapaPresentacion.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Rotativa;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Ocurrencias
{
    [seguridad]
    public class OcurrenciasController : Controller
    {
        // GET: Ocurrencias
        public OCU_CorreoBL correoBL = new OCU_CorreoBL();
        public OCU_TipoOcurrenciaBL tipoOcurrenciaBL = new OCU_TipoOcurrenciaBL();
        public OCU_CorreoSalaBL correoSalaBL = new OCU_CorreoSalaBL();
        public SalaBL _salaBl = new SalaBL();
        public TipoDOIBL tipoDoiBL = new TipoDOIBL();
        public OCU_OcurrenciaBL ocurrenciaBL = new OCU_OcurrenciaBL();
        private SmtpClient cliente;
        private MailMessage email;
        #region Vistas
        public ActionResult ListadoCorreo()
        {
            return View("~/Views/Ocurrencias/ListadoCorreo.cshtml");
        }
        public ActionResult RegistroCorreo()
        {
            return View("~/Views/Ocurrencias/RegistroCorreo.cshtml");
        }
        public ActionResult EditarCorreo(int id = 0)
        {
            string mensaje = "";
            OCU_CorreoEntidad correo= new OCU_CorreoEntidad();
            try
            {
                correo = correoBL.GetCorreoID(id);
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            ViewBag.correo = correo;
            ViewBag.mensaje = mensaje;
            return View("~/Views/Ocurrencias/EditarCorreo.cshtml");
        }
        public ActionResult ListadoTipoOcurrencia()
        {
            return View("~/Views/Ocurrencias/ListadoTipoOcurrencia.cshtml");
        }
        public ActionResult RegistroTipoOcurrencia()
        {
            return View("~/Views/Ocurrencias/RegistroTipoOcurrencia.cshtml");
        }
        public ActionResult EditarTipoOcurrencia(int id = 0)
        {
            string mensaje = "";
            OCU_TipoOcurrenciaEntidad tipoOcurrencia = new OCU_TipoOcurrenciaEntidad();
            try
            {
                tipoOcurrencia = tipoOcurrenciaBL.GetTipoOcurrenciaID(id);
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            ViewBag.tipoOcurrencia = tipoOcurrencia;
            ViewBag.mensaje = mensaje;
            return View("~/Views/Ocurrencias/EditarTipoOcurrencia.cshtml");
        }
        public ActionResult RegistroOcurrencia()
        {
            return View("~/Views/Ocurrencias/RegistroOcurrencia.cshtml");
        }
        public ActionResult ReporteOcurrencia() {
            return View("~/Views/Ocurrencias/ReporteOcurrencia.cshtml");
        }
        [seguridad(false)]
        public ActionResult PdfOcurrencia(string hash = "")
        {
            OCU_OcurrenciaEntidad ocurrencia = new OCU_OcurrenciaEntidad();
            if (!(hash==string.Empty))
            {
                ViewBag.respuesta = true;
                int ocurrenciaId = Convert.ToInt32(ClaseEncriptacion.Base64ForUrlDecode(hash));
                ViewBag.data = ocurrenciaBL.GetOcurrenciaId(ocurrenciaId);
            }
            else
            {
                ViewBag.respuesta = false;
                ViewBag.data = null;
            }
            return View("~/Views/Ocurrencias/PdfOcurrencia.cshtml");
        }
        [seguridad(false)]
        public ActionResult PdfOcurrenciaMultiple(string salas,DateTime fechaIni,DateTime fechaFin,int UsuarioId)
        {
            string condicion = "";
            List<OCU_OcurrenciaEntidad> lista = new List<OCU_OcurrenciaEntidad>();
            try
            {
                string accion = "PermisoListadoReporteOcurrencias";
                String busqueda = "";
                busqueda = funciones.consulta("PermisoUsuario", @"
                                                                SELECT [WEB_PRolID],[WEB_RolID],[WEB_PRolFechaRegistro]
                                                                FROM [dbo].[SEG_PermisoRol] 
                                                                left join [SEG_Permiso] on [SEG_Permiso].[WEB_PermID]=[SEG_PermisoRol].[WEB_PermID]
                                                                where [SEG_PermisoRol].WEB_RolID =" + UsuarioId +
                                                                                        " and [SEG_Permiso].[WEB_PermNombre]='" + accion + "'"

                                                                         );
                condicion = " [CodSala] in(" + "'" + salas + "'" + ") and ";
                if (busqueda.Length < 3)
                {
                    condicion = " [UsuarioReg]=" + Convert.ToInt32(Session["UsuarioID"]) + " and ";
                }
                lista = ocurrenciaBL.GetListadoOcurrencia(fechaIni, fechaFin, condicion);
                ViewBag.data = lista;
                ViewBag.respuesta = true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewBag.respuesta = false;
                ViewBag.data = null;
            }
       
     
            return View("~/Views/Ocurrencias/PdfOcurrenciaMultiple.cshtml");
        }

        [seguridad(false)]
        public ActionResult EditarEstadoCorreoJson(OCU_CorreoEntidad correo)
        {
            string mensaje = "";
            bool respuesta = false;
            try
            {
                respuesta = correoBL.EditarEstadoCorreo
                    (correo);
                if (respuesta)
                {
                    mensaje = "Registro Editado";
                }
                else
                {
                    mensaje = "No se pudo editar el registro";
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta });
        }
        #endregion
        [HttpPost]
        public ActionResult GetListadoCorreoJson()
        {
            List<OCU_CorreoEntidad> lista = new List<OCU_CorreoEntidad>();
            string mensaje = "";
            bool respuesta = false;
            try
            {
                lista = correoBL.GetListadoCorreos();
                mensaje = "Listando Data";
                respuesta = true;
            }catch(Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje,respuesta,data=lista});
        }
        public ActionResult GuardarCorreoJson(OCU_CorreoEntidad correo)
        {
            string mensaje = "";
            bool respuesta = false;
            int idCorreoInsertado = 0;
            int totalCorreoSalaInsertado = 0;
            string strValues = "";
            try
            {
                correo.Estado = 1;
                if (correo.CodTipoCorreo == 2) {
                    correo.Smtp = "";
                    correo.Password = "";
                    correo.Puerto = 0;
                }
                else
                {
                    correo.Password= ClaseEncriptacion.Encriptar(correo.Password);
                }
                idCorreoInsertado = correoBL.GuardarCorreo(correo);
                if (idCorreoInsertado != 0)
                {
                    List<string> listaValues = new List<string>();
                    foreach (var salaID in correo.arraySalas)
                    {
                        string strValue = String.Format("({0},{1})",
                                idCorreoInsertado,
                                salaID
                            ); ;
                        listaValues.Add(strValue);
                    }
                    strValues = String.Join(",", listaValues);
                    totalCorreoSalaInsertado = correoSalaBL.GuardarCorreoSalaVarios(strValues);
                    if (totalCorreoSalaInsertado != 0)
                    {
                        respuesta = true;
                        mensaje = "Registro Insertado";
                    }
                }
                else
                {
                    mensaje = "No se pudo insertar el registro";
                }
            }
            catch(Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { respuesta,mensaje });
        }
        public ActionResult EditarCorreoJson(OCU_CorreoEntidad correo)
        {
            string mensaje = "";
            bool respuesta = false;
            string strValues = "";
            try {
                if (correo.CodTipoCorreo == 2)
                {
                    correo.Smtp = "";
                    correo.Password = "";
                    correo.Puerto = 0;
                    correo.SSL = 0;
                }
                else
                {
                    correo.Password= correo.Password !=null? ClaseEncriptacion.Encriptar(correo.Password):correo.PasswordEncriptado;
                }

                respuesta = correoBL.EditarCorreo(correo);
                if (respuesta)
                {
                    respuesta = correoSalaBL.EliminarCorreoSalaxCorreoId(correo.Id);
                    if (respuesta)
                    {
                        List<string> listaValues = new List<string>();
                        foreach (var salaID in correo.arraySalas)
                        {
                            string strValue = String.Format("({0},{1})",
                                    correo.Id,
                                    salaID
                                ); ;
                            listaValues.Add(strValue);
                        }
                        strValues = String.Join(",", listaValues);
                        int totalCorreoSalaInsertado = correoSalaBL.GuardarCorreoSalaVarios(strValues);
                        if (totalCorreoSalaInsertado != 0)
                        {
                            respuesta = true;
                            mensaje = "Registro editado";
                        }
                        else
                        {
                            respuesta = false;
                            mensaje = "No se pudo modificar el detalle";
                        }
                    }
                    else
                    {
                        mensaje = "No se pudo eliminar el detalle";
                    }
                }
                else
                {
                    mensaje = "No se pudo editar el registro";
                }
            }
            catch (Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje,respuesta});
        }
        #region TipoOcurrencia
        public ActionResult GetListadoTipoOcurrenciaJson()
        {
            string mensaje = "";
            bool respuesta = false;
            List<OCU_TipoOcurrenciaEntidad> lista = new List<OCU_TipoOcurrenciaEntidad>();
            try
            {
                lista = tipoOcurrenciaBL.GetListadoTipoOcurrencia();
                mensaje = "Listando registros";
                respuesta = true;
            }catch(Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje,respuesta,data=lista});
        }
        public ActionResult GuardarTipoOcurrenciaJson(OCU_TipoOcurrenciaEntidad tipoOcurrencia) {
            string mensaje = "";
            bool respuesta = false;
            int idInsertado = 0;
            try
            {
                tipoOcurrencia.Estado = 1;
                idInsertado = tipoOcurrenciaBL.GuardarTipoOcurrencia(tipoOcurrencia);
                if (idInsertado != 0) {
                    mensaje = "Registro Insertado";
                    respuesta = true;
                }
                else
                {
                    mensaje = "No se pudo insertar el registro";
                }
            }
            catch (Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta });
        }
        public ActionResult EditarTipoOcurrenciaJson(OCU_TipoOcurrenciaEntidad tipoOcurrencia)
        {
            string mensaje = "";
            bool respuesta = false;
            try
            {
                respuesta = tipoOcurrenciaBL.EditarTipoOcurrencia(tipoOcurrencia);
                if (respuesta)
                {
                    mensaje = "Registro Editado";
                }
                else
                {
                    mensaje = "No se pudo editar el registro";
                }
            }catch(Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta });
        }
        public ActionResult EditarEstadoTipoOcurrenciaJson(OCU_TipoOcurrenciaEntidad tipoOcurrencia)
        {
            string mensaje = "";
            bool respuesta = false;
            try
            {
                respuesta = tipoOcurrenciaBL.EditarEstadoTipoOcurrencia(tipoOcurrencia);
                if (respuesta)
                {
                    mensaje = "Registro Editado";
                }
                else
                {
                    mensaje = "No se pudo editar el registro";
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta });
        }


        #endregion
        [HttpPost]
        public ActionResult GuardarOcurrenciaJson(OCU_OcurrenciaEntidad ocurrencia)
        {
            string mensaje = "";
            bool respuesta = false;
            int idInsertado = 0;
            List<OCU_CorreoSalaEntidad> listaCorreosSala = new List<OCU_CorreoSalaEntidad>();
            List<OCU_CorreoSalaEntidad> listaRemitentes = new List<OCU_CorreoSalaEntidad>();
            List<OCU_CorreoSalaEntidad> listaDestinatarios = new List<OCU_CorreoSalaEntidad>();
            string basepath = Request.Url.Scheme + "://" + ((Request.Url.Authority + Request.ApplicationPath).TrimEnd('/')) + "/";
            try
            {
                ocurrencia.UsuarioReg = Convert.ToInt32(Session["UsuarioID"]);
                idInsertado = ocurrenciaBL.GuardarOcurrencia(ocurrencia);
                if (idInsertado > 0)
                {
                    listaCorreosSala = correoSalaBL.GetListadoCorreoSalaxSala(ocurrencia.CodSala);
                    listaRemitentes = listaCorreosSala.Where(x => x.Correo.CodTipoCorreo == 1).ToList();
                    listaDestinatarios = listaCorreosSala.Where(x => x.Correo.CodTipoCorreo == 2).ToList();

                    if (listaRemitentes.Count > 0 && listaDestinatarios.Count > 0)
                    {
                        foreach(var remitente in listaRemitentes)
                        {
                            listaDestinatarios = listaDestinatarios.Where(x=>x.SalaId == remitente.SalaId).ToList();
                            if (listaDestinatarios.Count > 0)
                            {
                                List<string> listaDireccionesEnvio = new List<string>();
                                foreach (var destinatario in listaDestinatarios)
                                {
                                    listaDireccionesEnvio.Add(destinatario.Correo.Email.Trim());
                                }
                                string direccionesEnvio = String.Join(",", listaDireccionesEnvio);
                                string cuerpoMensaje = ("Ocurrencia<br>" +
                                     "Ocurrencia Creada el : " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") +
                                     "<br/> Presione aqui para ver la ocurrencia: <a href=" + basepath + "Ocurrencias/PdfOcurrencia?hash=" + ClaseEncriptacion.Base64ForUrlEncode(Convert.ToString(idInsertado)) + ">Link de Ocurrencia</a>" +
                                     "<div style='margin-top:10px;margin-bottom:-5px'><b>Fecha : " + ocurrencia.Fecha + "</b> </div><br/><div>Descripcion: " + ocurrencia.Descripcion + " </div>");
                                Task.Run(() =>
                                {
                                    Task oResp = EnviarCorreoAsync(remitente.Correo, direccionesEnvio, cuerpoMensaje);
                                }).ContinueWith(t => {
                                    if (t.IsCompleted)
                                    {
                                        OCU_OcurrenciaEntidad ocurrenciaEditar = new OCU_OcurrenciaEntidad();
                                        ocurrenciaEditar.Id = idInsertado;
                                        ocurrenciaEditar.Enviado = 1;
                                        bool respuest = ocurrenciaBL.EditarEstadoEnvioOcurrencia(ocurrenciaEditar);
                                    }
                                })/*.GetAwaiter().GetResult()*/;
                            }
                           
                        }
                  
                    }
                  
                    mensaje = "Ocurrencia registrada";
                    respuesta = true;
                }
                else
                {
                    mensaje = "No se pudo registrar la ocurrencia";
                }
            }catch(Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje,respuesta,data=ocurrencia });
        }
        [HttpPost]
        public ActionResult GetListadoOcurrenciaReporteJson(int[] ArraySalaId, DateTime fechaIni,DateTime fechaFin)
        {
            string mensaje = "";
            bool respuesta = false;
            List<OCU_OcurrenciaEntidad> lista = new List<OCU_OcurrenciaEntidad>();
            string condicion = "";
            try
            {
                string accion = "PermisoListadoReporteOcurrencias";
                String busqueda = "";
                busqueda = funciones.consulta("PermisoUsuario", @"
                                                                SELECT [WEB_PRolID],[WEB_RolID],[WEB_PRolFechaRegistro]
                                                                FROM [dbo].[SEG_PermisoRol] 
                                                                left join [SEG_Permiso] on [SEG_Permiso].[WEB_PermID]=[SEG_PermisoRol].[WEB_PermID]
                                                                where [SEG_PermisoRol].WEB_RolID =" + (int)Session["rol"] +
                                                                                        " and [SEG_Permiso].[WEB_PermNombre]='" + accion + "'"

                                                                         );
                condicion = " [CodSala] in(" + "'" + String.Join("','", ArraySalaId) + "'" + ") and ";
                if (busqueda.Length < 3)
                {
                     condicion = " [UsuarioReg]=" + Convert.ToInt32(Session["UsuarioID"])+" and ";
                }
                lista = ocurrenciaBL.GetListadoOcurrencia(fechaIni, fechaFin, condicion);
                foreach(var ocurrencia in lista)
                {
                    ocurrencia.Hash = ClaseEncriptacion.Base64ForUrlEncode(Convert.ToString(ocurrencia.Id));
                }

                mensaje = "Listando Registros";
                respuesta = true;
            }
            catch(Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new {mensaje,respuesta,data=lista });
        }
        [HttpPost]
        public ActionResult GetListadoOcurrenciaReporteExcelJson(int[] ArraySalaId, DateTime fechaIni, DateTime fechaFin)
        {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<OCU_OcurrenciaEntidad> lista = new List<OCU_OcurrenciaEntidad>();
            var strElementos = String.Empty;
            var salasSeleccionadas = String.Empty;
            string condicion = "";
            try
            {
                string accion = "PermisoListadoReporteOcurrencias";
                String busqueda = "";
                busqueda = funciones.consulta("PermisoUsuario", @"
                                                                SELECT [WEB_PRolID],[WEB_RolID],[WEB_PRolFechaRegistro]
                                                                FROM [dbo].[SEG_PermisoRol] 
                                                                left join [SEG_Permiso] on [SEG_Permiso].[WEB_PermID]=[SEG_PermisoRol].[WEB_PermID]
                                                                where [SEG_PermisoRol].WEB_RolID =" + (int)Session["rol"] +
                                                                                        " and [SEG_Permiso].[WEB_PermNombre]='" + accion + "'"

                                                                         );
                condicion = " [CodSala] in(" + "'" + String.Join("','", ArraySalaId) + "'" + ") and ";
                if (busqueda.Length < 3)
                {
                    condicion = " [UsuarioReg]=" + Convert.ToInt32(Session["UsuarioID"]) + " and ";
                }
                lista = ocurrenciaBL.GetListadoOcurrencia(fechaIni, fechaFin, condicion);

                if (lista.Count > 0)
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Ocurrencias");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;

                    workSheet.Cells[3, 2].Value = "Fecha";
                    workSheet.Cells[3, 3].Value = "Nombres";
                    workSheet.Cells[3, 4].Value = "Tipo Documento";
                    workSheet.Cells[3, 5].Value = "Nro Documento";
                    workSheet.Cells[3, 6].Value = "Tipo Ocurrencia";
                    workSheet.Cells[3, 7].Value = "Descripción";
                    workSheet.Cells[3, 8].Value = "Sala";
                    workSheet.Cells[3, 9].Value = "Jefe Sala";
                    workSheet.Cells[3, 10].Value = "Se Informó A";
                    //Body of table  
                    int recordIndex = 4;
                    int total = lista.Count;

                    foreach (var registro in lista)
                    {
                        workSheet.Cells[recordIndex, 2].Value = registro.Fecha.ToString("dd-MM-yyyy hh:mm:ss");
                        workSheet.Cells[recordIndex, 3].Value = registro.ApelPat + " " + registro.ApelMat + ", " + registro.Nombres;
                        workSheet.Cells[recordIndex, 4].Value = registro.TipoDocumento.DESCRIPCION;
                        workSheet.Cells[recordIndex, 5].Value = registro.NroDoc;
                        workSheet.Cells[recordIndex, 6].Value = registro.TipoOcurrencia.Nombre;
                        workSheet.Cells[recordIndex, 7].Value = registro.Descripcion;
                        workSheet.Cells[recordIndex, 7].Style.WrapText = true;
                        workSheet.Cells[recordIndex, 8].Value = registro.Sala.Nombre;
                        workSheet.Cells[recordIndex, 9].Value = registro.JefeSala;
                        workSheet.Cells[recordIndex, 10].Value = registro.SeInformoA;
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

                    int filaFooter_ = recordIndex;
                    workSheet.Cells["B" + filaFooter_ + ":J" + filaFooter_].Merge = true;
                    workSheet.Cells["B" + filaFooter_ + ":J" + filaFooter_].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter_ + ":J" + filaFooter_].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter_ + ":J" + filaFooter_].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter_ + ":J" + filaFooter_].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter_ + ":J" + filaFooter_].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[filaFooter_, 2].Value = "Total : " + (total) + " Registros";

                    workSheet.Cells[3, 2, filaFooter_, 5].AutoFilter = true;

                    //workSheet.Column(2).AutoFit();
                    workSheet.Column(2).Width = 30;
                    workSheet.Column(3).Width = 40;
                    workSheet.Column(4).Width = 25;
                    workSheet.Column(5).Width = 20;
                    workSheet.Column(6).Width = 40;
                    workSheet.Column(7).Width = 60;
                    workSheet.Column(8).Width = 40;
                    workSheet.Column(9).Width = 40;
                    workSheet.Column(10).Width = 40;

                    excelName = "ocurrencia" + fechaIni.ToString("dd_MM_yyyy") + "_al_" + fechaFin.ToString("dd_MM_yyyy") + "_ModuloOcurrencias.xlsx";
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());

                    mensaje = "Descargando Archivo";
                    respuesta = true;
                }
                else
                {
                    mensaje = "No se encontraron registros";
                }
            }
            catch (Exception ex) {
                mensaje = ex.Message;
                respuesta = false;
            }
            return Json(new { data = base64String, excelName, respuesta }); 
        }
        public ActionResult GetListadoOcurrenciaReportePdfJson(int[] ArraySalaId, DateTime fechaIni, DateTime fechaFin)
        {
            bool respuesta = false;
            List<OCU_OcurrenciaEntidad> listaOcurrencias = new List<OCU_OcurrenciaEntidad>();
            ActionAsPdf actionPDF;
            DateTime fechaActual = DateTime.Now;
            string filename = fechaActual.Day+"_"+fechaActual.Month+"_"+fechaActual.Year + "_Ocurrencias" + ".pdf";
            int UsuarioId=(int)Session["rol"];
            string salas = String.Join("','", ArraySalaId);
            actionPDF = new ActionAsPdf("PdfOcurrenciaMultiple", new { salas =salas,fechaIni=fechaIni,fechaFin=fechaFin,UsuarioId=UsuarioId })
            {
                FileName = filename,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = "--load-error-handling ignore",
                PageMargins = { Top = 15, Left = 10, Right = 10 }
            };

            byte[] applicationPDFData = actionPDF.BuildFile(ControllerContext);
            string file = Convert.ToBase64String(applicationPDFData);
            respuesta = true;

            return Json(new { data = file, filename, respuesta });
        }
        public ActionResult GenerarOcurrenciaPdfJson(string hash)
        {
            DateTime fechaActual = DateTime.Now;
            var filename = fechaActual+"_"+hash+ ".pdf";
            var actionPDF = new Rotativa.ActionAsPdf("PdfOcurrencia", new { hash })
            {
                FileName = filename,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = "--load-error-handling ignore",
                PageMargins = { Top = 15, Left = 10, Right = 10 }
            };
            //byte[] applicationPDFData = actionPDF.BuildFile(ControllerContext);
            return actionPDF;
        }
        [HttpPost]
        public ActionResult ListadoSalaPorUsuarioJson()
        {
            int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            string mensaje = "";
            bool respuesta = false;
            var lista = new List<SalaEntidad>();
            try
            {

                lista = _salaBl.ListadoSalaPorUsuario(usuarioId);
                respuesta = true;
                mensaje = "Listando registros";
            }
            catch (Exception exp)
            {
                mensaje = exp.Message;
            }
            return Json(new { data = lista.ToList(), mensaje, respuesta });
        }
        [HttpPost]
        public ActionResult TipoDocumentoListarJson()
        {
            string mensaje = "";
            bool respuesta = false;
            var lista = new List<TipoDOIEntidad>();
            try
            {
                lista = tipoDoiBL.TipoDocumentoListarJson();
                foreach(var documento in lista)
                {
                    documento.DESCRIPCION = documento.DESCRIPCION.ToUpper();
                }
                mensaje = "Listando registros";
                respuesta = true;
            }
            catch (Exception exp)
            {
                mensaje = exp.Message ;
            }
            return Json(new { data = lista.ToList(), mensaje , respuesta});
        }
        public async Task<bool> EnviarCorreoAsync(OCU_CorreoEntidad remitente,string destinatarios,string body="")
        {
            bool respuesta = false;
            SmtpClient cliente;
            MailMessage email;
            try
            {
                string password = ClaseEncriptacion.Desencriptar(remitente.Password.Trim());
                cliente = new SmtpClient(remitente.Smtp.Trim(), remitente.Puerto)
                {
                    EnableSsl = Boolean.Parse(remitente.SSL == 1 ? "true" : "false"),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(remitente.Email.Trim(), password )
                };
                email = new MailMessage(remitente.Email.Trim(), destinatarios.Trim(), "Ocurrencia", body)
                {
                    IsBodyHtml = true,
                    BodyEncoding = System.Text.Encoding.UTF8,
                    SubjectEncoding = System.Text.Encoding.Default
                };
                await cliente.SendMailAsync(email);
                respuesta = true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool PermisoListadoReporteOcurrencias()
        {
            return true;
        }
    }
}