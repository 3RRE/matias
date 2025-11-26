

using System;
using System.Configuration;

namespace CapaEntidad.Publicidad {
    public class Publicidad {
    }

    public static class PublicidadRutas {
        // <add key="VirtualPathArchivos" value="/PathArchivos" />
        public static string VirtualPath = ConfigurationManager.AppSettings["PathWebArchivos"] ?? "/PathArchivos";
    }

    public class PublicidadEntidad {
        public int IdPublicidad { get; set; }
        public int CodSala { get; set; }
        public string Titulo { get; set; }
        public string RutaImagen { get; set; }
        public string UrlEnlace { get; set; }
        public int Orden { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int Estado { get; set; }
        public DateTime FechaRegistro { get; set; }

        public string RutaArchivoLogoAnt { get; set; }

        public string NombreSala { get; set; } 

        public string UrlImagenCompleta {
            get {
                if (string.IsNullOrEmpty(this.RutaImagen))
                    return "/Content/assets/img/placeholder.png";

                // Construye la URL virtual
                return string.Format("{0}/Contenido/Publicaciones/{1}",
                    PublicidadRutas.VirtualPath.TrimEnd('/'),
                    this.RutaImagen);
            }
        }
    }

    public class EventoEntidad {
        public int IdEvento { get; set; }
        public int CodSala { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string UrlDireccion { get; set; }
        public string RutaImagen { get; set; }
        public DateTime FechaEvento { get; set; }
        public int Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string RutaArchivoLogoAnt { get; set; }
        public string NombreSala { get; set; }

        public string UrlImagenCompleta {
            get {
                if (string.IsNullOrEmpty(this.RutaImagen))
                    return "/Content/assets/img/placeholder.png";

                return string.Format("{0}/Contenido/Eventos/{1}",
                    PublicidadRutas.VirtualPath.TrimEnd('/'),
                    this.RutaImagen);
            }
        }
    }
}
