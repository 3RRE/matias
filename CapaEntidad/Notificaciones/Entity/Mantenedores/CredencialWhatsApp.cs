
namespace CapaEntidad.Notificaciones.Entity.Mantenedores {
    public class CredencialWhatsApp : NotificacionBaseClassMantenedor {
        public int IdSistema { get; set; } 
        public string UrlBase { get; set; } = string.Empty;
        public string Instancia { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;

    }
}
