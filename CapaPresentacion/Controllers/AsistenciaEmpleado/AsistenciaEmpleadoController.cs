using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilitarios;
using CapaEntidad.AsistenciaEmpleado;
using CapaNegocio.AsistenciaEmpleado;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Face;
using Emgu.CV.CvEnum;

using System.Drawing;
using System.Net.Http;
using Newtonsoft.Json;
using CapaEntidad.Imei;
using CapaNegocio.Imei;
using System.Threading.Tasks;
using System.Text;
using Aspose.Imaging;
using System.Web.WebPages;
using RestSharp;
using System.Net;
using System.Globalization;

namespace CapaPresentacion.Controllers.AsistenciaEmpleado
{
    [seguridad]
    public class AsistenciaEmpleadoController : Controller
    {
        private readonly LogTransac _log = new LogTransac();
        private SEG_EmpleadoBL empleadoBL = new SEG_EmpleadoBL();
        private SEG_UsuarioBL usuariobl = new SEG_UsuarioBL();
        private UsuarioSalaBL usuariosalabl = new UsuarioSalaBL();
        private EmpleadoAsistenciaBL empleadoAsistenciabl = new EmpleadoAsistenciaBL();
        private EmpleadoDispositivoBL empleadoDispositivobl = new EmpleadoDispositivoBL();
        private ControlImeiBL controlImeiBL = new ControlImeiBL();

        private readonly SalaBL _salaBl = new SalaBL();
        string ubicacionarchivos = ConfigurationManager.AppSettings["PathArchivos"].ToString();
        string logactivos = ConfigurationManager.AppSettings["log"].ToString();
    
        string distancia = ConfigurationManager.AppSettings["distancia_metros"].ToString();
        string calibracion = ConfigurationManager.AppSettings["calibracion"].ToString();
        string reconocimiento = ConfigurationManager.AppSettings["reconocmiento"].ToString();

        string urlAsistencia = ConfigurationManager.AppSettings["uriAsistencia"].ToString();
        string urlReconocimiento = ConfigurationManager.AppSettings["uriReconocimiento"].ToString();

