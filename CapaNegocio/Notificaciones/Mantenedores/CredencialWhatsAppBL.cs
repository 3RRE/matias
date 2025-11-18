using CapaDatos.Notificaciones.Mantenedores;
using CapaEntidad.Notificaciones.DTO.Mantenedores;
using CapaEntidad.Notificaciones.Entity.Mantenedores;
using System.Collections.Generic;

namespace CapaNegocio.Notificaciones.Mantenedores {
    public class CredencialWhatsAppBL {
        private readonly CredencialWhatsAppDAL credencialDAL;

        public CredencialWhatsAppBL() {
            credencialDAL = new CredencialWhatsAppDAL();
        }

        public List<CredencialWhatsAppDto> ObtenerCredencialesWhatsApp() {
            return credencialDAL.ObtenerCredencialesWhatsApp();
        }

        public bool InsertarCredencialWhatsApp(CredencialWhatsApp credencial) {
            return credencialDAL.InsertarCredencialWhatsApp(credencial) > 0;
        }

        public bool ActualizarCredencialWhatsApp(CredencialWhatsApp credencial) {
            return credencialDAL.ActualizarCredencialWhatsApp(credencial) > 0;
        }

        public List<CredencialWhatsAppDto> ObtenerCredencialesWhatsAppPorSistema(int IdSistema) {
            return credencialDAL.ObtenerCredencialesWhatsAppPorSistema(IdSistema);
        }

        public CredencialWhatsAppDto ObtenerCredencialWhatsAppPorId(int id) {
            return credencialDAL.ObtenerCredencialWhatsAppPorId(id);
        }

        public bool EliminarCredencialWhatsApp(int id) {
            return credencialDAL.EliminarCredencialWhatsApp(id) > 0;
        }
    }
}
