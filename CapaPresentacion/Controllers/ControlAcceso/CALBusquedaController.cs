using CapaDatos.Utilitarios;
using CapaEntidad;
using CapaEntidad.AsistenciaCliente;
using CapaEntidad.ControlAcceso;
using CapaNegocio;
using CapaNegocio.AsistenciaCliente;
using CapaNegocio.ControlAcceso;
using CapaPresentacion.Models;
using CapaPresentacion.Utilitarios;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.ControlAcceso {
    [seguridad]
    public class CALBusquedaController : Controller {
        private SEG_UsuarioBL usuarioBL = new SEG_UsuarioBL();
        private CAL_BusquedaBL busquedaBL = new CAL_BusquedaBL();
        private CAL_AuditoriaBL auditoriaBL = new CAL_AuditoriaBL();
        private AST_ClienteSalaBL ast_clienteSalaBL = new AST_ClienteSalaBL();
        private SalaBL salaBL = new SalaBL();
        private CAL_CodigoBL codigoBL = new CAL_CodigoBL();
        private AST_ClienteBL ast_ClienteBL = new AST_ClienteBL();
        private CAL_PersonaProhibidoIngresoIncidenciaIncidenciaBL timadorIncidenciaBL = new CAL_PersonaProhibidoIngresoIncidenciaIncidenciaBL();
        private int CodigoSalaSomosCasino = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoSalaSomosCasino"]);

        public ActionResult Busqueda() {
            return View("~/Views/ControlAcceso/Busqueda.cshtml");
        }

        //[seguridad(false)]
        //private dynamic apireniec(string dni)
        //{
        //    string vtoken = WebConfigurationManager.AppSettings["Token"];
        //    //string vtoken = "asd";
        //    var rpta = false;

        //    //string json = "";
        //    dynamic item = new DynamicDictionary();
        //    List<dynamic> Lista = new List<dynamic>();
        //    item.ErrorMensaje = string.Empty;
        //    try
        //    {
        //        if (Helpers.IsValidDNI(dni))
        //        {
        //            #region consultas.pe
        //            if (!rpta)
        //            {
        //                string url = "https://consulta.pe/api/reniec/dni";
        //                var client = new RestClient(url);
        //                var request = new RestRequest(Method.POST);
        //                request.AddHeader("Accept", "application/json");
        //                request.AddHeader("Authorization", "Bearer " + vtoken);
        //                request.AddParameter("dni", dni);
        //                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //                IRestResponse response = client.Execute(request);
        //                dynamic oData = JsonConvert.DeserializeObject(response.Content);
        //                if (oData != null)
        //                {
        //                    var tipoDato = oData.GetType();
        //                    if (tipoDato.FullName == "Newtonsoft.Json.Linq.JObject")
        //                    {
        //                        if (oData.dni != null)
        //                        {

        //                            item.NombreCompleto = oData.nombres + " " + oData.apellido_paterno + " " + oData.apellido_materno;
        //                            item.DNI = dni;
        //                            item.Nombre = oData.nombres;
        //                            item.ApellidoPaterno = oData.apellido_paterno;
        //                            item.ApellidoMaterno = oData.apellido_materno;
        //                            rpta = true;
        //                        }
        //                        else
        //                        {
        //                            item.NombreCompleto = "Cliente No Encontrado";
        //                            item.DNI = "";
        //                            item.Nombre = "";
        //                            item.ApellidoPaterno = "";
        //                            item.ApellidoMaterno = "";
        //                            item.ErrorMensaje = "Mensaje CONSULTA.PE: " + oData.message;

        //                        }
        //                    }
        //                    else
        //                    {
        //                        item.NombreCompleto = "Cliente No Encontrado";
        //                        item.DNI = "";
        //                        item.Nombre = "";
        //                        item.ApellidoPaterno = "";
        //                        item.ApellidoMaterno = "";
        //                        item.ErrorMensaje = oData;
        //                    }
        //                }
        //                else
        //                {
        //                    item.NombreCompleto = "Cliente No Encontrado";
        //                    item.DNI = "";
        //                    item.Nombre = "";
        //                    item.ApellidoPaterno = "";
        //                    item.ApellidoMaterno = "";
        //                    item.ErrorMensaje = "Error de Conexion a Consulta.pe";
        //                }
        //            }

        //            #endregion
        //        }
        //        else
        //        {
        //            item.NombreCompleto = "Cliente No Encontrado";
        //            item.DNI = "";
        //            item.Nombre = "";
        //            item.ApellidoPaterno = "";
        //            item.ApellidoMaterno = "";
        //            item.ErrorMensaje = "El número de documento es invalido";
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        item.NombreCompleto = "Cliente No Encontrado";
        //        item.DNI = "";
        //        item.Nombre = "";
        //        item.ApellidoPaterno = "";
        //        item.ApellidoMaterno = "";
        //        item.ErrorMensaje = e.Message;
        //    }
        //    Lista.Add(item);
        //    return Lista;
        //    // return Json(new { data = item, mensaje = "" }, JsonRequestBehavior.AllowGet);
        //}
        [seguridad(false)]
        private string generarImagen(string parthinsercion, string texto, string color, int idCodigo = 0) {
            color = color.Replace("#", "");
            color = "#ff" + color;
            int argb = Int32.Parse(color.Replace("#", ""), NumberStyles.HexNumber);
            Color coloreado = Color.FromArgb(argb);

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

            using(GraphicsPath path = RoundedRect(new Rectangle(marginSize, marginSize, Width, Height), Height / 2)) {
                objGraphics.DrawPath(new Pen(new SolidBrush(coloreado), marginSize), path);
            }

            //objGraphics.DrawRectangle(new Pen(new SolidBrush(coloreado), 4), new Rectangle(0, 0, Width, Height));


            objGraphics.Flush();

            objBitmap = RotateImage(objBitmap, -45);

            string nombreArchivo = idCodigo == 0 ? $"{texto}.png" : $"{texto}-{idCodigo}.png";
            string rutaImagen = Path.Combine(parthinsercion, nombreArchivo);
            objBitmap.Save(rutaImagen, ImageFormat.Png);
            return nombreArchivo;
        }
        [seguridad(false)]
        private Bitmap RotateImage(Bitmap bmp, float angle) {
            float height = bmp.Height;
            float width = bmp.Width;
            int hypotenuse = System.Convert.ToInt32(System.Math.Floor(Math.Sqrt(height * height + width * width)));
            Bitmap rotatedImage = new Bitmap(hypotenuse, hypotenuse);
            using(Graphics g = Graphics.FromImage(rotatedImage)) {
                g.TranslateTransform((float)rotatedImage.Width / 2, (float)rotatedImage.Height / 2);
                g.RotateTransform(angle);
                g.TranslateTransform(-(float)rotatedImage.Width / 2, -(float)rotatedImage.Height / 2);
                g.DrawImage(bmp, (hypotenuse - width) / 2, (hypotenuse - height) / 2, width, height);
            }
            return rotatedImage;
        }
        [seguridad(false)]
        public static GraphicsPath RoundedRect(Rectangle bounds, int radius) {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if(radius == 0) {
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
        [seguridad(false)]
        private bool VerificarArchivo(string path) {
            bool respuesta = false;
            try {
                if(System.IO.File.Exists(path)) {
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

        [seguridad(false)]
        [HttpGet]
        public async Task<ActionResult> BuscarGeneralJson(string buscar) {
            if(Session["usuario"] == null) {
                return new HttpStatusCodeResult(561, "No hay Sesión de Usuario");
            }

            CAL_AuditoriaEntidad cliente = new CAL_AuditoriaEntidad();
            string UrlImagenes = string.Empty;
            object data = new object();
            object dataAdicional = new object();
            string NombreCompleto = string.Empty;
            string DNI = string.Empty;
            string Foto = "default_image_profile.jpg";
            string FotoDefault = "default_image_profile.jpg";
            string ImgFondo = string.Empty;
            CAL_LudopataEntidad ludopata;
            CAL_PersonaProhibidoIngresoEntidad timador;
            CAL_RobaStackersBilleteroEntidad robaStackersBilletero;
            CAL_PoliticoEntidad politico;
            CAL_PersonaEntidadPublicaEntidad personaEntidadPublica;
            List<CAL_CodigoEntidad> listaCodigos;
            string TipoPersona = string.Empty;
            string nombreArchivo = string.Empty;
            List<SalaEntidad> listaSalas = new List<SalaEntidad>();
            CAL_CodigoEntidad codigoUsar = new CAL_CodigoEntidad();
            bool SeguirBuscando = true;
            string Tipo = string.Empty;
            AST_ClienteEntidad clienteBusqueda = new AST_ClienteEntidad();
            string UrlSistemaReclutamiento = string.Empty;
            string PathPrincipal = string.Empty;
            AST_ClienteEntidad clienteInsertar = new AST_ClienteEntidad();
            bool guardarCliente = false;
            CAL_MenorDeEdadEntidad menorDeEdad = new CAL_MenorDeEdadEntidad();
            try {
                DNI = buscar;
                UrlSistemaReclutamiento = ConfigurationManager.AppSettings["UriSistemaReclutamiento"].ToString();
                UrlImagenes = ConfigurationManager.AppSettings["UriImagenesLudopatas"].ToString();
                PathPrincipal = Path.Combine(ConfigurationManager.AppSettings["PathArchivos"].ToString(), "Ludopatas");
                SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];
                listaSalas = salaBL.ListadoSalaPorUsuario(usuario.UsuarioID).Where(x => x.CodSala != CodigoSalaSomosCasino).ToList();
                if(listaSalas.Count != 1) {
                    return Json(new { mensaje = "Para realizar esta acción usted debe tener asignada solo una(1) sala", respuesta = false }, JsonRequestBehavior.AllowGet);
                }
                SalaEntidad sala = listaSalas.FirstOrDefault();
                //SalaEntidad sala = salaBL.ListadoSalaPorUsuario(usuario.UsuarioID).FirstOrDefault();
                cliente.Dni = buscar;
                cliente.NombreUsuario = usuario.UsuarioNombre;
                cliente.NombreSala = sala.Nombre;
                listaCodigos = codigoBL.GetAllCodigoJoinCodigoPersona();
                string dni = buscar;
                bool errorRRHH = false;
                ludopata = busquedaBL.GetLudopataJson(buscar);
                string observacion = "";
                if(ludopata.LudopataID > 0 && SeguirBuscando) {
                    Tipo = "Ludopata";
                    TipoPersona = "PROHIBIDO";
                    NombreCompleto = $"{ludopata.Nombre} {ludopata.ApellidoPaterno} {ludopata.ApellidoMaterno}";
                    DNI = ludopata.DNI;
                    Foto = ludopata.Foto == "" ? Foto : ludopata.Foto;
                    if(ludopata.ContactoID > 0) {
                        dataAdicional = new {
                            ludopata.NombreContacto,
                            ludopata.ApellidoPaternoContacto,
                            ludopata.ApellidoMaternoContacto,
                            ludopata.TelefonoContacto,
                            ludopata.CelularContacto,
                            ludopata.ContactoID,
                            ludopata.CodRegistro,
                            ludopata.FechaInscripcion,
                            ludopata.Telefono
                        };
                    }
                    SeguirBuscando = false;
                }
                if(SeguirBuscando) {
                    timador = busquedaBL.GetTimadorJson(buscar);
                    if(timador.TimadorID > 0) {
                        List<int> codsSalas = salaBL.ObtenerCodsSalasDeSesion(Session);
                        List<CAL_PersonaProhibidoIngresoIncidenciaEntidad> incidenciaBusqueda = timadorIncidenciaBL.GetAllTimadorIncidenciaxTimadorActivo(timador.TimadorID, codsSalas);
                        bool esProhibido = timador.Prohibir == 1 || incidenciaBusqueda.Count > 0;

                        Tipo = esProhibido ? "Prohibido de Ingreso" : timador.ConAtenuante ? "Cliente Alerta" : "Problematico";
                        TipoPersona = esProhibido ? "PROHIBIDO" : timador.ConAtenuante ? "CLIENTE ALERTA" : "PROBLEMATICO";

                        Foto = timador.Foto == "" ? Foto : timador.Foto;
                        NombreCompleto = $"{timador.Nombre} {timador.ApellidoPaterno} {timador.ApellidoMaterno}";
                        DNI = timador.DNI;

                        dataAdicional = new {
                            FechaInscripcion = timador.FechaRegistro,
                            SalaNombre = timador.SalaNombre,
                            SalaNombreCompuesto = String.Join(" - ", incidenciaBusqueda.Select(x => x.SalaNombre.ToString()).ToList()),
                            Observacion = timador.Observacion
                        };
                        SeguirBuscando = false;
                    }
                }

                if(SeguirBuscando) {
                    politico = busquedaBL.GetPoliticoJson(buscar);
                    if(politico.PoliticoID > 0) {
                        Tipo = "Politico";
                        TipoPersona = "ENTIDADPUBLICA";
                        NombreCompleto = $"{politico.Nombres} {politico.Apellidos}";
                        DNI = politico.Dni;
                        dataAdicional = new {
                            FechaRegistro = politico.FechaRegistro,
                            EntidadEstatal = politico.EntidadEstatal,
                            cargoPoliticoNombre = politico.cargoPoliticoNombre
                        };
                        SeguirBuscando = false;
                    }
                }
                if(SeguirBuscando) {
                    personaEntidadPublica = busquedaBL.GetPersonaEntidadPublicaJson(buscar);
                    if(personaEntidadPublica.EntidadPublicaID > 0) {
                        Tipo = "Entidad Publica";
                        TipoPersona = "ENTIDADPUBLICA";
                        NombreCompleto = $"{personaEntidadPublica.Nombres} {personaEntidadPublica.Apellidos}";
                        DNI = personaEntidadPublica.Dni;
                        dataAdicional = new {
                            FechaRegistro = personaEntidadPublica.FechaRegistro,
                            EntidadPublicaNombre = personaEntidadPublica.EntidadPublicaNombre,
                            CargoEntidadNombre = personaEntidadPublica.CargoEntidadNombre
                        };
                        SeguirBuscando = false;
                    }
                }

                //Roba Stackers Billetero
                if(SeguirBuscando) {
                    robaStackersBilletero = busquedaBL.GetRobaStackersBilletero(buscar);
                    if(robaStackersBilletero.RobaStackersBilleteroID > 0) {
                        Tipo = "R. Stacker Billetero";
                        TipoPersona = "ROBASTACKER";
                        NombreCompleto = $"{robaStackersBilletero.Nombre} {robaStackersBilletero.ApellidoPaterno} {robaStackersBilletero.ApellidoMaterno}";
                        DNI = robaStackersBilletero.DNI;
                        Foto = robaStackersBilletero.Foto == "" ? Foto : robaStackersBilletero.Foto;

                        dataAdicional = new {
                            FechaInscripcion = robaStackersBilletero.FechaInscripcion,
                            SalaNombre = robaStackersBilletero.SalaNombre,
                            Observacion = robaStackersBilletero.Observacion
                        };
                        SeguirBuscando = false;
                    }
                }

                if(SeguirBuscando) {
                    menorDeEdad = busquedaBL.GetMenorDeEdad(buscar);
                    if(menorDeEdad.IdMenor > 0) {
                        Tipo = "Persona con Observación";
                        TipoPersona = "MENOREDAD";
                        NombreCompleto = $"{menorDeEdad.Nombre} {menorDeEdad.ApellidoPaterno} {menorDeEdad.ApellidoMaterno}";
                        DNI = menorDeEdad.Doi;
                        dataAdicional = new {
                            Mensaje = "Por favor, comuníquese con control interno"
                        };
                        SeguirBuscando = false;
                    }
                }

                //Consulta a RRHH
                if(SeguirBuscando) {
                    string url = UrlSistemaReclutamiento + "ofisis/PersonaPorDniFechaCeseV2?dni=" + buscar.Trim() + "&val=true";
                    string json = "";
                    try {
                        using(var client = new HttpClient()) {

                            using(var response = client.GetAsync(url).Result) {
                                if(response.IsSuccessStatusCode) {
                                    json = response.Content.ReadAsStringAsync().Result;
                                    if(json != "{}") {
                                        string foto = string.Empty;
                                        dynamic jsonObj = JsonConvert.DeserializeObject(json);
                                        var item = jsonObj.data;

                                        if(Convert.ToString(item.CO_TRAB) != "") {
                                            Tipo = "Trabajador";
                                            if(item.CESE_ESTADO == 1) {

                                                DateTime fechaActual = DateTime.Now;
                                                DateTime fechaCese = Convert.ToDateTime(item.FE_CESE_TRAB);
                                                fechaCese = fechaCese.AddMonths(6);
                                                if(fechaActual > fechaCese) {
                                                    TipoPersona = "CLIENTE";
                                                    Tipo = "Cliente";
                                                } else {
                                                    TipoPersona = "EXTRABAJADOR";
                                                    Tipo = "Ex Trabajador";
                                                }
                                                dataAdicional = new {
                                                    FE_CESE_TRAB = Convert.ToDateTime(item.FE_CESE_TRAB),
                                                    DE_NOMB = Convert.ToString(item.DE_NOMB),
                                                    DE_PUES_TRAB = Convert.ToString(item.DE_PUES_TRAB),
                                                    DE_SEDE = Convert.ToString(item.DE_SEDE)
                                                };

                                            } else {
                                                TipoPersona = Convert.ToString(item.DE_GRUP_OCUP) == "" ? "TRA" : Convert.ToString(item.DE_GRUP_OCUP);
                                                dataAdicional = new {
                                                    DE_NOMB = Convert.ToString(item.DE_NOMB),
                                                    DE_PUES_TRAB = Convert.ToString(item.DE_PUES_TRAB),
                                                    DE_SEDE = Convert.ToString(item.DE_SEDE)
                                                };
                                            }

                                            NombreCompleto = $"{Convert.ToString(item.NO_TRAB)} {Convert.ToString(item.NO_APEL_PATE)} {Convert.ToString(item.NO_APEL_MATE)}";
                                            DNI = Convert.ToString(item.CO_TRAB);
                                            SeguirBuscando = false;
                                        }
                                    }
                                } else {
                                    //data = null;
                                    errorRRHH = true;
                                    observacion = "ERROR OFISIS - No se pudo conectar";

                                    //dataAdicional = new
                                    //{
                                    //    errorOFISIS = true
                                    //};
                                }
                            }
                        }
                    } catch(Exception e) {
                        data = null;
                        errorRRHH = true;
                        observacion = "ERROR OFISIS - No se pudo conectar - " + e.ToString();
                    }

                }
                if(SeguirBuscando) {
                    TipoPersona = "CLIENTE";
                    string tipoCliente = "Cliente";
                    if(errorRRHH) {
                        tipoCliente += " - No verificado en RRHH";
                    }
                    Tipo = tipoCliente;
                    //Consultar Tabla AST_Cliente
                    var clienteBusquedaFirst = ast_ClienteBL.GetListaClientesxNroDocMetodoBusqueda(buscar).FirstOrDefault();
                    if(clienteBusquedaFirst != null) {
                        clienteBusqueda = clienteBusquedaFirst;
                        NombreCompleto = $"{clienteBusqueda.NombreCompleto}";
                        DNI = clienteBusqueda.NroDoc;
                    } else//Consulta a la API
                      {
                        string responseMessage = "";

                        var dataClienteAPI = await apireniec(buscar.Trim());
                        if(Convert.ToString(dataClienteAPI[0].dni).Trim() != "") {
                            NombreCompleto = Convert.ToString(dataClienteAPI[0].nombrecompleto);
                            DNI = Convert.ToString(dataClienteAPI[0].dni);

                            clienteInsertar.Nombre = Convert.ToString(dataClienteAPI[0].Nombre);
                            clienteInsertar.ApelPat = Convert.ToString(dataClienteAPI[0].ApellidoPaterno);
                            clienteInsertar.ApelMat = Convert.ToString(dataClienteAPI[0].ApellidoMaterno);
                            clienteInsertar.NombreCompleto = Convert.ToString(dataClienteAPI[0].NombreCompleto);
                            clienteInsertar.NroDoc = Convert.ToString(dataClienteAPI[0].DNI);
                            guardarCliente = true;

                            responseMessage = "Consulta completada";
                        } else {
                            TipoPersona = "CLIENTE ERROR";
                            observacion = "ERROR API - No se pudo conectar - " + Convert.ToString(dataClienteAPI[0].ErrorMensaje);
                            dataAdicional = new {
                                mensaje = Convert.ToString(dataClienteAPI[0].ErrorMensaje),
                                errorAPI = true
                            };

                            responseMessage = Convert.ToString(dataClienteAPI[0].ErrorMensaje);
                        }

                        // save log for channel web of api reniec
                        Helpers.APIReniecLog(Helpers.CHANNEL_WEB, cliente.NombreSala, cliente.NombreUsuario, cliente.Dni, responseMessage);
                    }
                }
                var codigoConsulta = listaCodigos.Where(x => x.TipoPersona.Trim().ToLower().Equals(TipoPersona.Trim().ToLower())).FirstOrDefault();
                if(codigoConsulta == null) {
                    codigoUsar.Color = "#00FF00";
                    codigoUsar.Alerta = "CLI";
                    codigoUsar.TipoPersona = "CLIENTE";
                } else {
                    codigoUsar = codigoConsulta;
                }

                if(Tipo == "Trabajador") {
                    codigoUsar.Color = "#ff0000";
                    codigoUsar.Alerta = "TRA";
                    codigoUsar.TipoPersona = "TRABAJADOR";
                }

                nombreArchivo = $"{codigoUsar.Alerta.ToUpper().Trim()}-{codigoUsar.CodigoID}.png";
                bool archivoVerificado = VerificarArchivo(Path.Combine(PathPrincipal, nombreArchivo));
                nombreArchivo = archivoVerificado ? nombreArchivo : generarImagen(PathPrincipal, codigoUsar.Alerta.ToUpper().Trim(), codigoUsar.Color.ToLower().Trim(), codigoUsar.CodigoID);
                ImgFondo = UrlImagenes + nombreArchivo;
                Foto = UrlImagenes + "profile/standard/" + Foto;
                FotoDefault = UrlImagenes + "profile/standard/" + FotoDefault;
                data = new {
                    NombreCompleto,
                    DNI,
                    Foto,
                    FotoDefault,
                    ImgFondo,
                    Codigo = codigoUsar.Alerta,
                    dataAdicional,
                    Tipo,
                };
                //Metodo para insertar auditoria log
                cliente.Cliente = NombreCompleto;
                cliente.TipoCliente = Tipo;
                cliente.FechaRegistro = DateTime.Now;
                cliente.codigo = codigoUsar.Alerta;
                cliente.observacion = observacion;
                var insertado = auditoriaBL.RegistrarBusquedaExterno(cliente);
                if(guardarCliente) {
                    clienteInsertar.FechaRegistro = DateTime.Now;
                    clienteInsertar.Estado = "A";
                    clienteInsertar.TipoRegistro = "SYSLUDOPATAS";
                    clienteInsertar.usuario_reg = usuario.UsuarioID;
                    clienteInsertar.SalaId = sala.CodSala;
                    GuardarClienteLudoBusqueda(clienteInsertar);
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                data = new {
                    NombreCompleto = "",
                    DNI = "",
                    Foto = UrlImagenes + "default_image_profile.jpg",
                    FotoDefault = UrlImagenes + "default_image_profile.jpg",
                    ImgFondo = "",
                    Codigo = "",
                    dataAdicional = new { },
                    Tipo = "",
                };
            }

            return Json(new { data, mensaje = Tipo, respuesta = true }, JsonRequestBehavior.AllowGet);

        }
        [seguridad(false)]
        private void GuardarClienteLudoBusqueda(AST_ClienteEntidad cliente) {
            try {
                List<AST_ClienteEntidad> clienteBusqueda = new List<AST_ClienteEntidad>();
                clienteBusqueda = ast_ClienteBL.GetListaClientesxNroDoc(cliente.NroDoc);
                if(clienteBusqueda.Count <= 0) {
                    cliente.TipoDocumentoId = 1;
                    int IdInsertado = ast_ClienteBL.GuardarClienteLudopatas(cliente);
                    if(IdInsertado > 0) {
                        //editar en tabla ClienteSala
                        AST_ClienteSalaEntidad clienteSala = new AST_ClienteSalaEntidad();
                        AST_ClienteSalaEntidad clienteSalaConsulta = new AST_ClienteSalaEntidad();
                        clienteSalaConsulta = ast_clienteSalaBL.GetClienteSalaID(cliente.Id, cliente.SalaId);
                        clienteSala.ClienteId = IdInsertado;
                        clienteSala.SalaId = cliente.SalaId;
                        clienteSala.ApuestaImportante = 0;
                        clienteSala.TipoFrecuenciaId = 0;
                        clienteSala.TipoClienteId = 0;
                        clienteSala.TipoJuegoId = 0;
                        clienteSala.TipoRegistro = cliente.TipoRegistro;
                        if(clienteSalaConsulta.ClienteId == 0) {
                            ast_clienteSalaBL.GuardarClienteSala(clienteSala);
                        }

                    }

                }

            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        [seguridad(false)]
        [HttpPost]
        public async Task<ActionResult> BuscarGeneralSinSeguridadJson(string buscar, int usuarioid = 0, int codsala = 0) {
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
            CAL_RobaStackersBilleteroEntidad robaStackersBilletero;
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
            string Observacion = "";
            string observacionReporte = "";
            CAL_MenorDeEdadEntidad menorDeEdad = new CAL_MenorDeEdadEntidad();
            try {
                string soloNumeros = @"^[0-9]+$";
                if(!System.Text.RegularExpressions.Regex.IsMatch(buscar, soloNumeros) || buscar == string.Empty || buscar.Length < 8) {
                    data = null;
                    return Json(new { mensaje = "Formato de Documento Incorrecto", data }, JsonRequestBehavior.AllowGet);
                }
                DNI = buscar;
                UrlSistemaReclutamiento = ConfigurationManager.AppSettings["UriSistemaReclutamiento"].ToString();
                UrlImagenes = ConfigurationManager.AppSettings["UriImagenesLudopatas"].ToString();
                PathPrincipal = Path.Combine(ConfigurationManager.AppSettings["PathArchivos"].ToString(), "Ludopatas");
                SEG_UsuarioEntidad usuario = usuarioBL.UsuarioEmpleadoIDObtenerJson(usuarioid);
                sala = salaBL.SalaListaIdJson(codsala);
                cliente.Dni = buscar;
                cliente.NombreUsuario = "Móvil - " + usuario.UsuarioNombre;
                cliente.NombreSala = sala.Nombre;
                listaCodigos = codigoBL.GetAllCodigoJoinCodigoPersona();
                string dni = buscar;
                ludopata = busquedaBL.GetLudopataJson(buscar);
                if(ludopata.LudopataID > 0 && SeguirBuscando) {
                    Tipo = "Ludopata";
                    TipoPersona = "PROHIBIDO";
                    NombreCompleto = $"{ludopata.Nombre} {ludopata.ApellidoPaterno} {ludopata.ApellidoMaterno}";
                    DNI = ludopata.DNI;
                    Foto = ludopata.Foto == "" ? Foto : ludopata.Foto;
                    SeguirBuscando = false;
                }
                if(SeguirBuscando) {
                    timador = busquedaBL.GetTimadorJson(buscar);
                    if(timador.TimadorID > 0) {
                        List<int> codsSalas = new List<int> { sala.CodSala };
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

                if(SeguirBuscando) {
                    politico = busquedaBL.GetPoliticoJson(buscar);
                    if(politico.PoliticoID > 0) {
                        Tipo = "Politico";
                        TipoPersona = "ENTIDADPUBLICA";
                        NombreCompleto = $"{politico.Nombres} {politico.Apellidos}";
                        DNI = politico.Dni;
                        Acceso = true;
                        SeguirBuscando = false;
                    }
                }
                if(SeguirBuscando) {
                    personaEntidadPublica = busquedaBL.GetPersonaEntidadPublicaJson(buscar);
                    if(personaEntidadPublica.EntidadPublicaID > 0) {
                        Tipo = "Entidad Publica";
                        TipoPersona = "ENTIDADPUBLICA";
                        NombreCompleto = $"{personaEntidadPublica.Nombres} {personaEntidadPublica.Apellidos}";
                        DNI = personaEntidadPublica.Dni;
                        Acceso = true;
                        SeguirBuscando = false;
                    }
                }

                //Roba Stackers Billetero
                if(SeguirBuscando) {
                    robaStackersBilletero = busquedaBL.GetRobaStackersBilletero(buscar);
                    if(robaStackersBilletero.RobaStackersBilleteroID > 0) {
                        Tipo = "R. Stacker Billetero";
                        TipoPersona = "ROBASTACKER";
                        NombreCompleto = $"{robaStackersBilletero.Nombre} {robaStackersBilletero.ApellidoPaterno} {robaStackersBilletero.ApellidoMaterno}";
                        DNI = robaStackersBilletero.DNI;
                        Foto = robaStackersBilletero.Foto == "" ? Foto : robaStackersBilletero.Foto;
                        SeguirBuscando = false;
                    }
                }
                if(SeguirBuscando) {
                    menorDeEdad = busquedaBL.GetMenorDeEdad(buscar);
                    if(menorDeEdad.IdMenor > 0) {
                        Tipo = "Persona con Observación";
                        TipoPersona = "MENOREDAD";
                        NombreCompleto = $"{menorDeEdad.Nombre} {menorDeEdad.ApellidoPaterno} {menorDeEdad.ApellidoMaterno}";
                        DNI = menorDeEdad.Doi;
                        SeguirBuscando = false;
                    }
                }

                //Consulta a RRHH
                if(SeguirBuscando) {
                    string url = UrlSistemaReclutamiento + "ofisis/PersonaPorDniFechaCeseV2?dni=" + buscar + "&val=true";
                    string json = "";
                    try {
                        using(var client = new HttpClient()) {

                            using(var response = client.GetAsync(url).Result) {
                                if(response.IsSuccessStatusCode) {
                                    json = response.Content.ReadAsStringAsync().Result;
                                    if(json != "{}") {
                                        string foto = string.Empty;
                                        dynamic jsonObj = JsonConvert.DeserializeObject(json);
                                        var item = jsonObj.data;

                                        if(Convert.ToString(item.CO_TRAB) != "") {
                                            Tipo = "Trabajador";
                                            if(item.CESE_ESTADO == 1) {
                                                Acceso = false;
                                                DateTime fechaActual = DateTime.Now;
                                                DateTime fechaCese = Convert.ToDateTime(item.FE_CESE_TRAB);
                                                fechaCese = fechaCese.AddMonths(6);
                                                if(fechaActual > fechaCese) {
                                                    Acceso = true;
                                                    TipoPersona = "CLIENTE";
                                                    Tipo = "Cliente";
                                                } else {
                                                    Acceso = false;
                                                    TipoPersona = "EXTRABAJADOR";
                                                    Tipo = "Ex Trabajador";
                                                }
                                            } else {

                                                Acceso = false;
                                                TipoPersona = Convert.ToString(item.DE_GRUP_OCUP) == "" ? "TRA" : Convert.ToString(item.DE_GRUP_OCUP);
                                            }

                                            NombreCompleto = $"{Convert.ToString(item.NO_TRAB)} {Convert.ToString(item.NO_APEL_PATE)} {Convert.ToString(item.NO_APEL_MATE)}";
                                            DNI = Convert.ToString(item.CO_TRAB);
                                            SeguirBuscando = false;

                                        }
                                    }
                                } else {
                                    //data = null;
                                    Observacion += "No respondió RRHH.";
                                    observacionReporte = "ERROR OFISIS - No se pudo conectar";
                                    /*
                                    dataAdicional = new
                                    {
                                        errorOFISIS = true
                                    };*/
                                }
                            }
                        }
                    } catch(Exception e) {
                        data = null;
                        Observacion += "No respondió RRHH.";
                        observacionReporte = "ERROR OFISIS - No se pudo conectar - " + e.ToString();
                    }
                }
                if(SeguirBuscando) {
                    Acceso = false;
                    TipoPersona = "CLIENTE";
                    string tipoCliente = "Cliente";
                    Tipo = tipoCliente;
                    //Consultar Tabla AST_Cliente
                    var clienteBusquedaFirst = ast_ClienteBL.GetListaClientesxNroDoc(buscar).FirstOrDefault();
                    if(clienteBusquedaFirst != null) {
                        clienteBusqueda = clienteBusquedaFirst;
                        NombreCompleto = $"{clienteBusqueda.NombreCompleto}";
                        DNI = clienteBusqueda.NroDoc;
                    } else//Consulta a la API
                      {
                        string responseMessage = "";

                        var dataClienteAPI = await apireniec(buscar);
                        if(Convert.ToString(dataClienteAPI[0].dni).Trim() != "") {
                            NombreCompleto = Convert.ToString(dataClienteAPI[0].nombrecompleto);
                            DNI = Convert.ToString(dataClienteAPI[0].dni);

                            clienteInsertar.Nombre = Convert.ToString(dataClienteAPI[0].Nombre);
                            clienteInsertar.ApelPat = Convert.ToString(dataClienteAPI[0].ApellidoPaterno);
                            clienteInsertar.ApelMat = Convert.ToString(dataClienteAPI[0].ApellidoMaterno);
                            clienteInsertar.NombreCompleto = Convert.ToString(dataClienteAPI[0].NombreCompleto);
                            clienteInsertar.NroDoc = Convert.ToString(dataClienteAPI[0].DNI);
                            guardarCliente = true;
                            Acceso = true;

                            responseMessage = "Consulta Completada";
                        } else {
                            TipoPersona = "CLIENTE ERROR";
                            observacionReporte = "ERROR API - No se pudo conectar - " + Convert.ToString(dataClienteAPI[0].ErrorMensaje);
                            Observacion += " No respondió API RENIEC.";
                            dataAdicional = new {
                                mensaje = Convert.ToString(dataClienteAPI[0].ErrorMensaje),
                                errorAPI = true
                            };

                            responseMessage = Convert.ToString(dataClienteAPI[0].ErrorMensaje);
                        }

                        // save log for channel app of api reniec
                        Helpers.APIReniecLog(Helpers.CHANNEL_APP, cliente.NombreSala, usuario.UsuarioNombre, cliente.Dni, responseMessage);
                    }
                }
                var codigoConsulta = listaCodigos.Where(x => x.TipoPersona.Trim().ToLower().Equals(TipoPersona.Trim().ToLower())).FirstOrDefault();
                if(codigoConsulta == null) {
                    codigoUsar.Color = "#00FF00";
                    codigoUsar.Alerta = "CLI";
                    codigoUsar.TipoPersona = "CLIENTE";
                } else {
                    codigoUsar = codigoConsulta;
                }

                if(Tipo == "Trabajador") {
                    codigoUsar.Color = "#ff0000";
                    codigoUsar.Alerta = "TRA";
                    codigoUsar.TipoPersona = "TRABAJADOR";
                }

                nombreArchivo = $"{codigoUsar.Alerta.ToUpper().Trim()}-{codigoUsar.CodigoID}.png";
                bool archivoVerificado = VerificarArchivo(Path.Combine(PathPrincipal, nombreArchivo));
                nombreArchivo = archivoVerificado ? nombreArchivo : generarImagen(PathPrincipal, codigoUsar.Alerta.ToUpper().Trim(), codigoUsar.Color.ToLower().Trim(), codigoUsar.CodigoID);
                ImgFondo = UrlImagenes + nombreArchivo;
                Foto = UrlImagenes + "profile/standard/" + Foto;

                data = new {
                    NombreCompleto,
                    DNI,
                    Foto,
                    ImgFondo,
                    Codigo = codigoUsar.Alerta,
                    dataAdicional,
                    Tipo,
                    Acceso,
                    Observacion
                };
                //Metodo para insertar auditoria log
                cliente.Cliente = NombreCompleto;
                cliente.TipoCliente = Tipo;
                cliente.FechaRegistro = DateTime.Now;
                cliente.codigo = codigoUsar.Alerta;
                cliente.observacion = observacionReporte;
                var insertado = auditoriaBL.RegistrarBusquedaExterno(cliente);
                if(guardarCliente) {
                    clienteInsertar.FechaRegistro = DateTime.Now;
                    clienteInsertar.Estado = "A";
                    clienteInsertar.TipoRegistro = "SYSLUDOPATAS";
                    clienteInsertar.usuario_reg = usuario.UsuarioID;
                    clienteInsertar.SalaId = sala.CodSala;
                    GuardarClienteLudoBusqueda(clienteInsertar);
                }
                return Json(new { data }, JsonRequestBehavior.AllowGet);
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                data = null;
                return Json(new { mensaje = "Error al conectarse al Sistema Ludopatas", data }, JsonRequestBehavior.AllowGet);
            }
        }
        [seguridad(false)]
        private async Task<dynamic> apireniec(string dni) {
            var rpta = false;
            ApiReniec _apiReniec = new ApiReniec();
            //string json = "";
            dynamic item = new DynamicDictionary();
            List<dynamic> Lista = new List<dynamic>();
            item.ErrorMensaje = string.Empty;
            try {
                if(Helpers.IsValidDNI(dni)) {
                    var itemResponse = await _apiReniec.Busqueda(dni);
                    item.NombreCompleto = itemResponse.NombreCompleto;
                    item.DNI = itemResponse.DNI;
                    item.Nombre = itemResponse.Nombre;
                    item.ApellidoPaterno = itemResponse.ApellidoPaterno;
                    item.ApellidoMaterno = itemResponse.ApellidoMaterno;
                    item.ErrorMensaje = itemResponse.ErrorMensaje;
                    rpta = itemResponse.Respuesta;

                } else {
                    item.NombreCompleto = "Cliente No Encontrado";
                    item.DNI = "";
                    item.Nombre = "";
                    item.ApellidoPaterno = "";
                    item.ApellidoMaterno = "";
                    item.ErrorMensaje = "El número de documento es invalido";
                }
            } catch(Exception e) {
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
        [HttpPost]
        public async Task<ActionResult> BuscarPersonaSinSeguridadJson(string buscar, int codsala) {
            CAL_AuditoriaEntidad cliente = new CAL_AuditoriaEntidad();
            bool respuesta = true;
            string UrlImagenes = string.Empty;
            object data = new object();
            object dataAdicional = new object();
            int CodTipoDOI = 1;
            string DNI = string.Empty;
            string Nombre = string.Empty;
            string ApellidoPaterno = string.Empty;
            string ApellidoMaterno = string.Empty;
            string NombreCompleto = string.Empty;
            string Genero = string.Empty;
            string Movil = string.Empty;
            string Telefono = string.Empty;
            int Ubigeo = 1874;
            int CodNacionalidad = 145;
            string FechaNacimiento = "";
            string Direccion = string.Empty;
            string CorreoPersonal = string.Empty;
            string CorreoTrabajo = string.Empty;
            CAL_LudopataEntidad ludopata;
            CAL_PersonaProhibidoIngresoEntidad timador;
            CAL_PoliticoEntidad politico;
            CAL_PersonaEntidadPublicaEntidad personaEntidadPublica;
            CAL_RobaStackersBilleteroEntidad robaStackersBilletero;
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
            string Observacion = "";
            string observacionReporte = "";
            CAL_MenorDeEdadEntidad menorDeEdad = new CAL_MenorDeEdadEntidad();
            try {
                string soloNumeros = @"^[0-9]+$";
                if(!System.Text.RegularExpressions.Regex.IsMatch(buscar, soloNumeros) || buscar == string.Empty || buscar.Length < 8) {
                    data = null;
                    return Json(new { mensaje = "Formato de Documento Incorrecto", data, respuesta = false }, JsonRequestBehavior.AllowGet);
                }
                DNI = buscar;
                UrlSistemaReclutamiento = ConfigurationManager.AppSettings["UriSistemaReclutamiento"].ToString();
                UrlImagenes = ConfigurationManager.AppSettings["UriImagenesLudopatas"].ToString();
                PathPrincipal = Path.Combine(ConfigurationManager.AppSettings["PathArchivos"].ToString(), "Ludopatas");
                //SEG_UsuarioEntidad usuario = usuarioBL.UsuarioEmpleadoIDObtenerJson(usuarioid);
                sala = salaBL.SalaListaIdJson(codsala);
                cliente.Dni = buscar;
                cliente.NombreUsuario = "ERP - ProgramacionSorteo ";
                cliente.NombreSala = sala.Nombre;
                listaCodigos = codigoBL.GetAllCodigoJoinCodigoPersona();
                string dni = buscar;
                ludopata = busquedaBL.GetLudopataJson(buscar);
                if(ludopata.LudopataID > 0 && SeguirBuscando) {
                    Tipo = "Ludopata";
                    TipoPersona = "PROHIBIDO";
                    NombreCompleto = $"{ludopata.Nombre} {ludopata.ApellidoPaterno} {ludopata.ApellidoMaterno}";
                    DNI = ludopata.DNI;
                    SeguirBuscando = false;
                }
                if(SeguirBuscando) {
                    timador = busquedaBL.GetTimadorJson(buscar);
                    if(timador.TimadorID > 0) {
                        List<int> codsSalas = salaBL.ObtenerCodsSalasDeSesion(Session);
                        List<CAL_PersonaProhibidoIngresoIncidenciaEntidad> incidenciaBusqueda = timadorIncidenciaBL.GetAllTimadorIncidenciaxTimadorActivo(timador.TimadorID, codsSalas);
                        bool esProhibido = timador.Prohibir == 1 || incidenciaBusqueda.Count > 0;

                        Tipo = esProhibido ? "Prohibido de Ingreso" : timador.ConAtenuante ? "Cliente Alerta" : "Problematico";
                        TipoPersona = esProhibido ? "PROHIBIDO" : timador.ConAtenuante ? "CLIENTE ALERTA" : "PROBLEMATICO";

                        NombreCompleto = $"{timador.Nombre} {timador.ApellidoPaterno} {timador.ApellidoMaterno}";
                        DNI = timador.DNI;
                        SeguirBuscando = false;
                    }
                }

                if(SeguirBuscando) {
                    politico = busquedaBL.GetPoliticoJson(buscar);
                    if(politico.PoliticoID > 0) {
                        Tipo = "Politico";
                        TipoPersona = "ENTIDADPUBLICA";
                        NombreCompleto = $"{politico.Nombres} {politico.Apellidos}";
                        DNI = politico.Dni;
                        Acceso = true;
                        SeguirBuscando = false;
                    }
                }
                if(SeguirBuscando) {
                    personaEntidadPublica = busquedaBL.GetPersonaEntidadPublicaJson(buscar);
                    if(personaEntidadPublica.EntidadPublicaID > 0) {
                        Tipo = "Entidad Publica";
                        TipoPersona = "ENTIDADPUBLICA";
                        NombreCompleto = $"{personaEntidadPublica.Nombres} {personaEntidadPublica.Apellidos}";
                        DNI = personaEntidadPublica.Dni;
                        Acceso = true;
                        SeguirBuscando = false;
                    }
                }

                //Roba Stackers Billetero
                if(SeguirBuscando) {
                    robaStackersBilletero = busquedaBL.GetRobaStackersBilletero(buscar);
                    if(robaStackersBilletero.RobaStackersBilleteroID > 0) {
                        Tipo = "R. Stacker Billetero";
                        TipoPersona = "ROBASTACKER";
                        NombreCompleto = $"{robaStackersBilletero.Nombre} {robaStackersBilletero.ApellidoPaterno} {robaStackersBilletero.ApellidoMaterno}";
                        DNI = robaStackersBilletero.DNI;
                        SeguirBuscando = false;
                    }
                }
                if(SeguirBuscando) {
                    menorDeEdad = busquedaBL.GetMenorDeEdad(buscar);
                    if(menorDeEdad.IdMenor > 0) {
                        Tipo = "Persona con Observación";
                        TipoPersona = "MENOREDAD";
                        NombreCompleto = $"{menorDeEdad.Nombre} {menorDeEdad.ApellidoPaterno} {menorDeEdad.ApellidoMaterno}";
                        DNI = menorDeEdad.Doi;
                        SeguirBuscando = false;
                    }
                }

                //Consulta a RRHH
                if(SeguirBuscando) {
                    string url = UrlSistemaReclutamiento + "ofisis/PersonaPorDniFechaCeseV2?dni=" + buscar + "&val=true";
                    string json = "";
                    try {
                        using(var client = new HttpClient()) {

                            using(var response = client.GetAsync(url).Result) {
                                if(response.IsSuccessStatusCode) {
                                    json = response.Content.ReadAsStringAsync().Result;
                                    if(json != "{}") {
                                        dynamic jsonObj = JsonConvert.DeserializeObject(json);
                                        var item = jsonObj.data;

                                        if(Convert.ToString(item.CO_TRAB) != "") {
                                            Tipo = "Trabajador";
                                            if(item.CESE_ESTADO == 1) {
                                                Acceso = false;
                                                DateTime fechaActual = DateTime.Now;
                                                DateTime fechaCese = Convert.ToDateTime(item.FE_CESE_TRAB);
                                                fechaCese = fechaCese.AddMonths(6);
                                                if(fechaActual > fechaCese) {
                                                    Acceso = true;
                                                    TipoPersona = "CLIENTE";
                                                    Tipo = "Cliente";
                                                } else {
                                                    Acceso = false;
                                                    TipoPersona = "EXTRABAJADOR";
                                                    Tipo = "Ex Trabajador";
                                                }
                                            } else {

                                                Acceso = false;
                                                TipoPersona = Convert.ToString(item.DE_GRUP_OCUP) == "" ? "TRA" : Convert.ToString(item.DE_GRUP_OCUP);
                                            }

                                            NombreCompleto = $"{Convert.ToString(item.NO_TRAB)} {Convert.ToString(item.NO_APEL_PATE)} {Convert.ToString(item.NO_APEL_MATE)}";
                                            DNI = Convert.ToString(item.CO_TRAB);
                                            SeguirBuscando = false;
                                        }
                                    }
                                } else {
                                    //data = null;
                                    Observacion += "No respondió RRHH.";
                                    observacionReporte = "ERROR OFISIS - No se pudo conectar";
                                    /*
                                    dataAdicional = new
                                    {
                                        errorOFISIS = true
                                    };*/
                                }
                            }
                        }
                    } catch(Exception e) {
                        data = null;
                        Observacion += "No respondió RRHH.";
                        observacionReporte = "ERROR OFISIS - No se pudo conectar - " + e.ToString();
                    }
                }
                if(SeguirBuscando) {
                    Acceso = true;
                    TipoPersona = "CLIENTE";
                    string tipoCliente = "Cliente";
                    Tipo = tipoCliente;
                    //Consultar Tabla AST_Cliente
                    var clienteBusquedaFirst = ast_ClienteBL.GetListaClientesxNroDoc(buscar).FirstOrDefault();
                    if(clienteBusquedaFirst != null) {
                        clienteBusqueda = clienteBusquedaFirst;
                        NombreCompleto = $"{clienteBusqueda.NombreCompleto}";
                        DNI = clienteBusqueda.NroDoc;
                        Nombre = clienteBusqueda.Nombre;
                        ApellidoPaterno = clienteBusqueda.ApelPat;
                        ApellidoMaterno = clienteBusqueda.ApelMat;
                        NombreCompleto = clienteBusqueda.NombreCompleto;
                        Genero = clienteBusqueda.Genero;
                        Movil = clienteBusqueda.Celular1;
                        Telefono = clienteBusqueda.Celular2;
                        Ubigeo = clienteBusqueda.UbigeoProcedenciaId == 0 ? 1874 : clienteBusqueda.UbigeoProcedenciaId;
                        FechaNacimiento = (clienteBusqueda.FechaNacimiento.ToShortDateString() == "1/01/1753") ? "" : clienteBusqueda.FechaNacimiento.ToString("yyyy/MM/dd");
                        CorreoPersonal = clienteBusqueda.Mail;
                        CorreoTrabajo = clienteBusqueda.Mail;
                    } else { //Consulta a la API
                        string responseMessage = "";

                        var dataClienteAPI = await apireniec(buscar);
                        if(Convert.ToString(dataClienteAPI[0].dni).Trim() != "") {
                            Nombre = Convert.ToString(dataClienteAPI[0].Nombre);
                            ApellidoPaterno = Convert.ToString(dataClienteAPI[0].ApellidoPaterno);
                            ApellidoMaterno = Convert.ToString(dataClienteAPI[0].ApellidoMaterno);
                            NombreCompleto = Convert.ToString(dataClienteAPI[0].nombrecompleto);
                            DNI = Convert.ToString(dataClienteAPI[0].dni);

                            clienteInsertar.Nombre = Convert.ToString(dataClienteAPI[0].Nombre);
                            clienteInsertar.ApelPat = Convert.ToString(dataClienteAPI[0].ApellidoPaterno);
                            clienteInsertar.ApelMat = Convert.ToString(dataClienteAPI[0].ApellidoMaterno);
                            clienteInsertar.NombreCompleto = Convert.ToString(dataClienteAPI[0].NombreCompleto);
                            clienteInsertar.NroDoc = Convert.ToString(dataClienteAPI[0].DNI);
                            guardarCliente = true;
                            Acceso = true;

                            responseMessage = "Consulta Completada";
                        } else {
                            observacionReporte = "ERROR API - No se pudo conectar - " + Convert.ToString(dataClienteAPI[0].ErrorMensaje);
                            Observacion += " No respondió API RENIEC.";
                            dataAdicional = new {
                                mensaje = Convert.ToString(dataClienteAPI[0].ErrorMensaje),
                                errorAPI = true
                            };

                            responseMessage = Convert.ToString(dataClienteAPI[0].ErrorMensaje);
                            respuesta = false;
                        }

                        // save log for channel app of api reniec
                        Helpers.APIReniecLog(Helpers.CHANNEL_APP, cliente.NombreSala, "ERP - ProgramacionSorteo ", cliente.Dni, responseMessage);
                    }
                }
                var codigoConsulta = listaCodigos.Where(x => x.TipoPersona.Trim().ToLower().Equals(TipoPersona.Trim().ToLower())).FirstOrDefault();
                if(codigoConsulta == null) {
                    codigoUsar.Color = "#00FF00";
                    codigoUsar.Alerta = "CLI";
                    codigoUsar.TipoPersona = "CLIENTE";
                } else {
                    codigoUsar = codigoConsulta;
                }

                if(Tipo == "Trabajador") {
                    codigoUsar.Color = "#ff0000";
                    codigoUsar.Alerta = "TRA";
                    codigoUsar.TipoPersona = "TRABAJADOR";
                }

                data = new {
                    Nombre,
                    ApellidoPaterno,
                    ApellidoMaterno,
                    NombreCompleto,
                    CodTipoDOI,
                    DOI = DNI,
                    Genero,
                    Movil,
                    Telefono,
                    CodNacionalidad,
                    CodUbigeo = Ubigeo,
                    FechaNacimiento,
                    Direccion,
                    MailPersonal = CorreoPersonal,
                    MailJob = CorreoTrabajo,
                    Codigo = codigoUsar.Alerta,
                    dataAdicional,
                    Tipo,
                    Observacion
                };
                //Metodo para insertar auditoria log
                cliente.Cliente = NombreCompleto;
                cliente.TipoCliente = Tipo;
                cliente.FechaRegistro = DateTime.Now;
                cliente.codigo = codigoUsar.Alerta;
                cliente.observacion = observacionReporte;
                var insertado = auditoriaBL.RegistrarBusquedaExterno(cliente);
                if(guardarCliente) {
                    clienteInsertar.TipoDocumentoId = 1;
                    clienteInsertar.UbigeoProcedenciaId = Ubigeo;
                    clienteInsertar.FechaRegistro = DateTime.Now;
                    clienteInsertar.Estado = "A";
                    clienteInsertar.TipoRegistro = "SORTEOSERP";
                    clienteInsertar.usuario_reg = 0;
                    clienteInsertar.SalaId = sala.CodSala;
                    GuardarClienteLudoBusqueda(clienteInsertar);
                }
                return Json(new { data, respuesta, acceso = Acceso }, JsonRequestBehavior.AllowGet);
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                data = null;
                return Json(new { mensaje = "Error al conectarse al Sistema Ludopatas", data, acceso = Acceso, respuesta }, JsonRequestBehavior.AllowGet);
            }
        }


        [seguridad(false)]
        [HttpPost]
        public async Task<ActionResult> BuscarPersonaSinSeguridadCortesiasJson(string buscar) {

            List<AST_ClienteEntidad> listaClientes = new List<AST_ClienteEntidad>();

            try {

                //Consultar Tabla AST_Cliente
                var clientesEncontrados = ast_ClienteBL.GetListaClientesxNroDocMetodoBusqueda(buscar);
                if(clientesEncontrados.Count > 0) {
                    listaClientes.AddRange(clientesEncontrados);
                } else {
                    var clientesEncontrados2 = ast_ClienteBL.GetListaClientesxNombreMetodoBusqueda(buscar);
                    if(clientesEncontrados2.Count > 0) {
                        listaClientes.AddRange(clientesEncontrados2);
                    } else {
                        var dataClienteAPI = await apireniec(buscar);
                        if(Convert.ToString(dataClienteAPI[0].dni).Trim() != "") {

                            AST_ClienteEntidad clienteAPI = new AST_ClienteEntidad();

                            clienteAPI.Nombre = Convert.ToString(dataClienteAPI[0].Nombre);
                            clienteAPI.ApelPat = Convert.ToString(dataClienteAPI[0].ApellidoPaterno);
                            clienteAPI.ApelMat = Convert.ToString(dataClienteAPI[0].ApellidoMaterno);
                            clienteAPI.NombreCompleto = Convert.ToString(dataClienteAPI[0].NombreCompleto);
                            clienteAPI.NroDoc = Convert.ToString(dataClienteAPI[0].DNI);

                            listaClientes.Add(clienteAPI);
                        }
                    }
                }
            } catch(Exception ex) {
                listaClientes = new List<AST_ClienteEntidad>();
            }

            return Json(new { mensaje = "Clientes encontrados", data = listaClientes, success = true });

        }
    }
}