        [seguridad(false)]
        [HttpPost]
        public ActionResult EmpleadoIdObtenerJson(int emp_id)
        {
            var errormensaje = "Accion realizada Correctamente.";
            var direccionFoto = ubicacionarchivos + "/rostros/";
            var usuario = new SEG_EmpleadoEntidad();
            try
            {
                usuario = empleadoBL.EmpleadoIdObtenerJson(emp_id);

                string fotoBUK = ObtenerFotoBUK(usuario.DOI);

                if(fotoBUK=="") {

                    if(usuario.emp_foto.Trim() != "") {
                        if(System.IO.File.Exists(direccionFoto + "" + usuario.emp_foto)) {
                            byte[] bmp = ImageToBinary(direccionFoto + "" + usuario.emp_foto);
                            usuario.emp_foto = Convert.ToBase64String(bmp);
                        }

                    }

                } else {

                    System.Net.WebRequest request = System.Net.WebRequest.Create(fotoBUK);
                    System.Net.WebResponse response = request.GetResponse();
                    System.IO.Stream responseStream = response.GetResponseStream();
                    Bitmap bmp_ = new Bitmap(responseStream);
                    System.IO.MemoryStream ms = new MemoryStream();
                    bmp_.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] byteImage = ms.ToArray();
                    usuario.emp_foto = Convert.ToBase64String(byteImage);

                } 


            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = usuario, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult Buscarpor_nrodocumentoJson(string nro_documento)
        {
            bool respuesta = false;
            string mensaje = "";
            string mensajeConsola = "";
            SEG_EmpleadoEntidad empleado = new SEG_EmpleadoEntidad();
           
            var direccionFoto = ubicacionarchivos + "/rostros/" + nro_documento.Trim() + "/";;
            var logubicacion = ubicacionarchivos + "/log/";
            //var direccion = Server.MapPath("/") + Request.ApplicationPath + "/reconocimientoFacial/rostros/";
            try
            {
                if (nro_documento == "")
                {
                    respuesta = false;
                    mensaje = "Error; Nro de Documento Obligatorio..";
                    return Json(new { respuesta, mensaje, mensajeConsola });
                }

                if (direccionFoto == "")
                {
                    respuesta = false;
                    mensaje = "Error; ubicación de archivos no registrada..";
                    return Json(new { respuesta, mensaje, mensajeConsola });
                }

                string UriSistemaRRHH = ConfigurationManager.AppSettings["UriSistemaRRHH"];
                string url = UriSistemaRRHH + "/api/RRHH/ListEmployeeForDniFechaCese?dni=" + nro_documento.Trim() + "&val=true";
                string json = "";

                using(var client = new HttpClient()) {

                    using(var response = client.GetAsync(url).Result) {
                        if(response.IsSuccessStatusCode) {
                            json = response.Content.ReadAsStringAsync().Result;
                            if(json != "{}") {
                                Int32 idempleado = 0;
                                dynamic jsonObj = JsonConvert.DeserializeObject(json);
                                var item = jsonObj;
                                if(item.fechacese != null) {
                                    mensaje = "¡Extrabajador!";
                                    DateTime fechaActual = DateTime.Now;
                                    DateTime fechaCese = Convert.ToDateTime(item.fechacese);
                                    fechaCese = fechaCese.AddMonths(6);
                                    if(fechaActual > fechaCese) {
                                        respuesta = false;

                                    } else {
                                        empleado.Nombres = item.nombres;
                                        empleado.ApellidosPaterno = item.apepaterno;
                                        empleado.ApellidosMaterno = item.apematerno;
                                        empleado.DOI = (nro_documento).Trim();
                                        mensaje = "Registro Extrabajador\n(fecha cese en rango)";
                                        respuesta = true;
                                    }

                                } else {
                                    empleado.Nombres = item.nombres;
                                    empleado.ApellidosPaterno = item.apepaterno;
                                    empleado.ApellidosMaterno = item.apematerno;
                                    empleado.DOI = (nro_documento).Trim();
                                    mensaje = "Registro Empleado";
                                    respuesta = true;

                                }
                                if(respuesta) {
                                    if(Directory.Exists(direccionFoto)) {

                                        string fotoBUK = ObtenerFotoBUK(empleado.DOI);

                                        if(fotoBUK == "") {

                                            if(System.IO.File.Exists(direccionFoto + "0_" + nro_documento.Trim() + ".bmp")) {
                                                byte[] bmp = ImageToBinary(direccionFoto + "0_" + nro_documento.Trim() + ".bmp");
                                                empleado.emp_foto = Convert.ToBase64String(bmp);
                                            } else {
                                                empleado.emp_foto = "";
                                            }

                                        } else {

                                            System.Net.WebRequest request = System.Net.WebRequest.Create(fotoBUK);
                                            System.Net.WebResponse response1 = request.GetResponse();
                                            System.IO.Stream responseStream = response1.GetResponseStream();
                                            Bitmap bmp_ = new Bitmap(responseStream);
                                            System.IO.MemoryStream ms = new MemoryStream();
                                            bmp_.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                            byte[] byteImage = ms.ToArray();
                                            empleado.emp_foto = Convert.ToBase64String(byteImage);
                                        }


                                    } else {

                                        empleado.emp_foto = "";
                                    }

                                    respuesta = true;
                                    mensaje = "Registro Empleado";
                                    return Json(new { respuesta, mensaje, data = empleado });
                                }

                            } else {
                                respuesta = false;
                                mensaje = "No se encontro registro de Empleado";

                            }
                        } else {
                            respuesta = false;
                            mensaje = "No se encontro registro de Empleado";
                        }
                    }
                }



            }
            catch (Exception exp)
            {
                mensaje = exp.Message + "";
            }

            return Json(new { respuesta, mensaje, mensajeConsola });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult LoginValdidarCredencialesJson(string usu_nombre, string usu_password,string token,string usu_imei)
        {
            bool respuesta = false;
            string mensaje = "";
            string mensajeConsola = "";
            SEG_UsuarioEntidad usuario = new SEG_UsuarioEntidad();
            SEG_EmpleadoEntidad empleado = new SEG_EmpleadoEntidad();
            bool respuestaConsulta = false;
            var direccionFoto = ubicacionarchivos + "/rostros/";
            var logubicacion = ubicacionarchivos + "/log/";
            //var direccion = Server.MapPath("/") + Request.ApplicationPath + "/reconocimientoFacial/rostros/";
            try
            {

                if (direccionFoto == "")
                {
                    respuesta = false;
                    mensaje = "Error; ubicación de archivos no registrada..";
                    return Json(new { respuesta, mensaje, mensajeConsola });
                }

                if (!Directory.Exists(direccionFoto))
                {
                    Directory.CreateDirectory(direccionFoto);
                }
                usuario = usuariobl.UsuarioCoincidenciaObtenerJson(usu_nombre, 0, 0);
               
                if (usuario.UsuarioID > 0)
                {
                    if (usuario.Estado == 1)
                    {
                        respuestaConsulta = PasswordHashTool.PasswordHashManager.ValidatePassword(usu_password, usuario.UsuarioContraseña);
                        if (respuestaConsulta)
                        {
                            empleado = empleadoBL.EmpleadoIdObtenerJson(usuario.EmpleadoID);
                            EmpleadoDispositivo dispoempleado = new EmpleadoDispositivo();
                            dispoempleado.emp_id = empleado.EmpleadoID;
                            dispoempleado.emd_firebaseid = token;
                            var tokenempleado = empleadoDispositivobl.EmpleadoDispositivoEditarFirebaseJson(dispoempleado);
                            if (logactivos == "1")
                            {
                                _log.escribir_logOK(logubicacion, " Login - Usuario : " + usuario.UsuarioNombre + " | imagen : " + direccionFoto + "" + empleado.emp_foto);
                            }

                            if (!string.IsNullOrEmpty(empleado.emp_foto))
                            {
                                if (empleado.emp_foto.Trim() != "")
                                {
                                    if (System.IO.File.Exists(direccionFoto + "" + empleado.emp_foto))
                                    {
                                        byte[] bmp = ImageToBinary(direccionFoto + "" + empleado.emp_foto);
                                        usuario.foto = Convert.ToBase64String(bmp);
                                    }

                                }
                                else
                                {
                                    usuario.foto = "";
                                }
                            }
                            else
                            {
                                usuario.foto = "";
                            }


                            if (!string.IsNullOrEmpty(usu_imei))
                            {

                                var empleadoDispositivo = empleadoDispositivobl.EmpleadoDispositivoemp_IdObtenerJson(empleado.EmpleadoID);
                                if (empleadoDispositivo != null)
                                {

                                    if (empleadoDispositivo.emd_imei != usu_imei)
                                    {

                                        mensaje = "Nuevo Dispositivo Detectado. Espere confirmacion del nuevo dispositivo, comuniquese con el area de soporte.";

                                        ControlImeiEntidad existe = controlImeiBL.ObtenerRegistroPendienteImei(empleado.EmpleadoID);

                                        if (existe.IdEmpleado == empleado.EmpleadoID)
                                        {

                                            existe.Imei = usu_imei;
                                            existe.FechaRegistro = DateTime.Now;

                                            var insertado = controlImeiBL.EditarNuevoImei(existe);

                                            if (!insertado)
                                            {
                                                mensaje = "Error al editar nuevo dispositivo, comuniquese con el area de soporte.";
                                            }

                                        }
                                        else
                                        {

                                            ControlImeiEntidad controlImeiEntidad = new ControlImeiEntidad();
                                            controlImeiEntidad.IdEmpleado = empleado.EmpleadoID;
                                            controlImeiEntidad.Imei = usu_imei;
                                            controlImeiEntidad.Estado = 0;
                                            controlImeiEntidad.FechaRegistro = DateTime.Now;

                                            var insertado = controlImeiBL.RegistrarNuevoImei(controlImeiEntidad);

                                            if (insertado == 0)
                                            {
                                                mensaje = "Error al registrar nuevo dispositivo, comuniquese con el area de soporte.";
                                            }

                                        }

                                        return Json(new { respuesta, mensaje, usuario });


                                    }

                                }

                            }

                            respuesta = true;
                            mensaje = "Bienvenido, " + usuario.UsuarioNombre;
                            return Json(new { respuesta, mensaje, usuario });
                        }
                        else
                        {
                            mensaje = "Contraseña no Coincide";
                        }
                    }
                    else
                    {
                        mensaje = "Usuario no se Encuentra Activo";
                    }
                }
                else
                {

                    mensaje = "Usuario no Existe";

                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + "";
            }

            return Json(new { respuesta, mensaje, mensajeConsola });
        }

        [HttpPost]
        public ActionResult EmpleadoEliminarImagenJson(int emp_id)
        {
            string mensaje = "";
            bool respuesta = false;
            string mensajeConsola = "";
            SEG_EmpleadoEntidad empleado = new SEG_EmpleadoEntidad();
            var direccionFoto = "/rostros/";

            try
            {
                if (ubicacionarchivos == "")
                {
                    respuesta = false;
                    mensaje = "Error; ubicación de archivos no registrada..";
                    return Json(new { respuesta, mensaje, mensajeConsola });
                }

                direccionFoto = ubicacionarchivos + direccionFoto;

                empleado = empleadoBL.EmpleadoIdObtenerJson(emp_id);

                if (empleado.EmpleadoID>0)
                {
                    
                    var imagen = empleado.emp_foto;
                    if (imagen != "")
                    {
                        var filePath = direccionFoto + imagen;
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }


                        try {
                            for(int i = 0; i < 4; i++) {
                                var filePaths = direccionFoto + i + imagen;
                                if(System.IO.File.Exists(filePaths)) {
                                    System.IO.File.Delete(filePaths);
                                }
                            }
                        } catch {

                        }

                        empleado.emp_foto = string.Empty;
                        var empleadoeditarfotoTupla = empleadoBL.EmpleadoFotoEditarJson(empleado);


                        respuesta = true;
                        mensaje = "Imagen Eliminada";
                    }
                    else
                    {
                        respuesta = true;
                        mensaje = "No se encontro imagen para eliminar";
                    }

                }
                else
                {
                    mensaje = "Error, no se Puede Eliminar";
                    mensajeConsola = "";
                }
            }
            catch (Exception exp)
            {
                respuesta = false;
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta, mensaje, mensajeConsola });
        }

        #region celular
        [seguridad(false)]
        [HttpPost]
        public ActionResult EmpleadoUbicacionAsistenciaJson(string latitud, string longitud)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<SalaEntidad> local = new List<SalaEntidad>();
            var dist = new List<dynamic>();
            var logubicacion = ubicacionarchivos + "/log/";



            SalaRegasist local_data = new SalaRegasist();
            calculosCoordenadas calculo = new calculosCoordenadas();
            string str_distancia = "";
            List<SalaRegasist> locallista = ObtenerSedePorDistancia(latitud, longitud);

            if(logactivos == "1") {
                _log.escribir_logOK(logubicacion, " Lista 0");
            }


            try {

                if(locallista.Count > 0) {

                    if(logactivos == "1") {
                        _log.escribir_logOK(logubicacion, " Lista 1");
                    }

                    List<SalaRegasist> listaDistancias = new List<SalaRegasist>();

                    locallista = locallista.Where(x => x.sal_latitud != null && x.sal_longitud != null).ToList();


                    foreach(var item in locallista) {
                        if(item.sal_latitud != "" && item.sal_longitud != "") {
                            try {

                                var lat1 = Convert.ToDouble(latitud.Trim().Replace(',', '.'));
                                var lng1 = Convert.ToDouble(longitud.Trim().Replace(',', '.'));
                                var lat2 = Convert.ToDouble(item.sal_latitud.Trim().Replace(',', '.'));
                                var lng2 = Convert.ToDouble(item.sal_longitud.Trim().Replace(',', '.'));

                                item.distancia = calculo.GetDistance(lng1, lat1, lng2, lat2);
                                listaDistancias.Add(item);
                                if(logactivos == "1") {
                                    _log.escribir_logOK(logubicacion, "Sala:" + item.sal_nombre + " Latitud:" + item.sal_latitud + " Longitud:" + item.sal_longitud);
                                }
                            } catch {

                            }
                        }
                    }


                    int cantidadconlatitudlongitud = listaDistancias.Count();
                    if(cantidadconlatitudlongitud == 0) {
                        mensaje = "No se registraron Latitud/Longitud de sede(s)";
                        respuesta = false;
                        return Json(new { respuesta, mensaje });
                    }


                    local_data = listaDistancias.OrderBy(x => x.distancia).FirstOrDefault();


                    if(local_data != null) {


                        if(logactivos == "1") {
                            _log.escribir_logOK(logubicacion, "Sala seleccionada:" + local_data.sal_nombre + " ID:" + local_data.sal_id + " Latitud:" + local_data.sal_latitud + " Longitud:" + local_data.sal_longitud);
                        }

                        if(local_data.distancia < 0) {

                            mensaje = "Error al obtener ubicacion de las sedes.";
                            respuesta = false;
                            return Json(new { respuesta, mensaje });
                        }

                        int distancia_local = calculo.round_up_to_even(local_data.distancia);

                        var distanciaCofiguracion = "";
                        if(distancia != "" && distancia != null) {
                            distanciaCofiguracion = distancia;
                        } else {
                            distanciaCofiguracion = "1000";
                        }

                        if(logactivos == "1") {
                            _log.escribir_logOK(logubicacion, "Distancia actual:" + distancia_local + " Distancia Configuracion:" + distanciaCofiguracion);
                        }

                        if(distanciaCofiguracion != "") {
                            if(Convert.ToInt32(distanciaCofiguracion) <= distancia_local) {
                                mensaje = "No se encuentra a una distancia aceptable (local cercano " + local_data.sal_nombre + "). Distancia actual: " + distancia_local;
                                respuesta = false;
                                return Json(new { respuesta, mensaje });
                            }
                        }

                        if(local_data.distancia > 0) {
                            str_distancia = ",distancia : " + distancia_local + " metros";
                        };

                        mensaje = " Ubicacion Actual : " + local_data.sal_nombre + " ¿es correcto? " + str_distancia;
                        respuesta = true;

                    }
                } else {

                    mensaje = "No se Pudo Obtener La Informacion de Local Seleccionado ";
                }

            } catch(Exception ex) {

                respuesta = false;
                mensaje = ex.Message + ",Llame Administrador";
            }

            return Json(new { data = local_data, respuesta, mensaje, mensajeConsola });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult EmpleadoRegistrarAsistenciaJson(int emp_logueoid, string emp_doi, int loc_empr_id, int loc_id, string imei, string latitud, string longitud)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;

            var logubicacion = ubicacionarchivos + "/log/";
            List<UsuarioSalaEntidad> empleadoLocal = new List<UsuarioSalaEntidad>();
            try
            {

                if(emp_doi.Trim() == "") {
                    mensaje = "No se envio DOI de Empleado";
                    return Json(new { respuesta, mensaje, mensajeconsola = mensajeConsola });

                } else {


                    EmpleadoDispositivo empDispositivo = new EmpleadoDispositivo();
                    empDispositivo = empleadoDispositivobl.EmpleadoDispositivoemp_IdObtenerJson(emp_logueoid);

                    if(empDispositivo.emd_id == 0) {
                        mensaje = "No tiene Dispositivo Registrado";
                        respuesta = false;
                        return Json(new { respuesta, mensaje });
                    } else {
                        if(empDispositivo.emd_imei != imei) {
                            if(logactivos == "1") {
                                _log.escribir_logOK(logubicacion, " EmpleadoRegistrarAsistenciaJson : imei no coincide - lat " + latitud + " ,long " + longitud + ", | imei : " + imei);
                            }
                            mensaje = "imei no Coincide";
                            return Json(new { respuesta, mensaje, mensajeConsola });
                        }

                    }

                    BitacoraMarcacion bitacora = new BitacoraMarcacion();
                    bitacora.sala_id = loc_id;
                    bitacora.bit_nrodocumento = emp_doi;
                    bitacora.bit_fechaHora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    bitacora.bit_mobile = true;
                    bitacora.bit_procedencia = 3; // Asistencia Manual

                    BitacoraMarcacionResponse serviceResponse = RegistroMarcacionAsistencia(bitacora);

                    respuesta = serviceResponse.respuesta;
                    mensaje = serviceResponse.message;

                    if(logactivos == "1") {
                        _log.escribir_logOK(logubicacion, " EmpleadoRegistrarAsistenciaJson : lat " + latitud + " ,long " + longitud + ", | imei : " + imei);
                    }


                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta, mensaje, mensajeConsola });
        }

