using CapaEntidad;
using CapaEntidad.Sala;
using CapaNegocio;
using CapaNegocio.Sala;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Sala {
    [seguridad]
    public class ZonaController : Controller {
        private readonly SL_ZonaBL _zonaBL = new SL_ZonaBL();
        private readonly SalaBL _salaBL = new SalaBL();
        private SalaBL salaBl = new SalaBL();
        private int CodigoSalaSomosCasino = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoSalaSomosCasino"]);

        public ActionResult ZonaVista() {
            return View();
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ObtenerZona(int zonaId) {
            bool status = false;
            string message = string.Empty;

            SL_ZonaEntidad zona = new SL_ZonaEntidad();

            try {
                zona = _zonaBL.ObtenerZona(zonaId);

                status = true;
            } catch(Exception exception) {
                message = exception.Message;
            }

            return Json(new {
                status,
                message,
                data = zona
            });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarZona() {
            bool status = false;
            string message = string.Empty;
            int userId = Convert.ToInt32(Session["UsuarioID"]);

            List<SL_ZonaEntidad> listaZona = new List<SL_ZonaEntidad>();

            try {
                List<SalaEntidad> listaSalas = _salaBL.ListadoSalaPorUsuario(userId);
                List<int> salaIds = listaSalas.Select(item => item.CodSala).ToList();

                listaZona = _zonaBL.ListarZona(salaIds);

                status = true;
            } catch(Exception exception) {
                message = exception.Message;
            }

            return Json(new {
                status,
                message,
                data = listaZona
            });
        }

        [HttpPost]
        public ActionResult GuardarZona(SL_ZonaEntidad zona) {
            bool status = false;
            string message = "No se pudo guardar los datos";

            try {
                int insertedId = _zonaBL.GuardarZona(zona);

                if(insertedId > 0) {
                    status = true;
                    message = "Los datos se han guardado";
                }
            } catch(Exception exception) {
                message = exception.Message;
            }

            return Json(new {
                status,
                message
            });
        }

        [HttpPost]
        public ActionResult ActualizarZona(SL_ZonaEntidad zona) {
            bool status = false;
            string message = "No se pudo actualizar los datos";

            try {
                bool updated = _zonaBL.ActualizarZona(zona);

                if(updated) {
                    status = true;
                    message = "Los datos se han actualizado";
                }
            } catch(Exception exception) {
                message = exception.Message;
            }

            return Json(new {
                status,
                message
            });
        }

        [HttpPost]
        public ActionResult ActualizarEstadoZona(byte estado, int zonaId) {
            bool status = false;
            string message = "No se pudo actualizar el estado";

            try {
                bool updated = _zonaBL.ActualizarEstadoZona(estado, zonaId);

                if(updated) {
                    status = true;
                    message = "El estado se ha actualizado";
                }
            } catch(Exception exception) {
                message = exception.Message;
            }

            return Json(new {
                status,
                message
            });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarZonasPorSala(int salaId) {
            bool status = false;
            string message = string.Empty;

            List<SL_ZonaEntidad> listaZona = new List<SL_ZonaEntidad>();

            try {
                listaZona = _zonaBL.ListarZonasPorSala(salaId);

                status = true;
            } catch(Exception exception) {
                message = exception.Message;
            }

            return Json(new {
                status,
                message,
                data = listaZona
            });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarZonasPorSalaMobil(int usuario_id) {
            bool respuesta = false;
            string mensaje = string.Empty;
            List<SalaEntidad> listaSalas = new List<SalaEntidad>();
            List<SL_ZonaEntidad> listaZona = new List<SL_ZonaEntidad>();
            SalaEntidad sala = new SalaEntidad();
            try {
                listaSalas = salaBl.ListadoSalaPorUsuario(usuario_id).Where(x => x.CodSala != CodigoSalaSomosCasino).ToList();
                if(listaSalas.Count == 1) {
                    sala = listaSalas.FirstOrDefault();
                    listaZona = _zonaBL.ListarZonasPorSala(sala.CodSala);
                }
                respuesta = true;
            } catch(Exception exception) {
                mensaje = exception.Message;
            }

            return Json(new {
                respuesta,
                mensaje,
                data = listaZona
            });
        }
    }
}
