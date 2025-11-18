using CapaDatos.ClienteSatisfaccion.Opciones;
using CapaDatos.ClienteSatisfaccion.Preguntas;
using CapaDatos.ContadoresBonusIn;
using CapaEntidad.ClienteSatisfaccion;
using CapaEntidad.ClienteSatisfaccion.DTO;
using CapaEntidad.ClienteSatisfaccion.Entidad;
using CapaEntidad.ContadoresBonusIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.ClienteSatisfaccion.Preguntas {
    public class PreguntasBL {
        private PreguntaDAL PreguntasDAL = new PreguntaDAL();


        // 🔹 Listado de preguntas por tipo de encuesta
        public List<PreguntaEntidad> ListadoPreguntas(int tipoEncuesta) {
            return PreguntasDAL.ListadoPreguntas(tipoEncuesta);
        }

        // 🔹 Crear nueva pregunta
        public int CrearPregunta(PreguntaEntidad entidad) {
            if(entidad == null)
                throw new ArgumentNullException(nameof(entidad));
            return PreguntasDAL.CrearPregunta(entidad);
        }

        // 🔹 Editar pregunta existente
        public bool EditarPregunta(PreguntaEntidad entidad) {
            if(entidad == null)
                throw new ArgumentNullException(nameof(entidad));
            return PreguntasDAL.EditarPregunta(entidad);
        }

        // 🔹 Eliminar pregunta
        public bool EliminarPregunta(int idPregunta) {
            if(idPregunta <= 0)
                throw new ArgumentException("IdPregunta inválido", nameof(idPregunta));
            return PreguntasDAL.EliminarPregunta(idPregunta);
        }

        // 🔹 Obtener detalle por Id
        public PreguntaEntidad ObtenerPorId(int idPregunta) {
            if(idPregunta <= 0)
                throw new ArgumentException("IdPregunta inválido", nameof(idPregunta));
            return PreguntasDAL.ObtenerPorId(idPregunta);
        }
        public bool TogglePregunta(int idPregunta) {
            return PreguntasDAL.TogglePregunta(idPregunta);

        }
            public  List<PreguntaEntidad> ObtenerPreguntasAtributo() {
                return PreguntasDAL.ObtenerPreguntasAtributo();
            }

        public List<PreguntaDTO> ObtenerPreguntasConOpcionesYFlujo() {
            
           return PreguntasDAL.ObtenerPreguntasConOpcionesYFlujo();

        }
    }
}
