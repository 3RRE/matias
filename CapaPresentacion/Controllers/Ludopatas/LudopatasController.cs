using CapaDatos;
using CapaDatos.Utilitarios;
using CapaEntidad;
using CapaEntidad.AsistenciaCliente;
using CapaEntidad.ControlAcceso;
using CapaNegocio;
using CapaNegocio.AsistenciaCliente;
using CapaNegocio.ControlAcceso;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Ludopatas
{
    //[seguridad(false)]
    public class LudopatasController : Controller
    {
  
        private SEG_UsuarioBL usuarioBL = new SEG_UsuarioBL();
        private SalaBL salaBl = new SalaBL();
        private AST_ClienteBL clienteBL = new AST_ClienteBL();
        private SEG_RolUsuarioBL seg_rol_usuarioBL = new SEG_RolUsuarioBL();
        private SEG_PermisoRolBL seg_PermisoRolBL = new SEG_PermisoRolBL();
        private CAL_BusquedaBL busquedaBL = new CAL_BusquedaBL();
        private CAL_AuditoriaBL auditoriaBL = new CAL_AuditoriaBL();
        private CAL_CodigoBL codigoBL = new CAL_CodigoBL();
        private CAL_PersonaProhibidoIngresoIncidenciaIncidenciaBL timadorIncidenciaBL = new CAL_PersonaProhibidoIngresoIncidenciaIncidenciaBL();

        //[seguridad(false)]
        //[HttpPost]
        //public ActionResult BuscarGeneralSinSeguridadJson(string buscar, int usuarioid=0, int codsala=0)
        //{
        //    string UriSistemaRRHH = ConfigurationManager.AppSettings["LinkSistemaLudopatas"];
        //    string url = UriSistemaRRHH + "/Busqueda/BuscarGeneralSinSeguridadJson?buscar=" + buscar;
        //    object data = new object();
        //    string json = "";
        //    bool guardar = false;
        //    AST_ClienteEntidad clienteInsertar = new AST_ClienteEntidad();
        //    try
        //    {
        //        string soloNumeros = @"^[0-9]+$";
        //        if (!System.Text.RegularExpressions.Regex.IsMatch(buscar, soloNumeros) || buscar == string.Empty || buscar.Length < 8)
        //        {
        //            data = null;
        //            return Json(new { mensaje = "Formato de Documento Incorrecto", data }, JsonRequestBehavior.AllowGet);
        //        }
        //        SEG_UsuarioEntidad usuario = new SEG_UsuarioEntidad();
        //        SalaEntidad sala = new SalaEntidad();
        //        if (usuarioid != 0 && codsala != 0)
        //        {
        //            usuario = usuarioDal.UsuarioEmpleadoIDObtenerJson(usuarioid);
        //            sala = salaBl.SalaListaIdJson(codsala);
        //            string usuarioEnvio = "Movil (" + usuarioid + "-" + usuario.UsuarioNombre + ")";
        //            url += "&nombresala=" + sala.Nombre + "&nombreusuario=" + usuarioEnvio;
        //            guardar = true;
        //            clienteInsertar.FechaRegistro = DateTime.Now;
        //            clienteInsertar.Estado = "A";
        //            clienteInsertar.TipoRegistro = "SYSLUDOPATAS";
        //            clienteInsertar.usuario_reg = usuario.UsuarioID;
        //            clienteInsertar.SalaId = sala.CodSala;
        //        }
        //        using (var client = new HttpClient())
        //        {
        //            using (var response = client.GetAsync(url).Result)
        //            {
        //                if (response.IsSuccessStatusCode)
        //                {
        //                    json = response.Content.ReadAsStringAsync().Result;
        //                    dynamic jsonObj = JsonConvert.DeserializeObject(json);
        //                    if (jsonObj.data != null)
        //                    {
        //                        var item = jsonObj.data;
        //                        data = new
        //                        {
        //                            NombreCompleto=Convert.ToString(item.NombreCompleto),
        //                            DNI=Convert.ToString(item.DNI),
        //                            Tipo=Convert.ToString(item.Tipo),
        //                            Acceso=Convert.ToString(item.Acceso),
        //                            Foto=Convert.ToString(item.Foto),
        //                            ImgFondo=Convert.ToString(item.ImgFondo)
        //                        };
        //                        if (guardar&&Convert.ToString(item.Tipo).Equals("cliente"))
        //                        { 
        //                            clienteInsertar.Nombre = Convert.ToString(item.Nombre);
        //                            clienteInsertar.ApelPat = Convert.ToString(item.ApelPat);
        //                            clienteInsertar.ApelMat = Convert.ToString(item.ApelMat);
        //                            clienteInsertar.NombreCompleto = Convert.ToString(item.NombreCompleto);
        //                            clienteInsertar.NroDoc = Convert.ToString(item.DNI);
        //                            GuardarClienteLudoBusqueda(clienteInsertar);
        //                            //int IdInsertado = clienteBL.GuardarClienteLudopatas(clienteInsertar);
        //                        }
        //                        return Json(new { data }, JsonRequestBehavior.AllowGet);
        //                    }
        //                    else
        //                    {
        //                        data = null;
        //                        string mensaje = Convert.ToString(jsonObj.mensaje);
        //                        return Json(new { data, mensaje }, JsonRequestBehavior.AllowGet);
        //                    }
        //                }
        //                else
        //                {
        //                    data = null;
        //                    return Json(new { data,mensaje="Error en consulta a Ludopatas" }, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        data = null;
        //        return Json(new { mensaje = "Error al conectarse al Sistema Ludopatas", data }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        [seguridad(false)]
        public void GuardarClienteLudoBusqueda(AST_ClienteEntidad cliente)
        {
            try
            {
                List<AST_ClienteEntidad> clienteBusqueda = new List<AST_ClienteEntidad>();
                clienteBusqueda = clienteBL.GetListaClientesxNroDoc(cliente.NroDoc);
                if (clienteBusqueda.Count <= 0)
                {
                    int IdInsertado = clienteBL.GuardarClienteLudopatas(cliente);
                }

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult consultaPermisoLudopatamultiplesala(int usuario_id)
        {
            SalaEntidad sala = new SalaEntidad();
            var errormensaje = "";
            List<SalaEntidad> listaSalas = new List<SalaEntidad>();
            bool respuesta = false;
            SalaEntidad sala_unica = new SalaEntidad();
            try
            {
                var rol = seg_rol_usuarioBL.GetRolUsuarioId(usuario_id);

                string accion = "MobileSalaLudopataMultipleJson";
                var permiso = seg_PermisoRolBL.GetPermisoRolUsuario(rol.WEB_RolID, accion);

                if (permiso.Count == 0)
                {
                    respuesta = false;
                    errormensaje = "No tiene Permiso para multiples salas";
                    listaSalas = salaBl.ListadoSalaPorUsuario(usuario_id);
                    return Json(new { respuesta, mensaje = errormensaje , listaSalas }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                   
                    respuesta = true;
                }

            }
            catch (Exception ex)
            {

                return Json(new { respuesta, mensaje = ex.Message.ToString(), listaSalas });
            }

            return Json(new { respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet); ;
        }
        [HttpPost]
        public JsonResult MobileSalaLudopataMultipleJson()
        {
            bool respuesta = true;
            return Json(new { respuesta }, JsonRequestBehavior.AllowGet);
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult BuscarGeneralSinSeguridadJson(string buscar, int usuarioid = 0, int codsala = 0)
        {
            CAL_AuditoriaEntidad cliente = new CAL_AuditoriaEntidad();
            string UrlImagenes = string.Empty;
            object data = new object();
            object dataAdicional = new object();
            string NombreCompleto = string.Empty;
            string DNI = string.Empty;
            string Foto = "default_image_profile.jpg";
            string ImgFondo = string.Empty;
            CAL_LudopataEntidad ludopata;
            CAL_PersonaProhibidoIngresoEntidad timador;
            CAL_PoliticoEntidad politico;
            CAL_PersonaEntidadPublicaEntidad personaEntidadPublica;
            List<CAL_CodigoEntidad> listaCodigos;
            string TipoPersona = string.Empty;
            string nombreArchivo = string.Empty;
            SalaEntidad sala = new SalaEntidad();
            CAL_CodigoEntidad codigoUsar = new CAL_CodigoEntidad();
            bool SeguirBuscando = true;
            string Tipo = string.Empty;
            AST_ClienteEntidad clienteBusqueda = new AST_ClienteEntidad();
            string UrlSistemaReclutamiento = string.Empty;
            string PathPrincipal = string.Empty;
            AST_ClienteEntidad clienteInsertar = new AST_ClienteEntidad();
            bool guardarCliente = false; 
            bool Acceso = false;
            try
            {
                string soloNumeros = @"^[0-9]+$";
                if (!System.Text.RegularExpressions.Regex.IsMatch(buscar, soloNumeros) || buscar == string.Empty || buscar.Length < 8)
                {
                    data = null;
                    return Json(new { mensaje = "Formato de Documento Incorrecto", data }, JsonRequestBehavior.AllowGet);
                }
                DNI = buscar;
                UrlSistemaReclutamiento = ConfigurationManager.AppSettings["UriSistemaReclutamiento"].ToString();
                UrlImagenes = ConfigurationManager.AppSettings["UriImagenesLudopatas"].ToString();
                PathPrincipal = Path.Combine(ConfigurationManager.AppSettings["PathArchivos"].ToString(), "Ludopatas");
                SEG_UsuarioEntidad usuario = usuarioBL.UsuarioEmpleadoIDObtenerJson(usuarioid);
                sala = salaBl.SalaListaIdJson(codsala);
                cliente.Dni = buscar;
                cliente.NombreUsuario = usuario.UsuarioNombre;
                cliente.NombreSala = sala.Nombre;
                listaCodigos = codigoBL.GetAllCodigoJoinCodigoPersona();
                string dni = buscar;
                bool errorRRHH = false;
                ludopata = busquedaBL.GetLudopataJson(buscar);
                if (ludopata.LudopataID > 0 && SeguirBuscando)
                {
                    Tipo = "Ludopata";
                    TipoPersona = "PROHIBIDO";
                    NombreCompleto = $"{ludopata.Nombre} {ludopata.ApellidoPaterno} {ludopata.ApellidoMaterno}";
                    DNI = ludopata.DNI;
                    Foto = ludopata.Foto == "" ? Foto : ludopata.Foto;
                    SeguirBuscando = false;
                }
                if (SeguirBuscando)
                {
                    timador = busquedaBL.GetTimadorJson(buscar);
                    if (timador.TimadorID > 0)
                    {
                        List<int> codsSalas = salaBl.ObtenerCodsSalasDeSesion(Session);
                        List<CAL_PersonaProhibidoIngresoIncidenciaEntidad> incidenciaBusqueda = timadorIncidenciaBL.GetAllTimadorIncidenciaxTimadorActivo(timador.TimadorID, codsSalas);
                        bool esProhibido = timador.Prohibir == 1 || incidenciaBusqueda.Count > 0;

                        Tipo = esProhibido ? "Prohibido de Ingreso" : timador.ConAtenuante ? "Cliente Alerta" : "Problematico";
                        TipoPersona = esProhibido ? "PROHIBIDO" : timador.ConAtenuante ? "CLIENTE ALERTA" : "PROBLEMATICO";

                        Foto = timador.Foto == "" ? Foto : timador.Foto;
                        NombreCompleto = $"{timador.Nombre} {timador.ApellidoPaterno} {timador.ApellidoMaterno}";
                        DNI = timador.DNI;
                        SeguirBuscando = false;
                    }
                }

                if (SeguirBuscando)
                {
                    politico = busquedaBL.GetPoliticoJson(buscar);
                    if (politico.PoliticoID > 0)
                    {
                        Tipo = "Politico";
                        TipoPersona = "ENTIDADPUBLICA";
                        NombreCompleto = $"{politico.Nombres} {politico.Apellidos}";
                        DNI = politico.Dni;
                        Acceso = true;
                        SeguirBuscando = false;
                    }
                }
                if (SeguirBuscando)
                {
                    personaEntidadPublica = busquedaBL.GetPersonaEntidadPublicaJson(buscar);
                    if (personaEntidadPublica.EntidadPublicaID > 0)
                    {
                        Tipo = "Entidad Publica";
                        TipoPersona = "ENTIDADPUBLICA";
                        NombreCompleto = $"{personaEntidadPublica.Nombres} {personaEntidadPublica.Apellidos}";
                        DNI = personaEntidadPublica.Dni;
                        Acceso = true;
                        SeguirBuscando = false;
                    }
                }


                //Consulta a RRHH
                if (SeguirBuscando)
                {
                    string url = UrlSistemaReclutamiento + "ofisis/PersonaPorDniFechaCeseV2?dni=" + buscar + "&val=true";
                    string json = "";
                    try
                    {
                        using (var client = new HttpClient())
                        {

                            using (var response = client.GetAsync(url).Result)
                            {
                                if (response.IsSuccessStatusCode)
                                {
                                    json = response.Content.ReadAsStringAsync().Result;
                                    if (json != "{}")
                                    {
                                        string foto = string.Empty;
                                        dynamic jsonObj = JsonConvert.DeserializeObject(json);
                                        var item = jsonObj.data;

                                        if (Convert.ToString(item.CO_TRAB) != "")
                                        {
                                            Tipo = "Trabajador";
                                            if (item.CESE_ESTADO == 1)
                                            {
                                                TipoPersona = "EXTRABAJADOR";
                                                Acceso = true;
                                                DateTime fechaActual = DateTime.Now;
                                                DateTime fechaCese = Convert.ToDateTime(item.FE_CESE_TRAB);
                                                fechaCese = fechaCese.AddMonths(6);
                                                if (fechaActual > fechaCese)
                                                {
                                                    Tipo = "Ex Trabajador";
                                                }
                                            } else
                                            {
                                                TipoPersona = Convert.ToString(item.DE_GRUP_OCUP) == "" ? "TRA" : Convert.ToString(item.DE_GRUP_OCUP);
                                            }
                                            
                                            NombreCompleto = $"{Convert.ToString(item.NO_TRAB)} {Convert.ToString(item.NO_APEL_PATE)} {Convert.ToString(item.NO_APEL_MATE)}";
                                            DNI = Convert.ToString(item.CO_TRAB);
                                            SeguirBuscando = false;
                                        }
                                    }
                                }
                                else
                                {
                                    //data = null;
                                    errorRRHH = true;
                                    /*
                                    dataAdicional = new
                                    {
                                        errorOFISIS = true
                                    };*/
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        data = null;
                        errorRRHH = true;
                    }
                }
                if (SeguirBuscando)
                {
                    Acceso = true;
                    TipoPersona = "CLIENTE";
                    string tipoCliente = "Cliente";
                    if (errorRRHH)
                    {
                        tipoCliente += " (No verificado en RRHH).";
                    }
                    Tipo = tipoCliente;
                    //Consultar Tabla AST_Cliente
                    clienteBusqueda = clienteBL.GetClientexNroyTipoDoc(1, buscar);
                    if (clienteBusqueda.Id > 0)
                    {
                        NombreCompleto = $"{clienteBusqueda.NombreCompleto}";
                        DNI = clienteBusqueda.NroDoc;
                    }
                    else//Consulta a la API
                    {
                        var dataClienteAPI = apireniec(buscar);
                        if (Convert.ToString(dataClienteAPI[0].dni).Trim() != "")
                        {
                            NombreCompleto = Convert.ToString(dataClienteAPI[0].nombrecompleto);
                            DNI = Convert.ToString(dataClienteAPI[0].dni);

                            clienteInsertar.Nombre = Convert.ToString(dataClienteAPI[0].Nombre);
                            clienteInsertar.ApelPat = Convert.ToString(dataClienteAPI[0].ApellidoPaterno);
                            clienteInsertar.ApelMat = Convert.ToString(dataClienteAPI[0].ApellidoMaterno);
                            clienteInsertar.NombreCompleto = Convert.ToString(dataClienteAPI[0].NombreCompleto);
                            clienteInsertar.NroDoc = Convert.ToString(dataClienteAPI[0].DNI);
                            guardarCliente = true;
                        }
                        else
                        {
                            dataAdicional = new
                            {
                                mensaje = Convert.ToString(dataClienteAPI[0].ErrorMensaje),
                                errorAPI = true
                            };

                        }
                    }
                }
                var codigoConsulta = listaCodigos.Where(x => x.TipoPersona.Trim().ToLower().Equals(TipoPersona.Trim().ToLower())).FirstOrDefault();
                if (codigoConsulta == null)
                {
                    codigoUsar.Color = "green";
                    codigoUsar.Alerta = "CLI";
                    codigoUsar.TipoPersona = "CLIENTE";
                }
                else
                {
                    codigoUsar = codigoConsulta;
                }

                if(Tipo == "Trabajador") {
                    codigoUsar.Color = "#ff0000";
                    codigoUsar.Alerta = "TRA";
                    codigoUsar.TipoPersona = "TRABAJADOR";
                }

                bool archivoVerificado = VerificarArchivo(Path.Combine(PathPrincipal, $"{codigoUsar.Alerta.ToUpper().Trim()}.png"));
                nombreArchivo = archivoVerificado ? $"{codigoUsar.Alerta.ToUpper().Trim()}.png" : generarImagen(PathPrincipal, codigoUsar.Alerta.ToUpper().Trim(), codigoUsar.Color.ToLower().Trim());
                ImgFondo = UrlImagenes + nombreArchivo;
                Foto = UrlImagenes + "profile/standard/" + Foto;
                data = new
                {
                    NombreCompleto,
                    DNI,
                    Foto,
                    ImgFondo,
                    Codigo = codigoUsar.Alerta,
                    dataAdicional,
                    Tipo,
                    Acceso
                };
                //Metodo para insertar auditoria log
                cliente.Cliente = NombreCompleto;
                cliente.TipoCliente = Tipo;
                cliente.FechaRegistro = DateTime.Now;
                cliente.codigo = codigoUsar.Alerta;
                var insertado = auditoriaBL.RegistrarBusquedaExterno(cliente);
                if (guardarCliente)
                {
                    clienteInsertar.FechaRegistro = DateTime.Now;
                    clienteInsertar.Estado = "A";
                    clienteInsertar.TipoRegistro = "SYSLUDOPATAS";
                    clienteInsertar.usuario_reg = usuario.UsuarioID;
                    clienteInsertar.SalaId = sala.CodSala;
                    GuardarClienteLudoBusqueda(clienteInsertar);
                }
                return Json(new { data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data = null;
                return Json(new { mensaje = "Error al conectarse al Sistema Ludopatas", data }, JsonRequestBehavior.AllowGet);
            }
        }
        [seguridad(false)]
        public dynamic apireniec(string dni)
        {
            string vtoken = WebConfigurationManager.AppSettings["Token"];
            var rpta = false;

            //string json = "";
            dynamic item = new DynamicDictionary();
            List<dynamic> Lista = new List<dynamic>();
            item.ErrorMensaje = string.Empty;
            try
            {

                #region ocultar
                //reniec antiguo
                //using (var client = new HttpClient())
                //{

                //    System.Net.ServicePointManager.SecurityProtocol =
                //        System.Net.SecurityProtocolType.Tls12;
                //    using (var response = client.GetAsync(url).Result)
                //    {
                //        if (response.IsSuccessStatusCode)
                //        {
                //            json = response.Content.ReadAsStringAsync().Result;
                //             dynamic cliente = JsonConvert.DeserializeObject<dynamic>(json);
                //            item.NombreCompleto = cliente.nombres + " " + cliente.apellido_paterno + " " + cliente.apellido_materno;
                //            item.DNI = dni;
                //            item.Nombre = cliente.nombres;
                //            item.ApellidoPaterno = cliente.apellido_paterno;
                //            item.ApellidoMaterno = cliente.apellido_materno;

                //        }


                //    }
                //}
                #endregion

                #region apiperu

                string uri = "https://apiperu.dev/api/dni/" + dni;
                var clientApi = new RestClient(uri);
                var requestApi = new RestRequest(Method.GET);
                requestApi.AddHeader("Accept", "application/json");
                requestApi.AddHeader("Authorization", "Bearer " + "d2a43838fa0ba5f9f3c891d801b94cdf86839eded828bf85d6dbfdbf8b9cef19");

                try
                {
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    IRestResponse responseApi = clientApi.Execute(requestApi);
                    dynamic oDataApiReal = JsonConvert.DeserializeObject(responseApi.Content);
                    dynamic oDataApi = oDataApiReal.data;
                    if (oDataApi != null)
                    {
                        if (oDataApi.numero != null)
                        {


                            item.NombreCompleto = oDataApi.nombre_completo;
                            item.DNI = oDataApi.numero;
                            item.Nombre = oDataApi.nombres;
                            item.ApellidoPaterno = oDataApi.apellido_paterno;
                            item.ApellidoMaterno = oDataApi.apellido_materno;
                            rpta = true;
                        }
                        else
                        {
                            if (oDataApiReal.message != null)
                            {
                                item.ErrorMensaje = "Mensaje APIPERU.DEV: " + oDataApiReal.message;
                            }
                        }
                    }

                }
                catch (Exception op1)
                {
                    item.ErrorMensaje = op1.Message;
                }



                #endregion

                #region apiperu
                if (!rpta)
                {
                    uri = "https://apiperu.dev/api/dni/" + dni;
                    clientApi = new RestClient(uri);
                    requestApi = new RestRequest(Method.GET);
                    requestApi.AddHeader("Accept", "application/json");
                    requestApi.AddHeader("Authorization", "Bearer " + "71dd9c2a2d474859794cdb4bd251652951c817a2fa1ff48110c02e8a4e0db2f5");
                    try
                    {
                        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        IRestResponse responseApi = clientApi.Execute(requestApi);
                        dynamic oDataApiReal = JsonConvert.DeserializeObject(responseApi.Content);
                        dynamic oDataApi = oDataApiReal.data;
                        if (oDataApi != null)
                        {
                            if (oDataApi.numero != null)
                            {


                                item.NombreCompleto = oDataApi.nombre_completo;
                                item.DNI = oDataApi.numero;
                                item.Nombre = oDataApi.nombres;
                                item.ApellidoPaterno = oDataApi.apellido_paterno;
                                item.ApellidoMaterno = oDataApi.apellido_materno;
                                rpta = true;
                            }
                            else
                            {
                                if (oDataApiReal.message != null)
                                {
                                    item.ErrorMensaje = "Mensaje APIPERU.DEV: " + oDataApiReal.message;
                                }
                            }
                        }

                    }
                    catch (Exception op1)
                    {

                        item.ErrorMensaje = op1.Message;
                    }
                }

                #endregion



                #region consultas.pe
                if (!rpta)
                {
                    string url = "https://consulta.pe/api/reniec/dni";
                    var client = new RestClient(url);
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Accept", "application/json");
                    request.AddHeader("Authorization", "Bearer " + vtoken);
                    request.AddParameter("dni", dni);
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    IRestResponse response = client.Execute(request);
                    dynamic oData = JsonConvert.DeserializeObject(response.Content);
                    if (oData != null)
                    {
                        if (oData.dni != null)
                        {

                            item.NombreCompleto = oData.nombres + " " + oData.apellido_paterno + " " + oData.apellido_materno;
                            item.DNI = dni;
                            item.Nombre = oData.nombres;
                            item.ApellidoPaterno = oData.apellido_paterno;
                            item.ApellidoMaterno = oData.apellido_materno;
                            rpta = true;
                        }
                        else
                        {
                            item.NombreCompleto = "Cliente No Encontrado";
                            item.DNI = "";
                            item.Nombre = "";
                            item.ApellidoPaterno = "";
                            item.ApellidoMaterno = "";
                            item.ErrorMensaje = "Mensaje CONSULTA.PE: " + oData.message;

                        }
                    }
                    else
                    {
                        item.NombreCompleto = "Cliente No Encontrado";
                        item.DNI = "";
                        item.Nombre = "";
                        item.ApellidoPaterno = "";
                        item.ApellidoMaterno = "";
                        item.ErrorMensaje = "Error de Conexion a Consulta.pe";
                    }
                }

                #endregion



            }
            catch (Exception e)
            {
                item.NombreCompleto = "Cliente No Encontrado";
                item.DNI = "";
                item.Nombre = "";
                item.ApellidoPaterno = "";
                item.ApellidoMaterno = "";
                item.ErrorMensaje = e.Message;
            }
            Lista.Add(item);
            return Lista;
            // return Json(new { data = item, mensaje = "" }, JsonRequestBehavior.AllowGet);
        }
        [seguridad(false)]
        public string generarImagen(string parthinsercion, string texto, string color)
        {
            Color coloreado;
            coloreado = Color.FromName(color);

            Bitmap objBitmap = new Bitmap(1, 1);
            int Width = 0;
            int Height = 0;
            int marginSize = 6;


            Font objFont = new Font("Arial", 100,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Pixel);

            Graphics objGraphics = Graphics.FromImage(objBitmap);

            Width = (int)objGraphics.MeasureString(texto, objFont).Width;
            Height = (int)objGraphics.MeasureString(texto, objFont).Height;
            objBitmap = new Bitmap(objBitmap, new Size(Width + marginSize * 2, Height + marginSize * 2));


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
                new SolidBrush(coloreado), marginSize, marginSize);

            using (GraphicsPath path = RoundedRect(new Rectangle(marginSize, marginSize, Width, Height), Height / 2))
            {
                objGraphics.DrawPath(new Pen(new SolidBrush(coloreado), marginSize), path);
            }

            //objGraphics.DrawRectangle(new Pen(new SolidBrush(coloreado), 4), new Rectangle(0, 0, Width, Height));


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
            Bitmap rotatedImage = new Bitmap(hypotenuse, hypotenuse);
            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                g.TranslateTransform((float)rotatedImage.Width / 2, (float)rotatedImage.Height / 2);
                g.RotateTransform(angle);
                g.TranslateTransform(-(float)rotatedImage.Width / 2, -(float)rotatedImage.Height / 2);
                g.DrawImage(bmp, (hypotenuse - width) / 2, (hypotenuse - height) / 2, width, height);
            }
            return rotatedImage;
        }

        public static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            // top left arc  
            path.AddArc(arc, 180, 90);

            // top right arc  
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc  
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc 
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
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