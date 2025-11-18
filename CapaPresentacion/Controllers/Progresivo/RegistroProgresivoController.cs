using CapaEntidad.Alertas;
using CapaEntidad.Progresivo;
using CapaEntidad;
using CapaNegocio.Progresivo;
using CapaNegocio;
using CapaPresentacion.Filters;
using CapaPresentacion.Utilitarios;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Web.Mvc;
using System.Linq;
using CapaEntidad.Response;
using System.Threading.Tasks;

namespace CapaPresentacion.Controllers.Progresivo
{
    [seguridad]
    [TokenProgresivo]
    public class RegistroProgresivoController : Controller
    {
        private readonly SalaBL _salaBL = new SalaBL();
        private readonly AlertaProgresivoBL _alertaProgresivoBL = new AlertaProgresivoBL();
        private readonly RegistroProgresivoBL _registroProgresivoBL = new RegistroProgresivoBL();
        private readonly AlertaProgresivoCargoBL _alertaProgresivoCargoBL = new AlertaProgresivoCargoBL();
        private readonly DestinatarioBL _destinatarioBL = new DestinatarioBL();
        private readonly Correo _correo = new Correo();
        private readonly NotificationHelper _notificationHelper = new NotificationHelper();
        private readonly RazorViewHelper _razorViewHelper = new RazorViewHelper();
        private readonly ObjectsHelper _objectsHelper = new ObjectsHelper();
        private readonly ServiceHelper _serviceHelper = new ServiceHelper();

        #region Historial Registro Progresivo

        [seguridad(false)]
        [TokenProgresivo(false)]
        [HttpPost]
        public async Task<ActionResult> GuardarRegistroProgresivo(RegistroProgresivoEntidad registroProgresivo, AlertaProgresivoEntidad alertaProgresivo)
        {
            var success = false;
            bool inVpn = false;
            var message = "Los datos no se pudieron registrar en IAS";
            int userId = Convert.ToInt32(Session["UsuarioID"]);
            string userName = Convert.ToString(Session["UsuarioNombre"]);

            try
            {
                // historial
                registroProgresivo.UsuarioId = userId;

                bool resultHRP = _registroProgresivoBL.HRPGuardarDetallePozo(registroProgresivo);

                if (resultHRP)
                {
                    // enviar template a NWO
                    SalaEntidad sala = _salaBL.ObtenerSalaPorCodigo(registroProgresivo.SalaId);
                    CheckPortHelper checkPortHelper = new CheckPortHelper();
                    CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, new List<string>());

                    string servicePath = "servicio/GuardarTemplateRegistroProgresivo";
                    string content = string.Empty;
                    string requestUri = string.Empty;

                    if (tcpConnection.IsVpn)
                    {
                        inVpn = true;

                        registroProgresivo.ipPrivada = $"{sala.IpPrivada}:{sala.PuertoServicioWebOnline}/{servicePath}";

                        content = JsonConvert.SerializeObject(registroProgresivo);
                        requestUri = $"{tcpConnection.Url}/servicio/VPNGenericoPost";
                    }
                    else
                    {
                        content = JsonConvert.SerializeObject(registroProgresivo);
                        requestUri = $"{sala.UrlProgresivo}/{servicePath}";
                    }

                    await _serviceHelper.PostAsync(requestUri, content);
                    // enviar template a NWO

                    success = true;
                    message = "Los cambios de la configuración del progresivo se registraron en IAS";
                }
                // end historial

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
                    alertaProgresivo.Descripcion = $"Usuario {userName} realizó cambios en la configuración desde IAS en {alertaProgresivo.SalaNombre}";
                    alertaProgresivo.Tipo = 2;

                    bool result = _alertaProgresivoBL.GuardarAlertaProgresivoDetallePozo(alertaProgresivo);

                    // check notification
                    bool checkNotification = Convert.ToBoolean(ValidationsHelper.GetValueAppSettingDB("ARP_IAS", false));

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
                            string title = $"{alertaProgresivo.SalaNombre} Cambios en Progresivo {alertaProgresivo.ProgresivoNombre}";
                            string body = alertaProgresivo.Descripcion;

                            _notificationHelper.SendToFirebase(_devices, title, body);
                        }

                        // for mails
                        if (emails.Count > 0)
                        {
                            ViewData["diferentes"] = diferentes;

                            string _emails = string.Join(",", emails);
                            string title = $"{alertaProgresivo.SalaNombre} Cambios en Progresivo {alertaProgresivo.ProgresivoNombre}";
                            string bodyHtml = _razorViewHelper.RenderViewToString(ControllerContext, "~/views/AlertaProgresivo/ReporteAlertaEmail.cshtml", alertaProgresivo);

                            _correo.EnviarCorreo(_emails, title, bodyHtml, true);
                        }
                        // notificaciones