        [seguridad(false)]
        [HttpPost]
        public async Task<ActionResult> EmpleadoRegistrarAsistencia_confotoJson(int emp_logueoid, string emp_doi, string imei, string foto, string latitud, string longitud) {

            ////https://stackoverflow.com/questions/27521043/comparing-detected-face-with-array-of-existing-faces-with-opencv-in-android
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            SEG_EmpleadoEntidad empleado = new SEG_EmpleadoEntidad();
            SalaEntidad local = new SalaEntidad();
            EmpleadoAsistencia empleadoAsistencia = new EmpleadoAsistencia();
            sp_SiscopBD valores = new sp_SiscopBD();
            Int64 IdAsistencia = 0;
            var logubicacion = ubicacionarchivos + "/log/";
            List<UsuarioSalaEntidad> empleadoLocal = new List<UsuarioSalaEntidad>();
            try {
                if(foto == "") {
                    mensaje = "No se envio Imagen de Empleado";
                    return Json(new { respuesta, mensaje, mensajeconsola = mensajeConsola });
                }
                try {

                } catch(Exception ex) {

                }

                SalaRegasist local_data = new SalaRegasist();
                calculosCoordenadas calculo = new calculosCoordenadas();
                string str_distancia = "";
                List<SalaRegasist> locallista = ObtenerSedePorDistancia(latitud, longitud);

                if(logactivos == "1") {
                    _log.escribir_logOK(logubicacion, " Lista 0");
                }


                try {

                    if(locallista.Count > 0) {

                        if(logactivos == "1") {
                            _log.escribir_logOK(logubicacion, " Lista 1");
                        }

                        List<SalaRegasist> listaDistancias = new List<SalaRegasist>();

                        locallista = locallista.Where(x => x.sal_latitud != null && x.sal_longitud != null).ToList();


                        foreach(var item in locallista) {
                            if(item.sal_latitud != "" && item.sal_longitud != "") {
                                try {

                                    var lat1 = Convert.ToDouble(latitud.Trim().Replace(',', '.'));
                                    var lng1 = Convert.ToDouble(longitud.Trim().Replace(',', '.'));
                                    var lat2 = Convert.ToDouble(item.sal_latitud.Trim().Replace(',', '.'));
                                    var lng2 = Convert.ToDouble(item.sal_longitud.Trim().Replace(',', '.'));

                                    item.distancia = calculo.GetDistance(lng1, lat1, lng2, lat2);
                                    listaDistancias.Add(item);
                                    if(logactivos == "1") {
                                        _log.escribir_logOK(logubicacion, "Sala:"+item.sal_nombre+" Latitud:"+item.sal_latitud+" Longitud:"+item.sal_longitud);
                                    }
                                } catch {

                                }
                            }
                        }


                        int cantidadconlatitudlongitud = listaDistancias.Count();
                        if(cantidadconlatitudlongitud == 0) {
                            mensaje = "No se registraron Latitud/Longitud de sede(s)";
                            respuesta = false;
                            return Json(new { respuesta, mensaje });
                        } 


                        local_data = listaDistancias.OrderBy(x => x.distancia).FirstOrDefault();


                        if(local_data != null) {


                            if(logactivos == "1") {
                                _log.escribir_logOK(logubicacion, "Sala seleccionada:" + local_data.sal_nombre + " ID:" + local_data.sal_id + " Latitud:" + local_data.sal_latitud + " Longitud:" + local_data.sal_longitud);
                            }

                            if(local_data.distancia < 0) {

                                mensaje = "Error al obtener ubicacion de las sedes.";
                                respuesta = false;
                                return Json(new { respuesta, mensaje });
                            }

                            int distancia_local = calculo.round_up_to_even(local_data.distancia);

                            var distanciaCofiguracion = "";
                            if(distancia != "" && distancia != null) {
                                distanciaCofiguracion = distancia;
                            }else {
                                distanciaCofiguracion = "1000";
                            }

                            if(logactivos == "1") {
                                _log.escribir_logOK(logubicacion, "Distancia actual:" + distancia_local + " Distancia Configuracion:" + distanciaCofiguracion);
                            }

                            if(distanciaCofiguracion != "") {
                                if(Convert.ToInt32(distanciaCofiguracion) <= distancia_local) {
                                    mensaje = "No se encuentra a una distancia aceptable (local cercano " + local_data.sal_nombre + "). Distancia actual: " + distancia_local;
                                    respuesta = false;
                                    return Json(new { respuesta, mensaje });
                                }
                            }

                            if(local_data.distancia > 0) {
                                str_distancia = ",distancia : " + distancia_local + " metros";
                            };

                            mensaje = " Ubicacion Actual : " + local_data.sal_nombre + " " + str_distancia;

                        }
                    } else
                    {
						mensaje = "Ocurrió un error inesperado. Intente nuevamente por favor. (Código error: 001)";
						respuesta = false;
						return Json(new { respuesta, mensaje, mensajeconsola = mensajeConsola });
					}

                } catch(Exception ex) {

                }


                if(emp_doi.Trim() == "") {
                    mensaje = "No se envio DOI de Empleado";
                    return Json(new { respuesta, mensaje, mensajeconsola = mensajeConsola });

                } else {

                    EmpleadoDispositivo empDispositivo = new EmpleadoDispositivo();
                    empDispositivo = empleadoDispositivobl.EmpleadoDispositivoemp_IdObtenerJson(emp_logueoid);
                    if(empDispositivo.emd_id == 0) {
                        mensaje = "No tiene Dispositivo Registrado";
                        respuesta = false;
                        return Json(new { respuesta, mensaje });
                    } else {
                        if(empDispositivo.emd_imei != imei) {
                            if(logactivos == "1") {
                                _log.escribir_logOK(logubicacion, " EmpleadoRegistrarAsistenciaJson : imei no coincide - lat " + latitud + " ,long " + longitud + ", | imei : " + imei);
                            }

                            mensaje = "imei no Coincide";
                            mensajeConsola = "";
                            return Json(new { respuesta, mensaje, mensajeConsola });
                        }

                    }

                    int porcentaje = 0;
                    if(reconocimiento == "1") {
                        Double coincidencia = 0;
                        if(logactivos == "1") {
                            _log.escribir_logOK(logubicacion, " EmpleadoRegistrarAsistencia_confotoJson | face activo : " + reconocimiento);
                        }

                        //FaceRecognizer LBPHrecognizer = new LBPHFaceRecognizer(1, 8, 8, 8, 100);//50
                        var fotos_etiqueta = new string[1];
                        var direccionFoto = ubicacionarchivos + "/rostros/" + emp_doi.Trim() + "/";

                        if(ubicacionarchivos == "") {
                            respuesta = false;
                            mensaje = "Error; ubicación de archivos no registrada..";
                            return Json(new { respuesta, mensaje, mensajeConsola });
                        }


                        var direccion = Server.MapPath("/") + Request.ApplicationPath;
                        //if(logactivos == "1") {
                        //    _log.escribir_logOK(logubicacion, " EmpleadoRegistrarAsistencia_confotoJson | archivo xml : " + direccion + "/Content/haarcascades/haarcascade_frontalface_default.xml");
                        //    _log.escribir_logOK(logubicacion, " EmpleadoRegistrarAsistencia_confotoJson | imagen empleado : " + direccionFoto + "" + empleado.emp_foto);

                        //}

                        //CascadeClassifier FaceDetection = new CascadeClassifier(direccion + "/Content/haarcascades/haarcascade_frontalface_default.xml");

                        //string fotoBUK = ObtenerFotoBUK(emp_doi);
                        //if(!fotoBUK.IsEmpty()) {

                        //    var trainingImages = new Image<Gray, Byte>[5];
                        //    var traininglabels = new int[5];
                        //    for(int i = 0; i < 4; i++) {
                        //        byte[] bmp = ImageToBinary(direccionFoto + i + "_" + emp_doi + ".bmp");
                        //        Bitmap bmp_Multiple = new Bitmap(ConvertByteToImg(bmp));
                        //        var faceImageMultiple = new Image<Gray, byte>(bmp_Multiple);
                        //        trainingImages[i] = faceImageMultiple.Resize(250, 250, Inter.Cubic);
                        //        traininglabels[i] = 1;
                        //    }

                        //    System.Net.WebRequest request = System.Net.WebRequest.Create(fotoBUK);
                        //    System.Net.WebResponse response = request.GetResponse();
                        //    System.IO.Stream responseStream = response.GetResponseStream();
                        //    Bitmap bmp_ = new Bitmap(responseStream);
                        //    var faceImage = new Image<Gray, byte>(bmp_);
                        //    trainingImages[4] = faceImage.Resize(250, 250, Inter.Cubic);
                        //    traininglabels[4] = 1;
                        //    LBPHrecognizer.Train(trainingImages, traininglabels);




                        //} else {

                        //    var trainingImages = new Image<Gray, Byte>[4];
                        //    var traininglabels = new int[4];
                        //    for(int i = 0; i < 4; i++) {
                        //        //byte[] bmp = ImageToBinary(direccionFoto + i + "_" + emp_doi + ".bmp");
                        //        byte[] bmp = ImageToBinary(direccionFoto + i + "_" + emp_doi + ".bmp");
                        //        Bitmap bmp_Multiple = new Bitmap(ConvertByteToImg(bmp));
                        //        var faceImageMultiple = new Image<Gray, byte>(bmp_Multiple);
                        //        trainingImages[i] = faceImageMultiple.Resize(250, 250, Inter.Cubic);
                        //        traininglabels[i] = 1;
                        //    }

                        //    LBPHrecognizer.Train(trainingImages, traininglabels);

                        //}                        

                        //byte[] imagenCelular = Convert.FromBase64String(foto.Trim());
                        //Bitmap imgcelular = new Bitmap(ConvertByteToImg(imagenCelular));
                        //Image<Bgr, Byte> ImagecelularMat = new Image<Bgr, Byte>(imgcelular);
                        //var proccesedImage = ImagecelularMat.Convert<Gray, byte>().Resize(250, 250, Inter.Cubic);
                        //proccesedImage._EqualizeHist();
                        //var result = LBPHrecognizer.Predict(proccesedImage);
                        //coincidencia = Math.Round(result.Distance, 0);

                        //var url = "http://127.0.0.1:5000/upload";
                        var url = urlReconocimiento + "/upload";
                        var data = new {
                            directorio_fotos = direccionFoto,
                            image = foto
                        };
                        double distance = 0;

                        using(HttpClient client = new HttpClient()) {
                            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                            StringContent contenido = new StringContent(jsonData, Encoding.UTF8, "application/json");
                            HttpResponseMessage response = await client.PostAsync(url, contenido);
                            if(response.IsSuccessStatusCode) {
                                string jsonRespuesta = await response.Content.ReadAsStringAsync();
                                var datos = JsonConvert.DeserializeObject<List<ApiFotosData>>(jsonRespuesta);
                                if(datos.Count > 0) {
                                    distance = datos[0].distance;
                                }
                            } else {
                                respuesta = false;
                                mensaje = "Error Api Fotos " + response.StatusCode;
                                return Json(new { respuesta, mensaje, mensajeConsola });
                            }
                        }
                        coincidencia = Math.Round(distance*100, 0);


                        //if(result.Label > 0) {
                        if(distance != 0) {
                            porcentaje = 100 - Convert.ToInt32(coincidencia);
                            if(logactivos == "1") {
                                _log.escribir_logOK(logubicacion, " EmpleadoRegistrarAsistencia_confotoJson | Coincidencia : " + coincidencia + " en Porcentaje :" + porcentaje + " % calibracion config " + calibracion);
                            }
                            int coincidenciawebconfig = Convert.ToInt32(calibracion);//40;  60%
                            if(porcentaje > coincidenciawebconfig) {

                                try {

                                    BitacoraMarcacion bitacora = new BitacoraMarcacion();
                                    bitacora.sala_id = local_data.sal_id;
                                    bitacora.bit_nrodocumento = emp_doi;
                                    bitacora.bit_fechaHora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                    bitacora.bit_mobile = true;
                                    bitacora.bit_procedencia = 2; // Asistencia Camara

                                    BitacoraMarcacionResponse serviceResponse = RegistroMarcacionAsistencia(bitacora);

                                    respuesta = serviceResponse.respuesta;
                                    mensaje = serviceResponse.message;
                                    mensaje = mensaje + " , Porcentaje : " + porcentaje + " %.";

                                } catch(Exception ex) {

                                    mensaje = "Error al registrar la marcacion asistencia";
                                    respuesta = false;
                                }

                            } else {
                                respuesta = false;
                                mensaje = "Rostro No Concuerda , intentelo denuevo ,Aproximacion :" + porcentaje + " %";
                                return Json(new { respuesta, mensaje, mensajeConsola });
                            }
                        } else {
                            respuesta = false;
                            mensaje = "Rostro No Concuerda , intentelo denuevo ";
                            return Json(new { respuesta, mensaje, mensajeConsola });
                        }
                    }


                }

            } catch(Exception exp) {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta, mensaje, mensajeConsola });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult EmpleadoRegistrarRostroJson(string emp_doi, string foto)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            SEG_EmpleadoEntidad empleado = new SEG_EmpleadoEntidad();
            //var direccion = Server.MapPath("/") + Request.ApplicationPath + "/reconocimientoFacial/rostros/";
            var direccionFoto = ubicacionarchivos + "/rostros/"+ emp_doi+"/";

