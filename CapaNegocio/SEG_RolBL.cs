using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaDatos;

namespace CapaNegocio
{
    public class SEG_RolBL
    {
        private SEG_RolDAL webRolDal = new SEG_RolDAL();

        public bool GuardarRol(SEG_RolEntidad rol)
        {
            return webRolDal.GuardarRol(rol);
        }
        public List<SEG_RolEntidad> ListarRol()
        {
            return webRolDal.GetRoles();
        }
        public List<SEG_RolEntidad> ListarRolActivos()
        {
            return webRolDal.GetRolesActivos();
        }
        public SEG_RolEntidad GetRolId(int rolId)
        {
            return webRolDal.GetRolId(rolId);
        }
        public bool ActualizarRol(SEG_RolEntidad rol)
        {
            return webRolDal.ActualizarRol(rol);
        }
        public bool ActualizarEstadoRol(int rolid, int estado)
        {
            return webRolDal.ActualizarEstadoRol(rolid, estado);
        }
        public bool EliminarRol(int rolid)
        {
            return webRolDal.EliminarRol(rolid);
        }
    }
}
