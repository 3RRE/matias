using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaDatos;
using System.Threading;

namespace CapaPresentacion.Controllers.GestionCambios
{
    [seguridad]
    public class GestionCambiosController : Controller
    {
        private ModuloBL modulobl = new ModuloBL();
        private GC_SistemaBL gcSistemaBl = new GC_SistemaBL();
        private SolicitudCambioBL solicitudbl = new SolicitudCambioBL();
        private TipoCambioBL tipocambiobl = new TipoCambioBL();
        
        private SEG_EmpleadoBL segcEmpleadoBL = new SEG_EmpleadoBL();
        private HistorialSolicitudCambioBL historialbl = new HistorialSolicitudCambioBL();

        private GC_ComiteCambiosBL gcComiteCambiosBl = new GC_ComiteCambiosBL();
        private EstadoSolicitudCambioBL estadobl = new EstadoSolicitudCambioBL();
        // GET: GestionCambios
        public ActionResult GestionCambiosSistemaListadoVista()
        {
            return View("~/Views/GestionCambios/GestionCambiosSistemaListadoVista.cshtml");
        }
        public ActionResult GestionCambiosSistemaInsertarVista()
        {
            return View("~/Views/GestionCambios/GestionCambiosSistemaInsertarVista.cshtml");
        }

        public ActionResult GestionCambiosSistemaEditarVista(string id)
        {
            int sub = Convert.ToInt32(id.Replace("Registro", ""));
            var errormensaje = "";
            var sistema = new Sistema();
            try
            {
                sistema = gcSistemaBl.GestionCambiosSistemaEditarJson(sub);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                errormensaje = "Verifique conexion,Llame Administrador";
            }
            ViewBag.sistema = sistema;
            ViewBag.errormensaje = errormensaje;
            return View("~/Views/GestionCambios/GestionCambiosSistemaEditarVista.cshtml");
        }
        public ActionResult GestionCambiosComiteCambiosPersonalListadoVista()
        {
            return View("~/Views/GestionCambios/GestionCambiosComiteCambiosPersonalListadoVista.cshtml");
        }

        #region Listado de Los empleados existentes activos
        [HttpPost]
        public ActionResult ComiteCambiosEmpleadoListadoJson()
        {
            var errormensaje = "";
            var lista = new List<SEG_EmpleadoEntidad>();
            try
            {
                lista = segcEmpleadoBL.EmpleadoEstadoActivoListarJson();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }

            return Json(new { data = lista.ToList(), mensaje = errormensaje });
            //  var aa = lista.ToList();

        }
        #endregion

        [HttpPost]
        public ActionResult ComiteCambiosListadoJson()
        {
            var errormensaje = "";
            var lista = new List<ComiteCambios>();
            try
            {
                lista = gcComiteCambiosBl.ComiteCambiosListadoJson();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }

            return Json(new { data = lista.ToList(), mensaje = errormensaje });
            //  var aa = lista.ToList();

        }
        #region Listado Los Sistemas Existentes
        [HttpPost]
        public ActionResult SistemaListadoJson()
        {
            var errormensaje = "";
            var lista = new List<Sistema>();
            try
            {
                lista = gcSistemaBl.SistemaListadoJson();

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }

            return Json(new { data = lista.ToList(), mensaje = errormensaje });
            //  var aa = lista.ToList();

        }
        #endregion

        [HttpPost]
        public ActionResult SistemaGuardarJson(Sistema sistema)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                sistema.Estado = true;   
                sistema.FechaRegistro = DateTime.Now;
                sistema.FechaModificacion = DateTime.Now;
                

