using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    [seguridad]
    public class UsuarioSalaController : Controller
    {
        private readonly EmpresaBL _empresaBL = new EmpresaBL();
        private readonly SalaBL _salaBL = new SalaBL();
        private readonly UsuarioSalaBL _usuarioSalaBl = new UsuarioSalaBL();

        public ActionResult UsuarioSalaVista()
        {
            return View();
        }

        [HttpGet]
        public JsonResult UsuarioSalasListarJson(int usuarioId)
        {
            var errormensaje = "";
            var lista = new List<UsuarioSalaEntidad>();
            try
            {
                lista = _usuarioSalaBl.UsuarioSalasListarJson(usuarioId);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult UsuarioSalaIdListarJson(int usuarioId, int salaId)
        {
            var errormensaje = "";
            var lista = new UsuarioSalaEntidad();
            try
            {
                lista = _usuarioSalaBl.UsuarioSalaIdListarJson(usuarioId, salaId);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = lista, mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UsuarioSalaInsertarJson(int usuarioId, List<int> salaIds)
        {
            bool status = false ;
            string message = "Se asignaron las salas";

            if(salaIds == null)
            {
                return Json(new
                {
                    status,
                    message = "No hay salas seleccionadas"
                });
            }

            try
            {
                //inserta = _usuarioSalaBl.UsuarioSalaInsertarJson(usuarioId,salaId);

                status = _usuarioSalaBl.UsuarioSalaAsignar(usuarioId, salaIds);
            }
            catch (Exception exception)
            {
                message = exception.Message + ", Llame Administrador";
            }

            return Json(new {
                status,
                message
            });
        }

        [HttpPost]
        public JsonResult UsuarioSalaEliminarJson(int usuarioId, List<int> salaIds)
        {
            bool status = false;
            string message = "Se denegaron las salas";

            if (salaIds == null)
            {
                return Json(new
                {
                    status,
                    message = "No hay salas seleccionadas"
                });
            }

            try
            {
                //inserta = _usuarioSalaBl.UsuarioSalaEliminarJson(usuarioId, salaId);

                status = _usuarioSalaBl.UsuarioSalaDenegar(usuarioId, salaIds);
            }
            catch (Exception exception)
            {
                message = exception.Message + ", Llame Administrador";
            }

            return Json(new {
                status,
                message
            });
        }

        [HttpPost]
        public ActionResult UsuarioEmpleadoIdListarJson(int usuarioId)
        {
            var errormensaje = "";
            var usuario = new EmpleadoUsuarioEntidad();
            try
            {
                usuario = _usuarioSalaBl.UsuarioEmpleadoIdListarJson(usuarioId);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ", Llame Administrador";
            }

            return Json(new { respuesta = usuario, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ObtenerEmpresaSala()
        {
            bool status = false;
            string message = "Datos obtenidos de Empresa Sala";

            List<EmpresaEntidad> listCompanies = new List<EmpresaEntidad>();
            List<SalaEntidad> listRooms = new List<SalaEntidad>();

            try
            {
                listCompanies = _empresaBL.ListadoEmpresa();
                listRooms = _salaBL.ListadoSala();

                status = true;
            }
            catch (Exception exception)
            {
                message = exception.Message + ", Llame al Administrador";
            }

            return Json(new
            {
                status,
                message,
                data = new
                {
                    companies = listCompanies,
                    rooms = listRooms
                }
            });
        }
    }
}