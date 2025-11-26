

using CapaDatos.Publicidad;
using CapaEntidad.Publicidad;
using System.Collections.Generic;

namespace CapaNegocio.Publicidad {
    public class PublicidadBL {
        private PublicidadDAL _publicidadDal = new PublicidadDAL();

        public List<PublicidadEntidad> ListarPublicidadActivaPorSala(int codSala) {
            return _publicidadDal.ListarPublicidadActivaPorSala(codSala);
        }

        public List<EventoEntidad> ListarEventosActivosPorSala(int codSala) {
            return _publicidadDal.ListarEventosActivosPorSala(codSala);
        }

        #region CRUD Publicidad
        public List<PublicidadEntidad> ListadoPublicidadAdmin(int codSala) {
            return _publicidadDal.ListadoPublicidadAdmin(codSala);
        }

        public PublicidadEntidad PublicidadListaIdJson(int idPublicidad) {
            return _publicidadDal.PublicidadListaIdJson(idPublicidad);
        }

        public bool InsertarPublicidadJson(PublicidadEntidad publicidad) {
            return _publicidadDal.InsertarPublicidadJson(publicidad);
        }

        public bool ModificarPublicidadJson(PublicidadEntidad publicidad) {
            return _publicidadDal.ModificarPublicidadJson(publicidad);
        }

        public bool ModificarEstadoPublicidadJson(int idPublicidad, int estado) {
            return _publicidadDal.ModificarEstadoPublicidadJson(idPublicidad, estado);
        }
        #endregion

        #region CRUD Evento
        public List<EventoEntidad> ListadoEventoAdmin(int codSala) { // <--- Añadido codSala
            return _publicidadDal.ListadoEventoAdmin(codSala); // <--- Pasado codSala
        }

        public EventoEntidad EventoListaIdJson(int idEvento) {
            return _publicidadDal.EventoListaIdJson(idEvento);
        }

        public bool InsertarEventoJson(EventoEntidad evento) {
            return _publicidadDal.InsertarEventoJson(evento);
        }

        public bool ModificarEventoJson(EventoEntidad evento) {
            return _publicidadDal.ModificarEventoJson(evento);
        }

        public bool ModificarEstadoEventoJson(int idEvento, int estado) {
            return _publicidadDal.ModificarEstadoEventoJson(idEvento, estado);
        }
        #endregion
    }
}