            try
            {

                if (ubicacionarchivos == "")
                {
                    respuesta = false;
                    mensaje = "Error; ubicación de archivos no registrada..";
                    return Json(new { respuesta, mensaje, mensajeConsola });
                }

                if (foto == "")
                {
                    mensaje = "Imagen Vacia";
                    return Json(new { respuesta, mensaje, mensajeConsola, foto });
                }

                if (!Directory.Exists(direccionFoto))
                {
                    Directory.CreateDirectory(direccionFoto);
                }

                byte[] imagen = Convert.FromBase64String(foto.Trim());
                Bitmap imgcelular = new Bitmap(ConvertByteToImg(imagen));


                imgcelular.Save(direccionFoto + "0_" + emp_doi + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                //MODIFY IMAGE
                // Cargue una imagen en una instancia de Imagen
                using(Aspose.Imaging.Image image = Aspose.Imaging.Image.Load(direccionFoto + "0_" + emp_doi + ".bmp")) {
                    // Transmitir a imagen ráster
                    RasterImage rasterImage = (RasterImage)image;

                    // Cache RasterImage para un mejor rendimiento
                    if(!rasterImage.IsCached) {
                        rasterImage.CacheData();
                    }

                    // Ajustar el contraste
                    rasterImage.AdjustContrast(30);

                    // Guardar imagen
                    image.Save(direccionFoto + "1_" + emp_doi + ".bmp");
                }
                // Cargue una imagen en una instancia de Imagen
                using(Aspose.Imaging.Image image = Aspose.Imaging.Image.Load(direccionFoto + "0_" + emp_doi + ".bmp")) {
                    // Transmitir a imagen ráster
                    RasterImage rasterImage = (RasterImage)image;

                    // Cache RasterImage para un mejor rendimiento
                    if(!rasterImage.IsCached) {
                        rasterImage.CacheData();
                    }

                    // Ajustar el brillo
                    rasterImage.AdjustBrightness(70);

                    // Guardar imagen
                    image.Save(direccionFoto + "2_" + emp_doi + ".bmp");
                }
                // Cargue una imagen en una instancia de Imagen
                using(Aspose.Imaging.Image image = Aspose.Imaging.Image.Load(direccionFoto + "0_" + emp_doi + ".bmp")) {
                    // Transmitir a imagen ráster
                    RasterImage rasterImage = (RasterImage)image;

                    // Cache RasterImage para un mejor rendimiento
                    if(!rasterImage.IsCached) {
                        rasterImage.CacheData();
                    }

                    // Ajustar gama
                    rasterImage.AdjustGamma(2.2f, 2.2f, 2.2f);

                    // Guardar imagen
                    image.Save(direccionFoto + "3_" + emp_doi + ".bmp");
                }


                imgcelular.Dispose();
                mensaje = "Se Registró Imagen Correctamente";
                respuesta = true;

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje, mensajeConsola });
        }

