using CapaDatos.ControlAcceso;
using CapaEntidad.ControlAcceso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.ControlAcceso {
    public class CAL_RobaStackersBilleteroBL {

        CAL_RobaStackersBilleteroDAL capaDatos = new CAL_RobaStackersBilleteroDAL();

        public List<CAL_RobaStackersBilleteroEntidad> RobaStackersBilleteroListadoCompletoJson() {
            return capaDatos.GetAllRobaStackersBilletero();
        }
        public CAL_RobaStackersBilleteroEntidad RobaStackersBilleteroIdObtenerJson(int id) {
            return capaDatos.GetIDRobaStackersBilletero(id);
        }
        public int RobaStackersBilleteroInsertarJson(CAL_RobaStackersBilleteroEntidad Entidad) {
            var id = capaDatos.InsertarRobaStackersBilletero(Entidad);

            return id;
        }
        public bool RobaStackersBilleteroEditarJson(CAL_RobaStackersBilleteroEntidad Entidad) {
            var status = capaDatos.EditarRobaStackersBilletero(Entidad);

            return status;
        }
        public bool RobaStackersBilleteroEliminarJson(int id) {
            var status = capaDatos.EliminarRobaStackersBilletero(id);

            return status;
        }
        public CAL_RobaStackersBilleteroEntidad GetRobaStackersBilleteroPorDNI(string dni) {
            return capaDatos.GetRobaStackersBilleteroPorDNI(dni);
        }
    }
}
