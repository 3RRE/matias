using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaDatos;


namespace CapaNegocio
{
    public class SEG_PermisoRolBL
    {
        private SEG_PermisoRolDAL webPermisoRolDal = new SEG_PermisoRolDAL();

        public bool GuardarPermisoRol(SEG_PermisoRolEntidad permisoRol)
        {
            return webPermisoRolDal.GuardarPermisoRol(permisoRol);
        }
        public List<SEG_PermisoRolEntidad> ListarPermisoRol()
        {
            return webPermisoRolDal.GetPermisoRol();
        }

        public SEG_PermisoRolEntidad GetPermisoRolId(int permisoRolId)
        {
            return webPermisoRolDal.GetPermisoRolId(permisoRolId);
        }
        public List<SEG_PermisoRolEntidad> GetPermisoRol(int RolId)
        {
            return webPermisoRolDal.GetPermisoRolrolid(RolId);
        }
        public bool ActualizarDestinatario(SEG_PermisoRolEntidad permisoRol)
        {
            return webPermisoRolDal.ActualizarPermisoRol(permisoRol);
        }

        public bool EliminarPermisoRol(int permisoId, int rolId)
        {
            return webPermisoRolDal.EliminarPermisoRol(permisoId, rolId);
        }
        public List<SEG_PermisoRolEntidad> GetPermisoRolUsuario(int rol_id, string accion)
        {
            return webPermisoRolDal.GetPermisoRolUsuario(rol_id, accion);
        }

        public List<SEG_RolEntidad> GetRolesPorAccion(string accion) {
            return webPermisoRolDal.GetRolesPorAccion(accion);
        }

        public bool AutorizedControllerAction(int rolId, string controllerName, string actionName)
        {
            return webPermisoRolDal.AutorizedControllerAction(rolId, controllerName, actionName);
        }
    }
}
