using CapaEntidad.Progresivo;
using CapaEntidad;
using CapaNegocio.Progresivo;
using CapaNegocio;
using CapaPresentacion.Filters;
using CapaPresentacion.Utilitarios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CapaEntidad.Alertas;
using System.Linq;

namespace CapaPresentacion.Controllers.Progresivo
{
    [seguridad]
    [TokenProgresivo]
    public class AlertaProgresivoController : Controller
    {
        private readonly SalaBL _salaBL = new SalaBL();
        private readonly SEG_CargoBL _cargoBL = new SEG_CargoBL();
        private readonly AlertaProgresivoBL _alertaProgresivoBL = new AlertaProgresivoBL();
        private readonly ProgresivoBL _progresivoBL = new ProgresivoBL();
        private readonly AlertaProgresivoCargoBL _alertaProgresivoCargoBL = new AlertaProgresivoCargoBL();
        private readonly Correo _correo = new Correo();
        private readonly DestinatarioBL _destinatarioBL = new DestinatarioBL();
        private readonly NotificationHelper _notificationHelper = new NotificationHelper();
        private readonly RazorViewHelper _razorViewHelper = new RazorViewHelper();
        private readonly ObjectsHelper _objectsHelper = new ObjectsHelper();

        #region Reporte Alerta Progresivo

        [TokenProgresivo(false)]
        [HttpGet]
        public ActionResult ReporteAlertasProgresivo()
        {
            return View("~/Views/AlertaProgresivo/ReporteAlertasProgresivo.cshtml");
        }

        [TokenProgresivo(false)]
        [HttpPost]
        public ActionResult ListarAlertasProgresivo(int salaId, string fechaInicio, string fechaFin)
        {
            int status = 2;
            string message = "No se encontraron registros";
            int userId = Convert.ToInt32(Session["UsuarioID"]);

            if (userId == 0)
            {
                return Json(new
                {
                    status = 2,
                    message = "Por favor, ingrese credenciales"
                });
            }

            if (salaId == 0)
            {
                return Json(new
                {
                    status = 2,
                    message = "Por favor, seleccione una sala"
                });
            }

            if (string.IsNullOrEmpty(fechaInicio))
            {
                return Json(new
                {
                    status = 2,
                    message = "Por favor, ingrese una fecha inicio"
                });
            }

            if (string.IsNullOrEmpty(fechaFin))
            {
                return Json(new
                {
                    status = 2,
                    message = "Por favor, ingrese una fecha fin"
                });
            }

            List<AlertaProgresivoEntidad> listaAlertasProgresivo = new List<AlertaProgresivoEntidad>();

            DateTime fromDate = Convert.ToDateTime(fechaInicio);
            DateTime toDate = Convert.ToDateTime(fechaFin);

            try
            {
                listaAlertasProgresivo = _alertaProgresivoBL.ListarAlertasProgresivoSala(salaId, fromDate, toDate);

                if (listaAlertasProgresivo.Count > 0)
                {
                    status = 1;
                    message = "Datos obtenidos correctamente";
                }
            }
            catch (Exception exception)
            {
                status = 0;
                message = exception.Message;
            }

            var resultData = new
            {
                status,
                message,
                data = listaAlertasProgresivo
            };

            JavaScriptSerializer serializer = new JavaScriptSerializer
            {
                MaxJsonLength = int.MaxValue
            };

            ContentResult result = new ContentResult
            {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };

            return result;
        }