                        success = true;
                        message = "Los cambios de la configuración del progresivo y los datos de la alerta se registraron en IAS";
                    }
                }
            }
            catch (Exception exception)
            {
                success = false;
                message = exception.Message;
            }

            return Json(new
            {
                success,
                message,
                inVpn
            });
        }

        [seguridad(false)]
        [TokenProgresivo(false)]
        [HttpPost]
        public ActionResult ObtenerUltimoRegistroProgresivo(int salaId, int progresivoId)
        {
            bool success = false;
            string message = "No se encontraron registros";

            if (salaId <= 0)
            {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione una sala"
                });
            }

            if (progresivoId <= 0)
            {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione un progresivo"
                });
            }

            RegistroProgresivoEntidad data = new RegistroProgresivoEntidad();

            try
            {
                RegistroProgresivoEntidad registroProgresivo = _registroProgresivoBL.HRPObtenerUltimoDetalle(salaId, progresivoId);

                if (registroProgresivo.Id > 0)
                {
                    data = registroProgresivo;

                    success = true;
                    message = "Registro obtenido correctamente";
                }
            }
            catch (Exception exception)
            {
                success = false;
                message = exception.Message;
            }

            return Json(new
            {
                success,
                message,
                data
            });
        }

        [seguridad(false)]
        [TokenProgresivo(false)]
        [HttpPost]
        public ActionResult ListarRegistrosProgresivos(int salaId, int progresivoId)
        {
            bool success = false;
            string message = "No se encontraron registros";

            if (salaId <= 0)
            {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione una sala"
                });
            }

            if (progresivoId <= 0)
            {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione un progresivo"
                });
            }

            List<RegistroProgresivoEntidad> data = new List<RegistroProgresivoEntidad>();

            try
            {
                int rows = Convert.ToInt32(ValidationsHelper.GetValueAppSettingDB("HRP_ListarDetalleTOP_IAS", 25));

                List<RegistroProgresivoEntidad> listRegistroProgresivo = _registroProgresivoBL.HRPListarDetalle(salaId, progresivoId, rows);

                if (listRegistroProgresivo.Count > 0)
                {
                    data = listRegistroProgresivo;

                    success = true;
                    message = "Datos obtenidos correctamente";
                }
            }
            catch (Exception exception)
            {
                success = false;
                message = exception.Message;
            }

            JsonResult jsonResult = Json(new
            {
                success,
                message,
                data
            });

            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        [seguridad(false)]
        [TokenProgresivo(false)]
        [HttpPost]
        public ActionResult ObtenerRegistroProgresivo(int salaId, long detalleId)
        {
            bool success = false;
            string message = "No se encontró el registro";

            if (salaId <= 0)
            {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione una sala"
                });
            }

            if (detalleId <= 0)
            {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione un registro"
                });
            }

            RegistroProgresivoEntidad data = new RegistroProgresivoEntidad();

            try
            {
                RegistroProgresivoEntidad registroProgresivo = _registroProgresivoBL.HRPObtenerDetalle(salaId, detalleId);

                if (registroProgresivo.Id > 0)
                {
                    data = registroProgresivo;

                    success = true;
                    message = "Registro obtenido correctamente";
                }
            }
            catch (Exception exception)
            {
                success = false;
                message = exception.Message;
            }

            return Json(new
            {
                success,
                message,
                data
            });
        }

        #endregion

        #region Historial Registro Online

        [seguridad(false)]
        [TokenProgresivo(false)]
        [HttpPost]
        public async Task<ActionResult> ObtenerUltimoRegistroOnline(int salaId, int progresivoId)
        {
            bool success = false;
            bool inVpn = false;
            string message = "No se encontraron registros";

            if (salaId <= 0)
            {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione una sala"
                });
            }

            if (progresivoId <= 0)
            {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione un progresivo"
                });
            }

            RegistroProgresivoEntidad data = new RegistroProgresivoEntidad();

            try
            {
                SalaEntidad sala = _salaBL.ObtenerSalaPorCodigo(salaId);
                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, new List<string>());

                if(!tcpConnection.IsOpen)
                {
                    return Json(new
                    {
                        success,
                        message = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                    });
                }

                string servicePath = "servicio/HRPObtenerUltimoProgresivo";
                string content = string.Empty;
                string requestUri = string.Empty;

                if (tcpConnection.IsVpn)
                {
                    inVpn = true;

                    object arguments = new
                    {
                        ipPrivada = $"{sala.IpPrivada}:{sala.PuertoServicioWebOnline}/{servicePath}",
                        salaId,
                        progresivoId
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{tcpConnection.Url}/servicio/VPNGenericoPost";
                }
                else
                {
                    object arguments = new
                    {
                        salaId,
                        progresivoId
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{sala.UrlProgresivo}/{servicePath}";
                }

                ResultEntidad result = await _serviceHelper.PostAsync(requestUri, content);

                RegistroProgresivoEntidad registroProgresivo = JsonConvert.DeserializeObject<RegistroProgresivoEntidad>(result.data) ?? new RegistroProgresivoEntidad();

                if (registroProgresivo.Id > 0)
                {
                    data = registroProgresivo;

                    success = true;
                    message = "Registro obtenido correctamente";
                }
            }
            catch (Exception exception)
            {
                success = false;
                message = exception.Message;
            }

            return Json(new
            {
                success,
                message,
                data,
                inVpn
            });
        }

        [seguridad(false)]
        [TokenProgresivo(false)]
        [HttpPost]
        public async Task<ActionResult> ListarRegistrosOnline(int salaId, int progresivoId)
        {
            bool success = false;
            bool inVpn = false;
            string message = "No se encontraron registros";

            if (salaId <= 0)
            {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione una sala"
                });
            }

            if (progresivoId <= 0)
            {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione un progresivo"
                });
            }

            List<RegistroProgresivoEntidad> data = new List<RegistroProgresivoEntidad>();

            try
            {
                SalaEntidad sala = _salaBL.ObtenerSalaPorCodigo(salaId);
                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, new List<string>());

                if (!tcpConnection.IsOpen)
                {
                    return Json(new
                    {
                        success,
                        message = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                    });
                }

                string servicePath = "servicio/HRPListarRegistrosProgresivos";
                string content = string.Empty;
                string requestUri = string.Empty;
                int rows = Convert.ToInt32(ValidationsHelper.GetValueAppSettingDB("HRP_ListarDetalleTOP_SWO", 25));

                if (tcpConnection.IsVpn)
                {
                    inVpn = true;

                    object arguments = new
                    {
                        ipPrivada = $"{sala.IpPrivada}:{sala.PuertoServicioWebOnline}/{servicePath}",
                        salaId,
                        progresivoId,
                        rows
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{tcpConnection.Url}/servicio/VPNGenericoPost";
                }
                else
                {
                    object arguments = new
                    {
                        salaId,
                        progresivoId,
                        rows
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{sala.UrlProgresivo}/{servicePath}";
                }

                ResultEntidad result = await _serviceHelper.PostAsync(requestUri, content);

                List<RegistroProgresivoEntidad> listRegistroProgresivo = JsonConvert.DeserializeObject<List<RegistroProgresivoEntidad>>(result.data) ?? new List<RegistroProgresivoEntidad>();

                if (listRegistroProgresivo.Count > 0)
                {
                    data = listRegistroProgresivo;

                    success = true;
                    message = "Datos obtenidos correctamente";
                }
            }
            catch (Exception exception)
            {
                success = false;
                message = exception.Message;
            }

            JsonResult jsonResult = Json(new
            {
                success,
                message,
                data,
                inVpn
            });

            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        [seguridad(false)]
        [TokenProgresivo(false)]
        [HttpPost]
        public async Task<ActionResult> ObtenerRegistroOnline(int salaId, long detalleId)
        {
            bool success = false;
            bool inVpn = false;
            string message = "No se encontró el registro";

            if (salaId <= 0)
            {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione una sala"
                });
            }

            if (detalleId <= 0)
            {
                return Json(new
                {
                    success,
                    message = "Por favor, seleccione un registro"
                });
            }

            RegistroProgresivoEntidad data = new RegistroProgresivoEntidad();

            try
            {
                SalaEntidad sala = _salaBL.ObtenerSalaPorCodigo(salaId);
                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, new List<string>());

                if (!tcpConnection.IsOpen)
                {
                    return Json(new
                    {
                        success,
                        message = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                    });
                }

                string servicePath = "servicio/HRPObtenerRegistroProgresivo";
                string content = string.Empty;
                string requestUri = string.Empty;

                if (tcpConnection.IsVpn)
                {
                    inVpn = true;

                    object arguments = new
                    {
                        ipPrivada = $"{sala.IpPrivada}:{sala.PuertoServicioWebOnline}/{servicePath}",
                        salaId,
                        detalleId
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{tcpConnection.Url}/servicio/VPNGenericoPost";
                }
                else
                {
                    object arguments = new
                    {
                        salaId,
                        detalleId
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{sala.UrlProgresivo}/{servicePath}";
                }

                ResultEntidad result = await _serviceHelper.PostAsync(requestUri, content);

                RegistroProgresivoEntidad registroProgresivo = JsonConvert.DeserializeObject<RegistroProgresivoEntidad>(result.data) ?? new RegistroProgresivoEntidad();

                if (registroProgresivo.Id > 0)
                {
                    data = registroProgresivo;

                    success = true;
                    message = "Registro obtenido correctamente";
                }
            }
            catch (Exception exception)
            {
                success = false;
                message = exception.Message;
            }

            return Json(new
            {
                success,
                message,
                data,
                inVpn
            });
        }

        #endregion
    }
}