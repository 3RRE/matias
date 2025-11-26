using CapaEntidad;
using CapaNegocio;
using S3k.Utilitario.Encriptacion;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;

namespace CapaPresentacion.Controllers {
    [seguridad]
    public class DispositivoController : Controller {
        private DispositivoBL dispositivoBl = new DispositivoBL();
        private string TtlMinutoHashLogin = ConfigurationManager.AppSettings["TtlMinutoHashLogin"];
        #region Mantenedor Dispositivo
        public ActionResult DispositivoListadoVista() {
            return View();
        }

        public ActionResult DispositivoInsertarVista() {
            return View();
        }

        public ActionResult DispositivoEditarVista(int id) {
            DispositivoEntidad dispositivo = new DispositivoEntidad();
            string errormensaje = "";

            try {
                dispositivo = dispositivoBl.DispositivoObtenerJson(id);
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }

            ViewBag.dispositivo = dispositivo;

            return View();
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult DispositivoListadoJson() {
            var errormensaje = "";
            var lista = new List<DispositivoEntidad>();

            try {
                lista = dispositivoBl.DispositivoListadoJson();
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }

            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult DispositivoInsertarJson(DispositivoEntidad dispositivo) {
            var errormensaje = "";
            var response = false;

            try {
                response = dispositivoBl.DispositivoInsertarJson(dispositivo);
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }

            return Json(new { data = response, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult DispositivoEditarJson(DispositivoEntidad dispositivo) {
            var errormensaje = "";
            var response = false;

            try {
                response = dispositivoBl.DispositivoEditarJson(dispositivo);
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }

            return Json(new { data = response, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult DispositivoObtenerJson(int dispositivoId) {
            var dispositivo = new DispositivoEntidad();
            var errormensaje = "";

            try {
                dispositivo = dispositivoBl.DispositivoObtenerJson(dispositivoId);
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }

            return Json(new { data = dispositivo, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ComprobarDispositivoJson(string mac) {
            var response = false;
            var errormensaje = "";

            try {
                response = dispositivoBl.ComprobarDispositivoJson(mac);
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }

            return Json(new { data = response, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ComprobarDispositivoJsonV2(string hash) {
            //if(hash.IsEmpty()) {
            //    return Json(new { data = false, mensaje = "Dispositivo no válido, Llame Administrador" });
            //}
            var response = false;
            var errormensaje = "";
            try {
                int minutosTtl = Convert.ToInt32(TtlMinutoHashLogin);
                string desencriptado = EncriptacionHelper.DecryptMachineId(hash);

                if(desencriptado == null || desencriptado.IsEmpty() || desencriptado.Contains("ERROR_DECRYPTION")) {
                    return Json(new { data = false, mensaje = "Dispositivo no válido, Llame Administrador" });
                }
                //split on _ hash
                var parts = desencriptado.Split('_');

                var fechaCreacion = Convert.ToDateTime(parts[1]);

                //if fechaCreacion is older than 3 minutes return false
                if((DateTime.Now - fechaCreacion).TotalMinutes > minutosTtl) {
                    return Json(new { data = false, mensaje = "Dispositivo no válido, Llame Administrador" });
                }
                response = dispositivoBl.ComprobarDispositivoJson(parts[0]);
            } catch(Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }

            return Json(new { data = response, mensaje = errormensaje });
        }
        #endregion
    }
}