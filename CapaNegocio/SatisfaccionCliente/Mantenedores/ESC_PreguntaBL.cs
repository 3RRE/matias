using CapaDatos.SatisfaccionCliente.Mantenedores;
using CapaEntidad.SatisfaccionCliente.DTO.Mantenedores;
using CapaEntidad.SatisfaccionCliente.Entity.Mantenedores;
using System.Collections.Generic;

namespace CapaNegocio.SatisfaccionCliente.Mantenedores {
    public class ESC_PreguntaBL {
        private readonly ESC_PreguntaDAL preguntaDAL;

        public ESC_PreguntaBL() {
            preguntaDAL = new ESC_PreguntaDAL();
        }

        public List<ESC_PreguntaDto> ObtenerPreguntas() {
            return preguntaDAL.ObtenerPreguntas();
        }

        public List<ESC_PreguntaDto> ObtenerPreguntasPorCodSala(int codSala) {
            return preguntaDAL.ObtenerPreguntasPorCodSala(codSala);
        }

        public ESC_PreguntaDto ObtenerPreguntaPorId(int id) {
            return preguntaDAL.ObtenerPreguntaPorId(id);
        }

        public bool InsertarPregunta(ESC_Pregunta pregunta) {
            return preguntaDAL.InsertarPregunta(pregunta) > 0;
        }

        public bool ActualizarPregunta(ESC_Pregunta pregunta) {
            return preguntaDAL.ActualizarPregunta(pregunta) > 0;
        }

        public bool EliminarPregunta(int id) {
            return preguntaDAL.EliminarPregunta(id) > 0;
        }

        public bool PreguntaEsDeSala(int codSala, int idPregunta) {
            return preguntaDAL.PreguntaEsDeSala(codSala, idPregunta);
        }

        public bool PreguntasSonDeSala(int codSala, List<int> idsPreguntas) {
            foreach(int id in idsPreguntas) {
                if(!PreguntaEsDeSala(codSala, id)) {
                    return false;
                }
            }
            return true;
        }
    }
}