        [seguridad(false)]
        public String Enviar_sp_SiscopBD_SP_Insertar_Marcacion_CP(sp_SiscopBD valores)
        {
            string errormensaje = "";
            //bool respuestaConsulta = false;

            try
            {
                //ConfiguracionEmpresa configuracionArchivos = new ConfiguracionEmpresa();
                //var ubicacionArchivos = configuracionEmpresabl.ConfiguracionEmpresaXconf_IdObtenerJson(4);//archivos;
                //configuracionArchivos = ubicacionArchivos.configuracionEmpresa;
                //_log.escribir_logOK(configuracionArchivos.cone_valor, "valores : " + valores.ToString());
                //var asistenciaTareajeotupla = tareajeModelbl.Enviar_DataServidor(valores);
                //_log.escribir_logOK(configuracionArchivos.cone_valor, "resultado : " + asistenciaTareajeotupla.envioTareaje_sp);
                //error = asistenciaTareajeotupla.error;
                //if (error.Key.Equals(string.Empty))
                //{
                //    respuestaConsulta = asistenciaTareajeotupla.envioTareaje_sp;
                //    errormensaje = "Tareaje Enviado";
                //}
                //else
                //{
                //    errormensaje = "Error, no se Puede Enviar a BD";
                //    mensajeConsola = error.Value;
                //}
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return "ok";
        }
        #endregion

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
        public System.Drawing.Image ConvertByteToImg(Byte[] img)
        {
            //Image FetImg;
            //MemoryStream ms = new MemoryStream(img);
            //FetImg = Image.FromStream(ms);
            //ms.Close();
            //return FetImg;
            using (var stream = new MemoryStream(img))
            {
                return System.Drawing.Image.FromStream(stream);
            }
        }

        [HttpPost]
        public ActionResult EmpleadoDispositivoIdObtenerJson(int emp_id)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            EmpleadoDispositivo empleadoDispositivo = new EmpleadoDispositivo();
            try
            {
                empleadoDispositivo = empleadoDispositivobl.EmpleadoDispositivoemp_IdObtenerJson(emp_id);
                if (empleadoDispositivo.emd_id>0)
                {
                    mensaje = "Obteniendo Informacion de Dispositivo";
                    respuesta = true;
                }
                else
                {
                    respuesta = true;
                    mensajeConsola = "";
                    mensaje = "No se Pudo Obtener La Informacion de Dispositivo Seleccionada";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = empleadoDispositivo, respuesta, mensaje, mensajeConsola });
        }

