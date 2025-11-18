using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos.AsistenciaEmpleado;
using CapaEntidad.AsistenciaEmpleado;

namespace CapaNegocio.AsistenciaEmpleado
{
    public class EmpleadoDispositivoBL
    {
        private EmpleadoDispositivoDAL EmpleadoDispositivodal = new EmpleadoDispositivoDAL();
        public EmpleadoDispositivo EmpleadoDispositivoemp_IdObtenerJson(Int64 id)
        {
            return EmpleadoDispositivodal.EmpleadoDispositivoemp_IdObtenerJson(id);
        }

        public Int64 EmpleadoDispositivoInsertarJson(EmpleadoDispositivo entidad)
        {
            return EmpleadoDispositivodal.EmpleadoDispositivoInsertarJson(entidad);
        }

        public bool EmpleadoDispositivoEditarJson(EmpleadoDispositivo entidad)
        {
            return EmpleadoDispositivodal.EmpleadoDispositivoEditarJson(entidad);
        }

        public bool EmpleadoDispositivoEditarFirebaseJson(EmpleadoDispositivo entidad)
        {
            return EmpleadoDispositivodal.EmpleadoDispositivoEditarFirebaseJson(entidad);
        }
        public bool EditarFirebaseJson(EmpleadoDispositivo entidad)
        {
            return EmpleadoDispositivodal.EditarFirebaseJson(entidad);
        }

    }
}
