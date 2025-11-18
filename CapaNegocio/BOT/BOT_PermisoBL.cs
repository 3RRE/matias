using CapaDatos.Cortesias;
using CapaEntidad.BOT.Entities;
using CapaEntidad.BOT.Enum;
using System.Collections.Generic;

namespace CapaNegocio.Cortesias {
    public class BOT_PermisoBL {
        private readonly BOT_PermisoDAL permisoDAL;

        public BOT_PermisoBL() {
            permisoDAL = new BOT_PermisoDAL();
        }

        public List<BOT_PermisoEntidad> ObtenerPermisosPorIdCargo(int idCargo) {
            return permisoDAL.ObtenerPermisosPorIdCargo(idCargo);
        }

        public List<BOT_PermisoEntidad> ObtenerPermisosPorIdEmpleado(int idEmpleado) {
            return permisoDAL.ObtenerPermisosPorIdEmpleado(idEmpleado);
        }

        public BOT_PermisoEntidad ObtenerPermisosPorCodAccionYIdCargo(BOT_Acciones codAccion, int idCargo) {
            return permisoDAL.ObtenerPermisosPorCodAccionYIdCargo(codAccion, idCargo);
        }

        public List<BOT_PermisoEntidad> ObtenerPermisosDeCargosPorCodAccion(BOT_Acciones codAccion) {
            return permisoDAL.ObtenerPermisosDeCargosPorCodAccion(codAccion);
        }

        public BOT_PermisoEntidad ObtenerPermisosPorCodAccionYIdEmpleado(BOT_Acciones codAccion, int idEmpleado) {
            return permisoDAL.ObtenerPermisosPorCodAccionYIdEmpleado(codAccion, idEmpleado);
        }

        public List<BOT_PermisoEntidad> ObtenerPermisosDeEmpleadosPorCodAccion(BOT_Acciones codAccion) {
            return permisoDAL.ObtenerPermisosDeEmpleadosPorCodAccion(codAccion);
        }

        public bool CrearPermiso(BOT_PermisoEntidad permiso) {
            return permisoDAL.CrearPermiso(permiso) != 0;
        }

        public bool CrearPermisos(List<BOT_PermisoEntidad> permisos) {
            bool success = true;
            foreach(BOT_PermisoEntidad permiso in permisos) {
                success &= permisoDAL.CrearPermiso(permiso) != 0;
            }
            return success;
        }

        public bool EliminarPermisoDeCargo(BOT_Acciones codAccion, int idCargo) {
            return permisoDAL.EliminarPermisoDeCargo(codAccion, idCargo) != 0;
        }

        public bool EliminarPermisos(List<BOT_PermisoEntidad> permisos) {
            bool success = true;
            foreach(BOT_PermisoEntidad permiso in permisos) {
                if(permiso.IdCargo > 0) {
                    success = EliminarPermisoDeCargo(permiso.CodAccion, permiso.IdCargo);
                } else if(permiso.IdEmpleado > 0) {
                    success = EliminarPermisoDeEmpleado(permiso.CodAccion, permiso.IdEmpleado);
                }
            }
            return success;
        }

        public bool EliminarPermisoDeEmpleado(BOT_Acciones codAccion, int idEmpleado) {
            return permisoDAL.EliminarPermisoDeEmpleado(codAccion, idEmpleado) != 0;
        }
    }
}
