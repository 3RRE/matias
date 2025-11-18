using CapaDatos.ControlAcceso;
using CapaEntidad.ControlAcceso;

namespace CapaNegocio.ControlAcceso {
    public class CAL_BusquedaBL {
        private readonly CAL_BusquedaDAL capaDatos;

        public CAL_BusquedaBL() {
            capaDatos = new CAL_BusquedaDAL();
        }

        public CAL_PersonaProhibidoIngresoEntidad GetTimadorJson(string buscar) {
            return capaDatos.GetTimador(buscar);
        }

        public CAL_LudopataEntidad GetLudopataJson(string buscar) {
            return capaDatos.GetLudopata(buscar);
        }

        public CAL_PoliticoEntidad GetPoliticoJson(string buscar) {
            return capaDatos.GetPolitico(buscar);
        }

        public CAL_PersonaEntidadPublicaEntidad GetPersonaEntidadPublicaJson(string buscar) {
            return capaDatos.GetPersonaEntidadPublica(buscar);
        }

        public CAL_RobaStackersBilleteroEntidad GetRobaStackersBilletero(string buscar) {
            return capaDatos.GetRobaStackersBilletero(buscar);
        }

        public CAL_MenorDeEdadEntidad GetMenorDeEdad(string dni) => capaDatos.GetMenorDeEdad(dni);
    }
}
