using CapaEntidad.BUK;
using CapaEntidad.BUK.Response;
using CapaEntidad.Response;
using CapaNegocio.BUK;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.BUK {
    [seguridad]
    public class EquivalenciaSedeController : Controller {

        private readonly BUK_EquivalenciaSedeBL _equivalenciaSedeBL;
        private readonly BUK_EquivalenciaEmpresaBL _equivalenciaEmpresaBL;

        public EquivalenciaSedeController() {
            _equivalenciaSedeBL = new BUK_EquivalenciaSedeBL();
            _equivalenciaEmpresaBL = new BUK_EquivalenciaEmpresaBL();
        }

        public ActionResult EquivalenciaSedeListadoVista() {
            return View();
        }

        public ActionResult EquivalenciaSedeInsertarVista() {
            return View();
        }

        public ActionResult EquivalenciaSedeActualizarVista(int id = 0) {
            var equivalenciaSede = _equivalenciaSedeBL.ObtenerEquivalenciaSedePorIdEquivalenciaSede(id);
            if(equivalenciaSede == null || equivalenciaSede.NombreSede == null) {
                return RedirectToAction("EquivalenciaSedeListadoVista");
            }
            return View(equivalenciaSede);
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarTodasLasEquivalenciasSedeJSON() {
            bool success = false;
            List<BUK_EquivalenciaSedeEntidad> equivalenciasSede = new List<BUK_EquivalenciaSedeEntidad>();
            string displayMessage;

            try {
                equivalenciasSede = _equivalenciaSedeBL.ObtenerTodasLasEquivalenciasSede();
                success = equivalenciasSede.Count > 0;
                displayMessage = success ? "Lista de equivalencias de sedes de BUK." : "No hay equivalencias de sedes de BUK";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = equivalenciasSede, displayMessage });
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ListarTodasLasEquivalenciasSedeCorrectasJSON() {
            bool success = false;
            List<BUK_EquivalenciaSedeEntidad> equivalenciasSede = new List<BUK_EquivalenciaSedeEntidad>();
            string displayMessage;

            try {
                equivalenciasSede = _equivalenciaSedeBL.ObtenerTodasLasEquivalenciasSedeCorrectas();
                success = equivalenciasSede.Count > 0;
                displayMessage = success ? "Lista de equivalencias de sedes de BUK." : "No hay equivalencias de sedes de BUK";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = equivalenciasSede, displayMessage });
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ObtenerEquivalenciasSedePorIdEquivalenciaEmpresaJSON(int idEquivalenciaEmpresa) {
            bool success = false;
            List<BUK_EquivalenciaSedeEntidad> equivalenciasSede = new List<BUK_EquivalenciaSedeEntidad>();
            string displayMessage;

            var equivalenciaEmpresa = _equivalenciaEmpresaBL.ObtenerEquivalenciaEmpresaPorIdEquivalenciaEmpresa(idEquivalenciaEmpresa);
            if(equivalenciaEmpresa.IdEquivalenciaEmpresa <= 0) {
                displayMessage = "No existe la equivalencia de empresa seleccionada.";
                return Json(new { success, displayMessage });
            }

            try {
                equivalenciasSede = _equivalenciaSedeBL.ObtenerEquivalenciaSedePorIdEquivalenciaEmpresa(idEquivalenciaEmpresa);
                success = equivalenciasSede.Count > 0;
                displayMessage = success ? $"Lista de equivalencias de sedes de BUK de la empresa {equivalenciaEmpresa.Nombre}." : $"No hay equivalencias de sedes de BUK en la empresa {equivalenciaEmpresa.Nombre}";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = equivalenciasSede, displayMessage });
        }
        
        [seguridad(false)]
        [HttpPost]
        public JsonResult ObtenerEquivalenciasSedePorCodempresaOfisisJSON(string codEmpresaOfisis) {
            bool success = false;
            List<BUK_EquivalenciaSedeEntidad> equivalenciasSede = new List<BUK_EquivalenciaSedeEntidad>();
            string displayMessage;

            var equivalenciaEmpresa = _equivalenciaEmpresaBL.ObtenerEquivalenciaEmpresaPorCodEmpresaOfisis(codEmpresaOfisis);
            if(equivalenciaEmpresa.IdEquivalenciaEmpresa <= 0) {
                displayMessage = "No existe la equivalencia de empresa seleccionada.";
                return Json(new { success, displayMessage });
            }

            try {
                equivalenciasSede = _equivalenciaSedeBL.ObtenerEquivalenciaSedePorCodEmpresaOfisis(codEmpresaOfisis);
                success = equivalenciasSede.Count > 0;
                displayMessage = success ? $"Lista de equivalencias de sedes de BUK de la empresa {equivalenciaEmpresa.Nombre}." : $"No hay equivalencias de sedes de BUK en la empresa {equivalenciaEmpresa.Nombre}";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = equivalenciasSede, displayMessage });
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ObtenerEquivalenciaSedePorCodEmpresaOfisisYNombreEquivalenciaSede(string codEmpresaOfisis, string nombreSede) {
            bool success = false;
            BUK_EquivalenciaSedeEntidad equivalenciaSede = new BUK_EquivalenciaSedeEntidad();
            string displayMessage;
                        
            try {
                equivalenciaSede = _equivalenciaSedeBL.ObtenerEquivalenciaSedePorCodEmpresaOfisisYNombreEquivalenciaSede(codEmpresaOfisis, nombreSede);
                success = equivalenciaSede.Existe();
                displayMessage = success ? $"Equivalencia de sede encontrada." : $"No hay equivalencias de sedes de BUK con el nombre {nombreSede}";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = equivalenciaSede, displayMessage });
        }

        [HttpPost]
        public JsonResult ObtenerEquivalenciaSedePorIdJSON(int idEquivalenciaSede) {
            bool success = false;
            BUK_EquivalenciaSedeEntidad equivalenciaSede = new BUK_EquivalenciaSedeEntidad();
            string displayMessage;

            try {
                equivalenciaSede = _equivalenciaSedeBL.ObtenerEquivalenciaSedePorIdEquivalenciaSede(idEquivalenciaSede);
                success = equivalenciaSede.Existe();
                displayMessage = success ? "Equivalencia de sede econtrada." : "No se encontró la equivalencia de sede";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = equivalenciaSede, displayMessage });
        }

        [seguridad(false)]
        [HttpPost]
        public JsonResult ObtenerEquivalenciaSedePorCodEmpresaYSedeOfisisJSON(string codEmpresaOfisis, string codSedeOfisis) {
            bool success = false;
            BUK_EquivalenciaSedeEntidad equivalenciaSede = new BUK_EquivalenciaSedeEntidad();
            string displayMessage;

            try {
                equivalenciaSede = _equivalenciaSedeBL.ObtenerEquivalenciaSedePorCodEmpresaYSedeOfisis(codEmpresaOfisis, codSedeOfisis);
                success = equivalenciaSede.Existe();
                displayMessage = success ? "Equivalencia de sede econtrada." :$"No se encontró la equivalencia de sede con código ofisis {codSedeOfisis} de la empresa {codEmpresaOfisis}.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }

            return Json(new { success, data = equivalenciaSede, displayMessage });
        }


        [HttpPost]
        public JsonResult InsertarEquivalenciaSedeJSON(BUK_EquivalenciaSedeEntidad equivalenciaSede) {
            bool success = false;
            string displayMessage;
            try {
                if(equivalenciaSede.Equals("0")) {
                    displayMessage = $"Cod. OFISIS debe ser un número mayor a cero (0).";
                    return Json(new { success, displayMessage });
                }
                
                BUK_EquivalenciaEmpresaEntidad empresaVerificacion = _equivalenciaEmpresaBL.ObtenerEquivalenciaEmpresaPorIdEquivalenciaEmpresa(equivalenciaSede.IdEquivalenciaEmpresa);

                BUK_EquivalenciaSedeEntidad sedeVerificacion = _equivalenciaSedeBL.ObtenerEquivalenciaSedePorCodEmpresaYSedeOfisis(empresaVerificacion.CodEmpresaOfisis, equivalenciaSede.CodSedeOfisis);
                if(sedeVerificacion.Existe()) {
                    displayMessage = $"Ya existe una equivalencia de sede con Cod. Ofisis {equivalenciaSede.CodSedeOfisis} en la empresa '{sedeVerificacion.NombreEmpresa}'.";
                    return Json(new { success, displayMessage });
                }

                sedeVerificacion = _equivalenciaSedeBL.ObtenerEquivalenciaSedePorCodEmpresaOfisisYNombreEquivalenciaSede(empresaVerificacion.CodEmpresaOfisis, equivalenciaSede.NombreSede);
                if(sedeVerificacion.Existe()) {
                    displayMessage = $"Ya existe una equivalencia de sede con el nombre '{equivalenciaSede.NombreSede}' en la empresa '{sedeVerificacion.NombreEmpresa}'.";
                    return Json(new { success, displayMessage });
                }

                success = _equivalenciaSedeBL.InsertarEquivalenciaSede(equivalenciaSede);
                displayMessage = success ? "Equivalencia de sede agregada correctamente." : "No se pudo agregar la equivalencia de sede, llame al Administrador.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult ActualizarEquivalenciaSedeJSON(BUK_EquivalenciaSedeEntidad equivalenciaSede) {
            bool success = false;
            string displayMessage;
            try {
                if(equivalenciaSede.Equals("0")) {
                    displayMessage = $"Cod. OFISIS debe ser un número mayor a cero (0).";
                    return Json(new { success, displayMessage });
                }

                BUK_EquivalenciaEmpresaEntidad empresaVerificacion = _equivalenciaEmpresaBL.ObtenerEquivalenciaEmpresaPorIdEquivalenciaEmpresa(equivalenciaSede.IdEquivalenciaEmpresa);

                BUK_EquivalenciaSedeEntidad sedeVerificacion = _equivalenciaSedeBL.ObtenerEquivalenciaSedePorCodEmpresaYSedeOfisis(empresaVerificacion.CodEmpresaOfisis, equivalenciaSede.CodSedeOfisis);
                if(sedeVerificacion.Existe() && sedeVerificacion.IdEquivalenciaSede != equivalenciaSede.IdEquivalenciaSede) {
                    displayMessage = $"Ya existe una equivalencia de sede con Cod. Ofisis {equivalenciaSede.CodSedeOfisis} en la empresa '{sedeVerificacion.NombreEmpresa}'.";
                    return Json(new { success, displayMessage });
                }

                sedeVerificacion = _equivalenciaSedeBL.ObtenerEquivalenciaSedePorCodEmpresaOfisisYNombreEquivalenciaSede(empresaVerificacion.CodEmpresaOfisis, equivalenciaSede.NombreSede);
                if(sedeVerificacion.Existe() && sedeVerificacion.IdEquivalenciaSede != equivalenciaSede.IdEquivalenciaSede) {
                    displayMessage = $"Ya existe una equivalencia de sede con el nombre '{equivalenciaSede.NombreSede}' en la empresa '{sedeVerificacion.NombreEmpresa}'.";
                    return Json(new { success, displayMessage });
                }

                success = _equivalenciaSedeBL.ActualizarEquivalenciaSede(equivalenciaSede);
                displayMessage = success ? "Equivalencia de sede actualizada correctamente." : "No se pudo actualizar la equivalencia de sede, llame al Administrador.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult EliminarEquivalenciaSedeJSON(int idEquivalenciaSede) {
            bool success = false;
            string displayMessage;
            try {
                success = _equivalenciaSedeBL.EliminarEquivalenciaSede(idEquivalenciaSede);
                displayMessage = success ? "Equivalencia de sede eliminada correctamente" : "No se pudo eliminar la equivalencia de sede, llame al Administrador.";
            } catch(Exception exp) {
                displayMessage = exp.Message + ". Llame al Administrador.";
            }
            return Json(new { success, displayMessage });
        }

        #region Sincronizador
        [seguridad(false)]
        public JsonResult SincronizarSedes(List<BUK_EquivalenciaSedeEntidad> sedes) {
            ResponseEntidad<BUK_EquivalenciaSedeResponse> response = new ResponseEntidad<BUK_EquivalenciaSedeResponse>();
            try {
                response = _equivalenciaSedeBL.SincornizarSedes(sedes);
            } catch(Exception ex) {
                response.displayMessage = $"Error al intentar sincronizar sedes. {ex.Message}";
            }
            return Json(response);
        }
        #endregion
    }
}