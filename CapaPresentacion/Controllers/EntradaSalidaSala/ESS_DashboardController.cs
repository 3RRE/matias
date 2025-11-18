using CapaEntidad.EntradaSalidaSala.CapaEntidad.EntradaSalidaSala;
using CapaNegocio;
using CapaNegocio.EntradaSalidaSala;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CapaPresentacion.Controllers.EntradaSalidaSala {
    public class ESS_DashboardController : Controller {
        private readonly SalaBL _salaBl = new SalaBL();
        ESS_DashboardBL _dashboardBL = new ESS_DashboardBL();

        [seguridad(true)]
        public ActionResult ESSDashboardVista() {
            return View("~/Views/EntradaSalidaSala/ESS_Dashboard.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarDashboardLudopatas(int[] codsala, DateTime fechaini, DateTime fechafin) {
            string mensaje = "No se encontraton registros";
            bool respuesta = false;
            List<ESS_DashboardLudopatasEntidad> listaESS_RecaudacionPersonal = new List<ESS_DashboardLudopatasEntidad>();
            try {
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                int[] salasBusqueda;
                if(codsala.Contains(-1)) {
                    salasBusqueda = salas.Select(x => x.CodSala).ToArray();
                } else {
                    salasBusqueda = codsala;
                }
                listaESS_RecaudacionPersonal = _dashboardBL.GetListDashboardLudopatasEntidad(salasBusqueda, fechaini, fechafin);
                mensaje = "Registros obtenidos";
                respuesta = true;
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, comunicarse con administrador";
            }
            var oRespuesta = new {
                mensaje,
                respuesta,
                data = listaESS_RecaudacionPersonal
            };
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var result = new ContentResult {
                Content = serializer.Serialize(oRespuesta),
                ContentType = "application/json"
            };
            return result;
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarDashboardRecaudaciones(int[] codsala, DateTime fechaini, DateTime fechafin) {
            string mensaje = "No se encontraton registros";
            bool respuesta = false;
            List<ESS_DashboardReacudacionEntidad> listaESS_RecaudacionPersonal = new List<ESS_DashboardReacudacionEntidad>();
            try {
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                int[] salasBusqueda;
                if(codsala.Contains(-1)) {
                    salasBusqueda = salas.Select(x => x.CodSala).ToArray();
                } else {
                    salasBusqueda = codsala;
                }
                listaESS_RecaudacionPersonal = _dashboardBL.GetListDashboardReacudacionEntidad(salasBusqueda, fechaini, fechafin);
                mensaje = "Registros obtenidos";
                respuesta = true;
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, comunicarse con administrador";
            }
            var oRespuesta = new {
                mensaje,
                respuesta,
                data = listaESS_RecaudacionPersonal
            };
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var result = new ContentResult {
                Content = serializer.Serialize(oRespuesta),
                ContentType = "application/json"
            };
            return result;
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarDashboardCajasTemporizadas(int[] codsala, DateTime fechaini, DateTime fechafin) {
            string mensaje = "No se encontraton registros";
            bool respuesta = false;
            List<ESS_DashboardCajasTemporizadasEntidad> listaESS_RecaudacionPersonal = new List<ESS_DashboardCajasTemporizadasEntidad>();
            try {
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                int[] salasBusqueda;
                if(codsala.Contains(-1)) {
                    salasBusqueda = salas.Select(x => x.CodSala).ToArray();
                } else {
                    salasBusqueda = codsala;
                }
                listaESS_RecaudacionPersonal = _dashboardBL.GetListDashboardCajasTemporizadasEntidad(salasBusqueda, fechaini, fechafin);
                mensaje = "Registros obtenidos";
                respuesta = true;
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, comunicarse con administrador";
            }
            var oRespuesta = new {
                mensaje,
                respuesta,
                data = listaESS_RecaudacionPersonal
            };
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var result = new ContentResult {
                Content = serializer.Serialize(oRespuesta),
                ContentType = "application/json"
            };
            return result;
        }


        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarDashboardEntesReguladoras(int[] codsala, DateTime fechaini, DateTime fechafin) {
            string mensaje = "No se encontraton registros";
            bool respuesta = false;
            List<ESS_DashboardEnteReguladoraEntidad> listaESS_RecaudacionPersonal = new List<ESS_DashboardEnteReguladoraEntidad>();
            try {
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                int[] salasBusqueda;
                if(codsala.Contains(-1)) {
                    salasBusqueda = salas.Select(x => x.CodSala).ToArray();
                } else {
                    salasBusqueda = codsala;
                }
                listaESS_RecaudacionPersonal = _dashboardBL.GetListDashboardEnteReguladoraEntidad(salasBusqueda, fechaini, fechafin);
                mensaje = "Registros obtenidos";
                respuesta = true;
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, comunicarse con administrador";
            }
            var oRespuesta = new {
                mensaje,
                respuesta,
                data = listaESS_RecaudacionPersonal
            };
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var result = new ContentResult {
                Content = serializer.Serialize(oRespuesta),
                ContentType = "application/json"
            };
            return result;
        }


        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarDashboardOcurrenciasLog(int[] codsala, DateTime fechaini, DateTime fechafin) {
            string mensaje = "No se encontraton registros";
            bool respuesta = false;
            List<ESS_DashboardOcurrenciasLogEntidad> listaESS_RecaudacionPersonal = new List<ESS_DashboardOcurrenciasLogEntidad>();
            try {
                var usuarioId = (int)Session["UsuarioID"];
                var salas = _salaBl.ListadoSalaPorUsuario(usuarioId);
                int[] salasBusqueda;
                if(codsala.Contains(-1)) {
                    salasBusqueda = salas.Select(x => x.CodSala).ToArray();
                } else {
                    salasBusqueda = codsala;
                }
                listaESS_RecaudacionPersonal = _dashboardBL.GetListDashboardOcurrenciasLogEntidad(salasBusqueda, fechaini, fechafin);
                mensaje = "Registros obtenidos";
                respuesta = true;
            } catch(Exception exp) {
                mensaje = "Ha ocurrido un error, comunicarse con administrador";
            }
            var oRespuesta = new {
                mensaje,
                respuesta,
                data = listaESS_RecaudacionPersonal
            };
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var result = new ContentResult {
                Content = serializer.Serialize(oRespuesta),
                ContentType = "application/json"
            };
            return result;
        }

    }
}
