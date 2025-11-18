using CapaDatos.AsistenciaEmpleado;
using CapaDatos.Utilitarios;
using CapaEntidad.AsistenciaEmpleado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.AsistenciaEmpleado
{   
    public class EmpleadoAsistenciaBL
    {
        private AsistenciaEmpleadoDAL AsistenciaEmpleadodal = new AsistenciaEmpleadoDAL();
        public Int64 EmpleadoAsistenciaInsertarJson(EmpleadoAsistencia asistenciaempleado)
        {
            return AsistenciaEmpleadodal.EmpleadoAsistenciaInsertarJson(asistenciaempleado);
        }

        public (List<EmpleadoDatosAsistencia> empleadoAsistencia, ClaseError error) EmpleadoAsistenciaxFechabetweenListarJson(DateTime fechaini, DateTime fechafin,string salas)
        {
            List<EmpleadoDatosAsistencia> lista = new List<EmpleadoDatosAsistencia>();
            ClaseError error = new ClaseError();
            var consulta = AsistenciaEmpleadodal.EmpleadoAsistenciaxFechabetweenListarJson(fechaini, fechafin, salas);
            lista = consulta.empleadoAsistencia;
            error = consulta.error;
            return (empleadoAsistencia:lista, error);
        }
    }
}
