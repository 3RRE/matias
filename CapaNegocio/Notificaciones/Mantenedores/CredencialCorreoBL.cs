
using CapaDatos.Notificaciones.Mantenedores;
using CapaEntidad.Notificaciones.DTO.Mantenedores;
using CapaEntidad.Notificaciones.Entity.Mantenedores;
using System.Collections.Generic;

namespace CapaNegocio.Notificaciones.Mantenedores {
    public class CredencialCorreoBL {
        private readonly CredencialCorreoDAL _credencialDAL;
        public CredencialCorreoBL() {
            _credencialDAL = new CredencialCorreoDAL();
        }
        public bool InsertarCredencialCorreo(CredencialCorreo credencial) {
            return _credencialDAL.InsertarCredencialCorreo(credencial) > 0;
        }

        public bool ActualizarCredencialCorreo(CredencialCorreo credencial) {
            return _credencialDAL.ActualizarCredencialCorreo(credencial) > 0;
        }

        public List<CredencialCorreoDto> ObtenerCredencialesCorreo() {
            return _credencialDAL.ObtenerCredencialesCorreo();
        }

        public List<CredencialCorreoDto> ObtenerCredencialesCorreoPorSistema(int IdSistema) {
            return _credencialDAL.ObtenerCredencialesCorreoPorSistema(IdSistema);
        }

        public CredencialCorreoDto ObtenerCredencialCorreoPorCorreo(string correo) {
            return _credencialDAL.ObtenerCredencialCorreoPorCorreo(correo);
        }

        public CredencialCorreoDto ObtenerCredencialCorreoPorId(int id) {
            return _credencialDAL.ObtenerCredencialCorreoPorId(id);
        }

        public bool EliminarCredencialCorreo(int id) {
            return _credencialDAL.EliminarCredencialCorreo(id) > 0;
        }
    }
}
