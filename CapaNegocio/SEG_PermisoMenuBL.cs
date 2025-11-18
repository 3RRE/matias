using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaDatos;

namespace CapaNegocio
{
    public class SEG_PermisoMenuBL
    {
        private SEG_PermisoMenuDAL web_PermisoMenuDAL = new SEG_PermisoMenuDAL();

        public bool GuardarPermisoMenu(SEG_PermisoMenuEntidad permisoMenu)
        {
            return web_PermisoMenuDAL.GuardarPermisoMenu(permisoMenu);
        }
        public List<SEG_PermisoMenuEntidad> ListarPermisoMenu()
        {
            return web_PermisoMenuDAL.GetPermisoMenu();
        }
        public SEG_PermisoMenuEntidad GetPermisoMenuId(int permisoMenuId)
        {
            return web_PermisoMenuDAL.GetPermisoMenuId(permisoMenuId);
        }
        public List<SEG_PermisoMenuEntidad> GetPermisoMenuRolId(int permisoMenuRolId)
        {
            return web_PermisoMenuDAL.GetPermisoMenuRolId(permisoMenuRolId);
        }
        public List<SEG_PermisoMenuEntidad> GetPermisoMenuIn(string dataMenu)
        {
            return web_PermisoMenuDAL.GetPermisoMenuIn(dataMenu);
        }

        public List<SEG_PermisoMenuEntidad> GetPermisoFechaMax()
        {
            return web_PermisoMenuDAL.GetPermisoFechaMax();
        }

        public bool ActualizarPermisoMenu(SEG_PermisoMenuEntidad permisoMenu)
        {
            return web_PermisoMenuDAL.ActualizarPermisoMenu(permisoMenu);
        }

        public bool EliminarPermisoMenu(string nombrePermiso, int rolId)
        {
            return web_PermisoMenuDAL.EliminarPermisoMenu(nombrePermiso, rolId);
        }
    }
}
