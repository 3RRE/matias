using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Documents;
using CapaEntidad;
using CapaEntidad.Reclamaciones;
using CapaNegocio;
using CapaNegocio.Reclamaciones;
using CapaPresentacion.Utilitarios;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QRCoder;
using Rotativa;
using S3k.Utilitario.clases_especial;

namespace CapaPresentacion.Controllers.Reclamaciones
{
    [seguridad]
    public class ReclamacionController : Controller
    {
        private readonly SalaBL _salaBl = new SalaBL();
        private readonly EmpresaBL _empresaBl = new EmpresaBL();
        REC_reclamacionBL rec_reclamacionbl = new REC_reclamacionBL();
        DestinatarioBL destinatariobl = new DestinatarioBL();
        ClaseError error = new ClaseError();
        
        // GET: REC_reclamacion


        public ActionResult ReclamacionListarVista()
        {
            return View("~/Views/Reclamaciones/ReclamacionesListaVista.cshtml");
        }

        public ActionResult ConfiguracionReclamacionesVista()
        {
            return View("~/Views/Reclamaciones/ConfiguracionReclamacionesVista.cshtml");
        }

        [seguridad(false)]
        public ActionResult ReclamacionRegistroVista(int id)
        {
            ViewBag.salaid = id;
            ViewBag.fecha = DateTime.Now.ToShortDateString();
            return View("~/Views/Reclamaciones/ReclamacionesRegistroVista.cshtml");
        }

        [seguridad(false)]
        public ActionResult ReclamacionNuevoVista(int id)
        {
            ViewBag.salaid = id;
            var sala = _salaBl.SalaListaIdJson(id);          
            var empresa = _empresaBl.EmpresaListaIdJson(sala.CodEmpresa);
            ViewBag.nombreSala = sala.Nombre;
            ViewBag.idSala = sala.CodSala;
            ViewBag.nombreEmpresa = empresa.RazonSocial;
            ViewBag.direccionEmpresa = empresa.Direccion;
            ViewBag.idEmpresa = empresa.CodEmpresa;
            ViewBag.logoEmpresa = empresa.RutaArchivoLogo;
            ViewBag.fecha = DateTime.Now.ToShortDateString();
            ViewBag.tiposala = sala.tipo;
            ViewBag.rucEmpresa = empresa.Ruc;
            ViewBag.logoSala = sala.RutaArchivoLogo;
            ViewBag.logoSala = sala.RutaArchivoLogo != "" ? "https://lh3.googleusercontent.com/d/" + sala.RutaArchivoLogo : "";
            return View("~/Views/Reclamaciones/ReclamacionesNuevoVista.cshtml");
        }

        [seguridad(false)]
        public ActionResult ReclamacionInicioVista()
        {
            ViewBag.Title = "Reclamaciones";
            return View("~/Views/Reclamaciones/ReclamacionesInicioVista.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ReclamacionListarJson()
        {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            List<REC_reclamacionEntidad> listaREC_reclamacion = new List<REC_reclamacionEntidad>();
            try
            {
                var rec_reclamacionTupla = rec_reclamacionbl.REC_reclamacionListarJson();
                error = rec_reclamacionTupla.error;
                listaREC_reclamacion = rec_reclamacionTupla.rec_reclamacionLista;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje ="No se Pudieron Listar";
                }

             }
            catch (Exception exp)
            {
                mensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = listaREC_reclamacion.ToList(), respuesta, mensaje ,mensajeConsola});
        }

        [HttpPost]
        public ActionResult ReclamacionListarxSalaFechaJson(int[] codsala, DateTime fechaini, DateTime fechafin)
        {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            List<REC_reclamacionEntidad> listaREC_reclamacion = new List<REC_reclamacionEntidad>();
            int cantElementos = (codsala == null) ? 0 : codsala.Length;
            var strElementos = String.Empty;
            try
            {
                if (cantElementos > 0)
                {
                    strElementos = " sala_id in(" + "'" + String.Join("','", codsala) + "'" + ") and ";
                }
                var rec_reclamacionTupla = rec_reclamacionbl.REC_reclamacionListarxSalaFechaJson(strElementos, fechaini, fechafin);
                error = rec_reclamacionTupla.error;
                listaREC_reclamacion = rec_reclamacionTupla.rec_reclamacionLista;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = listaREC_reclamacion.ToList(), respuesta, mensaje, mensajeConsola });
        }

