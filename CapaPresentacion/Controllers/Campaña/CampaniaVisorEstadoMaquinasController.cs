using CapaEntidad;
using CapaEntidad.AsistenciaCliente;
using CapaEntidad.Campañas;
using CapaNegocio;
using CapaNegocio.AsistenciaCliente;
using CapaNegocio.Campaña;
using CapaPresentacion.Utilitarios;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CapaPresentacion.Controllers.Campaña
{
    [seguridad]
    public class CampaniaVisorEstadoMaquinasController : Controller
    {
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
        private SEG_UsuarioBL usuarioBL = new SEG_UsuarioBL();
        // GET: CampaniaVisorEstadoMaquinas
        public ActionResult VisorEstadoMaquinas()
        {
            return View("~/Views/Campania/VisorEstadoMaquinas.cshtml");
        }
        [HttpPost]
        public ActionResult ListarEquiposNoLibresJsonV2(string UrlProgresivoSala)
        {
            object objRespuesta = new object();
            if(string.IsNullOrEmpty(UrlProgresivoSala)) {
                return Json(new { respuesta = false, mensaje = "No se configuró la url de sala" });
            }
            try
            {
                var myclient = new MyWebClient();
                myclient.Headers.Add("content-type", "application/json");
                myclient.Encoding = Encoding.UTF8;

                string mensaje = "";
                bool respuesta = false;
                //var client = new System.Net.WebClient();
                var response = "";
                //client.Headers.Add("content-type", "application/json");
                //client.Encoding = Encoding.UTF8;
                string url = UrlProgresivoSala;
                url += "/servicio/ObtenerListadoSesionesPorEstado";
                response = myclient.UploadString(url, "POST", "");
                //response = client.UploadString(url, "POST", "");
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                mensaje = jsonObj.mensaje;
                respuesta = Convert.ToBoolean(jsonObj.respuesta);
                var items = jsonObj.data;
                List<object> listObject = new List<object>();
                SEG_UsuarioEntidad usuario = new SEG_UsuarioEntidad();
                foreach (var myItem in items)
                {
                    int usuarioId = Convert.ToInt32(myItem.UsuarioIdIAS);
                    usuario = usuarioBL.UsuarioEmpleadoIDObtenerJson(usuarioId);
                    var data = new
                    {
                        SesionId = Convert.ToInt64(myItem.SesionId),
                        CodMaquina = Convert.ToString(myItem.CodMaquina),
                        CgId = Convert.ToInt64(myItem.CgId),
                        Terminado = Convert.ToInt32(myItem.Terminado),
                        Fecha = Convert.ToDateTime(myItem.Fecha),
                        ClienteId = Convert.ToInt32(myItem.ClienteId),
                        NombreCliente = Convert.ToString(myItem.NombreCliente),
                        NombreSala = Convert.ToString(myItem.NombreSala),
                        NroDocumento = Convert.ToString(myItem.NroDocumento),
                        Estado_Envio = Convert.ToInt32(myItem.Estado_Envio),
                        UsuarioIdIAS = Convert.ToInt32(myItem.UsuarioIdIAS),
                        Prefijo = Convert.ToString(myItem.Prefijo),
                        CoinOutIAS = Convert.ToDouble(myItem.CoinOutIAS),
                        TopeCuponesxJugada = Convert.ToInt32(myItem.TopeCuponesxJugada),
                        ParametrosImpresion = Convert.ToString(myItem.ParametrosImpresion),
                        HijoTerminado=Convert.ToInt32(myItem.HijoTerminado),
                        HijoEnviado = Convert.ToInt32(myItem.HijoEnviado),
                        CantidadCupones = Convert.ToInt32(myItem.CantidadCupones),
                        CantidadJugadas = Convert.ToInt32(myItem.CantidadJugadas),
                        NombreUsuario=usuario.UsuarioNombre,
                    };
                    listObject.Add(data);
                }
                objRespuesta = new
                {
                    mensaje,
                    respuesta,
                    data = listObject
                };
            }
            catch (Exception ex)
            {
                objRespuesta = new
                {
                    mensaje = ex.Message,
                    respuesta = false
                };
            }
            return Json(objRespuesta);
        }
        [HttpPost]
        public ActionResult ObtenerDetalleEstadoMaquinaSala(string UrlProgresivoSala, string CodMaq, long CgId, long SesionId)
        {
            object objRespuesta = new object();
            object oEnvio = new object();
            List<CMP_ContadoresOnlineWebCuponesEntidad> listaContadores = new List<CMP_ContadoresOnlineWebCuponesEntidad>();
            int TotalCupones = 0;
            if(string.IsNullOrEmpty(UrlProgresivoSala)) {
                return Json(new { respuesta = false, mensaje = "No se configuró la url de sala" });
            }
            try
            {
                oEnvio = new
                {
                    CodMaquina = CodMaq,
                    CgId = CgId,
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
                url += "/servicio/ObtenerDetalleRegistrosCuponesNoEnviados";
                response = client.UploadString(url, "POST", inputJson);
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                mensaje = jsonObj.mensaje;
                respuesta = Convert.ToBoolean(jsonObj.respuesta);
                var items = jsonObj.data;
                List<object> listObject = new List<object>();
                foreach (var myItem in items)
                {
                    DateTime fechaItem = myItem.Fecha;
                    DateTime horaItem = myItem.Hora;
                    DateTime fechaRegistroItem = myItem.FechaRegistro;
                    CMP_ContadoresOnlineWebCuponesEntidad contador = new CMP_ContadoresOnlineWebCuponesEntidad()
                    {
                        Cod_Cont = myItem.Cod_Cont,
                        Cod_Cont_OL = myItem.Cod_Cont_OL,
                        Fecha = myItem.Fecha,
                        Hora = myItem.Hora,
                        CodMaq = myItem.CodMaq,
                        CodMaqMin = myItem.CodMaqMin,
                        CoinOut = myItem.CoinOut,
                        CurrentCredits = myItem.CurrentCredits,
                        Monto = myItem.Monto,
                        Token = myItem.Token,
                        CoinOutAnterior = myItem.CoinOutAnterior,
                        Estado_Oln = myItem.Estado_Oln,
                        Win = myItem.Win,
                        CantidadCupones = myItem.CantidadCupones,
                        FechaRegistro = myItem.FechaRegistro,
                        CodCliente = myItem.CodCliente,
                        CoinOutIas = myItem.CoinOutIas,
                        EstadoEnvio = myItem.Estado_Envio,
                        CodSala = myItem.CodSala,
                        HandPay=myItem.HandPay,
                        JackPot=myItem.JackPot,
                        HandPayAnterior=myItem.HandPayAnterior,
                        JackPotAnterior=myItem.JackPotAnterior,
                        //CgId=myItem.CgId,
                    };
                    listaContadores.Add(contador);
                    //var data = new
                    //{
                    //    CodMaq = myItem.CodMaq,
                    //    CgId = myItem.CgId,
                    //    TotalNoEnviados = myItem.TotalNoEnviados,
                    //};
                    //listObject.Add(data);
                }
                TotalCupones = listaContadores.Sum(x => x.CantidadCupones);
                objRespuesta = new
                {
                    mensaje,
                    respuesta,
                    data = listaContadores,
                    cantidadCupones = TotalCupones
                };
            }
            catch (Exception ex)
            {
                objRespuesta = new
                {
                    mensaje = ex.Message,
                    respuesta = false
                };
            }
            return Json(objRespuesta);
        }
        [HttpPost]
        public ActionResult LiberarMaquinaSala(string UrlProgresivoSala, string CodMaq, long CgId, long SesionId)
        {
            object objRespuesta = new object();
            object oEnvio = new object();
            List<CMP_ContadoresOnlineWebCuponesEntidad> listaContadores = new List<CMP_ContadoresOnlineWebCuponesEntidad>();
            SalaEntidad sala = new SalaEntidad();
            CMP_CuponesGeneradosEntidad cupon = new CMP_CuponesGeneradosEntidad();//
            CMP_CuponesGeneradosEntidad cupones = new CMP_CuponesGeneradosEntidad();
            List<CMP_DetalleCuponesGeneradosEntidad> lista = new List<CMP_DetalleCuponesGeneradosEntidad>();
            List<CMP_DetalleCuponesGeneradosEntidad> listaDetalleGeneradosEnviar = new List<CMP_DetalleCuponesGeneradosEntidad>();
            List<CMP_DetalleCuponesImpresosEntidad> listaDetalleCuponesImpresos = new List<CMP_DetalleCuponesImpresosEntidad>();
            if(string.IsNullOrEmpty(UrlProgresivoSala)) {
                return Json(new { respuesta = false, mensaje = "No se configuró la url de sala" });
            }
            try
            {
                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                oEnvio = new
                {
                    CodMaquina = CodMaq,
                    CgId = CgId,
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
                url += "/servicio/EditarEstadoEnvioPorMaquinayCgId";
                response = client.UploadString(url, "POST", inputJson);
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                mensaje = jsonObj.mensaje;
                respuesta = Convert.ToBoolean(jsonObj.respuesta);
                if (respuesta)
                {
                    var items = jsonObj.listaContadores;
                    List<object> listObject = new List<object>();
                    foreach (var myItem in items)
                    {
                        DateTime fechaItem = myItem.Fecha;
                        DateTime horaItem = myItem.Hora;
                        DateTime fechaRegistroItem = myItem.FechaRegistro;
                        CMP_ContadoresOnlineWebCuponesEntidad contador = new CMP_ContadoresOnlineWebCuponesEntidad()
                        {
                            Cod_Cont = myItem.Cod_Cont,
                            Cod_Cont_OL = myItem.Cod_Cont_OL,
                            Fecha = myItem.Fecha,
                            Hora = myItem.Hora,
                            CodMaq = myItem.CodMaq,
                            CodMaqMin = myItem.CodMaqMin,
                            CoinOut = myItem.CoinOut,
                            CurrentCredits = myItem.CurrentCredits,
                            Monto = myItem.Monto,
                            Token = myItem.Token,
                            CoinOutAnterior = myItem.CoinOutAnterior,
                            Estado_Oln = myItem.Estado_Oln,
                            Win = myItem.Win,
                            CantidadCupones = myItem.CantidadCupones,
                            FechaRegistro = fechaRegistroItem,
                            CodCliente = myItem.CodCliente,
                            CoinOutIas = myItem.CoinOutIas,
                            EstadoEnvio = myItem.Estado_Envio,
                            CodSala = myItem.CodSala,
                        };
                        listaContadores.Add(contador);
                    }
                    var primerContador = listaContadores.FirstOrDefault();
                    cupones = cuponesBL.GetCuponGeneradoId(CgId);
                    sala = salaBl.SalaListaIdJson(Convert.ToInt32(primerContador.CodSala));

                    //Data Sala y Empresa
                    EmpresaEntidad empresa = empresaBL.EmpresaListaIdJson(sala.CodEmpresa);
                    //Datos de Cliente
                    AST_ClienteEntidad cliente = ast_ClienteBL.GetClienteID(Convert.ToInt32(primerContador.CodCliente));

                    #region CodAlerta
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
                    double totalCuponesEditar = cupones.CantidadCupones;
                    #endregion
                    foreach (var contador in listaContadores)
                    {
                        totalCuponesEditar += contador.CantidadCupones;
                        //Insertar DetalleImpresos
                        CMP_DetalleCuponesImpresosEntidad impresos = new CMP_DetalleCuponesImpresosEntidad();
                        impresos.CodSala = sala.CodSala;
                        impresos.CgId = CgId;
                        impresos.CantidadCuponesImpresos = contador.CantidadCupones;
                        var cuponesimpresosId = detalleCuponesImpresosBL.GuardarDetalleCuponImpreso(impresos);
                        //Insertar Contador Online Web
                        contador.FechaLlegada = DateTime.Now;
                        contador.DetalleCuponesImpresos_id = cuponesimpresosId;
                        //Arreglas Fechas
                        DateTime fechaContador = contador.Fecha;
                        DateTime horaContador = contador.Hora;
                        DateTime fechaRegistroContador = contador.FechaRegistro;

                        contador.Fecha = fechaContador.ToLocalTime();
                        contador.Hora = fechaContador.ToLocalTime();
                        contador.FechaRegistro = fechaRegistroContador;
                        var insertarcontadores = contadoresOnlineWebCuponesBL.GuardarCMP_ContadoresOnlineWebCupones(contador);
                        contador.id = insertarcontadores;
                        //Generar series e insertar en detalle Cupones Generados
                        Int64 total = detalleCuponesGeneradosBL.CuponesTotalJson(sala.CodSala);
                        total = (total == 0 ? 1 : total + 1);
                        string serieini = "";
                        string seriefin = "";
                        for (int i = 0; i < contador.CantidadCupones; i++)
                        {
                            CMP_DetalleCuponesGeneradosEntidad detalle = new CMP_DetalleCuponesGeneradosEntidad();
                            Int64 totalregistro = total + i;
                            var codigo = codigoletra + "-" + (totalregistro.ToString()).PadLeft(10, '0');
                            detalle.Serie = codigo;
                            detalle.DetImId = cuponesimpresosId;
                            detalle.CodSala = sala.CodSala;
                            detalle.Fecha = DateTime.Now;
                            detalle.CantidadImpresiones = 0;
                            detalle.UsuarioId = usuarioId;
                            detalleCuponesGeneradosBL.GuardarDetalleCuponGenerado(detalle);
                            if (i == 0)
                            {
                                serieini = codigo;
                            }
                            if (i == (contador.CantidadCupones - 1))
                            {
                                seriefin = codigo;
                            }
                        }

                        lista = detalleCuponesGeneradosBL.GetListadoDetalleCuponGenerado(cuponesimpresosId);
                        cupones.CgId = CgId;
                        cupones.SerieIni = cupones.SerieIni == "" ? serieini : cupones.SerieIni;
                        cupones.SerieFin = seriefin == "" ? cupones.SerieFin : seriefin;

                        cuponesBL.EditarCuponGeneradoSeries(cupones);
                        var detimpreso = new CMP_DetalleCuponesImpresosEntidad();
                        detimpreso.SerieIni = serieini;
                        detimpreso.SerieFin = seriefin;
                        detimpreso.UltimoCuponImpreso = seriefin;
                        detimpreso.DetImpId = cuponesimpresosId;
                        var detalleimpreso = detalleCuponesImpresosBL.EditarDetalleCuponImpreso(detimpreso);

                        foreach (var m in lista)
                        {
                            m.RazonSocialEmpresa = empresa.RazonSocial;
                            m.RucEmpresa = empresa.Ruc;
                            m.NombreSala = sala.Nombre;
                            m.DniCliente = cliente.NroDoc;
                            m.NombreCliente = cliente.NombreCompleto == "" ? cliente.Nombre + " " + cliente.ApelPat + " " + cliente.ApelMat : cliente.NombreCompleto;
                            listaDetalleGeneradosEnviar.Add(m);
                        }

                    }
                    //Editar total de cupones en cabecera
                    cupones.CgId = CgId;
                    cupones.CantidadCupones = totalCuponesEditar;
                    cuponesBL.EditarCantidadCuponGenerados(cupones);
                    //cupones.DetalleCuponesGenerados = listaDetalleGeneradosEnviar;
                    cupon = cupones;
                    cupon.DetalleCuponesGenerados = listaDetalleGeneradosEnviar;
                    //Obtener Detalle Cupones Impresos
                    var listaDetalleCuponesImpresosTotal = detalleCuponesImpresosBL.GetListadoDetalleCuponImpreso(CgId);
                    listaDetalleCuponesImpresos = listaDetalleCuponesImpresosTotal.Where(x => listaContadores.Select(j => j.DetalleCuponesImpresos_id).Contains(x.DetImpId)).ToList();
                    respuesta = true;
                }

                objRespuesta = new
                {
                    mensaje,
                    respuesta,
                    data = cupon,
                    listaDetalleCuponesImpresos
                };
            }
            catch (Exception ex)
            {
                objRespuesta = new
                {
                    mensaje = ex.Message,
                    respuesta = false
                };
            }
            return Json(objRespuesta);
        }
        [HttpPost]
        public ActionResult LiberarMaquinaSalaV2(string UrlProgresivoSala, long SesionId)
        {
            object objRespuesta = new object();
            bool respuesta = false;
            List<SalaEntidad> listaSalas = new List<SalaEntidad>();
            CMP_CuponesGeneradosEntidad cupon = new CMP_CuponesGeneradosEntidad();//
            CMP_CuponesGeneradosEntidad cupones = new CMP_CuponesGeneradosEntidad();
            List<CMP_DetalleCuponesGeneradosEntidad> lista = new List<CMP_DetalleCuponesGeneradosEntidad>();
            string serieInicial = string.Empty;
            string serieFinal = string.Empty;
            List<CMP_ContadoresOnlineWebCuponesEntidad> contadoresOnline = new List<CMP_ContadoresOnlineWebCuponesEntidad>();
            List<CMP_DetalleCuponesImpresosEntidad> listaDetalleCuponesImpresosTotal = new List<CMP_DetalleCuponesImpresosEntidad>();
            List<CMP_DetalleCuponesGeneradosEntidad> listaDetalleGeneradosEnviar = new List<CMP_DetalleCuponesGeneradosEntidad>();
            List<CMP_DetalleCuponesImpresosEntidad> listaDetalleCuponesImpresos = new List<CMP_DetalleCuponesImpresosEntidad>();
            List<CMP_DetalleCuponesImpresosEntidad> listaDetalleCuponesImpresosConsulta = new List<CMP_DetalleCuponesImpresosEntidad>();
            string mensaje = string.Empty;
            try
            {
                int UsuarioId = Convert.ToInt32(Session["UsuarioID"]);
                contadoresOnline = ObtenerRegistrosALiberar(UrlProgresivoSala, SesionId);
                if (contadoresOnline.Count > 0)
                {
                    //Obtener ListaDetalleCuponesImpresos para saber si la data ya se encuentra Sincronizada
                    //listaDetalleCuponesImpresosConsulta = detalleCuponesImpresosBL.GetListadoDetalleCuponImpreso(CuponId);
                    //if (listaDetalleCuponesImpresosConsulta.Count > 0)
                    //{
                    //
                    //    bool respuestaIAS = EditarEstadoEnvioContadoresOnlineWebCupones(UrlProgresivoSala, CodMaq, CgId, SesionId);
                    //    cupon = cuponesBL.GetCuponGeneradoId(CuponId);
                    //    mensaje = "El registro ya se encuentra Sincronizado";
                    //    respuesta = true;
                    //    var oResp = new
                    //    {
                    //        mensaje,
                    //        respuesta,
                    //        data = cupon,
                    //        listaDetalleCuponesImpresos=listaDetalleCuponesImpresosConsulta
                    //    };
                    //    return Json(oResp);
                    //}
                    ////end
                    //cupones = cuponesBL.GetCuponGeneradoId(CuponId);
                    //listaSalas = salaBl.ListadoSalaPorUsuario(UsuarioId);
                    //
                    ////Data Sala y Empresa
                    //EmpresaEntidad empresa = empresaBL.EmpresaListaIdJson(listaSalas[0].CodEmpresa);
                    ////Datos de Cliente
                    //var primerContador = contadoresOnline.FirstOrDefault();
                    //AST_ClienteEntidad cliente = ast_ClienteBL.GetClienteID(Convert.ToInt32(primerContador.CodCliente));
                    //SalaEntidad sala = salaBl.SalaListaIdJson(Convert.ToInt32(primerContador.CodSala));
                    ////Obtener Primera y ultima serie
                    //List<CMP_DetalleCuponesGeneradosEntidad> listaSeries = new List<CMP_DetalleCuponesGeneradosEntidad>();
                    //foreach (var contador in contadoresOnline)
                    //{
                    //    foreach (var serie in contador.ListaDetalleIASCupones)
                    //    {
                    //        listaSeries.Add(serie);
                    //    }
                    //}
                    //var primeraSerie = listaSeries.FirstOrDefault();
                    //var ultimaSerie = listaSeries.LastOrDefault();
                    //serieInicial = (primeraSerie != null) ? primeraSerie.Serie : string.Empty;
                    //serieFinal = (ultimaSerie != null) ? ultimaSerie.Serie : string.Empty;
                    ////Generar Prefijo 
                    //#region CodAlerta
                    //string[] words = listaSalas[0].Nombre.Split(' ');
                    //var codigoletra = GetPrefijoSala(listaSalas[0].Nombre);
                    //double totalCuponesEditar = cupones.CantidadCupones;
                    //#endregion
                    //foreach (var contador in contadoresOnline)
                    //{
                    //    string serieIni = string.Empty;
                    //    string serieFin = string.Empty;
                    //    totalCuponesEditar += contador.CantidadCupones;
                    //    //Insertar DetalleImpresos
                    //    CMP_DetalleCuponesImpresosEntidad impresos = new CMP_DetalleCuponesImpresosEntidad();
                    //    impresos.CodSala = listaSalas[0].CodSala;
                    //    impresos.CgId = CuponId;
                    //    impresos.CantidadCuponesImpresos = contador.CantidadCupones;
                    //    var cuponesimpresosId = detalleCuponesImpresosBL.GuardarDetalleCuponImpreso(impresos);
                    //    //Insertar Contador Online Web
                    //    contador.FechaLlegada = DateTime.Now;
                    //    contador.DetalleCuponesImpresos_id = cuponesimpresosId;
                    //
                    //    contador.Fecha = contador.Fecha;
                    //    contador.Hora = contador.Hora;
                    //    contador.FechaRegistro = contador.FechaRegistro;
                    //    contador.EstadoEnvio = 1;
                    //    var insertarcontadores = contadoresOnlineWebCuponesBL.GuardarCMP_ContadoresOnlineWebCupones(contador);
                    //    foreach (var detalleGenerado in contador.ListaDetalleIASCupones)
                    //    {
                    //        //DateTime fechaDetalle = detalleGenerado.Fecha;
                    //        //detalleGenerado.Fecha = fechaDetalle.ToLocalTime();
                    //        detalleGenerado.Fecha = detalleGenerado.Fecha;
                    //        detalleGenerado.DetImId = cuponesimpresosId;
                    //        detalleGenerado.CodSala = listaSalas[0].CodSala;
                    //        detalleGenerado.UsuarioId = UsuarioId;
                    //        detalleCuponesGeneradosBL.GuardarDetalleCuponGenerado(detalleGenerado);
                    //    }
                    //    //Generar series e insertar en detalle Cupones Generados
                    //
                    //    lista = detalleCuponesGeneradosBL.GetListadoDetalleCuponGenerado(cuponesimpresosId);
                    //    cupones.CgId = CuponId;
                    //    cupones.SerieIni = cupones.SerieIni == "" ? serieInicial : cupones.SerieIni;
                    //    //cupones.SerieIni = cupones.SerieIni;
                    //    cupones.SerieFin = serieFinal == "" ? cupones.SerieFin : serieFinal;
                    //
                    //    var primerTicket = contador.ListaDetalleIASCupones.FirstOrDefault();
                    //    var ultimoTicket = contador.ListaDetalleIASCupones.LastOrDefault();
                    //    serieIni = primerTicket != null ? primerTicket.Serie : string.Empty;
                    //    serieFin = ultimoTicket != null ? ultimoTicket.Serie : string.Empty;
                    //    cuponesBL.EditarCuponGeneradoSeries(cupones);
                    //    var detimpreso = new CMP_DetalleCuponesImpresosEntidad();
                    //    detimpreso.SerieIni = serieIni;
                    //    detimpreso.SerieFin = serieFin;
                    //    detimpreso.UltimoCuponImpreso = serieFin;
                    //    detimpreso.DetImpId = cuponesimpresosId;
                    //    var detalleimpreso = detalleCuponesImpresosBL.EditarDetalleCuponImpreso(detimpreso);
                    //    foreach (var m in lista)
                    //    {
                    //        m.RazonSocialEmpresa = empresa.RazonSocial;
                    //        m.RucEmpresa = empresa.Ruc;
                    //        m.NombreSala = sala.Nombre;
                    //        m.DniCliente = cliente.NroDoc;
                    //        m.NombreCliente = cliente.NombreCompleto == "" ? cliente.Nombre + " " + cliente.ApelPat + " " + cliente.ApelMat : cliente.NombreCompleto;
                    //        listaDetalleGeneradosEnviar.Add(m);
                    //    }
                    //}
                    ////Editar total de cupones en cabecera
                    //cupones.CgId = CuponId;
                    //cupones.CantidadCupones = totalCuponesEditar;
                    //cuponesBL.EditarCantidadCuponGenerados(cupones);
                    //bool respuestaEdicionIAS = EditarEstadoEnvioContadoresOnlineWebCupones(UrlProgresivoSala, CodMaq, CgId, SesionId);
                    ////cupones.DetalleCuponesGenerados = listaDetalleGeneradosEnviar;
                    //
                    //cupon = cupones;
                    //cupon.DetalleCuponesGenerados = listaDetalleGeneradosEnviar;
                    //Obtener Detalle Cupones Impresos



                    //var listaDetalleCuponesImpresosTotal = detalleCuponesImpresosBL.GetListadoDetalleCuponImpreso(CgId);
                    //
                    //listaDetalleCuponesImpresos = listaDetalleCuponesImpresosTotal.Where(x => contadoresOnline.Select(j => j.DetalleCuponesImpresos_id).Contains(x.DetImpId)).ToList();


                    foreach(CMP_ContadoresOnlineWebCuponesEntidad item in contadoresOnline)
                    {
                        CMP_DetalleCuponesImpresosEntidad temp = new CMP_DetalleCuponesImpresosEntidad();
                        temp.CodMaq = item.CodMaq;
                        temp.CodSala = Convert.ToInt32(item.CodSala);
                        temp.CantidadCuponesImpresos = item.CantidadCupones;
                        temp.CoinOutAnterior = item.CoinOutAnterior;
                        temp.CoinOut = item.CoinOut;
                        temp.HandPay = item.HandPay;
                        temp.JackPot = item.JackPot;
                        temp.CurrentCredits = item.CurrentCredits;
                        temp.CoinOutAnterior = item.CoinOutAnterior;
                        temp.Monto = Convert.ToDecimal(item.Monto);
                        temp.Token = Convert.ToDecimal(item.Token);
                        temp.CoinOutIas = item.CoinOutIas;
                        temp.FechaRegistro = item.Fecha;
                        temp.SerieIni = item.SerieIni;
                        temp.SerieFin = item.SerieFin;
                        temp.Cod_Cont = item.Cod_Cont;
                        listaDetalleCuponesImpresos.Add(temp);
                    }
                    

                }
                else
                {
                    //bool respuestaEdicionIAS = EditarEstadoEnvioContadoresOnlineWebCupones(UrlProgresivoSala, CodMaq, CgId, SesionId);
                }
                respuesta = true;
                mensaje = "Se libero la maquina";
                objRespuesta = new
                {
                    mensaje,
                    respuesta,
                    data = cupon,
                    listaDetalleCuponesImpresos
                };
            }
            catch (Exception ex)
            {
                objRespuesta = new
                {
                    mensaje = ex.Message,
                    respuesta = false
                };
            }
            return Json(objRespuesta);
        }
        [HttpPost]
        public ActionResult MigrarContadoresOnlineWebCupones(string UrlProgresivoSala, string CodMaq, long CgId, long SesionId)
        {
            object objRespuesta = new object();
            object oEnvio = new object();
            List<CMP_ContadoresOnlineWebCuponesEntidad> listaContadores = new List<CMP_ContadoresOnlineWebCuponesEntidad>();
            SalaEntidad sala = new SalaEntidad();
            CMP_CuponesGeneradosEntidad cupon = new CMP_CuponesGeneradosEntidad();//
            CMP_CuponesGeneradosEntidad cupones = new CMP_CuponesGeneradosEntidad();
            List<CMP_DetalleCuponesGeneradosEntidad> lista = new List<CMP_DetalleCuponesGeneradosEntidad>();
            List<CMP_DetalleCuponesGeneradosEntidad> listaDetalleGeneradosEnviar = new List<CMP_DetalleCuponesGeneradosEntidad>();
            List<CMP_DetalleCuponesImpresosEntidad> listaDetalleCuponesImpresos = new List<CMP_DetalleCuponesImpresosEntidad>();
            List<CMP_DetalleCuponesImpresosEntidad> listaDetalleCuponesImpresosConsulta = new List<CMP_DetalleCuponesImpresosEntidad>();
            string mensaje = "";
            bool respuesta = false;
            string serieInicial = string.Empty;
            string serieFinal = string.Empty;
            try
            {
                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                listaContadores = ObtenerRegistrosAMigrar(UrlProgresivoSala, CodMaq, CgId, SesionId);
                if (listaContadores.Count > 0)
                {
                    //Obtener ListaDetalleCuponesImpresos para saber si la data ya se encuentra Sincronizada
                    listaDetalleCuponesImpresosConsulta = detalleCuponesImpresosBL.GetListadoDetalleCuponImpreso(CgId);
                    if (listaDetalleCuponesImpresosConsulta.Count > 0)
                    {

                        bool respuestaIAS = EditarEstadoEnvioContadoresOnlineWebCupones(UrlProgresivoSala, CodMaq, CgId, SesionId);
                        mensaje = "El registro ya se encuentra Sincronizado";
                        respuesta = true;
                        var oResp = new
                        {
                            mensaje,
                            respuesta,
                        };
                        return Json(oResp);
                    }
                    cupones = cuponesBL.GetCuponGeneradoId(CgId);
                    var primerContador = listaContadores.FirstOrDefault();
                    sala = salaBl.SalaListaIdJson(Convert.ToInt32(primerContador.CodSala));

                    //Data Sala y Empresa
                    EmpresaEntidad empresa = empresaBL.EmpresaListaIdJson(sala.CodEmpresa);
                    //Datos de Cliente
                    AST_ClienteEntidad cliente = ast_ClienteBL.GetClienteID(Convert.ToInt32(primerContador.CodCliente));
          
                    //Obtener Primera y ultima serie
                    List<CMP_DetalleCuponesGeneradosEntidad> listaSeries = new List<CMP_DetalleCuponesGeneradosEntidad>();
                    foreach (var contador in listaContadores)
                    {
                        foreach (var serie in contador.ListaDetalleIASCupones)
                        {
                            listaSeries.Add(serie);
                        }
                    }
                    var primeraSerie = listaSeries.FirstOrDefault();
                    var ultimaSerie = listaSeries.LastOrDefault();
                    serieInicial = (primeraSerie != null) ? primeraSerie.Serie : string.Empty;
                    serieFinal = (ultimaSerie != null) ? ultimaSerie.Serie : string.Empty;


                    #region CodAlerta
                    string[] words = sala.Nombre.Split(' ');
                    double totalCuponesEditar = cupones.CantidadCupones;
                    #endregion
                    foreach (var contador in listaContadores)
                    {
                        string serieIni = string.Empty;
                        string serieFin = string.Empty;
                        totalCuponesEditar += contador.CantidadCupones;
                        //Insertar DetalleImpresos
                        CMP_DetalleCuponesImpresosEntidad impresos = new CMP_DetalleCuponesImpresosEntidad();
                        impresos.CodSala = sala.CodSala;
                        impresos.CgId = CgId;
                        impresos.CantidadCuponesImpresos = contador.CantidadCupones;
                        var cuponesimpresosId = detalleCuponesImpresosBL.GuardarDetalleCuponImpreso(impresos);
                        //Insertar Contador Online Web
                        contador.FechaLlegada = DateTime.Now;
                        contador.DetalleCuponesImpresos_id = cuponesimpresosId;
                        //Arreglas Fechas
                        contador.Fecha = contador.Fecha;
                        contador.Hora = contador.Hora;
                        contador.FechaRegistro = contador.FechaRegistro;
                        contador.EstadoEnvio = 1;
                        var insertarcontadores = contadoresOnlineWebCuponesBL.GuardarCMP_ContadoresOnlineWebCupones(contador);
                        foreach (var detalleGenerado in contador.ListaDetalleIASCupones)
                        {
                            detalleGenerado.DetImId = cuponesimpresosId;
                            detalleGenerado.CodSala = sala.CodSala;
                            detalleGenerado.UsuarioId = usuarioId;
                            detalleCuponesGeneradosBL.GuardarDetalleCuponGenerado(detalleGenerado);
                        }
                        //Generar series e insertar en detalle Cupones Generados

                        lista = detalleCuponesGeneradosBL.GetListadoDetalleCuponGenerado(cuponesimpresosId);
                        cupones.CgId = CgId;
                        cupones.SerieIni = cupones.SerieIni == "" ? serieInicial : cupones.SerieIni;
                        //cupones.SerieIni = cupones.SerieIni;
                        cupones.SerieFin = serieFinal == "" ? cupones.SerieFin : serieFinal;

                        var primerTicket = contador.ListaDetalleIASCupones.FirstOrDefault();
                        var ultimoTicket = contador.ListaDetalleIASCupones.LastOrDefault();
                        serieIni = primerTicket != null ? primerTicket.Serie : string.Empty;
                        serieFin = ultimoTicket != null ? ultimoTicket.Serie : string.Empty;
                        cuponesBL.EditarCuponGeneradoSeries(cupones);
                        var detimpreso = new CMP_DetalleCuponesImpresosEntidad();
                        detimpreso.SerieIni = serieIni;
                        detimpreso.SerieFin = serieFin;
                        detimpreso.UltimoCuponImpreso = serieFin;
                        detimpreso.DetImpId = cuponesimpresosId;
                        var detalleimpreso = detalleCuponesImpresosBL.EditarDetalleCuponImpreso(detimpreso);
                    }
                    //Editar total de cupones en cabecera
                    cupones.CgId = CgId;
                    cupones.CantidadCupones = totalCuponesEditar;
                    cuponesBL.EditarCantidadCuponGenerados(cupones);
                    mensaje = "Registros Migrados";
                    //cupones.DetalleCuponesGenerados = listaDetalleGeneradosEnviar;
                    respuesta = EditarEstadoEnvioContadoresOnlineWebCupones(UrlProgresivoSala, CodMaq, CgId, SesionId);
                    if (respuesta==false)
                    {
                        mensaje += "\n No se actualizo la información en Servicio Online";
                    }
                }
                else
                {
                    mensaje = "Registros Migrados";
                    respuesta = EditarEstadoEnvioContadoresOnlineWebCupones(UrlProgresivoSala, CodMaq, CgId, SesionId);
                }
                objRespuesta = new
                {
                    mensaje,
                    respuesta,
                    data = listaContadores
                };
            }
            catch (Exception ex)
            {
                objRespuesta = new
                {
                    mensaje = ex.Message,
                    respuesta = false
                };
            }
            return Json(objRespuesta);
        }
        [HttpPost]
        public ActionResult ReiniciarSesionClienteMaquinaSala(string UrlProgresivoSala, long SesionId) {
            object oRespuesta = new object();//ReiniciarSesionCliente
            bool respuesta = false;
            object oEnvio = new object();
            string mensaje = "";
            if(string.IsNullOrEmpty(UrlProgresivoSala)) {
                return Json(new { respuesta = false, mensaje = "No se configuró la url de sala" });
            }
            try
            {
                oEnvio = new
                {
                    SesionId = SesionId
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                var client = new System.Net.WebClient();
                var response = "";
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                string url = UrlProgresivoSala;
                url += "/servicio/ReiniciarSesionCliente";
                response = client.UploadString(url, "POST", inputJson);
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                mensaje = jsonObj.mensaje;
                respuesta = Convert.ToBoolean(jsonObj.respuesta);

            }
            catch(Exception ex)
            {
                mensaje = ex.Message;
            }
            oRespuesta = new
            {
                mensaje,
                respuesta
            };
            return Json(oRespuesta);
        }
        [seguridad(false)]
        public List<CMP_ContadoresOnlineWebCuponesEntidad> ObtenerRegistrosAMigrar(string UrlProgresivoSala, string CodMaq, long CgId, long SesionId)
        {
            List<CMP_ContadoresOnlineWebCuponesEntidad> listaRespuesta = new List<CMP_ContadoresOnlineWebCuponesEntidad>();
            object oEnvio = new object();
            if(string.IsNullOrEmpty(UrlProgresivoSala)) {
                return listaRespuesta;
            }
            try
            {
                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                oEnvio = new
                {
                    CodMaquina = CodMaq,
                    CgId = CgId,
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
                url += "/servicio/ObtenerDataMigrarContadoresOnlineWebCupones";
                response = client.UploadString(url, "POST", inputJson);
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                mensaje = jsonObj.mensaje;
                respuesta = Convert.ToBoolean(jsonObj.respuesta);
                if (respuesta)
                {
                    var items = jsonObj.data;
                    foreach (var myItem in items)
                    {
                        List<CMP_DetalleCuponesGeneradosEntidad> listaDetalles = new List<CMP_DetalleCuponesGeneradosEntidad>();
                        DateTime fechaItem = myItem.Fecha;
                        DateTime horaItem = myItem.Hora;
                        DateTime fechaRegistroItem = myItem.FechaRegistro;
                        CMP_ContadoresOnlineWebCuponesEntidad contador = new CMP_ContadoresOnlineWebCuponesEntidad()
                        {
                            Cod_Cont = myItem.Cod_Cont,
                            Cod_Cont_OL = myItem.Cod_Cont_OL,
                            Fecha = myItem.Fecha,
                            Hora = myItem.Hora,
                            CodMaq = myItem.CodMaq,
                            CodMaqMin = myItem.CodMaqMin,
                            CoinOut = myItem.CoinOut,
                            CurrentCredits = myItem.CurrentCredits,
                            Monto = myItem.Monto,
                            Token = myItem.Token,
                            CoinOutAnterior = myItem.CoinOutAnterior,
                            Estado_Oln = myItem.Estado_Oln,
                            Win = myItem.Win,
                            CantidadCupones = myItem.CantidadCupones,
                            FechaRegistro = fechaRegistroItem,
                            CodCliente = myItem.CodCliente,
                            CoinOutIas = myItem.CoinOutIas,
                            EstadoEnvio = myItem.Estado_Envio,
                            CodSala = myItem.CodSala,
                            HandPay=myItem.HandPay,
                            JackPot=myItem.JackPot,
                            HandPayAnterior=myItem.HandPayAnterior,
                            JackPotAnterior=myItem.JackPotAnterior
                        };
                        var detalle = myItem.ListaDetalleIASCupones;
                        foreach(var det in detalle)
                        {
                            DateTime detFecha = det.Fecha;
                            CMP_DetalleCuponesGeneradosEntidad detalleGenerado = new CMP_DetalleCuponesGeneradosEntidad();
                            detalleGenerado.Serie = det.Serie;
                            detalleGenerado.Fecha = det.Fecha;
                            detalleGenerado.CantidadImpresiones = det.CantidadImpresiones==null?0:det.CantidadImpresiones;
                            listaDetalles.Add(detalleGenerado);
                        }
                        contador.ListaDetalleIASCupones = listaDetalles;
                        listaRespuesta.Add(contador);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return listaRespuesta;
        }
        [seguridad(false)]
        public bool EditarEstadoEnvioContadoresOnlineWebCupones(string UrlProgresivoSala, string CodMaq, long CgId, long SesionId)
        {
            List<CMP_ContadoresOnlineWebCuponesEntidad> listaRespuesta = new List<CMP_ContadoresOnlineWebCuponesEntidad>();
            bool respuesta = false;
            object oEnvio = new object();
            if(string.IsNullOrEmpty(UrlProgresivoSala)) {
                return false;
            }
            try
            {
                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                oEnvio = new
                {
                    CodMaquina = CodMaq,
                    CgId = CgId,
                    SesionId = SesionId
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                var client = new System.Net.WebClient();
                var response = "";
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                string url = UrlProgresivoSala;
                url += "/servicio/EditarEstadoEnvioContadoresOnlineWebCupones";
                response = client.UploadString(url, "POST", inputJson);
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                respuesta = Convert.ToBoolean(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        [seguridad(false)]
        public List<CMP_ContadoresOnlineWebCuponesEntidad> ObtenerRegistrosALiberar(string UrlProgresivoSala, long SesionId)
        {
            List<CMP_ContadoresOnlineWebCuponesEntidad> listaRespuesta = new List<CMP_ContadoresOnlineWebCuponesEntidad>();
            object oEnvio = new object();
            if(string.IsNullOrEmpty(UrlProgresivoSala)) {
                listaRespuesta.Clear();
                return listaRespuesta;
            }
            try
            {
                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                oEnvio = new
                {
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
                url += "/servicio/ObtenerRegistrosALiberar";
                response = client.UploadString(url, "POST", inputJson);
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                mensaje = jsonObj.mensaje;
                respuesta = Convert.ToBoolean(jsonObj.respuesta);
                if (respuesta)
                {
                    var items = jsonObj.data;
                    foreach (var myItem in items)
                    {
                        List<CMP_DetalleCuponesGeneradosEntidad> listaDetalles = new List<CMP_DetalleCuponesGeneradosEntidad>();
                        DateTime fechaItem = myItem.Fecha;
                        DateTime horaItem = myItem.Hora;
                        DateTime fechaRegistroItem = myItem.FechaRegistro;
                        CMP_ContadoresOnlineWebCuponesEntidad contador = new CMP_ContadoresOnlineWebCuponesEntidad()
                        {
                            Cod_Cont = myItem.Cod_Cont,
                            Cod_Cont_OL = myItem.Cod_Cont_OL,
                            Fecha = myItem.Fecha,
                            Hora = myItem.Hora,
                            CodMaq = myItem.CodMaq,
                            CodMaqMin = myItem.CodMaqMin,
                            CoinOut = myItem.CoinOut,
                            CurrentCredits = myItem.CurrentCredits,
                            Monto = myItem.Monto,
                            Token = myItem.Token,
                            CoinOutAnterior = myItem.CoinOutAnterior,
                            Estado_Oln = myItem.Estado_Oln,
                            Win = myItem.Win,
                            CantidadCupones = myItem.CantidadCupones,
                            FechaRegistro = fechaRegistroItem,
                            CodCliente = myItem.CodCliente,
                            CoinOutIas = myItem.CoinOutIas,
                            EstadoEnvio = myItem.Estado_Envio,
                            CodSala = myItem.CodSala,
                            HandPay=myItem.HandPay,
                            JackPot=myItem.JackPot,
                            HandPayAnterior=myItem.HandPayAnterior,
                            JackPotAnterior=myItem.JackPotAnterior,
                            SerieIni=myItem.SerieIni,
                            SerieFin=myItem.SerieFin,
                        };
                        var detalle = myItem.ListaDetalleIASCupones;
                        foreach (var det in detalle)
                        {
                            DateTime detFecha = det.Fecha;
                            CMP_DetalleCuponesGeneradosEntidad detalleGenerado = new CMP_DetalleCuponesGeneradosEntidad();
                            detalleGenerado.Serie = det.Serie;
                            detalleGenerado.Fecha = det.Fecha;
                            listaDetalles.Add(detalleGenerado);
                        }
                        contador.ListaDetalleIASCupones = listaDetalles;
                        listaRespuesta.Add(contador);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                listaRespuesta.Clear();
            }
            return listaRespuesta;
        }
        [seguridad(false)]
        public string GetPrefijoSala(string NombreSala)
        {
            string codigoletra = string.Empty;

            try
            {
                string Sala = NombreSala.ToUpper();
                string[] words = Sala.Split(' '); ;
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
            }
            catch (Exception)
            {
                codigoletra = string.Empty;
            }
            return codigoletra;
        }
        [seguridad(false)]
        public ActionResult ObtenerCabeceraEstadoMaquinaSala(string UrlProgresivoSala, string CodMaq, long CgId, long SesionId, int Estado_Terminado=0)
        {
            object objRespuesta = new object();
            object oEnvio = new object();
            object dataCabecera = new object();
            object dataDetalle = new object();
            List<CMP_ContadoresOnlineWebCuponesEntidad> listaContadores = new List<CMP_ContadoresOnlineWebCuponesEntidad>();
            List<CMP_SesionCuponesClienteEntidad> listaSesiones = new List<CMP_SesionCuponesClienteEntidad>();
            CMP_SesionCuponesClienteEntidad sesionCabecera = new CMP_SesionCuponesClienteEntidad();
            bool respuesta = false;
            if(string.IsNullOrEmpty(UrlProgresivoSala)) {
                return Json(new { respuesta = false, mensaje = "No se configuró la url de sala" });
            }
            try
            {
                oEnvio = new
                {
                    CodMaquina = CodMaq,
                    CgId = CgId,
                    SesionId = SesionId
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                string mensaje = "";
                var client = new System.Net.WebClient();
                var response = "";
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                string url = UrlProgresivoSala;
                url += "/servicio/GetSesionCuponClientexMaquinayEstadoTerminado?CodMaquina=" + CodMaq.Trim() + "&EstadoTerminado=" + Estado_Terminado;
                response = client.UploadString(url, "POST", inputJson);
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                mensaje = jsonObj.mensaje;
                respuesta = Convert.ToBoolean(jsonObj.respuesta);
                if (respuesta)
                {
                    var items = jsonObj.data;
                    List<object> listObject = new List<object>();
                    foreach (var myItem in items)
                    {
                        var sesion = new CMP_SesionCuponesClienteEntidad()
                        {
                            CodMaquina = Convert.ToString(myItem.CodMaquina),
                            Terminado = Convert.ToInt32(myItem.Terminado),
                            SesionId = Convert.ToInt32(myItem.SesionId),
                            CgId = Convert.ToInt64(myItem.CgId),

                        };
                        listaSesiones.Add(sesion);
                    }
                    listaSesiones = listaSesiones.Where(x => x.SesionId == SesionId).ToList();
                    if (listaSesiones.Count > 0)
                    {
                        sesionCabecera = listaSesiones.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            objRespuesta = new
            {
                respuesta = respuesta,
                data = sesionCabecera
            };
            return Json(objRespuesta);
        }
        [seguridad(false)]
        public List<CMP_ContadoresOnlineWebCuponesEntidad> ObtenerDetalleNoEnviado(string UrlProgresivoSala, string CodMaq, long CgId, long SesionId)
        {
            object objRespuesta = new object();
            object oEnvio = new object();
            List<CMP_ContadoresOnlineWebCuponesEntidad> listaContadores = new List<CMP_ContadoresOnlineWebCuponesEntidad>();
            if(string.IsNullOrEmpty(UrlProgresivoSala)) {
                listaContadores.Clear();
                return listaContadores;
            }
            try
            {
                oEnvio = new
                {
                    CodMaquina = CodMaq,
                    CgId = CgId,
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
                url += "/servicio/ObtenerDetalleRegistrosCuponesNoEnviados";
                response = client.UploadString(url, "POST", inputJson);
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                mensaje = jsonObj.mensaje;
                respuesta = Convert.ToBoolean(jsonObj.respuesta);
                var items = jsonObj.data;
                List<object> listObject = new List<object>();
                foreach (var myItem in items)
                {
                    DateTime fechaItem = myItem.Fecha;
                    DateTime horaItem = myItem.Hora;
                    DateTime fechaRegistroItem = myItem.FechaRegistro;
                    CMP_ContadoresOnlineWebCuponesEntidad contador = new CMP_ContadoresOnlineWebCuponesEntidad()
                    {
                        Cod_Cont = myItem.Cod_Cont,
                        Cod_Cont_OL = myItem.Cod_Cont_OL,
                        Fecha = myItem.Fecha,
                        Hora = myItem.Hora,
                        CodMaq = myItem.CodMaq,
                        CodMaqMin = myItem.CodMaqMin,
                        CoinOut = myItem.CoinOut,
                        CurrentCredits = myItem.CurrentCredits,
                        Monto = myItem.Monto,
                        Token = myItem.Token,
                        CoinOutAnterior = myItem.CoinOutAnterior,
                        Estado_Oln = myItem.Estado_Oln,
                        Win = myItem.Win,
                        CantidadCupones = myItem.CantidadCupones,
                        FechaRegistro = myItem.FechaRegistro,
                        CodCliente = myItem.CodCliente,
                        CoinOutIas = myItem.CoinOutIas,
                        EstadoEnvio = myItem.Estado_Envio,
                        CodSala = myItem.CodSala,
                    };
                    listaContadores.Add(contador);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                listaContadores.Clear();
            }
            return listaContadores;
        }
    }
}