using CapaEntidad;
using CapaEntidad.AsistenciaCliente;
using CapaEntidad.AsistenciaCliente.DataWarehouse;
using CapaEntidad.ClienteSala;
using CapaEntidad.ControlAcceso.HistorialLudopata.Dto;
using CapaEntidad.Excel;
using CapaEntidad.Response;
using CapaNegocio;
using CapaNegocio.AsistenciaCliente;
using CapaNegocio.ControlAcceso;
using CapaNegocio.Excel;
using CapaPresentacion.Controllers.AsistenciaCliente;
using CapaPresentacion.Models;
using CapaPresentacion.Utilitarios;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using S3k.Utilitario;
using S3k.Utilitario.Constants;
using S3k.Utilitario.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CapaPresentacion.Controllers {
    [seguridad]
    public class AsistenciaClienteController : BaseController {
        #region Variables Globales
        private SalaBL salaBl = new SalaBL();
        private AST_ClienteBL ast_ClienteBL = new AST_ClienteBL();
        private AST_TipoDocumentoBL ast_TipoDocumentoBL = new AST_TipoDocumentoBL();
        private UbigeoBL ubigeoBL = new UbigeoBL();
        private AST_TipoClienteBL ast_TipoClienteBL = new AST_TipoClienteBL();
        private AST_TipoJuegoBL ast_TipoJuegoBL = new AST_TipoJuegoBL();
        private AST_TipoFrecuenciaBL ast_TipoFrecuenciaBL = new AST_TipoFrecuenciaBL();
        private SalaBL salaBL = new SalaBL();
        private AST_AsistenciaClienteSalaBL ast_asistenciaClienteSalaBL = new AST_AsistenciaClienteSalaBL();
        private AST_ClienteSalaBL ast_clienteSalaBL = new AST_ClienteSalaBL();
        private SEG_PermisoRolBL seg_PermisoRolBL = new SEG_PermisoRolBL();
        private readonly SalaMaestraBL salaMaestraBL;
        private readonly CAL_HistorialLudopataBL historialLudopataBL;
        private readonly PermisosClienteController permisosClienteController;
        private int CodigoSalaSomosCasino = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoSalaSomosCasino"]);

        public AsistenciaClienteController() {
            salaMaestraBL = new SalaMaestraBL();
            historialLudopataBL = new CAL_HistorialLudopataBL();
            permisosClienteController = new PermisosClienteController();
        }

        #endregion
        // GET: AsistenciaCliente
        #region Vistas
        public ActionResult RegistroAsistencia() {
            return View("~/Views/AsistenciaCliente/RegistroAsistencia.cshtml");
        }
        public ActionResult RegistroCliente(bool redirect = false, string nombre = "", string apelpat = "", string apelmat = "", string nrodoc = "") {
            ViewBag.redirect = redirect;
            ViewBag.nombre = nombre;
            ViewBag.apelpat = apelpat;
            ViewBag.apelmat = apelmat;
            ViewBag.nrodoc = nrodoc;
            ViewBag.tipodoc = nrodoc != string.Empty ? 1 : 0;
            return View("~/Views/AsistenciaCliente/RegistroCliente.cshtml");
        }
        public ActionResult ListadoCliente() {

            string accion = "PermisoBuscarCliente";
            bool buscar = false;
            var permiso = seg_PermisoRolBL.GetPermisoRolUsuario((int)Session["rol"], accion);
            if(permiso.Count > 0) {
                buscar = true;
            }
            ViewBag.buscar = buscar;
            return View("~/Views/AsistenciaCliente/ListadoCliente.cshtml");
        }
        public ActionResult EditarCliente(int id = 0) {
            string mensaje = "";
            AST_ClienteEntidad cliente = new AST_ClienteEntidad();
            AST_ClienteSalaEntidad clienteSala = new AST_ClienteSalaEntidad();
            int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            UbigeoEntidad ubigeo = new UbigeoEntidad();
            SalaEntidad sala = new SalaEntidad();
            bool buscar = false;
            try {
                sala = salaBl.ListadoSalaPorUsuario(usuarioId).FirstOrDefault();
                cliente = ast_ClienteBL.GetClienteID(id);
                clienteSala = ast_clienteSalaBL.GetClienteSalaID(cliente.Id, sala.CodSala);
                cliente.ClienteSala = clienteSala;
                ubigeo = ubigeoBL.GetDatosUbigeo(cliente.UbigeoProcedenciaId);
                string accion = "PermisoBuscarCliente";
                var permiso = seg_PermisoRolBL.GetPermisoRolUsuario((int)Session["rol"], accion);
                if(permiso.Count > 0) {
                    buscar = true;
                }

            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            ViewBag.cliente = cliente;
            ViewBag.mensaje = mensaje;
            ViewBag.ubigeo = ubigeo;
            ViewBag.buscar = buscar;
            return View("~/Views/AsistenciaCliente/EditarCliente.cshtml");
        }

        public ActionResult AsistenciaClienteSala() {
            return View();
        }
        public ActionResult ReporteAsistenciaSala() {
            return View("~/Views/AsistenciaCliente/ReporteAsistenciaSala.cshtml");
        }

        public ActionResult ReporteListaCliente() {
            return View("~/Views/AsistenciaCliente/ReporteListaClienteVista.cshtml");
        }

        [seguridad(false)]
        public ActionResult AsistenciaHora() {
            return View("~/Views/AsistenciaCliente/RegistroAsistenciaHora.cshtml");
        }
        public ActionResult ListadoTipoCliente() {
            return View("~/Views/AsistenciaCliente/ListadoTipoCliente.cshtml");
        }
        public ActionResult ListadoTipoJuego() {
            return View("~/Views/AsistenciaCliente/ListadoTipoJuego.cshtml");
        }
        public ActionResult RegistroTipoCliente() {
            return View("~/Views/AsistenciaCliente/RegistroTipoCliente.cshtml");
        }
        public ActionResult EditarTipoCliente(int id = 0) {
            string mensaje = "";
            AST_TipoClienteEntidad tipoCliente = new AST_TipoClienteEntidad();
            try {
                tipoCliente = ast_TipoClienteBL.GetTipoClienteID(id);
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            ViewBag.tipoCliente = tipoCliente;
            ViewBag.mensaje = mensaje;
            return View("~/Views/AsistenciaCliente/EditarTipoCliente.cshtml");
        }

        public ActionResult RegistroTipoJuego() {
            return View("~/Views/AsistenciaCliente/RegistroTipoJuego.cshtml");
        }
        public ActionResult EditarTipoJuego(int id = 0) {
            string mensaje = "";
            AST_TipoJuegoEntidad tipoJuego = new AST_TipoJuegoEntidad();
            try {
                tipoJuego = ast_TipoJuegoBL.GetTipoJuegoID(id);
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            ViewBag.tipoJuego = tipoJuego;
            ViewBag.mensaje = mensaje;
            return View("~/Views/AsistenciaCliente/EditarTipoJuego.cshtml");
        }
        public ActionResult ListadoTipoFrecuencia() {
            return View("~/Views/AsistenciaCliente/ListadoTipoFrecuencia.cshtml");
        }
        public ActionResult RegistroTipoFrecuencia() {
            return View("~/Views/AsistenciaCliente/RegistroTipoFrecuencia.cshtml");
        }
        public ActionResult EditarTipoFrecuencia(int id = 0) {
            string mensaje = "";
            AST_TipoFrecuenciaEntidad tipoFrecuencia = new AST_TipoFrecuenciaEntidad();
            try {
                tipoFrecuencia = ast_TipoFrecuenciaBL.GetTipoFrecuenciaID(id);
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            ViewBag.tipoFrecuencia = tipoFrecuencia;
            ViewBag.mensaje = mensaje;
            return View("~/Views/AsistenciaCliente/EditarTipoFrecuencia.cshtml");
        }
        public ActionResult SincronizarExcelClientes() {
            return View("~/Views/AsistenciaCliente/SincronizarExcelClientes.cshtml");
        }
        public ActionResult EnvioNotificacion() {
            return View("~/Views/AsistenciaCliente/EnvioNotificacion.cshtml");
        }

        #endregion
        #region Ubigeo
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoDepartamento() {
            string mensaje = "";
            bool respuesta = false;
            List<UbigeoEntidad> lista = new List<UbigeoEntidad>();
            try {
                lista = ubigeoBL.ListadoDepartamento();
                mensaje = "Listando Registros";
                respuesta = true;
            } catch(Exception ex) {
                mensaje = ex.Message;
                respuesta = false;
            }
            return Json(new { mensaje, respuesta, data = lista });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoProvincia(int DepartamentoID) {
            string mensaje = "";
            bool respuesta = false;
            List<UbigeoEntidad> lista = new List<UbigeoEntidad>();
            try {
                lista = ubigeoBL.GetListadoProvincia(DepartamentoID);
                mensaje = "Listando Registros";
                respuesta = true;
            } catch(Exception ex) {
                mensaje = ex.Message;
                respuesta = false;
            }
            return Json(new { mensaje, respuesta, data = lista });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoDistrito(int ProvinciaID, int DepartamentoID) {
            string mensaje = "";
            bool respuesta = false;
            List<UbigeoEntidad> lista = new List<UbigeoEntidad>();
            try {
                lista = ubigeoBL.GetListadoDistrito(ProvinciaID, DepartamentoID);
                mensaje = "Listando Registros";
                respuesta = true;
            } catch(Exception ex) {
                mensaje = ex.Message;
                respuesta = false;
            }
            return Json(new { mensaje, respuesta, data = lista });
        }

        #endregion

        [seguridad(false)]
        [HttpPost]
        public async Task<ActionResult> GetListadoClienteCoincidencia(string coincidencia) {
            string mensaje = "";
            bool respuesta = false;
            bool api = false;
            List<AST_ClienteEntidad> lista = new List<AST_ClienteEntidad>();
            AST_ClienteEntidad cliente = new AST_ClienteEntidad();
            try {
                Regex regexNroDocumento = new Regex(@"^[0-9]+$");
                if(!regexNroDocumento.IsMatch(coincidencia)) {
                    mensaje = "Listando Registros";
                    respuesta = true;
                    //nombres y apellidos
                    lista = ast_ClienteBL.GetListadoClientePorNombresyApellidos(coincidencia);
                    return Json(new { mensaje, respuesta, data = lista });
                } else if(coincidencia.Length == 8) {
                    //busqueda por dni
                    int TipoDocumentoId = 1;
                    //cliente=ast_ClienteBL.GetClientexNroyTipoDoc(TipoDocumentoId, coincidencia);
                    cliente = ast_ClienteBL.GetClientexNroDoc(coincidencia);

                    if(cliente.Id != 0) {
                        mensaje = "Listando Registros";
                        respuesta = true;
                        lista.Add(cliente);
                        return Json(new { mensaje, respuesta, data = lista });
                    } else {
                        mensaje = "Listando Registros";
                        respuesta = true;
                        //API
                        cliente = await API_RENIEC(coincidencia);
                        api = true;
                        lista.Add(cliente);
                        return Json(new { mensaje, respuesta, data = lista, api });
                    }
                } else {
                    //Nro documento cualquiera
                    mensaje = "Listando Registros";
                    respuesta = true;
                    lista = ast_ClienteBL.GetListaClientesxNroDoc(coincidencia);
                    return Json(new { mensaje, respuesta, data = lista });
                }

            } catch(Exception ex) {
                mensaje = ex.Message;
                respuesta = false;
                return Json(new { mensaje, respuesta, data = lista });
            }
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoCliente(DtParameters dtParameters) {
            string mensaje = "";
            bool respuesta = false;
            List<AST_ClienteEntidad> lista = new List<AST_ClienteEntidad>();
            object oRespuesta = new object();

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            var count = 0;
            List<dynamic> registro = new List<dynamic>();
            try {
                lista = ast_ClienteBL.GetListadoCliente();
                String busqueda = "";
                bool respuestaController = false;
                string accion = "CambiarEstadoCliente";
                busqueda = funciones.consulta("PermisoUsuario", @"
                                                                SELECT [WEB_PRolID],[WEB_RolID],[WEB_PRolFechaRegistro]
                                                                FROM [dbo].[SEG_PermisoRol] 
                                                                left join [SEG_Permiso] on [SEG_Permiso].[WEB_PermID]=[SEG_PermisoRol].[WEB_PermID]
                                                                where [SEG_PermisoRol].WEB_RolID =" + (int)Session["rol"] +
                                                                                        " and [SEG_Permiso].[WEB_PermNombre]='" + accion + "'"

                                                                         );
                if(busqueda.Length < 3) {
                    respuestaController = false;
                } else {
                    respuestaController = true;
                }
                DateTime fechaHoy = DateTime.Today;
                foreach(var cliente in lista) {
                    int edad = 0;
                    edad = fechaHoy.Year - cliente.FechaNacimiento.Year;
                    cliente.Edad = (cliente.FechaNacimiento.Month > fechaHoy.Month) ? --edad : edad;
                }

                if(dtParameters.Order != null) {
                    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
                } else {
                    orderCriteria = "Id";
                    orderAscendingDirection = false;
                }
                count = lista.Count();
                if(!string.IsNullOrEmpty(searchBy) && !(string.IsNullOrWhiteSpace(searchBy))) {
                    lista = lista.Where(x => x.ApelMat.ToLower().Contains(searchBy.ToLower())
                                                  || x.ApelPat.ToLower().Contains(searchBy.ToLower())
                                                  || x.Nombre.ToLower().Contains(searchBy.ToLower())
                                                  || x.NombreCompleto.ToLower().Contains(searchBy.ToLower())
                                                   || x.TipoDocumento.Nombre.ToLower().Contains(searchBy.ToLower())
                                                  || x.NroDoc.Contains(searchBy.ToLower())
                                                  || x.Edad.ToString() == searchBy
                                                  || ((x.Genero.ToLower() == "f") ? "FEMENINO" : "MASCULINO").ToLower().Contains(searchBy.ToLower())
                                                  || x.Celular1.Contains(searchBy.ToLower())
                                                  || x.Mail.ToLower().Contains(searchBy.ToLower())
                                                   || x.FechaNacimiento.ToString("dd/MM/yyyy").Contains(searchBy.ToLower())
                                                   || ((x.AsistioDespuesCuarentena.ToString() == "1") ? "SI" : "NO").Contains(searchBy.ToLower())
                                                    || ((x.Estado == "A") ? "ACTIVO" : "PENDIENTE").ToLower().Contains(searchBy.ToLower())
                                                  ).ToList();
                }

                lista = orderAscendingDirection ? lista.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc).ToList() : lista.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc).ToList();

                var filteredResultsCount = lista.Count();
                var totalResultsCount = count;

                registro.Add(new {
                    mensaje = "Listando Registros",
                    dataPermiso = respuestaController,
                    draw = dtParameters.Draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = lista.Skip(dtParameters.Start)
                    .Take(dtParameters.Length).ToList()
                });

            } catch(Exception ex) {
                mensaje = ex.Message;
            }

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var result = new ContentResult {
                Content = serializer.Serialize(registro.FirstOrDefault()),
                ContentType = "application/json"
            };
            return result;

            //return Json(new { mensaje, respuesta, data = oRespuesta });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoClienteJson(DtParameters dtParameters) {
            List<dynamic> records = new List<dynamic>();

            try {
                bool respuestaController = false;
                string searching = "";
                string accion = "CambiarEstadoCliente";

                searching = funciones.consulta("PermisoUsuario",
                    @"
                    SELECT [WEB_PRolID],[WEB_RolID],[WEB_PRolFechaRegistro] FROM [dbo].[SEG_PermisoRol] 
                    LEFT JOIN [SEG_Permiso] ON [SEG_Permiso].[WEB_PermID]=[SEG_PermisoRol].[WEB_PermID]
                    WHERE [SEG_PermisoRol].WEB_RolID = " + (int)Session["rol"] + " AND [SEG_Permiso].[WEB_PermNombre] = '" + accion + "'");

                if(searching.Length > 2) {
                    respuestaController = true;
                }

                // dtParameters
                int skip = dtParameters.Start;
                int pageSize = dtParameters.Length;
                string searchBy = dtParameters.Search?.Value;
                string criterionOrder = "cliente.Id";
                string criterionDirection = "DESC";

                if(dtParameters.Order != null) {
                    criterionOrder = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                    criterionDirection = dtParameters.Order[0].Dir.ToString().ToUpper();
                }

                // Filters
                string whereQuery = "";
                string queryOrderBy = $@" ORDER BY {criterionOrder} {criterionDirection}";
                string queryOffset = $@" OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY";

                if(!string.IsNullOrEmpty(searchBy) && !(string.IsNullOrWhiteSpace(searchBy))) {
                    string[] columns = {
                        "LTRIM(RTRIM(cliente.Nombre))",
                        "LTRIM(RTRIM(cliente.ApelPat))",
                        "LTRIM(RTRIM(cliente.ApelMat))",
                        "LTRIM(RTRIM(cliente.NombreCompleto))",
                        "tipodocumento.Nombre",
                        "cliente.NroDoc",
                        "FORMAT(cliente.[FechaNacimiento], 'dd/MM/yyyy')",
                        "CONVERT(Integer, DATEDIFF(YEAR, cliente.[FechaNacimiento], GETDATE()))",
                        "(CASE WHEN cliente.[Genero] = 'M' THEN 'Masculino' WHEN cliente.[Genero] = 'F' THEN 'Femenino' END)",
                        "cliente.Celular1",
                        "cliente.Mail",
                        "(CASE WHEN cliente.[AsistioDespuesCuarentena] = 1 THEN 'SI' WHEN cliente.[AsistioDespuesCuarentena] = 0 THEN 'NO' END)",
                        "(CASE WHEN cliente.[Estado] = 'A' THEN 'Activo' WHEN cliente.[Estado] = 'P' THEN 'Pendiente' END)"
                    };

                    List<string> listWhere = new List<string>();

                    foreach(string column in columns) {
                        listWhere.Add($@"({column} LIKE '%{searchBy}%')");
                    }

                    whereQuery += $@" WHERE ( {string.Join(" OR ", listWhere)} )";
                }

                // Records
                int recordsFiltered = 0;
                int recordsTotal = 0;

                recordsTotal = ast_ClienteBL.GetListadoClienteFiltradosTotal(string.Empty);
                recordsFiltered = ast_ClienteBL.GetListadoClienteFiltradosTotal(whereQuery);
                List<AST_ClienteEntidad> customerList = ast_ClienteBL.GetListadoClienteFiltrados(whereQuery + queryOrderBy + queryOffset);

                // Filtereds
                records.Add(new {
                    mensaje = "Listando Registros",
                    dataPermiso = respuestaController,
                    draw = dtParameters.Draw,
                    recordsTotal = recordsTotal,
                    recordsFiltered = recordsFiltered,
                    data = customerList
                });

            } catch(Exception exception) {
                Console.WriteLine(exception.Message);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;

            ContentResult result = new ContentResult {
                Content = serializer.Serialize(records.FirstOrDefault()),
                ContentType = "application/json"
            };

            return result;
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoTipoDocumento() {
            string mensaje = "";
            bool respuesta = false;
            List<AST_TipoDocumentoEntidad> lista = new List<AST_TipoDocumentoEntidad>();
            try {
                lista = ast_TipoDocumentoBL.GetListadoTipoDocumento();
                mensaje = "Listando Registros";
                respuesta = true;
            } catch(Exception ex) {
                mensaje = ex.Message;
                respuesta = false;
            }
            return Json(new { mensaje, respuesta, data = lista });
        }

        [HttpPost]
        public ActionResult GuardarCliente(AST_ClienteEntidad cliente) {
            string mensaje = "";
            bool respuesta = false;
            int idInsertado = 0;
            int usuarioId = Convert.ToInt32(Session["UsuarioID"]);

            try {
                if(!cliente.EsMayorDeEdad()) {
                    respuesta = false;
                    mensaje = "Solo se pueden registrar clientes mayores de edad.";
                    return Json(new { respuesta, mensaje });
                }

                List<SalaEntidad> listaSalas = salaBL.ListadoSalaPorUsuario(usuarioId).Where(x => x.CodSala != CodigoSalaSomosCasino).ToList();

                if(listaSalas.Count != 1) {
                    respuesta = false;
                    mensaje = "Usted debe tener asignado solo una sala para poder realizar la acción.";
                    return Json(new { respuesta, mensaje });
                }

                SalaEntidad sala = listaSalas.First();
                AST_ClienteEntidad clienteConsulta = ast_ClienteBL.GetClientexNroyTipoDoc(cliente.TipoDocumentoId, cliente.NroDoc);
                if(clienteConsulta.Existe()) {
                    List<AST_ClienteSalaEntidad> clienteSalas = ast_clienteSalaBL.GetListadoClienteSala(clienteConsulta.Id);
                    AST_ClienteSalaEntidad clienteSalaConsulta = clienteSalas.FirstOrDefault(x => x.SalaId == sala.CodSala) ?? new AST_ClienteSalaEntidad();
                    string registrado = clienteSalaConsulta.FechaRegistro.ToString("dd-MM-yyyy hh:mm tt");

                    if(!clienteSalaConsulta.Existe()) {
                        AST_ClienteSalaEntidad clienteSala = cliente.ClienteSala;
                        clienteSala.ClienteId = clienteConsulta.Id;
                        clienteSala.SalaId = sala.CodSala;
                        clienteSala.TipoRegistro = cliente.TipoRegistro;
                        clienteSala.EnviaNotificacionWhatsapp = cliente.EnviaNotificacionWhatsapp;
                        clienteSala.EnviaNotificacionSms = cliente.EnviaNotificacionSms;
                        clienteSala.EnviaNotificacionEmail = cliente.EnviaNotificacionEmail;
                        clienteSala.LlamadaCelular = cliente.LlamadaCelular;
                        respuesta = ast_clienteSalaBL.GuardarClienteSala(clienteSala);
                        mensaje = "Registro Insertado";
                    } else {
                        string salasRegistradas = string.Join(", ", clienteSalas.Select(x => x.Sala.Nombre).ToList());
                        mensaje = $"El número de documento ya se encuentra registrado, con fecha: {registrado}, cliente en la(s) sala(s): {salasRegistradas}.";
                    }

                } else {
                    DateTime fechaActual = DateTime.Now;
                    cliente.Edad = cliente.CalcularEdad();
                    cliente.Estado = "P";
                    cliente.FechaRegistro = fechaActual;
                    cliente.SalaId = sala.CodSala;
                    cliente.NombreCompleto = $"{cliente.Nombre} {cliente.ApelPat} {cliente.ApelMat}";
                    cliente.usuario_reg = usuarioId;

                    // Procedencia
                    cliente.Ciudadano = true;

                    if(!AST_ClienteEntidad.Ubigeo_Pais_Id.Equals(cliente.PaisId)) {
                        cliente.UbigeoProcedenciaId = cliente.CodigoUbigeo;
                        cliente.Ciudadano = false;
                    }

                    // Guardar Cliente
                    idInsertado = ast_ClienteBL.GuardarCliente(cliente);
                    if(idInsertado > 0) {
                        //insertar ClienteSala
                        AST_ClienteSalaEntidad clienteSala = cliente.ClienteSala;
                        clienteSala.ClienteId = idInsertado;
                        clienteSala.SalaId = sala.CodSala;
                        clienteSala.TipoRegistro = cliente.TipoRegistro;
                        clienteSala.EnviaNotificacionWhatsapp = cliente.EnviaNotificacionWhatsapp;
                        clienteSala.EnviaNotificacionSms = cliente.EnviaNotificacionSms;
                        clienteSala.EnviaNotificacionEmail = cliente.EnviaNotificacionEmail;
                        clienteSala.LlamadaCelular = cliente.LlamadaCelular;
                        respuesta = ast_clienteSalaBL.GuardarClienteSala(clienteSala);
                        mensaje = "Registro Insertado";
                    } else {
                        mensaje = "No se pudo insertar el registro";
                    }
                }
            } catch(Exception ex) {
                mensaje = $"{ex.Message}, llame al Administrador.";
            }
            return Json(new { mensaje, respuesta, idInsertado });
        }

        [HttpPost]
        public ActionResult EditarClienteJson(AST_ClienteEntidad cliente) {
            string mensaje = "";
            bool respuesta = false;
            int idInsertado = 0;
            AST_ClienteEntidad clienteConsulta = new AST_ClienteEntidad();
            List<SalaEntidad> listaSalas = new List<SalaEntidad>();
            SalaEntidad sala = new SalaEntidad();
            int usuarioId = Convert.ToInt32(Session["UsuarioID"]);

            UbigeoEntidad ubigeoConsulta = new UbigeoEntidad();
            List<UbigeoEntidad> listaDepartamentos = new List<UbigeoEntidad>();
            List<UbigeoEntidad> listaProvincias = new List<UbigeoEntidad>();
            List<UbigeoEntidad> listaDistritos = new List<UbigeoEntidad>();
            UbigeoEntidad departamento = new UbigeoEntidad();
            UbigeoEntidad provincia = new UbigeoEntidad();
            UbigeoEntidad distrito = new UbigeoEntidad();
            try {
                listaSalas = salaBl.ListadoSalaPorUsuario(usuarioId).Where(x => x.CodSala != CodigoSalaSomosCasino).ToList();
                if(listaSalas.Count == 1) {
                    clienteConsulta = ast_ClienteBL.GetClientexNroyTipoDoc(cliente.TipoDocumentoId, cliente.NroDoc);
                    if(clienteConsulta.Id == 0 || clienteConsulta.Id == cliente.Id) {
                        DateTime fechaActual = DateTime.Now;
                        cliente.Edad = DateTime.Today.Year - cliente.FechaNacimiento.Year;
                        if(cliente.ApelPat != "" && cliente.ApelPat != null) {
                            cliente.NombreCompleto = cliente.Nombre + " " + " " + cliente.ApelPat + " " + cliente.ApelMat;
                        }

                        // Procedencia
                        cliente.Ciudadano = true;

                        if(!AST_ClienteEntidad.Ubigeo_Pais_Id.Equals(cliente.PaisId)) {
                            cliente.UbigeoProcedenciaId = cliente.CodigoUbigeo;
                            cliente.Ciudadano = false;
                        }

                        // Editar Cliente
                        respuesta = ast_ClienteBL.EditarCliente(cliente);

                        if(respuesta) {
                            //editar ClienteSala
                            AST_ClienteSalaEntidad clienteSala = cliente.ClienteSala;
                            AST_ClienteSalaEntidad clienteSalaConsulta = new AST_ClienteSalaEntidad();

                            sala = listaSalas.FirstOrDefault();

                            clienteSalaConsulta = ast_clienteSalaBL.GetClienteSalaID(cliente.Id, sala.CodSala);

                            clienteSala.ClienteId = cliente.Id;
                            clienteSala.SalaId = sala.CodSala;
                            clienteSala.TipoRegistro = cliente.TipoRegistro;

                            if(clienteSalaConsulta.ClienteId > 0) {
                                //editar
                                respuesta = ast_clienteSalaBL.EditarClienteSala(clienteSala);
                            } else {
                                //insertar
                                respuesta = ast_clienteSalaBL.GuardarClienteSala(clienteSala);
                            }

                            mensaje = "Registro Editado";
                            //respuesta = true;
                        } else {
                            mensaje = "No se pudo editar el registro";
                        }
                    } else {
                        mensaje = "El Nro de Documento ya se encuentra registrado";
                    }
                } else {
                    mensaje = "Usted debe tener asignado solo una sala para poder realizar la acción";
                }

            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, idInsertado });
        }

        [HttpPost]
        public ActionResult ActualizarContactoCliente(AST_ClienteEntidad cliente) {
            string displayMessage = "No se pudo actualizar la información de contacto del cliente, Llame al administrador.";
            bool success = false;

            try {
                success = ast_ClienteBL.ActualizarContactoCliente(cliente);
                displayMessage = success ? "Información de contacto actualizada correctamente." : displayMessage;
            } catch(Exception ex) {
                displayMessage = ex.Message + ", Llame al administrador.";
            }
            return Json(new { success, displayMessage });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult GetClienteId(int ClienteId) {
            string mensaje = "";
            bool respuesta = false;
            AST_ClienteEntidad cliente = new AST_ClienteEntidad();
            List<SalaEntidad> listaSalas = new List<SalaEntidad>();
            SalaEntidad sala = new SalaEntidad();
            int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            try {
                cliente = ast_ClienteBL.GetClienteID(ClienteId);
                if(cliente.Id > 0) {
                    listaSalas = salaBl.ListadoSalaPorUsuario(usuarioId);

                    sala = listaSalas.FirstOrDefault();
                    AST_ClienteSalaEntidad clienteSala = new AST_ClienteSalaEntidad();
                    clienteSala = ast_clienteSalaBL.GetClienteSalaID(cliente.Id, sala.CodSala);
                    cliente.ClienteSala = clienteSala;
                    mensaje = "Obteniendo registro";
                    respuesta = true;
                } else {
                    mensaje = "No se pudo obtener el registro";
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data = cliente });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetDataSelects(UbigeoEntidad ubigeo) {
            List<UbigeoEntidad> listaUbigeo = new List<UbigeoEntidad>();
            List<AST_TipoFrecuenciaEntidad> listaTipoFrecuencia = new List<AST_TipoFrecuenciaEntidad>();
            List<AST_TipoClienteEntidad> listaTipoCliente = new List<AST_TipoClienteEntidad>();
            List<AST_TipoDocumentoEntidad> listaTipoDocumento = new List<AST_TipoDocumentoEntidad>();
            List<UbigeoEntidad> listaProvincias = new List<UbigeoEntidad>();
            List<UbigeoEntidad> listaDistritos = new List<UbigeoEntidad>();
            int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            string mensaje = "";
            bool respuesta = false;
            object oRespuesta = new object();
            List<AST_TipoJuegoEntidad> listaTipoJuego = new List<AST_TipoJuegoEntidad>();
            try {
                List<UbigeoEntidad> listaPaises = ubigeoBL.ListaPaises();

                listaUbigeo = ubigeoBL.ListadoDepartamento();
                listaTipoFrecuencia = ast_TipoFrecuenciaBL.GetListadoTipoFrecuencia();
                listaTipoCliente = ast_TipoClienteBL.GetListadoTipoCliente();
                listaTipoDocumento = ast_TipoDocumentoBL.GetListadoTipoDocumento();
                listaTipoJuego = ast_TipoJuegoBL.GetListadoTipoJuego();
                if(ubigeo.CodUbigeo != 0) {
                    listaProvincias = ubigeoBL.GetListadoProvincia(ubigeo.DepartamentoId);
                    listaDistritos = ubigeoBL.GetListadoDistrito(ubigeo.ProvinciaId, ubigeo.DepartamentoId);
                    oRespuesta = new {
                        dataPaises = listaPaises,
                        dataUbigeo = listaUbigeo,
                        dataTipoFrecuencia = listaTipoFrecuencia,
                        dataTipoCliente = listaTipoCliente,
                        dataTipoDocumento = listaTipoDocumento,
                        dataProvincias = listaProvincias,
                        dataDistritos = listaDistritos,
                        dataTipoJuego = listaTipoJuego
                    };
                } else {
                    oRespuesta = new {
                        dataPaises = listaPaises,
                        dataUbigeo = listaUbigeo,
                        dataTipoFrecuencia = listaTipoFrecuencia,
                        dataTipoCliente = listaTipoCliente,
                        dataTipoDocumento = listaTipoDocumento,
                        dataTipoJuego = listaTipoJuego
                    };
                }

                respuesta = true;
                mensaje = "Listando registros";
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data = oRespuesta });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetDataAsistenciaCliente(int ClienteId) {
            object oRespuesta = new object();
            string mensaje = "";
            bool respuesta = false;
            DateTime fechaFin = DateTime.Now;
            DateTime fechaIni = fechaFin.AddDays(-7);
            AST_ClienteEntidad cliente = new AST_ClienteEntidad();
            List<AST_AsistenciaClienteSalaEntidad> asistenciaCliente = new List<AST_AsistenciaClienteSalaEntidad>();
            SalaEntidad sala = new SalaEntidad();
            List<AST_TipoClienteEntidad> listaTipoCliente = new List<AST_TipoClienteEntidad>();
            List<AST_TipoFrecuenciaEntidad> listaTipoFrecuencia = new List<AST_TipoFrecuenciaEntidad>();
            List<SalaEntidad> listaSalas = new List<SalaEntidad>();
            AST_AsistenciaClienteSalaEntidad ultimaAsistenciaSala = new AST_AsistenciaClienteSalaEntidad();
            int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            AST_ClienteSalaEntidad clienteSala = new AST_ClienteSalaEntidad();
            List<AST_TipoJuegoEntidad> listaTipoJuego = new List<AST_TipoJuegoEntidad>();
            try {
                listaSalas = salaBl.ListadoSalaPorUsuario(usuarioId).Where(x => x.CodSala != CodigoSalaSomosCasino).ToList();
                if(listaSalas.Count == 1) {
                    sala = listaSalas.FirstOrDefault();
                    cliente = ast_ClienteBL.GetClienteID(ClienteId);
                    clienteSala = ast_clienteSalaBL.GetClienteSalaID(cliente.Id, sala.CodSala);
                    cliente.ClienteSala = clienteSala;
                    asistenciaCliente = ast_asistenciaClienteSalaBL.GetListadoAsistenciaClienteFiltros(cliente.Id, fechaIni, fechaFin, sala.CodSala).OrderByDescending(x => x.FechaRegistro).ToList();
                    ultimaAsistenciaSala = ast_asistenciaClienteSalaBL.GetUltimaAsistenciaClienteSalaID(cliente.Id, sala.CodSala);
                    listaTipoFrecuencia = ast_TipoFrecuenciaBL.GetListadoTipoFrecuencia();
                    listaTipoCliente = ast_TipoClienteBL.GetListadoTipoCliente();
                    listaTipoJuego = ast_TipoJuegoBL.GetListadoTipoJuego();
                    mensaje = "Listando Data";
                    respuesta = true;
                    oRespuesta = new {
                        dataCliente = cliente,
                        dataAsistenciaCliente = asistenciaCliente.ToList(),
                        dataTipoFrecuencia = listaTipoFrecuencia,
                        dataTipoCliente = listaTipoCliente,
                        dataUltimaAsistencia = ultimaAsistenciaSala,
                        dataTipoJuego = listaTipoJuego
                    };
                } else {
                    mensaje = "Usted debe tener asignado solo una sala para poder realizar la acción";
                }

            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data = oRespuesta });
        }
        [HttpPost]
        public ActionResult GuardarAsistenciaCliente(AST_AsistenciaClienteSalaEntidad asistencia) {
            string mensaje = "";
            bool respuesta = false;
            int idInsertado = 0;
            int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            List<SalaEntidad> listaSalas = new List<SalaEntidad>();
            SalaEntidad sala = new SalaEntidad();
            try {
                listaSalas = salaBl.ListadoSalaPorUsuario(usuarioId).Where(x => x.CodSala != CodigoSalaSomosCasino).ToList();
                if(listaSalas.Count == 1) {
                    sala = listaSalas.FirstOrDefault();
                    asistencia.SalaId = sala.CodSala;
                    //buscar data de maquina
                    string token = GetToken();
                    string uri = ConfigurationManager.AppSettings["AdministrativoUri"];
                    HttpClient client = new HttpClient {
                        BaseAddress = new Uri(uri)
                    };
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);

                    //HttpResponseMessage response = client.GetAsync("api/maquina/"+asistencia.CodMaquina+"?include=Juego&include=ModeloMaquina.MarcaMaquina").Result;
                    HttpResponseMessage response = client.GetAsync("odata/Maquina?$expand=ModeloMaquina($expand=MarcaMaquina),Juego($select=Nombre,CodJuego),Sala($select=CodSala,Nombre),EstadoMaquina($select=CodEstadoMaquina,Nombre)&$filter=CodMaquinaLey eq '" + asistencia.CodMaquina + "'").Result;
                    if(response.IsSuccessStatusCode) {
                        string data = response.Content.ReadAsStringAsync().Result;
                        if(data != "null") {
                            dynamic parsed = JObject.Parse(data);
                            dynamic value = parsed.value;

                            string casa = string.Join(" ", value);
                            if(casa != "") {
                                string CodMaquina = (string)value[0].CodMaquina;
                                int CodSala = Convert.ToInt32(value[0].CodSala);
                                string Juego = (string)value[0].Juego.Nombre;
                                string Marca = (string)value[0].ModeloMaquina.MarcaMaquina.Nombre;

                                if(sala.CodSala == CodSala && (!Juego.Equals(string.Empty) && !Marca.Equals(string.Empty))) {
                                    asistencia.JuegoMaquina = Juego;
                                    asistencia.MarcaMaquina = Marca;
                                    idInsertado = ast_asistenciaClienteSalaBL.GuardarAsistenciaClienteSala(asistencia);
                                    if(idInsertado > 0) {
                                        //editar en tabla ClienteSala
                                        AST_ClienteSalaEntidad clienteSala = new AST_ClienteSalaEntidad();
                                        AST_ClienteSalaEntidad clienteSalaConsulta = new AST_ClienteSalaEntidad();

                                        clienteSalaConsulta = ast_clienteSalaBL.GetClienteSalaID(asistencia.ClienteId, sala.CodSala);
                                        clienteSala.ClienteId = asistencia.ClienteId;
                                        clienteSala.SalaId = sala.CodSala;
                                        clienteSala.ApuestaImportante = asistencia.ApuestaImportante;
                                        clienteSala.TipoFrecuenciaId = asistencia.TipoFrecuenciaId;
                                        clienteSala.TipoClienteId = asistencia.TipoClienteId;
                                        clienteSala.TipoJuegoId = asistencia.TipoJuegoId;
                                        clienteSala.TipoRegistro = "WEB";
                                        if(clienteSalaConsulta.ClienteId > 0) {
                                            //Editar
                                            respuesta = ast_clienteSalaBL.EditarClienteSalaCompleto(clienteSala);
                                        } else {
                                            //insertar
                                            respuesta = ast_clienteSalaBL.GuardarClienteSala(clienteSala);
                                        }

                                        mensaje = "Registro insertado";
                                        respuesta = true;
                                    } else {
                                        mensaje = "No se pudo insertar el registro";
                                    }
                                } else {
                                    mensaje = "Datos de la Máquina: [" + asistencia.CodMaquina + "] inconsistentes";
                                }
                            } else {
                                mensaje = "No se encontraron datos de la Máquina";
                            }

                        } else {
                            mensaje = "No se encontraron datos de la Máquina";
                        }
                    }
                } else {
                    mensaje = "Usted debe tener asignado solo una sala para poder realizar la acción";
                }

            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoAsistenciaSalaFiltros(int[] ArraySalaId, DateTime fechaIni, DateTime fechaFin) {
            string mensaje = "";
            bool respuesta = false;
            int cantElementos = (ArraySalaId == null) ? 0 : ArraySalaId.Length;
            var strElementos = String.Empty;
            List<AST_AsistenciaClienteSalaEntidad> lista = new List<AST_AsistenciaClienteSalaEntidad>();
            object oRespuesta = new object();
            try {
                String busqueda = "";
                bool respuestaController = false;
                string accion = "EliminarAsistenciaClienteSala";
                busqueda = funciones.consulta("PermisoUsuario", @"
                                                                SELECT [WEB_PRolID],[WEB_RolID],[WEB_PRolFechaRegistro]
                                                                FROM [dbo].[SEG_PermisoRol] 
                                                                left join [SEG_Permiso] on [SEG_Permiso].[WEB_PermID]=[SEG_PermisoRol].[WEB_PermID]
                                                                where [SEG_PermisoRol].WEB_RolID =" + (int)Session["rol"] +
                                                                                        " and [SEG_Permiso].[WEB_PermNombre]='" + accion + "'"

                                                                         );
                if(busqueda.Length < 3) {
                    respuestaController = false;
                } else {
                    respuestaController = true;
                }
                //string strSalas =  String.Join(",", ArraySalaId);
                if(cantElementos > 0) {
                    strElementos = " [SalaId] in(" + "'" + String.Join("','", ArraySalaId) + "'" + ") and ";
                }
                lista = ast_asistenciaClienteSalaBL.GetListadoAsistenciaSalaFiltros(strElementos, fechaIni, fechaFin);
                oRespuesta = new {
                    dataListado = lista,
                    dataPermiso = respuestaController,
                };
                mensaje = "Listando registros";
                respuesta = true;
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data = oRespuesta });
        }
        [HttpPost]
        public ActionResult EliminarAsistenciaClienteSala(int AsistenciaId) {
            string mensaje = "";
            bool respuesta = false;
            try {
                respuesta = ast_asistenciaClienteSalaBL.EliminarAsistenciaClienteSala(AsistenciaId);
                mensaje = "Registro Eliminado";
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoAsistenciaSalaFiltrosExcel(int[] ArraySalaId, DateTime fechaIni, DateTime fechaFin) {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<AST_AsistenciaClienteSalaEntidad> lista = new List<AST_AsistenciaClienteSalaEntidad>();
            int cantElementos = (ArraySalaId == null) ? 0 : ArraySalaId.Length;
            var strElementos = String.Empty;
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;
            try {


                if(cantElementos > 0) {
                    strElementos = " [SalaId] in(" + "'" + String.Join("','", ArraySalaId) + "'" + ") and ";
                }

                lista = ast_asistenciaClienteSalaBL.GetListadoAsistenciaSalaFiltros(strElementos, fechaIni, fechaFin);
                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("ListadoAsistencia");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Cliente";
                    workSheet.Cells[3, 4].Value = "Sala";
                    workSheet.Cells[3, 5].Value = "Cod. Máquina";
                    workSheet.Cells[3, 6].Value = "Marca";
                    workSheet.Cells[3, 7].Value = "Juego";
                    workSheet.Cells[3, 8].Value = "Tipo Frecuencia";
                    workSheet.Cells[3, 9].Value = "Tipo Cliente";
                    workSheet.Cells[3, 10].Value = "Apuesta Importante";
                    workSheet.Cells[3, 11].Value = "Fecha";
                    //Body of table  
                    int recordIndex = 4;
                    int total = lista.Count;

                    foreach(var registro in lista) {
                        workSheet.Cells[recordIndex, 2].Value = registro.Id;
                        workSheet.Cells[recordIndex, 3].Value = registro.Cliente.NombreCompleto == "" ? registro.Cliente.Nombre + " " + registro.Cliente.ApelPat + "" + registro.Cliente.ApelMat : registro.Cliente.NombreCompleto;
                        workSheet.Cells[recordIndex, 4].Value = registro.Sala.Nombre;
                        workSheet.Cells[recordIndex, 5].Value = registro.CodMaquina;
                        workSheet.Cells[recordIndex, 6].Value = registro.MarcaMaquina;
                        workSheet.Cells[recordIndex, 7].Value = registro.JuegoMaquina;
                        workSheet.Cells[recordIndex, 8].Value = registro.TipoFrecuencia.Nombre;
                        workSheet.Cells[recordIndex, 9].Value = registro.TipoCliente.Nombre;
                        workSheet.Cells[recordIndex, 10].Value = registro.ApuestaImportante;

                        workSheet.Cells[recordIndex, 11].Value = registro.FechaRegistro.ToString("dd-MM-yyyy hh:mm:ss");
                        recordIndex++;
                    }
                    Color colbackground = ColorTranslator.FromHtml("#003268");
                    Color colborder = ColorTranslator.FromHtml("#074B88");

                    workSheet.Cells["B3:K3"].Style.Font.Bold = true;
                    workSheet.Cells["B3:K3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B3:K3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B3:K3"].Style.Font.Color.SetColor(Color.White);

                    workSheet.Cells["B3:K3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:K3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:K3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells["B3:K3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Cells["B3:K3"].Style.Border.Top.Color.SetColor(colborder);
                    workSheet.Cells["B3:K3"].Style.Border.Left.Color.SetColor(colborder);
                    workSheet.Cells["B3:K3"].Style.Border.Right.Color.SetColor(colborder);
                    workSheet.Cells["B3:K3"].Style.Border.Bottom.Color.SetColor(colborder);

                    int filaFooter_ = recordIndex;
                    workSheet.Cells["B" + filaFooter_ + ":K" + filaFooter_].Merge = true;
                    workSheet.Cells["B" + filaFooter_ + ":K" + filaFooter_].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter_ + ":K" + filaFooter_].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter_ + ":K" + filaFooter_].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter_ + ":K" + filaFooter_].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter_ + ":K" + filaFooter_].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[filaFooter_, 2].Value = "Total : " + (total) + " Registros";

                    workSheet.Cells[3, 2, filaFooter_, 8].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 30;
                    workSheet.Column(4).Width = 25;
                    workSheet.Column(5).Width = 25;
                    workSheet.Column(6).Width = 25;
                    workSheet.Column(7).Width = 25;
                    workSheet.Column(8).Width = 30;
                    workSheet.Column(9).Width = 30;
                    workSheet.Column(10).Width = 20;
                    workSheet.Column(11).Width = 25;

                    excelName = "asistencia_" + fechaIni.ToString("dd_MM_yyyy") + "_al_" + fechaFin.ToString("dd_MM_yyyy") + "_AsistenciaCliente.xlsx";
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());

                    mensaje = "Descargando Archivo";
                    respuesta = true;
                } else {
                    mensaje = "No se encontraron registros";
                }

            } catch(Exception exp) {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = base64String, excelName, respuesta });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoClienteExcel(bool buscar = false, string nroDoc = "") {
            string mensaje = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<AST_ClienteEntidad> lista = new List<AST_ClienteEntidad>();
            DateTime fechaHoy = DateTime.Now;
            try {
                if(buscar) {
                    if(nroDoc.Length >= 8) {
                        lista = ast_ClienteBL.GetListaClientesxNroDoc(nroDoc);
                    }
                } else {
                    lista = ast_ClienteBL.GetListadoCliente();
                }
                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("ListadoClientes");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[3, 2].Value = "Nombres y Apellidos";
                    workSheet.Cells[3, 3].Value = "Tipo Doc.";
                    workSheet.Cells[3, 4].Value = "Nro. Documento";
                    workSheet.Cells[3, 5].Value = "Edad";
                    workSheet.Cells[3, 6].Value = "Género";
                    workSheet.Cells[3, 7].Value = "Celular 1";
                    workSheet.Cells[3, 8].Value = "Celular 2";
                    workSheet.Cells[3, 9].Value = "Mail";
                    workSheet.Cells[3, 10].Value = "Fecha Cumpleaños";
                    workSheet.Cells[3, 11].Value = "Asist. des. de Cuarentena";
                    workSheet.Cells[3, 12].Value = "Estado";
                    //Body of table  
                    int recordIndex = 4;
                    int total = lista.Count;

                    foreach(var registro in lista) {
                        int edad = 0;
                        edad = fechaHoy.Year - registro.FechaNacimiento.Year;
                        registro.Edad = (registro.FechaNacimiento.Month > fechaHoy.Month) ? --edad : edad;

                        workSheet.Cells[recordIndex, 2].Value = registro.ApelPat != "" ? registro.Nombre + " " + registro.ApelPat + " " + registro.ApelMat : registro.NombreCompleto;
                        workSheet.Cells[recordIndex, 3].Value = registro.TipoDocumento.Nombre;
                        workSheet.Cells[recordIndex, 4].Value = registro.NroDoc;
                        workSheet.Cells[recordIndex, 5].Value = registro.Edad;
                        workSheet.Cells[recordIndex, 6].Value = registro.Genero == "M" ? "MASCULINO" : "FEMENINO";
                        workSheet.Cells[recordIndex, 7].Value = registro.Celular1;
                        workSheet.Cells[recordIndex, 8].Value = registro.Celular2;
                        workSheet.Cells[recordIndex, 9].Value = registro.Mail;
                        workSheet.Cells[recordIndex, 10].Value = registro.FechaNacimiento.ToString("dd-MM-yyyy hh:mm:ss");
                        workSheet.Cells[recordIndex, 11].Value = registro.AsistioDespuesCuarentena == 1 ? "SI" : "NO";
                        workSheet.Cells[recordIndex, 12].Value = registro.Estado == "A" ? "ACTIVO" : "PENDIENTE";
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

                    int filaFooter_ = recordIndex;
                    workSheet.Cells["B" + filaFooter_ + ":L" + filaFooter_].Merge = true;
                    workSheet.Cells["B" + filaFooter_ + ":L" + filaFooter_].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter_ + ":L" + filaFooter_].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter_ + ":L" + filaFooter_].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter_ + ":L" + filaFooter_].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter_ + ":L" + filaFooter_].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[filaFooter_, 2].Value = "Total : " + (total) + " Registros";

                    workSheet.Cells[3, 2, filaFooter_, 12].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 20;
                    workSheet.Column(4).Width = 20;
                    workSheet.Column(5).Width = 20;
                    workSheet.Column(6).Width = 20;
                    workSheet.Column(7).Width = 20;
                    workSheet.Column(8).Width = 20;
                    workSheet.Column(9).Width = 40;
                    workSheet.Column(10).Width = 20;
                    workSheet.Column(11).Width = 20;
                    workSheet.Column(12).Width = 20;


                    excelName = "listadoCliente_AsistenciaCliente_" + fechaHoy + ".xlsx";
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());

                    mensaje = "Descargando Archivo";
                    respuesta = true;
                } else {
                    mensaje = "No se encontraron registros";
                }

            } catch(Exception exp) {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
            }
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new {
                respuesta,
                excelName,
                data = base64String
            };
            var result = new ContentResult {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;
            //return Json(new { data = base64String, excelName, respuesta });
        }
        [HttpPost]
        public ActionResult CambiarEstadoCliente(int ID, string Estado) {
            string mensaje = "";
            bool respuesta = false;
            AST_ClienteEntidad cliente = new AST_ClienteEntidad();
            try {
                cliente.Id = ID;
                cliente.Estado = Estado;
                respuesta = ast_ClienteBL.EditarEstadoCliente(cliente);
                if(respuesta) {
                    mensaje = "Registro Editado";
                } else {
                    mensaje = "No se pudo editar el registro";
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult CambiarAsistenciaDespuesCuarentenaCliente(int ID, int Asistencia) {
            string mensaje = "";
            bool respuesta = false;
            AST_ClienteEntidad cliente = new AST_ClienteEntidad();
            try {
                cliente.Id = ID;
                cliente.AsistioDespuesCuarentena = Asistencia;
                respuesta = ast_ClienteBL.EditarAsistenciaDespuesCuarentena(cliente);
                if(respuesta) {
                    mensaje = "Registro Editado";
                } else {
                    mensaje = "No se pudo editar el registro";
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta });
        }
        #region TipoCliente
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoTipoCliente() {
            string mensaje = "";
            bool respuesta = false;
            List<AST_TipoClienteEntidad> lista = new List<AST_TipoClienteEntidad>();
            try {
                lista = ast_TipoClienteBL.GetListadoTipoCliente();
                mensaje = "Listando Registros";
                respuesta = true;
            } catch(Exception ex) {
                mensaje = ex.Message;
                respuesta = false;
            }
            return Json(new { mensaje, respuesta, data = lista });
        }
        [HttpPost]
        public ActionResult GuardarTipoCliente(AST_TipoClienteEntidad TipoCliente) {
            bool respuesta = false;
            string mensaje = "";
            int idInsertado = 0;
            try {
                idInsertado = ast_TipoClienteBL.GuardarTipoCliente(TipoCliente);
                if(idInsertado != 0) {
                    mensaje = "Registro Insertado";
                    respuesta = true;
                } else {
                    mensaje = "No se pudo insertar el registro";
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta });
        }
        [HttpPost]
        public ActionResult EditarTipoClienteJson(AST_TipoClienteEntidad TipoCliente) {
            bool respuesta = false;
            string mensaje = "";
            try {
                respuesta = ast_TipoClienteBL.EditarTipoCliente(TipoCliente);
                if(respuesta) {
                    mensaje = "Registro editado";
                } else {
                    mensaje = "No se pudo editar el registro";
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetTipoClienteId(int Id) {
            bool respuesta = false;
            string mensaje = "";
            AST_TipoClienteEntidad tipoCliente = new AST_TipoClienteEntidad();
            try {
                tipoCliente = ast_TipoClienteBL.GetTipoClienteID(Id);
                mensaje = "Obteniendo Registro";
                respuesta = true;
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data = tipoCliente });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoTipoClienteExcel() {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<AST_TipoClienteEntidad> lista = new List<AST_TipoClienteEntidad>();
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;
            try {


                lista = ast_TipoClienteBL.GetListadoTipoCliente();
                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("ListadoTipoCliente");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Nombre";
                    workSheet.Cells[3, 4].Value = "Descripcion";
                    workSheet.Cells[3, 5].Value = "Estado";
                    //Body of table  
                    int recordIndex = 4;
                    int total = lista.Count;

                    foreach(var registro in lista) {
                        workSheet.Cells[recordIndex, 2].Value = registro.Id;
                        workSheet.Cells[recordIndex, 3].Value = registro.Nombre;
                        workSheet.Cells[recordIndex, 4].Value = registro.Descripcion;
                        workSheet.Cells[recordIndex, 5].Value = registro.Estado == "A" ? "ACTIVO" : "INACTIVO";
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

                    int filaFooter_ = recordIndex;
                    workSheet.Cells["B" + filaFooter_ + ":E" + filaFooter_].Merge = true;
                    workSheet.Cells["B" + filaFooter_ + ":E" + filaFooter_].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter_ + ":E" + filaFooter_].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter_ + ":E" + filaFooter_].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter_ + ":E" + filaFooter_].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter_ + ":E" + filaFooter_].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[filaFooter_, 2].Value = "Total : " + (total) + " Registros";

                    workSheet.Cells[3, 2, filaFooter_, 5].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 25;
                    workSheet.Column(4).Width = 30;
                    workSheet.Column(5).Width = 15;

                    excelName = "tipoCliente_" + DateTime.Now.ToString("dd_MM_yyyy") + "_AsistenciaCliente.xlsx";
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());

                    mensaje = "Descargando Archivo";
                    respuesta = true;
                } else {
                    mensaje = "No se encontraron registros";
                }

            } catch(Exception exp) {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = base64String, excelName, respuesta });
        }
        #endregion
        #region TipoJuego
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoTipoJuego() {
            string mensaje = "";
            bool respuesta = false;
            List<AST_TipoJuegoEntidad> lista = new List<AST_TipoJuegoEntidad>();
            try {
                lista = ast_TipoJuegoBL.GetListadoTipoJuego();
                mensaje = "Listando Registros";
                respuesta = true;
            } catch(Exception ex) {
                mensaje = ex.Message;
                respuesta = false;
            }
            return Json(new { mensaje, respuesta, data = lista });
        }
        [HttpPost]
        public ActionResult GuardarTipoJuego(AST_TipoJuegoEntidad TipoJuego) {
            bool respuesta = false;
            string mensaje = "";
            int idInsertado = 0;
            try {
                idInsertado = ast_TipoJuegoBL.GuardarTipoJuego(TipoJuego);
                if(idInsertado != 0) {
                    mensaje = "Registro Insertado";
                    respuesta = true;
                } else {
                    mensaje = "No se pudo insertar el registro";
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta });
        }
        [HttpPost]
        public ActionResult EditarTipoJuegoJson(AST_TipoJuegoEntidad TipoJuego) {
            bool respuesta = false;
            string mensaje = "";
            try {
                respuesta = ast_TipoJuegoBL.EditarTipoJuego(TipoJuego);
                if(respuesta) {
                    mensaje = "Registro editado";
                } else {
                    mensaje = "No se pudo editar el registro";
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetTipoJuegoId(int Id) {
            bool respuesta = false;
            string mensaje = "";
            AST_TipoJuegoEntidad tipoJuego = new AST_TipoJuegoEntidad();
            try {
                tipoJuego = ast_TipoJuegoBL.GetTipoJuegoID(Id);
                mensaje = "Obteniendo Registro";
                respuesta = true;
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data = tipoJuego });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoTipoJuegoExcel() {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<AST_TipoJuegoEntidad> lista = new List<AST_TipoJuegoEntidad>();
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;
            try {


                lista = ast_TipoJuegoBL.GetListadoTipoJuego();
                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("ListadoTipoJuego");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Nombre";
                    workSheet.Cells[3, 4].Value = "Descripcion";
                    workSheet.Cells[3, 5].Value = "Estado";
                    //Body of table  
                    int recordIndex = 4;
                    int total = lista.Count;

                    foreach(var registro in lista) {
                        workSheet.Cells[recordIndex, 2].Value = registro.Id;
                        workSheet.Cells[recordIndex, 3].Value = registro.Nombre;
                        workSheet.Cells[recordIndex, 4].Value = registro.Descripcion;
                        workSheet.Cells[recordIndex, 5].Value = registro.Estado == "A" ? "ACTIVO" : "INACTIVO";
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

                    int filaFooter_ = recordIndex;
                    workSheet.Cells["B" + filaFooter_ + ":E" + filaFooter_].Merge = true;
                    workSheet.Cells["B" + filaFooter_ + ":E" + filaFooter_].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter_ + ":E" + filaFooter_].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter_ + ":E" + filaFooter_].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter_ + ":E" + filaFooter_].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter_ + ":E" + filaFooter_].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[filaFooter_, 2].Value = "Total : " + (total) + " Registros";

                    workSheet.Cells[3, 2, filaFooter_, 5].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 25;
                    workSheet.Column(4).Width = 30;
                    workSheet.Column(5).Width = 15;

                    excelName = "tipoJuego_" + DateTime.Now.ToString("dd_MM_yyyy") + "_AsistenciaCliente.xlsx";
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());

                    mensaje = "Descargando Archivo";
                    respuesta = true;
                } else {
                    mensaje = "No se encontraron registros";
                }

            } catch(Exception exp) {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = base64String, excelName, respuesta });
        }
        #endregion
        #region TipoFrecuencia
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoTipoFrecuencia() {
            string mensaje = "";
            bool respuesta = false;
            List<AST_TipoFrecuenciaEntidad> lista = new List<AST_TipoFrecuenciaEntidad>();
            try {
                lista = ast_TipoFrecuenciaBL.GetListadoTipoFrecuencia();
                mensaje = "Listando Registros";
                respuesta = true;
            } catch(Exception ex) {
                mensaje = ex.Message;
                respuesta = false;
            }
            return Json(new { mensaje, respuesta, data = lista });
        }
        [HttpPost]
        public ActionResult GuardarTipoFrecuencia(AST_TipoFrecuenciaEntidad TipoFrecuencia) {
            bool respuesta = false;
            string mensaje = "";
            int idInsertado = 0;
            try {
                idInsertado = ast_TipoFrecuenciaBL.GuardarTipoFrecuencia(TipoFrecuencia);
                if(idInsertado != 0) {
                    mensaje = "Registro Insertado";
                    respuesta = true;
                } else {
                    mensaje = "No se pudo insertar el registro";
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta });
        }
        [HttpPost]
        public ActionResult EditarTipoFrecuenciaJson(AST_TipoFrecuenciaEntidad TipoFrecuencia) {
            bool respuesta = false;
            string mensaje = "";
            try {
                respuesta = ast_TipoFrecuenciaBL.EditarTipoFrecuencia(TipoFrecuencia);
                if(respuesta) {
                    mensaje = "Registro editado";
                } else {
                    mensaje = "No se pudo editar el registro";
                }
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetTipoFrecuenciaId(int Id) {
            bool respuesta = false;
            string mensaje = "";
            AST_TipoFrecuenciaEntidad tipoCliente = new AST_TipoFrecuenciaEntidad();
            try {
                tipoCliente = ast_TipoFrecuenciaBL.GetTipoFrecuenciaID(Id);
                mensaje = "Obteniendo Registro";
                respuesta = true;
            } catch(Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data = tipoCliente });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoTipoFrecuenciaExcel() {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<AST_TipoFrecuenciaEntidad> lista = new List<AST_TipoFrecuenciaEntidad>();
            var nombresala = new List<dynamic>();
            var salasSeleccionadas = String.Empty;
            try {


                lista = ast_TipoFrecuenciaBL.GetListadoTipoFrecuencia();
                if(lista.Count > 0) {

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = new ExcelPackage();

                    var workSheet = excel.Workbook.Worksheets.Add("ListadoTipoFrecuencia");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    //Header of table  
                    //  
                    workSheet.Row(3).Height = 20;
                    workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(3).Style.Font.Bold = true;
                    workSheet.Cells[3, 2].Value = "ID";
                    workSheet.Cells[3, 3].Value = "Nombre";
                    workSheet.Cells[3, 4].Value = "Descripcion";
                    workSheet.Cells[3, 5].Value = "Estado";
                    //Body of table  
                    int recordIndex = 4;
                    int total = lista.Count;

                    foreach(var registro in lista) {
                        workSheet.Cells[recordIndex, 2].Value = registro.Id;
                        workSheet.Cells[recordIndex, 3].Value = registro.Nombre;
                        workSheet.Cells[recordIndex, 4].Value = registro.Descripcion;
                        workSheet.Cells[recordIndex, 5].Value = registro.Estado == "A" ? "ACTIVO" : "INACTIVO";
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

                    int filaFooter_ = recordIndex;
                    workSheet.Cells["B" + filaFooter_ + ":E" + filaFooter_].Merge = true;
                    workSheet.Cells["B" + filaFooter_ + ":E" + filaFooter_].Style.Font.Bold = true;
                    workSheet.Cells["B" + filaFooter_ + ":E" + filaFooter_].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["B" + filaFooter_ + ":E" + filaFooter_].Style.Fill.BackgroundColor.SetColor(colbackground);
                    workSheet.Cells["B" + filaFooter_ + ":E" + filaFooter_].Style.Font.Color.SetColor(Color.White);
                    workSheet.Cells["B" + filaFooter_ + ":E" + filaFooter_].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[filaFooter_, 2].Value = "Total : " + (total) + " Registros";

                    workSheet.Cells[3, 2, filaFooter_, 5].AutoFilter = true;

                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).Width = 25;
                    workSheet.Column(4).Width = 30;
                    workSheet.Column(5).Width = 15;

                    excelName = "tipoFrecuencia_" + DateTime.Now.ToString("dd_MM_yyyy") + "_AsistenciaCliente.xlsx";
                    var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());

                    mensaje = "Descargando Archivo";
                    respuesta = true;
                } else {
                    mensaje = "No se encontraron registros";
                }

            } catch(Exception exp) {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = base64String, excelName, respuesta });
        }
        #endregion
        #region Sincronizar Exel Clientes
        [HttpPost]
        public ActionResult SincronizarExcelClientesJson() {
            bool response = false;
            string errormensaje = "";
            HttpPostedFileBase file = Request.Files["file"];
            string[] cabeceras = { "Nombre y Apellido", "Género", "Registro", "Fecha de nacimiento", "Ciudad", "Teléfono", "Correo", "Categoría Recarga", "Segmento CV", "DNI" };
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage ExcelResultado = new ExcelPackage();
            List<string> listaCabeceras = cabeceras.ToList();
            List<object> listaResultado = new List<object>();
            try {
                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                using(var package = new ExcelPackage(file.InputStream)) {
                    // get the first worksheet in the workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int colCount = worksheet.Dimension.End.Column;  //get Column Count
                    int rowCount = worksheet.Dimension.End.Row;     //get row count
                    int index = 0;
                    foreach(var cabecera in cabeceras) {
                        string cabeceraModelo = cabecera.ToUpper();
                        string cabeceraExcelRecibido = worksheet.Cells[1, index + 1].Value?.ToString().Trim().ToUpper();
                        if(!cabeceraModelo.Equals(cabeceraExcelRecibido)) {
                            return Json(new {
                                respuesta = response,
                                mensaje = "Las cabeceras no coinciden con el excel modelo",
                            });
                        }
                        index++;
                    }
                    List<AST_ClienteEntidad> listaClientesBD = new List<AST_ClienteEntidad>();
                    List<AST_ClienteEntidad> listaClientesInsertar = new List<AST_ClienteEntidad>();
                    List<AST_ClienteEntidad> listaClientesEditar = new List<AST_ClienteEntidad>();
                    List<AST_ClienteEntidad> listaClientesNoInsertados = new List<AST_ClienteEntidad>();
                    listaClientesBD = ast_ClienteBL.GetListadoCliente();

                    //crear excel
                    ExcelResultado.Workbook.Worksheets.Add("Resultado");

                    ExcelWorksheet WSResultado = ExcelResultado.Workbook.Worksheets[0];
                    int rowResultado = 1;
                    string accionRealizada = "";
                    //string patternNroDocumento = @"^[0-9]+$";
                    Regex regexNroDocumento = new Regex(@"^[0-9]+$");



                    for(int row = 2; row <= rowCount; row++) {

                        AST_ClienteEntidad clienteIns = new AST_ClienteEntidad();
                        clienteIns.NombreCompleto = worksheet.Cells[row, 1].Value?.ToString().Replace("'", "").Trim().ToUpper();
                        clienteIns.Genero = worksheet.Cells[row, 2].Value?.ToString().Replace("'", "").Trim().ToUpper() == "MASCULINO" ? "M" : "F";
                        clienteIns.FechaRegistro = Convert.ToDateTime(worksheet.Cells[row, 3].Value?.ToString().Replace("'", ""));
                        clienteIns.FechaNacimiento = Convert.ToDateTime(worksheet.Cells[row, 4].Value?.ToString().Replace("'", ""));
                        clienteIns.UbigeoProcedenciaId = 0;
                        clienteIns.Celular1 = worksheet.Cells[row, 6].Value?.ToString().Replace("'", "");
                        clienteIns.Mail = worksheet.Cells[row, 7].Value?.ToString().Replace("'", "").Trim().ToUpper();
                        clienteIns.TipoDocumentoId = 0;
                        clienteIns.NroDoc = worksheet.Cells[row, 10].Value?.ToString().Replace("'", "");
                        clienteIns.TipoRegistro = "EXCEL";
                        clienteIns.Estado = "A";
                        if(!regexNroDocumento.IsMatch(clienteIns.NroDoc) || clienteIns.NroDoc == "0") {
                            //no coincide con ningun tipo de nro de documento
                            listaClientesNoInsertados.Add(clienteIns);
                            listaResultado.Add(new {
                                cliente = clienteIns,
                                accionRealizada = "No insertado, nro doc inválido."
                            });
                            WSResultado.Cells[rowResultado, 1].Value = clienteIns.NombreCompleto;
                            WSResultado.Cells[rowResultado, 2].Value = clienteIns.NroDoc;
                            WSResultado.Cells[rowResultado, 3].Value = "No insertado, nro doc inválido.";
                            rowResultado++;
                        } else {
                            AST_ClienteEntidad clienteConsulta = new AST_ClienteEntidad();
                            clienteConsulta = listaClientesBD.Where(x => x.NroDoc.Equals(clienteIns.NroDoc)).FirstOrDefault();
                            if(clienteConsulta == null) {
                                listaClientesInsertar.Add(clienteIns);
                            } else {
                                listaClientesNoInsertados.Add(clienteIns);
                                listaResultado.Add(new {
                                    cliente = clienteIns,
                                    accionRealizada = "No insertado, nro doc repetido."
                                });
                                WSResultado.Cells[rowResultado, 1].Value = clienteIns.NombreCompleto;
                                WSResultado.Cells[rowResultado, 2].Value = clienteIns.NroDoc;
                                WSResultado.Cells[rowResultado, 3].Value = "No insertado, nro doc repetido.";
                                rowResultado++;
                            }
                        }

                    }
                    //insertar registros
                    foreach(var cliente in listaClientesInsertar) {
                        cliente.usuario_reg = usuarioId;
                        int idClienteInsertado = ast_ClienteBL.GuardarCliente(cliente);
                        if(idClienteInsertado > 0) {
                            accionRealizada = "Insertado";
                            listaResultado.Add(new {
                                cliente,
                                accionRealizada
                            });
                        } else {
                            accionRealizada = "No se pudo Insertar";
                            listaResultado.Add(new {
                                cliente,
                                accionRealizada
                            });
                        }
                        WSResultado.Cells[rowResultado, 1].Value = cliente.NombreCompleto;
                        WSResultado.Cells[rowResultado, 2].Value = cliente.NroDoc;
                        WSResultado.Cells[rowResultado, 3].Value = accionRealizada;

                        rowResultado++;
                    }
                    WSResultado.Column(1).Width = 50;
                    WSResultado.Column(2).Width = 30;
                    WSResultado.Column(3).Width = 30;
                    response = true;
                    errormensaje = "Sincronizado";
                }
            } catch(Exception ex) {
                errormensaje = ex.Message;
            }
            byte[] imagebytes = ExcelResultado.GetAsByteArray();
            string base64String = Convert.ToBase64String(imagebytes);

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new {
                respuesta = response,
                mensaje = errormensaje,
                data = listaResultado.ToList(),
                base64 = base64String
            };
            var result = new ContentResult {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult SincronizarExcelClientesV2Json(int orden = 0) {
            bool response = false;
            string errormensaje = "";
            HttpPostedFileBase file = Request.Files["file"];
            int codsala = Convert.ToInt32(Request.Form["codsala"]);
            string[] cabeceras = { "Nombre y APellido del Cliente", "Fecha de Inscripción", "DNI", "Sala", "Número de teléfono", "Correo", "Tipo", "Frecuencia", "Tipo de Juego", "Cumpleaños" };
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage ExcelResultado = new ExcelPackage();
            List<string> listaCabeceras = cabeceras.ToList();
            List<object> listaResultado = new List<object>();
            try {
                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                using(var package = new ExcelPackage(file.InputStream)) {
                    // get the first worksheet in the workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int colCount = worksheet.Dimension.End.Column;  //get Column Count
                    int rowCount = worksheet.Dimension.End.Row;     //get row count
                    int index = 0;
                    foreach(var cabecera in cabeceras) {
                        string cabeceraModelo = cabecera.ToUpper();
                        string cabeceraExcelRecibido = worksheet.Cells[6, index + 1].Value?.ToString().Trim().ToUpper();
                        if(!cabeceraModelo.Equals(cabeceraExcelRecibido)) {
                            return Json(new {
                                respuesta = response,
                                mensaje = "Las cabeceras no coinciden con el excel modelo" + cabeceraExcelRecibido,
                            });
                        }
                        index++;
                    }
                    List<AST_ClienteEntidad> listaClientesBD = new List<AST_ClienteEntidad>();
                    List<AST_ClienteEntidad> listaClientesInsertar = new List<AST_ClienteEntidad>();
                    List<AST_ClienteEntidad> listaClientesSinFechas = new List<AST_ClienteEntidad>();
                    List<AST_ClienteEntidad> listaClientesEditar = new List<AST_ClienteEntidad>();
                    List<AST_ClienteEntidad> listaClientesNoInsertados = new List<AST_ClienteEntidad>();
                    listaClientesBD = ast_ClienteBL.GetListadoCliente();

                    //crear excel
                    ExcelResultado.Workbook.Worksheets.Add("Resultado");

                    ExcelWorksheet WSResultado = ExcelResultado.Workbook.Worksheets[0];
                    int rowResultado = 1;
                    string accionRealizada = "";
                    //string patternNroDocumento = @"^[0-9]+$";
                    //Regex regexNroDocumento = new Regex(@"^[0-9]+$");

                    string rowsala = worksheet.Cells[7, 4].Value?.ToString();
                    if(Convert.ToInt32(rowsala) != codsala) {
                        return Json(new {
                            respuesta = response,
                            mensaje = "La Sala selecciona no coincide con el código de sala del archivo adjunto",
                        });
                    }

                    for(int row = 7; row <= rowCount; row++) {

                        AST_ClienteEntidad clienteIns = new AST_ClienteEntidad();
                        clienteIns.NombreCompleto = worksheet.Cells[row, 1].Value?.ToString().Replace("'", "").Trim().ToUpper();

                        try {
                            string[] nombre_ = clienteIns.NombreCompleto.Split(' ');
                            clienteIns.Nombre = "";

                            if(orden == 0)//Orden: Nombres ApelPat ApelMat
                            {
                                clienteIns.ApelMat = nombre_[nombre_.Count() - 1];
                                clienteIns.ApelPat = nombre_[nombre_.Count() - 2];


                                for(int i = 0; i < nombre_.Count() - 2; i++) {
                                    clienteIns.Nombre = clienteIns.Nombre + " " + nombre_[i];
                                }
                            } else//Orden : ApelPat ApelMat Nombres
                              {
                                clienteIns.ApelPat = nombre_[0];
                                clienteIns.ApelMat = nombre_[1];
                                for(int i = 2; i <= nombre_.Count() - 1; i++) {
                                    clienteIns.Nombre = clienteIns.Nombre + " " + nombre_[i];
                                }
                            }
                        } catch(Exception exp) {
                            Console.WriteLine(exp.Message);
                        }


                        clienteIns.TipoDocumentoId = 0;
                        clienteIns.NroDoc = worksheet.Cells[row, 3].Value?.ToString().Replace("'", "").Trim().ToUpper();
                        clienteIns.SalaId = codsala;
                        clienteIns.Celular1 = worksheet.Cells[row, 5].Value?.ToString().Replace("'", "");
                        clienteIns.Mail = worksheet.Cells[row, 6].Value?.ToString().Replace("'", "").Trim().ToUpper();
                        clienteIns.UbigeoProcedenciaId = 0;
                        clienteIns.TipoRegistro = "EXCELV2";
                        clienteIns.Estado = "A";

                        try {

                            //if (!regexNroDocumento.IsMatch(clienteIns.NroDoc) || clienteIns.NroDoc == "0")
                            if(clienteIns.NroDoc == "0" || clienteIns.NroDoc == "" || clienteIns.NroDoc.Length < 8) {
                                //no coincide con ningun tipo de nro de documento
                                listaClientesNoInsertados.Add(clienteIns);
                                listaResultado.Add(new {
                                    cliente = clienteIns,
                                    accionRealizada = "ERROR, No insertado, nro doc inválido."
                                });
                                WSResultado.Cells[rowResultado, 1].Value = clienteIns.NombreCompleto;
                                WSResultado.Cells[rowResultado, 2].Value = clienteIns.NroDoc;
                                WSResultado.Cells[rowResultado, 3].Value = "ERROR, No insertado, nro doc inválido.";
                                rowResultado++;
                            } else {
                                bool pasoValidacion = false;
                                //Tratar de convertir fechas y tipos de cliente
                                try {
                                    clienteIns.ClienteSala.TipoClienteId = Convert.ToInt32(worksheet.Cells[row, 7].Value?.ToString());
                                    clienteIns.ClienteSala.TipoFrecuenciaId = Convert.ToInt32(worksheet.Cells[row, 8].Value?.ToString());
                                    clienteIns.ClienteSala.TipoJuegoId = Convert.ToInt32(worksheet.Cells[row, 9].Value?.ToString());
                                    clienteIns.FechaNacimiento = Convert.ToDateTime(worksheet.Cells[row, 10].Value?.ToString().Replace("'", ""));
                                    clienteIns.FechaRegistro = Convert.ToDateTime(worksheet.Cells[row, 2].Value?.ToString().Replace("'", ""));
                                    pasoValidacion = true;
                                } catch(Exception ex) {
                                    Console.WriteLine(ex.Message);

                                    listaClientesSinFechas.Add(clienteIns);
                                }
                                if(pasoValidacion) {
                                    AST_ClienteEntidad clienteConsulta = new AST_ClienteEntidad();
                                    clienteConsulta = listaClientesBD.Where(x => x.NroDoc.Equals(clienteIns.NroDoc)).FirstOrDefault();
                                    string fechaNacimiento = clienteIns.FechaNacimiento.ToString("dd/MM/yyyy");
                                    if(clienteConsulta == null) {
                                        if((fechaNacimiento == "01/01/0001" || fechaNacimiento == "31/12/1752")) {
                                            listaClientesSinFechas.Add(clienteIns);
                                            //listaClientesNoInsertados.Add(clienteIns);
                                            //listaResultado.Add(new
                                            //{
                                            //    cliente = clienteIns,
                                            //    accionRealizada = "Fecha de nacimiento null"
                                            //});
                                            //WSResultado.Cells[rowResultado, 1].Value = clienteIns.NombreCompleto;
                                            //WSResultado.Cells[rowResultado, 2].Value = clienteIns.NroDoc;
                                            //WSResultado.Cells[rowResultado, 3].Value = "Fecha de nacimiento null";
                                            //rowResultado++;
                                        } else {
                                            if(clienteIns.FechaNacimiento.Year < 1753) {
                                                listaClientesSinFechas.Add(clienteIns);
                                            } else {
                                                listaClientesInsertar.Add(clienteIns);
                                            }

                                        }
                                    } else {
                                        listaClientesNoInsertados.Add(clienteIns);
                                        listaResultado.Add(new {
                                            cliente = clienteIns,
                                            accionRealizada = "ERROR, No insertado, nro doc repetido."
                                        });
                                        WSResultado.Cells[rowResultado, 1].Value = clienteIns.NombreCompleto;
                                        WSResultado.Cells[rowResultado, 2].Value = clienteIns.NroDoc;
                                        WSResultado.Cells[rowResultado, 3].Value = "ERROR, No insertado, nro doc repetido.";
                                        rowResultado++;
                                    }
                                }
                            }

                        } catch(Exception ex) {
                            Console.WriteLine(ex.Message);
                            listaClientesSinFechas.Add(clienteIns);
                        }
                    }



                    //insertar registros
                    foreach(var cliente in listaClientesInsertar) {
                        cliente.usuario_reg = usuarioId;
                        cliente.FechaRegistro = DateTime.Now;
                        int idClienteInsertado = ast_ClienteBL.GuardarCliente(cliente);
                        if(idClienteInsertado > 0) {

                            AST_ClienteSalaEntidad clienteSala = cliente.ClienteSala;
                            clienteSala.ClienteId = idClienteInsertado;
                            clienteSala.SalaId = codsala;
                            clienteSala.TipoFrecuenciaId = cliente.ClienteSala.TipoFrecuenciaId;
                            clienteSala.TipoClienteId = cliente.ClienteSala.TipoClienteId;
                            clienteSala.TipoJuegoId = cliente.ClienteSala.TipoJuegoId;
                            clienteSala.TipoRegistro = cliente.TipoRegistro;
                            AST_ClienteSalaEntidad clienteSalaConsulta = new AST_ClienteSalaEntidad();
                            clienteSalaConsulta = ast_clienteSalaBL.GetClienteSalaID(idClienteInsertado, codsala);
                            if(clienteSalaConsulta.ClienteId > 0) {
                                //editar
                                //ast_clienteSalaBL.EditarClienteSala(clienteSala);
                            } else {
                                //insertar
                                ast_clienteSalaBL.GuardarClienteSala(clienteSala);
                            }

                            accionRealizada = "OK, Insertado";
                            listaResultado.Add(new {
                                cliente,
                                accionRealizada
                            });
                        } else {
                            accionRealizada = "ERROR, No se pudo Insertar";
                            listaResultado.Add(new {
                                cliente,
                                accionRealizada
                            });
                        }
                        WSResultado.Cells[rowResultado, 1].Value = cliente.NombreCompleto;
                        WSResultado.Cells[rowResultado, 2].Value = cliente.NroDoc;
                        WSResultado.Cells[rowResultado, 3].Value = accionRealizada;

                        rowResultado++;
                    }



                    //insertar registros
                    foreach(var cliente in listaClientesSinFechas) {
                        AST_ClienteEntidad clienteConsulta = new AST_ClienteEntidad();
                        clienteConsulta = listaClientesBD.Where(x => x.NroDoc.Equals(cliente.NroDoc)).FirstOrDefault();

                        if(clienteConsulta == null && cliente.NroDoc != null) {

                            cliente.usuario_reg = usuarioId;
                            cliente.FechaRegistro = DateTime.Now;
                            int idClienteInsertado = ast_ClienteBL.GuardarClienteSinFechas(cliente);
                            if(idClienteInsertado > 0) {

                                AST_ClienteSalaEntidad clienteSala = cliente.ClienteSala;
                                clienteSala.ClienteId = idClienteInsertado;
                                clienteSala.SalaId = codsala;
                                clienteSala.TipoFrecuenciaId = cliente.ClienteSala.TipoFrecuenciaId;
                                clienteSala.TipoClienteId = cliente.ClienteSala.TipoClienteId;
                                clienteSala.TipoJuegoId = cliente.ClienteSala.TipoJuegoId;
                                clienteSala.TipoRegistro = cliente.ClienteSala.TipoRegistro;
                                AST_ClienteSalaEntidad clienteSalaConsulta = new AST_ClienteSalaEntidad();
                                clienteSalaConsulta = ast_clienteSalaBL.GetClienteSalaID(idClienteInsertado, codsala);
                                if(clienteSalaConsulta.ClienteId > 0) {
                                    //editar
                                    //ast_clienteSalaBL.EditarClienteSala(clienteSala);
                                } else {
                                    //insertar
                                    ast_clienteSalaBL.GuardarClienteSala(clienteSala);
                                }

                                accionRealizada = "OK, Insertado";
                                listaResultado.Add(new {
                                    cliente,
                                    accionRealizada
                                });
                            } else {
                                accionRealizada = "ERROR, No se pudo Insertar";
                                listaResultado.Add(new {
                                    cliente,
                                    accionRealizada
                                });
                            }
                            WSResultado.Cells[rowResultado, 1].Value = cliente.NombreCompleto;
                            WSResultado.Cells[rowResultado, 2].Value = cliente.NroDoc;
                            WSResultado.Cells[rowResultado, 3].Value = accionRealizada;

                            rowResultado++;



                        } else {
                            if(cliente.NroDoc == null) {

                                listaResultado.Add(new {
                                    cliente = cliente,
                                    accionRealizada = "ERROR, No insertado, sin nro documento."
                                });
                                WSResultado.Cells[rowResultado, 1].Value = cliente.NombreCompleto;
                                WSResultado.Cells[rowResultado, 2].Value = cliente.NroDoc;
                                WSResultado.Cells[rowResultado, 3].Value = "ERROR, No insertado, sin nro documento.";
                                rowResultado++;
                            } else {

                                listaResultado.Add(new {
                                    cliente = cliente,
                                    accionRealizada = "ERROR, No insertado, nro doc repetido."
                                });
                                WSResultado.Cells[rowResultado, 1].Value = cliente.NombreCompleto;
                                WSResultado.Cells[rowResultado, 2].Value = cliente.NroDoc;
                                WSResultado.Cells[rowResultado, 3].Value = "ERROR, No insertado, nro doc repetido.";
                                rowResultado++;
                            }
                        }
                    }

                    WSResultado.Column(1).Width = 50;
                    WSResultado.Column(2).Width = 30;
                    WSResultado.Column(3).Width = 30;
                    response = true;
                    errormensaje = $"Sincronización satisfactoria ({listaClientesInsertar.Count()} insertados)";
                }




            } catch(Exception ex) {
                errormensaje = ex.Message;
            }
            byte[] imagebytes = ExcelResultado.GetAsByteArray();
            string base64String = Convert.ToBase64String(imagebytes);

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new {
                respuesta = response,
                mensaje = errormensaje,
                data = listaResultado.ToList(),
                base64 = base64String
            };
            var result = new ContentResult {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;
        }
        #endregion
        [seguridad(false)]
        public static string GetToken() {
            string administrativoUsername = ConfigurationManager.AppSettings["AdministrativoUsername"];
            string administrativoPassword = ConfigurationManager.AppSettings["AdministrativoPassword"];
            string key = administrativoUsername + ":" + administrativoPassword;
            return Encode(key);
        }
        [seguridad(false)]
        public static string Encode(string plainText) {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        [seguridad(false)]
        public static string Decode(string base64EncodedData) {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }


        ////
        ///cambios vacunacion
        public bool PermisoBuscarCliente() {
            return true;
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoClienteNroDocumento(string nrodoc) {
            string mensaje = "";
            bool respuesta = false;
            List<AST_ClienteEntidad> lista = new List<AST_ClienteEntidad>();
            try {
                if(nrodoc.Length >= 8) {
                    lista = ast_ClienteBL.GetListaClientesxNroDoc(nrodoc);

                }

            } catch(Exception ex) {
                mensaje = ex.Message;
                respuesta = false;
            }
            return Json(new { mensaje, respuesta, data = lista });
        }


        [HttpPost]
        public ActionResult GetListadoCLientesSala(int[] ArraySalaId, DateTime fechaIni, DateTime fechaFin, DtParameters dtParameters) {
            /*

            string mensaje = "";
            bool respuesta = false;
            int cantElementos = (ArraySalaId == null) ? 0 : ArraySalaId.Length;
            var strElementos = String.Empty;
            List<AST_ClienteSala> lista = new List<AST_ClienteSala>();

            try
            {
               
                //string strSalas =  String.Join(",", ArraySalaId);
                if (cantElementos > 0)
                {
                    strElementos = " sala.CodSala in(" + "'" + String.Join("','", ArraySalaId) + "'" + ") and ";
                }
                lista = ast_asistenciaClienteSalaBL.GetListadoClienteSala(strElementos, fechaIni, fechaFin);

                mensaje = "Listando registros";
                respuesta = true;   
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data = lista });
            */
            var errormensaje = "";
            List<AST_ClienteSala> lista = new List<AST_ClienteSala>();
            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = "";
            List<dynamic> registro = new List<dynamic>();
            int pageSize, skip;
            int recordsTotal = 0;
            int recordsFiltered = 0;
            int cantElementos = (ArraySalaId == null) ? 0 : ArraySalaId.Length;
            var strElementos = String.Empty;
            bool tienePermisoVerInformacionContactoCliente = false;

            try {
                if(cantElementos > 0) {
                    //strElementos = " sala.CodSala in(" + "'" + String.Join("','", ArraySalaId) + "'" + ") and ";
                    strElementos = $" sala.CodSala in('{string.Join("','", ArraySalaId)}');";
                }
                if(dtParameters.Order != null) {
                    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower();
                } else {
                    orderCriteria = "cliente.CodSala";
                    orderAscendingDirection = "asc";
                }
                string whereQuery = "";
                if(!string.IsNullOrEmpty(searchBy) && !(string.IsNullOrWhiteSpace(searchBy))) {
                    string[] values = { "codSala", "NombreSala", "TipoDocumento", "NroDoc", "NombreCliente", "cantDosis", "Celular", "Mail", "FechaNacimiento", "FechaRegistro" };
                    List<string> listaWhere = new List<string>();
                    foreach(var value in values) {
                        if(value == "cantDosis" || value == "FechaNacimiento" || value == "FechaRegistro" || value == "codSala") {
                            continue;
                        }
                        var strSearchValue = string.Empty;
                        switch(value) {
                            case "NombreSala":
                                strSearchValue = "cliente.NombreSala";
                                break;
                            case "TipoDocumento":
                                strSearchValue = "cliente.TipoDocumento";
                                break;
                            case "NroDoc":
                                strSearchValue = "cliente.NroDoc";
                                break;
                            case "NombreCliente":
                                strSearchValue = "cliente.NombreCliente";
                                break;
                            case "Celular":
                                strSearchValue = "cliente.Celular";
                                break;
                            case "Mail":
                                strSearchValue = "cliente.Mail";
                                break;

                        }
                        listaWhere.Add($@" {strSearchValue} like '%{searchBy}%' ");
                    }
                    whereQuery += $@" and ( {String.Join(" or ", listaWhere)} )";
                }
                pageSize = dtParameters.Length;
                skip = dtParameters.Start;

                recordsFiltered = ast_clienteSalaBL.ObtenerTotalRegistrosFiltrados(whereQuery, strElementos, fechaIni, fechaFin);
                whereQuery += $@" order by {orderCriteria} {orderAscendingDirection} offset {skip} rows fetch next {pageSize} rows only;";
                lista = ast_clienteSalaBL.GetAllClientesFiltrados(whereQuery, strElementos, fechaIni, fechaFin);
                recordsTotal = ast_clienteSalaBL.ObtenerTotalRegistros();
                tienePermisoVerInformacionContactoCliente = seg_PermisoRolBL.GetPermisoRolUsuario((int)Session["rol"], nameof(permisosClienteController.VerInfoContactoCliente)).Count > 0;


            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { draw = dtParameters.Draw, recordsFiltered = recordsFiltered, recordsTotal = recordsTotal, data = lista, mensaje = "Listando Registros", permisos = new { verInformacionContactoCliente = tienePermisoVerInformacionContactoCliente } });

        }

        [HttpPost]
        public ActionResult GetListadoCLientesSalaServer(int[] ArraySalaId, DateTime fechaIni, DateTime fechaFin, DtParameters dtParameters) {

            var errormensaje = "";
            List<AST_ClienteSala> lista = new List<AST_ClienteSala>();
            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = "";
            List<dynamic> registro = new List<dynamic>();
            int pageSize, skip;
            int recordsTotal = 0;
            int recordsFiltered = 0;
            int cantElementos = (ArraySalaId == null) ? 0 : ArraySalaId.Length;
            var strElementos = String.Empty;

            try {
                if(cantElementos > 0) {
                    //strElementos = " sala.CodSala in(" + "'" + String.Join("','", ArraySalaId) + "'" + ") and ";
                    strElementos = $" sala.CodSala in('{string.Join("','", ArraySalaId)}');";
                }
                if(dtParameters.Order != null) {
                    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower();
                } else {
                    orderCriteria = "cliente.codSala";
                    orderAscendingDirection = "asc";
                }
                string whereQuery = "";
                if(!string.IsNullOrEmpty(searchBy) && !(string.IsNullOrWhiteSpace(searchBy))) {
                    string[] values = { "codSala", "NombreSala", "NroDoc", "NombreCliente", "cantDosis", "Celular", "Mail", "FechaNacimiento", "FechaRegistro" };
                    List<string> listaWhere = new List<string>();
                    foreach(var value in values) {
                        listaWhere.Add($@" {value} like '%{searchBy}%' ");
                    }
                    whereQuery += $@" and ( {String.Join(" or ", listaWhere)} )";
                }
                pageSize = dtParameters.Length;
                skip = dtParameters.Start;

                recordsFiltered = ast_clienteSalaBL.ObtenerTotalRegistrosFiltrados(whereQuery, strElementos, fechaIni, fechaFin);
                whereQuery += $@" order by {orderCriteria} {orderAscendingDirection} offset {skip} rows fetch next {pageSize} rows only;";
                lista = ast_clienteSalaBL.GetAllClientesFiltrados(whereQuery, strElementos, fechaIni, fechaFin);
                recordsTotal = ast_clienteSalaBL.ObtenerTotalRegistros();


            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { draw = dtParameters.Draw, recordsFiltered = recordsFiltered, recordsTotal = recordsTotal, data = lista, mensaje = "Listando Registros", });

        }

        [HttpPost]
        public ActionResult GetAllListadoClienteSala(int salaId) {
            string mensaje = string.Empty;
            string mensajeConsola = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            try {
                SalaEntidad sala = salaBl.SalaListaIdJson(salaId);
                List<AST_ClienteSalaGlobal> lista = ast_asistenciaClienteSalaBL.GetAllClientesSala(salaId);
                if(lista.Count > 0) {
                    SalaMaestraEntidad salaMaestra = salaMaestraBL.ObtenerSalaMaestraPorCodigoSala(lista.First().codSala);

                    #region Clientes
                    List<AST_ClienteSalaGlobal> clientes = lista.Where(cliente => !cliente.EsLudopata && !cliente.EsProhibido && !cliente.EsRobaStacker).ToList();
                    DataTable dataTableCliente = new DataTable();
                    bool puedeInfoContacto = seg_PermisoRolBL.GetPermisoRolUsuario((int)Session["rol"], nameof(permisosClienteController.VerInfoContactoCliente)).Count > 0;
                    ExcelHelper.AddColumnsClienteSalaGlobal(dataTableCliente, puedeInfoContacto);
                    ExcelHelper.AddDataClienteSalaGlobal(dataTableCliente, clientes, puedeInfoContacto);
                    HojaExcelReporteAgrupado hojaClientes = new HojaExcelReporteAgrupado {
                        Data = dataTableCliente,
                        Nombre = "Clientes",
                        Color = "#28A745",
                        MetaData = new List<string> {
                            $"Fecha Generación: {DateTime.Now:dd/MM/yyyy HH:mm}",
                            $"Cantidad de Registros: {clientes.Count}",
                            $"SALA: {salaMaestra.Nombre}"
                        }
                    };
                    #endregion

                    #region Ludopatas
                    List<AST_ClienteSalaGlobal> ludopatas = lista.Where(cliente => cliente.EsLudopata).ToList();
                    DataTable dataTableLudopatas = new DataTable();
                    ExcelHelper.AddColumnsClienteSalaGlobal(dataTableLudopatas, false);
                    ExcelHelper.AddDataClienteSalaGlobal(dataTableLudopatas, ludopatas, false);
                    HojaExcelReporteAgrupado hojaLudopatas = new HojaExcelReporteAgrupado {
                        Data = dataTableLudopatas,
                        Nombre = "Ludópatas",
                        Color = "#FF8C00",
                        MetaData = new List<string> {
                            $"Fecha Generación: {DateTime.Now:dd/MM/yyyy HH:mm}",
                            $"Cantidad de Registros: {ludopatas.Count}",
                            $"SALA: {salaMaestra.Nombre}"
                        }
                    };
                    #endregion

                    #region Prohibidos
                    List<AST_ClienteSalaGlobal> prohibidos = lista.Where(cliente => cliente.EsProhibido).ToList();
                    DataTable dataTableProhibidos = new DataTable();
                    ExcelHelper.AddColumnsClienteSalaGlobal(dataTableProhibidos, false);
                    ExcelHelper.AddDataClienteSalaGlobal(dataTableProhibidos, prohibidos, false);
                    HojaExcelReporteAgrupado hojaProhibidos = new HojaExcelReporteAgrupado {
                        Data = dataTableProhibidos,
                        Nombre = "Prohibidos",
                        Color = "#DC3545",
                        MetaData = new List<string> {
                            $"Fecha Generación: {DateTime.Now:dd/MM/yyyy HH:mm}",
                            $"Cantidad de Registros: {prohibidos.Count}",
                            $"SALA: {salaMaestra.Nombre}"
                        }
                    };
                    #endregion

                    #region RobaStackers
                    List<AST_ClienteSalaGlobal> robaStacker = lista.Where(cliente => cliente.EsRobaStacker).ToList();
                    DataTable dataTableRobaStacker = new DataTable();
                    ExcelHelper.AddColumnsClienteSalaGlobal(dataTableRobaStacker, false);
                    ExcelHelper.AddDataClienteSalaGlobal(dataTableRobaStacker, robaStacker, false);
                    HojaExcelReporteAgrupado hojaRobaStacker = new HojaExcelReporteAgrupado {
                        Data = dataTableRobaStacker,
                        Nombre = "Roba Stackers",
                        Color = "#6F42C1",
                        MetaData = new List<string> {
                            $"Fecha Generación: {DateTime.Now:dd/MM/yyyy HH:mm}",
                            $"Cantidad de Registros: {robaStacker.Count}",
                            $"SALA: {salaMaestra.Nombre}"
                        }
                    };
                    #endregion

                    #region Historial Ludopata
                    List<CAL_HistorialLudopataDto> historialLudopata = historialLudopataBL.ObtenerHistorialLudopata();
                    List<string> dnis = lista.Select(x => x.NroDoc.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
                    List<CAL_HistorialLudopataDto> historialLudopataFiltrado = historialLudopata.Where(x => dnis.Contains(x.NumeroDocumento)).ToList();

                    DataTable dataTableHistorialLudopata = new DataTable();
                    ExcelHelper.AddColumnsHistorialLudopata(dataTableHistorialLudopata);
                    ExcelHelper.AddDataHistorialLudopata(dataTableHistorialLudopata, historialLudopataFiltrado);
                    HojaExcelReporteAgrupado hojaHistorialLudopatas = new HojaExcelReporteAgrupado {
                        Data = dataTableHistorialLudopata,
                        Nombre = "Historial Ludópata",
                        Color = "#007BFF",
                        MetaData = new List<string> {
                            $"Fecha Generación: {DateTime.Now:dd/MM/yyyy HH:mm}",
                            $"Cantidad de Registros: {historialLudopataFiltrado.Count}",
                            $"SALA: {salaMaestra.Nombre}"
                        }
                    };
                    #endregion

                    List<HojaExcelReporteAgrupado> sheets = new List<HojaExcelReporteAgrupado> {
                        hojaClientes, hojaLudopatas, hojaProhibidos, hojaRobaStacker, hojaHistorialLudopatas
                    };
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = ExcelHelper.GenerarExcel(sheets);

                    excelName = $"Listado Clientes {salaMaestra.Nombre}.xlsx";
                    MemoryStream memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());

                    mensaje = "Descargando Archivo";
                    respuesta = true;
                } else {
                    mensaje = "No se encontraron registros";
                }
            } catch(Exception exp) {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;

            object resultData = new {
                respuesta,
                excelName,
                mensaje,
                data = base64String
            };
            ContentResult result = new ContentResult {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;
        }

        public class Listado_porMes {
            public String mes { get; set; }
            public String salanombre { get; set; }
            public List<AST_ClienteSala> datames { get; set; }
        }
        public class Reporte {
            public String mes { get; set; }
            public String sala { get; set; }
            public List<AST_ClienteSala> datames { get; set; }
        }
        public class ExcelFile {
            public String sala { get; set; }
            public byte[] excelbyte { get; set; }
        }

        [HttpGet]
        [seguridad(true)]
        public ActionResult GetRepoteExcelSalaPorMes(string fechaini = "2023-01", string fechafin = "2024-12", string salas = "", bool verInfoContacto = false) {
            //       /AsistenciaCliente/ReporteListaCliente?salas=52,20&fechaini=2023-01-01&fechafin=2023-06-30"
            //string fechaIni = "2023-01";
            //string fechaFin = "2024-12";
            DateTime startDate;
            DateTime endDate;
            if(!DateTime.TryParse(fechaini, out startDate)) {
                return Json(new { success = false, message = "fechaini no válida. Debe ser formato YYYY-mm" }, JsonRequestBehavior.AllowGet);
            }
            if(!DateTime.TryParse(fechafin, out endDate)) {
                return Json(new { success = false, message = "fechafin no válida. Debe ser formato YYYY-mm" }, JsonRequestBehavior.AllowGet);
            }
            if(startDate > endDate) {
                return Json(new { success = false, message = "fechaini debe ser menos a fechafin" }, JsonRequestBehavior.AllowGet);
            }
            if(salas == "todas" || salas.Contains("todas")) {
                salas = "";
            }

            endDate = endDate.AddMonths(1).AddDays(-1);
            //DateTime startDate = DateTime.Parse(fechaini);
            //DateTime endDate = DateTime.Parse(fechafin);
            var excelFiles = new List<ExcelFile>();
            var salasarray = new SalaBL().ListadoSalaPorUsuario(Convert.ToInt32(Session["UsuarioID"]));

            if(salas != "") {
                List<int> codSalaList = salas.Split(',').Select(int.Parse).ToList();
                salasarray = salasarray.Where(x => codSalaList.Contains(x.CodSala)).ToList();
            }
            string codSalasAsString = string.Join(",", salasarray.Select(s => s.CodSala.ToString()));
            var monthsArray = GetMonthsBetween(startDate, endDate);

            List<Reporte> reportes = new List<Reporte>();

            List<Listado_porMes> Listado_mes = new List<Listado_porMes>();
            foreach(var sala in salasarray) {
                string strElementos = $" sala.CodSala in({sala.CodSala});";
                var dataporsala = ast_asistenciaClienteSalaBL.GetReporteListaClienteSala(strElementos, startDate, endDate, verInfoContacto);//data sala de todo el rango de fecha
                foreach(var month in monthsArray) {
                    var data_sala_mes = dataporsala.Where(x => x.FechaRegistro >= month.Value.StartDate && x.FechaRegistro < month.Value.EndDate.AddDays(1)).ToList();
                    Listado_mes.Add(new Listado_porMes {
                        salanombre = sala.Nombre,
                        mes = month.Key,
                        datames = data_sala_mes
                    });//2024-01
                }
            }
            foreach(var sala in salasarray) {
                foreach(var month in monthsArray) {
                    var lista_mes = Listado_mes.Where(x => x.salanombre == sala.Nombre && x.mes == month.Key).FirstOrDefault().datames;
                    reportes.Add(new Reporte { sala = sala.Nombre, mes = month.Key, datames = lista_mes });
                }
            }

            bool tienePermisoVerInformacionContactoCliente = seg_PermisoRolBL.GetPermisoRolUsuario((int)Session["rol"], nameof(permisosClienteController.VerInfoContactoCliente)).Count > 0;
            int lastColumn = tienePermisoVerInformacionContactoCliente ? 15 : 13;
            string lastColumnStr = tienePermisoVerInformacionContactoCliente ? "O" : "M";

            foreach(var sala in salasarray) {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excel = new ExcelPackage();

                foreach(var month in monthsArray) {
                    string mes_hoja = month.Key;
                    //var lista = ast_asistenciaClienteSalaBL.GetReporteListaClienteMes(month.Key);
                    var rep_mes = reportes.Where(x => x.mes == month.Key && x.sala == sala.Nombre).FirstOrDefault();
                    var lista = new List<AST_ClienteSala>();
                    if(rep_mes != null) {
                        lista = rep_mes.datames;
                    }

                    if(lista.Count > 0) {
                        var workSheet = excel.Workbook.Worksheets.Add(mes_hoja);
                        workSheet.TabColor = System.Drawing.Color.Black;
                        workSheet.DefaultRowHeight = 12;
                        //Header of table  
                        workSheet.Cells[1, 2].Value = "SALA : ";
                        workSheet.Cells[1, 3].Value = sala.Nombre;
                        workSheet.Cells[1, 6].Value = "Total : " + (lista.Count) + " Registros";

                        workSheet.Row(3).Height = 20;
                        workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet.Row(3).Style.Font.Bold = true;

                        int colIndex = 2;
                        workSheet.Cells[3, colIndex++].Value = "CodSala";
                        workSheet.Cells[3, colIndex++].Value = "Sala";
                        workSheet.Cells[3, colIndex++].Value = "Tipo Documento";
                        workSheet.Cells[3, colIndex++].Value = "Nro. Documento";
                        workSheet.Cells[3, colIndex++].Value = "Cliente";
                        workSheet.Cells[3, colIndex++].Value = "Cant. Dosis";
                        if(tienePermisoVerInformacionContactoCliente) {
                            workSheet.Cells[3, colIndex++].Value = "Celular";
                            workSheet.Cells[3, colIndex++].Value = "Correo";
                        }
                        workSheet.Cells[3, colIndex++].Value = "Notif. WhatsApp";
                        workSheet.Cells[3, colIndex++].Value = "Notif. SMS";
                        workSheet.Cells[3, colIndex++].Value = "Llamada";
                        workSheet.Cells[3, colIndex++].Value = "Notif. Email";
                        workSheet.Cells[3, colIndex++].Value = "F. Nacimiento";
                        workSheet.Cells[3, colIndex].Value = "F. Registro";

                        //Body of table  
                        int recordIndex = 4;
                        int total = lista.Count;

                        foreach(var registro in lista) {
                            colIndex = 2;
                            workSheet.Cells[recordIndex, colIndex++].Value = registro.codSala;
                            workSheet.Cells[recordIndex, colIndex++].Value = registro.NombreSala;
                            workSheet.Cells[recordIndex, colIndex++].Value = registro.TipoDocumento;
                            workSheet.Cells[recordIndex, colIndex++].Value = registro.NroDoc;
                            workSheet.Cells[recordIndex, colIndex++].Value = registro.NombreCliente;
                            workSheet.Cells[recordIndex, colIndex++].Value = registro.cantDosis;
                            if(tienePermisoVerInformacionContactoCliente) {
                                workSheet.Cells[recordIndex, colIndex++].Value = registro.Celular;
                                workSheet.Cells[recordIndex, colIndex++].Value = registro.Mail;
                            }
                            workSheet.Cells[recordIndex, colIndex++].Value = registro.EnviaNotificacionWhatsapp ? "Sí" : "No";
                            workSheet.Cells[recordIndex, colIndex++].Value = registro.EnviaNotificacionSms ? "Sí" : "No";
                            workSheet.Cells[recordIndex, colIndex++].Value = registro.LlamadaCelular ? "Sí" : "No";
                            workSheet.Cells[recordIndex, colIndex++].Value = registro.EnviaNotificacionEmail ? "Sí" : "No";
                            workSheet.Cells[recordIndex, colIndex++].Value = registro.FechaNacimiento.ToString("dd-MM-yyyy");
                            workSheet.Cells[recordIndex, colIndex++].Value = registro.FechaRegistro.ToString("dd-MM-yyyy hh:mm:ss");
                            recordIndex++;
                        }
                        Color colbackground = ColorTranslator.FromHtml("#003268");
                        Color colborder = ColorTranslator.FromHtml("#074B88");

                        workSheet.Cells[$"B3:{lastColumnStr}3"].Style.Font.Bold = true;
                        workSheet.Cells[$"B3:{lastColumnStr}3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[$"B3:{lastColumnStr}3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                        workSheet.Cells[$"B3:{lastColumnStr}3"].Style.Font.Color.SetColor(Color.White);

                        workSheet.Cells[$"B3:{lastColumnStr}3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[$"B3:{lastColumnStr}3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[$"B3:{lastColumnStr}3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[$"B3:{lastColumnStr}3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        workSheet.Cells[$"B3:{lastColumnStr}3"].Style.Border.Top.Color.SetColor(colborder);
                        workSheet.Cells[$"B3:{lastColumnStr}3"].Style.Border.Left.Color.SetColor(colborder);
                        workSheet.Cells[$"B3:{lastColumnStr}3"].Style.Border.Right.Color.SetColor(colborder);
                        workSheet.Cells[$"B3:{lastColumnStr}3"].Style.Border.Bottom.Color.SetColor(colborder);

                        int filaFooter_ = recordIndex;
                        workSheet.Cells[$"B{filaFooter_}:{lastColumnStr}{filaFooter_}"].Merge = true;
                        workSheet.Cells[$"B{filaFooter_}:{lastColumnStr}{filaFooter_}"].Style.Font.Bold = true;
                        workSheet.Cells[$"B{filaFooter_}:{lastColumnStr}{filaFooter_}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[$"B{filaFooter_}:{lastColumnStr}{filaFooter_}"].Style.Fill.BackgroundColor.SetColor(colbackground);
                        workSheet.Cells[$"B{filaFooter_}:{lastColumnStr}{filaFooter_}"].Style.Font.Color.SetColor(Color.White);
                        workSheet.Cells[$"B{filaFooter_}:{lastColumnStr}{filaFooter_}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet.Cells[filaFooter_, 2].Value = "Total : " + (total) + " Registros";

                        workSheet.Cells[3, 2, filaFooter_, lastColumn].AutoFilter = true;
                        workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();

                        workSheet.Column(2).Width = 15;
                        for(int i = 3; i <= lastColumn; i++) {
                            workSheet.Column(i).AutoFit();
                        }
                    } else {
                        var workSheet = excel.Workbook.Worksheets.Add(mes_hoja);
                        workSheet.TabColor = System.Drawing.Color.Black;
                        workSheet.DefaultRowHeight = 12;
                        //Header of table  
                        workSheet.Cells[1, 2].Value = "SALA : ";
                        workSheet.Cells[1, 3].Value = sala.Nombre;
                        workSheet.Cells[1, 6].Value = "Total : " + (lista.Count) + " Registros";

                        workSheet.Row(3).Height = 20;
                        workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet.Row(3).Style.Font.Bold = true;
                        int colIndex = 2;
                        workSheet.Cells[3, colIndex++].Value = "CodSala";
                        workSheet.Cells[3, colIndex++].Value = "Sala";
                        workSheet.Cells[3, colIndex++].Value = "Tipo Documento";
                        workSheet.Cells[3, colIndex++].Value = "Nro. Documento";
                        workSheet.Cells[3, colIndex++].Value = "Cliente";
                        workSheet.Cells[3, colIndex++].Value = "Cant. Dosis";
                        if(tienePermisoVerInformacionContactoCliente) {
                            workSheet.Cells[3, colIndex++].Value = "Celular";
                            workSheet.Cells[3, colIndex++].Value = "Correo";
                        }
                        workSheet.Cells[3, colIndex++].Value = "Notif. WhatsApp";
                        workSheet.Cells[3, colIndex++].Value = "Notif. SMS";
                        workSheet.Cells[3, colIndex++].Value = "Llamada";
                        workSheet.Cells[3, colIndex++].Value = "Notif. Email";
                        workSheet.Cells[3, colIndex++].Value = "F. Nacimiento";
                        workSheet.Cells[3, colIndex++].Value = "F. Registro";
                        //workSheet.Cells[4, 2 , 4 ,11].Value = "No hay datos";
                        workSheet.Cells[4, 2, 4, lastColumn].Merge = true; // Merge the cells in the range (row 4, columns 2 to 11)
                        workSheet.Cells[4, 2].Value = "No hay datos"; // Set the value for the merged cell
                        workSheet.Cells[4, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Optional: Center-align text
                        workSheet.Cells[4, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                        Color colbackground = ColorTranslator.FromHtml("#003268");
                        Color colborder = ColorTranslator.FromHtml("#074B88");
                        workSheet.Cells[$"B3:{lastColumnStr}3"].Style.Font.Bold = true;
                        workSheet.Cells[$"B3:{lastColumnStr}3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[$"B3:{lastColumnStr}3"].Style.Fill.BackgroundColor.SetColor(colbackground);
                        workSheet.Cells[$"B3:{lastColumnStr}3"].Style.Font.Color.SetColor(Color.White);

                        workSheet.Cells[$"B3:{lastColumnStr}3"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[$"B3:{lastColumnStr}3"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[$"B3:{lastColumnStr}3"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[$"B3:{lastColumnStr}3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        workSheet.Cells[$"B3:{lastColumnStr}3"].Style.Border.Top.Color.SetColor(colborder);
                        workSheet.Cells[$"B3:{lastColumnStr}3"].Style.Border.Left.Color.SetColor(colborder);
                        workSheet.Cells[$"B3:{lastColumnStr}3"].Style.Border.Right.Color.SetColor(colborder);
                        workSheet.Cells[$"B3:{lastColumnStr}3"].Style.Border.Bottom.Color.SetColor(colborder);

                        workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();

                        Console.WriteLine("No se encontraron registros", JsonRequestBehavior.AllowGet);
                    }
                    if(excel.Workbook.Worksheets.Count == 0) {
                        var workSheet = excel.Workbook.Worksheets.Add("No hay data");
                    }
                }
                excelFiles.Add(new ExcelFile { sala = sala.Nombre, excelbyte = excel.GetAsByteArray() });
            }

            using(var zipStream = new MemoryStream()) {
                using(var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true)) {
                    for(int i = 0; i < excelFiles.Count; i++) {
                        string nombre_file = excelFiles[i].sala;
                        var fileEntry = archive.CreateEntry($"{nombre_file}_{fechaini.Replace("/", "_")}_{fechafin.Replace("/", "_")}.xlsx");
                        using(var entryStream = fileEntry.Open()) {
                            entryStream.Write(excelFiles[i].excelbyte, 0, excelFiles[i].excelbyte.Length);
                        }
                    }
                }
                string nombre_zip = $@"Reportes_{fechaini.Replace("/", "_")}_{fechafin.Replace("/", "_")}.zip";
                return File(zipStream.ToArray(), "application/zip", nombre_zip);
            }
        }
        static Dictionary<string, (DateTime StartDate, DateTime EndDate)> GetMonthsBetween(DateTime start, DateTime end) {
            var result = new Dictionary<string, (DateTime StartDate, DateTime EndDate)>();
            DateTime current = new DateTime(start.Year, start.Month, 1);

            while(current <= end) {
                string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(current.Month);
                //string key = $"{monthName}{current.Year}";

                DateTime startDate = current; // Start of the month
                DateTime endDate = current.AddMonths(1).AddDays(-1); // End of the month
                string key = current.ToString("yyyy-MM");

                result[key] = (startDate, endDate);

                current = current.AddMonths(1);
            }

            return result;
        }

        [HttpPost]
        public ActionResult GetListadoCLientesSalaExcel(int[] ArraySalaId, DateTime fechaIni, DateTime fechaFin) {
            string mensaje = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<AST_ClienteSala> lista = new List<AST_ClienteSala>();
            int cantElementos = (ArraySalaId == null) ? 0 : ArraySalaId.Length;
            string strElementos = string.Empty;
            List<string> nombresSalas = new List<string>();
            string salasSeleccionadas = "-";
            try {
                if(cantElementos > 0) {
                    for(int i = 0; i < ArraySalaId.Length; i++) {
                        SalaEntidad salat = salaBl.SalaListaIdJson(ArraySalaId[i]);
                        nombresSalas.Add(salat.Nombre);
                    }
                    salasSeleccionadas = string.Join(", ", nombresSalas);
                    strElementos = $" sala.CodSala in('{string.Join("','", ArraySalaId)}');";
                }
                lista = ast_asistenciaClienteSalaBL.GetListadoClienteSala(strElementos, fechaIni, fechaFin);
                if(lista.Count > 0) {
                    #region Clientes
                    List<AST_ClienteSala> clientes = lista.Where(cliente => !cliente.EsLudopata && !cliente.EsProhibido && !cliente.EsRobaStacker).ToList();
                    DataTable dataTableCliente = new DataTable();
                    bool puedeInfoContacto = seg_PermisoRolBL.GetPermisoRolUsuario((int)Session["rol"], nameof(permisosClienteController.VerInfoContactoCliente)).Count > 0;
                    ExcelHelper.AddColumnsClienteSala(dataTableCliente, puedeInfoContacto);
                    ExcelHelper.AddDataClienteSala(dataTableCliente, clientes, puedeInfoContacto);
                    HojaExcelReporteAgrupado hojaClientes = new HojaExcelReporteAgrupado {
                        Data = dataTableCliente,
                        Nombre = "Clientes",
                        Color = "#28A745",
                        MetaData = new List<string> {
                            $"Fecha Generación: {DateTime.Now:dd/MM/yyyy HH:mm}",
                            $"Cantidad de Registros: {clientes.Count}",
                            $"SALA(S): {salasSeleccionadas}"
                        }
                    };
                    #endregion

                    #region Ludopatas
                    List<AST_ClienteSala> ludopatas = lista.Where(cliente => cliente.EsLudopata).ToList();
                    DataTable dataTableLudopatas = new DataTable();
                    ExcelHelper.AddColumnsClienteSala(dataTableLudopatas, false);
                    ExcelHelper.AddDataClienteSala(dataTableLudopatas, ludopatas, false);
                    HojaExcelReporteAgrupado hojaLudopatas = new HojaExcelReporteAgrupado {
                        Data = dataTableLudopatas,
                        Nombre = "Ludópatas",
                        Color = "#FF8C00",
                        MetaData = new List<string> {
                            $"Fecha Generación: {DateTime.Now:dd/MM/yyyy HH:mm}",
                            $"Cantidad de Registros: {ludopatas.Count}",
                            $"SALA(S): {salasSeleccionadas}"
                        }
                    };
                    #endregion

                    #region Prohibidos
                    List<AST_ClienteSala> prohibidos = lista.Where(cliente => cliente.EsProhibido).ToList();
                    DataTable dataTableProhibidos = new DataTable();
                    ExcelHelper.AddColumnsClienteSala(dataTableProhibidos, false);
                    ExcelHelper.AddDataClienteSala(dataTableProhibidos, prohibidos, false);
                    HojaExcelReporteAgrupado hojaProhibidos = new HojaExcelReporteAgrupado {
                        Data = dataTableProhibidos,
                        Nombre = "Prohibidos",
                        Color = "#DC3545",
                        MetaData = new List<string> {
                            $"Fecha Generación: {DateTime.Now:dd/MM/yyyy HH:mm}",
                            $"Cantidad de Registros: {prohibidos.Count}",
                            $"SALA(S): {salasSeleccionadas}"
                        }
                    };
                    #endregion

                    #region RobaStackers
                    List<AST_ClienteSala> robaStacker = lista.Where(cliente => cliente.EsRobaStacker).ToList();
                    DataTable dataTableRobaStacker = new DataTable();
                    ExcelHelper.AddColumnsClienteSala(dataTableRobaStacker, false);
                    ExcelHelper.AddDataClienteSala(dataTableRobaStacker, robaStacker, false);
                    HojaExcelReporteAgrupado hojaRobaStacker = new HojaExcelReporteAgrupado {
                        Data = dataTableRobaStacker,
                        Nombre = "Roba Stackers",
                        Color = "#6F42C1",
                        MetaData = new List<string> {
                            $"Fecha Generación: {DateTime.Now:dd/MM/yyyy HH:mm}",
                            $"Cantidad de Registros: {robaStacker.Count}",
                            $"SALA(S): {salasSeleccionadas}"
                        }
                    };
                    #endregion

                    #region Historial Ludopata
                    List<CAL_HistorialLudopataDto> historialLudopata = historialLudopataBL.ObtenerHistorialLudopata();
                    List<string> dnis = lista.Select(x => x.NroDoc.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
                    List<CAL_HistorialLudopataDto> historialLudopataFiltrado = historialLudopata.Where(x => dnis.Contains(x.NumeroDocumento)).ToList();

                    DataTable dataTableHistorialLudopata = new DataTable();
                    ExcelHelper.AddColumnsHistorialLudopata(dataTableHistorialLudopata);
                    ExcelHelper.AddDataHistorialLudopata(dataTableHistorialLudopata, historialLudopataFiltrado);
                    HojaExcelReporteAgrupado hojaHistorialLudopatas = new HojaExcelReporteAgrupado {
                        Data = dataTableHistorialLudopata,
                        Nombre = "Historial Ludópata",
                        Color = "#007BFF",
                        MetaData = new List<string> {
                            $"Fecha Generación: {DateTime.Now:dd/MM/yyyy HH:mm}",
                            $"Cantidad de Registros: {historialLudopataFiltrado.Count}",
                            $"SALA(S): {salasSeleccionadas}"
                        }
                    };
                    #endregion

                    List<HojaExcelReporteAgrupado> sheets = new List<HojaExcelReporteAgrupado> {
                        hojaClientes, hojaLudopatas, hojaProhibidos, hojaRobaStacker, hojaHistorialLudopatas
                    };
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = ExcelHelper.GenerarExcel(sheets);

                    excelName = $"Listado Clientes del {fechaIni:dd-MM-yyyy} al {fechaFin:dd-MM-yyyy}.xlsx";
                    MemoryStream memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());

                    mensaje = "Descargando Archivo";
                    respuesta = true;
                } else {
                    mensaje = "No se encontraron registros";
                }

            } catch(Exception exp) {
                respuesta = false;
                mensaje = exp.Message + ", Llame Administrador";
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;

            object resultData = new {
                respuesta,
                excelName,
                mensaje,
                data = base64String
            };
            ContentResult result = new ContentResult {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;
        }

        [HttpPost]
        public ActionResult GetListadoClientesNotificacionExcel(List<int> codSalas, DateTime fechaInicio, DateTime fechaFin, int enviaNotificacionWhatsapp, int enviaNotificacionSms, int enviaNotificacionEmail, int llamadaCelular) {
            string mensaje = string.Empty;
            bool respuesta = false;
            string base64String = "";
            string excelName = string.Empty;
            List<AST_ClienteSala> lista = new List<AST_ClienteSala>();
            string strElementos = String.Empty;
            List<string> nombresSalas = new List<string>();
            string salasSeleccionadas = "-";
            try {
                if(codSalas != null && codSalas.Count > 0) {
                    foreach(int id in codSalas) {
                        SalaEntidad sala = salaBl.SalaListaIdJson(id);
                        nombresSalas.Add(sala.Nombre);
                    }
                    salasSeleccionadas = string.Join(", ", nombresSalas);
                    strElementos = $" sala.CodSala in({string.Join(",", codSalas)});";
                }

                string whereQuery = string.Empty;
                if(enviaNotificacionWhatsapp >= 0) {
                    whereQuery += $"AND cliente.EnviaNotificacionWhatsapp = {enviaNotificacionWhatsapp}";
                }

                if(enviaNotificacionSms >= 0) {
                    whereQuery += $" AND cliente.EnviaNotificacionSms = {enviaNotificacionSms}";
                }

                if(enviaNotificacionEmail >= 0) {
                    whereQuery += $" AND cliente.EnviaNotificacionEmail = {enviaNotificacionEmail}";
                }

                if(llamadaCelular >= 0) {
                    whereQuery += $" AND cliente.LlamadaCelular = {enviaNotificacionEmail}";
                }

                lista = ast_clienteSalaBL.GetAllClientesFiltrados(whereQuery, strElementos, fechaInicio, fechaFin);

                if(lista.Count > 0) {
                    #region Clientes
                    List<AST_ClienteSala> clientes = lista.Where(cliente => !cliente.EsLudopata && !cliente.EsProhibido && !cliente.EsRobaStacker).ToList();
                    DataTable dataTableCliente = new DataTable();
                    bool puedeInfoContacto = seg_PermisoRolBL.GetPermisoRolUsuario((int)Session["rol"], nameof(permisosClienteController.VerInfoContactoCliente)).Count > 0;
                    ExcelHelper.AddColumnsClienteSala(dataTableCliente, puedeInfoContacto);
                    ExcelHelper.AddDataClienteSala(dataTableCliente, clientes, puedeInfoContacto);
                    HojaExcelReporteAgrupado hojaClientes = new HojaExcelReporteAgrupado {
                        Data = dataTableCliente,
                        Nombre = "Clientes",
                        Color = "#28A745",
                        MetaData = new List<string> {
                            $"Fecha Generación: {DateTime.Now:dd/MM/yyyy HH:mm}",
                            $"Cantidad de Registros: {clientes.Count}",
                            $"SALA(S): {salasSeleccionadas}"
                        }
                    };
                    #endregion

                    #region Ludopatas
                    List<AST_ClienteSala> ludopatas = lista.Where(cliente => cliente.EsLudopata).ToList();
                    DataTable dataTableLudopatas = new DataTable();
                    ExcelHelper.AddColumnsClienteSala(dataTableLudopatas, false);
                    ExcelHelper.AddDataClienteSala(dataTableLudopatas, ludopatas, false);
                    HojaExcelReporteAgrupado hojaLudopatas = new HojaExcelReporteAgrupado {
                        Data = dataTableLudopatas,
                        Nombre = "Ludópatas",
                        Color = "#FF8C00",
                        MetaData = new List<string> {
                            $"Fecha Generación: {DateTime.Now:dd/MM/yyyy HH:mm}",
                            $"Cantidad de Registros: {ludopatas.Count}",
                            $"SALA(S): {salasSeleccionadas}"
                        }
                    };
                    #endregion

                    #region Prohibidos
                    List<AST_ClienteSala> prohibidos = lista.Where(cliente => cliente.EsProhibido).ToList();
                    DataTable dataTableProhibidos = new DataTable();
                    ExcelHelper.AddColumnsClienteSala(dataTableProhibidos, false);
                    ExcelHelper.AddDataClienteSala(dataTableProhibidos, prohibidos, false);
                    HojaExcelReporteAgrupado hojaProhibidos = new HojaExcelReporteAgrupado {
                        Data = dataTableProhibidos,
                        Nombre = "Prohibidos",
                        Color = "#DC3545",
                        MetaData = new List<string> {
                            $"Fecha Generación: {DateTime.Now:dd/MM/yyyy HH:mm}",
                            $"Cantidad de Registros: {prohibidos.Count}",
                            $"SALA(S): {salasSeleccionadas}"
                        }
                    };
                    #endregion

                    #region RobaStackers
                    List<AST_ClienteSala> robaStacker = lista.Where(cliente => cliente.EsRobaStacker).ToList();
                    DataTable dataTableRobaStacker = new DataTable();
                    ExcelHelper.AddColumnsClienteSala(dataTableRobaStacker, false);
                    ExcelHelper.AddDataClienteSala(dataTableRobaStacker, robaStacker, false);
                    HojaExcelReporteAgrupado hojaRobaStacker = new HojaExcelReporteAgrupado {
                        Data = dataTableRobaStacker,
                        Nombre = "Roba Stackers",
                        Color = "#6F42C1",
                        MetaData = new List<string> {
                            $"Fecha Generación: {DateTime.Now:dd/MM/yyyy HH:mm}",
                            $"Cantidad de Registros: {robaStacker.Count}",
                            $"SALA(S): {salasSeleccionadas}"
                        }
                    };
                    #endregion

                    #region Historial Ludopata
                    List<CAL_HistorialLudopataDto> historialLudopata = historialLudopataBL.ObtenerHistorialLudopata();
                    List<string> dnis = lista.Select(x => x.NroDoc.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
                    List<CAL_HistorialLudopataDto> historialLudopataFiltrado = historialLudopata.Where(x => dnis.Contains(x.NumeroDocumento)).ToList();

                    DataTable dataTableHistorialLudopata = new DataTable();
                    ExcelHelper.AddColumnsHistorialLudopata(dataTableHistorialLudopata);
                    ExcelHelper.AddDataHistorialLudopata(dataTableHistorialLudopata, historialLudopataFiltrado);
                    HojaExcelReporteAgrupado hojaHistorialLudopatas = new HojaExcelReporteAgrupado {
                        Data = dataTableHistorialLudopata,
                        Nombre = "Historial Ludópata",
                        Color = "#007BFF",
                        MetaData = new List<string> {
                            $"Fecha Generación: {DateTime.Now:dd/MM/yyyy HH:mm}",
                            $"Cantidad de Registros: {historialLudopataFiltrado.Count}",
                            $"SALA(S): {salasSeleccionadas}"
                        }
                    };
                    #endregion

                    List<HojaExcelReporteAgrupado> sheets = new List<HojaExcelReporteAgrupado> {
                        hojaClientes, hojaLudopatas, hojaProhibidos, hojaRobaStacker, hojaHistorialLudopatas
                    };
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excel = ExcelHelper.GenerarExcel(sheets);

                    excelName = $"Reporte de notificaciones de clientes del {fechaInicio:dd-MM-yyyy} al {fechaFin:dd-MM-yyyy}.xlsx";
                    MemoryStream memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    base64String = Convert.ToBase64String(memoryStream.ToArray());

                    mensaje = "Descargando Archivo";
                    respuesta = true;
                } else {
                    mensaje = "No se encontraron registros";
                }
            } catch(Exception ex) {
                respuesta = false;
                mensaje = $"{ex.Message}, Llame al administrador";
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;

            object resultData = new {
                respuesta,
                excelName,
                mensaje,
                data = base64String
            };

            ContentResult result = new ContentResult {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult GetDataWidgets() {
            var totalClientes = ast_ClienteBL.GetTotalClientes();
            var totalSalasActivas = salaBl.GetTotalSalasActivas();
            var totalSalas = salaBL.GetTotalSalas();
            object objRespuesta = new {
                totalClientes,
                totalSalas,
                totalSalasActivas
            };
            return Json(objRespuesta);
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListaCumpleanios(int CodSala = 0) {
            var result = ast_ClienteBL.GetListaCumpleanios(CodSala);
            return Json(new { result });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetTotalSalasActivas() {
            var result = salaBl.GetTotalSalasActivas();
            return Json(new { result });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoSalasYTotalClientes() {
            var result = salaBl.GetListadoSalasYTotalClientes();
            return Json(new { result });
        }

        //public AST_ClienteEntidad API_RENIEC(string dni) {
        //    AST_ClienteEntidad cliente = new AST_ClienteEntidad();
        //    dynamic item = new DynamicDictionary();
        //    #region apiperu

        //    string uri = "https://apiperu.dev/api/dni/" + dni;
        //    var clientApi = new RestClient(uri);
        //    var requestApi = new RestRequest(Method.GET);
        //    requestApi.AddHeader("Accept", "application/json");
        //    requestApi.AddHeader("Authorization", "Bearer " + "d2a43838fa0ba5f9f3c891d801b94cdf86839eded828bf85d6dbfdbf8b9cef19");
        //    try {
        //        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //        IRestResponse responseApi = clientApi.Execute(requestApi);
        //        dynamic oDataApiReal = JsonConvert.DeserializeObject(responseApi.Content);
        //        dynamic oDataApi = oDataApiReal.data;
        //        if(oDataApi != null) {
        //            if(oDataApi.numero != null) {
        //                cliente.Nombre = oDataApi.nombres;
        //                cliente.ApelPat = oDataApi.apellido_paterno;
        //                cliente.ApelMat = oDataApi.apellido_materno;
        //                cliente.NroDoc = oDataApi.numero;
        //                cliente.TipoDocumentoId = 1;
        //                return cliente;
        //            }

        //        }
        //    } catch(Exception) {
        //    }
        //    #endregion
        //    #region apiperu
        //    //string tokenApiPeru = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.MTcxMg.obd3IEM2Zim6YPPFtMVok7gs5QbzYnY2BR9T9RPGXdw";
        //    string urlApiPeru = "https://quertium.com/api/v1/reniec/dni/" + dni + "?token=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.MTcxMg.obd3IEM2Zim6YPPFtMVok7gs5QbzYnY2BR9T9RPGXdw";
        //    var clientApiperu = new RestClient(urlApiPeru);
        //    var requestApiPeru = new RestRequest(Method.GET);
        //    //requestApiPeru.AddHeader("Accept", "application/json");
        //    try {
        //        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //        IRestResponse responseApiPeru = clientApiperu.Execute(requestApiPeru);
        //        dynamic oDataApiPeru = JsonConvert.DeserializeObject(responseApiPeru.Content);
        //        if(oDataApiPeru != null) {
        //            if(oDataApiPeru.apellidoPaterno != null) {

        //                cliente.NroDoc = dni;
        //                cliente.Nombre = oDataApiPeru.primerNombre + " " + oDataApiPeru.segundoNombre;
        //                cliente.ApelPat = oDataApiPeru.apellidoPaterno;
        //                cliente.ApelMat = oDataApiPeru.apellidoMaterno;
        //                cliente.TipoDocumentoId = 1;
        //                return cliente;
        //            }
        //        }
        //    } catch(Exception) {

        //    }
        //    #endregion
        //    //
        //    #region ConsultaPE
        //    string vtoken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImp0aSI6IjJkY2Q4OGU0MTNmMmFhNzc4MjcxNzVmYThiNzIwMmU2ZWVkM2NjNGM1MmEyYzJkZDg4Mjg3ZTJhY2E2YTM5NDlkM2Q2NmIzODhlZGJlZTc5In0.eyJhdWQiOiIxIiwianRpIjoiMmRjZDg4ZTQxM2YyYWE3NzgyNzE3NWZhOGI3MjAyZTZlZWQzY2M0YzUyYTJjMmRkODgyODdlMmFjYTZhMzk0OWQzZDY2YjM4OGVkYmVlNzkiLCJpYXQiOjE2MDcwMTAxNjgsIm5iZiI6MTYwNzAxMDE2OCwiZXhwIjoxNjM4NTQ2MTY4LCJzdWIiOiI2NzM1Iiwic2NvcGVzIjpbInVzZS1yZW5pZWMiXX0.jqVqCyr3pFkOIPvAK1rFfo_3aBTKKnv9P0Lbb-kJJ0_CiY-onVBUKjJa6d3xan8gokdhGzHOTC60zjuQRyk1AXb86-Fze8mE8ZRnfKdGokaQGSHAFj6xZixISQbKAhr86D796iTBXB-DeKsOpAQg3CrjiafgmT4eG4PgFNFv-6IkOfl_II6VYB0Lzh7Nvm7m1WQjLz5njk5ANsQu9M-HypFjAFnbHfw9fVtz_3Gg-oRr5xyggcOwI2MXM5H87vc7k2dHyGbmGDA6QdfNA9-NYNTGEzcHl-_Rn2wdV2NUkAhHICJpCVVxirqpyaxFqT6VtJ6oabzq9DHpZZi520im6zXSdmjKZfW_hYLnwFo2ml5AGffJbU4Lzi5zmEYMQwwKLFq9vyvMf8mxKxCMpIqz4Hidr-3mBFOr8ECGVQtCWqL4jSgp1rCRJf1UHXkDSS-wd4gB-tpLoNNKGPcnKIedO0xVKTXwt5Bgl5xnAKZoWFlQzaU-8L3zyB7Z2J3xYJnGnuprH5buN5kmdlwuhXUSGQK8L20eYDAxrF8PGTAtOvYV9EtoX4MPOVpdkiwytuuMe2TxucNR3o7nzgOO-4wIZu-7wFDlPknqxyR4WBpw_M6oK_6_neEhx7g5J76vX3E7JtSFtztQbpDTtl4-gVHXQr81DPjLwlN1QeSHOdwTjDs";
        //    string url = "https://consulta.pe/api/reniec/dni";
        //    var client = new RestClient(url);
        //    var request = new RestRequest(Method.POST);
        //    request.AddHeader("Accept", "application/json");
        //    request.AddHeader("Authorization", "Bearer " + vtoken);
        //    request.AddParameter("dni", dni);
        //    try {
        //        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //        IRestResponse response = client.Execute(request);
        //        dynamic oData = JsonConvert.DeserializeObject(response.Content);
        //        if(oData != null) {
        //            if(oData.dni != null) {

        //                cliente.NroDoc = dni;
        //                cliente.Nombre = oData.nombres;
        //                cliente.ApelPat = oData.apellido_paterno;
        //                cliente.ApelMat = oData.apellido_materno;
        //                cliente.TipoDocumentoId = 1;
        //                return cliente;
        //            }
        //        }
        //    } catch(Exception) {

        //    }
        //    #endregion

        //    #region APISPERU
        //    try {
        //        //var client = new RestClient("https://dni.optimizeperu.com/api/persons/" + NroDocumento);
        //        var clientAPis = new RestClient("https://dniruc.apisperu.com/api/v1/dni/" + dni + "?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJlbWFpbCI6ImQuY2FuY2hhcmlyQGdtYWlsLmNvbSJ9.SLYKVIvi-hXBARp_c0vZLErlgDshezI0ltiETdywmaY");

        //        var requestApis = new RestRequest(Method.GET);
        //        requestApis.AddHeader("Accept", "application/json");

        //        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //        IRestResponse response = client.Execute(request);
        //        dynamic oData = JsonConvert.DeserializeObject(response.Content);
        //        if(oData != null) {
        //            //cliente.ClienteNombre = oData.name;
        //            //cliente.ClienteApelPat = oData.first_name;
        //            //cliente.ClienteApelMat = oData.last_name;
        //            //cliente.ClienteNroDoc = oData.dni;
        //            if(oData.dni != null) {
        //                cliente.Nombre = oData.nombres;
        //                cliente.ApelPat = oData.apellidoPaterno;
        //                cliente.ApelMat = oData.apellidoMaterno;
        //                cliente.NroDoc = oData.dni;
        //                cliente.TipoDocumentoId = 1;
        //            }
        //        }
        //    } catch(Exception) {
        //    }
        //    #endregion
        //    return cliente;
        //}
        public async Task<AST_ClienteEntidad> API_RENIEC(string dni) {
            AST_ClienteEntidad cliente = new AST_ClienteEntidad();
            ApiReniec _apiReniec = new ApiReniec();
            var itemResponse = await _apiReniec.Busqueda(dni);
            cliente.Nombre = itemResponse.Nombre;
            cliente.ApelPat = itemResponse.ApellidoPaterno;
            cliente.ApelMat = itemResponse.ApellidoMaterno;
            cliente.NombreCompleto = itemResponse.NombreCompleto;
            cliente.TipoDocumentoId = 1;
            cliente.NroDoc = itemResponse.DNI;
            return cliente;
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetTotalClientesPorAnio(int anio) {
            return Json(ast_ClienteBL.GetTotalClientesPorAnio(anio));
        }
        [seguridad(false)]
        [HttpGet]
        public ActionResult ListarClienteMigracion(int Id) {
            List<AST_ClienteMigracion> data = new List<AST_ClienteMigracion>();
            try {
                data = ast_ClienteBL.ListarClienteMigracion(Id);

            } catch(Exception) {
                data = new List<AST_ClienteMigracion>();
            }
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var result = new ContentResult {
                Content = serializer.Serialize(data),
                ContentType = "application/json; charset=utf-8"
            };
            return result;
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult GuardarClienteCampaniaWhatsApp(AST_ClienteEntidad cliente) {
            string displayMessage;
            bool success = false;
            bool clientExists = false;
            string tipoRegistro = "CAMPAÑA WSP";
            try {
                if(!Constants.UrlVerificationClients.ContainsKey(cliente.SalaId) && !Constants.SalasNoVerificaCliente.Contains(cliente.SalaId)) {
                    ResponseEntidad<ClienteVerificacionResponse> response = ast_ClienteBL.GetExistenciaDeClienteParaCampaniaWhatsApp(cliente.NroDoc, cliente.TipoDocumentoId, cliente.Celular1, cliente.SalaId);
                    displayMessage = response.displayMessage;
                    success = response.success;

                    if(response.data.clientExist) {
                        success = response.success;
                        clientExists = response.data.clientExist;
                        displayMessage = response.displayMessage;
                        return Json(new { success, clientExists, displayMessage });
                    }
                }

                AST_ClienteEntidad verificacionCliente = ast_ClienteBL.GetClientexNroDoc(cliente.NroDoc);
                int idCliente = 0;
                if(verificacionCliente.Existe()) {
                    List<DateTime> fechasInvalidas = new List<DateTime> { new DateTime(1753, 1, 1), new DateTime(1, 1, 1) };
                    verificacionCliente.FechaNacimiento = verificacionCliente.EsMayorDeEdad() && !fechasInvalidas.Contains(verificacionCliente.FechaNacimiento.Date) ? verificacionCliente.FechaNacimiento : cliente.FechaNacimiento;
                    verificacionCliente.Celular1 = cliente.Celular1;
                    verificacionCliente.CodigoPais = cliente.CodigoPais;
                    verificacionCliente.Genero = string.IsNullOrEmpty(verificacionCliente.Genero) ? cliente.Genero : verificacionCliente.Genero;
                    verificacionCliente.TipoDocumentoId = verificacionCliente.TipoDocumentoId == 0 ? cliente.TipoDocumentoId : verificacionCliente.TipoDocumentoId;
                    verificacionCliente.Ciudadano = AST_ClienteEntidad.Ubigeo_Pais_Id.Equals(cliente.PaisId);
                    verificacionCliente.PaisId = cliente.PaisId;
                    ast_ClienteBL.EditarCliente(verificacionCliente);
                    idCliente = verificacionCliente.Id;
                } else {
                    cliente.Nombre = cliente.Nombre.Trim().ToUpper();
                    cliente.ApelPat = cliente.ApelPat.Trim().ToUpper();
                    cliente.ApelMat = cliente.ApelMat.Trim().ToUpper();
                    cliente.NombreCompleto = cliente.Nombre + " " + cliente.ApelPat + " " + cliente.ApelMat;
                    cliente.Ciudadano = AST_ClienteEntidad.Ubigeo_Pais_Id.Equals(cliente.PaisId);
                    cliente.Estado = "P";
                    cliente.FechaRegistro = DateTime.Now;
                    cliente.TipoRegistro = tipoRegistro;
                    idCliente = ast_ClienteBL.GuardarClienteCampaniaWhatsApp(cliente);
                }

                AST_ClienteSalaEntidad clienteSalaVerificacion = ast_clienteSalaBL.GetClienteSalaID(idCliente, cliente.SalaId);

                if(clienteSalaVerificacion.Existe()) {
                    success = true;
                } else {
                    AST_ClienteSalaEntidad clienteSala = new AST_ClienteSalaEntidad() {
                        ClienteId = idCliente,
                        SalaId = cliente.SalaId,
                        TipoRegistro = tipoRegistro,
                        EnviaNotificacionWhatsapp = cliente.EnviaNotificacionWhatsapp,
                        EnviaNotificacionSms = cliente.EnviaNotificacionSms,
                        EnviaNotificacionEmail = cliente.EnviaNotificacionEmail,
                        LlamadaCelular = cliente.LlamadaCelular,
                    };
                    success = ast_clienteSalaBL.GuardarClienteSala(clienteSala);
                }

                displayMessage = success ? "Cliente registrado correctamente." : "Ocurrió un problema al intentar registrar cliente.";

            } catch(Exception ex) {
                displayMessage = ex.Message;
            }

            return Json(new { success, clientExists, displayMessage });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult GuardarClienteSorteos(List<AST_ClienteEntidad> clientes) {
            List<int> insertados = new List<int>();
            string result = string.Empty;
            try {
                foreach(var cliente in clientes) {
                    int clienteid = 0;
                    int clienteRetornar = cliente.Id; //retorno este id a la sala para editar estado de envio
                    cliente.Id = 0;
                    var existe = ast_ClienteBL.GetClientexNroyTipoDoc(cliente.TipoDocumentoId, cliente.NroDoc.Trim());
                    if(existe.Id == 0) {
                        clienteid = ast_ClienteBL.GuardarClienteSorteosSala(cliente);
                        //verificar si existe en ast_clientesala
                        var existeClienteSala = ast_clienteSalaBL.GetListadoClienteSala(existe.Id).Where(x => x.SalaId == cliente.SalaId);
                        if(existeClienteSala.Count() > 0) {
                            continue;
                        }
                        //guardar en ast_clientesala
                        var clienteSalaIns = new AST_ClienteSalaEntidad() {
                            ClienteId = existe.Id,
                            SalaId = cliente.SalaId,
                            FechaRegistro = cliente.FechaRegistro,
                            TipoRegistro = "CAMPAÑA"
                        };
                        ast_clienteSalaBL.GuardarClienteSalaConFecha(clienteSalaIns);
                    } else {
                        clienteid = existe.Id;
                    }
                    if(clienteid > 0) {
                        insertados.Add(clienteRetornar);
                    }


                }
                result = String.Join(",", insertados);
            } catch(Exception) {
                return Json(new { respuesta = string.Empty });
            }
            return Json(new { respuesta = result });
        }

        [seguridad(false)]
        [HttpPost]
        public async Task<ActionResult> IngresarClienteSala(ClienteSalaEntidad[] clientes) {
            string tipoRegistro = "CAJA";
            string correoInvalido = "CORREO@PROVEEDOR.COM";

            int cantidadSinNumeroDocumento = 0;
            int cantidadExisteActualiza = 0;
            int cantidadExisteNoActualiza = 0;
            int cantidadExisteInsertaSalaCliente = 0;
            int cantidadExisteNoInsertaSalaCliente = 0;
            int cantidadNoExisteInserta = 0;
            int cantidadNoExisteNoInserta = 0;
            int cantidadNoExisteInsertaSalaCliente = 0;
            int cantidadNoExisteNoInsertaSalaCliente = 0;

            foreach(ClienteSalaEntidad clienteMigracion in clientes) {
                try {
                    if(string.IsNullOrEmpty(clienteMigracion.Dni)) {
                        cantidadSinNumeroDocumento++;
                        continue;
                    }

                    #region Manejo de nulos
                    clienteMigracion.CodClie = ManejoNulos.ManageNullInteger(clienteMigracion.CodClie);
                    clienteMigracion.NomCli = ManejoNulos.ManageNullStr(clienteMigracion.NomCli);
                    clienteMigracion.Apeclie = ManejoNulos.ManageNullStr(clienteMigracion.Apeclie);
                    clienteMigracion.FechaNacimiento = ManejoNulos.ManageNullDate(clienteMigracion.FechaNacimiento);
                    clienteMigracion.FechaRegistro = clienteMigracion.FechaRegistro != null ? clienteMigracion.FechaRegistro : DateTime.Now;
                    clienteMigracion.Nacionalidad = ManejoNulos.ManageNullStr(clienteMigracion.Nacionalidad);
                    clienteMigracion.Sexo = ManejoNulos.ManageNullStr(clienteMigracion.Sexo);
                    clienteMigracion.Dni = ManejoNulos.ManageNullStr(clienteMigracion.Dni);
                    clienteMigracion.CodSala = ManejoNulos.ManageNullInteger(clienteMigracion.CodSala);
                    clienteMigracion.CodEmpresa = ManejoNulos.ManageNullInteger(clienteMigracion.CodEmpresa);
                    clienteMigracion.Telefono = ManejoNulos.ManageNullStr(clienteMigracion.Telefono);
                    clienteMigracion.Correo = ManejoNulos.ManageNullStr(clienteMigracion.Correo);
                    clienteMigracion.EnviaNotificacionSms = ManejoNulos.ManegeNullBool(clienteMigracion.EnviaNotificacionSms);
                    clienteMigracion.EnviaNotificacionEmail = ManejoNulos.ManegeNullBool(clienteMigracion.EnviaNotificacionEmail);
                    clienteMigracion.EnviaNotificacionWhatsapp = ManejoNulos.ManegeNullBool(clienteMigracion.EnviaNotificacionWhatsapp);
                    clienteMigracion.LlamadaCelular = ManejoNulos.ManegeNullBool(clienteMigracion.LlamadaCelular);
                    #endregion

                    clienteMigracion.Dni = clienteMigracion.Dni.Trim();
                    clienteMigracion.Correo = clienteMigracion.Correo.Trim();
                    clienteMigracion.Telefono = clienteMigracion.Telefono.Trim();

                    AST_ClienteEntidad clienteLocal = ast_ClienteBL.GetClientexNroDoc(clienteMigracion.Dni);

                    if(clienteLocal.Existe()) {
                        bool actualizaCelular = string.IsNullOrEmpty(clienteLocal.Celular1) && !string.IsNullOrEmpty(clienteMigracion.Telefono) && clienteMigracion.Telefono.Length >= 9 && clienteMigracion.Telefono.Length <= 15;
                        bool actualizaCorreo = string.IsNullOrEmpty(clienteLocal.Mail) && !string.IsNullOrEmpty(clienteMigracion.Correo) && !clienteMigracion.Correo.Equals(correoInvalido, StringComparison.OrdinalIgnoreCase) && ValidationsHelper.IsValidEmail(clienteMigracion.Correo);

                        clienteLocal.Mail = actualizaCorreo ? clienteMigracion.Correo : clienteLocal.Mail;
                        clienteLocal.Celular1 = actualizaCelular ? clienteMigracion.Telefono : clienteLocal.Celular1;

                        bool actualizoClienteLocal = actualizaCelular || actualizaCorreo ? ast_ClienteBL.EditarCliente(clienteLocal) : false;

                        if(actualizoClienteLocal) {
                            cantidadExisteActualiza++;
                        } else {
                            cantidadExisteNoActualiza++;
                        }

                        AST_ClienteSalaEntidad clienteSala = ast_clienteSalaBL.GetClienteSalaID(clienteLocal.Id, clienteMigracion.CodSala);

                        if(!clienteSala.Existe()) {
                            AST_ClienteSalaEntidad astClienteSala = new AST_ClienteSalaEntidad() {
                                ClienteId = clienteLocal.Id,
                                SalaId = clienteMigracion.CodSala,
                                FechaRegistro = clienteMigracion.FechaRegistro,
                                TipoRegistro = tipoRegistro,
                                EnviaNotificacionSms = clienteMigracion.EnviaNotificacionSms,
                                EnviaNotificacionEmail = clienteMigracion.EnviaNotificacionEmail,
                                EnviaNotificacionWhatsapp = clienteMigracion.EnviaNotificacionWhatsapp,
                                LlamadaCelular = clienteMigracion.LlamadaCelular
                            };
                            bool ra = ast_clienteSalaBL.GuardarClienteSalaConFecha(astClienteSala);
                            if(ra) {
                                cantidadExisteInsertaSalaCliente++;
                            } else {
                                cantidadExisteNoInsertaSalaCliente++;
                            }
                        } else {
                            cantidadExisteNoInsertaSalaCliente++;
                        }
                    } else {
                        AST_ClienteEntidad clienteNuevo = new AST_ClienteEntidad();

                        if(clienteMigracion.Dni.Length == 8) {
                            ApiReniec _apiReniec = new ApiReniec();
                            ApiReniecResponse busquedaReniec = await _apiReniec.Busqueda(clienteMigracion.Dni);

                            if(busquedaReniec.Respuesta) {
                                clienteNuevo.Nombre = busquedaReniec.Nombre;
                                clienteNuevo.ApelPat = busquedaReniec.ApellidoPaterno;
                                clienteNuevo.ApelMat = busquedaReniec.ApellidoMaterno;
                                clienteNuevo.NombreCompleto = busquedaReniec.NombreCompleto;
                            } else {
                                clienteNuevo.Nombre = clienteMigracion.NomCli;
                                List<string> apellidos = clienteMigracion.Apeclie.Split(' ').ToList();
                                if(apellidos.Count == 2) {
                                    clienteNuevo.ApelPat = apellidos.ElementAt(0);
                                    clienteNuevo.ApelMat = apellidos.ElementAt(1);
                                } else {
                                    clienteNuevo.ApelPat = clienteMigracion.Apeclie;
                                }
                                clienteNuevo.NombreCompleto = $"{clienteMigracion.NomCli} {clienteMigracion.Apeclie}";
                            }

                            clienteNuevo.TipoDocumentoId = 1;
                            clienteNuevo.PaisId = AST_ClienteEntidad.Ubigeo_Pais_Id;
                        } else {
                            //dejar nulls
                            clienteNuevo.Nombre = clienteMigracion.NomCli;
                            clienteNuevo.ApelPat = clienteMigracion.Apeclie;
                            clienteNuevo.NombreCompleto = clienteMigracion.NomCli + " " + clienteMigracion.Apeclie;
                            clienteNuevo.TipoDocumentoId = 3;
                        }

                        clienteNuevo.Celular1 = clienteMigracion.Telefono.Length >= 9 && clienteMigracion.Telefono.Length <= 15 ? clienteMigracion.Telefono : string.Empty;
                        clienteNuevo.Mail = ValidationsHelper.IsValidEmail(clienteMigracion.Correo) && !clienteMigracion.Correo.Equals(correoInvalido, StringComparison.OrdinalIgnoreCase) ? clienteMigracion.Correo : string.Empty;
                        clienteNuevo.Genero = clienteMigracion.Sexo;
                        clienteNuevo.NroDoc = clienteMigracion.Dni;
                        clienteNuevo.FechaNacimiento = clienteMigracion.FechaNacimiento;
                        clienteNuevo.Estado = "P";
                        clienteNuevo.TipoRegistro = tipoRegistro;
                        clienteNuevo.FechaRegistro = clienteMigracion.FechaRegistro;
                        clienteNuevo.SalaId = clienteMigracion.CodSala;

                        int idCLienteRegistrado = ast_ClienteBL.GuardarCliente(clienteNuevo);
                        if(idCLienteRegistrado > 0) {
                            cantidadNoExisteInserta++;
                            AST_ClienteSalaEntidad astClienteSala = new AST_ClienteSalaEntidad {
                                ClienteId = idCLienteRegistrado,
                                SalaId = clienteMigracion.CodSala,
                                FechaRegistro = clienteMigracion.FechaRegistro,
                                TipoRegistro = tipoRegistro,
                                EnviaNotificacionSms = clienteMigracion.EnviaNotificacionSms,
                                EnviaNotificacionEmail = clienteMigracion.EnviaNotificacionEmail,
                                EnviaNotificacionWhatsapp = clienteMigracion.EnviaNotificacionWhatsapp,
                                LlamadaCelular = clienteMigracion.LlamadaCelular
                            };

                            bool insertaClienteSala = ast_clienteSalaBL.GuardarClienteSalaConFecha(astClienteSala);
                            if(insertaClienteSala) {
                                cantidadNoExisteInsertaSalaCliente++;
                            } else {
                                cantidadNoExisteNoInsertaSalaCliente++;
                            }
                        } else {
                            cantidadNoExisteNoInserta++;
                        }
                    }
                } catch {
                }
            }

            var data = new {
                cantidadSinNumeroDocumento,
                cantidadExisteActualiza,
                cantidadExisteNoActualiza,
                cantidadExisteInsertaSalaCliente,
                cantidadExisteNoInsertaSalaCliente,
                cantidadNoExisteInserta,
                cantidadNoExisteNoInserta,
                cantidadNoExisteInsertaSalaCliente,
                cantidadNoExisteNoInsertaSalaCliente
            };
            string result = JsonConvert.SerializeObject(data);
            return Json(new { displayMessage = result });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult GuardarClienteGanadorERP(AST_ClienteERP clienteERP, int codSala) {
            List<int> insertados = new List<int>();
            bool respuesta = false;
            try {
                //var existe = ast_ClienteBL.GetClientexNroDoc(cliente.NroDoc.Trim());
                var clienteIAS = ast_ClienteBL.GetListaClientesxNroDoc(clienteERP.DOI.Trim()).FirstOrDefault() ?? new AST_ClienteEntidad();

                if(!clienteIAS.Existe()) {
                    clienteIAS.NroDoc = clienteERP.DOI;
                    clienteIAS.Nombre = clienteERP.Nombre;
                    clienteIAS.ApelPat = clienteERP.ApellidoPaterno;
                    clienteIAS.ApelMat = clienteERP.ApellidoMaterno;
                    clienteIAS.Genero = clienteERP.Genero;
                    clienteIAS.Celular1 = clienteERP.Movil;
                    clienteIAS.Celular2 = clienteERP.Telefono;
                    clienteIAS.Mail = clienteERP.MailPersonal;
                    clienteIAS.FechaNacimiento = clienteERP.FechaNacimiento;
                    clienteIAS.TipoDocumentoId = clienteERP.CodTipoDOI;
                    clienteIAS.UbigeoProcedenciaId = clienteERP.CodUbigeo;
                    clienteIAS.NombreCompleto = clienteERP.Nombre.Trim() + " " + clienteERP.ApellidoPaterno.Trim() + " " + clienteERP.ApellidoMaterno.Trim();
                    clienteIAS.FechaRegistro = DateTime.Now;
                    clienteIAS.Estado = "A";
                    clienteIAS.TipoRegistro = "SORTEOSERP";
                    clienteIAS.usuario_reg = 0;
                    clienteIAS.SalaId = codSala;
                    int clienteid = ast_ClienteBL.GuardarCliente(clienteIAS);
                    if(clienteid > 0)
                        respuesta = true;
                } else {
                    clienteIAS.Nombre = clienteERP.Nombre;
                    clienteIAS.ApelPat = clienteERP.ApellidoPaterno;
                    clienteIAS.ApelMat = clienteERP.ApellidoMaterno;
                    clienteIAS.Genero = clienteERP.Genero;
                    clienteIAS.Celular1 = clienteERP.Movil;
                    clienteIAS.Celular2 = clienteERP.Telefono;
                    clienteIAS.Mail = clienteERP.MailPersonal;
                    clienteIAS.FechaNacimiento = clienteERP.FechaNacimiento;
                    clienteIAS.TipoDocumentoId = clienteERP.CodTipoDOI;
                    clienteIAS.UbigeoProcedenciaId = clienteERP.CodUbigeo;
                    clienteIAS.NombreCompleto = clienteERP.Nombre.Trim() + " " + clienteERP.ApellidoPaterno.Trim() + " " + clienteERP.ApellidoMaterno.Trim();
                    bool editado = ast_ClienteBL.EditarCliente(clienteIAS);
                    if(editado)
                        respuesta = true;
                }

                AST_ClienteSalaEntidad clienteSala = ast_clienteSalaBL.GetClienteSalaID(clienteIAS.Id, codSala);
                if(!clienteSala.Existe()) {
                    AST_ClienteSalaEntidad astClienteSala = new AST_ClienteSalaEntidad {
                        ClienteId = clienteIAS.Id,
                        SalaId = codSala,
                        FechaRegistro = clienteIAS.FechaRegistro,
                        TipoRegistro = clienteIAS.TipoRegistro
                    };

                    ast_clienteSalaBL.GuardarClienteSalaConFecha(astClienteSala);
                }
            } catch(Exception) {
                return Json(new { respuesta = false });
            }
            return Json(new { respuesta });
        }

        [HttpPost]
        public ActionResult ActualizarEnvioNotificacionCliente(int idCliente, int codSala, AST_TipoNotificacionCliente tipoNotificacion, bool enviaNotificacion) {
            string displayMessage = string.Empty;
            bool success = false;
            try {
                AST_ClienteSalaEntidad clienteSala = ast_clienteSalaBL.GetClienteSalaID(idCliente, codSala);

                if(clienteSala.Existe()) {
                    success = ast_clienteSalaBL.ActualizarEnvioNotificacion(idCliente, codSala, tipoNotificacion, enviaNotificacion);
                } else {
                    AST_ClienteEntidad cliente = ast_ClienteBL.GetClienteID(idCliente);
                    clienteSala.ClienteId = idCliente;
                    clienteSala.SalaId = codSala;
                    clienteSala.TipoRegistro = cliente.TipoRegistro;
                    clienteSala.FechaRegistro = cliente.FechaRegistro;
                    clienteSala.EnviaNotificacionWhatsapp = tipoNotificacion != AST_TipoNotificacionCliente.Whatsapp || enviaNotificacion;
                    clienteSala.EnviaNotificacionSms = tipoNotificacion != AST_TipoNotificacionCliente.Sms || enviaNotificacion;
                    clienteSala.EnviaNotificacionEmail = tipoNotificacion != AST_TipoNotificacionCliente.Email || enviaNotificacion;
                    clienteSala.LlamadaCelular = tipoNotificacion != AST_TipoNotificacionCliente.Llamada || enviaNotificacion;
                    success = ast_clienteSalaBL.GuardarClienteSalaConFecha(clienteSala);
                }

                displayMessage = success ? "Ajuste de notificaciones actualizado correctamente." : "No se pudo cambiar el ajuste de notificaciones";
            } catch(Exception ex) {
                success = false;
                displayMessage = $"{ex.Message}, llame al administrador";
            }

            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public ActionResult ObtenerClientesParaEnvioNotificacion(AST_FiltroCliente filtros) {
            string displayMessage = string.Empty;
            bool success = false;
            List<AST_ClienteSala> data = new List<AST_ClienteSala>();

            try {
                List<int> codsSalas = salaBl.ObtenerCodsSalasDeSesion(Session);
                data = ast_clienteSalaBL.ObtenerClientesParaEnvioNotificacion(filtros, codsSalas);
                success = data.Count > 0;
                displayMessage = success ? "Lista de clientes." : "No hay clientes con los filtros ingresados.";
            } catch(Exception ex) {
                success = false;
                displayMessage = $"{ex.Message}, llame al administrador";
            }

            JsonResult jsonResult = Json(new { success, data, displayMessage });
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ObtenerPermisosDeContactoDeCliente(string numeroDocumento, int idTipoDocumento, int codSala) {
            try {
                SalaEntidad sala = salaBl.ObtenerSalaPorCodigo(codSala);
                if(!sala.Existe()) {
                    return JsonResponse(new {
                        success = false,
                        displayMessage = $"No existe sala con código {codSala}."
                    }, HttpStatusCodes.Conflict);
                }

                AST_ClienteEntidad cliente = ast_ClienteBL.ObtenerPermisosDeContactoDeCliente(numeroDocumento, idTipoDocumento, codSala);
                if(string.IsNullOrEmpty(cliente?.NroDoc)) {
                    return JsonResponse(new {
                        success = false,
                        displayMessage = $"No hay registro de cliente con número de documento {numeroDocumento}."
                    }, HttpStatusCodes.NotFound);
                }

                object data = new {
                    NumeroDocumento = cliente.NroDoc,
                    NombreCompleto = cliente.NombreCompleto,
                    Nombres = cliente.Nombre,
                    ApellidoPaterno = cliente.ApelPat,
                    ApellidoMaterno = cliente.ApelMat,
                    cliente.EnviaNotificacionWhatsapp,
                    cliente.EnviaNotificacionEmail,
                    cliente.EnviaNotificacionSms,
                    cliente.LlamadaCelular
                };

                return JsonResponse(new {
                    success = true,
                    displayMessage = "Cliente encontrado.",
                    data
                }, HttpStatusCodes.Ok);

            } catch(Exception ex) {
                return JsonResponse(new {
                    success = false,
                    displayMessage = ex.Message
                }, HttpStatusCodes.InternalServerError);
            }
        }

        #region Methods Migration DWH
        [HttpPost]
        [seguridad(false)]
        public JsonResult ObtenerClientesControlAccesoParaDwh(AST_ClienteMigracionDwhFiltro filtro) {
            List<AST_ClienteMigracionDwhEntidad> clientes = new List<AST_ClienteMigracionDwhEntidad>();
            List<AST_ClienteEstadoMigracion> ids = new List<AST_ClienteEstadoMigracion>();
            string displayMessage = string.Empty;
            bool success = false;
            try {
                clientes = ast_ClienteBL.ObtenerClientesControlAccesoParaDwh(filtro);
                success = clientes.Count > 0;
                displayMessage = success ? $"{clientes.Count} de control de acceso clientes para migrar." : "No se encontraron registros de clientes por control de acceso para migrar.";
            } catch(Exception ex) {
                displayMessage = $"Error al obtener los ingresos de clientes a sala para migrar. {ex.Message}";
                success = false;
                ids = clientes.Select(x => new AST_ClienteEstadoMigracion() {
                    IdCliente = x.IdCliente,
                    CodSala = x.CodSala
                }).ToList();
                ast_ClienteBL.ActualizarEstadoMigracionesDwh(ids, null);
            }

            JsonResult jsonResult = Json(new { success, displayMessage, data = clientes });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        [seguridad(false)]
        public JsonResult MarcarComoMigrados(List<AST_ClienteEstadoMigracion> ids, DateTime fechaMigracionDwh) {
            string displayMessage = string.Empty;
            bool success = false;

            if(ids.Count == 0) {
                success = false;
                displayMessage = "Tiene que incluir al menos un id de cliente con su sala para revertir el estado de la migración.";
                return Json(new { success, displayMessage });
            }

            try {
                ast_ClienteBL.ActualizarEstadoMigracionesDwh(ids, fechaMigracionDwh);
                success = true;
                displayMessage = $"{ids.Count} ingreso(s) de cliente(s) a sala marcada(s) como migrado a Data Warehouse.";
            } catch(Exception ex) {
                displayMessage = $"Error al intentar revertir los estados de migración a Data Warehouse. {ex.Message}";
                success = false;
            }

            return Json(new { success, displayMessage });
        }

        [HttpPost]
        [seguridad(false)]
        public JsonResult RevertirEstadoMigracion(List<AST_ClienteEstadoMigracion> ids) {
            string displayMessage = string.Empty;
            bool success = false;

            if(ids.Count == 0) {
                success = false;
                displayMessage = "Tiene que incluir al menos un id de cliente con su sala para revertir el estado de la migración.";
                return Json(new { success, displayMessage });
            }

            try {
                ast_ClienteBL.ActualizarEstadoMigracionesDwh(ids, null);
                success = true;
                displayMessage = $"Estado de migración a Data Warehouse revertido correctamente.";
            } catch(Exception ex) {
                displayMessage = $"Error al intentar revertir los estados de migración a Data Warehouse. {ex.Message}";
                success = false;
            }

            return Json(new { success, displayMessage });
        }
        #endregion
    }
}