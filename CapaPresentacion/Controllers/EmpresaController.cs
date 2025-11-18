using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    [seguridad]
    public class EmpresaController : Controller
    {
        private readonly EmpresaBL _empresaBl = new EmpresaBL();
        private readonly UbigeoBL ubigeoBL = new UbigeoBL();

        // GET: Empresa
        public ActionResult EmpresaVista()
        {
            return View();
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListadoEmpresa()
        {
            var errormensaje = "";
            var lista = new List<EmpresaEntidad>();
            try
            {
                lista = _empresaBl.ListadoEmpresa();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmpresaInsertarVista()
        {
            return View();
        }

        public ActionResult EmpresaModificarVista(string id)
        {
            int sub = Convert.ToInt32(id);
            var errormensaje = "";
            var empresa = new EmpresaEntidad();
            UbigeoEntidad ubigeo = new UbigeoEntidad();
            try
            {
                empresa = _empresaBl.EmpresaObtenerporIdJson(sub);
                empresa.RutaArchivoLogoAnt = empresa.RutaArchivoLogo;
                ubigeo = ubigeoBL.GetDatosUbigeo(empresa.CodUbigeo);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                errormensaje = "Verifique conexion,Llame Administrador";
            }
            ViewBag.empresa = empresa;
            ViewBag.errormensaje = errormensaje;
            ViewBag.ubigeo = ubigeo;
            return View();
        }

        [HttpPost]
        public ActionResult InsertarEmpresaJson(EmpresaEntidad empresa)
        {
            string mensaje = "No se pudo insertar el registro";
            bool respuesta = false;

            HttpPostedFileBase file = Request.Files[0];
            int tamanioMaximo = 4194304;
            string extension = "";
            string rutaInsertar = "";
            string direccion = Server.MapPath("/") + Request.ApplicationPath + "/Uploads/LogosEmpresas/";

            //int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            try
            {

                if (file.ContentLength > 0 && file != null)
                {
                    if (file.ContentLength <= tamanioMaximo)
                    {
                        extension = Path.GetExtension(file.FileName).ToLower();
                        if (extension == ".jpg" || extension == ".png" || extension == ".jpeg")
                        {
                            string nombreArchivo = "Logo_"+empresa.Ruc +"_"+ DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                            rutaInsertar = Path.Combine(direccion, nombreArchivo);
                            if (!Directory.Exists(direccion))
                            {
                                System.IO.Directory.CreateDirectory(direccion);
                            }
                            file.SaveAs(rutaInsertar);
                            empresa.RutaArchivoLogo= nombreArchivo;
                        }
                        else
                        {
                            mensaje = "Solo se permiten archivos .jpg, .word o jpeg.";
                        }
                    }
                }
                else
                {
                    mensaje = "Debe seleccionar Un logo para la Empresa";
                    return Json(new { respuesta,mensaje});
                }
                empresa.FechaRegistro = DateTime.Now;
                empresa.FechaModificacion = DateTime.Now;
                empresa.Estado = 1;
                empresa.Activo = true;
                respuesta = _empresaBl.InsertarEmpresaJson(empresa);
                if (respuesta)
                {
                    mensaje = "Registro Insertado";
                }
                else
                {
                    System.IO.File.Delete(rutaInsertar);
                }
            }
            catch (Exception ex)
            {
                mensaje += ex.Message;
            }
            return Json(new { respuesta, mensaje });
        }

        [HttpPost]
        public JsonResult EmpresaModificarJson(EmpresaEntidad empresa)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            HttpPostedFileBase file = Request.Files[0];
            int tamanioMaximo = 4194304;
            string extension = "";
            string rutaInsertar = "";
            string rutaAnterior = "";
            string direccion = Server.MapPath("/") + Request.ApplicationPath + "/Uploads/LogosEmpresas/";
            try
            {
                empresa.FechaModificacion = DateTime.Now;
                if (file.ContentLength > 0 && file != null)
                {
                    if (file.ContentLength <= tamanioMaximo)
                    {
                        extension = Path.GetExtension(file.FileName).ToLower();
                        if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                        {
                            var nombreArchivo = ("Logo_" +empresa.Ruc+"_"+ DateTime.Now.ToString("yyyyMMddHHmmss") + extension);
                            rutaInsertar = Path.Combine(direccion, nombreArchivo);

                            rutaAnterior = Path.Combine(direccion + empresa.RutaArchivoLogoAnt);
                            if (!Directory.Exists(direccion))
                            {
                                System.IO.Directory.CreateDirectory(direccion);
                            }
                            if (System.IO.File.Exists(rutaAnterior))
                            {
                                System.IO.File.Delete(rutaAnterior);
                            }
                            file.SaveAs(rutaInsertar);
                            empresa.RutaArchivoLogo= nombreArchivo;
                        }
                        else
                        {
                            errormensaje = "Solo se adminten archivos .jpg, .jpeg o .png";
                            return Json(new { mensaje=errormensaje,respuesta=respuestaConsulta});
                        }
                    }
                    else
                    {
                        errormensaje = "El tamaño maximo de arhivo permitido es de 4Mb.";
                        return Json(new { mensaje = errormensaje, respuesta = respuestaConsulta });
                    }
                }
                else
                {
                    empresa.RutaArchivoLogo= empresa.RutaArchivoLogoAnt;
                }
                empresa.FechaModificacion = DateTime.Now;
                empresa.Estado = 1;
                empresa.Activo = true;
                respuestaConsulta = _empresaBl.EmpresaModificarJson(empresa);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult GetDataSelects(UbigeoEntidad ubigeo)
        {
            List<UbigeoEntidad> listaUbigeo = new List<UbigeoEntidad>();
            int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
            string mensaje = "No se pudieron listar los registros";
            bool respuesta = false;
            object oRespuesta = new object();
            List<UbigeoEntidad> listaProvincias = new List<UbigeoEntidad>();
            List<UbigeoEntidad> listaDistritos = new List<UbigeoEntidad>();
            try
            {

                listaUbigeo = ubigeoBL.ListadoDepartamento();
                if (ubigeo.CodUbigeo != 0)
                {
                    listaProvincias = ubigeoBL.GetListadoProvincia(ubigeo.DepartamentoId);
                    listaDistritos = ubigeoBL.GetListadoDistrito(ubigeo.ProvinciaId, ubigeo.DepartamentoId);
                    oRespuesta = new
                    {
                        dataUbigeo = listaUbigeo,
                        dataProvincias = listaProvincias,
                        dataDistritos = listaDistritos
                    };
                }
                else
                {
                    oRespuesta = new
                    {
                        dataUbigeo = listaUbigeo,
                    };
                }

                respuesta = true;
                mensaje = "Listando registros";
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data = oRespuesta });
        }

        #region Ubigeo
        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoDepartamento()
        {
            string mensaje = "";
            bool respuesta = false;
            List<UbigeoEntidad> lista = new List<UbigeoEntidad>();
            try
            {
                lista = ubigeoBL.ListadoDepartamento();
                mensaje = "Listando Registros";
                respuesta = true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                respuesta = false;
            }
            return Json(new { mensaje, respuesta, data = lista });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoProvincia(int DepartamentoID)
        {
            string mensaje = "";
            bool respuesta = false;
            List<UbigeoEntidad> lista = new List<UbigeoEntidad>();
            try
            {
                lista = ubigeoBL.GetListadoProvincia(DepartamentoID);
                mensaje = "Listando Registros";
                respuesta = true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                respuesta = false;
            }
            return Json(new { mensaje, respuesta, data = lista });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListadoDistrito(int ProvinciaID, int DepartamentoID)
        {
            string mensaje = "";
            bool respuesta = false;
            List<UbigeoEntidad> lista = new List<UbigeoEntidad>();
            try
            {
                lista = ubigeoBL.GetListadoDistrito(ProvinciaID, DepartamentoID);
                mensaje = "Listando Registros";
                respuesta = true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                respuesta = false;
            }
            return Json(new { mensaje, respuesta, data = lista });
        }

        #endregion
    }
}