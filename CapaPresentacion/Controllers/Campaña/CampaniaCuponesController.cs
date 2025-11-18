using CapaEntidad;
using CapaEntidad.AsistenciaCliente;
using CapaEntidad.Campañas;
using CapaNegocio;
using CapaNegocio.AsistenciaCliente;
using CapaNegocio.Campaña;
using CapaPresentacion.Models;
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
    public class CampaniaCuponesController : Controller
    {
        private SalaBL salaBl = new SalaBL();
        private CMP_SalalibreBL salalibreBl = new CMP_SalalibreBL();
        private CMP_CampañaBL campaniabl = new CMP_CampañaBL();
        private CMP_TicketBL ticketbl = new CMP_TicketBL();
        private EmpresaBL empresaBL = new EmpresaBL();
        private CMP_CuponesGeneradosBL cuponesBL = new CMP_CuponesGeneradosBL();
        private CMP_DetalleCuponesGeneradosBL detalleCuponesGeneradosBL = new CMP_DetalleCuponesGeneradosBL();
        private CMP_DetalleCuponesImpresosBL detalleCuponesImpresosBL = new CMP_DetalleCuponesImpresosBL();
        private CMP_Campaña_ParametrosBL campaniaparametrosbl = new CMP_Campaña_ParametrosBL();
        private AST_ClienteBL ast_ClienteBL = new AST_ClienteBL();
        private CMP_ContadoresOnlineWebCuponesBL contadoresOnlineWebCuponesBL = new CMP_ContadoresOnlineWebCuponesBL();
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoCuponesxCampania(DtParameters dtParameters,int CampaniaId, string UrlProgresivoSala)
        {
            string mensaje= "";
            bool respuesta = false;
            List<CMP_CuponesGeneradosEntidad> lista = new List<CMP_CuponesGeneradosEntidad>();

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            var count = 0;
            List<dynamic> registro = new List<dynamic>();
            try
            {
                //lista = cuponesBL.GetListadoCuponesxCampania(CampaniaId);
                lista = GetListadoCuponesxCampaniaV2(CampaniaId,UrlProgresivoSala);

                if (dtParameters.Order != null)
                {
                    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
                }
                else
                {
                    orderCriteria = "CgId";
                    orderAscendingDirection = false;
                }
                count = lista.Count();
                if (!string.IsNullOrEmpty(searchBy) && !(string.IsNullOrWhiteSpace(searchBy)))
                {
                    lista = lista.Where(x => x.CgId.ToString() == searchBy.ToLower()
                                                  || x.SlotId.ToString().Contains(searchBy.ToLower())
                                                 
                                                  || x.ApelMat.ToLower().Contains(searchBy.ToLower())
                                                  || x.ApelPat.ToLower().Contains(searchBy.ToLower())
                                                  || x.Nombre.ToLower().Contains(searchBy.ToLower())
                                                  || x.NombreCompleto.ToLower().Contains(searchBy.ToLower())
                                                   || x.NroDoc.ToLower().Contains(searchBy.ToLower())
                                                    || x.FechaNacimiento.ToString("dd/mm/yyyy").Contains(searchBy.ToLower())
                                                   || x.CantidadCupones.ToString() == searchBy.ToLower()
                                                   || x.SerieIni.ToLower().Contains(searchBy.ToLower())
                                                  || x.SerieFin.ToLower().Contains(searchBy.ToLower())
                                                    || x.Fecha.ToString("dd/mm/yyyy hh:mm:ss tt").Contains(searchBy.ToLower())
                                                    | x.Mail.ToLower().Contains(searchBy.ToLower())
                                                  ).ToList();
                }

                lista = orderAscendingDirection ? lista.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc).ToList() : lista.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc).ToList();

                var filteredResultsCount = lista.Count();
                var totalResultsCount = count;

                registro.Add(new
                {
                    mensaje = "Listando Registros",
                    draw = dtParameters.Draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = lista.Skip(dtParameters.Start)
                    .Take(dtParameters.Length).ToList()
                });
                respuesta = true;
                mensaje = "Listando Registros";
            }
            catch (Exception exp)
            {
                mensaje = exp.Message;
            }
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var result = new ContentResult
            {
                Content = serializer.Serialize(registro.FirstOrDefault()),
                ContentType = "application/json"
            };
            return result;
            //return Json(new { respuesta , mensaje ,data=listaCupones });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoCuponesImpresos(Int64 SesionId,string UrlProgresivo)
        {
            string mensaje = "";
            bool respuesta = false;
            List<CMP_DetalleCuponesImpresosEntidad> listaCupones = new List<CMP_DetalleCuponesImpresosEntidad>();
            try
            {
                //listaCupones = detalleCuponesImpresosBL.GetListadoDetalleCuponImpreso(cgid);
                listaCupones = GetListadoDetalleCuponImpresoV2(SesionId, UrlProgresivo);
                respuesta = true;
                mensaje = "Listando Registros";
            }
            catch (Exception exp)
            {
                mensaje = exp.Message;
            }

            return Json(new { respuesta, mensaje, data = listaCupones });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoCuponesGenerados(Int64 Cod_Cont,string UrlProgresivoSala)
        {
            string mensaje = "";
            bool respuesta = false;
            List<CMP_DetalleCuponesGeneradosEntidad> listaCupones = new List<CMP_DetalleCuponesGeneradosEntidad>();
            try
            {
                //listaCupones = detalleCuponesGeneradosBL.GetListadoDetalleCuponGenerado(DetImId);
                listaCupones = GetListadoDetalleCuponGeneradoV2(Cod_Cont, UrlProgresivoSala);
                respuesta = true;
                mensaje = "Listando Registros";
            }
            catch (Exception exp)
            {
                mensaje = exp.Message;
            }

            return Json(new { respuesta, mensaje, data = listaCupones });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ImprimirCuponGenerado(int CgId)
        {
            string mensaje = "";
            bool respuesta = false;
            CMP_CuponesGeneradosEntidad cupon = new CMP_CuponesGeneradosEntidad();
            List<CMP_DetalleCuponesGeneradosEntidad> listaDetalleGenerado = new List<CMP_DetalleCuponesGeneradosEntidad>();
            List<CMP_DetalleCuponesImpresosEntidad> listaDetalleImpreso = new List<CMP_DetalleCuponesImpresosEntidad>();
            SalaEntidad sala = new SalaEntidad();
            EmpresaEntidad empresa = new EmpresaEntidad();
            AST_ClienteEntidad cliente = new AST_ClienteEntidad();
            try
            {
                //Data Cupones
                cupon = cuponesBL.GetCuponGeneradoId(CgId);
                listaDetalleGenerado = detalleCuponesGeneradosBL.GetListadoDetalleCuponGenerado(cupon.CgId);
                listaDetalleImpreso = detalleCuponesImpresosBL.GetListadoDetalleCuponImpreso(CgId);
                //Data Sala y Empresa
                sala = salaBl.SalaListaIdJson(cupon.CodSala);
                empresa = empresaBL.EmpresaListaIdJson(sala.CodEmpresa);
                //Datos de Cliente
                cliente = ast_ClienteBL.GetClienteID(Convert.ToInt32(cupon.ClienteId));
                //Insertando data Sala y Empresa en detalles cupones para impresion
                foreach(var detalleGenerado in listaDetalleGenerado)
                {
                    detalleGenerado.FechaString = detalleGenerado.Fecha.ToString();
                    detalleGenerado.RazonSocialEmpresa = empresa.RazonSocial;
                    detalleGenerado.RucEmpresa = empresa.Ruc;
                    detalleGenerado.NombreSala = sala.Nombre;
                    detalleGenerado.DniCliente = cliente.NroDoc;
                    detalleGenerado.NombreCliente = cliente.NombreCompleto == "" ? cliente.Nombre + " " + cliente.ApelPat + " " +cliente.ApelMat : cliente.NombreCompleto;
                }
                cupon.DetalleCuponesGenerados = listaDetalleGenerado;
                cupon.DetalleCuponesImpresos = listaDetalleImpreso;
                respuesta = true;
                mensaje = "Listando Registros";
            }
            catch (Exception exp)
            {
                mensaje = exp.Message;
            }

            return Json(new { respuesta, mensaje, data = cupon });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult RecepcionarContadoresOnlineWeb(List<CMP_ContadoresOnlineWebCuponesEntidad> contadoresOnline, long CuponId, int UsuarioId)
        {
            object objRespuesta = new object();
            string mensaje = "";
            bool respuesta = false;
            List<SalaEntidad> listaSalas = new List<SalaEntidad>();
            CMP_CuponesGeneradosEntidad cupon = new CMP_CuponesGeneradosEntidad();//
            CMP_CuponesGeneradosEntidad cupones = new CMP_CuponesGeneradosEntidad();
            List<CMP_DetalleCuponesGeneradosEntidad> lista = new List<CMP_DetalleCuponesGeneradosEntidad>();
            List<CMP_DetalleCuponesGeneradosEntidad> listaDetalleGeneradosEnviar = new List<CMP_DetalleCuponesGeneradosEntidad>();
            try
            {
             

                //recepciona una lista de contadores_online_web_cupones
                if (contadoresOnline.Count != 0)
                {
                    cupones = cuponesBL.GetCuponGeneradoId(CuponId);
                    listaSalas = salaBl.ListadoSalaPorUsuario(UsuarioId);

                    //Data Sala y Empresa
                    EmpresaEntidad empresa = empresaBL.EmpresaListaIdJson(listaSalas[0].CodEmpresa);
                    //Datos de Cliente
                    var primerElemento = contadoresOnline.FirstOrDefault();
                    AST_ClienteEntidad cliente = ast_ClienteBL.GetClienteID(Convert.ToInt32(primerElemento.CodCliente));

                    #region CodAlerta
                    string[] words = listaSalas[0].Nombre.Split(' ');
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
                    foreach (var contador in contadoresOnline)
                    {
                        totalCuponesEditar += contador.CantidadCupones;
                        //Insertar DetalleImpresos
                        CMP_DetalleCuponesImpresosEntidad impresos = new CMP_DetalleCuponesImpresosEntidad();
                        impresos.CodSala = listaSalas[0].CodSala;
                        impresos.CgId = CuponId;
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
                        contador.FechaRegistro = fechaRegistroContador.ToLocalTime();
                        var insertarcontadores = contadoresOnlineWebCuponesBL.GuardarCMP_ContadoresOnlineWebCupones(contador);
                        //Generar series e insertar en detalle Cupones Generados
                        Int64 total = detalleCuponesGeneradosBL.CuponesTotalJson(listaSalas[0].CodSala);
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
                            detalle.CodSala = listaSalas[0].CodSala;
                            detalle.Fecha = DateTime.Now;
                            detalle.CantidadImpresiones = 1;
                            detalle.UsuarioId = UsuarioId;
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
                        cupones.CgId = CuponId;
                        cupones.SerieIni = cupones.SerieIni == "" ? serieini : cupones.SerieIni;
                        //cupones.SerieIni = cupones.SerieIni;
                        cupones.SerieFin = seriefin==""?cupones.SerieFin:seriefin;

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
                            m.NombreSala = listaSalas[0].Nombre;
                            m.DniCliente = cliente.NroDoc;
                            m.NombreCliente = cliente.NombreCompleto == "" ? cliente.Nombre + " " + cliente.ApelPat + " " + cliente.ApelMat : cliente.NombreCompleto;
                            listaDetalleGeneradosEnviar.Add(m);
                        }
                    }
                    //Editar total de cupones en cabecera
                    cupones.CgId = CuponId;
                    cupones.CantidadCupones = totalCuponesEditar;
                    cuponesBL.EditarCantidadCuponGenerados(cupones);
                    //cupones.DetalleCuponesGenerados = listaDetalleGeneradosEnviar;
                    cupon = cupones;
                    cupon.DetalleCuponesGenerados = listaDetalleGeneradosEnviar;
                    respuesta = true;
                }
                else
                {
                    respuesta = false;
                }
                //devuelve cupon con el detalle de tickets
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return Json(new { respuesta, cupon });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult IniciarSesionCliente(string IpImpresora, string PuertoImpresora,string CodCliente,string CodMaquina, double CoinOut,string CuponGeneradoId, string IpServicioOnline="")
        {
            string mensaje = "";
            bool respuesta = false;
            CMP_CuponesGeneradosEntidad Cupon = new CMP_CuponesGeneradosEntidad();
            if(string.IsNullOrEmpty(IpServicioOnline)) {
                return Json(new { respuesta = false, mensaje = "No se configuró la url de sala" }); ;
            }
            try
            {
                #region Region A comentar, Data de prueba
                Cupon = new CMP_CuponesGeneradosEntidad()
                {
                    CgId = 1,
                    nombreSala = "Las Musas",
                    Fecha = DateTime.Now,
                    SlotId = "123",
                };
                List<CMP_DetalleCuponesGeneradosEntidad> listaDetalleCupones = new List<CMP_DetalleCuponesGeneradosEntidad>();
                for (int i = 1; i <= 2; i++)
                {
                    CMP_DetalleCuponesGeneradosEntidad detalle = new CMP_DetalleCuponesGeneradosEntidad()
                    {
                        DetGenId = Convert.ToInt64(i),
                        RazonSocialEmpresa = "Mi Empresa",
                        RucEmpresa = "10458956320",
                        Fecha = DateTime.Now,
                        NombreSala = "Las Musas",
                        Serie = "MUS-00000000" + i,
                        NombreCliente = "Diego Canchari",
                        DniCliente = "47316152"
                    };
                    listaDetalleCupones.Add(detalle);
                }
                Cupon.DetalleCuponesGenerados = listaDetalleCupones;
                #endregion
                object oEnvio = new
                {
                    Cupon,
                    IpImpresora,PuertoImpresora,CodCliente,CodMaquina,CoinOut
                };
                var client = new System.Net.WebClient();
                var response = "";
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                string url = IpServicioOnline+ "/servicio/IniciarSesionCliente";
                response = client.UploadString(url, "POST", inputJson);
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                if (jsonObj.respuesta != null) {
                    string resp = jsonObj.respuesta;
                    if (Convert.ToBoolean(resp))
                    {
                        respuesta = true;
                    }
                    mensaje = jsonObj.mensaje;
                }
                else
                {
                    mensaje = "No se pudo conectar con el Servicio Online";
                }
                
            }
            catch(Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { respuesta,mensaje});
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult RecepcionarContadoresOnlineWebV2(List<CMP_ContadoresOnlineWebCuponesEntidad> contadoresOnline, long CuponId, int UsuarioId)
        {
            object objRespuesta = new object();
            bool respuesta = false;
            List<SalaEntidad> listaSalas = new List<SalaEntidad>();
            CMP_CuponesGeneradosEntidad cupon = new CMP_CuponesGeneradosEntidad();//
            CMP_CuponesGeneradosEntidad cupones = new CMP_CuponesGeneradosEntidad();
            List<CMP_DetalleCuponesGeneradosEntidad> lista = new List<CMP_DetalleCuponesGeneradosEntidad>();
            string serieInicial = string.Empty;
            string serieFinal = string.Empty;
            try
            {
                //quitar el primer elemento, ya que es la jugada por defecto que se usa en contadoresonlineweb para realizar el calculo de las demas jugadas.
                //if (contadoresOnline.Count > 0)
                //{
                //    var primercontador = contadoresOnline.FirstOrDefault();
                //    contadoresOnline.Remove(primercontador);
                //}
                if (contadoresOnline.Count > 0)
                {
                    cupones = cuponesBL.GetCuponGeneradoId(CuponId);
                    listaSalas = salaBl.ListadoSalaPorUsuario(UsuarioId);

                    //Data Sala y Empresa
                    EmpresaEntidad empresa = empresaBL.EmpresaListaIdJson(listaSalas[0].CodEmpresa);
                    //Datos de Cliente
                    var primerContador = contadoresOnline.FirstOrDefault();
                    AST_ClienteEntidad cliente = ast_ClienteBL.GetClienteID(Convert.ToInt32(primerContador.CodCliente));
                    List<CMP_DetalleCuponesGeneradosEntidad> listaSeries = new List<CMP_DetalleCuponesGeneradosEntidad>();
                    //Obtener Series
                    foreach (var contador in contadoresOnline)
                    {
                        foreach (var serie in contador.ListaDetalleIASCupones)
                        {
                            listaSeries.Add(serie);
                        }
                    }
                    var primeraSerie = listaSeries.FirstOrDefault();
                    var ultimaSerie = listaSeries.LastOrDefault();
                    serieInicial = (primeraSerie != null) ? primeraSerie.Serie : string.Empty;
                    serieFinal = (ultimaSerie!= null) ? ultimaSerie.Serie:string.Empty;
                    ////Obtener primera serie generada
                    //var primerDetalle = primerContador.ListaDetalleIASCupones.FirstOrDefault();
                    //serieInicial = primerDetalle.Serie;
                    ////Obtener ultima serie generada
                    //var ultimoContador = contadoresOnline.LastOrDefault();
                    //var ultimoDetalle = ultimoContador.ListaDetalleIASCupones.LastOrDefault();
                    //serieFinal = ultimoDetalle.Serie;
                    
                    #region CodAlerta
                    string[] words = listaSalas[0].Nombre.Split(' ');
                    var codigoletra = GetPrefijoSala(listaSalas[0].Nombre);
                    double totalCuponesEditar = cupones.CantidadCupones;
                    #endregion
                    foreach (var contador in contadoresOnline)
                    {
                        string serieIni = string.Empty;
                        string serieFin = string.Empty;
                        totalCuponesEditar += contador.CantidadCupones;
                        //Insertar DetalleImpresos
                        CMP_DetalleCuponesImpresosEntidad impresos = new CMP_DetalleCuponesImpresosEntidad();
                        impresos.CodSala = listaSalas[0].CodSala;
                        impresos.CgId = CuponId;
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
                        contador.FechaRegistro = fechaRegistroContador.ToLocalTime();
                        //contador.Fecha = contador.Fecha;
                        //contador.Hora = contador.Hora;
                        //contador.FechaRegistro = contador.FechaRegistro;
                        var insertarcontadores = contadoresOnlineWebCuponesBL.GuardarCMP_ContadoresOnlineWebCupones(contador);
                        foreach(var detalleGenerado in contador.ListaDetalleIASCupones)
                        {
                            detalleGenerado.DetImId = cuponesimpresosId;
                            detalleGenerado.CodSala = listaSalas[0].CodSala;
                            detalleGenerado.UsuarioId = UsuarioId;
                            detalleGenerado.CantidadImpresiones = detalleGenerado.CantidadImpresiones;
                            detalleCuponesGeneradosBL.GuardarDetalleCuponGenerado(detalleGenerado);
                        }
                        //Generar series e insertar en detalle Cupones Generados

                        lista = detalleCuponesGeneradosBL.GetListadoDetalleCuponGenerado(cuponesimpresosId);
                        cupones.CgId = CuponId;
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
                    cupones.CgId = CuponId;
                    cupones.CantidadCupones = totalCuponesEditar;
                    cuponesBL.EditarCantidadCuponGenerados(cupones);
                    //cupones.DetalleCuponesGenerados = listaDetalleGeneradosEnviar;
                    respuesta = true;
                }
                respuesta = true;
            }
            catch (Exception ex)
            {
                respuesta = false;
            }
            return Json(new { respuesta});
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
        public List<CMP_CuponesGeneradosEntidad> GetListadoCuponesxCampaniaV2(int campaniaId, string UrlProgresivoSala)
        {
            List<CMP_CuponesGeneradosEntidad> listaRespuesta = new List<CMP_CuponesGeneradosEntidad>();
            object oEnvio = new object();
            if(string.IsNullOrEmpty(UrlProgresivoSala)) {
                return (listaRespuesta);
            }
            try
            {
                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                oEnvio = new
                {
                    CampaniaId = campaniaId
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                string mensaje = "";
                bool respuesta = false;
                var client = new System.Net.WebClient();
                var response = "";
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                string url = UrlProgresivoSala;
                url += "/servicio/ObtenerListadoSesionCuponesClientePorCampaniaId";
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
                        //DateTime fechaItem = myItem.Fecha;
                        //DateTime horaItem = myItem.Hora;
                        //DateTime fechaRegistroItem = myItem.FechaRegistro;
                        CMP_CuponesGeneradosEntidad contador = new CMP_CuponesGeneradosEntidad()
                        {
                            CgId = myItem.CgId,
                            CampaniaId = myItem.CampaniaId,
                            ClienteId = myItem.ClienteId,
                            ApelPat = string.Empty,
                            ApelMat = string.Empty,
                            Nombre = string.Empty,
                            NombreCompleto = myItem.NombreCliente,
                            Mail = myItem.Correo,
                            FechaNacimiento = DateTime.Now,
                            NroDoc = myItem.NroDocumento,
                            CodSala = 0,
                            nombreSala = myItem.NombreSala,
                            UsuarioId = myItem.UsuarioIdIAS,
                            UsuarioNombre = myItem.UsuarioNombre,
                            SlotId = myItem.CodMaquina,
                            Juego = string.Empty,
                            Marca = string.Empty,
                            Modelo = string.Empty,
                            Win = 0,
                            Parametro = 0,
                            ValorJuego = myItem.CoinOutIAS,
                            CantidadCupones = myItem.CantidadCupones,
                            SaldoCupIni = 0,
                            SaldoCupFin = 0,
                            SerieIni = myItem.SerieIni,
                            SerieFin = myItem.SerieFin,
                            Fecha = myItem.Fecha,
                            //Hora = TimeSpan.MinValue,
                            Estado = myItem.Terminado,
                            SesionId=myItem.SesionId
                        };
                        //var detalle = myItem.ListaDetalleIASCupones;
                        //foreach (var det in detalle)
                        //{
                        //    DateTime detFecha = det.Fecha;
                        //    CMP_DetalleCuponesGeneradosEntidad detalleGenerado = new CMP_DetalleCuponesGeneradosEntidad();
                        //    detalleGenerado.Serie = det.Serie;
                        //    detalleGenerado.Fecha = det.Fecha;
                        //    detalleGenerado.CantidadImpresiones = det.CantidadImpresiones == null ? 0 : det.CantidadImpresiones;
                        //    listaDetalles.Add(detalleGenerado);
                        //}
                        //contador.ListaDetalleIASCupones = listaDetalles;
                        listaRespuesta.Add(contador);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return listaRespuesta;
        }

        [seguridad(false)]
        public List<CMP_DetalleCuponesImpresosEntidad> GetListadoDetalleCuponImpresoV2(long sesionId, string UrlProgresivoSala)
        {
            List<CMP_DetalleCuponesImpresosEntidad> listaRespuesta = new List<CMP_DetalleCuponesImpresosEntidad>();
            object oEnvio = new object();
            if(string.IsNullOrEmpty(UrlProgresivoSala)) {
                return (listaRespuesta);
            }
            try
            {
                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                oEnvio = new
                {
                    SesionId = sesionId
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                string mensaje = "";
                bool respuesta = false;
                var client = new System.Net.WebClient();
                var response = "";
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                string url = UrlProgresivoSala;
                url += "/servicio/OntenerListadoContadoresOnlineWebPorSesionId";
                response = client.UploadString(url, "POST", inputJson);
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                mensaje = jsonObj.mensaje;
                respuesta = Convert.ToBoolean(jsonObj.respuesta);
                if (respuesta)
                {
                    var items = jsonObj.data;
                    foreach (var myItem in items)
                    {
                        List<CMP_DetalleCuponesImpresosEntidad> listaDetalles = new List<CMP_DetalleCuponesImpresosEntidad>();
                        //DateTime fechaItem = myItem.Fecha;
                        //DateTime horaItem = myItem.Hora;
                        //DateTime fechaRegistroItem = myItem.FechaRegistro;
                        CMP_DetalleCuponesImpresosEntidad contador = new CMP_DetalleCuponesImpresosEntidad()
                        {
                            DetImpId = 0,
                            CgId = 0,
                            CodSala = 0,
                            SerieIni = myItem.SerieIni,
                            SerieFin = myItem.SerieFin,
                            CantidadCuponesImpresos = myItem.CantidadCupones,
                            UltimoCuponImpreso = string.Empty,
                            CoinOutIas = myItem.CoinOutIas,
                            CodMaq = myItem.CodMaq,
                            CoinOutAnterior = myItem.CoinOutAnterior,
                            CoinOut = myItem.CoinOut,
                            CurrentCredits = myItem.CurrentCredits,
                            Monto = myItem.Monto,
                            Token = myItem.Token,
                            FechaRegistro = myItem.FechaRegistro,
                            id = 0,
                            HandPay = myItem.HandPay,
                            JackPot = myItem.JackPot,
                            HandPayAnterior = myItem.HandPayAnterior,
                            JackPotAnterior = myItem.JackPotAnterior,
                            Cod_Cont=myItem.Cod_Cont
                        };
                        //var detalle = myItem.ListaDetalleIASCupones;
                        //foreach (var det in detalle)
                        //{
                        //    DateTime detFecha = det.Fecha;
                        //    CMP_DetalleCuponesGeneradosEntidad detalleGenerado = new CMP_DetalleCuponesGeneradosEntidad();
                        //    detalleGenerado.Serie = det.Serie;
                        //    detalleGenerado.Fecha = det.Fecha;
                        //    detalleGenerado.CantidadImpresiones = det.CantidadImpresiones == null ? 0 : det.CantidadImpresiones;
                        //    listaDetalles.Add(detalleGenerado);
                        //}
                        //contador.ListaDetalleIASCupones = listaDetalles;
                        listaRespuesta.Add(contador);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return listaRespuesta;
        }


        [seguridad(false)]
        public List<CMP_DetalleCuponesGeneradosEntidad> GetListadoDetalleCuponGeneradoV2(long Cod_Cont, string UrlProgresivoSala)
        {
            List<CMP_DetalleCuponesGeneradosEntidad> listaRespuesta = new List<CMP_DetalleCuponesGeneradosEntidad>();
            object oEnvio = new object();
            if(string.IsNullOrEmpty(UrlProgresivoSala)) {
                return (listaRespuesta);
            }
            try
            {
                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                oEnvio = new
                {
                    Cod_Cont = Cod_Cont
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                string mensaje = "";
                bool respuesta = false;
                var client = new System.Net.WebClient();
                var response = "";
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                string url = UrlProgresivoSala;
                url += "/servicio/ObtenerListadoDetalleCuponesGeneradosPorCod_Cont";
                response = client.UploadString(url, "POST", inputJson);
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                mensaje = jsonObj.mensaje;
                respuesta = Convert.ToBoolean(jsonObj.respuesta);
                if (respuesta)
                {
                    var items = jsonObj.data;
                    foreach (var myItem in items)
                    {
                        //List<CMP_DetalleCuponesImpresosEntidad> listaDetalles = new List<CMP_DetalleCuponesImpresosEntidad>();
                        //DateTime fechaItem = myItem.Fecha;
                        //DateTime horaItem = myItem.Hora;
                        //DateTime fechaRegistroItem = myItem.FechaRegistro;
                        CMP_DetalleCuponesGeneradosEntidad contador = new CMP_DetalleCuponesGeneradosEntidad()
                        {
                            DetGenId = myItem.DetGenId,
                            DetImId = 0,
                            CodSala = 0,
                            Serie = myItem.Serie,
                            CantidadImpresiones = myItem.CantidadImpresiones,
                            Fecha = myItem.Fecha,
                            UsuarioId = 0
                        };
                        //var detalle = myItem.ListaDetalleIASCupones;
                        //foreach (var det in detalle)
                        //{
                        //    DateTime detFecha = det.Fecha;
                        //    CMP_DetalleCuponesGeneradosEntidad detalleGenerado = new CMP_DetalleCuponesGeneradosEntidad();
                        //    detalleGenerado.Serie = det.Serie;
                        //    detalleGenerado.Fecha = det.Fecha;
                        //    detalleGenerado.CantidadImpresiones = det.CantidadImpresiones == null ? 0 : det.CantidadImpresiones;
                        //    listaDetalles.Add(detalleGenerado);
                        //}
                        //contador.ListaDetalleIASCupones = listaDetalles;
                        listaRespuesta.Add(contador);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return listaRespuesta;
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoCuponesxCampaniaV3(DtParameters dtParameters, int CampaniaId, string UrlProgresivoSala)
        {
            string mensaje = "";
            List<CMP_CuponesGeneradosEntidad> lista = new List<CMP_CuponesGeneradosEntidad>();

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = "";
            List<dynamic> registro = new List<dynamic>();
            int pageSize, skip;
            try
            {
                //lista = GetListadoCuponesxCampaniaV2(CampaniaId, UrlProgresivoSala);

                if (dtParameters.Order != null)
                {
                    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower();
                }
                else
                {
                    orderCriteria = "SesionId";
                    orderAscendingDirection = "asc";
                }

                string whereQuery = "where CampaniaId="+CampaniaId+" ";
                if (!string.IsNullOrEmpty(searchBy) && !(string.IsNullOrWhiteSpace(searchBy)))
                {
                    //Campos de la Tabla CMP_Sesion_Cupones_Cliente
                    string[] values = { "CodMaquina", "NombreCliente", "NroDocumento","SerieIni", "SerieFin" };
                    List<string> listaWhere = new List<string>();
                    foreach (var value in values)
                    {
                        listaWhere.Add($@" {value } like '%{searchBy}%' ");
                    }
                    whereQuery += $@" and ( {String.Join(" or ", listaWhere)} )";
                }
                pageSize = dtParameters.Length;
                skip = dtParameters.Start ;

                string AditionalWhere = $@" order by {orderCriteria} {orderAscendingDirection} offset {skip} rows fetch next {pageSize} rows only;";
                var respuestaTupla = GetListadoCuponesxCampaniaDataTable(whereQuery, AditionalWhere, UrlProgresivoSala);
                lista = respuestaTupla.lista;
                var filteredResultsCount = respuestaTupla.recordsFiltered;
                var totalResultsCount = respuestaTupla.recordsTotal;

                registro.Add(new
                {
                    mensaje = "Listando Registros",
                    draw = dtParameters.Draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = lista
                });
                mensaje = "Listando Registros";
            }
            catch (Exception exp)
            {
                mensaje = exp.Message;
            }
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var result = new ContentResult
            {
                Content = serializer.Serialize(registro.FirstOrDefault()),
                ContentType = "application/json"
            };
            return result;
            //return Json(new { respuesta , mensaje ,data=listaCupones });
        }
        [seguridad(false)]
        public (List<CMP_CuponesGeneradosEntidad>lista,int recordsTotal, int recordsFiltered) GetListadoCuponesxCampaniaDataTable(string WhereQuery, string QueryPaginacion, string UrlProgresivoSala)
        {
            List<CMP_CuponesGeneradosEntidad> listaRespuesta = new List<CMP_CuponesGeneradosEntidad>();
            object oEnvio = new object();
            int recordsTotal = 0;
            int recordsFiltered = 0;
            if(string.IsNullOrEmpty(UrlProgresivoSala)) {
                return (listaRespuesta, recordsTotal, recordsFiltered);
            }
            try
            {
                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                oEnvio = new
                {
                    Query = WhereQuery,
                    QueryPaginacion=QueryPaginacion
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                string mensaje = "";
                bool respuesta = false;
                var client = new System.Net.WebClient();
                var response = "";
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                string url = UrlProgresivoSala;
                url += "/servicio/ObtenerListadoSesionCuponesClienteDataTable";
                response = client.UploadString(url, "POST", inputJson);
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                mensaje = jsonObj.mensaje;
                respuesta = Convert.ToBoolean(jsonObj.respuesta);
                if (respuesta)
                {
                    var items = jsonObj.data;
                    recordsTotal = Convert.ToInt32(jsonObj.recordsTotal);
                    recordsFiltered =Convert.ToInt32(jsonObj.recordsFiltered);
                    foreach (var myItem in items)
                    {
                        List<CMP_DetalleCuponesGeneradosEntidad> listaDetalles = new List<CMP_DetalleCuponesGeneradosEntidad>();
                        CMP_CuponesGeneradosEntidad contador = new CMP_CuponesGeneradosEntidad()
                        {
                            CgId = myItem.CgId,
                            CampaniaId = myItem.CampaniaId,
                            ClienteId = myItem.ClienteId,
                            ApelPat = string.Empty,
                            ApelMat = string.Empty,
                            Nombre = string.Empty,
                            NombreCompleto = myItem.NombreCliente,
                            Mail = myItem.Correo,
                            FechaNacimiento = DateTime.Now,
                            NroDoc = myItem.NroDocumento,
                            CodSala = 0,
                            nombreSala = myItem.NombreSala,
                            UsuarioId = myItem.UsuarioIdIAS,
                            UsuarioNombre = myItem.UsuarioNombre,
                            SlotId = myItem.CodMaquina,
                            Juego = string.Empty,
                            Marca = string.Empty,
                            Modelo = string.Empty,
                            Win = 0,
                            Parametro = 0,
                            ValorJuego = myItem.CoinOutIAS,
                            CantidadCupones = myItem.CantidadCupones,
                            SaldoCupIni = 0,
                            SaldoCupFin = 0,
                            SerieIni = myItem.SerieIni,
                            SerieFin = myItem.SerieFin,
                            Fecha = myItem.Fecha,
                            //Hora = TimeSpan.MinValue,
                            Estado = myItem.Terminado,
                            SesionId = myItem.SesionId
                        };
                        listaRespuesta.Add(contador);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return (listaRespuesta,recordsTotal,recordsFiltered);
        }
    }
}