using CapaEntidad;
using CapaEntidad.AsistenciaCliente;
using CapaEntidad.Campañas;
using CapaNegocio;
using CapaNegocio.AsistenciaCliente;
using CapaNegocio.Campaña;
using CapaPresentacion.Models;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CapaPresentacion.Controllers.Campaña {
    [seguridad]
    public class CampaniaSorteoController : Controller {
        private SalaBL salaBl = new SalaBL();
        private CMP_SalalibreBL salalibreBl = new CMP_SalalibreBL();
        private CMP_CampañaBL campaniabl = new CMP_CampañaBL();
        private CMP_Campaña_ParametrosBL campaniaparametrosbl = new CMP_Campaña_ParametrosBL();
        private CMP_CuponesGeneradosBL cuponesBL = new CMP_CuponesGeneradosBL();
        private CMP_DetalleCuponesImpresosBL detalleCuponesImpresosBL = new CMP_DetalleCuponesImpresosBL();
        private CMP_DetalleCuponesGeneradosBL detalleCuponesGeneradosBL = new CMP_DetalleCuponesGeneradosBL();
        private CMP_ContadoresOnlineWebCuponesBL contadoresOnlineWebCuponesBL = new CMP_ContadoresOnlineWebCuponesBL();
        private EmpresaBL empresaBL = new EmpresaBL();
        private AST_ClienteBL ast_ClienteBL = new AST_ClienteBL();
        private CMP_SalasesionBL salasesionBL = new CMP_SalasesionBL();
        private CMP_impresoraBL impresoraBL = new CMP_impresoraBL();
        private CMP_MaquinaRestringidaBL maquinaRestingidaBL = new CMP_MaquinaRestringidaBL();
        private int TopeCuponesPorJugada = Convert.ToInt32(ConfigurationManager.AppSettings["TopeCuponesPorJugada"]);
        private int CodigoSalaSomosCasino = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoSalaSomosCasino"]);
        private bool MostrarWinCupones = Convert.ToBoolean(ConfigurationManager.AppSettings["MostrarWinCupones"]);

        private readonly CMP_SesionMigracionBL _sesionMigracionBL = new CMP_SesionMigracionBL();
        private readonly CMP_SesionSorteoSalaMigracionBL _sesionSorteoSalaMigracionBL = new CMP_SesionSorteoSalaMigracionBL();

        [seguridad(false)]
        [HttpPost]
        public ActionResult CampaniaSorteoParametro() {
            var errormensaje = "";
            bool respuesta = false;
            var campania = new CMP_Campania_ParametrosEntidad();
            List<SalaEntidad> listaSalas = new List<SalaEntidad>();
            List<dynamic> registro = new List<dynamic>();
            bool respuestaController = false;

            try {
                String busqueda = "";
                string accion = "Ingresar_CampaniaSorteoParametro";
                busqueda = funciones.consulta("PermisoUsuario", @"
                                                                SELECT [WEB_PRolID],[WEB_RolID],[WEB_PRolFechaRegistro]
                                                                FROM [dbo].[SEG_PermisoRol] 
                                                                left join [SEG_Permiso] on [SEG_Permiso].[WEB_PermID]=[SEG_PermisoRol].[WEB_PermID]
                                                                where [SEG_PermisoRol].WEB_RolID =" + (int)Session["rol"] +
                                                                                        " and [SEG_Permiso].[WEB_PermNombre]='" + accion + "'"
                                                                         );

                if(busqueda.Length < 3) {
                    respuestaController = false;
                    errormensaje = "No tiene permisos";

                } else {
                    respuestaController = true;
                }

                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                listaSalas = salaBl.ListadoSalaPorUsuario(usuarioId).Where(x => x.CodSala != CodigoSalaSomosCasino).ToList();
                if(listaSalas.Count > 1) {
                    respuesta = false;
                    errormensaje = "Usted debe tener asignado solo una sala para poder realizar la acción";

                } else {
                    if(listaSalas.Count == 0) {
                        respuesta = false;
                        errormensaje = "Usted debe tener asignado una sala para poder realizar la acción";
                    } else {
                        campania = campaniaparametrosbl.CampañaParametrosIdObtenerJson(listaSalas[0].CodSala);
                        respuesta = true;
                    }

                }
                registro.Add(new { campania, sala = listaSalas.FirstOrDefault() });


            } catch(Exception ex) {
                errormensaje = ex.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, data = registro.FirstOrDefault(), mensaje = errormensaje, dataPermiso = respuestaController });
        }

        [HttpPost]
        public ActionResult Ingresar_CampaniaSorteoParametro(CMP_Campania_ParametrosEntidad parametro) {
            var errormensaje = "";
            var response = false;
            try {
                parametro.fecha_reg = DateTime.Now;
                parametro.usuario_id = Convert.ToInt32(Session["UsuarioID"]);
                var registro = campaniaparametrosbl.CampañaParametrosIdObtenerJson(parametro.sala_id);
                if(registro != null) {
                    if(registro.id > 0) {
                        response = campaniaparametrosbl.CampañaParametrosEditarJson(parametro);
                    } else {
                        int id = campaniaparametrosbl.CampañaParametrosInsertarJson(parametro);
                        if(id > 0) {
                            response = true;
                        }
                    }
                }

                if(response) {
                    errormensaje = "Registrado Correctamente";
                } else {
                    errormensaje = "Error al Registrar";
                }
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta = response, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult ReporteCampaniasCuponesDescargarExcelJson(int campania_id) {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<CMP_CuponesGeneradosEntidad> lista = new List<CMP_CuponesGeneradosEntidad>();


            try {
                var campania = campaniabl.CampañaIdObtenerJson(campania_id);

                var sala = salaBl.SalaListaIdJson(campania.sala_id);

                lista = cuponesBL.GetListadoCuponesxCampania(campania_id);
                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("Campaña " + campania.nombre);
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[2, 2].Value = "SALA : " + sala.Nombre;

                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Slot";
                    workSheet.Cells[3, 4].Value = "Cliente";
                    workSheet.Cells[3, 5].Value = "Nro Documento";
                    workSheet.Cells[3, 6].Value = "Fecha Nac.";
                    workSheet.Cells[3, 7].Value = "Serie Ini.";
                    workSheet.Cells[3, 8].Value = "Serie Fin";
                    workSheet.Cells[3, 9].Value = "Fecha Reg.";
                    workSheet.Cells[3, 10].Value = "Hora Reg.";
                    workSheet.Cells[3, 11].Value = "Correo";
                    workSheet.Cells[3, 12].Value = "Cupones";
                    workSheet.Cells[3, 13].Value = "Usuario Reg.";
                    //Body of table  
                    //  
                    int recordIndex = 4;
                    int total = lista.Count;
                    List<CMP_DetalleCuponesGeneradosEntidad> listaCupones = new List<CMP_DetalleCuponesGeneradosEntidad>();
                    foreach(var registro in lista) {

                        string registroticket = string.Empty;
                        //workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                        var impresos = detalleCuponesImpresosBL.GetListadoDetalleCuponImpreso(registro.CgId);
                        listaCupones.Clear();
                        foreach(var registroimpresos in impresos) {
                            var listaCupon = detalleCuponesGeneradosBL.GetListadoDetalleCuponGenerado(registroimpresos.DetImpId);
                            foreach(var cupolista in listaCupon) {
                                listaCupones.Add(cupolista);
                            }

                        }

                        if(listaCupones.Count > 0) {
                            registroticket = string.Join(",", listaCupones.Select(x => x.DetGenId + "-" + x.Serie));
                        }

                        workSheet.Cells[recordIndex, 2].Value = registro.CgId;
                        workSheet.Cells[recordIndex, 3].Value = registro.SlotId;
                        workSheet.Cells[recordIndex, 4].Value = registro.NombreCompleto == "" ? registro.Nombre + " " + registro.ApelPat + " " + registro.ApelMat : registro.NombreCompleto; ;

                        workSheet.Cells[recordIndex, 5].Value = registro.NroDoc;
                        workSheet.Cells[recordIndex, 6].Value = registro.FechaNacimiento.ToString("dd-MM-yyyy");
                        workSheet.Cells[recordIndex, 7].Value = registro.SerieIni;
                        workSheet.Cells[recordIndex, 8].Value = registro.SerieFin;
                        workSheet.Cells[recordIndex, 9].Value = registro.Fecha.ToString("dd-MM-yyyy");
                        workSheet.Cells[recordIndex, 10].Value = registro.Hora.ToString(@"d\.hh\:mm\:ss"); ;
                        workSheet.Cells[recordIndex, 11].Value = registro.Mail;
                        workSheet.Cells[recordIndex, 12].Value = registroticket;
                        workSheet.Cells[recordIndex, 13].Value = registro.UsuarioNombre;
                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:M3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:M3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:M3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:M3"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B3:M3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:M3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:M3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:M3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B3:M3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:M3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:M3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:M3"].Style.Border.Bottom.Color.SetColor(colborder);

                    //workSheet.Cells.AutoFitColumns(60, 250);//overloaded min and max lengths

                    int filasagregadas = 3;
                    total = filasagregadas + total;

                    workSheet.Cells["B2:M2"].Merge = true;
                    workSheet.Cells["B2:M2"].Style.Font.Bold = true;

                    //workSheet.Cells["B4:M" + total].Style.WrapText = true;

                    int filaFooter_ = total + 1;
                    workSheet.Cells["B" + filaFooter_ + ":M" + filaFooter_].Merge = true;
                    workSheet.Cells["B" + filaFooter_ + ":M" + filaFooter_].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter_ + ":M" + filaFooter_].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter_ + ":M" + filaFooter_].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter_ + ":M" + filaFooter_].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter_ + ":M" + filaFooter_].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[filaFooter_, 2].Value = "Total : " + (total) + " Registros";

                    int filaultima = total;
                    workSheet.Cells[3, 2, filaultima, 13].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 20;
                    workSheet.Column(4).Width = 50;
                    workSheet.Column(5).Width = 38;
                    workSheet.Column(6).Width = 15;
                    workSheet.Column(7).Width = 20;
                    workSheet.Column(8).Width = 20;
                    workSheet.Column(9).Width = 15;
                    workSheet.Column(10).Width = 15;
                    workSheet.Column(11).Width = 35;
                    workSheet.Column(12).Width = 30;
                    workSheet.Column(13).Width = 20;
                    excelName = "Campañas_cupones " + campania.nombre + "_.xlsx";
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
        //[HttpPost]
        //public ActionResult ImprimirConsolidado(Int64 CgId)
        //{
        //    string mensaje = "";
        //    bool respuesta = false;
        //    CMP_CuponesGeneradosEntidad cupon = new CMP_CuponesGeneradosEntidad();
        //    List<CMP_DetalleCuponesImpresosEntidad> listaDetalleImpreso = new List<CMP_DetalleCuponesImpresosEntidad>();
        //    SalaEntidad sala = new SalaEntidad();
        //    EmpresaEntidad empresa = new EmpresaEntidad();
        //    AST_ClienteEntidad cliente = new AST_ClienteEntidad();
        //    string seriesString = string.Empty;
        //    object objData = new object();
        //    int cantidadCupones = 0;
        //    try
        //    {
        //        //Data Cupones
        //        cupon = cuponesBL.GetCuponGeneradoId(CgId);
        //        listaDetalleImpreso = detalleCuponesImpresosBL.GetListadoDetalleCuponImpreso(CgId);
        //        foreach(var impreso in listaDetalleImpreso)
        //        {
        //            List<CMP_DetalleCuponesGeneradosEntidad> listaDetalleGenerado = new List<CMP_DetalleCuponesGeneradosEntidad>();
        //            listaDetalleGenerado = detalleCuponesGeneradosBL.GetListadoDetalleCuponGenerado(impreso.DetImpId);
        //            seriesString += String.Join(",", listaDetalleGenerado.Select(x => x.Serie).ToList());
        //            cantidadCupones += listaDetalleGenerado.Count;
        //        }
        //        //Data Sala y Empresa
        //        sala = salaBl.SalaListaIdJson(cupon.CodSala);
        //        empresa = empresaBL.EmpresaListaIdJson(sala.CodEmpresa);
        //        //Datos de Cliente
        //        cliente = ast_ClienteBL.GetClienteID(Convert.ToInt32(cupon.ClienteId));
        //        objData = new
        //        {
        //            NombreSala=sala.Nombre,
        //            RazonSocialEmpresa=empresa.RazonSocial,
        //            RucEmpresa=empresa.Ruc,
        //            Fecha = cupon.Fecha,
        //            Serie = seriesString,
        //            SlotId = cupon.SlotId,
        //            NombreCliente = cliente.NombreCompleto == "" ? cliente.Nombre + " " + cliente.ApelPat + " " + cliente.ApelMat : cliente.NombreCompleto,
        //            DniCliente = cliente.NroDoc,
        //            CantidadCupones=cantidadCupones
        //        };
        //        respuesta = true;
        //        mensaje = "Listando Registros";
        //    }
        //    catch (Exception exp)
        //    {
        //        mensaje = exp.Message;
        //    }

        //    return Json(new { respuesta, mensaje, data = objData, CgId });
        //}      
        [seguridad(false)]
        public (bool respuesta, bool conexionExitosa, MaquinaProgresivoEntidad maquina, string mensaje) ObtenerEstadoSesionMaquinaOnline(string CodMaquina, string IpServicioOnline, long CodCliente) {
            bool respuesta = false;
            string mensaje = "";
            MaquinaProgresivoEntidad maquina = new MaquinaProgresivoEntidad();
            bool conexionExitosa = false;
            if(string.IsNullOrEmpty(IpServicioOnline)) {
                return (respuesta = false, conexionExitosa = false, maquina, mensaje = "No se configuró la url de sala");
            }
            try {
                object oEnvio = new {
                    CodMaquina,
                    CodCliente
                };
                var client = new System.Net.WebClient();
                var response = "";
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                string url = IpServicioOnline + "/servicio/ObtenerEstadoMaquinaySesionCupones?CodMaquina=" + CodMaquina + "&CodCliente=" + CodCliente;
                response = client.UploadString(url, "POST", "");
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                if(jsonObj.respuesta != null) {
                    string resp = jsonObj.respuesta;
                    if(Convert.ToBoolean(resp)) {
                        respuesta = true;
                    }
                    var maquinaOnline = jsonObj.maquina;
                    mensaje = jsonObj.mensaje;
                    maquina.Juego = maquinaOnline.Juego == null ? "" : maquinaOnline.Juego;
                    maquina.nombre_marca = maquinaOnline.Marca == null ? "" : maquinaOnline.Marca;
                    maquina.nombre_modelo = maquinaOnline.Modelo == null ? "" : maquinaOnline.Modelo;
                } else {
                    mensaje = "Error al obtener informacion de maquina";
                }
                conexionExitosa = true;
            } catch(Exception ex) {
                mensaje = ex.Message;
            }

            return (respuesta, conexionExitosa, maquina, mensaje);

        }
        //[HttpPost]
        //public ActionResult ReimprimirCupon(Int64 CgId, List<long> itemsImprimir, int IdImpresora)
        //{
        //    bool respuesta = false;
        //    string mensaje = "";
        //    CMP_CuponesGeneradosEntidad cupon = new CMP_CuponesGeneradosEntidad();
        //    List<CMP_DetalleCuponesGeneradosEntidad> listaDetalleGenerado = new List<CMP_DetalleCuponesGeneradosEntidad>();
        //    object oEnvio = new object();
        //    string urlEnvio = "";
        //    try
        //    {
        //        CMP_impresoraEntidad impresoraUsar = impresoraBL.ImpresoraIdObtenerJson(IdImpresora);
        //        if (impresoraUsar.id == 0)
        //        {
        //            respuesta = false;
        //            mensaje = "No se pudo obtener la impresora seleccionada";
        //            return Json(new { respuesta, mensaje });
        //        }
        //        cupon = cuponesBL.GetCuponGeneradoId(CgId);
        //        //reimpresion de un solo ticket
        //        //Info Sala
        //        SalaEntidad sala = salaBl.SalaListaIdJson(cupon.CodSala);
        //        //Data Sala y Empresa
        //        EmpresaEntidad empresa = empresaBL.EmpresaListaIdJson(sala.CodEmpresa);
        //        //Datos de Cliente
        //        AST_ClienteEntidad cliente = ast_ClienteBL.GetClienteID(Convert.ToInt32(cupon.ClienteId));

        //        foreach (var item in itemsImprimir)
        //        {
        //            CMP_DetalleCuponesGeneradosEntidad detalleGenerado = new CMP_DetalleCuponesGeneradosEntidad();
        //            detalleGenerado = detalleCuponesGeneradosBL.GetDetalleCuponGeneradoId(item);
        //            string[] arraySerie = detalleGenerado.Serie.Split('-');

        //            int correlativo = Convert.ToInt32(arraySerie[1]);
        //            detalleGenerado.DetGenId = correlativo;
        //            detalleGenerado.RazonSocialEmpresa = empresa.RazonSocial;
        //            detalleGenerado.RucEmpresa = empresa.Ruc;
        //            detalleGenerado.NombreSala = sala.Nombre;
        //            detalleGenerado.DniCliente = cliente.NroDoc;
        //            detalleGenerado.NombreCliente = cliente.NombreCompleto == "" ? cliente.Nombre + " " + cliente.ApelPat + " " + cliente.ApelMat : cliente.NombreCompleto;

        //            listaDetalleGenerado.Add(detalleGenerado);
        //        }
        //        cupon.DetalleCuponesGenerados = listaDetalleGenerado;
        //        //Enviar Data a Online
        //        oEnvio = new
        //        {
        //            Cupon = cupon,
        //            IpImpresora=impresoraUsar.ip,
        //            PuertoImpresora=impresoraUsar.puerto,
        //        };
        //        urlEnvio +=sala.UrlProgresivo+ "/servicio/ReImprimirCupones";
        //        var client = new System.Net.WebClient();
        //        var response = "";
        //        string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
        //        client.Headers.Add("content-type", "application/json");
        //        client.Encoding = Encoding.UTF8;
        //        string url = urlEnvio;
        //        response = client.UploadString(url, "POST", inputJson);
        //        dynamic jsonObj = JsonConvert.DeserializeObject(response);
        //        if (jsonObj.respuesta != null)
        //        {
        //            string resp = jsonObj.respuesta;
        //            if (Convert.ToBoolean(resp))
        //            {
        //                respuesta = true;
        //                string query = String.Join(",", itemsImprimir);
        //                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
        //                detalleCuponesGeneradosBL.AumentarCantidadImpresionesDetalleCuponGeneradoPorDetGenId(query, usuarioId);
        //            }
        //            mensaje = jsonObj.mensaje;
        //        }
        //        else
        //        {
        //            mensaje = "No se pudo conectar con el Servicio Online";
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        mensaje = ex.Message;
        //    }
        //    return Json(new { respuesta, mensaje });
        //}
        [HttpPost]
        public ActionResult ReimprimirTodo(Int64 CgId, int IdImpresora) {
            bool respuesta = false;
            string mensaje = "";
            CMP_CuponesGeneradosEntidad cupon = new CMP_CuponesGeneradosEntidad();
            List<CMP_DetalleCuponesGeneradosEntidad> listaDetalleGenerado = new List<CMP_DetalleCuponesGeneradosEntidad>();
            object oEnvio = new object();
            string urlEnvio = "";
            List<long> itemsEditar = new List<long>();
            const int MAXIMO_CUPONES_ENVIAR = 100;
            try {
                CMP_impresoraEntidad impresoraUsar = impresoraBL.ImpresoraIdObtenerJson(IdImpresora);
                if(impresoraUsar.id == 0) {
                    respuesta = false;
                    mensaje = "No se pudo obtener la impresora seleccionada";
                    return Json(new { respuesta, mensaje });
                }
                cupon = cuponesBL.GetCuponGeneradoId(CgId);
                //Info Sala
                SalaEntidad sala = salaBl.SalaListaIdJson(cupon.CodSala);
                if(string.IsNullOrEmpty(sala.UrlProgresivo)) {
                    return Json(new { respuesta = false, mensaje = "No se configuró la url de sala" });
                }
                //Data Sala y Empresa
                EmpresaEntidad empresa = empresaBL.EmpresaListaIdJson(sala.CodEmpresa);
                //Datos de Cliente
                AST_ClienteEntidad cliente = ast_ClienteBL.GetClienteID(Convert.ToInt32(cupon.ClienteId));
                //Obtener detalle Impresos
                List<CMP_DetalleCuponesImpresosEntidad> listaImpresos = new List<CMP_DetalleCuponesImpresosEntidad>();
                listaImpresos = detalleCuponesImpresosBL.GetListadoDetalleCuponImpreso(cupon.CgId);
                foreach(var item in listaImpresos) {
                    List<CMP_DetalleCuponesGeneradosEntidad> listaDetalleGeneradoConsulta = new List<CMP_DetalleCuponesGeneradosEntidad>();
                    listaDetalleGeneradoConsulta = detalleCuponesGeneradosBL.GetListadoDetalleCuponGenerado(item.DetImpId);
                    foreach(var detalle in listaDetalleGeneradoConsulta) {
                        itemsEditar.Add(detalle.DetGenId);
                        string[] arraySerie = detalle.Serie.Split('-');

                        int correlativo = Convert.ToInt32(arraySerie[1]);
                        detalle.RazonSocialEmpresa = empresa.RazonSocial;
                        detalle.RucEmpresa = empresa.Ruc;
                        detalle.NombreSala = sala.Nombre;
                        detalle.DniCliente = cliente.NroDoc;
                        detalle.NombreCliente = cliente.NombreCompleto == "" ? cliente.Nombre + " " + cliente.ApelPat + " " + cliente.ApelMat : cliente.NombreCompleto;
                        detalle.DetGenId = correlativo;
                        listaDetalleGenerado.Add(detalle);
                    }
                }
                int totalSolicitudes = (listaDetalleGenerado.Count / MAXIMO_CUPONES_ENVIAR);
                int residuoSolicitudes = (listaDetalleGenerado.Count % MAXIMO_CUPONES_ENVIAR);
                if(residuoSolicitudes != 0) {
                    totalSolicitudes++;
                }
                urlEnvio += sala.UrlProgresivo + "/servicio/ReImprimirCupones";

                for(int i = 1; i <= totalSolicitudes; i++) {
                    List<CMP_DetalleCuponesGeneradosEntidad> listaDetalleGeneradoEnviar = new List<CMP_DetalleCuponesGeneradosEntidad>();
                    listaDetalleGeneradoEnviar = listaDetalleGenerado.Take(MAXIMO_CUPONES_ENVIAR).ToList();
                    foreach(var detalle in listaDetalleGeneradoEnviar) {
                        listaDetalleGenerado.Remove(detalle);
                    }
                    cupon.DetalleCuponesGenerados = listaDetalleGeneradoEnviar;
                    //Enviar Data a Online
                    oEnvio = new {
                        Cupon = cupon,
                        IpImpresora = impresoraUsar.ip,
                        PuertoImpresora = impresoraUsar.puerto,
                    };
                    var client = new System.Net.WebClient();
                    var response = "";
                    string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                    client.Headers.Add("content-type", "application/json");
                    client.Encoding = Encoding.UTF8;
                    string url = urlEnvio;
                    response = client.UploadString(url, "POST", inputJson);
                    dynamic jsonObj = JsonConvert.DeserializeObject(response);
                    if(jsonObj.respuesta != null) {
                        string resp = jsonObj.respuesta;
                        if(Convert.ToBoolean(resp)) {
                            respuesta = true;
                            //string query = String.Join(",", listaDetalleGenerado.Select(x=>x.DetGenId));
                            //string query = String.Join(",", itemsEditar);
                            //int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                            //detalleCuponesGeneradosBL.AumentarCantidadImpresionesDetalleCuponGeneradoPorDetGenId(query, usuarioId);
                            mensaje = "Se enviaron los registros a Impresión";
                        } else {
                            mensaje = "Ocurrio un Error al imprimir";
                        }
                    } else {
                        mensaje = "No se pudo conectar con el Servicio Online";
                    }

                }
                string query = String.Join(",", itemsEditar);
                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                detalleCuponesGeneradosBL.AumentarCantidadImpresionesDetalleCuponGeneradoPorDetGenId(query, usuarioId);
                respuesta = true;
                mensaje = "Se enviaron los registros a Impresión";
                //cupon.DetalleCuponesGenerados = listaDetalleGenerado;
                ////Enviar Data a Online
                //oEnvio = new
                //{
                //    Cupon = cupon,
                //    IpImpresora = impresoraUsar.ip,
                //    PuertoImpresora = impresoraUsar.puerto,
                //};
                //urlEnvio += sala.UrlProgresivo + "/servicio/ReImprimirCupones";
                //var client = new System.Net.WebClient();
                //var response = "";
                //string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                //client.Headers.Add("content-type", "application/json");
                //client.Encoding = Encoding.UTF8;
                //string url = urlEnvio;
                //response = client.UploadString(url, "POST", inputJson);
                //dynamic jsonObj = JsonConvert.DeserializeObject(response);
                //if (jsonObj.respuesta != null)
                //{
                //    string resp = jsonObj.respuesta;
                //    if (Convert.ToBoolean(resp))
                //    {
                //        respuesta = true;
                //        //string query = String.Join(",", listaDetalleGenerado.Select(x=>x.DetGenId));
                //        string query = String.Join(",", itemsEditar);
                //        int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                //        detalleCuponesGeneradosBL.AumentarCantidadImpresionesDetalleCuponGeneradoPorDetGenId(query, usuarioId);
                //        mensaje = "Se enviaron los registros a Impresión";
                //    }
                //    else
                //    {
                //        mensaje = "Ocurrio un Error al imprimir";
                //    }
                //}
                //else
                //{
                //    mensaje = "No se pudo conectar con el Servicio Online";
                //}
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { respuesta, mensaje });
        }
        #region V2 de InsercionCupones
        [HttpPost]
        public ActionResult InsertarCuponeGenerarV2(CMP_CuponesGeneradosEntidad cupones) {
            string mensaje = "";
            bool respuesta = false;
            List<SalaEntidad> listaSalas = new List<SalaEntidad>();
            List<dynamic> data = new List<dynamic>();
            CMP_CuponesGeneradosEntidad cuponesgenerado = new CMP_CuponesGeneradosEntidad();
            List<CMP_DetalleCuponesGeneradosEntidad> lista = new List<CMP_DetalleCuponesGeneradosEntidad>();
            const int MAQUINA_RESTRINGIDA = 1;
            int cantReal = 0;
            try {
                int impresora_id = cupones.impresora_id;
                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                listaSalas = salaBl.ListadoSalaPorUsuario(usuarioId).Where(x => x.CodSala != CodigoSalaSomosCasino).ToList();
                if(listaSalas.Count != 1) {
                    respuesta = false;
                    mensaje = "Este usuario solo puede estar asignado a una sala para poder realizar la acción.";
                    return Json(new { respuesta, mensaje, data = data.FirstOrDefault() });
                }
                DateTime fecha = DateTime.Now;
                var parametros = campaniaparametrosbl.CampañaParametrosIdObtenerJson(listaSalas[0].CodSala);

                if(parametros == null) {
                    respuesta = false;
                    mensaje = "No se Configuro Parametros de Sorteo";
                    return Json(new { respuesta, mensaje, data = data.FirstOrDefault() });
                }
                if(MostrarWinCupones) {
                    if(parametros.condicion_juego > cupones.Win || cupones.Win == 0 || parametros.condicion_juego == 0) {
                        respuesta = false;
                        mensaje = "Los valores de Win y Coin Out no pueden ser cero y Win no puede ser menor que el parametro de Sorteo (" + parametros.condicion_juego + ")";
                        return Json(new { respuesta, mensaje, data = data.FirstOrDefault() });
                    }
                }
                //Obtener Data Impresora///OJO editar esta parte
                CMP_impresoraEntidad impresoraUsar = impresoraBL.ImpresoraIdObtenerJson(impresora_id);
                if(impresoraUsar.id == 0) {
                    respuesta = false;
                    mensaje = "No se pudo obtener la impresora seleccionada";
                    return Json(new { respuesta, mensaje, data = data.FirstOrDefault() });
                }
                //Consultar si maquina se encuentra restringida
                CMP_MaquinaRestringidaEntidad maquinaRestringida = maquinaRestingidaBL.GetMaquinaRestringidaSalaPorSalaYMaquina(listaSalas[0].CodSala, cupones.SlotId.Trim());
                if(maquinaRestringida.CodMaquina != null) {
                    if(maquinaRestringida.Restringido.Equals(MAQUINA_RESTRINGIDA)) {
                        respuesta = false;
                        mensaje = "La maquina :" + cupones.SlotId + " se encuentra RESTRINGIDA";
                        return Json(new { respuesta, mensaje, data = data.FirstOrDefault() });
                    }
                }
                if(parametros.condicion_juego == 0) {
                    respuesta = false;
                    mensaje = "La condicion de Juego no puede ser 0";
                    return Json(new { respuesta, mensaje, data = data.FirstOrDefault() });
                }
                int cant = Convert.ToInt32(cupones.Win / parametros.condicion_juego);
                cantReal = cant;
                cant = cant <= TopeCuponesPorJugada ? cant : TopeCuponesPorJugada;
                cupones.CodSala = parametros.sala_id;
                cupones.UsuarioId = usuarioId;
                cupones.Parametro = parametros.condicion_tipo;
                cupones.ValorJuego = parametros.condicion_juego;
                cupones.CantidadCupones = cant;
                cupones.Fecha = fecha;
                cupones.Hora = fecha.TimeOfDay;
                cupones.CodSala = listaSalas[0].CodSala;
                cupones.Estado = 1;

                var codigoletra = GetPrefijoSala(listaSalas[0].Nombre);

                var respuestaMaquinaOnline = ObtenerEstadoSesionMaquinaOnline(cupones.SlotId, listaSalas[0].UrlProgresivo, cupones.ClienteId);
                if(respuestaMaquinaOnline.conexionExitosa) {
                    //Se enviara la Data Al servicio online para creacion de sesion e impresion
                    if(respuestaMaquinaOnline.respuesta == false) {
                        respuesta = false;
                        mensaje = respuestaMaquinaOnline.mensaje;
                        return Json(new { respuesta, mensaje, data = data.FirstOrDefault() });
                    }
                    cupones.Juego = respuestaMaquinaOnline.maquina.Juego;
                    cupones.Marca = respuestaMaquinaOnline.maquina.nombre_marca;
                    cupones.Modelo = respuestaMaquinaOnline.maquina.nombre_modelo;
                    string serieini = string.Empty;
                    string seriefin = string.Empty;
                    string cadena = string.Empty;
                    //Info Sala
                    SalaEntidad sala = salaBl.SalaListaIdJson(cupones.CodSala);
                    //Datos de Cliente
                    AST_ClienteEntidad cliente = ast_ClienteBL.GetClienteID(Convert.ToInt32(cupones.ClienteId));
                    cupones.Nombre = cliente.NombreCompleto == "" ? cliente.Nombre + " " + cliente.ApelPat + " " + cliente.ApelMat : cliente.NombreCompleto;
                    cupones.nombreSala = sala.Nombre;
                    cupones.NroDoc = cliente.NroDoc;
                    cupones.CgId = 0;
                    cupones.Mail = cliente.Mail;
                    //Enviar al Servicio Online
                    var respuestaOnline = EnviarDatosOnlineV2(cupones, listaSalas[0].UrlProgresivo, impresoraUsar.ip, impresoraUsar.puerto, TopeCuponesPorJugada);
                    respuesta = respuestaOnline.respuesta;
                    mensaje += ", " + respuestaOnline.mensaje;
                    data.Add(new { cuponesgenerado.CgId, serieini, seriefin, parametros.condicion_juego, cupones.CantidadCupones, lista = cadena });
                }

            } catch(Exception exp) {
                respuesta = false;
                mensaje = exp.Message;
                return Json(new { respuesta, mensaje, data = data.FirstOrDefault() });
            }

            return Json(new { respuesta, mensaje, data = data.FirstOrDefault() });
        }
        [seguridad(false)]
        public string GetPrefijoSala(string NombreSala) {
            string codigoletra = string.Empty;

            try {
                string Sala = NombreSala.ToUpper();
                string[] words = Sala.Split(' '); ;
                if(words.Length == 1) {
                    codigoletra += words[0].Substring(0, 3);
                }
                if(words.Length == 2) {
                    int i = 0;
                    foreach(var word in words) {
                        if(i == 0) {
                            codigoletra += word.Substring(0, 2);
                        } else {
                            codigoletra += word.Substring(0, 1);
                        }
                        i++;
                    }
                }
                if(words.Length >= 3) {
                    int i = 0;
                    foreach(var word in words) {
                        if(i < 3) {
                            codigoletra += word.Substring(0, 1);
                        }
                        i++;
                    }
                }
            } catch(Exception) {
                codigoletra = string.Empty;
            }
            return codigoletra;
        }
        [seguridad(false)]
        public (bool respuesta, string mensaje) EnviarDatosOnlineV2(CMP_CuponesGeneradosEntidad cupon, string urlOnline, string IpImpresora, string PuertoImpresora, int TopeCantidadCupones) {
            bool respuesta = false;
            string mensaje = "";
            object oEnvio = new object();
            if(string.IsNullOrEmpty(urlOnline)) {
                return (respuesta = false, mensaje = "No se configuró la url de sala");
            }

            try {
                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                string urlEnvio = urlOnline;

                oEnvio = new {
                    Cupon = cupon,
                    IpImpresora,
                    PuertoImpresora,
                    CodCliente = cupon.ClienteId,
                    CodMaquina = cupon.SlotId,
                    CoinOut = cupon.ValorJuego,
                    UsuarioId = usuarioId,
                    TopeCantidadCupones = TopeCantidadCupones,
                    CampaniaId = cupon.CampaniaId,
                };
                urlEnvio += "/servicio/IniciarSesionCliente";

                var client = new System.Net.WebClient();
                var response = "";
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                string url = urlEnvio;
                response = client.UploadString(url, "POST", inputJson);
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                if(jsonObj.respuesta != null) {
                    string resp = jsonObj.respuesta;
                    if(Convert.ToBoolean(resp)) {
                        respuesta = true;
                    }
                    mensaje = jsonObj.mensaje;
                } else {
                    mensaje = "No se pudo conectar con el Servicio Online";
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return (respuesta, mensaje);
        }
        #endregion
        #region Nuevos Metodos de Impresion
        [seguridad(false)]
        [HttpPost]
        public ActionResult ReimprimirCuponPorDetGenId(long DetGenId, int IdImpresora, string UrlProgresivoSala) {
            bool respuesta = false;
            string mensaje = "";
            object oEnvio = new object();
            string urlEnvio = "";
            if(string.IsNullOrEmpty(UrlProgresivoSala)) {
                return Json(new { respuesta = false, mensaje = "No se configuró la url de sala" });
            }
            try {
                CMP_impresoraEntidad impresoraUsar = impresoraBL.ImpresoraIdObtenerJson(IdImpresora);
                if(impresoraUsar.id == 0) {
                    respuesta = false;
                    mensaje = "No se pudo obtener la impresora seleccionada";
                    return Json(new { respuesta, mensaje });
                }
                oEnvio = new {
                    DetGenId = DetGenId,
                    IpImpresora = impresoraUsar.ip,
                    PuertoImpresora = impresoraUsar.puerto,
                };
                urlEnvio += UrlProgresivoSala + "/servicio/ImprimirCuponesPorDetGenId";
                var client = new System.Net.WebClient();
                var response = "";
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                string url = urlEnvio;
                response = client.UploadString(url, "POST", inputJson);
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                if(jsonObj.respuesta != null) {
                    string resp = jsonObj.respuesta;
                    if(Convert.ToBoolean(resp)) {
                        respuesta = true;
                    }
                    mensaje = jsonObj.mensaje;
                } else {
                    mensaje = "No se pudo conectar con el Servicio Online";
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { respuesta, mensaje });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ReimprimirCuponPorCod_Cont(long Cod_Cont, int IdImpresora, string UrlProgresivoSala) {
            bool respuesta = false;
            string mensaje = "";
            object oEnvio = new object();
            string urlEnvio = "";
            if(string.IsNullOrEmpty(UrlProgresivoSala)) {
                return Json(new { respuesta = false, mensaje = "No se configuró la url de sala" });
            }
            try {
                CMP_impresoraEntidad impresoraUsar = impresoraBL.ImpresoraIdObtenerJson(IdImpresora);
                if(impresoraUsar.id == 0) {
                    respuesta = false;
                    mensaje = "No se pudo obtener la impresora seleccionada";
                    return Json(new { respuesta, mensaje });
                }
                oEnvio = new {
                    Cod_Cont = Cod_Cont,
                    IpImpresora = impresoraUsar.ip,
                    PuertoImpresora = impresoraUsar.puerto,
                };
                urlEnvio += UrlProgresivoSala + "/servicio/ImprimirCuponesPorCod_Cont";
                var client = new System.Net.WebClient();
                var response = "";
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                string url = urlEnvio;
                response = client.UploadString(url, "POST", inputJson);
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                if(jsonObj.respuesta != null) {
                    string resp = jsonObj.respuesta;
                    if(Convert.ToBoolean(resp)) {
                        respuesta = true;
                    }
                    mensaje = jsonObj.mensaje;
                } else {
                    mensaje = "No se pudo conectar con el Servicio Online";
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { respuesta, mensaje });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ReimprimirCuponPorSesionId(long SesionId, int IdImpresora, string UrlProgresivoSala) {
            bool respuesta = false;
            string mensaje = "";
            object oEnvio = new object();
            string urlEnvio = "";
            if(string.IsNullOrEmpty(UrlProgresivoSala)) {
                return Json(new { respuesta = false, mensaje = "No se configuró la url de sala" });
            }
            try {
                CMP_impresoraEntidad impresoraUsar = impresoraBL.ImpresoraIdObtenerJson(IdImpresora);
                if(impresoraUsar.id == 0) {
                    respuesta = false;
                    mensaje = "No se pudo obtener la impresora seleccionada";
                    return Json(new { respuesta, mensaje });
                }
                oEnvio = new {
                    SesionId = SesionId,
                    IpImpresora = impresoraUsar.ip,
                    PuertoImpresora = impresoraUsar.puerto,
                };
                urlEnvio += UrlProgresivoSala + "/servicio/ImprimirCuponesPorSesionId";
                var client = new System.Net.WebClient();
                var response = "";
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                string url = urlEnvio;
                response = client.UploadString(url, "POST", inputJson);
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                if(jsonObj.respuesta != null) {
                    string resp = jsonObj.respuesta;
                    if(Convert.ToBoolean(resp)) {
                        respuesta = true;
                    }
                    mensaje = jsonObj.mensaje;
                } else {
                    mensaje = "No se pudo conectar con el Servicio Online";
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { respuesta, mensaje });
        }
        #endregion
        [HttpPost]
        public ActionResult ReimprimirCupon(long IdImpresion, int IdImpresora, string UrlProgresivoSala, int Tipo) {
            //Tipo 1 : Por DetGenId; Tipo 2: Por Cod_Cont; Tipo 3 : Por SesionId
            bool respuesta = false;
            string mensaje = "";
            object oEnvio = new object();
            string urlEnvio = "";
            if(string.IsNullOrEmpty(UrlProgresivoSala)) {
                return Json(new { respuesta = false, mensaje = "No se configuró la url de sala" });
            }
            try {
                CMP_impresoraEntidad impresoraUsar = impresoraBL.ImpresoraIdObtenerJson(IdImpresora);
                if(impresoraUsar.id == 0) {
                    respuesta = false;
                    mensaje = "No se pudo obtener la impresora seleccionada";
                    return Json(new { respuesta, mensaje });
                }
                if(Tipo == 1)//Por DetGenId
                {
                    oEnvio = new {
                        DetGenId = IdImpresion,
                        IpImpresora = impresoraUsar.ip,
                        PuertoImpresora = impresoraUsar.puerto,
                    };
                    urlEnvio += UrlProgresivoSala + "/servicio/ImprimirCuponesPorDetGenId";
                } else if(Tipo == 2) {
                    oEnvio = new {
                        Cod_Cont = IdImpresion,
                        IpImpresora = impresoraUsar.ip,
                        PuertoImpresora = impresoraUsar.puerto,
                    };
                    urlEnvio += UrlProgresivoSala + "/servicio/ImprimirCuponesPorCod_Cont";
                } else if(Tipo == 3) {
                    oEnvio = new {
                        SesionId = IdImpresion,
                        IpImpresora = impresoraUsar.ip,
                        PuertoImpresora = impresoraUsar.puerto,
                    };
                    urlEnvio += UrlProgresivoSala + "/servicio/ImprimirCuponesPorSesionId";
                } else {
                    respuesta = false;
                    mensaje = "No se definio el tipo de impresion";
                    return Json(new { respuesta, mensaje });
                }

                var client = new System.Net.WebClient();
                var response = "";
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                string url = urlEnvio;
                response = client.UploadString(url, "POST", inputJson);
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                if(jsonObj.respuesta != null) {
                    string resp = jsonObj.respuesta;
                    if(Convert.ToBoolean(resp)) {
                        respuesta = true;
                    }
                    mensaje = jsonObj.mensaje;
                } else {
                    mensaje = "No se pudo conectar con el Servicio Online";
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { respuesta, mensaje });
        }
        [HttpPost]
        public ActionResult ImprimirConsolidado(Int64 SesionId, string UrlProgresivoSala) {
            string mensaje = "";
            bool respuesta = false;
            CMP_CuponesGeneradosEntidad cupon = new CMP_CuponesGeneradosEntidad();
            List<CMP_DetalleCuponesImpresosEntidad> listaDetalleImpreso = new List<CMP_DetalleCuponesImpresosEntidad>();
            List<CMP_DetalleCuponesGeneradosEntidad> listaDetalleGeneradoServicio = new List<CMP_DetalleCuponesGeneradosEntidad>();

            SalaEntidad sala = new SalaEntidad();
            EmpresaEntidad empresa = new EmpresaEntidad();
            AST_ClienteEntidad cliente = new AST_ClienteEntidad();
            string seriesString = string.Empty;
            object objData = new object();
            int cantidadCupones = 0;
            try {
                //Data Cupones
                var cuponTupla = GetInfoSesionPorSesionID(SesionId, UrlProgresivoSala);
                cupon = cuponTupla.cupon;
                listaDetalleGeneradoServicio = cuponTupla.listaDetalle;
                seriesString = String.Join(",", listaDetalleGeneradoServicio.Select(x => x.Serie).ToList());
                cantidadCupones = listaDetalleGeneradoServicio.Count;
                //foreach (var impreso in cupon.DetalleCuponesImpresos)
                //{
                //    //List<CMP_DetalleCuponesGeneradosEntidad> listaDetalleGenerado = new List<CMP_DetalleCuponesGeneradosEntidad>();
                //    //listaDetalleGenerado = detalleCuponesGeneradosBL.GetListadoDetalleCuponGenerado(impreso.DetImpId);
                //    seriesString += String.Join(",", listaDetalleGeneradoServicio.Select(x => x.Serie).Where(x=>).ToList());
                //    cantidadCupones += listaDetalleGenerado.Count;
                //}
                //Data Sala y Empresa
                sala = salaBl.SalaListaIdJson(cupon.CodSala);
                empresa = empresaBL.EmpresaListaIdJson(sala.CodEmpresa);
                //Datos de Cliente
                cliente = ast_ClienteBL.GetClienteID(Convert.ToInt32(cupon.ClienteId));
                objData = new {
                    NombreSala = sala.Nombre,
                    RazonSocialEmpresa = empresa.RazonSocial,
                    RucEmpresa = empresa.Ruc,
                    Fecha = cupon.Fecha,
                    Serie = seriesString,
                    SlotId = cupon.SlotId,
                    NombreCliente = cliente.NombreCompleto == "" ? cliente.Nombre + " " + cliente.ApelPat + " " + cliente.ApelMat : cliente.NombreCompleto,
                    DniCliente = cliente.NroDoc,
                    CantidadCupones = cantidadCupones
                };
                respuesta = true;
                mensaje = "Listando Registros";
            } catch(Exception exp) {
                mensaje = exp.Message;
            }

            return Json(new { respuesta, mensaje, data = objData, SesionId });
        }
        [seguridad(false)]
        public (CMP_CuponesGeneradosEntidad cupon, List<CMP_DetalleCuponesGeneradosEntidad> listaDetalle) GetInfoSesionPorSesionID(long SesionId, string UrlProgresivoSala) {
            CMP_CuponesGeneradosEntidad cupon = new CMP_CuponesGeneradosEntidad();
            List<CMP_DetalleCuponesImpresosEntidad> listaContadores = new List<CMP_DetalleCuponesImpresosEntidad>();
            List<CMP_DetalleCuponesGeneradosEntidad> listaDetalleCupones = new List<CMP_DetalleCuponesGeneradosEntidad>();
            object oEnvio = new object();
            if(string.IsNullOrEmpty(UrlProgresivoSala)) {
                Console.WriteLine("No se configuró la url de sala");
                return (cupon, listaDetalleCupones);
            }
            try {
                oEnvio = new {
                    SesionId = SesionId
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                string mensaje = "";
                bool respuesta = false;
                var client = new System.Net.WebClient();
                var response = "";
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                string url = UrlProgresivoSala;
                url += "/servicio/ObtenerDataCompletaSesionCuponesCliente?SesionId=" + SesionId;
                response = client.UploadString(url, "POST", inputJson);
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                mensaje = jsonObj.mensaje;
                respuesta = Convert.ToBoolean(jsonObj.respuesta);
                if(respuesta) {
                    var myItem = jsonObj.data;
                    cupon = new CMP_CuponesGeneradosEntidad() {
                        CampaniaId = ManejoNulos.ManageNullInteger64(myItem.CampaniaId),
                        ClienteId = ManejoNulos.ManageNullInteger64(myItem.ClienteId),
                        ApelPat = string.Empty,
                        ApelMat = string.Empty,
                        Nombre = string.Empty,
                        NombreCompleto = ManejoNulos.ManageNullStr(myItem.NombreCliente),
                        Mail = ManejoNulos.ManageNullStr(myItem.Correo),
                        FechaNacimiento = DateTime.Now,
                        NroDoc = ManejoNulos.ManageNullStr(myItem.NroDocumento),
                        CodSala = 0,
                        nombreSala = ManejoNulos.ManageNullStr(myItem.NombreSala),
                        UsuarioId = ManejoNulos.ManageNullInteger(myItem.UsuarioIdIAS),
                        UsuarioNombre = ManejoNulos.ManageNullStr(myItem.UsuarioNombre),
                        SlotId = ManejoNulos.ManageNullStr(myItem.CodMaquina),
                        Juego = string.Empty,
                        Marca = string.Empty,
                        Modelo = string.Empty,
                        Win = 0,
                        Parametro = 0,
                        ValorJuego = ManejoNulos.ManageNullInteger(myItem.CoinOutIAS),
                        CantidadCupones = ManejoNulos.ManageNullInteger(myItem.CantidadCupones),
                        SaldoCupIni = 0,
                        SaldoCupFin = 0,
                        SerieIni = ManejoNulos.ManageNullStr(myItem.SerieIni),
                        SerieFin = ManejoNulos.ManageNullStr(myItem.SerieFin),
                        Fecha = ManejoNulos.ManageNullDate(myItem.Fecha),
                        Estado = ManejoNulos.ManageNullInteger(myItem.Terminado),
                        SesionId = ManejoNulos.ManageNullInteger64(myItem.SesionId)
                    };
                    var detContadores = myItem.ListaContadores;
                    foreach(var cont in detContadores) {
                        List<CMP_DetalleCuponesImpresosEntidad> listaDetalles = new List<CMP_DetalleCuponesImpresosEntidad>();
                        CMP_DetalleCuponesImpresosEntidad contador = new CMP_DetalleCuponesImpresosEntidad() {
                            DetImpId = 0,
                            CgId = 0,
                            CodSala = 0,
                            SerieIni = ManejoNulos.ManageNullStr(cont.SerieIni),
                            SerieFin = ManejoNulos.ManageNullStr(cont.SerieFin),
                            CantidadCuponesImpresos = ManejoNulos.ManageNullInteger(cont.CantidadCupones),
                            UltimoCuponImpreso = string.Empty,
                            CoinOutIas = ManejoNulos.ManageNullDouble(cont.CoinOutIas),
                            CodMaq = ManejoNulos.ManageNullStr(cont.CodMaq),
                            CoinOutAnterior = ManejoNulos.ManageNullDouble(cont.CoinOutAnterior),
                            CoinOut = ManejoNulos.ManageNullDouble(cont.CoinOut),
                            CurrentCredits = ManejoNulos.ManageNullDouble(cont.CurrentCredits),
                            Monto = ManejoNulos.ManageNullDecimal(cont.Monto),
                            Token = ManejoNulos.ManageNullDecimal(cont.Token),
                            FechaRegistro = ManejoNulos.ManageNullDate(cont.FechaRegistro),
                            id = 0,
                            HandPay = ManejoNulos.ManageNullDouble(cont.HandPay),
                            JackPot = ManejoNulos.ManageNullDouble(cont.JackPot),
                            HandPayAnterior = ManejoNulos.ManageNullDouble(cont.HandPayAnterior),
                            JackPotAnterior = ManejoNulos.ManageNullDouble(cont.JackPotAnterior),
                            Cod_Cont = ManejoNulos.ManageNullInteger64(cont.Cod_Cont)
                        };
                        var detGenerado = cont.ListaDetalleCupones;

                        foreach(var detall in detGenerado) {
                            CMP_DetalleCuponesGeneradosEntidad detalle = new CMP_DetalleCuponesGeneradosEntidad() {
                                DetGenId = ManejoNulos.ManageNullInteger64(detall.DetGenId),
                                DetImId = 0,
                                CodSala = 0,
                                Serie = ManejoNulos.ManageNullStr(detall.Serie),
                                CantidadImpresiones = ManejoNulos.ManageNullInteger(detall.CantidadImpresiones),
                                Fecha = ManejoNulos.ManageNullDate(detall.Fecha),
                                UsuarioId = 0
                            };
                            listaDetalleCupones.Add(detalle);
                        }
                        listaContadores.Add(contador);
                    }
                    cupon.DetalleCuponesImpresos = listaContadores;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return (cupon, listaDetalleCupones);
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult RecepcionarDataMigracion(List<CMP_SesionMigracion> sesiones, List<CMP_SesionSorteoSalaMigracion> detalles, int codSala, string nombreSala) {
            try {
                foreach(var item in sesiones) {
                    item.CodSala = codSala;
                    item.NombreSala = nombreSala;
                    if(item.TipoSesion == 1) {
                        item.NombreTipoSesion = "SISTEMA";
                    } else {
                        item.NombreTipoSesion = "CLIENTE";
                    }
                    int registrado = _sesionMigracionBL.GuardarSesion(item);
                    if(registrado > 0) {
                        var detallesSesion = detalles.Where(x => x.SesionId == item.SesionId).ToList();
                        foreach(var det in detallesSesion) {
                            det.SesionMigracionId = registrado;
                            int detalleRegistrado = _sesionSorteoSalaMigracionBL.GuardarSesionSorteoSalaMigracion(det);
                        }
                    }
                }
                return Json(new { respuesta = true });
            } catch(Exception) {

                return Json(new { respuesta = false });
            }
        }
        [seguridad(false)]
        [HttpGet]
        public ActionResult ObtenerDataSesiones(int Id) {
            try {
                var querySesiones = $" where Id>{Id}";
                var sesiones = _sesionMigracionBL.ListarSesionPorQuery(querySesiones);

                var serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;

                var result = new ContentResult {
                    Content = serializer.Serialize(sesiones),
                    ContentType = "application/json; charset=utf-8"
                };
                return result;
            } catch(Exception) {

                return Json(new { respuesta = false });
            }
        }
        [seguridad(false)]
        [HttpGet]
        public ActionResult ObtenerDetallesSesiones(string ids) {
            try {
                var queryDetalles = $" where SesionMigracionId in ({ids})";
                var detalles = _sesionSorteoSalaMigracionBL.ListarSesionSorteoSalaMigracion(queryDetalles);

                var serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;

                var result = new ContentResult {
                    Content = serializer.Serialize(detalles),
                    ContentType = "application/json; charset=utf-8"
                };
                return result;
            } catch(Exception) {
                return Json(new { respuesta = false });
            }
        }
        [seguridad(false)]
        [HttpGet]
        public ActionResult ObtenerJugadasCliente(int maximoId) {
            try {
                var query = $" where sesion.Id > {maximoId}";
                var result = _sesionSorteoSalaMigracionBL.ListarJugadasTableau(query);

                var serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;

                var data = new ContentResult {
                    Content = serializer.Serialize(result),
                    ContentType = "application/json; charset=utf-8"
                };
                return data;
            } catch(Exception) {

                return Json(new { respuesta = false });
            }
        }
    }
}
