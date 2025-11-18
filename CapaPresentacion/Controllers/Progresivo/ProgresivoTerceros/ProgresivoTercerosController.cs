using CapaDatos;
using CapaDatos.Administrativo;
using CapaEntidad.Administrativo;
using CapaEntidad.Progresivo;
using CapaNegocio;
using CapaNegocio.Administrativo;
using CapaPresentacion.Utilitarios;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Progresivo.ProgresivoTerceros {
    [seguridad(false)]
    public class ProgresivoTercerosController : Controller {
        private readonly ADM_SalaProgresivoBL _bl = new ADM_SalaProgresivoBL();
        private readonly SalaBL _salaBL = new SalaBL();
        private readonly EmpresaBL _empresBL = new EmpresaBL();
        private readonly ADM_DetalleSalaProgresivoBL _detalleBL = new ADM_DetalleSalaProgresivoBL();
        private readonly ADM_MaquinaSalaProgresivoBL _maqSalaBL = new ADM_MaquinaSalaProgresivoBL();
        private readonly ADM_MaquinaBL _maqBL = new ADM_MaquinaBL();
        private readonly ADM_PozoHistoricoBL _pozoHistBL = new ADM_PozoHistoricoBL();

        public ActionResult Index() => View();

        private bool AsignarOActivarMaquina(int codSalaProgresivo, int codMaquina, string usuario) {
            var ahora = DateTime.Now;
            var fechaNull = new DateTime(1900, 1, 1);

            var existente = _maqSalaBL.GetADM_MaquinaSalaProgresivoPorCodSalaProgresivoyCodMaquina(codSalaProgresivo, codMaquina);

            if(existente != null && existente.CodMaquinaSalaProgresivo > 0) {
                if(existente.Activo && existente.Estado == 1)
                    return true;

                existente.Activo = true;
                existente.Estado = 1;
                existente.FechaEnlace = ahora;
                existente.FechaDesactivacion = fechaNull;
                existente.FechaModificacion = ahora;
                existente.CodUsuario = usuario;
                return _maqSalaBL.EditarADM_MaquinaSalaProgresivo(existente);
            }

            var nuevo = new ADM_MaquinaSalaProgresivoEntidad {
                CodMaquina = codMaquina,
                CodSalaProgresivo = codSalaProgresivo,
                FechaEnlace = ahora,
                FechaDesactivacion = fechaNull,
                FechaRegistro = ahora,
                FechaModificacion = ahora,
                Activo = true,
                Estado = 1,
                CodUsuario = usuario
            };

            var id = _maqSalaBL.GuardarADM_MaquinaSalaProgresivo(nuevo);
            return id > 0;
        }

        private bool DesactivarAsignacion(int codMaquinaSalaProgresivo, int codSalaProgresivo, string usuario) {
            var lista = _maqSalaBL.GetListadoADM_MaquinaSalaProgresivoPorCodSalaProgresivo(codSalaProgresivo)
                        ?? new List<ADM_MaquinaSalaProgresivoEntidad>();
            var it = lista.FirstOrDefault(x => x.CodMaquinaSalaProgresivo == codMaquinaSalaProgresivo);
            if(it == null)
                return false;

            it.Activo = false;
            it.Estado = 0;
            it.FechaDesactivacion = DateTime.Now;
            it.FechaModificacion = DateTime.Now;
            it.CodUsuario = usuario;

            return _maqSalaBL.EditarADM_MaquinaSalaProgresivo(it);
        }

        private void CargarCombos(int? selectedSalaId = null, int? usuarioId = null) {
            var salas = _salaBL.ListadoTodosSalaActivosOrderJson();
            var salasCombo = salas
                .Select(s => new { Value = s.CodSala, Text = string.IsNullOrWhiteSpace(s.Nombre) ? ("Sala " + s.CodSala) : s.Nombre })
                .OrderBy(s => s.Text)
                .ToList();

            ViewBag.Salas = new SelectList(salasCombo, "Value", "Text", selectedSalaId);
            ViewBag.Tablas = new SelectList(new[] { "ADM_SalaProgresivo" });
        }

        [HttpGet]
        [Route("ProgresivoTerceros/Sala/{id:int?}")]
        public ActionResult Sala(int? id, int? codSala, string clase) {
            if(codSala.HasValue && (!id.HasValue || codSala.Value != id.Value))
                return RedirectToAction("Sala", new { id = codSala.Value, clase });

            var salaId = id;
            CargarCombos(salaId);

            if(!salaId.HasValue) {
                ViewBag.CodSala = null;
                ViewBag.Linktek = new List<ADM_SalaProgresivoEntidad>();
                ViewBag.Terceros = new List<ADM_SalaProgresivoEntidad>();
                return View("Sala", new List<ADM_SalaProgresivoEntidad>());
            }

            var data = _bl.GetListadoADM_SalaProgresivoPorSala(salaId.Value) ?? new List<ADM_SalaProgresivoEntidad>();
            foreach(var x in data)
                x.ClaseProgresivo = string.IsNullOrWhiteSpace(x.ClaseProgresivo) ? "Linktek" : x.ClaseProgresivo;

            var visibles = data.Where(x => x.Activo && x.Estado == 1).ToList();

            ViewBag.CodSala = salaId.Value;
            ViewBag.Linktek = visibles.Where(x => x.ClaseProgresivo == "Linktek").ToList();
            ViewBag.Terceros = visibles.Where(x => x.ClaseProgresivo == "Terceros").ToList();

            return View("Sala", visibles);
        }


        [HttpPost]
        public ActionResult Guardar(ADM_SalaProgresivoEntidad model, bool Activo = false, bool EstadoCheck = false) {
            if(model == null || model.CodSala <= 0) {
                TempData["Error"] = "Seleccione una sala válida.";
                return RedirectToAction("Sala", new { id = model?.CodSala ?? 0, clase = "Terceros" });
            }

            var infoSala = _salaBL.ObtenerSalaEmpresa(model.CodSala);
            model.NombreSala = infoSala?.Nombre ?? string.Empty;
            model.RazonSocial = infoSala?.Empresa?.RazonSocial ?? string.Empty;

            model.ClaseProgresivo = "Terceros";
            model.CodProgresivo = 0;

            model.Activo = Activo;
            model.Estado = EstadoCheck ? 1 : 0;

            var ahora = DateTime.Now;
            model.FechaRegistro = ahora;
            model.FechaInstalacion = model.FechaRegistro;
            model.FechaModificacion = ahora;
            model.FechaDesinstalacion = model.FechaModificacion;

            model.CodUsuario = User?.Identity?.Name ?? "";
            model.Url = string.Empty;
            model.ColorHexa = model.ColorHexa ?? string.Empty;
            model.Sigla = model.Sigla ?? string.Empty;
            model.TipoProgresivo = model.TipoProgresivo ?? string.Empty;
            model.Nombre = model.Nombre ?? string.Empty;

            var idInsertado = _bl.GuardarADM_SalaProgresivo(model);
            if(idInsertado > 0) {
                try { SincronizarPozos(idInsertado, model.NroPozos); } catch {
                }
                TempData["Ok"] = "Se registró correctamente.";
            } else {
                TempData["Error"] = "No se pudo registrar.";
            }
            return RedirectToAction("Sala", new { id = model.CodSala, clase = "Terceros" });
        }

        [HttpPost]
        public ActionResult Editar(ADM_SalaProgresivoEntidad model, bool Activo = false, bool EstadoCheck = false) {
            if(model == null || model.CodSalaProgresivo <= 0 || model.CodSala <= 0) {
                TempData["Error"] = "Datos inválidos para edición.";
                return RedirectToAction("Sala", new { id = model?.CodSala ?? 0, clase = "Terceros" });
            }

            var lista = _bl.GetListadoADM_SalaProgresivoPorSala(model.CodSala) ?? new List<ADM_SalaProgresivoEntidad>();
            var actual = lista.FirstOrDefault(x => x.CodSalaProgresivo == model.CodSalaProgresivo);
            if(actual == null) {
                TempData["Error"] = "Registro no encontrado.";
                return RedirectToAction("Sala", new { id = model.CodSala, clase = "Terceros" });
            }

            int TakeInt(string name, int current) { var v = Request[name]; if(string.IsNullOrWhiteSpace(v)) return current; return int.TryParse(v, out var n) ? n : current; }
            string TakeStr(string posted, string current, bool notNull = false) { if(!string.IsNullOrWhiteSpace(posted)) return posted; return notNull ? (current ?? string.Empty) : (posted ?? current ?? string.Empty); }

            model.Nombre = TakeStr(model.Nombre, actual.Nombre);
            model.CodProgresivoWO = TakeInt("CodProgresivoWO", actual.CodProgresivoWO);
            model.CodProgresivo = 0;
            model.NroPozos = TakeInt("NroPozos", actual.NroPozos);
            model.NroJugadores = TakeInt("NroJugadores", actual.NroJugadores);
            model.SubidaCreditos = TakeInt("SubidaCreditos", actual.SubidaCreditos);

            model.Activo = Activo;
            model.Estado = EstadoCheck ? 1 : 0;

            var ahora = DateTime.Now;
            model.FechaRegistro = (actual.FechaRegistro == default(DateTime)) ? ahora : actual.FechaRegistro;
            model.FechaInstalacion = model.FechaRegistro;
            model.FechaModificacion = ahora;
            model.FechaDesinstalacion = model.FechaModificacion;

            model.CodUsuario = User?.Identity?.Name ?? "system";
            model.ColorHexa = TakeStr(model.ColorHexa, actual.ColorHexa, notNull: true);
            model.Sigla = TakeStr(model.Sigla, actual.Sigla, notNull: true);
            model.Url = string.Empty;

            var infoSala = _salaBL.ObtenerSalaEmpresa(model.CodSala);
            model.NombreSala = infoSala?.Nombre ?? (actual.NombreSala ?? string.Empty);
            model.RazonSocial = infoSala?.Empresa?.RazonSocial ?? (actual.RazonSocial ?? string.Empty);

            var ok = _bl.EditarADM_SalaProgresivo(model);
            TempData[ok ? "Ok" : "Error"] = ok ? "Se guardaron los cambios." : "No se pudo actualizar.";
            return RedirectToAction("Sala", new { id = model.CodSala, clase = "Terceros" });
        }

        [HttpPost]
        public ActionResult Eliminar(int id, int codSala, string clase) {
            var lista = _bl.GetListadoADM_SalaProgresivoPorSala(codSala) ?? new List<ADM_SalaProgresivoEntidad>();
            var actual = lista.FirstOrDefault(x => x.CodSalaProgresivo == id);
            if(actual == null) {
                TempData["Error"] = "Registro no encontrado.";
                return RedirectToAction("Sala", new { id = codSala, clase });
            }

            actual.Activo = false;
            actual.Estado = 0;
            actual.FechaModificacion = DateTime.Now;
            actual.CodUsuario = User?.Identity?.Name ?? "system";

            var ok = _bl.EditarADM_SalaProgresivo(actual);
            TempData[ok ? "Ok" : "Error"] = ok ? "Se eliminó el registro." : "No se pudo eliminar.";
            return RedirectToAction("Sala", new { id = codSala, clase });
        }

        [HttpGet]
        public JsonResult Obtener(int codSala, int id) {
            var lista = _bl.GetListadoADM_SalaProgresivoPorSala(codSala) ?? new List<ADM_SalaProgresivoEntidad>();
            var it = lista.FirstOrDefault(x => x.CodSalaProgresivo == id);
            if(it == null)
                return Json(new { ok = false, msg = "Registro no encontrado." }, JsonRequestBehavior.AllowGet);
            return Json(new { ok = true, value = it }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarPorSala(int codSala) {
            var lista = _bl.GetListadoADM_SalaProgresivoPorSala(codSala)?
                           .Where(x => x.Activo && x.Estado == 1)
                           .ToList() ?? new List<ADM_SalaProgresivoEntidad>();
            return Json(new { ok = true, value = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Pozos(int? salaId, int? progresivoId) {
            CargarCombos(salaId);
            ViewBag.CodSala = salaId ?? 0;
            ViewBag.CodSalaProgresivo = progresivoId ?? 0;
            return View("Pozos", new List<ADM_DetalleSalaProgresivoEntidad>());
        }

        [HttpGet]
        public JsonResult ProgresivosPorSala(int codSala) {
            var data = _bl.GetListadoADM_SalaProgresivoPorSala(codSala) ?? new List<ADM_SalaProgresivoEntidad>();
            var terceros = data
                .Where(x => (x.ClaseProgresivo ?? "Linktek") == "Terceros")
                .Select(x => new { id = x.CodSalaProgresivo, text = string.IsNullOrWhiteSpace(x.Nombre) ? ("Prog #" + x.CodSalaProgresivo) : x.Nombre })
                .OrderBy(x => x.text)
                .ToList();

            return Json(new { ok = true, value = terceros }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult PozosPorProgresivo(int codSalaProgresivo, bool soloActivos = true) {
            if(codSalaProgresivo <= 0)
                return Json(new { ok = false, msg = "Parámetro inválido." }, JsonRequestBehavior.AllowGet);

            var lista = _detalleBL
                .GetListadoADM_DetalleSalaProgresivoPorCodSalaProgresivo(codSalaProgresivo)
                ?? new List<ADM_DetalleSalaProgresivoEntidad>();

            var query = soloActivos
                ? lista.Where(x => x.Activo /* && x.Estado == 1 */)
                : lista.AsEnumerable();

            var rows = query.OrderBy(x => x.NroPozo).ToList();
            return Json(new { ok = true, value = rows }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult EditarPozo(ADM_DetalleSalaProgresivoEntidad model, int codSala, int codSalaProgresivo) {
            if(model == null || model.CodDetalleSalaProgresivo <= 0 || codSalaProgresivo <= 0) {
                var msg = "Datos inválidos.";
                if(Request.IsAjaxRequest())
                    return Json(new { ok = false, msg });

                TempData["Error"] = msg;
                return RedirectToAction("Pozos", new { salaId = codSala, progresivoId = codSalaProgresivo });
            }

            var existentes = _detalleBL.GetListadoADM_DetalleSalaProgresivoPorCodSalaProgresivo(codSalaProgresivo)
                            ?? new List<ADM_DetalleSalaProgresivoEntidad>();

            var duplicado = existentes.FirstOrDefault(x =>
                x.NroPozo == model.NroPozo &&
                x.CodDetalleSalaProgresivo != model.CodDetalleSalaProgresivo &&
                x.Activo);

            if(duplicado != null) {
                var msg = $"Ya existe un registro con Nro Pozo {model.NroPozo}.";
                if(Request.IsAjaxRequest())
                    return Json(new { ok = false, msg });

                TempData["Error"] = msg;
                return RedirectToAction("Pozos", new { salaId = codSala, progresivoId = codSalaProgresivo });
            }

            model.CodSalaProgresivo = codSalaProgresivo;
            if(string.IsNullOrWhiteSpace(model.NombrePozo))
                model.NombrePozo = $"Pozo {model.NroPozo}";
            model.FechaModificacion = DateTime.Now;
            if(model.FechaRegistro == default(DateTime))
                model.FechaRegistro = DateTime.Now;
            model.CodUsuario = User?.Identity?.Name ?? "system";

            var detalleSalaProgresivo = existentes
                .FirstOrDefault(x => x.CodDetalleSalaProgresivo == model.CodDetalleSalaProgresivo);

            model.fechaIni = detalleSalaProgresivo != null ? detalleSalaProgresivo.fechaIni : SqlDateTime.MinValue.Value;
            model.fechaFin = detalleSalaProgresivo != null ? detalleSalaProgresivo.fechaFin : SqlDateTime.MinValue.Value;

            var ok = _detalleBL.EditarADM_DetalleSalaProgresivo(model);
            var mensaje = ok ? "Pozo actualizado." : "No se pudo actualizar.";

            if(Request.IsAjaxRequest())
                return Json(new { ok, msg = mensaje, value = model });

            TempData[ok ? "Ok" : "Error"] = mensaje;
            return RedirectToAction("Pozos", new { salaId = codSala, progresivoId = codSalaProgresivo });
        }

        [HttpPost]
        public JsonResult EditarPozoAjax(ADM_DetalleSalaProgresivoEntidad model, int codSalaProgresivo) {
            if(model == null || model.CodDetalleSalaProgresivo <= 0 || codSalaProgresivo <= 0)
                return Json(new { ok = false, msg = "Datos inválidos." });

            var existentes = _detalleBL.GetListadoADM_DetalleSalaProgresivoPorCodSalaProgresivo(codSalaProgresivo)
                            ?? new List<ADM_DetalleSalaProgresivoEntidad>();

            var duplicado = existentes.FirstOrDefault(x =>
                x.NroPozo == model.NroPozo &&
                x.CodDetalleSalaProgresivo != model.CodDetalleSalaProgresivo &&
                x.Activo);

            if(duplicado != null)
                return Json(new { ok = false, msg = $"Ya existe un registro con Nro Pozo {model.NroPozo}." });

            model.CodSalaProgresivo = codSalaProgresivo;
            if(string.IsNullOrWhiteSpace(model.NombrePozo))
                model.NombrePozo = $"Pozo {model.NroPozo}";
            model.FechaModificacion = DateTime.Now;
            if(model.FechaRegistro == default(DateTime))
                model.FechaRegistro = DateTime.Now;
            model.CodUsuario = User?.Identity?.Name ?? "system";

            var previo = existentes.FirstOrDefault(x => x.CodDetalleSalaProgresivo == model.CodDetalleSalaProgresivo);
            model.fechaIni = previo != null ? previo.fechaIni : SqlDateTime.MinValue.Value;
            model.fechaFin = previo != null ? previo.fechaFin : SqlDateTime.MinValue.Value;

            var ok = _detalleBL.EditarADM_DetalleSalaProgresivo(model);
            return Json(new { ok, msg = ok ? "Pozo actualizado." : "No se pudo actualizar.", value = model });
        }

        [HttpPost]
        public ActionResult EliminarPozo(int id, int codSala, int codSalaProgresivo) {
            var lista = _detalleBL.GetListadoADM_DetalleSalaProgresivoPorCodSalaProgresivo(codSalaProgresivo) ?? new List<ADM_DetalleSalaProgresivoEntidad>();
            var it = lista.FirstOrDefault(x => x.CodDetalleSalaProgresivo == id);
            if(it == null) {
                TempData["Error"] = "Pozo no encontrado.";
                return RedirectToAction("Pozos", new { salaId = codSala, progresivoId = codSalaProgresivo });
            }

            it.Activo = false;
            it.Estado = 0;
            it.FechaModificacion = DateTime.Now;
            it.CodUsuario = User?.Identity?.Name ?? "system";

            var ok = _detalleBL.EditarADM_DetalleSalaProgresivo(it);
            TempData[ok ? "Ok" : "Error"] = ok ? "Pozo eliminado." : "No se pudo eliminar.";
            return RedirectToAction("Pozos", new { salaId = codSala, progresivoId = codSalaProgresivo });
        }

        //[HttpPost]
        //public ActionResult SincronizarPozosAjax(int codSala, int codSalaProgresivo) {
        //    var progs = _bl.GetListadoADM_SalaProgresivoPorSala(codSala) ?? new List<ADM_SalaProgresivoEntidad>();
        //    var prog = progs.FirstOrDefault(x => x.CodSalaProgresivo == codSalaProgresivo);
        //    if(prog == null) {
        //        TempData["Error"] = "Progresivo no encontrado.";
        //        return RedirectToAction("Pozos", new { salaId = codSala, progresivoId = codSalaProgresivo });
        //    }
        //    SincronizarPozos(codSalaProgresivo, prog.NroPozos);
        //    TempData["Ok"] = "Pozos sincronizados.";
        //    return RedirectToAction("Pozos", new { salaId = codSala, progresivoId = codSalaProgresivo });
        //}

        [HttpGet]
        public ActionResult AsignarMaquinas(int? salaId, int? progresivoId) {
            CargarCombos(salaId);
            ViewBag.CodSala = salaId ?? 0;
            ViewBag.CodSalaProgresivo = progresivoId ?? 0;
            return View("AsignarMaquinas", new List<ADM_MaquinaSalaProgresivoEntidad>());
        }

        [HttpGet]
        public JsonResult AsignacionesPorProgresivo(int codSalaProgresivo) {
            var lista = _maqSalaBL.GetListadoADM_MaquinaSalaProgresivoPorCodSalaProgresivo(codSalaProgresivo) ?? new List<ADM_MaquinaSalaProgresivoEntidad>();
            return Json(new { ok = true, value = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult MaquinasAsignadas(int codSala, int codSalaProgresivo, DateTime? desde = null, DateTime? hasta = null) {
            var asign = _maqSalaBL
                .GetListadoADM_MaquinaSalaProgresivoPorCodSalaProgresivo(codSalaProgresivo)
                ?? new List<ADM_MaquinaSalaProgresivoEntidad>();

            if(desde.HasValue)
                asign = asign.Where(a => a.FechaEnlace != default(DateTime) && a.FechaEnlace >= desde.Value).ToList();

            if(hasta.HasValue) {
                var hastaEnd = hasta.Value.Date.AddDays(1).AddTicks(-1);
                asign = asign.Where(a => a.FechaEnlace != default(DateTime) && a.FechaEnlace <= hastaEnd).ToList();
            }

            var maquinasSala = _maqBL.GetListadoADM_MaquinaPorSala(codSala) ?? new List<ADM_MaquinaEntidad>();
            var meta = maquinasSala.ToDictionary(m => m.CodMaquina, m => new { Ley = m.CodMaquinaLey ?? "", Alterno = m.CodAlterno ?? "" });

            var rows = asign.Select(a => new {
                a.CodMaquinaSalaProgresivo,
                a.CodMaquina,
                ley = meta.ContainsKey(a.CodMaquina) ? meta[a.CodMaquina].Ley : "",
                alterno = meta.ContainsKey(a.CodMaquina) ? meta[a.CodMaquina].Alterno : "",
                a.FechaEnlace,
                FechaEnlaceStr = (a.FechaEnlace == default(DateTime)) ? "" : a.FechaEnlace.ToString("dd/MM/yyyy HH:mm"),
                a.Activo,
                a.Estado
            }).ToList();

            return Json(new { ok = true, value = rows }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult MaquinasDisponiblesPorSala(int codSala, int codSalaProgresivo) {
            var maquinasSala = _maqBL.GetListadoADM_MaquinaPorSala(codSala)
                                ?? new List<ADM_MaquinaEntidad>();

            var asignadas = new HashSet<int>(
                  _maqSalaBL.GetListadoADM_MaquinaSalaProgresivoPorCodSalaProgresivo(codSalaProgresivo)?
                      .Where(x => x.Activo && x.Estado == 1)
                      .Select(x => x.CodMaquina)
                  ?? Enumerable.Empty<int>()
              );

            var disponibles = maquinasSala
                .Where(m => !asignadas.Contains(m.CodMaquina))
                .Select(m => new {
                    id = m.CodMaquina,
                    ley = m.CodMaquinaLey ?? "",
                    alterno = m.CodAlterno ?? ""
                })
                .OrderBy(x => x.ley).ThenBy(x => x.alterno).ThenBy(x => x.id)
                .ToList();

            return Json(new { ok = true, value = disponibles }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult MaquinasPicklist(int codSala, int codSalaProgresivo) {
            var maquinasSala = _maqBL.GetListadoADM_MaquinaPorSala(codSala) ?? new List<ADM_MaquinaEntidad>();
            var asignAct = _maqSalaBL.GetListadoADM_MaquinaSalaProgresivoPorCodSalaProgresivo(codSalaProgresivo)
                          ?.Where(a => a.Activo && a.Estado == 1)
                          .ToList() ?? new List<ADM_MaquinaSalaProgresivoEntidad>();

            var asignCods = new HashSet<int>(asignAct.Select(a => a.CodMaquina));
            var dispDto = maquinasSala
                .Where(m => !asignCods.Contains(m.CodMaquina))
                .Select(m => new {
                    CodMaquina = m.CodMaquina,
                    Ley = m.CodMaquinaLey ?? "",
                    Alterno = m.CodAlterno ?? ""
                })
                .OrderBy(x => x.Ley).ThenBy(x => x.Alterno).ThenBy(x => x.CodMaquina)
                .ToList();

            var meta = maquinasSala.ToDictionary(m => m.CodMaquina, m => new { Ley = m.CodMaquinaLey ?? "", Alterno = m.CodAlterno ?? "" });
            var asigDto = asignAct
                .Select(a => new {
                    a.CodMaquinaSalaProgresivo,
                    a.CodMaquina,
                    Ley = meta.ContainsKey(a.CodMaquina) ? meta[a.CodMaquina].Ley : "",
                    Alterno = meta.ContainsKey(a.CodMaquina) ? meta[a.CodMaquina].Alterno : ""
                })
                .OrderBy(x => x.Ley).ThenBy(x => x.Alterno).ThenBy(x => x.CodMaquina)
                .ToList();

            return Json(new { ok = true, disponibles = dispDto, asignadas = asigDto }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AsignarMaquinasBulk(int codSala, int codSalaProgresivo, int[] codMaquinas) {
            if(codSala <= 0 || codSalaProgresivo <= 0 || codMaquinas == null || codMaquinas.Length == 0)
                return Json(new { ok = false, msg = "Parámetros inválidos." });

            int okCount = 0, errCount = 0;
            var usuario = User?.Identity?.Name ?? "system";

            foreach(var codM in codMaquinas.Distinct()) {
                try {
                    if(AsignarOActivarMaquina(codSalaProgresivo, codM, usuario))
                        okCount++;
                    else
                        errCount++;
                } catch { errCount++; }
            }

            return Json(new { ok = errCount == 0, asignadas = okCount, fallidas = errCount });
        }

        [HttpPost]
        public JsonResult QuitarMaquinasBulkPorAsignacion(int codSala, int codSalaProgresivo, int[] idsAsignacion) {
            if(codSala <= 0 || codSalaProgresivo <= 0 || idsAsignacion == null || idsAsignacion.Length == 0)
                return Json(new { ok = false, msg = "Parámetros inválidos." });

            int okCount = 0, errCount = 0;
            var usuario = User?.Identity?.Name ?? "system";

            foreach(var id in idsAsignacion.Distinct()) {
                try {
                    if(DesactivarAsignacion(id, codSalaProgresivo, usuario))
                        okCount++;
                    else
                        errCount++;
                } catch { errCount++; }
            }

            return Json(new { ok = errCount == 0, desasignadas = okCount, fallidas = errCount });
        }

        [HttpPost]
        public ActionResult AsignarMaquina(int codSala, int codSalaProgresivo, int codMaquina) {
            if(codSala <= 0 || codSalaProgresivo <= 0 || codMaquina <= 0) {
                TempData["Error"] = "Datos inválidos.";
                return RedirectToAction("AsignarMaquinas", new { salaId = codSala, progresivoId = codSalaProgresivo });
            }

            var ok = AsignarOActivarMaquina(codSalaProgresivo, codMaquina, User?.Identity?.Name ?? "system");
            TempData[ok ? "Ok" : "Error"] = ok ? "Máquina asignada." : "No se pudo asignar.";

            return RedirectToAction("AsignarMaquinas", new { salaId = codSala, progresivoId = codSalaProgresivo });
        }


        [HttpPost]
        public ActionResult QuitarMaquina(int id, int codSala, int codSalaProgresivo) {
            var ok = DesactivarAsignacion(id, codSalaProgresivo, User?.Identity?.Name ?? "system");
            TempData[ok ? "Ok" : "Error"] = ok ? "Máquina desasignada." : "No se pudo desasignar.";
            return RedirectToAction("AsignarMaquinas", new { salaId = codSala, progresivoId = codSalaProgresivo });
        }


        [HttpGet]
        public JsonResult ProgresivosAsignadosPorMaquina(int codSala, int codMaquina, DateTime? desde = null, DateTime? hasta = null) {
            var progs = _bl.GetListadoADM_SalaProgresivoPorSala(codSala) ?? new List<ADM_SalaProgresivoEntidad>();
            var mapProg = progs.ToDictionary(p => p.CodSalaProgresivo, p => p.Nombre ?? $"Prog #{p.CodSalaProgresivo}");

            var asigns = progs.SelectMany(p =>
                            _maqSalaBL.GetListadoADM_MaquinaSalaProgresivoPorCodSalaProgresivo(p.CodSalaProgresivo)
                            ?? new List<ADM_MaquinaSalaProgresivoEntidad>())
                        .Where(a => a.CodMaquina == codMaquina);

            if(desde.HasValue)
                asigns = asigns.Where(a => a.FechaEnlace != default(DateTime) && a.FechaEnlace >= desde.Value);

            if(hasta.HasValue) {
                var hastaEnd = hasta.Value.Date.AddDays(1).AddTicks(-1); 
                asigns = asigns.Where(a => a.FechaEnlace != default(DateTime) && a.FechaEnlace <= hastaEnd);
            }

            var rows = asigns
                .Select(a => new {
                    a.CodMaquinaSalaProgresivo,
                    a.CodSalaProgresivo,
                    ProgresivoNombre = mapProg.TryGetValue(a.CodSalaProgresivo, out var nom) ? nom : $"Prog #{a.CodSalaProgresivo}",
                    a.Activo,
                    a.Estado,
                    a.FechaEnlace,
                    FechaEnlaceStr = (a.FechaEnlace == default(DateTime)) ? "" : a.FechaEnlace.ToString("dd/MM/yyyy HH:mm")
                })
                .OrderByDescending(x => x.Activo).ThenBy(x => x.CodSalaProgresivo)
                .ToList();

            return Json(new { ok = true, value = rows }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult MaquinasSinAsignacion(int? codSala = null) {
            var lista = _maqSalaBL.GetMaquinasActivasSinAsignacion(codSala) ?? new List<ADM_MaquinaEntidad>();

            var value = lista
                .Select(m => new {
                    id = m.CodMaquina,
                    ley = m.CodMaquinaLey ?? "",
                    alterno = m.CodAlterno ?? "",
                    codSala = m.CodSala
                })
                .OrderBy(x => x.ley)
                .ThenBy(x => x.alterno)
                .ThenBy(x => x.id)
                .ToList();

            return Json(new { ok = true, value }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditarAsignacion(int id, int codSala, int codSalaProgresivo, bool Activo = true, int Estado = 1) {
            if(id <= 0 || codSalaProgresivo <= 0) {
                TempData["Error"] = "Datos inválidos para editar la asignación.";
                return RedirectToAction("AsignarMaquinas", new { salaId = codSala, progresivoId = codSalaProgresivo });
            }

            var lista = _maqSalaBL.GetListadoADM_MaquinaSalaProgresivoPorCodSalaProgresivo(codSalaProgresivo)
                        ?? new List<ADM_MaquinaSalaProgresivoEntidad>();
            var it = lista.FirstOrDefault(x => x.CodMaquinaSalaProgresivo == id);
            if(it == null) {
                TempData["Error"] = "Asignación no encontrada.";
                return RedirectToAction("AsignarMaquinas", new { salaId = codSala, progresivoId = codSalaProgresivo });
            }

            it.Activo = Activo;
            it.Estado = Estado;

            it.FechaModificacion = DateTime.Now;
            //p.CodUsuario = User?.Identity?.Name ?? "system";

            var ok = _maqSalaBL.EditarADM_MaquinaSalaProgresivo(it);
            TempData[ok ? "Ok" : "Error"] = ok ? "Asignación actualizada." : "No se pudo actualizar la asignación.";

            return RedirectToAction("AsignarMaquinas", new { salaId = codSala, progresivoId = codSalaProgresivo });
        }

        
        [HttpGet]
        public ActionResult ExportarExcelProgresivos(int id, string clase = "Linktek") {
            var data = _bl.GetListadoADM_SalaProgresivoPorSala(id) ?? new List<ADM_SalaProgresivoEntidad>();
            clase = string.IsNullOrWhiteSpace(clase) ? "Linktek" : clase;
            data = data.Where(x => (string.IsNullOrWhiteSpace(x.ClaseProgresivo) ? "Linktek" : x.ClaseProgresivo) == clase).ToList();

            var salaInfo = _salaBL.SalaListaIdJson(id);
            var nombreSala = salaInfo?.Nombre ?? $"Sala {id}";
            var ahora = DateTime.Now;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using(var pck = new ExcelPackage()) {
                var ws = pck.Workbook.Worksheets.Add($"Progresivos_{clase}");

                var headers = new[]
                {
                    "CodSalaProgresivo","Nombre","Nombre Sala","Nro Pozos","Nro Jugadores",
                    "Subida Créditos","Fecha Instalación","Fecha Desinstalación","Activo",
                    "Estado","CodProgresivoWO","RazonSocial"
                };

                int titleRow = 1, infoRow = 2, headerRow = 4, firstDataRow = headerRow + 1;

                var titulo = $"Reporte de Progresivos — {clase} — {nombreSala}";
                ws.Cells[titleRow, 1].Value = titulo;
                ws.Cells[titleRow, 1, titleRow, headers.Length].Merge = true;
                ws.Cells[titleRow, 1, titleRow, headers.Length].Style.Font.Bold = true;
                ws.Cells[titleRow, 1, titleRow, headers.Length].Style.Font.Size = 14;
                ws.Cells[titleRow, 1, titleRow, headers.Length].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[infoRow, 1].Value = $"Generado: {ahora:dd/MM/yyyy HH:mm:ss}";
                ws.Cells[infoRow, 1, infoRow, headers.Length].Merge = true;
                ws.Cells[infoRow, 1, infoRow, headers.Length].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[infoRow, 1, infoRow, headers.Length].Style.Font.Italic = true;

                for(int c = 0; c < headers.Length; c++)
                    ws.Cells[headerRow, c + 1].Value = headers[c];

                using(var rng = ws.Cells[headerRow, 1, headerRow, headers.Length]) {
                    rng.Style.Font.Bold = true;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 234, 244));
                    rng.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                int r = firstDataRow;
                foreach(var s in data) {
                    ws.Cells[r, 1].Value = s.CodSalaProgresivo;
                    ws.Cells[r, 2].Value = s.Nombre ?? "";
                    ws.Cells[r, 3].Value = s.NombreSala ?? "";
                    ws.Cells[r, 4].Value = s.NroPozos;
                    ws.Cells[r, 5].Value = s.NroJugadores;
                    ws.Cells[r, 6].Value = s.SubidaCreditos;
                    if(s.FechaInstalacion != default(DateTime)) { ws.Cells[r, 7].Value = s.FechaInstalacion; ws.Cells[r, 7].Style.Numberformat.Format = "yyyy-mm-dd"; }
                    if(s.FechaDesinstalacion != default(DateTime)) { ws.Cells[r, 8].Value = s.FechaDesinstalacion; ws.Cells[r, 8].Style.Numberformat.Format = "yyyy-mm-dd"; }
                    ws.Cells[r, 9].Value = s.Activo ? "Sí" : "No";
                    ws.Cells[r, 10].Value = s.Estado == 1 ? "Habilitado" : "Deshabilitado";
                    ws.Cells[r, 11].Value = s.CodProgresivoWO;
                    ws.Cells[r, 12].Value = s.RazonSocial ?? "";
                    r++;
                }

                int lastDataRow = Math.Max(firstDataRow, r - 1);
                if(lastDataRow >= firstDataRow)
                    ws.Cells[4, 1, lastDataRow, headers.Length].AutoFilter = true;
                ws.View.FreezePanes(firstDataRow, 1);
                ws.Cells.AutoFitColumns();

                var bytes = pck.GetAsByteArray();
                var fileName = $"Progresivos_{clase}_Sala_{id}_{ahora:yyyyMMdd_HHmmss}.xlsx";
                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        [HttpGet]
        public ActionResult ExportarExcelPozos(int salaId, int codSalaProgresivo) {
            var progs = _bl.GetListadoADM_SalaProgresivoPorSala(salaId) ?? new List<ADM_SalaProgresivoEntidad>();
            var prog = progs.FirstOrDefault(x => x.CodSalaProgresivo == codSalaProgresivo);
            var salaInfo = _salaBL.SalaListaIdJson(salaId);
            var nombreSala = salaInfo?.Nombre ?? $"Sala {salaId}";
            var nombreProg = prog?.Nombre ?? $"Prog #{codSalaProgresivo}";
            var ahora = DateTime.Now;

            var data = _detalleBL.GetListadoADM_DetalleSalaProgresivoPorCodSalaProgresivo(codSalaProgresivo)
                       ?? new List<ADM_DetalleSalaProgresivoEntidad>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using(var pck = new ExcelPackage()) {
                var ws = pck.Workbook.Worksheets.Add("Pozos");

                var headers = new[] {
                    "NroPozo","NombrePozo","Dificultad",
                    "MontoBase","MontoIni","MontoFin","Modalidad",
                    "MontoOcultoBase","MontoOcultoIni","MontoOcultoFin",
                    "Incremento","IncrementoPozoOculto",
                    "FechaIni","FechaFin",
                    "Activo","Estado",
                    "FechaRegistro","FechaModificacion",
                    "CodDetalleSalaProgresivo","CodSalaProgresivo","CodProgresivoExterno"
                };

                int titleRow = 1, infoRow = 2, headerRow = 4, firstDataRow = headerRow + 1;

                ws.Cells[titleRow, 1].Value = $"Pozos — {nombreProg} — {nombreSala}";
                ws.Cells[titleRow, 1, titleRow, headers.Length].Merge = true;
                ws.Cells[titleRow, 1, titleRow, headers.Length].Style.Font.Bold = true;
                ws.Cells[titleRow, 1, titleRow, headers.Length].Style.Font.Size = 14;
                ws.Cells[titleRow, 1, titleRow, headers.Length].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[infoRow, 1].Value = $"Generado: {ahora:dd/MM/yyyy HH:mm:ss}";
                ws.Cells[infoRow, 1, infoRow, headers.Length].Merge = true;
                ws.Cells[infoRow, 1, infoRow, headers.Length].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[infoRow, 1, infoRow, headers.Length].Style.Font.Italic = true;

                for(int c = 0; c < headers.Length; c++)
                    ws.Cells[headerRow, c + 1].Value = headers[c];

                using(var rng = ws.Cells[headerRow, 1, headerRow, headers.Length]) {
                    rng.Style.Font.Bold = true;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 234, 244));
                    rng.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                int r = firstDataRow;
                foreach(var d in data.OrderBy(x => x.NroPozo)) {
                    int c = 1;
                    ws.Cells[r, c++].Value = d.NroPozo;
                    ws.Cells[r, c++].Value = d.NombrePozo ?? $"Pozo {d.NroPozo}";
                    ws.Cells[r, c++].Value = d.Dificultad;

                    ws.Cells[r, c++].Value = d.MontoBase;        // 0.000
                    ws.Cells[r, c++].Value = d.MontoIni;         // 0.000
                    ws.Cells[r, c++].Value = d.MontoFin;         // 0.000
                    ws.Cells[r, c++].Value = d.Modalidad;

                    ws.Cells[r, c++].Value = d.MontoOcultoBase;  // 0.000
                    ws.Cells[r, c++].Value = d.MontoOcultoIni;   // 0.000
                    ws.Cells[r, c++].Value = d.MontoOcultoFin;   // 0.000

                    ws.Cells[r, c++].Value = d.Incremento;            // 0.0000
                    ws.Cells[r, c++].Value = d.IncrementoPozoOculto;  // 0.000

                    if(d.fechaIni != default(DateTime)) { ws.Cells[r, c].Value = d.fechaIni; ws.Cells[r, c].Style.Numberformat.Format = "yyyy-mm-dd"; }
                    c++;
                    if(d.fechaFin != default(DateTime)) { ws.Cells[r, c].Value = d.fechaFin; ws.Cells[r, c].Style.Numberformat.Format = "yyyy-mm-dd"; }
                    c++;

                    ws.Cells[r, c++].Value = d.Activo ? "Sí" : "No";
                    ws.Cells[r, c++].Value = d.Estado == 1 ? "Habilitado" : "Deshabilitado";

                    if(d.FechaRegistro != default(DateTime)) { ws.Cells[r, c].Value = d.FechaRegistro; ws.Cells[r, c].Style.Numberformat.Format = "yyyy-mm-dd HH:mm"; }
                    c++;
                    if(d.FechaModificacion != default(DateTime)) { ws.Cells[r, c].Value = d.FechaModificacion; ws.Cells[r, c].Style.Numberformat.Format = "yyyy-mm-dd HH:mm"; }
                    c++;

                    ws.Cells[r, c++].Value = d.CodDetalleSalaProgresivo;
                    ws.Cells[r, c++].Value = d.CodSalaProgresivo;
                    ws.Cells[r, c++].Value = d.CodProgresivoExterno;

                    r++;
                }

                void fmt(int col, string format) { ws.Column(col).Style.Numberformat.Format = format; }
                foreach(var col in new[] { 4, 5, 6, 8, 9, 10, 12 })
                    fmt(col, "0.000"); 
                fmt(11, "0.0000"); 

                int lastDataRow = Math.Max(firstDataRow, r - 1);
                if(lastDataRow >= firstDataRow)
                    ws.Cells[headerRow, 1, lastDataRow, headers.Length].AutoFilter = true;

                ws.View.FreezePanes(firstDataRow, 1);
                ws.Cells.AutoFitColumns();

                var bytes = pck.GetAsByteArray();
                var fileName = $"Pozos_Prog_{codSalaProgresivo}_Sala_{salaId}_{ahora:yyyyMMdd_HHmmss}.xlsx";
                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        [HttpGet]
        public ActionResult ExportarExcelAsignacionesPorMaquina(int salaId, int codMaquina, DateTime? desde = null, DateTime? hasta = null) {
            var progs = _bl.GetListadoADM_SalaProgresivoPorSala(salaId) ?? new List<ADM_SalaProgresivoEntidad>();
            var mapProg = progs.ToDictionary(p => p.CodSalaProgresivo, p => p.Nombre ?? $"Prog #{p.CodSalaProgresivo}");

            var asigns = progs.SelectMany(p =>
                            _maqSalaBL.GetListadoADM_MaquinaSalaProgresivoPorCodSalaProgresivo(p.CodSalaProgresivo)
                            ?? new List<ADM_MaquinaSalaProgresivoEntidad>())
                        .Where(a => a.CodMaquina == codMaquina);

            if(desde.HasValue)
                asigns = asigns.Where(a => a.FechaEnlace != default(DateTime) && a.FechaEnlace >= desde.Value);
            if(hasta.HasValue) {
                var hastaEnd = hasta.Value.Date.AddDays(1).AddTicks(-1);
                asigns = asigns.Where(a => a.FechaEnlace != default(DateTime) && a.FechaEnlace <= hastaEnd);
            }

            var asignList = asigns.OrderByDescending(a => a.Activo).ThenBy(a => a.CodSalaProgresivo).ToList();

            var salaInfo = _salaBL.SalaListaIdJson(salaId);
            var nombreSala = salaInfo?.Nombre ?? $"Sala {salaId}";
            var maq = (_maqBL.GetListadoADM_MaquinaPorSala(salaId) ?? new List<ADM_MaquinaEntidad>())
                        .FirstOrDefault(m => m.CodMaquina == codMaquina);
            var maqDesc = (maq?.CodMaquinaLey ?? maq?.CodAlterno ?? $"Máquina {codMaquina}") + $" [{codMaquina}]";
            var ahora = DateTime.Now;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using(var pck = new ExcelPackage()) {
                var ws = pck.Workbook.Worksheets.Add("Asignaciones");

                var filtroTxt = (desde.HasValue || hasta.HasValue)
                    ? $" — Filtro: {(desde.HasValue ? desde.Value.ToString("dd/MM/yyyy") : "∞")} a {(hasta.HasValue ? hasta.Value.ToString("dd/MM/yyyy") : "∞")}"
                    : "";

                var headers = new[] {
            "IdAsignación","CodProgresivo","Nombre Progresivo",
            "Fecha Enlace","Fecha Desactivación","Activo","Estado",
            "Fecha Registro","Fecha Modificación"
        };

                int titleRow = 1, infoRow = 2, headerRow = 4, firstDataRow = headerRow + 1;

                ws.Cells[titleRow, 1].Value = $"Asignaciones — {maqDesc} — {nombreSala}{filtroTxt}";
                ws.Cells[titleRow, 1, titleRow, headers.Length].Merge = true;
                ws.Cells[titleRow, 1, titleRow, headers.Length].Style.Font.Bold = true;
                ws.Cells[titleRow, 1, titleRow, headers.Length].Style.Font.Size = 14;
                ws.Cells[titleRow, 1, titleRow, headers.Length].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[infoRow, 1].Value = $"Generado: {ahora:dd/MM/yyyy HH:mm:ss}";
                ws.Cells[infoRow, 1, infoRow, headers.Length].Merge = true;
                ws.Cells[infoRow, 1, infoRow, headers.Length].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[infoRow, 1, infoRow, headers.Length].Style.Font.Italic = true;

                for(int c = 0; c < headers.Length; c++)
                    ws.Cells[headerRow, c + 1].Value = headers[c];

                using(var rng = ws.Cells[headerRow, 1, headerRow, headers.Length]) {
                    rng.Style.Font.Bold = true;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 234, 244));
                    rng.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                int r = firstDataRow;
                foreach(var a in asignList) {
                    int c = 1;
                    ws.Cells[r, c++].Value = a.CodMaquinaSalaProgresivo;
                    ws.Cells[r, c++].Value = a.CodSalaProgresivo;
                    ws.Cells[r, c++].Value = mapProg.TryGetValue(a.CodSalaProgresivo, out var nom) ? nom : $"Prog #{a.CodSalaProgresivo}";

                    if(a.FechaEnlace != default(DateTime)) { ws.Cells[r, c].Value = a.FechaEnlace; ws.Cells[r, c].Style.Numberformat.Format = "dd/MM/yyyy HH:mm"; }
                    c++;
                    if(a.FechaDesactivacion != default(DateTime)) { ws.Cells[r, c].Value = a.FechaDesactivacion; ws.Cells[r, c].Style.Numberformat.Format = "dd/MM/yyyy HH:mm"; }
                    c++;

                    ws.Cells[r, c++].Value = a.Activo ? "Sí" : "No";
                    ws.Cells[r, c++].Value = a.Estado == 1 ? "Habilitado" : "Deshabilitado";

                    if(a.FechaRegistro != default(DateTime)) { ws.Cells[r, c].Value = a.FechaRegistro; ws.Cells[r, c].Style.Numberformat.Format = "dd/MM/yyyy HH:mm"; }
                    c++;
                    if(a.FechaModificacion != default(DateTime)) { ws.Cells[r, c].Value = a.FechaModificacion; ws.Cells[r, c].Style.Numberformat.Format = "dd/MM/yyyy HH:mm"; }
                    c++;

                    r++;
                }

                int lastDataRow = Math.Max(firstDataRow, r - 1);
                if(lastDataRow >= firstDataRow)
                    ws.Cells[headerRow, 1, lastDataRow, headers.Length].AutoFilter = true;

                ws.View.FreezePanes(firstDataRow, 1);
                ws.Cells.AutoFitColumns();

                var bytes = pck.GetAsByteArray();
                var fileName = $"Asignaciones_Maq_{codMaquina}_Sala_{salaId}_{ahora:yyyyMMdd_HHmmss}.xlsx";
                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }


        private void SincronizarPozos(int codSalaProgresivo, int targetCount) {
            if(codSalaProgresivo <= 0)
                return;
            if(targetCount < 0)
                targetCount = 0;

            var detalles = _detalleBL.GetListadoADM_DetalleSalaProgresivoPorCodSalaProgresivo(codSalaProgresivo) ?? new List<ADM_DetalleSalaProgresivoEntidad>();

            foreach(var grp in detalles.GroupBy(d => d.NroPozo)) {
                var keep = grp.OrderBy(d => d.CodDetalleSalaProgresivo).First();
                foreach(var dup in grp.Where(d => d.CodDetalleSalaProgresivo != keep.CodDetalleSalaProgresivo)) {
                    if(dup.Activo || dup.Estado != 0) {
                        dup.Activo = false;
                        dup.Estado = 0;
                        dup.FechaModificacion = DateTime.Now;
                        dup.CodUsuario = User?.Identity?.Name ?? "system";
                        _detalleBL.EditarADM_DetalleSalaProgresivo(dup);
                    }
                }
            }

            detalles = _detalleBL.GetListadoADM_DetalleSalaProgresivoPorCodSalaProgresivo(codSalaProgresivo) ?? new List<ADM_DetalleSalaProgresivoEntidad>();
            var byNumber = detalles.Where(d => d.Activo && d.Estado == 1).ToDictionary(x => x.NroPozo, x => x);

            for(int n = 1; n <= targetCount; n++) {
                if(byNumber.ContainsKey(n)) {
                    var it = byNumber[n];
                    if(string.IsNullOrWhiteSpace(it.NombrePozo)) {
                        it.NombrePozo = $"Pozo {n}";
                        it.FechaModificacion = DateTime.Now;
                        it.CodUsuario = User?.Identity?.Name ?? "system";
                        _detalleBL.EditarADM_DetalleSalaProgresivo(it);
                    }
                    continue;
                }

                var nuevo = new ADM_DetalleSalaProgresivoEntidad {
                    CodSalaProgresivo = codSalaProgresivo,
                    NroPozo = n,
                    NombrePozo = $"Pozo {n}",
                    Dificultad = 0,
                    MontoBase = 0,
                    MontoIni = 0,
                    MontoFin = 0,
                    Modalidad = 0,
                    MontoOcultoBase = 0,
                    MontoOcultoIni = 0,
                    MontoOcultoFin = 0,
                    Incremento = 0,
                    IncrementoPozoOculto = 0,
                    FechaRegistro = DateTime.Now,
                    FechaModificacion = DateTime.Now,
                    Activo = true,
                    Estado = 1,
                    CodProgresivoExterno = 0,
                    CodUsuario = User?.Identity?.Name ?? "system",
                    fechaIni = DateTime.Now,
                    fechaFin = DateTime.Now
                };
                _detalleBL.GuardarADM_DetalleSalaProgresivo(nuevo);
            }

            foreach(var d in detalles.Where(d => d.NroPozo > targetCount)) {
                if(d.Activo || d.Estado != 0) {
                    d.Activo = false;
                    d.Estado = 0;
                    d.FechaModificacion = DateTime.Now;
                    d.CodUsuario = User?.Identity?.Name ?? "system";
                    _detalleBL.EditarADM_DetalleSalaProgresivo(d);
                }
            }
        }

        [HttpGet]
        [Route("ProgresivoTerceros/Sala/{codSala:int}/MaquinasTerceros")]
        public JsonResult MaquinasTercerosPorSala(int codSala, bool soloActivas = true) {
            try {
                if(codSala <= 0)
                    return Json(new {
                        ok = false,
                        msg = "Código de sala inválido."
                    },
                    JsonRequestBehavior.AllowGet);

                var progresivos = _bl.GetListadoADM_SalaProgresivoPorSala(codSala) ?? new List<ADM_SalaProgresivoEntidad>();
                var terceros = progresivos
                    .Where(p => (p.ClaseProgresivo ?? "Linktek") == "Terceros")
                    .ToList();

                if(!terceros.Any())
                    return Json(new { ok = true, value = new List<object>() }, JsonRequestBehavior.AllowGet);

                var maquinasSala = _maqBL.GetListadoADM_MaquinaPorSala(codSala) ?? new List<ADM_MaquinaEntidad>();
                var metaMaquina = maquinasSala.ToDictionary(
                    m => m.CodMaquina,
                    m => new {
                        //Ley = m.CodMaquinaLey ?? "",
                        Alterno = m.CodAlterno ?? ""
                    }
                );

                var rows = new List<object>();
                foreach(var prog in terceros) {
                    var asigns = _maqSalaBL.GetListadoADM_MaquinaSalaProgresivoPorCodSalaProgresivo(prog.CodSalaProgresivo)
                                ?? new List<ADM_MaquinaSalaProgresivoEntidad>();

                    if(soloActivas)
                        asigns = asigns.Where(a => a.Activo && a.Estado == 1).ToList();

                    foreach(var a in asigns) {
                        metaMaquina.TryGetValue(a.CodMaquina, out var meta);

                        rows.Add(new {
                            codMaquina = a.CodMaquina,
                            //ley = meta?.Ley ?? "",
                            alterno = meta?.Alterno ?? "",
                            codSalaProgresivo = prog.CodSalaProgresivo,
                            progresivoNombre = string.IsNullOrWhiteSpace(prog.Nombre) ? $"Prog #{prog.CodSalaProgresivo}" : prog.Nombre,
                            claseProgresivo = prog.ClaseProgresivo ?? "Linktek",
                            activo = a.Activo,
                            estado = a.Estado,
                            fechaEnlace = a.FechaEnlace,
                            fechaEnlaceStr = (a.FechaEnlace == default(DateTime)) ? "" : a.FechaEnlace.ToString("dd/MM/yyyy HH:mm"),
                            fechaDesactivacion = a.FechaDesactivacion == default(DateTime) ? (DateTime?)null : a.FechaDesactivacion,
                            fechaDesactivacionStr = (a.FechaDesactivacion == default(DateTime)) ? "" : a.FechaDesactivacion.ToString("dd/MM/yyyy HH:mm"),
                            fechaRegistro = a.FechaRegistro == default(DateTime) ? (DateTime?)null : a.FechaRegistro,
                            fechaModificacion = a.FechaModificacion == default(DateTime) ? (DateTime?)null : a.FechaModificacion
                        });
                    }
                }

                var ordenado = rows
                    .OrderByDescending(r => ((dynamic)r).activo)
                    //.ThenBy(r => ((dynamic)r).ley)
                    .ThenBy(r => ((dynamic)r).alterno)
                    .ThenBy(r => ((dynamic)r).codMaquina)
                    .ToList();

                return Json(new {
                    ok = true,
                    value = ordenado
                }, JsonRequestBehavior.AllowGet);
            } catch(Exception ex) {
                return Json(new {
                    ok = false,
                    msg = "No se pudo listar las máquinas.",
                    detail = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("ProgresivoTerceros/ProgresivoPozosPorMaquina")]
        public JsonResult ProgresivoPozosPorMaquina(int codSala, int codMaquina) {
            try {
                if(codSala <= 0 || codMaquina <= 0)
                    return Json(new { ok = false, msg = "Parámetros inválidos." }, JsonRequestBehavior.AllowGet);

                var progsSala = _bl.GetListadoADM_SalaProgresivoPorSala(codSala) ?? new List<ADM_SalaProgresivoEntidad>();
                if(!progsSala.Any())
                    return Json(new { ok = true, value = (object)null, montos = new object[0] }, JsonRequestBehavior.AllowGet);

                var asigns = progsSala
                    .SelectMany(p => _maqSalaBL.GetListadoADM_MaquinaSalaProgresivoPorCodSalaProgresivo(p.CodSalaProgresivo)
                                        ?? new List<ADM_MaquinaSalaProgresivoEntidad>())
                    .Where(a => a.CodMaquina == codMaquina)
                    .ToList();

                if(!asigns.Any())
                    return Json(new { ok = true, value = (object)null, montos = new object[0] }, JsonRequestBehavior.AllowGet);

                var asignActual = asigns
                    .Where(a => a.Activo && a.Estado == 1)
                    .OrderByDescending(a => a.FechaEnlace)
                    .FirstOrDefault()
                    ?? asigns.OrderByDescending(a => a.FechaEnlace).First();

                var prog = progsSala.FirstOrDefault(p => p.CodSalaProgresivo == asignActual.CodSalaProgresivo);
                if(prog == null)
                    return Json(new { ok = false, msg = "No se encontró el progresivo asociado." }, JsonRequestBehavior.AllowGet);

                var detalles = (_detalleBL.GetListadoADM_DetalleSalaProgresivoPorCodSalaProgresivo(prog.CodSalaProgresivo)
                                ?? new List<ADM_DetalleSalaProgresivoEntidad>())
                               .Where(d => d.Activo && d.Estado == 1)
                               .OrderBy(d => d.NroPozo)
                               .ToList();
                var listaDetalles = detalles.Select(d => new {
                    d.CodDetalleSalaProgresivo,
                    d.CodSalaProgresivo,
                    d.NroPozo,
                    d.NombrePozo
                }).ToList();

                var montos = new List<object>();
                foreach(var d in detalles) {
                  
                    var historial = _pozoHistBL.GetHistoricoPorDetalle(d.CodDetalleSalaProgresivo)
                                    ?? new List<ADM_PozoHistoricoEntidad>();

                    var orden = historial
                        .OrderByDescending(h => h.FechaOperacion)
                        .ThenByDescending(h => h.FechaRegistro)
                        .FirstOrDefault();

                    montos.Add(new {
                        CodDetalleSalaProgresivo = d.CodDetalleSalaProgresivo,
                        MontoActualSala = orden?.MontoActualSala ?? 0.00m
                    });
                }
                var result = new {
                    CodSalaProgresivo = prog.CodSalaProgresivo,
                    CodSala = codSala,
                    CodProgresivo = prog.CodProgresivo,
                    Nombre = prog.Nombre,
                    NroPozos = listaDetalles.Count,
                    DetalleSalaProgresivo = listaDetalles
                };

                return Json(new { ok = true, value = result, montos = montos }, JsonRequestBehavior.AllowGet);
            } catch(Exception ex) {
                return Json(new { ok = false, msg = "No se pudo obtener los datos.", detail = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("ProgresivoTerceros/PozoHistorico/RegistrarPozoHistorico")]
        [ValidateInput(false)]
        public JsonResult RegistrarPozoHistorico(List<ADM_PozoHistoricoEntidad> listaPozos, bool actualizarSiExiste = false) {
            if(listaPozos == null || listaPozos.Count == 0)
                return Json(new { mensaje = "La lista está vacía." });

            var fechaActual = DateTime.Now;
            bool status = false;
            bool duplicados = false;

            foreach(var p in listaPozos) {
                try {
                    if(p == null || p.CodDetalleSalaProgresivo <= 0)
                        continue;

                    p.CodUsuario = string.Empty;
                    p.FechaOperacion = fechaActual;
                    p.FechaRegistro = fechaActual;
                    p.FechaModificacion = fechaActual;
                    p.MontoOcultoActualAutomatico = 0.00m;
                    p.MontoOcultoActualSala = 0.00m;
                    p.Estado = 1;
                    p.Activo = true;

                    var registrosMismoDia = _pozoHistBL
                        .GetListadoADM_PozoHistoricoPorCodDetalleSalaProgresivoYFecha(
                            p.CodDetalleSalaProgresivo, p.FechaOperacion);

                    if(registrosMismoDia != null && registrosMismoDia.Count > 0) {
                        if(!actualizarSiExiste) {
                            duplicados = true;
                            continue;
                        }

                        var registroParaActualizar = registrosMismoDia
                            .OrderByDescending(x => x.FechaRegistro)
                            .ThenByDescending(x => x.CodPozoHistorico)
                            .First();

                        p.CodPozoHistorico = registroParaActualizar.CodPozoHistorico;

                        if(_pozoHistBL.EditarADM_PozoHistorico(p))
                            status = true;

                        continue;
                    }

                    var idInsertado = _pozoHistBL.GuardarADM_PozoHistorico(p);
                    if(idInsertado > 0)
                        status = true;
                } catch {
                }
            }

            var mensaje =
                status ? "Se registró con éxito."
              : duplicados ? "Ya existe un registro."
              : "No se registró ningún dato.";

            return Json(new { mensaje });
        }


        [HttpGet]
        [Route("ProgresivoTerceros/ObtenerRegistrosPozoHistorico")]
        public JsonResult ObtenerRegistrosPozoHistorico(int codSala, int codMaquina) {
            try {
                if(codSala <= 0 || codMaquina <= 0)
                    return Json(new { ok = false, msg = "Parámetros inválidos." }, JsonRequestBehavior.AllowGet);

                var progsSala = _bl.GetListadoADM_SalaProgresivoPorSala(codSala) ?? new List<ADM_SalaProgresivoEntidad>();
                if(!progsSala.Any())
                    return Json(new { ok = true, value = new { codSala, codMaquina }, registros = new object[0] }, JsonRequestBehavior.AllowGet);

                var asigns = progsSala
                    .SelectMany(p => _maqSalaBL.GetListadoADM_MaquinaSalaProgresivoPorCodSalaProgresivo(p.CodSalaProgresivo)
                                        ?? new List<ADM_MaquinaSalaProgresivoEntidad>())
                    .Where(a => a.CodMaquina == codMaquina)
                    .ToList();

                if(!asigns.Any())
                    return Json(new { ok = true, value = new { codSala, codMaquina }, registros = new object[0] }, JsonRequestBehavior.AllowGet);

                var asignActual = asigns
                    .Where(a => a.Activo && a.Estado == 1)
                    .OrderByDescending(a => a.FechaEnlace)
                    .FirstOrDefault()
                    ?? asigns.OrderByDescending(a => a.FechaEnlace).First();

                var prog = progsSala.FirstOrDefault(p => p.CodSalaProgresivo == asignActual.CodSalaProgresivo);
                if(prog == null)
                    return Json(new { ok = true, value = new { codSala, codMaquina }, registros = new object[0] }, JsonRequestBehavior.AllowGet);

                var detalles = _detalleBL.GetListadoADM_DetalleSalaProgresivoPorCodSalaProgresivo(prog.CodSalaProgresivo)
                              ?? new List<ADM_DetalleSalaProgresivoEntidad>();
                var pozosActivos = detalles.Where(d => d.Activo && d.Estado == 1).OrderBy(d => d.NroPozo).ToList();

                var fechaActual = DateTime.Now;
                var registros = new List<object>();

                foreach(var d in pozosActivos) {
                    var actual = _pozoHistBL
                        .GetListadoADM_PozoHistoricoPorCodDetalleSalaProgresivoYFecha(d.CodDetalleSalaProgresivo, fechaActual);

                    if(actual != null && actual.Count > 0) {
                        var h = actual
                            .OrderByDescending(x => x.FechaRegistro)
                            .ThenByDescending(x => x.CodPozoHistorico)
                            .First();

                        registros.Add(new {
                            h.CodPozoHistorico,
                            d.CodDetalleSalaProgresivo,
                            d.NroPozo,
                            NombrePozo = d.NombrePozo ?? $"Pozo {d.NroPozo}",
                            h.MontoActualAutomatico,
                            h.MontoActualSala,
                            h.MontoOcultoActualAutomatico,
                            h.MontoOcultoActualSala,
                            h.Estado,
                            h.Activo,
                            FechaOperacion = h.FechaOperacion,
                            FechaRegistro = (h.FechaRegistro == default(DateTime)) ? (DateTime?)null : h.FechaRegistro,
                            FechaModificacion = (h.FechaModificacion == default(DateTime)) ? (DateTime?)null : h.FechaModificacion
                        });
                    }
                }

                registros = registros.OrderBy(r => ((dynamic)r).NroPozo).ToList();

                var header = new {
                    codSala,
                    codMaquina,
                    CodSalaProgresivo = prog.CodSalaProgresivo,
                    NombreProgresivo = prog.Nombre ?? $"Prog #{prog.CodSalaProgresivo}",
                    //TotalRegistros = registros.Count,
                    //Fecha = fechaActual.Date
                };

                return Json(new { ok = true, value = header, registros }, JsonRequestBehavior.AllowGet);
            } catch(Exception ex) {
                return Json(new { ok = false, msg = "No se pudo obtener los registros.", detail = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("ProgresivoTerceros/LeerPozosDesdeImagenMaquina")]
        public async Task<JsonResult> LeerPozosDesdeImagenMaquina(string image) {
            try {
                string urlApiOcr = Convert.ToString(ValidationsHelper.GetValueAppSettingDB("uriOcrProgresivoTercero", "http://127.0.0.1:5001"));

                using(HttpClient http = new HttpClient()) {
                    http.BaseAddress = new Uri(urlApiOcr);
                    string path = "/leerprogresivo";
                    var payload = JsonConvert.SerializeObject(new { image });
                    var content = new StringContent(payload, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await http.PostAsync(path, content);

                    if(response.IsSuccessStatusCode) {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var resultObject = JsonConvert.DeserializeObject<ApiOcrLeerImagenPozoResponse>(responseBody);
                        return Json(resultObject, JsonRequestBehavior.AllowGet);
                    } else {
                        return Json(new { message = "Error al leer los pozos desde la imagen." }, JsonRequestBehavior.AllowGet);
                    }
                }
            } catch(Exception ex) {
                return Json(new { message = "Error al leer los pozos desde la imagen." }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
