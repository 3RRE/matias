using CapaDatos.ProgresivoRuleta;
using CapaEntidad;
using CapaEntidad.ProgresivoRuleta.Config;
using CapaEntidad.ProgresivoRuleta.Dto;
using CapaEntidad.ProgresivoRuleta.Entidades;
using CapaEntidad.Response;
using CapaNegocio;
using CapaNegocio.ProgresivoRuleta;
using CapaPresentacion.Utilitarios;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.ProgresivoRuleta {
    public class PruGanadoresController : Controller {
        private readonly PRU_GanadorBL ganadorBL;
        private readonly ServiceHelper _serviceHelper = new ServiceHelper();
        private SalaBL salaBl = new SalaBL();
        private readonly PRU_AlertaBL alertaBL;
        public PruGanadoresController() {
            ganadorBL = new PRU_GanadorBL();
            alertaBL = new PRU_AlertaBL();
        }

        #region Methods
        [seguridad(false)]
        [HttpPost]
        public JsonResult CrearGanador(PRU_Ganador ganador) {
            bool success = false;
            string displayMessage;

            try {
                success = ganadorBL.InsertarGanador(ganador);
                displayMessage = success ? "Ganador creado correctamente." : "No se pudo crear el ganador.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }



        [seguridad(false)]
        [HttpPost]
        public async Task<JsonResult> CambiarAcreditacionGanador(PRU_AcreditacionGanadorConfig acreditacion) {
            string displayMessage = "";
            bool success = false;

            try {

                PRU_GanadorDto ultimoGanador = ganadorBL.ObtenerUltimoGanadorPorFiltro(acreditacion);

                if(ultimoGanador == null || !ultimoGanador.Existe()) {
                    return Json(new { success = false, displayMessage = "No se encontró el ganador especificado" });
                }

                var response = ganadorBL.CambiarAcreditacionGanador(acreditacion);
                 displayMessage = response ? "Acreditación de ganador actualizada correctamente." : "No se pudo actualizar la acreditación del ganador.";
               
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }


            return Json(new { success, displayMessage });
        }



        [seguridad(false)]
        [HttpPost]
        public async Task<JsonResult> CambiarAcreditacionGanadorAutomatico(PRU_AcreditacionGanadorConfig acreditacion) {
            bool success = false;
            string displayMessage = "";
            int segundosRange = 5;

            try {
                var ultimoGanador = ganadorBL.ObtenerUltimoGanadorPorFiltro(acreditacion);

                if(ultimoGanador == null || !ultimoGanador.Existe()) {
                    return Json(new {
                        success = false,
                        displayMessage = "No se encontró el ganador especificado"
                    });
                }

                string codigoLimpio = ultimoGanador.CodMaquina.Split('-', '_').First();
                var countersResult = await GetMachineCountersDataAsync(codigoLimpio, ultimoGanador.FechaGanador, segundosRange, ultimoGanador.Sala.CodSala);

                List<ContadorMaquinaDto> contadores = new List<ContadorMaquinaDto>();

                List<ContadorMaquinaDto> coincidentes = new List<ContadorMaquinaDto>();
                
                if(countersResult.success) {
                    contadores = JsonConvert.DeserializeObject<List<ContadorMaquinaDto>>(countersResult.data);

                    for(int i = 1; i < contadores.Count; i++) {
                        var actual = contadores[i];
                        var anterior = contadores[i - 1];
                        decimal resultado = (actual.EftIn - anterior.EftIn) * actual.Token;

                        if(resultado == ultimoGanador.Monto) {
                            coincidentes.Add(actual);
                        }
                    }
                } else {
                    System.Diagnostics.Trace.WriteLine($"Error obteniendo contadores: {countersResult.message}");
                }


                if(contadores.Count > 0 && coincidentes.Count > 0) {
                    acreditacion.EsAcreditado = 2;
                    acreditacion.Detalle = $"Ganador Acreditado. Fecha de búsqueda: {ultimoGanador.FechaGanador}. " +
                                           $"Rango de segundos: {segundosRange}s. Contadores revisados: {contadores.Count}. " +
                                           $"Valores de coincidencias: {string.Join("; ", coincidentes.Select(c => $"[CodContOL: {c.CodContOL}, Fecha: {c.Fecha:yyyy-MM-dd HH:mm:ss}]"))}";
                    displayMessage = "El ganador fue actualizado a acreditado 2.";
                } else {
                    acreditacion.EsAcreditado = 0;
                    acreditacion.Detalle = $"No acreditado. Fecha de búsqueda: {ultimoGanador.FechaGanador}. " +
                                           $"Rango de segundos: {segundosRange}s. Contadores revisados: {contadores.Count}.";
                    displayMessage = "El ganador fue actualizado a no acreditado.";
                }

                success = ganadorBL.CambiarAcreditacionGanador(acreditacion);

                if(!success) displayMessage = "No se pudo actualizar la acreditación del ganador.";
            } catch(Exception ex) {
                success = false;
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new {
                success,
                displayMessage,
                esAcreditado = acreditacion.EsAcreditado
            });
        }

        #endregion



        private async Task<(bool success, string message, dynamic data)>  GetMachineCountersDataAsync(string CodMaquina,DateTime fechaGanador, int segundosRange, int salaId) {
            bool success = false;
            string message = "No se encontraron registros";
            dynamic data = null;
            bool inVpn = false;

            try {

                SalaVpnEntidad sala = salaBl.ObtenerSalaVpnPorCodigo(salaId);

                if(sala.CodSala == 0) {
                    return (false, "No se ha encontrado datos de la sala", null);
                }

                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlProgresivo, new List<string>());

                if(!tcpConnection.IsOpen) {
                    return (false, "El servidor remoto no se encuentra disponible", null);
                }

                DateTime fechaIni = fechaGanador.AddSeconds(-segundosRange);
                DateTime fechaFin = fechaGanador.AddSeconds(segundosRange);

                string fechaIniParam = Uri.EscapeDataString(fechaIni.ToString("yyyy-MM-dd HH:mm:ss"));
                string fechaFinParam = Uri.EscapeDataString(fechaFin.ToString("yyyy-MM-dd HH:mm:ss"));

                string servicePath = "Servicio/GetContadoresMaquina";
                string content = string.Empty;
                string requestUri = string.Empty;
                //tcpConnection.IsVpn = true;
                if(tcpConnection.IsVpn) {
                    inVpn = true;
                    string endPoint = $"{tcpConnection.Url}/servicio/VPNGenericoPost";

                    object arguments = new {
                        ipPrivada = $"{sala.IpPrivada}:{sala.PuertoServicioWebOnline}/{servicePath}?CodMaquina={CodMaquina}&fechaIni={fechaIniParam}&fechaFin={fechaFinParam}"
                    };

                    content = JsonConvert.SerializeObject(arguments);
                    requestUri = $"{endPoint}";
                } else {
                    content = JsonConvert.SerializeObject(string.Empty);
                    requestUri = $"{sala.UrlProgresivo}/{servicePath}?CodMaquina={CodMaquina}&fechaIni={fechaIniParam}&fechaFin={fechaFinParam}";
                }

                ResultEntidad result = await _serviceHelper.PostAsync(requestUri, content);

                if(result.success) {
                    success = true;
                    message = "Datos obtenidos correctamente";
                    data = result.data;
                } else {
                    message = result.message;
                }
            } catch(Exception ex) {
                message = $"Error: {ex.Message}";
                System.Diagnostics.Trace.WriteLine($"Error en GetMachineCountersDataAsync: {ex}");
            }

            return (success, message, data);
        }

    }
}
