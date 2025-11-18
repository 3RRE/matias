
using System.Configuration;

namespace IASServiceServer
{
    public class AppConfig
    {
        public static string AdministrativoConnectionSring
        {
            get { return ConfigurationManager.ConnectionStrings["Administrativo"].ConnectionString; }
        }

        public static string SeguridadPjConnectionSring
        {
            get { return ConfigurationManager.ConnectionStrings["eguridadPj"].ConnectionString; }
        }

        public static string BaseAddress
        {
            get { return ConfigurationManager.AppSettings["BaseAddress"]; }
        }
    }
}
