using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CapaNegocio;
using System.Reflection;
using CapaEntidad;
using CapaPresentacion.Models;
using System.Net;
using S3k.Utilitario.Models;
using S3k.Utilitario;
using CapaPresentacion.Filters;
using S3k.Utilitario.Encriptacion;
using System.Numerics;
using System.IO;
using System.Security.Cryptography;

namespace CapaPresentacion.Controllers
{
    [seguridad]
    public class SeguridadController : Controller
    {
        private SEG_RolBL webRolBl = new SEG_RolBL();
        private SEG_PermisoRolBL webPermisoRolBl = new SEG_PermisoRolBL();
        private SEG_PermisoBL webPermisoBl = new SEG_PermisoBL();
        private SEG_PermisoMenuBL webPermisoMenuBl = new SEG_PermisoMenuBL();
        private SEG_RolUsuarioBL webRolUsuarioBl = new SEG_RolUsuarioBL();
        private SEG_UsuarioBL webUsuarioBl = new SEG_UsuarioBL();
        private SEG_AUDITORIABL seg_auditoriaBL = new SEG_AUDITORIABL();

        [HttpGet]
        public ActionResult SeguridadVista()
        {
            ViewBag.rolId = Session["rol"];
            return View("~/Views/Seguridad/SeguridadVista.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult AgregarPermisoRol(List<SEG_PermisoRolEntidad> webPermiso)
        {
            var errormensaje = "Accion realizada Correctamente.";
            bool respuestaConsulta = false;
            try
            {
                foreach (var permiso in webPermiso)
                {
                    permiso.WEB_PRolFechaRegistro = DateTime.Now;
                    respuestaConsulta = webPermisoRolBl.GuardarPermisoRol(permiso);
                }
                

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult QuitarPermisoRol(List<SEG_PermisoRolEntidad> webPermiso)
        {
            var errormensaje = "Accion realizada Correctamente.";
            bool respuestaConsulta = false;
            try
            {
                foreach (var permiso in webPermiso)
                {
                    var WEB_PermID = permiso.WEB_PermID;
                    var WEB_RolID = permiso.WEB_RolID;
                    respuestaConsulta = webPermisoRolBl.EliminarPermisoRol(WEB_PermID, WEB_RolID);
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }


        [seguridad(false)]
        [HttpPost]
        public ActionResult AgregarPermisoMenu(List<SEG_PermisoMenuEntidad> webPermisoMenu)
        {
            var errormensaje = "Accion realizada Correctamente.";
            bool respuestaConsulta = false;
            try
            {
                foreach(SEG_PermisoMenuEntidad permisoMenu in webPermisoMenu)
                {
                    permisoMenu.WEB_PMeFechaRegistro = DateTime.Now;
                    respuestaConsulta = webPermisoMenuBl.GuardarPermisoMenu(permisoMenu);
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame al Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult QuitarPermisoMenu(List<SEG_PermisoMenuEntidad> webPermisoMenu)
        {
            var errormensaje = "Accion realizada Correctamente.";
            bool respuestaConsulta = false;
            try
            {
                foreach(SEG_PermisoMenuEntidad permisoMenu in webPermisoMenu)
                {
                    var WEB_PMeDataMenu = permisoMenu.WEB_PMeDataMenu;
                    var WEB_RolID = permisoMenu.WEB_RolID;
                    respuestaConsulta = webPermisoMenuBl.EliminarPermisoMenu(WEB_PMeDataMenu, WEB_RolID);
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame al Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListadoFechasPrincipales()
        {

            var errormensaje = "";
            var listaxMenuPrincipal = new List<SEG_PermisoMenuEntidad>();
            try
            {
                listaxMenuPrincipal = webPermisoMenuBl.GetPermisoFechaMax();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Comuniquese con el Administrador";
            }

            return Json(new { data = listaxMenuPrincipal.ToList(), mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListadoRolesSeguridadPermiso()
        {
            var errormensaje = "";
            var listaRol = new List<SEG_RolEntidad>();
            try
            {
                listaRol = webRolBl.ListarRol().OrderBy(x => x.WEB_RolNombre).ToList();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }

            return Json(new { roles = listaRol, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListadoMenusRolId(int rolId)
        {
            var errormensaje = "";
            var resultado = new List<dynamic>();
            var listaxMenuPrincipal = new List<SEG_PermisoMenuEntidad>();
            try
            {

                listaxMenuPrincipal = webPermisoMenuBl.GetPermisoMenuRolId(rolId);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Comuniquese con el Administrador";
            }


            return Json(new { dataResultado = listaxMenuPrincipal.ToList(), mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListadoControladorPermisos(int rolid)
        {
            var errormensaje = "";

            var listaPermisos = new List<SEG_PermisoEntidad>();
            var listaPermisosRol = new List<SEG_PermisoRolEntidad>();

            try
            {

                listaPermisos = webPermisoBl.ListarPermisosActivos();
                listaPermisosRol = webPermisoRolBl.GetPermisoRol(rolid);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }

            var cabeceras = listaPermisos.GroupBy(x => new { x.WEB_PermControlador }).Select(g => new { g.Key.WEB_PermControlador }).OrderBy(x => x.WEB_PermControlador);
            var permisos = listaPermisos.OrderBy(x => x.WEB_PermNombreR).ToList();
            var permisosRol = listaPermisosRol.ToList();
            return Json(new { controlador = cabeceras.ToList(), listaPermisoControlador = permisos, listaPermisosRol = permisosRol, mensaje = errormensaje });

        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListadoPermisos()
        {
            var errormensaje = "";
            var listaRol = new List<SEG_RolEntidad>();
            var listaPermisos = new List<SEG_PermisoEntidad>();
            var listaPermisosRol = new List<SEG_PermisoRolEntidad>();
            var listaRolUsuario = new List<SEG_RolUsuarioEntidad>();
            try
            {
                listaRol = webRolBl.ListarRol().OrderBy(x => x.WEB_RolNombre).ToList();
                listaPermisos = webPermisoBl.ListarPermisosActivos();
                listaPermisosRol = webPermisoRolBl.ListarPermisoRol();
                listaRolUsuario = webRolUsuarioBl.ListarRolUsuarios();

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            var cabeceras = listaPermisos.GroupBy(x => new { x.WEB_PermControlador }).Select(g => new { g.Key.WEB_PermControlador }).OrderBy(x => x.WEB_PermControlador);
            var permisos = listaPermisos.OrderBy(x => x.WEB_PermNombreR).ToList();
            return Json(new { roles = listaRol, modulos = cabeceras.ToList(), permisos = permisos, permisosRol = listaPermisosRol.ToList(), mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListadoRolUsuario()
        {
            var errormensaje = "";
            var lista = new List<SEG_RolEntidad>();
            try
            {
                lista = webRolBl.ListarRol().OrderBy(x => x.WEB_RolNombre).ToList();

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = lista, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListadoTableUsuarioAsignarRol()
        {
            var errormensaje = "";
            var listaRol = new List<SEG_RolEntidad>();
            var listaUsu = new List<SEG_UsuarioEntidad>();
            var listaRolUsuario = new List<SEG_RolUsuarioEntidad>();
            try
            {
                listaRol = webRolBl.ListarRol().OrderBy(x => x.WEB_RolNombre).ToList();
                listaUsu = webUsuarioBl.UsuarioListadoJson();
                listaRolUsuario = webRolUsuarioBl.ListarRolUsuarios();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { roles = listaRol, usuarios = listaUsu.ToList(), rolUsuarios = listaRolUsuario.ToList(), mensaje = errormensaje });
        }
        //auditoria

        [seguridad(false)]
        [HttpPost]
        public void AgregarVisita(SEG_AUDITORIA auditoria)
        {
            var errormensaje = "Accion realizada Correctamente.";
            bool respuestaConsulta = false;
            try
            {
                auditoria.fechaRegistro = DateTime.Now;
                auditoria.usuario = Session["UsuarioNombre"].ToString();
                auditoria.usuariodata = Newtonsoft.Json.JsonConvert.SerializeObject(Session["usuario"]);
                auditoria.ip = GetIPAddress();
                respuestaConsulta = seg_auditoriaBL.Guardar(auditoria);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

           // return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        public ActionResult SeguridadAuditoriaVista()
        {
            return View("~/Views/Seguridad/AuditoriaVista.cshtml");
        }

        [HttpPost]

        public ActionResult ListarAuditoria(string txtModulo, DateTime horaIni, DateTime horaFin)
        {
            var errormensaje = "";
            var lista = new List<SEG_AUDITORIA>();
            try
            {
                lista = new SEG_AUDITORIABL().GetAllRangoFechas(horaIni, horaFin);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message;
            }

            var listado = from list in lista
                          orderby list.fechaRegistro descending
                          select new
                          {
                              codAuditoria = list.codAuditoria,
                              fechaRegistro = list.fechaRegistro.ToString("dd/MM/yyyy hh:mm:ss tt") == "01/01/1753" ? "--" : list.fechaRegistro.ToString("dd/MM/yyyy hh:mm:ss tt"),
                              usuario = list.usuario == "" ? "--" : list.usuario,
                              proceso = list.proceso == "" ? "--" : list.proceso,
                              descripcion = list.descripcion == "" ? "--" : list.descripcion,
                              subsistema = list.subsistema == "" ? "--" : list.subsistema,
                              codSala = list.codSala == 0 ? "--" : list.codSala.ToString(),
                              sala= list.sala == "" ? "--" : list.sala.ToString(),
                              ip = list.ip == "" ? "--" : list.ip
                          };

            var serializer = new JavaScriptSerializer();

            // For simplicity just use Int32's max value.
            // You could always read the value from the config section mentioned above.
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new { data = listado.ToList(), mensaje = errormensaje };
            var result = new ContentResult
            {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;

        }

        [HttpPost]
        public ActionResult DataAuditoria(string id)
        {
            int sub = Convert.ToInt32(id.Replace("Registro", ""));
            var errormensaje = "";
            var auditoria = new SEG_AUDITORIA();
            try
            {
                auditoria = new SEG_AUDITORIABL().getDataAuditoria(sub);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                errormensaje = "Verifique conexion,Llame Administrador";
            }
            

            return Json(new { dataResultado = auditoria, mensaje = errormensaje });
        }
        //finauditoria


        public static string GetIPAddress()
        {
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            string IPAddress = " ";
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress;
        }

        [HttpPost]
        [seguridad(false)]
        public ActionResult AutorizedControllerAction(string controllerName, string actionName, string permissionAs)
        {
            int sessionRolId = Convert.ToInt32(Session["rol"]);
            bool status = webPermisoRolBl.AutorizedControllerAction(sessionRolId, controllerName, actionName);
            string message = status ? $"Permiso {permissionAs} autorizado" : $"No tiene autorizado el permiso {permissionAs}";

            var data = new
            {
                controllerName,
                actionName
            };

            return Json(new
            {
                status,
                message,
                data
            });
        }

        [HttpGet]
        [seguridad(false)]
        public ActionResult RedirectAccessToken(string redirectUrl = null)
        {
            UserClaim userClaim = new UserClaim()
            {
                UserId = Convert.ToInt64(Session["UsuarioID"] ?? 0),
                UserName = Session["UsuarioNombre"]?.ToString()
            };

            string newLink = UrlUtil.GetNewUrlWithToken(userClaim, redirectUrl);

            return Redirect(newLink);
        }

        [HttpGet]
        [seguridad(false)]
        public ActionResult LogoutAccessToken()
        {
            Session["permisos"] = null;
            Session["empleado"] = null;
            Session["Empresa"] = null;
            Session["Sala"] = null;
            Session["UsuarioID"] = null;
            Session["UsuarioNombre"] = null;
            Session["token"] = null;
            Session["usuario"] = null;
            Session.Remove(TokenProgresivoAttribute.KEY_TOKEN_PROGRESIVO);
            Session.Remove(TokenProgresivoAttribute.KEY_SGN_ROOM_ID);

            funciones.BorrarCookie("token");
            funciones.BorrarCookie("codSala");
            funciones.BorrarCookie("datainicial");
            funciones.BorrarCookie("datafinal");
            funciones.BorrarCookie("c_id_sgn");

            return Json(new
            {
                status = true,
                message = "Has sido desconectado"
            }, JsonRequestBehavior.AllowGet);
        }

        [seguridad(false)]
        [HttpGet]
        public ActionResult GetLlaveDinamica() {

            var errormensaje = "";
            var llave = string.Empty;
            try {
                llave = seg_auditoriaBL.GetLlaveDinamica();
            } catch (Exception exp) {
                errormensaje = exp.Message + ",Comuniquese con el Administrador";
            }

            return Json(new { data = llave, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult DesencriptarLlave(string hash) {
            var errormensaje = "";
            var texto = string.Empty;
            try {
               
                texto = seg_auditoriaBL.DesencriptarLlave(hash);

            } catch (Exception exp) {
                errormensaje = exp.Message + ",Comuniquese con el Administrador";
            }
            return Json(new { data = texto, mensaje = errormensaje });
        }

        #region ActualizarMétodosBD con métodos en controladores

        [seguridad(false)]
        [HttpGet]
        public ActionResult ActualizarMetodos()
        {
            List<string> registrosborrados = new List<string>();
            var nuevalista = GetMetodos();   ///metodos en CONTROLADORES
            bool respuestaConsulta = false;
            var errormensaje = "";
            //var nuevalista2 = MetodosLista_List();
            var listapermisosbd = new List<SEG_PermisoEntidad>();
            listapermisosbd = webPermisoBl.ListarPermisos();/////registros en  SEG_Permiso
            bool seguridad = true;
            foreach (var registro in listapermisosbd)
            {
                seguridad = true;
                string nombrepermiso = registro.WEB_PermNombre;
                string nombrecontrolador = registro.WEB_PermControlador;
                var permiso = new SEG_PermisoEntidad();

                var nuevalistalinq = ((IEnumerable<dynamic>)nuevalista).Cast<dynamic>();
                var metodolinq = nuevalista.Where(a => a.Action == nombrepermiso && a.Controller == nombrecontrolador).SingleOrDefault();///buscar registro bd en   METODOS CONTROLADORES

                try
                {
                    if (metodolinq != null)
                    {
                        var atributosobjeto = metodolinq.AttributesMetodo;
                        var atributosControladorobjeto = metodolinq.AttributesControlador;
                        ///atributos de controlador , si hubiera sido definido
                        var atributoscontroladorlinq = ((IEnumerable<dynamic>)atributosControladorobjeto).Cast<dynamic>();
                        if (atributoscontroladorlinq != null)
                        {
                            var seguridadcontrolador = atributoscontroladorlinq.Where(x => x.AttributeType.Name == "seguridad").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                            if (seguridadcontrolador != null)
                            {
                                seguridad = atributoscontroladorlinq.Where(x => x.AttributeType.Name == "seguridad").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                            }
                        }
                        ///atributos de metodo,  reemplazan atributos de controlador
                        var objetolinq = ((IEnumerable<dynamic>)atributosobjeto).Cast<dynamic>();
                        if (objetolinq != null)
                        {
                            var seguridaddelobjeto = objetolinq.Where(x => x.AttributeType.Name == "seguridad").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                            if (seguridaddelobjeto != null)
                            {
                                seguridad = seguridaddelobjeto;
                            }

                        }
                    }
                    if (!seguridad)///si seguridad es false debido a controlador=>si controlador con seguridad false y metodo sin atributo seguridad
                    {
                        webPermisoBl.BorrarPermiso(nombrepermiso, nombrecontrolador);
                        registrosborrados.Add(nombrecontrolador + "/" + nombrepermiso);
                    }
                    var permisoexistectrl = nuevalista.Where(a => a.Action == nombrepermiso && a.Controller == nombrecontrolador);
                    int existepermisoencontrolador = permisoexistectrl.Count();
                    //respuestaConsulta = webPermisoBl.GetPermisoId(nombrepermiso);
                    if (existepermisoencontrolador == 0)//Permiso existe en tabla  pero no en controlador  => delete 
                    {//borrar
                        webPermisoBl.BorrarPermiso(nombrepermiso, nombrecontrolador);
                        registrosborrados.Add(nombrecontrolador + "/" + nombrepermiso);
                    }

                }
                catch (Exception exp)
                {
                    errormensaje = exp.Message + " ,Llame Administrador";
                    return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
                }

            }
            //return Json(new { respuesta = "", mensaje = errormensaje });

            return Json(new{borrados=registrosborrados }, JsonRequestBehavior.AllowGet);

        }

        [seguridad(false)]
        public List<dynamic> GetMetodos()
        {
            Assembly asm = Assembly.GetAssembly(typeof(CapaPresentacion.MvcApplication));
            var lista = asm.GetTypes()
                        .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                        .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                        .Where(m => (System.Attribute.GetCustomAttributes(typeof(seguridad), true).Length > 0))
                        .Where(m => (String.Join(",", (m.GetCustomAttributesData()
                                                                .Where(f => f.Constructor.DeclaringType.Name == "seguridad")
                                                                .Select(c => (c.ConstructorArguments[0].Value))))
                                                          != "False"))
                        .Select(x => new
                        {
                            Controller = x.DeclaringType.Name,
                            Action = x.Name,
                            ReturnType = x.ReturnType.Name,
                            Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))),



                            AttributesControlador = x.DeclaringType.GetCustomAttributesData(),
                            AttributesControladorString = String.Join(",", x.DeclaringType.GetCustomAttributesData().Select(
                                         a => String.Join(",", a.ConstructorArguments.Select(b => a.AttributeType.Name + " = " + b.Value))
                                )),
                            AttributesMetodo = x.GetCustomAttributesData(),
                            AttributesMetodostring = String.Join(",", x.GetCustomAttributesData()
                                  .Select(
                                         a => String.Join(",", a.ConstructorArguments.Select(b => a.AttributeType.Name + " = " + b.Value))
)),

                        })
                        .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();
            return lista.ToList<dynamic>(); ;

        }

        /// <summary>
        /// MetodosLista_List   RETURNS LISTA de métodos declarados con seguridad  de los CONTROLADORES
        /// </summary>
        /// <returns></returns>
        /// 



        [seguridad(false)]
        public List<dynamic> MetodosLista_List()
        {
            Assembly asm = Assembly.GetAssembly(typeof(CapaPresentacion.MvcApplication));
            var lista = asm.GetTypes()
                        .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                        .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                        .Where(m => (System.Attribute.GetCustomAttributes(typeof(seguridad), true).Length > 0))
                        .Where(m => (String.Join(",", (m.GetCustomAttributesData()
                                                                .Where(f => f.Constructor.DeclaringType.Name == "seguridad")
                                                                .Select(c => (c.ConstructorArguments[0].Value))))
                                                          != "False"))
                        .Select(x => new
                        {
                            Controller = x.DeclaringType.Name,
                            Action = x.Name,
                            ReturnType = x.ReturnType.Name,
                            Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))),
                            AttributesControlador = x.DeclaringType.GetCustomAttributesData(),
                            AttributesControladorString = String.Join(",", x.DeclaringType.GetCustomAttributesData().Select(
                                         a => String.Join(",", a.ConstructorArguments.Select(b => a.AttributeType.Name + " = " + b.Value))
                                )),
                            AttributesMetodo = x.GetCustomAttributesData(),
                            AttributesMetodostring = String.Join(",", x.GetCustomAttributesData()
                                  .Select(
                                         a => String.Join(",", a.ConstructorArguments.Select(b => a.AttributeType.Name + " = " + b.Value))
)),

                        })
                        .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();
            return lista.ToList<dynamic>(); 

        }

        /// <summary>
        /// MetodosLista_Objeto(nombrecontrolador,nombremetodo)
        /// </summary>
        /// <param name="control"></param>
        /// <param name="accion"></param>
        /// <returns></returns>
        [seguridad(false)]
        public Metodo_atributos Metodo_Objeto(string control,string accion)
        {
            Assembly asm = Assembly.GetAssembly(typeof(CapaPresentacion.MvcApplication));
            var metodoactual = asm.GetTypes()
                        .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                        .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                        //.Where(m => (System.Attribute.GetCustomAttributes(typeof(seguridad), true).Length > 0))
                        //.Where(m => (String.Join(",", (m.GetCustomAttributesData()
                        //                                        .Where(f => f.Constructor.DeclaringType.Name == "seguridad")
                        //                                        .Select(c => (c.ConstructorArguments[0].Value))))
                        //                                  != "False"))
                        .Where(x => (x.DeclaringType.Name).ToUpper() == (control + "Controller").ToUpper())   ////correccion
                        .Where(x => (x.Name).ToUpper() == accion.ToUpper())
                        .Select(x => new Metodo_clase
                        {
                            Controller = x.DeclaringType.Name,
                            Action = x.Name,
                            ReturnType = x.ReturnType.Name,
                            Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))),
                            AttributesControlador = x.DeclaringType.GetCustomAttributesData(),
                            AttributesControladorString = String.Join(",", x.DeclaringType.GetCustomAttributesData().Select(a => String.Join(",", a.ConstructorArguments.Select(b => a.AttributeType.Name + " = " + b.Value)))),
                            AttributesMetodo = x.GetCustomAttributesData(),
                            AttributesMetodostring = String.Join(",", x.GetCustomAttributesData()
                                  .Select(
                                         a => String.Join(",", a.ConstructorArguments.Select(b => a.AttributeType.Name + " = " + b.Value))
                            )),

                        })
                        .OrderBy(x => x.Controller).ThenBy(x => x.Action).FirstOrDefault();

            var atributosobjeto = metodoactual.AttributesMetodo;
            var atributosControladorobjeto = metodoactual.AttributesControlador;
            bool seguridad = true;///por defecto todos los metodos de  nuevalista  vienen con seguridad
            string modulo = "";
            string descripcion = "";
            bool atributoseguridaddefinido = false;
            bool atributomodulodefinido = false;
            bool atributodescripcion = false;

            foreach (var atributo in atributosobjeto)
            {
                if (atributo.AttributeType.Name == "seguridad")
                {
                    seguridad = (bool)atributo.ConstructorArguments[0].Value;
                    atributoseguridaddefinido = true;
                }
                if (atributo.AttributeType.Name == "modulo")
                {
                    modulo = (string)atributo.ConstructorArguments[0].Value;
                    atributomodulodefinido = true;
                }
                if (atributo.AttributeType.Name == "descripcion")
                {
                    descripcion = (string)atributo.ConstructorArguments[0].Value;
                    atributodescripcion = true;
                }
            }
            ///atributos de controlador , si hubiera sido definido
            var atributoscontroladorlinq = ((IEnumerable<dynamic>)atributosControladorobjeto).Cast<dynamic>();
            if (atributoscontroladorlinq != null)
            {
                var seguridadcontrolador = atributoscontroladorlinq.Where(x => x.AttributeType.Name == "seguridad").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                if (seguridadcontrolador != null)
                {
                    seguridad = atributoscontroladorlinq.Where(x => x.AttributeType.Name == "seguridad").Select(x => x.ConstructorArguments[0].Value
                                           ).SingleOrDefault();
                }
                var modulocontrolador = atributoscontroladorlinq.Where(x => x.AttributeType.Name == "modulo").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                if (modulocontrolador != null)
                {
                    modulo = modulocontrolador;
                }
                var descripcioncontrolador = atributoscontroladorlinq.Where(x => x.AttributeType.Name == "descripcion").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                if (descripcioncontrolador != null)
                {
                    descripcion = descripcioncontrolador;
                }
            }
            ///atributos de metodo,  reemplazan atributos de controlador
            var objetolinq = ((IEnumerable<dynamic>)atributosobjeto).Cast<dynamic>();
            if (objetolinq != null)
            {
                var seguridaddelobjeto = objetolinq.Where(x => x.AttributeType.Name == "seguridad").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                if (seguridaddelobjeto != null)
                {
                    seguridad = seguridaddelobjeto;
                }
                var modulodelobjeto = objetolinq.Where(x => x.AttributeType.Name == "modulo").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                if (modulodelobjeto != null)
                {
                    modulo = modulodelobjeto;
                }
                var descripciondelobjeto = objetolinq.Where(x => x.AttributeType.Name == "descripcion").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                if (descripciondelobjeto != null)
                {
                    descripcion = descripciondelobjeto;
                }
            }

            Metodo_atributos MetodoAtributos = new Metodo_atributos();
            MetodoAtributos.Controlador = control;
            MetodoAtributos.Metodo = accion;
            MetodoAtributos.seguridad = seguridad;
            MetodoAtributos.modulo= modulo;
            MetodoAtributos.descripcion = descripcion;


            return MetodoAtributos;
        }
        [seguridad(false)]
        public ActionResult updateMethod()
        {
            ActualizarMetodos();
            var nuevalista = MetodosLista_List();
            var errormensaje = "";
            bool respuestaConsulta = false;

            foreach (var item in nuevalista)
            {
                var permiso = new SEG_PermisoEntidad();
                try
                {
                    var atributosobjeto = item.AttributesMetodo;
                    var atributosControladorobjeto = item.AttributesControlador;
                    bool seguridad = true;///por defecto todos los metodos de  nuevalista  vienen con seguridad
                    string modulo = "";
                    bool atributoseguridaddefinido = false;
                    bool atributomodulodefinido = false;
                    foreach (var atributo in atributosobjeto)
                    {
                        if (atributo.AttributeType.Name == "seguridad")
                        {
                            seguridad = (bool)atributo.ConstructorArguments[0].Value;
                            atributoseguridaddefinido = true;
                        }
                        if (atributo.AttributeType.Name == "modulo")
                        {
                            modulo = (string)atributo.ConstructorArguments[0].Value;
                            atributomodulodefinido = true;
                        }

                    }
                    ///atributos de controlador , si hubiera sido definido
                    var atributoscontroladorlinq = ((IEnumerable<dynamic>)atributosControladorobjeto).Cast<dynamic>();
                    if (atributoscontroladorlinq != null)
                    {
                        var seguridadcontrolador = atributoscontroladorlinq.Where(x => x.AttributeType.Name == "seguridad").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                        if (seguridadcontrolador != null)
                        {
                            seguridad = atributoscontroladorlinq.Where(x => x.AttributeType.Name == "seguridad").Select(x => x.ConstructorArguments[0].Value
                                                   ).SingleOrDefault();
                        }
                        var modulocontrolador = atributoscontroladorlinq.Where(x => x.AttributeType.Name == "modulo").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                        if (modulocontrolador != null)
                        {
                            modulo = modulocontrolador;
                        }
                    }
                    ///atributos de metodo,  reemplazan atributos de controlador
                    var objetolinq = ((IEnumerable<dynamic>)atributosobjeto).Cast<dynamic>();
                    if (objetolinq != null)
                    {
                        var seguridaddelobjeto = objetolinq.Where(x => x.AttributeType.Name == "seguridad").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                        if (seguridaddelobjeto != null)
                        {
                            seguridad = seguridaddelobjeto;
                        }
                        var modulodelobjeto = objetolinq.Where(x => x.AttributeType.Name == "modulo").Select(x => x.ConstructorArguments[0].Value).SingleOrDefault();
                        if (modulodelobjeto != null)
                        {
                            modulo = modulodelobjeto;
                        }
                    }

                    permiso.WEB_ModuloNombre = modulo;
                    permiso.WEB_PermNombre = item.Action;
                    permiso.WEB_PermTipo = item.Attributes;
                    permiso.WEB_PermControlador = item.Controller;
                    permiso.WEB_PermEstado = "1";
                    if (seguridad)///guardar solo si seguridad es true 
                    {
                        respuestaConsulta = webPermisoBl.GuardarPermiso(permiso);
                    }
                }
                catch (Exception exp)
                {
                    errormensaje = exp.Message + " ,Llame Administrador";
                    return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
                }
            }

            errormensaje = "";
            var listaPermisos = new List<SEG_PermisoEntidad>();
            try
            {
                listaPermisos = webPermisoBl.ListarPermisos();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }

            var cabeceras = listaPermisos.GroupBy(x => new { x.WEB_PermControlador }).Select(g => new { g.Key.WEB_PermControlador }).OrderBy(x => x.WEB_PermControlador).ToList();
            return Json(new { respuesta = listaPermisos }, JsonRequestBehavior.AllowGet);

        } 
        #endregion

    }
}