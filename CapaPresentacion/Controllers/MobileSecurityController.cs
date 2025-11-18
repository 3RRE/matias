using CapaNegocio;
using System;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers {

    [seguridad]
    public class MobileSecurityController : Controller {

        private SEG_RolUsuarioBL seg_rol_usuarioBL = new SEG_RolUsuarioBL();
        private SEG_PermisoRolBL seg_PermisoRolBL = new SEG_PermisoRolBL();

        [seguridad(false)]
        [HttpPost]
        public ActionResult ValidatePermission(string permission, int usuario_id) {
            bool valid = false;
            string message = string.Empty;

            try {

                var rol = seg_rol_usuarioBL.GetRolUsuarioId(usuario_id);

                string accion = permission;
                var permiso = seg_PermisoRolBL.GetPermisoRolUsuario(rol.WEB_RolID, accion);

                if(permiso.Count == 0) {
                    valid = false;
                    message = "No tiene permiso para acceder a este módulo.";
                } else {

                    valid = true;
                    message = "Permiso concedido.";
                }


            } catch(Exception ex) {

                message = ex.Message;
            }

            return Json(new { valid, message });
        }


        [HttpPost]
        public bool VisitaVistaMovil() {
            return true;
        }

        [HttpPost]
        public bool EmpadronamientoVistaMovil() {
            return true;
        }

        [HttpPost]
        public bool ControlAccesoVistaMovil() {
            return true;
        }

        [HttpPost]
        public bool AlertaBilleterosVistaMovil() {
            return true;
        }

        [HttpPost]
        public bool AlertaEventosVistaMovil() {
            return true;
        }

        [HttpPost]
        public bool AsistenciaCamaraVistaMovil() {
            return true;
        }

        [HttpPost]
        public bool AsistenciaManualVistaMovil() {
            return true;
        }

        [HttpPost]
        public bool RegistroRemotoVistaMovil() {
            return true;
        }

        [HttpPost]
        public bool HistoricoPozoVistaMovil() {
            return true;
        }

    }
}