        [TokenProgresivo(false)]
        [HttpPost]
        public ActionResult ExcelAlertasProgresivo(int salaId, string fechaInicio, string fechaFin)
        {
            int status = 2;
            string message = "No se encontraron registros";
            int userId = Convert.ToInt32(Session["UsuarioID"]);

            if (userId == 0)
            {
                return Json(new
                {
                    status = 2,
                    message = "Por favor, ingrese credenciales"
                });
            }

            if (salaId == 0)
            {
                return Json(new
                {
                    status = 2,
                    message = "Por favor, seleccione una sala"
                });
            }

            if (string.IsNullOrEmpty(fechaInicio))
            {
                return Json(new
                {
                    status = 2,
                    message = "Por favor, ingrese una fecha inicio"
                });
            }

            if (string.IsNullOrEmpty(fechaFin))
            {
                return Json(new
                {
                    status = 2,
                    message = "Por favor, ingrese una fecha fin"
                });
            }

            SalaEntidad sala = _salaBL.ObtenerSalaPorCodigo(salaId);

            if (sala.CodSala == 0)
            {
                return Json(new
                {
                    status = 2,
                    message = "No se encontro datos, por favor seleccione una sala"
                });
            }

            // data
            string data = string.Empty;
            string fileName = string.Empty;
            string fileExtension = "xlsx";
            DateTime currentDate = DateTime.Now;
            DateTime fromDate = Convert.ToDateTime(fechaInicio);
            DateTime toDate = Convert.ToDateTime(fechaFin);

            try
            {
                List<AlertaProgresivoEntidad> listaAlertasProgresivo = _alertaProgresivoBL.ListarAlertasProgresivoSalaDetalles(salaId, fromDate, toDate);

                if (listaAlertasProgresivo.Count > 0)
                {
                    // Data Excel
                    dynamic arguments = new
                    {
                        roomCode = sala.CodSala,
                        roomName = sala.Nombre,
                        fromDate = fechaInicio,
                        toDate = fechaFin
                    };

                    MemoryStream memoryStream = ReportesHelper.ExcelAlertasProgresivo(listaAlertasProgresivo, arguments);

                    fileName = $"Reporte Alertas Registro Progresivo - {sala.Nombre} {fromDate.ToString("dd-MM-yyyy")} al {toDate.ToString("dd-MM-yyyy")} {currentDate.ToString("HHmmss")}.{fileExtension}";

                    status = 1;
                    message = "Excel generado";
                    data = Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            catch (Exception exception)
            {
                status = 0;
                message = exception.Message;
            }

            return Json(new
            {
                status,
                message,
                fileName,
                data
            });
        }

        [TokenProgresivo(false)]
        [HttpPost]
        public ActionResult ObtenerAlertaProgresivo(long alertaId)
        {
            bool success = false;
            string message = "No hay datos del registro seleccionado";

            AlertaProgresivoEntidad alertaProgresivo = new AlertaProgresivoEntidad();
            List<ResultadoEquals> diferentes = new List<ResultadoEquals>();

            try
            {
                alertaProgresivo = _alertaProgresivoBL.ObtenerAlertaProgresivo(alertaId);

                if (alertaProgresivo.Id > 0)
                {
                    AlertaProgresivoDetalleEntidad detalleActual = alertaProgresivo.Detalle.Where(x => x.ProActual == true).FirstOrDefault();
                    AlertaProgresivoDetalleEntidad detalleAnterior = alertaProgresivo.Detalle.Where(x => x.ProActual == false).FirstOrDefault();

                    List<string> noCompareProperties = new List<string>
                    {
                        "Id",
                        "FechaRegistro",
                        "AlertaId",
                        "DetalleId",
                        "ProActual",
                        "Anterior",
                        "ActualOculto",
                        "AnteriorOculto",
                        "Fecha"
                    };

                    List<ResultadoEquals> resultadoEquals = _objectsHelper.CompareObjects(detalleActual, detalleAnterior, noCompareProperties);
                    diferentes = resultadoEquals.Where(x => !x.Estado && !x.Campo.Equals("Pozos")).ToList();

                    success = true;
                    message = "Datos obtenidos correctamente";
                }
            }
            catch (Exception expception)
            {
                message = expception.Message + ", Llame Administrador";
            }

            return Json(new
            {
                success,
                message,
                data = alertaProgresivo,
                diferentes
            });
        }

        #endregion

        #region Configuracion Alerta Progresivo Cargo

        [TokenProgresivo(false)]
        [HttpGet]
        public ActionResult ConfiguracionAlertaProgresivoCargo()
        {
            return View("~/Views/AlertaProgresivo/ConfiguracionAlertaProgresivoCargo.cshtml");
        }

        [seguridad(false)]
        [TokenProgresivo(false)]
        [HttpPost]
        public ActionResult ListarAlertaProgresivoCargoSala()
        {
            bool response = false;
            string message = "Algo salio mal";

            List<AlertaProgresivoCargoEntidad> lista = new List<AlertaProgresivoCargoEntidad>();
            List<SEG_CargoEntidad> listaCargo = new List<SEG_CargoEntidad>();
            List<SalaEntidad> listaSala = new List<SalaEntidad>();
            List<object> listaFinal = new List<object>();

            try
            {
                lista = _alertaProgresivoCargoBL.ListarAlertaProgresivoCargo();
                listaCargo = _cargoBL.CargoListarJson();
                listaSala = _salaBL.ListadoSala();

                foreach (SalaEntidad sala in listaSala)
                {
                    List<object> cargos = new List<object>();

                    foreach (SEG_CargoEntidad cargo in listaCargo)
                    {
                        AlertaProgresivoCargoEntidad alertaCargo = lista.Where(x => x.CargoId == cargo.CargoID && x.SalaId == sala.CodSala).FirstOrDefault();

                        if (alertaCargo == null)
                        {
                            cargo.alt_id = 0;
                        }
                        else
                        {
                            cargo.alt_id = alertaCargo.Id;
                        }

                        cargos.Add(new
                        {
                            cargo.CargoID,
                            cargo.Descripcion,
                            cargo.Estado,
                            cargo.alt_id
                        });
                    }

                    listaFinal.Add(new
                    {
                        sala.CodSala,
                        sala.Nombre,
                        sala.NombreCorto,
                        cargos
                    });
                }

                response = true;
                message = "Datos obtenidos";
            }
            catch (Exception exception)
            {
                message = exception.Message + ", Llame Administrador";
            }
            return Json(new
            {
                respuesta = response,
                mensaje = message,
                data = listaFinal.ToList()
            });
        }

        [TokenProgresivo(false)]
        [HttpPost]
        public ActionResult GuardarAlertaProgresivoCargo(AlertaProgresivoCargoEntidad alertaCargo)
        {
            bool response = false;
            string message = "Error al Guardar Registro, LLame al Administrador";
            int insertedId = 0;

            try
            {
                alertaCargo.FechaRegistro = DateTime.Now;

                insertedId = _alertaProgresivoCargoBL.GuardarAlertaProgresivoCargo(alertaCargo);

                if (insertedId > 0)
                {
                    response = true;
                    message = "Registro Guardado Correctamente";
                }
            }
            catch (Exception expception)
            {
                message = expception.Message + ", Llame Administrador";
            }

            return Json(new
            {
                respuesta = response,
                mensaje = message,
                id = insertedId
            });
        }

        [TokenProgresivo(false)]
        [HttpPost]
        public ActionResult QuitarAlertaProgresivoCargo(int alerta_id)
        {
            bool response = false;
            var message = "Error al Quitar Registro, LLame al Administrador";

            try
            {
                bool isDeleted = _alertaProgresivoCargoBL.EliminarAlertaProgresivoCargo(alerta_id);

                if (isDeleted)
                {
                    response = true;
                    message = "Se quitó el Cargo correctamente";
                }
            }
            catch (Exception exception)
            {
                message = exception.Message + ", Llame Administrador";
            }

            return Json(new
            {
                respuesta = response,
                mensaje = message
            });
        }

        #endregion

        #region Progresivo Misterioso

        [seguridad(false)]
        [TokenProgresivo(false)]
        [HttpPost]
        public ActionResult EnviarMisteriosoAlertaProgresivo(AlertaProgresivoEntidad alertaProgresivo)
        {
            var success = false;
            var message = "Los datos no se pudieron registrar en IAS";

            if (alertaProgresivo.SalaId == 0)
            {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione una sala"
                });
            }

            try
            {
                // changes
                AlertaProgresivoDetalleEntidad detalleActual = alertaProgresivo.Detalle.Where(x => x.ProActual == true).FirstOrDefault();
                AlertaProgresivoDetalleEntidad detalleAnterior = alertaProgresivo.Detalle.Where(x => x.ProActual == false).FirstOrDefault();

                List<string> noCompareProperties = new List<string>
                {
                    "Id",
                    "FechaRegistro",
                    "AlertaId",
                    "DetalleId",
                    "ProActual",
                    "Anterior",
                    "ActualOculto",
                    "AnteriorOculto",
                    "Fecha"
                };

                List<ResultadoEquals> resultadoEquals = _objectsHelper.CompareObjects(detalleActual, detalleAnterior, noCompareProperties);
                List<ResultadoEquals> diferentes = resultadoEquals.Where(x => !x.Estado && !x.Campo.Equals("Pozos")).ToList();

                bool forNotification = diferentes.Count > 0;
                // end changes

                if (forNotification)
                {
                    alertaProgresivo.Tipo = 1;

                    bool result = _alertaProgresivoBL.GuardarAlertaProgresivoDetallePozo(alertaProgresivo);

                    // check notification
                    bool checkNotification = Convert.ToBoolean(ValidationsHelper.GetValueAppSettingDB("ARP_SWO", false));

                    if (result && checkNotification)
                    {
                        // notificaciones
                        int tipoDestinatario = 4;
                        int salaId = alertaProgresivo.SalaId;

                        List<ALT_AlertaDeviceEntidad> devices = _alertaProgresivoCargoBL.ListarAlertaDeviceSala(salaId);
                        List<string> emails = _alertaProgresivoCargoBL.ListarAlertaCorreosSala(salaId);
                        List<DestinatarioEntidad> listaDestinatarios = _destinatarioBL.ListarDestinatariosAsinadosTipo(tipoDestinatario);

                        // add destinatarios
                        emails.AddRange(listaDestinatarios.Select(x => x.Email).ToList());

                        // distinct
                        devices = devices.Distinct().ToList();
                        emails = emails.Distinct().ToList();

                        // valid emails
                        emails = ValidationsHelper.ValidEmails(emails);

                        // for devices
                        if (devices.Count > 0)
                        {
                            string[] _devices = devices.Select(x => x.id).ToArray();
                            string title = $"{alertaProgresivo.SalaNombre} Alerta Registro Progresivo {alertaProgresivo.ProgresivoNombre}";
                            string body = alertaProgresivo.Descripcion;

                            _notificationHelper.SendToFirebase(_devices, title, body);
                        }

                        // for mails
                        if (emails.Count > 0)
                        {
                            ViewData["diferentes"] = diferentes;

                            string _emails = string.Join(",", emails);
                            string title = $"{alertaProgresivo.SalaNombre} Alerta Registro Progresivo {alertaProgresivo.ProgresivoNombre}";
                            string bodyHtml = _razorViewHelper.RenderViewToString(ControllerContext, "~/views/AlertaProgresivo/ReporteAlertaEmail.cshtml", alertaProgresivo);

                            _correo.EnviarCorreo(_emails, title, bodyHtml, true);
                        }
                        // notificaciones

                        success = true;
                        message = "Los datos se registraron en IAS";
                    }
                }
            }
            catch (Exception exception)
            {
                success = false;
                message = exception.Message.ToString();
            }

            return Json(new
            {
                success,
                message
            });
        }

