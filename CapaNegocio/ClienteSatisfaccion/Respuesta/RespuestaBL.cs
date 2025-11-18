using CapaDatos.ClienteSatisfaccion.Preguntas;
using CapaDatos.ClienteSatisfaccion.Respuesta;
using CapaEntidad.ClienteSatisfaccion.DTO;
using CapaEntidad.ClienteSatisfaccion.Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.ClienteSatisfaccion.Respuesta {
    public class RespuestaBL {
        private RespuestaDAL respuestaDAL = new RespuestaDAL();


        public int GuardarRespuestaEncuesta(RespuestaEncuestaEntidad RespuestaEncuesta) {
            return respuestaDAL.GuardarRespuestaEncuesta(RespuestaEncuesta);
        }

        public int GuardarRespuestaPregunta(RespuestaPreguntaEntidad RespuestaPregunta) {
            return respuestaDAL.GuardarRespuestaPregunta(RespuestaPregunta);
        }

        public NpsResultadoDTO ObtenerNPSIndicador(DateTime fechaInicio, DateTime fechaFin) {
            return respuestaDAL.ObtenerNPSIndicador(fechaInicio, fechaFin);
        }

        public List<NpsMensualDTO> ObtenerNPSMensual(DateTime fechaInicio, DateTime fechaFin) {
            return respuestaDAL.ObtenerNPSMensual(fechaInicio, fechaFin);
        }


        public CsatResultadoDTO ObtenerCSATIndicador(DateTime fechaInicio, DateTime fechaFin, int salaId) {
            return respuestaDAL.ObtenerCSATIndicador(fechaInicio, fechaFin, salaId);
        }

        public List<CsatMensualDTO> ObtenerCSATMensual(DateTime fechaInicio, DateTime fechaFin) {
            return respuestaDAL.ObtenerCSATMensual(fechaInicio, fechaFin);
        }

        public IndicadorResultadoDTO ObtenerIndicador(string indicador, DateTime fechaInicio, DateTime fechaFin) {
            return respuestaDAL.ObtenerIndicador(indicador, fechaInicio, fechaFin);
        }

        public List<IndicadorDiarioDTO> ObtenerIndicadorDiario(DateTime fechaInicio, DateTime fechaFin, string indicador) {
            return respuestaDAL.ObtenerIndicadorDiario(fechaInicio, fechaFin, indicador);
        }



        public List<CsatRespuesta> ObtenerListaCSATIRespuestas(DateTime fechaInicio, DateTime fechaFin, int salaId) {
            return respuestaDAL.ObtenerListaCSATIRespuestas(fechaInicio, fechaFin, salaId);
        }
        public List<CsatDiarioDTO> ObtenerCSATDiario(DateTime fechaInicio, DateTime fechaFin, int salaId) {
            return respuestaDAL.ObtenerCSATDiario(fechaInicio, fechaFin, salaId);
        }

        public List<NpsRespuestaDTO> ObtenerNPSRespuestas(DateTime fechaInicio, DateTime fechaFin, int salaId) {
            return respuestaDAL.ObtenerNPSRespuestas(fechaInicio, fechaFin, salaId);
        }
        public List<RespuestaIndicadorDTO> ObtenerRespuestasIndicador(DateTime fechaInicio, DateTime fechaFin, string indicador, int salaId) {
            return respuestaDAL.ObtenerRespuestasIndicador(fechaInicio, fechaFin, indicador, salaId);

        }
        public List<RespuestaPlanoDTO> ObtenerRespuestasEncuestas(DateTime fechaInicio, DateTime fechaFin, int salaId) {
            return respuestaDAL.ObtenerRespuestasEncuestas(fechaInicio, fechaFin, salaId);

        }

        public List<PreguntaIndicadorDTO> ObtenerPreguntasIndicadores() {
            return respuestaDAL.ObtenerPreguntasIndicadores();

        }

        public List<IndicadorRespuesta> ObtenerListaIndicadorRespuestas(DateTime fechaInicio, DateTime fechaFin, string indicador, int salaId) {
            return respuestaDAL.ObtenerListaIndicadorRespuestas(indicador, fechaInicio, fechaFin, salaId);

        }

        public List<EncuestadoDTO> ObtenerEncuestados(int salaId, DateTime fechaInicio, DateTime fechaFin) {
            return respuestaDAL.ObtenerEncuestados(salaId, fechaInicio, fechaFin);
        }
        public List<RespuestaAtributoDTO> ObtenerRespuestasAtributos(int salaId, DateTime fechaInicio, DateTime fechaFin) {
            return respuestaDAL.ObtenerRespuestasAtributos(salaId, fechaInicio, fechaFin);
        }

        public int ObtenerCantidadRespuestasAtributos(int salaId, DateTime fechaInicio, DateTime fechaFin, string indicador) { 
            return respuestaDAL.ObtenerCantidadRespuestasAtributos(salaId, fechaInicio, fechaFin, indicador);

        }

    }
}