        [HttpPost]
        public ActionResult EmpleadoDispositivoNuevoJson(EmpleadoDispositivo empleadoDispositivo)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            Int64 idEmpleadoDispositivoInsertado = 0;

            try
            {
                idEmpleadoDispositivoInsertado = empleadoDispositivobl.EmpleadoDispositivoInsertarJson(empleadoDispositivo);
                if (idEmpleadoDispositivoInsertado>0)
                {
                    mensaje = "Se Registró Correctamente";
                    respuesta = true;
                   
                }
                else
                {
                    mensaje = "No se Pudo Registrar IMEI";
                    mensajeConsola = "";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje, mensajeConsola });
        }

        [HttpPost]
        public ActionResult EmpleadoDispositivoEditarJson(EmpleadoDispositivo empleadoDispositivo)
        {
            string mensaje = "";
            bool respuesta = false;
            string mensajeConsola = "";
            try
            {
                respuesta = empleadoDispositivobl.EmpleadoDispositivoEditarJson(empleadoDispositivo);
                if (respuesta)
                {
                    mensaje = "Se Editó Dispositivo Correctamente";
                }
                else
                {
                    mensajeConsola = "";
                    mensaje = "Error, no se Puede Editar";
                }
            }
            catch (Exception exp)
            {
                mensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje, mensajeConsola });
        }

        [seguridad(false)]
        [HttpPost]
        public async Task<ActionResult> EnviarPosicionremoto(CodigoUsuarioGet registro)
        {
            string mensaje = "";
            string mensajeConsola = "";
            ServiceResponse respuesta = new ServiceResponse();

            try
            {

                string url = urlAsistencia + "/api/Empleado/SendCodigoPosicion";
                var json = JsonConvert.SerializeObject(registro);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                var client = new HttpClient();
                var response = await client.PostAsync(url, stringContent);
                response.EnsureSuccessStatusCode();
                var contenidoRespuesta = await response.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                respuesta = JsonConvert.DeserializeObject<ServiceResponse>(contenidoRespuesta, settings);
                mensaje = respuesta.message;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message + " ,Llame Administrador";
            }
            return Json(new { respuesta= respuesta.response, mensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public async Task<ActionResult> EnvioMarcacionAsistencia(BitacoraMarcacion registro)
        {
            string mensaje = "";
            string mensajeConsola = "";
            ServiceResponse respuesta = new ServiceResponse();

            try
            {

                string url = urlAsistencia + "/api/Empleado/SaveMarcacionMobil";
                var json = JsonConvert.SerializeObject(registro);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                var client = new HttpClient();
                var response = await client.PostAsync(url, stringContent);
                response.EnsureSuccessStatusCode();
                var contenidoRespuesta = await response.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                respuesta = JsonConvert.DeserializeObject<ServiceResponse>(contenidoRespuesta, settings);
                mensaje = respuesta.message;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message + " ,Llame Administrador";
            }
            return Json(new { respuesta = respuesta.response, mensaje });
        }
        //[seguridad(false)]
        //[HttpPost]
        //public async Task<ActionResult> RegistroMarcacionAsistencia(BitacoraMarcacion registro)
        //{
        //    string mensaje = "";
        //    string mensajeConsola = "";
        //    ServiceResponse respuesta = new ServiceResponse();

        //    try
        //    {

        //        string url = urlAsistencia + "/api/Bitacora/CreateMarcacion";
        //        var json = JsonConvert.SerializeObject(registro);
        //        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
        //        var client = new HttpClient();
        //        var response = await client.PostAsync(url, stringContent);
        //        response.EnsureSuccessStatusCode();
        //        var contenidoRespuesta = await response.Content.ReadAsStringAsync();
        //        var settings = new JsonSerializerSettings
        //        {
        //            NullValueHandling = NullValueHandling.Ignore,
        //            MissingMemberHandling = MissingMemberHandling.Ignore
        //        };
        //        respuesta = JsonConvert.DeserializeObject<ServiceResponse>(contenidoRespuesta, settings);
        //        mensaje = respuesta.message;
        //    }
        //    catch (Exception ex)
        //    {
        //        mensaje = ex.Message + " ,Llame Administrador";
        //    }
        //    return Json(new { respuesta = respuesta.response, mensaje });
        //}
        [seguridad(false)]
        [HttpPost]
        public ActionResult EditarFirebaseJson(EmpleadoDispositivo dispoempleado)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            try
            {
                respuesta = empleadoDispositivobl.EditarFirebaseJson(dispoempleado);
                if(respuesta)
                {
                    mensaje = "Obteniendo Informacion de Dispositivo";
                    respuesta = true;
                }
                else
                {
                    respuesta = true;
                    mensajeConsola = "";
                    mensaje = "No se Pudo Obtener La Informacion de Dispositivo Seleccionada";
                }

            }
            catch(Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta, mensaje, mensajeConsola });
        }

        private BitacoraMarcacionResponse RegistroMarcacionAsistencia(BitacoraMarcacion registro) {

            string mensaje = "";
            string mensajeConsola = "";
            BitacoraMarcacionResponse respuesta = new BitacoraMarcacionResponse();

            try {

                var json = JsonConvert.SerializeObject(registro);
                string url = urlAsistencia + "/api/Bitacora/CreateMarcacionMobile";
                var client = new RestClient(url);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Accept", "application/json");
                request.AddJsonBody(json);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                IRestResponse response = client.Execute(request);

                if(response.StatusCode == HttpStatusCode.OK) {

                    respuesta = JsonConvert.DeserializeObject<BitacoraMarcacionResponse>(response.Content);
                }
            } catch(Exception ex) {
                respuesta.respuesta = false;
                respuesta.message = ex.Message + " ,Llame Administrador";
            }
            return respuesta;
        }

        private string ObtenerFotoBUK(string dni) {

            string foto = string.Empty;

            try {

                string urlBUK = "https://gladcon.buk.pe/api/v1/peru";
                string ApiKey = "JiyarLYVsLnbfTbSL8NrnX1v";
                string url = urlBUK + "/employees?document_number=" + dni;
                var client = new RestClient(url);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Auth_token", ApiKey);
                //request.AddParameter("dni", dni);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                IRestResponse response = client.Execute(request);

                if(response.StatusCode == HttpStatusCode.OK) {

                    dynamic dataJson = JsonConvert.DeserializeObject(response.Content);
                    if(dataJson.data.ToString().Trim().TrimStart('{').TrimEnd('}') != "[]") {

                        dynamic data = JsonConvert.DeserializeObject<dynamic>(dataJson.data.ToString().Trim().TrimStart('{').TrimEnd('}'));
                        
                        foto = data[0].picture_url;

                        if(foto == null)
                        {
                            foto = string.Empty;
                        }

                    }
                }

            } catch(Exception ex) {

                foto = string.Empty;
            }

            return foto;

        }

        private List<SalaRegasist> ObtenerSedePorDistancia( string latitud, string longitud) {
            string mensaje = "";
            string mensajeConsola = "";
            ServiceResponse respuesta = new ServiceResponse();

            List<SalaRegasist> salas = new List<SalaRegasist>();
            try {

                SalaRegasist registro = new SalaRegasist();
                registro.sal_latitud = latitud;
                registro.sal_longitud = longitud;

                var json = JsonConvert.SerializeObject(registro);
                string url = urlAsistencia + "/api/Sala/GetSalaMasCercana";
                var client = new RestClient(url);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Accept", "application/json");
                request.AddJsonBody(json);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                IRestResponse response = client.Execute(request);

                if(response.StatusCode == HttpStatusCode.OK) {

                    SalaRegasistResponse salaRegasist = JsonConvert.DeserializeObject<SalaRegasistResponse>(response.Content);

                    if(salaRegasist.success) {

                        salas = salaRegasist.data;

                    } else {
                        salaRegasist.data = new List<SalaRegasist>();
                    }

                }
            } catch(Exception ex) {
                mensaje = ex.Message + " ,Llame Administrador";
            }

            return salas;
        }


        [seguridad(false)]
        [HttpPost]
        public ActionResult EliminarFotoAzure(string nro_documento) {
            bool response = false;
            string message = "";

            var direccionFoto = ubicacionarchivos + "/rostros/" + nro_documento.Trim() + "/";

            try {
                if(nro_documento == "") {
                    message = "Error. Nro de Documento Obligatorio.";
                    return Json(new { response, message });
                }

                if(Directory.Exists(direccionFoto)) {

                    Directory.Delete(direccionFoto, true);
                    response = true;
                    message = "Foto eliminada correctamente.";

                } else {

                    message = "No tiene foto registrada.";
                }



            } catch(Exception exp) {
                response = false;
                message = exp.Message + "";
            }

            return Json(new { response, message });
        }

    }
}
