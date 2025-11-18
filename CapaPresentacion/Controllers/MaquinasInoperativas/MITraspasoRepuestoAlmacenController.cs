using CapaDatos.MaquinasInoperativas;
using CapaEntidad;
using CapaEntidad.MaquinasInoperativas;
using CapaNegocio.MaquinasInoperativas;
using CapaPresentacion.Utilitarios;
using S3k.Utilitario;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.MaquinasInoperativas
{
    [seguridad]
    public class MITraspasoRepuestoAlmacenController : Controller
    {

        private MI_TraspasoRepuestoAlmacenBL repuestoBL = new MI_TraspasoRepuestoAlmacenBL();
        private MI_PiezaRepuestoAlmacenBL piezaRepuestoAlmacenBL = new MI_PiezaRepuestoAlmacenBL();
        private MI_TraspasoRepuestoAlmacenBL traspasoRepuestoAlmacenBL = new MI_TraspasoRepuestoAlmacenBL();
        private MI_MaquinaInoperativaBL maquinaInoperativaBL = new MI_MaquinaInoperativaBL();
        private MI_MaquinaInoperativaRepuestosBL maquinaInoperativaRepuestosBL = new MI_MaquinaInoperativaRepuestosBL();


        public ActionResult ListadoTraspasoRepuestoAlmacen()
        {
            return View("~/Views/MaquinasInoperativas/ListadoTraspasoRepuestoAlmacen.cshtml");
        }

        [HttpPost]
        public JsonResult ListarTraspasoRepuestoAlmacenJson()
        {
            var errormensaje = "";
            var lista = new List<MI_TraspasoRepuestoAlmacenEntidad>();

            try
            {

                lista = repuestoBL.TraspasoRepuestoAlmacenListadoCompletoJson();

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ListarTraspasoRepuestoAlmacenJsonxMaquinaInoperativa(int cod)
        {
            var errormensaje = "";
            var lista = new List<MI_TraspasoRepuestoAlmacenEntidad>();

            try {
                bool estadoAlmacenes = Convert.ToBoolean(ValidationsHelper.GetValueAppSettingDB("EstadoAlmacenes", false));

                if(estadoAlmacenes) {

                    lista = repuestoBL.TraspasoRepuestoAlmacenListadoCompletoxMaquinaInoperativaJson(cod);

                } else {

                    lista = repuestoBL.TraspasoRepuestoAlmacenListadoCompletoxMaquinaInoperativaJsonSinAlmacenes(cod);

                }

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarTraspasoRepuestoAlmacenActiveJson()
        {
            var errormensaje = "";
            bool respuesta = false;
            var lista = new List<MI_TraspasoRepuestoAlmacenEntidad>();

            try
            {

                lista = repuestoBL.TraspasoRepuestoAlmacenListadoActiveJson();
                respuesta = true;

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
                respuesta = false;
            }
            return Json(new { data = lista.ToList(), respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarTraspasoRepuestoAlmacenInactiveJson()
        {
            var errormensaje = "";
            bool respuesta = false;
            var lista = new List<MI_TraspasoRepuestoAlmacenEntidad>();

            try
            {

                lista = repuestoBL.TraspasoRepuestoAlmacenListadoActiveJson();
                respuesta = true;

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
                respuesta = false;
            }
            return Json(new { data = lista.ToList(), respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarTraspasoRepuestoAlmacenCodJson(int codTraspasoRepuestoAlmacen)
        {
            var errormensaje = "";
            bool respuesta = false;
            MI_TraspasoRepuestoAlmacenEntidad item = new MI_TraspasoRepuestoAlmacenEntidad();

            try
            {

                item = repuestoBL.TraspasoRepuestoAlmacenCodObtenerJson(codTraspasoRepuestoAlmacen);
                respuesta = true;

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
                respuesta = false;
            }
            return Json(new { data = item, respuesta = respuesta, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult TraspasoRepuestoAlmacenEditarJson(MI_TraspasoRepuestoAlmacenEntidad repuesto)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            repuesto.FechaModificacion = DateTime.Now;
            try
            {
                respuestaConsulta = repuestoBL.TraspasoRepuestoAlmacenEditarJson(repuesto);

                if (respuestaConsulta)
                {

                    errormensaje = "Registro de TraspasoRepuestoAlmacen Actualizado Correctamente";
                }
                else
                {
                    errormensaje = "Error al Actualizar TraspasoRepuestoAlmacen , LLame Administrador";
                    respuestaConsulta = false;
                }

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult TraspasoRepuestoAlmacenGuardarJson(MI_TraspasoRepuestoAlmacenEntidad repuesto)
        {
            var errormensaje = "";
            Int64 respuestaConsulta = 0;
            bool respuesta = false;
            try
            {
                SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];
                //repuesto.CodMaquinaInoperativa = 0;
                repuesto.FechaRegistro = DateTime.Now;
                repuesto.FechaModificacion = DateTime.Now;
                repuesto.CodUsuarioRemitente = usuario.UsuarioID;
                repuesto.CodUsuarioDestinatario = 0;
                repuesto.Estado = 0;
                
                bool resp = piezaRepuestoAlmacenBL.AgregarCantidadPendientePiezaRepuestoAlmacen( repuesto.CodPiezaRepuestoAlmacen, repuesto.Cantidad);

                if (resp)
                {

                    respuestaConsulta = repuestoBL.TraspasoRepuestoAlmacenInsertarJson(repuesto);
                } else
                {

                    errormensaje = "Error cantidad pendiente no se pudo asignar";
                }

                

                if (respuestaConsulta > 0)
                {
                    respuesta = true;
                    errormensaje = "Registro de TraspasoRepuestoAlmacen Guardado Correctamente";
                }
                else
                {
                    errormensaje = "Error al Crear TraspasoRepuestoAlmacen , LLame Administrador";
                    respuesta = false;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuesta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult TraspasoRepuestoAlmacenEliminarJson(int cod)
        {
            var errormensaje = "";
            bool respuesta = false;

            try
            {
                respuesta = repuestoBL.TraspasoRepuestoAlmacenEliminarJson(cod);
                if (respuesta)
                {
                    respuesta = true;
                    errormensaje = "Se quitó el TraspasoRepuestoAlmacen Correctamente";
                }
                else
                {
                    errormensaje = "Error al Quitar el TraspasoRepuestoAlmacen , LLame Administrador";
                    respuesta = false;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult TraspasoRepuestoAlmacenAceptarJson(MI_TraspasoRepuestoAlmacenEntidad repuesto, int codSala)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            bool respuesta = false;
            try
            {

                SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];
                bool estadoAlmacenes = Convert.ToBoolean(ValidationsHelper.GetValueAppSettingDB("EstadoAlmacenes", false));
                repuesto.FechaModificacion = DateTime.Now;
                repuesto.CodUsuarioDestinatario = usuario.UsuarioID;

                if(estadoAlmacenes) {


                    repuesto = repuestoBL.TraspasoRepuestoAlmacenCodObtenerJson(repuesto.CodTraspasoRepuestoAlmacen);
                    repuesto.FechaModificacion = DateTime.Now;
                    repuesto.CodUsuarioDestinatario = usuario.UsuarioID;

                    bool respuesta1 = piezaRepuestoAlmacenBL.AceptarDescontarCantidadPendientePiezaRepuestoAlmacen(repuesto.CodPiezaRepuestoAlmacen, repuesto.Cantidad);

                    List<MI_PiezaRepuestoAlmacenEntidad> listaRepuestosAlmacen = piezaRepuestoAlmacenBL.GetAllPiezaRepuestoAlmacenxRepuestoxAlmacen(repuesto.CodAlmacenDestino);

                    var entidad = listaRepuestosAlmacen.FirstOrDefault(x=>x.CodPiezaRepuesto==repuesto.CodRepuesto);

                    if (entidad != null)
                    {

                        respuesta = piezaRepuestoAlmacenBL.AceptarAgregarCantidadPendientePiezaRepuestoAlmacen(entidad.CodPiezaRepuestoAlmacen, repuesto.Cantidad);

                    } else
                    {

                        MI_PiezaRepuestoAlmacenEntidad piezaRepuestoAlmacen = new MI_PiezaRepuestoAlmacenEntidad();

                        piezaRepuestoAlmacen.CodPiezaRepuesto = repuesto.CodRepuesto;
                        piezaRepuestoAlmacen.CodTipo = 2;
                        piezaRepuestoAlmacen.CodAlmacen = repuesto.CodAlmacenDestino;
                        piezaRepuestoAlmacen.Cantidad = repuesto.Cantidad;
                        piezaRepuestoAlmacen.FechaRegistro = DateTime.Now;
                        piezaRepuestoAlmacen.FechaModificacion = DateTime.Now;
                        piezaRepuestoAlmacen.Estado = 1;

                        int res = piezaRepuestoAlmacenBL.PiezaRepuestoAlmacenInsertarJson(piezaRepuestoAlmacen);

                        if (res > 0)
                        {
                            respuesta = true;
                            errormensaje = "Registro de Categoria Pieza Guardado Correctamente";
                        }
                        else
                        {
                            errormensaje = "Error al Crear Categoria Pieza , LLame Administrador";
                            respuesta = false;
                            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
                        }

                        errormensaje = "No existe repuesto en almacen destino";

                    }

                    if (respuesta)
                    {
                        respuestaConsulta = repuestoBL.TraspasoRepuestoAlmacenAceptarJson(repuesto);
                        respuestaConsulta = maquinaInoperativaRepuestosBL.AceptarTraspasoRepuestoAlmacen(repuesto);
                    }
                    else
                    {
                        errormensaje = "Error con la cantidad pendiente";
                        respuestaConsulta = false;
                        return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
                    }

                } else {

                    respuestaConsulta = repuestoBL.TraspasoRepuestoAlmacenAceptarJson(repuesto);
                    respuestaConsulta = maquinaInoperativaRepuestosBL.AceptarTraspasoRepuestoAlmacen(repuesto);
                }

                if (respuestaConsulta)
                {

                    errormensaje = "Registro de TraspasoRepuestoAlmacen Actualizado Correctamente";



                    //Problemas Maquina Inoperativa
                    /*
                    var listaProblemasArray = new List<MI_MaquinaInoperativaProblemasEntidad>();
                    listaProblemasArray = maquinaInoperativaProblemasBL.MaquinaInoperativaProblemasListadoxMaquinaInoperativaJson(repuesto.CodMaquinaInoperativa);
                    */

                    List<MI_TraspasoRepuestoAlmacenEntidad> listaAux = new List<MI_TraspasoRepuestoAlmacenEntidad>();

                    if(estadoAlmacenes) {
                        listaAux = traspasoRepuestoAlmacenBL.GetAllTraspasoRepuestoAlmacenxCodMaquinaInoperativa(repuesto.CodMaquinaInoperativa, 0);
                    } else {
                        listaAux = traspasoRepuestoAlmacenBL.GetAllTraspasoRepuestoAlmacenxCodMaquinaInoperativaSinAlmacenes(repuesto.CodMaquinaInoperativa, 0);
                    }

                    bool cambioEstado = false;

                    MI_MaquinaInoperativaEntidad maquinaInoperativa = maquinaInoperativaBL.MaquinaInoperativaCodObtenerJson(repuesto.CodMaquinaInoperativa);

                    if (maquinaInoperativa != null)
                    {

                        maquinaInoperativa.FechaAtendidaInoperativaAprobado = DateTime.Now;
                        maquinaInoperativa.CodUsuarioAtendidaInoperativaAprobado = usuario.UsuarioID;

                        cambioEstado = maquinaInoperativaBL.AprobarSolicitudMaquinaInoperativa(maquinaInoperativa);
                        if (!cambioEstado)
                        {

                            errormensaje = "Error al actualizar usuario y fecha aprobacion de maquina inoperativa";
                        }
                    }

                    cambioEstado= false;

                    if (listaAux.Count == 0)
                    {

                        cambioEstado = maquinaInoperativaBL.AtencionPendienteEditarMaquinaInoperativa(repuesto.CodMaquinaInoperativa);



                        if (!cambioEstado)
                        {

                            errormensaje = "Error al cambiar estado de maquina inoperativa";
                        }
                        else
                        {
                            MIMaquinaInoperativaController correo = new MIMaquinaInoperativaController();

                            string uribase = "http://" + Request.Url.Authority + Request.ApplicationPath + "/";
                            correo.EnviarCorreoMaquinaInoperativa(codSala, 4, uribase);
                            errormensaje = "Correos enviados";
                        }


                    }

                }
                else
                {
                    errormensaje = "Error al Actualizar TraspasoRepuestoAlmacen , LLame Administrador";
                    respuestaConsulta = false;
                }

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult AtencionPendienteEditarMaquinaInoperativa(MI_TraspasoRepuestoAlmacenEntidad repuesto)
        {
            string mensaje = "Error al atender de todas formas";
            bool cambioEstado = false;
            cambioEstado = maquinaInoperativaBL.AtencionPendienteEditarMaquinaInoperativa(repuesto.CodMaquinaInoperativa);

            if (cambioEstado)
            {
                mensaje = "Atendido de todas formas exitosamente";
            }
            
            return Json(new { respuesta = cambioEstado, mensaje = mensaje });
        }

        [HttpPost]
        public ActionResult TraspasoRepuestoAlmacenRechazarJson(MI_TraspasoRepuestoAlmacenEntidad repuesto, int codSala)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {

                SEG_UsuarioEntidad usuario = (SEG_UsuarioEntidad)Session["usuario"];
                bool estadoAlmacenes = Convert.ToBoolean(ValidationsHelper.GetValueAppSettingDB("EstadoAlmacenes", true));
                repuesto.FechaModificacion = DateTime.Now;
                repuesto.CodUsuarioDestinatario = usuario.UsuarioID;


                if(estadoAlmacenes) {

                    repuesto = repuestoBL.TraspasoRepuestoAlmacenCodObtenerJson(repuesto.CodTraspasoRepuestoAlmacen);
                    repuesto.FechaModificacion = DateTime.Now;
                    repuesto.CodUsuarioDestinatario = usuario.UsuarioID;

                    bool respuesta = piezaRepuestoAlmacenBL.RechazarCantidadPendientePiezaRepuestoAlmacen(repuesto.CodPiezaRepuestoAlmacen,repuesto.Cantidad);

                    if (respuesta)
                    {
                        respuestaConsulta = repuestoBL.TraspasoRepuestoAlmacenRechazarJson(repuesto);
                        respuestaConsulta = maquinaInoperativaRepuestosBL.RechazarTraspasoRepuestoAlmacen(repuesto);    

                    } else
                    {
                        errormensaje = "Error con la cantidad pendiente";
                        respuestaConsulta = false;
                        return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
                    }

                } else {

                    respuestaConsulta = repuestoBL.TraspasoRepuestoAlmacenRechazarJson(repuesto);
                    respuestaConsulta = maquinaInoperativaRepuestosBL.RechazarTraspasoRepuestoAlmacen(repuesto);

                }

                if (respuestaConsulta)
                {

                    errormensaje = "Registro de TraspasoRepuestoAlmacen Actualizado Correctamente";

                    List<MI_TraspasoRepuestoAlmacenEntidad> listaAux = new List<MI_TraspasoRepuestoAlmacenEntidad>();

                    if(estadoAlmacenes) {
                        listaAux = traspasoRepuestoAlmacenBL.GetAllTraspasoRepuestoAlmacenxCodMaquinaInoperativa(repuesto.CodMaquinaInoperativa, 0);
                    } else {
                        listaAux = traspasoRepuestoAlmacenBL.GetAllTraspasoRepuestoAlmacenxCodMaquinaInoperativaSinAlmacenes(repuesto.CodMaquinaInoperativa, 0);
                    }


                    bool cambioEstado = false;


                    MI_MaquinaInoperativaEntidad maquinaInoperativa = maquinaInoperativaBL.MaquinaInoperativaCodObtenerJson(repuesto.CodMaquinaInoperativa);

                    if (maquinaInoperativa != null)
                    {

                        maquinaInoperativa.FechaAtendidaInoperativaAprobado = DateTime.Now;
                        maquinaInoperativa.CodUsuarioAtendidaInoperativaAprobado = usuario.UsuarioID;

                        cambioEstado = maquinaInoperativaBL.AprobarSolicitudMaquinaInoperativa(maquinaInoperativa);
                        if (!cambioEstado)
                        {

                            errormensaje = "Error al actualizar usuario y fecha aprobacion de maquina inoperativa";
                        }
                    }

                    cambioEstado= false;

                    if (listaAux.Count == 0)
                    {

                        cambioEstado = maquinaInoperativaBL.AtencionPendienteEditarMaquinaInoperativa(repuesto.CodMaquinaInoperativa);

                        if (!cambioEstado)
                        {

                            errormensaje = "Error al cambiar estado de maquina inoperativa";
                        }
                        else
                        {
                            MIMaquinaInoperativaController correo = new MIMaquinaInoperativaController();
                            string uribase = "http://" + Request.Url.Authority + Request.ApplicationPath + "/";
                            correo.EnviarCorreoMaquinaInoperativa(codSala, 4,uribase);
                            errormensaje = "Correos enviados";
                        }


                    }
                }
                else
                {
                    errormensaje = "Error al Actualizar TraspasoRepuestoAlmacen , LLame Administrador";
                    respuestaConsulta = false;
                }

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
    }
}