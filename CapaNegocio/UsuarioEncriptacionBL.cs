using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class UsuarioEncriptacionBL
    {
        private UsuarioEncriptacionDAL usuarioEncriptacionDAL = new UsuarioEncriptacionDAL();

        public UsuarioEncriptacionEntidad UsuarioEncriptacionIDObtenerJson(int EmpleadoId)
        {
            return usuarioEncriptacionDAL.UsuarioEncriptacionIDObtenerJson(EmpleadoId);
        }

        public bool UsuarioEncriptacionRenovarContraseniaJson(UsuarioEncriptacionEntidad usuarioencriptacion)
        {
            return usuarioEncriptacionDAL.UsuarioEncriptacionRenovarContraseniaJson(usuarioencriptacion);
        }

        public bool UsuarioEncriptacionInsertarJson(UsuarioEncriptacionEntidad usuarioencriptacion)
        {
            return usuarioEncriptacionDAL.UsuarioEncriptacionInsertarJson(usuarioencriptacion);
        }

        public bool UsuarioEncriptacionEditarJson(UsuarioEncriptacionEntidad usuarioencriptacion)
        {
            return usuarioEncriptacionDAL.UsuarioEncriptacionEditarJson(usuarioencriptacion);
        }

        public UsuarioEncriptacionEntidad VerificarUsuarioNombreEncriptacionJson(string UsuarioNombre)
        {
            return usuarioEncriptacionDAL.VerificarUsuarioNombreEncriptacionJson(UsuarioNombre);
        }

        public List<TecnicoUsuarioEncriptado> TecnicoListarJson()
        {
            //throw new NotImplementedException();
            return usuarioEncriptacionDAL.TecnicoListarJson();
        }

        public UsuarioEncriptacionEntidad UsuarioCoincidenciaJsonPrograma(string usuario)
        {
            return usuarioEncriptacionDAL.UsuarioCoincidenciaJsonPrograma(usuario);
        }

        public bool UsuarioEncriptacionHistorialInsertar(int usuarioEncriptacionID, int tipoAcceso)
        {
            return usuarioEncriptacionDAL.UsuarioEncriptacionHistorialInsertar(usuarioEncriptacionID, tipoAcceso);
        }
    }

}
