using CapaDatos.Cortesias;
using CapaEntidad.BOT.Entities;
using System.Collections.Generic;

namespace CapaNegocio.Cortesias {
    public class BOT_EmpleadoBL {
        private readonly BOT_EmpleadoDAL emlpeadoDAL;

        public BOT_EmpleadoBL() {
            emlpeadoDAL = new BOT_EmpleadoDAL();
        }

        public List<BOT_EmpleadoEntidad> ObtenerEmpleados() {
            return emlpeadoDAL.ObtenerEmpleados();
        }

        public BOT_EmpleadoEntidad ObtenerEmpleadoPorId(int id) {
            return emlpeadoDAL.ObtenerEmpleadoPorId(id);
        }

        public BOT_EmpleadoEntidad ObtenerEmpleadoPorNumeroDocumento(string documentNumber) {
            return emlpeadoDAL.ObtenerEmpleadoPorNumeroDocumento(documentNumber);
        }

        public bool InsertarEmpleado(BOT_EmpleadoEntidad empleado) {
            empleado.NombreCompleto = $"{empleado.Nombres} {empleado.ApellidoPaterno} {empleado.ApellidoMaterno}".Trim();
            return emlpeadoDAL.InsertarEmpleado(empleado) != 0;
        }

        public bool ActualizarEmpleado(BOT_EmpleadoEntidad empleado) {
            empleado.NombreCompleto = $"{empleado.Nombres} {empleado.ApellidoPaterno} {empleado.ApellidoMaterno}".Trim();
            return emlpeadoDAL.ActualizarEmpleado(empleado) != 0;
        }

        public bool EliminarEmpleado(int id) {
            return emlpeadoDAL.EliminarEmpleado(id) != 0;
        }
    }
}
