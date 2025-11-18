using CapaEntidad.GLPI.Helper;
using CapaNegocio;
using CapaNegocio.GLPI.Mantenedores;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.GLPI {
    [seguridad(false)]
    public class GlpiSelectsController : Controller {
        private readonly GlpiTicketController glpiTicketController;
        private readonly GLPI_TipoOperacionBL tipoOperacionBL;
        private readonly GLPI_NivelAtencionBL nivelAtencionBL;
        private readonly GLPI_PartidaBL partidaBL;
        private readonly GLPI_CategoriaBL categoriaBL;
        private readonly GLPI_SubCategoriaBL subCategoriaBL;
        private readonly GLPI_ClasificacionProblemaBL clasificacionProblemaBL;
        private readonly GLPI_EstadoActualBL estadoActualBL;
        private readonly GLPI_IdentificadorBL identificadorBL;
        private readonly GLPI_EstadoTicketBL estadoTicketBL;
        private readonly GLPI_UsuarioBL usuarioBL;
        private readonly GLPI_ProcesoBL procesoBL;
        private readonly GLPI_CorreoBL correoBL;
        private readonly SEG_PermisoRolBL permisoRolBL;

        public GlpiSelectsController() {
            glpiTicketController = new GlpiTicketController();
            tipoOperacionBL = new GLPI_TipoOperacionBL();
            nivelAtencionBL = new GLPI_NivelAtencionBL();
            partidaBL = new GLPI_PartidaBL();
            categoriaBL = new GLPI_CategoriaBL();
            subCategoriaBL = new GLPI_SubCategoriaBL();
            clasificacionProblemaBL = new GLPI_ClasificacionProblemaBL();
            estadoActualBL = new GLPI_EstadoActualBL();
            identificadorBL = new GLPI_IdentificadorBL();
            estadoTicketBL = new GLPI_EstadoTicketBL();
            procesoBL = new GLPI_ProcesoBL();
            usuarioBL = new GLPI_UsuarioBL();
            correoBL = new GLPI_CorreoBL();
            permisoRolBL = new SEG_PermisoRolBL();
        }

        [HttpPost]
        public JsonResult GetSelectCrearTicket() {
            bool success = false;
            List<GLPI_SelectHelper> tiposOperacion = new List<GLPI_SelectHelper>();
            List<GLPI_SelectHelper> nivelesAtencion = new List<GLPI_SelectHelper>();
            List<GLPI_SelectHelper> partidas = new List<GLPI_SelectHelper>();
            List<GLPI_SelectHelper> categorias = new List<GLPI_SelectHelper>();
            List<GLPI_SelectHelper> subCategorias = new List<GLPI_SelectHelper>();
            List<GLPI_SelectHelper> clasificacionProblemas = new List<GLPI_SelectHelper>();
            List<GLPI_SelectHelper> estadosActual = new List<GLPI_SelectHelper>();
            List<GLPI_SelectHelper> identificadores = new List<GLPI_SelectHelper>();
            List<GLPI_SelectHelper> correos = new List<GLPI_SelectHelper>();

            try {
                tiposOperacion = tipoOperacionBL.ObtenerTiposOperacionSelect();
                nivelesAtencion = nivelAtencionBL.ObtenerNivelesAtencionSelect();
                partidas = partidaBL.ObtenerPartidasSelect();
                categorias = categoriaBL.ObtenerCategoriasSelect();
                subCategorias = subCategoriaBL.ObtenerSubCategoriasSelect();
                clasificacionProblemas = clasificacionProblemaBL.ObtenerClasificacionProblemasSelect();
                estadosActual = estadoActualBL.ObtenerEstadosActualesSelect();
                identificadores = identificadorBL.ObtenerIdentificadoresSelect();
                correos = correoBL.ObtenerCorreosSelect();
                success = true;
            } catch {
                success = false;
            }

            object data = new { tiposOperacion, nivelesAtencion, partidas, categorias, subCategorias, clasificacionProblemas, estadosActual, identificadores, correos };
            JsonResult jsonResult = Json(new { success, data });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetSelectAsignarTicket() {
            bool success = false;
            List<GLPI_SelectHelper> estadosTicket = new List<GLPI_SelectHelper>();
            List<GLPI_SelectHelper> usuariosAsignables = new List<GLPI_SelectHelper>();
            List<GLPI_SelectHelper> correos = new List<GLPI_SelectHelper>();

            try {
                estadosTicket = estadoTicketBL.ObtenerEstadosTicketSelect();
                usuariosAsignables = usuarioBL.ObtenerUsuariosPorAccionSelect(nameof(glpiTicketController.RecibirTicket));
                correos = correoBL.ObtenerCorreosSelect();
                success = true;
            } catch {
                success = false;
            }

            object data = new { estadosTicket, usuariosAsignables, correos };
            JsonResult jsonResult = Json(new { success, data });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetSelectSeguimientoTicket() {
            bool success = false;
            List<GLPI_SelectHelper> estadosTicket = new List<GLPI_SelectHelper>();
            List<GLPI_SelectHelper> procesos = new List<GLPI_SelectHelper>();
            List<GLPI_SelectHelper> correos = new List<GLPI_SelectHelper>();

            try {
                estadosTicket = estadoTicketBL.ObtenerEstadosTicketSelect();
                procesos = procesoBL.ObtenerProcesosSelect();
                correos = correoBL.ObtenerCorreosSelect();
                success = true;
            } catch {
                success = false;
            }

            object data = new { estadosTicket, procesos, correos };
            JsonResult jsonResult = Json(new { success, data });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetSelectCierreTicket() {
            bool success = false;
            List<GLPI_SelectHelper> estadosTicket = new List<GLPI_SelectHelper>();

            try {
                estadosTicket = estadoTicketBL.ObtenerEstadosTicketSelect();
                success = true;
            } catch {
                success = false;
            }

            object data = new { estadosTicket };
            JsonResult jsonResult = Json(new { success, data });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetSelectCrearUsuario() {
            bool success = false;
            List<GLPI_SelectHelper> roles = new List<GLPI_SelectHelper>();

            try {
                roles = permisoRolBL.GetRolesPorAccion(nameof(glpiTicketController.RecibirTicket)).Select(x => new GLPI_SelectHelper {
                    Valor = x.WEB_RolID.ToString(),
                    Texto = x.WEB_RolNombre,
                }).ToList();
                success = true;
            } catch {
                success = false;
            }

            object data = new { roles };
            JsonResult jsonResult = Json(new { success, data });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}