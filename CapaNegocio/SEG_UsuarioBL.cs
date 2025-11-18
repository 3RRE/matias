using CapaDatos;
using CapaEntidad;
using S3k.Utilitario.clases_especial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class SEG_UsuarioBL
    {
        private SEG_UsuarioDAL segUsuarioDal = new SEG_UsuarioDAL();
        public List<SEG_UsuarioEntidad> UsuarioListadoJson()
        {
            return segUsuarioDal.UsuarioListadoJson();
        }
      
        public bool UsuarioGuardarJson(SEG_UsuarioEntidad usuario)
        {
            return segUsuarioDal.UsuarioGuardarJson(usuario);
        }
        public Int32 UsuarioGuardarGlpiJson(SEG_UsuarioEntidad usuario)
        {
            return segUsuarioDal.UsuarioGuardarGlpiJson(usuario);
        }
        public SEG_UsuarioEntidad UsuarioGuardarEntidadJson(SEG_UsuarioEntidad usuario)
        {
            return segUsuarioDal.UsuarioGuardarEntidadJson(usuario);
        }
        public bool UsuarioCambiarContrasenia(SEG_UsuarioEntidad usuario)
        {
            return segUsuarioDal.UsuarioCambiarContrasenia(usuario);
        }
        public SEG_UsuarioEntidad UsuarioEmpleadoIDObtenerJson(int usuarioId)
        {
            return segUsuarioDal.UsuarioEmpleadoIDObtenerJson(usuarioId);
        }

        public SEG_UsuarioEntidad UsuarioEmpleadoIdObtenerJson(int empleadoid)
        {
            return segUsuarioDal.UsuarioEmpleadoIdObtenerJson(empleadoid);
        }

        public SEG_UsuarioEntidad UsuarioCoincidenciaObtenerJson(string usuario, int usuarioID, int condicion)
        {
            return segUsuarioDal.UsuarioCoincidenciaObtenerJson(usuario, usuarioID, condicion);
        }
        public bool UsuarioActualizarJson(SEG_UsuarioEntidad usuario)
        {
            return segUsuarioDal.UsuarioActualizarJson(usuario);
        }

        public bool ActualizarEstadousuario(int usuarioId, int estado)
        {
            return segUsuarioDal.ActualizarEstadoUsuario(usuarioId, estado);
        }
        public bool clrEnabled()
        {
            return segUsuarioDal.clrEnabled();
        }
        public bool CambiarContrasena(string usuPassword, int usuarioId)
        {
            return segUsuarioDal.CambiarContrasena(usuPassword, usuarioId);
        }
        public bool RestablecerContrasena(string contrasena, int usuarioID)
        {
            return segUsuarioDal.RestablecerContrasena(contrasena, usuarioID);
        }
        public bool UsuarioBloquearJson(SEG_UsuarioEntidad Entidad)
        {
            return segUsuarioDal.UsuarioBloquearJson(Entidad);

        }
        public bool UsuarioDesbloquearJson(SEG_UsuarioEntidad Entidad)
        {
            return segUsuarioDal.UsuarioDesbloquearJson(Entidad);
        }
        public SEG_UsuarioEntidad UsuarioObtenerEmpleadoUsuarioIdJson(int usuarioId)
        {
            return segUsuarioDal.UsuarioObtenerEmpleadoUsuarioIdJson(usuarioId);
        }
        public bool UsuarioActualizarUsuarioNombreJson(int UsuarioID, string UsuarioNombre)
        {
            return segUsuarioDal.UsuarioActualizarUsuarioNombreJson(UsuarioID, UsuarioNombre);
        }
        public (string respuesta, ClaseError error) UsuarioEditarTokenAccesoIntranetJson(SEG_UsuarioEntidad usuario) {
            return segUsuarioDal.UsuarioEditarTokenAccesoIntranetJson(usuario);
        }


        public List<SEG_UsuarioEntidad> UsuarioListadoxSalasJson(string query) {
            return segUsuarioDal.UsuarioListadoxSalasJson(query);
        }

    }
}
