using CapaEntidad.BOT.Entities;
using CapaEntidad.BOT.Enum;
using CapaNegocio.Cortesias;
using S3k.Utilitario.Extensions;
using S3k.Utilitario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.BOT {
    [seguridad(false)]
    public class BotPermisosController : BaseController {
        private readonly BOT_PermisoBL permisoBL;
        private readonly BOT_CargoBL cargoBL;
        private readonly BOT_EmpleadoBL empleadoBL;

        public BotPermisosController() {
            permisoBL = new BOT_PermisoBL();
            cargoBL = new BOT_CargoBL();
            empleadoBL = new BOT_EmpleadoBL();
        }

        #region Views
        public ActionResult Index() {
            return View("~/Views/Bot/Permiso/Index.cshtml");
        }
        #endregion

        #region Methods
        [HttpPost]
        public JsonResult ObtenerPorIdCargo(int idCargo) {
            bool success = false;
            string displayMessage;
            List<BOT_PermisoEntidad> data = new List<BOT_PermisoEntidad>();

            try {
                BOT_CargoEntidad cargo = cargoBL.ObtenerCargoPorId(idCargo);
                if(!cargo.Existe()) {
                    success = false;
                    displayMessage = "No existe el cargo.";
                    return JsonResponse(new { success, displayMessage });
                }

                data = permisoBL.ObtenerPermisosPorIdCargo(idCargo);
                success = data.Count > 0;
                displayMessage = success ? $"Permisos del cargo {cargo.Nombre}." : "No se encontraron permisos para el cargo.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return JsonResponse(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult ObtenerPorIdEmpleado(int idEmpleado) {
            bool success = false;
            string displayMessage;
            List<BOT_PermisoEntidad> data = new List<BOT_PermisoEntidad>();

            try {
                BOT_EmpleadoEntidad empleado = empleadoBL.ObtenerEmpleadoPorId(idEmpleado);
                if(!empleado.Existe()) {
                    success = false;
                    displayMessage = "No existe el empleado.";
                    return JsonResponse(new { success, displayMessage });
                }

                data = permisoBL.ObtenerPermisosPorIdEmpleado(idEmpleado);
                success = data.Count > 0;
                displayMessage = success ? $"Permisos de {empleado.Nombres} {empleado.ApellidoPaterno}." : "No se encontraron permisos para el empleado.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return JsonResponse(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult CrearPermiso(BOT_PermisoEntidad permiso) {
            bool success = false;
            string displayMessage;

            try {
                success = permisoBL.CrearPermiso(permiso);
                displayMessage = success ? "Permiso asignado correctamente." : "No se asignar el permiso.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return JsonResponse(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult CrearPermisos(List<BOT_PermisoEntidad> permisos) {
            bool success = false;
            string displayMessage;

            try {
                success = permisoBL.CrearPermisos(permisos);
                displayMessage = success ? "Permiso asignado correctamente." : "No se asignar el permiso.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return JsonResponse(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult EliminarPermiso(BOT_Acciones codAccion, int idCargo, int idEmpleado) {
            bool success = false;
            string displayMessage;

            try {
                if(idCargo > 0) {
                    success = permisoBL.EliminarPermisoDeCargo(codAccion, idCargo);
                } else if(idEmpleado > 0) {
                    success = permisoBL.EliminarPermisoDeEmpleado(codAccion, idEmpleado);
                } else {
                    success = false;
                }

                displayMessage = success ? "Permiso removido." : "No se pudo remover el permiso.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return JsonResponse(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult EliminarPermisos(List<BOT_PermisoEntidad> permisos) {
            bool success = false;
            string displayMessage;

            try {
                success = permisoBL.EliminarPermisos(permisos);
                displayMessage = success ? "Permiso removido." : "No se pudo remover el permiso.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return JsonResponse(new { success, displayMessage });
        }

        [seguridad(false)]
        [HttpGet]
        public JsonResult ObtenerAcciones() {
            bool success = false;
            string displayMessage;
            List<EnumItemDto> data = new List<EnumItemDto>();

            try {
                data = EnumExtensions.ToListEnumItems<BOT_Acciones>();
                success = true;
                displayMessage = success ? "Lista de acciones." : "Aun no hay acciones.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return JsonResponse(new { success, displayMessage, data });
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ObtenerPermisosPorCodAccion(BOT_Acciones codAccion) {
            bool success = false;
            string displayMessage;
            List<BOT_CargoEntidad> cargos = new List<BOT_CargoEntidad>();
            List<BOT_PermisoEntidad> permisosAccion = new List<BOT_PermisoEntidad>();

            try {
                cargos = cargoBL.ObtenerCargos();
                permisosAccion = permisoBL.ObtenerPermisosDeCargosPorCodAccion(codAccion);
                success = true;
                displayMessage = success ? "Lista de acciones." : "Aun no hay acciones.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            var areas = cargos.GroupBy(x => new { x.NombreArea }).Select(g => new { Nombre = g.Key.NombreArea }).OrderBy(x => x.Nombre);

            object data = new {
                areas,
                cargos,
                permisosAccion
            };
            return JsonResponse(new { success, displayMessage, data });
        }
        #endregion
    }
}
