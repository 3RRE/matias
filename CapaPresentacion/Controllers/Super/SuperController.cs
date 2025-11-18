using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Super
{
    [seguridad(false)]
    [SuperSecurity()]
    public class SuperController : Controller
    {

        SEG_PermisoBL webPermisoBl = new SEG_PermisoBL();
        SEG_UsuarioBL segUsuarioBl = new SEG_UsuarioBL();

        [seguridad(false)]
        [SuperSecurity(false)]
        public ActionResult Login()
        {
            return View("~/Views/Super/Login.cshtml");
        }

        [seguridad(false)]
        public ActionResult control()
        {
            return View("~/Views/Super/PanelPrincipal.cshtml");
        }

        [seguridad(false)]
        public ActionResult permisos()
        {
            return View("~/Views/Super/Permisos.cshtml");
        }

        [seguridad(false)]
        public ActionResult ListadoControladores()
        {
            ActualizarMetodos();
            return View("~/Views/Super/ControladoresPermisos.cshtml");
        }


        [seguridad(false)]
        public ActionResult ContraseniaUsuarios()
        {
            ActualizarMetodos();
            return View("~/Views/Super/ContraseniaUsuarios.cshtml");
        }

        [seguridad(false)]
        public ActionResult Salir()
        {
            Session["ROOT_SUPERUSERNAME"] = null;

            return RedirectToAction("Login");
        }

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

            return Json(new { borrados = registrosborrados }, JsonRequestBehavior.AllowGet);

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


        [seguridad(false)]
        [SuperSecurity(false)]
        [HttpPost]
        public ActionResult ValidacionLogin(string username, string password)
        {
            string status = "failed";
            string message = "Super Usuario No Encontrado";

            string path = Server.MapPath(@"~/Content/credentials_super.json");
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (StreamReader streamReader = new StreamReader(fileStream))
            using (JsonReader jsonReader = new JsonTextReader(streamReader))
            {
                JsonSerializer serializer = new JsonSerializer();
                dynamic superUserObject = serializer.Deserialize<dynamic>(jsonReader);
                string usernameValue = superUserObject["super"]["username"];
                string passwordValue = superUserObject["super"]["password"];

                if (usernameValue.Equals(username) && passwordValue.Equals(password))
                {
                    status = "success";
                    message = "Bienvenido, Super Usuario";
                    Session["ROOT_SUPERUSERNAME"] = usernameValue;
                }
            }

            return Json(new { status = status, message = message });
        }


        [seguridad(false)]
        [HttpPost]
        public ActionResult ActionNames(string controllerName)
        {
            var nuevalista = GetMetodos();
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
                    string descripcion = "";
                    bool atributoseguridaddefinido = false;
                    bool atributomodulodefinido = false;
                    bool atributodescripciondefinido = false;
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
                            atributodescripciondefinido = true;
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


                    permiso.WEB_PermDescripcion = descripcion;
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

            return Json(new { respuesta = listaPermisos, cabeceras = cabeceras.ToList(), mensaje = errormensaje });

        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ControllersName()
        {
            string message = "";

            var permissionList = new List<SEG_PermisoEntidad>();

            try
            {
                permissionList = webPermisoBl.ListarPermisos();
            }
            catch (Exception exp)
            {
                message = exp.Message + ", Llame Administrador";
            }

            var controllers = permissionList.GroupBy(x => new { x.WEB_PermControlador }).Select(g => new { g.Key.WEB_PermControlador }).OrderBy(x => x.WEB_PermControlador).ToList();

            return Json(new { data = controllers.ToList(), message = message });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ActionsName(string controllerName)
        {
            string message = "";

            var permissionList = new List<SEG_PermisoEntidad>();

            try
            {
                permissionList = webPermisoBl.ListPermissionsByController(controllerName);
            }
            catch (Exception exp)
            {
                message = exp.Message + ", Llame Administrador";
            }

            return Json(new { data = permissionList, message = message });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ActualizarEstadoPermiso(int permisoId, int estado)
        {
            var errormensaje = "Accion realizada Correctamente.";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = webPermisoBl.ActualizarEstadoPermiso(permisoId, estado);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ActualizarDescripcionPermiso(int permisoId, string descripcion)
        {
            var errormensaje = "Accion realizada Correctamente.";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = webPermisoBl.ActualizarDescripcionPermiso(permisoId, descripcion);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        [seguridad(false)]
        [HttpPost]
        public ActionResult ActualizarNombrePermiso(int permisoId, string nombre)
        {
            var errormensaje = "Accion realizada Correctamente.";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = webPermisoBl.ActualizarNombrePermiso(permisoId, nombre);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [seguridad(false)]
        [SuperSecurity(false)]
        [HttpPost]
        public void registrarauditoria(dynamic datos)
        {
            string path = Server.MapPath(@"~/Content/auditoriadatos.txt"); ;
            if (!System.IO.File.Exists(path))
            {
                System.IO.File.Create(path).Dispose();
                using (System.IO.TextWriter tw = new System.IO.StreamWriter(path))
                {
                    tw.WriteLine(datos);
                }
            }
            else if (System.IO.File.Exists(path))
            {
                using (System.IO.TextWriter tw = new System.IO.StreamWriter(path))
                {
                    tw.WriteLine(datos);

                }
            }
        }


        [seguridad(false)]
        [HttpPost]
        public ActionResult UsuarioBloquearJson(int UsuarioID)
        {
            var respuestaConsulta = segUsuarioBl.UsuarioBloquearJson(new SEG_UsuarioEntidad { UsuarioID = UsuarioID });
            return Json(respuestaConsulta);
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult UsuarioDesbloquearJson(int UsuarioID)
        {
            var respuestaConsulta = segUsuarioBl.UsuarioDesbloquearJson(new SEG_UsuarioEntidad { UsuarioID = UsuarioID });
            return Json(respuestaConsulta);
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult UsuarioCambiarContrasenia(SEG_UsuarioEntidad usuario)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                usuario.FailedAttempts = 0;
                usuario.UsuarioContraseña = PasswordHashTool.PasswordHashManager.CreateHash(usuario.UsuarioContraseña);
                respuestaConsulta = segUsuarioBl.UsuarioCambiarContrasenia(usuario);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
    }
}