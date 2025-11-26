

using CapaEntidad.Publicidad;
using CapaNegocio.Publicidad;
using System.Collections.Generic;
using System.Web.Mvc;
using System;
using System.Linq;
using CapaNegocio;
using System.Configuration;
using System.IO;
using System.Web.Script.Serialization;
using System.Web;
using System.Text;
using Newtonsoft.Json;

namespace CapaPresentacion.Controllers.Publicidad {
    [seguridad]
    public class PublicidadController : Controller {
        private readonly PublicidadBL _publicidadBl = new PublicidadBL();
        // Para cargar las salas en los dropdowns
        private readonly SalaBL _salaBl = new SalaBL();

        // Ruta de guardado de imágenes
        private readonly string _rutaImagenes;

        public PublicidadController() {
            // Lee la ruta desde Web.config
            _rutaImagenes = ConfigurationManager.AppSettings["PathArchivos"];
            if (string.IsNullOrEmpty(_rutaImagenes)) {
                // Fallback por si no está en Web.config
                _rutaImagenes = "C:\\imagenes\\";
            }

            // Asegurarse de que el directorio exista
            if (!Directory.Exists(_rutaImagenes)) {
                try {
                    Directory.CreateDirectory(_rutaImagenes);
                } catch (Exception ex) {
                    // Manejar error si no se puede crear el directorio
                    Console.WriteLine("Error creando directorio de imágenes: " + ex.Message);
                }
            }
        }

        // --- Métodos de Listado (Consumo Angular) ---

        [seguridad(false)]
        [HttpPost]
        public ContentResult ListarPublicidadActiva(int codSala) {
            var errormensaje = "";
            var lista = new List<PublicidadEntidad>();
            bool respuesta = false;
            try {
                lista = _publicidadBl.ListarPublicidadActivaPorSala(codSala);
                respuesta = true;
            } catch (Exception exp) { errormensaje = exp.Message + ", Llame al Administrador"; }

            var responseObject = new { data = lista.ToList(), respuesta = respuesta, mensaje = errormensaje };

            var serializerSettings = new JsonSerializerSettings {
                DateFormatHandling = DateFormatHandling.IsoDateFormat
            };
            string jsonResponse = JsonConvert.SerializeObject(responseObject, serializerSettings);
            return Content(jsonResponse, "application/json", Encoding.UTF8);
        }

        [seguridad(false)]
        [HttpPost]
        public ContentResult ListarEventosActivos(int codSala) {
            var errormensaje = "";
            var lista = new List<EventoEntidad>();
            bool respuesta = false;
            try {
                lista = _publicidadBl.ListarEventosActivosPorSala(codSala);
                respuesta = true;
            } catch (Exception exp) { errormensaje = exp.Message + ", Llame al Administrador"; }
          //  return Json(new { data = lista.ToList(), respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
            var responseObject = new { data = lista.ToList(), respuesta = respuesta, mensaje = errormensaje };

            var serializerSettings = new JsonSerializerSettings {
                DateFormatHandling = DateFormatHandling.IsoDateFormat
            };
            string jsonResponse = JsonConvert.SerializeObject(responseObject, serializerSettings);
            return Content(jsonResponse, "application/json", Encoding.UTF8);
        }

        // --- Nuevos Métodos CRUD (Para MVC Admin) ---

        #region CRUD Vistas Publicidad

        public ActionResult PublicidadVista() {
            return View();
        }

        public ActionResult PublicidadInsertarVista() {

            return View();
        }

        public ActionResult PublicidadModificarVista(int id) {
            var publicidad = _publicidadBl.PublicidadListaIdJson(id);
            if (publicidad == null) return RedirectToAction("PublicidadVista");

            // Guardamos la ruta antigua para la lógica de modificación
            publicidad.RutaArchivoLogoAnt = publicidad.RutaImagen;
            ViewBag.Publicidad = publicidad;
            // Serializamos el objeto para pasarlo a JavaScript
            ViewBag.PublicidadJson = new JavaScriptSerializer().Serialize(publicidad);

            return View();
        }

        #endregion

        #region CRUD API Publicidad