        [HttpPost]
        public ActionResult ReporteReclamacionDescargarExcelJson(int[] codsala, int[] ArrayReclamacionesId, DateTime fechaini, DateTime fechafin)
        {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<REC_reclamacionEntidad> listaREC_reclamacion = new List<REC_reclamacionEntidad>();
            int cantElementos = (codsala == null) ? 0 : codsala.Length;
            var strElementos = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;

            try
            {

                List<int> order = ArrayReclamacionesId.Select(x => Convert.ToInt32(x)).ToList();
                if (cantElementos > 0)
                {
                    for (int i = 0; i < codsala.Length; i++)
                    {
                        var salat = _salaBl.SalaListaIdJson(codsala[i]);
                        nombresala.Add(salat.Nombre);
                    }
                    salasSeleccionadas = String.Join(",", nombresala);

                    strElementos = " sala_id in(" + "'" + String.Join("','", codsala) + "'" + ") and ";
                }
                string reclamaciones = String.Join(",", ArrayReclamacionesId);
                string consulta = " id in (" + reclamaciones + ")";
                var rec_reclamacionTupla = rec_reclamacionbl.REC_reclamacionListarporIdsJson(consulta);
                //var rec_reclamacionTupla = rec_reclamacionbl.REC_reclamacionListarxSalaFechaJson(strElementos, fechaini, fechafin);
                error = rec_reclamacionTupla.error;
                listaREC_reclamacion = rec_reclamacionTupla.lista;

                Dictionary<long, REC_reclamacionEntidad> d = listaREC_reclamacion.ToDictionary(x => x.id);
                List<REC_reclamacionEntidad> ordered = order.Select(i => d[i]).ToList();

                if (error.Key.Equals(string.Empty))
                {
                    if (ordered.Count > 0)
                    {

                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        ExcelPackage excel = new ExcelPackage();

                        var workSheet = excel.Workbook.Worksheets.Add("Reclamaciones");
                        workSheet.TabColor = System.Drawing.Color.Black;
                        workSheet.DefaultRowHeight = 12;
                        //Header of table  
                        //  
                        workSheet.Row(3).Height = 20;
                        workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet.Row(3).Style.Font.Bold = true;
                        workSheet.Cells[2, 2].Value = "SALAS : " + salasSeleccionadas;

                        workSheet.Cells[3, 2].Value = "ID";
                        workSheet.Cells[3, 3].Value = "Cod. Sala";
                        workSheet.Cells[3, 4].Value = "Sala";
                        workSheet.Cells[3, 5].Value = "Cod. Reclamo";
                        workSheet.Cells[3, 6].Value = "Nro. Doc.";
                        workSheet.Cells[3, 7].Value = "Cliente";
                        workSheet.Cells[3, 8].Value = "Telefono";
                        workSheet.Cells[3, 9].Value = "Correo";
                        workSheet.Cells[3, 10].Value = "Fecha Reg.";
                        workSheet.Cells[3, 11].Value = "Tipo Bien";
                        workSheet.Cells[3, 12].Value = "Tipo Reclamo";
                        //Body of table  
                        //  
                        int recordIndex = 4;
                        int total = ordered.Count;
                        foreach (var registro in ordered)
                        {
                            workSheet.Cells[recordIndex, 2].Value = registro.id;
                            workSheet.Cells[recordIndex, 3].Value = registro.sala_id;
                            workSheet.Cells[recordIndex, 4].Value = registro.local_nombre;
                            workSheet.Cells[recordIndex, 5].Value = registro.codigo;
                            workSheet.Cells[recordIndex, 6].Value = registro.documento;
                            workSheet.Cells[recordIndex, 7].Value = registro.nombre;
                            workSheet.Cells[recordIndex, 8].Value = registro.telefono;
                            workSheet.Cells[recordIndex, 9].Value = registro.correo;
                            workSheet.Cells[recordIndex, 10].Value = registro.fecha.ToString("dd-MM-yyyy hh:mm:ss tt");
                            workSheet.Cells[recordIndex, 11].Value = registro.tipo;
                            workSheet.Cells[recordIndex, 12].Value = registro.tipo_reclamo;

                            recordIndex++;
                        }
                        Color colbackground = ColorTranslator.FromHtml("#003268");
                        Color colborder = ColorTranslator.FromHtml("#074B88");

                        workSheet.Cells["B3:L3"].Style.Font.Bold = true;
                        workSheet.Cells["B3:L3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells["B3:L3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                        workSheet.Cells["B3:L3"].Style.Font.Color.SetColor(Color.White);

                        workSheet.Cells["B3:L3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells["B3:L3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells["B3:L3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells["B3:L3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        workSheet.Cells["B3:L3"].Style.Border.Top.Color.SetColor(colborder);
                        workSheet.Cells["B3:L3"].Style.Border.Left.Color.SetColor(colborder);
                        workSheet.Cells["B3:L3"].Style.Border.Right.Color.SetColor(colborder);
                        workSheet.Cells["B3:L3"].Style.Border.Bottom.Color.SetColor(colborder);

                        //workSheet.Cells.AutoFitColumns(60, 250);//overloaded min and max lengths
                        int filasagregadas = 3;
                        total = filasagregadas + total;

                        workSheet.Cells["B4:B" + total].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        workSheet.Cells["B2:L2"].Merge = true;
                        workSheet.Cells["B2:L2"].Style.Font.Bold = true;

                        int filaFooter_ = total + 1;

                        workSheet.Cells["B" + filaFooter_ + ":L" + filaFooter_].Merge = true;
                        workSheet.Cells["B" + filaFooter_ + ":L" + filaFooter_].Style.Font.Bold = true;
                        workSheet.Cells["B" + filaFooter_ + ":L" + filaFooter_].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells["B" + filaFooter_ + ":L" + filaFooter_].Style.Fill.BackgroundColor.SetColor(colbackground);
                        workSheet.Cells["B" + filaFooter_ + ":L" + filaFooter_].Style.Font.Color.SetColor(Color.White);
                        workSheet.Cells["B" + filaFooter_ + ":L" + filaFooter_].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet.Cells[filaFooter_, 2].Value = "Total : " + (total - filasagregadas) + " Registros";

                        int filaultima = total;
                        workSheet.Cells[3, 2, filaultima, 12].AutoFilter = true;

                        workSheet.Column(2).AutoFit();
                        workSheet.Column(3).Width = 15;
                        workSheet.Column(4).Width = 30;
                        workSheet.Column(5).Width = 20;
                        workSheet.Column(6).Width = 24;
                        workSheet.Column(7).Width = 35;
                        workSheet.Column(8).Width = 25;
                        workSheet.Column(9).Width = 27;
                        workSheet.Column(10).Width = 24;
                        workSheet.Column(11).Width = 24;
                        workSheet.Column(12).Width = 24;
                        excelName = "Reclamaciones_" + fechaini.ToString("dd_MM_yyyy") + "_al_" + fechafin.ToString("dd_MM_yyyy") + "_.xlsx";
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
        
       
        [HttpPost]
        public ActionResult ReclamacionIdObtenerJson(Int64 id)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            var imagen = string.Empty;
            REC_reclamacionEntidad rec_reclamacion = new REC_reclamacionEntidad();
            SalaEntidad sala = new SalaEntidad();
            try
            {
                var rec_reclamacionTupla = rec_reclamacionbl.REC_reclamacionIdObtenerJson(id);
                error = rec_reclamacionTupla.error;
                rec_reclamacion = rec_reclamacionTupla.rec_reclamacion;

                sala = _salaBl.SalaListaIdJson(rec_reclamacion.sala_id);
                var empresa = _empresaBl.EmpresaListaIdJson(sala.CodEmpresa);
                
                imagen = empresa.RutaArchivoLogo;

                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Obteniendo Información de Registro Seleccionado";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudo Obtener La Información de Registro Seleccionado";
                }

            }
            catch (Exception exp)
            {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = rec_reclamacion, imagen, respuesta , mensaje , mensajeConsola, dataSala=sala });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ReclamacionNuevoJson(REC_reclamacionEntidad rec_reclamacion)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            Int64 idREC_reclamacionInsertado = 0;
            SalaEntidad sala = new SalaEntidad();
            EmpresaEntidad empresa = new EmpresaEntidad();
            //var correoAdministradorlocal = "";
            List<string> correosAdministradorlocal = new List<string>();
            List<DestinatarioEntidad> destinatarios = new List<DestinatarioEntidad>();

            string IdSalaAsunto = ConfigurationManager.AppSettings["IdSalaAsunto"];

            try
            {
                string path = Path.GetRandomFileName();
                path = path.Replace(".", "");
                var clave = path.Substring(0, 8).Trim() + DateTime.Now.ToString("MMddyyyyHHmmss");

                rec_reclamacion.fecha = DateTime.Now;
                sala = _salaBl.SalaListaIdJson(rec_reclamacion.sala_id);
                string[] words = sala.Nombre.Split(' ');
                var codigoletra = "";

                if (words.Length == 1)
                {
                    codigoletra += words[0].Substring(0, 3);
                }

                if (words.Length == 2)
                {
                    int i = 0;
                    foreach (var word in words)
                    {
                        if (i == 0)
                        {
                            codigoletra += word.Substring(0, 2);
                        }
                        else
                        {
                            codigoletra += word.Substring(0, 1);
                        }
                        i++;
                    }                  
                }

                if (words.Length >= 3)
                {
                    int i = 0;
                    foreach (var word in words)
                    {
                        if (i < 3)
                        {
                            codigoletra += word.Substring(0, 1);
                        }
                        i++;
                    }
                }
                //string sDato = "1";
                //sDato = sDato.ToString("00000");
                var totalessala = rec_reclamacionbl.REC_reclamacionTotalSalaJson(rec_reclamacion.sala_id);
                Int64 totalregistro = totalessala.total+1;
                rec_reclamacion.codigo = codigoletra+""+ rec_reclamacion.sala_id + "-"+(totalregistro.ToString()).PadLeft(10, '0');

                destinatarios = destinatariobl.DestinatarioListadoTipoEmailJson(2).Where(x=>x.estado==1).ToList();

                List<string> listaDestinatariosInsertar = destinatarios.Select(x => x.Email).ToList();
                if (sala.correo != "")
                {

                    if (sala.correo != null)
                    {

                        var correos = sala.correo.Split(',').ToList();

                        foreach (var item in correos)
                        {
                            listaDestinatariosInsertar.Add(item.Trim());
                            correosAdministradorlocal.Add(item.Trim());
                        }
                    }

                }
                rec_reclamacion.direcciones_adjuntas = String.Join("; ", listaDestinatariosInsertar);
                var rec_reclamacionTupla = rec_reclamacionbl.REC_reclamacionInsertarJson(rec_reclamacion);
                error = rec_reclamacionTupla.error;

                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Su reclamo ha sido recibido exitosamente.";
                    respuesta = true;
                    idREC_reclamacionInsertado = rec_reclamacionTupla.REC_reclamacionInsertado;
                    rec_reclamacion.id = idREC_reclamacionInsertado;
                    rec_reclamacion.hash = idREC_reclamacionInsertado.ToString() + clave;

                    empresa = _empresaBl.EmpresaListaIdJson(sala.CodEmpresa);
                    //if (sala.correo!="")
                    //{
                        
                    //    if (sala.correo != null)
                    //    {
                    //        correoAdministradorlocal = sala.correo.Trim();
                    //    }
                        
                    //}

                    rec_reclamacion.local_nombre = sala.Nombre;
                    rec_reclamacion.local_direccion = empresa.Direccion;
                    rec_reclamacion.razon_social = empresa.RazonSocial.ToUpper();
                    var editar = rec_reclamacionbl.REC_reclamacionEditarHashJson(rec_reclamacion);


                    Correo correo_enviar = new Correo();
                    string basepath = Request.Url.Scheme + "://" + ((Request.Url.Authority + Request.ApplicationPath).TrimEnd('/')) + "/";
                    sala.RutaArchivoLogo=sala.RutaArchivoLogo != basepath + "Content/assets/images/no_image.jpg" ? "https://drive.google.com/uc?id=" + sala.RutaArchivoLogo : "";
                    //Envio de Email a cliente y otros destinatarios
                    string srcLogoEmpresa=empresa.RutaArchivoLogo==string.Empty ? basepath + "Content/assets/images/no_image.jpg" : basepath + "Uploads/LogosEmpresas/"+empresa.RutaArchivoLogo;
                    string htmlEnvio = $@"
                                     <div style='background: rgb(250,251,63);
                                                    background: radial-gradient(circle, rgba(250,251,63,1) 0%, rgba(255,162,0,1) 100%);width: 100%;'>
                                            <table style='max-width: 500px; display: table;margin:0 auto; padding: 25px;'>
                                                <tbody>
                                                <tr>
                                                    <td colspan='2'>
                                                        <div style='text-align: center;font-family: Helvetica, Arial, sans-serif; font-size: 18px; color: #000000;'>
                                                            <h3>{sala.Nombre}</h3>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div style='text-align: center;'>
                                                            <img style='width:100px;height:100px;object-fit: cover;' src='{sala.RutaArchivoLogo}' alt='{sala.Nombre}' />
                                                        </div>
                                                    </td>
                                                    <td >
                                                        <div style='max-height:150px;text-align: center;font-family: Helvetica, Arial, sans-serif; font-size: 18px; color: #000000;'>
                                                            <p style=' margin-top: 0px;
                                                            margin-bottom: 0px;'>{empresa.RazonSocial}</p>                                                            
                                                            <p style=' margin-top: 0px;
                                                            margin-bottom: 0px;'><strong>RUC: {empresa.Ruc}</strong></p>
                                                        </div>    
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan='2'>
                                                            <div style='font-family: Helvetica, Arial, sans-serif;color: #000000;'>
                                                                <p style='text-align: center; font-size:18px'><strong>DETALLE DE RECLAMO</strong></p>
                                                                <p style=' margin-top: 0px;
                                                                margin-bottom: 0px;'><strong>Codigo de Registro:</strong> {rec_reclamacion.codigo}</p>
                                                                <p style=' margin-top: 10px;
                                                                margin-bottom: 0px;'><strong>Empresa:</strong> {empresa.RazonSocial}</p>
                                                                <p style=' margin-top: 10px;
                                                                margin-bottom: 0px;'>Estimado cliente,su reclamación ha sido registrada.</p>
                                                                <p style=' margin-top: 0px;
                                                                margin-bottom: 0px;'>Puede visualizar la informacion ingresada en el siguiente link: 
                                                                    <a style='color: #213f7e; 
                                                                    font-weight: bold; text-decoration: none;'
                                                                        href='{basepath}ReclamacionDetalle/{idREC_reclamacionInsertado.ToString()}{clave}'
                                                                        > Ver reclamación
                                                                    </a><br>
                                                                </p>
                                                                <p style=' margin-top: 10px;
                                                                margin-bottom: 0px;'>Gracias. </p>
                                                            </div>
                                                    </td>
                                                </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    ";

                    string htmlEnvioCliente = $@"
                                     <div style='background: rgb(250,251,63);
                                                    background: radial-gradient(circle, rgba(250,251,63,1) 0%, rgba(255,162,0,1) 100%);width: 100%;'>
                                            <table style='max-width: 500px; display: table;margin:0 auto; padding: 25px;'>
                                                <tbody>
                                                <tr>
                                                    <td colspan='2'>
                                                        <div style='text-align: center;font-family: Helvetica, Arial, sans-serif; font-size: 18px; color: #000000;'>
                                                            <h3>{sala.Nombre}</h3>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div style='text-align: center;'>
                                                            <img style='width:100px;height:100px;object-fit: cover;' src='{sala.RutaArchivoLogo}' alt='{sala.Nombre}' />
                                                        </div>
                                                    </td>
                                                    <td >
                                                        <div style='max-height:150px;text-align: center;font-family: Helvetica, Arial, sans-serif; font-size: 18px; color: #000000;'>
                                                            <p style=' margin-top: 0px;
                                                            margin-bottom: 0px;'>{empresa.RazonSocial}</p>                                                            
                                                            <p style=' margin-top: 0px;
                                                            margin-bottom: 0px;'><strong>RUC: {empresa.Ruc}</strong></p>
                                                        </div>    
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan='2'>
                                                            <div style='font-family: Helvetica, Arial, sans-serif;color: #000000;'>
                                                                <p style='text-align: center; font-size:18px'><strong>DETALLE DE RECLAMO</strong></p>
                                                                <p style=' margin-top: 0px;
                                                                margin-bottom: 0px;'><strong>Codigo de Registro:</strong> {rec_reclamacion.codigo}</p>
                                                                <p style=' margin-top: 10px;
                                                                margin-bottom: 0px;'><strong>Empresa:</strong> {empresa.RazonSocial}</p>
                                                                <p style=' margin-top: 10px;
                                                                margin-bottom: 0px;'>Estimado cliente,su reclamación ha sido registrada.</p>
                                                                <p style=' margin-top: 0px;
                                                                margin-bottom: 0px;'>Puede visualizar la informacion ingresada en el siguiente link: 
                                                                    <a style='color: #213f7e; 
                                                                    font-weight: bold; text-decoration: none;'
                                                                        href='{basepath}ReclamacionDetalle/{idREC_reclamacionInsertado.ToString()}{clave}'
                                                                        > Ver reclamación
                                                                    </a><br>
                                                                </p>
                                                                <p style=' margin-top: 10px;
                                                                margin-bottom: 0px;'>Gracias. </p>
                                                            </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan='2'>
                                                        <div style='text-align: center;font-family: Helvetica, Arial, sans-serif; font-size: 15px; '>
                                                            <p style='font-size: 13px;text-decoration: underline;'><strong><a style='color: #ff0000;' href='{basepath}Reclamacion/ReclamaciondesistimientoVista?hash={rec_reclamacion.hash}'>EN CASO DE DESISTIMIENTO HAGA CLICK AQUI</a></strong></p>
                                                        </div>
                                                    </td>
                                                </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    ";


                    if(rec_reclamacion.sala_id == int.Parse(IdSalaAsunto)) {
                        correo_enviar.EnviarCorreo(
                           rec_reclamacion.correo,
                            "SOMOSCASINO :: Reclamo Código " + rec_reclamacion.codigo,
                            htmlEnvioCliente,
                            true
                            );
                    } else {
                        correo_enviar.EnviarCorreo(
                             rec_reclamacion.correo,
                             "RETAIL - Reclamo Código " + rec_reclamacion.codigo,
                             htmlEnvioCliente,
                             true
                             );
                    }
                    
                   
                    var listadestinatarios = new List<dynamic>();
                    if (destinatarios.Count > 0)
                    {
                        foreach (var item in destinatarios)
                        {
                            listadestinatarios.Add(item.Email.Trim());
                        }

                        foreach (var item in correosAdministradorlocal)
                        {
                            listadestinatarios.Add(item.Trim());
                        }

                        //if (correoAdministradorlocal != "")
                        //{
                        //    listadestinatarios.Add(correoAdministradorlocal);
                        //}

                        var listac = String.Join(",", listadestinatarios);
                        Correo correo_destinatario = new Correo();

                        if(rec_reclamacion.sala_id == int.Parse(IdSalaAsunto)) {
                            correo_destinatario.EnviarCorreo(
                                listac,
                                "SOMOSCASINO :: Reclamo Código " + rec_reclamacion.codigo,
                                htmlEnvio,
                                true
                                );
                        } else {
                            correo_destinatario.EnviarCorreo(
                                listac,
                                "INTERNO - RETAIL - Reclamo Código " + rec_reclamacion.codigo,
                                   htmlEnvio,
                                true
                                );
                        }
                           
                    }
                }
                else
                {
                    mensaje = "No se Pudo insertar registro";
                    mensajeConsola = error.Value;
                }
            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ", Llame Administrador";
                respuesta = false;
            }

            return Json(new { respuesta , mensaje,mensajeConsola  });
        }

        [seguridad(false)]
        public ActionResult ReclamacionDetalle(string doc)
        {
            REC_reclamacionEntidad reclamacion = new REC_reclamacionEntidad();
            try
            {
                //Int64 id = Convert.ToInt64(doc);
                //var reclamacionTupla = reclamacionbl.reclamacionIdObtenerJson(id);
                var reclamacionTupla = rec_reclamacionbl.REC_reclamacionHashObtenerJson(doc);
                error = reclamacionTupla.error;
                reclamacion = reclamacionTupla.rec_reclamacion;
                var sala = _salaBl.SalaListaIdJson(reclamacion.sala_id);
                var empresa = _empresaBl.EmpresaListaIdJson(sala.CodEmpresa);
                var imagen = empresa.RutaArchivoLogo;
                //ViewBag.logoSala = sala.RutaArchivoLogo;
                ViewBag.logoSala = sala.RutaArchivoLogo != "" ? "https://lh3.googleusercontent.com/d/" + sala.RutaArchivoLogo : "";
                if (imagen == "")
                {
                    ViewBag.imagen = "";
                }
                else
                {
                    ViewBag.imagen = imagen;
                }

                if (error.Key.Equals(string.Empty))
                {
                    ViewBag.mensaje = "Obteniendo Información de Registro Seleccionado";
                    ViewBag.respuesta = true;
                    ViewBag.reclamacion = reclamacion;
                    ViewBag.tiposala = sala.tipo;
                    ViewBag.rucEmpresa = empresa.Ruc;
                }
                else
                {
                    ViewBag.mensajeConsola = error.Value;
                    ViewBag.mensaje = "No se Pudo Obtener La Información de Registro Seleccionado";
                    return View("~/Views/Home/Error.cshtml");
                }
            }
            catch (Exception exp)
            {
                ViewBag.mensaje = exp.Message + ", Llame Administrador";
                return View("~/Views/Home/Error.cshtml");
            }
            return View("~/Views/Reclamaciones/ReclamacionesRegistroVista.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ReclamacionEditarJson(REC_reclamacionEntidad rec_reclamacion)
        {
            bool respuesta = false;
            string mensaje = "";
            string mensajeConsola = "";
            try
            {
                var rec_reclamacionTupla = rec_reclamacionbl.REC_reclamacionEditarJson(rec_reclamacion);
                error = rec_reclamacionTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    respuesta = rec_reclamacionTupla.REC_reclamacionEditado;
                    mensaje = "Se Editó registro Correctamente";
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "Error, no se Puede Editar";
                }
            }
            catch (Exception exp)
            {
                mensaje = exp.Message +", Llame Administrador";
            }

            return Json(new { respuesta, mensaje  , mensajeConsola});
        }

        [HttpPost]
        public ActionResult ReclamacionAtencionSalaJson(REC_reclamacionEntidad rec_reclamacion)
        {
            bool respuesta = false;
            string mensaje = "";
            string mensajeConsola = "";
            REC_reclamacionEntidad rec_reclamacionID = new REC_reclamacionEntidad();
            //Adjuntos
            List<string> listaAdjuntos = new List<string>();
            string direccion = Server.MapPath("/") + Request.ApplicationPath + "/Uploads/ReclamacionesAdjuntos/";
            long tamanioMaximo = 8388608;//8Mb
            long tamanioTotal = 0;
            try
            {
                //obtener nombre de usuario
                rec_reclamacion.usuario_sala= Convert.ToString(Session["UsuarioNombre"]);
                var rec_reclamacionTupla = rec_reclamacionbl.ReclamacionAtencionSalaJson(rec_reclamacion);
                error = rec_reclamacionTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Se registro Correctamente";
                    respuesta = rec_reclamacionTupla.REC_reclamacionEditado;
                    //var rec_reclamacionIDTupla = rec_reclamacionbl.REC_reclamacionIdObtenerJson(rec_reclamacion.id);
                    //rec_reclamacionID = rec_reclamacionIDTupla.rec_reclamacion;

                    ////Adjuntos, agregar ruta completa del archivo
                    //if (!rec_reclamacionID.adjunto.Equals(string.Empty))
                    //{
                    //    String[] strlist = rec_reclamacionID.adjunto.Split(',');
                    //    foreach (var str in strlist)
                    //    {
                    //        tamanioTotal += new System.IO.FileInfo(Path.Combine(direccion,str)).Length;
                    //        listaAdjuntos.Add(Path.Combine(direccion, str));
                    //    }
                    //}
                    //if (tamanioTotal <= tamanioMaximo)
                    //{
                    //    Correo correo_enviar = new Correo();
                    //    string basepath = Request.Url.Scheme + "://" + ((Request.Url.Authority + Request.ApplicationPath).TrimEnd('/')) + "/";
                    //    correo_enviar.EnviarCorreo(
                    //             rec_reclamacionID.correo,
                    //             "RESPUESTA - Reclamo Código " + rec_reclamacionID.codigo + " SALA " + rec_reclamacionID.local_nombre,
                    //             "Detalle de Reclamo <br> " +
                    //             " Presione aqui : <a href='" + basepath + "ReclamacionDetalleRespuesta/" + rec_reclamacionID.hash + "'> Link de Reclamo " + rec_reclamacionID.codigo + " </a>" +
                    //             "<div style='margin-top:10px;margin-bottom:-5px'><b>" + rec_reclamacionID.razon_social + " </b></div><div>" + rec_reclamacionID.local_direccion + " </div>",
                    //             true,
                    //             listaAdjuntos.Count > 0 ? listaAdjuntos : null
                    //             );

                    //    respuesta = rec_reclamacionTupla.REC_reclamacionEditado;
                    //    mensaje = "Se registro Correctamente";
                    //}
                    //else
                    //{
                    //    mensajeConsola = "Tamaño maximo por mensaje sobrepasado -> tamño total de archivos en bytes : " + tamanioTotal + " - tamaño maximo en bytes : " + tamanioMaximo;
                    //    mensaje = "No se pudo realizar la actualizacion, tamaño maximo de adjuntos sobrepasado";
                    //    respuesta = false;
                    //}
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "Error, no se Puede Editar";
                }
            }
            catch (Exception exp)
            {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
            }

            return Json(new { respuesta, mensaje, mensajeConsola });
        }

        [HttpPost]
        public ActionResult ReclamacionAtencionLegalJson(REC_reclamacionEntidad rec_reclamacion)
        {
            bool respuesta = false;
            string mensaje = "";
            string mensajeConsola = "";
            REC_reclamacionEntidad rec_reclamacionID = new REC_reclamacionEntidad();
            //Adjuntos
            List<string> listaAdjuntos = new List<string>();
            string direccion = Server.MapPath("/") + Request.ApplicationPath + "/Uploads/ReclamacionesAdjuntos/";
            long tamanioMaximo = 8388608;//8Mb
            long tamanioTotal = 0;
            SalaEntidad sala = new SalaEntidad();
            EmpresaEntidad empresa = new EmpresaEntidad();
            try
            {
                rec_reclamacion.usuario_legal = Convert.ToString(Session["UsuarioNombre"]);
                rec_reclamacion.fecha_enviolegal = DateTime.Now;
                var rec_reclamacionTupla = rec_reclamacionbl.ReclamacionAtencionLegalJson(rec_reclamacion);
                error = rec_reclamacionTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    var rec_reclamacionIDTupla = rec_reclamacionbl.REC_reclamacionIdObtenerJson(rec_reclamacion.id);
                    rec_reclamacionID = rec_reclamacionIDTupla.rec_reclamacion;
                    sala = _salaBl.SalaListaIdJson(rec_reclamacionID.sala_id);
                    empresa = _empresaBl.EmpresaListaIdJson(sala.CodEmpresa);
                    string basepath = Request.Url.Scheme + "://" + ((Request.Url.Authority + Request.ApplicationPath).TrimEnd('/')) + "/";
                    sala.RutaArchivoLogo = sala.RutaArchivoLogo != basepath + "Content/assets/images/no_image.jpg" ? "https://drive.google.com/uc?id=" + sala.RutaArchivoLogo : "";
                    //Envio de Email a cliente y otros destinatarios
                    string srcLogoEmpresa = empresa.RutaArchivoLogo == string.Empty ? basepath + "Content/assets/images/no_image.jpg" : basepath + "Uploads/LogosEmpresas/" + empresa.RutaArchivoLogo;
                    string htmlEnvio = $@"
                                     <div style='background: rgb(250,251,63);
                                                    background: radial-gradient(circle, rgba(250,251,63,1) 0%, rgba(255,162,0,1) 100%);width: 100%;'>
                                            <table style='max-width: 500px; display: table;margin:0 auto; padding: 25px;'>
                                                <tbody>
                                                <tr>
                                                    <td colspan='2'>
                                                        <div style='text-align: center;font-family: Helvetica, Arial, sans-serif; font-size: 18px; color: #000000;'>
                                                            <h3>{sala.Nombre}</h3>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div style='text-align: center;'>
                                                            <img style='width:100px;height:100px;object-fit: cover;' src='{sala.RutaArchivoLogo}' alt='{sala.Nombre}' />
                                                        </div>
                                                    </td>
                                                    <td >
                                                        <div style='max-height:150px;text-align: center;font-family: Helvetica, Arial, sans-serif; font-size: 18px; color: #000000;'>
                                                            <p style=' margin-top: 0px;
                                                            margin-bottom: 0px;'>{empresa.RazonSocial}</p>                                                            
                                                            <p style=' margin-top: 0px;
                                                            margin-bottom: 0px;'><strong>RUC: {empresa.Ruc}</strong></p>
                                                            
                                                        </div>    
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan='2'>
                                                            <div style='font-family: Helvetica, Arial, sans-serif;color: #000000;'>
                                                                <p style='text-align: center; font-size:18px'><strong>DETALLE DE RECLAMO</strong></p>
                                                                <p style=' margin-top: 0px;
                                                                margin-bottom: 0px;'><strong>Codigo de Registro:</strong> {rec_reclamacionID.codigo}</p>
                                                                <p style=' margin-top: 10px;
                                                                margin-bottom: 0px;'><strong>Empresa:</strong> {empresa.RazonSocial}</p>
                                                                <p style=' margin-top: 10px;
                                                                margin-bottom: 0px;'>Estimado cliente,se ha registrado una respuesta a su reclamo.</p>
                                                                <p style=' margin-top: 0px;
                                                                margin-bottom: 0px;'>Puede visualizarlo en el siguiente link: 
                                                                    <a style='color: #213f7e; 
                                                                    font-weight: bold; text-decoration: none;'
                                                                        href='{basepath}ReclamacionDetalleRespuesta/{rec_reclamacionID.hash}'
                                                                        > Ver reclamación
                                                                    </a><br>
                                                                </p>
                                                                <p style=' margin-top: 10px;
                                                                margin-bottom: 0px;'>Gracias. </p>
                                                            </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan='2'>
                                                        <div style='text-align: center;font-family: Helvetica, Arial, sans-serif; font-size: 15px; color: #000000;'>
                                                            <p style='font-size: 13px;'><strong></strong></p>
                                                        </div>
                                                    </td>
                                                </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    ";
                    Correo correo_enviar = new Correo();
                    correo_enviar.EnviarCorreo(
                             rec_reclamacionID.correo,
                             "RESPUESTA AREA LEGAL - Reclamo Código " + rec_reclamacionID.codigo + " SALA " + rec_reclamacionID.local_nombre,
                                htmlEnvio,
                             true
                             );

                    respuesta = rec_reclamacionTupla.REC_reclamacionEditado;
                    mensaje = "Se registro Correctamente";

                    //Adjuntos, agregar ruta completa del archivo
                    //if (!rec_reclamacionID.adjunto.Equals(string.Empty))
                    //{
                    //    String[] strlist = rec_reclamacionID.adjunto.Split(',');
                    //    foreach (var str in strlist)
                    //    {
                    //        tamanioTotal += new System.IO.FileInfo(Path.Combine(direccion, str)).Length;
                    //        listaAdjuntos.Add(Path.Combine(direccion, str));
                    //    }
                    //}
                    //if (tamanioTotal <= tamanioMaximo)
                    //{
                        
                    //}
                    //else
                    //{
                    //    mensajeConsola = "Tamaño maximo por mensaje sobrepasado -> tamño total de archivos en bytes : " + tamanioTotal + " - tamaño maximo en bytes : " + tamanioMaximo;
                    //    mensaje = "No se pudo realizar la actualizacion, tamaño maximo de adjuntos sobrepasado";
                    //    respuesta = false;
                    //}

                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "Error, no se Puede Editar";
                }
            }
            catch (Exception exp)
            {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
            }

            return Json(new { respuesta, mensaje, mensajeConsola });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ReclamacionEliminarJson(Int64 id)
        {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            REC_reclamacionEntidad rec_reclamacion = new REC_reclamacionEntidad();

            try
            {
                var rec_reclamaciontupla = rec_reclamacionbl.REC_reclamacionEliminarJson(id);
                error = rec_reclamaciontupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    respuesta = rec_reclamaciontupla.rec_reclamacionEliminado;
                    mensaje = "Registro Eliminado";
                }
                else
                {
                    mensaje = "Error, no se Puede Eliminar";
                    mensajeConsola = error.Value;
                }
            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new {respuesta , mensaje ,mensajeConsola  });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ReclamacionEliminarVariosJson(Int64[] listaEliminar)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            ClaseError error = new ClaseError();
            try
            {
                for (int i = 0; i <= listaEliminar.Length - 1; i++)
                {
                    var Tupla = rec_reclamacionbl.REC_reclamacionEliminarJson(listaEliminar[i]);
                    error = Tupla.error;
                    if (error.Key.Equals(string.Empty))
                    {
                        respuesta = Tupla.rec_reclamacionEliminado;
                        mensaje = "Registros Eliminados";
                    }
                    else
                    {
                        mensaje = "Error, no se Puede Eliminar";
                        mensajeConsola = error.Value;
                        respuesta = false;
                        return Json(new {respuesta, mensaje , mensajeConsola });
                    }
                }
                respuesta = true;
            }
            catch (Exception ex)
            {
                mensaje = "Error, no se Puede Eliminar, " + ex.Message;
                respuesta = false;
            }

            return Json(new {respuesta, mensaje,mensajeConsola });
        }

        [seguridad(false)]
        public ActionResult GeneratePDF(string doc)
        {
            REC_reclamacionEntidad reclamacion = new REC_reclamacionEntidad();
            GoogleDriveApiHelperV2 helper = new GoogleDriveApiHelperV2();
            string base64String = string.Empty;
            try
            {
                //Int64 id = Convert.ToInt64(doc);
                //var reclamacionTupla = reclamacionbl.reclamacionIdObtenerJson(id);
                var reclamacionTupla = rec_reclamacionbl.REC_reclamacionHashObtenerJson(doc);
                error = reclamacionTupla.error;
                reclamacion = reclamacionTupla.rec_reclamacion;
                var sala = _salaBl.SalaListaIdJson(reclamacion.sala_id);
                var empresa = _empresaBl.EmpresaListaIdJson(sala.CodEmpresa);
                var imagen = empresa.RutaArchivoLogo;

                if (imagen == "")
                {
                    ViewBag.imagen = "";
                }
                else
                {
                    ViewBag.imagen = imagen;
                }
                if (error.Key.Equals(string.Empty))
                {
                    ViewBag.mensaje = "Obteniendo Información de Registro Seleccionado";
                    ViewBag.respuesta = true;
                    ViewBag.reclamacion = reclamacion;
                    ViewBag.tiposala = sala.tipo;
                    ViewBag.rucEmpresa = empresa.Ruc;
                }
                else
                {
                    ViewBag.mensajeConsola = error.Value;
                    ViewBag.mensaje = "No se Pudo Obtener La Información de Registro Seleccionado";
                    return View("~/Views/Home/Error.cshtml");
                }
                //UserCredential credential;

                //credential = helper.GetCredentials();

                // Create Drive API service.
                //var service = new DriveService(new BaseClientService.Initializer()
                //{
                //    HttpClientInitializer = credential,
                //    ApplicationName = "Sistema IAS",
                //});
                ViewBag.logoSala= sala.RutaArchivoLogo != "" ? "https://drive.google.com/uc?id=" + sala.RutaArchivoLogo : "";
                //if (sala.RutaArchivoLogo != null || sala.RutaArchivoLogo != "")
                //{
                //    base64String = helper.DownloadFile(service, sala.RutaArchivoLogo);
                //    ViewBag.logoSala = "data:image/png;base64," + base64String;
                //}
                //else
                //{
                //    ViewBag.logoSala = "";
                //}
            }
            catch (Exception exp)
            {
                ViewBag.mensaje = exp.Message + ", Llame Administrador";
                return View("~/Views/Home/Error.cshtml");
            }
            return View("~/Views/Reclamaciones/ReclamacionesRegistroPDFVista.cshtml");
        }
        [seguridad(false)]
        public ActionResult GeneratePDFRespuestaLegal(string doc)
        {
            REC_reclamacionEntidad reclamacion = new REC_reclamacionEntidad();
            try
            {
                var reclamacionTupla = rec_reclamacionbl.REC_reclamacionHashObtenerJson(doc);
                error = reclamacionTupla.error;
                reclamacion = reclamacionTupla.rec_reclamacion;
                var sala = _salaBl.SalaListaIdJson(reclamacion.sala_id);
                var empresa = _empresaBl.EmpresaListaIdJson(sala.CodEmpresa);
                var imagen = empresa.RutaArchivoLogo;
                ViewBag.logoSala= sala.RutaArchivoLogo != "" ? "https://drive.google.com/uc?id=" + sala.RutaArchivoLogo : "";
                if (imagen == "")
                {
                    ViewBag.imagen = "";
                }
                else
                {
                    ViewBag.imagen = imagen;
                }
                if (error.Key.Equals(string.Empty))
                {
                    ViewBag.mensaje = "Obteniendo Información de Registro Seleccionado";
                    ViewBag.respuesta = true;
                    ViewBag.reclamacion = reclamacion;
                    ViewBag.tiposala = sala.tipo;
                    ViewBag.rucEmpresa = empresa.Ruc;
                    ViewBag.mostrartodo = false;
                }
                else
                {
                    ViewBag.mensajeConsola = error.Value;
                    ViewBag.mensaje = "No se Pudo Obtener La Información de Registro Seleccionado";
                    return View("~/Views/Home/Error.cshtml");
                }
            }
            catch (Exception exp)
            {
                ViewBag.mensaje = exp.Message + ", Llame Administrador";
                return View("~/Views/Home/Error.cshtml");
            }
            return View("~/Views/Reclamaciones/ReclamacionesRespuestaPDFVista.cshtml");
        }
        [seguridad(false)]
        public ActionResult reclamacionDoc(string doc)
        {
            var filename = doc + ".pdf";
            var actionPDF = new Rotativa.ActionAsPdf("GeneratePDF", new { doc = doc })
            {
                FileName = filename,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = "--load-error-handling ignore",
                PageMargins = { Top = 15, Left = 10, Right = 10 }
            };

            byte[] applicationPDFData = actionPDF.BuildFile(ControllerContext);


            //Response.Clear();
            //MemoryStream outstream = new MemoryStream(applicationPDFData);
            //Response.ContentType = "application/pdf";
            //Response.AddHeader(
            //     "content-disposition", "attachment;filename=MyFile.pdf");
            //Response.Buffer = true;
            //outstream.WriteTo(Response.OutputStream);
            //Response.End();

            return actionPDF;
        }
        [seguridad(false)]
        public ActionResult reclamacionDocRespuestaLegal(string doc)
        {
            var filename = doc + ".pdf";
            var actionPDF = new Rotativa.ActionAsPdf("GeneratePDFRespuestaLegal", new { doc = doc })
            {
                FileName = filename,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = "--load-error-handling ignore",
                PageMargins = { Top = 15, Left = 10, Right = 10 }
            };

            byte[] applicationPDFData = actionPDF.BuildFile(ControllerContext);
            return actionPDF;
        }

        public ActionResult ReclamacionPDFDescarga(string doc)
        {
            var filename = doc + ".pdf";
            var actionPDF = new Rotativa.ActionAsPdf("GeneratePDF", new { doc = doc })
            {
                FileName = filename,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = "--load-error-handling ignore",
                PageMargins = { Top = 15, Left = 10, Right = 10 }
            };

            byte[] applicationPDFData = actionPDF.BuildFile(ControllerContext);

            return actionPDF;
        }

        [seguridad(false)]
        public ActionResult ReclamacionDetalleRespuesta(string doc)
        {
            REC_reclamacionEntidad reclamacion = new REC_reclamacionEntidad();
            try
            {
                //Int64 id = Convert.ToInt64(doc);
                //var reclamacionTupla = reclamacionbl.reclamacionIdObtenerJson(id);
                var reclamacionTupla = rec_reclamacionbl.REC_reclamacionHashObtenerJson(doc);
                error = reclamacionTupla.error;
                reclamacion = reclamacionTupla.rec_reclamacion;
                var sala = _salaBl.SalaListaIdJson(reclamacion.sala_id);
                var empresa = _empresaBl.EmpresaListaIdJson(sala.CodEmpresa);
                var imagen = empresa.RutaArchivoLogo;

                if (imagen == "")
                {
                    ViewBag.imagen = "";
                }
                else
                {
                    ViewBag.imagen = imagen;
                }

                if (error.Key.Equals(string.Empty))
                {
                    ViewBag.tiposala = sala.tipo;
                    ViewBag.mensaje = "Obteniendo Información de Registro Seleccionado";
                    ViewBag.respuesta = true;
                    ViewBag.reclamacion = reclamacion;
                    ViewBag.rucEmpresa = empresa.Ruc;
                }
                else
                {
                    ViewBag.mensajeConsola = error.Value;
                    ViewBag.mensaje = "No se Pudo Obtener La Información de Registro Seleccionado";
                    return View("~/Views/Home/Error.cshtml");
                }
                ViewBag.logoSala= sala.RutaArchivoLogo != "" ? "https://lh3.googleusercontent.com/d/" + sala.RutaArchivoLogo : "";
            }
            catch (Exception exp)
            {
                ViewBag.mensaje = exp.Message + ", Llame Administrador";
                return View("~/Views/Home/Error.cshtml");
            }
            return View("~/Views/Reclamaciones/ReclamacionesRespuestaVista.cshtml");
        }


        public ActionResult ReclamacionRespuestaPDFDescarga(string doc, bool todo=false)
        {
            var filename = doc + ".pdf";
            var actionPDF = new Rotativa.ActionAsPdf("GenerateRespuestaPDF", new { doc = doc,todo=todo })
            {
                FileName = filename,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = "--load-error-handling ignore",
                PageMargins = { Top = 15, Left = 10, Right = 10 }
            };

            byte[] applicationPDFData = actionPDF.BuildFile(ControllerContext);

            return actionPDF;
        }

        


        [seguridad(false)]
        public ActionResult GenerateRespuestaPDF(string doc, bool todo)
        {
            REC_reclamacionEntidad reclamacion = new REC_reclamacionEntidad();
            GoogleDriveApiHelperV2 helper = new GoogleDriveApiHelperV2();
            string base64String = string.Empty;
            try
            {
                //Int64 id = Convert.ToInt64(doc);
                //var reclamacionTupla = reclamacionbl.reclamacionIdObtenerJson(id);
                var reclamacionTupla = rec_reclamacionbl.REC_reclamacionHashObtenerJson(doc);
                error = reclamacionTupla.error;
                reclamacion = reclamacionTupla.rec_reclamacion;
                var sala = _salaBl.SalaListaIdJson(reclamacion.sala_id);
                var empresa = _empresaBl.EmpresaListaIdJson(sala.CodEmpresa);
                var imagen = empresa.RutaArchivoLogo;

                if (imagen == "")
                {
                    ViewBag.imagen = "";
                }
                else
                {
                    ViewBag.imagen = imagen;
                }
                if (error.Key.Equals(string.Empty))
                {
                    ViewBag.mensaje = "Obteniendo Información de Registro Seleccionado";
                    ViewBag.respuesta = true;
                    ViewBag.reclamacion = reclamacion;
                    ViewBag.mostrartodo = todo;
                    ViewBag.tiposala = sala.tipo;
                    ViewBag.rucEmpresa = empresa.Ruc;
                }
                else
                {
                    ViewBag.mensajeConsola = error.Value;
                    ViewBag.mensaje = "No se Pudo Obtener La Información de Registro Seleccionado";
                    return View("~/Views/Home/Error.cshtml");
                }
                ////Imagen de Drive
                //UserCredential credential;

                //credential = helper.GetCredentials();

                //// Create Drive API service.
                //var service = new DriveService(new BaseClientService.Initializer()
                //{
                //    HttpClientInitializer = credential,
                //    ApplicationName = "Sistema IAS",
                //});
                //if (sala.RutaArchivoLogo != null || sala.RutaArchivoLogo != "")
                //{
                //    base64String = helper.DownloadFile(service, sala.RutaArchivoLogo);
                //    ViewBag.logoSala = "data:image/png;base64,"+base64String;
                //}
                //else
                //{
                //    ViewBag.logoSala = "";
                //}
                ViewBag.logoSala= sala.RutaArchivoLogo != "" ? "https://drive.google.com/uc?id=" + sala.RutaArchivoLogo : "";
            }
            catch (Exception exp)
            {
                ViewBag.mensaje = exp.Message + ", Llame Administrador";
                return View("~/Views/Home/Error.cshtml");
            }
            return View("~/Views/Reclamaciones/ReclamacionesRespuestaPDFVista.cshtml");
        }
        [seguridad(false)]
        public ActionResult ReclamacionSubirAdjuntosJson(int reclamacionid)
        {
            HttpFileCollectionBase myBase = Request.Files;
            HttpPostedFileBase hpf = null;
            int tamanioMaximo = 4194304;
            string extension = "";
            string rutaInsertar = "";
            string direccion = Server.MapPath("/") + Request.ApplicationPath + "/Uploads/ReclamacionesAdjuntos/";
            List<string> listaNombresAdjuntos=new List<string>();
            bool respuesta = false;
            string adjuntos = "";
            string mensaje = "No se pudo realizar la acción";

            REC_reclamacionEntidad reclamacion = new REC_reclamacionEntidad();
            try
            {
                var reclamacionTupla = rec_reclamacionbl.REC_reclamacionIdObtenerJson(reclamacionid);
                reclamacion = reclamacionTupla.rec_reclamacion;
                if (!reclamacion.adjunto.Equals(string.Empty))
                {
                    String[] strlist = reclamacion.adjunto.Split(',');
                    foreach (var str in strlist)
                    {
                        listaNombresAdjuntos.Add(str);
                    }
                }
                int contador = 1;
                foreach (string file in myBase)
                {
                    hpf = Request.Files[file] as HttpPostedFileBase;
                    if (hpf.ContentLength <= tamanioMaximo)
                    {
                        extension = Path.GetExtension(hpf.FileName).ToLower();
                        string nombreArchivo = "ReclamacionAdjunto_" + reclamacionid + "_" + contador + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        rutaInsertar = Path.Combine(direccion, nombreArchivo);
                        if (!Directory.Exists(direccion))
                        {
                            System.IO.Directory.CreateDirectory(direccion);
                        }
                        hpf.SaveAs(rutaInsertar);
                        listaNombresAdjuntos.Add(nombreArchivo);
                    }
                    contador++;
                }
                reclamacion.adjunto = String.Join(",", listaNombresAdjuntos);
                var editadoTupla = rec_reclamacionbl.REC_ReclamacionEditarAdjunto(reclamacion);
                if (editadoTupla.rec_reclamacionEditado)
                {
                    adjuntos = reclamacion.adjunto;
                    respuesta = true;
                    mensaje = "Adjunto guardado";
                }
            }
            catch(Exception ex)
            {
                mensaje = ex.Message;
            }
          
            return Json(new { adjuntos, respuesta,mensaje});
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ReclamacionEliminarAdjuntoJson(int reclamacionid, string adjunto)
        {
            string adjuntos = "";
            string mensaje = "No se pudo eliminar el registro";
            bool respuesta = false;
            string direccion = Server.MapPath("/") + Request.ApplicationPath + "/Uploads/ReclamacionesAdjuntos/";
            List<string> listaNombresAdjuntos = new List<string>();
            REC_reclamacionEntidad reclamacion = new REC_reclamacionEntidad();
            try
            {
                var reclamacionTupla = rec_reclamacionbl.REC_reclamacionIdObtenerJson(reclamacionid);
                reclamacion = reclamacionTupla.rec_reclamacion;
                if (!reclamacion.adjunto.Equals(string.Empty))
                {
                    String[] strlist = reclamacion.adjunto.Split(',');
                    foreach (var str in strlist)
                    {
                        listaNombresAdjuntos.Add(str);
                    }
                }
                string path = Path.Combine(direccion, adjunto);
                if (System.IO.File.Exists(path))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(path);
                    //delete from database
                    listaNombresAdjuntos.Remove(adjunto);
                    reclamacion.adjunto = String.Join(",", listaNombresAdjuntos);
                    var editadoTupla = rec_reclamacionbl.REC_ReclamacionEditarAdjunto(reclamacion);
                    if (editadoTupla.rec_reclamacionEditado)
                    {
                        adjuntos = reclamacion.adjunto;
                        respuesta = true;
                        mensaje = "Adjunto guardado";
                    }
                }

            }
            catch(Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, adjuntos });
        }
        [seguridad(false)]
        public FileResult ReclamacionDescargarAdjunto(string fileName = "")
        {
            var direccion = Server.MapPath("/") + Request.ApplicationPath + "/Uploads/ReclamacionesAdjuntos//";
            string fullName = Path.Combine(direccion, fileName);
            byte[] fileBytes = ConvertirArchivo(fullName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        [seguridad(false)]
        byte[] ConvertirArchivo(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }
        [HttpPost]
        public ActionResult GetListadoREclamacionesReportePdfJson(int[] ArrayReclamacionesId)
        {
            bool respuesta = false;
            ActionAsPdf actionPDF;
            DateTime fechaActual = DateTime.Now;
            string filename = fechaActual.Day + "_" + fechaActual.Month + "_" + fechaActual.Year + "_Reclamaciones" + ".pdf";
            //string reclamaciones = " id in('" + String.Join("','", ArrayReclamacionesId) + "')";
            string reclamaciones = String.Join(",", ArrayReclamacionesId);

            actionPDF = new ActionAsPdf("PdfReclamacionesMultiple", new { reclamaciones = reclamaciones })
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
        [seguridad(false)]
        public ActionResult PdfReclamacionesMultiple(string reclamaciones)
        {
            List<REC_reclamacionEntidad> lista = new List<REC_reclamacionEntidad>();
            try
            {
                string consulta = " id in ("+reclamaciones+")";
                List<string> arrayIds = reclamaciones.Split(',').ToList();
                List<int> order = arrayIds.Select(x => Convert.ToInt32(x)).ToList();

                var listaTupla = rec_reclamacionbl.REC_reclamacionListarporIdsJson(consulta);
                lista = listaTupla.lista;
                Dictionary<long, REC_reclamacionEntidad> d = lista.ToDictionary(x => x.id);
                List<REC_reclamacionEntidad> ordered = order.Select(i => d[i]).ToList();
                //List<REC_reclamacionEntidad> ordered = lista.Select(i => order.Find(x))).ToList();

                foreach (var reclamacion in ordered)
                {
                    var sala = _salaBl.SalaListaIdJson(reclamacion.sala_id);
                    var empresa = _empresaBl.EmpresaListaIdJson(sala.CodEmpresa);
                    reclamacion.nombre_sala = sala.Nombre;
                    reclamacion.imagen_empresa = empresa.RutaArchivoLogo;
                    reclamacion.mostrar_todo = true;
                    reclamacion.tipo_sala = sala.tipo;
                    reclamacion.ruc_empresa = empresa.Ruc;
                    reclamacion.imagen_sala = sala.RutaArchivoLogo!=""? "https://drive.google.com/uc?id="+sala.RutaArchivoLogo:"";
                }
                ViewBag.data = ordered;
                ViewBag.respuesta = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewBag.respuesta = false;
                ViewBag.data = null;
            }


            return View("~/Views/Reclamaciones/ReclamacionesRespuestaPDFVistaMultiple.cshtml");
        }
        [HttpPost]
        public ActionResult ReclamacionIdObtenerCargoJson(int id)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            REC_reclamacionEntidad rec_reclamacion = new REC_reclamacionEntidad();
            SalaEntidad sala = new SalaEntidad();
            EmpresaEntidad empresa = new EmpresaEntidad();
            try
            {
                var rec_reclamacionTupla = rec_reclamacionbl.REC_reclamacionIdObtenerJson(id);
                error = rec_reclamacionTupla.error;
                rec_reclamacion = rec_reclamacionTupla.rec_reclamacion;
                sala = _salaBl.SalaListaIdJson(rec_reclamacion.sala_id);
                empresa = _empresaBl.EmpresaListaIdJson(sala.CodEmpresa);
                //destinatarios
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Obteniendo Información de Registro Seleccionado";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudo Obtener La Información de Registro Seleccionado";
                }

            }
            catch (Exception exp)
            {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = rec_reclamacion, respuesta, mensaje, mensajeConsola, dataSala = sala, dataEmpresa=empresa });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ReclamacionCargoPdf(int id)
        {
            bool respuesta = false;
            ActionAsPdf actionPDF;
            DateTime fechaActual = DateTime.Now;
            string filename = fechaActual.Day + "_" + fechaActual.Month + "_" + fechaActual.Year + "_ReclamacionCargo" + ".pdf";
            actionPDF = new ActionAsPdf("PdfReclamacionCargo", new { id = id })
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
        [seguridad(false)]
        public ActionResult PdfReclamacionCargo(int id)
        {
            REC_reclamacionEntidad rec_reclamacion = new REC_reclamacionEntidad();
            SalaEntidad sala = new SalaEntidad();
            EmpresaEntidad empresa = new EmpresaEntidad();
            try
            {

                var rec_reclamacionTupla = rec_reclamacionbl.REC_reclamacionIdObtenerJson(id);
                error = rec_reclamacionTupla.error;
                rec_reclamacion = rec_reclamacionTupla.rec_reclamacion;
                sala = _salaBl.SalaListaIdJson(rec_reclamacion.sala_id);
                empresa = _empresaBl.EmpresaListaIdJson(sala.CodEmpresa);
              
                ViewBag.reclamacion = rec_reclamacion;
                ViewBag.sala = sala;
                ViewBag.empresa = empresa;
                ViewBag.respuesta = true;
                ViewBag.logoSala= sala.RutaArchivoLogo != "" ? "https://drive.google.com/uc?id=" + sala.RutaArchivoLogo : "";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewBag.respuesta = false;
                ViewBag.data = null;
            }


            return View("~/Views/Reclamaciones/ReclamacionesCargoPdf.cshtml");
        }

        [seguridad(false)]
        public ActionResult ReclamacionDesistimientoVista(string hash)
        {
            REC_reclamacionEntidad reclamacion = new REC_reclamacionEntidad();
            string direccion = Server.MapPath("/") + Request.ApplicationPath + "/Uploads/ReclamacionesAdjuntos/Desistimientos/";
            try
            {
                var reclamacionTupla = rec_reclamacionbl.REC_reclamacionHashObtenerJson(hash);
                error = reclamacionTupla.error;
                reclamacion = reclamacionTupla.rec_reclamacion;
                var sala = _salaBl.SalaListaIdJson(reclamacion.sala_id);
                var empresa = _empresaBl.EmpresaListaIdJson(sala.CodEmpresa);
                var imagen = empresa.RutaArchivoLogo;
                ViewBag.logoSala = sala.RutaArchivoLogo != "" ? "https://drive.google.com/uc?id=" + sala.RutaArchivoLogo : "";
                if (imagen == "")
                {
                    ViewBag.imagen = "";
                }
                else
                {
                    ViewBag.imagen = imagen;
                }

                if (error.Key.Equals(string.Empty))
                {
                    ViewBag.mensaje = "Obteniendo Información de Registro Seleccionado";
                    ViewBag.respuesta = true;
                    ViewBag.tiposala = sala.tipo;
                    ViewBag.rucEmpresa = empresa.Ruc;
                    if (reclamacion.desistimiento == 1)
                    {
                        direccion = Path.Combine(direccion, reclamacion.ruta_firma_desistimiento);
                        if (System.IO.File.Exists(direccion))
                        {
                            byte[] bmp = ImageToBinary(direccion);
                            reclamacion.firma_desistimientobase64 = Convert.ToBase64String(bmp);
                        }
                    }
                    ViewBag.reclamacion = reclamacion;

                }
                else
                {
                    ViewBag.mensajeConsola = error.Value;
                    ViewBag.mensaje = "No se Pudo Obtener La Información de Registro Seleccionado";
                    return View("~/Views/Home/Error.cshtml");
                }
            }
            catch (Exception exp)
            {
                ViewBag.mensaje = exp.Message + ", Llame Administrador";
                return View("~/Views/Home/Error.cshtml");
            }
            return View("~/Views/Reclamaciones/ReclamacionesDesistimientoVista.cshtml");
        }
        [seguridad(false)]
        public ActionResult ReclamacionDesistimientoGuardarJson(string Firma,string NroDocumento,string hashReclamacion)
        {
            string mensaje = string.Empty;
            bool respuesta = false;
            string direccion = Server.MapPath("/") + Request.ApplicationPath + "/Uploads/ReclamacionesAdjuntos/Desistimientos/";
            try
            {
                var rec_reclamacionTupla = rec_reclamacionbl.REC_reclamacionHashObtenerJson(hashReclamacion);
                if (rec_reclamacionTupla.error.Value.Equals(string.Empty))
                {
                    //guardar desistimiento
                    REC_reclamacionEntidad reclamacion = rec_reclamacionTupla.rec_reclamacion;
                    if (reclamacion.documento == NroDocumento)
                    {
                        string nombreFirma = $@"{reclamacion.id}_{reclamacion.documento}.png";

                        reclamacion.desistimiento = 1;
                        reclamacion.ruta_firma_desistimiento = nombreFirma;
                        var editadoTupla = rec_reclamacionbl.reclamacionGuardarDesistimientoJson(reclamacion);
                        DirectoryInfo di = Directory.CreateDirectory(direccion);


                        string[] cadena = Firma.Split(',');
                        byte[] imagen = Convert.FromBase64String(cadena[1]);
                        Bitmap img = new Bitmap(ConvertByteToImg(imagen));
                        img.Save(Path.Combine(di.FullName, nombreFirma), System.Drawing.Imaging.ImageFormat.Png);
                        //img.Save(direccionFoto + "_" + fichaSintomatologica.FichaId + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                        img.Dispose();
                        mensaje = "Desestimiento Guardado";
                        respuesta = true;
                    }
                    else
                    {
                        mensaje = "El Nro de Documento Ingresado no corresponde a esta reclamación";
                    }
                }
            }
            catch(Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta });
        }
        [seguridad(false)]
        public Image ConvertByteToImg(Byte[] img)
        {
            using (var stream = new MemoryStream(img))
            {
                return Image.FromStream(stream);
            }
        }
        [seguridad(false)]
        public static byte[] ImageToBinary(string imagePath)
        {
            FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, (int)fileStream.Length);
            fileStream.Close();
            return buffer;
        }

        [seguridad(false)]
        public ActionResult GenerarQrReclamacion(string urlSala, string nombreSala, int codSala) {

            string mensaje = "Codigo QR generado correctamente.";
            bool respuesta = true;
            string base64String = string.Empty;
            string filename = string.Empty;
            try {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode($"{urlSala}", QRCodeGenerator.ECCLevel.H);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);

                using(Graphics graphics = Graphics.FromImage(qrCodeImage)) {
                    Font annotationFont = new Font(FontFamily.GenericSansSerif, 60, FontStyle.Regular, GraphicsUnit.Pixel);
                    string topAnnotation = nombreSala;
                    //string bottomAnnotation = urlSala;

                    SizeF topAnnotationSize = graphics.MeasureString(topAnnotation, annotationFont);
                    //SizeF bottomAnnotationSize = graphics.MeasureString(bottomAnnotation, annotationFont);

                    graphics.DrawString(topAnnotation, annotationFont, Brushes.Black, new PointF((qrCodeImage.Width - topAnnotationSize.Width)/2, 0));
                    //graphics.DrawString(bottomAnnotation, annotationFont, Brushes.Black, new PointF((qrCodeImage.Width - bottomAnnotationSize.Width) / 2, qrCodeImage.Height - bottomAnnotationSize.Height));
                }
                Bitmap bImage = qrCodeImage;  // Your Bitmap Image
                System.IO.MemoryStream ms = new MemoryStream();
                bImage.Save(ms, ImageFormat.Png);
                byte[] byteImage = ms.ToArray();
                base64String = Convert.ToBase64String(byteImage); // Get Base64
                filename = nombreSala+codSala.ToString();
                mensaje = "Descargando Archivo";
                respuesta = true;

            } catch(Exception ex) {
                mensaje = "Error al generar codigo QR.";
                respuesta = false;
            }

            return Json(new { mensaje, respuesta, base64String, filename });
        }



