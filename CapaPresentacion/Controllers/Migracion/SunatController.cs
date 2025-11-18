using CapaEntidad.Response;
using CapaEntidad;
using CapaEntidad.Sunat;
using CapaNegocio;
using CapaNegocio.Sunat;
using CapaPresentacion.Utilitarios;
using Newtonsoft.Json;
using S3k.Utilitario.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Linq;

namespace CapaPresentacion.Controllers.Migracion {
    [seguridad]
    public class SunatController : Controller {
        private readonly SunatBL sunatBL;
        private readonly string DATE_FORMAT;
        private SalaBL _salaBl = new SalaBL();
        private readonly ServiceHelper _serviceHelper = new ServiceHelper();

        public SunatController() {
            sunatBL = new SunatBL();
            DATE_FORMAT = "dd/MM/yyyy HH:mm:ss";
        }

        #region Views
        public ActionResult Eventos() {
            return View();
        }
        public ActionResult Contadores() {
            return View();
        }
        #endregion

        #region Methods
        [HttpPost]
        public async Task<JsonResult> ObtenerEventosSunat(int codSala, DateTime fechaIni, DateTime fechaFin) {
            bool success = false;
            int diferenciaDias = (fechaFin - fechaIni).Days;
            List<EventosSunatEntidad> eventosSunat = new List<EventosSunatEntidad>();
            string displayMessage;
            if(diferenciaDias > 5) {
                return Json(new {
                    success,
                    data = eventosSunat,
                    displayMessage = "El rango máximo de consulta es de 5 días"
                });
            }
            try {
                await SincronizarEventosSunat(codSala, fechaIni, fechaFin);
                eventosSunat = sunatBL.ObtenerEventosSunatxSala(codSala, fechaIni, fechaFin);
                success = eventosSunat.Count > 0;
                displayMessage = success ? $"Lista de eventos mincetur del {fechaIni.ToShortDateString()} al {fechaFin.ToShortDateString()}." : "No se encontraron registros para los eventos mincetur";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador.";
            }
            JsonResult jsonResult = Json(new { success, data = eventosSunat, displayMessage });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public async Task<JsonResult> ObtenerContadoresSunat(int codSala, DateTime fechaIni, DateTime fechaFin) {
            int diferenciaDias = (fechaFin - fechaIni).Days;
            bool success = false;
            List<ContadoresSunatEntidad> contadoresSunat = new List<ContadoresSunatEntidad>();
            string displayMessage;

            // Check the date range limit
            if(diferenciaDias > 5) {
                return Json(new {
                    success,
                    data = contadoresSunat,
                    displayMessage = "El rango máximo de consulta es de 5 días"
                });
            }

            try {
                // Await the synchronization process to ensure it finishes before continuing
                await SincronizarContadoresSunat(codSala, fechaIni, fechaFin);

                // Fetch the updated data after synchronization
                contadoresSunat = sunatBL.ObtenerContadoresSunatxFecha(codSala, fechaIni, fechaFin);
                success = contadoresSunat.Count > 0;
                displayMessage = success
                    ? $"Lista de contadores mincetur del {fechaIni.ToShortDateString()} al {fechaFin.ToShortDateString()}."
                    : "No se encontraron registros para los contadores mincetur";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador.";
            }

            JsonResult jsonResult = Json(new {
                success,
                data = contadoresSunat,
                displayMessage
            });

            // Set the maximum JSON length if needed
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        [seguridad(false)]
        [HttpPost]
        public async Task<ActionResult> SincronizarEventosSunat(int salaId, DateTime fechaIni, DateTime fechaFin) {
            bool success = false;
            bool inVpn = false;
            string message = "Eventos Sunat no sincronizados";

            if(salaId <= 0) {
                return Json(new {
                    success,
                    message = "Por favor, seleccione una sala"
                });
            }
            
            try {
                SalaEntidad sala = _salaBl.ObtenerSalaPorCodigo(salaId);
                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, new List<string>());

                if(!tcpConnection.IsOpen) {
                    return Json(new {
                        success,
                        message = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                    });
                }


                string servicePath = "servicio/ConsultarEventosSunat";
                string content = string.Empty;
                string requestUri = string.Empty;

                if(tcpConnection.IsVpn) {
                    inVpn = true;

                    object arguments = new {
                        ipPrivada = $"{sala.IpPrivada}:{sala.PuertoServicioWebOnline}/{servicePath}",
                        fechaIni = fechaIni.ToString(),
                        fechaFin = fechaFin.ToString()
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{tcpConnection.Url}/servicio/VPNGenericoPost";
                } else {
                    object arguments = new {
                        fechaIni = fechaIni.ToString(),
                        fechaFin = fechaFin.ToString()
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{sala.UrlProgresivo}/{servicePath}";
                }

                ResultEntidad result = await _serviceHelper.PostAsync(requestUri, content);
                List<EventosSunatEntidad> contadoresNuevosSala = JsonConvert.DeserializeObject<List<EventosSunatEntidad>>(result.data);
                List<EventosSunatEntidad> contadoresIas = sunatBL.ObtenerEventosSunatxSala(salaId, fechaIni, fechaFin);
                List<EventosSunatEntidad> nuevosEventosInsertar = new List<EventosSunatEntidad>();
                if(result.success) {
                    if(contadoresIas.Count > 0) {
                        foreach(var nuevoEvento in contadoresNuevosSala) {
                            EventosSunatEntidad eventoSunatEncontrado = contadoresIas.FirstOrDefault(x => x.IdEvSunat == nuevoEvento.IdEvSunat) ?? new EventosSunatEntidad();
                            if(eventoSunatEncontrado.CodSala != 0) {
                                if(eventoSunatEncontrado.Envio != nuevoEvento.Envio || eventoSunatEncontrado.FechaEnvio != nuevoEvento.FechaEnvio) {
                                    sunatBL.EditarEventoSunat(nuevoEvento, salaId);
                                }
                            } else {
                                nuevosEventosInsertar.Add(nuevoEvento);
                            }
                        }

                        if(nuevosEventosInsertar.Count > 0) {
                            sunatBL.GuardarEventosSunat(nuevosEventosInsertar);
                        }
                    } else {
                        sunatBL.GuardarEventosSunat(contadoresNuevosSala);
                    }
                }

                if(result.success) {
                    success = true;
                    message = result.message;
                }
            } catch(Exception exception) {
                message = exception.Message;
            }

            return Json(new {
                success,
                message,
                inVpn
            });
        }

        [seguridad(false)]
        [HttpPost]
        public async Task<ActionResult> SincronizarContadoresSunat(int salaId, DateTime fechaIni, DateTime fechaFin) {
            bool success = false;
            bool inVpn = false;
            string message = "Contadores no sincronizados";

            if(salaId <= 0) {
                return Json(new {
                    success,
                    message = "Por favor, seleccione una sala"
                });
            }

            try {
                SalaEntidad sala = _salaBl.ObtenerSalaPorCodigo(salaId);
                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, new List<string>());

                if(!tcpConnection.IsOpen) {
                    return Json(new {
                        success,
                        message = "El servidor remoto no se encuentra disponible, por favor inténtelo de nuevo"
                    });
                }

                //List<WEB_DestinatarioEntidad> webDestinatarios = alertaSalaBL.ObtenerDestinatariosOnline(salaId);

                string servicePath = "servicio/ConsultarContadoresSunat";
                string content = string.Empty;
                string requestUri = string.Empty;

                if(tcpConnection.IsVpn) {
                    inVpn = true;

                    object arguments = new {
                        ipPrivada = $"{sala.IpPrivada}:{sala.PuertoServicioWebOnline}/{servicePath}",
                        fechaIni = fechaIni.ToString(),
                        fechaFin = fechaFin.ToString()
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{tcpConnection.Url}/servicio/VPNGenericoPost";
                } else {
                    object arguments = new {
                        fechaIni = fechaIni.ToString(),
                        fechaFin = fechaFin.ToString()
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{sala.UrlProgresivo}/{servicePath}";
                }

                ResultEntidad result = await _serviceHelper.PostAsync(requestUri, content);

                List<ContadoresSunatEntidad> contadoresNuevosSala = JsonConvert.DeserializeObject<List<ContadoresSunatEntidad>>(result.data);
                List<ContadoresSunatEntidad> contadoresIas = sunatBL.ObtenerContadoresSunatxFecha(salaId, fechaIni, fechaFin);
                List<ContadoresSunatEntidad> nuevosEventosInsertar = new List<ContadoresSunatEntidad>();
                if(result.success) {
                    if(contadoresIas.Count > 0) {
                        foreach(var nuevoEvento in contadoresNuevosSala) {
                            ContadoresSunatEntidad eventoSunatEncontrado = contadoresIas.FirstOrDefault(x => x.IdConSunat == nuevoEvento.IdConSunat) ?? new ContadoresSunatEntidad();
                            if(eventoSunatEncontrado.CodSala != 0) {
                                if(eventoSunatEncontrado.Envio != nuevoEvento.Envio || eventoSunatEncontrado.FechaEnvio != nuevoEvento.FechaEnvio) {
                                    sunatBL.EditarContadorSunat(nuevoEvento, salaId);
                                }
                            } else {
                                nuevosEventosInsertar.Add(nuevoEvento);
                            }
                        }

                        if(nuevosEventosInsertar.Count > 0) {
                            sunatBL.GuardarContadoresSunat(nuevosEventosInsertar);
                        }
                    } else {
                        sunatBL.GuardarContadoresSunat(contadoresNuevosSala);
                    }
                }

                if(result.success) {
                    success = true;
                    message = result.message;
                }
            } catch(Exception exception) {
                message = exception.Message;
            }

            return Json(new {
                success,
                message,
                inVpn
            });
        }


        #endregion

        #region Methods Migration
        [HttpPost]
        [seguridad(false)]
        public JsonResult GuardarEventosSunat(List<EventosSunatEntidad> eventosSunat) {
            bool success = sunatBL.GuardarEventosSunat(eventosSunat);
            string displayMessage = success ? $"{eventosSunat.Count} eventos sunat migrados correctamente." : "Error al migrar los eventos sunat.";
            JsonResult jsonResult = Json(new { success, displayMessage });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        [seguridad(false)]
        public JsonResult GuardarContadoresSunat(List<ContadoresSunatEntidad> contadoresSunat) {
            bool success = sunatBL.GuardarContadoresSunat(contadoresSunat);
            string displayMessage = success ? $"{contadoresSunat.Count} contadores sunat migrados correctamente." : "Error al migrar los eventos sunat.";
            JsonResult jsonResult = Json(new { success, displayMessage });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        [seguridad(false)]
        public JsonResult ObtenerUltimoIdContadorSunatPorCodSala(int codSala) {
            int ultimoId = sunatBL.ObtenerUltimoIdContadorSunatPorCodSala(codSala);
            bool success = ultimoId > 0;
            string displayMessage = success ? $"Valor del ultimo ID contador sunat encontrado para la sala {codSala}: {ultimoId}." : $"Aún no hay registros de contadores sunat para la sala {codSala}.";
            JsonResult jsonResult = Json(new { success, data = ultimoId, displayMessage });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        [seguridad(false)]
        public JsonResult ObtenerUltimoIdEventoSunatPorCodSala(int codSala) {
            int ultimoId = sunatBL.ObtenerUltimoIdEventoSunatPorCodSala(codSala);
            bool success = ultimoId > 0;
            string displayMessage = success ? $"Valor del ultimo ID evento sunat encontrado para la sala {codSala}: {ultimoId}." : $"Aún no hay registros de eventos sunat para la sala {codSala}.";
            JsonResult jsonResult = Json(new { success, data = ultimoId, displayMessage });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region Excel
        [HttpPost]
        public ActionResult GenerarExcelEventosSunat(int codSala, DateTime fechaIni, DateTime fechaFin) {
            int diferenciaDias = (fechaFin - fechaIni).Days;
            string displayMessage;
            bool success;
            //cantidadDias = cantidadDias > 0 ? cantidadDias : 10;
            if(diferenciaDias > 5) {
                return Json(new {
                    success=false,
                    displayMessage = "El rango máximo de consulta es de 5 días"
                });
            }
            List<EventosSunatEntidad> eventosSunat = sunatBL.ObtenerEventosSunatxSala(codSala, fechaIni, fechaFin);
            success = eventosSunat.Count > 0;
            if(!success) {
                displayMessage = $"No se encontraron registros para los eventos sunat";
                return Json(new { success, displayMessage });
            }

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Cod. Sala", typeof(string));
            dataTable.Columns.Add("Sala", typeof(string));
            dataTable.Columns.Add("Fecha Migración", typeof(DateTime));

            dataTable.Columns.Add("Fecha", typeof(DateTime));
            dataTable.Columns.Add("Trama", typeof(string));
            dataTable.Columns.Add("Tipo Trama", typeof(int));
            dataTable.Columns.Add("Id Ev Sunat", typeof(int));
            dataTable.Columns.Add("Envio", typeof(int));
            dataTable.Columns.Add("Fecha Envío", typeof(DateTime));
            dataTable.Columns.Add("Fecha Proceso", typeof(DateTime));
            dataTable.Columns.Add("Motivo", typeof(string));
            dataTable.Columns.Add("Id Conf Sunat", typeof(int));
            dataTable.Columns.Add("Band Busq", typeof(int));

            dataTable.Columns.Add("Cabecera", typeof(string));
            dataTable.Columns.Add("DGJM", typeof(string));
            dataTable.Columns.Add("Cod Maq", typeof(string));
            dataTable.Columns.Add("Id Colector", typeof(string));
            dataTable.Columns.Add("Fecha Trama", typeof(string));
            dataTable.Columns.Add("Pccm", typeof(string));
            dataTable.Columns.Add("Pccsuctr", typeof(string));
            dataTable.Columns.Add("Rce", typeof(string));
            dataTable.Columns.Add("Embbram", typeof(string));
            dataTable.Columns.Add("Apl", typeof(string));
            dataTable.Columns.Add("Fmc", typeof(string));
            dataTable.Columns.Add("Frammr", typeof(string));
            dataTable.Columns.Add("Cereo Trama", typeof(string));
            dataTable.Columns.Add("Reserva 1", typeof(string));
            dataTable.Columns.Add("Reserva 2", typeof(string));

            foreach(var eventoSunat in eventosSunat) {
                dataTable.Rows.Add(
                    eventoSunat.CodSala,
                    eventoSunat.Sala,
                    eventoSunat.FechaMigracion.ToString(DATE_FORMAT),

                    eventoSunat.Fecha.ToString(DATE_FORMAT),
                    eventoSunat.Trama,
                    eventoSunat.TipoTrama ? 1 : 0,
                    eventoSunat.IdEvSunat,
                    eventoSunat.Envio,
                    eventoSunat.FechaEnvio.ToString(DATE_FORMAT),
                    eventoSunat.FechaProceso.ToString(DATE_FORMAT),
                    eventoSunat.Motivo,
                    eventoSunat.IdConfSunat,
                    eventoSunat.BandBusq,

                    eventoSunat.Cabecera,
                    eventoSunat.DGJM,
                    eventoSunat.CodMaq,
                    eventoSunat.IdColector,
                    eventoSunat.FechaTrama,
                    eventoSunat.Pccm,
                    eventoSunat.Pccsuctr,
                    eventoSunat.Rce,
                    eventoSunat.Embbram,
                    eventoSunat.Apl,
                    eventoSunat.Fmc,
                    eventoSunat.Frammr,
                    eventoSunat.CereoTrama,
                    eventoSunat.Reserva1,
                    eventoSunat.Reserva2
                );
            }

            try {
                ExportExcel exportExcel = new ExportExcel {
                    FileName = $"Eventos Mincetur",
                    SheetName = "Eventos Mincetur",
                    Data = dataTable,
                    Title = $"Eventos Mincetur",
                    FirstColumNumber = false,
                };

                var excelBytes = ExcelHelper.GenerateExcel(exportExcel);
                displayMessage = success ? "Archivo excel generado correctamente." : "Ocurrio un error al intentar generar el archiv excel.";

                exportExcel.Data = null;

                object obj = new {
                    success,
                    bytes = Convert.ToBase64String(excelBytes),
                    displayMessage,
                    fileInfo = exportExcel
                };

                var json = Json(obj);
                json.MaxJsonLength = int.MaxValue;
                return json;
            } catch(Exception exp) {
                success = false;
                displayMessage = exp.Message + ". Llame al Administrador.";
            }


            return Json(new { success, data = eventosSunat, displayMessage });
        }

        [HttpPost]
        public ActionResult GenerarExcelContadoresSunat(int codSala, DateTime fechaIni, DateTime fechaFin) {
            string displayMessage;
            bool success;
            int diferenciaDias = (fechaFin - fechaIni).Days;
            if(diferenciaDias > 5) {
                return Json(new {
                    success = false,
                    displayMessage = "El rango máximo de consulta es de 5 días"
                });
            }
                List<ContadoresSunatEntidad> contadoresSunat = sunatBL.ObtenerContadoresSunatxFecha(codSala, fechaIni,fechaFin);
            success = contadoresSunat.Count > 0;
            if(!success) {
                displayMessage = $"No se encontraron registros para los contadores mincetur";
                return Json(new { success, displayMessage });
            }

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Cod. Sala", typeof(string));
            dataTable.Columns.Add("Sala", typeof(string));
            dataTable.Columns.Add("Fecha Migración", typeof(DateTime));

            dataTable.Columns.Add("Fecha", typeof(DateTime));
            dataTable.Columns.Add("Trama", typeof(string));
            dataTable.Columns.Add("Cereo", typeof(int));
            dataTable.Columns.Add("Id Con Sunat", typeof(int));
            dataTable.Columns.Add("Envio", typeof(int));
            dataTable.Columns.Add("IdCereo", typeof(int));
            dataTable.Columns.Add("Fecha Envío", typeof(DateTime));
            dataTable.Columns.Add("Fecha Proceso", typeof(DateTime));
            dataTable.Columns.Add("Motivo", typeof(string));
            dataTable.Columns.Add("Id Conf Sunat", typeof(int));
            dataTable.Columns.Add("Band Busq", typeof(int));

            dataTable.Columns.Add("Cabecera", typeof(string));
            dataTable.Columns.Add("DGJM", typeof(string));
            dataTable.Columns.Add("Cod Maq", typeof(string));
            dataTable.Columns.Add("Fecha Trama", typeof(string));
            dataTable.Columns.Add("Reserva 1", typeof(string));
            dataTable.Columns.Add("Moneda", typeof(string));
            dataTable.Columns.Add("Denominacion", typeof(string));
            dataTable.Columns.Add("Coin In Final", typeof(string));
            dataTable.Columns.Add("Coin Out Final", typeof(string));
            dataTable.Columns.Add("Pago Manual Final", typeof(string));
            dataTable.Columns.Add("Otro Final", typeof(string));
            dataTable.Columns.Add("Tipo Cambio", typeof(string));

            foreach(var contadorSunat in contadoresSunat) {
                dataTable.Rows.Add(
                    contadorSunat.CodSala,
                    contadorSunat.Sala,
                    contadorSunat.FechaMigracion.ToString(DATE_FORMAT),

                    contadorSunat.Fecha.ToString(DATE_FORMAT),
                    contadorSunat.Trama,
                    contadorSunat.Cereo ? 1 : 0,
                    contadorSunat.IdConfSunat,
                    contadorSunat.Envio,
                    contadorSunat.IdCereo,
                    contadorSunat.FechaEnvio.ToString(DATE_FORMAT),
                    contadorSunat.FechaProceso.ToString(DATE_FORMAT),
                    contadorSunat.Motivo,
                    contadorSunat.IdConfSunat,
                    contadorSunat.BandBusq,

                    contadorSunat.Cabecera,
                    contadorSunat.DGJM,
                    contadorSunat.CodMaq,
                    contadorSunat.FechaTrama,
                    contadorSunat.Reserva1,
                    contadorSunat.Moneda,
                    contadorSunat.Denominacion,
                    contadorSunat.CoinInFinal,
                    contadorSunat.CoinOutFinal,
                    contadorSunat.PagoManualFinal,
                    contadorSunat.OtroFinal,
                    contadorSunat.TipoCambio
                );
            }

            try {
                ExportExcel exportExcel = new ExportExcel {
                    FileName = $"Contadores Mincetur",
                    SheetName = "Contadores Mincetur",
                    Data = dataTable,
                    Title = $"Contadores Mincetur",
                    FirstColumNumber = false,
                };

                var excelBytes = ExcelHelper.GenerateExcel(exportExcel);
                displayMessage = success ? "Archivo excel generado correctamente." : "Ocurrio un error al intentar generar el archiv excel.";

                exportExcel.Data = null;

                object obj = new {
                    success,
                    bytes = Convert.ToBase64String(excelBytes),
                    displayMessage,
                    fileInfo = exportExcel
                };

                var json = Json(obj);
                json.MaxJsonLength = int.MaxValue;
                return json;
            } catch(Exception exp) {
                success = false;
                displayMessage = exp.Message + ". Llame al Administrador.";
            }


            return Json(new { success, data = contadoresSunat, displayMessage });
        }
        #endregion
    }
}