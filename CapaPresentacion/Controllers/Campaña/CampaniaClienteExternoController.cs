using CapaDatos.Utilitarios;
using CapaEntidad.AsistenciaCliente;
using CapaNegocio;
using CapaNegocio.AsistenciaCliente;
using CapaPresentacion.Models;
using CapaPresentacion.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Campaña
{
    [seguridad(false)]
    public class CampaniaClienteExternoController : Controller
    {
        SalaBL salaBL = new SalaBL();
        AST_ClienteBL ast_ClienteBL = new AST_ClienteBL();
        // GET: CampaniaClienteExterno
        public ActionResult Index(int CodSala=0)
        {
            var sala = salaBL.SalaListaIdJson(CodSala);
            ViewBag.CodSala = CodSala;
            ViewBag.UrlProgresivo = sala.UrlProgresivo;
            ViewBag.NombreSala = sala.Nombre;
            return View("~/Views/Campania/SesionClienteExterno.cshtml");
        }
        [HttpPost]
        public ActionResult GuardarClienteJson(AST_ClienteEntidad cliente)
        {
            var errormensaje = "";
            int respuestaConsulta = 0;
            bool respuesta = false;
            AST_ClienteEntidad clienteregistro = new AST_ClienteEntidad();
            try
            {
                var result = ast_ClienteBL.GetClientexNroyTipoDoc(cliente.TipoDocumentoId, cliente.NroDoc);
                if (result.Id > 0)
                {
                    errormensaje = "Cliente ya Registrado";
                    return Json(new { respuesta, mensaje = errormensaje });
                }
                cliente.NombreCompleto = cliente.Nombre + " " + cliente.ApelPat + " " + cliente.ApelMat;
                cliente.usuario_reg = 0;
                cliente.TipoRegistro = "SORTEOSALA";

                cliente.FechaRegistro = DateTime.Now;
                cliente.Estado = "A";
                cliente.TipoRegistro = "SORTEOSALA";
                cliente.usuario_reg = 0;
                respuestaConsulta = ast_ClienteBL.GuardarClienteSorteoExterno(cliente);
                clienteregistro = ast_ClienteBL.GetClienteID(respuestaConsulta);
                if (respuestaConsulta > 0)
                {
                    respuesta = true;
                    errormensaje = "Registro Guardado Correctamente";
                }
                else
                {
                    errormensaje = "error al crear , LLame Administrador";
                    respuesta = false;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta, mensaje = errormensaje, cliente = clienteregistro });
        }
        [HttpPost]
        public async Task<ActionResult> ObtenerCliente(int tipodocumento = 1, string nroDocumento = "", int CodSala = 0) {
            bool reniec = false;
            bool respuesta = false;
            try
            {
                var result = ast_ClienteBL.GetClientexNroyTipoDoc(tipodocumento, nroDocumento);
                if (result.Id > 0)
                {
                    respuesta = true;
                    return Json(new { data = result, respuesta,reniec});
                }
                if (tipodocumento == 1)
                {
                    var dataClienteAPI = await apireniec(nroDocumento);
                    if (Convert.ToString(dataClienteAPI[0].dni) != string.Empty)
                    {
                        var cliente = new AST_ClienteEntidad();
                        cliente.NroDoc = Convert.ToString(dataClienteAPI[0].dni);
                        cliente.Nombre = Convert.ToString(dataClienteAPI[0].Nombre);
                        cliente.ApelPat = Convert.ToString(dataClienteAPI[0].ApellidoPaterno);
                        cliente.ApelMat = Convert.ToString(dataClienteAPI[0].ApellidoMaterno);
                        cliente.NombreCompleto = Convert.ToString(dataClienteAPI[0].NombreCompleto);
                        cliente.NroDoc = Convert.ToString(dataClienteAPI[0].DNI);

                        cliente.TipoDocumentoId = tipodocumento;
                        cliente.FechaRegistro = DateTime.Now;
                        cliente.Estado = "A";
                        cliente.TipoRegistro = "SORTEOSALA";
                        cliente.usuario_reg = 0;
                        cliente.SalaId = CodSala;
                        ast_ClienteBL.GuardarClienteSorteoExterno(cliente);
                        respuesta = true;
                        reniec = true;
                        return Json(new { respuesta, data = cliente,reniec });
                    }
                    return Json(new { respuesta});
                }
                return Json(new { respuesta});
            }
            catch (Exception ex)
            {
                return Json(new { respuesta});
            }
        }
        [seguridad(false)]
        private async Task<dynamic> apireniec(string dni)
        {
            var rpta = false;
            ApiReniec _apiReniec = new ApiReniec();
            //string json = "";
            dynamic item = new DynamicDictionary();
            List<dynamic> Lista = new List<dynamic>();
            item.ErrorMensaje = string.Empty;
            try
            {
                if (Helpers.IsValidDNI(dni))
                {
                    var itemResponse = await _apiReniec.Busqueda(dni);
                    item.NombreCompleto = itemResponse.NombreCompleto;
                    item.DNI = itemResponse.DNI;
                    item.Nombre = itemResponse.Nombre;
                    item.ApellidoPaterno = itemResponse.ApellidoPaterno;
                    item.ApellidoMaterno = itemResponse.ApellidoMaterno;
                    item.ErrorMensaje = itemResponse.ErrorMensaje;
                    rpta = itemResponse.Respuesta;

                }
                else
                {
                    item.NombreCompleto = "Cliente No Encontrado";
                    item.DNI = "";
                    item.Nombre = "";
                    item.ApellidoPaterno = "";
                    item.ApellidoMaterno = "";
                    item.ErrorMensaje = "El número de documento es invalido";
                }
            }
            catch (Exception e)
            {
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
    }
}
