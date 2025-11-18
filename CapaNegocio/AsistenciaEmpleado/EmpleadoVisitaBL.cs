using CapaDatos.AsistenciaEmpleado;
using CapaEntidad.AsistenciaEmpleado;
using S3k.Utilitario.clases_especial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.AsistenciaEmpleado
{
    public class EmpleadoVisitaBL
    {
        private EmpleadoVisitaDAL empleadoVisitasdal = new EmpleadoVisitaDAL();
       

        public Int64 EmpleadoVisitaInsertarJson(EmpleadoVisitaEntidad entidad)
        {
            return empleadoVisitasdal.GuardarVisita(entidad);
        }

        public Int64 EmpleadoVisitaDetalleInsertarJson(EmpleadoVisitaDetalleEntidad entidad)
        {
            return empleadoVisitasdal.GuardarVisitaDetalle(entidad);
        }

        public List<EmpleadoVisitaEmpleadoEntidad> VisitaListaxFechabetweenListarJson(DateTime fechaini, DateTime fechafin, string salas)
        {
            return empleadoVisitasdal.VisitaListaxFechabetweenListarJson(fechaini, fechafin, salas);
        }

        public List<EmpleadoVisitaDetalleEntidad> VisitaListaDetalleJson(Int64 visita_id)
        {
            return empleadoVisitasdal.VisitaListaDetalleJson(visita_id);
        }

        public EmpleadoVisitaDetalleEntidad visitadetalleId(Int64 visd_id)
        {
            return empleadoVisitasdal.visitadetalleId(visd_id);
        }
    }
}
