using CapaEntidad.Notificaciones.DTO.Mantenedores;
using CapaEntidad.Notificaciones.Entity.Mantenedores;
using CapaNegocio.Notificaciones.Mantenedores;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Notificaciones.Mantenedores {
    [seguridad(true)]
    public class CredencialCorreoController : Controller {
        private readonly CredencialCorreoBL _credencialBL;
        private readonly SistemaBL sistemaBL;

        public CredencialCorreoController() {
            _credencialBL = new CredencialCorreoBL();
            sistemaBL = new SistemaBL();
        }

        #region Views
        public ActionResult CredencialCorreoView() {
            return View("~/Views/Notificaciones/CredencialCorreo/CredencialCorreo.cshtml");
        }

        public ActionResult AgregarEditarCredencialCorreoView(int id = 0) {
            CredencialCorreoDto credencial = id == 0 ? new CredencialCorreoDto() : _credencialBL.ObtenerCredencialCorreoPorId(id);
            return View("~/Views/Notificaciones/CredencialCorreo/CredencialCorreoAddEdit.cshtml", credencial);
        }
        #endregion

        #region Methods
        [HttpPost]
        public JsonResult SaveCredencialCorreo(CredencialCorreoDto credencialDto) {
            bool success = false;
            string displayMessage;
            bool isEdit = credencialDto.Existe();

            try {
                CredencialCorreo credencial = new CredencialCorreo {
                    Id = credencialDto.Id,
                    IdSistema = credencialDto.IdSistema,
                    NombreRemitente = credencialDto.NombreRemitente,
                    Correo = credencialDto.Correo,
                    ClaveSMTP = credencialDto.ClaveSMTP,
                    ServidorSMTP = credencialDto.ServidorSMTP,
                    PuertoSMTP = credencialDto.PuertoSMTP,
                    SSLHabilitado = credencialDto.SSLHabilitado,
                    CuotaDiaria = credencialDto.CuotaDiaria,
                    Prioridad = credencialDto.Prioridad,
                    Estado = credencialDto.Estado
                };

                CredencialCorreoDto credencialVerificacion = _credencialBL.ObtenerCredencialCorreoPorCorreo(credencial.Correo);
                if(isEdit) {
                    if(credencialVerificacion.Existe() && credencialVerificacion.Id != credencial.Id) {
                        displayMessage = $"El correo {credencial.Correo} ya se encuentra registrado, intente con otro correo.";
                        return Json(new { success, displayMessage });
                    }
                } else {
                    if(credencialVerificacion.Existe()) {
                        displayMessage = $"El correo {credencial.Correo} ya se encuentra registrado, intente con otro correo.";
                        return Json(new { success, displayMessage });
                    }
                }

                success = isEdit ? _credencialBL.ActualizarCredencialCorreo(credencial) : _credencialBL.InsertarCredencialCorreo(credencial);
                displayMessage = success ? "Credencial guardada correctamente." : "No se pudo guardar la credencial.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador.";
            }

            return Json(new { success, displayMessage });
        }

        [seguridad(false)] //debido a que se usa al cargar la vista
        [HttpPost]
        public JsonResult ObtenerCredencialesCorreo() {
            bool success = false;
            string displayMesasge;
            List<CredencialCorreoDto> data = new List<CredencialCorreoDto>();

            try {
                data = _credencialBL.ObtenerCredencialesCorreo();
                success = data.Count > 0;
                displayMesasge = success ? "Lista de Credenciales de Correo." : "No hay lista de Credenciales de Correo registrados.";
            } catch(Exception ex) {
                displayMesasge = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMesasge, data });
        }

        [HttpPost]
        public JsonResult EliminarCredencialCorreo(int id) {
            bool success = false;
            string displayMessage;

            try {
                success = _credencialBL.EliminarCredencialCorreo(id);
                displayMessage = success ? "Credencial eliminado correctamente." : "No se pudo eliminar la credencial.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador.";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult ObtenerCredencialesCorreoPorSistema(int IdSistema) {
            bool success = false;
            string displayMessage;
            List<CredencialCorreoDto> data = new List<CredencialCorreoDto>();

            try {
                SistemaDto sistema = sistemaBL.ObtenerSistemaPorId(IdSistema);
                if(!sistema.Existe()) {
                    success = false;
                    displayMessage = $"No hay lista de Credenciales de Correo {IdSistema}.";
                    return Json(new { success, displayMessage });
                }
                data = _credencialBL.ObtenerCredencialesCorreoPorSistema(IdSistema);
                success = data.Count > 0;
                displayMessage = success ? "Lista de Credenciales de Correo." : "No hay lista de Credenciales de Correo.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage, data });
        }
        #endregion
    }
}