        #endregion

        #region Job Registro Alerta Progresivo

        [seguridad(false)]
        [TokenProgresivo(false)]
        [HttpPost]
        public ActionResult GuardarAlertasProgresivo(List<AlertaProgresivoEntidad> alertasProgresivo) {
            var success = false;
            var message = "Los datos no se pudieron registrar en IAS";

            foreach(AlertaProgresivoEntidad alertaProgresivo in alertasProgresivo) {

                try {

                    long idInsertado = _alertaProgresivoBL.GuardarAlertaProgresivo(alertaProgresivo);

                    if(idInsertado > 0) {
                        success = true;
                        message = "Alertas progresivo registrada en IAS.";
                    }

                } catch(Exception exception) {
                    success = false;
                    message = exception.Message.ToString();
                    return Json(new
                    {
                        success,
                        message
                    });
                }
            }

            return Json(new
            {
                success,
                message
            });
        }

        [seguridad(false)]
        [TokenProgresivo(false)]
        [HttpPost]
        public ActionResult ObtenerFechaUltimoGanador(int codSala,int codProgresivo) {
            var success = false;
            var message = "No se pudo obtener fecha del ultimo ganador.";
            var data = DateTime.Now;

            List<CabeceraOfflineEntidad> listaGanadores = new List<CabeceraOfflineEntidad>();
            DateTime fechaIni = DateTime.Now.AddDays(-5);
            DateTime fechaFin = DateTime.Now;

            if(codSala < 0 || codProgresivo<0) {
                return Json(new
                {
                    success,
                    message = "Por favor envie el codigo sala y/o progresivo."
                });
            }

            try {

                listaGanadores = _progresivoBL.GetCabeceraOfflinexSalaxProgresivo(codSala, codProgresivo, fechaIni, fechaFin);

                if(listaGanadores.Count > 0) {

                    CabeceraOfflineEntidad ultimoGanador = listaGanadores.OrderByDescending(x => x.Fecha).FirstOrDefault();

                    if(ultimoGanador!=null) {
                        data = ultimoGanador.Fecha;
                        success = true;
                        message = "Fecha del ultimo ganador obtenida correctamente.";
                    }

                } else {

                    data = DateTime.Now.AddDays(-5);
                    success = true;
                    message = "No se encontraron ganadores en los ultimos 5 dias.";
                }

            } catch(Exception exception) {
                success = false;
                message = exception.Message.ToString();
            }

            return Json(new
            {
                success,
                message,
                data
            });
        }

        #endregion
    }
}
