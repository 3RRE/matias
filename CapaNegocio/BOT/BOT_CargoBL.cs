using CapaDatos.Cortesias;
using CapaEntidad.BOT.Entities;
using System.Collections.Generic;

namespace CapaNegocio.Cortesias {
    public class BOT_CargoBL {
        private readonly BOT_CargoDAL cargoDAL;

        public BOT_CargoBL() {
            cargoDAL = new BOT_CargoDAL();
        }

        public List<BOT_CargoEntidad> ObtenerCargos() {
            return cargoDAL.ObtenerCargos();
        }

        public BOT_CargoEntidad ObtenerCargoPorId(int id) {
            return cargoDAL.ObtenerCargoPorId(id);
        }

        public bool InsertarCargo(BOT_CargoEntidad cargo) {
            return cargoDAL.InsertarCargo(cargo) != 0;
        }

        public bool ActualizarCargo(BOT_CargoEntidad cargo) {
            return cargoDAL.ActualizarCargo(cargo) != 0;
        }

        public bool EliminarCargo(int id) {
            return cargoDAL.EliminarCargo(id) != 0;
        }
    }
}
