using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaDatos;

namespace CapaNegocio
{
    public class SEG_RolUsuarioBL
    {
        private SEG_RolUsuarioDAL webRolUsuarioDal = new SEG_RolUsuarioDAL();

        public bool GuardarRolUsuario(SEG_RolUsuarioEntidad rolUsuario)
        {
            return webRolUsuarioDal.GuardarRolUsuario(rolUsuario);
        }
        public List<SEG_RolUsuarioEntidad> ListarRolUsuarios()
        {
            return webRolUsuarioDal.GetRolUsuario();
        }

        public SEG_RolUsuarioEntidad GetRolUsuarioId(int Usuarioid)
        {
            return webRolUsuarioDal.GetRolUsuarioId(Usuarioid);
        }
        public bool ActualizarRolUsuario(SEG_RolUsuarioEntidad rolUsuario)
        {
            return webRolUsuarioDal.ActualizarRolUsuario(rolUsuario);
        }

        public bool EliminarRolUsuario(int rolUsuarioid)
        {
            return webRolUsuarioDal.EliminarRolUsuario(rolUsuarioid);
        }
    }
}
