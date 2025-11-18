using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class SEG_CargoBL
    {
        private SEG_CargoDAL segCargoDal = new SEG_CargoDAL();
        public List<SEG_CargoEntidad> CargoListarJson()
        {
            return segCargoDal.CargoListarJson();
        }

        public List<SEG_CargoEntidad> CargoMantenimientoListarJson()
        {
            return segCargoDal.CargoMantenimientoListarJson();
        }
        public bool CargoGuardarJson(SEG_CargoEntidad cargo)
        {
            return segCargoDal.CargoGuardarJson(cargo);
        }
        public SEG_CargoEntidad GetCargoId(int cargoId)
        {
            return segCargoDal.GetCargoId(cargoId);
        }

        public bool CargoActualizarJson(SEG_CargoEntidad cargo)
        {
            return segCargoDal.CargoActualizarJson(cargo);
        }
    }
}