        [HttpPost]
        public JsonResult ListadoPublicidadAdmin(int codSala) {
            var errormensaje = "";
            var lista = new List<PublicidadEntidad>();
            try {
                lista = _publicidadBl.ListadoPublicidadAdmin(codSala);
            } catch (Exception exp) { errormensaje = exp.Message + ",Llame Administrador"; }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertarPublicidadJson(PublicidadEntidad publicidad) {
            string mensaje = "No se pudo insertar el registro";
            bool respuesta = false;
            HttpPostedFileBase file = Request.Files[0]; // Asumimos que la imagen es el primer archivo

            if (file != null && file.ContentLength > 0) {
                try {
                    string extension = Path.GetExtension(file.FileName).ToLower();
                    // Validar extensiones
                    if (extension == ".jpg" || extension == ".png" || extension == ".jpeg" || extension == ".gif") {
                        // Crear un nombre único para evitar colisiones
                        string nombreArchivo = Guid.NewGuid().ToString() + extension;
                        string rutaCompleta = Path.Combine(_rutaImagenes, "Contenido", "Publicaciones");
                        if (!Directory.Exists(rutaCompleta)) {
                            try {
                                Directory.CreateDirectory(rutaCompleta);
                            } catch (Exception ex) {
                                // Manejar error si no se puede crear el directorio
                                Console.WriteLine("Error creando directorio de imágenes: " + ex.Message);
                            }
                        }

                        rutaCompleta = Path.Combine(rutaCompleta, nombreArchivo);

                        // Guardar el archivo en el disco
                        file.SaveAs(rutaCompleta);

                        // Guardar solo el nombre del archivo en la BD
                        publicidad.RutaImagen = nombreArchivo;
                        publicidad.Estado = 1; // Por defecto activo

                        respuesta = _publicidadBl.InsertarPublicidadJson(publicidad);
                        if (respuesta) {
                            mensaje = "Registro Insertado Correctamente";
                        }
                    } else {
                        mensaje = "Solo se permiten archivos .jpg, .png, .jpeg o .gif.";
                    }
                } catch (Exception ex) {
                    mensaje = ex.Message;
                }
            } else {
                mensaje = "Debe seleccionar una imagen.";
            }

            return Json(new { respuesta, mensaje });
        }

        [HttpPost]
        public JsonResult ModificarPublicidadJson(PublicidadEntidad publicidad) {
            string mensaje = "No se pudo modificar el registro";
            bool respuesta = false;
            HttpPostedFileBase file = Request.Files[0]; // Asumimos que la imagen es el primer archivo

            try {
                // 1. Lógica de Imagen
                if (file != null && file.ContentLength > 0) {
                    // Hay un archivo nuevo
                    string extension = Path.GetExtension(file.FileName).ToLower();
                    if (extension == ".jpg" || extension == ".png" || extension == ".jpeg" || extension == ".gif") {
                        string nombreArchivo = Guid.NewGuid().ToString() + extension;
                        string rutaCompleta = Path.Combine(_rutaImagenes, "Contenido", "Publicaciones");
                        if (!Directory.Exists(rutaCompleta)) {
                            try {
                                Directory.CreateDirectory(rutaCompleta);
                            } catch (Exception ex) {
                                // Manejar error si no se puede crear el directorio
                                Console.WriteLine("Error creando directorio de imágenes: " + ex.Message);
                            }
                        }
                        rutaCompleta = Path.Combine(rutaCompleta, nombreArchivo);

                        // Guardar nuevo archivo
                        file.SaveAs(rutaCompleta);
                        publicidad.RutaImagen = nombreArchivo; // Asignar nuevo nombre

                        // Eliminar archivo anterior si existe
                        if (!string.IsNullOrEmpty(publicidad.RutaArchivoLogoAnt)) {
                            string rutaAntigua = Path.Combine(_rutaImagenes, "Contenido", "Publicaciones", publicidad.RutaArchivoLogoAnt);
                            if (System.IO.File.Exists(rutaAntigua)) {
                                System.IO.File.Delete(rutaAntigua);
                            }
                        }
                    } else {
                        return Json(new { respuesta = false, mensaje = "Solo se permiten archivos .jpg, .png, .jpeg o .gif." });
                    }
                } else {
                    // No hay archivo nuevo, mantener el anterior
                    publicidad.RutaImagen = publicidad.RutaArchivoLogoAnt;
                }

                // 2. Lógica de BD
                respuesta = _publicidadBl.ModificarPublicidadJson(publicidad);
                if (respuesta) {
                    mensaje = "Registro Modificado Correctamente";
                }
            } catch (Exception ex) {
                mensaje = ex.Message;
            }

            return Json(new { respuesta, mensaje });
        }

        [HttpPost]
        public JsonResult ModificarEstadoPublicidadJson(int Id, int Estado) {
            var errormensaje = "";
            bool respuesta = false;
            try {
                // Estado 0 = Inactivo (Eliminado lógico)
                respuesta = _publicidadBl.ModificarEstadoPublicidadJson(Id, Estado);
            } catch (Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        // --- Se repite la misma estructura para EVENTO ---

        #region CRUD Vistas Evento

        public ActionResult EventoVista() {
            return View("~/Views/Evento/EventoVista.cshtml");
        }

        public ActionResult EventoInsertarVista() {
            return View("~/Views/Evento/EventoInsertarVista.cshtml");
        }

        public ActionResult EventoModificarVista(int id) {
            var evento = _publicidadBl.EventoListaIdJson(id);
            if (evento == null) return RedirectToAction("EventoVista");

            evento.RutaArchivoLogoAnt = evento.RutaImagen;
            ViewBag.Evento = evento;
            // No es necesario pasar las salas, el JS las carga
            ViewBag.EventoJson = new JavaScriptSerializer().Serialize(evento);

            return View("~/Views/Evento/EventoModificarVista.cshtml");
        }

        #endregion

        #region CRUD API Evento

        [HttpPost]
        public JsonResult ListadoEventoAdmin(int codSala) {
            var errormensaje = "";
            var lista = new List<EventoEntidad>();
            try {
                lista = _publicidadBl.ListadoEventoAdmin(codSala);
            } catch (Exception exp) { errormensaje = exp.Message + ",Llame Administrador"; }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertarEventoJson(EventoEntidad evento) {
            string mensaje = "No se pudo insertar el registro";
            bool respuesta = false;
            HttpPostedFileBase file = Request.Files[0];

            if (file != null && file.ContentLength > 0) {
                try {
                    string extension = Path.GetExtension(file.FileName).ToLower();
                    if (extension == ".jpg" || extension == ".png" || extension == ".jpeg" || extension == ".gif") {
                        string nombreArchivo = Guid.NewGuid().ToString() + extension;

                        string rutaCompleta = Path.Combine(_rutaImagenes, "Contenido", "Eventos");
                        if (!Directory.Exists(rutaCompleta)) {
                            try {
                                Directory.CreateDirectory(rutaCompleta);
                            } catch (Exception ex) {
                                // Manejar error si no se puede crear el directorio
                                Console.WriteLine("Error creando directorio de imágenes: " + ex.Message);
                            }
                        }
                        rutaCompleta = Path.Combine(rutaCompleta, nombreArchivo);

                        file.SaveAs(rutaCompleta);
                        evento.RutaImagen = nombreArchivo;
                        evento.Estado = 1;

                        respuesta = _publicidadBl.InsertarEventoJson(evento);
                        if (respuesta) {
                            mensaje = "Registro Insertado Correctamente";
                        }
                    } else {
                        mensaje = "Solo se permiten archivos .jpg, .png, .jpeg o .gif.";
                    }
                } catch (Exception ex) {
                    mensaje = ex.Message;
                }
            } else {
                mensaje = "Debe seleccionar una imagen.";
            }

            return Json(new { respuesta, mensaje });
        }

        [HttpPost]
        public JsonResult ModificarEventoJson(EventoEntidad evento) {
            string mensaje = "No se pudo modificar el registro";
            bool respuesta = false;
            HttpPostedFileBase file = Request.Files[0];

            try {
                if (file != null && file.ContentLength > 0) {
                    string extension = Path.GetExtension(file.FileName).ToLower();
                    if (extension == ".jpg" || extension == ".png" || extension == ".jpeg" || extension == ".gif") {
                        string nombreArchivo = Guid.NewGuid().ToString() + extension;

                        string rutaCompleta = Path.Combine(_rutaImagenes, "Contenido", "Eventos");
                        if (!Directory.Exists(rutaCompleta)) {
                            try {
                                Directory.CreateDirectory(rutaCompleta);
                            } catch (Exception ex) {
                                // Manejar error si no se puede crear el directorio
                                Console.WriteLine("Error creando directorio de imágenes: " + ex.Message);
                            }
                        }
                        rutaCompleta = Path.Combine(rutaCompleta, nombreArchivo);

                        file.SaveAs(rutaCompleta);
                        evento.RutaImagen = nombreArchivo;

                        if (!string.IsNullOrEmpty(evento.RutaArchivoLogoAnt)) {
                            string rutaAntigua = Path.Combine(_rutaImagenes, "Contenido", "Eventos", evento.RutaArchivoLogoAnt);
                            if (System.IO.File.Exists(rutaAntigua)) {
                                System.IO.File.Delete(rutaAntigua);
                            }
                        }
                    } else {
                        return Json(new { respuesta = false, mensaje = "Solo se permiten archivos .jpg, .png, .jpeg o .gif." });
                    }
                } else {
                    evento.RutaImagen = evento.RutaArchivoLogoAnt;
                }

                respuesta = _publicidadBl.ModificarEventoJson(evento);
                if (respuesta) {
                    mensaje = "Registro Modificado Correctamente";
                }
            } catch (Exception ex) {
                mensaje = ex.Message;
            }

            return Json(new { respuesta, mensaje });
        }

        [HttpPost]
        public JsonResult ModificarEstadoEventoJson(int Id, int Estado) {
            var errormensaje = "";
            bool respuesta = false;
            try {
                respuesta = _publicidadBl.ModificarEstadoEventoJson(Id, Estado);
            } catch (Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}