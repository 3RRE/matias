using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaDatos;

namespace CapaNegocio
{
    public class SEG_PermisoBL
    {
        private SEG_PermisoDAL webPermisoDal = new SEG_PermisoDAL();
        public bool GuardarPermiso(SEG_PermisoEntidad permiso)
        {
            return webPermisoDal.GuardarPermiso(permiso);
        }

        public bool GetPermisoId(string permisonombre)
        {
            return webPermisoDal.GetPermisoId(permisonombre);
        }

        public bool BorrarPermiso(string permisonombre, string nombrecontrolador)
        {
            return webPermisoDal.BorrarPermiso(permisonombre, nombrecontrolador);
        }
        public List<SEG_PermisoEntidad> ListarPermisos()
        {
            return webPermisoDal.GetPermisos();
        }

        public List<SEG_PermisoEntidad> ListPermissionsByController(string controllerName)
        {
            return webPermisoDal.PermissionsByController(controllerName);
        }
        public List<SEG_PermisoEntidad> ListarPermisosActivos()
        {
            return webPermisoDal.GetPermisosActivos();
        }
        
        public bool ActualizarEstadoPermiso(int permisoId, int estado)
        {
            return webPermisoDal.ActualizarEstadoPermiso(permisoId, estado);
        }
        public bool ActualizarDescripcionPermiso(int permisoId, string descripcion)
        {
            return webPermisoDal.ActualizarDescripcionPermiso(permisoId, descripcion);
        }
        public bool ActualizarNombrePermiso(int permisoId, string nombre)
        {
            return webPermisoDal.ActualizarNombrePermiso(permisoId, nombre);
        }
    }
}
