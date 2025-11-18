using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos.ControlAcceso;
using CapaEntidad.ControlAcceso;

namespace CapaNegocio.ControlAcceso
{
    public class CAL_CodigoBL
    {
        CAL_CodigoDAL capaDatos = new CAL_CodigoDAL();

        public List<CAL_CodigoEntidad> CodigoListadoCompletoJson()
        {
            return capaDatos.GetAllCodigo();
        }
        public List<CAL_CodigoEntidad> GetAllCodigoJoinCodigoPersona()
        {
            return capaDatos.GetAllCodigoJoinCodigoPersona();
        }
        public CAL_CodigoEntidad CodigoIdObtenerJson(int id)
        {
            return capaDatos.GetIDCodigo(id);
        }
        public int CodigoInsertarJson(CAL_CodigoEntidad Entidad)
        {
            var id = capaDatos.InsertarCodigo(Entidad);

            return id;
        }
        public bool CodigoEditarJson(CAL_CodigoEntidad Entidad)
        {
            var status = capaDatos.EditarCodigo(Entidad);

            return status;
        }
        public bool CodigoEliminarJson(int id)
        {
            var status = capaDatos.EliminarCodigo(id);

            return status;
        }
    }
}