        [HttpPost]
        public ActionResult ListarCorreosxSala(int codSala)
        {
            var errormensaje = "";
            var respuesta = false;
            var sala = new SalaEntidad();
            List<dynamic> lista= new List<dynamic>();
            try
            {
                sala = _salaBl.SalaListaIdJson(codSala);

                var correos =  sala.correo.Split(',').ToList();

                int i = 0;

                foreach(var item in correos)
                {
                    dynamic correo = new
                    {
                        id = i,
                        correo = item
                    };
                    i++;
                    lista.Add(correo);
                }


                respuesta = true;
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult EditarCorreoSala(int codSala, string correo = "", int codSplit=0)
        {
            var errormensaje = "";
            var respuesta = false;
            var sala = new SalaEntidad();

            try
            {
                sala = _salaBl.SalaListaIdJson(codSala);

                var correos = sala.correo.Split(',').ToList();

                if(codSplit == 0)
                {
                    if( correo.Trim() !="")
                    {
                        correos.Add(correo);
                        errormensaje = "Correo agregado exitosamente.";
                    }
                } else
                {
                    correos.RemoveAt(codSplit);
                    errormensaje = "Correo removido exitosamente.";
                }


                sala.correo = String.Join(",", correos);

                respuesta = _salaBl.SalaModificarJson(sala);

                respuesta = true;
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new {  respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ActualizarEnviarAdjunto(long reclamacionId, int enviarAdjunto)
        {
            bool success = false;
            string message = "No se pudo realizar la actualización";
            string consoleMessage = string.Empty;

            if (reclamacionId <= 0)
            {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione una Hoja de Reclamación"
                });
            }

            try
            {
                var reclamacionTupa = rec_reclamacionbl.ActualizarEnviarAdjunto(reclamacionId, enviarAdjunto);

                ClaseError claseError = reclamacionTupa.error;
                bool enviarAdjuntoEditado = reclamacionTupa.enviarAdjuntoEditado;

                if (claseError.Key.Equals(string.Empty))
                {
                    if (enviarAdjuntoEditado)
                    {
                        success = true;
                        message = "Enviar Adjunto Actualizado";
                    }
                }
                else
                {
                    message = "Error, No se puede actualizar";
                    consoleMessage = error.Value;
                }
            }
            catch (Exception expception)
            {
                message = expception.Message;
            }

            return Json(new
            {
                success,
                message,
                consoleMessage
            });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult DescargarArchivoAdjunto(string guid, string attachament)
        {
            bool success = false;
            string message = "No se ha encontrado el archivo adjunto";

            if (string.IsNullOrEmpty(guid))
            {
                return Json(new
                {
                    success,
                    message = "Por favor, ingrese código"
                });
            }

            if (string.IsNullOrEmpty(attachament))
            {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione archivo"
                });
            }

            string filename = string.Empty;
            string data = string.Empty;

            try
            {
                string folderPath = Server.MapPath($"{Request.ApplicationPath}/Uploads/ReclamacionesAdjuntos");
                string filePath = Path.Combine(folderPath, attachament);
                string fileExtension = Path.GetExtension(filePath);

                if (System.IO.File.Exists(filePath))
                {
                    byte[] fileBytes = ConvertirArchivo(filePath);
                    data = Convert.ToBase64String(fileBytes);

                    filename = $"{guid}{fileExtension}";

                    success = true;
                    message = "Archivo Adjunto Generado";
                }
            }
            catch (Exception exception)
            {
                message = exception.Message;
            }

            JsonResult jsonResult = Json(new
            {
                success,
                message,
                filename,
                data
            });

            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

    }
}

