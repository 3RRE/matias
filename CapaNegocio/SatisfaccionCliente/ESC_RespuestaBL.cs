using CapaDatos.SatisfaccionCliente;
using CapaEntidad.SatisfaccionCliente.DTO;
using CapaEntidad.SatisfaccionCliente.DTO.Configuracion;
using CapaEntidad.SatisfaccionCliente.DTO.Request;
using CapaEntidad.SatisfaccionCliente.Entity;
using CapaEntidad.SatisfaccionCliente.Reporte;
using System;
using System.Collections.Generic;

namespace CapaNegocio.SatisfaccionCliente {
    public class ESC_RespuestaBL {
        private readonly ESC_RespuestaDAL respuestaDAL;

        public ESC_RespuestaBL() {
            respuestaDAL = new ESC_RespuestaDAL();
        }

        public List<ESC_RespuestaDto> ObtenerRespuestas() {
            return respuestaDAL.ObtenerRespuestas();
        }

        public List<ESC_RespuestaDto> ObtenerRespuestasPorCodSala(int codSala) {
            return respuestaDAL.ObtenerRespuestasPorCodSala(codSala);
        }

        public List<ESC_RespuestaDto> ObtenerRespuestasPorCodSalaYFechas(int codSala, DateTime fechaInicio, DateTime fechaFin) {
            return respuestaDAL.ObtenerRespuestasPorCodSalaYFechas(codSala, fechaInicio, fechaFin);
        }
        
        public List<ESC_ReportePorcentaje> ObtenerReportePorcentajes(int codSala, DateTime fechaInicio, DateTime fechaFin) {
            return respuestaDAL.ObtenerReportePorcentajes(codSala, fechaInicio, fechaFin);
        }

        public ESC_RespuestaDto ObtenerRespuestaPorId(int id) {
            return respuestaDAL.ObtenerRespuestaPorId(id);
        }

        public ESC_RespuestaDto ObtenerUltimaRespuestaClienteDeSala(int codSala, string numeroDocumento) {
            return respuestaDAL.ObtenerUltimaRespuestaClienteDeSala(codSala, numeroDocumento);
        }
        
        public List<ESC_RespuestaDto> ObtenerRespuestasDeClientePorMesDeSala(int codSala, string numeroDocumento, int mes) {
            return respuestaDAL.ObtenerRespuestasDeClientePorMesDeSala(codSala, numeroDocumento, mes);
        }
        
        public int ObtenerCantidadRespuestasDeClientePorMesDeSala(int codSala, string numeroDocumento, int mes) {
            return respuestaDAL.ObtenerCantidadRespuestasDeClientePorMesDeSala(codSala, numeroDocumento, mes);
        }

        public int ObtenerCantidadRespuestasDeClientePorFechaDeSala(int codSala, string numeroDocumento, DateTime fecha) {
            return respuestaDAL.ObtenerCantidadRespuestasDeClientePorFechaDeSala(codSala, numeroDocumento, fecha);
        }

        public bool InsertarRespuesta(ESC_Respuesta respuesta) {
            return respuestaDAL.InsertarRespuesta(respuesta) > 0;
        }

        public int InsertarRespuestas(ESC_RespuestaClienteRequest request) {
            int insertados = 0;
            foreach(ESC_RespuestaRequest respuestaRequest in request.Respuestas) {
                ESC_Respuesta respuesta = new ESC_Respuesta {
                    IdRespuestaSala = respuestaRequest.IdRespuestaSala,
                    NumeroDocumento = request.Cliente.NumeroDocumento,
                    IdPregunta = respuestaRequest.IdPregunta,
                    Puntaje = respuestaRequest.Puntaje
                };
                if(InsertarRespuesta(respuesta)) {
                    insertados++;
                }
            }
            return insertados;
        }

        public bool ClientePuedeEnviarRespuestaPorTiempo(ESC_ConfiguracionDto configuracion, ESC_RespuestaDto ultimaRespuesta) {
            DateTime proximaRespuestaPermitida = ultimaRespuesta.FechaRegistro.AddMinutes(configuracion.TiempoEsperaRespuesta);
            return DateTime.Now >= proximaRespuestaPermitida;
        }

        public string ObtenerTiempoEspera(ESC_ConfiguracionDto configuracion, ESC_RespuestaDto ultimaRespuesta) {
            DateTime proximaRespuestaPermitida = ultimaRespuesta.FechaRegistro.AddMinutes(configuracion.TiempoEsperaRespuesta);
            TimeSpan tiempoRestante = proximaRespuestaPermitida - DateTime.Now;

            int horas = (int)tiempoRestante.TotalHours;
            int minutos = tiempoRestante.Minutes;
            string horasTexto = horas == 1 ? "hora" : "horas";
            string minutosTexto = minutos == 1 ? "minuto" : "minutos";
            if(horas > 0 && minutos > 0) {
                return $"{horas} {horasTexto} y {minutos} {minutosTexto}";
            } else if(horas > 0) {
                return $"{horas} {horasTexto}";
            } else {
                return $"{minutos} {minutosTexto}";
            }
        }
    }
}