                respuestaConsulta = gcSistemaBl.SistemaGuardarJson(sistema);
                
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult ComiteCambiosGuardarJson(FormCollection frm)
        {

            var errormensaje = "";
            bool respuestaConsulta = false;
            bool respuestaConsulta2 = false;
            ComiteCambios comite = new ComiteCambios();

            //string proyecto_id = frmcollection.Get("proyecto_id");
            string[] personal_id = frm.GetValues("cmbpersonal[]");
            
            try
            {
                respuestaConsulta2 = gcComiteCambiosBl.ComiteCambioActualizarJson();
                foreach (string personalId in personal_id)
                {
                    comite = new ComiteCambios();
                    comite.EmpleadoID= Int32.Parse(personalId);
                    comite.Estado=true;

                    respuestaConsulta = gcComiteCambiosBl.ComiteCambioGuardarJson(comite);
                    
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }
            Thread.Sleep(1500);
           
            return RedirectToAction("../GestionCambios/GestionCambiosComiteCambiosPersonalListadoVista");
            //return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        public ActionResult GestionCambiosListadoModuloVista()
        {
            return View();
        }

        public ActionResult GestionCambiosNuevoModuloVista()
        {
            return View();
        }

        public ActionResult GestionCambiosEditarModuloVista(string id)
        {
            int sub = Convert.ToInt32(id.Replace("Editar", ""));
            var errormensaje = "";
            var modulo = new ModuloEntidad();
            try
            {
                modulo = modulobl.ModuloIdObtenerJson(sub);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                errormensaje = "Verifique conexion,Llame Administrador";
            }
            ViewBag.modulo = modulo;
            ViewBag.errormensaje = errormensaje;
            return View();
        }

        public ActionResult GestionCambiosSolicitudCambioVista()
        {
            int validt = 0;

            SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];

            if (gcComiteCambiosBl.VerificarEmpleadoComiteJson(usuario.EmpleadoID))
            {
                validt = 1;
            }

            ViewBag.validar = validt;
            return View();
        }
        public ActionResult GestionCambiosNuevoSolicitudCambioVista()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CargarListaModuloJson()
        {
            var errormensaje = "";
            var lista = new List<ModuloEntidad>();
            try
            {
                lista = modulobl.ModuloListarJson();

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            var listado = from list in lista
                          orderby list.ModuloId descending
                          select new
                          {
                              list.ModuloId,
                              list.SistemaDescripcion,
                              list.SistemaID,
                              list.Descripcion,
                              list.FechaRegistro,
                              list.FechaModificacion,
                              estadomodulo = list.Estado.ToString() == "1" ? "Habilitado" : "Deshabilitado"
                          };
            return Json(new { data = listado.ToList(), mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult CargarListaSistemasJson()
        {
            var errormensaje = "";
            var lista = new List<Sistema>();
            try
            {
                lista = gcSistemaBl.SistemaListadoJson();

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult ModuloGuardarJson(ModuloEntidad moduloEntidad)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                moduloEntidad.Estado = 1;
                moduloEntidad.FechaRegistro = DateTime.Now;
                moduloEntidad.FechaModificacion = DateTime.Now;
                respuestaConsulta = modulobl.ModuloGuardarJson(moduloEntidad);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult SistemaEditarJson(Sistema sistema)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                sistema.FechaModificacion = DateTime.Now;
                respuestaConsulta = gcSistemaBl.SistemaEditarJson(sistema);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult EstadoSistemaActualizarJson(int SistemaId, int Estado)
        {
            var errormensaje = "Accion realizada Correctamente.";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = gcSistemaBl.EstadoSistemaActualizarJson(SistemaId, Estado);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        public ActionResult ModuloActualizarJson(ModuloEntidad moduloEntidad)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                moduloEntidad.FechaModificacion = DateTime.Now;
                respuestaConsulta = modulobl.ModuloActualizarJson(moduloEntidad);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        public ActionResult ObtenerListaSolicitudCambioJson(string id_sala,string fechaInicio, string fechaFin, string tipoCambio, string estadoSolicitudCambioId)
        {
            var errormensaje = "";
            //var newvar = Convert.ToDateTime(fechaInicio).Date.End.ToString("s");
            var lista = new List<SolicitudCambioEntidad>();
            //fechaInicio = fechaInicio + " 00:00:00";
            //fechaFin = fechaFin+ " 23:59:59";
            var fechIn = Convert.ToDateTime(fechaInicio).Date;
            var fechFin = Convert.ToDateTime(fechaFin);
            fechFin = fechFin.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];
            try
            {
                if (gcComiteCambiosBl.VerificarEmpleadoComiteJson(usuario.EmpleadoID))
                {
                    lista = solicitudbl.BuscarSolicitudCambioJson(id_sala, fechIn, fechFin, tipoCambio,estadoSolicitudCambioId);
                }
                else
                {
                    lista = solicitudbl.BuscarSolicitudCambioNoComite(id_sala, fechIn, fechFin, usuario.EmpleadoID,tipoCambio,estadoSolicitudCambioId);
                }

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }

        public ActionResult BuscarModuloSistemaJson(int id)
        {
            var errormensaje = "";
            var lista = new List<ModuloEntidad>();
            try
            {
                    lista = modulobl.BuscarModuloSistemaJson(id);                
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }

        public ActionResult ListarTipoCambioJson()
        {
            var errormensaje = "";
            var lista = new List<TipoCambioEntidad>();

            try
            {
                lista = tipocambiobl.ListaTipoCambioJson();

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }

        public ActionResult GuardarSolicitudCambioBorradorJson(SolicitudCambioEntidad solicitudCambio)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];
            try
            {
                solicitudCambio.FechaRegistro = DateTime.Now;
                solicitudCambio.EstadoSolicitudCambioId = 1;
                solicitudCambio.SolicitanteId = usuario.EmpleadoID;
                solicitudCambio.ImpactoId = 1;
                respuestaConsulta = solicitudbl.GuardarBorradorSolicitudCambioJson(solicitudCambio);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        public ActionResult EnviarSolicitudCambioJson(SolicitudCambioEntidad solicitudCambio)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;

            SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];
            try
            {
                solicitudCambio.FechaRegistro = DateTime.Now;
                solicitudCambio.EstadoSolicitudCambioId = 2;
                solicitudCambio.SolicitanteId = usuario.EmpleadoID;
                solicitudCambio.ImpactoId = 1;
                respuestaConsulta = solicitudbl.EnviarSolicitudCambioJson(solicitudCambio);

                var solicitud = new SolicitudCambioEntidad();

                solicitud = solicitudbl.ObtenerUltimaSolicitudUsuarioLogeado(usuario.EmpleadoID);


                HistorialSolicitudCambioEntidad historial = new HistorialSolicitudCambioEntidad();
                historial.SolicitudId = solicitud.SolicitudId;
                historial.EstadoSolicitudId = solicitud.EstadoSolicitudCambioId;
                historial.FechaRegistro = DateTime.Now;
                historial.AprobadorPor = usuario.EmpleadoID;
                historial.FechaRespuesta = DateTime.Now;
                historialbl.RegistrarHistorialSolicitudCambio(historial);


            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        public ActionResult ObtenerBusquedaHistorialSolicitudCambioJson(int id)
        {
            var errormensaje = "";
            var lista = new List<HistorialSolicitudCambioEntidad>();

            try
            {
                lista = historialbl.BusquedaHistorialSolicitudCambioJson(id);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }

        public ActionResult GestionCambiosHistorialSolicitudModal(int id)
        {
            ViewBag.id_solicitud = id;
            return PartialView();
        }

        public ActionResult GestionCambioLineaTiempoHistorialSolicitudVista(int id)
        {
            ViewBag.id_solicitud = id;
            return View();
        }

        public ActionResult ListarEstadoSolicitudCambioJson()
        {
            var errormensaje = "";
            var lista = new List<EstadoSolicitudCambioEntidad>();

            try
            {
                lista = estadobl.EstadoSolicitudCambioListadoJson();

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }

        public ActionResult GestionCambiosEditarSolicitudCambioVista(int id)
        {
            var errormensaje = "";
            var solicitud = new SolicitudCambioEntidad();
            try
            {
                solicitud = solicitudbl.ObtenerSolicitudIdJson(id);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                errormensaje = "Verifique conexion,Llame Administrador";
            }

            int validt = 0;

            SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];

            if (gcComiteCambiosBl.VerificarEmpleadoComiteJson(usuario.EmpleadoID))
            {
                validt = 1;
            }

            ViewBag.solicitud = solicitud;
            ViewBag.validar = validt;
            ViewBag.errormensaje = errormensaje;
            return View();
        }

        public ActionResult GestionCambiosModificarSolicitudCambioJson(SolicitudCambioEntidad solicitud,HistorialSolicitudCambioEntidad historial)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];
            try
            {
                if (gcComiteCambiosBl.VerificarEmpleadoComiteJson(usuario.EmpleadoID))
                {
                    respuestaConsulta = solicitudbl.SolicitudCambioActualizarJson(solicitud);

                    historial.SolicitudId = solicitud.SolicitudId;
                    historial.EstadoSolicitudId = solicitud.EstadoSolicitudCambioId;
                    historial.FechaRegistro = DateTime.Now;
                    historial.AprobadorPor = usuario.EmpleadoID;
                    historial.FechaRespuesta = DateTime.Now;
                    historialbl.RegistrarHistorialSolicitudCambio(historial);
                }
                else
                {
                    respuestaConsulta = solicitudbl.SolicitudCambioActualizarJson(solicitud);
                }
                

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        public ActionResult GestionCambiosEliminarSolicitudCambioJson(int id)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = solicitudbl.SolicitudCambioEliminarJson(id);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
    }
}