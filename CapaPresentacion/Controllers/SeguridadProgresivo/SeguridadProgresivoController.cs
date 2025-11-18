using CapaEntidad;
using CapaEntidad.ProgresivoSeguridad;
using CapaNegocio;
using CapaNegocio.SeguridadProgresivo;
using CapaPresentacion.Filters;
using CapaPresentacion.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.SeguridadProgresivo
{
    [seguridad]
    public class SeguridadProgresivoController : Controller
    {
        private SeguridadProgresivoBL segprogresivoBl = new SeguridadProgresivoBL();
        private SalaBL salaBl = new SalaBL();
        private UsuarioSalaBL usuariobl = new UsuarioSalaBL();
        private SEG_PermisoRolBL seg_PermisoRolBL = new SEG_PermisoRolBL();

        public ActionResult PeticionesToken()
        {
            return View("~/Views/SeguridadProgresivo/ListaPeticionesToken.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult GetListaPeticiones()
        {
            var errormensaje = "";
            var lista = new List<Signalr_usuarioEntidad>();
            List<SalaEntidad> listaSalas = new List<SalaEntidad>();
            List<UsuarioSalaEntidad> listaUsuarios = new List<UsuarioSalaEntidad>();
            try
            {
                int usuarioId = Convert.ToInt32(Session["UsuarioID"]);
                int rolId = Convert.ToInt32(Session["rol"]);
                if (rolId != 1)
                {
                    listaSalas = salaBl.ListadoSalaPorUsuario(usuarioId);
                    var ids_sala = listaSalas.Select(x => x.CodSala).ToList();//anteriores guardados idconsunat
                    //string cadena = " (" + "'" + String.Join("','", ids_sala) + "'" + ")";
                    //listaUsuarios = usuariobl.UsuarioSalasListarxsalaidJson(cadena);
                    //var idUsuarios = listaUsuarios.Select(x => x.UsuarioId).ToList();
                    lista = segprogresivoBl.GetListaSignalr_usuario();
                    lista = lista.Where(z => ids_sala.Contains(z.sala_id)).ToList();

                }
                else
                {
                    lista = segprogresivoBl.GetListaSignalr_usuario();
                }  
                
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult GenerarProgresivoToken(Int64 sgn_id)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                funcionesExtra fun = new funcionesExtra();
                var number = fun.RandomNumbers(4);             
                respuestaConsulta = segprogresivoBl.ActualizarToken(number, sgn_id, 2);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        // Token Progresivo
        [seguridad(false)]
        [HttpPost]
        public ActionResult IsValidToken()
        {
            bool status = false;
            string message = "Token No Validado";

            string accion = "ProgresivoAutorizado";
            List<SEG_PermisoRolEntidad> permiso = seg_PermisoRolBL.GetPermisoRolUsuario((int)Session["rol"], accion);

            if (permiso.Count > 0)
            {
                return Json(new
                {
                    status = true,
                    message = "Permiso Autorizado",
                });
            }

            string sgn_id = Request.Cookies["c_id_sgn"].Value;
            Signalr_usuarioEntidad sgn_registro = segprogresivoBl.GetSignalr_usuarioId(Convert.ToInt64(sgn_id));
            string tokenUsuario = sgn_registro.sgn_token;
            string tokenProgresivo = Session[TokenProgresivoAttribute.KEY_TOKEN_PROGRESIVO] as string;

            if (!string.IsNullOrEmpty(tokenUsuario) && tokenUsuario.Equals(tokenProgresivo))
            {
                status = true;
                message = "Token Validado";
            }

            return Json(new
            {
                status,
                message
            });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ValidateToken(string token)
        {
            bool status = false;
            string message = "Token No Válido";
           
            string sgn_id = Request.Cookies["c_id_sgn"].Value;
            Signalr_usuarioEntidad sgn_registro = segprogresivoBl.GetSignalr_usuarioId(Convert.ToInt64(sgn_id));
            string tokenUsuario = sgn_registro.sgn_token;

            if (!string.IsNullOrEmpty(tokenUsuario) && tokenUsuario.Equals(token))
            {
                status = true;
                message = "Token Válido";

                Session[TokenProgresivoAttribute.KEY_TOKEN_PROGRESIVO] = tokenUsuario;
                Session[TokenProgresivoAttribute.KEY_SGN_ROOM_ID] = sgn_registro.sala_id;
            }

            return Json(new
            {
                status,
                message
            });
        }

        public bool ProgresivoAutorizado()
        {
            return true;
        }
    }
}