using CapaDatos.Cortesias;
using CapaEntidad.BOT.Entities;
using System.Collections.Generic;

namespace CapaNegocio.Cortesias {
    public class BOT_AreaBL {
        private readonly BOT_AreaDAL areaDAL;

        public BOT_AreaBL() {
            areaDAL = new BOT_AreaDAL();
        }

        public List<BOT_AreaEntidad> ObtenerAreas() {
            return areaDAL.ObtenerAreas();
        }

        public BOT_AreaEntidad ObtenerAreaPorId(int id) {
            return areaDAL.ObtenerAreaPorId(id);
        }

        public bool InsertarArea(BOT_AreaEntidad area) {
            return areaDAL.InsertarArea(area) != 0;
        }

        public bool ActualizarArea(BOT_AreaEntidad area) {
            return areaDAL.ActualizarArea(area) != 0;
        }

        public bool EliminarArea(int id) {
            return areaDAL.EliminarArea(id) != 0;
        }
    }
}
