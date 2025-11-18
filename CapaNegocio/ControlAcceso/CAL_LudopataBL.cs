using CapaDatos.ControlAcceso;
using CapaEntidad.ControlAcceso;
using CapaEntidad.ControlAcceso.Ludopata.Dto;
using System.Collections.Generic;

namespace CapaNegocio.ControlAcceso {
    public class CAL_LudopataBL {
        private readonly CAL_LudopataDAL capaDatos;

        public CAL_LudopataBL() {
            capaDatos = new CAL_LudopataDAL();
        }

        public List<CAL_LudopataEntidad> LudopataListadoCompletoJson() {
            return capaDatos.GetAllLudopata();
        }

        public List<CAL_LudopataDto> GetLudopatasActivos() {
            return capaDatos.GetLudopatasActivos();
        }

        public CAL_LudopataEntidad LudopataIdObtenerJson(int id) {
            return capaDatos.GetIDLudopata(id);
        }

        public int LudopataInsertarJson(CAL_LudopataEntidad Entidad) {
            int id = capaDatos.InsertarLudopata(Entidad);

            return id;
        }

        public bool LudopataEditarJson(CAL_LudopataEntidad Entidad) {
            bool status = capaDatos.EditarLudopata(Entidad);

            return status;
        }

        public bool LudopataEliminarJson(int id) {
            bool status = capaDatos.EliminarLudopata(id);

            return status;
        }

        public int ObtenerTotalRegistrosFiltrados(string WhereQuery) {
            return capaDatos.ObtenerTotalRegistrosFiltrados(WhereQuery);
        }

        public int ObtenerTotalRegistros() {
            return capaDatos.ObtenerTotalRegistros();
        }

        public List<CAL_LudopataEntidad> GetAllLudopataFiltrados(string WhereQuery) {
            return capaDatos.GetAllLudopataFiltrados(WhereQuery);
        }

        public CAL_LudopataEntidad GetLudopataPorDNI(string dni) {
            return capaDatos.GetLudopataPorDNI(dni);
        }

        public void EditarFechaEnvioCorreo(CAL_LudopataEntidad Entidad) {
            capaDatos.EditarFechaEnvioCorreo(Entidad);
        }

        public bool ModificarEstadoLudopata(int idLudopata, bool estado) {
            return capaDatos.ModificarEstadoLudopata(idLudopata, estado) > 0;
        }

        public List<CAL_LudopataEntidad> ReporteLudopatasClientes() {
            return capaDatos.ReporteLudopatasClientes();
        }
    }
}
