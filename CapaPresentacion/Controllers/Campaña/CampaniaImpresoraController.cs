using CapaEntidad;
using CapaEntidad.Campañas;
using CapaNegocio;
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
    [seguridad]
    public class CampaniaImpresoraController : Controller
    {
        private CMP_impresoraBL impresoracampaniabl = new CMP_impresoraBL();
        private CMP_impresora_usuarioBL impresorausuariobl = new CMP_impresora_usuarioBL();
        private SEG_UsuarioBL seg_usuariobl = new SEG_UsuarioBL();
        public ActionResult ListadoImpresoras()
        {
            return View("~/Views/Campania/ListadoImpresora.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarImpresorasJson()
        {
            var errormensaje = "";
            var lista = new List<CMP_impresoraEntidad>();
           
            try
            {
               
                lista = impresoracampaniabl.ImpresoraListadoCompletoJson();
                
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarImpresorasxUsuarioidJson(Int64 codsala)
        {
            var errormensaje = "";
            var lista = new List<CMP_impresoraEntidad>();

            try
            {
                Int64 usuario_id = Convert.ToInt64(Session["UsuarioID"]); ;
                lista = impresoracampaniabl.GetListadoxSala_idxUsuarioid(codsala, usuario_id);
                //lista = GetListadoxSala_idxUsuarioidV2(codsala, usuario_id,UrlProgresivoSala);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarImpresorasServerJson(DtParameters dtParameters)
        {
            var errormensaje = "";
            IEnumerable<CMP_impresoraEntidad> lista = new List<CMP_impresoraEntidad>();
            List<dynamic> registro = new List<dynamic>();
            var count = 0;
            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            try
            {

                if (dtParameters.Order != null)
                {
                    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
                }
                else
                {
                    orderCriteria = "id";
                    orderAscendingDirection = false;
                }

                lista = impresoracampaniabl.ImpresoraListadoCompletoJson().ToList();
                count = lista.Count();


                if (!string.IsNullOrEmpty(searchBy) && !(string.IsNullOrWhiteSpace(searchBy)))
                {
                    lista = lista.Where(x => x.id.ToString()== searchBy
                                                  || x.nombre.ToLower().Contains(searchBy.ToLower())
                                                  || x.sala_nombre.ToLower().Contains(searchBy.ToLower())
                                                  || x.ip.ToString().Contains(searchBy.ToLower())
                                                  || x.puerto.ToString().Contains(searchBy.ToLower())
                                                  ).ToList();
                }
                
                lista = orderAscendingDirection ? lista.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc).ToList() : lista.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc).ToList();

                var filteredResultsCount = count;
                var totalResultsCount = lista.Count();

                registro.Add(new { draw = dtParameters.Draw , recordsTotal = totalResultsCount, recordsFiltered = filteredResultsCount, data = lista.Skip(dtParameters.Start)
                    .Take(dtParameters.Length).ToList()
                }) ;
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(registro.FirstOrDefault(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ImpresoraNuevoVista()
        {
            return View("~/Views/Campania/ImpresoraInsertarVista.cshtml");
        }

        [HttpPost]
        public ActionResult InsertarImpresoraJson(CMP_impresoraEntidad impresora)
        {
            string mensaje = "No se pudo insertar el registro";
            bool respuesta = false;
            Int64 id = 0;

            //int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            try
            {
                
                impresora.estado = 1;
               
                id = impresoracampaniabl.ImpresoraInsertarJson(impresora);
                if (id>0)
                {
                    respuesta = true;
                    mensaje = "Registro Insertado";
                }
                else
                {
                    respuesta = false;
                    mensaje = "Error al Registrar";
                }
            }
            catch (Exception ex)
            {
                mensaje += ex.Message;
            }
            return Json(new { respuesta, mensaje });
        }

        public ActionResult ImpresoraModificarVista(string id)
        {
            int sub = Convert.ToInt32(id);
            var errormensaje = "";
            var impresora = new CMP_impresoraEntidad();
          
            try
            {
                impresora = impresoracampaniabl.ImpresoraIdObtenerJson(sub);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                errormensaje = "Verifique conexion,Llame Administrador";
            }
            ViewBag.errormensaje = errormensaje;
            ViewBag.impresora = impresora;
            return View("~/Views/Campania/ImpresoraModificarVista.cshtml");
        }

        [HttpPost]
        public JsonResult ImpresoraModificarJson(CMP_impresoraEntidad impresora)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = impresoracampaniabl.ImpresoraEditarJson(impresora);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        public ActionResult AsignarImpresoras()
        {
            return View("~/Views/Campania/ImpresoraUsuarioVista.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ImpresoraSalaUsuarioAsignadoListarJson(Int64 codsala)
        {
            var errormensaje = "";
            var lista = new List<CMP_impresora_usuarioEntidad>();
            var listaImpresora = new List<CMP_impresoraEntidad>();
            var listaUsuario = new List<SEG_UsuarioEntidad>();
            List<object> listaFinal = new List<object>();
            try
            {
                lista = impresorausuariobl.ImpresoraUsuarioListadoCompletoJson();
                listaImpresora = impresoracampaniabl.ImpresoraListadoxSala_idJson(codsala);
                listaUsuario = seg_usuariobl.UsuarioListadoJson();

                foreach (var registro in listaImpresora)
                {
                    List<object> usuarios = new List<object>();
                    foreach (var registrousuario in listaUsuario.OrderBy(x=>x.UsuarioNombre))
                    {
                        Int64 id = 0;
                        var reg = lista.Where(x => x.usuario_id == registrousuario.UsuarioID && x.impresora_id == registro.id).FirstOrDefault();
                        if (reg != null)

                        {
                            id = reg.id;
                        }

                        usuarios.Add(new
                        {
                            id,
                            registrousuario.UsuarioID,
                            registrousuario.UsuarioNombre,
                        });
                    }
                    //var registr = listaalerta.Where(x => x.cargo_id == registro.CargoID && x.sala_id==registro.).FirstOrDefault();
                    //registro.alt_id = registr.alt_id;
                    listaFinal.Add(new
                    {
                        registro.id,
                        registro.nombre,
                        registro.sala_id,
                        registro.ip,
                        registro.puerto,
                        usuarios
                    }); ;
                }



            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = listaFinal.ToList(), mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult ImpresoraUsuarioGuardarJson(CMP_impresora_usuarioEntidad impresorausuario)
        {
            var errormensaje = "";
            Int64 respuestaConsulta = 0;
            bool respuesta = false;
            try
            {        
                respuestaConsulta = impresorausuariobl.ImpresoraUsuarioInsertarJson(impresorausuario);

                if (respuestaConsulta > 0)
                {
                    respuesta = true;
                    errormensaje = "Registro Guardado Correctamente";
                }
                else
                {
                    errormensaje = "error al Guardar Registro , LLame Administrador";
                    respuesta = false;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, id = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult ImpresoraUsuarioQuitarJson(int id)
        {
            var errormensaje = "";
            bool respuesta = false;
            try
            {
                respuesta = impresorausuariobl.ImpresoraUsuarioEliminarJson(id);
                if (respuesta)
                {
                    respuesta = true;
                    errormensaje = "Se quitó el Registro Correctamente";
                }
                else
                {
                    errormensaje = "error al Quitar Registros , LLame Administrador";
                    respuesta = false;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje });
        }


        [seguridad(false)]
        [HttpPost]
        public JsonResult ListausuarioimpresoraJson()
        {
            var errormensaje = "";
            var lista = new List<CMP_impresora_usuarioEntidad>();

            try
            {

                lista = impresorausuariobl.ImpresoraUsuarioListadoCompletoJson();

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }



        [seguridad(false)]
        public List<CMP_impresoraEntidad> GetListadoxSala_idxUsuarioidV2(int codsala, int usuario_id,string UrlProgresivoSala)
        {
            List<CMP_impresoraEntidad> listaRespuesta = new List<CMP_impresoraEntidad>();
            object oEnvio = new object();
            if(string.IsNullOrEmpty(UrlProgresivoSala)) {
                return (listaRespuesta); ;
            }
            try
            {
                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                oEnvio = new
                {
                    codsala = codsala,
                    usuario_id = usuario_id
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(oEnvio);
                string mensaje = "";
                bool respuesta = false;
                var client = new System.Net.WebClient();
                var response = "";
                client.Headers.Add("content-type", "application/json");
                client.Encoding = Encoding.UTF8;
                string url = UrlProgresivoSala;
                url += "/servicio/GetListadoxSala_idxUsuarioidV2";
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
                        CMP_impresoraEntidad contador = new CMP_impresoraEntidad()
                        {
                            id = myItem.id,
                            sala_id = myItem.sala_id,
                            sala_nombre = myItem.sala_nombre,
                            nombre = myItem.nombre,
                            ip = myItem.ip,
                            puerto = myItem.puerto,
                            estado = myItem.estado
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

    }
}
