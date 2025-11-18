using CapaDatos.ControlAcceso;
using CapaEntidad.ControlAcceso;
using System.Collections.Generic;

namespace CapaNegocio {
    public class CAL_PersonaProhibidoIngresoIncidenciaIncidenciaBL {
        private readonly CAL_PersonaProhibidoIngresoIncidenciaDAL capaDatos;

        public CAL_PersonaProhibidoIngresoIncidenciaIncidenciaBL() {
            capaDatos = new CAL_PersonaProhibidoIngresoIncidenciaDAL();
        }

        public List<CAL_PersonaProhibidoIngresoIncidenciaEntidad> GetAllTimadorIncidencia() {
            return capaDatos.GetAllTimadorIncidencia();
        }

        public List<CAL_PersonaProhibidoIngresoIncidenciaEntidad> GetAllTimadorIncidenciaxTimador(int id, List<int> codsSalas) {
            string codsSalasStr = string.Join(",", codsSalas);
            return capaDatos.GetAllTimadorIncidenciaxTimador(id, codsSalasStr);
        }

        public List<CAL_PersonaProhibidoIngresoIncidenciaEntidad> GetAllTimadorIncidenciaxTimadorActivo(int id, List<int> codsSalas) {
            string codsSalasStr = string.Join(",", codsSalas);
            return capaDatos.GetAllTimadorIncidenciaxTimadorActivo(id, codsSalasStr);
        }

        public CAL_PersonaProhibidoIngresoIncidenciaEntidad GetIDTimadorIncidencia(int id) {
            return capaDatos.GetIDTimadorIncidencia(id);
        }

        public int InsertarTimadorIncidencia(CAL_PersonaProhibidoIngresoIncidenciaEntidad Entidad) {
            int id = capaDatos.InsertarTimadorIncidencia(Entidad);
            return id;
        }

        public bool EditarTimadorIncidencia(CAL_PersonaProhibidoIngresoIncidenciaEntidad Entidad) {
            bool status = capaDatos.EditarTimadorIncidencia(Entidad);
            return status;
        }

        public bool EliminarTimadorIncidencia(int id) {
            bool status = capaDatos.EliminarTimadorIncidencia(id);
            return status;
        }
    }
}
